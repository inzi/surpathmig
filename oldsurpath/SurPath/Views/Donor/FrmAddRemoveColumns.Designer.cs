namespace SurPath
{
    partial class FrmAddRemoveColumns
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
            this.lblColumnList = new System.Windows.Forms.Label();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.gvFieldList = new System.Windows.Forms.DataGridView();
            this.FieldSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gvFieldList)).BeginInit();
            this.SuspendLayout();
            // 
            // lblColumnList
            // 
            this.lblColumnList.AutoSize = true;
            this.lblColumnList.Location = new System.Drawing.Point(16, 15);
            this.lblColumnList.Name = "lblColumnList";
            this.lblColumnList.Size = new System.Drawing.Size(61, 13);
            this.lblColumnList.TabIndex = 0;
            this.lblColumnList.Text = "&Column List";
            // 
            // btnRemove
            // 
            this.btnRemove.AutoSize = true;
            this.btnRemove.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnRemove.Location = new System.Drawing.Point(2, 201);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 3;
            this.btnRemove.Text = "&Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Visible = false;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.AutoSize = true;
            this.btnAdd.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAdd.Location = new System.Drawing.Point(2, 172);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "&Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Visible = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // gvFieldList
            // 
            this.gvFieldList.AllowUserToAddRows = false;
            this.gvFieldList.AllowUserToDeleteRows = false;
            this.gvFieldList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvFieldList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FieldSelect,
            this.ColumnId,
            this.ColumnName});
            this.gvFieldList.Location = new System.Drawing.Point(82, 15);
            this.gvFieldList.Name = "gvFieldList";
            this.gvFieldList.ReadOnly = true;
            this.gvFieldList.RowHeadersVisible = false;
            this.gvFieldList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.gvFieldList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvFieldList.Size = new System.Drawing.Size(192, 245);
            this.gvFieldList.TabIndex = 1;
            this.gvFieldList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvFieldList_CellClick);
            this.gvFieldList.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gvFieldList_CellMouseDown);
            this.gvFieldList.DragEnter += new System.Windows.Forms.DragEventHandler(this.gvFieldList_DragEnter);
            this.gvFieldList.DragOver += new System.Windows.Forms.DragEventHandler(this.gvFieldList_DragOver);
            // 
            // FieldSelect
            // 
            this.FieldSelect.HeaderText = "";
            this.FieldSelect.Name = "FieldSelect";
            this.FieldSelect.ReadOnly = true;
            this.FieldSelect.Width = 30;
            // 
            // ColumnId
            // 
            this.ColumnId.DataPropertyName = "ColumnId";
            this.ColumnId.HeaderText = "Id";
            this.ColumnId.Name = "ColumnId";
            this.ColumnId.ReadOnly = true;
            this.ColumnId.Visible = false;
            this.ColumnId.Width = 30;
            // 
            // ColumnName
            // 
            this.ColumnName.DataPropertyName = "ColumnName";
            this.ColumnName.HeaderText = "";
            this.ColumnName.Name = "ColumnName";
            this.ColumnName.ReadOnly = true;
            this.ColumnName.Width = 150;
            // 
            // btnClose
            // 
            this.btnClose.AutoSize = true;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(158, 274);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.TextChanged += new System.EventHandler(this.btnClose_TextChanged);
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.AutoSize = true;
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOk.Location = new System.Drawing.Point(77, 274);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.TextChanged += new System.EventHandler(this.btnOk_TextChanged);
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // FrmAddRemoveColumns
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(311, 334);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gvFieldList);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lblColumnList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAddRemoveColumns";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add/Remove";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmAddRemoveColumns_FormClosing);
            this.Load += new System.EventHandler(this.FrmAddRemoveColumns_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gvFieldList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblColumnList;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridView gvFieldList;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.DataGridViewCheckBoxColumn FieldSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
    }
}