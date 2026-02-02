namespace SurPath
{
    partial class FrmAttorneyInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAttorneyInfo));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbNew = new System.Windows.Forms.ToolStripButton();
            this.tsbEdit = new System.Windows.Forms.ToolStripButton();
            this.tsbArchive = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lblSearch = new System.Windows.Forms.ToolStripLabel();
            this.txtSearchKeyword = new System.Windows.Forms.ToolStripTextBox();
            this.btnSearch = new System.Windows.Forms.ToolStripButton();
            this.btnShowAll = new System.Windows.Forms.ToolStripButton();
            this.LblAttorney = new System.Windows.Forms.Label();
            this.dgvAttorney = new System.Windows.Forms.DataGridView();
            this.AttorneyId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AttorneyFirstName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AttorneyLastName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AttorneyAddress1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AttorneyAddress2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AttorneyCity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AttorneyState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AttorneyZip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AttorneyPhone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AttorneyFax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AttorneyEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsActive = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkIncludeInactive = new System.Windows.Forms.CheckBox();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttorney)).BeginInit();
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
            this.toolStrip1.Size = new System.Drawing.Size(1028, 25);
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
            // LblAttorney
            // 
            this.LblAttorney.AutoSize = true;
            this.LblAttorney.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblAttorney.Location = new System.Drawing.Point(12, 28);
            this.LblAttorney.Name = "LblAttorney";
            this.LblAttorney.Size = new System.Drawing.Size(114, 20);
            this.LblAttorney.TabIndex = 1;
            this.LblAttorney.Text = "Attorney Info";
            // 
            // dgvAttorney
            // 
            this.dgvAttorney.AllowUserToAddRows = false;
            this.dgvAttorney.AllowUserToDeleteRows = false;
            this.dgvAttorney.AllowUserToOrderColumns = true;
            this.dgvAttorney.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAttorney.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAttorney.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AttorneyId,
            this.AttorneyFirstName,
            this.AttorneyLastName,
            this.AttorneyAddress1,
            this.AttorneyAddress2,
            this.AttorneyCity,
            this.AttorneyState,
            this.AttorneyZip,
            this.AttorneyPhone,
            this.AttorneyFax,
            this.AttorneyEmail,
            this.Status,
            this.IsActive});
            this.dgvAttorney.Location = new System.Drawing.Point(16, 51);
            this.dgvAttorney.MultiSelect = false;
            this.dgvAttorney.Name = "dgvAttorney";
            this.dgvAttorney.ReadOnly = true;
            this.dgvAttorney.RowHeadersWidth = 25;
            this.dgvAttorney.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAttorney.Size = new System.Drawing.Size(996, 518);
            this.dgvAttorney.TabIndex = 2;
            this.dgvAttorney.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAttorney_CellDoubleClick);
            this.dgvAttorney.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvAttorney_ColumnHeaderMouseClick);
            this.dgvAttorney.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvAttorney_DataBindingComplete);
            this.dgvAttorney.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvAttorney_KeyDown);
            // 
            // AttorneyId
            // 
            this.AttorneyId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.AttorneyId.DataPropertyName = "AttorneyId";
            this.AttorneyId.HeaderText = "Attorney Id";
            this.AttorneyId.Name = "AttorneyId";
            this.AttorneyId.ReadOnly = true;
            this.AttorneyId.Visible = false;
            // 
            // AttorneyFirstName
            // 
            this.AttorneyFirstName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.AttorneyFirstName.DataPropertyName = "AttorneyFirstName";
            this.AttorneyFirstName.HeaderText = "First Name";
            this.AttorneyFirstName.Name = "AttorneyFirstName";
            this.AttorneyFirstName.ReadOnly = true;
            // 
            // AttorneyLastName
            // 
            this.AttorneyLastName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.AttorneyLastName.DataPropertyName = "AttorneyLastName";
            this.AttorneyLastName.HeaderText = "Last Name";
            this.AttorneyLastName.Name = "AttorneyLastName";
            this.AttorneyLastName.ReadOnly = true;
            // 
            // AttorneyAddress1
            // 
            this.AttorneyAddress1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.AttorneyAddress1.DataPropertyName = "AttorneyAddress1";
            this.AttorneyAddress1.HeaderText = "Address 1";
            this.AttorneyAddress1.Name = "AttorneyAddress1";
            this.AttorneyAddress1.ReadOnly = true;
            // 
            // AttorneyAddress2
            // 
            this.AttorneyAddress2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.AttorneyAddress2.DataPropertyName = "AttorneyAddress2";
            this.AttorneyAddress2.HeaderText = "Address 2";
            this.AttorneyAddress2.Name = "AttorneyAddress2";
            this.AttorneyAddress2.ReadOnly = true;
            // 
            // AttorneyCity
            // 
            this.AttorneyCity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.AttorneyCity.DataPropertyName = "AttorneyCity";
            this.AttorneyCity.HeaderText = "City";
            this.AttorneyCity.Name = "AttorneyCity";
            this.AttorneyCity.ReadOnly = true;
            // 
            // AttorneyState
            // 
            this.AttorneyState.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.AttorneyState.DataPropertyName = "AttorneyState";
            this.AttorneyState.HeaderText = "State";
            this.AttorneyState.Name = "AttorneyState";
            this.AttorneyState.ReadOnly = true;
            // 
            // AttorneyZip
            // 
            this.AttorneyZip.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.AttorneyZip.DataPropertyName = "AttorneyZip";
            this.AttorneyZip.HeaderText = "Zip Code";
            this.AttorneyZip.Name = "AttorneyZip";
            this.AttorneyZip.ReadOnly = true;
            // 
            // AttorneyPhone
            // 
            this.AttorneyPhone.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.AttorneyPhone.DataPropertyName = "AttorneyPhone";
            this.AttorneyPhone.HeaderText = "Phone";
            this.AttorneyPhone.Name = "AttorneyPhone";
            this.AttorneyPhone.ReadOnly = true;
            // 
            // AttorneyFax
            // 
            this.AttorneyFax.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.AttorneyFax.DataPropertyName = "AttorneyFax";
            this.AttorneyFax.HeaderText = "Fax";
            this.AttorneyFax.Name = "AttorneyFax";
            this.AttorneyFax.ReadOnly = true;
            // 
            // AttorneyEmail
            // 
            this.AttorneyEmail.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.AttorneyEmail.DataPropertyName = "AttorneyEmail";
            this.AttorneyEmail.HeaderText = "Email";
            this.AttorneyEmail.Name = "AttorneyEmail";
            this.AttorneyEmail.ReadOnly = true;
            // 
            // Status
            // 
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            // 
            // IsActive
            // 
            this.IsActive.DataPropertyName = "IsActive";
            this.IsActive.HeaderText = "Status";
            this.IsActive.Name = "IsActive";
            this.IsActive.ReadOnly = true;
            this.IsActive.Visible = false;
            // 
            // chkIncludeInactive
            // 
            this.chkIncludeInactive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkIncludeInactive.AutoSize = true;
            this.chkIncludeInactive.Location = new System.Drawing.Point(910, 31);
            this.chkIncludeInactive.Name = "chkIncludeInactive";
            this.chkIncludeInactive.Size = new System.Drawing.Size(102, 17);
            this.chkIncludeInactive.TabIndex = 3;
            this.chkIncludeInactive.Text = "Include Inactive";
            this.chkIncludeInactive.UseVisualStyleBackColor = true;
            this.chkIncludeInactive.CheckedChanged += new System.EventHandler(this.chkIncludeInactive_CheckedChanged);
            // 
            // FrmAttorneyInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 592);
            this.Controls.Add(this.chkIncludeInactive);
            this.Controls.Add(this.dgvAttorney);
            this.Controls.Add(this.LblAttorney);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmAttorneyInfo";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Attorney Info";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmAttorneyInfo_FormClosed);
            this.Load += new System.EventHandler(this.FrmAttorneyInfo_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttorney)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbNew;
        private System.Windows.Forms.ToolStripButton tsbEdit;
        private System.Windows.Forms.ToolStripButton tsbArchive;
        private System.Windows.Forms.Label LblAttorney;
        private System.Windows.Forms.DataGridView dgvAttorney;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripTextBox txtSearchKeyword;
        private System.Windows.Forms.ToolStripButton btnSearch;
        private System.Windows.Forms.ToolStripLabel lblSearch;
        private System.Windows.Forms.CheckBox chkIncludeInactive;
        private System.Windows.Forms.ToolStripButton btnShowAll;
        private System.Windows.Forms.DataGridViewTextBoxColumn AttorneyId;
        private System.Windows.Forms.DataGridViewTextBoxColumn AttorneyFirstName;
        private System.Windows.Forms.DataGridViewTextBoxColumn AttorneyLastName;
        private System.Windows.Forms.DataGridViewTextBoxColumn AttorneyAddress1;
        private System.Windows.Forms.DataGridViewTextBoxColumn AttorneyAddress2;
        private System.Windows.Forms.DataGridViewTextBoxColumn AttorneyCity;
        private System.Windows.Forms.DataGridViewTextBoxColumn AttorneyState;
        private System.Windows.Forms.DataGridViewTextBoxColumn AttorneyZip;
        private System.Windows.Forms.DataGridViewTextBoxColumn AttorneyPhone;
        private System.Windows.Forms.DataGridViewTextBoxColumn AttorneyFax;
        private System.Windows.Forms.DataGridViewTextBoxColumn AttorneyEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsActive;
    }
}