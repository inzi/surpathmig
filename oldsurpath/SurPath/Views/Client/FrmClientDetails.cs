using SurPath.Business;
using SurPath.Business.Master;
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
using System.Text.RegularExpressions;
using SurPath.Entity.Master;
using Serilog;
using SurpathBackend.Classes;

namespace SurPath
{
    public partial class FrmClientDetails : Form
    {
        #region Private Variables
        static ILogger _logger = Program._logger;

        private OperationMode _mode = OperationMode.None;
        private int _clientId = 0;

        private ClientBL clientBL = new ClientBL();
        private List<ClientDepartment> clientDepartments = new List<ClientDepartment>();

        private ClientDepartment currentClientDepartment = null;

        private ClientDeptTestCategory currentUATestCategory = null;
        private ClientDeptTestCategory currentHairTestCategory = null;
        private ClientDeptTestCategory currentDNATestCategory = null;
        private ClientDeptTestCategory currentBCTestCategory = null;
        private ClientDeptTestCategory currentRecordKeepingTestCategory = null;

        private bool _isPhysicalRequired = true;
        // private bool _isMailingRequired = false;
        //private bool _isMainContactInformation = true;

        private bool testPanelResetFlag = false;
        private bool clientaddressFlag = false;

        private bool tvSelectionFlag = true;

        private bool viewFlag = false;

        #endregion Private Variables

        #region Constructor

        public FrmClientDetails()
        {
            InitializeComponent();
        }

        public FrmClientDetails(OperationMode mode, int clientId)
        {
            InitializeComponent();
            this._mode = mode;
            this._clientId = clientId;
        }

        #endregion Constructor

        #region Event Methods

        private void btnClientClose_Click(object sender, EventArgs e)
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

