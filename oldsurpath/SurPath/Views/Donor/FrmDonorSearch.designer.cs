namespace SurPath
{
    partial class FrmDonorSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDonorSearch));
            this.dgvSearchResult = new System.Windows.Forms.DataGridView();
            this.DonorSelection = new System.Windows.Forms.DataGridViewCheckBoxColumn();
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
            this.backend_notifications_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.backend_notification_window_data_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NotifiedViaFormFox = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PaymentDateValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Result = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TestReason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DonorCity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ZipCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MROType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PaymentType = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.btnViewAll = new System.Windows.Forms.Button();
            this.btnViewSelected = new System.Windows.Forms.Button();
            this.btnDeselectAll = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.lblPageHeader = new System.Windows.Forms.Label();
            this.btnAddRemove = new System.Windows.Forms.Button();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.lblCity = new System.Windows.Forms.Label();
            this.lblLastName = new System.Windows.Forms.Label();
            this.lblZipCode = new System.Windows.Forms.Label();
            this.lblDepartment = new System.Windows.Forms.Label();
            this.lblSSN = new System.Windows.Forms.Label();
            this.lblSpecimebId = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblDOB = new System.Windows.Forms.Label();
            this.lblTestReason = new System.Windows.Forms.Label();
            this.lblTestType = new System.Windows.Forms.Label();
            this.lblClient = new System.Windows.Forms.Label();
            this.cmbTestReason = new System.Windows.Forms.ComboBox();
            this.cmbTestType = new System.Windows.Forms.ComboBox();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.cmbDepartment = new System.Windows.Forms.ComboBox();
            this.cmbClient = new System.Windows.Forms.ComboBox();
            this.chkIncludeArchived = new System.Windows.Forms.CheckBox();
            this.chkwalkin = new System.Windows.Forms.CheckBox();
            this.rbtnLast3Days = new System.Windows.Forms.RadioButton();
            this.rbtnNone = new System.Windows.Forms.RadioButton();
            this.rbtnLast30Days = new System.Windows.Forms.RadioButton();
            this.rbtnLast60Days = new System.Windows.Forms.RadioButton();
            this.rbtnLast90Days = new System.Windows.Forms.RadioButton();
            this.rbtnDateRange = new System.Windows.Forms.RadioButton();
            this.rbtnLast7Days = new System.Windows.Forms.RadioButton();
            this.txtFirstName = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtCity = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtLastName = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtZipCode = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtSSN = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtSpecimenId = new SurPath.Controls.TextBoxes.SurTextBox();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.cmbSearchMonth = new System.Windows.Forms.ComboBox();
            this.cmbSearchDate = new System.Windows.Forms.ComboBox();
            this.cmbSearchYear = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblBefore = new System.Windows.Forms.Label();
            this.lblAfter = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dtBeforeFilter = new System.Windows.Forms.DateTimePicker();
            this.dtAfterFilter = new System.Windows.Forms.DateTimePicker();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.chkShowAll = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.nudNumberToSelect = new System.Windows.Forms.NumericUpDown();
            this.btnNotifyNow = new System.Windows.Forms.Button();
            this.btnSetNotified = new System.Windows.Forms.Button();
            this.btnSelectRandom = new System.Windows.Forms.Button();
            this.btnNotify = new System.Windows.Forms.Button();
            this.lblPresets = new System.Windows.Forms.Label();
            this.cbPresets = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchResult)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumberToSelect)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvSearchResult
            // 
            this.dgvSearchResult.AllowDrop = true;
            this.dgvSearchResult.AllowUserToAddRows = false;
            this.dgvSearchResult.AllowUserToDeleteRows = false;
            this.dgvSearchResult.AllowUserToOrderColumns = true;
            this.dgvSearchResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSearchResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSearchResult.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DonorSelection,
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
            this.backend_notifications_id,
            this.backend_notification_window_data_id,
            this.NotifiedViaFormFox,
            this.PaymentDateValue,
            this.Result,
            this.TestReason,
            this.DonorCity,
            this.ZipCode,
            this.MROType,
            this.PaymentType,
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
            this.dgvSearchResult.Location = new System.Drawing.Point(19, 280);
            this.dgvSearchResult.Name = "dgvSearchResult";
            this.dgvSearchResult.ReadOnly = true;
            this.dgvSearchResult.RowHeadersVisible = false;
            this.dgvSearchResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSearchResult.Size = new System.Drawing.Size(1272, 343);
            this.dgvSearchResult.TabIndex = 0;
            this.dgvSearchResult.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSearchResult_CellClick);
            this.dgvSearchResult.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSearchResult_CellDoubleClick);
            this.dgvSearchResult.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSearchResult_CellMouseClick);
            this.dgvSearchResult.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSearchResult_ColumnHeaderMouseClick);
            this.dgvSearchResult.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvSearchResult_DataBindingComplete);
            this.dgvSearchResult.DragDrop += new System.Windows.Forms.DragEventHandler(this.dgvSearchResult_DragDrop);
            this.dgvSearchResult.DragEnter += new System.Windows.Forms.DragEventHandler(this.dgvSearchResult_DragEnter);
            this.dgvSearchResult.DragOver += new System.Windows.Forms.DragEventHandler(this.dgvSearchResult_DragOver);
            this.dgvSearchResult.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvSearchResult_KeyDown);
            this.dgvSearchResult.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvSearchResult_MouseDown);
            // 
            // DonorSelection
            // 
            this.DonorSelection.HeaderText = "";
            this.DonorSelection.Name = "DonorSelection";
            this.DonorSelection.ReadOnly = true;
            this.DonorSelection.Width = 50;
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
            // 
            // ClearStarID
            // 
            this.ClearStarID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.ClearStarID.DataPropertyName = "DonorClearStarProfId";
            this.ClearStarID.HeaderText = "ClearStar ID";
            this.ClearStarID.Name = "ClearStarID";
            this.ClearStarID.ReadOnly = true;
            this.ClearStarID.Width = 5;
            // 
            // SpecimenDateValue
            // 
            this.SpecimenDateValue.HeaderText = "Specimen Date";
            this.SpecimenDateValue.Name = "SpecimenDateValue";
            this.SpecimenDateValue.ReadOnly = true;
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
            // 
            // DonorTestRegisteredDateValue
            // 
            this.DonorTestRegisteredDateValue.HeaderText = "Date Registered";
            this.DonorTestRegisteredDateValue.Name = "DonorTestRegisteredDateValue";
            this.DonorTestRegisteredDateValue.ReadOnly = true;
            // 
            // DonorDateNotified
            // 
            this.DonorDateNotified.DataPropertyName = "notified_by_email_timestamp";
            this.DonorDateNotified.HeaderText = "Donor Date Notified";
            this.DonorDateNotified.Name = "DonorDateNotified";
            this.DonorDateNotified.ReadOnly = true;
            // 
            // backend_notifications_id
            // 
            this.backend_notifications_id.DataPropertyName = "backend_notifications_id";
            this.backend_notifications_id.HeaderText = "backend_notifications_id";
            this.backend_notifications_id.Name = "backend_notifications_id";
            this.backend_notifications_id.ReadOnly = true;
            this.backend_notifications_id.Visible = false;
            // 
            // backend_notification_window_data_id
            // 
            this.backend_notification_window_data_id.DataPropertyName = "backend_notification_window_data_id";
            this.backend_notification_window_data_id.HeaderText = "backend_notification_window_data_id";
            this.backend_notification_window_data_id.Name = "backend_notification_window_data_id";
            this.backend_notification_window_data_id.ReadOnly = true;
            this.backend_notification_window_data_id.Visible = false;
            // 
            // NotifiedViaFormFox
            // 
            this.NotifiedViaFormFox.DataPropertyName = "Notified_via_FormFox";
            this.NotifiedViaFormFox.HeaderText = "Form Fox Used";
            this.NotifiedViaFormFox.Name = "NotifiedViaFormFox";
            this.NotifiedViaFormFox.ReadOnly = true;
            // 
            // PaymentDateValue
            // 
            this.PaymentDateValue.HeaderText = "Payment Date";
            this.PaymentDateValue.Name = "PaymentDateValue";
            this.PaymentDateValue.ReadOnly = true;
            // 
            // Result
            // 
            this.Result.HeaderText = "Result";
            this.Result.Name = "Result";
            this.Result.ReadOnly = true;
            // 
            // TestReason
            // 
            this.TestReason.HeaderText = "Test Reason";
            this.TestReason.Name = "TestReason";
            this.TestReason.ReadOnly = true;
            this.TestReason.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
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
            // 
            // PaymentType
            // 
            this.PaymentType.HeaderText = "Payment Type";
            this.PaymentType.Name = "PaymentType";
            this.PaymentType.ReadOnly = true;
            this.PaymentType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
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
            // btnViewAll
            // 
            this.btnViewAll.AutoSize = true;
            this.btnViewAll.Location = new System.Drawing.Point(278, 248);
            this.btnViewAll.Name = "btnViewAll";
            this.btnViewAll.Size = new System.Drawing.Size(75, 23);
            this.btnViewAll.TabIndex = 6;
            this.btnViewAll.Text = "&View All";
            this.btnViewAll.UseVisualStyleBackColor = true;
            this.btnViewAll.Click += new System.EventHandler(this.btnViewAll_Click);
            // 
            // btnViewSelected
            // 
            this.btnViewSelected.AutoSize = true;
            this.btnViewSelected.Location = new System.Drawing.Point(185, 248);
            this.btnViewSelected.Name = "btnViewSelected";
            this.btnViewSelected.Size = new System.Drawing.Size(85, 23);
            this.btnViewSelected.TabIndex = 5;
            this.btnViewSelected.Text = "Vie&w Selected";
            this.btnViewSelected.UseVisualStyleBackColor = true;
            this.btnViewSelected.Click += new System.EventHandler(this.btnViewSelected_Click);
            // 
            // btnDeselectAll
            // 
            this.btnDeselectAll.AutoSize = true;
            this.btnDeselectAll.Location = new System.Drawing.Point(102, 248);
            this.btnDeselectAll.Name = "btnDeselectAll";
            this.btnDeselectAll.Size = new System.Drawing.Size(75, 23);
            this.btnDeselectAll.TabIndex = 4;
            this.btnDeselectAll.Text = "&Deselect All";
            this.btnDeselectAll.UseVisualStyleBackColor = true;
            this.btnDeselectAll.Click += new System.EventHandler(this.btnDeselectAll_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.AutoSize = true;
            this.btnSelectAll.Location = new System.Drawing.Point(19, 248);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(75, 23);
            this.btnSelectAll.TabIndex = 3;
            this.btnSelectAll.Text = "Se&lect All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnExport
            // 
            this.btnExport.AutoSize = true;
            this.btnExport.Location = new System.Drawing.Point(361, 248);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 7;
            this.btnExport.Text = "&Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // lblPageHeader
            // 
            this.lblPageHeader.AutoSize = true;
            this.lblPageHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageHeader.Location = new System.Drawing.Point(12, 9);
            this.lblPageHeader.Name = "lblPageHeader";
            this.lblPageHeader.Size = new System.Drawing.Size(120, 20);
            this.lblPageHeader.TabIndex = 0;
            this.lblPageHeader.Text = "Donor Search";
            // 
            // btnAddRemove
            // 
            this.btnAddRemove.AutoSize = true;
            this.btnAddRemove.Location = new System.Drawing.Point(442, 248);
            this.btnAddRemove.Name = "btnAddRemove";
            this.btnAddRemove.Size = new System.Drawing.Size(124, 23);
            this.btnAddRemove.TabIndex = 8;
            this.btnAddRemove.Text = "&Add/Remove ";
            this.btnAddRemove.UseVisualStyleBackColor = true;
            this.btnAddRemove.Click += new System.EventHandler(this.btnAddRemove_Click);
            // 
            // lblFirstName
            // 
            this.lblFirstName.AutoSize = true;
            this.lblFirstName.Location = new System.Drawing.Point(11, 14);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(57, 13);
            this.lblFirstName.TabIndex = 0;
            this.lblFirstName.Text = "First Name";
            // 
            // lblCity
            // 
            this.lblCity.AutoSize = true;
            this.lblCity.Location = new System.Drawing.Point(11, 50);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(24, 13);
            this.lblCity.TabIndex = 10;
            this.lblCity.Text = "City";
            // 
            // lblLastName
            // 
            this.lblLastName.AutoSize = true;
            this.lblLastName.Location = new System.Drawing.Point(333, 13);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(58, 13);
            this.lblLastName.TabIndex = 2;
            this.lblLastName.Text = "Last Name";
            // 
            // lblZipCode
            // 
            this.lblZipCode.AutoSize = true;
            this.lblZipCode.Location = new System.Drawing.Point(333, 50);
            this.lblZipCode.Name = "lblZipCode";
            this.lblZipCode.Size = new System.Drawing.Size(50, 13);
            this.lblZipCode.TabIndex = 12;
            this.lblZipCode.Text = "Zip Code";
            // 
            // lblDepartment
            // 
            this.lblDepartment.AutoSize = true;
            this.lblDepartment.Location = new System.Drawing.Point(333, 87);
            this.lblDepartment.Name = "lblDepartment";
            this.lblDepartment.Size = new System.Drawing.Size(62, 13);
            this.lblDepartment.TabIndex = 20;
            this.lblDepartment.Text = "Department";
            // 
            // lblSSN
            // 
            this.lblSSN.AutoSize = true;
            this.lblSSN.Location = new System.Drawing.Point(654, 13);
            this.lblSSN.Name = "lblSSN";
            this.lblSSN.Size = new System.Drawing.Size(39, 13);
            this.lblSSN.TabIndex = 4;
            this.lblSSN.Text = "SSN #";
            // 
            // lblSpecimebId
            // 
            this.lblSpecimebId.AutoSize = true;
            this.lblSpecimebId.Location = new System.Drawing.Point(654, 50);
            this.lblSpecimebId.Name = "lblSpecimebId";
            this.lblSpecimebId.Size = new System.Drawing.Size(68, 13);
            this.lblSpecimebId.TabIndex = 14;
            this.lblSpecimebId.Text = "Specimen ID";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(654, 87);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(37, 13);
            this.lblStatus.TabIndex = 22;
            this.lblStatus.Text = "Status";
            // 
            // lblDOB
            // 
            this.lblDOB.AutoSize = true;
            this.lblDOB.Location = new System.Drawing.Point(986, 13);
            this.lblDOB.Name = "lblDOB";
            this.lblDOB.Size = new System.Drawing.Size(36, 13);
            this.lblDOB.TabIndex = 6;
            this.lblDOB.Text = "D.O.B";
            // 
            // lblTestReason
            // 
            this.lblTestReason.AutoSize = true;
            this.lblTestReason.Location = new System.Drawing.Point(986, 50);
            this.lblTestReason.Name = "lblTestReason";
            this.lblTestReason.Size = new System.Drawing.Size(68, 13);
            this.lblTestReason.TabIndex = 16;
            this.lblTestReason.Text = "Test Reason";
            // 
            // lblTestType
            // 
            this.lblTestType.AutoSize = true;
            this.lblTestType.Location = new System.Drawing.Point(986, 87);
            this.lblTestType.Name = "lblTestType";
            this.lblTestType.Size = new System.Drawing.Size(55, 13);
            this.lblTestType.TabIndex = 24;
            this.lblTestType.Text = "Test Type";
            // 
            // lblClient
            // 
            this.lblClient.AutoSize = true;
            this.lblClient.Location = new System.Drawing.Point(11, 87);
            this.lblClient.Name = "lblClient";
            this.lblClient.Size = new System.Drawing.Size(33, 13);
            this.lblClient.TabIndex = 18;
            this.lblClient.Text = "Client";
            // 
            // cmbTestReason
            // 
            this.cmbTestReason.DropDownHeight = 95;
            this.cmbTestReason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTestReason.FormattingEnabled = true;
            this.cmbTestReason.IntegralHeight = false;
            this.cmbTestReason.Items.AddRange(new object[] {
            "(Select Test Reason)",
            "Pre-Employment",
            "Random",
            "Reasonable Suspicion / Cause",
            "Post-Accident",
            "Return to Duty",
            "Follow-Up",
            "Other"});
            this.cmbTestReason.Location = new System.Drawing.Point(1059, 46);
            this.cmbTestReason.Name = "cmbTestReason";
            this.cmbTestReason.Size = new System.Drawing.Size(153, 21);
            this.cmbTestReason.TabIndex = 17;
            // 
            // cmbTestType
            // 
            this.cmbTestType.DropDownHeight = 95;
            this.cmbTestType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTestType.FormattingEnabled = true;
            this.cmbTestType.IntegralHeight = false;
            this.cmbTestType.Items.AddRange(new object[] {
            "(Select Test Type)",
            "UA",
            "Hair",
            "DNA"});
            this.cmbTestType.Location = new System.Drawing.Point(1060, 83);
            this.cmbTestType.Name = "cmbTestType";
            this.cmbTestType.Size = new System.Drawing.Size(117, 21);
            this.cmbTestType.TabIndex = 25;
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownHeight = 95;
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.IntegralHeight = false;
            this.cmbStatus.Items.AddRange(new object[] {
            "(Select Status)",
            "Pre-Registered",
            "In-Queue",
            "Suspension Queue",
            "Processing",
            "Completed"});
            this.cmbStatus.Location = new System.Drawing.Point(731, 83);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(125, 21);
            this.cmbStatus.TabIndex = 23;
            // 
            // cmbDepartment
            // 
            this.cmbDepartment.DropDownHeight = 95;
            this.cmbDepartment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDepartment.FormattingEnabled = true;
            this.cmbDepartment.IntegralHeight = false;
            this.cmbDepartment.Items.AddRange(new object[] {
            "(Select Department)"});
            this.cmbDepartment.Location = new System.Drawing.Point(403, 83);
            this.cmbDepartment.Name = "cmbDepartment";
            this.cmbDepartment.Size = new System.Drawing.Size(198, 21);
            this.cmbDepartment.TabIndex = 21;
            // 
            // cmbClient
            // 
            this.cmbClient.DropDownHeight = 95;
            this.cmbClient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbClient.FormattingEnabled = true;
            this.cmbClient.IntegralHeight = false;
            this.cmbClient.Items.AddRange(new object[] {
            "(Select Client)"});
            this.cmbClient.Location = new System.Drawing.Point(82, 83);
            this.cmbClient.Name = "cmbClient";
            this.cmbClient.Size = new System.Drawing.Size(196, 21);
            this.cmbClient.TabIndex = 19;
            this.cmbClient.SelectedIndexChanged += new System.EventHandler(this.cmbClient_SelectedIndexChanged);
            // 
            // chkIncludeArchived
            // 
            this.chkIncludeArchived.AutoSize = true;
            this.chkIncludeArchived.Location = new System.Drawing.Point(11, 124);
            this.chkIncludeArchived.Name = "chkIncludeArchived";
            this.chkIncludeArchived.Size = new System.Drawing.Size(106, 17);
            this.chkIncludeArchived.TabIndex = 26;
            this.chkIncludeArchived.Text = "Include Archived";
            this.chkIncludeArchived.UseVisualStyleBackColor = true;
            // 
            // chkwalkin
            // 
            this.chkwalkin.AutoSize = true;
            this.chkwalkin.Location = new System.Drawing.Point(128, 124);
            this.chkwalkin.Name = "chkwalkin";
            this.chkwalkin.Size = new System.Drawing.Size(62, 17);
            this.chkwalkin.TabIndex = 27;
            this.chkwalkin.Text = "Walk-in";
            this.chkwalkin.UseVisualStyleBackColor = true;
            // 
            // rbtnLast3Days
            // 
            this.rbtnLast3Days.AutoSize = true;
            this.rbtnLast3Days.Location = new System.Drawing.Point(256, 124);
            this.rbtnLast3Days.Name = "rbtnLast3Days";
            this.rbtnLast3Days.Size = new System.Drawing.Size(81, 17);
            this.rbtnLast3Days.TabIndex = 29;
            this.rbtnLast3Days.Text = "Last 3 Days";
            this.rbtnLast3Days.UseVisualStyleBackColor = true;
            // 
            // rbtnNone
            // 
            this.rbtnNone.AutoSize = true;
            this.rbtnNone.Checked = true;
            this.rbtnNone.Location = new System.Drawing.Point(197, 124);
            this.rbtnNone.Name = "rbtnNone";
            this.rbtnNone.Size = new System.Drawing.Size(51, 17);
            this.rbtnNone.TabIndex = 28;
            this.rbtnNone.TabStop = true;
            this.rbtnNone.Text = "None";
            this.rbtnNone.UseVisualStyleBackColor = true;
            // 
            // rbtnLast30Days
            // 
            this.rbtnLast30Days.AutoSize = true;
            this.rbtnLast30Days.Location = new System.Drawing.Point(434, 124);
            this.rbtnLast30Days.Name = "rbtnLast30Days";
            this.rbtnLast30Days.Size = new System.Drawing.Size(87, 17);
            this.rbtnLast30Days.TabIndex = 31;
            this.rbtnLast30Days.Text = "Last 30 Days";
            this.rbtnLast30Days.UseVisualStyleBackColor = true;
            // 
            // rbtnLast60Days
            // 
            this.rbtnLast60Days.AutoSize = true;
            this.rbtnLast60Days.Location = new System.Drawing.Point(529, 124);
            this.rbtnLast60Days.Name = "rbtnLast60Days";
            this.rbtnLast60Days.Size = new System.Drawing.Size(87, 17);
            this.rbtnLast60Days.TabIndex = 32;
            this.rbtnLast60Days.Text = "Last 60 Days";
            this.rbtnLast60Days.UseVisualStyleBackColor = true;
            // 
            // rbtnLast90Days
            // 
            this.rbtnLast90Days.AutoSize = true;
            this.rbtnLast90Days.Location = new System.Drawing.Point(624, 124);
            this.rbtnLast90Days.Name = "rbtnLast90Days";
            this.rbtnLast90Days.Size = new System.Drawing.Size(87, 17);
            this.rbtnLast90Days.TabIndex = 33;
            this.rbtnLast90Days.Text = "Last 90 Days";
            this.rbtnLast90Days.UseVisualStyleBackColor = true;
            // 
            // rbtnDateRange
            // 
            this.rbtnDateRange.AutoSize = true;
            this.rbtnDateRange.Location = new System.Drawing.Point(715, 124);
            this.rbtnDateRange.Name = "rbtnDateRange";
            this.rbtnDateRange.Size = new System.Drawing.Size(83, 17);
            this.rbtnDateRange.TabIndex = 34;
            this.rbtnDateRange.Text = "Date Range";
            this.rbtnDateRange.UseVisualStyleBackColor = true;
            this.rbtnDateRange.CheckedChanged += new System.EventHandler(this.rbtnDateRange_CheckedChanged);
            // 
            // rbtnLast7Days
            // 
            this.rbtnLast7Days.AutoSize = true;
            this.rbtnLast7Days.Location = new System.Drawing.Point(345, 124);
            this.rbtnLast7Days.Name = "rbtnLast7Days";
            this.rbtnLast7Days.Size = new System.Drawing.Size(81, 17);
            this.rbtnLast7Days.TabIndex = 30;
            this.rbtnLast7Days.Text = "Last 7 Days";
            this.rbtnLast7Days.UseVisualStyleBackColor = true;
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(80, 9);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(198, 20);
            this.txtFirstName.TabIndex = 1;
            this.txtFirstName.WaterMark = "Enter First Name";
            this.txtFirstName.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtFirstName.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFirstName.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // txtCity
            // 
            this.txtCity.Location = new System.Drawing.Point(80, 46);
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(198, 20);
            this.txtCity.TabIndex = 11;
            this.txtCity.WaterMark = "Enter City";
            this.txtCity.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtCity.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCity.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(403, 9);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(198, 20);
            this.txtLastName.TabIndex = 3;
            this.txtLastName.WaterMark = "Enter Last  Name";
            this.txtLastName.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtLastName.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLastName.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // txtZipCode
            // 
            this.txtZipCode.Location = new System.Drawing.Point(403, 46);
            this.txtZipCode.MaxLength = 5;
            this.txtZipCode.Name = "txtZipCode";
            this.txtZipCode.Size = new System.Drawing.Size(84, 20);
            this.txtZipCode.TabIndex = 13;
            this.txtZipCode.WaterMark = "Enter  Zip Code";
            this.txtZipCode.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtZipCode.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtZipCode.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // txtSSN
            // 
            this.txtSSN.Location = new System.Drawing.Point(731, 9);
            this.txtSSN.MaxLength = 4;
            this.txtSSN.Name = "txtSSN";
            this.txtSSN.Size = new System.Drawing.Size(198, 20);
            this.txtSSN.TabIndex = 5;
            this.txtSSN.WaterMark = "Enter Last Four Digits";
            this.txtSSN.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtSSN.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSSN.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // txtSpecimenId
            // 
            this.txtSpecimenId.Location = new System.Drawing.Point(731, 46);
            this.txtSpecimenId.Name = "txtSpecimenId";
            this.txtSpecimenId.Size = new System.Drawing.Size(198, 20);
            this.txtSpecimenId.TabIndex = 15;
            this.txtSpecimenId.WaterMark = "Enter Specimen Id";
            this.txtSpecimenId.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtSpecimenId.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSpecimenId.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.CustomFormat = "MM/dd/yyyy";
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromDate.Location = new System.Drawing.Point(802, 122);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(88, 20);
            this.dtpFromDate.TabIndex = 35;
            this.dtpFromDate.Value = new System.DateTime(2014, 11, 5, 13, 28, 40, 0);
            // 
            // dtpToDate
            // 
            this.dtpToDate.CustomFormat = "MM/dd/yyyy";
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToDate.Location = new System.Drawing.Point(898, 122);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(88, 20);
            this.dtpToDate.TabIndex = 36;
            this.dtpToDate.Value = new System.DateTime(2014, 11, 5, 13, 28, 40, 0);
            // 
            // btnSearch
            // 
            this.btnSearch.AutoSize = true;
            this.btnSearch.Location = new System.Drawing.Point(1145, 153);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 27);
            this.btnSearch.TabIndex = 39;
            this.btnSearch.Text = "&Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnReset
            // 
            this.btnReset.AutoSize = true;
            this.btnReset.Location = new System.Drawing.Point(1067, 153);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 27);
            this.btnReset.TabIndex = 38;
            this.btnReset.Text = "&Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
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
            this.cmbSearchMonth.Location = new System.Drawing.Point(1059, 9);
            this.cmbSearchMonth.Name = "cmbSearchMonth";
            this.cmbSearchMonth.Size = new System.Drawing.Size(42, 21);
            this.cmbSearchMonth.TabIndex = 7;
            this.cmbSearchMonth.TextChanged += new System.EventHandler(this.cmbSearchMonth_TextChanged);
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
            this.cmbSearchDate.Location = new System.Drawing.Point(1103, 9);
            this.cmbSearchDate.Name = "cmbSearchDate";
            this.cmbSearchDate.Size = new System.Drawing.Size(40, 21);
            this.cmbSearchDate.TabIndex = 8;
            this.cmbSearchDate.TextChanged += new System.EventHandler(this.cmbSearchDate_TextChanged);
            // 
            // cmbSearchYear
            // 
            this.cmbSearchYear.DropDownHeight = 90;
            this.cmbSearchYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSearchYear.FormattingEnabled = true;
            this.cmbSearchYear.IntegralHeight = false;
            this.cmbSearchYear.Items.AddRange(new object[] {
            "YYYY"});
            this.cmbSearchYear.Location = new System.Drawing.Point(1145, 9);
            this.cmbSearchYear.Name = "cmbSearchYear";
            this.cmbSearchYear.Size = new System.Drawing.Size(67, 21);
            this.cmbSearchYear.TabIndex = 9;
            this.cmbSearchYear.TextChanged += new System.EventHandler(this.cmbSearchYear_TextChanged);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.cbPresets);
            this.panel1.Controls.Add(this.lblPresets);
            this.panel1.Controls.Add(this.lblBefore);
            this.panel1.Controls.Add(this.lblAfter);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.dtBeforeFilter);
            this.panel1.Controls.Add(this.dtAfterFilter);
            this.panel1.Controls.Add(this.cbFilter);
            this.panel1.Controls.Add(this.cmbSearchYear);
            this.panel1.Controls.Add(this.cmbSearchDate);
            this.panel1.Controls.Add(this.cmbSearchMonth);
            this.panel1.Controls.Add(this.btnReset);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.dtpToDate);
            this.panel1.Controls.Add(this.dtpFromDate);
            this.panel1.Controls.Add(this.txtSpecimenId);
            this.panel1.Controls.Add(this.txtSSN);
            this.panel1.Controls.Add(this.chkShowAll);
            this.panel1.Controls.Add(this.txtZipCode);
            this.panel1.Controls.Add(this.txtLastName);
            this.panel1.Controls.Add(this.txtCity);
            this.panel1.Controls.Add(this.txtFirstName);
            this.panel1.Controls.Add(this.rbtnLast7Days);
            this.panel1.Controls.Add(this.rbtnDateRange);
            this.panel1.Controls.Add(this.rbtnLast90Days);
            this.panel1.Controls.Add(this.rbtnLast60Days);
            this.panel1.Controls.Add(this.rbtnLast30Days);
            this.panel1.Controls.Add(this.rbtnNone);
            this.panel1.Controls.Add(this.rbtnLast3Days);
            this.panel1.Controls.Add(this.chkwalkin);
            this.panel1.Controls.Add(this.chkIncludeArchived);
            this.panel1.Controls.Add(this.cmbClient);
            this.panel1.Controls.Add(this.cmbDepartment);
            this.panel1.Controls.Add(this.cmbStatus);
            this.panel1.Controls.Add(this.cmbTestType);
            this.panel1.Controls.Add(this.cmbTestReason);
            this.panel1.Controls.Add(this.lblClient);
            this.panel1.Controls.Add(this.lblTestType);
            this.panel1.Controls.Add(this.lblTestReason);
            this.panel1.Controls.Add(this.lblDOB);
            this.panel1.Controls.Add(this.lblStatus);
            this.panel1.Controls.Add(this.lblSpecimebId);
            this.panel1.Controls.Add(this.lblSSN);
            this.panel1.Controls.Add(this.lblDepartment);
            this.panel1.Controls.Add(this.lblZipCode);
            this.panel1.Controls.Add(this.lblLastName);
            this.panel1.Controls.Add(this.lblCity);
            this.panel1.Controls.Add(this.lblFirstName);
            this.panel1.Location = new System.Drawing.Point(18, 42);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1236, 189);
            this.panel1.TabIndex = 2;
            // 
            // lblBefore
            // 
            this.lblBefore.AutoSize = true;
            this.lblBefore.Location = new System.Drawing.Point(353, 160);
            this.lblBefore.Name = "lblBefore";
            this.lblBefore.Size = new System.Drawing.Size(38, 13);
            this.lblBefore.TabIndex = 44;
            this.lblBefore.Text = "Before";
            // 
            // lblAfter
            // 
            this.lblAfter.AutoSize = true;
            this.lblAfter.Location = new System.Drawing.Point(216, 160);
            this.lblAfter.Name = "lblAfter";
            this.lblAfter.Size = new System.Drawing.Size(29, 13);
            this.lblAfter.TabIndex = 44;
            this.lblAfter.Text = "After";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 160);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 43;
            this.label1.Text = "Date Filter";
            // 
            // dtBeforeFilter
            // 
            this.dtBeforeFilter.CustomFormat = "MM/dd/yyyy";
            this.dtBeforeFilter.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtBeforeFilter.Location = new System.Drawing.Point(397, 158);
            this.dtBeforeFilter.Name = "dtBeforeFilter";
            this.dtBeforeFilter.Size = new System.Drawing.Size(88, 20);
            this.dtBeforeFilter.TabIndex = 42;
            this.dtBeforeFilter.Value = new System.DateTime(2014, 11, 5, 13, 28, 40, 0);
            this.dtBeforeFilter.ValueChanged += new System.EventHandler(this.dtBeforeFilter_ValueChanged);
            // 
            // dtAfterFilter
            // 
            this.dtAfterFilter.CustomFormat = "MM/dd/yyyy";
            this.dtAfterFilter.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtAfterFilter.Location = new System.Drawing.Point(251, 158);
            this.dtAfterFilter.Name = "dtAfterFilter";
            this.dtAfterFilter.Size = new System.Drawing.Size(88, 20);
            this.dtAfterFilter.TabIndex = 41;
            this.dtAfterFilter.Value = new System.DateTime(2014, 11, 5, 13, 28, 40, 0);
            this.dtAfterFilter.ValueChanged += new System.EventHandler(this.dtAfterFilter_ValueChanged);
            // 
            // cbFilter
            // 
            this.cbFilter.FormattingEnabled = true;
            this.cbFilter.Items.AddRange(new object[] {
            "None",
            "Tested Date",
            "Registration Date"});
            this.cbFilter.Location = new System.Drawing.Point(80, 157);
            this.cbFilter.Name = "cbFilter";
            this.cbFilter.Size = new System.Drawing.Size(121, 21);
            this.cbFilter.TabIndex = 40;
            this.cbFilter.SelectedIndexChanged += new System.EventHandler(this.cbFilter_SelectedIndexChanged);
            // 
            // chkShowAll
            // 
            this.chkShowAll.AutoSize = true;
            this.chkShowAll.Location = new System.Drawing.Point(991, 124);
            this.chkShowAll.Name = "chkShowAll";
            this.chkShowAll.Size = new System.Drawing.Size(67, 17);
            this.chkShowAll.TabIndex = 37;
            this.chkShowAll.Text = "Show All";
            this.chkShowAll.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.nudNumberToSelect);
            this.groupBox1.Controls.Add(this.btnNotifyNow);
            this.groupBox1.Controls.Add(this.btnSetNotified);
            this.groupBox1.Controls.Add(this.btnSelectRandom);
            this.groupBox1.Controls.Add(this.btnNotify);
            this.groupBox1.Location = new System.Drawing.Point(620, 234);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(634, 41);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Notifications";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(510, 14);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(109, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Undo Sent In";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // nudNumberToSelect
            // 
            this.nudNumberToSelect.Location = new System.Drawing.Point(110, 17);
            this.nudNumberToSelect.Name = "nudNumberToSelect";
            this.nudNumberToSelect.Size = new System.Drawing.Size(44, 20);
            this.nudNumberToSelect.TabIndex = 5;
            this.nudNumberToSelect.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // btnNotifyNow
            // 
            this.btnNotifyNow.Location = new System.Drawing.Point(183, 14);
            this.btnNotifyNow.Margin = new System.Windows.Forms.Padding(2);
            this.btnNotifyNow.Name = "btnNotifyNow";
            this.btnNotifyNow.Size = new System.Drawing.Size(111, 23);
            this.btnNotifyNow.TabIndex = 4;
            this.btnNotifyNow.Text = "Notifiy Now";
            this.btnNotifyNow.UseVisualStyleBackColor = true;
            this.btnNotifyNow.Visible = false;
            this.btnNotifyNow.Click += new System.EventHandler(this.btnNotifyNow_Click);
            // 
            // btnSetNotified
            // 
            this.btnSetNotified.Location = new System.Drawing.Point(377, 14);
            this.btnSetNotified.Margin = new System.Windows.Forms.Padding(2);
            this.btnSetNotified.Name = "btnSetNotified";
            this.btnSetNotified.Size = new System.Drawing.Size(129, 23);
            this.btnSetNotified.TabIndex = 3;
            this.btnSetNotified.Text = "Manually Sent In";
            this.btnSetNotified.UseVisualStyleBackColor = true;
            this.btnSetNotified.Click += new System.EventHandler(this.btnSetNotified_Click);
            // 
            // btnSelectRandom
            // 
            this.btnSelectRandom.Location = new System.Drawing.Point(4, 14);
            this.btnSelectRandom.Margin = new System.Windows.Forms.Padding(2);
            this.btnSelectRandom.Name = "btnSelectRandom";
            this.btnSelectRandom.Size = new System.Drawing.Size(101, 23);
            this.btnSelectRandom.TabIndex = 1;
            this.btnSelectRandom.Text = "Select Random";
            this.btnSelectRandom.UseVisualStyleBackColor = true;
            this.btnSelectRandom.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnNotify
            // 
            this.btnNotify.Location = new System.Drawing.Point(298, 14);
            this.btnNotify.Margin = new System.Windows.Forms.Padding(2);
            this.btnNotify.Name = "btnNotify";
            this.btnNotify.Size = new System.Drawing.Size(75, 23);
            this.btnNotify.TabIndex = 0;
            this.btnNotify.Text = "Send In";
            this.btnNotify.UseVisualStyleBackColor = true;
            this.btnNotify.Click += new System.EventHandler(this.btnNotify_Click);
            // 
            // lblPresets
            // 
            this.lblPresets.AutoSize = true;
            this.lblPresets.Location = new System.Drawing.Point(505, 160);
            this.lblPresets.Name = "lblPresets";
            this.lblPresets.Size = new System.Drawing.Size(42, 13);
            this.lblPresets.TabIndex = 45;
            this.lblPresets.Text = "Presets";
            // 
            // cbPresets
            // 
            this.cbPresets.FormattingEnabled = true;
            this.cbPresets.Items.AddRange(new object[] {
            "Pick To Set",
            "Last 3 Days",
            "Last 7 Days",
            "Last 30 Days",
            "Last 60 Days",
            "Last 90 Days"});
            this.cbPresets.Location = new System.Drawing.Point(553, 157);
            this.cbPresets.Name = "cbPresets";
            this.cbPresets.Size = new System.Drawing.Size(121, 21);
            this.cbPresets.TabIndex = 46;
            this.cbPresets.SelectedIndexChanged += new System.EventHandler(this.cbPresets_SelectedIndexChanged);
            // 
            // FrmDonorSearch
            // 
            this.AcceptButton = this.btnSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1302, 632);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblPageHeader);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dgvSearchResult);
            this.Controls.Add(this.btnAddRemove);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnViewAll);
            this.Controls.Add(this.btnViewSelected);
            this.Controls.Add(this.btnDeselectAll);
            this.Controls.Add(this.btnSelectAll);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmDonorSearch";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Donor Search";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmDonorSearch_FormClosed);
            this.Load += new System.EventHandler(this.FrmDonorSearch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchResult)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudNumberToSelect)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSearchResult;
        private System.Windows.Forms.Button btnViewAll;
        private System.Windows.Forms.Button btnViewSelected;
        private System.Windows.Forms.Button btnDeselectAll;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label lblPageHeader;
        private System.Windows.Forms.Button btnAddRemove;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.Label lblCity;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.Label lblZipCode;
        private System.Windows.Forms.Label lblDepartment;
        private System.Windows.Forms.Label lblSSN;
        private System.Windows.Forms.Label lblSpecimebId;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblDOB;
        private System.Windows.Forms.Label lblTestReason;
        private System.Windows.Forms.Label lblTestType;
        private System.Windows.Forms.Label lblClient;
        private System.Windows.Forms.ComboBox cmbTestReason;
        private System.Windows.Forms.ComboBox cmbTestType;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.ComboBox cmbDepartment;
        private System.Windows.Forms.ComboBox cmbClient;
        private System.Windows.Forms.CheckBox chkIncludeArchived;
        private System.Windows.Forms.CheckBox chkwalkin;
        private System.Windows.Forms.RadioButton rbtnLast3Days;
        private System.Windows.Forms.RadioButton rbtnNone;
        private System.Windows.Forms.RadioButton rbtnLast30Days;
        private System.Windows.Forms.RadioButton rbtnLast60Days;
        private System.Windows.Forms.RadioButton rbtnLast90Days;
        private System.Windows.Forms.RadioButton rbtnDateRange;
        private System.Windows.Forms.RadioButton rbtnLast7Days;
        private Controls.TextBoxes.SurTextBox txtFirstName;
        private Controls.TextBoxes.SurTextBox txtCity;
        private Controls.TextBoxes.SurTextBox txtLastName;
        private Controls.TextBoxes.SurTextBox txtZipCode;
        private Controls.TextBoxes.SurTextBox txtSSN;
        private Controls.TextBoxes.SurTextBox txtSpecimenId;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.ComboBox cmbSearchMonth;
        private System.Windows.Forms.ComboBox cmbSearchDate;
        private System.Windows.Forms.ComboBox cmbSearchYear;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkShowAll;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnNotify;
        private System.Windows.Forms.Button btnSelectRandom;
        private System.Windows.Forms.Button btnSetNotified;
        private System.Windows.Forms.DataGridViewTextBoxColumn DonorDateNotifiedData;
        private System.Windows.Forms.NumericUpDown nudNumberToSelect;
        private System.Windows.Forms.Button btnNotifyNow;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn DonorSelection;
        private System.Windows.Forms.DataGridViewTextBoxColumn DonorFirstName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DonorLastName;
        private System.Windows.Forms.DataGridViewTextBoxColumn SSN;
        private System.Windows.Forms.DataGridViewTextBoxColumn DOB;
        private System.Windows.Forms.DataGridViewTextBoxColumn SpecimenID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClearStarID;
        private System.Windows.Forms.DataGridViewTextBoxColumn SpecimenDateValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClientName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DepartmentName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn PaymentMode;
        private System.Windows.Forms.DataGridViewTextBoxColumn PaymentMethodId;
        private System.Windows.Forms.DataGridViewTextBoxColumn PaymentAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn DonorTestRegisteredDateValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn DonorDateNotified;
        private System.Windows.Forms.DataGridViewTextBoxColumn backend_notifications_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn backend_notification_window_data_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn NotifiedViaFormFox;
        private System.Windows.Forms.DataGridViewTextBoxColumn PaymentDateValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn Result;
        private System.Windows.Forms.DataGridViewTextBoxColumn TestReason;
        private System.Windows.Forms.DataGridViewTextBoxColumn DonorCity;
        private System.Windows.Forms.DataGridViewTextBoxColumn ZipCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn MROType;
        private System.Windows.Forms.DataGridViewTextBoxColumn PaymentType;
        private System.Windows.Forms.DataGridViewTextBoxColumn DonorId;
        private System.Windows.Forms.DataGridViewTextBoxColumn DonorTestInfoId;
        private System.Windows.Forms.DataGridViewTextBoxColumn DonorInitialClientId;
        private System.Windows.Forms.DataGridViewTextBoxColumn TestInfoClientId;
        private System.Windows.Forms.DataGridViewTextBoxColumn TestInfoDepartmentId;
        private System.Windows.Forms.DataGridViewTextBoxColumn MROTypeId;
        private System.Windows.Forms.DataGridViewTextBoxColumn PaymentTypeId;
        private System.Windows.Forms.DataGridViewTextBoxColumn TestRequestedDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReasonForTestId;
        private System.Windows.Forms.DataGridViewTextBoxColumn TestCategoryId;
        private System.Windows.Forms.DataGridViewTextBoxColumn TestPanelId;
        private System.Windows.Forms.DataGridViewTextBoxColumn DonorDateOfBirth;
        private System.Windows.Forms.DataGridViewTextBoxColumn SpecimenDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn DonorRegistrationStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn TestStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn DonorSSN;
        private System.Windows.Forms.DataGridViewTextBoxColumn TestOverallResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn PaymentReceived;
        private System.Windows.Forms.DataGridViewTextBoxColumn PaymentDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn DonorTestRegisteredDate;
        private System.Windows.Forms.Label lblBefore;
        private System.Windows.Forms.Label lblAfter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtBeforeFilter;
        private System.Windows.Forms.DateTimePicker dtAfterFilter;
        private System.Windows.Forms.ComboBox cbFilter;
        private System.Windows.Forms.ComboBox cbPresets;
        private System.Windows.Forms.Label lblPresets;
        //  private Controls.TextBoxes.SurTextBox txtZipCode;
    }
}