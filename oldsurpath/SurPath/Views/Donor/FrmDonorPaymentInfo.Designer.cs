namespace SurPath
{
    partial class FrmDonorPaymentInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDonorPaymentInfo));
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.lblFromDate = new System.Windows.Forms.Label();
            this.lblToDate = new System.Windows.Forms.Label();
            this.Mode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FirstName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DonorId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDonorPaymentInfo = new System.Windows.Forms.DataGridView();
            this.txtToDate = new System.Windows.Forms.DateTimePicker();
            this.txtFromDate = new System.Windows.Forms.DateTimePicker();
            this.cmbPaymentMethod = new System.Windows.Forms.ComboBox();
            this.lblPaymentMethod = new System.Windows.Forms.Label();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDonorPaymentInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Image = global::SurPath.Properties.Resources.Search_Small;
            this.btnSearch.Location = new System.Drawing.Point(439, 10);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(28, 25);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.TextChanged += new System.EventHandler(this.btnSearch_TextChanged);
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(474, 11);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(56, 25);
            this.btnExport.TabIndex = 7;
            this.btnExport.Text = "Print";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.TextChanged += new System.EventHandler(this.btnExport_TextChanged);
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // lblFromDate
            // 
            this.lblFromDate.AutoSize = true;
            this.lblFromDate.Location = new System.Drawing.Point(12, 16);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(53, 13);
            this.lblFromDate.TabIndex = 0;
            this.lblFromDate.Text = "&FromDate";
            // 
            // lblToDate
            // 
            this.lblToDate.AutoSize = true;
            this.lblToDate.Location = new System.Drawing.Point(178, 16);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(43, 13);
            this.lblToDate.TabIndex = 2;
            this.lblToDate.Text = "&ToDate";
            // 
            // Mode
            // 
            this.Mode.DataPropertyName = "PaymentMethodId";
            this.Mode.HeaderText = "Mode";
            this.Mode.Name = "Mode";
            this.Mode.ReadOnly = true;
            // 
            // Amount
            // 
            this.Amount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Amount.DataPropertyName = "TotalPaymentAmount";
            this.Amount.HeaderText = "Amount";
            this.Amount.Name = "Amount";
            this.Amount.ReadOnly = true;
            // 
            // LastName
            // 
            this.LastName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.LastName.DataPropertyName = "DonorLastName";
            this.LastName.HeaderText = "Last Name";
            this.LastName.Name = "LastName";
            this.LastName.ReadOnly = true;
            // 
            // FirstName
            // 
            this.FirstName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.FirstName.DataPropertyName = "DonorFirstName";
            this.FirstName.HeaderText = "First Name";
            this.FirstName.Name = "FirstName";
            this.FirstName.ReadOnly = true;
            // 
            // DonorId
            // 
            this.DonorId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DonorId.DataPropertyName = "DonorId";
            this.DonorId.HeaderText = "DonorId";
            this.DonorId.Name = "DonorId";
            this.DonorId.ReadOnly = true;
            this.DonorId.Visible = false;
            // 
            // dgvDonorPaymentInfo
            // 
            this.dgvDonorPaymentInfo.AllowUserToAddRows = false;
            this.dgvDonorPaymentInfo.AllowUserToDeleteRows = false;
            this.dgvDonorPaymentInfo.AllowUserToOrderColumns = true;
            this.dgvDonorPaymentInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDonorPaymentInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDonorPaymentInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DonorId,
            this.FirstName,
            this.LastName,
            this.Amount,
            this.Mode});
            this.dgvDonorPaymentInfo.Location = new System.Drawing.Point(12, 55);
            this.dgvDonorPaymentInfo.MultiSelect = false;
            this.dgvDonorPaymentInfo.Name = "dgvDonorPaymentInfo";
            this.dgvDonorPaymentInfo.ReadOnly = true;
            this.dgvDonorPaymentInfo.RowHeadersWidth = 25;
            this.dgvDonorPaymentInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDonorPaymentInfo.Size = new System.Drawing.Size(950, 546);
            this.dgvDonorPaymentInfo.TabIndex = 8;
            this.dgvDonorPaymentInfo.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvDonorPaymentInfo_DataBindingComplete);
            // 
            // txtToDate
            // 
            this.txtToDate.CustomFormat = "MM/dd/yyyy";
            this.txtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.txtToDate.Location = new System.Drawing.Point(227, 12);
            this.txtToDate.Name = "txtToDate";
            this.txtToDate.Size = new System.Drawing.Size(105, 20);
            this.txtToDate.TabIndex = 3;
            this.txtToDate.ValueChanged += new System.EventHandler(this.txtToDate_ValueChanged);
            // 
            // txtFromDate
            // 
            this.txtFromDate.CustomFormat = "MM/dd/yyyy";
            this.txtFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.txtFromDate.Location = new System.Drawing.Point(67, 12);
            this.txtFromDate.Name = "txtFromDate";
            this.txtFromDate.Size = new System.Drawing.Size(105, 20);
            this.txtFromDate.TabIndex = 1;
            this.txtFromDate.ValueChanged += new System.EventHandler(this.txtFromDate_ValueChanged);
            // 
            // cmbPaymentMethod
            // 
            this.cmbPaymentMethod.DropDownHeight = 65;
            this.cmbPaymentMethod.FormattingEnabled = true;
            this.cmbPaymentMethod.IntegralHeight = false;
            this.cmbPaymentMethod.Items.AddRange(new object[] {
            "All",
            "Cash",
            "Card"});
            this.cmbPaymentMethod.Location = new System.Drawing.Point(374, 13);
            this.cmbPaymentMethod.Name = "cmbPaymentMethod";
            this.cmbPaymentMethod.Size = new System.Drawing.Size(59, 21);
            this.cmbPaymentMethod.TabIndex = 5;
            this.cmbPaymentMethod.TextChanged += new System.EventHandler(this.cmbPaymentMethod_TextChanged);
            // 
            // lblPaymentMethod
            // 
            this.lblPaymentMethod.AutoSize = true;
            this.lblPaymentMethod.Location = new System.Drawing.Point(340, 17);
            this.lblPaymentMethod.Name = "lblPaymentMethod";
            this.lblPaymentMethod.Size = new System.Drawing.Size(34, 13);
            this.lblPaymentMethod.TabIndex = 4;
            this.lblPaymentMethod.Text = "&Mode";
            // 
            // printDocument1
            // 
            this.printDocument1.BeginPrint += new System.Drawing.Printing.PrintEventHandler(this.printDocument1_BeginPrint);
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // FrmDonorPaymentInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(974, 632);
            this.Controls.Add(this.cmbPaymentMethod);
            this.Controls.Add(this.lblPaymentMethod);
            this.Controls.Add(this.txtFromDate);
            this.Controls.Add(this.txtToDate);
            this.Controls.Add(this.dgvDonorPaymentInfo);
            this.Controls.Add(this.lblToDate);
            this.Controls.Add(this.lblFromDate);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnSearch);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmDonorPaymentInfo";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Donor Payment Info";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmDonorPaymentInfo_FormClosed);
            this.Load += new System.EventHandler(this.FrmDonorPaymentInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDonorPaymentInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label lblFromDate;
        private System.Windows.Forms.Label lblToDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Mode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FirstName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DonorId;
        private System.Windows.Forms.DataGridView dgvDonorPaymentInfo;
        private System.Windows.Forms.DateTimePicker txtToDate;
        private System.Windows.Forms.DateTimePicker txtFromDate;
        private System.Windows.Forms.ComboBox cmbPaymentMethod;
        private System.Windows.Forms.Label lblPaymentMethod;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.PrintDialog printDialog1;
    }
}