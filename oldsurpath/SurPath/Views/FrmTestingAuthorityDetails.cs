using SurPath.Business.Master;
using SurPath.Entity.Master;
using SurPath.Enum;
using System;
using System.Data;
using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmTestingAuthorityDetails : Form
    {
        #region Private Variables

        private OperationMode _mode = OperationMode.None;
        private int _testingAuthorityId = 0;

        private TestingAuthorityBL testingAuthorityBL = new TestingAuthorityBL();

        #endregion Private Variables

        #region Constructor

        public FrmTestingAuthorityDetails()
        {
            InitializeComponent();
        }

        public FrmTestingAuthorityDetails(OperationMode mode, int testingAuthorityId)
        {
            InitializeComponent();

            this._mode = mode;
            this._testingAuthorityId = testingAuthorityId;
        }

        #endregion Constructor

        #region Event Methods

        private void FrmTestingAuthorityDetails_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeControls();

                if (this._mode == OperationMode.Edit && this._testingAuthorityId != 0)
                {
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while loading the page.");
            }
        }

        private void FrmTestingAuthorityDetails_FormClosing(object sender, FormClosingEventArgs e)
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

        private void txtTestingAuthorityName_TextChanged(object sender, EventArgs e)
        {
            txtTestingAuthorityName.CausesValidation = false;
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
                    TestingAuthority testingAuthority = testingAuthorityBL.Get(this._testingAuthorityId);

                    if (testingAuthority != null)
                    {
                        txtTestingAuthorityName.Text = testingAuthority.TestingAuthorityName;
                        chkActive.Checked = testingAuthority.IsActive;
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
            if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
            {
                //TESTING_AUTHORITY_VIEW
                DataRow[] testingAuthorityView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.TESTING_AUTHORITY_VIEW.ToDescriptionString() + "'");

                if (testingAuthorityView.Length > 0)
                {
                    foreach (Control ctrl in this.Controls)
                    {
                        if (ctrl is TextBox)
                        {
                            ((TextBox)ctrl).ReadOnly = true;
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
                TestingAuthority testingAuthority = null;

                if (this._mode == OperationMode.New)
                {
                    testingAuthority = new TestingAuthority();
                    testingAuthority.TestingAuthorityId = 0;
                    testingAuthority.CreatedBy = Program.currentUserName;
                }
                else if (this._mode == OperationMode.Edit)
                {
                    testingAuthority = testingAuthorityBL.Get(this._testingAuthorityId);
                }

                testingAuthority.TestingAuthorityName = txtTestingAuthorityName.Text.Trim();
                testingAuthority.IsActive = chkActive.Checked;
                testingAuthority.LastModifiedBy = Program.currentUserName;

                int returnVal = testingAuthorityBL.Save(testingAuthority);

                if (returnVal > 0)
                {
                    if (this._mode == OperationMode.New)
                    {
                        testingAuthority.TestingAuthorityId = returnVal;
                        this.TestingAuthorityId = returnVal;
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
            if (txtTestingAuthorityName.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Testing Authority Name cannot be empty.");
                return false;
            }
            else
            {
                if (!ValidateTestingAuthorityName())
                {
                    MessageBox.Show("Testing Authority Name already exits.");
                    txtTestingAuthorityName.Focus();
                    return false;
                }
            }

            return true;
        }

        private bool ValidateTestingAuthorityName()
        {
            DataTable testingAuthority = testingAuthorityBL.GetByTestingAuthorityName(txtTestingAuthorityName.Text.Trim());

            if (testingAuthority.Rows.Count > 0 && this._mode == OperationMode.New)
            {
                return false;
            }
            else if (testingAuthority.Rows.Count > 1 && this._mode == OperationMode.Edit)
            {
                return false;
            }
            else if (testingAuthority.Rows.Count == 1 && this._mode == OperationMode.Edit)
            {
                if ((int)testingAuthority.Rows[0]["TestingAuthorityId"] != this._testingAuthorityId)
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

        public int TestingAuthorityId
        {
            get
            {
                return this._testingAuthorityId;
            }
            set
            {
                this._testingAuthorityId = value;
            }
        }

        #endregion Public Properties
    }
}