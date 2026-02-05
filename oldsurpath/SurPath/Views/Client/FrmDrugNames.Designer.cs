namespace SurPath
{
    partial class FrmDrugNames
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDrugNames));
            this.dgvDrugNames = new System.Windows.Forms.DataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbNew = new System.Windows.Forms.ToolStripButton();
            this.tsbEdit = new System.Windows.Forms.ToolStripButton();
            this.tsbArchive = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lblSearch = new System.Windows.Forms.ToolStripLabel();
            this.txtSearchKeyword = new System.Windows.Forms.ToolStripTextBox();
            this.btnSearch = new System.Windows.Forms.ToolStripButton();
            this.btnShowAll = new System.Windows.Forms.ToolStripButton();
            this.LblDrugName = new System.Windows.Forms.Label();
            this.chkIncludeInactive = new System.Windows.Forms.CheckBox();
            this.DrugNameId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DrugNameValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DrugCodeValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HaveUA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsUA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HaveHair = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsHair = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UAScreenValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UAConfirmationValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HairScreenValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HairConfirmationValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UnitOfMeasurement = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsActive = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDrugNames)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvDrugNames
            // 
            this.dgvDrugNames.AllowUserToAddRows = false;
            this.dgvDrugNames.AllowUserToDeleteRows = false;
            this.dgvDrugNames.AllowUserToOrderColumns = true;
            this.dgvDrugNames.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDrugNames.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDrugNames.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DrugNameId,
            this.DrugNameValue,
            this.DrugCodeValue,
            this.HaveUA,
            this.IsUA,
            this.HaveHair,
            this.IsHair,
            this.UAScreenValue,
            this.UAConfirmationValue,
            this.HairScreenValue,
            this.HairConfirmationValue,
            this.UnitOfMeasurement,
            this.Status,
            this.IsActive});
            this.dgvDrugNames.Location = new System.Drawing.Point(16, 51);
            this.dgvDrugNames.MultiSelect = false;
            this.dgvDrugNames.Name = "dgvDrugNames";
            this.dgvDrugNames.ReadOnly = true;
            this.dgvDrugNames.RowHeadersWidth = 25;
            this.dgvDrugNames.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDrugNames.Size = new System.Drawing.Size(996, 557);
            this.dgvDrugNames.TabIndex = 2;
            this.dgvDrugNames.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDrugNames_CellDoubleClick);
            this.dgvDrugNames.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvDrugNames_ColumnHeaderMouseClick);
            this.dgvDrugNames.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvDrugNames_DataBindingComplete);
            this.dgvDrugNames.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvDrugNames_KeyDown);
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
            this.tsbArchive.Image = global::SurPath.Properties.Resources.Archive;
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
            // LblDrugName
            // 
            this.LblDrugName.AutoSize = true;
            this.LblDrugName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblDrugName.Location = new System.Drawing.Point(12, 28);
            this.LblDrugName.Name = "LblDrugName";
            this.LblDrugName.Size = new System.Drawing.Size(108, 20);
            this.LblDrugName.TabIndex = 1;
            this.LblDrugName.Text = "Drug Names";
            // 
            // chkIncludeInactive
            // 
            this.chkIncludeInactive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkIncludeInactive.AutoSize = true;
            this.chkIncludeInactive.Location = new System.Drawing.Point(910, 28);
            this.chkIncludeInactive.Name = "chkIncludeInactive";
            this.chkIncludeInactive.Size = new System.Drawing.Size(102, 17);
            this.chkIncludeInactive.TabIndex = 8;
            this.chkIncludeInactive.Text = "Include Inactive";
            this.chkIncludeInactive.UseVisualStyleBackColor = true;
            this.chkIncludeInactive.CheckedChanged += new System.EventHandler(this.chkIncludeInactive_CheckedChanged);
            // 
            // DrugNameId
            // 
            this.DrugNameId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DrugNameId.DataPropertyName = "DrugNameId";
            this.DrugNameId.HeaderText = "Drug Name Id";
            this.DrugNameId.Name = "DrugNameId";
            this.DrugNameId.ReadOnly = true;
            this.DrugNameId.Visible = false;
            // 
            // DrugNameValue
            // 
            this.DrugNameValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DrugNameValue.DataPropertyName = "DrugNameValue";
            this.DrugNameValue.HeaderText = "Drug Names";
            this.DrugNameValue.Name = "DrugNameValue";
            this.DrugNameValue.ReadOnly = true;
            // 
            // DrugCodeValue
            // 
            this.DrugCodeValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DrugCodeValue.DataPropertyName = "DrugCodeValue";
            this.DrugCodeValue.HeaderText = "Drug Code";
            this.DrugCodeValue.Name = "DrugCodeValue";
            this.DrugCodeValue.ReadOnly = true;
            // 
            // HaveUA
            // 
            this.HaveUA.HeaderText = "UA";
            this.HaveUA.Name = "HaveUA";
            this.HaveUA.ReadOnly = true;
            // 
            // IsUA
            // 
            this.IsUA.DataPropertyName = "IsUA";
            this.IsUA.HeaderText = "IsUA";
            this.IsUA.Name = "IsUA";
            this.IsUA.ReadOnly = true;
            this.IsUA.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.IsUA.Visible = false;
            // 
            // HaveHair
            // 
            this.HaveHair.HeaderText = "Hair";
            this.HaveHair.Name = "HaveHair";
            this.HaveHair.ReadOnly = true;
            // 
            // IsHair
            // 
            this.IsHair.DataPropertyName = "IsHair";
            this.IsHair.HeaderText = "IsHair";
            this.IsHair.Name = "IsHair";
            this.IsHair.ReadOnly = true;
            this.IsHair.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.IsHair.Visible = false;
            // 
            // UAScreenValue
            // 
            this.UAScreenValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.UAScreenValue.DataPropertyName = "UAScreenValue";
            this.UAScreenValue.HeaderText = "UA Screen Value";
            this.UAScreenValue.Name = "UAScreenValue";
            this.UAScreenValue.ReadOnly = true;
            // 
            // UAConfirmationValue
            // 
            this.UAConfirmationValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.UAConfirmationValue.DataPropertyName = "UAConfirmationValue";
            this.UAConfirmationValue.HeaderText = "UA Confirmation Value";
            this.UAConfirmationValue.Name = "UAConfirmationValue";
            this.UAConfirmationValue.ReadOnly = true;
            // 
            // HairScreenValue
            // 
            this.HairScreenValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.HairScreenValue.DataPropertyName = "HairScreenValue";
            this.HairScreenValue.HeaderText = "Hair Screen Value";
            this.HairScreenValue.Name = "HairScreenValue";
            this.HairScreenValue.ReadOnly = true;
            // 
            // HairConfirmationValue
            // 
            this.HairConfirmationValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.HairConfirmationValue.DataPropertyName = "HairConfirmationValue";
            this.HairConfirmationValue.HeaderText = "Hair Confirmation Value";
            this.HairConfirmationValue.Name = "HairConfirmationValue";
            this.HairConfirmationValue.ReadOnly = true;
            // 
            // UnitOfMeasurement
            // 
            this.UnitOfMeasurement.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.UnitOfMeasurement.DataPropertyName = "UnitOfMeasurement";
            this.UnitOfMeasurement.HeaderText = "Unit of Measurement";
            this.UnitOfMeasurement.Name = "UnitOfMeasurement";
            this.UnitOfMeasurement.ReadOnly = true;
            this.UnitOfMeasurement.Visible = false;
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
            // FrmDrugNames
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 632);
            this.Controls.Add(this.chkIncludeInactive);
            this.Controls.Add(this.dgvDrugNames);
            this.Controls.Add(this.LblDrugName);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmDrugNames";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Drug Names";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmDrugNames_FormClosed);
            this.Load += new System.EventHandler(this.FrmDrugNames_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDrugNames)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbNew;
        private System.Windows.Forms.ToolStripButton tsbEdit;
        private System.Windows.Forms.ToolStripButton tsbArchive;
        private System.Windows.Forms.Label LblDrugName;
        private System.Windows.Forms.ToolStripTextBox txtSearchKeyword;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnSearch;
        private System.Windows.Forms.ToolStripLabel lblSearch;
        private System.Windows.Forms.ToolStripButton btnShowAll;
        private System.Windows.Forms.CheckBox chkIncludeInactive;
        private System.Windows.Forms.DataGridView dgvDrugNames;
        private System.Windows.Forms.DataGridViewTextBoxColumn DrugNameId;
        private System.Windows.Forms.DataGridViewTextBoxColumn DrugNameValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn DrugCodeValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn HaveUA;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsUA;
        private System.Windows.Forms.DataGridViewTextBoxColumn HaveHair;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsHair;
        private System.Windows.Forms.DataGridViewTextBoxColumn UAScreenValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn UAConfirmationValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn HairScreenValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn HairConfirmationValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn UnitOfMeasurement;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsActive;
    }
}