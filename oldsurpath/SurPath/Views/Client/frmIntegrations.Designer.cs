namespace SurPath
{
    partial class FrmIntegrations
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
            this.gbIntegrations = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rdoRequireNoLogin = new System.Windows.Forms.RadioButton();
            this.rdoRequireRemoteLogin = new System.Windows.Forms.RadioButton();
            this.rdoRequireLogin = new System.Windows.Forms.RadioButton();
            this.txtClientCode = new SurPath.Controls.TextBoxes.SurTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblClientCodeMan = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblPushFolder = new System.Windows.Forms.Label();
            this.txtPushFolder = new SurPath.Controls.TextBoxes.SurTextBox();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbIntegrations
            // 
            this.gbIntegrations.Location = new System.Drawing.Point(26, 143);
            this.gbIntegrations.Name = "gbIntegrations";
            this.gbIntegrations.Size = new System.Drawing.Size(332, 258);
            this.gbIntegrations.TabIndex = 163;
            this.gbIntegrations.TabStop = false;
            this.gbIntegrations.Text = "Integration Partners";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rdoRequireNoLogin);
            this.groupBox3.Controls.Add(this.rdoRequireRemoteLogin);
            this.groupBox3.Controls.Add(this.rdoRequireLogin);
            this.groupBox3.Location = new System.Drawing.Point(26, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(332, 125);
            this.groupBox3.TabIndex = 162;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Require Login";
            // 
            // rdoRequireNoLogin
            // 
            this.rdoRequireNoLogin.AutoSize = true;
            this.rdoRequireNoLogin.Location = new System.Drawing.Point(16, 19);
            this.rdoRequireNoLogin.Name = "rdoRequireNoLogin";
            this.rdoRequireNoLogin.Size = new System.Drawing.Size(261, 17);
            this.rdoRequireNoLogin.TabIndex = 161;
            this.rdoRequireNoLogin.Text = "Require No Integration Login To Register For Test";
            this.rdoRequireNoLogin.UseVisualStyleBackColor = true;
            // 
            // rdoRequireRemoteLogin
            // 
            this.rdoRequireRemoteLogin.AutoSize = true;
            this.rdoRequireRemoteLogin.Location = new System.Drawing.Point(16, 65);
            this.rdoRequireRemoteLogin.Name = "rdoRequireRemoteLogin";
            this.rdoRequireRemoteLogin.Size = new System.Drawing.Size(297, 17);
            this.rdoRequireRemoteLogin.TabIndex = 160;
            this.rdoRequireRemoteLogin.Text = "Require Donor To Login With Partner to Register For Test";
            this.rdoRequireRemoteLogin.UseVisualStyleBackColor = true;
            // 
            // rdoRequireLogin
            // 
            this.rdoRequireLogin.AutoSize = true;
            this.rdoRequireLogin.Location = new System.Drawing.Point(16, 42);
            this.rdoRequireLogin.Name = "rdoRequireLogin";
            this.rdoRequireLogin.Size = new System.Drawing.Size(239, 17);
            this.rdoRequireLogin.TabIndex = 159;
            this.rdoRequireLogin.Text = "Require Donor To Login To Register For Test";
            this.rdoRequireLogin.UseVisualStyleBackColor = true;
            // 
            // txtClientCode
            // 
            this.txtClientCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtClientCode.ForeColor = System.Drawing.Color.Maroon;
            this.txtClientCode.Location = new System.Drawing.Point(160, 407);
            this.txtClientCode.MaxLength = 35;
            this.txtClientCode.Name = "txtClientCode";
            this.txtClientCode.Size = new System.Drawing.Size(198, 21);
            this.txtClientCode.TabIndex = 164;
            this.txtClientCode.WaterMark = "Enter Client Code";
            this.txtClientCode.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtClientCode.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtClientCode.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 412);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 165;
            this.label1.Text = "Partner Client Code";
            // 
            // lblClientCodeMan
            // 
            this.lblClientCodeMan.AutoSize = true;
            this.lblClientCodeMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClientCodeMan.ForeColor = System.Drawing.Color.Red;
            this.lblClientCodeMan.Location = new System.Drawing.Point(23, 407);
            this.lblClientCodeMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblClientCodeMan.Name = "lblClientCodeMan";
            this.lblClientCodeMan.Size = new System.Drawing.Size(13, 17);
            this.lblClientCodeMan.TabIndex = 166;
            this.lblClientCodeMan.Text = "*";
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(117, 595);
            this.btnClose.Name = "btnClose";
            this.btnClose.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 168;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(26, 595);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 167;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblPushFolder
            // 
            this.lblPushFolder.AutoSize = true;
            this.lblPushFolder.Location = new System.Drawing.Point(39, 439);
            this.lblPushFolder.Name = "lblPushFolder";
            this.lblPushFolder.Size = new System.Drawing.Size(82, 13);
            this.lblPushFolder.TabIndex = 170;
            this.lblPushFolder.Text = "Push SubFolder";
            this.lblPushFolder.Visible = false;
            this.lblPushFolder.Click += new System.EventHandler(this.label2_Click);
            // 
            // txtPushFolder
            // 
            this.txtPushFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPushFolder.ForeColor = System.Drawing.Color.Maroon;
            this.txtPushFolder.Location = new System.Drawing.Point(160, 434);
            this.txtPushFolder.MaxLength = 35;
            this.txtPushFolder.Name = "txtPushFolder";
            this.txtPushFolder.Size = new System.Drawing.Size(198, 21);
            this.txtPushFolder.TabIndex = 169;
            this.txtPushFolder.Visible = false;
            this.txtPushFolder.WaterMark = "Enter Client Code";
            this.txtPushFolder.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtPushFolder.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPushFolder.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtPushFolder.TextChanged += new System.EventHandler(this.surTextBox1_TextChanged);
            // 
            // FrmIntegrations
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(948, 645);
            this.Controls.Add(this.lblPushFolder);
            this.Controls.Add(this.txtPushFolder);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblClientCodeMan);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtClientCode);
            this.Controls.Add(this.gbIntegrations);
            this.Controls.Add(this.groupBox3);
            this.Name = "FrmIntegrations";
            this.ShowIcon = false;
            this.Text = "Partner Integrations";
            this.Load += new System.EventHandler(this.frmIntegrations_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbIntegrations;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rdoRequireNoLogin;
        private System.Windows.Forms.RadioButton rdoRequireRemoteLogin;
        private System.Windows.Forms.RadioButton rdoRequireLogin;
        private Controls.TextBoxes.SurTextBox txtClientCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblClientCodeMan;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblPushFolder;
        private Controls.TextBoxes.SurTextBox txtPushFolder;
    }
}