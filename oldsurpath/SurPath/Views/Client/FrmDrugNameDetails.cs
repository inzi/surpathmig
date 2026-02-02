using SurPath.Business.Master;
using SurPath.Entity.Master;
using SurPath.Enum;
using System;
using System.Data;
using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmDrugNameDetails : Form
    {
        #region Private Variables

        private OperationMode _mode = OperationMode.None;
        private int _drugNameId = 0;

        private DrugNameBL drugNameBL = new DrugNameBL();

        private bool viewFlag = false;

        #endregion Private Variables

        #region Constructor

        public FrmDrugNameDetails()
        {
            InitializeComponent();
        }

        public FrmDrugNameDetails(OperationMode mode, int drugNameId)
        {
            InitializeComponent();

            this._mode = mode;
            this._drugNameId = drugNameId;
        }

        #endregion Constructor

        #region Event Methods

        private void FrmDrugNameDetails_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                InitializeControls();

                if (this._mode == OperationMode.Edit && this._drugNameId != 0)
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

        private void FrmDrugNameDetails_FormClosing(object sender, FormClosingEventArgs e)
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

        private void txtDrugName_TextChanged(object sender, EventArgs e)
        {
            txtDrugName.CausesValidation = false;
        }

        private void txtDrugCode_TextChanged(object sender, EventArgs e)
        {
            txtDrugCode.CausesValidation = false;
        }

        private void chkUA_CheckedChanged(object sender, EventArgs e)
        {
            chkUA.CausesValidation = false;
        }

        private void chkHair_CheckedChanged(object sender, EventArgs e)
        {
            chkHair.CausesValidation = false;
        }

        private void txtUAScreenValue_TextChanged(object sender, EventArgs e)
        {
            txtUAScreenValue.CausesValidation = false;
        }

        private void txtUAConfirmationValue_TextChanged(object sender, EventArgs e)
        {
            txtUAConfirmationValue.CausesValidation = false;
        }

        private void txtHairScreenValue_TextChanged(object sender, EventArgs e)
        {
            txtHairScreenValue.CausesValidation = false;
        }

        private void txtHairConfirmationValue_TextChanged(object sender, EventArgs e)
        {
            txtUAConfirmationValue.CausesValidation = false;
        }

        private void txtUnitofMeasurement_TextChanged(object sender, EventArgs e)
        {
            //txtUnitofMeasurement.CausesValidation = false;
        }

        private void chkUA_Click(object sender, EventArgs e)
        {
            if (chkUA.Checked == true)
            {
                txtUAScreenValue.Enabled = true;
                txtUAConfirmationValue.Enabled = true;
                cmbUAUOM.Enabled = true;
                // grbUA.Enabled = true;
                txtUAScreenValue.Focus();
            }
            else
            {
                txtUAScreenValue.Enabled = false;
                txtUAConfirmationValue.Enabled = false;
                cmbUAUOM.Enabled = false;
                // grbUA.Enabled = false;
                txtUAScreenValue.Text = string.Empty;
                txtUAConfirmationValue.Text = string.Empty;
                cmbUAUOM.SelectedIndex = 0;
                txtUAScreenValue.Focus();
            }
        }

        private void chkHair_Click(object sender, EventArgs e)
        {
            if (chkHair.Checked == true)
            {
                txtHairScreenValue.Enabled = true;
                txtHairConfirmationValue.Enabled = true;
                cmbHairUOM.Enabled = true;
                //grbHair.Enabled = true;
                txtHairScreenValue.Focus();
            }
            else
            {
                //grbHair.Enabled = false;
                txtHairScreenValue.Enabled = false;
                txtHairConfirmationValue.Enabled = false;
                cmbHairUOM.Enabled = false;
                txtHairScreenValue.Text = string.Empty;
                txtHairConfirmationValue.Text = string.Empty;
                cmbHairUOM.SelectedIndex = 0;
                txtHairScreenValue.Focus();
            }
        }

        private void txtDrugCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void txtUAScreenValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void txtUAConfirmationValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void txtHairScreenValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void txtHairConfirmationValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
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
                    DrugName drugName = drugNameBL.Get(this._drugNameId);

                    if (drugName != null)
                    {
                        txtDrugName.Text = drugName.DrugNameValue;
                        txtDrugCode.Text = drugName.DrugCodeValue;
                        chkUA.Checked = drugName.IsUA;
                        chkHair.Checked = drugName.IsHair;
                        if (chkUA.Checked == true)
                        {
                            txtUAScreenValue.Enabled = true;
                            txtUAConfirmationValue.Enabled = true;
                            cmbUAUOM.Enabled = true;
                            //grbUA.Enabled = true;
                            txtUAScreenValue.Text = drugName.UAScreenValue;
                            txtUAConfirmationValue.Text = drugName.UAConfirmationValue;
                            cmbUAUOM.Text = drugName.UAUnitOfMeasurement;
                            if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()) && viewFlag == true)
                            {
                                txtUAUOM.Text = cmbUAUOM.Text;
                                cmbUAUOM.Visible = false;
                                txtUAUOM.Visible = true;
                                txtUAUOM.Enabled = false;
                            }
                        }
                        else
                        {
                            txtUAScreenValue.Enabled = false;
                            txtUAConfirmationValue.Enabled = false;
                            cmbUAUOM.Enabled = false;
                            //grbUA.Enabled = false;
                            txtUAScreenValue.Text = string.Empty;
                            txtUAConfirmationValue.Text = string.Empty;
                            cmbUAUOM.SelectedIndex = 0;

                            txtUAUOM.Visible = false;
                            txtUAUOM.Text = string.Empty;
                        }
                        if (chkHair.Checked == true)
                        {
                            txtHairScreenValue.Enabled = true;
                            txtHairConfirmationValue.Enabled = true;
                            cmbHairUOM.Enabled = true;
                            //grbHair.Enabled = true;
                            txtHairScreenValue.Text = drugName.HairScreenValue;
                            txtHairConfirmationValue.Text = drugName.HairConfirmationValue;
                            cmbHairUOM.Text = drugName.HairUnitOfMeasurement;

                            if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()) && viewFlag == true)
                            {
                                txtHairUOM.Text = cmbHairUOM.Text;
                                cmbHairUOM.Visible = false;
                                txtHairUOM.Visible = true;
                                txtHairUOM.Enabled = false;
                            }
                        }
                        else
                        {
                            txtHairScreenValue.Enabled = false;
                            txtHairConfirmationValue.Enabled = false;
                            cmbHairUOM.Enabled = false;
                            // grbHair.Enabled = false;
                            txtHairScreenValue.Text = string.Empty;
                            txtHairScreenValue.Text = string.Empty;
                            cmbHairUOM.SelectedIndex = 0;

                            txtHairUOM.Visible = false;
                            txtHairUOM.Text = string.Empty;
                        }
                        // txtUnitofMeasurement.Text = drugName.UnitOfMeasurement;
                        chkActive.Checked = drugName.IsActive;
                        ResetControlsCauseValidation();
                    }
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void InitializeControls()
        {
            chkUA.Checked = false;
            chkHair.Checked = false;
            cmbUAUOM.SelectedIndex = 0;
            cmbHairUOM.SelectedIndex = 0;

            if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
            {
                //DRUG_NAMES_VIEW
                DataRow[] drugNamesView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DRUG_NAMES_VIEW.ToDescriptionString() + "'");

                if (drugNamesView.Length > 0)
                {
                    foreach (Control ctrl in this.Controls)
                    {
                        //ctrl.Enabled = false;
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
                        if (ctrl is GroupBox)
                        {
                            foreach (Control T in ctrl.Controls)
                            {
                                if (T is TextBox)
                                {
                                    ((TextBox)T).ReadOnly = true;
                                }
                                if (T is ComboBox)
                                {
                                    ((ComboBox)T).Enabled = false;
                                }
                            }
                        }
                    }
                    viewFlag = true;
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

                DrugName drugName = null;

                if (this._mode == OperationMode.New)
                {
                    drugName = new DrugName();
                    drugName.DrugNameId = 0;
                    drugName.CreatedBy = Program.currentUserName;
                }
                else if (this._mode == OperationMode.Edit)
                {
                    drugName = drugNameBL.Get(this._drugNameId);
                }

                drugName.DrugNameValue = txtDrugName.Text.Trim();
                drugName.DrugCodeValue = txtDrugCode.Text.Trim();
                drugName.IsUA = chkUA.Checked;
                drugName.IsHair = chkHair.Checked;
                drugName.IsBC = chkBC.Checked;
                drugName.IsDNA = chkDNA.Checked;

                if (chkUA.Checked == true)
                {
                    drugName.UAScreenValue = txtUAScreenValue.Text.Trim();
                    drugName.UAConfirmationValue = txtUAConfirmationValue.Text.Trim();
                    drugName.UAUnitOfMeasurement = cmbUAUOM.Text;
                }
                else
                {
                    drugName.UAScreenValue = string.Empty;
                    drugName.UAConfirmationValue = string.Empty;
                    drugName.UAUnitOfMeasurement = string.Empty;
                }

                if (chkHair.Checked == true)
                {
                    drugName.HairScreenValue = txtHairScreenValue.Text.Trim();
                    drugName.HairConfirmationValue = txtHairConfirmationValue.Text.Trim();
                    drugName.HairUnitOfMeasurement = cmbHairUOM.Text;
                }
                else
                {
                    drugName.HairScreenValue = string.Empty;
                    drugName.HairConfirmationValue = string.Empty;
                    drugName.HairUnitOfMeasurement = string.Empty;
                }
                if (chkBC.Checked == true)
                {
                    drugName.IsBC = chkBC.Checked;
                }
                if (chkDNA.Checked == true)
                {
                    drugName.IsDNA = chkDNA.Checked;
                }

                // drugName.UnitOfMeasurement = txtUnitofMeasurement.Text.Trim();
                drugName.IsActive = chkActive.Checked;
                drugName.LastModifiedBy = Program.currentUserName;

                int returnVal = drugNameBL.Save(drugName);

                if (returnVal > 0)
                {
                    if (this._mode == OperationMode.New)
                    {
                        drugName.DrugNameId = returnVal;
                        this.DrugNameId = returnVal;
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
            if (txtDrugName.Text.Trim() == string.Empty)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Drug Name cannot be empty.");
                txtDrugName.Focus();
                return false;
            }
            else
            {
                if (!ValidateDrugNames())
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Drug Name already exits.");
                    txtDrugName.Focus();
                    return false;
                }
            }
            if (txtDrugCode.Text.Trim() == string.Empty)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Drug Code cannot be empty.");
                txtDrugCode.Focus();
                return false;
            }
            if (chkUA.Checked == false && chkHair.Checked == false && chkBC.Checked == false && chkDNA.Checked == false)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Test Category must be selected");
                //chkUA.Focus();
                return false;
            }
            if (chkUA.Checked == true)
            {
                if (txtUAScreenValue.Text.Trim() == string.Empty)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("UA Screen Value cannot be empty.");
                    txtUAScreenValue.Focus();
                    return false;
                }
                else if (txtUAConfirmationValue.Text.Trim() == string.Empty)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("UA Confirmation Value cannot be empty.");
                    txtUAConfirmationValue.Focus();
                    return false;
                }
                else if (cmbUAUOM.SelectedIndex == 0)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("UA Unit of Measurement Value cannot be empty.");
                    return false;
                }
            }
            if (chkHair.Checked == true)
            {
                if (txtHairScreenValue.Text.Trim() == string.Empty)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Hair Screen Value cannot be empty.");
                    txtHairScreenValue.Focus();
                    return false;
                }
                else if (txtHairConfirmationValue.Text.Trim() == string.Empty)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Hair Confirmation Value cannot be empty.");
                    txtHairConfirmationValue.Focus();
                    return false;
                }
                else if (cmbHairUOM.SelectedIndex == 0)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Hair Unit of Measurement Value cannot be empty.");
                    return false;
                }
            }

            //if (txtUnitofMeasurement.Text.Trim() == string.Empty)
            //{
            //    Cursor.Current = Cursors.Default;
            //    MessageBox.Show("Unit of Measurement cannot be empty.");
            //    txtUnitofMeasurement.Focus();
            //    return false;
            //}

            return true;
        }

        private bool ValidateDrugNames()
        {
            try
            {
                DataTable drugNames = drugNameBL.GetByDrugName(txtDrugName.Text.Trim());

                if (drugNames.Rows.Count > 0 && this._mode == OperationMode.New)
                {
                    return false;
                }
                else if (drugNames.Rows.Count > 1 && this._mode == OperationMode.Edit)
                {
                    return false;
                }
                else if (drugNames.Rows.Count == 1 && this._mode == OperationMode.Edit)
                {
                    if ((int)drugNames.Rows[0]["DrugNameId"] != this._drugNameId)
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
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

        public int DrugNameId
        {
            get
            {
                return this._drugNameId;
            }
            set
            {
                this._drugNameId = value;
            }
        }

        #endregion Public Properties

        private void grbHair_Enter(object sender, EventArgs e)
        {
        }

        private void cmbUAUOM_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbUAUOM.CausesValidation = false;
        }

        private void cmbHairUOM_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbHairUOM.CausesValidation = false;
        }

        private void cmbUAUOM_TextChanged(object sender, EventArgs e)
        {
            cmbUAUOM.CausesValidation = false;
        }

        private void chkBC_CheckedChanged(object sender, EventArgs e)
        {
            chkBC.CausesValidation = false;
        }

        private void chkDNA_CheckedChanged(object sender, EventArgs e)
        {
            chkDNA.CausesValidation = false;
        }
    }
}