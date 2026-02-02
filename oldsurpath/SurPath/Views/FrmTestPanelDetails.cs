using SurPath.Business.Master;
using SurPath.Entity.Master;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmTestPanelDetails : Form
    {
        #region Private Variables

        private OperationMode _mode = OperationMode.None;
        private int _testPanelId = 0;

        private TestPanelBL testPanelBL = new TestPanelBL();

        #endregion Private Variables

        #region Constructor

        public FrmTestPanelDetails()
        {
            InitializeComponent();
        }

        public FrmTestPanelDetails(OperationMode mode, int testPanelId)
        {
            InitializeComponent();
            this._mode = mode;
            this._testPanelId = testPanelId;
        }

        #endregion Constructor

        #region Event Methods

        private void FrmTestPanelDetails_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeControls();

                if (this._mode == OperationMode.Edit && this._testPanelId != 0)
                {
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while loading the page.");
            }
        }

        private void FrmTestPanelDetails_FormClosing(object sender, FormClosingEventArgs e)
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

        private void btnDrugNotFound_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                FrmDrugNameDetails frmDrugNameDetails = new FrmDrugNameDetails(Enum.OperationMode.New, 0);
                if (frmDrugNameDetails.ShowDialog() == DialogResult.OK)
                {
                    List<int> selectedDrugNames = chkDrugNameList.ValueList;
                    LoadDrugNames();
                    chkDrugNameList.ValueList = selectedDrugNames;
                    chkDrugNameList.CausesValidation = false;
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

        private void txtTestPanelCode_TextChanged(object sender, EventArgs e)
        {
            txtTestPanelCode.CausesValidation = false;
        }

        private void txtCost_TextChanged(object sender, EventArgs e)
        {
            txtCost.CausesValidation = false;
        }

        private void chkDrugNameList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            chkDrugNameList.CausesValidation = false;
        }

        private void rbtnUA_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                LoadDrugNames();
                rbtnUA.CausesValidation = false;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void rbtnHair_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                LoadDrugNames();
                rbtnUA.CausesValidation = false;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void rbtnUA_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                LoadDrugNames();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void rbtnHair_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                LoadDrugNames();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {
            txtDescription.CausesValidation = false;
        }

        private void txtCost_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtTestPanelCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            string arr = "~`!@#$%^&*()+=-[]\\\';,./{}|\":<>?_";
            for (int k = 0; k < arr.Length; k++)
            {
                if (e.KeyChar == arr[k])
                {
                    e.Handled = true;
                    break;
                }
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
                    TestPanel testPanel = testPanelBL.Get(this._testPanelId);

                    if (testPanel != null)
                    {
                        txtTestPanelCode.Text = testPanel.TestPanelName;
                        txtDescription.Text = testPanel.TestPanelDescription;

                        if (testPanel.TestCategoryId == (int)TestCategories.UA)
                        {
                            rbtnUA.Checked = true;
                        }
                        else if (testPanel.TestCategoryId == (int)TestCategories.Hair)
                        {
                            rbtnHair.Checked = true;
                        }
                        else if (testPanel.TestCategoryId == (int)TestCategories.None)
                        {
                            rbtnUA.Checked = false;
                            rbtnHair.Checked = false;
                        }

                        txtCost.Text = ((double)testPanel.TestCost).ToString();
                        chkActive.Checked = testPanel.IsActive;
                        LoadDrugNames();

                        chkDrugNameList.ValueList = testPanel.DrugNames;

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
            chkDrugNameList.Items.Clear();

            rbtnUA.Checked = true;
            rbtnHair.Checked = false;

            LoadDrugNames();

            if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
            {
                //TEST_PANEL_DRUG_NAMES_ADD
                DataRow[] testPanelDrugName = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.TEST_PANEL_DRUG_NAMES_ADD.ToDescriptionString() + "'");

                if (testPanelDrugName.Length > 0)
                {
                    btnDrugNotFound.Visible = true;
                }
                else
                {
                    btnDrugNotFound.Visible = false;
                }

                //TEST_PANEL_VIEW
                DataRow[] testPanelView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.TEST_PANEL_VIEW.ToDescriptionString() + "'");

                if (testPanelView.Length > 0)
                {
                    foreach (Control ctrl in this.Controls)
                    {
                        //ctrl.Enabled = false;
                        if (ctrl is TextBox)
                        {
                            ((TextBox)ctrl).ReadOnly = true;
                        }
                        if (ctrl is RadioButton)
                        {
                            ((RadioButton)ctrl).Enabled = false;
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
                        if (ctrl is CheckedListBox)
                        {
                            ((CheckedListBox)ctrl).Enabled = false;
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

                TestPanel testPanel = null;

                if (this._mode == OperationMode.New)
                {
                    testPanel = new TestPanel();
                    testPanel.TestPanelId = 0;
                    testPanel.CreatedBy = Program.currentUserName;
                }
                else if (this._mode == OperationMode.Edit)
                {
                    testPanel = testPanelBL.Get(this._testPanelId);
                }

                testPanel.TestPanelName = txtTestPanelCode.Text.Trim();
                testPanel.TestPanelDescription = txtDescription.Text.Trim();

                if (rbtnUA.Checked == true)
                {
                    testPanel.TestCategoryId = (int)SurPath.Enum.TestCategories.UA;
                }
                else if (rbtnHair.Checked == true)
                {
                    testPanel.TestCategoryId = (int)SurPath.Enum.TestCategories.Hair;
                }
                else
                {
                    testPanel.TestCategoryId = (int)SurPath.Enum.TestCategories.None;
                }

                try
                {
                    testPanel.TestCost = Convert.ToDouble(txtCost.Text.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cost Must Have only numeric values");
                    txtCost.Focus();
                    return false;
                }
                testPanel.DrugNames = chkDrugNameList.ValueList;
                testPanel.IsActive = chkActive.Checked;
                if (chkActive.Checked == false && this._mode == OperationMode.Edit)
                {
                    int active = testPanelBL.TestPanelActive(this._testPanelId);
                    if (active > 0)
                    {
                        DialogResult userConfirmation = MessageBox.Show("This Test Panel is mapped with the department.\n To inactive the test panel you must un map the test panel with the department.\n Do you want to automatically un map the test panel with the department?", "SurPath Drug Testing", MessageBoxButtons.YesNoCancel);

                        if (userConfirmation == DialogResult.Cancel)
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            return false;
                            Cursor.Current = Cursors.Default;
                        }
                        else if (userConfirmation == DialogResult.No)
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            return false;
                            Cursor.Current = Cursors.Default;
                        }
                        else
                        {
                            int unmap = testPanelBL.UnmapTestPanel(testPanel);
                            if (unmap == 1)
                            {
                                MessageBox.Show("Test panel is un mapped");
                            }
                            else
                            {
                                MessageBox.Show("Cannot un map the test panel automatically.");
                                return false;
                            }
                        }
                    }
                }
                testPanel.LastModifiedBy = Program.currentUserName;

                int returnVal = testPanelBL.Save(testPanel);

                if (returnVal > 0)
                {
                    if (this._mode == OperationMode.New)
                    {
                        testPanel.TestPanelId = returnVal;
                        this.TestPanelId = returnVal;
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
            if (txtTestPanelCode.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Test Panel Code cannot be empty.");
                txtTestPanelCode.Focus();
                return false;
            }
            else
            {
                if (!ValidateTestPanelCode())
                {
                    MessageBox.Show("Test Panel Code already exits.");
                    txtTestPanelCode.Focus();
                    return false;
                }
            }

            if (rbtnUA.Checked == false && rbtnHair.Checked == false && rbtnBC.Checked == false && rbtnDNA.Checked == false)
            {
                MessageBox.Show("Category cannot be empty.");
                rbtnUA.Focus();
                return false;
            }

            if (txtCost.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Cost cannot be empty.");
                txtCost.Focus();
                return false;
            }
            else
            {
                try
                {
                    double cost = Convert.ToDouble(txtCost.Text.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Invalid format of Cost.");
                    txtCost.Focus();
                    return false;
                }
            }

            if (chkDrugNameList.ValueList.Count == 0)
            {
                MessageBox.Show("Drugs Name cannot be empty.");
                chkDrugNameList.Focus();
                return false;
            }

            return true;
        }

        private void LoadDrugNames()
        {
            try
            {
                DrugNameBL drugNameBL = new DrugNameBL();
                List<DrugName> drugNameList = null;

                TestCategories testCategory = rbtnUA.Checked ? TestCategories.UA : TestCategories.Hair;

                drugNameList = drugNameBL.GetList(testCategory);

                chkDrugNameList.DataSource = null;
                chkDrugNameList.SelectedValue = new List<int>();

                chkDrugNameList.DataSource = drugNameList;
                chkDrugNameList.DisplayMember = "DrugNameValue";
                chkDrugNameList.ValueMember = "DrugNameId";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadBackgroundChecks()
        {
            try
            {
                DrugNameBL drugNameBL = new DrugNameBL();
                List<DrugName> drugNameList = null;

                TestCategories testCategory = rbtnBC.Checked ? TestCategories.BC : TestCategories.UA;

                drugNameList = drugNameBL.GetList(testCategory);

                chkDrugNameList.DataSource = null;
                chkDrugNameList.SelectedValue = new List<int>();

                chkDrugNameList.DataSource = drugNameList;
                chkDrugNameList.DisplayMember = "DrugNameValue";
                chkDrugNameList.ValueMember = "DrugNameId";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool ValidateTestPanelCode()
        {
            DataTable testPanels = testPanelBL.GetByTestPanelCode(txtTestPanelCode.Text.Trim());

            if (testPanels.Rows.Count > 0 && this._mode == OperationMode.New)
            {
                return false;
            }
            else if (testPanels.Rows.Count > 1 && this._mode == OperationMode.Edit)
            {
                return false;
            }
            else if (testPanels.Rows.Count == 1 && this._mode == OperationMode.Edit)
            {
                if ((int)testPanels.Rows[0]["TestPanelId"] != this._testPanelId)
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

        public int TestPanelId
        {
            get
            {
                return this._testPanelId;
            }
            set
            {
                this._testPanelId = value;
            }
        }

        #endregion Public Properties

        private void rbtnBC_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                LoadBackgroundChecks();
                rbtnBC.CausesValidation = false;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void rbtnDNA_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                LoadDrugNames();
                rbtnDNA.CausesValidation = false;
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