using SurPath.Business;
using SurPath.Entity;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmChangePassword : Form
    {
        #region Private Variables

        private UserBL userBL = new UserBL();

        private string _mode = "";

        #endregion Private Variables

        #region Constructor

        public FrmChangePassword()
        {
            InitializeComponent();
        }

        public FrmChangePassword(string mode)
        {
            this._mode = mode;

            InitializeComponent();
        }

        #endregion Constructor

        #region Event Methods

        private void FrmChangePassword_Load(object sender, EventArgs e)
        {
            try
            {
                lbl1Username.Text = Program.currentUserName;

                if (_mode != string.Empty)
                {
                    lblOldPassword.Visible = false;
                    txtOldPassword.Visible = false;
                    lblOldPswMan.Visible = false;

                    lblNewPassword.Location = new Point(12, 54);
                    lblNewPswMan.Location = new Point(91, 51);
                    txtNewPassword.Location = new Point(119, 49);

                    lblConfirmPassword.Location = new Point(12, 86);
                    lblConfirmPswMan.Location = new Point(101, 84);
                    txtConfirmPassword.Location = new Point(119, 82);

                    lblPasswordRule1.Location = new Point(119, 141);
                    lblPasswordRule2.Location = new Point(119, 166);

                    chkConfirmShowPassword.Location = new Point(119, 116);

                    btnOK.Location = new Point(119, 192);
                    btnClose.Location = new Point(200, 193);

                    this.Size = new Size(384, 255);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnChangeClose_Click(object sender, EventArgs e)
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

        private void btnChangeOk_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (SaveData())
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
                else
                {
                    return;
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        #endregion Event Methods

        #region Public Methods

        public void LoadData()
        {
            try
            {
                ResetControlsCauseValidation();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void InitializeControls()
        {
            try
            {
                ResetControlsCauseValidation();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ResetControlsCauseValidation()
        {
            foreach (Control ctrl in this.Controls)
            {
                ctrl.CausesValidation = true;
            }
        }

        private bool SaveData()
        {
            try
            {
                if (!ValidateControls())
                {
                    return false;
                }

                int returnVal = userBL.ChangePassword(Program.currentUserName, UserAuthentication.Encrypt(txtNewPassword.Text, true));

                if (returnVal > 0)
                {
                    ResetControlsCauseValidation();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private bool ValidateControls()
        {
            try
            {
                if (_mode == string.Empty)
                {
                    if (txtOldPassword.Text == string.Empty)
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Old password cannot be empty.");
                        txtOldPassword.Focus();
                        return false;
                    }
                    else
                    {
                        if (!ValidatePassword())
                        {
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("The old password does not match with the database value.");
                            txtOldPassword.Focus();
                            return false;
                        }
                    }
                }

                if (txtNewPassword.Text == string.Empty)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("New password cannot be empty.");
                    txtNewPassword.Focus();
                    return false;
                }

                if (txtNewPassword.Text != string.Empty)
                {
                    // string NewPassword = "^.*(?=.{6,})(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%=~]).*$";
                    string NewPassword = "^.*(?=.{6,})(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%=+^~]).[\\S]*$";
                    if (!string.IsNullOrEmpty(txtNewPassword.Text.Trim()))
                    {
                        if (!Regex.IsMatch(txtNewPassword.Text, NewPassword))
                        {
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("Password doesn't meet with security level.");
                            txtNewPassword.Focus();
                            return false;
                        }
                    }
                }

                if (txtConfirmPassword.Text == string.Empty)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Confirm password cannot be empty.");
                    txtConfirmPassword.Focus();
                    return false;
                }

                if (txtNewPassword.Text != txtConfirmPassword.Text)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Confirm password doesn't match with new password.");
                    txtConfirmPassword.Focus();
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            return true;
        }

        private bool ValidatePassword()
        {
            try
            {
                User user = userBL.GetByUsernameOrEmail(Program.currentUserName);

                if (user != null)
                {
                    if (user.UserPassword == UserAuthentication.Encrypt(txtOldPassword.Text, true))
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            return false;
        }

        #endregion Private Methods

        private void txtNewPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = (e.KeyChar == (char)Keys.Space);
        }

        private void txtConfirmPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = (e.KeyChar == (char)Keys.Space);
        }

        private void chkConfirmShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (chkConfirmShowPassword.Checked == true)
            {
                txtOldPassword.PasswordChar = '\0';
                txtConfirmPassword.PasswordChar = '\0';
                txtNewPassword.PasswordChar = '\0';
            }
            else
            {
                txtOldPassword.PasswordChar = '*';
                txtConfirmPassword.PasswordChar = '*';
                txtNewPassword.PasswordChar = '*';
            }
        }

        private void chkConfirmShowPassword_TextChanged(object sender, EventArgs e)
        {
            chkConfirmShowPassword.CausesValidation = false;
        }
    }
}