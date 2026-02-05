namespace SurPath
{
    partial class FrmVendorServiceInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmVendorServiceInfo));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbNew = new System.Windows.Forms.ToolStripButton();
            this.tsbEdit = new System.Windows.Forms.ToolStripButton();
            this.tsbArchive = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lblSearch = new System.Windows.Forms.ToolStripLabel();
            this.txtSearchKeyword = new System.Windows.Forms.ToolStripTextBox();
            this.btnSearch = new System.Windows.Forms.ToolStripButton();
            this.btnShowAll = new System.Windows.Forms.ToolStripButton();
            this.dgvVendorService = new System.Windows.Forms.DataGridView();
            this.VendorServiceId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TestCategoryId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ObservedType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FormTypeId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VendorServiceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TestCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsObserved = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FormType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsActive = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LblvendorService = new System.Windows.Forms.Label();
            this.lblVendorNameHeader = new System.Windows.Forms.Label();
            this.lblVendorName = new System.Windows.Forms.Label();
            this.chkIncludeInactive = new System.Windows.Forms.CheckBox();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVendorService)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNew,
            this.tsbEdit,
            this.tsbArchive,
            this.toolStripSeparator1,
            this.lblSearch,
            this.txtSearchKeyword,
            this.btnSearch,
            this.btnShowAll});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1015, 25);
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
            // lblSearch
            // 
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(42, 22);
            this.lblSearch.Tag = "Search";
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
            // dgvVendorService
            // 
            this.dgvVendorService.AllowUserToAddRows = false;
            this.dgvVendorService.AllowUserToDeleteRows = false;
            this.dgvVendorService.AllowUserToOrderColumns = true;
            this.dgvVendorService.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvVendorService.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVendorService.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.VendorServiceId,
            this.TestCategoryId,
            this.ObservedType,
            this.FormTypeId,
            this.VendorServiceName,
            this.Cost,
            this.TestCategory,
            this.IsObserved,
            this.FormType,
            this.Status,
            this.IsActive});
            this.dgvVendorService.Location = new System.Drawing.Point(17, 97);
            this.dgvVendorService.MultiSelect = false;
            this.dgvVendorService.Name = "dgvVendorService";
            this.dgvVendorService.ReadOnly = true;
            this.dgvVendorService.RowHeadersWidth = 25;
            this.dgvVendorService.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvVendorService.Size = new System.Drawing.Size(979, 223);
            this.dgvVendorService.TabIndex = 5;
            this.dgvVendorService.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVendorService_CellDoubleClick);
            this.dgvVendorService.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvVendorService_ColumnHeaderMouseClick);
            this.dgvVendorService.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvVendorService_DataBindingComplete);
            this.dgvVendorService.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvVendorService_KeyDown);
            // 
            // VendorServiceId
            // 
            this.VendorServiceId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.VendorServiceId.DataPropertyName = "VendorServiceId";
            this.VendorServiceId.HeaderText = "Vendor Service Id";
            this.VendorServiceId.Name = "VendorServiceId";
            this.VendorServiceId.ReadOnly = true;
            this.VendorServiceId.Visible = false;
            // 
            // TestCategoryId
            // 
            this.TestCategoryId.DataPropertyName = "TestCategoryId";
            this.TestCategoryId.HeaderText = "Test Category Id";
            this.TestCategoryId.Name = "TestCategoryId";
            this.TestCategoryId.ReadOnly = true;
            this.TestCategoryId.Visible = false;
            // 
            // ObservedType
            // 
            this.ObservedType.DataPropertyName = "IsObserved";
            this.ObservedType.HeaderText = "Observed Type";
            this.ObservedType.Name = "ObservedType";
            this.ObservedType.ReadOnly = true;
            this.ObservedType.Visible = false;
            // 
            // FormTypeId
            // 
            this.FormTypeId.DataPropertyName = "FormTypeId";
            this.FormTypeId.HeaderText = "Form Type Id";
            this.FormTypeId.Name = "FormTypeId";
            this.FormTypeId.ReadOnly = true;
            this.FormTypeId.Visible = false;
            // 
            // VendorServiceName
            // 
            this.VendorServiceName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.VendorServiceName.DataPropertyName = "VendorServiceNameValue";
            this.VendorServiceName.HeaderText = "Vendor Service Name";
            this.VendorServiceName.Name = "VendorServiceName";
            this.VendorServiceName.ReadOnly = true;
            // 
            // Cost
            // 
            this.Cost.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Cost.DataPropertyName = "Cost";
            this.Cost.HeaderText = "Cost";
            this.Cost.Name = "Cost";
            this.Cost.ReadOnly = true;
            // 
            // TestCategory
            // 
            this.TestCategory.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TestCategory.HeaderText = "Test Category";
            this.TestCategory.Name = "TestCategory";
            this.TestCategory.ReadOnly = true;
            // 
            // IsObserved
            // 
            this.IsObserved.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.IsObserved.HeaderText = "Observed Type";
            this.IsObserved.Name = "IsObserved";
            this.IsObserved.ReadOnly = true;
            // 
            // FormType
            // 
            this.FormType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.FormType.HeaderText = "Form Type";
            this.FormType.Name = "FormType";
            this.FormType.ReadOnly = true;
            // 
            // Status
            // 
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.Width = 85;
            // 
            // IsActive
            // 
            this.IsActive.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.IsActive.DataPropertyName = "IsActive";
            this.IsActive.HeaderText = "Status";
            this.IsActive.Name = "IsActive";
            this.IsActive.ReadOnly = true;
            this.IsActive.Visible = false;
            // 
            // LblvendorService
            // 
            this.LblvendorService.AutoSize = true;
            this.LblvendorService.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblvendorService.Location = new System.Drawing.Point(17, 27);
            this.LblvendorService.Name = "LblvendorService";
            this.LblvendorService.Size = new System.Drawing.Size(168, 20);
            this.LblvendorService.TabIndex = 1;
            this.LblvendorService.Text = "Vendor Service Info";
            // 
            // lblVendorNameHeader
            // 
            this.lblVendorNameHeader.AutoSize = true;
            this.lblVendorNameHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVendorNameHeader.Location = new System.Drawing.Point(17, 65);
            this.lblVendorNameHeader.Name = "lblVendorNameHeader";
            this.lblVendorNameHeader.Size = new System.Drawing.Size(83, 13);
            this.lblVendorNameHeader.TabIndex = 2;
            this.lblVendorNameHeader.Text = "Vendor Name";
            // 
            // lblVendorName
            // 
            this.lblVendorName.AutoSize = true;
            this.lblVendorName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVendorName.ForeColor = System.Drawing.Color.Maroon;
            this.lblVendorName.Location = new System.Drawing.Point(121, 65);
            this.lblVendorName.Name = "lblVendorName";
            this.lblVendorName.Size = new System.Drawing.Size(83, 13);
            this.lblVendorName.TabIndex = 3;
            this.lblVendorName.Text = "Vendor Name";
            // 
            // chkIncludeInactive
            // 
            this.chkIncludeInactive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkIncludeInactive.AutoSize = true;
            this.chkIncludeInactive.Location = new System.Drawing.Point(894, 74);
            this.chkIncludeInactive.Name = "chkIncludeInactive";
            this.chkIncludeInactive.Size = new System.Drawing.Size(102, 17);
            this.chkIncludeInactive.TabIndex = 4;
            this.chkIncludeInactive.Text = "Include Inactive";
            this.chkIncludeInactive.UseVisualStyleBackColor = true;
            this.chkIncludeInactive.CheckedChanged += new System.EventHandler(this.chkIncludeInactive_CheckedChanged);
            // 
            // FrmVendorServiceInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1015, 346);
            this.Controls.Add(this.chkIncludeInactive);
            this.Controls.Add(this.lblVendorName);
            this.Controls.Add(this.lblVendorNameHeader);
            this.Controls.Add(this.dgvVendorService);
            this.Controls.Add(this.LblvendorService);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmVendorServiceInfo";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Vendor Service Info";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmVendorServiceInfo_FormClosed);
            this.Load += new System.EventHandler(this.FrmVendorServiceInfo_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVendorService)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbNew;
        private System.Windows.Forms.ToolStripButton tsbEdit;
        private System.Windows.Forms.ToolStripButton tsbArchive;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripTextBox txtSearchKeyword;
        private System.Windows.Forms.ToolStripButton btnSearch;
        private System.Windows.Forms.DataGridView dgvVendorService;
        private System.Windows.Forms.Label LblvendorService;
        private System.Windows.Forms.ToolStripLabel lblSearch;
        private System.Windows.Forms.Label lblVendorNameHeader;
        private System.Windows.Forms.Label lblVendorName;
        private System.Windows.Forms.CheckBox chkIncludeInactive;
        private System.Windows.Forms.ToolStripButton btnShowAll;
        private System.Windows.Forms.DataGridViewTextBoxColumn VendorServiceId;
        private System.Windows.Forms.DataGridViewTextBoxColumn TestCategoryId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ObservedType;
        private System.Windows.Forms.DataGridViewTextBoxColumn FormTypeId;
        private System.Windows.Forms.DataGridViewTextBoxColumn VendorServiceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cost;
        private System.Windows.Forms.DataGridViewTextBoxColumn TestCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsObserved;
        private System.Windows.Forms.DataGridViewTextBoxColumn FormType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsActive;

    }
}