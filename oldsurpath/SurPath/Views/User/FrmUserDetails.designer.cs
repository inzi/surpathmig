namespace SurPath
{
    partial class FrmUserDetails
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("View Result On Website");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Ability To View Unmasked SSN#");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Donar Tab", new System.Windows.Forms.TreeNode[] {
            treeNode2});
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("View Tab Information");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Accounting Tab", new System.Windows.Forms.TreeNode[] {
            treeNode4});
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("View Test Performed");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("View Issues");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("View Accounting");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("View all sales representatives  performance and commission");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("View Commissions", new System.Windows.Forms.TreeNode[] {
            treeNode9});
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Dashboard", new System.Windows.Forms.TreeNode[] {
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode10});
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Can export data from system");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Search", new System.Windows.Forms.TreeNode[] {
            treeNode12});
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Can add client information");
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("Can edit client information");
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("Can add program information");
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("Can edit program information");
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("Can add test panel configuration");
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("Can edit test panel configuration");
            System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("Can add drug name");
            System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode("Can edit drug name");
            System.Windows.Forms.TreeNode treeNode22 = new System.Windows.Forms.TreeNode("Client Setup", new System.Windows.Forms.TreeNode[] {
            treeNode14,
            treeNode15,
            treeNode16,
            treeNode17,
            treeNode18,
            treeNode19,
            treeNode20,
            treeNode21});
            System.Windows.Forms.TreeNode treeNode23 = new System.Windows.Forms.TreeNode("Can add vendor information");
            System.Windows.Forms.TreeNode treeNode24 = new System.Windows.Forms.TreeNode("Can edit vendor information");
            System.Windows.Forms.TreeNode treeNode25 = new System.Windows.Forms.TreeNode("Vendor Setup", new System.Windows.Forms.TreeNode[] {
            treeNode23,
            treeNode24});
            System.Windows.Forms.TreeNode treeNode26 = new System.Windows.Forms.TreeNode("Ability to right click to edit programs and service information");
            System.Windows.Forms.TreeNode treeNode27 = new System.Windows.Forms.TreeNode("Ability to add/edit users");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmUserDetails));
            this.tvClientRule = new System.Windows.Forms.TreeView();
            this.tvAuthorizationRule = new System.Windows.Forms.TreeView();
            this.lblAuthorization = new System.Windows.Forms.Label();
            this.lblClient = new System.Windows.Forms.Label();
            this.btnResetPassword = new System.Windows.Forms.Button();
            this.btnAvailability = new System.Windows.Forms.Button();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblFax = new System.Windows.Forms.Label();
            this.lblLastName = new System.Windows.Forms.Label();
            this.lblPhone = new System.Windows.Forms.Label();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.lblUserType = new System.Windows.Forms.Label();
            this.rbtnSurScan = new System.Windows.Forms.RadioButton();
            this.rbtnDonor = new System.Windows.Forms.RadioButton();
            this.rbtnClient = new System.Windows.Forms.RadioButton();
            this.rbtnVendor = new System.Windows.Forms.RadioButton();
            this.cmbUserType = new System.Windows.Forms.ComboBox();
            this.lblPageHeader = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.MaskedTextBox();
            this.txtFax = new System.Windows.Forms.MaskedTextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.chkShowPassword = new System.Windows.Forms.CheckBox();
            this.lblFirstMan = new System.Windows.Forms.Label();
            this.lblUserTypeMan = new System.Windows.Forms.Label();
            this.lblLastMan = new System.Windows.Forms.Label();
            this.lblUserMan = new System.Windows.Forms.Label();
            this.lblPasswordMan = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblEmailMan = new System.Windows.Forms.Label();
            this.lblUserTypeStar = new System.Windows.Forms.Label();
            this.lblUserTypeDonor = new System.Windows.Forms.Label();
            this.lblDepartmentName = new System.Windows.Forms.Label();
            this.btnClientCollapseAll = new System.Windows.Forms.Button();
            this.btnClientExpandAll = new System.Windows.Forms.Button();
            this.btnAuthCollapseAll = new System.Windows.Forms.Button();
            this.btnAuthExpandAll = new System.Windows.Forms.Button();
            this.rbtnAttorney = new System.Windows.Forms.RadioButton();
            this.rbtnCourt = new System.Windows.Forms.RadioButton();
            this.rbtnJudge = new System.Windows.Forms.RadioButton();
            this.lblUserTypeAttorney = new System.Windows.Forms.Label();
            this.lblUserTypeCourt = new System.Windows.Forms.Label();
            this.lblUserTypeJudge = new System.Windows.Forms.Label();
            this.txtEmail = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtPassword = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtUsername = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtLastName = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtFirstName = new SurPath.Controls.TextBoxes.SurTextBox();
            this.listUserActivity = new System.Windows.Forms.ListView();
            this.clmnCategory = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmnDT = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmNote = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tvClientRule
            // 
            this.tvClientRule.CheckBoxes = true;
            this.tvClientRule.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tvClientRule.ItemHeight = 16;
            this.tvClientRule.Location = new System.Drawing.Point(13, 257);
            this.tvClientRule.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tvClientRule.Name = "tvClientRule";
            this.tvClientRule.Size = new System.Drawing.Size(432, 251);
            this.tvClientRule.TabIndex = 38;
            this.tvClientRule.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvClientRule_AfterCheck);
            this.tvClientRule.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvClientRule_AfterSelect);
            this.tvClientRule.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvClientRule_NodeMouseClick);
            // 
            // tvAuthorizationRule
            // 
            this.tvAuthorizationRule.CheckBoxes = true;
            this.tvAuthorizationRule.ItemHeight = 16;
            this.tvAuthorizationRule.Location = new System.Drawing.Point(473, 257);
            this.tvAuthorizationRule.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tvAuthorizationRule.Name = "tvAuthorizationRule";
            treeNode1.Name = "Node0";
            treeNode1.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode1.Text = "View Result On Website";
            treeNode2.Name = "Node2";
            treeNode2.Text = "Ability To View Unmasked SSN#";
            treeNode3.Name = "Node1";
            treeNode3.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode3.Text = "Donar Tab";
            treeNode4.Name = "Node4";
            treeNode4.Text = "View Tab Information";
            treeNode5.Name = "Node3";
            treeNode5.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode5.Text = "Accounting Tab";
            treeNode6.Name = "Node6";
            treeNode6.Text = "View Test Performed";
            treeNode7.Name = "Node7";
            treeNode7.Text = "View Issues";
            treeNode8.Name = "Node8";
            treeNode8.Text = "View Accounting";
            treeNode9.Name = "Node10";
            treeNode9.Text = "View all sales representatives  performance and commission";
            treeNode10.Name = "Node9";
            treeNode10.Text = "View Commissions";
            treeNode11.Name = "Node5";
            treeNode11.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode11.Text = "Dashboard";
            treeNode12.Name = "Node12";
            treeNode12.Text = "Can export data from system";
            treeNode13.Name = "Node11";
            treeNode13.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode13.Text = "Search";
            treeNode14.Name = "Node14";
            treeNode14.Text = "Can add client information";
            treeNode15.Name = "Node15";
            treeNode15.Text = "Can edit client information";
            treeNode16.Name = "Node16";
            treeNode16.Text = "Can add program information";
            treeNode17.Name = "Node17";
            treeNode17.Text = "Can edit program information";
            treeNode18.Name = "Node18";
            treeNode18.Text = "Can add test panel configuration";
            treeNode19.Name = "Node19";
            treeNode19.Text = "Can edit test panel configuration";
            treeNode20.Name = "Node20";
            treeNode20.Text = "Can add drug name";
            treeNode21.Name = "Node21";
            treeNode21.Text = "Can edit drug name";
            treeNode22.Name = "Node13";
            treeNode22.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode22.Text = "Client Setup";
            treeNode23.Name = "Node23";
            treeNode23.Text = "Can add vendor information";
            treeNode24.Name = "Node24";
            treeNode24.Text = "Can edit vendor information";
            treeNode25.Name = "Node22";
            treeNode25.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode25.Text = "Vendor Setup";
            treeNode26.Name = "Node25";
            treeNode26.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode26.Text = "Ability to right click to edit programs and service information";
            treeNode27.Name = "Node26";
            treeNode27.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode27.Text = "Ability to add/edit users";
            this.tvAuthorizationRule.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode3,
            treeNode5,
            treeNode11,
            treeNode13,
            treeNode22,
            treeNode25,
            treeNode26,
            treeNode27});
            this.tvAuthorizationRule.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tvAuthorizationRule.Size = new System.Drawing.Size(506, 251);
            this.tvAuthorizationRule.TabIndex = 40;
            this.tvAuthorizationRule.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvAuthorizationRule_AfterCheck);
            // 
            // lblAuthorization
            // 
            this.lblAuthorization.AutoSize = true;
            this.lblAuthorization.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuthorization.Location = new System.Drawing.Point(473, 233);
            this.lblAuthorization.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAuthorization.Name = "lblAuthorization";
            this.lblAuthorization.Size = new System.Drawing.Size(163, 13);
            this.lblAuthorization.TabIndex = 39;
            this.lblAuthorization.Text = "Authorization Rules / Roles";
            // 
            // lblClient
            // 
            this.lblClient.AutoSize = true;
            this.lblClient.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClient.Location = new System.Drawing.Point(13, 233);
            this.lblClient.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblClient.Name = "lblClient";
            this.lblClient.Size = new System.Drawing.Size(168, 13);
            this.lblClient.TabIndex = 37;
            this.lblClient.Text = "Client Department / Program";
            // 
            // btnResetPassword
            // 
            this.btnResetPassword.Location = new System.Drawing.Point(882, 79);
            this.btnResetPassword.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnResetPassword.Name = "btnResetPassword";
            this.btnResetPassword.Size = new System.Drawing.Size(97, 23);
            this.btnResetPassword.TabIndex = 35;
            this.btnResetPassword.Text = "Reset Password";
            this.btnResetPassword.UseVisualStyleBackColor = true;
            this.btnResetPassword.Click += new System.EventHandler(this.btnResetPassword_Click);
            // 
            // btnAvailability
            // 
            this.btnAvailability.Location = new System.Drawing.Point(882, 42);
            this.btnAvailability.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnAvailability.Name = "btnAvailability";
            this.btnAvailability.Size = new System.Drawing.Size(97, 23);
            this.btnAvailability.TabIndex = 31;
            this.btnAvailability.Text = "Availability";
            this.btnAvailability.UseVisualStyleBackColor = true;
            this.btnAvailability.Click += new System.EventHandler(this.btnAvailablity_Click);
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Checked = true;
            this.chkActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkActive.Location = new System.Drawing.Point(311, 121);
            this.chkActive.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(56, 17);
            this.chkActive.TabIndex = 14;
            this.chkActive.Text = "Active";
            this.chkActive.UseVisualStyleBackColor = true;
            this.chkActive.CheckedChanged += new System.EventHandler(this.chkActive_CheckedChanged);
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassword.Location = new System.Drawing.Point(596, 84);
            this.lblPassword.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 32;
            this.lblPassword.Text = "Password";
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserName.Location = new System.Drawing.Point(596, 48);
            this.lblUserName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(60, 13);
            this.lblUserName.TabIndex = 28;
            this.lblUserName.Text = "User Name";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(13, 124);
            this.lblEmail.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(32, 13);
            this.lblEmail.TabIndex = 11;
            this.lblEmail.Text = "Email";
            // 
            // lblFax
            // 
            this.lblFax.AutoSize = true;
            this.lblFax.Location = new System.Drawing.Point(311, 84);
            this.lblFax.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFax.Name = "lblFax";
            this.lblFax.Size = new System.Drawing.Size(61, 13);
            this.lblFax.TabIndex = 9;
            this.lblFax.Text = "Fax / Other";
            // 
            // lblLastName
            // 
            this.lblLastName.AutoSize = true;
            this.lblLastName.Location = new System.Drawing.Point(311, 48);
            this.lblLastName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(58, 13);
            this.lblLastName.TabIndex = 4;
            this.lblLastName.Text = "Last Name";
            // 
            // lblPhone
            // 
            this.lblPhone.AutoSize = true;
            this.lblPhone.Location = new System.Drawing.Point(13, 84);
            this.lblPhone.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(38, 13);
            this.lblPhone.TabIndex = 7;
            this.lblPhone.Text = "Phone";
            // 
            // lblFirstName
            // 
            this.lblFirstName.AutoSize = true;
            this.lblFirstName.Location = new System.Drawing.Point(13, 48);
            this.lblFirstName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(57, 13);
            this.lblFirstName.TabIndex = 1;
            this.lblFirstName.Text = "First Name";
            // 
            // lblUserType
            // 
            this.lblUserType.AutoSize = true;
            this.lblUserType.Location = new System.Drawing.Point(13, 157);
            this.lblUserType.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblUserType.Name = "lblUserType";
            this.lblUserType.Size = new System.Drawing.Size(56, 13);
            this.lblUserType.TabIndex = 15;
            this.lblUserType.Text = "User Type";
            // 
            // rbtnSurScan
            // 
            this.rbtnSurScan.AutoSize = true;
            this.rbtnSurScan.Checked = true;
            this.rbtnSurScan.Location = new System.Drawing.Point(106, 155);
            this.rbtnSurScan.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.rbtnSurScan.Name = "rbtnSurScan";
            this.rbtnSurScan.Size = new System.Drawing.Size(46, 17);
            this.rbtnSurScan.TabIndex = 17;
            this.rbtnSurScan.TabStop = true;
            this.rbtnSurScan.Text = "TPA";
            this.rbtnSurScan.UseVisualStyleBackColor = true;
            this.rbtnSurScan.CheckedChanged += new System.EventHandler(this.rbtnSurScan_CheckedChanged);
            // 
            // rbtnDonor
            // 
            this.rbtnDonor.AutoSize = true;
            this.rbtnDonor.Location = new System.Drawing.Point(160, 155);
            this.rbtnDonor.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.rbtnDonor.Name = "rbtnDonor";
            this.rbtnDonor.Size = new System.Drawing.Size(54, 17);
            this.rbtnDonor.TabIndex = 18;
            this.rbtnDonor.Text = "Donor";
            this.rbtnDonor.UseVisualStyleBackColor = true;
            this.rbtnDonor.CheckedChanged += new System.EventHandler(this.rbtnDonor_CheckedChanged);
            // 
            // rbtnClient
            // 
            this.rbtnClient.AutoSize = true;
            this.rbtnClient.Location = new System.Drawing.Point(222, 155);
            this.rbtnClient.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.rbtnClient.Name = "rbtnClient";
            this.rbtnClient.Size = new System.Drawing.Size(51, 17);
            this.rbtnClient.TabIndex = 19;
            this.rbtnClient.Text = "Client";
            this.rbtnClient.UseVisualStyleBackColor = true;
            this.rbtnClient.CheckedChanged += new System.EventHandler(this.rbtnClient_CheckedChanged);
            // 
            // rbtnVendor
            // 
            this.rbtnVendor.AutoSize = true;
            this.rbtnVendor.Location = new System.Drawing.Point(281, 155);
            this.rbtnVendor.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.rbtnVendor.Name = "rbtnVendor";
            this.rbtnVendor.Size = new System.Drawing.Size(59, 17);
            this.rbtnVendor.TabIndex = 20;
            this.rbtnVendor.Text = "Vendor";
            this.rbtnVendor.UseVisualStyleBackColor = true;
            this.rbtnVendor.CheckedChanged += new System.EventHandler(this.rbtnVendor_CheckedChanged);
            // 
            // cmbUserType
            // 
            this.cmbUserType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUserType.FormattingEnabled = true;
            this.cmbUserType.Items.AddRange(new object[] {
            "(Select)"});
            this.cmbUserType.Location = new System.Drawing.Point(120, 186);
            this.cmbUserType.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cmbUserType.Name = "cmbUserType";
            this.cmbUserType.Size = new System.Drawing.Size(233, 21);
            this.cmbUserType.TabIndex = 26;
            this.cmbUserType.TextChanged += new System.EventHandler(this.cmbUserType_TextChanged);
            // 
            // lblPageHeader
            // 
            this.lblPageHeader.AutoSize = true;
            this.lblPageHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageHeader.Location = new System.Drawing.Point(13, 7);
            this.lblPageHeader.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPageHeader.Name = "lblPageHeader";
            this.lblPageHeader.Size = new System.Drawing.Size(100, 20);
            this.lblPageHeader.TabIndex = 0;
            this.lblPageHeader.Text = "User Setup";
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(104, 79);
            this.txtPhone.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtPhone.Mask = "(999) 000-0000";
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(198, 20);
            this.txtPhone.TabIndex = 8;
            this.txtPhone.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtPhone_MouseClick);
            this.txtPhone.TextChanged += new System.EventHandler(this.txtPhone_TextChanged);
            // 
            // txtFax
            // 
            this.txtFax.Location = new System.Drawing.Point(384, 79);
            this.txtFax.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtFax.Mask = "(999) 000-0000";
            this.txtFax.Name = "txtFax";
            this.txtFax.Size = new System.Drawing.Size(198, 20);
            this.txtFax.TabIndex = 10;
            this.txtFax.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtFax_MouseClick);
            this.txtFax.TextChanged += new System.EventHandler(this.txtFax_TextChanged);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(494, 555);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnClose.Size = new System.Drawing.Size(74, 23);
            this.btnClose.TabIndex = 47;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(403, 555);
            this.btnOK.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(74, 23);
            this.btnOK.TabIndex = 46;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // chkShowPassword
            // 
            this.chkShowPassword.AutoSize = true;
            this.chkShowPassword.Location = new System.Drawing.Point(761, 103);
            this.chkShowPassword.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chkShowPassword.Name = "chkShowPassword";
            this.chkShowPassword.Size = new System.Drawing.Size(102, 17);
            this.chkShowPassword.TabIndex = 36;
            this.chkShowPassword.Text = "Show Password";
            this.chkShowPassword.UseVisualStyleBackColor = true;
            this.chkShowPassword.CheckedChanged += new System.EventHandler(this.chkShowPassword_CheckedChanged);
            // 
            // lblFirstMan
            // 
            this.lblFirstMan.AutoSize = true;
            this.lblFirstMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFirstMan.ForeColor = System.Drawing.Color.Red;
            this.lblFirstMan.Location = new System.Drawing.Point(66, 46);
            this.lblFirstMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFirstMan.Name = "lblFirstMan";
            this.lblFirstMan.Size = new System.Drawing.Size(13, 16);
            this.lblFirstMan.TabIndex = 2;
            this.lblFirstMan.Text = "*";
            // 
            // lblUserTypeMan
            // 
            this.lblUserTypeMan.AutoSize = true;
            this.lblUserTypeMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserTypeMan.ForeColor = System.Drawing.Color.Red;
            this.lblUserTypeMan.Location = new System.Drawing.Point(66, 155);
            this.lblUserTypeMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblUserTypeMan.Name = "lblUserTypeMan";
            this.lblUserTypeMan.Size = new System.Drawing.Size(13, 16);
            this.lblUserTypeMan.TabIndex = 16;
            this.lblUserTypeMan.Text = "*";
            // 
            // lblLastMan
            // 
            this.lblLastMan.AutoSize = true;
            this.lblLastMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastMan.ForeColor = System.Drawing.Color.Red;
            this.lblLastMan.Location = new System.Drawing.Point(367, 46);
            this.lblLastMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLastMan.Name = "lblLastMan";
            this.lblLastMan.Size = new System.Drawing.Size(13, 16);
            this.lblLastMan.TabIndex = 5;
            this.lblLastMan.Text = "*";
            // 
            // lblUserMan
            // 
            this.lblUserMan.AutoSize = true;
            this.lblUserMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserMan.ForeColor = System.Drawing.Color.Red;
            this.lblUserMan.Location = new System.Drawing.Point(654, 46);
            this.lblUserMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblUserMan.Name = "lblUserMan";
            this.lblUserMan.Size = new System.Drawing.Size(13, 16);
            this.lblUserMan.TabIndex = 29;
            this.lblUserMan.Text = "*";
            // 
            // lblPasswordMan
            // 
            this.lblPasswordMan.AutoSize = true;
            this.lblPasswordMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPasswordMan.ForeColor = System.Drawing.Color.Red;
            this.lblPasswordMan.Location = new System.Drawing.Point(647, 82);
            this.lblPasswordMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPasswordMan.Name = "lblPasswordMan";
            this.lblPasswordMan.Size = new System.Drawing.Size(13, 16);
            this.lblPasswordMan.TabIndex = 33;
            this.lblPasswordMan.Text = "*";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(872, 12);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 13);
            this.label6.TabIndex = 45;
            this.label6.Text = "*  Mandatory Fields";
            // 
            // lblEmailMan
            // 
            this.lblEmailMan.AutoSize = true;
            this.lblEmailMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmailMan.ForeColor = System.Drawing.Color.Red;
            this.lblEmailMan.Location = new System.Drawing.Point(44, 122);
            this.lblEmailMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblEmailMan.Name = "lblEmailMan";
            this.lblEmailMan.Size = new System.Drawing.Size(13, 16);
            this.lblEmailMan.TabIndex = 12;
            this.lblEmailMan.Text = "*";
            // 
            // lblUserTypeStar
            // 
            this.lblUserTypeStar.AutoSize = true;
            this.lblUserTypeStar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserTypeStar.ForeColor = System.Drawing.Color.Red;
            this.lblUserTypeStar.Location = new System.Drawing.Point(106, 188);
            this.lblUserTypeStar.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblUserTypeStar.Name = "lblUserTypeStar";
            this.lblUserTypeStar.Size = new System.Drawing.Size(13, 16);
            this.lblUserTypeStar.TabIndex = 25;
            this.lblUserTypeStar.Text = "*";
            // 
            // lblUserTypeDonor
            // 
            this.lblUserTypeDonor.AutoSize = true;
            this.lblUserTypeDonor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserTypeDonor.Location = new System.Drawing.Point(108, 189);
            this.lblUserTypeDonor.Name = "lblUserTypeDonor";
            this.lblUserTypeDonor.Size = new System.Drawing.Size(77, 13);
            this.lblUserTypeDonor.TabIndex = 26;
            this.lblUserTypeDonor.Text = "Donor Name";
            this.lblUserTypeDonor.Visible = false;
            // 
            // lblDepartmentName
            // 
            this.lblDepartmentName.AutoSize = true;
            this.lblDepartmentName.Location = new System.Drawing.Point(13, 190);
            this.lblDepartmentName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDepartmentName.Name = "lblDepartmentName";
            this.lblDepartmentName.Size = new System.Drawing.Size(93, 13);
            this.lblDepartmentName.TabIndex = 24;
            this.lblDepartmentName.Text = "Department Name";
            this.lblDepartmentName.Visible = false;
            // 
            // btnClientCollapseAll
            // 
            this.btnClientCollapseAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClientCollapseAll.Location = new System.Drawing.Point(379, 514);
            this.btnClientCollapseAll.Name = "btnClientCollapseAll";
            this.btnClientCollapseAll.Size = new System.Drawing.Size(29, 23);
            this.btnClientCollapseAll.TabIndex = 41;
            this.btnClientCollapseAll.Text = "+";
            this.btnClientCollapseAll.UseVisualStyleBackColor = true;
            this.btnClientCollapseAll.Click += new System.EventHandler(this.btnClientCollapseAll_Click);
            // 
            // btnClientExpandAll
            // 
            this.btnClientExpandAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClientExpandAll.Location = new System.Drawing.Point(416, 514);
            this.btnClientExpandAll.Name = "btnClientExpandAll";
            this.btnClientExpandAll.Size = new System.Drawing.Size(29, 23);
            this.btnClientExpandAll.TabIndex = 42;
            this.btnClientExpandAll.Text = "-";
            this.btnClientExpandAll.UseVisualStyleBackColor = true;
            this.btnClientExpandAll.Click += new System.EventHandler(this.btnClientExpandAll_Click);
            // 
            // btnAuthCollapseAll
            // 
            this.btnAuthCollapseAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAuthCollapseAll.Location = new System.Drawing.Point(913, 514);
            this.btnAuthCollapseAll.Name = "btnAuthCollapseAll";
            this.btnAuthCollapseAll.Size = new System.Drawing.Size(29, 23);
            this.btnAuthCollapseAll.TabIndex = 43;
            this.btnAuthCollapseAll.Text = "+";
            this.btnAuthCollapseAll.UseVisualStyleBackColor = true;
            this.btnAuthCollapseAll.Click += new System.EventHandler(this.btnAuthCollapseAll_Click);
            // 
            // btnAuthExpandAll
            // 
            this.btnAuthExpandAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAuthExpandAll.Location = new System.Drawing.Point(950, 514);
            this.btnAuthExpandAll.Name = "btnAuthExpandAll";
            this.btnAuthExpandAll.Size = new System.Drawing.Size(29, 23);
            this.btnAuthExpandAll.TabIndex = 44;
            this.btnAuthExpandAll.Text = "-";
            this.btnAuthExpandAll.UseVisualStyleBackColor = true;
            this.btnAuthExpandAll.Click += new System.EventHandler(this.btnAuthExpandAll_Click);
            // 
            // rbtnAttorney
            // 
            this.rbtnAttorney.AutoSize = true;
            this.rbtnAttorney.Location = new System.Drawing.Point(346, 155);
            this.rbtnAttorney.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.rbtnAttorney.Name = "rbtnAttorney";
            this.rbtnAttorney.Size = new System.Drawing.Size(64, 17);
            this.rbtnAttorney.TabIndex = 21;
            this.rbtnAttorney.Text = "Attorney";
            this.rbtnAttorney.UseVisualStyleBackColor = true;
            this.rbtnAttorney.CheckedChanged += new System.EventHandler(this.rbtnAttorney_CheckedChanged);
            // 
            // rbtnCourt
            // 
            this.rbtnCourt.AutoSize = true;
            this.rbtnCourt.Location = new System.Drawing.Point(416, 155);
            this.rbtnCourt.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.rbtnCourt.Name = "rbtnCourt";
            this.rbtnCourt.Size = new System.Drawing.Size(50, 17);
            this.rbtnCourt.TabIndex = 22;
            this.rbtnCourt.Text = "Court";
            this.rbtnCourt.UseVisualStyleBackColor = true;
            this.rbtnCourt.CheckedChanged += new System.EventHandler(this.rbtnCourt_CheckedChanged);
            // 
            // rbtnJudge
            // 
            this.rbtnJudge.AutoSize = true;
            this.rbtnJudge.Location = new System.Drawing.Point(477, 155);
            this.rbtnJudge.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.rbtnJudge.Name = "rbtnJudge";
            this.rbtnJudge.Size = new System.Drawing.Size(54, 17);
            this.rbtnJudge.TabIndex = 23;
            this.rbtnJudge.Text = "Judge";
            this.rbtnJudge.UseVisualStyleBackColor = true;
            this.rbtnJudge.CheckedChanged += new System.EventHandler(this.rbtnJudge_CheckedChanged);
            // 
            // lblUserTypeAttorney
            // 
            this.lblUserTypeAttorney.AutoSize = true;
            this.lblUserTypeAttorney.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserTypeAttorney.Location = new System.Drawing.Point(117, 188);
            this.lblUserTypeAttorney.Name = "lblUserTypeAttorney";
            this.lblUserTypeAttorney.Size = new System.Drawing.Size(90, 13);
            this.lblUserTypeAttorney.TabIndex = 26;
            this.lblUserTypeAttorney.Text = "Attorney Name";
            this.lblUserTypeAttorney.Visible = false;
            // 
            // lblUserTypeCourt
            // 
            this.lblUserTypeCourt.AutoSize = true;
            this.lblUserTypeCourt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserTypeCourt.Location = new System.Drawing.Point(124, 189);
            this.lblUserTypeCourt.Name = "lblUserTypeCourt";
            this.lblUserTypeCourt.Size = new System.Drawing.Size(73, 13);
            this.lblUserTypeCourt.TabIndex = 27;
            this.lblUserTypeCourt.Text = "Court Name";
            this.lblUserTypeCourt.Visible = false;
            // 
            // lblUserTypeJudge
            // 
            this.lblUserTypeJudge.AutoSize = true;
            this.lblUserTypeJudge.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserTypeJudge.Location = new System.Drawing.Point(117, 188);
            this.lblUserTypeJudge.Name = "lblUserTypeJudge";
            this.lblUserTypeJudge.Size = new System.Drawing.Size(77, 13);
            this.lblUserTypeJudge.TabIndex = 26;
            this.lblUserTypeJudge.Text = "Judge Name";
            this.lblUserTypeJudge.Visible = false;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(104, 120);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtEmail.MaxLength = 320;
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(198, 20);
            this.txtEmail.TabIndex = 13;
            this.txtEmail.WaterMark = "Enter Email";
            this.txtEmail.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtEmail.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtEmail.TextChanged += new System.EventHandler(this.txtEmail_TextChanged);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(669, 79);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtPassword.MaxLength = 30;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.ReadOnly = true;
            this.txtPassword.Size = new System.Drawing.Size(198, 20);
            this.txtPassword.TabIndex = 34;
            this.txtPassword.WaterMark = "Enter Password";
            this.txtPassword.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtPassword.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(669, 43);
            this.txtUsername.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtUsername.MaxLength = 320;
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(198, 20);
            this.txtUsername.TabIndex = 30;
            this.txtUsername.WaterMark = "Enter User Name";
            this.txtUsername.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtUsername.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsername.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtUsername.TextChanged += new System.EventHandler(this.txtUsername_TextChanged);
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(384, 43);
            this.txtLastName.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtLastName.MaxLength = 200;
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(198, 20);
            this.txtLastName.TabIndex = 6;
            this.txtLastName.WaterMark = "Enter Last Name";
            this.txtLastName.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtLastName.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLastName.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtLastName.TextChanged += new System.EventHandler(this.txtLastName_TextChanged);
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(104, 43);
            this.txtFirstName.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtFirstName.MaxLength = 200;
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(198, 20);
            this.txtFirstName.TabIndex = 3;
            this.txtFirstName.WaterMark = "Enter First  Name";
            this.txtFirstName.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtFirstName.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFirstName.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtFirstName.TextChanged += new System.EventHandler(this.txtFirstName_TextChanged);
            // 
            // listUserActivity
            // 
            this.listUserActivity.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmnCategory,
            this.clmnDT,
            this.clmNote});
            this.listUserActivity.HideSelection = false;
            this.listUserActivity.Location = new System.Drawing.Point(599, 141);
            this.listUserActivity.Name = "listUserActivity";
            this.listUserActivity.Size = new System.Drawing.Size(380, 89);
            this.listUserActivity.TabIndex = 48;
            this.listUserActivity.UseCompatibleStateImageBehavior = false;
            this.listUserActivity.View = System.Windows.Forms.View.Details;
            // 
            // clmnCategory
            // 
            this.clmnCategory.Text = "Category";
            this.clmnCategory.Width = 68;
            // 
            // clmnDT
            // 
            this.clmnDT.Text = "Date Time";
            this.clmnDT.Width = 87;
            // 
            // clmNote
            // 
            this.clmNote.Text = "Note";
            this.clmNote.Width = 167;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(596, 123);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 49;
            this.label1.Text = "Activity Log (Last 10)";
            // 
            // FrmUserDetails
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(995, 589);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listUserActivity);
            this.Controls.Add(this.btnAuthExpandAll);
            this.Controls.Add(this.btnAuthCollapseAll);
            this.Controls.Add(this.btnClientExpandAll);
            this.Controls.Add(this.btnClientCollapseAll);
            this.Controls.Add(this.lblEmailMan);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblUserTypeMan);
            this.Controls.Add(this.lblPasswordMan);
            this.Controls.Add(this.lblUserMan);
            this.Controls.Add(this.lblLastMan);
            this.Controls.Add(this.lblFirstMan);
            this.Controls.Add(this.chkShowPassword);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtFax);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.txtLastName);
            this.Controls.Add(this.txtFirstName);
            this.Controls.Add(this.lblPageHeader);
            this.Controls.Add(this.rbtnVendor);
            this.Controls.Add(this.rbtnJudge);
            this.Controls.Add(this.rbtnCourt);
            this.Controls.Add(this.rbtnAttorney);
            this.Controls.Add(this.rbtnClient);
            this.Controls.Add(this.rbtnDonor);
            this.Controls.Add(this.rbtnSurScan);
            this.Controls.Add(this.tvClientRule);
            this.Controls.Add(this.tvAuthorizationRule);
            this.Controls.Add(this.lblAuthorization);
            this.Controls.Add(this.lblClient);
            this.Controls.Add(this.btnResetPassword);
            this.Controls.Add(this.btnAvailability);
            this.Controls.Add(this.chkActive);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblUserType);
            this.Controls.Add(this.lblUserName);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.lblFax);
            this.Controls.Add(this.lblLastName);
            this.Controls.Add(this.lblPhone);
            this.Controls.Add(this.lblFirstName);
            this.Controls.Add(this.cmbUserType);
            this.Controls.Add(this.lblDepartmentName);
            this.Controls.Add(this.lblUserTypeStar);
            this.Controls.Add(this.lblUserTypeJudge);
            this.Controls.Add(this.lblUserTypeCourt);
            this.Controls.Add(this.lblUserTypeAttorney);
            this.Controls.Add(this.lblUserTypeDonor);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmUserDetails";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "User Setup";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmUserDetails_FormClosing);
            this.Load += new System.EventHandler(this.FrmUserDetails_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView tvClientRule;
        private System.Windows.Forms.TreeView tvAuthorizationRule;
        private System.Windows.Forms.Label lblAuthorization;
        private System.Windows.Forms.Label lblClient;
        private System.Windows.Forms.Button btnResetPassword;
        private System.Windows.Forms.Button btnAvailability;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblFax;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.Label lblUserType;
        private System.Windows.Forms.RadioButton rbtnSurScan;
        private System.Windows.Forms.RadioButton rbtnDonor;
        private System.Windows.Forms.RadioButton rbtnClient;
        private System.Windows.Forms.RadioButton rbtnVendor;
        private System.Windows.Forms.ComboBox cmbUserType;
        private System.Windows.Forms.Label lblPageHeader;
        private Controls.TextBoxes.SurTextBox txtFirstName;
        private Controls.TextBoxes.SurTextBox txtLastName;
        private Controls.TextBoxes.SurTextBox txtUsername;
        private Controls.TextBoxes.SurTextBox txtPassword;
        private Controls.TextBoxes.SurTextBox txtEmail;
        private System.Windows.Forms.MaskedTextBox txtPhone;
        private System.Windows.Forms.MaskedTextBox txtFax;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.CheckBox chkShowPassword;
        private System.Windows.Forms.Label lblFirstMan;
        private System.Windows.Forms.Label lblUserTypeMan;
        private System.Windows.Forms.Label lblLastMan;
        private System.Windows.Forms.Label lblUserMan;
        private System.Windows.Forms.Label lblPasswordMan;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblEmailMan;
        private System.Windows.Forms.Label lblUserTypeStar;
        private System.Windows.Forms.Label lblUserTypeDonor;
        private System.Windows.Forms.Label lblDepartmentName;
        private System.Windows.Forms.Button btnClientCollapseAll;
        private System.Windows.Forms.Button btnClientExpandAll;
        private System.Windows.Forms.Button btnAuthCollapseAll;
        private System.Windows.Forms.Button btnAuthExpandAll;
        private System.Windows.Forms.RadioButton rbtnAttorney;
        private System.Windows.Forms.RadioButton rbtnCourt;
        private System.Windows.Forms.RadioButton rbtnJudge;
        private System.Windows.Forms.Label lblUserTypeAttorney;
        private System.Windows.Forms.Label lblUserTypeCourt;
        private System.Windows.Forms.Label lblUserTypeJudge;
        private System.Windows.Forms.ListView listUserActivity;
        private System.Windows.Forms.ColumnHeader clmnCategory;
        private System.Windows.Forms.ColumnHeader clmnDT;
        private System.Windows.Forms.ColumnHeader clmNote;
        private System.Windows.Forms.Label label1;
    }
}