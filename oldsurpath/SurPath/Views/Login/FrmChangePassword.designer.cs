namespace SurPath
{
    partial class FrmChangePassword
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmChangePassword));
            this.lblOldPassword = new System.Windows.Forms.Label();
            this.lblNewPassword = new System.Windows.Forms.Label();
            this.lblConfirmPassword = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtOldPassword = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtNewPassword = new SurPath.Controls.TextBoxes.SurTextBox();
            this.txtConfirmPassword = new SurPath.Controls.TextBoxes.SurTextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lbl1Username = new System.Windows.Forms.Label();
            this.lblPasswordRule1 = new System.Windows.Forms.Label();
            this.lblPasswordRule2 = new System.Windows.Forms.Label();
            this.lblOldPswMan = new System.Windows.Forms.Label();
            this.lblNewPswMan = new System.Windows.Forms.Label();
            this.lblConfirmPswMan = new System.Windows.Forms.Label();
            this.chkConfirmShowPassword = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblOldPassword
            // 
            this.lblOldPassword.AutoSize = true;
            this.lblOldPassword.Location = new System.Drawing.Point(12, 53);
            this.lblOldPassword.Name = "lblOldPassword";
            this.lblOldPassword.Size = new System.Drawing.Size(72, 13);
            this.lblOldPassword.TabIndex = 2;
            this.lblOldPassword.Text = "O&ld Password";
            // 
            // lblNewPassword
            // 
            this.lblNewPassword.AutoSize = true;
            this.lblNewPassword.Location = new System.Drawing.Point(12, 86);
            this.lblNewPassword.Name = "lblNewPassword";
            this.lblNewPassword.Size = new System.Drawing.Size(78, 13);
            this.lblNewPassword.TabIndex = 4;
            this.lblNewPassword.Text = "&New Password";
            // 
            // lblConfirmPassword
            // 
            this.lblConfirmPassword.AutoSize = true;
            this.lblConfirmPassword.Location = new System.Drawing.Point(12, 120);
            this.lblConfirmPassword.Name = "lblConfirmPassword";
            this.lblConfirmPassword.Size = new System.Drawing.Size(91, 13);
            this.lblConfirmPassword.TabIndex = 6;
            this.lblConfirmPassword.Text = "Confir&m Password";
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(187, 223);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 22);
            this.btnClose.TabIndex = 11;
            this.btnClose.Tag = "Close";
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnChangeClose_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(106, 222);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 22);
            this.btnOK.TabIndex = 10;
            this.btnOK.Tag = "OK";
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnChangeOk_Click);
            // 
            // txtOldPassword
            // 
            this.txtOldPassword.Location = new System.Drawing.Point(119, 49);
            this.txtOldPassword.MaxLength = 30;
            this.txtOldPassword.Name = "txtOldPassword";
            this.txtOldPassword.PasswordChar = '*';
            this.txtOldPassword.Size = new System.Drawing.Size(156, 20);
            this.txtOldPassword.TabIndex = 3;
            this.txtOldPassword.WaterMark = "Enter Old Password";
            this.txtOldPassword.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtOldPassword.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOldPassword.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // txtNewPassword
            // 
            this.txtNewPassword.Location = new System.Drawing.Point(119, 82);
            this.txtNewPassword.MaxLength = 30;
            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.PasswordChar = '*';
            this.txtNewPassword.Size = new System.Drawing.Size(156, 20);
            this.txtNewPassword.TabIndex = 5;
            this.txtNewPassword.WaterMark = "Enter New Password";
            this.txtNewPassword.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtNewPassword.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNewPassword.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtNewPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNewPassword_KeyPress);
            // 
            // txtConfirmPassword
            // 
            this.txtConfirmPassword.Location = new System.Drawing.Point(119, 118);
            this.txtConfirmPassword.MaxLength = 30;
            this.txtConfirmPassword.Name = "txtConfirmPassword";
            this.txtConfirmPassword.PasswordChar = '*';
            this.txtConfirmPassword.Size = new System.Drawing.Size(156, 20);
            this.txtConfirmPassword.TabIndex = 7;
            this.txtConfirmPassword.WaterMark = "Enter Confirm Password";
            this.txtConfirmPassword.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.txtConfirmPassword.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConfirmPassword.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.txtConfirmPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtConfirmPassword_KeyPress);
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(12, 22);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(55, 13);
            this.lblUsername.TabIndex = 0;
            this.lblUsername.Text = "&Username";
            // 
            // lbl1Username
            // 
            this.lbl1Username.AutoSize = true;
            this.lbl1Username.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl1Username.Location = new System.Drawing.Point(119, 22);
            this.lbl1Username.Name = "lbl1Username";
            this.lbl1Username.Size = new System.Drawing.Size(63, 13);
            this.lbl1Username.TabIndex = 1;
            this.lbl1Username.Text = "Username";
            // 
            // lblPasswordRule1
            // 
            this.lblPasswordRule1.AutoSize = true;
            this.lblPasswordRule1.ForeColor = System.Drawing.Color.Maroon;
            this.lblPasswordRule1.Location = new System.Drawing.Point(119, 173);
            this.lblPasswordRule1.Name = "lblPasswordRule1";
            this.lblPasswordRule1.Size = new System.Drawing.Size(201, 13);
            this.lblPasswordRule1.TabIndex = 8;
            this.lblPasswordRule1.Text = " Password must be minimum 6 characters";
            // 
            // lblPasswordRule2
            // 
            this.lblPasswordRule2.AutoSize = true;
            this.lblPasswordRule2.ForeColor = System.Drawing.Color.Maroon;
            this.lblPasswordRule2.Location = new System.Drawing.Point(119, 198);
            this.lblPasswordRule2.Name = "lblPasswordRule2";
            this.lblPasswordRule2.Size = new System.Drawing.Size(237, 13);
            this.lblPasswordRule2.TabIndex = 9;
            this.lblPasswordRule2.Text = " Characters Allowed ( A-Z, a-z,0-9, !@#$%=+^~ )";
            // 
            // lblOldPswMan
            // 
            this.lblOldPswMan.AutoSize = true;
            this.lblOldPswMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOldPswMan.ForeColor = System.Drawing.Color.Red;
            this.lblOldPswMan.Location = new System.Drawing.Point(83, 51);
            this.lblOldPswMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblOldPswMan.Name = "lblOldPswMan";
            this.lblOldPswMan.Size = new System.Drawing.Size(13, 17);
            this.lblOldPswMan.TabIndex = 12;
            this.lblOldPswMan.Text = "*";
            // 
            // lblNewPswMan
            // 
            this.lblNewPswMan.AutoSize = true;
            this.lblNewPswMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNewPswMan.ForeColor = System.Drawing.Color.Red;
            this.lblNewPswMan.Location = new System.Drawing.Point(91, 84);
            this.lblNewPswMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNewPswMan.Name = "lblNewPswMan";
            this.lblNewPswMan.Size = new System.Drawing.Size(13, 17);
            this.lblNewPswMan.TabIndex = 13;
            this.lblNewPswMan.Text = "*";
            // 
            // lblConfirmPswMan
            // 
            this.lblConfirmPswMan.AutoSize = true;
            this.lblConfirmPswMan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConfirmPswMan.ForeColor = System.Drawing.Color.Red;
            this.lblConfirmPswMan.Location = new System.Drawing.Point(101, 118);
            this.lblConfirmPswMan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblConfirmPswMan.Name = "lblConfirmPswMan";
            this.lblConfirmPswMan.Size = new System.Drawing.Size(13, 17);
            this.lblConfirmPswMan.TabIndex = 14;
            this.lblConfirmPswMan.Text = "*";
            // 
            // chkConfirmShowPassword
            // 
            this.chkConfirmShowPassword.AutoSize = true;
            this.chkConfirmShowPassword.Location = new System.Drawing.Point(119, 147);
            this.chkConfirmShowPassword.Name = "chkConfirmShowPassword";
            this.chkConfirmShowPassword.Size = new System.Drawing.Size(102, 17);
            this.chkConfirmShowPassword.TabIndex = 16;
            this.chkConfirmShowPassword.Text = "&Show Password";
            this.chkConfirmShowPassword.UseVisualStyleBackColor = true;
            this.chkConfirmShowPassword.CheckedChanged += new System.EventHandler(this.chkConfirmShowPassword_CheckedChanged);
            this.chkConfirmShowPassword.TextChanged += new System.EventHandler(this.chkConfirmShowPassword_TextChanged);
            // 
            // FrmChangePassword
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(368, 259);
            this.Controls.Add(this.chkConfirmShowPassword);
            this.Controls.Add(this.lblConfirmPswMan);
            this.Controls.Add(this.lblNewPswMan);
            this.Controls.Add(this.lblOldPswMan);
            this.Controls.Add(this.lblPasswordRule2);
            this.Controls.Add(this.lblPasswordRule1);
            this.Controls.Add(this.lbl1Username);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtConfirmPassword);
            this.Controls.Add(this.txtNewPassword);
            this.Controls.Add(this.txtOldPassword);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblConfirmPassword);
            this.Controls.Add(this.lblNewPassword);
            this.Controls.Add(this.lblOldPassword);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmChangePassword";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change Password";
            this.Load += new System.EventHandler(this.FrmChangePassword_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblOldPassword;
        private System.Windows.Forms.Label lblNewPassword;
        private System.Windows.Forms.Label lblConfirmPassword;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOK;
        private Controls.TextBoxes.SurTextBox txtOldPassword;
        private Controls.TextBoxes.SurTextBox txtNewPassword;
        private Controls.TextBoxes.SurTextBox txtConfirmPassword;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lbl1Username;
        private System.Windows.Forms.Label lblPasswordRule1;
        private System.Windows.Forms.Label lblPasswordRule2;
        private System.Windows.Forms.Label lblOldPswMan;
        private System.Windows.Forms.Label lblNewPswMan;
        private System.Windows.Forms.Label lblConfirmPswMan;
        private System.Windows.Forms.CheckBox chkConfirmShowPassword;
    }
}