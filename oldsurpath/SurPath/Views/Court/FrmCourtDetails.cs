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
    public partial class FrmCourtDetails : Form
    {
        #region Private Variables

        private OperationMode _mode = OperationMode.None;
        private int _courtId = 0;

        private CourtBL courtBL = new CourtBL();
        private UserBL userBL = new UserBL();

        #endregion Private Variables

        #region Constructor

        public FrmCourtDetails()
        {
            InitializeComponent();
        }

        public FrmCourtDetails(OperationMode mode, int courtId)
        {
            InitializeComponent();
            this._mode = mode;
            this._courtId = courtId;
        }

        #endregion Constructor

        #region Event Methods

        private void FrmCourtDetails_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                InitializeControls();

                if (this._mode == OperationMode.Edit && this._courtId != 0)
                {
                    LoadData();
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Error occurred while loading the page.");
            }
        }

        private void FrmCourtDetails_FormClosing(object sender, FormClosingEventArgs e)
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

        private void txtCourtName_TextChanged(object sender, EventArgs e)
        {
            txtCourtName.CausesValidation = false;
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
            txtZipCode.CausesValidation = false;
        }

        private void chkActive_CheckedChanged(object sender, EventArgs e)
        {
            chkActive.CausesValidation = false;
        }

        private void txtZipCode_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            txtUsername.CausesValidation = false;
        }

        private void btnAvailablity_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (txtUsername.Text.Trim() == string.Empty)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Username/Email cannot be empty.");
                    txtUsername.Focus();
                }
                else
                {
                    if (ValidateEmail())
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Username is available.");
                    }
                    else
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Username is not available.");
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

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            txtPassword.CausesValidation = false;
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                RandomStringGenerator rsg = new RandomStringGenerator(true, false, true, false);
                string newPassword = rsg.Generate(6, 8);
                txtPassword.Text = newPassword;
                txtPassword.CausesValidation = false;
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
                    txtUsername.ReadOnly = true;
                    lblPassword.Visible = false;
                    txtPassword.Visible = false;
                    lblPasswordMan.Visible = false;
                    chkShowPassword.Visible = false;
                    btnResetPassword.Visible = false;

                    Court court = courtBL.Get(this._courtId);
                    //User user = userBL.GetJudgeId(this._courtId);

                    if (court != null)
                    {
                        txtUsername.Text = court.CourtUsername;
                        //  txtPassword.Text = user.UserPassword;
                        txtCourtName.Text = court.CourtName;
                        if (court.CourtAddress1 != string.Empty)
                        {
                            txtAddress1.Text = court.CourtAddress1;
                        }
                        else
                        {
                            txtAddress1.Text = string.Empty;
                        }
                        if (court.CourtAddress2 != string.Empty)
                        {
                            txtAddress2.Text = court.CourtAddress2;
                        }
                        else
                        {
                            txtAddress2.Text = string.Empty;
                        }
                        txtCity.Text = court.CourtCity;
                        cmbState.Text = court.CourtState;
                        txtZipCode.Text = court.CourtZip;
                        chkActive.Checked = court.IsActive;
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
            try
            {
                RandomStringGenerator rsg = new RandomStringGenerator(true, false, true, false);
                string newPassword = rsg.Generate(6, 8);
                cmbState.SelectedIndex = 0;
                txtPassword.Text = newPassword;

                if (this._mode == OperationMode.Edit)
                {
                    btnAvailablity.Visible = false;
                    btnResetPassword.Visible = false;
                }

                if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                {
                    //COURT_VIEW
                    DataRow[] courtView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.COURT_VIEW.ToDescriptionString() + "'");

                    if (courtView.Length > 0)
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

                Court court = null;

                if (this._mode == OperationMode.New)
                {
                    court = new Court();
                    court.CourtId = 0;
                    court.CreatedBy = Program.currentUserName;
                }
                else if (this._mode == OperationMode.Edit)
                {
                    court = courtBL.Get(this._courtId);
                }
                court.CourtUsername = txtUsername.Text.Trim();
                court.CourtPassword = txtPassword.Text.Trim();
                court.CourtName = txtCourtName.Text.Trim();
                court.CourtAddress1 = txtAddress1.Text.Trim();
                court.CourtAddress2 = txtAddress2.Text.Trim();
                court.CourtCity = txtCity.Text.Trim();
                court.CourtState = cmbState.Text.Trim();
                if (txtZipCode.Text.EndsWith("-") == true)
                {
                    court.CourtZip = txtZipCode.Text.Replace("-", "").Trim();
                }
                else
                {
                    court.CourtZip = txtZipCode.Text.Trim();
                }
                court.IsActive = chkActive.Checked;
                court.LastModifiedBy = Program.currentUserName;

                int returnVal = courtBL.Save(court);

                if (returnVal > 0)
                {
                    if (this._mode == OperationMode.New)
                    {
                        court.CourtId = returnVal;
                        this.CourtId = returnVal;
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
                if (txtCourtName.Text.Trim() == string.Empty)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Court Name cannot be empty.");
                    txtCourtName.Focus();
                    return false;
                }
                else
                {
                    if (!ValidateCourtName())
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Court Name already exits.");
                        txtCourtName.Focus();
                        return false;
                    }
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

                string ZipCode = txtZipCode.Text.Trim();
                ZipCode = ZipCode.EndsWith("-") ? ZipCode.Replace("-", "") : ZipCode;
                if (ZipCode.Trim() == string.Empty)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Zip Code cannot be empty.");
                    txtZipCode.Focus();
                    return false;
                }

                if (!Program.regexZipCode.IsMatch(ZipCode))
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Invalid format of Zip Code.");
                    txtZipCode.Focus();
                    return false;
                }

                if (txtUsername.Text.Trim() == string.Empty)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Username/Email cannot be empty.");
                    txtUsername.Focus();
                    return false;
                }
                else
                {
                    if (this._mode == OperationMode.New)
                    {
                        if (!ValidateEmail())
                        {
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("Username/Email is not available.");
                            txtUsername.Focus();
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

        private bool ValidateCourtName()
        {
            try
            {
                DataTable courts = courtBL.GetByCourtName(txtCourtName.Text.Trim());

                if (courts.Rows.Count > 0 && this._mode == OperationMode.New)
                {
                    return false;
                }
                else if (courts.Rows.Count > 1 && this._mode == OperationMode.Edit)
                {
                    return false;
                }
                else if (courts.Rows.Count == 1 && this._mode == OperationMode.Edit)
                {
                    if ((int)courts.Rows[0]["CourtId"] != this._courtId)
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
                User user = userBL.Get(txtUsername.Text.Trim());

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

        private bool ValidateEmail()
        {
            try
            {
                DataTable court = userBL.GetByEmail(txtUsername.Text.Trim());

                if (court.Rows.Count > 0 && this._mode == OperationMode.New)
                {
                    return false;
                }
                else if (court.Rows.Count > 1 && this._mode == OperationMode.Edit)
                {
                    return false;
                }
                else if (court.Rows.Count == 1 && this._mode == OperationMode.Edit)
                {
                    if ((int)court.Rows[0]["courtId"] != this._courtId)
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

        public int CourtId
        {
            get
            {
                return this._courtId;
            }
            set
            {
                this._courtId = value;
            }
        }

        #endregion Public Properties

        private void txtZipCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back)))
                e.Handled = true;
        }
    }
}