using System.Windows.Forms;

namespace SurPath
{
    partial class FrmPartners : Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPartners));
            this.chkIncludeInactive = new System.Windows.Forms.CheckBox();
            this.LbPartners = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbNew = new System.Windows.Forms.ToolStripButton();
            this.dgvPartners = new System.Windows.Forms.DataGridView();
            this.partner_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.partner_key = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.partner_crypto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.created_on = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.last_modified_by = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.last_modified_on = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.active = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPartners)).BeginInit();
            this.SuspendLayout();
            // 
            // chkIncludeInactive
            // 
            this.chkIncludeInactive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkIncludeInactive.AutoSize = true;
            this.chkIncludeInactive.Location = new System.Drawing.Point(897, 31);
            this.chkIncludeInactive.Name = "chkIncludeInactive";
            this.chkIncludeInactive.Size = new System.Drawing.Size(102, 17);
            this.chkIncludeInactive.TabIndex = 10;
            this.chkIncludeInactive.Text = "Include Inactive";
            this.chkIncludeInactive.UseVisualStyleBackColor = true;
            this.chkIncludeInactive.CheckedChanged += new System.EventHandler(this.chkIncludeInactive_CheckedChanged);
            // 
            // LbPartners
            // 
            this.LbPartners.AutoSize = true;
            this.LbPartners.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbPartners.Location = new System.Drawing.Point(12, 28);
            this.LbPartners.Name = "LbPartners";
            this.LbPartners.Size = new System.Drawing.Size(105, 20);
            this.LbPartners.TabIndex = 9;
            this.LbPartners.Text = "Partner Info";
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNew});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1022, 27);
            this.toolStrip1.TabIndex = 8;
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
            // dgvPartners
            // 
            this.dgvPartners.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPartners.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPartners.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.partner_name,
            this.partner_key,
            this.partner_crypto,
            this.created_on,
            this.last_modified_by,
            this.last_modified_on,
            this.active});
            this.dgvPartners.Location = new System.Drawing.Point(16, 60);
            this.dgvPartners.Name = "dgvPartners";
            this.dgvPartners.RowTemplate.Height = 24;
            this.dgvPartners.Size = new System.Drawing.Size(990, 592);
            this.dgvPartners.TabIndex = 11;
            this.dgvPartners.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvPartners_CellMouseDoubleClick);
            this.dgvPartners.ColumnDisplayIndexChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgvPartners_ColumnDisplayIndexChanged);
            this.dgvPartners.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvPartners_DataBindingComplete);
            // 
            // partner_name
            // 
            this.partner_name.DataPropertyName = "partner_name";
            this.partner_name.HeaderText = "Name";
            this.partner_name.Name = "partner_name";
            // 
            // partner_key
            // 
            this.partner_key.DataPropertyName = "partner_key";
            this.partner_key.HeaderText = "Key";
            this.partner_key.Name = "partner_key";
            // 
            // partner_crypto
            // 
            this.partner_crypto.DataPropertyName = "partner_crypto";
            this.partner_crypto.HeaderText = "Crypto";
            this.partner_crypto.Name = "partner_crypto";
            // 
            // created_on
            // 
            this.created_on.DataPropertyName = "created_on";
            this.created_on.HeaderText = "Created On";
            this.created_on.Name = "created_on";
            // 
            // last_modified_by
            // 
            this.last_modified_by.DataPropertyName = "last_modified_by";
            this.last_modified_by.HeaderText = "Last Modified By";
            this.last_modified_by.Name = "last_modified_by";
            // 
            // last_modified_on
            // 
            this.last_modified_on.DataPropertyName = "last_modified_on";
            this.last_modified_on.HeaderText = "Last Modified On";
            this.last_modified_on.Name = "last_modified_on";
            // 
            // active
            // 
            this.active.DataPropertyName = "active";
            this.active.HeaderText = "Active";
            this.active.Name = "active";
            // 
            // FrmPartners
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1022, 672);
            this.Controls.Add(this.dgvPartners);
            this.Controls.Add(this.chkIncludeInactive);
            this.Controls.Add(this.LbPartners);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmPartners";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Partners";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmPartners_FormClosed);
            this.Load += new System.EventHandler(this.frmPartners_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPartners)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkIncludeInactive;
        private System.Windows.Forms.Label LbPartners;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbNew;
        private DataGridView dgvPartners;
        private DataGridViewTextBoxColumn partner_name;
        private DataGridViewTextBoxColumn partner_key;
        private DataGridViewTextBoxColumn partner_crypto;
        private DataGridViewTextBoxColumn created_on;
        private DataGridViewTextBoxColumn last_modified_by;
        private DataGridViewTextBoxColumn last_modified_on;
        private DataGridViewTextBoxColumn active;
    }
}