using SurPath.Business;
using SurPath.Entity;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SurPath.Controls.TextBoxes;
using Surpath.ClearStar.BL;
using Surpath.CSTest.Models;
using SurPath.Data;
using SurpathBackend;
using Serilog;
using Newtonsoft.Json;

namespace SurPath
{
    public partial class FrmClientDepartmentDetails : Form
    {
        #region Private Variables
        static ILogger _logger = Program._logger;

        private OperationMode _mode = OperationMode.None;
        private int _clientId;
        private int _clientDepartmentId = 0;

        private bool _UseFormFoxStateOnLoad;

        private ClientBL clientBL = new ClientBL();

        private bool _isPhysicalRequired = true;
        private bool _isMailingRequired = false;
        private bool _isMainContactInformation = true;

        //CheckBoxReverseChecked chkForceManualNotificaitons_ReverseChecked = new CheckBoxReverseChecked();
        private CheckBoxReverseChecked checkBoxReverseChecked_chkForceManualNotificaitons = new CheckBoxReverseChecked();

        private ClientNotificationDataSettings _clientNotificationDataSettings = new ClientNotificationDataSettings();
        private string _clientNotificationDataSettings_original_json = string.Empty;
        //private ClientNotificationDataSettings _clientNotificationDataSettings_original = new ClientNotificationDataSettings();
        private ClientNotificationDataSettings _clientNotificationDataSettings_reset = new ClientNotificationDataSettings();
        private List<ClientNotificationDataSettings> _ALLclientNotificationDataSettings = new List<ClientNotificationDataSettings>();

        //private BackendData _backendData = new BackendData();
        private BackendLogic backendLogic; // = new BackendLogic();
        private PDFengine _PDFengine; // = new PDFengine();
        private PDFengine _PDFengine_reset; // = new PDFengine();

       // private List<IntegrationPartner> _partners = new List<IntegrationPartner>();
       // List<IntegrationPartnerClient> _integrations = new List<IntegrationPartnerClient>();
       //// private List<CheckBox> _partnerCheckboxes = new List<CheckBox>();
       // private List<RadioButton> _partnerRadioButtons = new List<RadioButton>();
        private bool _checkboxssetup = false;
        #endregion Private Variables

        #region Constructor

        public FrmClientDepartmentDetails()
        {
            try
            {
                // setBackend();
                InitializeComponent();
            }
            catch (Exception ex)
            {
                Program.LogError(ex);
                MessageBox.Show(ex.Message);
            }
        }

        public FrmClientDepartmentDetails(OperationMode mode, int clientId, int clientDepartmentId)
        {
            setBackend();
            InitializeComponent();
            this._mode = mode;
            this._clientId = clientId;
            this._clientDepartmentId = clientDepartmentId;

            if (!Program.IsProduction)
            {
                this.txtPreviewEmail.Text = "chris@inzi.com";
                this.txtPreviewPhone.Text = "2148019441";
                this.txtPreviewZipCode.Text = "76210";
            }

        }

        private void setBackend()
        {

            _logger.Debug("FrmClientDepartmentDetails loaded");

            backendLogic = new BackendLogic(null, Program._logger);
            _PDFengine = new PDFengine(Program._logger);
            _PDFengine_reset = new PDFengine(Program._logger);
        }
        #endregion

        #region Event Methods

        private void FrmClientDepartmentDetails_FormClosing(object sender, FormClosingEventArgs e)
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

                        btnOk_Click(new Object(), new EventArgs());
                        //if (!SaveData())
                        //{
                        //    e.Cancel = true;
                        //}
                        //else
                        //{
                        //    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        //}
                    }
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Program.LogError(ex);
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmClientDepartmentDetails_Load(object sender, EventArgs e)
        {
            try
            {
                lblUseFormFox.Text = "FF Disabled";
                chkUseFormFox.CheckedChanged -= chkUseFormFox_CheckedChanged;
                chkUseFormFox.Checked = false;
                chkUseFormFox.CheckedChanged += chkUseFormFox_CheckedChanged;

                InitializeControls();

                CreateLists();

                if ((this._mode == OperationMode.Edit || this._mode == OperationMode.View) && this._clientDepartmentId != 0)
                {
                    LoadData();
                }
                //LoadIntegrationPartners();

                if (this._mode == OperationMode.View)
                {
                    txtDepartmentName.ReadOnly = true;
                    txtLabCode.ReadOnly = true;
                    txtQuestCode.ReadOnly = true;
                    txtClearStarClientCode.ReadOnly = true;
                    chkActive.Enabled = false;
                    chkUA.Enabled = false;
                    chkHair.Enabled = false;
                    chkDNA.Enabled = false;
                    chkRecordKeeping.Enabled = false;
                    pnlMROType.Enabled = false;
                    pnlPaymentType.Enabled = false;

                    chkSameAsClient.Enabled = false;

                    txtPhysicalAddress1.ReadOnly = true;
                    txtPhysicalAddress2.ReadOnly = true;
                    txtPhysicalCity.ReadOnly = true;
                    txtPhysicalState.ReadOnly = true;
                    cmbPhysicalState.Visible = false;
                    txtPhysicalState.Visible = true;
                    txtPhysicalState.ReadOnly = true;
                    txtPhysicalState.Text = cmbPhysicalState.Text != "(Select)" ? cmbPhysicalState.Text : string.Empty;

                    txtPhysicalZipCode.ReadOnly = true;

                    cmbSalesRepresentative.Visible = false;
                    txtSalesRepresentative.ReadOnly = true;
                    txtSalesRepresentative.Visible = true;
                    txtSalesRepresentative.Text = cmbSalesRepresentative.Text != "(Select)" ? cmbSalesRepresentative.Text : string.Empty; ;

                    txtFirstName.ReadOnly = true;
                    txtLastName.ReadOnly = true;
                    txtPhone.ReadOnly = true;
                    txtFax.ReadOnly = true;
                    txtEmail.ReadOnly = true;

                    chkSameAsPhysical.Enabled = false;

                    txtMailingAddress1.ReadOnly = true;
                    txtMailingAddress2.ReadOnly = true;
                    txtMailingCity.ReadOnly = true;
                    txtMailingState.ReadOnly = true;
                    cmbMailingState.Visible = false;
                    txtMailingState.Visible = true;
                    txtMailingState.ReadOnly = true;
                    txtMailingState.Text = cmbMailingState.Text != "(Select)" ? cmbMailingState.Text : string.Empty; ; ;
                    txtMailingZipCode.ReadOnly = true;

                    btnOk.Visible = false;
                    btnClose.Location = new Point(353, 473);
                }
            }
            catch (Exception ex)
            {
                Program.LogError(ex);
                MessageBox.Show("Error occurred while loading the page.");
            }
        }

