namespace SurPath
{
    partial class FrmZipCodeDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmZipCodeDetails));
            this.lblZipCode = new System.Windows.Forms.Label();
            this.lblState = new System.Windows.Forms.Label();
            this.cmbState = new System.Windows.Forms.ComboBox();
            this.cmbZipcode = new System.Windows.Forms.ComboBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.lblZipCodeList = new System.Windows.Forms.Label();
            this.btnAvailability = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblAvailability = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            this.dgvVendorInfo = new System.Windows.Forms.DataGridView();
            this.VendorId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VendorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VendorTypes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VendorTypeId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VendorMainContact = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VendorPhone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VendorFax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VendorEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.City = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.State = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VendorStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InActiveDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InActiveReason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkZipCodeList = new SurPath.Controls.CheckedListBoxes.SurCheckedListBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVendorInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // lblZipCode
            // 
            this.lblZipCode.AutoSize = true;
            this.lblZipCode.Location = new System.Drawing.Point(23, 15);
            this.lblZipCode.Name = "lblZipCode";
            this.lblZipCode.Size = new System.Drawing.Size(92, 13);
            this.lblZipCode.TabIndex = 2;
            this.lblZipCode.Text = "&Donor\'s  Zip Code";
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Location = new System.Drawing.Point(23, 15);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(32, 13);
            this.lblState.TabIndex = 0;
            this.lblState.Text = "&State";
            this.lblState.Visible = false;
            // 
            // cmbState
            // 
            this.cmbState.DropDownHeight = 90;
            this.cmbState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbState.FormattingEnabled = true;
            this.cmbState.IntegralHeight = false;
            this.cmbState.Location = new System.Drawing.Point(126, 11);
            this.cmbState.Name = "cmbState";
            this.cmbState.Size = new System.Drawing.Size(159, 21);
            this.cmbState.TabIndex = 1;
            this.cmbState.Visible = false;
            this.cmbState.SelectedIndexChanged += new System.EventHandler(this.cmbState_SelectedIndexChanged);
            // 
            // cmbZipcode
            // 
            this.cmbZipcode.DropDownHeight = 400;
            this.cmbZipcode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbZipcode.FormattingEnabled = true;
            this.cmbZipcode.IntegralHeight = false;
            this.cmbZipcode.Location = new System.Drawing.Point(125, 11);
            this.cmbZipcode.Name = "cmbZipcode";
            this.cmbZipcode.Size = new System.Drawing.Size(172, 21);
            this.cmbZipcode.TabIndex = 3;
            this.cmbZipcode.SelectedIndexChanged += new System.EventHandler(this.cmbZipcode_SelectedIndexChanged);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnClose.AutoSize = true;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(529, 486);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOk.AutoSize = true;
            this.btnOk.Location = new System.Drawing.Point(447, 486);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "&Apply";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // lblZipCodeList
            // 
            this.lblZipCodeList.AutoSize = true;
            this.lblZipCodeList.Location = new System.Drawing.Point(23, 42);
            this.lblZipCodeList.Name = "lblZipCodeList";
            this.lblZipCodeList.Size = new System.Drawing.Size(69, 13);
            this.lblZipCodeList.TabIndex = 7;
            this.lblZipCodeList.Text = "Zip Code &List";
            // 
            // btnAvailability
            // 
            this.btnAvailability.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAvailability.AutoSize = true;
            this.btnAvailability.Location = new System.Drawing.Point(125, 292);
            this.btnAvailability.Name = "btnAvailability";
            this.btnAvailability.Size = new System.Drawing.Size(100, 23);
            this.btnAvailability.TabIndex = 5;
            this.btnAvailability.Text = "&Check Availability";
            this.btnAvailability.UseVisualStyleBackColor = true;
            this.btnAvailability.Click += new System.EventHandler(this.btnAvailability_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(122, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(179, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "(Select 4 nearest zip code maximum)";
            // 
            // lblAvailability
            // 
            this.lblAvailability.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblAvailability.AutoSize = true;
            this.lblAvailability.Location = new System.Drawing.Point(280, 297);
            this.lblAvailability.Name = "lblAvailability";
            this.lblAvailability.Size = new System.Drawing.Size(65, 13);
            this.lblAvailability.TabIndex = 7;
            this.lblAvailability.Text = "Availability  :";
            // 
            // lblCount
            // 
            this.lblCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCount.AutoSize = true;
            this.lblCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCount.Location = new System.Drawing.Point(340, 297);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(47, 13);
            this.lblCount.TabIndex = 7;
            this.lblCount.Text = "          ";
            // 
            // dgvVendorInfo
            // 
            this.dgvVendorInfo.AllowUserToAddRows = false;
            this.dgvVendorInfo.AllowUserToDeleteRows = false;
            this.dgvVendorInfo.AllowUserToOrderColumns = true;
            this.dgvVendorInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvVendorInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVendorInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.VendorId,
            this.VendorName,
            this.VendorTypes,
            this.VendorTypeId,
            this.VendorMainContact,
            this.VendorPhone,
            this.VendorFax,
            this.VendorEmail,
            this.City,
            this.State,
            this.VendorStatus,
            this.Status,
            this.InActiveDate,
            this.InActiveReason});
            this.dgvVendorInfo.Location = new System.Drawing.Point(26, 321);
            this.dgvVendorInfo.MultiSelect = false;
            this.dgvVendorInfo.Name = "dgvVendorInfo";
            this.dgvVendorInfo.ReadOnly = true;
            this.dgvVendorInfo.RowHeadersWidth = 25;
            this.dgvVendorInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvVendorInfo.Size = new System.Drawing.Size(1013, 159);
            this.dgvVendorInfo.TabIndex = 18;
            // 
            // VendorId
            // 
            this.VendorId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.VendorId.DataPropertyName = "VendorId";
            this.VendorId.HeaderText = "VendorId";
            this.VendorId.Name = "VendorId";
            this.VendorId.ReadOnly = true;
            this.VendorId.Visible = false;
            // 
            // VendorName
            // 
            this.VendorName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.VendorName.DataPropertyName = "VendorName";
            this.VendorName.HeaderText = "Vendor Name";
            this.VendorName.Name = "VendorName";
            this.VendorName.ReadOnly = true;
            // 
            // VendorTypes
            // 
            this.VendorTypes.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.VendorTypes.HeaderText = "Vendor Types";
            this.VendorTypes.Name = "VendorTypes";
            this.VendorTypes.ReadOnly = true;
            // 
            // VendorTypeId
            // 
            this.VendorTypeId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.VendorTypeId.DataPropertyName = "VendorTypeId";
            this.VendorTypeId.HeaderText = "Vendor TypeId";
            this.VendorTypeId.Name = "VendorTypeId";
            this.VendorTypeId.ReadOnly = true;
            this.VendorTypeId.Visible = false;
            // 
            // VendorMainContact
            // 
            this.VendorMainContact.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.VendorMainContact.DataPropertyName = "VendorMainContact";
            this.VendorMainContact.HeaderText = "Vendor Main Contact";
            this.VendorMainContact.Name = "VendorMainContact";
            this.VendorMainContact.ReadOnly = true;
            // 
            // VendorPhone
            // 
            this.VendorPhone.DataPropertyName = "VendorPhone";
            this.VendorPhone.HeaderText = "Phone";
            this.VendorPhone.Name = "VendorPhone";
            this.VendorPhone.ReadOnly = true;
            // 
            // VendorFax
            // 
            this.VendorFax.DataPropertyName = "VendorFax";
            this.VendorFax.HeaderText = "Fax";
            this.VendorFax.Name = "VendorFax";
            this.VendorFax.ReadOnly = true;
            // 
            // VendorEmail
            // 
            this.VendorEmail.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.VendorEmail.DataPropertyName = "VendorEmail";
            this.VendorEmail.HeaderText = "Email";
            this.VendorEmail.Name = "VendorEmail";
            this.VendorEmail.ReadOnly = true;
            // 
            // City
            // 
            this.City.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.City.DataPropertyName = "VendorCity";
            this.City.HeaderText = "City";
            this.City.Name = "City";
            this.City.ReadOnly = true;
            // 
            // State
            // 
            this.State.DataPropertyName = "VendorState";
            this.State.HeaderText = "State";
            this.State.Name = "State";
            this.State.ReadOnly = true;
            // 
            // VendorStatus
            // 
            this.VendorStatus.DataPropertyName = "VendorStatus";
            this.VendorStatus.HeaderText = "Vendor Status";
            this.VendorStatus.Name = "VendorStatus";
            this.VendorStatus.ReadOnly = true;
            this.VendorStatus.Visible = false;
            // 
            // Status
            // 
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            // 
            // InActiveDate
            // 
            this.InActiveDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.InActiveDate.DataPropertyName = "InActiveDate";
            this.InActiveDate.HeaderText = "InActive Date";
            this.InActiveDate.Name = "InActiveDate";
            this.InActiveDate.ReadOnly = true;
            this.InActiveDate.Visible = false;
            // 
            // InActiveReason
            // 
            this.InActiveReason.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.InActiveReason.DataPropertyName = "InActiveReason";
            this.InActiveReason.HeaderText = "InActive Reason";
            this.InActiveReason.Name = "InActiveReason";
            this.InActiveReason.ReadOnly = true;
            this.InActiveReason.Visible = false;
            // 
            // chkZipCodeList
            // 
            this.chkZipCodeList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkZipCodeList.CheckOnClick = true;
            this.chkZipCodeList.DataSource = null;
            this.chkZipCodeList.FormattingEnabled = true;
            this.chkZipCodeList.Location = new System.Drawing.Point(125, 59);
            this.chkZipCodeList.MultiColumn = true;
            this.chkZipCodeList.Name = "chkZipCodeList";
            this.chkZipCodeList.ScrollAlwaysVisible = true;
            this.chkZipCodeList.Size = new System.Drawing.Size(869, 229);
            this.chkZipCodeList.TabIndex = 17;
            this.chkZipCodeList.ValueList = ((System.Collections.Generic.List<int>)(resources.GetObject("chkZipCodeList.ValueList")));
            this.chkZipCodeList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chkZipCodeList_ItemCheck);
            // 
            // FrmZipCodeDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1051, 519);
            this.Controls.Add(this.dgvVendorInfo);
            this.Controls.Add(this.chkZipCodeList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.lblAvailability);
            this.Controls.Add(this.lblZipCodeList);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnAvailability);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.cmbZipcode);
            this.Controls.Add(this.cmbState);
            this.Controls.Add(this.lblZipCode);
            this.Controls.Add(this.lblState);
            this.Name = "FrmZipCodeDetails";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Zip Code Details";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmZipCodeDetails_FormClosed);
            this.Load += new System.EventHandler(this.FrmZipCodeDetails_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVendorInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblZipCode;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.ComboBox cmbState;
        private System.Windows.Forms.ComboBox cmbZipcode;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblZipCodeList;
        private Controls.CheckedListBoxes.SurCheckedListBox chkZipCodeList;
        private System.Windows.Forms.Button btnAvailability;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblAvailability;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.DataGridView dgvVendorInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn VendorId;
        private System.Windows.Forms.DataGridViewTextBoxColumn VendorName;
        private System.Windows.Forms.DataGridViewTextBoxColumn VendorTypes;
        private System.Windows.Forms.DataGridViewTextBoxColumn VendorTypeId;
        private System.Windows.Forms.DataGridViewTextBoxColumn VendorMainContact;
        private System.Windows.Forms.DataGridViewTextBoxColumn VendorPhone;
        private System.Windows.Forms.DataGridViewTextBoxColumn VendorFax;
        private System.Windows.Forms.DataGridViewTextBoxColumn VendorEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn City;
        private System.Windows.Forms.DataGridViewTextBoxColumn State;
        private System.Windows.Forms.DataGridViewTextBoxColumn VendorStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn InActiveDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn InActiveReason;
    }
}