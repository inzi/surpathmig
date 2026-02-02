using SurPath.Business;
using System;
using System.Data;
using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();

#if DEBUG
            this.txtPassword.Text = "Add!son09";
            this.txtUserName.Text = "david";
#endif
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowPassword.Checked == true)
            {
                txtPassword.PasswordChar = '\0';
            }
            else
            {
                txtPassword.PasswordChar = '*';
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (txtUserName.Text.Trim() == string.Empty)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    MessageBox.Show("Username cannot be empty.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtUserName.Focus();
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (txtPassword.Text.Trim() == string.Empty)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    MessageBox.Show("Password cannot be empty.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPassword.Focus();
                    Cursor.Current = Cursors.Default;
                    return;
                }

                UserAuthentication userAuth = new UserAuthentication();
                var _val = userAuth.ValidateUser(txtUserName.Text.Trim(), txtPassword.Text);
                bool IsValidated = _val.Item1;
                int user_id = _val.Item2;
                int donor_id = _val.Item3;
                if (IsValidated)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    if (txtUserName.Text != string.Empty)
                    {
                        DonorBL donorBL = new DonorBL();
                        DataTable dtInActiveUser = donorBL.GetInActiveUserWithPW(txtUserName.Text.Trim(), userAuth.EncryptedPW(txtPassword.Text));
                        if (dtInActiveUser.Rows.Count != 0)
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            MessageBox.Show("Account is inactive,hence contact the administrator", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            Program.currentUserName = txtUserName.Text.Trim();
                            this.DialogResult = DialogResult.OK;
                        }
                    }

                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    Cursor.Current = Cursors.WaitCursor;
                    MessageBox.Show("Invalid login credential.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Program.LogError(ex);
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                this.DialogResult = DialogResult.Cancel;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void lblForgotPassword_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                FrmForgotPassword frmForgotPassword = new FrmForgotPassword();
                frmForgotPassword.ShowDialog();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }
    }
}