using SurPath.Business;
using SurPath.Business.Master;
using SurPath.Entity;
using SurPath.Entity.Master;
using SurPath.Enum;
using System;
using System.Data;
using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmAttorneyDetails : Form
    {
        #region Private Variables

        private OperationMode _mode = OperationMode.None;
        private int _attorneyId = 0;

        private AttorneyBL attorneyBL = new AttorneyBL();
        private UserBL userBL = new UserBL();

        #endregion Private Variables

        #region Constructor

        public FrmAttorneyDetails()
        {
            InitializeComponent();
        }

        public FrmAttorneyDetails(OperationMode mode, int attorneyId)
        {
            InitializeComponent();
            this._mode = mode;
            this._attorneyId = attorneyId;
        }

        #endregion Constructor

        #region Event Methods

        private void FrmAttorneyDetails_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeControls();

                if (this._mode == OperationMode.Edit && this._attorneyId != 0)
                {
                    txtEmailAddress.Enabled = false;
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                Program.LogError(ex);
                MessageBox.Show("Error occurred while loading the page.");
            }
        }

        private void FrmAttorneyDetails_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
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
                        e.Cancel = true;
                    }
                    else if (userConfirmation == DialogResult.Yes)
                    {
                        if (!SaveData())
                        {
                            e.Cancel = true;
                        }
                        else
                        {
                            this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        }
                    }
                }
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

        private void txtAttorneyFirstname_TextChanged(object sender, EventArgs e)
        {
            txtAttorneyFirstname.CausesValidation = false;
        }

        private void txtAttorneyLastName_TextChanged(object sender, EventArgs e)
        {
            txtAttorneyLastName.CausesValidation = false;
        }

        private void txtAddress1_TextChanged(object sender, EventArgs e)
        {
            txtAddress1.CausesValidation = false;
        }

        private void txtAddress2_TextChanged(object sender, EventArgs e)
        {
            txtAddress2.CausesValidation = false;
        }

        private void txtCity_TextChanged(object sender, EventArgs e)
        {
            txtCity.CausesValidation = false;
        }

        private void cmbState_TextChanged(object sender, EventArgs e)
        {
            cmbState.CausesValidation = false;
        }

        private void txtZipcode_TextChanged(object sender, EventArgs e)
        {
            txtZipcode.CausesValidation = false;
        }

        private void txtPhoneNumber_TextChanged(object sender, EventArgs e)
        {
            txtPhoneNumber.CausesValidation = false;
        }

        private void txtFaxNumber_TextChanged(object sender, EventArgs e)
        {
            txtFaxNumber.CausesValidation = false;
        }

        private void txtEmailAddress_TextChanged(object sender, EventArgs e)
        {
            txtEmailAddress.CausesValidation = false;
        }

        private void txtZipcode_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void txtPhoneNumber_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void txtFaxNumber_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void chkActive_CheckedChanged(object sender, EventArgs e)
        {
            chkActive.CausesValidation = false;
        }

        private void btnAvailability_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (txtEmailAddress.Text.Trim() == string.Empty)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Email cannot be empty.");
                    txtEmailAddress.Focus();
                }
                else
                {
                    if (ValidateEmail())
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Email is available.");
                    }
                    else
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Email is not available.");
                    }
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
                if (this._mode == OperationMode.Edit)
                {
                    Attorney attorney = attorneyBL.Get(this._attorneyId);

                    if (attorney != null)
                    {
                        txtAttorneyFirstname.Text = attorney.AttorneyFirstName;
                        txtAttorneyLastName.Text = attorney.AttorneyLastName;
                        if (attorney.AttorneyAddress1 != string.Empty)
                        {
                            txtAddress1.Text = attorney.AttorneyAddress1;
                        }
                        else
                        {
                            txtAddress1.Text = string.Empty;
                        }
                        if (attorney.AttorneyAddress2 != string.Empty)
                        {
                            txtAddress2.Text = attorney.AttorneyAddress2;
                        }
                        else
                        {
                            txtAddress2.Text = string.Empty;
                        }
                        txtCity.Text = attorney.AttorneyCity;
                        cmbState.Text = attorney.AttorneyState;
                        txtZipcode.Text = attorney.AttorneyZip;
                        txtPhoneNumber.Text = attorney.AttorneyPhone;
                        txtFaxNumber.Text = attorney.AttorneyFax;
                        txtEmailAddress.Text = attorney.AttorneyEmail;
                        chkActive.Checked = attorney.IsActive;
                        ResetControlsCauseValidation();
                    }
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
            cmbState.SelectedIndex = 0;

            if (this._mode == OperationMode.Edit)
            {
                btnAvailability.Visible = false;
            }
            if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
            {
                //ATTORNEY_VIEW
                DataRow[] attorneyView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.ATTORNEY_VIEW.ToDescriptionString() + "'");

                if (attorneyView.Length > 0)
                {
                    foreach (Control ctrl in this.Controls)
                    {
                        if (ctrl is TextBox)
                        {
                            ((TextBox)ctrl).ReadOnly = true;
                        }
                        if (ctrl is ComboBox)
                        {
                            ((ComboBox)ctrl).Enabled = false;
                        }
                        if (ctrl is Button)
                        {
                            ((Button)ctrl).Enabled = false;
                        }
                        if (ctrl is CheckBox)
                        {
                            ((CheckBox)ctrl).Enabled = false;
                        }
                        if (ctrl is MaskedTextBox)
                        {
                            ((MaskedTextBox)ctrl).ReadOnly = true;
                        }
                    }
                }
            }

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

                Attorney attorney = null;

                if (this._mode == OperationMode.New)
                {
                    attorney = new Attorney();
                    attorney.AttorneyId = 0;
                    attorney.CreatedBy = Program.currentUserName;
                }
                else if (this._mode == OperationMode.Edit)
                {
                    attorney = attorneyBL.Get(this._attorneyId);
                }
                attorney.AttorneyFirstName = txtAttorneyFirstname.Text.Trim();
                attorney.AttorneyLastName = txtAttorneyLastName.Text.Trim();
                attorney.AttorneyAddress1 = txtAddress1.Text.Trim();
                attorney.AttorneyAddress2 = txtAddress2.Text.Trim();
                attorney.AttorneyCity = txtCity.Text.Trim();
                attorney.AttorneyState = cmbState.Text.Trim();
                if (txtZipcode.Text.EndsWith("-") == true)
                {
                    attorney.AttorneyZip = txtZipcode.Text.Replace("-", "").Trim();
                }
                else
                {
                    attorney.AttorneyZip = txtZipcode.Text.Trim();
                }
                if (txtPhoneNumber.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") != string.Empty)
                {
                    attorney.AttorneyPhone = txtPhoneNumber.Text.Trim();
                }
                else
                {
                    attorney.AttorneyPhone = string.Empty;
                }
                if (txtFaxNumber.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") != string.Empty)
                {
                    attorney.AttorneyFax = txtFaxNumber.Text.Trim();
                }
                else
                {
                    attorney.AttorneyFax = string.Empty;
                }
                attorney.AttorneyEmail = txtEmailAddress.Text.Trim();
                attorney.IsActive = chkActive.Checked;
                attorney.LastModifiedBy = Program.currentUserName;

                int returnVal = attorneyBL.Save(attorney);

                if (returnVal > 0)
                {
                    if (this._mode == OperationMode.New)
                    {
                        attorney.AttorneyId = returnVal;
                        this.AttorneyId = returnVal;
                    }

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
                if (txtAttorneyFirstname.Text.Trim() == string.Empty)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("First Name cannot be empty.");
                    txtAttorneyFirstname.Focus();
                    return false;
                }
                if (txtAttorneyLastName.Text.Trim() == string.Empty)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Last Name cannot be empty.");
                    txtAttorneyLastName.Focus();
                    return false;
                }
                if (txtAddress1.Text.Trim() == string.Empty)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Address 1 cannot be empty.");
                    txtAddress1.Focus();
                    return false;
                }
                if (txtCity.Text.Trim() == string.Empty)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("City cannot be empty.");
                    txtCity.Focus();
                    return false;
                }
                if (cmbState.Text.Trim().ToUpper() == "(Select)".ToUpper())
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("State must be selected.");
                    cmbState.Focus();
                    return false;
                }
                string ZipCode = txtZipcode.Text.Trim();
                ZipCode = ZipCode.EndsWith("-") ? ZipCode.Replace("-", "") : ZipCode;

                if (!Program.regexZipCode.IsMatch(ZipCode))
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Invalid format of Zip Code.");
                    txtZipcode.Focus();
                    return false;
                }
                if (txtEmailAddress.Text.Trim() == string.Empty)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Email cannot be empty.");
                    txtEmailAddress.Focus();
                    return false;
                }
                if (txtPhoneNumber.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") != string.Empty)
                {
                    if (!Program.regexPhoneNumber.IsMatch(txtPhoneNumber.Text.Trim()))
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Invalid format of Phone number.");
                        txtPhoneNumber.Focus();
                        return false;
                    }
                }
                if (txtFaxNumber.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") != string.Empty)
                {
                    if (!Program.regexPhoneNumber.IsMatch(txtFaxNumber.Text.Trim()))
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Invalid format of Fax number.");
                        txtFaxNumber.Focus();
                        return false;
                    }
                }
                if (txtEmailAddress.Text.Trim() != string.Empty)
                {
                    if (!Program.regexEmail.IsMatch(txtEmailAddress.Text.Trim()))
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Invalid format of Email.");
                        txtEmailAddress.Focus();
                        return false;
                    }
                    else
                    {
                        if (!ValidateEmail())
                        {
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("Email Already Exists.");
                            txtEmailAddress.Focus();
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            return true;
        }

        private bool ValidateEmail()
        {
            try
            {
                DataTable attorney = userBL.GetByEmail(txtEmailAddress.Text.Trim());

                if (attorney.Rows.Count > 0 && this._mode == OperationMode.New)
                {
                    return false;
                }
                else if (attorney.Rows.Count > 1 && this._mode == OperationMode.Edit)
                {
                    return false;
                }
                else if (attorney.Rows.Count == 1 && this._mode == OperationMode.Edit)
                {
                    if ((int)attorney.Rows[0]["AttorneyId"] != this._attorneyId)
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            return true;
        }

        private bool ValidateUsername()
        {
            try
            {
                User user = userBL.Get(txtEmailAddress.Text.Trim());

                if (user != null)
                {
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

        #endregion Private Methods

        #region Public Properties

        public OperationMode Mode
        {
            get
            {
                return this._mode;
            }
            set
            {
                this._mode = value;
            }
        }

        public int AttorneyId
        {
            get
            {
                return this._attorneyId;
            }
            set
            {
                this._attorneyId = value;
            }
        }

        #endregion Public Properties



        private void txtZipcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back)))
                e.Handled = true;
        }
    }
}