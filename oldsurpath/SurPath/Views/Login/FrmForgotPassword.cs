using SurPath.Business;
using SurPath.Entity;
using System;
using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmForgotPassword : Form
    {
        public FrmForgotPassword()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                this.Close();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (txtUserName.Text.Trim() == string.Empty)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    MessageBox.Show("Username/Email cannot be empty.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtUserName.Focus();
                    Cursor.Current = Cursors.WaitCursor;
                    return;
                }

                UserAuthentication userAuth = new UserAuthentication();
                User user = userAuth.GetByUsernameOrEmail(txtUserName.Text.Trim());
                if (user == null)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    MessageBox.Show("Username/Email is not found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtUserName.Focus();
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (userAuth.SendForgotPassword(txtUserName.Text.Trim()))
                {
                    Cursor.Current = Cursors.WaitCursor;
                    MessageBox.Show("The new password has been sent to your email address.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                    Cursor.Current = Cursors.Default;
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}