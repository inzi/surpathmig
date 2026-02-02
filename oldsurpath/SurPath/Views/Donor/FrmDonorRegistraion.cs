using SurPath.Business;
using SurPath.Entity;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmDonorRegistraion : Form
    {
        #region Private Variables

        private OperationMode _mode = OperationMode.None;

        private int _donorId = 0;
        private int _donorTestInfoId = 0;
        private string ssn = string.Empty;
        //private int currentTestInfoId = 0;

        private DonorBL donorBL = new DonorBL();

        private Client client = new Client();
        private ClientBL clientBL = new ClientBL();

        #endregion Private Variables

        #region Constructor

        public FrmDonorRegistraion(OperationMode mode)
        {
            InitializeComponent();

            this._mode = mode;
        }

        #endregion Constructor

        #region Event Methods

        private void FrmDonorRegistraion_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                InitializeControls();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmDonorRegistraion_FormClosing(object sender, FormClosingEventArgs e)
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

                foreach (Control ctrl in this.gboxDonorDetails.Controls)
                {
                    if (ctrl.CausesValidation == false)
                    {
                        validationFlag = false;
                        break;
                    }
                }

                foreach (Control ctrl in this.gboxClientDetails.Controls)
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

        private void txtFirstName_TextChanged(object sender, EventArgs e)
        {
            txtFirstName.CausesValidation = false;
        }

        private void txtMiddleInitial_TextChanged(object sender, EventArgs e)
        {
            txtMiddleInitial.CausesValidation = false;
        }

        private void txtLastName_TextChanged(object sender, EventArgs e)
        {
            txtLastName.CausesValidation = false;
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            txtEmail.CausesValidation = false;
        }

        private void txtSSN_TextChanged(object sender, EventArgs e)
        {
            txtSSN.CausesValidation = false;
        }

        private void cmbSuffix_TextChanged(object sender, EventArgs e)
        {
            cmbSuffix.CausesValidation = false;
        }

        private void rbtnFemale_CheckedChanged(object sender, EventArgs e)
        {
            rbtnFemale.CausesValidation = false;
        }

        private void rbtnMale_CheckedChanged(object sender, EventArgs e)
        {
            rbtnMale.CausesValidation = false;
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

        private void txtZipCode_TextChanged(object sender, EventArgs e)
        {
            txtZipCode.CausesValidation = false;
        }

        private void txtPhone1_TextChanged(object sender, EventArgs e)
        {
            txtPhone1.CausesValidation = false;
        }

        private void txtPhone2_TextChanged(object sender, EventArgs e)
        {
            txtPhone2.CausesValidation = false;
        }

        private void cmbClient_TextChanged(object sender, EventArgs e)
        {
            cmbClient.CausesValidation = false;
        }

        private void cmbDepartment_TextChanged(object sender, EventArgs e)
        {
            cmbDepartment.CausesValidation = false;
        }

        private void btnClientNotFound_Click(object sender, EventArgs e)
        {
            FrmClientDetails frmClientDetails = new FrmClientDetails();
            frmClientDetails.ShowDialog();
        }

        private void btnDepartmentNotFound_Click(object sender, EventArgs e)
        {
            FrmClientDetails frmClientDetails = new FrmClientDetails();
            frmClientDetails.ShowDialog();
        }

        private void txtSSN_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void txtDOB_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void txtZipCode_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void txtPhone2_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void txtPhone1_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void cmbClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadClientDepartment();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                DoDonorSearch();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void txtSearchSSN_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void txtSearchSSN_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    DoDonorSearch();
                    return;
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void txtSearchEmail_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    DoDonorSearch();
                    return;
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void txtSearchDOB_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void txtSearchZipCode_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void txtSearchFirstName_TextChanged(object sender, EventArgs e)
        {
            txtSearchFirstName.CausesValidation = false;
        }

        private void txtSearchLastName_TextChanged(object sender, EventArgs e)
        {
            txtSearchLastName.CausesValidation = false;
        }

        private void txtSearchSSN_TextChanged(object sender, EventArgs e)
        {
            txtSearchSSN.CausesValidation = false;
        }

        private void txtSearchDOB_TextChanged(object sender, EventArgs e)
        {
            // txtSearchDOB.CausesValidation = false;
        }

        private void txtSearchCity_TextChanged(object sender, EventArgs e)
        {
            txtSearchCity.CausesValidation = false;
        }

        private void txtSearchZipCode_TextChanged(object sender, EventArgs e)
        {
            txtSearchZipCode.CausesValidation = false;
        }

        private void txtSearchEmail_TextChanged(object sender, EventArgs e)
        {
            txtSearchEmail.CausesValidation = false;
        }

        private void dgvSearch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (e.RowIndex >= 0)
                {
                    int selectedIndex = e.RowIndex;
                    int donorId = (int)dgvSearch.Rows[e.RowIndex].Cells["DonorId"].Value;
                    DonorDetail(donorId);
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvSearch_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvSearch.Rows)
            {
                if (row.Cells["DonorSSN"].Value != null && row.Cells["DonorSSN"].Value.ToString() != string.Empty)
                {
                    if (row.Cells["DonorSSN"].Value.ToString().Length == 11)
                    {
                        row.Cells["SSN"].Value = "***-**-" + row.Cells["DonorSSN"].Value.ToString().Substring(7);
                    }
                }

                if (row.Cells["DonorDOB"].Value != null && row.Cells["DonorDOB"].Value.ToString() != string.Empty)
                {
                    DateTime dob = Convert.ToDateTime(row.Cells["DonorDOB"].Value);
                    if (dob != DateTime.MinValue)
                    {
                        row.Cells["DOB"].Value = dob.ToString("MM/dd/yyyy");
                    }
                }
            }
        }

        #endregion Event Methods

        #region Private Methods

        private void InitializeControls()
        {
            dgvSearch.AutoGenerateColumns = false;
            dgvSearch.DataSource = null;
            dgvSearch.ClearSelection();
            txtSearchFirstName.Text = string.Empty;
            txtSearchLastName.Text = string.Empty;
            txtSearchSSN.Text = string.Empty;
            txtSearchCity.Text = string.Empty;
            txtSearchEmail.Text = string.Empty;
            txtSearchZipCode.Text = string.Empty;
            cmbSearchDate.SelectedIndex = 0;
            cmbSearchMonth.SelectedIndex = 0;
            cmbSearchYear.SelectedIndex = 0;

            txtFirstName.Text = string.Empty;
            txtMiddleInitial.Text = string.Empty;
            txtLastName.Text = string.Empty;
            cmbSuffix.Text = string.Empty;

            txtSSN.Text = string.Empty;
            cmbDonorMonth.SelectedIndex = 0;
            cmbDonorDate.SelectedIndex = 0;
            cmbDonorYear.SelectedIndex = 0;
            txtAddress1.Text = string.Empty;
            txtAddress2.Text = string.Empty;
            txtCity.Text = string.Empty;
            cmbState.SelectedIndex = 0;
            rbtnMale.Checked = false;
            rbtnFemale.Checked = true;
            txtZipCode.Text = string.Empty;
            txtPhone1.Text = string.Empty;
            txtPhone2.Text = string.Empty;
            txtEmail.Text = string.Empty;
            try
            {
                if (this._mode == OperationMode.Edit)
                {
                    gboxDonorDetails.Enabled = false;
                    gboxClientDetails.Enabled = false;
                }

                LoadClient();

                cmbSuffix.SelectedIndex = 0;
                cmbClient.SelectedIndex = 0;
                cmbDepartment.SelectedIndex = 0;
                cmbState.SelectedIndex = 0;
                // ----Donor Search-----//
                cmbSearchYear.Items.Clear();
                var myDate = DateTime.Now;
                var newDate = myDate.AddYears(-125).Year;
                for (int i = newDate; i <= DateTime.Now.Year; i++)
                {
                    cmbSearchYear.Items.Add(i);
                }
                cmbSearchYear.Items.Insert(0, "YYYY");

                //cmbMonth.Items.Clear();
                //var months = System.Globalization.DateTimeFormatInfo.InvariantInfo.MonthNames;
                //for (int i = 0; i < 12; i++ )
                //{
                //    cmbMonth.Items.Add(months[i]);
                //}
                //cmbMonth.Items.Insert(0, "MM");

                cmbSearchMonth.SelectedIndex = 0;
                cmbSearchDate.SelectedIndex = 0;
                cmbSearchYear.SelectedIndex = 0;

                // ----Donor Details-----//
                for (int i = newDate; i <= DateTime.Now.Year; i++)
                {
                    cmbDonorYear.Items.Add(i);
                }
                //  cmbDonorYear.Items.Insert(0, "YYYY");

                cmbDonorMonth.SelectedIndex = 0;
                cmbDonorDate.SelectedIndex = 0;
                cmbDonorYear.SelectedIndex = 0;

                ResetControlsCauseValidation();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadClient()
        {
            try
            {
                List<Client> clientList = clientBL.GetList();

                Client tmpClient = new Client();
                tmpClient.ClientId = 0;
                tmpClient.ClientName = "(Select Client)";

                clientList.Insert(0, tmpClient);

                cmbClient.DataSource = clientList;
                cmbClient.ValueMember = "ClientId";
                cmbClient.DisplayMember = "ClientName";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadClientDepartment()
        {
            try
            {
                int clientId = 0;

                if (cmbClient.SelectedIndex > 0)
                {
                    clientId = Convert.ToInt32(cmbClient.SelectedValue.ToString());
                }

                List<ClientDepartment> clientDepartmentList = clientBL.GetClientDepartmentList(clientId);

                ClientDepartment tmpclientDepartment = new ClientDepartment();
                tmpclientDepartment.ClientDepartmentId = 0;
                tmpclientDepartment.DepartmentName = "(Select Department)";

                clientDepartmentList.Insert(0, tmpclientDepartment);

                cmbDepartment.DataSource = clientDepartmentList;
                cmbDepartment.ValueMember = "ClientDepartmentId";
                cmbDepartment.DisplayMember = "DepartmentName";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ResetControlsCauseValidation()
        {
            try
            {
                foreach (Control ctrl in this.Controls)
                {
                    ctrl.CausesValidation = true;
                }

                foreach (Control ctrl in this.gboxDonorSearch.Controls)
                {
                    ctrl.CausesValidation = true;
                }

                foreach (Control ctrl in this.gboxDonorDetails.Controls)
                {
                    ctrl.CausesValidation = true;
                }

                foreach (Control ctrl in this.gboxClientDetails.Controls)
                {
                    ctrl.CausesValidation = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

                Donor donor = new Donor();

                if (this._mode == OperationMode.New)
                {
                    donor = new Donor();
                    donor.DonorId = 0;
                    //
                    // donor.DonorRegistrationStatusValue = DonorRegistrationStatus.Registered;
                    donor.DonorInitialClientId = (int)cmbClient.SelectedValue;
                    donor.DonorInitialDepartmentId = (int)cmbDepartment.SelectedValue;
                    //
                    donor.CreatedBy = Program.currentUserName;
                }
                else if (this._mode == OperationMode.Edit)
                {
                    donor = donorBL.Get(this._donorId, "Desktop");
                }

                if (cmbSuffix.SelectedIndex != 0)
                {
                    donor.DonorSuffix = cmbSuffix.Text.Trim();
                }
                donor.DonorFirstName = txtFirstName.Text.Trim();
                donor.DonorMI = txtMiddleInitial.Text.Trim();
                donor.DonorLastName = txtLastName.Text.Trim();
                donor.DonorSSN = txtSSN.Text.Trim();

                //if (txtSSN.Text.Length == 9)
                //{
                //    if (txtSSN.Text.Trim() != string.Empty)
                //    {
                //        string NewSSN = txtSSN.Text.Substring(0, 3);
                //        string NewSSN1 = txtSSN.Text.Substring(3, 2);
                //        string NewSSN2 = txtSSN.Text.Substring(5, 4);
                //        string Unmask = NewSSN + "-" + NewSSN1 + "-" + NewSSN2;
                //        //  Unmask = "***-**-" + Unmask.ToString().Substring(7);

                //        donor.DonorSSN = Unmask.ToString();
                //        txtSSN.Tag = donor.DonorSSN;
                //    }
                //}
                //if (txtSSN.Text.Length == 11)
                //{
                //    txtSSN.Tag = donor.DonorSSN;
                //    donor.DonorSSN = txtSSN.Tag.ToString();
                //}

                string donorDOB = cmbDonorYear.Text + '-' + cmbDonorMonth.Text + '-' + cmbDonorDate.Text;
                try
                {
                    DateTime DOB = Convert.ToDateTime(donorDOB);

                    if (DOB > DateTime.Today || DOB < DateTime.Today.AddYears(-125))
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Invalid Date.");
                        cmbDonorMonth.Focus();
                        return false;
                    }

                    donor.DonorDateOfBirth = Convert.ToDateTime(donorDOB.Trim());
                }
                catch
                {
                    MessageBox.Show("Invalid Date");
                    cmbDonorMonth.Focus();
                    return false;
                }
                donor.DonorAddress1 = txtAddress1.Text.Trim();
                donor.DonorAddress2 = txtAddress2.Text.Trim();
                donor.DonorCity = txtCity.Text.Trim();

                if (cmbState.SelectedIndex != 0)
                {
                    donor.DonorState = cmbState.Text.Trim();
                }

                if (rbtnMale.Checked == true)
                {
                    donor.DonorGender = Gender.Male;
                }
                else if (rbtnFemale.Checked == true)
                {
                    donor.DonorGender = Gender.Female;
                }

                if (txtZipCode.Text.EndsWith("-") == true)
                {
                    donor.DonorZip = txtZipCode.Text.Replace("-", "").Trim();
                }
                else
                {
                    donor.DonorZip = txtZipCode.Text.Trim();
                }

                if (txtPhone1.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") != string.Empty)
                {
                    donor.DonorPhone1 = txtPhone1.Text.Trim();
                }
                else
                {
                    donor.DonorPhone1 = string.Empty;
                }

                if (txtPhone2.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") != string.Empty)
                {
                    donor.DonorPhone2 = txtPhone2.Text.Trim();
                }
                else
                {
                    donor.DonorPhone2 = string.Empty;
                }

                donor.DonorEmail = txtEmail.Text.Trim();
                donor.IsWalkinDonor = chkWalkin.Checked;

                donor.LastModifiedBy = Program.currentUserName;

                int returnVal = 0;

                if (this._mode == OperationMode.New)
                {
                    returnVal = donorBL.AddDonor(donor, ref this._donorTestInfoId);
                }
                else if (this._mode == OperationMode.Edit)
                {
                    DonorTestInfo donorTestInfo = donorBL.GetDonorTestInfoByDonorId(this._donorId);
                    if (donorTestInfo.PaymentStatus == PaymentStatus.Paid)
                    {
                        returnVal = donorBL.AddTest(donor, Convert.ToInt32(cmbDepartment.SelectedValue), ref this._donorTestInfoId);
                    }
                    else
                    {
                        MessageBox.Show("You cannot apply / take another test until existing dues / payments are settled");
                    }
                }

                if (returnVal > 0)
                {
                    if (this._mode == OperationMode.New)
                    {
                        donor.DonorId = returnVal;
                        this._donorId = returnVal;
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
            if (txtFirstName.Text.Trim() == string.Empty)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("First Name cannot be empty.");
                txtFirstName.Focus();
                return false;
            }

            if (txtLastName.Text.Trim() == string.Empty)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Last Name cannot be empty.");
                txtLastName.Focus();
                return false;
            }

            if (txtEmail.Text.Trim() == string.Empty)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Email cannot be empty.");
                txtEmail.Focus();
                return false;
            }

            if (txtEmail.Text.Trim() != string.Empty)
            {
                if (!Program.regexEmail.IsMatch(txtEmail.Text.Trim()))
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Invalid format of Email.");
                    txtEmail.Focus();
                    return false;
                }
                else
                {
                    if (!ValidateEmail())
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Email already exists.");
                        txtEmail.Focus();
                        return false;
                    }
                }
            }

            if (txtSSN.Text.Replace("_", "").Replace("-", "").Trim() == string.Empty)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("SSN cannot be empty.");
                txtSSN.Focus();
                return false;
            }
            else
            {
                if (!ValidateSSN())
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("SSN already exists.");
                    txtSSN.Focus();
                    return false;
                }
            }

            if (txtSSN.Text.Contains(" ") || (txtSSN.Text.Length != 11))
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Invalid Format of SSN.");
                txtSSN.Focus();
                return false;
            }

            if (rbtnMale.Checked == false && rbtnFemale.Checked == false)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Gender must be selected.");
                rbtnFemale.Focus();
                return false;
            }

            if (cmbDonorMonth.SelectedIndex == 0)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Date of Birth cannot be empty.");
                cmbDonorMonth.Focus();
                return false;
            }
            if (cmbDonorDate.SelectedIndex == 0)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Date of Birth cannot be empty.");
                cmbDonorDate.Focus();
                return false;
            }
            if (cmbDonorYear.SelectedIndex == 0)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Date of Birth cannot be empty.");
                cmbDonorYear.Focus();
                return false;
            }

            //if (txtDOB.Text.Replace("_", "").Replace("/", "").Trim() == string.Empty)
            //{
            //    Cursor.Current = Cursors.Default;
            //    MessageBox.Show("Date of Birth cannot  be empty.");
            //    txtDOB.Focus();
            //    return false;
            //}
            //if (txtDOB.Text != string.Empty)
            //{
            //    DateTime DOB = Convert.ToDateTime(txtDOB.Text);

            //    if (DOB > DateTime.Today || DOB < DateTime.Today.AddYears(-125))
            //    {
            //        Cursor.Current = Cursors.Default;
            //        MessageBox.Show("Invalid Date.");
            //        txtDOB.Focus();
            //        return false;
            //    }
            //}

            //if (txtAddress1.Text.Trim() == string.Empty)
            //{
            //    Cursor.Current = Cursors.Default;
            //    MessageBox.Show("Address 1 can not be empty.");
            //    txtAddress1.Focus();
            //    return false;
            //}

            //if (txtCity.Text.Trim() == string.Empty)
            //{
            //    Cursor.Current = Cursors.Default;
            //    MessageBox.Show("City can not be empty.");
            //    txtCity.Focus();
            //    return false;
            //}

            //if (cmbState.Text.Trim().ToUpper() == "(Select)".ToUpper())
            //{
            //    Cursor.Current = Cursors.Default;
            //    MessageBox.Show("State must be selected.");
            //    cmbState.Focus();
            //    return false;
            //}

            string ZipCode = txtZipCode.Text.Trim();
            ZipCode = ZipCode.EndsWith("-") ? ZipCode.Replace("-", "") : ZipCode;

            //if (ZipCode == string.Empty)
            //{
            //    Cursor.Current = Cursors.Default;
            //    MessageBox.Show("Zip Code can not be empty.");
            //    txtZipCode.Focus();
            //    return false;
            //}
            if (ZipCode != string.Empty)
            {
                if (!Program.regexZipCode.IsMatch(ZipCode))
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Invalid format of Zip Code.");
                    txtZipCode.Focus();
                    return false;
                }
            }

            if (txtPhone1.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") != string.Empty)
            {
                if (!Program.regexPhoneNumber.IsMatch(txtPhone1.Text.Trim()))
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Invalid format of Phone number.");
                    txtPhone1.Focus();
                    return false;
                }
            }
            else
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Phone 1 cannot be empty.");
                txtPhone1.Focus();
                return false;
            }

            if (txtPhone2.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") != string.Empty)
            {
                if (!Program.regexPhoneNumber.IsMatch(txtPhone2.Text.Trim()))
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Invalid format of Phone number.");
                    txtPhone2.Focus();
                    return false;
                }
            }

            if (cmbClient.SelectedIndex == 0)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Client must be selected.");
                cmbClient.Focus();
                return false;
            }

            if (cmbDepartment.SelectedIndex == 0)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Client Department must be selected.");
                cmbDepartment.Focus();
                return false;
            }
            if (cmbDepartment.SelectedIndex > 0)
            {
                if (!ValidateClientDepartment())
                {
                    return false;
                }
            }
            return true;
        }

        private bool ValidateEmail()
        {
            try
            {
                if (this._mode == OperationMode.New)
                {
                    UserBL userBL = new UserBL();
                    User user = userBL.GetByUsernameOrEmail(txtEmail.Text.Trim());

                    if (user != null)
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

        private bool ValidateClientDepartment()
        {
            ClientBL clientBL = new ClientBL();
            ClientDeptTestCategory testcategory = new ClientDeptTestCategory();
            DataTable dtclient = clientBL.ClientDepartment((int)cmbDepartment.SelectedValue);

            List<ClientDepartment> clientDepartmentList = new List<ClientDepartment>();
            bool UAtestpanelidflag = true;
            bool Hairtestpanelidflag = true;
            foreach (DataRow dr in dtclient.Rows)
            {
                testcategory.TestCategoryId = (TestCategories)(dr["TestCategoryId"]);

                if (testcategory.TestCategoryId == TestCategories.UA)
                {
                    ClientDeptTestPanel clientDepartment = new ClientDeptTestPanel();

                    clientDepartment.TestPanelId = Convert.ToInt32(dr["TestPanelId"]);
                    if (clientDepartment.TestPanelId == 0)
                    {
                        UAtestpanelidflag = false;
                    }
                    else
                    {
                        UAtestpanelidflag = true;
                    }
                }

                if (testcategory.TestCategoryId == TestCategories.Hair)
                {
                    ClientDeptTestPanel clientDepartment = new ClientDeptTestPanel();

                    clientDepartment.TestPanelId = Convert.ToInt32(dr["TestPanelId"]);
                    if (clientDepartment.TestPanelId == 0)
                    {
                        Hairtestpanelidflag = false;
                    }
                    else
                    {
                        Hairtestpanelidflag = true;
                    }
                }
            }
            if (UAtestpanelidflag == false)
            {
                MessageBox.Show("You cannot select this department. Because the UA test Panel is not defined for this department.");
                return false;
            }
            if (Hairtestpanelidflag == false)
            {
                MessageBox.Show("You cannot select this department. Because the Hair test Panel is not defined for this department.");
                return false;
            }

            return true;
        }

        private bool ValidateSSN()
        {
            try
            {
                if (this._mode == OperationMode.New)
                {
                    DonorBL donorBL = new DonorBL();
                    Donor donor = donorBL.GetBySSN(txtSSN.Text.Trim(), "Desktop");

                    if (donor != null)
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

        private void DoDonorSearch()
        {
            try
            {
                // Donor donor = null;

                Dictionary<string, string> searchParam = new Dictionary<string, string>();

                if (txtSearchFirstName.Text.Trim() != string.Empty)
                {
                    searchParam.Add("FirstName", "%" + txtSearchFirstName.Text.Trim() + "%");
                }

                if (txtSearchLastName.Text.Trim() != string.Empty)
                {
                    searchParam.Add("LastName", "%" + txtSearchLastName.Text.Trim() + "%");
                }

                if (txtSearchSSN.Text.Trim() != string.Empty)
                {
                    if (txtSearchSSN.Text.Replace("_", "").Replace("-", "").Trim() != string.Empty)
                    {
                        searchParam.Add("SSN", "%" + txtSearchSSN.Text.Trim() + "%");
                    }
                }

                if (cmbSearchYear.SelectedIndex != 0 && cmbSearchMonth.SelectedIndex != 0 && cmbSearchDate.SelectedIndex != 0)
                {
                    string donorDOB = cmbSearchYear.Text + '-' + cmbSearchMonth.Text + '-' + cmbSearchDate.Text;
                    if (donorDOB != null)
                    {
                        try
                        {
                            DateTime dt = Convert.ToDateTime(donorDOB.ToString());

                            //if (dt > DateTime.Today || dt < DateTime.Today.AddYears(-125))
                            //{
                            //    Cursor.Current = Cursors.Default;
                            //    MessageBox.Show("Invalid Date.");
                            //    cmbSearchMonth.Focus();
                            //    return;
                            //}

                            if (dt > DateTime.Now)
                            {
                                Cursor.Current = Cursors.Default;
                                MessageBox.Show("Invalid DOB.");
                                cmbSearchMonth.Focus();
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("Invalid format of DOB.");
                            cmbSearchMonth.Focus();
                            return;
                        }
                        searchParam.Add("DOB", donorDOB.Trim());
                    }
                }

                //if (txtSearchDOB.Text.Replace("_", "").Replace("/", "").Trim() != string.Empty)
                //{
                //    try
                //    {
                //        DateTime dt = Convert.ToDateTime(txtSearchDOB.Text);
                //        if (dt > DateTime.Now)
                //        {
                //            Cursor.Current = Cursors.Default;
                //            MessageBox.Show("Invalid DOB.");
                //            txtSearchDOB.Focus();
                //            return;
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Cursor.Current = Cursors.Default;
                //        MessageBox.Show("Invalid format of DOB.");
                //        txtSearchDOB.Focus();
                //        return;
                //    }
                //    searchParam.Add("DOB", txtSearchDOB.Text.Trim());
                //}

                if (txtSearchCity.Text.Trim() != string.Empty)
                {
                    searchParam.Add("City", "%" + txtSearchCity.Text.Trim() + "%");
                }

                if (txtSearchZipCode.Text.Trim() != string.Empty)
                {
                    string ZipCode = txtSearchZipCode.Text.Trim();
                    ZipCode = ZipCode.EndsWith("-") ? ZipCode.Replace("-", "") : ZipCode;

                    if (!Program.regexZipCode.IsMatch(ZipCode))
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Invalid format of Zip Code.");
                        txtSearchZipCode.Focus();
                        return;
                    }
                    else
                    {
                        searchParam.Add("ZipCode", txtSearchZipCode.Text.Trim());
                    }
                }

                if (txtSearchEmail.Text.Trim() != string.Empty)
                {
                    if (!Program.regexEmail.IsMatch(txtSearchEmail.Text.Trim()))
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Invalid format of Email.");
                        txtSearchEmail.Focus();
                        return;
                    }
                    else
                    {
                        searchParam.Add("Email", txtSearchEmail.Text.Trim());
                    }
                }
                DonorBL donorBL = new DonorBL();

                DataTable dtDonors = donorBL.DonorSearch(searchParam);

                dgvSearch.DataSource = dtDonors;

                if (dgvSearch.Rows.Count > 0)
                {
                    dgvSearch.Focus();
                }
                else
                {
                    btnSearch.Focus();
                    MessageBox.Show("No Records Found");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DonorDetail(int donorId)
        {
            Donor donor = donorBL.Get(donorId, "Desktop");

            try
            {
                if (donor != null)
                {
                    this._donorId = donor.DonorId;

                    txtFirstName.Text = donor.DonorFirstName;
                    txtMiddleInitial.Text = donor.DonorMI;
                    txtLastName.Text = donor.DonorLastName;

                    if (donor.DonorSuffix != string.Empty)
                    {
                        cmbSuffix.Text = donor.DonorSuffix;
                    }
                    // row.Cells["SSN"].Value = "***-**-" + row.Cells["DonorSSN"].Value.ToString().Substring(7);
                    ssn = donor.DonorSSN;
                    txtSSN.Visible = false;
                    int count = Regex.Matches(ssn, "-").Count;
                    if (ssn.Length == 11 && count == 2)
                    {
                        ssndup.Text = "***-**-" + ssn.ToString().Substring(7);
                        txtSSN.Text = donor.DonorSSN;
                    }
                    else if (ssn.Length == 9)
                    {
                        string NewSSN = ssn.Substring(0, 3);
                        string NewSSN1 = ssn.Substring(3, 2);
                        string NewSSN2 = ssn.Substring(5, 4);
                        string Unmask = NewSSN + "-" + NewSSN1 + "-" + NewSSN2;
                        ssndup.Text = "***-**-" + Unmask.ToString().Substring(7); ;
                        txtSSN.Text = Unmask.ToString();
                    }
                    else
                    {
                        ssndup.Text = string.Empty;
                        txtSSN.Text = string.Empty;
                    }
                    //  ssndup.Text = "***-**-" + donor.DonorSSN.ToString().Substring(7);

                    // txtDOB.Text = donor.DonorDateOfBirth.ToString("MM/dd/yyyy");

                    string DOB = donor.DonorDateOfBirth.ToString("MM/dd/yyyy");
                    cmbDonorMonth.Text = Convert.ToDateTime(DOB).ToString("MM");
                    cmbDonorDate.Text = Convert.ToDateTime(DOB).ToString("dd");
                    cmbDonorYear.Text = Convert.ToDateTime(DOB).ToString("yyyy");
                    txtAddress1.Text = donor.DonorAddress1;
                    txtAddress2.Text = donor.DonorAddress2;
                    txtCity.Text = donor.DonorCity;

                    if (donor.DonorState != string.Empty)
                    {
                        cmbState.Text = donor.DonorState;
                    }

                    if (donor.DonorGender == Gender.Male)
                    {
                        rbtnMale.Checked = true;
                    }
                    else if (donor.DonorGender == Gender.Female)
                    {
                        rbtnFemale.Checked = true;
                    }

                    if (donor.DonorRegistrationStatusValue != DonorRegistrationStatus.PreRegistration && donor.DonorRegistrationStatusValue != DonorRegistrationStatus.Activated)
                    {
                        DonorTestInfo donorTestInfo = donorBL.GetDonorTestInfoByDonorId(donorId);
                        DonorTestInfo donorTestInfo1 = donorBL.GetDonorTestInfo(donorTestInfo.DonorTestInfoId);
                        if (donorTestInfo1.IsWalkinDonor)
                        {
                            chkWalkin.Checked = true;
                        }
                        else
                        {
                            chkWalkin.Checked = false;
                        }
                    }

                    txtZipCode.Text = donor.DonorZip;
                    txtPhone1.Text = donor.DonorPhone1;
                    txtPhone2.Text = donor.DonorPhone2;
                    txtEmail.Text = donor.DonorEmail;

                    txtEmail.Enabled = false;
                    txtSSN.Enabled = false;
                    ssndup.Enabled = false;

                    gboxDonorDetails.Enabled = true;
                    gboxClientDetails.Enabled = true;
                }

                ResetControlsCauseValidation();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion Private Methods

        #region Public Properties

        public int DonorId
        {
            get
            {
                return this._donorId;
            }
            set
            {
                this._donorId = value;
            }
        }

        public int DonorTestInfoId
        {
            get
            {
                return this._donorTestInfoId;
            }
            set
            {
                this._donorTestInfoId = value;
            }
        }

        #endregion Public Properties

        private void cmbSearchMonth_TextChanged(object sender, EventArgs e)
        {
            cmbSearchMonth.CausesValidation = false;
        }

        private void cmbSearchDate_TextChanged(object sender, EventArgs e)
        {
            cmbSearchDate.CausesValidation = false;
        }

        private void cmbSearchYear_TextChanged(object sender, EventArgs e)
        {
            cmbSearchYear.CausesValidation = false;
        }

        private void cmbDonorMonth_TextChanged(object sender, EventArgs e)
        {
            cmbDonorMonth.CausesValidation = false;
        }

        private void cmbDonorDate_TextChanged(object sender, EventArgs e)
        {
            cmbDonorDate.CausesValidation = false;
        }

        private void cmbDonorYear_TextChanged(object sender, EventArgs e)
        {
            cmbDonorYear.CausesValidation = false;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            InitializeControls();
        }

        private void txtSearchZipCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back)))
                e.Handled = true;
        }

        private void txtZipCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back)))
                e.Handled = true;
        }

        private void txtSSN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (txtSSN.TextLength == 9)
            {
                e.Handled = true;
            }
            if (!(Char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void dgvSearch_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            DataGridViewColumn col = dgv.Columns[e.ColumnIndex];
            if (col.Name == "DOB")
            {
                DataGridViewColumn BirthDate = dgv.Columns["DonorDOB"];

                if (BirthDate.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    dgv.Sort(BirthDate, ListSortDirection.Descending);
                    BirthDate.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                    col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
                else
                {
                    dgv.Sort(BirthDate, ListSortDirection.Ascending);
                    BirthDate.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                    col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
            }
            else if (col.Name == "SSN")
            {
                DataGridViewColumn SSN = dgv.Columns["DonorSSN"];

                if (SSN.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    dgv.Sort(SSN, ListSortDirection.Descending);
                    SSN.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                    col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
                else
                {
                    dgv.Sort(SSN, ListSortDirection.Ascending);
                    SSN.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                    col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
            }
        }
    }
}