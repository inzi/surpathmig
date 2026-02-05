using Serilog;

namespace SurPath
{
    partial class FrmClientDepartmentInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmClientDepartmentInfo));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbNew = new System.Windows.Forms.ToolStripButton();
            this.tsbEdit = new System.Windows.Forms.ToolStripButton();
            this.tsbArchive = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lblSearch = new System.Windows.Forms.ToolStripLabel();
            this.txtSearchKeyword = new System.Windows.Forms.ToolStripTextBox();
            this.btnSearch = new System.Windows.Forms.ToolStripButton();
            this.btnShowAll = new System.Windows.Forms.ToolStripButton();
            this.lblClientDepartmentInfo = new System.Windows.Forms.Label();
            this.dgvClientDepartment = new System.Windows.Forms.DataGridView();
            this.lblClientName = new System.Windows.Forms.Label();
            this.lblClientNameHeader = new System.Windows.Forms.Label();
            this.chkIncludeInactive = new System.Windows.Forms.CheckBox();
            this.ClientDepartmentId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClientId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DepartmentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LabCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QuestCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Hair = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DNA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MROType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsUA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsHair = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsDNA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsRC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsBC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MROTypeId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PaymentTypeId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PaymentType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsDepartmentActive = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientDepartment)).BeginInit();
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
            this.toolStrip1.Size = new System.Drawing.Size(1304, 27);
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
            this.lblSearch.Size = new System.Drawing.Size(42, 24);
            this.lblSearch.Text = "&Search";
            // 
            // txtSearchKeyword
            // 
            this.txtSearchKeyword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearchKeyword.Name = "txtSearchKeyword";
            this.txtSearchKeyword.Size = new System.Drawing.Size(200, 27);
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
            // lblClientDepartmentInfo
            // 
            this.lblClientDepartmentInfo.AutoSize = true;
            this.lblClientDepartmentInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClientDepartmentInfo.Location = new System.Drawing.Point(13, 28);
            this.lblClientDepartmentInfo.Name = "lblClientDepartmentInfo";
            this.lblClientDepartmentInfo.Size = new System.Drawing.Size(192, 20);
            this.lblClientDepartmentInfo.TabIndex = 1;
            this.lblClientDepartmentInfo.Text = "Client Department Info";
            // 
            // dgvClientDepartment
            // 
            this.dgvClientDepartment.AllowUserToAddRows = false;
            this.dgvClientDepartment.AllowUserToDeleteRows = false;
            this.dgvClientDepartment.AllowUserToOrderColumns = true;
            this.dgvClientDepartment.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvClientDepartment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvClientDepartment.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ClientDepartmentId,
            this.ClientId,
            this.DepartmentName,
            this.LabCode,
            this.QuestCode,
            this.UA,
            this.Hair,
            this.DNA,
            this.BC,
            this.RC,
            this.MROType,
            this.IsUA,
            this.IsHair,
            this.IsDNA,
            this.IsRC,
            this.IsBC,
            this.MROTypeId,
            this.PaymentTypeId,
            this.PaymentType,
            this.Status,
            this.IsDepartmentActive});
            this.dgvClientDepartment.Location = new System.Drawing.Point(17, 97);
            this.dgvClientDepartment.MultiSelect = false;
            this.dgvClientDepartment.Name = "dgvClientDepartment";
            this.dgvClientDepartment.ReadOnly = true;
            this.dgvClientDepartment.RowHeadersWidth = 25;
            this.dgvClientDepartment.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvClientDepartment.Size = new System.Drawing.Size(1268, 326);
            this.dgvClientDepartment.TabIndex = 5;
            this.dgvClientDepartment.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvClientDepartment_CellDoubleClick);
            this.dgvClientDepartment.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvClientDepartment_ColumnHeaderMouseClick);
            this.dgvClientDepartment.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvClientDepartment_DataBindingComplete);
            this.dgvClientDepartment.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvClientDepartment_KeyDown);
            // 
            // lblClientName
            // 
            this.lblClientName.AutoSize = true;
            this.lblClientName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClientName.ForeColor = System.Drawing.Color.Maroon;
            this.lblClientName.Location = new System.Drawing.Point(118, 63);
            this.lblClientName.Name = "lblClientName";
            this.lblClientName.Size = new System.Drawing.Size(75, 13);
            this.lblClientName.TabIndex = 3;
            this.lblClientName.Text = "Client Name";
            // 
            // lblClientNameHeader
            // 
            this.lblClientNameHeader.AutoSize = true;
            this.lblClientNameHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClientNameHeader.Location = new System.Drawing.Point(14, 63);
            this.lblClientNameHeader.Name = "lblClientNameHeader";
            this.lblClientNameHeader.Size = new System.Drawing.Size(75, 13);
            this.lblClientNameHeader.TabIndex = 2;
            this.lblClientNameHeader.Text = "Client Name";
            // 
            // chkIncludeInactive
            // 
            this.chkIncludeInactive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkIncludeInactive.AutoSize = true;
            this.chkIncludeInactive.Location = new System.Drawing.Point(1183, 74);
            this.chkIncludeInactive.Name = "chkIncludeInactive";
            this.chkIncludeInactive.Size = new System.Drawing.Size(102, 17);
            this.chkIncludeInactive.TabIndex = 4;
            this.chkIncludeInactive.Text = "Include Inactive";
            this.chkIncludeInactive.UseVisualStyleBackColor = true;
            this.chkIncludeInactive.CheckedChanged += new System.EventHandler(this.chkIncludeInactive_CheckedChanged);
            // 
            // ClientDepartmentId
            // 
            this.ClientDepartmentId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ClientDepartmentId.DataPropertyName = "ClientDepartmentId";
            this.ClientDepartmentId.HeaderText = "Client Department Id";
            this.ClientDepartmentId.Name = "ClientDepartmentId";
            this.ClientDepartmentId.ReadOnly = true;
            this.ClientDepartmentId.Visible = false;
            // 
            // ClientId
            // 
            this.ClientId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ClientId.DataPropertyName = "ClientId";
            this.ClientId.HeaderText = "Client Id";
            this.ClientId.Name = "ClientId";
            this.ClientId.ReadOnly = true;
            this.ClientId.Visible = false;
            // 
            // DepartmentName
            // 
            this.DepartmentName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DepartmentName.DataPropertyName = "DepartmentName";
            this.DepartmentName.HeaderText = "Department Name";
            this.DepartmentName.Name = "DepartmentName";
            this.DepartmentName.ReadOnly = true;
            // 
            // LabCode
            // 
            this.LabCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.LabCode.DataPropertyName = "LabCode";
            this.LabCode.HeaderText = "Lab Code";
            this.LabCode.Name = "LabCode";
            this.LabCode.ReadOnly = true;
            // 
            // QuestCode
            // 
            this.QuestCode.DataPropertyName = "QuestCode";
            this.QuestCode.HeaderText = "QuestCode";
            this.QuestCode.Name = "QuestCode";
            this.QuestCode.ReadOnly = true;
            // 
            // UA
            // 
            this.UA.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.UA.HeaderText = "UA";
            this.UA.Name = "UA";
            this.UA.ReadOnly = true;
            // 
            // Hair
            // 
            this.Hair.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Hair.HeaderText = "Hair";
            this.Hair.Name = "Hair";
            this.Hair.ReadOnly = true;
            // 
            // DNA
            // 
            this.DNA.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DNA.HeaderText = "DNA";
            this.DNA.Name = "DNA";
            this.DNA.ReadOnly = true;
            // 
            // BC
            // 
            this.BC.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.BC.HeaderText = "BC";
            this.BC.Name = "BC";
            this.BC.ReadOnly = true;
            this.BC.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // RC
            // 
            this.RC.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.RC.HeaderText = "RC";
            this.RC.Name = "RC";
            this.RC.ReadOnly = true;
            // 
            // MROType
            // 
            this.MROType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.MROType.HeaderText = "MRO Type";
            this.MROType.Name = "MROType";
            this.MROType.ReadOnly = true;
            // 
            // IsUA
            // 
            this.IsUA.DataPropertyName = "IsUA";
            this.IsUA.HeaderText = "IsUA";
            this.IsUA.Name = "IsUA";
            this.IsUA.ReadOnly = true;
            this.IsUA.Visible = false;
            // 
            // IsHair
            // 
            this.IsHair.DataPropertyName = "IsHair";
            this.IsHair.HeaderText = "IsHair";
            this.IsHair.Name = "IsHair";
            this.IsHair.ReadOnly = true;
            this.IsHair.Visible = false;
            // 
            // IsDNA
            // 
            this.IsDNA.DataPropertyName = "IsDNA";
            this.IsDNA.HeaderText = "IsDNA";
            this.IsDNA.Name = "IsDNA";
            this.IsDNA.ReadOnly = true;
            this.IsDNA.Visible = false;
            // 
            // IsRC
            // 
            this.IsRC.DataPropertyName = "IsRecordKeeping";
            this.IsRC.HeaderText = "IsRC";
            this.IsRC.Name = "IsRC";
            this.IsRC.ReadOnly = true;
            this.IsRC.Visible = false;
            // 
            // IsBC
            // 
            this.IsBC.DataPropertyName = "IsBC";
            this.IsBC.HeaderText = "IsBC";
            this.IsBC.Name = "IsBC";
            this.IsBC.ReadOnly = true;
            this.IsBC.Visible = false;
            // 
            // MROTypeId
            // 
            this.MROTypeId.DataPropertyName = "MROTypeId";
            this.MROTypeId.HeaderText = "MROTypeId";
            this.MROTypeId.Name = "MROTypeId";
            this.MROTypeId.ReadOnly = true;
            this.MROTypeId.Visible = false;
            // 
            // PaymentTypeId
            // 
            this.PaymentTypeId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PaymentTypeId.DataPropertyName = "PaymentTypeId";
            this.PaymentTypeId.HeaderText = "PaymentTypeId";
            this.PaymentTypeId.Name = "PaymentTypeId";
            this.PaymentTypeId.ReadOnly = true;
            this.PaymentTypeId.Visible = false;
            // 
            // PaymentType
            // 
            this.PaymentType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PaymentType.HeaderText = "Payment Type";
            this.PaymentType.Name = "PaymentType";
            this.PaymentType.ReadOnly = true;
            // 
            // Status
            // 
            this.Status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            // 
            // IsDepartmentActive
            // 
            this.IsDepartmentActive.DataPropertyName = "IsDepartmentActive";
            this.IsDepartmentActive.HeaderText = "Status";
            this.IsDepartmentActive.Name = "IsDepartmentActive";
            this.IsDepartmentActive.ReadOnly = true;
            this.IsDepartmentActive.Visible = false;
            // 
            // FrmClientDepartmentInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1304, 449);
            this.Controls.Add(this.chkIncludeInactive);
            this.Controls.Add(this.lblClientName);
            this.Controls.Add(this.lblClientNameHeader);
            this.Controls.Add(this.dgvClientDepartment);
            this.Controls.Add(this.lblClientDepartmentInfo);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmClientDepartmentInfo";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Client Department Info";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmClientDepartmentInfo_FormClosed);
            this.Load += new System.EventHandler(this.FrmClientDepartmentInfo_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientDepartment)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbNew;
        private System.Windows.Forms.ToolStripButton tsbEdit;
        private System.Windows.Forms.ToolStripButton tsbArchive;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel lblSearch;
        private System.Windows.Forms.ToolStripTextBox txtSearchKeyword;
        private System.Windows.Forms.ToolStripButton btnSearch;
        private System.Windows.Forms.Label lblClientDepartmentInfo;
        private System.Windows.Forms.DataGridView dgvClientDepartment;
        private System.Windows.Forms.Label lblClientName;
        private System.Windows.Forms.Label lblClientNameHeader;
        private System.Windows.Forms.ToolStripButton btnShowAll;
        private System.Windows.Forms.CheckBox chkIncludeInactive;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClientDepartmentId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClientId;
        private System.Windows.Forms.DataGridViewTextBoxColumn DepartmentName;
        private System.Windows.Forms.DataGridViewTextBoxColumn LabCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn QuestCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn UA;
        private System.Windows.Forms.DataGridViewTextBoxColumn Hair;
        private System.Windows.Forms.DataGridViewTextBoxColumn DNA;
        private System.Windows.Forms.DataGridViewTextBoxColumn BC;
        private System.Windows.Forms.DataGridViewTextBoxColumn RC;
        private System.Windows.Forms.DataGridViewTextBoxColumn MROType;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsUA;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsHair;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsDNA;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsRC;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsBC;
        private System.Windows.Forms.DataGridViewTextBoxColumn MROTypeId;
        private System.Windows.Forms.DataGridViewTextBoxColumn PaymentTypeId;
        private System.Windows.Forms.DataGridViewTextBoxColumn PaymentType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsDepartmentActive;
    }
}