using SurPath.Business;
using SurPath.Business.Master;
using SurPath.Entity;
using SurPath.Entity.Master;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmVendorDetails : Form
    {
        #region Private Variables

        private OperationMode _mode = OperationMode.None;
        private int _vendorId = 0;

        //double? cost = null;
        private VendorBL vendorBL = new VendorBL();

        private bool _isPhysical1Required = true;
        private bool _isPhysical2Required = false;
        private bool _isPhysical3Required = false;
        private bool _isMailingRequired = false;

        private bool viewFlag = false;

        #endregion Private Variables

        #region Constructor

        public FrmVendorDetails()
        {
            InitializeComponent();
        }

        public FrmVendorDetails(OperationMode mode, int vendorId)
        {
            InitializeComponent();

            this._mode = mode;
            this._vendorId = vendorId;
        }

        #endregion Constructor

        #region Event Methods

        private void FrmVendorDetails_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeControls();

                if (this._mode == OperationMode.Edit && this._vendorId != 0)
                {
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while loading the page.");
            }
        }

        private void FrmVendorDetails_FormClosing(object sender, FormClosingEventArgs e)
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

        private void btnVendorService_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                //if (this._vendorId == 0 && this._mode == OperationMode.New)
                //{
                //    if (MessageBox.Show("This operation requires to save the Vendor Details. Do you want to continue?", "SurPath Drug Testing", MessageBoxButtons.YesNoCancel) == System.Windows.Forms.DialogResult.Yes)
                //    {
                Cursor.Current = Cursors.WaitCursor;
                if (!SaveData())
                {
                    return;
                }
                //}
                //else
                //{
                //    return;
                //}
                Cursor.Current = Cursors.Default;
                //}

                FrmVendorServiceDetails frmVendorService = new FrmVendorServiceDetails(Enum.OperationMode.New, this._vendorId, 0);
                if (frmVendorService.ShowDialog() == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    LoadVendorService();
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void btnVendorServicesDetails_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                //if (this._vendorId == 0 && this._mode == OperationMode.New)
                //{
                //    if (MessageBox.Show("This operation requires to save the Vendor Details. Do you want to continue?", "SurPath Drug Testing", MessageBoxButtons.YesNoCancel) == System.Windows.Forms.DialogResult.Yes)
                //    {
                Cursor.Current = Cursors.WaitCursor;
                if (!SaveData())
                {
                    return;
                }
                //    }
                //    else
                //    {
                //        Cursor.Current = Cursors.WaitCursor;
                //        return;
                //    }
                //}

                FrmVendorServiceInfo frmVendorServiceInfo = new FrmVendorServiceInfo(this._vendorId);
                frmVendorServiceInfo.ShowDialog();
                Cursor.Current = Cursors.WaitCursor;
                LoadVendorService();
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

        private void rbActive_Click(object sender, EventArgs e)
        {
            if (rbActive.Checked == true)
            {
                lblInactiveSpecify.Visible = false;
                lblInactiveDate.Visible = false;
                // txtInactiveDate.Visible = false;
                cmbMonth.Visible = false;
                cmbDate.Visible = false;
                cmbYear.Visible = false;
                txtInactiveReason.Visible = false;
            }
            else
            {
                lblInactiveSpecify.Visible = true;
                lblInactiveDate.Visible = true;
                //txtInactiveDate.Visible = true;
                cmbMonth.Visible = true;
                cmbDate.Visible = true;
                cmbYear.Visible = true;

                txtInactiveReason.Visible = true;
            }
        }

        private void rbInActive_Click(object sender, EventArgs e)
        {
            if (rbInActive.Checked == true)
            {
                lblInactiveSpecify.Visible = true;
                lblInactiveDate.Visible = true;
                //  txtInactiveDate.Visible = true;
                cmbMonth.Visible = true;
                cmbDate.Visible = true;
                cmbYear.Visible = true;
                var myDate = DateTime.Now;
                cmbMonth.Text = myDate.ToString("MM");
                cmbDate.Text = myDate.ToString("dd");
                cmbYear.Text = myDate.ToString("yyyy");
                txtInactiveReason.Visible = true;
            }
            else
            {
                lblInactiveSpecify.Visible = false;
                lblInactiveDate.Visible = false;
                // txtInactiveDate.Visible = false;
                cmbMonth.Visible = false;
                cmbDate.Visible = false;
                cmbYear.Visible = false;
                txtInactiveReason.Visible = false;
            }
        }

        private void txtVendorName_TextChanged(object sender, EventArgs e)
        {
            txtVendorName.CausesValidation = false;
        }

        private void txtMainContact_TextChanged(object sender, EventArgs e)
        {
            txtMainContact.CausesValidation = false;
        }

        private void txtPhoneNumber_TextChanged(object sender, EventArgs e)
        {
            txtPhoneNumber.CausesValidation = false;
        }

        private void txtFaxNumber_TextChanged(object sender, EventArgs e)
        {
            txtFaxNumber.CausesValidation = false;
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            txtEmail.CausesValidation = false;
        }

        private void rbCollectionCenter_CheckedChanged(object sender, EventArgs e)
        {
            rbCollectionCenter.CausesValidation = false;
        }

        private void rbLab_CheckedChanged(object sender, EventArgs e)
        {
            rbLab.CausesValidation = false;
        }

        private void rbMRO_CheckedChanged(object sender, EventArgs e)
        {
            rbMRO.CausesValidation = false;
        }

        private void chkVendorServices_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            chkVendorServices.CausesValidation = false;

            e.NewValue = CheckState.Checked;
        }

        private void rbActive_CheckedChanged(object sender, EventArgs e)
        {
            LoadVendorService();
            rbActive.CausesValidation = false;
        }

        private void rbInActive_CheckedChanged(object sender, EventArgs e)
        {
            LoadVendorService();
            rbInActive.CausesValidation = false;
        }

        private void txtInvoiceDate_TextChanged(object sender, EventArgs e)
        {
            //txtInactiveDate.CausesValidation = false;
        }

        private void txtReason_TextChanged(object sender, EventArgs e)
        {
            txtInactiveReason.CausesValidation = false;
        }

        private void txtPhysical1Address1_TextChanged(object sender, EventArgs e)
        {
            txtPhysical1Address1.CausesValidation = false;
        }

        private void txtPhysical1Address2_TextChanged(object sender, EventArgs e)
        {
            txtPhysical1Address2.CausesValidation = false;
        }

        private void txtPhysical1City_TextChanged(object sender, EventArgs e)
        {
            txtPhysical1City.CausesValidation = false;
        }

        private void txtPhysical1ZipCode_TextChanged(object sender, EventArgs e)
        {
            txtPhysical1ZipCode.CausesValidation = false;
        }

        private void txtPhysical1PhoneNumber_TextChanged(object sender, EventArgs e)
        {
            txtPhysical1PhoneNumber.CausesValidation = false;
        }

        private void txtPhysical1FaxNumber_TextChanged(object sender, EventArgs e)
        {
            txtPhysical1FaxNumber.CausesValidation = false;
        }

        private void txtPhysical1Email_TextChanged(object sender, EventArgs e)
        {
            txtPhysical1Email.CausesValidation = false;
        }

        private void txtPhysical2Address1_TextChanged(object sender, EventArgs e)
        {
            txtPhysical2Address1.CausesValidation = false;
        }

        private void txtPhysical2Address2_TextChanged(object sender, EventArgs e)
        {
            txtPhysical2Address2.CausesValidation = false;
        }

        private void txtPhysical2City_TextChanged(object sender, EventArgs e)
        {
            txtPhysical2City.CausesValidation = false;
        }

        private void txtPhysical2ZipCode_TextChanged(object sender, EventArgs e)
        {
            txtPhysical2ZipCode.CausesValidation = false;
        }

        private void txtPhysical2PhoneNumber_TextChanged(object sender, EventArgs e)
        {
            txtPhysical2PhoneNumber.CausesValidation = false;
        }

        private void txtPhysical2FaxNumber_TextChanged(object sender, EventArgs e)
        {
            txtPhysical2FaxNumber.CausesValidation = false;
        }

        private void txtPhysical2Email_TextChanged(object sender, EventArgs e)
        {
            txtPhysical2Email.CausesValidation = false;
        }

        private void txtPhysical3Address1_TextChanged(object sender, EventArgs e)
        {
            txtPhysical3Address1.CausesValidation = false;
        }

        private void txtPhysical3Address2_TextChanged(object sender, EventArgs e)
        {
            txtPhysical3Address2.CausesValidation = false;
        }

        private void txtPhysical3City_TextChanged(object sender, EventArgs e)
        {
            txtPhysical3City.CausesValidation = false;
        }

        private void txtPhysical3ZipCode_TextChanged(object sender, EventArgs e)
        {
            txtPhysical3ZipCode.CausesValidation = false;
        }

        private void txtPhysical3PhoneNumber_TextChanged(object sender, EventArgs e)
        {
            txtPhysical3PhoneNumber.CausesValidation = false;
        }

        private void txtPhysical3FaxNumber_TextChanged(object sender, EventArgs e)
        {
            txtPhysical3FaxNumber.CausesValidation = false;
        }

        private void txtPhysical3Email_TextChanged(object sender, EventArgs e)
        {
            txtPhysical3Email.CausesValidation = false;
        }

        private void chkSamePhysical1_CheckedChanged(object sender, EventArgs e)
        {
            chkSamePhysical1.CausesValidation = false;

            if (chkSamePhysical1.Checked)
            {
                txtMailingAddress1.Enabled = false;
                txtMailingAddress2.Enabled = false;
                txtMailingCity.Enabled = false;
                cmbMailingState.Enabled = false;
                txtMailingZipCode.Enabled = false;

                #region Physical Address

                Vendor vendor = vendorBL.GetAddress(this._vendorId);
                if (vendor != null)
                {
                    foreach (VendorAddress address in vendor.Addresses)
                    {
                        if (address.AddressTypeId == AddressTypes.MailingAddress && !chkSamePhysical1.Checked)
                        {
                            txtMailingAddress1.Text = address.Address1;
                            txtMailingAddress2.Text = address.Address2;
                            txtMailingCity.Text = address.City;
                            cmbMailingState.Text = address.State;
                            txtMailingZipCode.Text = address.ZipCode;
                        }
                        if (chkSamePhysical1.Checked == true)
                        {
                            txtMailingAddress1.Text = txtPhysical1Address1.Text;
                            txtMailingAddress2.Text = txtPhysical1Address2.Text;
                            txtMailingCity.Text = txtPhysical1City.Text;
                            cmbMailingState.Text = cmbPhysical1State.Text;
                            txtMailingZipCode.Text = txtPhysical1ZipCode.Text;
                        }
                    }
                }
                else if (chkSamePhysical1.Checked == true)
                {
                    txtMailingAddress1.Text = txtPhysical1Address1.Text;
                    txtMailingAddress2.Text = txtPhysical1Address2.Text;
                    txtMailingCity.Text = txtPhysical1City.Text;
                    cmbMailingState.Text = cmbPhysical1State.Text;
                    txtMailingZipCode.Text = txtPhysical1ZipCode.Text;
                }

                #endregion Physical Address
            }
            else
            {
                txtMailingAddress1.Enabled = true;
                txtMailingAddress2.Enabled = true;
                txtMailingCity.Enabled = true;
                cmbMailingState.Enabled = true;
                txtMailingZipCode.Enabled = true;

                txtMailingAddress1.Text = string.Empty;
                txtMailingAddress2.Text = string.Empty;
                txtMailingCity.Text = string.Empty;
                cmbMailingState.SelectedIndex = 0;
                txtMailingZipCode.Text = string.Empty;
            }
        }

        private void txtMailingAddress1_TextChanged(object sender, EventArgs e)
        {
            txtMailingAddress1.CausesValidation = false;
        }

        private void txtMailingAddress2_TextChanged(object sender, EventArgs e)
        {
            txtMailingAddress2.CausesValidation = false;
        }

        private void txtMailingCity_TextChanged(object sender, EventArgs e)
        {
            txtMailingCity.CausesValidation = false;
        }

        private void txtMailingZipCode_TextChanged(object sender, EventArgs e)
        {
            txtMailingZipCode.CausesValidation = false;
        }

        private void cmbPhysical1State_TextChanged(object sender, EventArgs e)
        {
            cmbPhysical1State.CausesValidation = false;
        }

        private void cmbPhysical2State_TextChanged(object sender, EventArgs e)
        {
            cmbPhysical2State.CausesValidation = false;
        }

        private void cmbPhysical3State_TextChanged(object sender, EventArgs e)
        {
            cmbPhysical3State.CausesValidation = false;
        }

        private void cmbMailingState_TextChanged(object sender, EventArgs e)
        {
            cmbMailingState.CausesValidation = false;
        }

        private void txtInactiveDate_MouseClick(object sender, MouseEventArgs e)
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

        private void txtPhysical1PhoneNumber_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void txtPhysical1FaxNumber_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void txtPhysical2PhoneNumber_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void txtPhysical2FaxNumber_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void txtPhysical3PhoneNumber_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void txtPhysical3FaxNumber_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void txtPhysical3ZipCode_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void txtPhysical1ZipCode_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void txtPhysical2ZipCode_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void txtMailingZipCode_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void rbMRO_Click(object sender, EventArgs e)
        {
            if (rbMRO.Checked == true)
            {
                //grbCost.Visible =true;
                grbCost.Visible = true;
                grbServices.Visible = false;
                txtMPOSCost.Focus();
            }
            else
            {
                grbCost.Visible = false;
                txtMPOSCost.Text = string.Empty;
                txtMALLCost.Text = string.Empty;
                grbServices.Visible = true;
            }
        }

        private void rbCollectionCenter_Click(object sender, EventArgs e)
        {
            if (rbCollectionCenter.Checked == true)
            {
                grbServices.Visible = true;
                grbCost.Visible = false;
            }
            else if (rbLab.Checked == true)
            {
                grbServices.Visible = true;
                grbCost.Visible = false;
            }
            else
            {
                grbCost.Visible = true;
                grbServices.Visible = false;
                txtMPOSCost.Text = string.Empty;
                txtMALLCost.Text = string.Empty;
            }
        }

        private void rbLab_Click(object sender, EventArgs e)
        {
            if (rbLab.Checked == true)
            {
                grbServices.Visible = true;
                grbCost.Visible = false;
            }
            else if (rbCollectionCenter.Checked == true)
            {
                grbServices.Visible = true;
                grbCost.Visible = false;
            }
            else
            {
                grbCost.Visible = true;
                grbServices.Visible = false;
                txtMPOSCost.Text = string.Empty;
                txtMALLCost.Text = string.Empty;
            }
        }

        private void txtMPOSCost_TextChanged(object sender, EventArgs e)
        {
            txtMPOSCost.CausesValidation = false;
        }

        private void txtMALLCost_TextChanged(object sender, EventArgs e)
        {
            txtMALLCost.CausesValidation = false;
        }

        private void chkVendorServices_ItemCheck_1(object sender, ItemCheckEventArgs e)
        {
            e.NewValue = CheckState.Checked;
        }

        private void txtMPOSCost_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtMALLCost_KeyPress(object sender, KeyPressEventArgs e)
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

        #endregion Event Methods

        #region Public Methods

        public void LoadData()
        {
            try
            {
                if (this._mode == OperationMode.Edit)
                {
                    Vendor vendor = vendorBL.GetAddress(this._vendorId);

                    if (vendor != null)
                    {
                        if (vendor.VendorTypeId == VendorTypes.CollectionCenter)
                        {
                            rbCollectionCenter.Checked = true;
                        }
                        else if (vendor.VendorTypeId == VendorTypes.Lab)
                        {
                            rbLab.Checked = true;
                        }
                        else if (vendor.VendorTypeId == VendorTypes.MRO)
                        {
                            rbMRO.Checked = true;
                        }
                        else if (vendor.VendorTypeId == VendorTypes.None)
                        {
                            rbCollectionCenter.Checked = false;
                            rbLab.Checked = false;
                            rbMRO.Checked = false;
                        }

                        txtVendorName.Text = vendor.VendorName;
                        txtMainContact.Text = vendor.VendorMainContact;
                        txtPhoneNumber.Text = vendor.VendorPhone;
                        txtFaxNumber.Text = vendor.VendorFax;
                        txtEmail.Text = vendor.VendorEmail;

                        if (vendor.VendorStatus == VendorStatus.Active)
                        {
                            rbActive.Checked = true;
                            // txtInactiveDate.Text = string.Empty;
                            cmbMonth.SelectedIndex = 0;
                            cmbDate.SelectedIndex = 0;
                            cmbYear.SelectedIndex = 0;
                            txtInactiveReason.Text = string.Empty;
                            // txtInactiveDate.Visible = false;
                            cmbMonth.Visible = false;
                            cmbDate.Visible = false;
                            cmbYear.Visible = false;
                            lblInactiveSpecify.Visible = false;
                            lblInactiveDate.Visible = false;
                            txtInactiveReason.Visible = false;
                        }
                        else if (vendor.VendorStatus == VendorStatus.Inactive)
                        {
                            rbInActive.Checked = true;
                            // txtInactiveDate.Text = vendor.InactiveDate.ToString("MM/dd/yyyy");
                            DateTime inActiveDate = Convert.ToDateTime(vendor.InactiveDate.ToString("MM/dd/yyyy"));
                            cmbMonth.Text = inActiveDate.ToString("MM");
                            cmbDate.Text = inActiveDate.ToString("dd");
                            cmbYear.Text = inActiveDate.ToString("yyyy");
                            txtInactiveReason.Text = vendor.InactiveReason;

                            // txtInactiveDate.Visible = true;
                            cmbMonth.Visible = true;
                            cmbDate.Visible = true;
                            cmbYear.Visible = true;
                            lblInactiveSpecify.Visible = true;
                            lblInactiveDate.Visible = true;
                            txtInactiveReason.Visible = true;
                        }
                        else if (vendor.VendorStatus == (int)VendorStatus.None)
                        {
                            rbActive.Checked = false;
                            rbInActive.Checked = false;
                            //  txtInactiveDate.Text = string.Empty;
                            cmbMonth.SelectedIndex = 0;
                            cmbDate.SelectedIndex = 0;
                            cmbYear.SelectedIndex = 0;
                            txtInactiveReason.Text = string.Empty;

                            // txtInactiveDate.Visible = false;
                            cmbMonth.Visible = false;
                            cmbDate.Visible = false;
                            cmbYear.Visible = false;
                            lblInactiveSpecify.Visible = false;
                            lblInactiveDate.Visible = false;
                            txtInactiveReason.Visible = false;
                        }

                        int AddressType = (int)vendor.IsMailingAddressPhysical1;
                        cmbSameasPhysical.SelectedIndex = AddressType;

                        if (vendor.VendorTypeId == VendorTypes.MRO)
                        {
                            grbServices.Visible = false;
                            grbCost.Visible = true;
                            txtMPOSCost.Text = ((double)vendor.MPOSMROCost).ToString();
                            txtMALLCost.Text = ((double)vendor.MALLMROCost).ToString();
                        }
                        else
                        {
                            grbServices.Visible = false;
                            grbCost.Visible = true;
                            txtMPOSCost.Text = string.Empty;
                            txtMALLCost.Text = string.Empty;
                        }

                        LoadVendorService();

                        if (vendor.VendorTypeId == VendorTypes.CollectionCenter)
                        {
                            grbServices.Visible = true;
                            grbCost.Visible = false;
                            chkVendorServices.ValueList = vendor.Services;
                        }
                        else if (vendor.VendorTypeId == VendorTypes.Lab)
                        {
                            grbServices.Visible = true;
                            grbCost.Visible = false;
                            chkVendorServices.ValueList = vendor.Services;
                        }

                        foreach (VendorAddress address in vendor.Addresses)
                        {
                            if (address.AddressTypeId == AddressTypes.PhysicalAddress1)
                            {
                                txtPhysical1Address1.Text = address.Address1;
                                txtPhysical1Address2.Text = address.Address2;
                                txtPhysical1City.Text = address.City;
                                cmbPhysical1State.Text = address.State;
                                txtPhysical1ZipCode.Text = address.ZipCode;
                                txtPhysical1PhoneNumber.Text = address.Phone;
                                txtPhysical1FaxNumber.Text = address.Fax;
                                txtPhysical1Email.Text = address.Email;
                            }

                            if (address.AddressTypeId == AddressTypes.PhysicalAddress2)
                            {
                                txtPhysical2Address1.Text = address.Address1;
                                txtPhysical2Address2.Text = address.Address2;
                                txtPhysical2City.Text = address.City;
                                cmbPhysical2State.Text = address.State;
                                txtPhysical2ZipCode.Text = address.ZipCode;
                                txtPhysical2PhoneNumber.Text = address.Phone;
                                txtPhysical2FaxNumber.Text = address.Fax;
                                txtPhysical2Email.Text = address.Email;
                            }

                            if (address.AddressTypeId == AddressTypes.PhysicalAddress3)
                            {
                                txtPhysical3Address1.Text = address.Address1;
                                txtPhysical3Address2.Text = address.Address2;
                                txtPhysical3City.Text = address.City;
                                cmbPhysical3State.Text = address.State;
                                txtPhysical3ZipCode.Text = address.ZipCode;
                                txtPhysical3PhoneNumber.Text = address.Phone;
                                txtPhysical3FaxNumber.Text = address.Fax;
                                txtPhysical3Email.Text = address.Email;
                            }

                            if (address.AddressTypeId == AddressTypes.MailingAddress && cmbSameasPhysical.SelectedIndex == 0)
                            {
                                txtMailingAddress1.Text = address.Address1;
                                txtMailingAddress2.Text = address.Address2;
                                txtMailingCity.Text = address.City;
                                cmbMailingState.Text = address.State;
                                txtMailingZipCode.Text = address.ZipCode;

                                txtMailingAddress1.Enabled = true;
                                txtMailingAddress2.Enabled = true;
                                txtMailingCity.Enabled = true;
                                cmbMailingState.Enabled = true;
                                txtMailingZipCode.Enabled = true;
                            }
                            else if (cmbSameasPhysical.SelectedIndex == 1)
                            {
                                txtMailingAddress1.Text = txtPhysical1Address1.Text;
                                txtMailingAddress2.Text = txtPhysical1Address2.Text;
                                txtMailingCity.Text = txtPhysical1City.Text;
                                cmbMailingState.Text = cmbPhysical1State.Text;
                                txtMailingZipCode.Text = txtPhysical1ZipCode.Text;
                            }
                            else if (cmbSameasPhysical.SelectedIndex == 2)
                            {
                                txtMailingAddress1.Text = txtPhysical2Address1.Text;
                                txtMailingAddress2.Text = txtPhysical2Address2.Text;
                                txtMailingCity.Text = txtPhysical2City.Text;
                                cmbMailingState.Text = cmbPhysical2State.Text;
                                txtMailingZipCode.Text = txtPhysical2ZipCode.Text;
                            }
                            else if (cmbSameasPhysical.SelectedIndex == 3)
                            {
                                txtMailingAddress1.Text = txtPhysical3Address1.Text;
                                txtMailingAddress2.Text = txtPhysical3Address2.Text;
                                txtMailingCity.Text = txtPhysical3City.Text;
                                cmbMailingState.Text = cmbPhysical3State.Text;
                                txtMailingZipCode.Text = txtPhysical3ZipCode.Text;
                            }
                        }

                        if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()) && viewFlag == true)
                        {
                            cmbMailingState.Enabled = false;
                        }

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
            chkVendorServices.Items.Clear();

            rbCollectionCenter.Checked = true;
            rbActive.Checked = true;
            chkSamePhysical1.Checked = false;

            cmbPhysical1State.SelectedIndex = 0;
            cmbPhysical2State.SelectedIndex = 0;
            cmbPhysical3State.SelectedIndex = 0;
            cmbMailingState.SelectedIndex = 0;
            cmbSameasPhysical.SelectedIndex = 0;

            txtMailingAddress1.Enabled = false;
            txtMailingAddress2.Enabled = false;
            txtMailingCity.Enabled = false;
            cmbMailingState.Enabled = false;
            txtMailingZipCode.Enabled = false;
            // cmbSameasPhysical.Enabled = false;

            cmbYear.Items.Clear();
            var myDate = DateTime.Now;
            var newDate = myDate.AddYears(+15).Year;
            for (int i = 1950; i <= newDate; i++)
            {
                cmbYear.Items.Add(i);
            }
            cmbYear.Items.Insert(0, "YYYY");
            cmbMonth.SelectedIndex = 0;
            cmbDate.SelectedIndex = 0;
            cmbYear.SelectedIndex = 0;

            LoadVendorService();

            if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
            {
                //VENDOR_VENDOR_SERVICE_ADD
                DataRow[] vendorVendorServiceAdd = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.VENDOR_VENDOR_SERVICE_ADD.ToDescriptionString() + "'");

                if (vendorVendorServiceAdd.Length > 0)
                {
                    btnVendorService.Visible = true;
                    btnVendorServicesDetails.Visible = false;
                }

                //VENDOR_VENDOR_SERVICE_EDIT
                DataRow[] vendorServiceInfoEdit = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.VENDOR_VENDOR_SERVICE_EDIT.ToDescriptionString() + "'");

                if (vendorServiceInfoEdit.Length > 0)
                {
                    btnVendorService.Visible = false;
                    btnVendorServicesDetails.Visible = true;
                }
                if (vendorVendorServiceAdd.Length > 0)
                {
                    btnVendorService.Visible = true;
                }

                //VENDOR_VENDOR_SERVICE_ARCHIVE
                DataRow[] vendorServiceInfoArchive = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.VENDOR_VENDOR_SERVICE_ARCHIVE.ToDescriptionString() + "'");

                if (vendorServiceInfoArchive.Length > 0)
                {
                    btnVendorService.Visible = false;
                    btnVendorServicesDetails.Visible = true;
                }
                if (vendorVendorServiceAdd.Length > 0)
                {
                    btnVendorService.Visible = true;
                }
                if (vendorVendorServiceAdd.Length > 0 && vendorServiceInfoEdit.Length > 0 && vendorServiceInfoArchive.Length > 0)
                {
                    btnVendorService.Visible = true;
                    btnVendorServicesDetails.Visible = true;
                }

                //VENDOR_VIEW
                DataRow[] vendorView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.VENDOR_VIEW.ToDescriptionString() + "'");

                if (vendorView.Length > 0)
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
                        if (ctrl is Panel)
                        {
                            ((Panel)ctrl).Enabled = false;
                        }
                        if (ctrl is RadioButton)
                        {
                            ((RadioButton)ctrl).Enabled = false;
                        }
                        if (ctrl is CheckedListBox)
                        {
                            ((CheckedListBox)ctrl).Enabled = false;
                        }
                        if (ctrl is GroupBox)
                        {
                            ((GroupBox)ctrl).Enabled = false;
                        }
                    }
                    viewFlag = true;

                    if (vendorVendorServiceAdd.Length > 0)
                    {
                        grbServices.Enabled = true;
                        chkVendorServices.Enabled = false;
                        btnVendorService.Enabled = true;
                    }
                    if (vendorServiceInfoEdit.Length > 0)
                    {
                        grbServices.Enabled = true;
                        chkVendorServices.Enabled = false;
                        btnVendorServicesDetails.Enabled = true;
                    }
                    if (vendorServiceInfoArchive.Length > 0)
                    {
                        grbServices.Enabled = true;
                        chkVendorServices.Enabled = false;
                        btnVendorServicesDetails.Enabled = true;
                    }
                }
            }
            if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
            {
                //VENDOR_VENDOR_SERVICE_VIEW
                DataRow[] vendorServiceView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.VENDOR_VENDOR_SERVICE_VIEW.ToDescriptionString() + "'");

                if (vendorServiceView.Length > 0)
                {
                    if (rbCollectionCenter.Checked == true || rbLab.Checked == true)
                    {
                        grbServices.Enabled = true;
                        chkVendorServices.Enabled = false;
                        btnVendorService.Enabled = false;
                        btnVendorServicesDetails.Enabled = true;
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

                Vendor vendor = null;

                if (this._mode == OperationMode.New)
                {
                    vendor = new Vendor();
                    vendor.VendorId = 0;
                    vendor.CreatedBy = Program.currentUserName;
                }
                else if (this._mode == OperationMode.Edit)
                {
                    vendor = vendorBL.Get(this._vendorId);
                }

                if (rbCollectionCenter.Checked == true)
                {
                    vendor.VendorTypeId = VendorTypes.CollectionCenter;
                }
                else if (rbLab.Checked == true)
                {
                    vendor.VendorTypeId = VendorTypes.Lab;
                }
                else if (rbMRO.Checked == true)
                {
                    vendor.VendorTypeId = VendorTypes.MRO;
                }
                else
                {
                    vendor.VendorTypeId = VendorTypes.None;
                }

                vendor.VendorName = txtVendorName.Text.Trim();
                vendor.VendorMainContact = txtMainContact.Text.Trim();

                if (txtPhoneNumber.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") != string.Empty)
                {
                    vendor.VendorPhone = txtPhoneNumber.Text.Trim();
                }
                else
                {
                    vendor.VendorPhone = string.Empty;
                }

                if (txtFaxNumber.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") != string.Empty)
                {
                    vendor.VendorFax = txtFaxNumber.Text.Trim();
                }
                else
                {
                    vendor.VendorFax = string.Empty;
                }

                vendor.VendorEmail = txtEmail.Text.Trim();

                if (rbActive.Checked == true)
                {
                    vendor.VendorStatus = VendorStatus.Active;
                    vendor.InactiveDate = DateTime.MinValue;
                    vendor.InactiveReason = string.Empty;
                }
                else if (rbInActive.Checked == true)
                {
                    vendor.VendorStatus = VendorStatus.Inactive;
                    string inActiveDate = cmbYear.Text + '-' + cmbMonth.Text + '-' + cmbDate.Text;
                    try
                    {
                        DateTime inactiveDate = Convert.ToDateTime(inActiveDate.ToString());
                        if (inactiveDate > DateTime.Today)
                        {
                            MessageBox.Show("Invalid date.");
                            cmbMonth.Focus();
                            return false;
                        }
                        else
                        {
                            vendor.InactiveDate = Convert.ToDateTime(inActiveDate.Trim());
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Invalid Date Format");
                        cmbMonth.Focus();
                        return false;
                    }
                    //vendor.InactiveDate = Convert.ToDateTime(txtInactiveDate.Text.Trim(), Program.culture);
                    vendor.InactiveReason = txtInactiveReason.Text.Trim();
                }
                else
                {
                    vendor.VendorStatus = (int)SurPath.Enum.VendorStatus.None;
                    vendor.InactiveDate = DateTime.MinValue;
                    vendor.InactiveReason = string.Empty;
                }

                vendor.IsMailingAddressPhysical1 = (int)cmbSameasPhysical.SelectedIndex;

                if (txtMPOSCost.Text != string.Empty)
                {
                    try
                    {
                        vendor.MPOSMROCost = Convert.ToDouble(txtMPOSCost.Text.ToString());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("MPOS Cost Must Have only numeric values");
                        txtMPOSCost.Focus();
                        return false;
                    }
                }
                else
                {
                    vendor.MPOSMROCost = null;
                }

                if (txtMALLCost.Text != string.Empty)
                {
                    try
                    {
                        vendor.MALLMROCost = Convert.ToDouble(txtMALLCost.Text.ToString());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("MPOS Cost Must Have only numeric values");
                        txtMALLCost.Focus();
                        return false;
                    }
                }
                else
                {
                    vendor.MALLMROCost = null;
                }

                #region Physical Address 1

                if (_isPhysical1Required)
                {
                    VendorAddress physicalAddress1 = null;
                    if (this._mode == OperationMode.New)
                    {
                        physicalAddress1 = new VendorAddress();
                        physicalAddress1.AddressTypeId = AddressTypes.PhysicalAddress1;
                        vendor.Addresses.Add(physicalAddress1);
                    }
                    else
                    {
                        foreach (VendorAddress address in vendor.Addresses)
                        {
                            if (address.AddressTypeId == AddressTypes.PhysicalAddress1)
                            {
                                physicalAddress1 = address;
                                break;
                            }
                        }

                        if (physicalAddress1 == null)
                        {
                            physicalAddress1 = new VendorAddress();
                            physicalAddress1.AddressTypeId = AddressTypes.PhysicalAddress1;
                            vendor.Addresses.Add(physicalAddress1);
                        }
                    }

                    physicalAddress1.Address1 = txtPhysical1Address1.Text.Trim();
                    physicalAddress1.Address2 = txtPhysical1Address2.Text.Trim();
                    physicalAddress1.City = txtPhysical1City.Text.Trim();
                    physicalAddress1.State = cmbPhysical1State.Text.Trim();

                    string physical1ZipCode = txtPhysical1ZipCode.Text.Trim();
                    physical1ZipCode = physical1ZipCode.EndsWith("-") ? physical1ZipCode.Replace("-", "") : physical1ZipCode;

                    physicalAddress1.ZipCode = physical1ZipCode;

                    if (txtPhysical1PhoneNumber.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") != string.Empty)
                    {
                        physicalAddress1.Phone = txtPhysical1PhoneNumber.Text.Trim();
                    }
                    else
                    {
                        physicalAddress1.Phone = string.Empty;
                    }

                    if (txtPhysical1FaxNumber.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") != string.Empty)
                    {
                        physicalAddress1.Fax = txtPhysical1FaxNumber.Text.Trim();
                    }
                    else
                    {
                        physicalAddress1.Fax = string.Empty;
                    }

                    physicalAddress1.Email = txtPhysical1Email.Text.Trim();

                    physicalAddress1.LastModifiedBy = Program.currentUserName;
                }

                #endregion Physical Address 1

                #region Physical Address 2

                if (_isPhysical2Required)
                {
                    VendorAddress physicalAddress2 = null;
                    if (this._mode == OperationMode.New)
                    {
                        physicalAddress2 = new VendorAddress();
                        physicalAddress2.AddressTypeId = AddressTypes.PhysicalAddress2;
                        vendor.Addresses.Add(physicalAddress2);
                    }
                    else
                    {
                        foreach (VendorAddress address in vendor.Addresses)
                        {
                            if (address.AddressTypeId == AddressTypes.PhysicalAddress2)
                            {
                                physicalAddress2 = address;
                                break;
                            }
                        }

                        if (physicalAddress2 == null)
                        {
                            physicalAddress2 = new VendorAddress();
                            physicalAddress2.AddressTypeId = AddressTypes.PhysicalAddress2;
                            vendor.Addresses.Add(physicalAddress2);
                        }
                    }

                    physicalAddress2.Address1 = txtPhysical2Address1.Text.Trim();
                    physicalAddress2.Address2 = txtPhysical2Address2.Text.Trim();
                    physicalAddress2.City = txtPhysical2City.Text.Trim();
                    physicalAddress2.State = cmbPhysical2State.Text.Trim();

                    string physical2ZipCode = txtPhysical2ZipCode.Text.Trim();
                    physical2ZipCode = physical2ZipCode.EndsWith("-") ? physical2ZipCode.Replace("-", "") : physical2ZipCode;

                    physicalAddress2.ZipCode = physical2ZipCode;

                    if (txtPhysical2PhoneNumber.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") != string.Empty)
                    {
                        physicalAddress2.Phone = txtPhysical2PhoneNumber.Text.Trim();
                    }
                    else
                    {
                        physicalAddress2.Phone = string.Empty;
                    }

                    if (txtPhysical2FaxNumber.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") != string.Empty)
                    {
                        physicalAddress2.Fax = txtPhysical2FaxNumber.Text.Trim();
                    }
                    else
                    {
                        physicalAddress2.Fax = string.Empty;
                    }

                    physicalAddress2.Email = txtPhysical2Email.Text.Trim();

                    physicalAddress2.LastModifiedBy = Program.currentUserName;
                }

                #endregion Physical Address 2

                #region Physical Address 3

                if (_isPhysical3Required)
                {
                    VendorAddress physicalAddress3 = null;
                    if (this._mode == OperationMode.New)
                    {
                        physicalAddress3 = new VendorAddress();
                        physicalAddress3.AddressTypeId = AddressTypes.PhysicalAddress3;
                        vendor.Addresses.Add(physicalAddress3);
                    }
                    else
                    {
                        foreach (VendorAddress address in vendor.Addresses)
                        {
                            if (address.AddressTypeId == AddressTypes.PhysicalAddress3)
                            {
                                physicalAddress3 = address;
                                break;
                            }
                        }

                        if (physicalAddress3 == null)
                        {
                            physicalAddress3 = new VendorAddress();
                            physicalAddress3.AddressTypeId = AddressTypes.PhysicalAddress3;
                            vendor.Addresses.Add(physicalAddress3);
                        }
                    }

                    physicalAddress3.Address1 = txtPhysical3Address1.Text.Trim();
                    physicalAddress3.Address2 = txtPhysical3Address2.Text.Trim();
                    physicalAddress3.City = txtPhysical3City.Text.Trim();
                    physicalAddress3.State = cmbPhysical3State.Text.Trim();

                    string physical3ZipCode = txtPhysical3ZipCode.Text.Trim();
                    physical3ZipCode = physical3ZipCode.EndsWith("-") ? physical3ZipCode.Replace("-", "") : physical3ZipCode;

                    physicalAddress3.ZipCode = physical3ZipCode;

                    if (txtPhysical3PhoneNumber.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") != string.Empty)
                    {
                        physicalAddress3.Phone = txtPhysical3PhoneNumber.Text.Trim();
                    }
                    else
                    {
                        physicalAddress3.Phone = string.Empty;
                    }

                    if (txtPhysical3FaxNumber.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") != string.Empty)
                    {
                        physicalAddress3.Fax = txtPhysical3FaxNumber.Text.Trim();
                    }
                    else
                    {
                        physicalAddress3.Fax = string.Empty;
                    }

                    physicalAddress3.Email = txtPhysical3Email.Text.Trim();

                    physicalAddress3.LastModifiedBy = Program.currentUserName;
                }

                #endregion Physical Address 3

                #region Mailing Address

                _isMailingRequired = true;

                if (_isMailingRequired)
                {
                    VendorAddress mailingAddress = null;
                    if (this._mode == OperationMode.New)
                    {
                        mailingAddress = new VendorAddress();
                        mailingAddress.AddressTypeId = AddressTypes.MailingAddress;
                        vendor.Addresses.Add(mailingAddress);
                    }
                    else
                    {
                        foreach (VendorAddress address in vendor.Addresses)
                        {
                            if (address.AddressTypeId == AddressTypes.MailingAddress)
                            {
                                mailingAddress = address;
                                break;
                            }
                        }

                        if (mailingAddress == null)
                        {
                            mailingAddress = new VendorAddress();
                            mailingAddress.AddressTypeId = AddressTypes.MailingAddress;
                            vendor.Addresses.Add(mailingAddress);
                        }
                    }

                    mailingAddress.Address1 = txtMailingAddress1.Text.Trim();
                    mailingAddress.Address2 = txtMailingAddress2.Text.Trim();
                    mailingAddress.City = txtMailingCity.Text.Trim();
                    mailingAddress.State = cmbMailingState.Text.Trim();

                    string mailingZipCode = txtMailingZipCode.Text.Trim();
                    mailingZipCode = mailingZipCode.EndsWith("-") ? mailingZipCode.Replace("-", "") : mailingZipCode;

                    mailingAddress.ZipCode = mailingZipCode;

                    mailingAddress.Phone = string.Empty;
                    mailingAddress.Fax = string.Empty;
                    mailingAddress.Email = string.Empty;

                    mailingAddress.LastModifiedBy = Program.currentUserName;
                }

                #endregion Mailing Address

                vendor.LastModifiedBy = Program.currentUserName;

                int returnVal = vendorBL.Save(vendor);

                if (returnVal > 0)
                {
                    if (this._mode == OperationMode.New)
                    {
                        vendor.VendorId = returnVal;
                        this._vendorId = returnVal;
                        this._mode = OperationMode.Edit;
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
            if (txtVendorName.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Vendor Name cannot be empty.");
                txtVendorName.Focus();
                return false;
            }

            if (txtMainContact.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Main Contact cannot be empty.");
                txtMainContact.Focus();
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

            if (txtEmail.Text.Trim() != string.Empty)
            {
                if (!Program.regexEmail.IsMatch(txtEmail.Text.Trim()))
                {
                    MessageBox.Show("Invalid format of Email.");
                    txtEmail.Focus();
                    return false;
                }
                else if (!ValidateEmail())
                {
                    MessageBox.Show("The email provided is already exists in database.");
                    txtEmail.Focus();
                    return false;
                }
            }

            if (rbCollectionCenter.Checked == false && rbLab.Checked == false && rbMRO.Checked == false)
            {
                MessageBox.Show("Vendor Types cannot be empty.");
                rbCollectionCenter.Focus();
                return false;
            }

            if (rbMRO.Checked == true)
            {
                if (txtMPOSCost.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("MPOS Cost cannot be empty.");
                    txtMPOSCost.Focus();
                    return false;
                }
                else if (txtMALLCost.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("MALL Cost cannot be empty.");
                    txtMALLCost.Focus();
                    return false;
                }
            }

            if (rbActive.Checked == false && rbInActive.Checked == false)
            {
                MessageBox.Show("Vendor Status cannot be empty.");
                rbActive.Focus();
                return false;
            }

            if (rbInActive.Checked)
            {
                if (cmbMonth.SelectedIndex == 0)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Inactive Date cannot be empty.");
                    cmbMonth.Focus();
                    return false;
                }
                if (cmbDate.SelectedIndex == 0)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Inactive Date cannot be empty.");
                    cmbDate.Focus();
                    return false;
                }
                if (cmbYear.SelectedIndex == 0)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Inactive Date cannot be empty.");
                    cmbYear.Focus();
                    return false;
                }

                //if (txtInactiveDate.Text.Trim().Replace("/", "").Replace("_", "").Replace(" ", "") != string.Empty)
                //{
                //    try
                //    {
                //        DateTime inactiveDate = Convert.ToDateTime(txtInactiveDate.Text.Trim(), Program.culture);

                //        if (inactiveDate > DateTime.Today)
                //        {
                //            MessageBox.Show("Invalid date.");
                //            txtInactiveDate.Focus();
                //            return false;
                //        }
                //    }
                //    catch
                //    {
                //        MessageBox.Show("Invalid format of date.");
                //        txtInactiveDate.Focus();
                //        return false;
                //    }
                //}
                //else
                //{
                //    MessageBox.Show("Inactive Date can not be empty.");
                //    txtInactiveDate.Focus();
                //    return false;
                //}

                if (txtInactiveReason.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Reason for inactive cannot be empty.");
                    txtInactiveReason.Focus();
                    return false;
                }
            }

            _isPhysical1Required = true;
            _isPhysical2Required = false;
            _isPhysical3Required = false;
            _isMailingRequired = false;

            #region Physical Address 1 validation

            string physical1Address1 = txtPhysical1Address1.Text.Trim();
            string physical1Address2 = txtPhysical1Address2.Text.Trim();
            string physical1City = txtPhysical1City.Text.Trim();
            string physical1State = cmbPhysical1State.Text.Trim();
            string physical1ZipCode = txtPhysical1ZipCode.Text.Trim();
            string physical1Phone = txtPhysical1PhoneNumber.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
            string physical1Fax = txtPhysical1FaxNumber.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
            string physical1Email = txtPhysical1Email.Text.Trim();

            physical1ZipCode = physical1ZipCode.EndsWith("-") ? physical1ZipCode.Replace("-", "") : physical1ZipCode;

            if (_isPhysical1Required)
            {
                if (physical1Address1 == string.Empty)
                {
                    MessageBox.Show("Physical Address 1 - Address 1 cannot be empty.");
                    txtPhysical1Address1.Focus();
                    return false;
                }

                if (physical1City == string.Empty)
                {
                    MessageBox.Show("Physical Address 1 - City cannot be empty.");
                    txtPhysical1City.Focus();
                    return false;
                }

                if (physical1State.ToUpper() == "(Select)".ToUpper())
                {
                    MessageBox.Show("Physical Address 1 -State must be selected.");
                    cmbPhysical1State.Focus();
                    return false;
                }

                if (physical1ZipCode == string.Empty)
                {
                    MessageBox.Show("Physical Address 1 - Zip Code cannot be empty.");
                    txtPhysical1ZipCode.Focus();
                    return false;
                }
                else
                {
                    if (!Program.regexZipCode.IsMatch(physical1ZipCode))
                    {
                        MessageBox.Show("Invalid format of Zip Code (Physical Address 1).");
                        txtPhysical1ZipCode.Focus();
                        return false;
                    }
                }

                if (physical1Phone == string.Empty)
                {
                    MessageBox.Show("Physical Address 1 - Phone number cannot be empty.");
                    txtPhysical1PhoneNumber.Focus();
                    return false;
                }
                else
                {
                    if (!Program.regexPhoneNumber.IsMatch(txtPhysical1PhoneNumber.Text.Trim()))
                    {
                        MessageBox.Show("Invalid format of Phone number (Physical Address 1).");
                        txtPhysical1PhoneNumber.Focus();
                        return false;
                    }
                }

                if (physical1Fax != string.Empty)
                {
                    if (!Program.regexPhoneNumber.IsMatch(txtPhysical1FaxNumber.Text.Trim()))
                    {
                        MessageBox.Show("Invalid format of Fax number (Physical Address 1).");
                        txtPhysical1FaxNumber.Focus();
                        return false;
                    }
                }

                if (physical1Email == string.Empty)
                {
                    MessageBox.Show("Physical Address 1 - Email cannot be empty.");
                    txtPhysical1Email.Focus();
                    return false;
                }
                else
                {
                    if (!Program.regexEmail.IsMatch(txtPhysical1Email.Text.Trim()))
                    {
                        MessageBox.Show("Invalid format of Email (Physical Address 1).");
                        txtPhysical1Email.Focus();
                        return false;
                    }
                }
            }

            #endregion Physical Address 1 validation

            #region Physical Address 2 validation

            string physical2Address1 = txtPhysical2Address1.Text.Trim();
            string physical2Address2 = txtPhysical2Address2.Text.Trim();
            string physical2City = txtPhysical2City.Text.Trim();
            string physical2State = cmbPhysical2State.Text.Trim();
            string physical2ZipCode = txtPhysical2ZipCode.Text.Trim();
            string physical2Phone = txtPhysical2PhoneNumber.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
            string physical2Fax = txtPhysical2FaxNumber.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
            string physical2Email = txtPhysical2Email.Text.Trim();

            physical2ZipCode = physical2ZipCode.EndsWith("-") ? physical2ZipCode.Replace("-", "") : physical2ZipCode;

            if (physical2Address1 != string.Empty
                || physical2Address2 != string.Empty
                || physical2City != string.Empty
                || physical2State != "(Select)"
                || physical2ZipCode != string.Empty
                || physical2Phone != string.Empty
                || physical2Fax != string.Empty
                || physical2Email != string.Empty)
            {
                _isPhysical2Required = true;
            }

            if (_isPhysical2Required)
            {
                if (physical2Address1 == string.Empty)
                {
                    MessageBox.Show("Physical Address 2 - Address 1 cannot be empty.");
                    txtPhysical2Address1.Focus();
                    return false;
                }

                if (physical2City == string.Empty)
                {
                    MessageBox.Show("Physical Address 2 - City cannot be empty.");
                    txtPhysical2City.Focus();
                    return false;
                }

                if (physical2State.ToUpper() == "(Select)".ToUpper())
                {
                    MessageBox.Show("Physical Address 2 - State must be selected.");
                    cmbPhysical2State.Focus();
                    return false;
                }

                if (physical2ZipCode == string.Empty)
                {
                    MessageBox.Show("Physical Address 2 - Zip Code cannot be empty.");
                    txtPhysical2ZipCode.Focus();
                    return false;
                }
                else
                {
                    if (!Program.regexZipCode.IsMatch(physical2ZipCode))
                    {
                        MessageBox.Show("Invalid format of Zip Code (Physical Address 2).");
                        txtPhysical2ZipCode.Focus();
                        return false;
                    }
                }

                if (physical2Phone == string.Empty)
                {
                    MessageBox.Show("Physical Address 2 - Phone number cannot be empty.");
                    txtPhysical2PhoneNumber.Focus();
                    return false;
                }
                else
                {
                    if (!Program.regexPhoneNumber.IsMatch(txtPhysical2PhoneNumber.Text.Trim()))
                    {
                        MessageBox.Show("Invalid format of Phone number (Physical Address 2).");
                        txtPhysical2PhoneNumber.Focus();
                        return false;
                    }
                }

                if (physical2Fax != string.Empty)
                {
                    if (!Program.regexPhoneNumber.IsMatch(txtPhysical2FaxNumber.Text.Trim()))
                    {
                        MessageBox.Show("Invalid format of Fax number (Physical Address 2).");
                        txtPhysical2FaxNumber.Focus();
                        return false;
                    }
                }

                if (physical2Email == string.Empty)
                {
                    MessageBox.Show("Physical Address 2 - Email cannot be empty.");
                    txtPhysical2Email.Focus();
                    return false;
                }
                else
                {
                    if (!Program.regexEmail.IsMatch(txtPhysical2Email.Text.Trim()))
                    {
                        MessageBox.Show("Invalid format of Email (Physical Address 2).");
                        txtPhysical2Email.Focus();
                        return false;
                    }
                }
            }

            #endregion Physical Address 2 validation

            #region Physical Address 3 validation

            string physical3Address1 = txtPhysical3Address1.Text.Trim();
            string physical3Address2 = txtPhysical3Address2.Text.Trim();
            string physical3City = txtPhysical3City.Text.Trim();
            string physical3State = cmbPhysical3State.Text.Trim();
            string physical3ZipCode = txtPhysical3ZipCode.Text.Trim();
            string physical3Phone = txtPhysical3PhoneNumber.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
            string physical3Fax = txtPhysical3FaxNumber.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
            string physical3Email = txtPhysical3Email.Text.Trim();

            physical3ZipCode = physical3ZipCode.EndsWith("-") ? physical3ZipCode.Replace("-", "") : physical3ZipCode;

            if (physical3Address1 != string.Empty
                || physical3Address2 != string.Empty
                || physical3City != string.Empty
                || physical3State != "(Select)"
                || physical3ZipCode != string.Empty
                || physical3Phone != string.Empty
                || physical3Fax != string.Empty
                || physical3Email != string.Empty)
            {
                _isPhysical3Required = true;
            }

            if (_isPhysical3Required)
            {
                if (physical3Address1 == string.Empty)
                {
                    MessageBox.Show("Physical Address 3 - Address 1 cannot be empty.");
                    txtPhysical3Address1.Focus();
                    return false;
                }

                if (physical3City == string.Empty)
                {
                    MessageBox.Show("Physical Address 3 - City cannot be empty.");
                    txtPhysical3City.Focus();
                    return false;
                }

                if (physical3State.ToUpper() == "(Select)".ToUpper())
                {
                    MessageBox.Show("Physical Address 3 - State must be selected .");
                    cmbPhysical3State.Focus();
                    return false;
                }

                if (physical3ZipCode == string.Empty)
                {
                    MessageBox.Show("Physical Address 3 - Zip Code cannot be empty.");
                    txtPhysical3ZipCode.Focus();
                    return false;
                }
                else
                {
                    if (!Program.regexZipCode.IsMatch(physical3ZipCode))
                    {
                        MessageBox.Show("Invalid format of Zip Code (Physical Address 3).");
                        txtPhysical3ZipCode.Focus();
                        return false;
                    }
                }

                if (physical3Phone == string.Empty)
                {
                    MessageBox.Show("Physical Address 3 - Phone number cannot be empty.");
                    txtPhysical3PhoneNumber.Focus();
                    return false;
                }
                else
                {
                    if (!Program.regexPhoneNumber.IsMatch(txtPhysical3PhoneNumber.Text.Trim()))
                    {
                        MessageBox.Show("Invalid format of Phone number (Physical Address 3).");
                        txtPhysical3PhoneNumber.Focus();
                        return false;
                    }
                }

                if (physical3Fax != string.Empty)
                {
                    if (!Program.regexPhoneNumber.IsMatch(txtPhysical3FaxNumber.Text.Trim()))
                    {
                        MessageBox.Show("Invalid format of Fax number (Physical Address 3).");
                        txtPhysical3FaxNumber.Focus();
                        return false;
                    }
                }

                if (physical3Email == string.Empty)
                {
                    MessageBox.Show("Physical Address 3 - Email cannot be empty.");
                    txtPhysical3Email.Focus();
                    return false;
                }
                else
                {
                    if (!Program.regexEmail.IsMatch(txtPhysical3Email.Text.Trim()))
                    {
                        MessageBox.Show("Invalid format of Email (Physical Address 3).");
                        txtPhysical3Email.Focus();
                        return false;
                    }
                }
            }

            #endregion Physical Address 3 validation

            #region Mailing Address Validation

            //if (!chkSamePhysical1.Checked)
            //{
            //    string mailingAddress1 = txtMailingAddress1.Text.Trim();
            //    string mailingAddress2 = txtMailingAddress2.Text.Trim();
            //    string mailingCity = txtMailingCity.Text.Trim();
            //    string mailingState = cmbMailingState.Text.Trim();
            //    string mailingZipCode = txtMailingZipCode.Text.Trim();

            //    mailingZipCode = mailingZipCode.EndsWith("-") ? mailingZipCode.Replace("-", "") : mailingZipCode;

            //    _isMailingRequired = true;

            //    if (_isMailingRequired)
            //    {
            //        if (mailingAddress1 == string.Empty)
            //        {
            //            MessageBox.Show("Mailing Address - Address 1 can not be empty.");
            //            txtMailingAddress1.Focus();
            //            return false;
            //        }

            //        if (mailingCity == string.Empty)
            //        {
            //            MessageBox.Show("Mailing Address - City can not be empty.");
            //            txtMailingCity.Focus();
            //            return false;
            //        }

            //        if (mailingState.ToUpper() == "(Select)".ToUpper())
            //        {
            //            MessageBox.Show("Mailing Address - Please select a State.");
            //            cmbMailingState.Focus();
            //            return false;
            //        }

            //        if (mailingZipCode == string.Empty)
            //        {
            //            MessageBox.Show("Mailing Address - Zip Code can not be empty.");
            //            txtMailingZipCode.Focus();
            //            return false;
            //        }
            //        else
            //        {
            //            if (!Program.regexZipCode.IsMatch(mailingZipCode))
            //            {
            //                MessageBox.Show("Invalid format of Zip Code (Mailing Address).");
            //                txtMailingZipCode.Focus();
            //                return false;
            //            }
            //        }
            //    }
            //}

            if (cmbSameasPhysical.SelectedIndex == 0)
            {
                string mailingAddress1 = txtMailingAddress1.Text.Trim();
                string mailingAddress2 = txtMailingAddress2.Text.Trim();
                string mailingCity = txtMailingCity.Text.Trim();
                string mailingState = cmbMailingState.Text.Trim();
                string mailingZipCode = txtMailingZipCode.Text.Trim();

                mailingZipCode = mailingZipCode.EndsWith("-") ? mailingZipCode.Replace("-", "") : mailingZipCode;

                _isMailingRequired = true;

                if (_isMailingRequired)
                {
                    if (mailingAddress1 == string.Empty)
                    {
                        MessageBox.Show("Mailing Address - Address 1 cannot be empty.");
                        txtMailingAddress1.Focus();
                        return false;
                    }

                    if (mailingCity == string.Empty)
                    {
                        MessageBox.Show("Mailing Address - City cannot be empty.");
                        txtMailingCity.Focus();
                        return false;
                    }

                    if (mailingState.ToUpper() == "(Select)".ToUpper())
                    {
                        MessageBox.Show("Mailing Address - Please select a State.");
                        cmbMailingState.Focus();
                        return false;
                    }

                    if (mailingZipCode == string.Empty)
                    {
                        MessageBox.Show("Mailing Address - Zip Code cannot be empty.");
                        txtMailingZipCode.Focus();
                        return false;
                    }
                    else
                    {
                        if (!Program.regexZipCode.IsMatch(mailingZipCode))
                        {
                            MessageBox.Show("Invalid format of Zip Code (Mailing Address).");
                            txtMailingZipCode.Focus();
                            return false;
                        }
                    }
                }
            }
            if (cmbSameasPhysical.SelectedIndex == 2)
            {
                #region Physical Address 2 validation

                _isPhysical2Required = true;
                //string physicalAddress1 = txtPhysical2Address1.Text.Trim();
                //string physicalAddress2 = txtPhysical2Address2.Text.Trim();
                //string physicalCity = txtPhysical2City.Text.Trim();
                //string physicalState = cmbPhysical2State.Text.Trim();
                //string physicalZipCode = txtPhysical2ZipCode.Text.Trim();
                //string physicalPhone = txtPhysical2PhoneNumber.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
                //string physicalFax = txtPhysical2FaxNumber.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
                //string physicalEmail = txtPhysical2Email.Text.Trim();

                //physical2ZipCode = physical2ZipCode.EndsWith("-") ? physical2ZipCode.Replace("-", "") : physical2ZipCode;

                if (_isPhysical2Required)
                {
                    if (physical2Address1 == string.Empty)
                    {
                        MessageBox.Show("Physical Address 2 - Address 1 cannot be empty.");
                        txtPhysical2Address1.Focus();
                        return false;
                    }

                    if (physical2City == string.Empty)
                    {
                        MessageBox.Show("Physical Address 2 - City cannot be empty.");
                        txtPhysical2City.Focus();
                        return false;
                    }

                    if (physical2State.ToUpper() == "(Select)".ToUpper())
                    {
                        MessageBox.Show("Physical Address 2 - State must be selected.");
                        cmbPhysical2State.Focus();
                        return false;
                    }

                    if (physical2ZipCode == string.Empty)
                    {
                        MessageBox.Show("Physical Address 2 - Zip Code cannot be empty.");
                        txtPhysical2ZipCode.Focus();
                        return false;
                    }
                    else
                    {
                        if (!Program.regexZipCode.IsMatch(physical2ZipCode))
                        {
                            MessageBox.Show("Invalid format of Zip Code (Physical Address 2).");
                            txtPhysical2ZipCode.Focus();
                            return false;
                        }
                    }

                    if (physical2Phone == string.Empty)
                    {
                        MessageBox.Show("Physical Address 2 - Phone number cannot be empty.");
                        txtPhysical2PhoneNumber.Focus();
                        return false;
                    }
                    else
                    {
                        if (!Program.regexPhoneNumber.IsMatch(txtPhysical2PhoneNumber.Text.Trim()))
                        {
                            MessageBox.Show("Invalid format of Phone number (Physical Address 2).");
                            txtPhysical2PhoneNumber.Focus();
                            return false;
                        }
                    }

                    if (physical2Fax != string.Empty)
                    {
                        if (!Program.regexPhoneNumber.IsMatch(txtPhysical2FaxNumber.Text.Trim()))
                        {
                            MessageBox.Show("Invalid format of Fax number (Physical Address 2).");
                            txtPhysical2FaxNumber.Focus();
                            return false;
                        }
                    }

                    if (physical2Email == string.Empty)
                    {
                        MessageBox.Show("Physical Address 2 - Email cannot be empty.");
                        txtPhysical2Email.Focus();
                        return false;
                    }
                    else
                    {
                        if (!Program.regexEmail.IsMatch(txtPhysical2Email.Text.Trim()))
                        {
                            MessageBox.Show("Invalid format of Email (Physical Address 2).");
                            txtPhysical2Email.Focus();
                            return false;
                        }
                    }
                }

                #endregion Physical Address 2 validation
            }
            if (cmbSameasPhysical.SelectedIndex == 3)
            {
                #region Physical Address 3 validation

                _isPhysical3Required = true;
                //string physical3Address1 = txtPhysical3Address1.Text.Trim();
                //string physical3Address2 = txtPhysical3Address2.Text.Trim();
                //string physical3City = txtPhysical3City.Text.Trim();
                //string physical3State = cmbPhysical3State.Text.Trim();
                //string physical3ZipCode = txtPhysical3ZipCode.Text.Trim();
                //string physical3Phone = txtPhysical3PhoneNumber.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
                //string physical3Fax = txtPhysical3FaxNumber.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
                //string physical3Email = txtPhysical3Email.Text.Trim();

                physical3ZipCode = physical3ZipCode.EndsWith("-") ? physical3ZipCode.Replace("-", "") : physical3ZipCode;

                if (_isPhysical3Required)
                {
                    if (physical3Address1 == string.Empty)
                    {
                        MessageBox.Show("Physical Address 3 - Address 1 cannot be empty.");
                        txtPhysical3Address1.Focus();
                        return false;
                    }

                    if (physical3City == string.Empty)
                    {
                        MessageBox.Show("Physical Address 3 - City cannot be empty.");
                        txtPhysical3City.Focus();
                        return false;
                    }

                    if (physical3State.ToUpper() == "(Select)".ToUpper())
                    {
                        MessageBox.Show("Physical Address 3 - State must be selected .");
                        cmbPhysical3State.Focus();
                        return false;
                    }

                    if (physical3ZipCode == string.Empty)
                    {
                        MessageBox.Show("Physical Address 3 - Zip Code cannot be empty.");
                        txtPhysical3ZipCode.Focus();
                        return false;
                    }
                    else
                    {
                        if (!Program.regexZipCode.IsMatch(physical3ZipCode))
                        {
                            MessageBox.Show("Invalid format of Zip Code (Physical Address 3).");
                            txtPhysical3ZipCode.Focus();
                            return false;
                        }
                    }

                    if (physical3Phone == string.Empty)
                    {
                        MessageBox.Show("Physical Address 3 - Phone number cannot be empty.");
                        txtPhysical3PhoneNumber.Focus();
                        return false;
                    }
                    else
                    {
                        if (!Program.regexPhoneNumber.IsMatch(txtPhysical3PhoneNumber.Text.Trim()))
                        {
                            MessageBox.Show("Invalid format of Phone number (Physical Address 3).");
                            txtPhysical3PhoneNumber.Focus();
                            return false;
                        }
                    }

                    if (physical3Fax != string.Empty)
                    {
                        if (!Program.regexPhoneNumber.IsMatch(txtPhysical3FaxNumber.Text.Trim()))
                        {
                            MessageBox.Show("Invalid format of Fax number (Physical Address 3).");
                            txtPhysical3FaxNumber.Focus();
                            return false;
                        }
                    }

                    if (physical3Email == string.Empty)
                    {
                        MessageBox.Show("Physical Address 3 - Email cannot be empty.");
                        txtPhysical3Email.Focus();
                        return false;
                    }
                    else
                    {
                        if (!Program.regexEmail.IsMatch(txtPhysical3Email.Text.Trim()))
                        {
                            MessageBox.Show("Invalid format of Email (Physical Address 3).");
                            txtPhysical3Email.Focus();
                            return false;
                        }
                    }
                }

                #endregion Physical Address 3 validation
            }

            #endregion Mailing Address Validation

            return true;
        }

        private void LoadVendorService()
        {
            try
            {
                VendorServiceBL vendorServiceBL = new VendorServiceBL();

                List<VendorService> vendorServiceList = null;

                vendorServiceList = vendorServiceBL.GetList(this._vendorId);
                chkVendorServices.DataSource = vendorServiceList;
                chkVendorServices.DisplayMember = "VendorServiceNameValue";
                chkVendorServices.ValueMember = "VendorServiceId";

                List<int> vendorServiceIdList = new List<int>();
                foreach (int vendorSeriviceId in vendorServiceIdList)
                {
                    vendorServiceIdList.Add(vendorSeriviceId);
                }
                chkVendorServices.SelectedValue = vendorServiceIdList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool ValidateEmail()
        {
            DataTable vendor = vendorBL.GetByEmail(txtEmail.Text.Trim());

            if (vendor.Rows.Count > 0 && this._mode == OperationMode.New)
            {
                return false;
            }
            else if (vendor.Rows.Count > 1 && this._mode == OperationMode.Edit)
            {
                return false;
            }
            else if (vendor.Rows.Count == 1 && this._mode == OperationMode.Edit)
            {
                if ((int)vendor.Rows[0]["VendorId"] != this._vendorId)
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

        private void cmbMonth_TextChanged(object sender, EventArgs e)
        {
            cmbMonth.CausesValidation = false;
        }

        private void cmbDate_TextChanged(object sender, EventArgs e)
        {
            cmbDate.CausesValidation = false;
        }

        private void cmbYear_TextChanged(object sender, EventArgs e)
        {
            cmbYear.CausesValidation = false;
        }

        private void txtPhysical1ZipCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back)))
                e.Handled = true;
        }

        private void txtPhysical2ZipCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back)))
                e.Handled = true;
        }

        private void txtPhysical3ZipCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back)))
                e.Handled = true;
        }

        private void txtMailingZipCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back)))
                e.Handled = true;
        }

        private void rbCollectionCenter_TextChanged(object sender, EventArgs e)
        {
            rbCollectionCenter.CausesValidation = false;
        }

        private void rbLab_TextChanged(object sender, EventArgs e)
        {
            rbLab.CausesValidation = false;
        }

        private void rbMRO_TextChanged(object sender, EventArgs e)
        {
            rbMRO.CausesValidation = false;
        }

        private void cmbSameasPhysical_TextChanged(object sender, EventArgs e)
        {
            cmbSameasPhysical.CausesValidation = false;
        }

        private void cmbSameasPhysical_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbSameasPhysical.CausesValidation = false;

            if (cmbSameasPhysical.SelectedIndex == 1)
            {
                txtMailingAddress1.Enabled = false;
                txtMailingAddress2.Enabled = false;
                txtMailingCity.Enabled = false;
                cmbMailingState.Enabled = false;
                txtMailingZipCode.Enabled = false;

                #region Physical Address

                Vendor vendor = vendorBL.GetAddress(this._vendorId);
                if (vendor != null)
                {
                    foreach (VendorAddress address in vendor.Addresses)
                    {
                        if (address.AddressTypeId == AddressTypes.MailingAddress && cmbSameasPhysical.SelectedIndex != 0)
                        {
                            txtMailingAddress1.Text = address.Address1;
                            txtMailingAddress2.Text = address.Address2;
                            txtMailingCity.Text = address.City;
                            cmbMailingState.Text = address.State;
                            txtMailingZipCode.Text = address.ZipCode;
                        }
                        if (cmbSameasPhysical.SelectedIndex == 1)
                        {
                            txtMailingAddress1.Text = txtPhysical1Address1.Text;
                            txtMailingAddress2.Text = txtPhysical1Address2.Text;
                            txtMailingCity.Text = txtPhysical1City.Text;
                            cmbMailingState.Text = cmbPhysical1State.Text;
                            txtMailingZipCode.Text = txtPhysical1ZipCode.Text;
                        }
                    }
                }
                else if (cmbSameasPhysical.SelectedIndex == 1)
                {
                    txtMailingAddress1.Text = txtPhysical1Address1.Text;
                    txtMailingAddress2.Text = txtPhysical1Address2.Text;
                    txtMailingCity.Text = txtPhysical1City.Text;
                    cmbMailingState.Text = cmbPhysical1State.Text;
                    txtMailingZipCode.Text = txtPhysical1ZipCode.Text;
                }

                #endregion Physical Address
            }
            else if (cmbSameasPhysical.SelectedIndex == 2)
            {
                txtMailingAddress1.Enabled = false;
                txtMailingAddress2.Enabled = false;
                txtMailingCity.Enabled = false;
                cmbMailingState.Enabled = false;
                txtMailingZipCode.Enabled = false;

                #region Physical Address 2

                Vendor vendor = vendorBL.GetAddress(this._vendorId);

                //if (!ValidateMailingAddress())
                //{
                if (vendor != null)
                {
                    foreach (VendorAddress address in vendor.Addresses)
                    {
                        if (address.AddressTypeId == AddressTypes.MailingAddress && cmbSameasPhysical.SelectedIndex != 0)
                        {
                            txtMailingAddress1.Text = address.Address1;
                            txtMailingAddress2.Text = address.Address2;
                            txtMailingCity.Text = address.City;
                            cmbMailingState.Text = address.State;
                            txtMailingZipCode.Text = address.ZipCode;
                        }
                        if (cmbSameasPhysical.SelectedIndex == 2)
                        {
                            txtMailingAddress1.Text = txtPhysical2Address1.Text;
                            txtMailingAddress2.Text = txtPhysical2Address2.Text;
                            txtMailingCity.Text = txtPhysical2City.Text;
                            cmbMailingState.Text = cmbPhysical2State.Text;
                            txtMailingZipCode.Text = txtPhysical2ZipCode.Text;
                        }
                    }
                }
                else if (cmbSameasPhysical.SelectedIndex == 2)
                {
                    txtMailingAddress1.Text = txtPhysical2Address1.Text;
                    txtMailingAddress2.Text = txtPhysical2Address2.Text;
                    txtMailingCity.Text = txtPhysical2City.Text;
                    cmbMailingState.Text = cmbPhysical2State.Text;
                    txtMailingZipCode.Text = txtPhysical2ZipCode.Text;
                }
                //  }

                #endregion Physical Address 2
            }
            else if (cmbSameasPhysical.SelectedIndex == 3)
            {
                txtMailingAddress1.Enabled = false;
                txtMailingAddress2.Enabled = false;
                txtMailingCity.Enabled = false;
                cmbMailingState.Enabled = false;
                txtMailingZipCode.Enabled = false;

                #region Physical Address 3

                Vendor vendor = vendorBL.GetAddress(this._vendorId);
                //if (!ValidateMailingAddress())
                //{
                if (vendor != null)
                {
                    foreach (VendorAddress address in vendor.Addresses)
                    {
                        if (address.AddressTypeId == AddressTypes.MailingAddress && cmbSameasPhysical.SelectedIndex != 0)
                        {
                            txtMailingAddress1.Text = address.Address1;
                            txtMailingAddress2.Text = address.Address2;
                            txtMailingCity.Text = address.City;
                            cmbMailingState.Text = address.State;
                            txtMailingZipCode.Text = address.ZipCode;
                        }
                        if (cmbSameasPhysical.SelectedIndex == 3)
                        {
                            txtMailingAddress1.Text = txtPhysical3Address1.Text;
                            txtMailingAddress2.Text = txtPhysical3Address2.Text;
                            txtMailingCity.Text = txtPhysical3City.Text;
                            cmbMailingState.Text = cmbPhysical3State.Text;
                            txtMailingZipCode.Text = txtPhysical3ZipCode.Text;
                        }
                    }
                }
                else if (cmbSameasPhysical.SelectedIndex == 3)
                {
                    txtMailingAddress1.Text = txtPhysical3Address1.Text;
                    txtMailingAddress2.Text = txtPhysical3Address2.Text;
                    txtMailingCity.Text = txtPhysical3City.Text;
                    cmbMailingState.Text = cmbPhysical3State.Text;
                    txtMailingZipCode.Text = txtPhysical3ZipCode.Text;
                }
                // }

                #endregion Physical Address 3
            }
            else
            {
                txtMailingAddress1.Enabled = true;
                txtMailingAddress2.Enabled = true;
                txtMailingCity.Enabled = true;
                cmbMailingState.Enabled = true;
                txtMailingZipCode.Enabled = true;

                txtMailingAddress1.Text = string.Empty;
                txtMailingAddress2.Text = string.Empty;
                txtMailingCity.Text = string.Empty;
                cmbMailingState.SelectedIndex = 0;
                txtMailingZipCode.Text = string.Empty;
            }
        }
    }
}