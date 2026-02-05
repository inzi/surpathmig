namespace SurPath
{
    partial class Frm3rdPartyInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm3rdPartyInfo));
            this.lblThirdPartyInfo = new System.Windows.Forms.Label();
            this.dgvThirdParty = new System.Windows.Forms.DataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbNew = new System.Windows.Forms.ToolStripButton();
            this.tsbEdit = new System.Windows.Forms.ToolStripButton();
            this.tsbArchive = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lblSearch = new System.Windows.Forms.ToolStripLabel();
            this.txtSearchKeyword = new System.Windows.Forms.ToolStripTextBox();
            this.btnSearch = new System.Windows.Forms.ToolStripButton();
            this.BtnShowAll = new System.Windows.Forms.ToolStripButton();
            this.chkIncludeInactive = new System.Windows.Forms.CheckBox();
            this.DonorID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ThirdPartyId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ThirdPartyFirstName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ThirdPartyLastName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ThirdPartyAddress1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ThirdPartyAddress2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ThirdPartyCity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ThirdPartyState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ThirdPartyZip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ThirdPartyPhone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ThirdPartyFax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ThirdPartyEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsActive = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvThirdParty)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblThirdPartyInfo
            // 
            this.lblThirdPartyInfo.AutoSize = true;
            this.lblThirdPartyInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThirdPartyInfo.Location = new System.Drawing.Point(16, 40);
            this.lblThirdPartyInfo.Name = "lblThirdPartyInfo";
            this.lblThirdPartyInfo.Size = new System.Drawing.Size(132, 20);
            this.lblThirdPartyInfo.TabIndex = 1;
            this.lblThirdPartyInfo.Text = "Third Party Info";
            // 
            // dgvThirdParty
            // 
            this.dgvThirdParty.AllowUserToAddRows = false;
            this.dgvThirdParty.AllowUserToDeleteRows = false;
            this.dgvThirdParty.AllowUserToOrderColumns = true;
            this.dgvThirdParty.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvThirdParty.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvThirdParty.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DonorID,
            this.ThirdPartyId,
            this.ThirdPartyFirstName,
            this.ThirdPartyLastName,
            this.ThirdPartyAddress1,
            this.ThirdPartyAddress2,
            this.ThirdPartyCity,
            this.ThirdPartyState,
            this.ThirdPartyZip,
            this.ThirdPartyPhone,
            this.ThirdPartyFax,
            this.ThirdPartyEmail,
            this.Status,
            this.IsActive});
            this.dgvThirdParty.Location = new System.Drawing.Point(16, 65);
            this.dgvThirdParty.MultiSelect = false;
            this.dgvThirdParty.Name = "dgvThirdParty";
            this.dgvThirdParty.ReadOnly = true;
            this.dgvThirdParty.RowHeadersWidth = 25;
            this.dgvThirdParty.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvThirdParty.Size = new System.Drawing.Size(1120, 272);
            this.dgvThirdParty.TabIndex = 2;
            this.dgvThirdParty.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvThirdParty_CellDoubleClick);
            this.dgvThirdParty.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvThirdParty_ColumnHeaderMouseClick);
            this.dgvThirdParty.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvThirdParty_DataBindingComplete);
            this.dgvThirdParty.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvThirdParty_KeyDown);
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
            this.BtnShowAll});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1152, 25);
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
            // BtnShowAll
            // 
            this.BtnShowAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnShowAll.Image = ((System.Drawing.Image)(resources.GetObject("BtnShowAll.Image")));
            this.BtnShowAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnShowAll.Name = "BtnShowAll";
            this.BtnShowAll.Size = new System.Drawing.Size(23, 22);
            this.BtnShowAll.Text = "Show All";
            this.BtnShowAll.Click += new System.EventHandler(this.BtnShowAll_Click);
            // 
            // chkIncludeInactive
            // 
            this.chkIncludeInactive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkIncludeInactive.AutoSize = true;
            this.chkIncludeInactive.Location = new System.Drawing.Point(1034, 42);
            this.chkIncludeInactive.Name = "chkIncludeInactive";
            this.chkIncludeInactive.Size = new System.Drawing.Size(102, 17);
            this.chkIncludeInactive.TabIndex = 3;
            this.chkIncludeInactive.Text = "Include Inactive";
            this.chkIncludeInactive.UseVisualStyleBackColor = true;
            this.chkIncludeInactive.CheckedChanged += new System.EventHandler(this.chkIncludeInactive_CheckedChanged);
            // 
            // DonorID
            // 
            this.DonorID.DataPropertyName = "DonorID";
            this.DonorID.HeaderText = "Donor ID";
            this.DonorID.Name = "DonorID";
            this.DonorID.ReadOnly = true;
            this.DonorID.Visible = false;
            // 
            // ThirdPartyId
            // 
            this.ThirdPartyId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ThirdPartyId.DataPropertyName = "ThirdPartyId";
            this.ThirdPartyId.HeaderText = "ThirdParty Id";
            this.ThirdPartyId.Name = "ThirdPartyId";
            this.ThirdPartyId.ReadOnly = true;
            this.ThirdPartyId.Visible = false;
            // 
            // ThirdPartyFirstName
            // 
            this.ThirdPartyFirstName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ThirdPartyFirstName.DataPropertyName = "ThirdPartyFirstName";
            this.ThirdPartyFirstName.HeaderText = "First Name";
            this.ThirdPartyFirstName.Name = "ThirdPartyFirstName";
            this.ThirdPartyFirstName.ReadOnly = true;
            // 
            // ThirdPartyLastName
            // 
            this.ThirdPartyLastName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ThirdPartyLastName.DataPropertyName = "ThirdPartyLastName";
            this.ThirdPartyLastName.HeaderText = "Last Name";
            this.ThirdPartyLastName.Name = "ThirdPartyLastName";
            this.ThirdPartyLastName.ReadOnly = true;
            // 
            // ThirdPartyAddress1
            // 
            this.ThirdPartyAddress1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ThirdPartyAddress1.DataPropertyName = "ThirdPartyAddress1";
            this.ThirdPartyAddress1.HeaderText = "Address 1";
            this.ThirdPartyAddress1.Name = "ThirdPartyAddress1";
            this.ThirdPartyAddress1.ReadOnly = true;
            // 
            // ThirdPartyAddress2
            // 
            this.ThirdPartyAddress2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ThirdPartyAddress2.DataPropertyName = "ThirdPartyAddress2";
            this.ThirdPartyAddress2.HeaderText = "Address 2";
            this.ThirdPartyAddress2.Name = "ThirdPartyAddress2";
            this.ThirdPartyAddress2.ReadOnly = true;
            // 
            // ThirdPartyCity
            // 
            this.ThirdPartyCity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ThirdPartyCity.DataPropertyName = "ThirdPartyCity";
            this.ThirdPartyCity.HeaderText = "City";
            this.ThirdPartyCity.Name = "ThirdPartyCity";
            this.ThirdPartyCity.ReadOnly = true;
            // 
            // ThirdPartyState
            // 
            this.ThirdPartyState.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ThirdPartyState.DataPropertyName = "ThirdPartyState";
            this.ThirdPartyState.HeaderText = "State";
            this.ThirdPartyState.Name = "ThirdPartyState";
            this.ThirdPartyState.ReadOnly = true;
            // 
            // ThirdPartyZip
            // 
            this.ThirdPartyZip.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ThirdPartyZip.DataPropertyName = "ThirdPartyZip";
            this.ThirdPartyZip.HeaderText = "Zip Code";
            this.ThirdPartyZip.Name = "ThirdPartyZip";
            this.ThirdPartyZip.ReadOnly = true;
            // 
            // ThirdPartyPhone
            // 
            this.ThirdPartyPhone.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ThirdPartyPhone.DataPropertyName = "ThirdPartyPhone";
            this.ThirdPartyPhone.HeaderText = "Phone";
            this.ThirdPartyPhone.Name = "ThirdPartyPhone";
            this.ThirdPartyPhone.ReadOnly = true;
            // 
            // ThirdPartyFax
            // 
            this.ThirdPartyFax.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ThirdPartyFax.DataPropertyName = "ThirdPartyFax";
            this.ThirdPartyFax.HeaderText = "Fax";
            this.ThirdPartyFax.Name = "ThirdPartyFax";
            this.ThirdPartyFax.ReadOnly = true;
            // 
            // ThirdPartyEmail
            // 
            this.ThirdPartyEmail.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ThirdPartyEmail.DataPropertyName = "ThirdPartyEmail";
            this.ThirdPartyEmail.HeaderText = "Email";
            this.ThirdPartyEmail.Name = "ThirdPartyEmail";
            this.ThirdPartyEmail.ReadOnly = true;
            // 
            // Status
            // 
            this.Status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
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
            // Frm3rdPartyInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1152, 346);
            this.Controls.Add(this.chkIncludeInactive);
            this.Controls.Add(this.lblThirdPartyInfo);
            this.Controls.Add(this.dgvThirdParty);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Frm3rdPartyInfo";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Third PartyInfo";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Frm3rdPartyInfo_FormClosed);
            this.Load += new System.EventHandler(this.Frm3rdPartyInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvThirdParty)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblThirdPartyInfo;
        private System.Windows.Forms.DataGridView dgvThirdParty;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbNew;
        private System.Windows.Forms.ToolStripButton tsbEdit;
        private System.Windows.Forms.ToolStripButton tsbArchive;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel lblSearch;
        private System.Windows.Forms.ToolStripTextBox txtSearchKeyword;
        private System.Windows.Forms.ToolStripButton btnSearch;
        private System.Windows.Forms.ToolStripButton BtnShowAll;
        private System.Windows.Forms.CheckBox chkIncludeInactive;
        private System.Windows.Forms.DataGridViewTextBoxColumn DonorID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ThirdPartyId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ThirdPartyFirstName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ThirdPartyLastName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ThirdPartyAddress1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ThirdPartyAddress2;
        private System.Windows.Forms.DataGridViewTextBoxColumn ThirdPartyCity;
        private System.Windows.Forms.DataGridViewTextBoxColumn ThirdPartyState;
        private System.Windows.Forms.DataGridViewTextBoxColumn ThirdPartyZip;
        private System.Windows.Forms.DataGridViewTextBoxColumn ThirdPartyPhone;
        private System.Windows.Forms.DataGridViewTextBoxColumn ThirdPartyFax;
        private System.Windows.Forms.DataGridViewTextBoxColumn ThirdPartyEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsActive;
    }
}