namespace SurPath
{
    partial class FrmDisplayColumns
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDisplayColumns));
            this.lblExportAs = new System.Windows.Forms.Label();
            this.rbExcel = new System.Windows.Forms.RadioButton();
            this.rbCSV = new System.Windows.Forms.RadioButton();
            this.rbPDF = new System.Windows.Forms.RadioButton();
            this.rbWord = new System.Windows.Forms.RadioButton();
            this.lblColumnList = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.chkColumnList = new System.Windows.Forms.CheckedListBox();
            this.gvFieldList = new System.Windows.Forms.DataGridView();
            this.fbdExport = new System.Windows.Forms.FolderBrowserDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.rbSelect = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.gvFieldList)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblExportAs
            // 
            this.lblExportAs.AutoSize = true;
            this.lblExportAs.Location = new System.Drawing.Point(8, 240);
            this.lblExportAs.Name = "lblExportAs";
            this.lblExportAs.Size = new System.Drawing.Size(52, 13);
            this.lblExportAs.TabIndex = 4;
            this.lblExportAs.Text = "Export &As";
            // 
            // rbExcel
            // 
            this.rbExcel.AutoSize = true;
            this.rbExcel.Location = new System.Drawing.Point(80, 238);
            this.rbExcel.Name = "rbExcel";
            this.rbExcel.Size = new System.Drawing.Size(51, 17);
            this.rbExcel.TabIndex = 5;
            this.rbExcel.TabStop = true;
            this.rbExcel.Text = "Excel";
            this.rbExcel.UseVisualStyleBackColor = true;
            this.rbExcel.CheckedChanged += new System.EventHandler(this.rbExcel_CheckedChanged);
            // 
            // rbCSV
            // 
            this.rbCSV.AutoSize = true;
            this.rbCSV.Location = new System.Drawing.Point(138, 238);
            this.rbCSV.Name = "rbCSV";
            this.rbCSV.Size = new System.Drawing.Size(46, 17);
            this.rbCSV.TabIndex = 6;
            this.rbCSV.TabStop = true;
            this.rbCSV.Text = "CSV";
            this.rbCSV.UseVisualStyleBackColor = true;
            this.rbCSV.CheckedChanged += new System.EventHandler(this.rbCSV_CheckedChanged);
            // 
            // rbPDF
            // 
            this.rbPDF.AutoSize = true;
            this.rbPDF.Location = new System.Drawing.Point(188, 238);
            this.rbPDF.Name = "rbPDF";
            this.rbPDF.Size = new System.Drawing.Size(46, 17);
            this.rbPDF.TabIndex = 7;
            this.rbPDF.TabStop = true;
            this.rbPDF.Text = "PDF";
            this.rbPDF.UseVisualStyleBackColor = true;
            this.rbPDF.CheckedChanged += new System.EventHandler(this.rbPDF_CheckedChanged);
            // 
            // rbWord
            // 
            this.rbWord.AutoSize = true;
            this.rbWord.Location = new System.Drawing.Point(238, 238);
            this.rbWord.Name = "rbWord";
            this.rbWord.Size = new System.Drawing.Size(51, 17);
            this.rbWord.TabIndex = 8;
            this.rbWord.TabStop = true;
            this.rbWord.Text = "Word";
            this.rbWord.UseVisualStyleBackColor = true;
            this.rbWord.CheckedChanged += new System.EventHandler(this.rbWord_CheckedChanged);
            // 
            // lblColumnList
            // 
            this.lblColumnList.AutoSize = true;
            this.lblColumnList.Location = new System.Drawing.Point(8, 16);
            this.lblColumnList.Name = "lblColumnList";
            this.lblColumnList.Size = new System.Drawing.Size(61, 13);
            this.lblColumnList.TabIndex = 0;
            this.lblColumnList.Text = "&Column List";
            // 
            // btnOk
            // 
            this.btnOk.AutoSize = true;
            this.btnOk.Location = new System.Drawing.Point(70, 286);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.TextChanged += new System.EventHandler(this.btnOk_TextChanged);
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnClose
            // 
            this.btnClose.AutoSize = true;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(152, 286);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.TextChanged += new System.EventHandler(this.btnClose_TextChanged);
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.AutoSize = true;
            this.btnBrowse.Location = new System.Drawing.Point(206, 142);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(64, 23);
            this.btnBrowse.TabIndex = 12;
            this.btnBrowse.Text = "&browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Visible = false;
            this.btnBrowse.TextChanged += new System.EventHandler(this.btnBrowse_TextChanged);
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // chkColumnList
            // 
            this.chkColumnList.CheckOnClick = true;
            this.chkColumnList.Enabled = false;
            this.chkColumnList.FormattingEnabled = true;
            this.chkColumnList.Items.AddRange(new object[] {
            "First Name",
            "Last Name",
            "SSN",
            "DOB",
            "Specimen ID",
            "Specimen Date",
            "Client",
            "Department",
            "Status",
            "Payment Mode",
            "Amount",
            "Result",
            "Test Reason",
            "Donor City",
            "Zip Code",
            "MRO Type",
            "Payment Type"});
            this.chkColumnList.Location = new System.Drawing.Point(83, 16);
            this.chkColumnList.Name = "chkColumnList";
            this.chkColumnList.Size = new System.Drawing.Size(205, 169);
            this.chkColumnList.TabIndex = 15;
            this.chkColumnList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chkColumnList_ItemCheck);
            this.chkColumnList.SelectedIndexChanged += new System.EventHandler(this.chkColumnList_SelectedIndexChanged);
            // 
            // gvFieldList
            // 
            this.gvFieldList.AllowUserToAddRows = false;
            this.gvFieldList.AllowUserToDeleteRows = false;
            this.gvFieldList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvFieldList.Location = new System.Drawing.Point(84, 16);
            this.gvFieldList.MultiSelect = false;
            this.gvFieldList.Name = "gvFieldList";
            this.gvFieldList.ReadOnly = true;
            this.gvFieldList.RowHeadersVisible = false;
            this.gvFieldList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvFieldList.Size = new System.Drawing.Size(205, 169);
            this.gvFieldList.TabIndex = 2;
            this.gvFieldList.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 202);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "&Export";
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Checked = true;
            this.rbAll.Location = new System.Drawing.Point(3, 2);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(36, 17);
            this.rbAll.TabIndex = 0;
            this.rbAll.TabStop = true;
            this.rbAll.Text = "All";
            this.rbAll.UseVisualStyleBackColor = true;
            this.rbAll.CheckedChanged += new System.EventHandler(this.rbtAll_CheckedChanged);
            this.rbAll.Click += new System.EventHandler(this.rbAll_Click);
            // 
            // rbSelect
            // 
            this.rbSelect.AutoSize = true;
            this.rbSelect.Location = new System.Drawing.Point(59, 3);
            this.rbSelect.Name = "rbSelect";
            this.rbSelect.Size = new System.Drawing.Size(67, 17);
            this.rbSelect.TabIndex = 1;
            this.rbSelect.Text = "Selected";
            this.rbSelect.UseVisualStyleBackColor = true;
            this.rbSelect.CheckedChanged += new System.EventHandler(this.rbtSelection_CheckedChanged);
            this.rbSelect.Click += new System.EventHandler(this.rbSelect_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbAll);
            this.panel1.Controls.Add(this.rbSelect);
            this.panel1.Location = new System.Drawing.Point(77, 196);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(139, 21);
            this.panel1.TabIndex = 3;
            // 
            // FrmDisplayColumns
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(306, 321);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblColumnList);
            this.Controls.Add(this.rbWord);
            this.Controls.Add(this.rbPDF);
            this.Controls.Add(this.rbCSV);
            this.Controls.Add(this.rbExcel);
            this.Controls.Add(this.lblExportAs);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.chkColumnList);
            this.Controls.Add(this.gvFieldList);
            this.Controls.Add(this.btnBrowse);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDisplayColumns";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Export Columns";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmDisplayColumns_FormClosed);
            this.Load += new System.EventHandler(this.FrmDisplayColumns_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gvFieldList)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblExportAs;
        private System.Windows.Forms.RadioButton rbExcel;
        private System.Windows.Forms.RadioButton rbCSV;
        private System.Windows.Forms.RadioButton rbPDF;
        private System.Windows.Forms.RadioButton rbWord;
        private System.Windows.Forms.Label lblColumnList;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.CheckedListBox chkColumnList;
        private System.Windows.Forms.DataGridView gvFieldList;
        private System.Windows.Forms.FolderBrowserDialog fbdExport;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbAll;
        private System.Windows.Forms.RadioButton rbSelect;
        private System.Windows.Forms.Panel panel1;
    }
}