        private void btnTestPanel_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                FrmTestPanelDetails frmTestPanelDetails = new FrmTestPanelDetails();
                frmTestPanelDetails.ShowDialog();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmClientDetails_Load(object sender, EventArgs e)
        {
            FrmClientDetails frmcl = new FrmClientDetails();
            frmcl.Enabled = false;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                InitializeControls();

                if (this._mode == OperationMode.Edit && this._clientId != 0)
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

        private void FrmClientDetails_FormClosing(object sender, FormClosingEventArgs e)
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
                MessageBox.Show("Error occurred while loading the page.");
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

        private void txtClientName_TextChanged(object sender, EventArgs e)
        {
            txtClientName.CausesValidation = false;
        }

        private void chkActive_CheckedChanged(object sender, EventArgs e)
        {
            chkActive.CausesValidation = false;
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

        private void txtPhysicalZipCode_TextChanged(object sender, EventArgs e)
        {
            txtPhysicalZipCode.CausesValidation = false;
        }

        private void chkSameAsPhysical_CheckedChanged(object sender, EventArgs e)
        {
            chkSameAsPhysical.CausesValidation = false;

            if (chkSameAsPhysical.Checked)
            {
                txtMailingAddress1.Enabled = false;
                txtMailingAddress2.Enabled = false;
                txtMailingCity.Enabled = false;
                cmbMailingState.Enabled = false;
                txtMailingZipCode.Enabled = false;

                #region Physical Address

                if (chkSameAsPhysical.Checked == true)
                {
                    txtMailingAddress1.Text = txtPhysicalAddress1.Text;
                    txtMailingAddress2.Text = txtPhysicalAddress2.Text;
                    txtMailingCity.Text = txtPhysicalCity.Text;
                    cmbMailingState.Text = cmbPhysicalState.Text;
                    txtMailingZipCode.Text = txtPhysicalZipCode.Text;
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

        private void txtMailingZipCode_TextChanged(object sender, EventArgs e)
        {
            txtMailingZipCode.CausesValidation = false;
        }

        private void txtMailingAddress2_TextChanged(object sender, EventArgs e)
        {
            txtMailingAddress2.CausesValidation = false;
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

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            txtEmail.CausesValidation = false;
        }

        private void txtClientCode_TextChanged(object sender, EventArgs e)
        {
            txtClientCode.CausesValidation = false;
        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
            txtPhone.CausesValidation = false;
        }

        private void txtFax_TextChanged(object sender, EventArgs e)
        {
            txtFax.CausesValidation = false;
        }

        private void btnDepartmentNotFound_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (this._clientId == 0 && this._mode == OperationMode.New)
                {
                    Cursor.Current = Cursors.Default;
                    //if (MessageBox.Show("This operation requires to save the Client Details. Do you want to continue?", "SurPath Drug Testing", MessageBoxButtons.YesNoCancel) == System.Windows.Forms.DialogResult.Yes)
                    //{
                    if (!SaveData())
                    {
                        return;
                    }
                    //else
                    //{
                    //    return;
                    //}
                    // }
                }
                else if (this._clientId != 0 && this._mode == OperationMode.Edit)
                {
                    // ResetControlsCauseValidation();
                    Cursor.Current = Cursors.Default;
                    //DialogResult userConfirmation = MessageBox.Show("You may lose some test panel data. Do you want to save changes?", "SurPath Drug Testing", MessageBoxButtons.YesNoCancel);
                    //if (userConfirmation == DialogResult.Cancel)
                    //{
                    //    return;
                    //}
                    //else if (userConfirmation == DialogResult.Yes)
                    //{
                    if (!SaveData())
                    {
                        return;
                    }
                    //}
                }

                FrmClientDepartmentDetails frmClientDepartmentDetails = new FrmClientDepartmentDetails(Enum.OperationMode.New, this._clientId, 0);
                if (frmClientDepartmentDetails.ShowDialog() == DialogResult.OK)
                {
                    LoadClientDepartments();
                    LoadTestPanelDisplayDetails();
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDepartmentDetails_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (this._clientId == 0 && this._mode == OperationMode.New)
                {
                    Cursor.Current = Cursors.Default;
                    //if (MessageBox.Show("This operation requires to save the Client Details. Do you want to continue?", "SurPath Drug Testing", MessageBoxButtons.YesNoCancel) == System.Windows.Forms.DialogResult.Yes)
                    //{
                    if (!SaveData())
                    {
                        return;
                    }
                    //}
                    //else
                    //{
                    //    return;
                    //}
                }
                else if (this._clientId != 0 && this._mode == OperationMode.Edit)
                {
                    Cursor.Current = Cursors.Default;
                    //DialogResult userConfirmation = MessageBox.Show("You may lose some test panel data. Do you want to save changes?", "SurPath Drug Testing", MessageBoxButtons.YesNoCancel);

                    //if (userConfirmation == DialogResult.Cancel)
                    //{
                    //    return;
                    //}
                    //else if (userConfirmation == DialogResult.Yes)
                    //{
                    if (!SaveData())
                    {
                        return;
                    }
                    //}
                }

                FrmClientDepartmentInfo frmClientDepartmentInfo = new FrmClientDepartmentInfo(this._clientId);
                if (frmClientDepartmentInfo.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    LoadClientDepartments();
                    LoadTestPanelDisplayDetails();
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void tvDepartmentInfo_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // When we change depts - the values in the text fields are probably lost.
            // Values need to copied to clone objects or captured when they change depts - 
            // maybe pop up a msg "do you want to keep changes?
            // and if yes - then pump those into the depts.
        
            if (tvSelectionFlag)
            {
                try
                {
                    tvDepartmentInfo.CausesValidation = false;

                    TreeNode node = e.Node;

                    DepartmentAddress(node);
                    DoTVSelectionProcess(node);
                    LoadTestPanelDisplayDetails();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void tvDepartmentInfo_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (tvSelectionFlag)
            {
                try
                {
                    TreeNode node = e.Node;

                    DepartmentAddress(node);
                    DoTVSelectionProcess(node);
                    LoadTestPanelDisplayDetails();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void cmbTestPanel_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  chkClient.Checked = true;
            try
            {
                if (!testPanelResetFlag)
                {
                    ComboBox cmbTestPanel = (ComboBox)sender;
                    bool flag = false;

                    if (cmbTestPanel.Tag.ToString().StartsWith("UA") && currentUATestCategory != null)
                    {
                        if (cmbTestPanel.Tag.ToString() == "UA_Main")
                        {
                            if (currentUATestCategory.ClientDeptTestPanels.Count > 0)
                            {
                                foreach (ClientDeptTestPanel testPanel in currentUATestCategory.ClientDeptTestPanels)
                                {
                                    if (testPanel.IsMainTestPanel)
                                    {
                                        testPanel.TestPanelId = (int)cmbTestPanel.SelectedValue;
                                        flag = true;
                                        break;
                                    }
                                }
                            }

                            if (!flag)
                            {
                                ClientDeptTestPanel newTestPanel = new ClientDeptTestPanel();

                                newTestPanel.ClientDeptTestPanelId = 1;
                                newTestPanel.ClientDeptTestCategoryId = currentUATestCategory.ClientDeptTestCategoryId;
                                newTestPanel.IsMainTestPanel = true;
                                newTestPanel.TestPanelId = (int)cmbTestPanel.SelectedValue;

                                if (txtUAMainTestPanelPrice.Text.Trim() != string.Empty)
                                {
                                    newTestPanel.TestPanelPrice = Convert.ToDouble(txtUAMainTestPanelPrice.Text.Trim());
                                }

                                newTestPanel.DisplayOrder = 1;
                                newTestPanel.CreatedBy = Program.currentUserName;
                                newTestPanel.LastModifiedBy = Program.currentUserName;

                                currentUATestCategory.ClientDeptTestPanels.Add(newTestPanel);
                            }

                            if (cmbTestPanel.SelectedValue.ToString() == "0")
                            {
                                txtUAMainTestPanelPrice.Text = string.Empty;
                                txtUAMainTestPanelPrice.Enabled = false;
                            }
                            else
                            {
                                txtUAMainTestPanelPrice.Enabled = true;
                            }
                        }
                    }
                    else if (cmbTestPanel.Tag.ToString().StartsWith("Hair") && currentHairTestCategory != null)
                    {
                        if (cmbTestPanel.Tag.ToString() == "Hair_Main")
                        {
                            if (currentHairTestCategory.ClientDeptTestPanels.Count > 0)
                            {
                                foreach (ClientDeptTestPanel testPanel in currentHairTestCategory.ClientDeptTestPanels)
                                {
                                    if (testPanel.IsMainTestPanel)
                                    {
                                        testPanel.TestPanelId = (int)cmbTestPanel.SelectedValue;
                                        flag = true;
                                        break;
                                    }
                                }
                            }

                            if (!flag)
                            {
                                ClientDeptTestPanel newTestPanel = new ClientDeptTestPanel();

                                newTestPanel.ClientDeptTestPanelId = 0;
                                newTestPanel.ClientDeptTestCategoryId = currentHairTestCategory.ClientDeptTestCategoryId;
                                newTestPanel.IsMainTestPanel = true;
                                newTestPanel.TestPanelId = (int)cmbTestPanel.SelectedValue;

                                if (txtHairMainTestPanelPrice.Text.Trim() != string.Empty)
                                {
                                    newTestPanel.TestPanelPrice = Convert.ToDouble(txtHairMainTestPanelPrice.Text.Trim());
                                }

                                newTestPanel.DisplayOrder = 1;
                                newTestPanel.CreatedBy = Program.currentUserName;
                                newTestPanel.LastModifiedBy = Program.currentUserName;

                                currentHairTestCategory.ClientDeptTestPanels.Add(newTestPanel);
                            }

                            if (cmbTestPanel.SelectedValue.ToString() == "0")
                            {
                                txtHairMainTestPanelPrice.Text = string.Empty;
                                txtHairMainTestPanelPrice.Enabled = false;
                            }
                            else
                            {
                                txtHairMainTestPanelPrice.Enabled = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtTestPanelPrice_Leave(object sender, EventArgs e)
        {
            TextBox txtTestPanelPrice = (TextBox)sender;
            double price = 0.0;

            if (txtTestPanelPrice.Text.Trim() != string.Empty)
            {
                try
                {
                    price = Convert.ToDouble(txtTestPanelPrice.Text.Trim());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Invalid format of price.");
                    txtTestPanelPrice.Focus();
                    return;
                }
            }

            if (txtTestPanelPrice.Tag.ToString().StartsWith("UA"))
            {
                if (txtTestPanelPrice.Tag.ToString() == "UA_Main")
                {
                    foreach (ClientDeptTestPanel testPanel in currentUATestCategory.ClientDeptTestPanels)
                    {
                        if (testPanel.IsMainTestPanel)
                        {
                            testPanel.TestPanelPrice = price;
                        }
                    }
                }
            }
            else if (txtTestPanelPrice.Tag.ToString().StartsWith("Hair"))
            {
                if (txtTestPanelPrice.Tag.ToString() == "Hair_Main")
                {
                    foreach (ClientDeptTestPanel testPanel in currentHairTestCategory.ClientDeptTestPanels)
                    {
                        if (testPanel.IsMainTestPanel)
                        {
                            testPanel.TestPanelPrice = price;
                        }
                    }
                }
            }
            else if (txtTestPanelPrice.Tag.ToString().StartsWith("Hair"))
            {
                if (txtTestPanelPrice.Tag.ToString() == "Hair_Main")
                {
                    foreach (ClientDeptTestPanel testPanel in currentHairTestCategory.ClientDeptTestPanels)
                    {
                        if (testPanel.IsMainTestPanel)
                        {
                            testPanel.TestPanelPrice = price;
                        }
                    }
                }
            }
            else if (txtTestPanelPrice.Tag.ToString().StartsWith("DNA"))
            {
                if (txtTestPanelPrice.Tag.ToString() == "DNA_Main")
                {
                    foreach (ClientDeptTestPanel testPanel in currentDNATestCategory.ClientDeptTestPanels)
                    {
                        if (testPanel.IsMainTestPanel)
                        {
                            testPanel.TestPanelPrice = price;
                        }
                    }
                }
            }
            else if (txtTestPanelPrice.Tag.ToString().StartsWith("BC"))
            {
                if (txtTestPanelPrice.Tag.ToString() == "BC_Main")
                {
                    foreach (ClientDeptTestPanel testPanel in currentBCTestCategory.ClientDeptTestPanels)
                    {
                        if (testPanel.IsMainTestPanel)
                        {
                            testPanel.TestPanelPrice = price;
                        }
                    }
                }
            }
            else if (txtTestPanelPrice.Tag.ToString().StartsWith("RC"))
            {
                if (txtTestPanelPrice.Tag.ToString() == "RC_Main")
                {
                    foreach (ClientDeptTestPanel testPanel in currentRecordKeepingTestCategory.ClientDeptTestPanels)
                    {
                        if (testPanel.IsMainTestPanel)
                        {
                            testPanel.TestPanelPrice = price;
                        }
                    }
                }
            }
        }

        private void txtDNATestPrice_Leave(object sender, EventArgs e)
        {
        }

        private void txtTestPanelPrice_Validating(object sender, CancelEventArgs e)
        {
            TextBox txtTestPanelPrice = (TextBox)sender;

            try
            {
                if (txtTestPanelPrice.Text.Trim() != string.Empty)
                {
                    double price = Convert.ToDouble(txtTestPanelPrice.Text);
                }
            }
            catch
            {
                MessageBox.Show("Invalid format of price.");
                e.Cancel = true;
            }
        }

        private void txtPhone_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void txtFax_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void txtPhysicalZipCode_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void txtMailingZipCode_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void cmbPhysicalState_TextChanged(object sender, EventArgs e)
        {
            cmbPhysicalState.CausesValidation = false;
        }

        private void cmbMailingState_TextChanged(object sender, EventArgs e)
        {
            cmbMailingState.CausesValidation = false;
        }

        private void btnTestPanelNotFound_Click(object sender, EventArgs e)
        {
            try
            {
                FrmTestPanelDetails frmTestPanelDetails = new FrmTestPanelDetails(OperationMode.New, 0);
                if (frmTestPanelDetails.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    LoadTestPanels();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbLab_TextChanged(object sender, EventArgs e)
        {
            cmbLab.CausesValidation = false;
        }

        private void cmbMRO_TextChanged(object sender, EventArgs e)
        {
            cmbMRO.CausesValidation = false;
        }

        private void cmbSalesRepresentative_TextChanged(object sender, EventArgs e)
        {
            cmbSalesRepresentative.CausesValidation = false;
        }

        private void cmbUAMainTestPanel_TextChanged(object sender, EventArgs e)
        {
            cmbUAMainTestPanel.CausesValidation = false;
        }

        private void txtUAMainTestPanelPrice_TextChanged(object sender, EventArgs e)
        {
            txtUAMainTestPanelPrice.CausesValidation = false;
        }

        private void cmbHairMainTestPanel_TextChanged(object sender, EventArgs e)
        {
            cmbHairMainTestPanel.CausesValidation = false;
        }

        private void txtHairMainTestPanelPrice_TextChanged(object sender, EventArgs e)
        {
            txtHairMainTestPanelPrice.CausesValidation = false;
        }

        private void txtDNATestPrice_TextChanged(object sender, EventArgs e)
        {
            txtDNATestPrice.CausesValidation = false;
        }

        private void TxtRecordKeepingTestPrice_TextChanged(object sender, EventArgs e)
        {
            txtRecordKeepingTestPrice.CausesValidation = false;
        }

        private void btnDepartmentContact_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentClientDepartment != null)
                {
                    FrmClientDepartmentDetails frmClientDepartmentDetails = new FrmClientDepartmentDetails(Enum.OperationMode.View, currentClientDepartment.ClientId, currentClientDepartment.ClientDepartmentId);
                    if (frmClientDepartmentDetails.ShowDialog() == DialogResult.OK)
                    {
                        //
                    }
                }
                else
                {
                    MessageBox.Show("Please select one Department.");
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

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

        private void chkClient_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDepartmentAddress.Checked == false)
            {
                LoadData();
            }
            //if(chkClient.Checked = true)
            //{
            //    DepartmentAddress();
            //}
        }

        private void cmbSalesRepresentative_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbSalesRepresentative.CausesValidation = false;
        }

        #endregion Event Methods

        #region Public Methods

        public void LoadData()
        {
            try
            {
                if (this._mode == OperationMode.Edit)
                {
                    Client client = clientBL.Get(this._clientId);

                    if (client != null)
                    {
                        txtClientCode.Text = client.ClientCode;
                        txtClientName.Text = client.ClientName;
                        chkActive.Checked = client.IsClientActive;
                        if (client.LaboratoryVendorId != null)
                        {
                            cmbLab.SelectedValue = client.LaboratoryVendorId;
                        }
                        else
                        {
                            cmbLab.SelectedValue = 0;
                        }

                        if (client.MROVendorId != null)
                        {
                            cmbMRO.SelectedValue = client.MROVendorId;
                        }
                        else
                        {
                            cmbMRO.SelectedValue = 0;
                        }

                        chkSameAsPhysical.Checked = client.IsMailingAddressPhysical;

                        if (client.SalesRepresentativeId != null)
                        {
                            cmbSalesRepresentative.SelectedValue = client.SalesRepresentativeId;
                        }
                        else
                        {
                            cmbSalesRepresentative.SelectedValue = 0;
                        }

                        if (client.CanEditTestCategory == true)
                        {
                            chkedittestcategory.Checked = true;
                        }
                        else
                        {
                            chkedittestcategory.Checked = false;
                        }

                        #region TimeZone
                        // client_timezoneinfo
                        cmbTimeZone.SelectedValue = TimeZoneInfo.FindSystemTimeZoneById(client.client_timezoneinfo).Id;
                        #endregion TimeZone


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
                            txtMailingAddress1.Enabled = true;
                            txtMailingAddress2.Enabled = true;
                            txtMailingCity.Enabled = true;
                            cmbMailingState.Enabled = true;
                            txtMailingZipCode.Enabled = true;

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
                            }
                        }
                        else if (chkSameAsPhysical.Checked == true)
                        {
                            txtMailingAddress1.Text = physicaAddress.Address1;
                            txtMailingAddress2.Text = physicaAddress.Address2;
                            txtMailingCity.Text = physicaAddress.City;
                            cmbMailingState.Text = physicaAddress.State;
                            txtMailingZipCode.Text = physicaAddress.ZipCode;
                        }

                        #endregion Mailing Address

                        if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()) && viewFlag == true)
                        {
                            cmbMailingState.Enabled = false;
                        }

                        clientDepartments = client.ClientDepartments;

                        //  LoadClientDepartmentTreeView();

                        LoadClientDepartments();

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
                chkActive.Checked = true;
                chkSameAsPhysical.Checked = false;

                cmbPhysicalState.SelectedIndex = 0;
                cmbMailingState.SelectedIndex = 0;

                //txtMailingAddress1.Enabled = false;
                //txtMailingAddress2.Enabled = false;
                //txtMailingCity.Enabled = false;
                //cmbMailingState.Enabled = false;
                //txtMailingZipCode.Enabled = false;

                tvDepartmentInfo.Nodes.Clear();

                LoadLaboratoryVendors();
                LoadTimeZones();
                LoadMROVendors();
                LoadSalesRepresentatives();
                LoadTestPanels();

                //txtFirstName.Enabled = false;
                //txtLastName.Enabled = false;
                //txtPhone.Enabled = false;
                //txtFax.Enabled = false;
                //txtEmail.Enabled = false;

                //txtPhysicalAddress1.Enabled = false;
                //txtPhysicalAddress2.Enabled = false;
                //txtPhysicalCity.Enabled = false;
                //cmbPhysicalState.Enabled = false;
                //txtPhysicalZipCode.Enabled = false;

                //chkSameAsPhysical.Enabled = false;
                //txtMailingAddress1.Enabled = false;
                //txtMailingAddress2.Enabled = false;
                //cmbMailingState.Enabled = false;
                //txtMailingCity.Enabled = false;
                //txtMailingZipCode.Enabled = false;

                lblDepartmentNameValue.Text = string.Empty;
                gboxUA.Enabled = false;
                gboxHair.Enabled = false;
                gboxBC.Enabled = false;
                gboxDNA.Enabled = false;
                gboxRecordKeeping.Enabled = false;

                //gboxUA.Visible = false;
                //gboxHair.Visible = false;
                //gboxBC.Visible = false;
                //gboxDNA.Visible = false;
                //gboxRecordKeeping.Visable = false;

                cmbUAMainTestPanel.SelectedIndex = 0;
                cmbHairMainTestPanel.SelectedIndex = 0;

                if (this._mode == OperationMode.Edit)
                {
                    txtClientCode.ReadOnly = true;
                }

                if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                {
                    //CLIENT_TEST_PANEL_ADD
                    DataRow[] clientTestPanelAdd = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.CLIENT_TEST_PANEL_ADD.ToDescriptionString() + "'");

                    if (clientTestPanelAdd.Length > 0)
                    {
                        btnTestPanelNotFound.Visible = true;
                    }
                    else
                    {
                        btnTestPanelNotFound.Visible = false;
                    }

                    //CLIENT_DEPARTMENT_ADD
                    DataRow[] clientDepartmentAdd = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.CLIENT_DEPARTMENT_ADD.ToDescriptionString() + "'");

                    if (clientDepartmentAdd.Length > 0)
                    {
                        btnDepartmentNotFound.Visible = true;
                        btnDepartmentDetails.Visible = false;
                        btnDepartmentContact.Visible = false;
                    }

                    //CLIENT_DEPARTMENT_EDIT
                    DataRow[] clientDepartmentEdit = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.CLIENT_DEPARTMENT_EDIT.ToDescriptionString() + "'");

                    if (clientDepartmentEdit.Length > 0)
                    {
                        btnDepartmentNotFound.Visible = false;
                        btnDepartmentDetails.Visible = true;
                        btnDepartmentContact.Visible = true;
                    }
                    if (clientDepartmentAdd.Length > 0)
                    {
                        btnDepartmentNotFound.Visible = true;
                    }

                    //CLIENT_DEPARTMENT_ARCHIVE
                    DataRow[] clientDepartmentArchive = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.CLIENT_DEPARTMENT_ARCHIVE.ToDescriptionString() + "'");

                    if (clientDepartmentArchive.Length > 0)
                    {
                        btnDepartmentNotFound.Visible = false;
                        btnDepartmentDetails.Visible = true;
                        btnDepartmentContact.Visible = true;
                    }
                    if (clientDepartmentAdd.Length > 0)
                    {
                        btnDepartmentNotFound.Visible = true;
                    }

                    if (clientDepartmentAdd.Length > 0 && clientDepartmentEdit.Length > 0 && clientDepartmentArchive.Length > 0)
                    {
                        btnDepartmentNotFound.Visible = true;
                        btnDepartmentDetails.Visible = true;
                        btnDepartmentContact.Visible = true;
                    }

                    //CLIENT_VIEW
                    DataRow[] clientView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.CLIENT_VIEW.ToDescriptionString() + "'");

                    if (clientView.Length > 0)
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
                            if (ctrl is RadioButton)
                            {
                                ((RadioButton)ctrl).Enabled = false;
                            }
                            if (ctrl is TreeView)
                            {
                                ((TreeView)ctrl).Enabled = false;
                            }
                        }
                        viewFlag = true;
                        //tvDepartmentInfo.Enabled = true;
                        //lblDepartmentNameHeader.Visible = false;
                        //lblDepartmentNameValue.Visible = false;
                        //gboxUA.Visible = false;
                        //gboxHair.Visible = false;
                        //gboxDNA.Visible = false;
                        if (clientDepartmentAdd.Length > 0)
                        {
                            btnDepartmentNotFound.Enabled = true;
                            btnDepartmentDetails.Visible = false;
                            btnDepartmentContact.Visible = false;
                        }

                        if (clientDepartmentEdit.Length > 0)
                        {
                            btnDepartmentDetails.Visible = true;
                            btnDepartmentDetails.Enabled = true;
                            btnDepartmentContact.Visible = false;

                            if (clientDepartmentAdd.Length > 0)
                            {
                                btnDepartmentNotFound.Visible = true;
                                btnDepartmentNotFound.Enabled = true;
                            }
                        }

                        if (clientDepartmentArchive.Length > 0)
                        {
                            btnDepartmentNotFound.Visible = false;
                            btnDepartmentDetails.Enabled = true;
                            btnDepartmentContact.Visible = false;

                            if (clientDepartmentAdd.Length > 0)
                            {
                                btnDepartmentNotFound.Visible = true;
                                btnDepartmentNotFound.Enabled = true;
                            }
                        }
                    }
                }

                if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                {
                    //CLIENT_DEPARTMENT_VIEW
                    DataRow[] clientDepartmentView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.CLIENT_DEPARTMENT_VIEW.ToDescriptionString() + "'");

                    if (clientDepartmentView.Length > 0)
                    {
                        btnDepartmentDetails.Enabled = true;
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

                Client client = null;

                if (this._mode == OperationMode.New)
                {
                    client = new Client();
                    client.ClientId = 0;
                    client.CreatedBy = Program.currentUserName;
                }
                else if (this._mode == OperationMode.Edit)
                {
                    client = clientBL.Get(this._clientId);
                }

                //Default values - fileds are not used
                client.ClientDivision = null;
                client.ClientTypeId = ClientTypes.None;
                client.MROTypeId = ClientMROTypes.None;
                //

                client.ClientCode = txtClientCode.Text.Trim();
                client.ClientName = txtClientName.Text.Trim();
                client.IsClientActive = chkActive.Checked;

                if (cmbLab.SelectedValue.ToString() != "0")
                {
                    client.LaboratoryVendorId = (int)cmbLab.SelectedValue;
                }
                else
                {
                    client.LaboratoryVendorId = null;
                }

                if (cmbMRO.SelectedValue.ToString() != "0")
                {
                    client.MROVendorId = (int)cmbMRO.SelectedValue;
                }
                else
                {
                    client.MROVendorId = null;
                }

                client.IsMailingAddressPhysical = chkSameAsPhysical.Checked;

                if (cmbSalesRepresentative.SelectedValue.ToString() != "0" && cmbSalesRepresentative.SelectedValue.ToString() != null)
                {
                    client.SalesRepresentativeId = (int)cmbSalesRepresentative.SelectedValue;
                }
                else
                {
                    client.SalesRepresentativeId = null;
                }

                client.client_timezoneinfo = cmbTimeZone.SelectedValue.ToString(); //((TimeZoneInfo)cmbTimeZone.SelectedValue).Id; /// TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time")

                #region Client Contact

                client.ClientContact.ClientContactFirstName = txtFirstName.Text.Trim();
                client.ClientContact.ClientContactLastName = txtLastName.Text.Trim();

                if (txtPhone.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") != string.Empty)
                {
                    client.ClientContact.ClientContactPhone = txtPhone.Text.Trim();
                }
                else
                {
                    client.ClientContact.ClientContactPhone = string.Empty;
                }

                if (txtFax.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") != string.Empty)
                {
                    client.ClientContact.ClientContactFax = txtFax.Text.Trim();
                }
                else
                {
                    client.ClientContact.ClientContactFax = string.Empty;
                }

                client.ClientContact.ClientContactEmail = txtEmail.Text.Trim();

                #endregion Client Contact

                #region Physical Address

                if (_isPhysicalRequired)
                {
                    ClientAddress physicaAddress = null;
                    if (this._mode == OperationMode.New)
                    {
                        physicaAddress = new ClientAddress();
                        physicaAddress.AddressTypeId = AddressTypes.PhysicalAddress1;
                        client.ClientAddresses.Add(physicaAddress);
                    }
                    else
                    {
                        foreach (ClientAddress address in client.ClientAddresses)
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
                            client.ClientAddresses.Add(physicaAddress);
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

                #endregion Physical Address

                #region Mailing Address

                if (chkSameAsPhysical.Checked == false)
                {
                    ClientAddress mailingAddress = null;
                    if (this._mode == OperationMode.New)
                    {
                        mailingAddress = new ClientAddress();
                        mailingAddress.AddressTypeId = AddressTypes.MailingAddress;
                        client.ClientAddresses.Add(mailingAddress);
                    }
                    else
                    {
                        foreach (ClientAddress address in client.ClientAddresses)
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
                            client.ClientAddresses.Add(mailingAddress);
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
                if (chkedittestcategory.Checked == true)
                {
                    client.CanEditTestCategory = true;
                }
                else
                {
                    client.CanEditTestCategory = false;
                }

                #endregion Mailing Address

                //

                client.ClientDepartments = clientDepartments;

                client.LastModifiedBy = Program.currentUserName;

                int returnVal = clientBL.Save(client);

                if (returnVal > 0)
                {
                    if (this._mode == OperationMode.New)
                    {
                        client.ClientId = returnVal;
                        this._clientId = returnVal;
                        this._mode = OperationMode.Edit;
                    }

                    LoadClientDepartments();

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
                if (txtClientCode.Text.Trim() == string.Empty)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Client Code cannot be empty.");
                    txtClientCode.Focus();
                    return false;
                }
                else
                {
                    if (this._mode == OperationMode.New)
                    {
                        Client client = clientBL.Get(txtClientCode.Text.Trim());
                        if (client != null)
                        {
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("Client Code already exists.");
                            txtClientCode.Focus();
                            return false;
                        }
                    }
                }

                if (txtClientName.Text.Trim() == string.Empty)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Client Name cannot be empty.");
                    txtClientName.Focus();
                    return false;
                }

                if (cmbLab.SelectedValue.ToString() == "0" || cmbLab.SelectedValue.ToString() == null)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Laboratory must be selected.");
                    cmbLab.Focus();
                    return false;
                }
                if (cmbLab.SelectedIndex != 0)
                {
                    if (!LaboratoryActive(cmbLab))
                    {
                        return false;
                    }
                }

                if (cmbMRO.SelectedValue.ToString() == "0" || cmbMRO.SelectedValue.ToString() == null)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("MRO must be selected.");
                    cmbMRO.Focus();
                    return false;
                }
                if (cmbMRO.SelectedIndex != 0)
                {
                    if (!MROActive(cmbMRO))
                    {
                        return false;
                    }
                }
                if (cmbSalesRepresentative.SelectedIndex != 0)
                {
                    if (!SalesRepActive(cmbSalesRepresentative))
                    {
                        return false;
                    }
                }

                if (txtEmail.Text != string.Empty)
                {
                    if (!Program.regexEmail.IsMatch(txtEmail.Text.Trim()))
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Invalid format of Email.");
                        txtEmail.Focus();
                        return false;
                    }
                }

                if (txtPhone.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") != string.Empty)
                {
                    if (!Program.regexPhoneNumber.IsMatch(txtPhone.Text.Trim()))
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Invalid format of Phone number.");
                        txtPhone.Focus();
                        return false;
                    }
                }
                if (txtFax.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") != string.Empty)
                {
                    if (!Program.regexPhoneNumber.IsMatch(txtFax.Text.Trim()))
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Invalid format of Fax number.");
                        txtFax.Focus();
                        return false;
                    }
                }

                if (txtPhysicalZipCode.Text != string.Empty)
                {
                    if (!Program.regexZipCode.IsMatch(txtPhysicalZipCode.Text))
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Invalid format of Zip Code (Physical Address ).");
                        txtPhysicalZipCode.Focus();
                        return false;
                    }
                }

                if (txtMailingZipCode.Text != string.Empty)
                {
                    if (!Program.regexZipCode.IsMatch(txtMailingZipCode.Text))
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Invalid format of Zip Code (Mailing Address ).");
                        txtMailingZipCode.Focus();
                        return false;
                    }
                }

                if (chkDepartmentAddress.Checked == true && clientaddressFlag == true)
                {
                    DialogResult userConfirmation = MessageBox.Show("Do you want to save department address as client address?", "SurPath Drug Testing", MessageBoxButtons.YesNoCancel);

                    if (userConfirmation == DialogResult.Cancel)
                    {
                        return false;
                    }
                    else if (userConfirmation == DialogResult.No)
                    {
                        return false;
                    }
                    else
                    {
                        //
                    }
                    chkDepartmentAddress.Focus();
                }

                #region Test Panel Validation

                bool testPanleValidateFlag = true;

                string validationMessage = string.Empty;
                int validationClientDepartmentId = 0;

                if (clientDepartments.Count > 0)
                {
                    foreach (ClientDepartment dept in clientDepartments)
                    {
                        validationClientDepartmentId = dept.ClientDepartmentId;

                        foreach (ClientDeptTestCategory testCategory in dept.ClientDeptTestCategories)
                        {
                            if (testCategory.TestCategoryId == TestCategories.UA || testCategory.TestCategoryId == TestCategories.Hair)
                            {
                                bool mainTestPanleValidateFlag = false;

                                foreach (ClientDeptTestPanel testPanel in testCategory.ClientDeptTestPanels)
                                {
                                    if (testPanel.IsMainTestPanel)
                                    {
                                        mainTestPanleValidateFlag = true;
                                    }

                                    if (testPanel.TestPanelId == 0)
                                    {
                                        testPanleValidateFlag = false;
                                        validationMessage = "Test Panel must be selected for the department " + dept.DepartmentName + " (" + testCategory.TestCategoryId.ToDescriptionString() + ")";
                                        break;
                                    }

                                    if (testPanel.TestPanelId != 0)
                                    {
                                        if (!TestPanelActive(cmbUAMainTestPanel))
                                        {
                                            return false;
                                        }
                                        if (!TestPanelActive(cmbHairMainTestPanel))
                                        {
                                            return false;
                                        }
                                    }

                                    if (testPanel.TestPanelPrice <= 0)
                                    {
                                        testPanleValidateFlag = false;
                                        validationMessage = "Test Panel Price cannot be empty for the department " + dept.DepartmentName + " (" + testCategory.TestCategoryId.ToDescriptionString() + ")";
                                        break;
                                    }
                                }

                                if (!mainTestPanleValidateFlag)
                                {
                                    testPanleValidateFlag = false;
                                    validationMessage = "Test Panel must be selected for the department " + dept.DepartmentName + " (" + testCategory.TestCategoryId.ToDescriptionString() + ")";
                                    break;
                                }
                            }
                            else if (testCategory.TestCategoryId == TestCategories.DNA)
                            {
                                if (testCategory.TestPanelPrice == null || testCategory.TestPanelPrice == 0)
                                {
                                    testPanleValidateFlag = false;
                                    validationMessage = "Test Panel Price cannot be empty for the department " + dept.DepartmentName + " (" + testCategory.TestCategoryId.ToDescriptionString() + ")";
                                    break;
                                }
                            }
                            else if (testCategory.TestCategoryId == TestCategories.BC)
                            {
                                // This issue here - is if you current dept doesn't have BC or RC, the 
                                // input is null, doesn't exist, or whatever.
                                // What needs to happen is all of this needs to be cloned to temp objects
                                // those neeed to be worked with
                                // and it needs to go through the departments and 
                                // examine those values.
                                // THEN - when user hits OK, the depts, their test categories, etc. 
                                // have to be updated.

                                // Better solution might to refactor the 
                                // whole form to 

                                if (testCategory.TestPanelPrice == null || testCategory.TestPanelPrice == 0)
                                {
                                    //testPanleValidateFlag = false;
                                    // validationMessage = "Test Panel Price cannot be empty for the department " + dept.DepartmentName + " (" + testCategory.TestCategoryId.ToDescriptionString() + ")";
                                    //break;
                                }
                            }
                            else if (testCategory.TestCategoryId == TestCategories.RC)
                            {
                                if (testCategory.TestPanelPrice == null || testCategory.TestPanelPrice == 0)
                                {
                                    //testPanleValidateFlag = false;
                                    // validationMessage = "Test Panel Price cannot be empty for the department " + dept.DepartmentName + " (" + testCategory.TestCategoryId.ToDescriptionString() + ")";
                                    //break;
                                }
                            }

                            if (!testPanleValidateFlag)
                            {
                                break;
                            }
                        }

                        if (!testPanleValidateFlag)
                        {
                            break;
                        }
                    }
                }

                if (!testPanleValidateFlag)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show(validationMessage);

                    foreach (TreeNode node in tvDepartmentInfo.Nodes)
                    {
                        node.BackColor = Color.Transparent;
                    }

                    foreach (TreeNode node in tvDepartmentInfo.Nodes)
                    {
                        node.BackColor = Color.Transparent;
                    }

                    TreeNode currentDeptNode = null;

                    foreach (TreeNode node in tvDepartmentInfo.Nodes)
                    {
                        if (node.Tag.ToString().StartsWith("Dept"))
                        {
                            currentDeptNode = node;
                        }
                        else if (node.Tag.ToString().StartsWith("TestCategory"))
                        {
                            currentDeptNode = node.Parent;
                        }
                        else if (node.Tag.ToString().StartsWith("Alt"))
                        {
                            currentDeptNode = node.Parent.Parent;
                        }

                        string clientDeparmentId = (currentDeptNode.Tag.ToString().Split('#'))[1];

                        if (clientDeparmentId == validationClientDepartmentId.ToString())
                        {
                            currentDeptNode.BackColor = Color.Red;
                            tvDepartmentInfo.SelectedNode = currentDeptNode;
                            break;
                        }
                    }
                    return false;
                }

                #endregion Test Panel Validation
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            return true;
        }

        private void LoadLaboratoryVendors()
        {
            try
            {
                VendorBL vendorBL = new VendorBL();
                List<Vendor> vendorList = vendorBL.GetListByVendorType(VendorTypes.Lab);

                Vendor tmpVendor = new Vendor();
                tmpVendor.VendorId = 0;
                tmpVendor.VendorName = "(Select Laboratory)";

                vendorList.Insert(0, tmpVendor);

                cmbLab.Items.Clear();
                cmbLab.DataSource = vendorList;
                cmbLab.ValueMember = "VendorId";
                cmbLab.DisplayMember = "VendorName";
                cmbLab.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadTimeZones()
        {

            List<TimeZoneInfo> _tz = USATimeZoneHelper.USATZList();
            cmbTimeZone.Items.Clear();
            cmbTimeZone.DataSource = _tz;
            cmbTimeZone.ValueMember = "Id";
            cmbTimeZone.SelectedItem = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"); // cmbTimeZone.Items.IndexOf(TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time").Id);

            try
            {

            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool LaboratoryActive(ComboBox cmb)
        {
            VendorBL vendorBL = new VendorBL();

            int vendorId = 0;

            if (cmb.SelectedIndex > 0)
            {
                vendorId = Convert.ToInt32(cmb.SelectedValue);
                Vendor vendor = vendorBL.Get(vendorId);
                if (vendor.VendorStatus == VendorStatus.Inactive)
                {
                    MessageBox.Show("Laboratory is inactive. Select some other laboratory.");
                    cmb.SelectedIndex = 0;
                    cmb.Focus();
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return true;
        }

        private void LoadMROVendors()
        {
            try
            {
                VendorBL vendorBL = new VendorBL();
                List<Vendor> vendorList = vendorBL.GetListByVendorType(VendorTypes.MRO);

                Vendor tmpVendor = new Vendor();
                tmpVendor.VendorId = 0;
                tmpVendor.VendorName = "(Select MRO)";

                vendorList.Insert(0, tmpVendor);

                cmbMRO.Items.Clear();
                cmbMRO.DataSource = vendorList;
                cmbMRO.ValueMember = "VendorId";
                cmbMRO.DisplayMember = "VendorName";
                cmbMRO.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool MROActive(ComboBox cmb)
        {
            VendorBL vendorBL = new VendorBL();

            int vendorId = 0;

            if (cmb.SelectedIndex > 0)
            {
                vendorId = Convert.ToInt32(cmb.SelectedValue);
                Vendor vendor = vendorBL.Get(vendorId);
                if (vendor.VendorStatus == VendorStatus.Inactive)
                {
                    MessageBox.Show("MRO is inactive. Select some other MRO.");
                    cmb.SelectedIndex = 0;
                    cmb.Focus();
                    return false;
                }
                else
                {
                    return true;
                }
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
                MessageBox.Show(ex.Message);
            }
        }

        private bool SalesRepActive(ComboBox cmb)
        {
            UserBL userBL = new UserBL();
            int userId = 0;

            if (cmb.SelectedIndex > 0)
            {
                userId = Convert.ToInt32(cmb.SelectedValue);
                User user = userBL.Get(userId);
                if (user.IsUserActive == false)
                {
                    MessageBox.Show("Sales representative is inactive. Select some other sales representative.");
                    cmb.SelectedIndex = 0;
                    cmb.Focus();
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return true;
        }

        private void LoadTestPanels()
        {
            try
            {
                //UA Test Panel
                LoadTestPanels(TestCategories.UA, cmbUAMainTestPanel);
                //LoadTestPanels(TestCategories.UA, cmbUAAlt1TestPanel);
                //LoadTestPanels(TestCategories.UA, cmbUAAlt2TestPanel);
                //LoadTestPanels(TestCategories.UA, cmbUAAlt3TestPanel);
                //LoadTestPanels(TestCategories.UA, cmbUAAlt4TestPanel);

                //Hair Test Panel
                LoadTestPanels(TestCategories.Hair, cmbHairMainTestPanel);
                //LoadTestPanels(TestCategories.Hair, cmbHairAlt1TestPanel);
                //LoadTestPanels(TestCategories.Hair, cmbHairAlt2TestPanel);
                //LoadTestPanels(TestCategories.Hair, cmbHairAlt3TestPanel);
                //LoadTestPanels(TestCategories.Hair, cmbHairAlt4TestPanel);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadTestPanels(TestCategories testCategory, ComboBox cmb)
        {
            try
            {
                int tmpValue = Convert.ToInt32(cmb.SelectedValue);

                TestPanelBL testPanelBL = new TestPanelBL();
                List<TestPanel> testPanelUAList = testPanelBL.GetListByCatgory(testCategory);

                TestPanel tmpTestPanel1 = new TestPanel();
                tmpTestPanel1.TestPanelId = 0;
                tmpTestPanel1.TestPanelName = "(Select)";

                testPanelUAList.Insert(0, tmpTestPanel1);

                cmb.DataSource = testPanelUAList;
                cmb.ValueMember = "TestPanelId";
                cmb.DisplayMember = "TestPanelName";

                cmb.SelectedValue = tmpValue;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool TestPanelActive(ComboBox cmb)
        {
            TestPanelBL testPanelBL = new TestPanelBL();
            int testPanelId = 0;

            if (cmb.SelectedIndex > 0)
            {
                testPanelId = Convert.ToInt32(cmb.SelectedValue);
                TestPanel testPanel = testPanelBL.Get(testPanelId);
                if (testPanel.IsActive == false)
                {
                    MessageBox.Show("Test panel  is inactive. Select some other Test panel.");
                    cmb.SelectedIndex = 0;
                    cmb.Focus();
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return true;
        }

        public void LoadClientDepartments()
        {
            try
            {
                if (this._mode == OperationMode.Edit)
                {
                    clientDepartments = clientBL.GetClientDepartmentList(this._clientId);

                    LoadClientDepartmentTreeView();

                    if (currentClientDepartment != null)
                    {
                        string clientDeparmentId = currentClientDepartment.ClientDepartmentId.ToString();
                        currentClientDepartment = null;

                        foreach (ClientDepartment clientDepartment in clientDepartments)
                        {
                            if (clientDepartment.ClientDepartmentId.ToString() == clientDeparmentId)
                            {
                                currentClientDepartment = clientDepartment;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadClientDepartmentTreeView()
        {
            try
            {
                tvDepartmentInfo.Nodes.Clear();

                List<TreeNode> deptNodeList = new List<TreeNode>();

                foreach (ClientDepartment clientDept in clientDepartments)
                {
                    List<TreeNode> testCategoryNodeList = new List<TreeNode>();

                    foreach (ClientDeptTestCategory testCategory in clientDept.ClientDeptTestCategories)
                    {
                        TreeNode testCategoryNode = new TreeNode(testCategory.TestCategoryId.ToDescriptionString());
                        testCategoryNode.Text = testCategory.TestCategoryId.ToDescriptionString();
                        testCategoryNode.ToolTipText = testCategory.TestCategoryId.ToDescriptionString();
                        testCategoryNode.Tag = "TestCategory#" + testCategory.ClientDeptTestCategoryId.ToString() + "#" + testCategory.TestCategoryId.ToString();

                        testCategoryNodeList.Add(testCategoryNode);
                    }

                    TreeNode deptNode = new TreeNode(clientDept.DepartmentName, testCategoryNodeList.ToArray<TreeNode>());
                    deptNode.Text = clientDept.DepartmentName;
                    deptNode.ToolTipText = clientDept.DepartmentName;
                    deptNode.Tag = "Dept#" + clientDept.ClientDepartmentId.ToString();

                    tvDepartmentInfo.Nodes.Add(deptNode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DoTVSelectionProcess(TreeNode node)
        {
            tvSelectionFlag = false;

            foreach (TreeNode tempNode in tvDepartmentInfo.Nodes)
            {
                tempNode.Checked = false;
                foreach (TreeNode tempNode1 in tempNode.Nodes)
                {
                    tempNode1.Checked = false;
                }
            }

            TreeNode currentDeptNode = null;

            if (node.Tag.ToString().StartsWith("Dept"))
            {
                currentDeptNode = node;
                node.Checked = true;
                tvDepartmentInfo.SelectedNode = node;
            }
            else if (node.Tag.ToString().StartsWith("TestCategory"))
            {
                currentDeptNode = node.Parent;
                node.Parent.Checked = true;
                tvDepartmentInfo.SelectedNode = node.Parent;
            }
            else if (node.Tag.ToString().StartsWith("Alt"))
            {
                currentDeptNode = node.Parent.Parent;
                node.Parent.Parent.Checked = true;
                tvDepartmentInfo.SelectedNode = node.Parent.Parent;
            }

            string clientDeparmentId = (currentDeptNode.Tag.ToString().Split('#'))[1];

            if (currentClientDepartment != null)
            {
                if (currentClientDepartment.ClientDepartmentId.ToString() == clientDeparmentId)
                {
                    tvSelectionFlag = true;
                    return;
                }
                else
                {
                    //
                }
            }

            foreach (ClientDepartment clientDepartment in clientDepartments)
            {
                if (clientDepartment.ClientDepartmentId.ToString() == clientDeparmentId)
                {
                    currentClientDepartment = clientDepartment;
                    break;
                }
            }

            tvSelectionFlag = true;
        }

        private void DepartmentAddress(TreeNode node)
        {
            tvSelectionFlag = false;
            if (chkDepartmentAddress.Checked == true)
            {
                clientaddressFlag = true;
                foreach (TreeNode tempNode in tvDepartmentInfo.Nodes)
                {
                    tempNode.Checked = false;
                    foreach (TreeNode tempNode1 in tempNode.Nodes)
                    {
                        tempNode1.Checked = false;
                    }
                }

                TreeNode currentDeptNode = null;

                if (node.Tag.ToString().StartsWith("Dept"))
                {
                    currentDeptNode = node;
                    node.Checked = true;
                    tvDepartmentInfo.SelectedNode = node;
                }
                else if (node.Tag.ToString().StartsWith("TestCategory"))
                {
                    currentDeptNode = node.Parent;
                    node.Parent.Checked = true;
                    tvDepartmentInfo.SelectedNode = node.Parent;
                }
                else if (node.Tag.ToString().StartsWith("Alt"))
                {
                    currentDeptNode = node.Parent.Parent;
                    node.Parent.Parent.Checked = true;
                    tvDepartmentInfo.SelectedNode = node.Parent.Parent;
                }

                string clientDeparmentId = (currentDeptNode.Tag.ToString().Split('#'))[1];
                int ClientDeparmentId = Convert.ToInt32(clientDeparmentId);

                ClientDepartment clientDepartment = clientBL.GetClientDepartment(ClientDeparmentId);

                if (clientDepartment != null)
                {
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

                    //if (chkSameAsClient.Checked == true)
                    //{
                    //    Client client = clientBL.Get(this._clientId);
                    //    chkSameAsPhysical.Checked = client.IsMailingAddressPhysical;

                    //    if (client.SalesRepresentativeId != null)
                    //    {
                    //        cmbSalesRepresentative.SelectedValue = client.SalesRepresentativeId;
                    //    }
                    //    else
                    //    {
                    //        cmbSalesRepresentative.SelectedValue = 0;
                    //    }

                    //    #region Client Contact

                    //    if (client.ClientContact != null)
                    //    {
                    //        txtFirstName.Text = client.ClientContact.ClientContactFirstName;
                    //        txtLastName.Text = client.ClientContact.ClientContactLastName;
                    //        txtPhone.Text = client.ClientContact.ClientContactPhone;
                    //        txtFax.Text = client.ClientContact.ClientContactFax;
                    //        txtEmail.Text = client.ClientContact.ClientContactEmail;
                    //    }

                    //    #endregion

                    //    #region Physical Address

                    //    ClientAddress physicaAddress = null;

                    //    foreach (ClientAddress address in client.ClientAddresses)
                    //    {
                    //        if (address.AddressTypeId == AddressTypes.PhysicalAddress1)
                    //        {
                    //            physicaAddress = address;
                    //            break;
                    //        }
                    //    }

                    //    if (physicaAddress != null)
                    //    {
                    //        txtPhysicalAddress1.Text = physicaAddress.Address1;
                    //        txtPhysicalAddress2.Text = physicaAddress.Address2;
                    //        txtPhysicalCity.Text = physicaAddress.City;
                    //        cmbPhysicalState.Text = physicaAddress.State;
                    //        txtPhysicalZipCode.Text = physicaAddress.ZipCode;
                    //    }

                    //    #endregion

                    //    #region Mailing Address

                    //    ClientAddress mailingAddress = null;
                    //    if (chkSameAsPhysical.Checked == false)
                    //    {
                    //        foreach (ClientAddress address in client.ClientAddresses)
                    //        {
                    //            if (address.AddressTypeId == AddressTypes.MailingAddress)
                    //            {
                    //                mailingAddress = address;
                    //                break;
                    //            }
                    //        }

                    //        if (mailingAddress != null)
                    //        {
                    //            txtMailingAddress1.Text = mailingAddress.Address1;
                    //            txtMailingAddress2.Text = mailingAddress.Address2;
                    //            txtMailingCity.Text = mailingAddress.City;
                    //            cmbMailingState.Text = mailingAddress.State;
                    //            txtMailingZipCode.Text = mailingAddress.ZipCode;

                    //            chkSameAsPhysical.Enabled = false;
                    //            txtMailingAddress1.Enabled = false;
                    //            txtMailingAddress2.Enabled = false;
                    //            txtMailingCity.Enabled = false;
                    //            cmbMailingState.Enabled = false;
                    //            txtMailingZipCode.Enabled = false;

                    //        }
                    //    }
                    //    else if (chkSameAsPhysical.Checked == true)
                    //    {
                    //        txtMailingAddress1.Text = physicaAddress.Address1;
                    //        txtMailingAddress2.Text = physicaAddress.Address2;
                    //        txtMailingCity.Text = physicaAddress.City;
                    //        cmbMailingState.Text = physicaAddress.State;
                    //        txtMailingZipCode.Text = physicaAddress.ZipCode;

                    //        chkSameAsPhysical.Enabled = false;
                    //        txtMailingAddress1.Enabled = false;
                    //        txtMailingAddress2.Enabled = false;
                    //        txtMailingCity.Enabled = false;
                    //        cmbMailingState.Enabled = false;
                    //        txtMailingZipCode.Enabled = false;
                    //    }

                    //    #endregion

                    //}

                    ResetControlsCauseValidation();
                }

                tvSelectionFlag = true;
            }
        }

        private void LoadTestPanelDisplayDetails()
        {
            if (currentClientDepartment != null)
            {
                try
                {
                    testPanelResetFlag = true;

                    lblDepartmentNameValue.Text = currentClientDepartment.DepartmentName;

                    currentUATestCategory = null;
                    currentHairTestCategory = null;
                    currentBCTestCategory = null;
                    currentDNATestCategory = null;
                    currentRecordKeepingTestCategory = null;

                    foreach (ClientDeptTestCategory testCategory in currentClientDepartment.ClientDeptTestCategories)
                    {
                        if (testCategory.TestCategoryId == TestCategories.UA)
                        {
                            currentUATestCategory = testCategory;
                        }
                        else if (testCategory.TestCategoryId == TestCategories.Hair)
                        {
                            currentHairTestCategory = testCategory;
                        }
                        else if (testCategory.TestCategoryId == TestCategories.BC)
                        {
                            currentBCTestCategory = testCategory;
                        }
                        else if (testCategory.TestCategoryId == TestCategories.DNA)
                        {
                            currentDNATestCategory = testCategory;
                        }
                        else if (testCategory.TestCategoryId == TestCategories.RC)
                        {
                            currentRecordKeepingTestCategory = testCategory;
                        }
                    }

                    #region Group Box Visible/Invisible

                    if (currentClientDepartment.IsUA || currentClientDepartment.IsHair || currentClientDepartment.IsDNA || currentClientDepartment.IsBC || currentClientDepartment.IsRecordKeeping)
                    {
                        chkedittestcategory.Visible = true;
                    }
                    else
                    {
                        chkedittestcategory.Visible = false;
                    }

                    if (currentClientDepartment.IsUA)
                    {
                        gboxUA.Visible = true;
                        gboxUA.Enabled = true;
                    }
                    else
                    {
                        gboxUA.Visible = false;
                        gboxUA.Enabled = false;
                    }

                    if (currentClientDepartment.IsHair)
                    {
                        gboxHair.Visible = true;
                        gboxHair.Enabled = true;

                        if (!gboxUA.Visible)
                        {
                            gboxHair.Location = gboxUA.Location;
                        }
                        else
                        {
                            gboxHair.Location = new Point(794, 315);
                        }
                    }
                    else
                    {
                        gboxHair.Visible = false;
                        gboxHair.Enabled = false;
                    }

                    if (currentClientDepartment.IsBC)
                    {
                        gboxBC.Visible = true;
                        gboxBC.Enabled = true;
                        //ToDo:Mike May need to do some placement here.
                    }
                    else
                    {
                        gboxBC.Visible = false;
                        gboxBC.Enabled = false;
                    }

                    if (currentClientDepartment.IsDNA)
                    {
                        gboxDNA.Visible = true;
                        gboxDNA.Enabled = true;

                        //if (!gboxUA.Visible && !gboxHair.Visible)
                        //{
                        //    gboxDNA.Location = gboxUA.Location;
                        //}
                        //else if (!gboxUA.Visible && gboxHair.Visible)
                        //{
                        //    gboxDNA.Location = new Point(867, 315);
                        //}
                        //else if (gboxUA.Visible && !gboxHair.Visible)
                        //{
                        //    gboxDNA.Location = new Point(794, 315);
                        //}
                        //else if (gboxUA.Visible && gboxHair.Visible)
                        //{
                        //    gboxDNA.Location = new Point(561, 414);
                        //}
                    }
                    else
                    {
                        gboxDNA.Visible = false;
                        gboxDNA.Enabled = false;
                    }

                    if (currentClientDepartment.IsRecordKeeping)
                    {
                        gboxRecordKeeping.Visible = true;
                        gboxRecordKeeping.Enabled = true;

                        //if (!gboxUA.Visible && !gboxHair.Visible)
                        //{
                        //    gboxDNA.Location = gboxUA.Location;
                        //}
                        //else if (!gboxUA.Visible && gboxHair.Visible)
                        //{
                        //    gboxDNA.Location = new Point(867, 315);
                        //}
                        //else if (gboxUA.Visible && !gboxHair.Visible)
                        //{
                        //    gboxDNA.Location = new Point(794, 315);
                        //}
                        //else if (gboxUA.Visible && gboxHair.Visible)
                        //{
                        //    gboxDNA.Location = new Point(561, 414);
                        //}
                    }
                    else
                    {
                        gboxRecordKeeping.Visible = false;
                        gboxRecordKeeping.Enabled = false;
                    }

                    #endregion Group Box Visible/Invisible

                    #region Test Panel Values - UA

                    if (currentClientDepartment.IsUA)
                    {
                        cmbUAMainTestPanel.SelectedValue = 0;
                        txtUAMainTestPanelPrice.Text = string.Empty;
                        txtUAMainTestPanelPrice.Enabled = false;

                        foreach (ClientDeptTestPanel testPanel in currentUATestCategory.ClientDeptTestPanels)
                        {
                            if (testPanel.IsMainTestPanel)
                            {
                                cmbUAMainTestPanel.SelectedValue = testPanel.TestPanelId;
                                txtUAMainTestPanelPrice.Text = testPanel.TestPanelPrice.ToString();
                                txtUAMainTestPanelPrice.Enabled = true;
                            }
                        }
                    }

                    #endregion Test Panel Values - UA

                    #region Test Panel Values - Hair

                    if (currentClientDepartment.IsHair)
                    {
                        cmbHairMainTestPanel.SelectedValue = 0;
                        txtHairMainTestPanelPrice.Text = string.Empty;
                        txtHairMainTestPanelPrice.Enabled = false;

                        foreach (ClientDeptTestPanel testPanel in currentHairTestCategory.ClientDeptTestPanels)
                        {
                            if (testPanel.IsMainTestPanel)
                            {
                                cmbHairMainTestPanel.SelectedValue = testPanel.TestPanelId;
                                txtHairMainTestPanelPrice.Text = testPanel.TestPanelPrice.ToString();
                                txtHairMainTestPanelPrice.Enabled = true;
                            }
                        }
                    }

                    #endregion Test Panel Values - Hair

                    #region Test Panel Values - DNA

                    if (currentClientDepartment.IsDNA)
                    {
                        foreach (ClientDeptTestPanel testPanel in currentDNATestCategory.ClientDeptTestPanels)
                        {
                            if (testPanel.IsMainTestPanel)
                            {
                                txtDNATestPrice.Text = currentDNATestCategory.TestPanelPrice.ToString();
                            }
                        }
                    }

                    #endregion Test Panel Values - DNA

                    #region Test Panel Values - BC

                    if (currentClientDepartment.IsBC)
                    {
                        foreach (ClientDeptTestPanel testPanel in currentBCTestCategory.ClientDeptTestPanels)
                        {
                            if (testPanel.IsMainTestPanel)
                            {
                                txtBCTestPrice.Text = testPanel.TestPanelPrice.ToString();
                            }
                        }
                    }

                    #endregion Test Panel Values - BC

                    #region Test Panel Values - RecordKeeping

                    if (currentClientDepartment.IsRecordKeeping)
                    {
                        foreach (ClientDeptTestPanel testPanel in currentRecordKeepingTestCategory.ClientDeptTestPanels)
                        {
                            if (testPanel.IsMainTestPanel)
                            {
                                txtRecordKeepingTestPrice.Text = testPanel.TestPanelPrice.ToString();
                            }
                        }
                    }

                    #endregion Test Panel Values - RecordKeeping

                    testPanelResetFlag = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                //To do reset the controls
            }
        }

        private bool ValidateEmail()
        {
            try
            {
                DataTable client = clientBL.GetByEmail(txtEmail.Text.Trim());

                if (client.Rows.Count > 0 && this._mode == OperationMode.New)
                {
                    return false;
                }
                else if (client.Rows.Count > 1 && this._mode == OperationMode.Edit)
                {
                    return false;
                }
                else if (client.Rows.Count == 1 && this._mode == OperationMode.Edit)
                {
                    if ((int)client.Rows[0]["ClientId"] != this._clientId)
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
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

        #endregion Public Properties

        private void txtUAMainTestPanelPrice_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtHairMainTestPanelPrice_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtDNATestPrice_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtClientCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '.')
            {
                e.Handled = true;
            }
        }

        private void btnBCItemNotFound_Click(object sender, EventArgs e)
        {
            try
            {
                FrmTestPanelDetails frmTestPanelDetails = new FrmTestPanelDetails(OperationMode.New, 0);
                if (frmTestPanelDetails.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    LoadTestPanels();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtBCTestPrice_Leave(object sender, EventArgs e)
        {
            txtBCTestPrice.CausesValidation = false;
            double price = 0.0;

            if (txtBCTestPrice.Text.Trim() != string.Empty)
            {
                price = Convert.ToDouble(txtBCTestPrice.Text.Trim());
            }
            currentBCTestCategory.TestPanelPrice = price;

            var count = currentBCTestCategory.ClientDeptTestPanels.Count();
            if (count <= 0)
            {
                try
                {
                    ClientDeptTestPanel newTestPanel = new ClientDeptTestPanel();

                    newTestPanel.ClientDeptTestPanelId = 0;
                    //newTestPanel.ClientDeptTestCategoryId = currentHairTestCategory.ClientDeptTestCategoryId;
                    newTestPanel.ClientDeptTestCategoryId = currentBCTestCategory.ClientDeptTestCategoryId;

                    newTestPanel.IsMainTestPanel = true;
                    newTestPanel.TestPanelId = 1;

                    if (txtBCTestPrice.Text.Trim() != string.Empty)
                    {
                        newTestPanel.TestPanelPrice = Convert.ToDouble(txtBCTestPrice.Text.Trim());
                    }

                    newTestPanel.DisplayOrder = 0;
                    newTestPanel.CreatedBy = Program.currentUserName;
                    newTestPanel.LastModifiedBy = Program.currentUserName;

                    currentBCTestCategory.ClientDeptTestPanels.Add(newTestPanel);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                if (txtBCTestPrice.Text.Trim() != string.Empty)
                {
                    currentBCTestCategory.TestPanelPrice = Convert.ToDouble(txtBCTestPrice.Text.Trim());
                    //newTestPanel.TestPanelPrice = Convert.ToDouble(txtBCTestPrice.Text.Trim());
                }
            }
        }

        private void txtBCTestPrice_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtBCTestPrice_TextChanged(object sender, EventArgs e)
        {
            txtBCTestPrice.CausesValidation = false;
        }

        private void txtBCTestPrice_Validating(object sender, CancelEventArgs e)
        {
            TextBox txtBCTestPrice = (TextBox)sender;

            try
            {
                if (txtBCTestPrice.Text.Trim() != string.Empty)
                {
                    double price = Convert.ToDouble(txtBCTestPrice.Text);
                }
            }
            catch
            {
                MessageBox.Show("Invalid format of price.");
                e.Cancel = true;
            }
        }

        private void TxtRecordKeepingTestPrice_KeyPress(object sender, KeyPressEventArgs e)
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

        private void TxtRecordKeepingTestPrice_Leave(object sender, EventArgs e)
        {
            TextBox txtRecordKeeping = (TextBox)sender;
            double price = 0.0;

            if (txtRecordKeeping.Text.Trim() != string.Empty)
            {
                try
                {
                    price = Convert.ToDouble(txtRecordKeeping.Text.Trim());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Invalid format of price.");
                    txtRecordKeeping.Focus();
                    return;
                }
            }
        }

        private void TxtRecordKeepingTestPrice_Validating(object sender, CancelEventArgs e)
        {
            TextBox txtRecordKeeping = (TextBox)sender;

            try
            {
                if (txtRecordKeeping.Text.Trim() != string.Empty)
                {
                    double price = Convert.ToDouble(txtRecordKeeping.Text);
                }
            }
            catch
            {
                MessageBox.Show("Invalid format of price.");
                e.Cancel = true;
            }
        }

        private void TxtRecordKeepingTestPrice_Leave_1(object sender, EventArgs e)
        {
            this.txtRecordKeepingTestPrice.CausesValidation = false;

            double price = 0.0;

            if (txtRecordKeepingTestPrice.Text.Trim() != string.Empty)
            {
                price = Convert.ToDouble(txtRecordKeepingTestPrice.Text.Trim());
            }
            currentRecordKeepingTestCategory.TestPanelPrice = price;

            var count = currentRecordKeepingTestCategory.ClientDeptTestPanels.Count();
            if (count <= 0)
            {
                try
                {
                    ClientDeptTestPanel newTestPanel = new ClientDeptTestPanel();

                    newTestPanel.ClientDeptTestPanelId = 0;
                    //newTestPanel.ClientDeptTestCategoryId = currentHairTestCategory.ClientDeptTestCategoryId;
                    newTestPanel.ClientDeptTestCategoryId = currentRecordKeepingTestCategory.ClientDeptTestCategoryId;

                    newTestPanel.IsMainTestPanel = true;
                    newTestPanel.TestPanelId = 1;

                    if (txtRecordKeepingTestPrice.Text.Trim() != string.Empty)
                    {
                        newTestPanel.TestPanelPrice = Convert.ToDouble(txtRecordKeepingTestPrice.Text.Trim());
                    }

                    newTestPanel.DisplayOrder = 0;
                    newTestPanel.CreatedBy = Program.currentUserName;
                    newTestPanel.LastModifiedBy = Program.currentUserName;

                    currentRecordKeepingTestCategory.ClientDeptTestPanels.Add(newTestPanel);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                if (txtBCTestPrice.Text.Trim() != string.Empty)
                {
                    currentBCTestCategory.TestPanelPrice = Convert.ToDouble(txtBCTestPrice.Text.Trim());
                    //newTestPanel.TestPanelPrice = Convert.ToDouble(txtBCTestPrice.Text.Trim());
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                   //FrmClientDepartmentDetails frmClientDepartmentDetails = new FrmClientDepartmentDetails(Enum.OperationMode.View, currentClientDepartment.ClientId, currentClientDepartment.ClientDepartmentId);
                    FrmIntegrations frmIntegrations = new FrmIntegrations(Enum.OperationMode.View, this.ClientId, 0);
                    if (frmIntegrations.ShowDialog() == DialogResult.OK)
                    {
                        //
                    }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }
    }
}