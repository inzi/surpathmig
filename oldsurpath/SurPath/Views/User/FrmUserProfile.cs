using SurPath.Business;
using SurPath.Entity;
using System;
using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmUserProfile : Form
    {
        #region Private Variables

        private int _userId = 0;

        private UserBL userBL = new UserBL();

        #endregion Private Variables

        #region Constructor

        public FrmUserProfile()
        {
            InitializeComponent();
        }

        #endregion Constructor

        #region Event Methods

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                FrmChangePassword frmChange = new FrmChangePassword();
                frmChange.ShowDialog();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmUserProfile_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeControls();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while loading the page.");
            }
        }

        private void FrmUserProfile_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                bool validationFlag = true;

                foreach (Control ctrl in this.Controls)
                {
                    if (ctrl.CausesValidation == false)
                    {
                        validationFlag = false;
                        break;
                    }
                }

                if (validationFlag == false)
                {
                    DialogResult userConfirmation = MessageBox.Show("Do you want to save changes?", "SurPath Drug Testing", MessageBoxButtons.YesNoCancel);

                    if (userConfirmation == DialogResult.Cancel)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        e.Cancel = true;
                        Cursor.Current = Cursors.Default;
                    }
                    else if (userConfirmation == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        if (!SaveData())
                        {
                            e.Cancel = true;
                        }
                        Cursor.Current = Cursors.Default;
                    }
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (SaveData())
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                this.Close();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void txtFirstName_TextChanged(object sender, EventArgs e)
        {
            txtFirstName.CausesValidation = false;
        }

        private void txtLastName_TextChanged(object sender, EventArgs e)
        {
            txtLastName.CausesValidation = false;
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            txtEmail.CausesValidation = false;
        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
            txtPhone.CausesValidation = false;
        }

        private void txtFax_TextChanged(object sender, EventArgs e)
        {
            txtFax.CausesValidation = false;
        }

        private void txtPhone_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void txtFax_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        #endregion Event Methods

        #region Public Methods

        public void LoadData()
        {
            try
            {
                User user = userBL.Get(Program.currentUserName);

                if (user != null)
                {
                    lblprousername.Text = user.Username;
                    txtFirstName.Text = user.UserFirstName;
                    txtLastName.Text = user.UserLastName;
                    txtPhone.Text = user.UserPhoneNumber;
                    txtFax.Text = user.UserFax;
                    txtEmail.Text = user.UserEmail;

                    ResetControlsCauseValidation();
                }
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
            ResetControlsCauseValidation();
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

                User user = null;

                user = userBL.Get(Program.currentUserName);

                user.UserFirstName = txtFirstName.Text.Trim();
                user.UserLastName = txtLastName.Text.Trim();
                user.UserPhoneNumber = txtPhone.Text.Trim();
                user.UserFax = txtFax.Text.Trim();
                user.UserEmail = txtEmail.Text.Trim();

                if (txtPhone.Text.Equals("(   )    -") == true)
                {
                    user.UserPhoneNumber = string.Empty;
                }
                else
                {
                    user.UserPhoneNumber = txtPhone.Text.Trim();
                }
                if (txtFax.Text.Equals("(   )    -") == true)
                {
                    user.UserFax = string.Empty;
                }
                else
                {
                    user.UserFax = txtFax.Text.Trim();
                }
                user.UserEmail = txtEmail.Text.Trim();
                user.LastModifiedBy = Program.currentUserName;

                //  int returnVal = userBL.Save(user);

                int returnVal = userBL.UserSave(user);

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
            if (txtFirstName.Text.Trim() == string.Empty)
            {
                MessageBox.Show("First Name cannot be empty.");
                txtFirstName.Focus();
                return false;
            }
            if (txtLastName.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Last Name cannot be empty.");
                txtLastName.Focus();
                return false;
            }

            if (txtEmail.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Email cannot be empty.");
                return false;
            }
            if (txtEmail.Text.Trim() != string.Empty)
            {
                if (!Program.regexEmail.IsMatch(txtEmail.Text.Trim()))
                {
                    MessageBox.Show("Invalid format of Email.");
                    txtEmail.Focus();
                    return false;
                }
            }

            return true;
        }

        #endregion Private Methods

        #region Public Properties

        public int UserId
        {
            get
            {
                return this._userId;
            }
            set
            {
                this._userId = value;
            }
        }

        #endregion Public Properties
    }
}