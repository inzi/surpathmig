using SurPath.Business;
using SurPath.Entity;
using SurPath.Enum;
using System;
using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmThirdPartyDetails : Form
    {
        #region Private Variables

        private OperationMode _mode = OperationMode.None;
        private int _thirdPartyId = 0;
        private int _donorId;

        private ThirdPartyBL thirdPartyBL = new ThirdPartyBL();

        #endregion Private Variables

        #region Constructor

        public FrmThirdPartyDetails()
        {
            InitializeComponent();
        }

        public FrmThirdPartyDetails(OperationMode mode, int donorId, int thirdPartyId)
        {
            InitializeComponent();
            this._mode = mode;
            this._thirdPartyId = thirdPartyId;
            this._donorId = donorId;
        }

        #endregion Constructor

        #region Event Methods

        private void FrmThirdPartyDetails_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeControls();

                if (this._mode == OperationMode.Edit && this._thirdPartyId != 0)
                {
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while loading the page.");
            }
        }

        private void FrmThirdPartyDetails_FormClosing(object sender, FormClosingEventArgs e)
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
                        else
                        {
                            this.DialogResult = System.Windows.Forms.DialogResult.OK;
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

        private void txtThirdPartyFirstname_TextChanged(object sender, EventArgs e)
        {
            txtThirdPartyFirstname.CausesValidation = false;
        }

        private void txtThirdPartyLastName_TextChanged(object sender, EventArgs e)
        {
            txtThirdPartyLastName.CausesValidation = false;
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

        private void cmbState_SelectedIndexChanged(object sender, EventArgs e)
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

        #endregion Event Methods

        #region Public Methods

        public void LoadData()
        {
            try
            {
                if (this._mode == OperationMode.Edit)
                {
                    ThirdParty thirdParty = thirdPartyBL.Get(this._thirdPartyId);

                    if (thirdParty != null)
                    {
                        txtThirdPartyFirstname.Text = thirdParty.ThirdPartyFirstName;
                        txtThirdPartyLastName.Text = thirdParty.ThirdPartyLastName;
                        if (thirdParty.ThirdPartyAddress1 != string.Empty)
                        {
                            txtAddress1.Text = thirdParty.ThirdPartyAddress1;
                        }
                        else
                        {
                            txtAddress1.Text = string.Empty;
                        }
                        if (thirdParty.ThirdPartyAddress2 != string.Empty)
                        {
                            txtAddress2.Text = thirdParty.ThirdPartyAddress2;
                        }
                        else
                        {
                            txtAddress2.Text = string.Empty;
                        }
                        txtCity.Text = thirdParty.ThirdPartyCity;
                        cmbState.Text = thirdParty.ThirdPartyState;
                        txtZipcode.Text = thirdParty.ThirdPartyZip;
                        txtPhoneNumber.Text = thirdParty.ThirdPartyPhone;
                        txtFaxNumber.Text = thirdParty.ThirdPartyFax;
                        txtEmailAddress.Text = thirdParty.ThirdPartyEmail;
                        chkActive.Checked = thirdParty.IsActive;
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

                ThirdParty thirdParty = null;

                if (this._mode == OperationMode.New)
                {
                    thirdParty = new ThirdParty();
                    thirdParty.ThirdPartyId = 0;
                    thirdParty.CreatedBy = Program.currentUserName;
                }
                else if (this._mode == OperationMode.Edit)
                {
                    thirdParty = thirdPartyBL.Get(this._thirdPartyId);
                }

                thirdParty.DonorId = this._donorId;
                thirdParty.ThirdPartyFirstName = txtThirdPartyFirstname.Text.Trim();
                thirdParty.ThirdPartyLastName = txtThirdPartyLastName.Text.Trim();
                thirdParty.ThirdPartyAddress1 = txtAddress1.Text.Trim();
                thirdParty.ThirdPartyAddress2 = txtAddress2.Text.Trim();
                thirdParty.ThirdPartyCity = txtCity.Text.Trim();
                thirdParty.ThirdPartyState = cmbState.Text.Trim();
                if (txtZipcode.Text.EndsWith("-") == true)
                {
                    thirdParty.ThirdPartyZip = txtZipcode.Text.Replace("-", "").Trim();
                }
                else
                {
                    thirdParty.ThirdPartyZip = txtZipcode.Text.Trim();
                }
                if (txtPhoneNumber.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") != string.Empty)
                {
                    thirdParty.ThirdPartyPhone = txtPhoneNumber.Text.Trim();
                }
                else
                {
                    thirdParty.ThirdPartyPhone = string.Empty;
                }
                if (txtFaxNumber.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") != string.Empty)
                {
                    thirdParty.ThirdPartyFax = txtFaxNumber.Text.Trim();
                }
                else
                {
                    thirdParty.ThirdPartyFax = string.Empty;
                }
                thirdParty.ThirdPartyEmail = txtEmailAddress.Text.Trim();
                thirdParty.IsActive = chkActive.Checked;
                thirdParty.LastModifiedBy = Program.currentUserName;

                int returnVal = thirdPartyBL.Save(thirdParty);

                if (returnVal > 0)
                {
                    if (this._mode == OperationMode.New)
                    {
                        thirdParty.ThirdPartyId = returnVal;
                        this.ThirdPartyId = returnVal;
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
            if (this._donorId == 0)
            {
                MessageBox.Show("Invalid Vendor Id.");
                txtThirdPartyFirstname.Focus();
                return false;
            }

            if (txtThirdPartyFirstname.Text.Trim() == string.Empty)
            {
                MessageBox.Show("First Name cannot be empty.");
                txtThirdPartyFirstname.Focus();
                return false;
            }
            if (txtThirdPartyLastName.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Last Name cannot be empty.");
                txtThirdPartyLastName.Focus();
                return false;
            }
            if (txtAddress1.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Address 1 cannot be empty.");
                txtAddress1.Focus();
                return false;
            }
            if (txtCity.Text.Trim() == string.Empty)
            {
                MessageBox.Show("City cannot be empty.");
                txtCity.Focus();
                return false;
            }
            if (cmbState.Text.Trim().ToUpper() == "(Select)".ToUpper())
            {
                MessageBox.Show("State must be selected.");
                cmbState.Focus();
                return false;
            }
            string ZipCode = txtZipcode.Text.Trim();
            ZipCode = ZipCode.EndsWith("-") ? ZipCode.Replace("-", "") : ZipCode;

            if (!Program.regexZipCode.IsMatch(ZipCode))
            {
                MessageBox.Show("Invalid format of Zip Code.");
                txtZipcode.Focus();
                return false;
            }
            if (txtEmailAddress.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Email cannot be empty.");
                txtEmailAddress.Focus();
                return false;
            }
            if (txtPhoneNumber.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") != string.Empty)
            {
                if (!Program.regexPhoneNumber.IsMatch(txtPhoneNumber.Text.Trim()))
                {
                    MessageBox.Show("Invalid format of Phone number.");
                    txtPhoneNumber.Focus();
                    return false;
                }
            }
            if (txtFaxNumber.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") != string.Empty)
            {
                if (!Program.regexPhoneNumber.IsMatch(txtFaxNumber.Text.Trim()))
                {
                    MessageBox.Show("Invalid format of Fax number.");
                    txtFaxNumber.Focus();
                    return false;
                }
            }
            if (txtEmailAddress.Text.Trim() != string.Empty)
            {
                if (!Program.regexEmail.IsMatch(txtEmailAddress.Text.Trim()))
                {
                    MessageBox.Show("Invalid format of Email.");
                    txtEmailAddress.Focus();
                    return false;
                }
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

        public int ThirdPartyId
        {
            get
            {
                return this._thirdPartyId;
            }
            set
            {
                this._thirdPartyId = value;
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