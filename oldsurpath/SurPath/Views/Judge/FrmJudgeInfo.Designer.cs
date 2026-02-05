namespace SurPath
{
    partial class FrmJudgeInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmJudgeInfo));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbNew = new System.Windows.Forms.ToolStripButton();
            this.tsbEdit = new System.Windows.Forms.ToolStripButton();
            this.tsbArchive = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lblSearch = new System.Windows.Forms.ToolStripLabel();
            this.txtSearchKeyword = new System.Windows.Forms.ToolStripTextBox();
            this.btnSearch = new System.Windows.Forms.ToolStripButton();
            this.btnShowAll = new System.Windows.Forms.ToolStripButton();
            this.dgvJudge = new System.Windows.Forms.DataGridView();
            this.JudgeId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.JudgePrefix = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.JudgeFirstName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.JudgeLastName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.JudgeSuffix = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.JudgeAddress1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.JudgeAddress2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.JudgeUsername = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.JudgeCity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.JudgeState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.JudgeZip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsActive = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LblJudge = new System.Windows.Forms.Label();
            this.chkIncludeInactive = new System.Windows.Forms.CheckBox();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvJudge)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
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
            this.toolStrip1.Size = new System.Drawing.Size(1371, 27);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbNew
            // 
            this.tsbNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNew.Image = ((System.Drawing.Image)(resources.GetObject("tsbNew.Image")));
            this.tsbNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNew.Name = "tsbNew";
            this.tsbNew.Size = new System.Drawing.Size(24, 24);
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
            this.tsbEdit.Size = new System.Drawing.Size(24, 24);
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
            this.tsbArchive.Size = new System.Drawing.Size(24, 24);
            this.tsbArchive.Tag = "Archive";
            this.tsbArchive.Text = "Archive";
            this.tsbArchive.Click += new System.EventHandler(this.tsbArchive_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // lblSearch
            // 
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(53, 24);
            this.lblSearch.Tag = "Search";
            this.lblSearch.Text = "&Search";
            // 
            // txtSearchKeyword
            // 
            this.txtSearchKeyword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearchKeyword.Name = "txtSearchKeyword";
            this.txtSearchKeyword.Size = new System.Drawing.Size(266, 27);
            this.txtSearchKeyword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearchKeyword_KeyDown);
            // 
            // btnSearch
            // 
            this.btnSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(24, 24);
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
            this.btnShowAll.Size = new System.Drawing.Size(24, 24);
            this.btnShowAll.Text = "Show All";
            this.btnShowAll.Click += new System.EventHandler(this.btnShowAll_Click);
            // 
            // dgvJudge
            // 
            this.dgvJudge.AllowUserToAddRows = false;
            this.dgvJudge.AllowUserToDeleteRows = false;
            this.dgvJudge.AllowUserToOrderColumns = true;
            this.dgvJudge.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvJudge.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvJudge.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.JudgeId,
            this.JudgePrefix,
            this.JudgeFirstName,
            this.JudgeLastName,
            this.JudgeSuffix,
            this.JudgeAddress1,
            this.JudgeAddress2,
            this.JudgeUsername,
            this.JudgeCity,
            this.JudgeState,
            this.JudgeZip,
            this.Status,
            this.IsActive});
            this.dgvJudge.Location = new System.Drawing.Point(21, 63);
            this.dgvJudge.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvJudge.MultiSelect = false;
            this.dgvJudge.Name = "dgvJudge";
            this.dgvJudge.ReadOnly = true;
            this.dgvJudge.RowHeadersWidth = 25;
            this.dgvJudge.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvJudge.Size = new System.Drawing.Size(1320, 734);
            this.dgvJudge.TabIndex = 6;
            this.dgvJudge.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvJudge_CellDoubleClick);
            this.dgvJudge.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvJudge_ColumnHeaderMouseClick);
            this.dgvJudge.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvJudge_DataBindingComplete);
            this.dgvJudge.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvJudge_KeyDown);
            // 
            // JudgeId
            // 
            this.JudgeId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.JudgeId.DataPropertyName = "JudgeId";
            this.JudgeId.HeaderText = "Judge Id";
            this.JudgeId.Name = "JudgeId";
            this.JudgeId.ReadOnly = true;
            this.JudgeId.Visible = false;
            // 
            // JudgePrefix
            // 
            this.JudgePrefix.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.JudgePrefix.DataPropertyName = "JudgePrefix";
            this.JudgePrefix.HeaderText = "Prefix";
            this.JudgePrefix.Name = "JudgePrefix";
            this.JudgePrefix.ReadOnly = true;
            // 
            // JudgeFirstName
            // 
            this.JudgeFirstName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.JudgeFirstName.DataPropertyName = "JudgeFirstName";
            this.JudgeFirstName.HeaderText = "First Name";
            this.JudgeFirstName.Name = "JudgeFirstName";
            this.JudgeFirstName.ReadOnly = true;
            // 
            // JudgeLastName
            // 
            this.JudgeLastName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.JudgeLastName.DataPropertyName = "JudgeLastName";
            this.JudgeLastName.HeaderText = "Last Name";
            this.JudgeLastName.Name = "JudgeLastName";
            this.JudgeLastName.ReadOnly = true;
            // 
            // JudgeSuffix
            // 
            this.JudgeSuffix.DataPropertyName = "JudgeSuffix";
            this.JudgeSuffix.HeaderText = "Suffix";
            this.JudgeSuffix.Name = "JudgeSuffix";
            this.JudgeSuffix.ReadOnly = true;
            // 
            // JudgeAddress1
            // 
            this.JudgeAddress1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.JudgeAddress1.DataPropertyName = "JudgeAddress1";
            this.JudgeAddress1.HeaderText = "Address 1";
            this.JudgeAddress1.Name = "JudgeAddress1";
            this.JudgeAddress1.ReadOnly = true;
            // 
            // JudgeAddress2
            // 
            this.JudgeAddress2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.JudgeAddress2.DataPropertyName = "JudgeAddress2";
            this.JudgeAddress2.HeaderText = "Address 2";
            this.JudgeAddress2.Name = "JudgeAddress2";
            this.JudgeAddress2.ReadOnly = true;
            // 
            // JudgeUsername
            // 
            this.JudgeUsername.DataPropertyName = "JudgeUsername";
            this.JudgeUsername.HeaderText = "Username";
            this.JudgeUsername.Name = "JudgeUsername";
            this.JudgeUsername.ReadOnly = true;
            // 
            // JudgeCity
            // 
            this.JudgeCity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.JudgeCity.DataPropertyName = "JudgeCity";
            this.JudgeCity.HeaderText = "City";
            this.JudgeCity.Name = "JudgeCity";
            this.JudgeCity.ReadOnly = true;
            // 
            // JudgeState
            // 
            this.JudgeState.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.JudgeState.DataPropertyName = "JudgeState";
            this.JudgeState.HeaderText = "State";
            this.JudgeState.Name = "JudgeState";
            this.JudgeState.ReadOnly = true;
            // 
            // JudgeZip
            // 
            this.JudgeZip.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.JudgeZip.DataPropertyName = "JudgeZip";
            this.JudgeZip.HeaderText = "Zip Code";
            this.JudgeZip.Name = "JudgeZip";
            this.JudgeZip.ReadOnly = true;
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
            // LblJudge
            // 
            this.LblJudge.AutoSize = true;
            this.LblJudge.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblJudge.Location = new System.Drawing.Point(16, 34);
            this.LblJudge.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblJudge.Name = "LblJudge";
            this.LblJudge.Size = new System.Drawing.Size(114, 25);
            this.LblJudge.TabIndex = 5;
            this.LblJudge.Text = "Judge Info";
            // 
            // chkIncludeInactive
            // 
            this.chkIncludeInactive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkIncludeInactive.AutoSize = true;
            this.chkIncludeInactive.Location = new System.Drawing.Point(1214, 38);
            this.chkIncludeInactive.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkIncludeInactive.Name = "chkIncludeInactive";
            this.chkIncludeInactive.Size = new System.Drawing.Size(127, 21);
            this.chkIncludeInactive.TabIndex = 7;
            this.chkIncludeInactive.Text = "Include Inactive";
            this.chkIncludeInactive.UseVisualStyleBackColor = true;
            this.chkIncludeInactive.CheckedChanged += new System.EventHandler(this.chkIncludeInactive_CheckedChanged);
            // 
            // FrmJudgeInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1371, 827);
            this.Controls.Add(this.chkIncludeInactive);
            this.Controls.Add(this.dgvJudge);
            this.Controls.Add(this.LblJudge);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FrmJudgeInfo";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Judge Info";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmJudgeInfo_FormClosed);
            this.Load += new System.EventHandler(this.FrmJudgeInfo_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvJudge)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbNew;
        private System.Windows.Forms.ToolStripButton tsbEdit;
        private System.Windows.Forms.ToolStripButton tsbArchive;
        private System.Windows.Forms.DataGridView dgvJudge;
        private System.Windows.Forms.Label LblJudge;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripTextBox txtSearchKeyword;
        private System.Windows.Forms.ToolStripButton btnSearch;
        private System.Windows.Forms.ToolStripLabel lblSearch;
        private System.Windows.Forms.CheckBox chkIncludeInactive;
        private System.Windows.Forms.ToolStripButton btnShowAll;
        private System.Windows.Forms.DataGridViewTextBoxColumn JudgeId;
        private System.Windows.Forms.DataGridViewTextBoxColumn JudgePrefix;
        private System.Windows.Forms.DataGridViewTextBoxColumn JudgeFirstName;
        private System.Windows.Forms.DataGridViewTextBoxColumn JudgeLastName;
        private System.Windows.Forms.DataGridViewTextBoxColumn JudgeSuffix;
        private System.Windows.Forms.DataGridViewTextBoxColumn JudgeAddress1;
        private System.Windows.Forms.DataGridViewTextBoxColumn JudgeAddress2;
        private System.Windows.Forms.DataGridViewTextBoxColumn JudgeUsername;
        private System.Windows.Forms.DataGridViewTextBoxColumn JudgeCity;
        private System.Windows.Forms.DataGridViewTextBoxColumn JudgeState;
        private System.Windows.Forms.DataGridViewTextBoxColumn JudgeZip;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsActive;
    }
}