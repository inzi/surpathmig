namespace SurPath
{
    partial class FrmVendorInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmVendorInfo));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbNew = new System.Windows.Forms.ToolStripButton();
            this.tsbEdit = new System.Windows.Forms.ToolStripButton();
            this.tsbArchive = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lblVendorType = new System.Windows.Forms.ToolStripLabel();
            this.cmbVendorType = new System.Windows.Forms.ToolStripComboBox();
            this.lblSearch = new System.Windows.Forms.ToolStripLabel();
            this.txtSearchKeyword = new System.Windows.Forms.ToolStripTextBox();
            this.btnSearch = new System.Windows.Forms.ToolStripButton();
            this.btnShowAll = new System.Windows.Forms.ToolStripButton();
            this.LblVendorName = new System.Windows.Forms.Label();
            this.dgvVendorInfo = new System.Windows.Forms.DataGridView();
            this.chkIncludeInactive = new System.Windows.Forms.CheckBox();
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
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVendorInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNew,
            this.tsbEdit,
            this.tsbArchive,
            this.toolStripSeparator1,
            this.lblVendorType,
            this.cmbVendorType,
            this.lblSearch,
            this.txtSearchKeyword,
            this.btnSearch,
            this.btnShowAll});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1234, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbNew
            // 
            this.tsbNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNew.Image = ((System.Drawing.Image)(resources.GetObject("tsbNew.Image")));
            this.tsbNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNew.Name = "tsbNew";
            this.tsbNew.Size = new System.Drawing.Size(23, 22);
            this.tsbNew.Tag = "New";
            this.tsbNew.Text = "New";
            this.tsbNew.Click += new System.EventHandler(this.tsbNew_Click);
            // 
            // tsbEdit
            // 
            this.tsbEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbEdit.Image = ((System.Drawing.Image)(resources.GetObject("tsbEdit.Image")));
            this.tsbEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEdit.Name = "tsbEdit";
            this.tsbEdit.Size = new System.Drawing.Size(23, 22);
            this.tsbEdit.Tag = "Edit";
            this.tsbEdit.Text = "Edit";
            this.tsbEdit.Click += new System.EventHandler(this.tsbEdit_Click);
            // 
            // tsbArchive
            // 
            this.tsbArchive.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbArchive.Image = ((System.Drawing.Image)(resources.GetObject("tsbArchive.Image")));
            this.tsbArchive.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbArchive.Name = "tsbArchive";
            this.tsbArchive.Size = new System.Drawing.Size(23, 22);
            this.tsbArchive.Tag = "Archive";
            this.tsbArchive.Text = "Archive";
            this.tsbArchive.Click += new System.EventHandler(this.tsbArchive_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // lblVendorType
            // 
            this.lblVendorType.Name = "lblVendorType";
            this.lblVendorType.Size = new System.Drawing.Size(74, 22);
            this.lblVendorType.Text = "Vendor Type";
            // 
            // cmbVendorType
            // 
            this.cmbVendorType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVendorType.Items.AddRange(new object[] {
            "All",
            "Collection Center",
            "Lab",
            "MRO"});
            this.cmbVendorType.Name = "cmbVendorType";
            this.cmbVendorType.Size = new System.Drawing.Size(121, 25);
            this.cmbVendorType.SelectedIndexChanged += new System.EventHandler(this.cmbVendorType_SelectedIndexChanged);
            // 
            // lblSearch
            // 
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(42, 22);
            this.lblSearch.Text = "&Search";
            // 
            // txtSearchKeyword
            // 
            this.txtSearchKeyword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearchKeyword.Name = "txtSearchKeyword";
            this.txtSearchKeyword.Size = new System.Drawing.Size(200, 25);
            this.txtSearchKeyword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearchKeyword_KeyDown);
            // 
            // btnSearch
            // 
            this.btnSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(23, 22);
            this.btnSearch.Tag = "Search";
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnShowAll
            // 
            this.btnShowAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnShowAll.Image = ((System.Drawing.Image)(resources.GetObject("btnShowAll.Image")));
            this.btnShowAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnShowAll.Name = "btnShowAll";
            this.btnShowAll.Size = new System.Drawing.Size(23, 22);
            this.btnShowAll.Text = "Show All";
            this.btnShowAll.Click += new System.EventHandler(this.btnShowAll_Click);
            // 
            // LblVendorName
            // 
            this.LblVendorName.AutoSize = true;
            this.LblVendorName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblVendorName.Location = new System.Drawing.Point(12, 28);
            this.LblVendorName.Name = "LblVendorName";
            this.LblVendorName.Size = new System.Drawing.Size(104, 20);
            this.LblVendorName.TabIndex = 1;
            this.LblVendorName.Text = "Vendor Info";
            // 
            // dgvVendorInfo
            // 
            this.dgvVendorInfo.AllowUserToAddRows = false;
            this.dgvVendorInfo.AllowUserToDeleteRows = false;
            this.dgvVendorInfo.AllowUserToOrderColumns = true;
            this.dgvVendorInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
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
            this.dgvVendorInfo.Location = new System.Drawing.Point(16, 51);
            this.dgvVendorInfo.MultiSelect = false;
            this.dgvVendorInfo.Name = "dgvVendorInfo";
            this.dgvVendorInfo.ReadOnly = true;
            this.dgvVendorInfo.RowHeadersWidth = 25;
            this.dgvVendorInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvVendorInfo.Size = new System.Drawing.Size(1206, 546);
            this.dgvVendorInfo.TabIndex = 2;
            this.dgvVendorInfo.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVendorInfo_CellDoubleClick);
            this.dgvVendorInfo.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvVendorInfo_ColumnHeaderMouseClick);
            this.dgvVendorInfo.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvVendorInfo_DataBindingComplete);
            this.dgvVendorInfo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvVendorInfo_KeyDown);
            // 
            // chkIncludeInactive
            // 
            this.chkIncludeInactive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkIncludeInactive.AutoSize = true;
            this.chkIncludeInactive.Location = new System.Drawing.Point(1120, 31);
            this.chkIncludeInactive.Name = "chkIncludeInactive";
            this.chkIncludeInactive.Size = new System.Drawing.Size(102, 17);
            this.chkIncludeInactive.TabIndex = 7;
            this.chkIncludeInactive.Text = "Include Inactive";
            this.chkIncludeInactive.UseVisualStyleBackColor = true;
            this.chkIncludeInactive.CheckedChanged += new System.EventHandler(this.chkIncludeInactive_CheckedChanged);
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
            // FrmVendorInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1234, 632);
            this.Controls.Add(this.chkIncludeInactive);
            this.Controls.Add(this.dgvVendorInfo);
            this.Controls.Add(this.LblVendorName);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmVendorInfo";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Vendor Info";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmVendorInfo_FormClosed);
            this.Load += new System.EventHandler(this.FrmVendorInfo_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVendorInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbNew;
        private System.Windows.Forms.ToolStripButton tsbEdit;
        private System.Windows.Forms.ToolStripButton tsbArchive;
        private System.Windows.Forms.Label LblVendorName;
        private System.Windows.Forms.DataGridView dgvVendorInfo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripTextBox txtSearchKeyword;
        private System.Windows.Forms.ToolStripButton btnSearch;
        private System.Windows.Forms.ToolStripLabel lblSearch;
        private System.Windows.Forms.CheckBox chkIncludeInactive;
        private System.Windows.Forms.ToolStripButton btnShowAll;
        private System.Windows.Forms.ToolStripLabel lblVendorType;
        private System.Windows.Forms.ToolStripComboBox cmbVendorType;
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