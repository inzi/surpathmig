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
    public partial class FrmJudgeDetails : Form
    {
        #region Private Variables

        private OperationMode _mode = OperationMode.None;
        private int _judgeId = 0;

        private JudgeBL judgeBL = new JudgeBL();
        private UserBL userBL = new UserBL();

        #endregion Private Variables

        #region Constructor

        public FrmJudgeDetails()
        {
            InitializeComponent();
        }

        public FrmJudgeDetails(OperationMode mode)
        {
            InitializeComponent();

            this._mode = mode;
        }

        public FrmJudgeDetails(OperationMode mode, int judgeId)
        {
            InitializeComponent();

            this._mode = mode;
            this._judgeId = judgeId;
        }

        #endregion Constructor

        #region Event Methods

        private void FrmJudgeDetails_Load(object sender, EventArgs e)
        {
            cmbPrefix.SelectedIndex = 0;
            cmbSuffix.SelectedIndex = 0;
            try
            {
                InitializeControls();

                if (this._mode == OperationMode.Edit && this._judgeId != 0)
                {
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while loading the page.");
            }
        }

        private void FrmJudgeDetails_FormClosing(object sender, FormClosingEventArgs e)
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

        private void cmbPrefix_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbPrefix.CausesValidation = false;
        }

        private void txtFirstName_TextChanged(object sender, EventArgs e)
        {
            txtFirstName.CausesValidation = false;
        }

        private void txtLastName_TextChanged(object sender, EventArgs e)
        {
            txtLastName.CausesValidation = false;
        }

        private void cmbSuffix_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbSuffix.CausesValidation = false;
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

        private void txtZipCode_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void txtZipCode_TextChanged(object sender, EventArgs e)
        {
            txtZipCode.CausesValidation = false;
        }

        private void chkActive_CheckedChanged(object sender, EventArgs e)
        {
            chkActive.CausesValidation = false;
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
                    Cursor.Current = Cursors.WaitCursor;
                    MessageBox.Show("Username/Email cannot be empty.");
                    txtUsername.Focus();
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    if (ValidateEmail())
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        MessageBox.Show("Username is available.");
                        Cursor.Current = Cursors.Default;
                    }
                    else
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        MessageBox.Show("Username is not available.");
                        Cursor.Current = Cursors.Default;
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
                    Judge judge = judgeBL.Get(this._judgeId);

                    txtUsername.ReadOnly = true;
                    lblPassword.Visible = false;
                    txtPassword.Visible = false;
                    lblPasswordMan.Visible = false;
                    chkShowPassword.Visible = false;
                    btnResetPassword.Visible = false;

                    if (judge != null)
                    {
                        txtUsername.Text = judge.JudgeUsername;
                        cmbPrefix.SelectedItem = judge.JudgePrefix;
                        txtFirstName.Text = judge.JudgeFirstName;
                        txtLastName.Text = judge.JudgeLastName;
                        cmbSuffix.SelectedItem = judge.JudgeSuffix;
                        txtAddress1.Text = judge.JudgeAddress1;
                        txtAddress2.Text = judge.JudgeAddress2;
                        txtCity.Text = judge.JudgeCity;
                        cmbState.Text = judge.JudgeState;
                        txtZipCode.Text = judge.JudgeZip;
                        chkActive.Checked = judge.IsActive;
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
            RandomStringGenerator rsg = new RandomStringGenerator(true, false, true, false);
            string newPassword = rsg.Generate(6, 8);

            cmbState.SelectedIndex = 0;
            cmbPrefix.SelectedIndex = 0;
            cmbSuffix.SelectedIndex = 0;
            txtPassword.Text = newPassword;

            if (this._mode == OperationMode.Edit)
            {
                btnAvailablity.Visible = false;
            }

            if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
            {
                //JUDGE_VIEW
                DataRow[] judgeView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.JUDGE_VIEW.ToDescriptionString() + "'");

                if (judgeView.Length > 0)
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

                Judge judge = null;

                if (this._mode == OperationMode.New)
                {
                    judge = new Judge();
                    judge.JudgeId = 0;
                    judge.CreatedBy = Program.currentUserName;
                }
                else if (this._mode == OperationMode.Edit)
                {
                    txtUsername.ReadOnly = true;
                    lblPassword.Visible = false;
                    txtPassword.Visible = false;
                    lblPasswordMan.Visible = false;
                    chkShowPassword.Visible = false;
                    btnResetPassword.Visible = false;

                    judge = judgeBL.Get(this._judgeId);
                }
                judge.JudgeUsername = txtUsername.Text;
                judge.JudgePassword = txtPassword.Text;

                if (cmbPrefix.Text.ToUpper() == "(Select)".ToUpper())
                {
                    judge.JudgePrefix = string.Empty;
                }
                else
                {
                    judge.JudgePrefix = cmbPrefix.SelectedItem.ToString();
                }
                judge.JudgeFirstName = txtFirstName.Text;
                judge.JudgeLastName = txtLastName.Text;
                if (cmbSuffix.Text.ToUpper() == "(Select)".ToUpper())
                {
                    judge.JudgeSuffix = string.Empty;
                }
                else
                {
                    judge.JudgeSuffix = cmbSuffix.SelectedItem.ToString();
                }
                judge.JudgeAddress1 = txtAddress1.Text;
                judge.JudgeAddress2 = txtAddress2.Text;
                judge.JudgeCity = txtCity.Text;
                judge.JudgeState = cmbState.Text;

                if (txtZipCode.Text.EndsWith("-") == true)
                {
                    judge.JudgeZip = txtZipCode.Text.Replace("-", "").Trim();
                }
                else
                {
                    judge.JudgeZip = txtZipCode.Text.Trim();
                }
                judge.IsActive = chkActive.Checked;
                judge.LastModifiedBy = Program.currentUserName;

                int returnVal = judgeBL.Save(judge);

                if (returnVal > 0)
                {
                    if (this._mode == OperationMode.New)
                    {
                        judge.JudgeId = returnVal;
                        this.JudgeId = returnVal;
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

                string ZipCode = txtZipCode.Text.Trim();

                ZipCode = ZipCode.EndsWith("-") ? ZipCode.Replace("-", "") : ZipCode;

                if (!Program.regexZipCode.IsMatch(ZipCode))
                {
                    MessageBox.Show("Invalid format of Zip Code.");
                    txtZipCode.Focus();
                    return false;
                }

                if (txtUsername.Text.Trim() == string.Empty)
                {
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

        private bool ValidateUsername()
        {
            User user = userBL.Get(txtUsername.Text.Trim());

            if (user != null)
            {
                return false;
            }

            return true;
        }

        private bool ValidateEmail()
        {
            DataTable judge = userBL.GetByEmail(txtUsername.Text.Trim());

            if (judge.Rows.Count > 0 && this._mode == OperationMode.New)
            {
                return false;
            }
            else if (judge.Rows.Count > 1 && this._mode == OperationMode.Edit)
            {
                return false;
            }
            else if (judge.Rows.Count == 1 && this._mode == OperationMode.Edit)
            {
                if ((int)judge.Rows[0]["judgeId"] != this._judgeId)
                {
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

        public int JudgeId
        {
            get
            {
                return this._judgeId;
            }
            set
            {
                this._judgeId = value;
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