namespace SurPath
{
    partial class FrmTestPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTestPanel));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbNew = new System.Windows.Forms.ToolStripButton();
            this.tsbEdit = new System.Windows.Forms.ToolStripButton();
            this.tsbArchive = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lblSearch = new System.Windows.Forms.ToolStripLabel();
            this.txtSearchKeyword = new System.Windows.Forms.ToolStripTextBox();
            this.btnSearch = new System.Windows.Forms.ToolStripButton();
            this.btnShowAll = new System.Windows.Forms.ToolStripButton();
            this.LblTestPanel = new System.Windows.Forms.Label();
            this.dgvTestPanel = new System.Windows.Forms.DataGridView();
            this.TestPanelId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TestPanelName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TestCategoryId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TestCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsActive = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkIncludeInactive = new System.Windows.Forms.CheckBox();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTestPanel)).BeginInit();
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
            this.txtSearchKeyword.Tag = "";
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
            // LblTestPanel
            // 
            this.LblTestPanel.AutoSize = true;
            this.LblTestPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblTestPanel.Location = new System.Drawing.Point(12, 28);
            this.LblTestPanel.Name = "LblTestPanel";
            this.LblTestPanel.Size = new System.Drawing.Size(94, 20);
            this.LblTestPanel.TabIndex = 1;
            this.LblTestPanel.Text = "Test Panel";
            // 
            // dgvTestPanel
            // 
            this.dgvTestPanel.AllowUserToAddRows = false;
            this.dgvTestPanel.AllowUserToDeleteRows = false;
            this.dgvTestPanel.AllowUserToOrderColumns = true;
            this.dgvTestPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTestPanel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTestPanel.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TestPanelId,
            this.TestPanelName,
            this.TestCategoryId,
            this.Category,
            this.TestCost,
            this.Status,
            this.IsActive});
            this.dgvTestPanel.Location = new System.Drawing.Point(16, 51);
            this.dgvTestPanel.MultiSelect = false;
            this.dgvTestPanel.Name = "dgvTestPanel";
            this.dgvTestPanel.ReadOnly = true;
            this.dgvTestPanel.RowHeadersWidth = 25;
            this.dgvTestPanel.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTestPanel.Size = new System.Drawing.Size(996, 546);
            this.dgvTestPanel.TabIndex = 3;
            this.dgvTestPanel.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTestPanel_CellDoubleClick);
            this.dgvTestPanel.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvTestPanel_ColumnHeaderMouseClick);
            this.dgvTestPanel.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvTestPanel_DataBindingComplete);
            this.dgvTestPanel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvTestPanel_KeyDown);
            // 
            // TestPanelId
            // 
            this.TestPanelId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TestPanelId.DataPropertyName = "TestPanelId";
            this.TestPanelId.HeaderText = "Test Panel Id";
            this.TestPanelId.Name = "TestPanelId";
            this.TestPanelId.ReadOnly = true;
            this.TestPanelId.Visible = false;
            // 
            // TestPanelName
            // 
            this.TestPanelName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TestPanelName.DataPropertyName = "TestPanelName";
            this.TestPanelName.HeaderText = "Test Panel Code";
            this.TestPanelName.Name = "TestPanelName";
            this.TestPanelName.ReadOnly = true;
            // 
            // TestCategoryId
            // 
            this.TestCategoryId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TestCategoryId.DataPropertyName = "TestCategoryId";
            this.TestCategoryId.HeaderText = "Test Category Id";
            this.TestCategoryId.Name = "TestCategoryId";
            this.TestCategoryId.ReadOnly = true;
            this.TestCategoryId.Visible = false;
            // 
            // Category
            // 
            this.Category.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Category.HeaderText = "Category";
            this.Category.Name = "Category";
            this.Category.ReadOnly = true;
            // 
            // TestCost
            // 
            this.TestCost.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TestCost.DataPropertyName = "TestCost";
            this.TestCost.HeaderText = "Test Cost";
            this.TestCost.Name = "TestCost";
            this.TestCost.ReadOnly = true;
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
            // chkIncludeInactive
            // 
            this.chkIncludeInactive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkIncludeInactive.AutoSize = true;
            this.chkIncludeInactive.Location = new System.Drawing.Point(910, 28);
            this.chkIncludeInactive.Name = "chkIncludeInactive";
            this.chkIncludeInactive.Size = new System.Drawing.Size(102, 17);
            this.chkIncludeInactive.TabIndex = 2;
            this.chkIncludeInactive.Text = "Include Inactive";
            this.chkIncludeInactive.UseVisualStyleBackColor = true;
            this.chkIncludeInactive.CheckedChanged += new System.EventHandler(this.chkIncludeInactive_CheckedChanged);
            // 
            // FrmTestPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 619);
            this.Controls.Add(this.chkIncludeInactive);
            this.Controls.Add(this.dgvTestPanel);
            this.Controls.Add(this.LblTestPanel);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmTestPanel";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Test Panel";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmTestPanel_FormClosed);
            this.Load += new System.EventHandler(this.FrmTestPanel_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTestPanel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbNew;
        private System.Windows.Forms.ToolStripButton tsbEdit;
        private System.Windows.Forms.ToolStripButton tsbArchive;
        private System.Windows.Forms.Label LblTestPanel;
        private System.Windows.Forms.DataGridView dgvTestPanel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripTextBox txtSearchKeyword;
        private System.Windows.Forms.ToolStripButton btnSearch;
        private System.Windows.Forms.ToolStripLabel lblSearch;
        private System.Windows.Forms.ToolStripButton btnShowAll;
        private System.Windows.Forms.CheckBox chkIncludeInactive;
        private System.Windows.Forms.DataGridViewTextBoxColumn TestPanelId;
        private System.Windows.Forms.DataGridViewTextBoxColumn TestPanelName;
        private System.Windows.Forms.DataGridViewTextBoxColumn TestCategoryId;
        private System.Windows.Forms.DataGridViewTextBoxColumn Category;
        private System.Windows.Forms.DataGridViewTextBoxColumn TestCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsActive;
    }
}