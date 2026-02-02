using SurPath.Business.Master;
using SurPath.Entity.Master;
using SurPath.Enum;
using System;
using System.Data;
using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmVendorServiceDetails : Form
    {
        #region Private Variables

        private OperationMode _mode = OperationMode.None;
        private int _vendorServiceId = 0;
        private int _vendorId = 0;

        private VendorServiceBL vendorServiceBL = new VendorServiceBL();

        #endregion Private Variables

        #region Constructor

        public FrmVendorServiceDetails()
        {
            InitializeComponent();
        }

        public FrmVendorServiceDetails(OperationMode mode, int vendorId, int vendorServiceId)
        {
            InitializeComponent();

            this._mode = mode;
            this._vendorServiceId = vendorServiceId;
            this._vendorId = vendorId;
        }

        #endregion Constructor

        #region Event Methods

        private void FrmVendorServiceDetails_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeControls();

                if (this._mode == OperationMode.Edit && this._vendorServiceId != 0)
                {
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while loading the page.");
            }
        }

        private void FrmVendorServiceDetails_FormClosing(object sender, FormClosingEventArgs e)
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

        private void txtServiceName_TextChanged(object sender, EventArgs e)
        {
            txtServiceName.CausesValidation = false;
        }

        private void txtServiceCost_TextChanged(object sender, EventArgs e)
        {
            txtServiceCost.CausesValidation = false;
        }

        private void txtServiceCost_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void chkActive_TextChanged(object sender, EventArgs e)
        {
            chkActive.CausesValidation = false;
        }

        private void chkActive_CheckedChanged(object sender, EventArgs e)
        {
            chkActive.CausesValidation = false;
        }

        #endregion Event Methods

        #region Public Methods

        public void LoadData()
        {
            if (this._mode == OperationMode.Edit)
            {
                VendorService vendorService = vendorServiceBL.Get(this._vendorServiceId);

                if (vendorService != null)
                {
                    txtServiceName.Text = vendorService.VendorServiceNameValue;
                    txtServiceCost.Text = ((double)vendorService.Cost).ToString();
                    if (vendorService.TestCategoryId == (int)TestCategories.UA)
                    {
                        rbtnUA.Checked = true;
                    }
                    else if (vendorService.TestCategoryId == (int)TestCategories.Hair)
                    {
                        rbtnHair.Checked = true;
                    }
                    else if (vendorService.TestCategoryId == (int)TestCategories.DNA)
                    {
                        rbtnDNA.Checked = true;
                    }
                    else if (vendorService.TestCategoryId == (int)TestCategories.None)
                    {
                        rbtnUA.Checked = false;
                        rbtnHair.Checked = false;
                        rbtnDNA.Checked = false;
                    }

                    if (vendorService.IsObserved == YesNo.Yes)
                    {
                        rbObserved.Checked = true;
                    }
                    else if (vendorService.IsObserved == YesNo.No)
                    {
                        rbUnObserved.Checked = true;
                    }
                    else if (vendorService.IsObserved == YesNo.None)
                    {
                        rbObserved.Checked = false;
                        rbUnObserved.Checked = false;
                    }

                    if (vendorService.FormTypeId == SpecimenFormType.Federal)
                    {
                        rbFederal.Checked = true;
                    }
                    else if (vendorService.FormTypeId == SpecimenFormType.NonFederal)
                    {
                        rbNonFederal.Checked = true;
                    }
                    else if (vendorService.FormTypeId == SpecimenFormType.None)
                    {
                        rbFederal.Checked = false;
                        rbNonFederal.Checked = false;
                    }

                    chkActive.Checked = vendorService.IsActive;
                    ResetControlsCauseValidation();
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void InitializeControls()
        {
            rbtnUA.Checked = false;
            rbtnHair.Checked = false;
            rbtnDNA.Checked = false;
            rbFederal.Checked = false;
            rbNonFederal.Checked = false;
            rbObserved.Checked = false;
            rbUnObserved.Checked = false;

            if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
            {
                //VENDOR_VENDOR_SERVICE_VIEW
                DataRow[] vendorServiceView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.VENDOR_VENDOR_SERVICE_VIEW.ToDescriptionString() + "'");

                if (vendorServiceView.Length > 0)
                {
                    foreach (Control ctrl in this.Controls)
                    {
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
                        if (ctrl is Panel)
                        {
                            ((Panel)ctrl).Enabled = false;
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

                VendorService vendorService = null;

                if (this._mode == OperationMode.New)
                {
                    vendorService = new VendorService();
                    vendorService.VendorServiceId = 0;
                    vendorService.CreatedBy = Program.currentUserName;
                }
                else if (this._mode == OperationMode.Edit)
                {
                    vendorService = vendorServiceBL.Get(this._vendorServiceId);
                }

                vendorService.VendorId = this._vendorId;

                vendorService.VendorServiceNameValue = txtServiceName.Text.Trim();

                try
                {
                    vendorService.Cost = Convert.ToDouble(txtServiceCost.Text.Trim());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("cost must have only numeric values");
                    txtServiceCost.Focus();
                    return false;
                }
                if (rbtnUA.Checked == true)
                {
                    vendorService.TestCategoryId = (int)SurPath.Enum.TestCategories.UA;
                }
                else if (rbtnHair.Checked == true)
                {
                    vendorService.TestCategoryId = (int)SurPath.Enum.TestCategories.Hair;
                }
                else if (rbtnDNA.Checked == true)
                {
                    vendorService.TestCategoryId = (int)SurPath.Enum.TestCategories.DNA;
                }

                //Observed
                if (rbObserved.Checked)
                {
                    vendorService.IsObserved = YesNo.Yes;
                }
                else if (rbUnObserved.Checked)
                {
                    vendorService.IsObserved = YesNo.No;
                }

                //Form Type
                if (rbFederal.Checked)
                {
                    vendorService.FormTypeId = SpecimenFormType.Federal;
                }
                else if (rbNonFederal.Checked)
                {
                    vendorService.FormTypeId = SpecimenFormType.NonFederal;
                }

                vendorService.IsActive = chkActive.Checked;
                vendorService.LastModifiedBy = Program.currentUserName;

                int returnVal = vendorServiceBL.Save(vendorService);

                if (returnVal > 0)
                {
                    if (this._mode == OperationMode.New)
                    {
                        vendorService.VendorServiceId = returnVal;
                        this.VendorServiceId = returnVal;
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
            if (this._vendorId == 0)
            {
                MessageBox.Show("Invalid Vendor Id.");
                txtServiceName.Focus();
                return false;
            }

            if (txtServiceName.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Service Name cannot be empty.");
                txtServiceName.Focus();
                return false;
            }
            else
            {
                if (this._mode == OperationMode.New)
                {
                    if (!ValidateServiceName())
                    {
                        MessageBox.Show("Service Name already exits.");
                        txtServiceName.Focus();
                        return false;
                    }
                }
            }
            if (txtServiceCost.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Service Cost cannot be empty.");
                txtServiceCost.Focus();
                return false;
            }
            else
            {
                try
                {
                    double cost = Convert.ToDouble(txtServiceCost.Text.Trim());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Invalid format of Cost.");
                    txtServiceCost.Focus();
                    return false;
                }
            }

            if (rbtnUA.Checked == false && rbtnHair.Checked == false && rbtnDNA.Checked == false)
            {
                MessageBox.Show("Test Category must be selected.");
                rbtnUA.Focus();
                return false;
            }

            if (rbObserved.Checked == false && rbUnObserved.Checked == false)
            {
                MessageBox.Show("Observed Type must be selected.");
                rbObserved.Focus();
                return false;
            }

            if (rbFederal.Checked == false && rbNonFederal.Checked == false)
            {
                MessageBox.Show("Form Type must be selected.");
                rbFederal.Focus();
                return false;
            }

            //Cost Parameter Validation
            int testCategroyId = 0;
            if (rbtnUA.Checked)
            {
                testCategroyId = 1;
            }

            if (rbtnHair.Checked)
            {
                testCategroyId = 2;
            }

            if (rbtnDNA.Checked)
            {
                testCategroyId = 3;
            }

            YesNo observedType = YesNo.None;
            if (rbObserved.Checked)
            {
                observedType = YesNo.Yes;
            }

            if (rbUnObserved.Checked)
            {
                observedType = YesNo.No;
            }

            SpecimenFormType formType = SpecimenFormType.None;
            if (rbFederal.Checked)
            {
                formType = SpecimenFormType.Federal;
            }

            if (rbNonFederal.Checked)
            {
                formType = SpecimenFormType.NonFederal;
            }

            VendorService vendorService = vendorServiceBL.GetVendorServiceByCostParam(this.VendorId, testCategroyId, observedType, formType);

            if (vendorService != null && this.Mode == OperationMode.New)
            {
                MessageBox.Show("Given Cost Parameter are already exists.");
                txtServiceCost.Focus();
                return false;
            }

            //else if (vendorService != null && this.Mode == OperationMode.Edit)
            //{
            //    MessageBox.Show("Given Cost Parameter are already exists.");
            //    txtServiceCost.Focus();
            //    return false;
            //}
            else if (vendorService != null && this.Mode == OperationMode.Edit)
            {
                if (vendorService.VendorServiceId != this.VendorServiceId && vendorService.VendorId == this.VendorId)
                {
                    MessageBox.Show("Given Cost Parameter are already exists.");
                    txtServiceCost.Focus();
                    return false;
                }
            }

            return true;
        }

        private bool ValidateServiceName()
        {
            DataTable vendorServices = vendorServiceBL.GetByVendorServiceName(VendorId, txtServiceName.Text.Trim());

            if (vendorServices.Rows.Count > 0 && this._mode == OperationMode.New)
            {
                return false;
            }
            else if (vendorServices.Rows.Count > 1 && this._mode == OperationMode.Edit)
            {
                return false;
            }
            else if (vendorServices.Rows.Count == 1 && this._mode == OperationMode.Edit)
            {
                if ((int)vendorServices.Rows[0]["VendorId"] != this._vendorId)
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

        public int VendorServiceId
        {
            get
            {
                return this._vendorServiceId;
            }
            set
            {
                this._vendorServiceId = value;
            }
        }

        public int VendorId
        {
            get
            {
                return this._vendorId;
            }
            set
            {
                this._vendorId = value;
            }
        }

        #endregion Public Properties
    }
}