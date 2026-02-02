namespace SurPath
{
    partial class FrmClientDetails
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Main Test (P909) ($ 200)");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Alt Test 1 (P802) ($ 150)");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Alt Test 2");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Alt Test 3");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Alt Test4");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("UA", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5});
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Main Test");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Alt Test 1");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Alt Test 2");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Alt Test 3");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Alt Test 4");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Hair", new System.Windows.Forms.TreeNode[] {
            treeNode7,
            treeNode8,
            treeNode9,
            treeNode10,
            treeNode11});
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("DNA");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("CNA", new System.Windows.Forms.TreeNode[] {
            treeNode6,
            treeNode12,
            treeNode13});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmClientDetails));
            this.lblClientCode = new System.Windows.Forms.Label();
            this.lblClientName = new System.Windows.Forms.Label();
            this.lblPageHeader = new System.Windows.Forms.Label();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblLab = new System.Windows.Forms.Label();
            this.cmbLab = new System.Windows.Forms.ComboBox();
            this.cmbMRO = new System.Windows.Forms.ComboBox();
            this.lblMRO = new System.Windows.Forms.Label();
            this.lblDepartmentNameValue = new System.Windows.Forms.Label();
            this.lblDepartmentNameHeader = new System.Windows.Forms.Label();
            this.cmbUAMainTestPanel = new System.Windows.Forms.ComboBox();
            this.lblUAPriceHeading = new System.Windows.Forms.Label();
            this.cmbHairMainTestPanel = new System.Windows.Forms.ComboBox();
            this.lblHairPriceHeading = new System.Windows.Forms.Label();
            this.lblHairTestPanelHeading = new System.Windows.Forms.Label();
            this.lblDNAPriceMan = new System.Windows.Forms.Label();
            this.lblDNATestPrice = new System.Windows.Forms.Label();
            this.lblClientMandatory = new System.Windows.Forms.Label();
            this.lblClientNameMan = new System.Windows.Forms.Label();
            this.lblLabMan = new System.Windows.Forms.Label();
            this.lblMROMan = new System.Windows.Forms.Label();
            this.lblClientCodeMan = new System.Windows.Forms.Label();
            this.cmbPhysicalState = new System.Windows.Forms.ComboBox();
            this.txtFax = new System.Windows.Forms.MaskedTextBox();
            this.txtPhone = new System.Windows.Forms.MaskedTextBox();
            this.lblZipCode = new System.Windows.Forms.Label();
            this.cmbSalesRepresentative = new System.Windows.Forms.ComboBox();
            this.lblSalesInfo = new System.Windows.Forms.Label();
            this.lblSalesRepresentive = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblFax = new System.Windows.Forms.Label();
            this.lblPhone = new System.Windows.Forms.Label();
            this.lblLastName = new System.Windows.Forms.Label();
            this.lblMainContactInfo = new System.Windows.Forms.Label();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.chkSameAsPhysical = new System.Windows.Forms.CheckBox();
            this.lblMailingState = new System.Windows.Forms.Label();
            this.lblPhysicalZipCode = new System.Windows.Forms.Label();
            this.lblMailingCity = new System.Windows.Forms.Label();
            this.lblPhysicalState = new System.Windows.Forms.Label();
            this.lblMailingAddress2 = new System.Windows.Forms.Label();
            this.lblPhysicalCity = new System.Windows.Forms.Label();
            this.lblPhysicalAddress1 = new System.Windows.Forms.Label();
            this.lblMailingAddress1 = new System.Windows.Forms.Label();
            this.lblPhysicalAddress2 = new System.Windows.Forms.Label();
            this.lblMailingAddress = new System.Windows.Forms.Label();
            this.lblPhysicalAddress = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblUATestPanelHeading = new System.Windows.Forms.Label();
            this.gboxUA = new System.Windows.Forms.GroupBox();
            this.txtUAMainTestPanelPrice = new SurPath.Controls.TextBoxes.SurTextBox();
            this.gboxHair = new System.Windows.Forms.GroupBox();
            this.txtHairMainTestPanelPrice = new SurPath.Controls.TextBoxes.SurTextBox();
            this.gboxDNA = new System.Windows.Forms.GroupBox();
            this.txtDNATestPrice = new SurPath.Controls.TextBoxes.SurTextBox();
            this.btnTestPanelNotFound = new System.Windows.Forms.Button();
            this.btnDepartmentDetails = new System.Windows.Forms.Button();
            this.btnDepartmentNotFound = new System.Windows.Forms.Button();
            this.tvDepartmentInfo = new System.Windows.Forms.TreeView();
            this.lblDepartment = new System.Windows.Forms.Label();
            this.btnDepartmentContact = new System.Windows.Forms.Button();
            this.cmbMailingState = new System.Windows.Forms.ComboBox();
            this.txtMailingZipCode = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtPhysicalZipCode = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtLastName = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtEmail = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtFirstName = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtMailingCity = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtMailingAddress2 = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtMailingAddress1 = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtPhysicalCity = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtPhysicalAddress2 = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtPhysicalAddress1 = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtClientCode = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtClientName = new SurPath.Controls.TextBoxes.SurTextBox();
            this.chkDepartmentAddress = new System.Windows.Forms.CheckBox();
            this.chkedittestcategory = new System.Windows.Forms.CheckBox();
            this.gboxBC = new System.Windows.Forms.GroupBox();
            this.lblBCTestPrice = new System.Windows.Forms.Label();
            this.txtBCTestPrice = new SurPath.Controls.TextBoxes.SurTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnBCItemNotFound = new System.Windows.Forms.Button();
            this.gboxRecordKeeping = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblRecordKeepingTestPrice = new System.Windows.Forms.Label();
            this.txtRecordKeepingTestPrice = new SurPath.Controls.TextBoxes.SurTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbTimeZone = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.gboxUA.SuspendLayout();
            this.gboxHair.SuspendLayout();
            this.gboxDNA.SuspendLayout();
            this.gboxBC.SuspendLayout();
            this.gboxRecordKeeping.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblClientCode
            // 
            this.lblClientCode.AutoSize = true;
            this.lblClientCode.Location = new System.Drawing.Point(13, 41);
            this.lblClientCode.Name = "lblClientCode";
            this.lblClientCode.Size = new System.Drawing.Size(61, 13);
            this.lblClientCode.TabIndex = 1;
            this.lblClientCode.Text = "Client Code";
            // 
            // lblClientName
            // 
            this.lblClientName.AutoSize = true;
            this.lblClientName.Location = new System.Drawing.Point(13, 75);
            this.lblClientName.Name = "lblClientName";
            this.lblClientName.Size = new System.Drawing.Size(64, 13);
            this.lblClientName.TabIndex = 4;
            this.lblClientName.Text = "Client Name";
            // 
            // lblPageHeader
            // 
            this.lblPageHeader.AutoSize = true;
            this.lblPageHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageHeader.Location = new System.Drawing.Point(12, 9);
            this.lblPageHeader.Name = "lblPageHeader";
            this.lblPageHeader.Size = new System.Drawing.Size(108, 20);
            this.lblPageHeader.TabIndex = 0;
            this.lblPageHeader.Text = "Client Setup";
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Checked = true;
            this.chkActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkActive.Location = new System.Drawing.Point(98, 98);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(56, 17);
            this.chkActive.TabIndex = 7;
            this.chkActive.Text = "Active";
            this.chkActive.UseVisualStyleBackColor = true;
            this.chkActive.CheckedChanged += new System.EventHandler(this.chkActive_CheckedChanged);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(553, 623);
            this.btnClose.Name = "btnClose";
            this.btnClose.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 64;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(462, 623);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 63;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblLab
            // 
            this.lblLab.AutoSize = true;
            this.lblLab.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLab.Location = new System.Drawing.Point(13, 131);
            this.lblLab.Name = "lblLab";
            this.lblLab.Size = new System.Drawing.Size(57, 13);
            this.lblLab.TabIndex = 8;
            this.lblLab.Text = "Laboratory";
            // 
            // cmbLab
            // 
            this.cmbLab.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLab.FormattingEnabled = true;
            this.cmbLab.Items.AddRange(new object[] {
            "(Select)",
            "aa"});
            this.cmbLab.Location = new System.Drawing.Point(98, 127);
            this.cmbLab.Name = "cmbLab";
            this.cmbLab.Size = new System.Drawing.Size(238, 21);
            this.cmbLab.TabIndex = 10;
            // 
            // cmbMRO
            // 
            this.cmbMRO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMRO.FormattingEnabled = true;
            this.cmbMRO.Items.AddRange(new object[] {
            "(Select)",
            "cc"});
            this.cmbMRO.Location = new System.Drawing.Point(98, 162);
            this.cmbMRO.Name = "cmbMRO";
            this.cmbMRO.Size = new System.Drawing.Size(238, 21);
            this.cmbMRO.TabIndex = 13;
            // 
            // lblMRO
            // 
            this.lblMRO.AutoSize = true;
            this.lblMRO.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMRO.Location = new System.Drawing.Point(13, 166);
            this.lblMRO.Name = "lblMRO";
            this.lblMRO.Size = new System.Drawing.Size(32, 13);
            this.lblMRO.TabIndex = 11;
            this.lblMRO.Text = "MRO";
            // 
            // lblDepartmentNameValue
            // 
            this.lblDepartmentNameValue.AutoSize = true;
            this.lblDepartmentNameValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDepartmentNameValue.ForeColor = System.Drawing.Color.Maroon;
            this.lblDepartmentNameValue.Location = new System.Drawing.Point(669, 284);
            this.lblDepartmentNameValue.Name = "lblDepartmentNameValue";
            this.lblDepartmentNameValue.Size = new System.Drawing.Size(32, 13);
            this.lblDepartmentNameValue.TabIndex = 59;
            this.lblDepartmentNameValue.Text = "CNA";
            // 
            // lblDepartmentNameHeader
            // 
            this.lblDepartmentNameHeader.AutoSize = true;
            this.lblDepartmentNameHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDepartmentNameHeader.Location = new System.Drawing.Point(558, 284);
            this.lblDepartmentNameHeader.Name = "lblDepartmentNameHeader";
            this.lblDepartmentNameHeader.Size = new System.Drawing.Size(108, 13);
            this.lblDepartmentNameHeader.TabIndex = 58;
            this.lblDepartmentNameHeader.Text = "Department Name";
            // 
            // cmbUAMainTestPanel
            // 
            this.cmbUAMainTestPanel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUAMainTestPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbUAMainTestPanel.FormattingEnabled = true;
            this.cmbUAMainTestPanel.Items.AddRange(new object[] {
            "(Select)",
            "V909"});
            this.cmbUAMainTestPanel.Location = new System.Drawing.Point(85, 18);
            this.cmbUAMainTestPanel.MaxDropDownItems = 10;
            this.cmbUAMainTestPanel.Name = "cmbUAMainTestPanel";
            this.cmbUAMainTestPanel.Size = new System.Drawing.Size(124, 21);
            this.cmbUAMainTestPanel.TabIndex = 2;
            this.cmbUAMainTestPanel.Tag = "UA_Main";
            this.cmbUAMainTestPanel.SelectedIndexChanged += new System.EventHandler(this.cmbTestPanel_SelectedIndexChanged);
            // 
            // lblUAPriceHeading
            // 
            this.lblUAPriceHeading.AutoSize = true;
            this.lblUAPriceHeading.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUAPriceHeading.Location = new System.Drawing.Point(10, 51);
            this.lblUAPriceHeading.Name = "lblUAPriceHeading";
            this.lblUAPriceHeading.Size = new System.Drawing.Size(31, 13);
            this.lblUAPriceHeading.TabIndex = 3;
            this.lblUAPriceHeading.Text = "Price";
            // 
            // cmbHairMainTestPanel
            // 
            this.cmbHairMainTestPanel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbHairMainTestPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbHairMainTestPanel.FormattingEnabled = true;
            this.cmbHairMainTestPanel.Items.AddRange(new object[] {
            "(Select)",
            "V909"});
            this.cmbHairMainTestPanel.Location = new System.Drawing.Point(148, 17);
            this.cmbHairMainTestPanel.MaxDropDownItems = 10;
            this.cmbHairMainTestPanel.Name = "cmbHairMainTestPanel";
            this.cmbHairMainTestPanel.Size = new System.Drawing.Size(124, 21);
            this.cmbHairMainTestPanel.TabIndex = 2;
            this.cmbHairMainTestPanel.Tag = "Hair_Main";
            this.cmbHairMainTestPanel.SelectedIndexChanged += new System.EventHandler(this.cmbTestPanel_SelectedIndexChanged);
            // 
            // lblHairPriceHeading
            // 
            this.lblHairPriceHeading.AutoSize = true;
            this.lblHairPriceHeading.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHairPriceHeading.Location = new System.Drawing.Point(6, 51);
            this.lblHairPriceHeading.Name = "lblHairPriceHeading";
            this.lblHairPriceHeading.Size = new System.Drawing.Size(124, 13);
            this.lblHairPriceHeading.TabIndex = 3;
            this.lblHairPriceHeading.Text = "Price Per 90 Days / Test";
            // 
            // lblHairTestPanelHeading
            // 
            this.lblHairTestPanelHeading.AutoSize = true;
            this.lblHairTestPanelHeading.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHairTestPanelHeading.Location = new System.Drawing.Point(6, 21);
            this.lblHairTestPanelHeading.Name = "lblHairTestPanelHeading";
            this.lblHairTestPanelHeading.Size = new System.Drawing.Size(58, 13);
            this.lblHairTestPanelHeading.TabIndex = 0;
            this.lblHairTestPanelHeading.Text = "Test Panel";
            // 
            // lblDNAPriceMan
            // 
            this.lblDNAPriceMan.AutoSize = true;
            this.lblDNAPriceMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDNAPriceMan.ForeColor = System.Drawing.Color.Red;
            this.lblDNAPriceMan.Location = new System.Drawing.Point(89, 20);
            this.lblDNAPriceMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDNAPriceMan.Name = "lblDNAPriceMan";
            this.lblDNAPriceMan.Size = new System.Drawing.Size(13, 17);
            this.lblDNAPriceMan.TabIndex = 1;
            this.lblDNAPriceMan.Text = "*";
            // 
            // lblDNATestPrice
            // 
            this.lblDNATestPrice.AutoSize = true;
            this.lblDNATestPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDNATestPrice.Location = new System.Drawing.Point(10, 22);
            this.lblDNATestPrice.Name = "lblDNATestPrice";
            this.lblDNATestPrice.Size = new System.Drawing.Size(81, 13);
            this.lblDNATestPrice.TabIndex = 0;
            this.lblDNATestPrice.Text = "DNA Test Price";
            // 
            // lblClientMandatory
            // 
            this.lblClientMandatory.AutoSize = true;
            this.lblClientMandatory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClientMandatory.ForeColor = System.Drawing.Color.Red;
            this.lblClientMandatory.Location = new System.Drawing.Point(989, 16);
            this.lblClientMandatory.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblClientMandatory.Name = "lblClientMandatory";
            this.lblClientMandatory.Size = new System.Drawing.Size(94, 13);
            this.lblClientMandatory.TabIndex = 65;
            this.lblClientMandatory.Text = "* Mandatory Fields";
            // 
            // lblClientNameMan
            // 
            this.lblClientNameMan.AutoSize = true;
            this.lblClientNameMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClientNameMan.ForeColor = System.Drawing.Color.Red;
            this.lblClientNameMan.Location = new System.Drawing.Point(73, 73);
            this.lblClientNameMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblClientNameMan.Name = "lblClientNameMan";
            this.lblClientNameMan.Size = new System.Drawing.Size(13, 17);
            this.lblClientNameMan.TabIndex = 5;
            this.lblClientNameMan.Text = "*";
            // 
            // lblLabMan
            // 
            this.lblLabMan.AutoSize = true;
            this.lblLabMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLabMan.ForeColor = System.Drawing.Color.Red;
            this.lblLabMan.Location = new System.Drawing.Point(68, 129);
            this.lblLabMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLabMan.Name = "lblLabMan";
            this.lblLabMan.Size = new System.Drawing.Size(13, 17);
            this.lblLabMan.TabIndex = 9;
            this.lblLabMan.Text = "*";
            // 
            // lblMROMan
            // 
            this.lblMROMan.AutoSize = true;
            this.lblMROMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMROMan.ForeColor = System.Drawing.Color.Red;
            this.lblMROMan.Location = new System.Drawing.Point(41, 164);
            this.lblMROMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMROMan.Name = "lblMROMan";
            this.lblMROMan.Size = new System.Drawing.Size(13, 17);
            this.lblMROMan.TabIndex = 12;
            this.lblMROMan.Text = "*";
            // 
            // lblClientCodeMan
            // 
            this.lblClientCodeMan.AutoSize = true;
            this.lblClientCodeMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClientCodeMan.ForeColor = System.Drawing.Color.Red;
            this.lblClientCodeMan.Location = new System.Drawing.Point(73, 39);
            this.lblClientCodeMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblClientCodeMan.Name = "lblClientCodeMan";
            this.lblClientCodeMan.Size = new System.Drawing.Size(13, 17);
            this.lblClientCodeMan.TabIndex = 2;
            this.lblClientCodeMan.Text = "*";
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
            this.cmbPhysicalState.Location = new System.Drawing.Point(259, 362);
            this.cmbPhysicalState.Name = "cmbPhysicalState";
            this.cmbPhysicalState.Size = new System.Drawing.Size(113, 21);
            this.cmbPhysicalState.TabIndex = 34;
            // 
            // txtFax
            // 
            this.txtFax.Location = new System.Drawing.Point(364, 252);
            this.txtFax.Mask = "(999) 000-0000";
            this.txtFax.Name = "txtFax";
            this.txtFax.Size = new System.Drawing.Size(120, 20);
            this.txtFax.TabIndex = 23;
            this.txtFax.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtFax_MouseClick);
            this.txtFax.TextChanged += new System.EventHandler(this.txtFax_TextChanged);
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(94, 249);
            this.txtPhone.Mask = "(999) 000-0000";
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(120, 20);
            this.txtPhone.TabIndex = 21;
            this.txtPhone.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtPhone_MouseClick);
            this.txtPhone.TextChanged += new System.EventHandler(this.txtPhone_TextChanged);
            // 
            // lblZipCode
            // 
            this.lblZipCode.AutoSize = true;
            this.lblZipCode.Location = new System.Drawing.Point(383, 464);
            this.lblZipCode.Name = "lblZipCode";
            this.lblZipCode.Size = new System.Drawing.Size(50, 13);
            this.lblZipCode.TabIndex = 47;
            this.lblZipCode.Text = "Zip Code";
            // 
            // cmbSalesRepresentative
            // 
            this.cmbSalesRepresentative.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSalesRepresentative.FormattingEnabled = true;
            this.cmbSalesRepresentative.Items.AddRange(new object[] {
            "(Select)"});
            this.cmbSalesRepresentative.Location = new System.Drawing.Point(140, 521);
            this.cmbSalesRepresentative.Name = "cmbSalesRepresentative";
            this.cmbSalesRepresentative.Size = new System.Drawing.Size(192, 21);
            this.cmbSalesRepresentative.TabIndex = 51;
            this.cmbSalesRepresentative.SelectedIndexChanged += new System.EventHandler(this.cmbSalesRepresentative_SelectedIndexChanged);
            // 
            // lblSalesInfo
            // 
            this.lblSalesInfo.AutoSize = true;
            this.lblSalesInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSalesInfo.Location = new System.Drawing.Point(13, 493);
            this.lblSalesInfo.Name = "lblSalesInfo";
            this.lblSalesInfo.Size = new System.Drawing.Size(105, 13);
            this.lblSalesInfo.TabIndex = 49;
            this.lblSalesInfo.Text = "Sales Information";
            // 
            // lblSalesRepresentive
            // 
            this.lblSalesRepresentive.AutoSize = true;
            this.lblSalesRepresentive.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSalesRepresentive.Location = new System.Drawing.Point(13, 525);
            this.lblSalesRepresentive.Name = "lblSalesRepresentive";
            this.lblSalesRepresentive.Size = new System.Drawing.Size(108, 13);
            this.lblSalesRepresentive.TabIndex = 50;
            this.lblSalesRepresentive.Text = "Sales Representative";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(13, 283);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(32, 13);
            this.lblEmail.TabIndex = 24;
            this.lblEmail.Text = "Email";
            // 
            // lblFax
            // 
            this.lblFax.AutoSize = true;
            this.lblFax.Location = new System.Drawing.Point(287, 256);
            this.lblFax.Name = "lblFax";
            this.lblFax.Size = new System.Drawing.Size(24, 13);
            this.lblFax.TabIndex = 22;
            this.lblFax.Text = "Fax";
            // 
            // lblPhone
            // 
            this.lblPhone.AutoSize = true;
            this.lblPhone.Location = new System.Drawing.Point(13, 253);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(38, 13);
            this.lblPhone.TabIndex = 20;
            this.lblPhone.Text = "Phone";
            // 
            // lblLastName
            // 
            this.lblLastName.AutoSize = true;
            this.lblLastName.Location = new System.Drawing.Point(287, 220);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(58, 13);
            this.lblLastName.TabIndex = 18;
            this.lblLastName.Text = "Last Name";
            // 
            // lblMainContactInfo
            // 
            this.lblMainContactInfo.AutoSize = true;
            this.lblMainContactInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMainContactInfo.Location = new System.Drawing.Point(13, 195);
            this.lblMainContactInfo.Name = "lblMainContactInfo";
            this.lblMainContactInfo.Size = new System.Drawing.Size(149, 13);
            this.lblMainContactInfo.TabIndex = 15;
            this.lblMainContactInfo.Text = "Main Contact Information";
            // 
            // lblFirstName
            // 
            this.lblFirstName.AutoSize = true;
            this.lblFirstName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFirstName.Location = new System.Drawing.Point(13, 220);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(60, 13);
            this.lblFirstName.TabIndex = 16;
            this.lblFirstName.Text = "First Name ";
            // 
            // chkSameAsPhysical
            // 
            this.chkSameAsPhysical.AutoSize = true;
            this.chkSameAsPhysical.Location = new System.Drawing.Point(140, 401);
            this.chkSameAsPhysical.Name = "chkSameAsPhysical";
            this.chkSameAsPhysical.Size = new System.Drawing.Size(150, 17);
            this.chkSameAsPhysical.TabIndex = 38;
            this.chkSameAsPhysical.Text = "Same as Physical Address";
            this.chkSameAsPhysical.UseVisualStyleBackColor = true;
            this.chkSameAsPhysical.CheckedChanged += new System.EventHandler(this.chkSameAsPhysical_CheckedChanged);
            // 
            // lblMailingState
            // 
            this.lblMailingState.AutoSize = true;
            this.lblMailingState.Location = new System.Drawing.Point(215, 464);
            this.lblMailingState.Name = "lblMailingState";
            this.lblMailingState.Size = new System.Drawing.Size(32, 13);
            this.lblMailingState.TabIndex = 45;
            this.lblMailingState.Text = "State";
            // 
            // lblPhysicalZipCode
            // 
            this.lblPhysicalZipCode.AutoSize = true;
            this.lblPhysicalZipCode.Location = new System.Drawing.Point(383, 366);
            this.lblPhysicalZipCode.Name = "lblPhysicalZipCode";
            this.lblPhysicalZipCode.Size = new System.Drawing.Size(50, 13);
            this.lblPhysicalZipCode.TabIndex = 35;
            this.lblPhysicalZipCode.Text = "Zip Code";
            // 
            // lblMailingCity
            // 
            this.lblMailingCity.AutoSize = true;
            this.lblMailingCity.Location = new System.Drawing.Point(13, 464);
            this.lblMailingCity.Name = "lblMailingCity";
            this.lblMailingCity.Size = new System.Drawing.Size(24, 13);
            this.lblMailingCity.TabIndex = 43;
            this.lblMailingCity.Text = "City";
            // 
            // lblPhysicalState
            // 
            this.lblPhysicalState.AutoSize = true;
            this.lblPhysicalState.Location = new System.Drawing.Point(215, 366);
            this.lblPhysicalState.Name = "lblPhysicalState";
            this.lblPhysicalState.Size = new System.Drawing.Size(32, 13);
            this.lblPhysicalState.TabIndex = 33;
            this.lblPhysicalState.Text = "State";
            // 
            // lblMailingAddress2
            // 
            this.lblMailingAddress2.AutoSize = true;
            this.lblMailingAddress2.Location = new System.Drawing.Point(287, 434);
            this.lblMailingAddress2.Name = "lblMailingAddress2";
            this.lblMailingAddress2.Size = new System.Drawing.Size(54, 13);
            this.lblMailingAddress2.TabIndex = 41;
            this.lblMailingAddress2.Text = "Address 2";
            // 
            // lblPhysicalCity
            // 
            this.lblPhysicalCity.AutoSize = true;
            this.lblPhysicalCity.Location = new System.Drawing.Point(13, 366);
            this.lblPhysicalCity.Name = "lblPhysicalCity";
            this.lblPhysicalCity.Size = new System.Drawing.Size(24, 13);
            this.lblPhysicalCity.TabIndex = 31;
            this.lblPhysicalCity.Text = "City";
            // 
            // lblPhysicalAddress1
            // 
            this.lblPhysicalAddress1.AutoSize = true;
            this.lblPhysicalAddress1.Location = new System.Drawing.Point(13, 335);
            this.lblPhysicalAddress1.Name = "lblPhysicalAddress1";
            this.lblPhysicalAddress1.Size = new System.Drawing.Size(54, 13);
            this.lblPhysicalAddress1.TabIndex = 27;
            this.lblPhysicalAddress1.Text = "Address 1";
            // 
            // lblMailingAddress1
            // 
            this.lblMailingAddress1.AutoSize = true;
            this.lblMailingAddress1.Location = new System.Drawing.Point(13, 434);
            this.lblMailingAddress1.Name = "lblMailingAddress1";
            this.lblMailingAddress1.Size = new System.Drawing.Size(54, 13);
            this.lblMailingAddress1.TabIndex = 39;
            this.lblMailingAddress1.Text = "Address 1";
            // 
            // lblPhysicalAddress2
            // 
            this.lblPhysicalAddress2.AutoSize = true;
            this.lblPhysicalAddress2.Location = new System.Drawing.Point(291, 335);
            this.lblPhysicalAddress2.Name = "lblPhysicalAddress2";
            this.lblPhysicalAddress2.Size = new System.Drawing.Size(54, 13);
            this.lblPhysicalAddress2.TabIndex = 29;
            this.lblPhysicalAddress2.Text = "Address 2";
            // 
            // lblMailingAddress
            // 
            this.lblMailingAddress.AutoSize = true;
            this.lblMailingAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMailingAddress.Location = new System.Drawing.Point(13, 401);
            this.lblMailingAddress.Name = "lblMailingAddress";
            this.lblMailingAddress.Size = new System.Drawing.Size(96, 13);
            this.lblMailingAddress.TabIndex = 37;
            this.lblMailingAddress.Text = "Mailing Address";
            // 
            // lblPhysicalAddress
            // 
            this.lblPhysicalAddress.AutoSize = true;
            this.lblPhysicalAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPhysicalAddress.Location = new System.Drawing.Point(13, 310);
            this.lblPhysicalAddress.Name = "lblPhysicalAddress";
            this.lblPhysicalAddress.Size = new System.Drawing.Size(103, 13);
            this.lblPhysicalAddress.TabIndex = 26;
            this.lblPhysicalAddress.Text = "Physical Address";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(69, 19);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(13, 17);
            this.label4.TabIndex = 1;
            this.label4.Text = "*";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(67, 20);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(13, 17);
            this.label8.TabIndex = 1;
            this.label8.Text = "*";
            // 
            // lblUATestPanelHeading
            // 
            this.lblUATestPanelHeading.AutoSize = true;
            this.lblUATestPanelHeading.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUATestPanelHeading.Location = new System.Drawing.Point(10, 22);
            this.lblUATestPanelHeading.Name = "lblUATestPanelHeading";
            this.lblUATestPanelHeading.Size = new System.Drawing.Size(58, 13);
            this.lblUATestPanelHeading.TabIndex = 0;
            this.lblUATestPanelHeading.Text = "Test Panel";
            // 
            // gboxUA
            // 
            this.gboxUA.Controls.Add(this.lblUATestPanelHeading);
            this.gboxUA.Controls.Add(this.txtUAMainTestPanelPrice);
            this.gboxUA.Controls.Add(this.lblUAPriceHeading);
            this.gboxUA.Controls.Add(this.label8);
            this.gboxUA.Controls.Add(this.cmbUAMainTestPanel);
            this.gboxUA.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gboxUA.Location = new System.Drawing.Point(558, 315);
            this.gboxUA.Name = "gboxUA";
            this.gboxUA.Size = new System.Drawing.Size(220, 77);
            this.gboxUA.TabIndex = 60;
            this.gboxUA.TabStop = false;
            this.gboxUA.Text = "Urinalysis";
            // 
            // txtUAMainTestPanelPrice
            // 
            this.txtUAMainTestPanelPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUAMainTestPanelPrice.Location = new System.Drawing.Point(87, 49);
            this.txtUAMainTestPanelPrice.MaxLength = 6;
            this.txtUAMainTestPanelPrice.Multiline = true;
            this.txtUAMainTestPanelPrice.Name = "txtUAMainTestPanelPrice";
            this.txtUAMainTestPanelPrice.Size = new System.Drawing.Size(82, 20);
            this.txtUAMainTestPanelPrice.TabIndex = 4;
            this.txtUAMainTestPanelPrice.Tag = "UA_Main";
            this.txtUAMainTestPanelPrice.WaterMark = "Enter Price";
            this.txtUAMainTestPanelPrice.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtUAMainTestPanelPrice.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUAMainTestPanelPrice.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtUAMainTestPanelPrice.TextChanged += new System.EventHandler(this.txtUAMainTestPanelPrice_TextChanged);
            this.txtUAMainTestPanelPrice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUAMainTestPanelPrice_KeyPress);
            this.txtUAMainTestPanelPrice.Leave += new System.EventHandler(this.txtTestPanelPrice_Leave);
            this.txtUAMainTestPanelPrice.Validating += new System.ComponentModel.CancelEventHandler(this.txtTestPanelPrice_Validating);
            // 
            // gboxHair
            // 
            this.gboxHair.Controls.Add(this.lblHairTestPanelHeading);
            this.gboxHair.Controls.Add(this.lblHairPriceHeading);
            this.gboxHair.Controls.Add(this.txtHairMainTestPanelPrice);
            this.gboxHair.Controls.Add(this.label4);
            this.gboxHair.Controls.Add(this.cmbHairMainTestPanel);
            this.gboxHair.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gboxHair.Location = new System.Drawing.Point(794, 315);
            this.gboxHair.Name = "gboxHair";
            this.gboxHair.Size = new System.Drawing.Size(286, 77);
            this.gboxHair.TabIndex = 61;
            this.gboxHair.TabStop = false;
            this.gboxHair.Text = "Hair Analysis";
            // 
            // txtHairMainTestPanelPrice
            // 
            this.txtHairMainTestPanelPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHairMainTestPanelPrice.Location = new System.Drawing.Point(148, 47);
            this.txtHairMainTestPanelPrice.MaxLength = 6;
            this.txtHairMainTestPanelPrice.Multiline = true;
            this.txtHairMainTestPanelPrice.Name = "txtHairMainTestPanelPrice";
            this.txtHairMainTestPanelPrice.Size = new System.Drawing.Size(82, 20);
            this.txtHairMainTestPanelPrice.TabIndex = 4;
            this.txtHairMainTestPanelPrice.Tag = "Hair_Main";
            this.txtHairMainTestPanelPrice.WaterMark = "Enter Price";
            this.txtHairMainTestPanelPrice.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtHairMainTestPanelPrice.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHairMainTestPanelPrice.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtHairMainTestPanelPrice.TextChanged += new System.EventHandler(this.txtHairMainTestPanelPrice_TextChanged);
            this.txtHairMainTestPanelPrice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtHairMainTestPanelPrice_KeyPress);
            this.txtHairMainTestPanelPrice.Leave += new System.EventHandler(this.txtTestPanelPrice_Leave);
            this.txtHairMainTestPanelPrice.Validating += new System.ComponentModel.CancelEventHandler(this.txtTestPanelPrice_Validating);
            // 
            // gboxDNA
            // 
            this.gboxDNA.Controls.Add(this.lblDNATestPrice);
            this.gboxDNA.Controls.Add(this.txtDNATestPrice);
            this.gboxDNA.Controls.Add(this.lblDNAPriceMan);
            this.gboxDNA.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gboxDNA.Location = new System.Drawing.Point(794, 404);
            this.gboxDNA.Name = "gboxDNA";
            this.gboxDNA.Size = new System.Drawing.Size(285, 46);
            this.gboxDNA.TabIndex = 62;
            this.gboxDNA.TabStop = false;
            this.gboxDNA.Text = "DNA";
            // 
            // txtDNATestPrice
            // 
            this.txtDNATestPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDNATestPrice.Location = new System.Drawing.Point(111, 18);
            this.txtDNATestPrice.MaxLength = 6;
            this.txtDNATestPrice.Multiline = true;
            this.txtDNATestPrice.Name = "txtDNATestPrice";
            this.txtDNATestPrice.Size = new System.Drawing.Size(82, 20);
            this.txtDNATestPrice.TabIndex = 2;
            this.txtDNATestPrice.Tag = "DNA_Main";
            this.txtDNATestPrice.WaterMark = "Enter Price";
            this.txtDNATestPrice.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtDNATestPrice.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDNATestPrice.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtDNATestPrice.TextChanged += new System.EventHandler(this.txtDNATestPrice_TextChanged);
            this.txtDNATestPrice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDNATestPrice_KeyPress);
            this.txtDNATestPrice.Leave += new System.EventHandler(this.txtTestPanelPrice_Leave);
            this.txtDNATestPrice.Validating += new System.ComponentModel.CancelEventHandler(this.txtTestPanelPrice_Validating);
            // 
            // btnTestPanelNotFound
            // 
            this.btnTestPanelNotFound.Location = new System.Drawing.Point(946, 216);
            this.btnTestPanelNotFound.Name = "btnTestPanelNotFound";
            this.btnTestPanelNotFound.Size = new System.Drawing.Size(133, 23);
            this.btnTestPanelNotFound.TabIndex = 57;
            this.btnTestPanelNotFound.Text = "Test Panel not found";
            this.btnTestPanelNotFound.UseVisualStyleBackColor = true;
            this.btnTestPanelNotFound.Click += new System.EventHandler(this.btnTestPanelNotFound_Click);
            // 
            // btnDepartmentDetails
            // 
            this.btnDepartmentDetails.Location = new System.Drawing.Point(706, 216);
            this.btnDepartmentDetails.Name = "btnDepartmentDetails";
            this.btnDepartmentDetails.Size = new System.Drawing.Size(75, 23);
            this.btnDepartmentDetails.TabIndex = 55;
            this.btnDepartmentDetails.Text = "Details";
            this.btnDepartmentDetails.UseVisualStyleBackColor = true;
            this.btnDepartmentDetails.Click += new System.EventHandler(this.btnDepartmentDetails_Click);
            // 
            // btnDepartmentNotFound
            // 
            this.btnDepartmentNotFound.Location = new System.Drawing.Point(558, 216);
            this.btnDepartmentNotFound.Name = "btnDepartmentNotFound";
            this.btnDepartmentNotFound.Size = new System.Drawing.Size(133, 23);
            this.btnDepartmentNotFound.TabIndex = 54;
            this.btnDepartmentNotFound.Text = "Department not found";
            this.btnDepartmentNotFound.UseVisualStyleBackColor = true;
            this.btnDepartmentNotFound.Click += new System.EventHandler(this.btnDepartmentNotFound_Click);
            // 
            // tvDepartmentInfo
            // 
            this.tvDepartmentInfo.CheckBoxes = true;
            this.tvDepartmentInfo.Location = new System.Drawing.Point(558, 62);
            this.tvDepartmentInfo.Name = "tvDepartmentInfo";
            treeNode1.Name = "Node0";
            treeNode1.Text = "Main Test (P909) ($ 200)";
            treeNode2.Name = "Node2";
            treeNode2.Text = "Alt Test 1 (P802) ($ 150)";
            treeNode3.Checked = true;
            treeNode3.Name = "Node3";
            treeNode3.Text = "Alt Test 2";
            treeNode4.Name = "Node4";
            treeNode4.Text = "Alt Test 3";
            treeNode5.Name = "Node5";
            treeNode5.Text = "Alt Test4";
            treeNode6.Name = "Node1";
            treeNode6.Text = "UA";
            treeNode7.Name = "Node1";
            treeNode7.Text = "Main Test";
            treeNode8.Name = "Node2";
            treeNode8.Text = "Alt Test 1";
            treeNode9.Name = "Node3";
            treeNode9.Text = "Alt Test 2";
            treeNode10.Name = "Node4";
            treeNode10.Text = "Alt Test 3";
            treeNode11.Name = "Node5";
            treeNode11.Text = "Alt Test 4";
            treeNode12.Name = "Node6";
            treeNode12.Text = "Hair";
            treeNode13.Name = "Node7";
            treeNode13.Text = "DNA";
            treeNode14.Checked = true;
            treeNode14.Name = "Node0";
            treeNode14.Text = "CNA";
            this.tvDepartmentInfo.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode14});
            this.tvDepartmentInfo.Size = new System.Drawing.Size(521, 142);
            this.tvDepartmentInfo.TabIndex = 53;
            this.tvDepartmentInfo.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvDepartmentInfo_AfterCheck);
            this.tvDepartmentInfo.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvDepartmentInfo_AfterSelect);
            // 
            // lblDepartment
            // 
            this.lblDepartment.AutoSize = true;
            this.lblDepartment.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDepartment.Location = new System.Drawing.Point(558, 37);
            this.lblDepartment.Name = "lblDepartment";
            this.lblDepartment.Size = new System.Drawing.Size(139, 13);
            this.lblDepartment.TabIndex = 52;
            this.lblDepartment.Text = "Department Information";
            // 
            // btnDepartmentContact
            // 
            this.btnDepartmentContact.AutoSize = true;
            this.btnDepartmentContact.Location = new System.Drawing.Point(794, 216);
            this.btnDepartmentContact.Name = "btnDepartmentContact";
            this.btnDepartmentContact.Size = new System.Drawing.Size(135, 23);
            this.btnDepartmentContact.TabIndex = 56;
            this.btnDepartmentContact.Text = "View Contact Information";
            this.btnDepartmentContact.UseVisualStyleBackColor = true;
            this.btnDepartmentContact.Click += new System.EventHandler(this.btnDepartmentContact_Click);
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
            this.cmbMailingState.Location = new System.Drawing.Point(259, 460);
            this.cmbMailingState.Name = "cmbMailingState";
            this.cmbMailingState.Size = new System.Drawing.Size(113, 21);
            this.cmbMailingState.TabIndex = 46;
            this.cmbMailingState.TextChanged += new System.EventHandler(this.cmbMailingState_TextChanged);
            // 
            // txtMailingZipCode
            // 
            this.txtMailingZipCode.Location = new System.Drawing.Point(439, 460);
            this.txtMailingZipCode.MaxLength = 5;
            this.txtMailingZipCode.Name = "txtMailingZipCode";
            this.txtMailingZipCode.Size = new System.Drawing.Size(84, 20);
            this.txtMailingZipCode.TabIndex = 48;
            this.txtMailingZipCode.WaterMark = "Enter  Zip Code";
            this.txtMailingZipCode.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtMailingZipCode.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMailingZipCode.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtMailingZipCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMailingZipCode_KeyPress);
            // 
            // txtPhysicalZipCode
            // 
            this.txtPhysicalZipCode.Location = new System.Drawing.Point(439, 362);
            this.txtPhysicalZipCode.MaxLength = 5;
            this.txtPhysicalZipCode.Name = "txtPhysicalZipCode";
            this.txtPhysicalZipCode.Size = new System.Drawing.Size(84, 20);
            this.txtPhysicalZipCode.TabIndex = 36;
            this.txtPhysicalZipCode.WaterMark = "Enter  Zip Code";
            this.txtPhysicalZipCode.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtPhysicalZipCode.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhysicalZipCode.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtPhysicalZipCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPhysicalZipCode_KeyPress);
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(364, 216);
            this.txtLastName.MaxLength = 200;
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(173, 20);
            this.txtLastName.TabIndex = 19;
            this.txtLastName.WaterMark = "Enter  Last Name";
            this.txtLastName.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtLastName.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLastName.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtLastName.TextChanged += new System.EventHandler(this.txtLastName_TextChanged);
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(94, 279);
            this.txtEmail.MaxLength = 320;
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(173, 20);
            this.txtEmail.TabIndex = 25;
            this.txtEmail.WaterMark = "Enter  Email";
            this.txtEmail.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtEmail.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtEmail.TextChanged += new System.EventHandler(this.txtEmail_TextChanged);
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(94, 216);
            this.txtFirstName.MaxLength = 200;
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(173, 20);
            this.txtFirstName.TabIndex = 17;
            this.txtFirstName.WaterMark = "Enter First Name";
            this.txtFirstName.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtFirstName.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFirstName.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtFirstName.TextChanged += new System.EventHandler(this.txtFirstName_TextChanged);
            // 
            // txtMailingCity
            // 
            this.txtMailingCity.Location = new System.Drawing.Point(94, 460);
            this.txtMailingCity.MaxLength = 225;
            this.txtMailingCity.Multiline = true;
            this.txtMailingCity.Name = "txtMailingCity";
            this.txtMailingCity.Size = new System.Drawing.Size(113, 20);
            this.txtMailingCity.TabIndex = 44;
            this.txtMailingCity.WaterMark = "Enter City";
            this.txtMailingCity.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtMailingCity.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMailingCity.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // txtMailingAddress2
            // 
            this.txtMailingAddress2.Location = new System.Drawing.Point(364, 430);
            this.txtMailingAddress2.MaxLength = 225;
            this.txtMailingAddress2.Multiline = true;
            this.txtMailingAddress2.Name = "txtMailingAddress2";
            this.txtMailingAddress2.Size = new System.Drawing.Size(173, 20);
            this.txtMailingAddress2.TabIndex = 42;
            this.txtMailingAddress2.WaterMark = "Enter Address 2";
            this.txtMailingAddress2.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtMailingAddress2.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMailingAddress2.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // txtMailingAddress1
            // 
            this.txtMailingAddress1.Location = new System.Drawing.Point(94, 430);
            this.txtMailingAddress1.MaxLength = 225;
            this.txtMailingAddress1.Multiline = true;
            this.txtMailingAddress1.Name = "txtMailingAddress1";
            this.txtMailingAddress1.Size = new System.Drawing.Size(173, 20);
            this.txtMailingAddress1.TabIndex = 40;
            this.txtMailingAddress1.WaterMark = "Enter Address 1";
            this.txtMailingAddress1.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtMailingAddress1.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMailingAddress1.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // txtPhysicalCity
            // 
            this.txtPhysicalCity.Location = new System.Drawing.Point(94, 362);
            this.txtPhysicalCity.MaxLength = 225;
            this.txtPhysicalCity.Multiline = true;
            this.txtPhysicalCity.Name = "txtPhysicalCity";
            this.txtPhysicalCity.Size = new System.Drawing.Size(113, 20);
            this.txtPhysicalCity.TabIndex = 32;
            this.txtPhysicalCity.WaterMark = "Enter  City";
            this.txtPhysicalCity.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtPhysicalCity.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhysicalCity.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtPhysicalCity.TextChanged += new System.EventHandler(this.txtPhysicalCity_TextChanged);
            // 
            // txtPhysicalAddress2
            // 
            this.txtPhysicalAddress2.Location = new System.Drawing.Point(364, 331);
            this.txtPhysicalAddress2.MaxLength = 225;
            this.txtPhysicalAddress2.Multiline = true;
            this.txtPhysicalAddress2.Name = "txtPhysicalAddress2";
            this.txtPhysicalAddress2.Size = new System.Drawing.Size(173, 20);
            this.txtPhysicalAddress2.TabIndex = 30;
            this.txtPhysicalAddress2.WaterMark = "Enter Address 2";
            this.txtPhysicalAddress2.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtPhysicalAddress2.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhysicalAddress2.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtPhysicalAddress2.TextChanged += new System.EventHandler(this.txtPhysicalAddress2_TextChanged);
            // 
            // txtPhysicalAddress1
            // 
            this.txtPhysicalAddress1.Location = new System.Drawing.Point(94, 331);
            this.txtPhysicalAddress1.MaxLength = 225;
            this.txtPhysicalAddress1.Multiline = true;
            this.txtPhysicalAddress1.Name = "txtPhysicalAddress1";
            this.txtPhysicalAddress1.Size = new System.Drawing.Size(173, 20);
            this.txtPhysicalAddress1.TabIndex = 28;
            this.txtPhysicalAddress1.WaterMark = "Enter Address 1";
            this.txtPhysicalAddress1.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtPhysicalAddress1.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhysicalAddress1.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtPhysicalAddress1.TextChanged += new System.EventHandler(this.txtPhysicalAddress1_TextChanged);
            // 
            // txtClientCode
            // 
            this.txtClientCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtClientCode.ForeColor = System.Drawing.Color.Maroon;
            this.txtClientCode.Location = new System.Drawing.Point(98, 37);
            this.txtClientCode.MaxLength = 35;
            this.txtClientCode.Name = "txtClientCode";
            this.txtClientCode.Size = new System.Drawing.Size(198, 21);
            this.txtClientCode.TabIndex = 3;
            this.txtClientCode.WaterMark = "Enter Client Code";
            this.txtClientCode.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtClientCode.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtClientCode.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtClientCode.TextChanged += new System.EventHandler(this.txtClientCode_TextChanged);
            this.txtClientCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtClientCode_KeyPress);
            // 
            // txtClientName
            // 
            this.txtClientName.Location = new System.Drawing.Point(98, 71);
            this.txtClientName.MaxLength = 200;
            this.txtClientName.Name = "txtClientName";
            this.txtClientName.Size = new System.Drawing.Size(283, 20);
            this.txtClientName.TabIndex = 6;
            this.txtClientName.WaterMark = "Enter Client Name";
            this.txtClientName.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtClientName.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtClientName.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtClientName.TextChanged += new System.EventHandler(this.txtClientName_TextChanged);
            // 
            // chkDepartmentAddress
            // 
            this.chkDepartmentAddress.AutoSize = true;
            this.chkDepartmentAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDepartmentAddress.Location = new System.Drawing.Point(397, 193);
            this.chkDepartmentAddress.Name = "chkDepartmentAddress";
            this.chkDepartmentAddress.Size = new System.Drawing.Size(140, 17);
            this.chkDepartmentAddress.TabIndex = 14;
            this.chkDepartmentAddress.Text = "Department Address";
            this.chkDepartmentAddress.UseVisualStyleBackColor = true;
            this.chkDepartmentAddress.CheckedChanged += new System.EventHandler(this.chkClient_CheckedChanged);
            // 
            // chkedittestcategory
            // 
            this.chkedittestcategory.AutoSize = true;
            this.chkedittestcategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkedittestcategory.Location = new System.Drawing.Point(561, 254);
            this.chkedittestcategory.Name = "chkedittestcategory";
            this.chkedittestcategory.Size = new System.Drawing.Size(217, 17);
            this.chkedittestcategory.TabIndex = 66;
            this.chkedittestcategory.Text = "Edit Test Category From Test Info";
            this.chkedittestcategory.UseVisualStyleBackColor = true;
            this.chkedittestcategory.Visible = false;
            // 
            // gboxBC
            // 
            this.gboxBC.Controls.Add(this.lblBCTestPrice);
            this.gboxBC.Controls.Add(this.txtBCTestPrice);
            this.gboxBC.Controls.Add(this.label2);
            this.gboxBC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gboxBC.Location = new System.Drawing.Point(561, 404);
            this.gboxBC.Name = "gboxBC";
            this.gboxBC.Size = new System.Drawing.Size(217, 46);
            this.gboxBC.TabIndex = 67;
            this.gboxBC.TabStop = false;
            this.gboxBC.Text = "BC";
            // 
            // lblBCTestPrice
            // 
            this.lblBCTestPrice.AutoSize = true;
            this.lblBCTestPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBCTestPrice.Location = new System.Drawing.Point(10, 22);
            this.lblBCTestPrice.Name = "lblBCTestPrice";
            this.lblBCTestPrice.Size = new System.Drawing.Size(72, 13);
            this.lblBCTestPrice.TabIndex = 0;
            this.lblBCTestPrice.Text = "BC Test Price";
            // 
            // txtBCTestPrice
            // 
            this.txtBCTestPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBCTestPrice.Location = new System.Drawing.Point(111, 18);
            this.txtBCTestPrice.MaxLength = 6;
            this.txtBCTestPrice.Multiline = true;
            this.txtBCTestPrice.Name = "txtBCTestPrice";
            this.txtBCTestPrice.Size = new System.Drawing.Size(82, 20);
            this.txtBCTestPrice.TabIndex = 2;
            this.txtBCTestPrice.Tag = "BC_Main";
            this.txtBCTestPrice.WaterMark = "Enter Price";
            this.txtBCTestPrice.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtBCTestPrice.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBCTestPrice.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtBCTestPrice.TextChanged += new System.EventHandler(this.txtBCTestPrice_TextChanged);
            this.txtBCTestPrice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBCTestPrice_KeyPress);
            this.txtBCTestPrice.Leave += new System.EventHandler(this.txtBCTestPrice_Leave);
            this.txtBCTestPrice.Validating += new System.ComponentModel.CancelEventHandler(this.txtBCTestPrice_Validating);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(89, 20);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(13, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "*";
            // 
            // btnBCItemNotFound
            // 
            this.btnBCItemNotFound.Location = new System.Drawing.Point(946, 246);
            this.btnBCItemNotFound.Name = "btnBCItemNotFound";
            this.btnBCItemNotFound.Size = new System.Drawing.Size(133, 23);
            this.btnBCItemNotFound.TabIndex = 68;
            this.btnBCItemNotFound.Text = "BC Item Not Found";
            this.btnBCItemNotFound.UseVisualStyleBackColor = true;
            this.btnBCItemNotFound.Click += new System.EventHandler(this.btnBCItemNotFound_Click);
            // 
            // gboxRecordKeeping
            // 
            this.gboxRecordKeeping.Controls.Add(this.label1);
            this.gboxRecordKeeping.Controls.Add(this.lblRecordKeepingTestPrice);
            this.gboxRecordKeeping.Controls.Add(this.txtRecordKeepingTestPrice);
            this.gboxRecordKeeping.Controls.Add(this.label3);
            this.gboxRecordKeeping.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gboxRecordKeeping.Location = new System.Drawing.Point(561, 464);
            this.gboxRecordKeeping.Name = "gboxRecordKeeping";
            this.gboxRecordKeeping.Size = new System.Drawing.Size(522, 46);
            this.gboxRecordKeeping.TabIndex = 69;
            this.gboxRecordKeeping.TabStop = false;
            this.gboxRecordKeeping.Text = "Record Keeping";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(129, 21);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "*";
            // 
            // lblRecordKeepingTestPrice
            // 
            this.lblRecordKeepingTestPrice.AutoSize = true;
            this.lblRecordKeepingTestPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecordKeepingTestPrice.Location = new System.Drawing.Point(10, 22);
            this.lblRecordKeepingTestPrice.Name = "lblRecordKeepingTestPrice";
            this.lblRecordKeepingTestPrice.Size = new System.Drawing.Size(111, 13);
            this.lblRecordKeepingTestPrice.TabIndex = 0;
            this.lblRecordKeepingTestPrice.Text = "Record Keeping Price";
            // 
            // txtRecordKeepingTestPrice
            // 
            this.txtRecordKeepingTestPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRecordKeepingTestPrice.Location = new System.Drawing.Point(145, 19);
            this.txtRecordKeepingTestPrice.MaxLength = 6;
            this.txtRecordKeepingTestPrice.Multiline = true;
            this.txtRecordKeepingTestPrice.Name = "txtRecordKeepingTestPrice";
            this.txtRecordKeepingTestPrice.Size = new System.Drawing.Size(82, 20);
            this.txtRecordKeepingTestPrice.TabIndex = 2;
            this.txtRecordKeepingTestPrice.Tag = "RC_Main";
            this.txtRecordKeepingTestPrice.WaterMark = "Enter Price";
            this.txtRecordKeepingTestPrice.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtRecordKeepingTestPrice.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRecordKeepingTestPrice.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtRecordKeepingTestPrice.TextChanged += new System.EventHandler(this.TxtRecordKeepingTestPrice_TextChanged);
            this.txtRecordKeepingTestPrice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtRecordKeepingTestPrice_KeyPress);
            this.txtRecordKeepingTestPrice.Leave += new System.EventHandler(this.TxtRecordKeepingTestPrice_Leave_1);
            this.txtRecordKeepingTestPrice.Validating += new System.ComponentModel.CancelEventHandler(this.TxtRecordKeepingTestPrice_Validating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(89, 20);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(13, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "*";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(13, 555);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 70;
            this.label5.Text = "Time Zone";
            // 
            // cmbTimeZone
            // 
            this.cmbTimeZone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTimeZone.FormattingEnabled = true;
            this.cmbTimeZone.Items.AddRange(new object[] {
            "(Select)"});
            this.cmbTimeZone.Location = new System.Drawing.Point(140, 587);
            this.cmbTimeZone.Name = "cmbTimeZone";
            this.cmbTimeZone.Size = new System.Drawing.Size(192, 21);
            this.cmbTimeZone.TabIndex = 72;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(13, 591);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 71;
            this.label6.Text = "Time Zone";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(794, 245);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(135, 23);
            this.button1.TabIndex = 73;
            this.button1.Text = "Integrations";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FrmClientDetails
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1102, 658);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cmbTimeZone);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.gboxRecordKeeping);
            this.Controls.Add(this.btnBCItemNotFound);
            this.Controls.Add(this.gboxBC);
            this.Controls.Add(this.chkedittestcategory);
            this.Controls.Add(this.chkDepartmentAddress);
            this.Controls.Add(this.cmbMailingState);
            this.Controls.Add(this.txtMailingZipCode);
            this.Controls.Add(this.txtPhysicalZipCode);
            this.Controls.Add(this.btnTestPanelNotFound);
            this.Controls.Add(this.btnDepartmentContact);
            this.Controls.Add(this.btnDepartmentDetails);
            this.Controls.Add(this.btnDepartmentNotFound);
            this.Controls.Add(this.tvDepartmentInfo);
            this.Controls.Add(this.lblDepartment);
            this.Controls.Add(this.gboxDNA);
            this.Controls.Add(this.gboxHair);
            this.Controls.Add(this.gboxUA);
            this.Controls.Add(this.cmbPhysicalState);
            this.Controls.Add(this.txtFax);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.lblZipCode);
            this.Controls.Add(this.txtLastName);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.txtFirstName);
            this.Controls.Add(this.txtMailingCity);
            this.Controls.Add(this.txtMailingAddress2);
            this.Controls.Add(this.txtMailingAddress1);
            this.Controls.Add(this.txtPhysicalCity);
            this.Controls.Add(this.txtPhysicalAddress2);
            this.Controls.Add(this.txtPhysicalAddress1);
            this.Controls.Add(this.cmbSalesRepresentative);
            this.Controls.Add(this.lblSalesInfo);
            this.Controls.Add(this.lblSalesRepresentive);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.lblFax);
            this.Controls.Add(this.lblPhone);
            this.Controls.Add(this.lblLastName);
            this.Controls.Add(this.lblMainContactInfo);
            this.Controls.Add(this.lblFirstName);
            this.Controls.Add(this.chkSameAsPhysical);
            this.Controls.Add(this.lblMailingState);
            this.Controls.Add(this.lblPhysicalZipCode);
            this.Controls.Add(this.lblMailingCity);
            this.Controls.Add(this.lblPhysicalState);
            this.Controls.Add(this.lblMailingAddress2);
            this.Controls.Add(this.lblPhysicalCity);
            this.Controls.Add(this.lblPhysicalAddress1);
            this.Controls.Add(this.lblMailingAddress1);
            this.Controls.Add(this.lblPhysicalAddress2);
            this.Controls.Add(this.lblMailingAddress);
            this.Controls.Add(this.lblPhysicalAddress);
            this.Controls.Add(this.lblClientCodeMan);
            this.Controls.Add(this.lblMROMan);
            this.Controls.Add(this.lblLabMan);
            this.Controls.Add(this.lblClientNameMan);
            this.Controls.Add(this.lblClientMandatory);
            this.Controls.Add(this.lblDepartmentNameValue);
            this.Controls.Add(this.lblDepartmentNameHeader);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.chkActive);
            this.Controls.Add(this.txtClientCode);
            this.Controls.Add(this.txtClientName);
            this.Controls.Add(this.lblPageHeader);
            this.Controls.Add(this.lblClientCode);
            this.Controls.Add(this.lblMRO);
            this.Controls.Add(this.lblLab);
            this.Controls.Add(this.cmbMRO);
            this.Controls.Add(this.cmbLab);
            this.Controls.Add(this.lblClientName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmClientDetails";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Client Setup";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmClientDetails_FormClosing);
            this.Load += new System.EventHandler(this.FrmClientDetails_Load);
            this.gboxUA.ResumeLayout(false);
            this.gboxUA.PerformLayout();
            this.gboxHair.ResumeLayout(false);
            this.gboxHair.PerformLayout();
            this.gboxDNA.ResumeLayout(false);
            this.gboxDNA.PerformLayout();
            this.gboxBC.ResumeLayout(false);
            this.gboxBC.PerformLayout();
            this.gboxRecordKeeping.ResumeLayout(false);
            this.gboxRecordKeeping.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblClientCode;
        private System.Windows.Forms.Label lblClientName;
        private System.Windows.Forms.Label lblPageHeader;
        private Controls.TextBoxes.SurTextBox txtClientName;
        private Controls.TextBoxes.SurTextBox txtClientCode;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblLab;
        private System.Windows.Forms.ComboBox cmbLab;
        private System.Windows.Forms.ComboBox cmbMRO;
        private System.Windows.Forms.Label lblMRO;
        private System.Windows.Forms.Label lblDepartmentNameValue;
        private System.Windows.Forms.Label lblDepartmentNameHeader;
        private System.Windows.Forms.ComboBox cmbUAMainTestPanel;
        private Controls.TextBoxes.SurTextBox txtUAMainTestPanelPrice;
        private System.Windows.Forms.Label lblUAPriceHeading;
        private System.Windows.Forms.ComboBox cmbHairMainTestPanel;
        private Controls.TextBoxes.SurTextBox txtHairMainTestPanelPrice;
        private System.Windows.Forms.Label lblHairPriceHeading;
        private System.Windows.Forms.Label lblHairTestPanelHeading;
        private Controls.TextBoxes.SurTextBox txtDNATestPrice;
        private System.Windows.Forms.Label lblDNATestPrice;
        private System.Windows.Forms.Label lblClientMandatory;
        private System.Windows.Forms.Label lblClientNameMan;
        private System.Windows.Forms.Label lblLabMan;
        private System.Windows.Forms.Label lblMROMan;
        private System.Windows.Forms.Label lblDNAPriceMan;
        private System.Windows.Forms.Label lblClientCodeMan;
        private System.Windows.Forms.ComboBox cmbPhysicalState;
        private System.Windows.Forms.MaskedTextBox txtFax;
        private System.Windows.Forms.MaskedTextBox txtPhone;
        private System.Windows.Forms.Label lblZipCode;
        private Controls.TextBoxes.SurTextBox txtLastName;
        private Controls.TextBoxes.SurTextBox txtEmail;
        private Controls.TextBoxes.SurTextBox txtFirstName;
        private Controls.TextBoxes.SurTextBox txtMailingCity;
        private Controls.TextBoxes.SurTextBox txtMailingAddress2;
        private Controls.TextBoxes.SurTextBox txtMailingAddress1;
        private Controls.TextBoxes.SurTextBox txtPhysicalCity;
        private Controls.TextBoxes.SurTextBox txtPhysicalAddress2;
        private Controls.TextBoxes.SurTextBox txtPhysicalAddress1;
        private System.Windows.Forms.ComboBox cmbSalesRepresentative;
        private System.Windows.Forms.Label lblSalesInfo;
        private System.Windows.Forms.Label lblSalesRepresentive;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblFax;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.Label lblMainContactInfo;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.CheckBox chkSameAsPhysical;
        private System.Windows.Forms.Label lblMailingState;
        private System.Windows.Forms.Label lblPhysicalZipCode;
        private System.Windows.Forms.Label lblMailingCity;
        private System.Windows.Forms.Label lblPhysicalState;
        private System.Windows.Forms.Label lblMailingAddress2;
        private System.Windows.Forms.Label lblPhysicalCity;
        private System.Windows.Forms.Label lblPhysicalAddress1;
        private System.Windows.Forms.Label lblMailingAddress1;
        private System.Windows.Forms.Label lblPhysicalAddress2;
        private System.Windows.Forms.Label lblMailingAddress;
        private System.Windows.Forms.Label lblPhysicalAddress;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblUATestPanelHeading;
        private System.Windows.Forms.GroupBox gboxUA;
        private System.Windows.Forms.GroupBox gboxHair;
        private System.Windows.Forms.GroupBox gboxDNA;
        private System.Windows.Forms.Button btnTestPanelNotFound;
        private System.Windows.Forms.Button btnDepartmentDetails;
        private System.Windows.Forms.Button btnDepartmentNotFound;
        private System.Windows.Forms.TreeView tvDepartmentInfo;
        private System.Windows.Forms.Label lblDepartment;
        private System.Windows.Forms.Button btnDepartmentContact;
        private Controls.TextBoxes.SurTextBox txtPhysicalZipCode;
        private Controls.TextBoxes.SurTextBox txtMailingZipCode;
        private System.Windows.Forms.ComboBox cmbMailingState;
        private System.Windows.Forms.CheckBox chkDepartmentAddress;
        private System.Windows.Forms.CheckBox chkedittestcategory;
        private System.Windows.Forms.GroupBox gboxBC;
        private System.Windows.Forms.Label lblBCTestPrice;
        private Controls.TextBoxes.SurTextBox txtBCTestPrice;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnBCItemNotFound;
        private System.Windows.Forms.GroupBox gboxRecordKeeping;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblRecordKeepingTestPrice;
        private Controls.TextBoxes.SurTextBox txtRecordKeepingTestPrice;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbTimeZone;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
    }
}