        private void LoadClientDepartmentInfo(int selectedIndex)
        {
            try
            {
                ClientBL clientBL = new ClientBL();
                List<ClientDepartment> clientDepartmentList = null;
                //Dictionary<string, string> searchParam = new Dictionary<string, string>();
                //searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                //searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());
                //clientDepartmentList = clientBL.GetClientDepartmentList(this._clientId, searchParam);

                dg.DataSource = clientDepartmentList;

                if (dg.Rows.Count > 0)
                {
                    if (selectedIndex > dg.Rows.Count - 1)
                    {
                        selectedIndex = dg.Rows.Count - 1;
                    }

                    dg.Rows[selectedIndex].Selected = true;
                    dg.Focus();
                }
            }
            catch (Exception ex)
            {
                Program.LogError(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void CreateLists()
        {
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
                else
                {
                    return;
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Program.LogError(ex);
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
                Program.LogError(ex);
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void txtDepartmentName_TextChanged(object sender, EventArgs e)
        {
            txtDepartmentName.CausesValidation = false;
        }

        private void chkUA_CheckedChanged(object sender, EventArgs e)
        {
            chkUA.CausesValidation = false;
        }

        private void chkHair_CheckedChanged(object sender, EventArgs e)
        {
            chkHair.CausesValidation = false;
        }

        private void chkDNA_CheckedChanged(object sender, EventArgs e)
        {
            chkDNA.CausesValidation = false;
        }

        private void chkRecordKeeping_CheckChanged(object sender, EventArgs e)
        {
            //this.chkRecordKeeping.CausesValidation = false;
            if (this.chkRecordKeeping.Checked == true)
            {
                ShowRecordKeepingControls();
            }
            else if (this.chkRecordKeeping.Checked == false)
            {
                HideRecordKeepingControls();
            }
        }

        private void ShowRecordKeepingControls()
        {
            foreach (Control ctrl in this.tabDeptDetailsTabs.TabPages[1].Controls)
            {
                ctrl.Visible = true;
                //this.TabControl1.TabPages[0];
            }
        }

        private void HideRecordKeepingControls()
        {
            foreach (Control ctrl in this.tabDeptDetailsTabs.TabPages[1].Controls)
            {
                ctrl.Visible = false;
                //this.TabControl1.TabPages[0];
            }
        }

        private void rbMPOS_CheckedChanged(object sender, EventArgs e)
        {
            rbMPOS.CausesValidation = false;
        }

        private void rbMALL_CheckedChanged(object sender, EventArgs e)
        {
            rbMALL.CausesValidation = false;
        }

        private void rbDonorPays_CheckedChanged(object sender, EventArgs e)
        {
            rbDonorPays.CausesValidation = false;
        }

        private void rbInvoiceClient_CheckedChanged(object sender, EventArgs e)
        {
            rbInvoiceClient.CausesValidation = false;
        }

        private void chkActive_CheckedChanged(object sender, EventArgs e)
        {
            chkActive.CausesValidation = false;
        }

        private void chkSameAsPhysical_CheckedChanged(object sender, EventArgs e)
        {
            chkSameAsPhysical.CausesValidation = false;

            if (chkSameAsPhysical.Checked)
            {
                #region Physical Address

                if (chkSameAsPhysical.Checked == true)
                {
                    txtMailingAddress1.Text = txtPhysicalAddress1.Text;
                    txtMailingAddress2.Text = txtPhysicalAddress2.Text;
                    txtMailingCity.Text = txtPhysicalCity.Text;
                    cmbMailingState.Text = cmbPhysicalState.Text;
                    txtMailingZipCode.Text = txtPhysicalZipCode.Text;

                    txtMailingAddress1.Enabled = false;
                    txtMailingAddress2.Enabled = false;
                    txtMailingCity.Enabled = false;
                    cmbMailingState.Enabled = false;
                    txtMailingZipCode.Enabled = false;
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

        private void txtPhysicalZipCode_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void txtPhone_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void txtFax_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void txtMailingZipCode_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void txtMailingCity_TextChanged(object sender, EventArgs e)
        {
            txtMailingCity.CausesValidation = false;
        }

        private void txtFirstName_TextChanged(object sender, EventArgs e)
        {
            txtFirstName.CausesValidation = false;
        }

        private void txtLastName_TextChanged(object sender, EventArgs e)
        {
            txtLastName.CausesValidation = false;
        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
            txtPhone.CausesValidation = false;
        }

        private void txtFax_TextChanged(object sender, EventArgs e)
        {
            txtFax.CausesValidation = false;
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            txtEmail.CausesValidation = false;
        }

        private void txtPhysicalAddress1_TextChanged(object sender, EventArgs e)
        {
            txtPhysicalAddress1.CausesValidation = false;
        }

        private void txtPhysicalAddress2_TextChanged(object sender, EventArgs e)
        {
            txtPhysicalAddress2.CausesValidation = false;
        }

        private void txtPhysicalCity_TextChanged(object sender, EventArgs e)
        {
            txtPhysicalCity.CausesValidation = false;
        }

        private void cmbPhysicalState_TextChanged(object sender, EventArgs e)
        {
            cmbPhysicalState.CausesValidation = false;
        }

        private void txtPhysicalZipCode_TextChanged(object sender, EventArgs e)
        {
            txtPhysicalZipCode.CausesValidation = false;
        }

        private void cmbSalesRepresentative_TextChanged(object sender, EventArgs e)
        {
            cmbSalesRepresentative.CausesValidation = false;
        }

        private void chkSameAsPhysical_TextChanged(object sender, EventArgs e)
        {
            chkSameAsPhysical.CausesValidation = false;
        }

        private void txtMailingAddress1_TextChanged(object sender, EventArgs e)
        {
            txtMailingAddress1.CausesValidation = false;
        }

        private void txtMailingAddress2_TextChanged(object sender, EventArgs e)
        {
            txtMailingAddress2.CausesValidation = false;
        }

        private void cmbMailingState_TextChanged(object sender, EventArgs e)
        {
            cmbMailingState.CausesValidation = false;
        }

        private void txtMailingZipCode_TextChanged(object sender, EventArgs e)
        {
            txtMailingZipCode.CausesValidation = false;
        }

        private void chkSameAsClient_CheckedChanged(object sender, EventArgs e)
        {
            chkSameAsClient.CausesValidation = false;

            if (chkSameAsClient.Checked)
            {
                txtPhysicalAddress1.Enabled = false;
                txtPhysicalAddress2.Enabled = false;
                txtPhysicalCity.Enabled = false;
                cmbPhysicalState.Enabled = false;
                txtPhysicalZipCode.Enabled = false;

                txtMailingAddress1.Enabled = false;
                txtMailingAddress2.Enabled = false;
                txtMailingCity.Enabled = false;
                cmbMailingState.Enabled = false;
                txtMailingZipCode.Enabled = false;

                txtFirstName.Enabled = false;
                txtLastName.Enabled = false;
                txtPhone.Enabled = false;
                txtFax.Enabled = false;
                txtEmail.Enabled = false;

                cmbSalesRepresentative.Enabled = false;

                Client client = clientBL.Get(this._clientId);
                chkSameAsPhysical.Checked = client.IsMailingAddressPhysical;

                if (client.SalesRepresentativeId != null)
                {
                    cmbSalesRepresentative.SelectedValue = client.SalesRepresentativeId;
                }
                else
                {
                    cmbSalesRepresentative.SelectedValue = 0;
                }

                #region Client Contact

                if (client.ClientContact != null)
                {
                    txtFirstName.Text = client.ClientContact.ClientContactFirstName;
                    txtLastName.Text = client.ClientContact.ClientContactLastName;
                    txtPhone.Text = client.ClientContact.ClientContactPhone;
                    txtFax.Text = client.ClientContact.ClientContactFax;
                    txtEmail.Text = client.ClientContact.ClientContactEmail;
                }

                #endregion Client Contact

                #region Physical Address

                ClientAddress physicaAddress = null;

                foreach (ClientAddress address in client.ClientAddresses)
                {
                    if (address.AddressTypeId == AddressTypes.PhysicalAddress1)
                    {
                        physicaAddress = address;
                        break;
                    }
                }

                if (physicaAddress != null)
                {
                    txtPhysicalAddress1.Text = physicaAddress.Address1;
                    txtPhysicalAddress2.Text = physicaAddress.Address2;
                    txtPhysicalCity.Text = physicaAddress.City;
                    cmbPhysicalState.Text = physicaAddress.State;
                    txtPhysicalZipCode.Text = physicaAddress.ZipCode;
                }

                #endregion Physical Address

                #region Mailing Address

                ClientAddress mailingAddress = null;
                if (chkSameAsPhysical.Checked == false)
                {
                    foreach (ClientAddress address in client.ClientAddresses)
                    {
                        if (address.AddressTypeId == AddressTypes.MailingAddress)
                        {
                            mailingAddress = address;
                            break;
                        }
                    }

                    if (mailingAddress != null)
                    {
                        txtMailingAddress1.Text = mailingAddress.Address1;
                        txtMailingAddress2.Text = mailingAddress.Address2;
                        txtMailingCity.Text = mailingAddress.City;
                        cmbMailingState.Text = mailingAddress.State;
                        txtMailingZipCode.Text = mailingAddress.ZipCode;

                        chkSameAsPhysical.Enabled = false;
                        txtMailingAddress1.Enabled = false;
                        txtMailingAddress2.Enabled = false;
                        txtMailingCity.Enabled = false;
                        cmbMailingState.Enabled = false;
                        txtMailingZipCode.Enabled = false;
                    }
                }
                else if (chkSameAsPhysical.Checked == true)
                {
                    txtMailingAddress1.Text = physicaAddress.Address1;
                    txtMailingAddress2.Text = physicaAddress.Address2;
                    txtMailingCity.Text = physicaAddress.City;
                    cmbMailingState.Text = physicaAddress.State;
                    txtMailingZipCode.Text = physicaAddress.ZipCode;

                    chkSameAsPhysical.Enabled = false;
                    txtMailingAddress1.Enabled = false;
                    txtMailingAddress2.Enabled = false;
                    txtMailingCity.Enabled = false;
                    cmbMailingState.Enabled = false;
                    txtMailingZipCode.Enabled = false;
                }

                #endregion Mailing Address
            }
            else
            {
                chkSameAsPhysical.Enabled = true;

                txtPhysicalAddress1.Enabled = true;
                txtPhysicalAddress2.Enabled = true;
                txtPhysicalCity.Enabled = true;
                cmbPhysicalState.Enabled = true;
                txtPhysicalZipCode.Enabled = true;

                txtMailingAddress1.Enabled = true;
                txtMailingAddress2.Enabled = true;
                txtMailingCity.Enabled = true;
                cmbMailingState.Enabled = true;
                txtMailingZipCode.Enabled = true;

                txtFirstName.Enabled = true;
                txtLastName.Enabled = true;
                txtPhone.Enabled = true;
                txtFax.Enabled = true;
                txtEmail.Enabled = true;

                cmbSalesRepresentative.Enabled = true;

                txtPhysicalAddress1.Text = string.Empty;
                txtPhysicalAddress2.Text = string.Empty;
                txtPhysicalCity.Text = string.Empty;
                cmbPhysicalState.SelectedIndex = 0;
                txtPhysicalZipCode.Text = string.Empty;

                txtMailingAddress1.Text = string.Empty;
                txtMailingAddress2.Text = string.Empty;
                txtMailingCity.Text = string.Empty;
                cmbMailingState.SelectedIndex = 0;
                txtMailingZipCode.Text = string.Empty;

                txtFirstName.Text = string.Empty;
                txtLastName.Text = string.Empty;
                txtPhone.Text = string.Empty;
                txtFax.Text = string.Empty;
                txtEmail.Text = string.Empty;

                cmbSalesRepresentative.SelectedIndex = 0;
            }
        }

        #endregion Event Methods

        #region Public Methods

        public void LoadData()
        {
            try
            {

                if (this._mode == OperationMode.Edit || this._mode == OperationMode.View)
                {
                    ClientDepartment clientDepartment = clientBL.GetClientDepartment(this._clientDepartmentId);

                    if (clientDepartment != null)
                    {
                        txtDepartmentName.Text = clientDepartment.DepartmentName;
                        txtLabCode.Text = clientDepartment.LabCode;
                        txtFormFoxCode.Text = clientDepartment.FormFoxCode;
                        try
                        {
                            txtQuestCode.Text = clientDepartment.QuestCode;
                        }
                        catch (Exception ex)
                        {
                            Program.LogError(ex);
                        }
                        try
                        {
                            if (clientDepartment.ClearStarCode != null)
                            {
                                txtClearStarClientCode.Text = clientDepartment.ClearStarCode;
                                if (clientDepartment.ClearStarCode.Length >= 1)
                                {
                                    this.btnClearStarAssign.Visible = false;
                                }
                                else
                                {
                                    this.btnClearStarAssign.Visible = true;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Program.LogError(ex);
                        }
                        chkUA.Checked = clientDepartment.IsUA;
                        chkHair.Checked = clientDepartment.IsHair;
                        chkDNA.Checked = clientDepartment.IsDNA;
                        chkRecordKeeping.Checked = clientDepartment.IsRecordKeeping;
                        chkBC.Checked = clientDepartment.IsBC;

                        if (clientDepartment.MROTypeId == ClientMROTypes.MPOS)
                        {
                            rbMPOS.Checked = true;
                        }
                        else if (clientDepartment.MROTypeId == ClientMROTypes.MALL)
                        {
                            rbMALL.Checked = true;
                        }
                        else if (clientDepartment.MROTypeId == ClientMROTypes.None)
                        {
                            rbMPOS.Checked = false;
                            rbMALL.Checked = false;
                        }
                        if (clientDepartment.PaymentTypeId == ClientPaymentTypes.DonorPays)
                        {
                            rbDonorPays.Checked = true;
                        }
                        else if (clientDepartment.PaymentTypeId == ClientPaymentTypes.InvoiceClient)
                        {
                            rbInvoiceClient.Checked = true;
                        }
                        else
                        {
                            rbDonorPays.Checked = false;
                            rbInvoiceClient.Checked = false;
                        }

                        chkActive.Checked = clientDepartment.IsDepartmentActive;

                        chkSameAsClient.Checked = clientDepartment.IsPhysicalAddressAsClient;

                        chkSameAsPhysical.Checked = clientDepartment.IsMailingAddressPhysical;

                        if (chkSameAsClient.Checked == false)
                        {
                            if (clientDepartment.SalesRepresentativeId != null)
                            {
                                cmbSalesRepresentative.SelectedValue = clientDepartment.SalesRepresentativeId;
                            }
                            else
                            {
                                cmbSalesRepresentative.SelectedValue = 0;
                            }

                            #region Client Contact

                            if (clientDepartment.ClientContact != null)
                            {
                                txtFirstName.Text = clientDepartment.ClientContact.ClientContactFirstName;
                                txtLastName.Text = clientDepartment.ClientContact.ClientContactLastName;
                                txtPhone.Text = clientDepartment.ClientContact.ClientContactPhone;
                                txtFax.Text = clientDepartment.ClientContact.ClientContactFax;
                                txtEmail.Text = clientDepartment.ClientContact.ClientContactEmail;
                            }

                            #endregion Client Contact

                            #region Physical Address

                            ClientAddress physicaAddress = null;

                            foreach (ClientAddress address in clientDepartment.ClientAddresses)
                            {
                                if (address.AddressTypeId == AddressTypes.PhysicalAddress1)
                                {
                                    physicaAddress = address;
                                    break;
                                }
                            }

                            if (physicaAddress != null)
                            {
                                txtPhysicalAddress1.Text = physicaAddress.Address1;
                                txtPhysicalAddress2.Text = physicaAddress.Address2;
                                txtPhysicalCity.Text = physicaAddress.City;
                                cmbPhysicalState.Text = physicaAddress.State;
                                txtPhysicalZipCode.Text = physicaAddress.ZipCode;
                            }

                            #endregion Physical Address

                            #region Mailing Address

                            ClientAddress mailingAddress = null;
                            if (chkSameAsPhysical.Checked == false)
                            {
                                foreach (ClientAddress address in clientDepartment.ClientAddresses)
                                {
                                    if (address.AddressTypeId == AddressTypes.MailingAddress)
                                    {
                                        mailingAddress = address;
                                        break;
                                    }
                                }

                                if (mailingAddress != null)
                                {
                                    txtMailingAddress1.Text = mailingAddress.Address1;
                                    txtMailingAddress2.Text = mailingAddress.Address2;
                                    txtMailingCity.Text = mailingAddress.City;
                                    cmbMailingState.Text = mailingAddress.State;
                                    txtMailingZipCode.Text = mailingAddress.ZipCode;
                                }
                            }
                            else
                            {
                                txtMailingAddress1.Text = physicaAddress.Address1;
                                txtMailingAddress2.Text = physicaAddress.Address2;
                                txtMailingCity.Text = physicaAddress.City;
                                cmbMailingState.Text = physicaAddress.State;
                                txtMailingZipCode.Text = physicaAddress.ZipCode;
                            }

                            #endregion Mailing Address
                        }
                        else if (chkSameAsClient.Checked == true)
                        {
                            Client client = clientBL.Get(this._clientId);
                            chkSameAsPhysical.Checked = client.IsMailingAddressPhysical;

                            if (client.SalesRepresentativeId != null)
                            {
                                cmbSalesRepresentative.SelectedValue = client.SalesRepresentativeId;
                            }
                            else
                            {
                                cmbSalesRepresentative.SelectedValue = 0;
                            }

                            #region Client Contact

                            if (client.ClientContact != null)
                            {
                                txtFirstName.Text = client.ClientContact.ClientContactFirstName;
                                txtLastName.Text = client.ClientContact.ClientContactLastName;
                                txtPhone.Text = client.ClientContact.ClientContactPhone;
                                txtFax.Text = client.ClientContact.ClientContactFax;
                                txtEmail.Text = client.ClientContact.ClientContactEmail;
                            }

                            #endregion Client Contact

                            #region Physical Address

                            ClientAddress physicaAddress = null;

                            foreach (ClientAddress address in client.ClientAddresses)
                            {
                                if (address.AddressTypeId == AddressTypes.PhysicalAddress1)
                                {
                                    physicaAddress = address;
                                    break;
                                }
                            }

                            if (physicaAddress != null)
                            {
                                txtPhysicalAddress1.Text = physicaAddress.Address1;
                                txtPhysicalAddress2.Text = physicaAddress.Address2;
                                txtPhysicalCity.Text = physicaAddress.City;
                                cmbPhysicalState.Text = physicaAddress.State;
                                txtPhysicalZipCode.Text = physicaAddress.ZipCode;
                            }

                            #endregion Physical Address

                            #region Mailing Address

                            ClientAddress mailingAddress = null;
                            if (chkSameAsPhysical.Checked == false)
                            {
                                foreach (ClientAddress address in client.ClientAddresses)
                                {
                                    if (address.AddressTypeId == AddressTypes.MailingAddress)
                                    {
                                        mailingAddress = address;
                                        break;
                                    }
                                }

                                if (mailingAddress != null)
                                {
                                    txtMailingAddress1.Text = mailingAddress.Address1;
                                    txtMailingAddress2.Text = mailingAddress.Address2;
                                    txtMailingCity.Text = mailingAddress.City;
                                    cmbMailingState.Text = mailingAddress.State;
                                    txtMailingZipCode.Text = mailingAddress.ZipCode;

                                    chkSameAsPhysical.Enabled = false;
                                    txtMailingAddress1.Enabled = false;
                                    txtMailingAddress2.Enabled = false;
                                    txtMailingCity.Enabled = false;
                                    cmbMailingState.Enabled = false;
                                    txtMailingZipCode.Enabled = false;
                                }
                            }
                            else if (chkSameAsPhysical.Checked == true)
                            {
                                txtMailingAddress1.Text = physicaAddress.Address1;
                                txtMailingAddress2.Text = physicaAddress.Address2;
                                txtMailingCity.Text = physicaAddress.City;
                                cmbMailingState.Text = physicaAddress.State;
                                txtMailingZipCode.Text = physicaAddress.ZipCode;

                                chkSameAsPhysical.Enabled = false;
                                txtMailingAddress1.Enabled = false;
                                txtMailingAddress2.Enabled = false;
                                txtMailingCity.Enabled = false;
                                cmbMailingState.Enabled = false;
                                txtMailingZipCode.Enabled = false;
                            }

                            #endregion Mailing Address
                        }

                        //////if (clientDepartment.integrationPartner == true && clientDepartment.requireLogin == true)
                        //////{
                        //////   // chkRequireLogin.Checked = true;
                        //////    rdoRequireLogin.Checked = true;
                        //////}
                        //////else if (clientDepartment.integrationPartner == true && clientDepartment.require_remote_login == true)
                        //////{
                        //////    rdoRequireRemoteLogin.Checked = true;
                        //////}
                        //////else
                        //////{
                        //////    rdoRequireNoLogin.Checked = true;
                        //////}
                        // if (clientDepartment.integrationPartner == true && clientDepartment.require_remote_login == true) chkRequireLogin.Checked = true;

                        PopulateNotificationSettings();
                        ResetControlsCauseValidation();
                        
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Program.LogError(ex);
                MessageBox.Show(ex.Message);
            }
        }

        ///////// <summary>
        ///////// Load integration partners and build radio buttons on form
        ///////// </summary>
        //////public void LoadIntegrationPartners()
        //////{
        //////    _partners = backendLogic.GetIntegrationPartners();
        //////    _integrations = new List<IntegrationPartnerClient>();
        //////    _integrations = backendLogic.GetIntegrationPartnerClientByClientAndDepartmentId(this.ClientId, this.ClientDepartmentId);
        //////    var i = 0;
        //////    var left = 23;
        //////    var gap = 23;

        //////    var top = 20; // gbIntegrations.Location.Y + 20;
        //////    var _partnerId = -1;

        //////    RadioButton rdo = new RadioButton();
        //////    rdo.Tag = _partnerId;
        //////    rdo.Text = "None";
        //////    rdo.Checked = false;
        //////    rdo.Location = new Point(left, top + (i * gap));
        //////    //tabIntegrationsClientDepartmentDetails.Controls.Add(rdo);
        //////    gbIntegrations.Controls.Add(rdo);

        //////    _partnerRadioButtons.Add(rdo);
        //////    i++;
        //////    foreach (IntegrationPartner _partner in _partners)
        //////    {
        //////        rdo = new RadioButton();
        //////        rdo.Tag = _partner.backend_integration_partner_id;
        //////        rdo.Text = _partner.partner_name;


        //////        //box.CheckedChanged += delegate (object sender, EventArgs e)
        //////        //{
        //////        //    var _cb = sender as CheckBox;
        //////        //    if (_cb.Checked == true)
        //////        //    {
        //////        //        foreach (CheckBox _box in _partnerCheckboxes)
        //////        //        {

        //////        //            if (_checkboxssetup == true && _box.Tag != _cb.Tag)
        //////        //            {
        //////        //                _box.Checked = false;
        //////        //            }
        //////        //        }
        //////        //    }


        //////        if (_integrations.Exists(_i => _i.active == true && _i.backend_integration_partner_id == _partner.backend_integration_partner_id && _i.client_department_id == this._clientDepartmentId && _i.client_id == this._clientId))
        //////        {
        //////            _partnerId = _partner.backend_integration_partner_id;
        //////            //rdo.Checked = _integrations.Where(_i => _i.backend_integration_partner_id == _partner.backend_integration_partner_id).First().active;
        //////            //_partnerRadioButtons.Where(r => (int)r.Tag == -1).First().Checked = false;
        //////        }
        //////        rdo.Location = new Point(left, top + (i * gap));
        //////        //tabIntegrationsClientDepartmentDetails.Controls.Add(rdo);
        //////        gbIntegrations.Controls.Add(rdo);
        //////        _partnerRadioButtons.Add(rdo);

        //////        //CheckBox box = new CheckBox();
        //////        //box.CheckedChanged += delegate (object sender, EventArgs e)
        //////        //{
        //////        //    var _cb = sender as CheckBox;
        //////        //    if (_cb.Checked == true)
        //////        //    {
        //////        //        foreach (CheckBox _box in _partnerCheckboxes)
        //////        //        {

        //////        //            if (_checkboxssetup == true && _box.Tag != _cb.Tag)
        //////        //            {
        //////        //                _box.Checked = false;
        //////        //            }
        //////        //        }
        //////        //    }

        //////        //};
        //////        //box.Tag = _partner.backend_integration_partner_id;
        //////        //box.Text = _partner.partner_name;
        //////        //box.AutoSize = true;
        //////        //box.Checked = false;
        //////        //if (_integrations.Exists(_i => _i.backend_integration_partner_id == _partner.backend_integration_partner_id && _i.client_department_id == this._clientDepartmentId && _i.client_id == this._clientId))
        //////        //{
        //////        //    box.Checked = _integrations.Where(_i => _i.backend_integration_partner_id == _partner.backend_integration_partner_id).First().active;
        //////        //}
        //////        //box.Location = new Point(left, top + (i * gap));
        //////        //tabIntegrationsClientDepartmentDetails.Controls.Add(box);

        //////        //_partnerCheckboxes.Add(box);
        //////        i++;
        //////    }
        //////    foreach (RadioButton __rdo in _partnerRadioButtons)
        //////    {
        //////        __rdo.Click += delegate (object sender, EventArgs e)
        //////        {
        //////            var _rdo = sender as RadioButton;
        //////            if ((int)_rdo.Tag != -1)
        //////            {
        //////                rdoRequireNoLogin.Checked = false;
        //////                rdoRequireNoLogin.Enabled = false;
        //////            }
        //////            else
        //////            {
        //////                rdoRequireNoLogin.Enabled = true;
        //////                rdoRequireNoLogin.Checked = true;

        //////            }
        //////        };
        //////    }
        //////    _partnerRadioButtons.Where(_rdo => (int)_rdo.Tag == _partnerId).First().Checked = true;
        //////    _checkboxssetup = true;

        //////}


        public static void BindField(Control control, string propertyName, object dataSource, string dataMember)
        {
            Binding bd;

            for (int index = control.DataBindings.Count - 1; (index == 0); index--)
            {
                bd = control.DataBindings[index];
                if (bd.PropertyName == propertyName)
                    control.DataBindings.Remove(bd);
            }
            control.DataBindings.Add(propertyName, dataSource, dataMember, false, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void PopulateNotificationSettings()
        {
            try
            {
                _clientNotificationDataSettings = backendLogic.GetClientNotificationDataSettings(this._clientId, this._clientDepartmentId);
                _clientNotificationDataSettings_original_json = JsonConvert.SerializeObject(_clientNotificationDataSettings);
                //_backendData.GetClientNotificationDataSettings(this._clientId, this._clientDepartmentId);

                if (_clientNotificationDataSettings.client_id == 0)
                {
                    _clientNotificationDataSettings.client_id = this._clientId;
                    _clientNotificationDataSettings.client_department_id = this._clientDepartmentId;
                    _clientNotificationDataSettings_reset.client_id = this._clientId;
                    _clientNotificationDataSettings_reset.client_department_id = this._clientDepartmentId;
                }
                _ALLclientNotificationDataSettings = backendLogic.GetAllNotificaitonDataSettings();

                if (_clientNotificationDataSettings.client_id == 0 && _clientNotificationDataSettings.client_department_id == 0)
                {
                    _clientNotificationDataSettings.client_id = this._clientId;
                    _clientNotificationDataSettings.client_department_id = this._clientDepartmentId;
                    _clientNotificationDataSettings.backend_notification_window_data_id = backendLogic.SetClientNotificationSettings(_clientNotificationDataSettings);
                }

                #region copyFrom

                List<ClientsWithSettings> _clientDepartmentsForComboBox = _ALLclientNotificationDataSettings
                    .Where(x => x.client_id != this._clientId || x.client_department_id != this._clientDepartmentId)
                    .Select(x => new ClientsWithSettings
                    {
                        backend_notification_window_data_id = x.backend_notification_window_data_id,
                        name = $"{x.client_name} {x.department_name}"
                    })
                    .ToList();
                cmboCopyWindowSettings.DataSource = _clientDepartmentsForComboBox;
                cmboCopyWindowSettings.DisplayMember = "name";
                cmboCopyWindowSettings.ValueMember = "backend_notification_window_data_id";

                #endregion copyFrom

                SetNotificationValuesOnForm(_clientNotificationDataSettings, _PDFengine);

                #region Global

                txtGlobalMROName.Text = _PDFengine.EngineSettings.Settings["MROName"];

                #endregion Global
            }
            catch (Exception ex)
            {
                Program.LogError(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void SetNotificationValuesOnForm(ClientNotificationDataSettings cnds, PDFengine pdfEngine)
        {
            #region NotificationOptions

            this.chkForceManualNotificaitons.Checked = cnds.force_manual;
            this.dtSweepDate.Value = cnds.notification_sweep_date ?? DateTime.Now;
            this.dtSendInStart.Value = cnds.notification_start_date ?? DateTime.Now.AddDays(1);
            this.dtSendInStop.Value = cnds.notification_stop_date ?? DateTime.Now.AddDays(2);
            if (cnds.deadline_alert_in_days < 1) cnds.deadline_alert_in_days = 1;
            this.nudDeadlineAlert.Value = cnds.deadline_alert_in_days;
            if (cnds.max_sendins < 1) cnds.max_sendins = 1;
            this._UseFormFoxStateOnLoad = cnds.use_formfox;
            if (cnds.use_formfox == true && VerifyUseFormFox() == true)
            {
                lblUseFormFox.Text = "FF Enabled";
                chkUseFormFox.CheckedChanged -= chkUseFormFox_CheckedChanged;
                chkUseFormFox.Checked = true;
                chkUseFormFox.CheckedChanged += chkUseFormFox_CheckedChanged;

            }
            else
            {
                lblUseFormFox.Text = "FF Disabled";
                chkUseFormFox.CheckedChanged -= chkUseFormFox_CheckedChanged;
                chkUseFormFox.Checked = false;
                chkUseFormFox.CheckedChanged += chkUseFormFox_CheckedChanged;

            }
            this.nudMaxSendIns.Value = cnds.max_sendins;

            setScheduleInputs();
            this.radASAPorDelay1.Checked = cnds.send_asap;
            this.radASAPorDelay2.Checked = !cnds.send_asap;
            this.chkOnsiteTesting.Checked = cnds.onsite_testing;
            this.chkEnableSMS.Checked = cnds.enable_sms;
            this.numDelayHours.Value = cnds.delay_in_hours;
            //_clientNotificationDataSettings.show_web_notify_button = this.ckbWebShowNotifyButton.Checked;
            this.ckbWebShowNotifyButton.Checked = _clientNotificationDataSettings.show_web_notify_button;

            #endregion NotificationOptions

            #region SMSReply

            this.txtSMSReply.Text = cnds.client_autoresponse;

            #endregion SMSReply

            #region WindowSettings

            foreach (ClientNotificationDataSettingsDay daySettings in cnds.DaySettings)
            {
                switch (daySettings.DayOfWeek)
                {
                    case (int)DayOfWeekEnum.Sunday:
                        SetDayValues(chkSunday, dtSundayStart, dtSundayEnd, daySettings);
                        break;

                    case (int)DayOfWeekEnum.Monday:
                        SetDayValues(chkMonday, dtMondayStart, dtMondayEnd, daySettings);
                        break;

                    case (int)DayOfWeekEnum.Tuesday:
                        SetDayValues(chkTuesday, dtTuesdayStart, dtTuesdayEnd, daySettings);
                        break;

                    case (int)DayOfWeekEnum.Wednesday:
                        SetDayValues(chkWednesday, dtWednesdayStart, dtWednesdayEnd, daySettings);
                        break;

                    case (int)DayOfWeekEnum.Thursday:
                        SetDayValues(chkThursday, dtThursdayStart, dtThursdayEnd, daySettings);
                        break;

                    case (int)DayOfWeekEnum.Friday:
                        SetDayValues(chkFriday, dtFridayStart, dtFridayEnd, daySettings);
                        break;

                    case (int)DayOfWeekEnum.Saturday:
                        SetDayValues(chkSaturday, dtSaturdayStart, dtSaturdayEnd, daySettings);
                        break;
                }
            }

            #endregion WindowSettings

            #region PDFSettings

            if (string.IsNullOrEmpty(cnds.pdf_render_settings_filename))
            {
                txtFilenameRenderSettings.Text = _PDFengine.DefaultRenderSettingsFile;
            }
            else
            {
                txtFilenameRenderSettings.Text = cnds.pdf_render_settings_filename;
            }
            txtJSONTemplateSettings.Text = pdfEngine.GetRenderSettingsAsJson();

            txtFilenamePDFTemplate.Text = pdfEngine.RenderSettings.TemplateFileName;
            if (string.IsNullOrEmpty(_PDFengine.RenderSettings.TemplateFileName))
            {
                txtFilenamePDFTemplate.Text = _PDFengine.DefaultTemplate;
            }
            #endregion PDFSettings
        }

        private void SetDayValues(CheckBox chkBox, DateTimePicker windowStart, DateTimePicker windowEnd, ClientNotificationDataSettingsDay daySettings)
        {
            chkBox.Checked = daySettings.Enabled;
            DateTime dtStart = DateTime.Now.Date; // new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DateTime dtEnd = DateTime.Now.Date; // new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            dtStart = dtStart.AddSeconds(daySettings.send_time_start_seconds_from_midnight);
            dtEnd = dtEnd.AddSeconds(daySettings.send_time_stop_seconds_from_midnight);
            windowStart.Value = dtStart;
            windowEnd.Value = dtEnd;
            chkBox.Tag = "noti_" + daySettings.DayOfWeek.ToString() + "_day";
            windowStart.Tag = "noti_" + daySettings.DayOfWeek.ToString() + "_start";
            windowEnd.Tag = "noti_" + daySettings.DayOfWeek.ToString() + "_stop";
        }

        private void GetDayValues(CheckBox chkBox, DateTimePicker windowStart, DateTimePicker windowEnd, ClientNotificationDataSettingsDay daySettings)
        {
            daySettings.Enabled = chkBox.Checked;

            daySettings.send_time_start_seconds_from_midnight = (int)windowStart.Value.TimeOfDay.TotalSeconds;
            daySettings.send_time_stop_seconds_from_midnight = (int)windowEnd.Value.TimeOfDay.TotalSeconds;
        }

        private void GetDayValues()
        {
            foreach (Control c in groupNotifcationDaySettings.Controls)
            {
                string tag = c.Tag.ToString();
                string[] tags = tag.Split('_');
                DateTimePicker dateTimePicker;
                CheckBox checkBox;
                if (tags[0].Equals("noti_", StringComparison.InvariantCultureIgnoreCase))
                {
                    int iDay = int.Parse(tags[1]);
                    switch (tags[2])
                    {
                        case "day":
                            checkBox = c as CheckBox;
                            _clientNotificationDataSettings.DaySettings.Where(x => x.DayOfWeek == iDay).First().Enabled = checkBox.Checked;
                            break;

                        case "start":
                            dateTimePicker = c as DateTimePicker;
                            //double secsFromMidnight =
                            _clientNotificationDataSettings.DaySettings.Where(x => x.DayOfWeek == iDay).First().send_time_start_seconds_from_midnight = (int)dateTimePicker.Value.TimeOfDay.TotalSeconds;
                            break;

                        case "stop":
                            dateTimePicker = c as DateTimePicker;
                            _clientNotificationDataSettings.DaySettings.Where(x => x.DayOfWeek == iDay).First().send_time_stop_seconds_from_midnight = (int)dateTimePicker.Value.TimeOfDay.TotalSeconds;

                            break;
                    }
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void InitializeControls()
        {
            chkUA.Checked = true;
            chkHair.Checked = false;
            chkDNA.Checked = false;
            chkRecordKeeping.Checked = false;
            HideRecordKeepingControls();
            this.tabDeptDetailsTabs.TabPages[1].Hide();
            rbMPOS.Checked = true;
            rbMALL.Checked = false;
            rbDonorPays.Checked = true;
            rbInvoiceClient.Checked = false;

            chkSameAsClient.Checked = false;
            chkSameAsPhysical.Checked = false;

            cmbPhysicalState.SelectedIndex = 0;
            cmbMailingState.SelectedIndex = 0;

            //txtMailingAddress1.Enabled = false;
            //txtMailingAddress2.Enabled = false;
            //txtMailingCity.Enabled = false;
            //cmbMailingState.Enabled = false;
            //txtMailingZipCode.Enabled = false;

            txtPhysicalState.Visible = false;
            txtSalesRepresentative.Visible = false;
            txtMailingState.Visible = false;

            LoadSalesRepresentatives();

            if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
            {
                //CLIENT_DEPARTMENT_VIEW
                DataRow[] clientDepartmentView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.CLIENT_DEPARTMENT_VIEW.ToDescriptionString() + "'");

                if (clientDepartmentView.Length > 0)
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
                        if (ctrl is ComboBox)
                        {
                            ((ComboBox)ctrl).Enabled = false;
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

                ClientDepartment clientDepartment = null;

                if (this._mode == OperationMode.New)
                {
                    clientDepartment = new ClientDepartment();
                    clientDepartment.ClientDepartmentId = 0;
                    clientDepartment.CreatedBy = Program.currentUserName;
                }
                else if (this._mode == OperationMode.Edit)
                {
                    clientDepartment = clientBL.GetClientDepartment(this._clientDepartmentId);
                }

                clientDepartment.ClientId = this._clientId;
                clientDepartment.DepartmentName = txtDepartmentName.Text.Trim();

                clientDepartment.LabCode = txtLabCode.Text.Trim();
                txtduplicatetoLabCode.Text = txtLabCode.Text.Trim();
                try
                {
                    clientDepartment.QuestCode = txtQuestCode.Text.Trim();
                }
                catch (Exception ex)
                {
                    Program.LogError(ex);
                }
                try
                {
                    clientDepartment.ClearStarCode = txtClearStarClientCode.Text.Trim();
                    if (clientDepartment.ClearStarCode.Length >= 1)
                    {
                        this.btnClearStarAssign.Visible = false;
                    }
                    else
                    {
                        this.btnClearStarAssign.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    Program.LogError(ex);
                }

                clientDepartment.FormFoxCode = txtFormFoxCode.Text.Trim();

                if (rbMPOS.Checked == true)
                {
                    clientDepartment.MROTypeId = SurPath.Enum.ClientMROTypes.MPOS;
                }
                else if (rbMALL.Checked == true)
                {
                    clientDepartment.MROTypeId = SurPath.Enum.ClientMROTypes.MALL;
                }
                else
                {
                    clientDepartment.MROTypeId = SurPath.Enum.ClientMROTypes.None;
                }

                if (rbDonorPays.Checked)
                {
                    clientDepartment.PaymentTypeId = ClientPaymentTypes.DonorPays;
                }
                else if (rbInvoiceClient.Checked)
                {
                    clientDepartment.PaymentTypeId = ClientPaymentTypes.InvoiceClient;
                }
                else
                {
                    clientDepartment.PaymentTypeId = ClientPaymentTypes.None;
                }
                clientDepartment.IsUA = chkUA.Checked;
                clientDepartment.IsHair = chkHair.Checked;
                clientDepartment.IsDNA = chkDNA.Checked;
                clientDepartment.IsRecordKeeping = chkRecordKeeping.Checked;
                clientDepartment.IsBC = chkBC.Checked;

                clientDepartment.IsDepartmentActive = chkActive.Checked;

                clientDepartment.IsPhysicalAddressAsClient = chkSameAsClient.Checked;

                //if (chkSameAsClient.Checked == false)
                //{
                clientDepartment.IsMailingAddressPhysical = chkSameAsPhysical.Checked;

                if (cmbSalesRepresentative.SelectedValue.ToString() != "0")
                {
                    clientDepartment.SalesRepresentativeId = (int)cmbSalesRepresentative.SelectedValue;
                }
                else
                {
                    clientDepartment.SalesRepresentativeId = null;
                }

                #region Client Contact

                clientDepartment.ClientContact.ClientContactFirstName = txtFirstName.Text.Trim();
                clientDepartment.ClientContact.ClientContactLastName = txtLastName.Text.Trim();

                if (txtPhone.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") != string.Empty)
                {
                    clientDepartment.ClientContact.ClientContactPhone = txtPhone.Text.Trim();
                }
                else
                {
                    clientDepartment.ClientContact.ClientContactPhone = string.Empty;
                }

                if (txtFax.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") != string.Empty)
                {
                    clientDepartment.ClientContact.ClientContactFax = txtFax.Text.Trim();
                }
                else
                {
                    clientDepartment.ClientContact.ClientContactFax = string.Empty;
                }

                clientDepartment.ClientContact.ClientContactEmail = txtEmail.Text.Trim();

                #endregion Client Contact

                #region Physical Address

                if (_isPhysicalRequired)
                {
                    ClientAddress physicaAddress = null;
                    if (this._mode == OperationMode.New)
                    {
                        physicaAddress = new ClientAddress();
                        physicaAddress.AddressTypeId = AddressTypes.PhysicalAddress1;
                        clientDepartment.ClientAddresses.Add(physicaAddress);
                    }
                    else
                    {
                        foreach (ClientAddress address in clientDepartment.ClientAddresses)
                        {
                            if (address.AddressTypeId == AddressTypes.PhysicalAddress1)
                            {
                                physicaAddress = address;
                                break;
                            }
                        }

                        if (physicaAddress == null)
                        {
                            physicaAddress = new ClientAddress();
                            physicaAddress.AddressTypeId = AddressTypes.PhysicalAddress1;
                            clientDepartment.ClientAddresses.Add(physicaAddress);
                        }
                    }

                    physicaAddress.Address1 = txtPhysicalAddress1.Text.Trim();
                    physicaAddress.Address2 = txtPhysicalAddress2.Text.Trim();
                    physicaAddress.City = txtPhysicalCity.Text.Trim();
                    physicaAddress.State = cmbPhysicalState.Text.Trim();

                    string physicalZipCode = txtPhysicalZipCode.Text.Trim();
                    physicalZipCode = physicalZipCode.EndsWith("-") ? physicalZipCode.Replace("-", "") : physicalZipCode;

                    physicaAddress.ZipCode = physicalZipCode;

                    physicaAddress.LastModifiedBy = Program.currentUserName;
                }

                if (chkSameAsPhysical.Checked)
                {
                    ClientAddress mailingAddress = null;
                    if (this._mode == OperationMode.New)
                    {
                        mailingAddress = new ClientAddress();
                        mailingAddress.AddressTypeId = AddressTypes.MailingAddress;
                        clientDepartment.ClientAddresses.Add(mailingAddress);
                    }
                    else
                    {
                        foreach (ClientAddress address in clientDepartment.ClientAddresses)
                        {
                            if (address.AddressTypeId == AddressTypes.MailingAddress)
                            {
                                mailingAddress = address;
                                break;
                            }
                        }

                        if (mailingAddress == null)
                        {
                            mailingAddress = new ClientAddress();
                            mailingAddress.AddressTypeId = AddressTypes.MailingAddress;
                            clientDepartment.ClientAddresses.Add(mailingAddress);
                        }
                    }
                    mailingAddress.Address1 = txtPhysicalAddress1.Text.Trim();
                    mailingAddress.Address2 = txtPhysicalAddress2.Text.Trim();
                    mailingAddress.City = txtPhysicalCity.Text.Trim();
                    mailingAddress.State = cmbPhysicalState.Text.Trim();

                    string mailingZipCode = txtPhysicalZipCode.Text.Trim();

                    mailingZipCode = mailingZipCode.EndsWith("-") ? mailingZipCode.Replace("-", "") : mailingZipCode;

                    mailingAddress.ZipCode = mailingZipCode;

                    mailingAddress.LastModifiedBy = Program.currentUserName;
                }

                #endregion Physical Address

                #region Mailing Address

                if (_isMailingRequired)
                {
                    ClientAddress mailingAddress = null;
                    if (this._mode == OperationMode.New)
                    {
                        mailingAddress = new ClientAddress();
                        mailingAddress.AddressTypeId = AddressTypes.MailingAddress;
                        clientDepartment.ClientAddresses.Add(mailingAddress);
                    }
                    else
                    {
                        foreach (ClientAddress address in clientDepartment.ClientAddresses)
                        {
                            if (address.AddressTypeId == AddressTypes.MailingAddress)
                            {
                                mailingAddress = address;
                                break;
                            }
                        }

                        if (mailingAddress == null)
                        {
                            mailingAddress = new ClientAddress();
                            mailingAddress.AddressTypeId = AddressTypes.MailingAddress;
                            clientDepartment.ClientAddresses.Add(mailingAddress);
                        }
                    }
                    mailingAddress.Address1 = txtMailingAddress1.Text.Trim();
                    mailingAddress.Address2 = txtMailingAddress2.Text.Trim();
                    mailingAddress.City = txtMailingCity.Text.Trim();
                    mailingAddress.State = cmbMailingState.Text.Trim();

                    string mailingZipCode = txtMailingZipCode.Text.Trim();
                    mailingZipCode = mailingZipCode.EndsWith("-") ? mailingZipCode.Replace("-", "") : mailingZipCode;

                    mailingAddress.ZipCode = mailingZipCode;

                    mailingAddress.LastModifiedBy = Program.currentUserName;
                }
                else
                {
                    ClientAddress mailingAddress = null;
                    if (this._mode == OperationMode.New)
                    {
                        mailingAddress = new ClientAddress();
                        mailingAddress.AddressTypeId = AddressTypes.MailingAddress;
                        clientDepartment.ClientAddresses.Add(mailingAddress);
                    }
                    else
                    {
                        foreach (ClientAddress address in clientDepartment.ClientAddresses)
                        {
                            if (address.AddressTypeId == AddressTypes.MailingAddress)
                            {
                                mailingAddress = address;
                                break;
                            }
                        }

                        if (mailingAddress == null)
                        {
                            mailingAddress = new ClientAddress();
                            mailingAddress.AddressTypeId = AddressTypes.MailingAddress;
                            clientDepartment.ClientAddresses.Add(mailingAddress);
                        }
                    }
                    mailingAddress.Address1 = txtMailingAddress1.Text.Trim();
                    mailingAddress.Address2 = txtMailingAddress2.Text.Trim();
                    mailingAddress.City = txtMailingCity.Text.Trim();
                    mailingAddress.State = cmbMailingState.Text.Trim();

                    string mailingZipCode = txtMailingZipCode.Text.Trim();
                    mailingZipCode = mailingZipCode.EndsWith("-") ? mailingZipCode.Replace("-", "") : mailingZipCode;

                    mailingAddress.ZipCode = mailingZipCode;

                    mailingAddress.LastModifiedBy = Program.currentUserName;
                }

                #endregion Mailing Address

                //Todo:MK  need to remove this at some point not sure what it is

                #region More commented out code for some reason

                //}
                //else
                //{
                //    DataTable dtclientDepartment = clientBL.GetClientContatListByClientId(this.ClientId);

                //    List<ClientContact> clientContactList = new List<ClientContact>();
                //    if (dtclientDepartment.Rows.Count <= 0)
                //    {
                //        MessageBox.Show("Client email is empty.");
                //        //chkSameAsClient.Checked = false;
                //        return false;
                //    }

                //    foreach (DataRow dr in dtclientDepartment.Rows)
                //    {
                //        ClientContact clientContact = new ClientContact();
                //        clientContact.ClientId = (int)dr["ClientId"];
                //        clientContact.ClientContactFirstName = dr["ClientContactFirstName"].ToString();
                //        clientContact.ClientContactLastName = dr["ClientContactLastName"].ToString();
                //        clientContact.ClientContactPhone = dr["ClientContactPhone"].ToString();
                //        clientContact.ClientContactFax = dr["ClientContactFax"].ToString();
                //        clientContact.ClientContactEmail = dr["ClientContactEmail"].ToString();

                //        if (clientContact.ClientContactEmail == string.Empty)
                //        {
                //            MessageBox.Show("Client email is empty.");
                //            chkSameAsClient.Checked = false;
                //            return false;
                //        }
                //    }

                //    clientDepartment.IsMailingAddressPhysical = false;
                //    clientDepartment.SalesRepresentativeId = null;

                //    clientDepartment.ClientContact.ClientContactFirstName = string.Empty;
                //    clientDepartment.ClientContact.ClientContactLastName = string.Empty;
                //    clientDepartment.ClientContact.ClientContactPhone = string.Empty;
                //    clientDepartment.ClientContact.ClientContactFax = string.Empty;
                //    clientDepartment.ClientContact.ClientContactEmail = string.Empty;

                //    clientDepartment.ClientAddresses = new List<ClientAddress>();
                //}

                #endregion More commented out code for some reason

                clientDepartment.LastModifiedBy = Program.currentUserName;

                int returnVal = clientBL.SaveClientDepartment(clientDepartment);

                if (returnVal > 0)
                {
                    if (this._mode == OperationMode.New && _clientNotificationDataSettings.client_id==0 && _clientNotificationDataSettings.client_department_id==0)
                    {
                        clientDepartment.ClientDepartmentId = returnVal;
                        _clientNotificationDataSettings.client_id = clientDepartment.ClientId;
                        _clientNotificationDataSettings.client_department_id = clientDepartment.ClientDepartmentId;
                    }
                    // set integrations
                    if (SaveNotificationSettingsFinal() == false)
                    {
                        return false;
                    }
                    //////foreach (IntegrationPartner _partner in this._partners)
                    //////{
                    //////    // find the checkbox
                    //////    //CheckBox box = _partnerCheckboxes.Where(pc => (int)pc.Tag == _partner.backend_integration_partner_id).First();
                    //////    RadioButton rdo = _partnerRadioButtons.Where(pc => (int)pc.Tag == _partner.backend_integration_partner_id).First();
                    //////    // if no integration exists, add it
                    //////    if (!_integrations.Exists(_i => _i.backend_integration_partner_id == _partner.backend_integration_partner_id && _i.client_id == this.ClientId && _i.client_department_id == this.ClientDepartmentId))
                    //////    {
                    //////        // add it
                    //////        RandomStringGenerator rsg = new RandomStringGenerator(true, true, true, false);
                    //////        string rndClientCode = rsg.Generate(10, 12);
                    //////        string rndClientId = rsg.Generate(10, 12);


                    //////        IntegrationPartnerClient integrationPartnerClient = new IntegrationPartnerClient()
                    //////        {
                    //////            active = true,
                    //////            backend_integration_partner_client_map_GUID = Guid.NewGuid(),
                    //////            backend_integration_partner_id = _partner.backend_integration_partner_id,
                    //////            client_department_id = this.ClientDepartmentId,
                    //////            partner_client_code = rndClientCode,
                    //////            partner_client_id = rndClientId,
                    //////            client_id = this.ClientId,
                    //////            require_login = rdoRequireLogin.Checked,
                    //////            require_remote_login = rdoRequireRemoteLogin.Checked,
                    //////            last_modified_by = Program.currentUserName,

                    //////        };

                    //////        _integrations.Add(integrationPartnerClient);
                    //////    }

                    //////    // set the partner to active or not
                    //////    _integrations.Where(_i => _i.backend_integration_partner_id == _partner.backend_integration_partner_id && _i.client_id == this.ClientId && _i.client_department_id == this.ClientDepartmentId).First().active = rdo.Checked;
                    //////    // set login requirements
                    //////    _integrations.Where(_i => _i.backend_integration_partner_id == _partner.backend_integration_partner_id && _i.client_id == this.ClientId && _i.client_department_id == this.ClientDepartmentId).First().require_login = false;
                    //////    _integrations.Where(_i => _i.backend_integration_partner_id == _partner.backend_integration_partner_id && _i.client_id == this.ClientId && _i.client_department_id == this.ClientDepartmentId).First().require_login = rdoRequireLogin.Checked;// chkRequireLogin.Checked;

                    //////    _integrations.Where(_i => _i.backend_integration_partner_id == _partner.backend_integration_partner_id && _i.client_id == this.ClientId && _i.client_department_id == this.ClientDepartmentId).First().require_remote_login = false;
                    //////    _integrations.Where(_i => _i.backend_integration_partner_id == _partner.backend_integration_partner_id && _i.client_id == this.ClientId && _i.client_department_id == this.ClientDepartmentId).First().require_remote_login = rdoRequireRemoteLogin.Checked;// chkRequireLogin.Checked;
                        
                    //////}
                    //////// update the partners
                    //////foreach (IntegrationPartnerClient _ip in _integrations)
                    //////{
                    //////    backendLogic.SetIntegrationPartnerClient(_ip);
                    //////}
                }

                if (returnVal > 0)
                {
                    if (this._mode == OperationMode.New)
                    {
                        clientDepartment.ClientDepartmentId = returnVal;
                        this.ClientDepartmentId = returnVal;
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
                Program.LogError(ex);
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private bool ValidateControls()
        {
            try
            {
                if (chkUseFormFox.Checked == true)
                {
                    if (string.IsNullOrEmpty(txtFormFoxCode.Text))
                    {
                        MessageBox.Show("A FormFox Lab code is required to enable FormFox notifications!", "FormFox labcode is required");

                        txtFormFoxCode.Focus();
                        return false;

                    }
                }

                //////if ((int)_partnerRadioButtons.Where(_r => _r.Checked == true).First().Tag > 0)
                //////{
                //////    if (rdoRequireLogin.Checked == false && rdoRequireRemoteLogin.Checked == false)
                //////    {
                //////        MessageBox.Show("Please select integration donor login requirements!", "Integrations require local or remote login");
                //////        //tabDeptDetailsTabs.SelectedIndex = tabDeptDetailsTabs.TabPages
                //////        var _tabidx = tabDeptDetailsTabs.TabPages.IndexOfKey("tabIntegrationsClientDepartmentDetails");
                //////        //tabDeptDetailsTabs.SelectTab("tabIntegrationsClientDepartmentDetails");
                //////        tabDeptDetailsTabs.SelectedIndex = _tabidx;
                //////        tabDeptDetailsTabs.Focus();
                //////        rdoRequireLogin.Focus();
                //////        return false;
                //////    }
                //////}
                // validate our json
                if (!_PDFengine.ValidateJson(txtJSONTemplateSettings.Text))
                {
                    MessageBox.Show("The setting JSON file are invalid as entered.\r\nPlease double check!", "Invalid JSON!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtJSONTemplateSettings.Focus();
                    return false;
                }

                if (txtFilenameRenderSettings.Text.Equals(_PDFengine.DefaultRenderSettingsFile, StringComparison.InvariantCultureIgnoreCase) && !txtJSONTemplateSettings.Text.Equals(_PDFengine.GetRenderSettingsAsJson(), StringComparison.InvariantCultureIgnoreCase))
                {
                    if (MessageBox.Show("This will overwrite the default config!\r\nAre you sure?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    {
                        txtFilenameRenderSettings.Focus();
                        return false;
                    }
                }

                if (txtDepartmentName.Text.Trim() == string.Empty)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Department Name cannot be empty.");
                    txtDepartmentName.Focus();
                    return false;
                }

                if (txtLabCode.Text.Trim() == string.Empty)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Lab Code cannot be empty.");
                    txtLabCode.Focus();
                    return false;
                }
                else
                {
                    if (!ValidateLabCode())
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Lab Code already exists.");
                        txtLabCode.Focus();
                        return false;
                    }
                }

                if (chkUA.Checked == false && chkHair.Checked == false && chkBC.Checked == false && chkDNA.Checked == false)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Test Category must be selected.");
                    //chkUA.Focus();
                    return false;
                }

                if (rbMPOS.Checked == false && rbMALL.Checked == false)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("MRO Types must be selected.");
                    rbMPOS.Focus();
                    return false;
                }
                if (rbDonorPays.Checked == false && rbInvoiceClient.Checked == false)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Payment Types must be selected.");
                    rbDonorPays.Focus();
                    return false;
                }
                //Todo: MK need to comment out at some point.. not sure why this wasn't taken out.

                #region - commented out code for some reason

                //_isPhysicalRequired = true;
                //_isMailingRequired = false;
                //_isMainContactInformation = true;

                //if (chkSameAsClient.Checked == true)
                //{
                //    #region Main Contact Information

                //    string contactFirstName = txtFirstName.Text.Trim();
                //    string contactLastName = txtLastName.Text.Trim();
                //    string contactPhone = txtPhone.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
                //    string contactFax = txtFax.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
                //    string contactEmail = txtEmail.Text.Trim();

                //    if (_isMainContactInformation)
                //    {
                //        if (contactFirstName == string.Empty)
                //        {
                //            Cursor.Current = Cursors.Default;
                //            MessageBox.Show("Main Contact Information -First Name can not be empty.");
                //            txtFirstName.Focus();
                //            return false;
                //        }

                //        if (contactLastName == string.Empty)
                //        {
                //            Cursor.Current = Cursors.Default;
                //            MessageBox.Show("Main Contact Information - Last Name can not be empty.");
                //            txtLastName.Focus();
                //            return false;
                //        }

                //        if (contactPhone == string.Empty)
                //        {
                //            Cursor.Current = Cursors.Default;
                //            MessageBox.Show("Main Contact Information - Phone number can not be empty.");
                //            txtPhone.Focus();
                //            return false;
                //        }
                //        else
                //        {
                //            if (!Program.regexPhoneNumber.IsMatch(txtPhone.Text.Trim()))
                //            {
                //                Cursor.Current = Cursors.Default;
                //                MessageBox.Show("Invalid format of Phone number (Main Contact Information).");
                //                txtPhone.Focus();
                //                return false;
                //            }
                //        }

                //        if (contactFax != string.Empty)
                //        {
                //            if (!Program.regexPhoneNumber.IsMatch(txtFax.Text.Trim()))
                //            {
                //                Cursor.Current = Cursors.Default;
                //                MessageBox.Show("Invalid format of Fax number (Main Contact Information).");
                //                txtFax.Focus();
                //                return false;
                //            }
                //        }

                //        if (contactEmail == string.Empty)
                //        {
                //            Cursor.Current = Cursors.Default;
                //            MessageBox.Show("Main Contact Information- Email can not be empty.");
                //            txtEmail.Focus();
                //            return false;
                //        }
                //        else
                //        {
                //            if (!Program.regexEmail.IsMatch(txtEmail.Text.Trim()))
                //            {
                //                Cursor.Current = Cursors.Default;
                //                MessageBox.Show("Invalid format of Email (Main Contact Information).");
                //                txtEmail.Focus();
                //                return false;
                //            }
                //            //else if (!ValidateEmail())
                //            //{
                //            //    Cursor.Current = Cursors.Default;
                //            //    MessageBox.Show("The email provided is already exists in database.");
                //            //    txtEmail.Focus();
                //            //    return false;
                //            //}
                //        }
                //    }

                //    #endregion

                //    #region Physical Address validation

                //    string physicalAddress1 = txtPhysicalAddress1.Text.Trim();
                //    string physicalAddress2 = txtPhysicalAddress2.Text.Trim();
                //    string physicalCity = txtPhysicalCity.Text.Trim();
                //    string physicalState = cmbPhysicalState.Text.Trim();
                //    string physicalZipCode = txtPhysicalZipCode.Text.Trim();

                //    physicalZipCode = physicalZipCode.EndsWith("-") ? physicalZipCode.Replace("-", "") : physicalZipCode;

                //    if (_isPhysicalRequired)
                //    {
                //        if (physicalAddress1 == string.Empty)
                //        {
                //            Cursor.Current = Cursors.Default;
                //            MessageBox.Show("Physical Address - Address 1 can not be empty.");
                //            txtPhysicalAddress1.Focus();
                //            return false;
                //        }

                //        if (physicalCity == string.Empty)
                //        {
                //            Cursor.Current = Cursors.Default;
                //            MessageBox.Show("Physical Address - City can not be empty.");
                //            txtPhysicalCity.Focus();
                //            return false;
                //        }

                //        if (physicalState.ToUpper() == "(Select)".ToUpper())
                //        {
                //            Cursor.Current = Cursors.Default;
                //            MessageBox.Show("Physical Address -State must be selected.");
                //            cmbPhysicalState.Focus();
                //            return false;
                //        }

                //        if (physicalZipCode == string.Empty)
                //        {
                //            Cursor.Current = Cursors.Default;
                //            MessageBox.Show("Physical Address - Zip Code can not be empty.");
                //            txtPhysicalZipCode.Focus();
                //            return false;
                //        }
                //        else
                //        {
                //            if (!Program.regexZipCode.IsMatch(physicalZipCode))
                //            {
                //                Cursor.Current = Cursors.Default;
                //                MessageBox.Show("Invalid format of Zip Code (Physical Address ).");
                //                txtPhysicalZipCode.Focus();
                //                return false;
                //            }
                //        }
                //    }

                //    #endregion

                //    #region Mailing Address Validation

                //    if (!chkSameAsPhysical.Checked)
                //    {
                //        string mailingAddress1 = txtMailingAddress1.Text.Trim();
                //        string mailingAddress2 = txtMailingAddress2.Text.Trim();
                //        string mailingCity = txtMailingCity.Text.Trim();
                //        string mailingState = cmbMailingState.Text.Trim();
                //        string mailingZipCode = txtMailingZipCode.Text.Trim();

                //        mailingZipCode = mailingZipCode.EndsWith("-") ? mailingZipCode.Replace("-", "") : mailingZipCode;

                //        _isMailingRequired = true;

                //        if (_isMailingRequired)
                //        {
                //            if (mailingAddress1 == string.Empty)
                //            {
                //                Cursor.Current = Cursors.Default;
                //                MessageBox.Show("Mailing Address - Address 1 can not be empty.");
                //                txtMailingAddress1.Focus();
                //                return false;
                //            }

                //            if (mailingCity == string.Empty)
                //            {
                //                Cursor.Current = Cursors.Default;
                //                MessageBox.Show("Mailing Address - City can not be empty.");
                //                txtMailingCity.Focus();
                //                return false;
                //            }

                //            if (mailingState.ToUpper() == "(Select)".ToUpper())
                //            {
                //                Cursor.Current = Cursors.Default;
                //                MessageBox.Show("Mailing Address - State must be selected.");
                //                cmbMailingState.Focus();
                //                return false;
                //            }

                //            if (mailingZipCode == string.Empty)
                //            {
                //                Cursor.Current = Cursors.Default;
                //                MessageBox.Show("Mailing Address - Zip Code can not be empty.");
                //                txtMailingZipCode.Focus();
                //                return false;
                //            }
                //            else
                //            {
                //                if (!Program.regexZipCode.IsMatch(mailingZipCode))
                //                {
                //                    Cursor.Current = Cursors.Default;
                //                    MessageBox.Show("Invalid format of Zip Code (Mailing Address).");
                //                    txtMailingZipCode.Focus();
                //                    return false;
                //                }
                //            }
                //        }
                //    }

                //    #endregion
                //}
                // if (chkSameAsClient.Checked == false)
                // {
                //     #region Main Contact Information

                //     string contactFirstName = txtFirstName.Text.Trim();
                //     string contactLastName = txtLastName.Text.Trim();
                //     string contactPhone = txtPhone.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
                //     string contactFax = txtFax.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
                //     string contactEmail = txtEmail.Text.Trim();

                //     if (_isMainContactInformation)
                //     {
                //         if (contactFirstName == string.Empty)
                //         {
                //             Cursor.Current = Cursors.Default;
                //             MessageBox.Show("Main Contact Information -First Name cannot be empty.");
                //             txtFirstName.Focus();
                //             return false;
                //         }

                //         if (contactLastName == string.Empty)
                //         {
                //             Cursor.Current = Cursors.Default;
                //             MessageBox.Show("Main Contact Information - Last Name cannot be empty.");
                //             txtLastName.Focus();
                //             return false;
                //         }

                //         if (contactPhone == string.Empty)
                //         {
                //             Cursor.Current = Cursors.Default;
                //             MessageBox.Show("Main Contact Information - Phone number cannot be empty.");
                //             txtPhone.Focus();
                //             return false;
                //         }
                //         else
                //         {
                //             if (!Program.regexPhoneNumber.IsMatch(txtPhone.Text.Trim()))
                //             {
                //                 Cursor.Current = Cursors.Default;
                //                 MessageBox.Show("Invalid format of Phone number (Main Contact Information).");
                //                 txtPhone.Focus();
                //                 return false;
                //             }
                //         }

                //         if (contactFax != string.Empty)
                //         {
                //             if (!Program.regexPhoneNumber.IsMatch(txtFax.Text.Trim()))
                //             {
                //                 Cursor.Current = Cursors.Default;
                //                 MessageBox.Show("Invalid format of Fax number (Main Contact Information).");
                //                 txtFax.Focus();
                //                 return false;
                //             }
                //         }

                //         if (contactEmail == string.Empty)
                //         {
                //             Cursor.Current = Cursors.Default;
                //             MessageBox.Show("Main Contact Information- Email cannot be empty.");
                //             txtEmail.Focus();
                //             return false;
                //         }
                //         else
                //         {
                //             if (!Program.regexEmail.IsMatch(txtEmail.Text.Trim()))
                //             {
                //                 Cursor.Current = Cursors.Default;
                //                 MessageBox.Show("Invalid format of Email (Main Contact Information).");
                //                 txtEmail.Focus();
                //                 return false;
                //             }
                //             //else if (!ValidateEmail())
                //             //{
                //             //    Cursor.Current = Cursors.Default;
                //             //    MessageBox.Show("The email provided is already exists in database.");
                //             //    txtEmail.Focus();
                //             //    return false;
                //             //}
                //         }
                //     }

                //     #endregion

                //     #region Physical Address validation

                //     string physicalAddress1 = txtPhysicalAddress1.Text.Trim();
                //     string physicalAddress2 = txtPhysicalAddress2.Text.Trim();
                //     string physicalCity = txtPhysicalCity.Text.Trim();
                //     string physicalState = cmbPhysicalState.Text.Trim();
                //     string physicalZipCode = txtPhysicalZipCode.Text.Trim();

                //     physicalZipCode = physicalZipCode.EndsWith("-") ? physicalZipCode.Replace("-", "") : physicalZipCode;

                //     if (_isPhysicalRequired)
                //     {
                //         if (physicalAddress1 == string.Empty)
                //         {
                //             Cursor.Current = Cursors.Default;
                //             MessageBox.Show("Physical Address - Address 1 cannot be empty.");
                //             txtPhysicalAddress1.Focus();
                //             return false;
                //         }

                //         if (physicalCity == string.Empty)
                //         {
                //             Cursor.Current = Cursors.Default;
                //             MessageBox.Show("Physical Address - City cannot be empty.");
                //             txtPhysicalCity.Focus();
                //             return false;
                //         }

                //         if (physicalState.ToUpper() == "(Select)".ToUpper())
                //         {
                //             Cursor.Current = Cursors.Default;
                //             MessageBox.Show("Physical Address -State must be selected.");
                //             cmbPhysicalState.Focus();
                //             return false;
                //         }

                //         if (physicalZipCode == string.Empty)
                //         {
                //             Cursor.Current = Cursors.Default;
                //             MessageBox.Show("Physical Address - Zip Code cannot be empty.");
                //             txtPhysicalZipCode.Focus();
                //             return false;
                //         }
                //         else
                //         {
                //             if (!Program.regexZipCode.IsMatch(physicalZipCode))
                //             {
                //                 Cursor.Current = Cursors.Default;
                //                 MessageBox.Show("Invalid format of Zip Code (Physical Address ).");
                //                 txtPhysicalZipCode.Focus();
                //                 return false;
                //             }
                //         }
                //     }

                //     #endregion

                //     #region Mailing Address Validation

                //     if (!chkSameAsPhysical.Checked)
                //     {
                //         string mailingAddress1 = txtMailingAddress1.Text.Trim();
                //         string mailingAddress2 = txtMailingAddress2.Text.Trim();
                //         string mailingCity = txtMailingCity.Text.Trim();
                //         string mailingState = cmbMailingState.Text.Trim();
                //         string mailingZipCode = txtMailingZipCode.Text.Trim();

                //         mailingZipCode = mailingZipCode.EndsWith("-") ? mailingZipCode.Replace("-", "") : mailingZipCode;

                //         _isMailingRequired = true;

                //         if (_isMailingRequired)
                //         {
                //             if (mailingAddress1 == string.Empty)
                //             {
                //                 Cursor.Current = Cursors.Default;
                //                 MessageBox.Show("Mailing Address - Address 1 cannot be empty.");
                //                 txtMailingAddress1.Focus();
                //                 return false;
                //             }

                //             if (mailingCity == string.Empty)
                //             {
                //                 Cursor.Current = Cursors.Default;
                //                 MessageBox.Show("Mailing Address - City cannot be empty.");
                //                 txtMailingCity.Focus();
                //                 return false;
                //             }

                //             if (mailingState.ToUpper() == "(Select)".ToUpper())
                //             {
                //                 Cursor.Current = Cursors.Default;
                //                 MessageBox.Show("Mailing Address - State must be selected.");
                //                 cmbMailingState.Focus();
                //                 return false;
                //             }

                //             if (mailingZipCode == string.Empty)
                //             {
                //                 Cursor.Current = Cursors.Default;
                //                 MessageBox.Show("Mailing Address - Zip Code cannot be empty.");
                //                 txtMailingZipCode.Focus();
                //                 return false;
                //             }
                //             else
                //             {
                //                 if (!Program.regexZipCode.IsMatch(mailingZipCode))
                //                 {
                //                     Cursor.Current = Cursors.Default;
                //                     MessageBox.Show("Invalid format of Zip Code (Mailing Address).");
                //                     txtMailingZipCode.Focus();
                //                     return false;
                //                 }
                //             }
                //         }
                //     }

                //     #endregion
                //}

                #endregion - commented out code for some reason
            }
            catch (Exception ex)
            {
                Program.LogError(ex);
                MessageBox.Show(ex.Message);
                return false;
            }
            return true;
        }

        private bool ValidateEmail()
        {
            try
            {
                DataTable clientDepartment = clientBL.GetByEmail(txtEmail.Text.Trim());

                if (clientDepartment.Rows.Count > 0 && this._mode == OperationMode.New)
                {
                    return false;
                }
                else if (clientDepartment.Rows.Count > 1 && this._mode == OperationMode.Edit)
                {
                    return false;
                }
                else if (clientDepartment.Rows.Count == 1 && this._mode == OperationMode.Edit)
                {
                    if ((int)clientDepartment.Rows[0]["ClientDepartmentId"] != this._clientDepartmentId)
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.LogError(ex);
                MessageBox.Show(ex.Message);
            }
            return true;
        }

        private void LoadSalesRepresentatives()
        {
            try
            {
                UserBL userBL = new UserBL();
                List<User> userList = userBL.GetListByDepartment("SALES");

                User tmpUser = new User();
                tmpUser.UserId = 0;
                tmpUser.UserFirstName = "(Select)";

                userList.Insert(0, tmpUser);

                cmbSalesRepresentative.Items.Clear();
                cmbSalesRepresentative.DataSource = userList;
                cmbSalesRepresentative.ValueMember = "UserId";
                cmbSalesRepresentative.DisplayMember = "UserDisplayName";
            }
            catch (Exception ex)
            {
                Program.LogError(ex);
                MessageBox.Show(ex.Message);
            }
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

        public int ClientId
        {
            get
            {
                return this._clientId;
            }
            set
            {
                this._clientId = value;
            }
        }

        public int ClientDepartmentId
        {
            get
            {
                return this._clientDepartmentId;
            }
            set
            {
                this._clientDepartmentId = value;
            }
        }

        #endregion Public Properties

        private void txtPhysicalZipCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back)))
                e.Handled = true;
        }

        private void txtMailingZipCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back)))
                e.Handled = true;
        }

        private void txtLabCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (!char.IsControl(e.KeyChar) && (e.KeyChar != '.'))
            //{
            //    e.Handled = true;
            //}
        }

        private void txtLabCode_TextChanged(object sender, EventArgs e)
        {
            txtLabCode.CausesValidation = false;
        }

        private bool ValidateLabCode()
        {
            try
            {
                DataTable clientDepartmentLabCode = clientBL.GetClientLabCode(txtLabCode.Text.Trim());

                if (clientDepartmentLabCode.Rows.Count > 0 && this._mode == OperationMode.New)
                {
                    return false;
                }
                else if (clientDepartmentLabCode.Rows.Count > 1 && this._mode == OperationMode.Edit)
                {
                    return false;
                }
                else if (clientDepartmentLabCode.Rows.Count == 1 && this._mode == OperationMode.Edit)
                {
                    if ((int)clientDepartmentLabCode.Rows[0]["ClientDepartmentId"] != this._clientDepartmentId)
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.LogError(ex);
                MessageBox.Show(ex.Message);
                return false;
            }

            return true;
        }

        private void btnClearStarAssign_Click(object sender, EventArgs e)
        {
            _logger.Debug($"btnClearStarAssign_Click clicked");
            SurPath.Business.Master.ClearStar cs = new Business.Master.ClearStar();

            if (!chkBC.Checked)
            {
                chkBC.Checked = true;
            }
            //Make Clearstar Call to create new Company Profile.
            var creds = DefaultCredentialsBL.GetCredentials();
            Surpath.ClearStar.BL.CustomerBL service = new CustomerBL(_logger);
            CustomerModel cust = new CustomerModel();
            long ticks = DateTime.Now.Ticks;
            byte[] bytes = BitConverter.GetBytes(ticks);
            string id = Convert.ToBase64String(bytes)
                                    .Replace('+', '_')
                                    .Replace('/', '-')
                                    .TrimEnd('=');

            cust.FullName = this.Tag + " - " + txtDepartmentName.Text + " [" + id + "]"; //txtLabCode.Text;  //this is the same short code for OVN. //txtFirstName.Text + " " + txtLastName.Text;
            cust.City = "Plano";//txtPhysicalCity.Text;
            cust.State = "TX";// txtPhysicalState.Text;
            cust.Zip = "75074";// txtPhysicalZipCode.Text;
            cust.Phone = "972-633-1388";// txtPhone.Text;
            cust.Address1 = "2030 G Ave, Ste 1102";// txtPhysicalAddress1.Text;
            cust.Comments = "Department Name: " + txtDepartmentName.Text;
            cust.Email = "david@surscan.com";//txtEmail.Text;
            cust.ShortName = txtLabCode.Text;
            CustomerModel newcust = service.CreateCustomer(cust, "tester,", "test");

            var res = service.AddCustomerService(newcust.CustomerId, "SLSS_10092", "Background Check");
            //res = service.AddCustomerService("SLSS_00043", "SLSS_00001", "National Criminal Database (NCD) Check");  //ToDo:Not sure we need this additional item.  the above adss 5 items.
            this.txtClearStarClientCode.Text = newcust.CustomerId;
            //department
            this.btnClearStarAssign.Visible = false;
        }

        private void btnAssignServices_Click(object sender, EventArgs e)
        {
            var CustomerId = this.txtClearStarClientCode.Text;
            var creds = DefaultCredentialsBL.GetCredentials();
            Surpath.ClearStar.BL.CustomerBL service = new CustomerBL();
            var res = service.AddCustomerService(CustomerId, "SLSS_10092", "Background Check");//this is on test system
            res = service.AddCustomerService(CustomerId, "SLSS_10095", "Statewide Repository Search");
            res = service.AddCustomerService(CustomerId, "SLSS_10002", "National Criminal Database All Names (NCD)");
            res = service.AddCustomerService(CustomerId, "SLSS_10010", "County Criminal Record Search");
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabDeptDetailsTabs.SelectedIndex == 1)
            {
                this.LoadClientDoctypes();
            }
            if (this.tabDeptDetailsTabs.SelectedIndex == 2)
            {
            }

            // move save and cancel buttons
            //listBox1.Parent = tabControl1.TabPages[1];
            btnOk.Parent = tabDeptDetailsTabs.TabPages[tabDeptDetailsTabs.SelectedIndex];
            btnClose.Parent = tabDeptDetailsTabs.TabPages[tabDeptDetailsTabs.SelectedIndex];
        }

        private void LoadClientDoctypes()
        {
            try
            {
                ClientBL clientBL = new ClientBL();
                DataTable client = clientBL.GetClientDepartmentDocTypes(this._clientDepartmentId);
                DataGridViewCheckBoxColumn c1 = new DataGridViewCheckBoxColumn();

                this.dg.Columns.Add("client_departmentdoctypeid", "Id");
                this.dg.Columns.Add("client_department_id", "Client Id");
                this.dg.Columns.Add("created_on", "Created");
                this.dg.Columns.Add("description", "Description");
                this.dg.Columns.Add("instructions", "Instructions");
                this.dg.Columns.Add("duedate", "Date Due");
                this.dg.Columns.Add("semester", "Semester");

                DataGridViewCheckBoxColumn col = new DataGridViewCheckBoxColumn();
                col.DataPropertyName = "is_notifystudent";
                col.Name = "is_notifystudent";
                col.HeaderText = "Notify";
                this.dg.Columns.Add(col);
                this.dg.Columns.Add("notifydays1", "Notify1");
                this.dg.Columns.Add("notifydays2", "Notify2");
                this.dg.Columns.Add("notifydays3", "Notify3");
                DataGridViewCheckBoxColumn col2 = new DataGridViewCheckBoxColumn();
                col2.DataPropertyName = "is_doesexpire";
                col2.Name = "is_doesexpire";
                col2.HeaderText = "Expires";
                this.dg.Columns.Add(col2);
                DataGridViewCheckBoxColumn col3 = new DataGridViewCheckBoxColumn();
                col3.DataPropertyName = "is_required";
                col3.Name = "is_required";
                col3.HeaderText = "Required";
                this.dg.Columns.Add(col3);
                DataGridViewCheckBoxColumn col4 = new DataGridViewCheckBoxColumn();
                col4.DataPropertyName = "is_archived";
                col4.Name = "is_archived";
                col4.HeaderText = "Archived";
                this.dg.Columns.Add(col4);
                this.dg.AutoGenerateColumns = false;

                this.dg.DataSource = client;
                this.dg.Columns[0].DataPropertyName = "client_departmentdoctypeid";
                //this.dg.Columns[0].Visible = false;
                this.dg.Columns[1].DataPropertyName = "client_department_id";
                this.dg.Columns[1].Visible = false;
                this.dg.Columns[2].DataPropertyName = "created_on";

                this.dg.Columns[3].DataPropertyName = "Description";
                this.dg.Columns[4].DataPropertyName = "Instructions";
                this.dg.Columns[5].DataPropertyName = "duedate";
                this.dg.Columns[6].DataPropertyName = "semester";
                //this.dg.Columns[7].DataPropertyName = "Notify Student";
                //this.dgvClientDepartment.Columns[7].ValueType = System.Type.GetType("System.Boolean");
                this.dg.Columns[8].DataPropertyName = "notifydays1";
                this.dg.Columns[9].DataPropertyName = "notifydays2";
                this.dg.Columns[10].DataPropertyName = "notifydays3";
                //this.dg.Columns[11].DataPropertyName = "Expires";
                //this.dg.Columns[12].DataPropertyName = "Required";
                //this.dg.Columns[13].DataPropertyName = "Archived";
                this.dg.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                this.dg.AutoResizeColumns();
                this.dg.Update();
            }
            catch (Exception ex)
            {
                Program.LogError(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void Dg_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (e.RowIndex >= 0)
                {
                    int selectedIndex = e.RowIndex;
                    this.txtID.Text = ((int)dg.Rows[e.RowIndex].Cells["client_departmentdoctypeid"].Value).ToString();
                    this.txtDescription.Text = (string)dg.Rows[e.RowIndex].Cells["Description"].Value;
                    this.txtInstructions.Text = (string)dg.Rows[e.RowIndex].Cells["Instructions"].Value;
                    this.txtSemester.Text = (string)dg.Rows[e.RowIndex].Cells["Semester"].Value;

                    this.txtNotify1.Text = ((int)dg.Rows[e.RowIndex].Cells["notifydays1"].Value).ToString();
                    this.txtNotify2.Text = ((int)dg.Rows[e.RowIndex].Cells["notifydays2"].Value).ToString();
                    this.txtNotify3.Text = ((int)dg.Rows[e.RowIndex].Cells["notifydays3"].Value).ToString();
                    this.dtDueDate.Value = ((DateTime)dg.Rows[e.RowIndex].Cells["duedate"].Value);
                    //this.dtDueDate.Text=
                    if (Convert.ToBoolean(dg.Rows[e.RowIndex].Cells["is_notifystudent"].Value))
                    {
                        this.chkNotifyStudent.Checked = true;
                    }
                    else
                    {
                        this.chkNotifyStudent.Checked = false;
                    }
                    if (Convert.ToBoolean(dg.Rows[e.RowIndex].Cells["is_required"].Value))
                    {
                        this.chkRequired.Checked = true;
                    }
                    else
                    {
                    }
                    if (Convert.ToBoolean(dg.Rows[e.RowIndex].Cells["is_archived"].Value))
                    {
                        this.chkArchived.Checked = true;
                    }
                    else
                    {
                        this.chkArchived.Checked = false;
                    }
                    if (Convert.ToBoolean(dg.Rows[e.RowIndex].Cells["is_doesexpire"].Value))
                    {
                        this.chkExpires.Checked = true;
                    }
                    else
                    {
                        this.chkArchived.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.LogError(ex);
            }
        }

        private void BtnClearFields_Click(object sender, EventArgs e)
        {
            ClearClientDocumentForm();
        }

        private void ClearClientDocumentForm()
        {
            this.txtDescription.Text = string.Empty;
            this.txtInstructions.Text = string.Empty;
            this.txtSemester.Text = string.Empty;
            this.txtNotify1.Text = string.Empty;
            this.txtNotify2.Text = string.Empty;
            this.txtNotify3.Text = string.Empty;
            this.dtDueDate.ResetText();
            this.chkNotifyStudent.Checked = false;
            this.chkRequired.Checked = false;
            this.chkArchived.Checked = false;
            this.chkArchived.Checked = false;
            this.txtID.Text = string.Empty;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtID.Text))
            {
                AddClientDepartmentDocType();
                ClearClientDocumentForm();
            }
        }

        private void AddClientDepartmentDocType()
        {
            ClientDocTypes cdoc = new ClientDocTypes();
            cdoc.ClientDepartmentId = this._clientDepartmentId;
            cdoc.Description = this.txtDescription.Text;
            cdoc.Instructions = this.txtInstructions.Text;
            cdoc.Semester = this.txtSemester.Text;
            cdoc.NotifyDays1 = Convert.ToInt32(this.txtNotify1.Text);
            cdoc.NotifyDays2 = Convert.ToInt32(this.txtNotify2.Text);
            cdoc.NotifyDays3 = Convert.ToInt32(this.txtNotify3.Text);
            cdoc.DueDate = this.dtDueDate.Value;
            cdoc.IsNotifyStudent = this.chkNotifyStudent.Checked;
            cdoc.IsRequired = this.chkRequired.Checked;
            cdoc.IsDoesExpire = this.chkExpires.Checked;
            cdoc.IsArchived = this.chkArchived.Checked;

            ClientBL clientBL = new ClientBL();
            var retval = clientBL.SaveClientDocType(cdoc);
            this.LoadClientDoctypes();
            MessageBox.Show("New document type added", "Add Complete");
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to update record: " + this.txtDescription.Text + "?", "Update Record?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                ClientDocTypes cdoc = new ClientDocTypes();
                cdoc.ClientDoctypeId = Convert.ToInt32(this.txtID.Text);
                cdoc.ClientDepartmentId = this._clientDepartmentId;
                cdoc.Description = this.txtDescription.Text;
                cdoc.Instructions = this.txtInstructions.Text;
                cdoc.Semester = this.txtSemester.Text;
                cdoc.NotifyDays1 = Convert.ToInt32(this.txtNotify1.Text);
                cdoc.NotifyDays2 = Convert.ToInt32(this.txtNotify2.Text);
                cdoc.NotifyDays3 = Convert.ToInt32(this.txtNotify3.Text);
                cdoc.DueDate = this.dtDueDate.Value;
                cdoc.IsNotifyStudent = this.chkNotifyStudent.Checked;
                cdoc.IsRequired = this.chkRequired.Checked;
                cdoc.IsDoesExpire = this.chkExpires.Checked;
                cdoc.IsArchived = this.chkArchived.Checked;

                ClientBL clientBL = new ClientBL();
                var retval = clientBL.SaveClientDocType(cdoc);
                this.LoadClientDoctypes();

                MessageBox.Show("Update Successful", "Update Complete");
                ClearClientDocumentForm();
            }
            else if (dialogResult == DialogResult.No)
            {
            }
        }

        private void LblID_TextChanged(object sender, EventArgs e)
        {
        }

        private void TxtID_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                this.btnAdd.Enabled = true;
                this.btnAddRetain.Enabled = true;
            }
            else
            {
                this.btnAdd.Enabled = false;
                this.btnAddRetain.Enabled = false;
            }
        }

        private void BtnAddRetain_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtID.Text))
            {
                AddClientDepartmentDocType();
                txtID.Text = string.Empty;
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Load the default JSON
        }


        private void radASAPorDelay_CheckedChanged(object sender, EventArgs e)
        {
        }

        private bool SaveNotificationSettingsFinal()
        {
            bool retval = false;
            try
            {
               
                SaveNotificationSettings();
                // MessageBox.Show($"Settings Saved!", "Saved!");
                Cursor.Current = Cursors.Default;
                retval = true;
            }
            catch (Exception)
            {

                throw;
            }
            return retval;
        }

        private void btnSaveNotification_Click(object sender, EventArgs e)
        {
            SaveNotificationSettingsFinal();

        }

        private void SaveNotificationSettings()
        {
            #region NotificationOptions

            _clientNotificationDataSettings.force_manual = this.chkForceManualNotificaitons.Checked;
            _clientNotificationDataSettings.notification_sweep_date = dtSweepDate.Value;
            _clientNotificationDataSettings.notification_start_date = dtSendInStart.Value;
            _clientNotificationDataSettings.notification_stop_date = dtSendInStop.Value;
            _clientNotificationDataSettings.send_asap = this.radASAPorDelay1.Checked;
            _clientNotificationDataSettings.onsite_testing = this.chkOnsiteTesting.Checked;
            _clientNotificationDataSettings.enable_sms = this.chkEnableSMS.Checked;
            _clientNotificationDataSettings.use_formfox = this.chkUseFormFox.Checked;
            _clientNotificationDataSettings.delay_in_hours = (int)this.numDelayHours.Value;
            _clientNotificationDataSettings.show_web_notify_button = this.ckbWebShowNotifyButton.Checked;

            _clientNotificationDataSettings.deadline_alert_in_days = (int)this.nudDeadlineAlert.Value;
            _clientNotificationDataSettings.max_sendins = (int)this.nudMaxSendIns.Value;

            #endregion NotificationOptions

            #region SMSReply

            _clientNotificationDataSettings.client_autoresponse = this.txtSMSReply.Text;

            #endregion SMSReply

            #region WindowSettings

            foreach (ClientNotificationDataSettingsDay daySettings in _clientNotificationDataSettings.DaySettings)
            {
                switch (daySettings.DayOfWeek)
                {
                    case (int)DayOfWeekEnum.Sunday:
                        GetDayValues(chkSunday, dtSundayStart, dtSundayEnd, daySettings);
                        break;

                    case (int)DayOfWeekEnum.Monday:
                        GetDayValues(chkMonday, dtMondayStart, dtMondayEnd, daySettings);
                        break;

                    case (int)DayOfWeekEnum.Tuesday:
                        GetDayValues(chkTuesday, dtTuesdayStart, dtTuesdayEnd, daySettings);
                        break;

                    case (int)DayOfWeekEnum.Wednesday:
                        GetDayValues(chkWednesday, dtWednesdayStart, dtWednesdayEnd, daySettings);
                        break;

                    case (int)DayOfWeekEnum.Thursday:
                        GetDayValues(chkThursday, dtThursdayStart, dtThursdayEnd, daySettings);
                        break;

                    case (int)DayOfWeekEnum.Friday:
                        GetDayValues(chkFriday, dtFridayStart, dtFridayEnd, daySettings);
                        break;

                    case (int)DayOfWeekEnum.Saturday:
                        GetDayValues(chkSaturday, dtSaturdayStart, dtSaturdayEnd, daySettings);
                        break;
                }
            }

            #endregion WindowSettings

            #region PDFSettings

            if (string.IsNullOrEmpty(txtFilenameRenderSettings.Text))
            {
                txtFilenameRenderSettings.Text = this.ClientId.ToString() + "_" + this.ClientDepartmentId + "_RenderSettings.json";
            }
            txtJSONTemplateSettings.Text = _PDFengine.GetRenderSettingsAsJson();

            _PDFengine.SetRenderSettingsFromJson(txtJSONTemplateSettings.Text);

            #endregion PDFSettings

            if (!txtGlobalMROName.Text.Equals(_PDFengine.EngineSettings.Settings["MROName"], StringComparison.InvariantCultureIgnoreCase))
            {
                _PDFengine.EngineSettings.Settings["MROName"] = txtGlobalMROName.Text;
            }
            _PDFengine.SaveEngineSettings();
            _PDFengine.LoadSettings();
            _clientNotificationDataSettings.pdf_render_settings_filename = txtFilenameRenderSettings.Text;
            //_clientNotificationDataSettings.pdf_template_filename = txtFilenamePDFTemplate.Text; // don't save this as it's stored in the json, only display if it's set
            _PDFengine.SaveRenderSettingsToFile(txtFilenameRenderSettings.Text);
            //ParamSetClientNotificationSettings p = new ParamSetClientNotificationSettings();
            //p.clientNotificationDataSettings = _clientNotificationDataSettings;
            _clientNotificationDataSettings.last_modified_by = Program.currentUserName;
            //_backendData.SetClientNotificationSettings(p);

            //var _orig = _clientNotificationDataSettings_reset;
            //var _new = _clientNotificationDataSettings;
   


            int _id= backendLogic.SetClientNotificationSettings(_clientNotificationDataSettings);

            string _new = JsonConvert.SerializeObject(_clientNotificationDataSettings);
            try
            {
                if (_clientNotificationDataSettings_original_json != _new)
                {
                    backendLogic.SetBackend_client_edit_activity(_id, "Notification settings modified.", Program.currentUserId);
                }
            }
            catch (Exception ex)
            {
                Program.LogError(ex);
                
            }

        }
        private ClientNotificationDataSettings GetCurrentNotificationSettings()
        {
            #region NotificationOptions
            ClientNotificationDataSettings __clientNotificationDataSettings = backendLogic.CloneClientNotificationDataSettings(_clientNotificationDataSettings);




            __clientNotificationDataSettings.force_manual = this.chkForceManualNotificaitons.Checked;
            __clientNotificationDataSettings.notification_sweep_date = dtSweepDate.Value;
            __clientNotificationDataSettings.notification_start_date = dtSendInStart.Value;
            __clientNotificationDataSettings.notification_stop_date = dtSendInStop.Value;
            __clientNotificationDataSettings.send_asap = this.radASAPorDelay1.Checked;
            __clientNotificationDataSettings.onsite_testing = this.chkOnsiteTesting.Checked;
            __clientNotificationDataSettings.enable_sms = this.chkEnableSMS.Checked;
            __clientNotificationDataSettings.delay_in_hours = (int)this.numDelayHours.Value;
            __clientNotificationDataSettings.show_web_notify_button = this.ckbWebShowNotifyButton.Checked;

            __clientNotificationDataSettings.deadline_alert_in_days = (int)this.nudDeadlineAlert.Value;
            __clientNotificationDataSettings.max_sendins = (int)this.nudMaxSendIns.Value;

            #endregion NotificationOptions

            #region SMSReply

            __clientNotificationDataSettings.client_autoresponse = this.txtSMSReply.Text;

            #endregion SMSReply

            #region WindowSettings

            foreach (ClientNotificationDataSettingsDay daySettings in __clientNotificationDataSettings.DaySettings)
            {
                switch (daySettings.DayOfWeek)
                {
                    case (int)DayOfWeekEnum.Sunday:
                        GetDayValues(chkSunday, dtSundayStart, dtSundayEnd, daySettings);
                        break;

                    case (int)DayOfWeekEnum.Monday:
                        GetDayValues(chkMonday, dtMondayStart, dtMondayEnd, daySettings);
                        break;

                    case (int)DayOfWeekEnum.Tuesday:
                        GetDayValues(chkTuesday, dtTuesdayStart, dtTuesdayEnd, daySettings);
                        break;

                    case (int)DayOfWeekEnum.Wednesday:
                        GetDayValues(chkWednesday, dtWednesdayStart, dtWednesdayEnd, daySettings);
                        break;

                    case (int)DayOfWeekEnum.Thursday:
                        GetDayValues(chkThursday, dtThursdayStart, dtThursdayEnd, daySettings);
                        break;

                    case (int)DayOfWeekEnum.Friday:
                        GetDayValues(chkFriday, dtFridayStart, dtFridayEnd, daySettings);
                        break;

                    case (int)DayOfWeekEnum.Saturday:
                        GetDayValues(chkSaturday, dtSaturdayStart, dtSaturdayEnd, daySettings);
                        break;
                }
            }

            #endregion WindowSettings

            #region PDFSettings

            if (string.IsNullOrEmpty(txtFilenameRenderSettings.Text))
            {
                txtFilenameRenderSettings.Text = this.ClientId.ToString() + "_" + this.ClientDepartmentId + "_RenderSettings.json";
            }
            txtJSONTemplateSettings.Text = _PDFengine.GetRenderSettingsAsJson();

            _PDFengine.SetRenderSettingsFromJson(txtJSONTemplateSettings.Text);

            #endregion PDFSettings

            if (!txtGlobalMROName.Text.Equals(_PDFengine.EngineSettings.Settings["MROName"], StringComparison.InvariantCultureIgnoreCase))
            {
                _PDFengine.EngineSettings.Settings["MROName"] = txtGlobalMROName.Text;
            }
            _PDFengine.SaveEngineSettings();
            _PDFengine.LoadSettings();
            __clientNotificationDataSettings.pdf_render_settings_filename = txtFilenameRenderSettings.Text;
            _PDFengine.SaveRenderSettingsToFile(txtFilenameRenderSettings.Text);
            __clientNotificationDataSettings.last_modified_by = Program.currentUserName;
            return __clientNotificationDataSettings;
        }

        private void txtJSONTemplateSettings_Leave(object sender, EventArgs e)
        {
            if (!_PDFengine.ValidateJson(txtJSONTemplateSettings.Text))
            {
                MessageBox.Show("The setting JSON file are invalid as entered.\r\nPlease double check!", "Invalid JSON!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtJSONTemplateSettings.Focus();
            }
            _PDFengine.SetRenderSettingsFromJson(txtJSONTemplateSettings.Text);
            txtFilenamePDFTemplate.Text = _PDFengine.RenderSettings.TemplateFileName;
            if (string.IsNullOrEmpty(_PDFengine.RenderSettings.TemplateFileName))
            {
                txtFilenamePDFTemplate.Text = _PDFengine.DefaultTemplate;
            }
        }

        private void btnAdvanced_Click(object sender, EventArgs e)
        {
            gboxAdvanced.Visible = !gboxAdvanced.Visible;

            //if ((int)this.WindowState == (int)FormWindowState.Maximized) return;
            //if (gboxAdvanced.Visible)
            //{
            //    if (this.Width < gboxAdvanced.Width + gboxAdvanced.Left )
            //    {
            //        this.Width = gboxAdvanced.Width + gboxAdvanced.Left;
            //    }

            //}else
            //{
            //    this.Width = this.MinimumSize.Width;
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cmboCopyWindowSettings.SelectedIndex > -1)
            {
                int _idx = cmboCopyWindowSettings.SelectedIndex;
                int backend_notification_window_data_id = (int)cmboCopyWindowSettings.SelectedValue;
                ClientNotificationDataSettings clientNotificationDataSettings_source = backendLogic.GetClientNotificationDataSettingsById(backend_notification_window_data_id);
                ClientNotificationDataSettings t = _clientNotificationDataSettings;
                _clientNotificationDataSettings = clientNotificationDataSettings_source;
                _clientNotificationDataSettings.client_id = t.client_id;
                _clientNotificationDataSettings.client_department_id = t.client_department_id;
                _clientNotificationDataSettings.client_name = t.client_name;

                SetNotificationValuesOnForm(_clientNotificationDataSettings, _PDFengine);
            }
        }

        private void btnResetNotification_Click(object sender, EventArgs e)
        {
            SetNotificationValuesOnForm(_clientNotificationDataSettings_reset, _PDFengine_reset);
        }

        private void chkForceManualNotificaitons_CheckedChanged(object sender, EventArgs e)
        {
            // Bind the dates to the force manual checkbox.
            //chkForceManualNotificaitons
            setScheduleInputs();
        }

        private void setScheduleInputs()
        {
            dtSweepDate.Enabled = !chkForceManualNotificaitons.Checked;
            dtSendInStart.Enabled = !chkForceManualNotificaitons.Checked;
            dtSendInStop.Enabled = !chkForceManualNotificaitons.Checked;
        }

        private void label25_Click(object sender, EventArgs e)
        {
        }

        private void btnLoadDefaults_Click(object sender, EventArgs e)
        {
            try
            {
                backendLogic.SetDefaultClientNotificationSettings(this._clientId, this._clientDepartmentId);
                PopulateNotificationSettings();
            }
            catch (Exception ex)
            {
                Program.LogError(ex);
                //_logger.Error(ex.ToString());
                //if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                //if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
        }



        private void btnEmailPreview_Click(object sender, EventArgs e)
        {
            if (!backendLogic.IsValidEmail(txtPreviewEmail.Text))
            {
                MessageBox.Show("Not a valid email!", "Preview email invalid");
                txtPreviewEmail.Focus();
                return;
            }
            if (!backendLogic.IsPhoneNumber(txtPreviewPhone.Text))
            {
                MessageBox.Show("Not a valid phone!", "Preview phone invalid");
                txtPreviewPhone.Focus();
                return;
            }
            if (!backendLogic.IsFiveDigitZip(txtPreviewZipCode.Text))
            {
                MessageBox.Show("Not a valid zip code!", "Preview zip code invalid");
                txtPreviewZipCode.Focus();
                return;
            }
            if (!_PDFengine.ValidateJson(txtJSONTemplateSettings.Text))
            {
                MessageBox.Show("The setting JSON file are invalid as entered.\r\nPlease double check!", "Invalid JSON!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtJSONTemplateSettings.Focus();
            }


            Notification notification = new Notification()
            {
                donor_test_info_id = 0,
                client_id = this.ClientId,
                client_department_id = this.ClientDepartmentId,
                clinic_radius = 50
            };

            NotificationInformation notificationInformation = new NotificationInformation()
            {
                donor_last_name = "Test",
                donor_id = 0,
                donor_full_name = "Test User",
                donor_email = txtPreviewEmail.Text,
                donor_phone_1 = txtPreviewPhone.Text,
                donor_zip = txtPreviewZipCode.Text,
                donor_test_info_id = 0,
                test_panel_id = 0,
                test_panel_name = "PANEL NAME",
                lab_code = "LABCODE",
                test_category_name = "CATEGORY NAME",
                department_name = "DEPARTMENT NAME"



            };

            ClientNotificationDataSettings clientNotificationDataSettings = GetCurrentNotificationSettings();
            backendLogic.PreviewNotification(notification, notificationInformation, clientNotificationDataSettings, txtJSONTemplateSettings.Text);

            // Pick a donor at random (last registered) and generate a sample pdf and email it
        }

        private void tabPage1_Click_1(object sender, EventArgs e)
        {
            tabNotificationSettingsClientDepartmentDetails.Focus();
        }

        private bool VerifyUseFormFox()
        {
            //    lblUseFormFox.Text = "FF Enabled";
            //    chkUseFormFox.Checked = true;
            //}
            //    else
            //    {
            Client client = clientBL.Get(this._clientId);
            bool retval = false;
            if (string.IsNullOrEmpty(txtFirstName.Text) ||
                string.IsNullOrEmpty(txtLastName.Text) ||
                string.IsNullOrEmpty(txtPhone.Text) ||
                string.IsNullOrEmpty(txtPhysicalAddress1.Text) ||
                string.IsNullOrEmpty(txtPhysicalCity.Text) ||
                cmbPhysicalState.Text == "(Select)" ||
                string.IsNullOrEmpty(txtPhysicalZipCode.Text)
                )
            {
                _logger.Debug($"Formfox enable failed:");
                _logger.Debug($"string.IsNullOrEmpty(txtFirstName.Text) {string.IsNullOrEmpty(txtFirstName.Text)}");
                _logger.Debug($"string.IsNullOrEmpty(txtLastName.Text) {string.IsNullOrEmpty(txtLastName.Text)}");
                _logger.Debug($"string.IsNullOrEmpty(txtPhone.Text) {string.IsNullOrEmpty(txtPhone.Text)}");
                _logger.Debug($"string.IsNullOrEmpty(txtPhysicalAddress1.Text) {string.IsNullOrEmpty(txtPhysicalAddress1.Text)}");
                _logger.Debug($"string.IsNullOrEmpty(txtPhysicalCity.Text) {string.IsNullOrEmpty(txtPhysicalCity.Text)}");
                _logger.Debug($"cmbPhysicalState.Text == (Select) {cmbPhysicalState.Text == "(Select)"}");
                _logger.Debug($"string.IsNullOrEmpty(txtPhysicalZipCode.Text) {string.IsNullOrEmpty(txtPhysicalZipCode.Text)}");

            }
            else
            {
                retval = true;
            }
            return retval;
        }

        private void SetFormFoxLabel(bool _set)
        {
            if (_set == true)
            {
                lblUseFormFox.Text = "FF Enabled";

                if (string.IsNullOrEmpty(txtFormFoxCode.Text) == true)
                {
                    lblUseFormFox.ForeColor = System.Drawing.Color.DarkRed;
                }
                else
                {
                    lblUseFormFox.ForeColor = lblFormFoxCode.ForeColor;

                }
            }
            else
            {
                lblUseFormFox.Text = "FF Disabled";
                lblUseFormFox.ForeColor = lblFormFoxCode.ForeColor;

            }
        }


        private void chkUseFormFox_CheckedChanged(object sender, EventArgs e)
        {
            // Contact, phone number and address are all requred to enable.

            if (VerifyUseFormFox() == false)
            {
                chkUseFormFox.CheckedChanged -= chkUseFormFox_CheckedChanged;
                chkUseFormFox.Checked = false;
                chkUseFormFox.CheckedChanged += chkUseFormFox_CheckedChanged;
                lblUseFormFox.Text = "FF Disabled";
                MessageBox.Show("Formfox Marketplace requires Address and contact information\r\nMake sure the client has a physical address, too!\r\nPlease complete these before enabling!", "Missing information required");

                return;

            }
            SetFormFoxLabel(chkUseFormFox.Checked);



        }
        private void txtFormFoxCode_TextChanged(object sender, EventArgs e)
        {

            if (chkUseFormFox.Checked == true)
            {
                if (string.IsNullOrEmpty(txtFormFoxCode.Text) == true)
                {
                    lblUseFormFox.ForeColor = System.Drawing.Color.DarkRed;
                }
                else
                {
                    lblUseFormFox.ForeColor = lblFormFoxCode.ForeColor;

                }
            }
            else
            {
                lblUseFormFox.ForeColor = lblFormFoxCode.ForeColor;

            }
        }

        private bool FormShown = false;
        private void FrmClientDepartmentDetails_Shown(object sender, EventArgs e)
        {
            this.FormShown = true;
        }

        private void tabIntegrationsClientDepartmentDetails_Click(object sender, EventArgs e)
        {

        }

        private void rdoRequireRemoteLogin_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void lblIntegrations_Click(object sender, EventArgs e)
        {

        }
    }
}