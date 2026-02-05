using RTF;
using Serilog;
using SurPath.Business;
using SurPath.Business.Master;
using SurPath.Data;
using SurPath.Entity;
using SurPath.Entity.Master;
using SurPath.Enum;
using SurpathBackend;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SurPath
{

    //public static class Prompt
    //{
    //    public static int ShowDialog(string text, string caption)
    //    {
    //        Form prompt = new Form();
    //        prompt.Width = 500;
    //        prompt.Height = 100;
    //        prompt.Text = caption;
    //        Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
    //        NumericUpDown inputBox = new NumericUpDown() { Left = 50, Top = 50, Width = 400 };
    //        Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70 };
    //        confirmation.Click += (sender, e) => { prompt.Close(); };
    //        prompt.Controls.Add(confirmation);
    //        prompt.Controls.Add(textLabel);
    //        prompt.Controls.Add(inputBox);
    //        prompt.ShowDialog();
    //        return (int)inputBox.Value;
    //    }
    //}


    public partial class FrmDonorDetails : Form
    {
        #region Private Variables

        //private List<string> _currentTabList = new List<string>();
        //private Dictionary<string, string> _currentTabDetails = new Dictionary<string, string>();
        private ILogger _logger = Program._logger;
        // private OperationMode _mode = OperationMode.None;
        private int currentDonorId = 0;

        private int currentTestInfoId = 0;
        private int currentDonorTabIndex = -1;
        private string zipcode = string.Empty;
        private string UASpecimenId = string.Empty;

        private bool donorTabChangeFlag = false;
        private bool testInfoTabChangeFlag = false;
        private bool activityChangeFlag = false;
        private bool legalChangeFlag = false;
        private bool paymentChangeFlag = false;

        private bool testflag = false;

        private bool isValidString = false;
        private bool viewflag = false;

        private DonorBL donorBL = new DonorBL();
        private ClientBL clientBL = new ClientBL();
        private VendorBL vendorBL = new VendorBL();

        private HL7ParserBL hl7Parser = new HL7ParserBL();

        private BackendData backendData = new BackendData();
        private BackendLogic backendLogic = new BackendLogic(Program.currentUserName);
        private Notification notification = new Notification();

        private int documentReportId = 0;

        public int radius = 0;
        //  private bool attorneyResetFlag = false;

        #endregion Private Variables

        #region Constructor

        public FrmDonorDetails()
        {
            Program._logger.Debug($"Donor Details Loading");
            InitializeComponent();
        }

        #endregion Constructor

        #region Event Methods

        #region General Events

        private void FrmDonorDetails_Load(object sender, EventArgs e)
        {
            Program._logger.Debug($"InitializeControls");
            InitializeControls();
            Program._logger.Debug("InitializeControls called");
        }

        private void FrmDonorDetails_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.frmMain.frmDonorDetails = null;
        }

        private void tsmiCloseTab_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                RemoveDonorNamesTab();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void tsmiCloseAllTab_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsmiCloseAllButThis_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                RemoveDonorNamesTabButThis();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void tsmiSave_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                SaveData();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void tcDonorList_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTabDetails(tcDonorList.SelectedIndex);
        }

        private void btnVendorSave_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (this.currentTestInfoId > 0)
                {
                    DonorTestInfo donorTestInfo = donorBL.GetDonorTestInfo(this.currentTestInfoId);
                    if (donorTestInfo != null)
                    {
                        if (SaveVendorDetails(false, donorTestInfo))
                        {
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("Vendor Info has been saved successfully.");
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

        private bool SaveVendorDetails(bool validateFlag, DonorTestInfo donorTestInfo)
        {
            if (!validateFlag)
            {
                if (!ValidateVendorDetails())
                {
                    return false;
                }
            }

            //VendorInfo
            if (cmbVendor1Name.SelectedIndex != 0)
            {
                if (!VendorActive(cmbVendor1Name))
                {
                    return false;
                }
            }
            if (cmbVendor1Name.SelectedIndex != 0)
            {
                donorTestInfo.CollectionSite1Id = (int)cmbVendor1Name.SelectedValue;
            }
            if (cmbVendor2Name.SelectedIndex != 0)
            {
                if (!VendorActive(cmbVendor2Name))
                {
                    return false;
                }
            }
            if (cmbVendor2Name.SelectedIndex != 0)
            {
                donorTestInfo.CollectionSite2Id = (int)cmbVendor2Name.SelectedValue;
            }
            if (cmbVendor3Name.SelectedIndex != 0)
            {
                if (!VendorActive(cmbVendor3Name))
                {
                    return false;
                }
            }
            if (cmbVendor3Name.SelectedIndex != 0)
            {
                donorTestInfo.CollectionSite3Id = (int)cmbVendor3Name.SelectedValue;
            }
            if (cmbVendor4Name.SelectedIndex != 0)
            {
                if (!VendorActive(cmbVendor4Name))
                {
                    return false;
                }
            }
            if (cmbVendor4Name.SelectedIndex != 0)
            {
                donorTestInfo.CollectionSite4Id = (int)cmbVendor4Name.SelectedValue;
            }
            //DateTime scheduledate = Convert.ToDateTime(txtScheduleDate.Text);
            //if (scheduledate >= DateTime.Today)
            //{
            //    donorTestInfo.ScheduleDate = Convert.ToDateTime(txtScheduleDate.Text);
            //}
            //else
            //{
            //    MessageBox.Show("Invalid Date.");
            //    return false;
            //}

            //if (cmbDate.Text != "DD" || cmbMonth.Text != "MM" || cmbYear.Text != "YYYY")
            //{
            //    if (cmbDate.Text == "DD" || cmbMonth.Text == "MM" || cmbYear.Text == "YYYY")
            //    {
            //        MessageBox.Show("Invalid format of date.");
            //        return false;
            //    }
            //    string inActiveDate = cmbYear.Text + '-' + cmbMonth.Text + '-' + cmbDate.Text;
            //    DateTime scheduledate = Convert.ToDateTime(inActiveDate.ToString());
            //    try
            //    {
            //        if (scheduledate >= DateTime.Today)
            //        {
            //            donorTestInfo.ScheduleDate = Convert.ToDateTime(inActiveDate.Trim());
            //        }
            //        else
            //        {
            //            Cursor.Current = Cursors.Default;
            //            MessageBox.Show("Invalid Date.");
            //            cmbDonorMonth.Focus();
            //            return false;
            //        }
            //    }
            //    catch
            //    {
            //        MessageBox.Show("Invalid Date");
            //        cmbMonth.Focus();
            //        return false;
            //    }
            //}
            donorTestInfo.LastModifiedBy = Program.currentUserName;
            donorBL.SaveVendorInfoDetails(donorTestInfo);
            //btnLegalInfoSave.Enabled = false;

            //  }
            return true;
        }

        private void cmbVendor1Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            //VendorActive(cmbVendor1Name);
            if (cmbVendor1Name.SelectedIndex != 0)
            {
                int vendorId = (int)cmbVendor1Name.SelectedValue;
                Vendor vendor = vendorBL.Get(vendorId);

                txtVendor1City.Text = vendor.VendorCity;
                txtVendor1State.Text = vendor.VendorState;
                txtVendor1Phone.Text = vendor.VendorPhone;
                txtVendor1Fax.Text = vendor.VendorFax;
            }
            else
            {
                txtVendor1City.Text = string.Empty;
                txtVendor1State.Text = string.Empty;
                txtVendor1Phone.Text = string.Empty;
                txtVendor1Fax.Text = string.Empty;
            }
        }

        private void cmbVendor2Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            // VendorActive(cmbVendor2Name);

            if (cmbVendor2Name.SelectedIndex != 0)
            {
                int vendorId = (int)cmbVendor2Name.SelectedValue;
                Vendor vendor = vendorBL.Get(vendorId);

                txtVendor2City.Text = vendor.VendorCity;
                txtVendor2State.Text = vendor.VendorState;
                txtVendor2Phone.Text = vendor.VendorPhone;
                txtVendor2Fax.Text = vendor.VendorFax;
            }
            else
            {
                txtVendor2City.Text = string.Empty;
                txtVendor2State.Text = string.Empty;
                txtVendor2Phone.Text = string.Empty;
                txtVendor2Fax.Text = string.Empty;
            }
        }

        private void cmbVendor3Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            //VendorActive(cmbVendor3Name);

            if (cmbVendor3Name.SelectedIndex != 0)
            {
                int vendorId = (int)cmbVendor3Name.SelectedValue;
                Vendor vendor = vendorBL.Get(vendorId);

                txtVendor3City.Text = vendor.VendorCity;
                txtVendor3State.Text = vendor.VendorState;
                txtVendor3Phone.Text = vendor.VendorPhone;
                txtVendor3Fax.Text = vendor.VendorFax;
            }
            else
            {
                txtVendor3City.Text = string.Empty;
                txtVendor3State.Text = string.Empty;
                txtVendor3Phone.Text = string.Empty;
                txtVendor3Fax.Text = string.Empty;
            }
        }

        private void cmbVendor4Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            // VendorActive(cmbVendor4Name);

            if (cmbVendor4Name.SelectedIndex != 0)
            {
                int vendorId = (int)cmbVendor4Name.SelectedValue;
                Vendor vendor = vendorBL.Get(vendorId);

                txtVendor4City.Text = vendor.VendorCity;
                txtVendor4State.Text = vendor.VendorState;
                txtVendor4Phone.Text = vendor.VendorPhone;
                txtVendor4Fax.Text = vendor.VendorFax;
            }
            else
            {
                txtVendor4City.Text = string.Empty;
                txtVendor4State.Text = string.Empty;
                txtVendor4Phone.Text = string.Empty;
                txtVendor4Fax.Text = string.Empty;
            }
        }

        private void btnUnmaskSSN_MouseUp(object sender, MouseEventArgs e)
        {
            //btnUnmaskSSN.Image = global::SurPath.Properties.Resources.unmask_SSN;
            //if (txtSSN.Tag.ToString().Length == 11)
            //{
            //   // btnUnmaskSSN.BackColor = Color.WhiteSmoke;
            //    txtSSN.Text = "***-**-" + txtSSN.Tag.ToString().Substring(7);
            //}
        }

        private void cmbDonorMonth_TextChanged(object sender, EventArgs e)
        {
            cmbDonorMonth.CausesValidation = false;
            donorTabChangeFlag = true;
        }

        private void cmbDonorDate_TextChanged(object sender, EventArgs e)
        {
            cmbDonorDate.CausesValidation = false;
            donorTabChangeFlag = true;
        }

        private void cmbDonorYear_TextChanged(object sender, EventArgs e)
        {
            cmbDonorYear.CausesValidation = false;
            donorTabChangeFlag = true;
        }

        private void cmbLegalStartMonth_TextChanged(object sender, EventArgs e)
        {
            cmbLegalStartMonth.CausesValidation = false;
            legalChangeFlag = true;
        }

        private void cmbLegalStartDate_TextChanged(object sender, EventArgs e)
        {
            cmbLegalStartDate.CausesValidation = false;
            legalChangeFlag = true;
        }

        private void cmbLegalStartYear_TextChanged(object sender, EventArgs e)
        {
            cmbLegalStartYear.CausesValidation = false;
            legalChangeFlag = true;
        }

        private void cmbLegalEndMonth_TextChanged(object sender, EventArgs e)
        {
            cmbLegalEndMonth.CausesValidation = false;
            legalChangeFlag = true;
        }

        private void cmbLegalEndDate_TextChanged(object sender, EventArgs e)
        {
            cmbLegalEndDate.CausesValidation = false;
            legalChangeFlag = true;
        }

        private void cmbLegalEndYear_TextChanged(object sender, EventArgs e)
        {
            cmbLegalEndYear.CausesValidation = false;
            legalChangeFlag = true;
        }

        private void pbUnmaskSSN_MouseUp(object sender, MouseEventArgs e)
        {
            pbUnmaskSSN.Image = global::SurPath.Properties.Resources.unmask_SSN;
            if (txtSSN.Tag.ToString().Length == 11)
            {
                // btnUnmaskSSN.BackColor = Color.WhiteSmoke;
                txtSSN.Text = "***-**-" + txtSSN.Tag.ToString().Substring(7);
                //string SSN = txtSSN.Tag.ToString().Substring(7);
                //txtSSN.Mask = "***-**-" + SSN + "".ToString();
                //string Mask = txtSSN.Mask;
                //txtSSN.Text = Mask.ToString();
            }
        }

        private void pbUnmaskSSN_MouseDown(object sender, MouseEventArgs e)
        {
            pbUnmaskSSN.Image = global::SurPath.Properties.Resources.unmask_SSN_2;
            if (txtSSN.Text != string.Empty)
            {
                // btnUnmaskSSN.BackColor = Color.Silver;
                txtSSN.Text = txtSSN.Tag.ToString();
            }
        }

        private void txtZipCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back)))
                e.Handled = true;
        }

        private void tabTestInfo_Click(object sender, EventArgs e)
        {
        }

        private void chkInstant_CheckedChanged(object sender, EventArgs e)
        {
            DoSpecimenIdValidation();
        }

        private void tabDonorInfo_Click(object sender, EventArgs e)
        {
        }

        private void lbldonors_Click(object sender, EventArgs e)
        {
        }

        private void chkInstant_Click(object sender, EventArgs e)
        {
            if (chkInstant.Checked == true)
            {
                pnlTest.Visible = true;
                rbPositive.Enabled = true;
                rbNegative.Enabled = true;
            }
            else if (chkInstant.Checked == false)
            {
                pnlTest.Visible = false;
                rbPositive.Checked = false;
                rbNegative.Checked = false;
            }
        }

        private void cmbCourtName_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbCourtName.CausesValidation = false;
        }

        private void cmbJudgeName_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbJudgeName.CausesValidation = false;
        }

        #endregion General Events

        #region Donor Tab

        private void txtFirstName_TextChanged(object sender, EventArgs e)
        {
            txtFirstName.CausesValidation = false;
            donorTabChangeFlag = true;
        }

        private void txtMiddleInitial_TextChanged(object sender, EventArgs e)
        {
            txtMiddleInitial.CausesValidation = false;
            donorTabChangeFlag = true;
        }

        private void txtLastName_TextChanged(object sender, EventArgs e)
        {
            txtLastName.CausesValidation = false;
            donorTabChangeFlag = true;
        }

        private void cmbSuffix_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbSuffix.CausesValidation = false;
            donorTabChangeFlag = true;
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            txtEmail.CausesValidation = false;
            donorTabChangeFlag = true;
        }

        private void txtDOB_TextChanged(object sender, EventArgs e)
        {
            //txtDOB.CausesValidation = false;
            //donorTabChangeFlag = true;
        }

        private void rbtnFemale_CheckedChanged(object sender, EventArgs e)
        {
            rbtnFemale.CausesValidation = false;
            donorTabChangeFlag = true;
        }

        private void rbtnMale_CheckedChanged(object sender, EventArgs e)
        {
            rbtnMale.CausesValidation = false;
            donorTabChangeFlag = true;
        }

        private void txtAddress1_TextChanged(object sender, EventArgs e)
        {
            txtAddress1.CausesValidation = false;
            donorTabChangeFlag = true;
        }

        private void txtAddress2_TextChanged(object sender, EventArgs e)
        {
            txtAddress2.CausesValidation = false;
            donorTabChangeFlag = true;
        }

        private void txtCity_TextChanged(object sender, EventArgs e)
        {
            txtCity.CausesValidation = false;
            donorTabChangeFlag = true;
        }

        private void txtState_TextChanged(object sender, EventArgs e)
        {
            txtState.CausesValidation = false;
            donorTabChangeFlag = true;
        }

        private void txtZipCode_TextChanged(object sender, EventArgs e)
        {
            txtZipCode.CausesValidation = false;
            donorTabChangeFlag = true;
        }

        private void txtPhone1_TextChanged(object sender, EventArgs e)
        {
            txtPhone1.CausesValidation = false;
            donorTabChangeFlag = true;
        }

        private void txtPhone2_TextChanged(object sender, EventArgs e)
        {
            txtPhone2.CausesValidation = false;
            donorTabChangeFlag = true;
        }

        private void cmbClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadClientDepartment();

            cmbClient.CausesValidation = false;
            donorTabChangeFlag = true;
        }

        private void cmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbDepartment.CausesValidation = false;
            donorTabChangeFlag = true;
        }

        private void txtSSN_TextChanged(object sender, EventArgs e)
        {
            txtSSN.CausesValidation = false;
            donorTabChangeFlag = true;
        }

        private void txtSSN_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void txtDOB_MouseClick(object sender, MouseEventArgs e)
        {
            // SendKeys.Send("{HOME}");
        }

        private void cmbState_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbState.CausesValidation = false;
            donorTabChangeFlag = true;
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

        private void btnUnmaskSSN_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (btnUnmaskSSN.Text.Trim() == "Unmask SSN")
            //    {
            //        if (txtSSN.Text != string.Empty)
            //        {
            //            txtSSN.Text = txtSSN.Tag.ToString();
            //            txtSSN.Mask = "000-00-0000";
            //            btnUnmaskSSN.Text = "Mask SSN";
            //        }
            //    }
            //    else
            //    {
            //        if (txtSSN.Tag.ToString().Replace("_", "").Replace("-", "").Trim() == string.Empty)
            //        {
            //            Cursor.Current = Cursors.Default;
            //            MessageBox.Show("SSN can not be empty.");
            //            txtSSN.Focus();
            //            return;
            //        }

            //        if (txtSSN.Tag.ToString().Contains(" ") || txtSSN.Tag.ToString().Length < 11)
            //        {
            //            Cursor.Current = Cursors.Default;
            //            MessageBox.Show("Invalid Format of SSN.");
            //            txtSSN.Focus();
            //            return;
            //        }

            //        if (txtSSN.Tag.ToString().Length == 11)
            //        {
            //            txtSSN.Tag = txtSSN.Text.Trim();
            //            txtSSN.Mask = "";
            //            txtSSN.Text = "***-**-" + txtSSN.Tag.ToString().Substring(7);
            //            btnUnmaskSSN.Text = "Unmask SSN";
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void btnActivationMailSend_Click(object sender, EventArgs e)
        {
            try
            {
                //MessageBox.Show("Under Development.");
                Cursor.Current = Cursors.WaitCursor;
                UserBL userBL = new UserBL();
                int retunvalue = donorBL.DonorResendMail(this.currentDonorId);
                Cursor.Current = Cursors.Default;
                if (retunvalue == 1)
                {
                    MessageBox.Show("Activation mail send successfully.");
                }
                else
                {
                    MessageBox.Show(" Activation mail not Sent.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDonorSave_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (this.currentTestInfoId > 0)
                {
                    if (SaveDonorDetails(false))
                    {
                        Cursor.Current = Cursors.Default;
                        txtSSN.Text = "***-**-" + txtSSN.Tag.ToString().Substring(7);
                        //string SSN = txtSSN.Tag.ToString().Substring(7);
                        //txtSSN.Mask = "***-**-" + SSN + "".ToString();
                        //string Mask = txtSSN.Mask;
                        //txtSSN.Text = Mask.ToString();

                        MessageBox.Show("Donor Info has been saved successfully.");
                        // Program.frmMain.frmDonorSearch.LoadDonorDetails(tabPageList);
                    }
                }
                if (this.currentTestInfoId == 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    if (SavePreregisteredDonorDetails(false))
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Email has been updated successfully.");
                    }
                }

                LoadActivityDetails();
                LoadDocumentDetails();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        #endregion Donor Tab

        #region Test Info Tab

        private void txtCollectionZipCode_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void txtCollectionPhone_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void txtCollectionFax_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void btnTestingAuthority_Click(object sender, EventArgs e)
        {
            FrmTestingAuthorityDetails frmTestingAuthorityDetails = new FrmTestingAuthorityDetails();
            frmTestingAuthorityDetails.ShowDialog();
        }

        private void chkTestCategory_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUrinalysis.Checked || chkHair.Checked || chkDNA.Checked)
            {
                chkInstant.Enabled = true;
                pnlReason.Enabled = true;
                pnlCup.Enabled = true;
                pnlObserved.Enabled = true;
                pnlFormType.Enabled = true;
                pnlTemprature.Enabled = true;
                pnlAdulteration.Enabled = true;
                pnlQNS.Enabled = true;
                btnTestInfoSave.Enabled = true;

                if (rbFederal.Checked)
                {
                    cmbTestingAuthority.Enabled = true;
                    cmbTestingAuthority.Visible = true;
                    lblTestingAuthority.Visible = true;
                    lblTestingAuthorityMan.Visible = true;
                }
                DoSpecimenIdValidation();

                if (chkDNA.Checked == true && chkHair.Checked == false && chkUrinalysis.Checked == false)
                {
                    lbTestQuestion.Visible = false;
                    label4.Visible = false;
                    pnlTemprature.Visible = false;
                    pnlAdulteration.Visible = false;
                    pnlQNS.Visible = false;
                }
                else
                {
                    lbTestQuestion.Visible = true;
                    label4.Visible = true;
                    pnlTemprature.Visible = true;
                    pnlAdulteration.Visible = true;
                    pnlQNS.Visible = true;
                }
            }
            else
            {
                chkInstant.Enabled = false;
                pnlReason.Enabled = false;
                pnlCup.Enabled = false;
                pnlObserved.Enabled = false;
                pnlFormType.Enabled = false;
                pnlTemprature.Enabled = false;
                pnlAdulteration.Enabled = false;
                pnlQNS.Enabled = false;
                cmbTestingAuthority.Enabled = false;
                cmbTestingAuthority.Visible = false;
                lblTestingAuthority.Visible = false;
                lblTestingAuthorityMan.Visible = false;

                btnTestInfoSave.Enabled = false;
                lblUASpecimenId.Visible = false;
                lblUASpecimenMan.Visible = false;
                txtUASpecimenId.Visible = false;
                txtUASpecimenId.Enabled = false;

                lblHairSpecimenId.Visible = false;
                lblHairSpecimenMan.Visible = false;
                txtHairSpecimenId.Visible = false;
                txtHairSpecimenId.Enabled = false;
            }
        }

        private void btnTestInfoSave_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (this.currentTestInfoId > 0)
                {
                    DonorTestInfo donorTestInfo = donorBL.GetDonorTestInfo(this.currentTestInfoId);
                    if (donorTestInfo != null)
                    {
                        if (donorTestInfo.IsReverseEntry == true)
                        {
                            Donor donor = donorBL.Get(this.currentDonorId, "Desktop");

                            if (SaveTestInfoReverseEntryDetails(false, donorTestInfo, donor))
                            {
                                Cursor.Current = Cursors.Default;
                                MessageBox.Show("Test Info has been saved successfully.");
                                cmbLocationName.Visible = false;
                                cmbCollectionName.Visible = false;
                                txtLocationName.Visible = true;
                                txtCollectorName.Visible = true;
                                LoadTestInfoDetails(donorTestInfo);
                            }
                        }
                        else if (donorTestInfo.IsReverseEntry == false)
                        {
                            if (SaveTestInfoDetails(false, donorTestInfo))
                            {
                                Cursor.Current = Cursors.Default;
                                MessageBox.Show("Test Info has been saved successfully.");
                            }
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

        private void rbReasonForTest_CheckedChanged(object sender, EventArgs e)
        {
            if (rbOther.Checked)
            {
                txtReason.Visible = true;
            }
            else
            {
                txtReason.Visible = false;
            }

            DoSpecimenIdValidation();
        }

        private void rbFormType_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFederal.Checked)
            {
                cmbTestingAuthority.Enabled = true;
                cmbTestingAuthority.Visible = true;
                lblTestingAuthority.Visible = true;
                lblTestingAuthorityMan.Visible = true;
            }
            else
            {
                cmbTestingAuthority.Enabled = false;
                cmbTestingAuthority.Visible = false;
                lblTestingAuthority.Visible = false;
                lblTestingAuthorityMan.Visible = false;
            }

            DoSpecimenIdValidation();
        }

        private void rbSpecimenCup_CheckedChanged(object sender, EventArgs e)
        {
            DoSpecimenIdValidation();
        }

        private void rbObserved_CheckedChanged(object sender, EventArgs e)
        {
            DoSpecimenIdValidation();
        }

        private void rbTemperature_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTemperatureNo.Checked)
            {
                txtTemperature.Visible = true;
                txtTemperature.Enabled = true;
            }
            else
            {
                txtTemperature.Visible = false;
            }

            DoSpecimenIdValidation();
        }

        private void rbAdulteration_CheckedChanged(object sender, EventArgs e)
        {
            DoSpecimenIdValidation();
        }

        private void rbQNS_CheckedChanged(object sender, EventArgs e)
        {
            DoSpecimenIdValidation();
        }

        private void txtReason_TextChanged(object sender, EventArgs e)
        {
            if (rbOther.Checked)
            {
                DoSpecimenIdValidation();
            }
        }

        private void cmbTestingAuthority_SelectedIndexChanged(object sender, EventArgs e)
        {
            DoSpecimenIdValidation();
        }

        #endregion Test Info Tab

        #region Activity / Note

        private void btnActivityNoteSave_Click(object sender, EventArgs e)
        {
            this.CreateNote();
        }

        private void CreateNote()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (this.currentTestInfoId > 0)
                {
                    if (!(rbActivityVisibleYes.Checked || rbActivityVisibleNo.Checked))
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        MessageBox.Show("Activity / Note visible status must be selected.");
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    if (txtActivityNote.Text.Trim() == string.Empty)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        MessageBox.Show("Activity / Note cannot be empty.");
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    DonorActivityNote donorActivityNote = new DonorActivityNote();

                    donorActivityNote.DonorTestInfoId = this.currentTestInfoId;
                    donorActivityNote.ActivityUserId = Program.currentUserId;

                    donorActivityNote.ActivityNote = txtActivityNote.Text.Trim();

                    donorActivityNote.ActivityCategoryId = DonorActivityCategories.General;

                    if (rbActivityVisibleYes.Checked == true)
                    {
                        donorActivityNote.IsActivityVisible = true;
                    }
                    else
                    {
                        donorActivityNote.IsActivityVisible = false;
                    }

                    donorBL.AddDonorActivityNote(donorActivityNote);

                    LoadActivityDetails();

                    txtActivityNote.Text = string.Empty;
                    rbActivityVisibleYes.Checked = false;
                    rbActivityVisibleNo.Checked = false;
                    btnActivityNoteSave.Enabled = true;
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Activity / Note has been saved successfully.");
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void CreateNoteWithText(string NoteText)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                DonorActivityNote donorActivityNote = new DonorActivityNote();

                donorActivityNote.DonorTestInfoId = this.currentTestInfoId;
                donorActivityNote.ActivityUserId = Program.currentUserId;

                donorActivityNote.ActivityNote = NoteText;

                donorActivityNote.ActivityCategoryId = DonorActivityCategories.General;

                donorBL.AddDonorActivityNote(donorActivityNote);
                Cursor.Current = Cursors.Default;
                //MessageBox.Show("Activity / Note has been saved successfully.");

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        #endregion Activity / Note

        #region Legal Info Tab

        private void btnThirdPartyNotFound_Click(object sender, EventArgs e)
        {
            try
            {
                FrmThirdPartyDetails frmThirdPartyDetails = new FrmThirdPartyDetails(Enum.OperationMode.New, this.currentDonorId, 0);
                if (frmThirdPartyDetails.ShowDialog() == DialogResult.OK)
                {
                    LoadThirdParties();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAttorneyNotFound_Click(object sender, EventArgs e)
        {
            try
            {
                if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                {
                    //ATTORNEY_ADD
                    DataRow[] attorneyInfoAdd = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.ATTORNEY_ADD.ToDescriptionString() + "'");

                    if (attorneyInfoAdd.Length > 0)
                    {
                        FrmAttorneyDetails frmAttorneyDetails = new FrmAttorneyDetails(Enum.OperationMode.New, 0);
                        if (frmAttorneyDetails.ShowDialog() == DialogResult.OK)
                        {
                            LoadAttorneys();
                        }
                    }
                    else
                    {
                        // btnAttorneyNotFound.Visible = false;
                    }
                }
                else
                {
                    FrmAttorneyDetails frmAttorneyDetails = new FrmAttorneyDetails(Enum.OperationMode.New, 0);
                    if (frmAttorneyDetails.ShowDialog() == DialogResult.OK)
                    {
                        LoadAttorneys();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCourtNotFound_Click(object sender, EventArgs e)
        {
            try
            {
                string CourtName = cmbCourtName.Text;
                if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                {
                    //COURT_ADD
                    DataRow[] courtInfoAdd = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.COURT_ADD.ToDescriptionString() + "'");

                    if (courtInfoAdd.Length > 0)
                    {
                        FrmCourtDetails frmCourtDetails = new FrmCourtDetails(Enum.OperationMode.New, 0);
                        if (frmCourtDetails.ShowDialog() == DialogResult.OK)
                        {
                            LoadCourts();
                            cmbCourtName.Text = CourtName.ToString();
                        }
                    }
                    else
                    {
                        // btnCourtNotFound.Visible = false;
                    }
                }
                else
                {
                    FrmCourtDetails frmCourtDetails = new FrmCourtDetails(Enum.OperationMode.New, 0);
                    if (frmCourtDetails.ShowDialog() == DialogResult.OK)
                    {
                        LoadCourts();
                        cmbCourtName.Text = CourtName.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnJudgeNotFound_Click(object sender, EventArgs e)
        {
            try
            {
                string JudgeName = cmbJudgeName.Text;

                if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                {
                    //JUDGE_ADD
                    DataRow[] judgeInfoAdd = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.JUDGE_ADD.ToDescriptionString() + "'");

                    if (judgeInfoAdd.Length > 0)
                    {
                        FrmJudgeDetails frmJudgeDetails = new FrmJudgeDetails(Enum.OperationMode.New, 0);
                        if (frmJudgeDetails.ShowDialog() == DialogResult.OK)
                        {
                            LoadJudges();
                            cmbJudgeName.Text = JudgeName.ToString();
                        }
                    }
                    else
                    {
                        // btnCourtNotFound.Visible = false;
                    }
                }
                else
                {
                    FrmJudgeDetails frmJudgeDetails = new FrmJudgeDetails(Enum.OperationMode.New, 0);
                    if (frmJudgeDetails.ShowDialog() == DialogResult.OK)
                    {
                        LoadJudges();
                        cmbJudgeName.Text = JudgeName.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnThirdPartyDetails_Click(object sender, EventArgs e)
        {
            try
            {
                Frm3rdPartyInfo frm3rdPartyInfo = new Frm3rdPartyInfo(this.currentDonorId);
                if (frm3rdPartyInfo.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    LoadThirdParties();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAttorneyInfo1_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbAttorneyName1.SelectedIndex > 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    int attorneyId = (int)cmbAttorneyName1.SelectedValue;

                    if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                    {
                        //ATTORNEY_EDIT
                        DataRow[] attorneyInfoEdit = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.ATTORNEY_EDIT.ToDescriptionString() + "'");

                        if (attorneyInfoEdit.Length > 0)
                        {
                            FrmAttorneyDetails frmAttorneyDetails = new FrmAttorneyDetails(Enum.OperationMode.Edit, attorneyId);
                            if (frmAttorneyDetails.ShowDialog() == DialogResult.OK)
                            {
                                LoadAttornyCombo(cmbAttorneyName1);
                                LoadAttornyCombo(cmbAttorneyName2);
                                LoadAttornyCombo(cmbAttorneyName3);
                            }
                        }
                        else
                        {
                            //
                        }
                    }
                    else
                    {
                        FrmAttorneyDetails frmAttorneyDetails = new FrmAttorneyDetails(Enum.OperationMode.Edit, attorneyId);
                        if (frmAttorneyDetails.ShowDialog() == DialogResult.OK)
                        {
                            LoadAttornyCombo(cmbAttorneyName1);
                            LoadAttornyCombo(cmbAttorneyName2);
                            LoadAttornyCombo(cmbAttorneyName3);
                        }
                    }
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    Cursor.Current = Cursors.WaitCursor;
                    MessageBox.Show("Select Attorney.");
                    cmbAttorneyName1.Focus();
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAttorneyInfo2_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbAttorneyName2.SelectedIndex > 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    int attorneyId = (int)cmbAttorneyName2.SelectedValue;
                    if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                    {
                        //ATTORNEY_EDIT
                        DataRow[] attorneyInfoEdit = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.ATTORNEY_EDIT.ToDescriptionString() + "'");

                        if (attorneyInfoEdit.Length > 0)
                        {
                            FrmAttorneyDetails frmAttorneyDetails = new FrmAttorneyDetails(Enum.OperationMode.Edit, attorneyId);
                            if (frmAttorneyDetails.ShowDialog() == DialogResult.OK)
                            {
                                LoadAttornyCombo(cmbAttorneyName2);
                                LoadAttornyCombo(cmbAttorneyName1);
                                LoadAttornyCombo(cmbAttorneyName3);
                            }
                        }
                        else
                        {
                            //
                        }
                    }
                    else
                    {
                        FrmAttorneyDetails frmAttorneyDetails = new FrmAttorneyDetails(Enum.OperationMode.Edit, attorneyId);
                        if (frmAttorneyDetails.ShowDialog() == DialogResult.OK)
                        {
                            LoadAttornyCombo(cmbAttorneyName2);
                            LoadAttornyCombo(cmbAttorneyName1);
                            LoadAttornyCombo(cmbAttorneyName3);
                        }
                    }
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    Cursor.Current = Cursors.WaitCursor;
                    MessageBox.Show("Select Attorney.");
                    cmbAttorneyName2.Focus();
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAttorneyInfo3_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbAttorneyName3.SelectedIndex > 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    int attorneyId = (int)cmbAttorneyName3.SelectedValue;
                    if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                    {
                        //ATTORNEY_EDIT
                        DataRow[] attorneyInfoEdit = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.ATTORNEY_EDIT.ToDescriptionString() + "'");

                        if (attorneyInfoEdit.Length > 0)
                        {
                            FrmAttorneyDetails frmAttorneyDetails = new FrmAttorneyDetails(Enum.OperationMode.Edit, attorneyId);
                            if (frmAttorneyDetails.ShowDialog() == DialogResult.OK)
                            {
                                LoadAttornyCombo(cmbAttorneyName3);
                                LoadAttornyCombo(cmbAttorneyName2);
                                LoadAttornyCombo(cmbAttorneyName1);
                            }
                        }
                        else
                        {
                            //
                        }
                    }
                    else
                    {
                        FrmAttorneyDetails frmAttorneyDetails = new FrmAttorneyDetails(Enum.OperationMode.Edit, attorneyId);
                        if (frmAttorneyDetails.ShowDialog() == DialogResult.OK)
                        {
                            LoadAttornyCombo(cmbAttorneyName3);
                            LoadAttornyCombo(cmbAttorneyName2);
                            LoadAttornyCombo(cmbAttorneyName1);
                        }
                    }
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    Cursor.Current = Cursors.WaitCursor;
                    MessageBox.Show("Select Attorney.");
                    cmbAttorneyName3.Focus();
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbAttorneyName1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //AttorneyActive(cmbAttorneyName1);
            cmbAttorneyName1.CausesValidation = false;
        }

        private void cmbAttorneyName2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //AttorneyActive(cmbAttorneyName2);
            cmbAttorneyName2.CausesValidation = false;
        }

        private void cmbAttorneyName3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //AttorneyActive(cmbAttorneyName3);
            cmbAttorneyName3.CausesValidation = false;
        }

        private void btnThirdPartyInfo1_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbThirdPartyInfo1Name.SelectedIndex > 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    int thirdPartyId = (int)cmbThirdPartyInfo1Name.SelectedValue;
                    FrmThirdPartyDetails frmThirdPartyDetails = new FrmThirdPartyDetails(Enum.OperationMode.Edit, this.currentDonorId, thirdPartyId);
                    if (frmThirdPartyDetails.ShowDialog() == DialogResult.OK)
                    {
                        LoadThirdPartiesCombo(cmbThirdPartyInfo1Name);
                        LoadThirdPartiesCombo(cmbThirdPartyInfo2Name);
                    }
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    Cursor.Current = Cursors.WaitCursor;
                    MessageBox.Show("Select Third Party.");
                    cmbThirdPartyInfo1Name.Focus();
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void btnThirdPartyInfo2_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbThirdPartyInfo2Name.SelectedIndex > 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    int thirdPartyId = (int)cmbThirdPartyInfo2Name.SelectedValue;
                    FrmThirdPartyDetails frmThirdPartyDetails = new FrmThirdPartyDetails(Enum.OperationMode.Edit, this.currentDonorId, thirdPartyId);
                    if (frmThirdPartyDetails.ShowDialog() == DialogResult.OK)
                    {
                        LoadThirdPartiesCombo(cmbThirdPartyInfo2Name);
                        LoadThirdPartiesCombo(cmbThirdPartyInfo1Name);
                    }
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    Cursor.Current = Cursors.WaitCursor;
                    MessageBox.Show("Select Third Party.");
                    cmbThirdPartyInfo2Name.Focus();
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void txtStartDate_MouseClick(object sender, MouseEventArgs e)
        {
            //  SendKeys.Send("{HOME}");
        }

        private void txtEndDate_MouseClick(object sender, MouseEventArgs e)
        {
            // SendKeys.Send("{HOME}");
        }

        private void cmbThirdPartyInfo1Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbThirdPartyInfo1Name.CausesValidation = false;
            //ThirdPartyActive(cmbThirdPartyInfo1Name);
        }

        private void cmbThirdPartyInfo2Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbThirdPartyInfo2Name.CausesValidation = false;
            //ThirdPartyActive(cmbThirdPartyInfo2Name);
        }

        private void btnLegalInfoSave_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (this.currentTestInfoId > 0)
                {
                    DonorTestInfo donorTestInfo = donorBL.GetDonorTestInfo(this.currentTestInfoId);
                    if (donorTestInfo != null)
                    {
                        string SpecialNotes = txtLegalInfoNotes.Text.Trim();
                        if (SaveLegalDetails(false, donorTestInfo, SpecialNotes))
                        {
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("Legal Info has been saved successfully.");
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

        #endregion Legal Info Tab

        #region Document Tab

        private void dgvDocuments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (e.RowIndex >= 0)
                {
                    string donorDocumentId = dgvDocuments.Rows[e.RowIndex].Cells["DonorDocumentId"].Value.ToString();
                    string fileName = dgvDocuments.Rows[e.RowIndex].Cells["FileName"].Value.ToString();
                    string documentTitle = dgvDocuments.Rows[e.RowIndex].Cells["DocumentTitle"].Value.ToString();

                    if (fileName != string.Empty && donorDocumentId != string.Empty)
                    {
                        DonorDocument donorDocument = donorBL.GetDonorDocument(Convert.ToInt32(donorDocumentId));

                        string targetFile = System.Environment.GetEnvironmentVariable("TEMP") + "\\" + donorDocumentId + "_" + documentTitle + "." + fileName.Trim().Substring(fileName.Trim().LastIndexOf('.') + 1);

                        File.WriteAllBytes(targetFile, donorDocument.DocumentContent);

                        Process Proc = new Process();
                        Proc.StartInfo.FileName = targetFile;
                        Proc.Start();

                        DataGridViewCheckBoxCell fieldSelection = (DataGridViewCheckBoxCell)dgvDocuments.Rows[e.RowIndex].Cells["View"];

                        if (Convert.ToBoolean(fieldSelection.Value))
                        {
                            fieldSelection.Value = false;
                        }
                        else
                        {
                            fieldSelection.Value = true;
                        }
                    }
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Unable to open the document(s).");
            }
        }

        private void dgvDocuments_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    if (dgvTestHistory.CurrentRow.Index >= 0)
                    {
                        string donorDocumentId = dgvDocuments.Rows[dgvTestHistory.CurrentRow.Index].Cells["DonorDocumentId"].Value.ToString();
                        string fileName = dgvDocuments.Rows[dgvTestHistory.CurrentRow.Index].Cells["FileName"].Value.ToString();
                        string documentTitle = dgvDocuments.Rows[dgvTestHistory.CurrentRow.Index].Cells["DocumentTitle"].Value.ToString();

                        if (fileName != string.Empty && donorDocumentId != string.Empty)
                        {
                            DonorDocument donorDocument = donorBL.GetDonorDocument(Convert.ToInt32(donorDocumentId));

                            string targetFile = System.Environment.GetEnvironmentVariable("TEMP") + "\\" + donorDocumentId + "_" + documentTitle + "." + fileName.Trim().Substring(fileName.Trim().LastIndexOf('.') + 1);

                            File.WriteAllBytes(targetFile, donorDocument.DocumentContent);

                            Process Proc = new Process();
                            Proc.StartInfo.FileName = targetFile;
                            Proc.Start();
                        }
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Unable to open the document(s).");
            }
        }

        private void dgvDocuments_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //if (e.ColumnIndex == 0)
                //{
                //    Cursor.Current = Cursors.WaitCursor;
                //    if (e.RowIndex != -1)
                //    {
                //        DataGridViewCheckBoxCell viewSelection = (DataGridViewCheckBoxCell)dgvDocuments.Rows[e.RowIndex].Cells["View"];
                //        if (Convert.ToBoolean(viewSelection.Value))
                //        {
                //            viewSelection.Value = false;
                //        }
                //        else
                //        {
                //            viewSelection.Value = true;
                //        }
                //    }
                //    Cursor.Current = Cursors.Default;
                //}
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDocumentSelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvDocuments.Rows.Count > 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    for (int i = 0; i < dgvDocuments.Rows.Count; i++)
                    {
                        DataGridViewCheckBoxCell viewSelection = (DataGridViewCheckBoxCell)dgvDocuments.Rows[i].Cells["View"];
                        viewSelection.Value = true;
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDocumentDeselectAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvDocuments.Rows.Count > 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    for (int i = 0; i < dgvDocuments.Rows.Count; i++)
                    {
                        DataGridViewCheckBoxCell viewSelection = (DataGridViewCheckBoxCell)dgvDocuments.Rows[i].Cells["View"];
                        viewSelection.Value = false;
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDocumentUpload_Click(object sender, EventArgs e)
        {
            try
            {
                FrmDonorDocumentDetails frmDonorDocumentDetails = new FrmDonorDocumentDetails(this.currentDonorId);
                if (frmDonorDocumentDetails.ShowDialog() == DialogResult.OK)
                {
                    LoadDocumentDetails();

                    MessageBox.Show("Documents has been uploaded succesfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDocumentViewSelected_Click(object sender, EventArgs e)
        {
            try
            {
                int count1 = 0;
                if (dgvDocuments.Rows.Count > 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    for (int i = 0; i < dgvDocuments.Rows.Count; i++)
                    {
                        DataGridViewCheckBoxCell documentSelection = (DataGridViewCheckBoxCell)dgvDocuments.Rows[i].Cells["View"];

                        if (Convert.ToBoolean(documentSelection.Value) == true)
                        {
                            count1++;
                        }
                    }
                }

                if (count1 > 0)
                {
                    if (dgvDocuments.Rows.Count > 0)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        for (int i = 0; i < dgvDocuments.Rows.Count; i++)
                        {
                            DataGridViewCheckBoxCell documentSelection = (DataGridViewCheckBoxCell)dgvDocuments.Rows[i].Cells["View"];

                            if (Convert.ToBoolean(documentSelection.Value))
                            {
                                string donorDocumentId = dgvDocuments.Rows[i].Cells["DonorDocumentId"].Value.ToString();
                                string fileName = dgvDocuments.Rows[i].Cells["FileName"].Value.ToString();
                                string documentTitle = dgvDocuments.Rows[i].Cells["DocumentTitle"].Value.ToString();

                                if (fileName != string.Empty && donorDocumentId != string.Empty)
                                {
                                    DonorDocument donorDocument = donorBL.GetDonorDocument(Convert.ToInt32(donorDocumentId));

                                    string targetFile = System.Environment.GetEnvironmentVariable("TEMP") + "\\" + donorDocumentId + "_" + documentTitle + "." + fileName.Trim().Substring(fileName.Trim().LastIndexOf('.') + 1);

                                    File.WriteAllBytes(targetFile, donorDocument.DocumentContent);

                                    Process Proc = new Process();
                                    Proc.StartInfo.FileName = targetFile;
                                    Proc.Start();
                                }
                            }
                        }
                        Cursor.Current = Cursors.Default;
                    }
                }
                else
                {
                    MessageBox.Show("Select a Document");
                    return;
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Unable to open the document(s).");
            }
        }

        private void btnDocumentViewAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvDocuments.Rows.Count > 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    for (int i = 0; i < dgvDocuments.Rows.Count; i++)
                    {
                        string donorDocumentId = dgvDocuments.Rows[i].Cells["DonorDocumentId"].Value.ToString();
                        string fileName = dgvDocuments.Rows[i].Cells["FileName"].Value.ToString();
                        string documentTitle = dgvDocuments.Rows[i].Cells["DocumentTitle"].Value.ToString();

                        if (fileName != string.Empty && donorDocumentId != string.Empty)
                        {
                            DonorDocument donorDocument = donorBL.GetDonorDocument(Convert.ToInt32(donorDocumentId));

                            string targetFile = System.Environment.GetEnvironmentVariable("TEMP") + "\\" + donorDocumentId + "_" + documentTitle + "." + fileName.Trim().Substring(fileName.Trim().LastIndexOf('.') + 1);

                            File.WriteAllBytes(targetFile, donorDocument.DocumentContent);

                            Process Proc = new Process();
                            Proc.StartInfo.FileName = targetFile;
                            Proc.Start();
                        }
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Unable to open the document(s).");
            }
        }

        private void btnDocumentExportSelected_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                string path = string.Empty;
                int count = 0;
                int count1 = 0;

                if (dgvDocuments.Rows.Count > 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    for (int i = 0; i < dgvDocuments.Rows.Count; i++)
                    {
                        DataGridViewCheckBoxCell documentSelection = (DataGridViewCheckBoxCell)dgvDocuments.Rows[i].Cells["View"];

                        if (Convert.ToBoolean(documentSelection.Value) == true)
                        {
                            count1++;
                        }
                    }
                }
                if (count1 > 0)
                {
                    if (fbdExport.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        path = fbdExport.SelectedPath.ToString().Trim();
                        Cursor.Current = Cursors.Default;
                    }
                }
                else
                {
                    MessageBox.Show("Select a Document");
                    return;
                }

                if (path != string.Empty)
                {
                    if (dgvDocuments.Rows.Count > 0)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        for (int i = 0; i < dgvDocuments.Rows.Count; i++)
                        {
                            DataGridViewCheckBoxCell documentSelection = (DataGridViewCheckBoxCell)dgvDocuments.Rows[i].Cells["View"];

                            if (Convert.ToBoolean(documentSelection.Value))
                            {
                                string donorDocumentId = dgvDocuments.Rows[i].Cells["DonorDocumentId"].Value.ToString();
                                string fileName = dgvDocuments.Rows[i].Cells["FileName"].Value.ToString();
                                string documentTitle = dgvDocuments.Rows[i].Cells["DocumentTitle"].Value.ToString();

                                if (fileName != string.Empty && donorDocumentId != string.Empty)
                                {
                                    DonorDocument donorDocument = donorBL.GetDonorDocument(Convert.ToInt32(donorDocumentId));

                                    string targetFile = path + "\\" + donorDocumentId + "_" + documentTitle + "." + fileName.Trim().Substring(fileName.Trim().LastIndexOf('.') + 1);

                                    File.WriteAllBytes(targetFile, donorDocument.DocumentContent);

                                    count++;
                                }
                            }
                        }
                        Cursor.Current = Cursors.Default;
                    }
                }

                if (count > 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    MessageBox.Show(count.ToString() + " document(s) exported.");
                    Cursor.Current = Cursors.Default;
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Unable to export the document(s).");
            }
        }

        private void btnDocumentExportAll_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                string path = string.Empty;
                int count = 0;

                if (fbdExport.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    path = fbdExport.SelectedPath.ToString().Trim();
                    Cursor.Current = Cursors.Default;
                }

                if (path != string.Empty)
                {
                    if (dgvDocuments.Rows.Count > 0)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        for (int i = 0; i < dgvDocuments.Rows.Count; i++)
                        {
                            string donorDocumentId = dgvDocuments.Rows[i].Cells["DonorDocumentId"].Value.ToString();
                            string fileName = dgvDocuments.Rows[i].Cells["FileName"].Value.ToString();
                            string documentTitle = dgvDocuments.Rows[i].Cells["DocumentTitle"].Value.ToString();

                            if (fileName != string.Empty && donorDocumentId != string.Empty)
                            {
                                DonorDocument donorDocument = donorBL.GetDonorDocument(Convert.ToInt32(donorDocumentId));

                                string targetFile = path + "\\" + donorDocumentId + "_" + documentTitle + "." + fileName.Trim().Substring(fileName.Trim().LastIndexOf('.') + 1);

                                File.WriteAllBytes(targetFile, donorDocument.DocumentContent);

                                count++;
                            }
                        }
                        Cursor.Current = Cursors.Default;
                    }
                }

                if (count > 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    MessageBox.Show(count.ToString() + " document(s) exported.");
                    Cursor.Current = Cursors.Default;
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Unable to export the document(s).");
            }
        }

        #endregion Document Tab

        #region Payment Tab

        private void btnPaymentSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.currentTestInfoId > 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    DonorTestInfo donorTestInfo = donorBL.GetDonorTestInfo(this.currentTestInfoId);
                    if (donorTestInfo != null)
                    {
                        if (SavePaymentDetails(false, donorTestInfo))
                        {
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("Payment has been made successfully.");
                            LoadTestInfoDetails(donorTestInfo);
                            LoadAccountDetails(this.currentDonorId, this.currentTestInfoId);
                        }
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        #endregion Payment Tab

        #region Test History Tab

        private void dgvTestHistory_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvTestHistory.Rows)
            {
                if (row.Cells["SpecimenDate"].Value != null && row.Cells["SpecimenDate"].Value.ToString() != string.Empty)
                {
                    DateTime specimenDate = Convert.ToDateTime(row.Cells["SpecimenDate"].Value);
                    if (specimenDate != DateTime.MinValue)
                    {
                        row.Cells["SpecimenDateValue"].Value = specimenDate.ToString("MM/dd/yyyy");
                    }
                }

                if (row.Cells["MROTypeId"].Value != null && row.Cells["MROTypeId"].Value.ToString() != string.Empty)
                {
                    ClientMROTypes clientMRoTypes = (ClientMROTypes)((int)row.Cells["MROTypeId"].Value);
                    row.Cells["MROType"].Value = clientMRoTypes.ToString();
                }

                if (row.Cells["PaymentTypeId"].Value != null && row.Cells["PaymentTypeId"].Value.ToString() != string.Empty)
                {
                    ClientPaymentTypes clientPaymentTypes = (ClientPaymentTypes)((int)row.Cells["PaymentTypeId"].Value);
                    row.Cells["PaymentType"].Value = clientPaymentTypes.ToDescriptionString();
                }

                if (row.Cells["ReasonForTestId"].Value != null && row.Cells["ReasonForTestId"].Value.ToString() != string.Empty)
                {
                    TestInfoReasonForTest testInfoReasonForTest = (TestInfoReasonForTest)((int)row.Cells["ReasonForTestId"].Value);
                    row.Cells["TestReason"].Value = testInfoReasonForTest.ToDescriptionString();
                }

                //if (row.Cells["TestStatus"].Value != null && row.Cells["TestStatus"].Value.ToString() != string.Empty)
                //{
                //    DonorRegistrationStatus status = (DonorRegistrationStatus)((int)row.Cells["TestStatus"].Value);
                //    row.Cells["Status"].Value = status.ToDescriptionString();
                //}
                if (row.Cells["TestOverallResult"].Value != null && row.Cells["TestOverallResult"].Value.ToString() != string.Empty)
                {
                    OverAllTestResult result = (OverAllTestResult)((int)row.Cells["TestOverallResult"].Value);
                    if (result.ToString() != "None")
                    {
                        row.Cells["Result"].Value = result.ToDescriptionString();
                    }
                    else
                    {
                        row.Cells["Result"].Value = " ";
                    }
                    //row.Cells["Result"].Value = result.ToDescriptionString();
                }
                if (row.Cells["InstantResultTest"].Value != null && row.Cells["InstantResultTest"].Value.ToString() != string.Empty)
                {
                    InstantTestResult instantTestResult = (InstantTestResult)((int)row.Cells["InstantResultTest"].Value);
                    row.Cells["InstantResult"].Value = instantTestResult.ToString();
                }
                if (row.Cells["IsWalkinDonor"].Value != null && row.Cells["IsWalkinDonor"].Value.ToString() != string.Empty)
                {
                    int WalkinDonor = Convert.ToInt32(row.Cells["IsWalkinDonor"].Value);
                    if (WalkinDonor == 1)
                    {
                        row.Cells["WalkinDonor"].Value = "YES";
                    }
                    else
                    {
                        row.Cells["WalkinDonor"].Value = "NO";
                    }
                }
                if (row.Cells["InstantTestTaken"].Value != null && row.Cells["InstantTestTaken"].Value.ToString() != string.Empty)
                {
                    int InstantTest = Convert.ToInt32(row.Cells["InstantTestTaken"].Value);
                    if (InstantTest == 1)
                    {
                        row.Cells["InstantTest"].Value = "YES";
                    }
                    else
                    {
                        row.Cells["InstantTest"].Value = "NO";
                    }
                }
                if (row.Cells["TestStatus"].Value != null && row.Cells["TestStatus"].Value.ToString() != string.Empty)
                {
                    DonorRegistrationStatus status = (DonorRegistrationStatus)((int)row.Cells["TestStatus"].Value);
                    row.Cells["Status"].Value = status.ToDescriptionString();
                }
                else if (row.Cells["DonorRegistrationStatusId"].Value != null && row.Cells["DonorRegistrationStatusId"].Value.ToString() != string.Empty)
                {
                    DonorRegistrationStatus status = (DonorRegistrationStatus)((int)row.Cells["DonorRegistrationStatusId"].Value);
                    if (status == Enum.DonorRegistrationStatus.PreRegistration)
                    {
                        row.Cells["Status"].Value = "Pre-Registered";
                    }
                    else
                    {
                        row.Cells["Status"].Value = status.ToDescriptionString();
                    }
                }
            }
        }

        private void btnTestHistorySelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvTestHistory.Rows.Count > 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    for (int i = 0; i < dgvTestHistory.Rows.Count; i++)
                    {
                        DataGridViewCheckBoxCell donorSelection = (DataGridViewCheckBoxCell)dgvTestHistory.Rows[i].Cells["DonorSelection"];
                        donorSelection.Value = true;
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void btnTestHistoryDeselectAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvTestHistory.Rows.Count > 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    for (int i = 0; i < dgvTestHistory.Rows.Count; i++)
                    {
                        DataGridViewCheckBoxCell donorSelection = (DataGridViewCheckBoxCell)dgvTestHistory.Rows[i].Cells["DonorSelection"];
                        donorSelection.Value = false;
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void btnTestHistoryViewSelected_Click(object sender, EventArgs e)
        {
            try
            {
                int count1 = 0;
                if (dgvTestHistory.Rows.Count > 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    for (int i = 0; i < dgvTestHistory.Rows.Count; i++)
                    {
                        DataGridViewCheckBoxCell documentSelection = (DataGridViewCheckBoxCell)dgvTestHistory.Rows[i].Cells["DonorSelection"];

                        if (Convert.ToBoolean(documentSelection.Value) == true)
                        {
                            count1++;
                        }
                    }
                }

                if (count1 > 0)
                {
                    if (dgvTestHistory.Rows.Count > 0)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        Dictionary<string, string> tabPageList = new Dictionary<string, string>();

                        for (int i = 0; i < dgvTestHistory.Rows.Count; i++)
                        {
                            DataGridViewCheckBoxCell donorSelection = (DataGridViewCheckBoxCell)dgvTestHistory.Rows[i].Cells["DonorSelection"];

                            if (Convert.ToBoolean(donorSelection.Value))
                            {
                                string donorId = dgvTestHistory.Rows[i].Cells["DonorId"].Value.ToString();
                                string donorTestInfoId = dgvTestHistory.Rows[i].Cells["DonorTestInfoId"].Value.ToString();
                                string donorFistName = dgvTestHistory.Rows[i].Cells["DonorFirstName"].Value.ToString();
                                string donorLastName = dgvTestHistory.Rows[i].Cells["DonorLastName"].Value.ToString();

                                donorTestInfoId = donorTestInfoId != string.Empty ? donorTestInfoId : "0";

                                if (donorId != string.Empty && donorTestInfoId != string.Empty)
                                {
                                    string key = donorId + "#" + donorTestInfoId;
                                    string value = donorFistName + " " + donorLastName;
                                    if (!tabPageList.ContainsKey(key))
                                    {
                                        tabPageList.Add(key, value);
                                    }
                                }
                            }
                        }

                        if (tabPageList.Count > 0)
                        {
                            LoadDonorTabDetails(tabPageList);
                        }
                        Cursor.Current = Cursors.Default;
                    }
                }
                else
                {
                    MessageBox.Show("Select a data from list.");
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

        private void btnTestHistoryViewAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvTestHistory.Rows.Count > 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    Dictionary<string, string> tabPageList = new Dictionary<string, string>();

                    for (int i = 0; i < dgvTestHistory.Rows.Count; i++)
                    {
                        string donorId = dgvTestHistory.Rows[i].Cells["DonorId"].Value.ToString();
                        string donorTestInfoId = dgvTestHistory.Rows[i].Cells["DonorTestInfoId"].Value.ToString();
                        string donorFistName = dgvTestHistory.Rows[i].Cells["DonorFirstName"].Value.ToString();
                        string donorLastName = dgvTestHistory.Rows[i].Cells["DonorLastName"].Value.ToString();

                        donorTestInfoId = donorTestInfoId != string.Empty ? donorTestInfoId : "0";

                        if (donorId != string.Empty && donorTestInfoId != string.Empty)
                        {
                            string key = donorId + "#" + donorTestInfoId;
                            string value = donorFistName + " " + donorLastName;
                            if (!tabPageList.ContainsKey(key))
                            {
                                tabPageList.Add(key, value);
                            }
                        }
                    }

                    if (tabPageList.Count > 0)
                    {
                        LoadDonorTabDetails(tabPageList);
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvTestHistory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //if (e.ColumnIndex == 0)
                //{
                //    Cursor.Current = Cursors.WaitCursor;
                //    if (e.RowIndex != -1)
                //    {
                //        DataGridViewCheckBoxCell donorSelection = (DataGridViewCheckBoxCell)dgvTestHistory.Rows[e.RowIndex].Cells["DonorSelection"];
                //        if (Convert.ToBoolean(donorSelection.Value))
                //        {
                //            donorSelection.Value = false;
                //        }
                //        else
                //        {
                //            donorSelection.Value = true;
                //        }
                //    }
                //    Cursor.Current = Cursors.Default;
                //}
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvTestHistory_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    Dictionary<string, string> tabPageList = new Dictionary<string, string>();

                    string donorId = dgvTestHistory.Rows[e.RowIndex].Cells["DonorId"].Value.ToString();
                    string donorTestInfoId = dgvTestHistory.Rows[e.RowIndex].Cells["DonorTestInfoId"].Value.ToString();
                    string donorFistName = dgvTestHistory.Rows[e.RowIndex].Cells["DonorFirstName"].Value.ToString();
                    string donorLastName = dgvTestHistory.Rows[e.RowIndex].Cells["DonorLastName"].Value.ToString();

                    donorTestInfoId = donorTestInfoId != string.Empty ? donorTestInfoId : "0";

                    if (donorId != string.Empty && donorTestInfoId != string.Empty)
                    {
                        string key = donorId + "#" + donorTestInfoId;
                        string value = donorFistName + " " + donorLastName;
                        if (!tabPageList.ContainsKey(key))
                        {
                            tabPageList.Add(key, value);
                        }
                    }

                    LoadDonorTabDetails(tabPageList);

                    if (Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper())
                    {
                        tcMain.SelectedTab = tabDonorInfo;
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvTestHistory_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    if (dgvTestHistory.CurrentRow.Index >= 0)
                    {
                        Dictionary<string, string> tabPageList = new Dictionary<string, string>();

                        string donorId = dgvTestHistory.Rows[dgvTestHistory.CurrentRow.Index].Cells["DonorId"].Value.ToString();
                        string donorTestInfoId = dgvTestHistory.Rows[dgvTestHistory.CurrentRow.Index].Cells["DonorTestInfoId"].Value.ToString();
                        string donorFistName = dgvTestHistory.Rows[dgvTestHistory.CurrentRow.Index].Cells["DonorFirstName"].Value.ToString();
                        string donorLastName = dgvTestHistory.Rows[dgvTestHistory.CurrentRow.Index].Cells["DonorLastName"].Value.ToString();

                        donorTestInfoId = donorTestInfoId != string.Empty ? donorTestInfoId : "0";

                        if (donorId != string.Empty && donorTestInfoId != string.Empty)
                        {
                            string key = donorId + "#" + donorTestInfoId;
                            string value = donorFistName + " " + donorLastName;
                            if (!tabPageList.ContainsKey(key))
                            {
                                tabPageList.Add(key, value);
                            }
                        }

                        LoadDonorTabDetails(tabPageList);
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        #endregion Test History Tab

        #endregion Event Methods

        #region Public Properties

        //public List<string> CurrentTabList
        //{
        //    get
        //    {
        //        return this._currentTabList;
        //    }
        //    set
        //    {
        //        this._currentTabList = value;
        //    }
        //}

        //public Dictionary<string, string> CurrentTabDetails
        //{
        //    get
        //    {
        //        return this._currentTabDetails;
        //    }
        //    set
        //    {
        //        this._currentTabDetails = value;
        //    }
        //}

        #endregion Public Properties

        #region Public Methods

        public int AddDonorNamesTab(string tabText, string tabTag)
        {
            TabPage tabPage = new TabPage();
            tabPage.Text = tabText;
            tabPage.Tag = tabTag;

            tcDonorList.TabPages.Add(tabPage);
            return tcDonorList.TabPages.Count - 1;
        }

        public void AddDonorNamesTab(Dictionary<string, string> tabPages)
        {
            foreach (KeyValuePair<string, string> tabPage in tabPages)
            {
                TabPage newTabPage = new TabPage();
                newTabPage.Text = tabPage.Value;
                newTabPage.Tag = tabPage.Key;

                tcDonorList.TabPages.Add(newTabPage);
            }
        }

        public void RemoveDonorNamesTab()
        {
            if (tcDonorList.TabPages.Count > 0 && this.currentDonorTabIndex >= 0)
            {
                tcDonorList.TabPages.RemoveAt(this.currentDonorTabIndex);

                if (tcDonorList.TabPages.Count == 0)
                {
                    this.Close();
                }
            }
        }

        public void RemoveDonorNamesTabButThis()
        {
            if (tcDonorList.TabPages.Count > 1 && this.currentDonorTabIndex >= 0)
            {
                if (this.currentDonorTabIndex > 0)
                {
                    for (int i = this.currentDonorTabIndex - 1; i >= 0; i--)
                    {
                        tcDonorList.TabPages.RemoveAt(i);
                    }
                }

                if (tcDonorList.TabPages.Count > 1)
                {
                    for (int i = tcDonorList.TabPages.Count - 1; i > 0; i--)
                    {
                        tcDonorList.TabPages.RemoveAt(i);
                    }
                }

                if (tcDonorList.TabPages.Count == 0)
                {
                    this.Close();
                }
            }
        }

        public void LoadTabDetails(string tabTag)
        {
            if (tcDonorList.TabPages.Count > 0)
            {
                for (int i = 0; i < tcDonorList.TabPages.Count; i++)
                {
                    if (tcDonorList.TabPages[i].Tag.ToString().Trim() == tabTag)
                    {
                        LoadTabDetails(i);
                        break;
                    }
                }
            }
        }

        public void LoadTabDetails(int tabIndex, int _radius = -0)
        {
            Program._logger.Debug($"LoadTabDetails {tabIndex} - Radius {_radius}");
            this.radius = 0;
            if (_radius > 0) this.radius = _radius;


            try
            {
                if (tcDonorList.TabPages.Count > 0)
                {
                    string tabTag = tcDonorList.TabPages[tabIndex].Tag.ToString().Trim();
                    string[] ids = tabTag.Split('#');

                    if (ids.Length == 2)
                    {
                        this.currentDonorId = Convert.ToInt32(ids[0]);
                        this.currentTestInfoId = Convert.ToInt32(ids[1]);
                        this.currentDonorTabIndex = tabIndex;

                        tcDonorList.SelectedIndex = tabIndex;

                        LoadDonorDetails();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void InitializeControls()
        {
            dgvTestHistory.AutoGenerateColumns = false;
            dgvDocuments.AutoGenerateColumns = false;
            Program._logger.Debug("ResetControls");
            ResetControls();
            Program._logger.Debug("ResetControls called");
        }

        private void ResetControls()
        {
            #region Tab Header
            Program._logger.Debug("Tab Header");
            lblTabHeaderFirstName.Text = string.Empty;
            lblTabHeaderMI.Text = string.Empty;
            lblTabHeaderLastName.Text = string.Empty;
            lblTabHeaderDOB.Text = string.Empty;
            lblTabHeaderClient.Text = string.Empty;
            lblTabHeaderDepartment.Text = string.Empty;
            lblTabHeaderSpecimenDate.Text = string.Empty;

            #endregion Tab Header

            #region Tab Pages
            Program._logger.Debug("Tab Pages");

            if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
            {
                tcMain.TabPages.Remove(tabDonorInfo);
                tcMain.TabPages.Remove(tabTestInfo);
                tcMain.TabPages.Remove(tabResults);
                tcMain.TabPages.Remove(tabActivity);
                tcMain.TabPages.Remove(tabLegalInfo);
                tcMain.TabPages.Remove(tabClientVendor);
                tcMain.TabPages.Remove(tabDocuments);
                tcMain.TabPages.Remove(tabPayment);
                tcMain.TabPages.Remove(tabAccounting);
                tcMain.TabPages.Remove(tabTestHistory);

                int tabCount = 0;
                bool firstTabFlag = true;

                //DONOR_VIEW_DONOR_INFO_TAB
                Program._logger.Debug("DONOR_VIEW_DONOR_INFO_TAB");

                DataRow[] donorInfoTab = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DONOR_VIEW_DONOR_INFO_TAB.ToDescriptionString() + "'");

                if (donorInfoTab.Length > 0)
                {
                    tcMain.TabPages.Add(tabDonorInfo);
                    tabCount++;

                    if (firstTabFlag)
                    {
                        tabDonorInfo.BringToFront();
                        firstTabFlag = false;

                        tcMain.SelectedTab = tabDonorInfo;
                    }
                }

                //DONOR_VIEW_TEST_INFO_TAB
                Program._logger.Debug("DONOR_VIEW_TEST_INFO_TAB");

                DataRow[] testInfoTab = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DONOR_VIEW_TEST_INFO_TAB.ToDescriptionString() + "'");

                if (testInfoTab.Length > 0)
                {
                    tcMain.TabPages.Add(tabTestInfo);
                    tabCount++;

                    if (firstTabFlag)
                    {
                        tabTestInfo.BringToFront();
                        firstTabFlag = false;

                        tcMain.SelectedTab = tabTestInfo;
                    }
                }

                //DONOR_VIEW_RESULTS_TAB
                Program._logger.Debug("DONOR_VIEW_RESULTS_TAB");

                DataRow[] resultsTab = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DONOR_VIEW_RESULTS_TAB.ToDescriptionString() + "'");

                if (resultsTab.Length > 0)
                {
                    tcMain.TabPages.Add(tabResults);
                    tabCount++;

                    if (firstTabFlag)
                    {
                        tabResults.BringToFront();
                        firstTabFlag = false;

                        tcMain.SelectedTab = tabResults;
                    }
                }

                //DONOR_VIEW_ACTIVITY_NOTES_TAB
                Program._logger.Debug("DONOR_VIEW_ACTIVITY_NOTES_TAB");

                DataRow[] activityTab = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DONOR_VIEW_ACTIVITY_NOTES_TAB.ToDescriptionString() + "'");

                if (activityTab.Length > 0)
                {
                    tcMain.TabPages.Add(tabActivity);
                    tabCount++;

                    if (firstTabFlag)
                    {
                        tabActivity.BringToFront();
                        firstTabFlag = false;

                        tcMain.SelectedTab = tabActivity;
                    }
                }

                //DONOR_VIEW_LEGAL_INFO_TAB
                Program._logger.Debug("DONOR_VIEW_LEGAL_INFO_TAB");

                DataRow[] legalInfoTab = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DONOR_VIEW_LEGAL_INFO_TAB.ToDescriptionString() + "'");

                if (legalInfoTab.Length > 0)
                {
                    tcMain.TabPages.Add(tabLegalInfo);
                    tabCount++;
                    viewflag = true;
                    if (firstTabFlag)
                    {
                        tabLegalInfo.BringToFront();
                        firstTabFlag = false;

                        tcMain.SelectedTab = tabLegalInfo;
                    }
                }

                //DONOR_VIEW_VENDOR_TAB
                Program._logger.Debug("DONOR_VIEW_VENDOR_TAB");

                DataRow[] vendorTab = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DONOR_VIEW_VENDOR_TAB.ToDescriptionString() + "'");

                if (vendorTab.Length > 0)
                {
                    tcMain.TabPages.Add(tabClientVendor);
                    tabCount++;

                    if (firstTabFlag)
                    {
                        tabClientVendor.BringToFront();
                        firstTabFlag = false;

                        tcMain.SelectedTab = tabClientVendor;
                    }
                }

                //DONOR_VIEW_DOCUMENT_TAB
                Program._logger.Debug("DONOR_VIEW_DOCUMENT_TAB");

                DataRow[] documentTab = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DONOR_VIEW_DOCUMENT_TAB.ToDescriptionString() + "'");

                if (documentTab.Length > 0)
                {
                    tcMain.TabPages.Add(tabDocuments);
                    tabCount++;

                    if (firstTabFlag)
                    {
                        tabDocuments.BringToFront();
                        firstTabFlag = false;

                        tcMain.SelectedTab = tabDocuments;
                    }
                }

                //DONOR_VIEW_PAYMENT_TAB
                Program._logger.Debug("DONOR_VIEW_PAYMENT_TAB");

                DataRow[] paymentTab = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DONOR_VIEW_PAYMENT_TAB.ToDescriptionString() + "' OR AuthRuleInternalName = '" + AuthorizationRules.DONOR_COLLECT_PAYMENT.ToDescriptionString() + "'");

                if (paymentTab.Length > 0)
                {
                    tcMain.TabPages.Add(tabPayment);
                    tabCount++;

                    if (firstTabFlag)
                    {
                        tabPayment.BringToFront();
                        firstTabFlag = false;

                        tcMain.SelectedTab = tabPayment;
                    }
                }

                //DONOR_COLLECT_PAYMENT
                Program._logger.Debug("DONOR_COLLECT_PAYMENT");

                DataRow[] collectPaymentTab = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DONOR_COLLECT_PAYMENT.ToDescriptionString() + "'");

                if (collectPaymentTab.Length > 0)
                {
                    //tcMain.TabPages.Add(tabPayment);
                    //tabCount++;

                    if (firstTabFlag)
                    {
                        tabPayment.BringToFront();
                        firstTabFlag = false;

                        tcMain.SelectedTab = tabPayment;
                    }
                    if (paymentTab.Length == 0 && collectPaymentTab.Length > 0)
                    {
                        tcMain.TabPages.Add(tabPayment);
                        tabCount++;

                        if (firstTabFlag)
                        {
                            tabPayment.BringToFront();
                            firstTabFlag = false;

                            tcMain.SelectedTab = tabPayment;
                        }
                    }
                }

                //DONOR_VIEW_ACCOUNTING_TAB
                DataRow[] accountingTab = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DONOR_VIEW_ACCOUNTING_TAB.ToDescriptionString() + "'");

                if (accountingTab.Length > 0)
                {
                    tcMain.TabPages.Add(tabAccounting);
                    tabCount++;

                    if (firstTabFlag)
                    {
                        tabAccounting.BringToFront();
                        firstTabFlag = false;

                        tcMain.SelectedTab = tabAccounting;
                    }
                }

                //DONOR_VIEW_TEST_HISTORY_TAB
                Program._logger.Debug("DONOR_VIEW_TEST_HISTORY_TAB");

                DataRow[] testHistoryTab = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DONOR_VIEW_TEST_HISTORY_TAB.ToDescriptionString() + "'");

                if (testHistoryTab.Length > 0)
                {
                    tcMain.TabPages.Add(tabTestHistory);
                    tabCount++;

                    if (firstTabFlag)
                    {
                        tabTestHistory.BringToFront();
                        firstTabFlag = false;

                        tcMain.SelectedTab = tabTestHistory;
                    }
                }
            }

            #endregion Tab Pages

            #region Donor Info Tab
            Program._logger.Debug("Donor Info Tab");

            txtFirstName.Text = string.Empty;
            txtMiddleInitial.Text = string.Empty;
            txtLastName.Text = string.Empty;

            cmbSuffix.SelectedIndex = 0;
            txtSuffix.Text = string.Empty;

            txtEmail.Text = string.Empty;
            txtSSN.Text = string.Empty;

            txtDOB.Text = string.Empty;

            rbtnFemale.Checked = false;
            rbtnMale.Checked = false;
            txtAddress1.Text = string.Empty;
            txtAddress2.Text = string.Empty;
            txtCity.Text = string.Empty;
            cmbState.SelectedIndex = 0;
            txtState.Text = string.Empty;
            txtZipCode.Text = string.Empty;
            txtPhone1.Text = string.Empty;
            txtPhone2.Text = string.Empty;
            LoadClientName();
            cmbClient.SelectedIndex = 0;
            txtClient.Text = string.Empty;
            cmbDepartment.SelectedIndex = 0;
            txtDepartment.Text = string.Empty;

            lblStatus.Text = string.Empty;

            txtFirstName.ReadOnly = true;
            txtMiddleInitial.ReadOnly = true;
            txtLastName.ReadOnly = true;

            cmbSuffix.Enabled = false;
            cmbSuffix.Visible = false;

            txtSuffix.ReadOnly = true;
            txtSuffix.Visible = true;

            txtEmail.ReadOnly = true;
            txtSSN.ReadOnly = true;
            pbUnmaskSSN.Enabled = true;
            pbUnmaskSSN.BackgroundImage = global::SurPath.Properties.Resources.unmask_SSN;
            //  btnUnmaskSSN.BackColor = Color.WhiteSmoke;
            txtDOB.ReadOnly = true;
            txtDOB.Visible = true;

            //cmbDonorMonth.Enabled = true;
            //cmbDonorDate.Enabled = true;
            //cmbDonorYear.Enabled = true;

            cmbDonorMonth.Enabled = false;
            cmbDonorDate.Enabled = false;
            cmbDonorYear.Enabled = false;

            cmbDonorMonth.Visible = false;
            cmbDonorDate.Visible = false;
            cmbDonorYear.Visible = false;

            rbtnFemale.Enabled = false;
            rbtnMale.Enabled = false;
            txtAddress1.ReadOnly = true;
            txtAddress2.ReadOnly = true;
            txtCity.ReadOnly = true;

            cmbState.Enabled = false;
            cmbState.Visible = false;
            txtState.ReadOnly = true;
            txtState.Visible = true;

            txtZipCode.ReadOnly = true;
            txtPhone1.ReadOnly = true;
            txtPhone2.ReadOnly = true;

            //cmbClient.Enabled = false;
            //cmbClient.Visible = false;
            txtClient.ReadOnly = true;
            txtClient.Visible = true;

            //cmbDepartment.Enabled = false;
            //cmbDepartment.Visible = false;
            txtDepartment.ReadOnly = true;
            txtDepartment.Visible = false;

            btnDonorSave.Visible = false;

            btnActivationMailSend.Visible = true;

            cmbDonorYear.Items.Clear();
            var myDate = DateTime.Now;
            var newDate = myDate.AddYears(-125).Year;
            for (int i = newDate; i <= DateTime.Now.Year; i++)
            {
                cmbDonorYear.Items.Add(i);
            }
            cmbDonorYear.Items.Insert(0, "YYYY");
            cmbDonorMonth.SelectedIndex = 0;
            cmbDonorDate.SelectedIndex = 0;
            cmbDonorYear.SelectedIndex = 0;

            if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
            {
                //DONOR_SSN_UNMASK
                DataRow[] unmaskSSN = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DONOR_SSN_UNMASK.ToDescriptionString() + "'");

                if (unmaskSSN.Length > 0)
                {
                    //btnUnmaskSSN.BackColor = Color.White;
                    pbUnmaskSSN.Image = global::SurPath.Properties.Resources.unmask_SSN;
                    pbUnmaskSSN.Visible = true;
                }
                else
                {
                    pbUnmaskSSN.Visible = false;
                }

                //DONOR_EDIT
                DataRow[] donorInfoEdit = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DONOR_EDIT.ToDescriptionString() + "'");

                if (donorInfoEdit.Length > 0)
                {
                    txtFirstName.ReadOnly = false;
                    txtMiddleInitial.ReadOnly = false;
                    txtLastName.ReadOnly = false;

                    cmbSuffix.Enabled = true;
                    cmbSuffix.Visible = true;
                    txtSuffix.ReadOnly = true;
                    txtSuffix.Visible = false;

                    txtEmail.ReadOnly = true;
                    txtSSN.ReadOnly = true;

                    cmbDonorMonth.Enabled = true;
                    cmbDonorDate.Enabled = true;
                    cmbDonorYear.Enabled = true;
                    cmbDonorMonth.Visible = true;
                    cmbDonorDate.Visible = true;
                    cmbDonorYear.Visible = true;
                    txtDOB.ReadOnly = true;
                    txtDOB.Visible = false;

                    rbtnFemale.Enabled = true;
                    rbtnMale.Enabled = true;
                    txtAddress1.ReadOnly = false;
                    txtAddress2.ReadOnly = false;
                    txtCity.ReadOnly = false;

                    cmbState.Enabled = true;
                    cmbState.Visible = true;
                    txtState.ReadOnly = true;
                    txtState.Visible = false;

                    txtZipCode.ReadOnly = false;
                    txtPhone1.ReadOnly = false;
                    txtPhone2.ReadOnly = false;

                    //cmbClient.Visible = false;
                    //txtClient.ReadOnly = true;
                    txtClient.Visible = true;

                    //cmbDepartment.Visible = false;
                    //txtDepartment.ReadOnly = true;
                    txtDepartment.Visible = true;

                    btnDonorSave.Visible = true;
                }

                //DONOR_ACTIVATION_MAIL_RESEND
                DataRow[] activationMailResend = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DONOR_ACTIVATION_MAIL_RESEND.ToDescriptionString() + "'");

                //if (activationMailResend.Length > 0)
                //{
                btnActivationMailSend.Visible = true;
                //}

                ////DONOR_EDIT_CLIENT_DEPARTMENT
                //DataRow[] donorClientDepartmentChange = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DONOR_EDIT_CLIENT_DEPARTMENT.ToDescriptionString() + "'");

                //if (donorClientDepartmentChange.Length > 0)
                //{
                //    cmbClient.Enabled = true;
                //    cmbDepartment.Enabled = true;
                //}
            }
            else
            {
                txtFirstName.ReadOnly = false;
                txtMiddleInitial.ReadOnly = false;
                txtLastName.ReadOnly = false;

                cmbSuffix.Enabled = true;
                cmbSuffix.Visible = true;
                txtSuffix.ReadOnly = true;
                txtSuffix.Visible = false;

                txtEmail.ReadOnly = false;
                txtSSN.ReadOnly = false;
                // txtDOB.ReadOnly = false;
                cmbDonorMonth.Enabled = true;
                cmbDonorDate.Enabled = true;
                cmbDonorYear.Enabled = true;
                cmbDonorMonth.Visible = true;
                cmbDonorDate.Visible = true;
                cmbDonorYear.Visible = true;
                txtDOB.ReadOnly = true;
                txtDOB.Visible = false;

                cmbDonorMonth.Enabled = true;
                cmbDonorDate.Enabled = true;
                cmbDonorYear.Enabled = true;
                rbtnFemale.Enabled = true;
                rbtnMale.Enabled = true;
                txtAddress1.ReadOnly = false;
                txtAddress2.ReadOnly = false;
                txtCity.ReadOnly = false;

                cmbState.Enabled = true;
                cmbState.Visible = true;
                txtState.ReadOnly = true;
                txtState.Visible = false;

                txtZipCode.ReadOnly = false;
                txtPhone1.ReadOnly = false;
                txtPhone2.ReadOnly = false;

                //cmbClient.Visible = false;
                //txtClient.ReadOnly = true;
                txtClient.Visible = true;

                //cmbDepartment.Visible = false;
                //txtDepartment.ReadOnly = true;
                txtDepartment.Visible = true;

                btnDonorSave.Visible = true;
            }

            if (btnDonorSave.Visible == false && btnActivationMailSend.Visible == true)
            {
                btnActivationMailSend.Location = btnDonorSave.Location;
            }
            else
            {
                btnActivationMailSend.Location = new Point(547, 388);
            }

            #endregion Donor Info Tab

            #region Test Info Tab
            Program._logger.Debug("Test Info Tab");
            ResetTestInfoControls();

            #endregion Test Info Tab

            #region Result Tab
            Program._logger.Debug("Result Tab");

            txtLabOverallResults.Text = string.Empty;
            txtLbReportedDate.Text = string.Empty;
            txtMROOverallResults.Text = string.Empty;
            txtMROReportedDate.Text = string.Empty;
            txtReceivedDate.Text = string.Empty;

            dgvLabResult1.DataSource = null;
            dgvLabResult2.DataSource = null;
            dgvLabResult3.DataSource = null;

            dgvMROResult1.DataSource = null;
            dgvMROResult2.DataSource = null;
            dgvMROResult3.DataSource = null;

            #endregion Result Tab

            #region Activity & Notes Tab
            Program._logger.Debug("Activity & Notes Tab");

            lstActivityNoteHistory.Items.Clear();
            cmbActivityCategory.SelectedIndex = 0;
            rbActivityVisibleYes.Checked = false;
            txtActivityNote.Text = string.Empty;
            btnActivityNoteSave.Enabled = true;

            if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
            {
                //DONOR_EDIT_ACTIVITY_NOTES_TAB
                DataRow[] donorActivityNotesEdit = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DONOR_EDIT_ACTIVITY_NOTES_TAB.ToDescriptionString() + "'");

                if (donorActivityNotesEdit.Length > 0)
                {
                    lstActivityNoteHistory.Items.Clear();
                    cmbActivityCategory.SelectedIndex = 0;
                    rbActivityVisibleYes.Checked = false;
                    rbActivityVisibleNo.Checked = false;
                    txtActivityNote.Text = string.Empty;
                    btnActivityNoteSave.Enabled = true;
                }
                else
                {
                    lstActivityNoteHistory.Items.Clear();
                    cmbActivityCategory.SelectedIndex = 0;
                    rbActivityVisibleYes.Checked = false;
                    rbActivityVisibleNo.Checked = false;
                    txtActivityNote.Text = string.Empty;
                    btnActivityNoteSave.Enabled = false;
                    txtActivityNote.Enabled = false;
                    lstActivityNoteHistory.Enabled = false;
                    cmbActivityCategory.Enabled = false;
                    rbActivityVisibleYes.Enabled = false;
                    rbActivityVisibleNo.Enabled = false;
                }
            }

            #endregion Activity & Notes Tab

            #region Legal Info Tab
            Program._logger.Debug("Legal Info Tab");

            LoadAttorneys();
            LoadThirdParties();
            LoadCourts();
            LoadJudges();

            rbOneTimeTesting.Checked = false;
            rbRandomTesting.Checked = false;
            chkSurScanDates.Checked = false;
            chkThirdPartyDates.Checked = false;

            txtCaseNumber.Text = string.Empty;
            //   txtStartDate.Text = string.Empty;
            //   txtEndDate.Text = string.Empty;
            cmbLegalStartYear.Items.Clear();
            var myLegalDate = DateTime.Now;
            var newLegalDate = myDate.AddYears(125).Year;
            for (int i = DateTime.Now.Year; i <= newLegalDate; i++)
            {
                cmbLegalStartYear.Items.Add(i);
            }
            cmbLegalStartYear.Items.Insert(0, "YYYY");
            cmbLegalStartMonth.SelectedIndex = 0;
            cmbLegalStartDate.SelectedIndex = 0;
            cmbLegalStartYear.SelectedIndex = 0;

            //cmbLegalStartMonth.Text = myLegalDate.ToString("MM");
            //cmbLegalStartDate.Text = myLegalDate.ToString("dd");
            //cmbLegalStartYear.Text = myLegalDate.ToString("yyyy");

            cmbLegalEndYear.Items.Clear();
            var myLegalEndDate = DateTime.Now;
            var newLegalEndDate = myDate.AddYears(125).Year;
            for (int i = DateTime.Now.Year; i <= newLegalEndDate; i++)
            {
                cmbLegalEndYear.Items.Add(i);
            }
            cmbLegalEndYear.Items.Insert(0, "YYYY");
            cmbLegalEndMonth.SelectedIndex = 0;
            cmbLegalEndDate.SelectedIndex = 0;
            cmbLegalEndYear.SelectedIndex = 0;

            //cmbLegalEndMonth.Text = myLegalEndDate.ToString("MM");
            //cmbLegalEndDate.Text = myLegalEndDate.ToString("dd");
            //cmbLegalEndYear.Text = myLegalEndDate.ToString("yyyy");

            txtLegalInfoNotes.Text = string.Empty;
            btnLegalInfoSave.Enabled = true;
            //LoadAttorneys();
            //LoadThirdParties();
            //LoadCourts();
            //LoadJudges();

            if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
            {
                //DONOR_EDIT_LEGAL_INFO_TAB
                DataRow[] donorLegalInfoEdit = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DONOR_EDIT_LEGAL_INFO_TAB.ToDescriptionString() + "'");

                if (donorLegalInfoEdit.Length > 0)
                {
                    rbOneTimeTesting.Checked = false;
                    rbRandomTesting.Checked = false;
                    chkSurScanDates.Checked = false;
                    chkThirdPartyDates.Checked = false;

                    txtCaseNumber.Text = string.Empty;

                    cmbLegalStartMonth.SelectedIndex = 0;
                    cmbLegalStartDate.SelectedIndex = 0;
                    cmbLegalStartYear.SelectedIndex = 0;

                    cmbLegalEndMonth.SelectedIndex = 0;
                    cmbLegalEndDate.SelectedIndex = 0;
                    cmbLegalEndYear.SelectedIndex = 0;

                    txtLegalInfoNotes.Text = string.Empty;
                    btnLegalInfoSave.Enabled = true;
                    LoadAttorneys();
                    LoadThirdParties();
                    LoadCourts();
                    LoadJudges();
                    //  viewflag = true;
                }
                else
                {
                    cmbAttorneyName1.Enabled = false;
                    cmbAttorneyName2.Enabled = false;
                    cmbAttorneyName3.Enabled = false;

                    cmbThirdPartyInfo1Name.Enabled = false;
                    cmbThirdPartyInfo2Name.Enabled = false;

                    btnAttorneyInfo1.Enabled = false;
                    btnAttorneyInfo2.Enabled = false;
                    btnAttorneyInfo3.Enabled = false;

                    btnThirdPartyInfo1.Enabled = false;
                    btnThirdPartyInfo2.Enabled = false;

                    btnAttorneyNotFound.Enabled = false;
                    btnThirdPartyNotFound.Enabled = false;
                    btnThirdPartyDetails.Enabled = false;

                    rbOneTimeTesting.Enabled = false;
                    rbRandomTesting.Enabled = false;

                    chkSurScanDates.Enabled = false;
                    chkThirdPartyDates.Enabled = false;

                    cmbLegalStartMonth.Enabled = false;
                    cmbLegalStartDate.Enabled = false;
                    cmbLegalStartYear.Enabled = false;

                    cmbLegalEndMonth.Enabled = false;
                    cmbLegalEndDate.Enabled = false;
                    cmbLegalEndYear.Enabled = false;

                    txtCaseNumber.Enabled = false;
                    cmbCourtName.Enabled = false;
                    btnCourtNotFound.Enabled = false;
                    btnJudgeNotFound.Enabled = false;
                    cmbJudgeName.Enabled = false;
                    btnLegalInfoSave.Enabled = false;
                    txtLegalInfoNotes.Enabled = false;
                    //viewflag = false;
                }
            }

            #endregion Legal Info Tab

            #region vendor Tab
            Program._logger.Debug("vendor Tab");

            // LoadVendors();
            if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
            {
                //DONOR_EDIT_VENDOR_TAB
                DataRow[] donorVendorInfoEdit = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DONOR_EDIT_VENDOR_TAB.ToDescriptionString() + "'");

                if (donorVendorInfoEdit.Length > 0)
                {
                }
                else
                {
                    cmbVendor1Name.Enabled = false;
                    cmbVendor2Name.Enabled = false;
                    cmbVendor3Name.Enabled = false;
                    cmbVendor4Name.Enabled = false;

                    btnVendorSave.Enabled = false;
                }
            }

            #endregion vendor Tab

            #region Documents
            Program._logger.Debug("Documents");

            dgvDocuments.DataSource = null;
            btnDocumentUpload.Enabled = true;
            btnDocumentSelectAll.Enabled = false;
            btnDocumentDeselectAll.Enabled = false;
            btnDocumentViewSelected.Enabled = false;
            btnDocumentViewAll.Enabled = false;
            btnDocumentExportSelected.Enabled = false;
            btnDocumentExportAll.Enabled = false;

            #endregion Documents

            #region Payment
            Program._logger.Debug("Payment");

            txtPaymentAmount.Text = "0.00";
            // dtpPaymentDate.Value = DateTime.Today;
            cmbPaymentYear.Items.Clear();
            var myPaymentDate = DateTime.Now;
            var newPaymentDate = myDate.AddYears(-125).Year;
            for (int i = newPaymentDate; i <= DateTime.Now.Year; i++)
            {
                cmbPaymentYear.Items.Add(i);
            }
            cmbPaymentYear.Items.Insert(0, "YYYY");
            cmbPaymentMonth.SelectedIndex = 0;
            cmbPaymentDate.SelectedIndex = 0;
            cmbPaymentYear.SelectedIndex = 0;
            cmbPaymentMonth.Text = myPaymentDate.ToString("MM");
            cmbPaymentDate.Text = myPaymentDate.ToString("dd");
            cmbPaymentYear.Text = myPaymentDate.ToString("yyyy");

            rbtnPaymentCash.Checked = false;
            rbtnPaymentCard.Checked = false;
            txtPaymentNote.Text = string.Empty;
            btnPaymentSave.Enabled = false;

            #endregion Payment

            #region Accounting
            Program._logger.Debug("Accounting");

            txtAccSingleUARevenue.Text = string.Empty;
            txtAccSingleHairRevenue.Text = string.Empty;
            txtAccSingleDNARevenue.Text = string.Empty;
            txtAccSingleTotalRevenue.Text = string.Empty;
            txtAccSingleLabCost.Text = string.Empty;
            txtAccSingleMROCost.Text = string.Empty;
            //  txtAccSingleCupCost.Text = string.Empty;
            //    txtAccSingleShippingCost.Text = string.Empty;
            txtAccSingleVendorCost.Text = string.Empty;
            txtAccSingleTotalCost.Text = string.Empty;
            txtAccSingleGrossProfit.Text = string.Empty;

            txtAccConsolUARevenue.Text = string.Empty;
            txtAccConsolHairRevenue.Text = string.Empty;
            txtAccConsolDNARevenue.Text = string.Empty;
            txtAccConsolTotalRevenue.Text = string.Empty;
            txtAccConsolLabCost.Text = string.Empty;
            txtAccConsolMROCost.Text = string.Empty;
            //txtAccConsolCupCost.Text = string.Empty;
            //txtAccConsolShippingCost.Text = string.Empty;
            txtAccConsolVendorCost.Text = string.Empty;
            txtAccConsolTotalCost.Text = string.Empty;
            txtAccConsolGrossProfit.Text = string.Empty;

            #endregion Accounting

            #region Test History
            Program._logger.Debug("Test History");

            dgvTestHistory.DataSource = null;
            btnTestHistorySelectAll.Enabled = false;
            btnTestHistoryDeselectAll.Enabled = false;
            btnTestHistoryViewSelected.Enabled = false;
            btnTestHistoryViewAll.Enabled = false;

            #endregion Test History

            Program._logger.Debug("Calling LoadTestingAuthorityName");

            LoadTestingAuthorityName();
            Program._logger.Debug("Calling LoadClientName");
            LoadClientName();

            //LoadVendorName();
            //cmbClientState.SelectedIndex = 0;
            //cmbVendorState.SelectedIndex = 0;

            donorTabChangeFlag = false;
            testInfoTabChangeFlag = false;
            activityChangeFlag = false;
            legalChangeFlag = false;
            paymentChangeFlag = false;
            Program._logger.Debug("Calling ResetControlsCauseValidation 1");

            ResetControlsCauseValidation();
            Program._logger.Debug("Called ResetControlsCauseValidation 2");

        }

        private void ResetTestInfoControls()
        {
            chkUrinalysis.Checked = false;
            chkUrinalysis.Enabled = false;
            cmbUATestPanel.DataSource = null;
            cmbUATestPanel.Enabled = false;
            lblUASpecimenId.Visible = false;
            lblUASpecimenMan.Visible = false;
            txtUASpecimenId.Text = string.Empty;
            txtUASpecimenId.Visible = false;

            chkHair.Checked = false;
            chkHair.Enabled = false;
            cmbHairTestPanel.DataSource = null;
            cmbHairTestPanel.Enabled = false;
            lblHairSpecimenId.Visible = false;
            lblHairSpecimenMan.Visible = false;
            txtHairSpecimenId.Text = string.Empty;
            txtHairSpecimenId.Visible = false;
            cmbDays.SelectedIndex = 0;
            cmbDays.Enabled = false;

            chkDNA.Checked = false;
            chkDNA.Enabled = false;

            pnlReason.Enabled = false;
            rbPreEmployment.Checked = false;
            rbRandom.Checked = false;
            rbReasonable.Checked = false;
            rbPostAccident.Checked = false;
            rbReturntoDuty.Checked = false;
            rbFollowUp.Checked = false;
            rbOther.Checked = false;
            txtReason.Text = string.Empty;
            txtReason.Visible = false;

            pnlCup.Enabled = false;
            rbUrineSingle.Checked = false;
            rbUrineSplit.Checked = false;
            rbSaliva.Checked = false;
            rbBlood.Checked = false;

            pnlObserved.Enabled = false;
            rbObservedYes.Checked = false;
            rbObservedNo.Checked = false;

            pnlFormType.Enabled = false;
            rbFederal.Checked = false;
            rbNonFederal.Checked = false;

            cmbTestingAuthority.DataSource = null;
            cmbTestingAuthority.Enabled = false;
            cmbTestingAuthority.Visible = false;
            lblTestingAuthority.Visible = false;
            lblTestingAuthorityMan.Visible = false;

            pnlTemprature.Enabled = false;
            rbTemperatureYes.Checked = false;
            rbTemperatureNo.Checked = false;
            txtTemperature.Text = string.Empty;
            txtTemperature.Visible = false;

            pnlAdulteration.Enabled = false;
            rbAdulterationYes.Checked = false;
            rbAdulterationNo.Checked = false;

            pnlQNS.Enabled = false;
            rbQNSYes.Checked = false;
            rbQNSNo.Checked = false;

            txtLocationName.Text = string.Empty;
            txtCollectionAddress1.Text = string.Empty;
            txtCollectionAddress2.Text = string.Empty;
            txtCollectionCity.Text = string.Empty;
            txtCollectionState.Text = string.Empty;
            txtCollectionZipCode.Text = string.Empty;
            txtCollectionPhone.Text = string.Empty;
            txtCollectionFax.Text = string.Empty;
            txtCollectionEmail.Text = string.Empty;
            txtScreeningDate.Text = string.Empty;
            txtScreeningTime.Text = string.Empty;
            txtCollectorName.Text = string.Empty;

            chkDonorRefuses.Checked = false;

            if (chkInstant.Checked == true)
            {
                pnlTest.Visible = true;
            }
            if (chkInstant.Checked == false)
            {
                pnlTest.Visible = false;
            }
        }

        private void LoadTestingAuthorityName(string getInActive = null)
        {
            TestingAuthorityBL testingAuthorityBL = new TestingAuthorityBL();
            List<TestingAuthority> testingAuthorityList = testingAuthorityBL.GetList(getInActive);

            TestingAuthority tmpTestingAuthority = new TestingAuthority();
            tmpTestingAuthority.TestingAuthorityId = 0;
            tmpTestingAuthority.TestingAuthorityName = "(Select Testing Authority)";

            testingAuthorityList.Insert(0, tmpTestingAuthority);

            cmbTestingAuthority.DataSource = testingAuthorityList;
            cmbTestingAuthority.ValueMember = "TestingAuthorityId";
            cmbTestingAuthority.DisplayMember = "TestingAuthorityName";
        }

        private void LoadAttorneys()
        {
            LoadAttornyCombo(cmbAttorneyName1);
            LoadAttornyCombo(cmbAttorneyName2);
            LoadAttornyCombo(cmbAttorneyName3);
        }

        private void LoadThirdParties()
        {
            LoadThirdPartiesCombo(cmbThirdPartyInfo1Name);
            LoadThirdPartiesCombo(cmbThirdPartyInfo2Name);
        }

        private void LoadThirdPartiesCombo(ComboBox cmb)
        {
            ThirdPartyBL thirdPartyBL = new ThirdPartyBL();

            List<ThirdParty> thirdPartyList = thirdPartyBL.GetList(this.currentDonorId);

            ThirdParty tmpThirdParty = new ThirdParty();
            tmpThirdParty.ThirdPartyId = 0;
            tmpThirdParty.ThirdPartyFirstName = "(Select Third Party)";
            thirdPartyList.Insert(0, tmpThirdParty);

            int tmpId = 0;

            if (cmb.SelectedIndex > 0)
            {
                tmpId = Convert.ToInt32(cmb.SelectedValue);
            }

            cmb.DataSource = thirdPartyList;
            cmb.ValueMember = "ThirdPartyId";
            cmb.DisplayMember = "UserDisplayName";

            cmb.SelectedValue = tmpId;
        }

        private bool ThirdPartyActive(ComboBox cmb)
        {
            ThirdPartyBL thirdPartyBL = new ThirdPartyBL();

            int thirdPartyId = 0;

            if (cmb.SelectedIndex > 0)
            {
                thirdPartyId = Convert.ToInt32(cmb.SelectedValue);
                ThirdParty thirdParty = thirdPartyBL.Get(thirdPartyId);
                if (thirdParty.IsActive == false)
                {
                    MessageBox.Show("ThirdParty is inactive. Select some other thirdparty.");
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

        private void LoadAttornyCombo(ComboBox cmb)
        {
            AttorneyBL attorneyBL = new AttorneyBL();

            List<Attorney> attorneyList = attorneyBL.GetList();

            Attorney tmpAttorney = new Attorney();
            tmpAttorney.AttorneyId = 0;
            tmpAttorney.AttorneyFirstName = "(Select Attorney)";
            attorneyList.Insert(0, tmpAttorney);
            cmb.DataSource = attorneyList;
            cmb.ValueMember = "AttorneyId";
            cmb.DisplayMember = "UserDisplayName";

            int tmpId = 0;

            if (cmb.SelectedIndex > 0)
            {
                tmpId = Convert.ToInt32(cmb.SelectedValue);
            }

            cmb.SelectedValue = tmpId;
        }

        private bool AttorneyActive(ComboBox cmb)
        {
            AttorneyBL attorneyBL = new AttorneyBL();

            int attorneyId = 0;

            if (cmb.SelectedIndex > 0)
            {
                attorneyId = Convert.ToInt32(cmb.SelectedValue);
                Attorney attorney = attorneyBL.Get(attorneyId);
                if (attorney.IsActive == false)
                {
                    MessageBox.Show("Attorney is inactive. Select some other attorney.");
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

        private void LoadVendors()
        {
            LoadVendorsCombo(cmbVendor1Name);
            LoadVendorsCombo(cmbVendor2Name);
            LoadVendorsCombo(cmbVendor3Name);
            LoadVendorsCombo(cmbVendor4Name);

            //cmbYear.Items.Clear();

            //var myDate = DateTime.Now;
            //var newDate = myDate.AddYears(125).Year;
            //for (int i = DateTime.Now.Year; i <= newDate; i++)
            //{
            //    cmbYear.Items.Add(i);
            //}
            //cmbYear.Items.Insert(0, "YYYY");
            //cmbMonth.SelectedIndex = 0;
            //cmbDate.SelectedIndex = 0;
            //cmbYear.SelectedIndex = 0;

            //cmbMonth.Text = myDate.ToString("MM");
            //cmbDate.Text = myDate.ToString("dd");
            //cmbYear.Text = myDate.ToString("yyyy");
        }

        private void LoadVendorsCombo(ComboBox cmb)
        {
            VendorBL vendorBL = new VendorBL();
            //Donor donor = donorBL.Get(this.currentDonorId);
            // List<Vendor> vendorList = vendorBL.GetCollectionCenterList();
            List<Vendor> vendorList = vendorBL.GetVendorCollectionCenterList();
            Vendor tmpVendor = new Vendor();
            tmpVendor.VendorId = 0;
            tmpVendor.VendorName = "(Select Vendor)";
            vendorList.Insert(0, tmpVendor);

            cmb.DataSource = vendorList;
            cmb.ValueMember = "VendorId";
            cmb.DisplayMember = "VendorName";
        }

        private bool VendorActive(ComboBox cmb)
        {
            VendorBL vendorBL = new VendorBL();
            int VendorId = 0;

            if (cmb.SelectedIndex > 0)
            {
                VendorId = Convert.ToInt32(cmb.SelectedValue);
                Vendor vendor = vendorBL.Get(VendorId);
                if (vendor.VendorStatus == VendorStatus.Inactive)
                {
                    MessageBox.Show("Vendor is inactive. Select some other vendor.");
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

        private void LoadCourts()
        {
            CourtBL courtBL = new CourtBL();
            List<Court> courtList = courtBL.GetList();
            Court tmpCourt = new Court();
            tmpCourt.CourtId = 0;
            tmpCourt.CourtName = "(Select Court)";

            courtList.Insert(0, tmpCourt);

            cmbCourtName.DataSource = courtList;
            cmbCourtName.ValueMember = "CourtId";
            cmbCourtName.DisplayMember = "CourtName";
        }

        private bool CourtActive(ComboBox cmb)
        {
            CourtBL courtBL = new CourtBL();

            int CourtId = 0;

            if (cmb.SelectedIndex > 0)
            {
                CourtId = Convert.ToInt32(cmb.SelectedValue);
                Court court = courtBL.Get(CourtId);
                if (court.IsActive == false)
                {
                    MessageBox.Show("Court is inactive. Select some other court.");
                    cmb.SelectedIndex = 0;
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return true;
        }

        private void LoadJudges()
        {
            JudgeBL judgeBL = new JudgeBL();
            List<Judge> judgeList = judgeBL.GetList();
            Judge tmpJudge = new Judge();
            tmpJudge.JudgeId = 0;
            tmpJudge.JudgeFirstName = "(Select Judge)";

            judgeList.Insert(0, tmpJudge);

            cmbJudgeName.DataSource = judgeList;
            cmbJudgeName.ValueMember = "JudgeId";
            cmbJudgeName.DisplayMember = "UserDisplayName";
        }

        private bool JudgeActive(ComboBox cmb)
        {
            JudgeBL judgeBL = new JudgeBL();

            int JudgeId = 0;

            if (cmb.SelectedIndex > 0)
            {
                JudgeId = Convert.ToInt32(cmb.SelectedValue);
                Judge judge = judgeBL.Get(JudgeId);
                if (judge.IsActive == false)
                {
                    MessageBox.Show("Judge is inactive. Select some other judge.");
                    cmb.SelectedIndex = 0;
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return true;
        }

        private void LoadClientName()
        {
            ClientBL clientBL = new ClientBL();
            List<Client> clientList = clientBL.GetList("1");

            Client tmpClient = new Client();
            tmpClient.ClientId = 0;
            tmpClient.ClientName = "(Select Client)";

            clientList.Insert(0, tmpClient);

            cmbClient.DataSource = clientList;
            cmbClient.ValueMember = "ClientId";
            cmbClient.DisplayMember = "ClientName";
        }

        private void LoadClientDepartment()
        {
            int clientId = 0;

            if (cmbClient.SelectedIndex > 0)
            {
                clientId = Convert.ToInt32(cmbClient.SelectedValue.ToString());
            }

            List<ClientDepartment> clientDepartmentList = clientBL.GetClientDepartmentList(clientId, "1");

            ClientDepartment tmpclientDepartment = new ClientDepartment();
            tmpclientDepartment.ClientDepartmentId = 0;
            tmpclientDepartment.DepartmentName = "(Select Department)";

            clientDepartmentList.Insert(0, tmpclientDepartment);

            cmbDepartment.DataSource = clientDepartmentList;
            cmbDepartment.ValueMember = "ClientDepartmentId";
            cmbDepartment.DisplayMember = "DepartmentName";
        }

        //private void LoadVendorName()
        //{
        //    VendorBL vendorBL = new VendorBL();
        //    List<Vendor> vendorList = vendorBL.GetList();

        //    Vendor tmpVendor = new Vendor();
        //    tmpVendor.VendorId = 0;
        //    tmpVendor.VendorName = "(Select Vendor)";

        //    vendorList.Insert(0, tmpVendor);

        //    cmbVendor2Name.DataSource = vendorList;
        //    cmbVendor2Name.ValueMember = "VendorId";
        //    cmbVendor2Name.DisplayMember = "VendorName";

        //}

        private void ResetControlsCauseValidation()
        {
            //  Program._logger.Debug("ResetControlsCauseValidation loop start");

            foreach (Control ctrl in this.Controls)
            {

                ctrl.CausesValidation = true;
            }
            //Program._logger.Debug("ResetControlsCauseValidation loop done");
        }

        private bool SaveData()
        {
            try
            {
                //DonorTestInfo donorTestInfo = donorBL.GetDonorTestInfo(this.currentTestInfoId);

                //if (!ValidateDonorDetails())
                //{
                //    return false;
                //}

                //if (chkUrinalysis.Checked || chkHair.Checked || chkDNA.Checked)
                //{
                //    if (!ValidateTestInfoDetails())
                //    {
                //        return false;
                //    }
                //}

                //if (!ValidateActivityDetails())
                //{
                //    return false;
                //}

                //if (!ValidateLegalDetails())
                //{
                //    return false;
                //}

                //if ((rbtnPaymentCash.Checked
                //    || rbtnPaymentCard.Checked
                //    || txtPaymentNote.Text.Trim() != string.Empty)
                //    && (txtPaymentAmount.Text != "0" && txtPaymentAmount.Text != "0.00"))
                //{
                //    if (!ValidatePaymentDetails(donorTestInfo))
                //    {
                //        return false;
                //    }
                //}

                //if (this.currentTestInfoId > 0)
                //{
                //    if (donorTestInfo != null)
                //    {
                //        if ((rbtnPaymentCash.Checked
                //                || rbtnPaymentCard.Checked
                //                || txtPaymentNote.Text.Trim() != string.Empty)
                //                && (txtPaymentAmount.Text != "0" && txtPaymentAmount.Text != "0.00"))
                //        {
                //            SavePaymentDetails(true, donorTestInfo);
                //        }

                //        if (chkUrinalysis.Checked || chkHair.Checked || chkDNA.Checked)
                //        {
                //            if (btnTestInfoSave.Enabled)
                //            {
                //                SaveTestInfoDetails(true, donorTestInfo);
                //            }
                //        }
                //    }
                //}

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void LoadDonorTabDetails(Dictionary<string, string> tabPageList)
        {
            string firstTabTag = string.Empty;
            int index = 0;

            foreach (KeyValuePair<string, string> tabPage in tabPageList)
            {
                int tabIndex = this.AddDonorNamesTab(tabPage.Value, tabPage.Key);

                if (index == 0)
                {
                    index = tabIndex;
                }
            }

            this.LoadTabDetails(index);
        }

        private bool ValidateSSN()
        {
            try
            {
                string SSN = string.Empty;
                if (txtSSN.Text.Length == 9)
                {
                    string NewSSN = txtSSN.Text.Substring(0, 3);
                    string NewSSN1 = txtSSN.Text.Substring(3, 2);
                    string NewSSN2 = txtSSN.Text.Substring(5, 4);
                    SSN = NewSSN + "-" + NewSSN1 + "-" + NewSSN2;
                }
                else
                {
                    SSN = txtSSN.Tag.ToString();
                    // txtSSN.Text = "***-**-" + SSN.ToString();
                }

                DonorBL donorBL = new DonorBL();
                Donor donor = donorBL.GetBySSN(SSN.ToString(), "Desktop");
                if (donor != null)
                {
                    if (currentDonorId != donor.DonorId)
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

        private bool ValidateEmail()
        {
            try
            {
                UserDao userDao = new UserDao();
                // Donor donor = donorBL.GetByEmail(txtEmail.Text.Trim());
                User users = userDao.GetByUsernameOrEmail(txtEmail.Text.Trim());
                if (users != null)
                {
                    if (currentDonorId != users.DonorId)// && users.UserType == Enum.UserType.Donor)
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

        #region DB to UI

        //used for default
        private void LoadDonorDetails()
        {
            Program._logger.Debug("LoadDonorDetails");

            if (this.currentDonorId > 0)
            {
                Program._logger.Debug("calling ResetControls");

                ResetControls();
                Program._logger.Debug("called  ResetControls");
                Donor donor = donorBL.Get(this.currentDonorId, "Desktop");

                if (donor != null)
                {
                    lblTabHeaderFirstName.Text = donor.DonorFirstName;
                    lblTabHeaderMI.Text = donor.DonorMI;
                    lblTabHeaderLastName.Text = donor.DonorLastName;
                    this.chkHideWeb.CheckStateChanged -= new System.EventHandler(this.chkHideWeb_CheckStateChanged);
                    this.chkHideWeb.Checked = donor.IsHiddenWeb;
                    this.chkHideWeb.CheckStateChanged += new System.EventHandler(this.chkHideWeb_CheckStateChanged);

                    if (donor.DonorRegistrationStatusValue.ToDescriptionString() == "Activated")
                    {
                        lblTabHeaderDOB.Text = string.Empty;
                    }
                    else
                    {
                        DateTime lblTime = Convert.ToDateTime(donor.DonorDateOfBirth.ToString("MM/dd/yyyy"));
                        if (lblTime == DateTime.MinValue)
                        {
                            lblTabHeaderDOB.Text = string.Empty;
                        }
                        else
                        {
                            lblTabHeaderDOB.Text = donor.DonorDateOfBirth.ToString("MM/dd/yyyy");
                        }
                    }
                    txtFirstName.Text = donor.DonorFirstName;
                    txtMiddleInitial.Text = donor.DonorMI;
                    txtLastName.Text = donor.DonorLastName;

                    cmbSuffix.Text = donor.DonorSuffix;

                    if (cmbSuffix.SelectedIndex != 0)
                    {
                        txtSuffix.Text = cmbSuffix.Text;
                    }

                    txtEmail.Text = donor.DonorEmail;
                    int count = Regex.Matches(donor.DonorSSN, "-").Count;
                    if (donor.DonorSSN != string.Empty && donor.DonorSSN.Length == 11 && count == 2)
                    {
                        txtSSN.Tag = donor.DonorSSN;
                        txtSSN.Text = "***-**-" + txtSSN.Tag.ToString().Substring(7);
                        //string SSN = txtSSN.Tag.ToString().Substring(7);
                        //txtSSN.Mask = "***-**-" + SSN + "".ToString();
                        //string Mask = txtSSN.Mask;
                        //  txtSSN.Text = Mask.ToString();
                    }
                    else if (donor.DonorSSN.Length == 9)
                    {
                        string NewSSN = donor.DonorSSN.Substring(0, 3);
                        string NewSSN1 = donor.DonorSSN.Substring(3, 2);
                        string NewSSN2 = donor.DonorSSN.Substring(5, 4);
                        string Unmask = NewSSN + "-" + NewSSN1 + "-" + NewSSN2;
                        txtSSN.Text = "***-**-" + Unmask.ToString().Substring(7);
                        txtSSN.Tag = Unmask.ToString();
                    }

                    //  btnUnmaskSSN.BackColor = Color.White;
                    pbUnmaskSSN.Enabled = true;
                    pbUnmaskSSN.Image = global::SurPath.Properties.Resources.unmask_SSN;

                    DateTime inActiveDate = Convert.ToDateTime(donor.DonorDateOfBirth.ToString("MM/dd/yyyy"));
                    if (inActiveDate == DateTime.MinValue)
                    {
                        cmbDonorMonth.SelectedIndex = 0;
                        cmbDonorDate.SelectedIndex = 0;
                        cmbDonorYear.SelectedIndex = 0;

                        cmbDonorMonth.Visible = true;
                        cmbDonorDate.Visible = true;
                        cmbDonorYear.Visible = true;
                        txtDOB.Visible = false;
                    }
                    else
                    {
                        cmbDonorMonth.Text = inActiveDate.ToString("MM");
                        cmbDonorDate.Text = inActiveDate.ToString("dd");
                        cmbDonorYear.Text = inActiveDate.ToString("yyyy");

                        txtDOB.Text = inActiveDate.ToString("MM/dd/yyyy");
                    }

                    if (donor.DonorGender == Enum.Gender.Female)
                    {
                        rbtnFemale.Checked = true;
                    }
                    else if (donor.DonorGender == Enum.Gender.Male)
                    {
                        rbtnMale.Checked = true;
                    }

                    txtAddress1.Text = donor.DonorAddress1;
                    txtAddress2.Text = donor.DonorAddress2;
                    txtCity.Text = donor.DonorCity;

                    cmbState.Text = donor.DonorState;
                    txtState.Text = donor.DonorState;
                    txtClearStarProfileId.Text = donor.DonorClearStarProfId;

                    txtZipCode.Text = donor.DonorZip;

                    zipcode = donor.DonorZip;
                    LoadVendors();

                    txtPhone1.Text = donor.DonorPhone1;
                    txtPhone2.Text = donor.DonorPhone2;

                    if (this.currentTestInfoId == 0)
                    {
                        cmbClient.SelectedValue = donor.DonorInitialClientId;
                        LoadClientDepartment();
                        cmbDepartment.SelectedValue = donor.DonorInitialDepartmentId;

                        if (cmbClient.SelectedIndex > 0)
                        {
                            lblTabHeaderClient.Text = cmbClient.Text.Trim();
                            txtClient.Text = cmbClient.Text;
                            txtClient.Visible = true;
                            //cmbClient.Visible = false;
                        }

                        if (cmbDepartment.SelectedIndex > 0)
                        {
                            lblTabHeaderDepartment.Text = cmbDepartment.Text.Trim();
                            txtDepartment.Text = cmbDepartment.Text;
                            txtDepartment.Visible = true;
                            //cmbDepartment.Visible = false;
                        }
                    }

                    if (donor.DonorRegistrationStatusValue == Enum.DonorRegistrationStatus.PreRegistration)
                    {
                        lblStatus.Text = "Pre-Registered";
                    }
                    else
                    {
                        lblStatus.Text = donor.DonorRegistrationStatusValue.ToDescriptionString();
                    }

                    //btnActivationMailSend.Visible = false;

                    UserBL userBL = new UserBL();
                    User user = userBL.GetDonor(this.currentDonorId);
                    if (user != null)
                    {
                        if ((donor.DonorRegistrationStatusValue == Enum.DonorRegistrationStatus.PreRegistration && user.ChangePasswordRequired == true) || (donor.DonorRegistrationStatusValue == Enum.DonorRegistrationStatus.Registered && user.ChangePasswordRequired == true))
                        {
                            if (inActiveDate == DateTime.MinValue)
                            {
                                lblTabHeaderDOB.Text = string.Empty;
                            }
                            else
                            {
                                lblTabHeaderDOB.Text = donor.DonorDateOfBirth.ToString("MM/dd/yyyy");
                            }

                            DataTable dtGetTestInfoCount = donorBL.GetMoreThanTestInfoList(this.currentDonorId);
                            if (dtGetTestInfoCount.Rows.Count <= 1)
                            {
                                if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                                {
                                    //DONOR_ACTIVATION_MAIL_RESEND
                                    DataRow[] activationMailResend = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DONOR_ACTIVATION_MAIL_RESEND.ToDescriptionString() + "'");

                                    if (activationMailResend.Length > 0)
                                    {
                                        btnActivationMailSend.Visible = true;

                                        if (!btnDonorSave.Visible)
                                        {
                                            btnActivationMailSend.Location = new Point(547, 388);
                                            btnDonorSave.Location = new Point(722, 388);
                                        }
                                        else
                                        {
                                            btnActivationMailSend.Location = new Point(547, 388);
                                        }
                                    }
                                }
                                else
                                {
                                    btnActivationMailSend.Visible = true;

                                    if (!btnDonorSave.Visible)
                                    {
                                        btnActivationMailSend.Location = new Point(547, 388);
                                        btnDonorSave.Location = new Point(722, 388);
                                    }
                                    else
                                    {
                                        btnActivationMailSend.Location = new Point(547, 388);
                                    }
                                }
                            }
                            else
                            {
                                //btnActivationMailSend.Visible = false;
                            }
                        }
                    }
                }
                if (this.currentTestInfoId > 0)
                {
                    DonorTestInfo donorTestInfo = donorBL.GetDonorTestInfo(this.currentTestInfoId);
                    if (donorTestInfo != null)
                    {
                        LoadPaymentDetails(donorTestInfo);
                        LoadTestInfoDetails(donorTestInfo);
                        LoadResultDetails(donorTestInfo);
                        LoadActivityDetails();
                        LoadLegalDetails(donorTestInfo);
                        LoadVendorDetails(donorTestInfo);
                        // Get Notification Data
                        LoadNotificationData(donorTestInfo);
                    }
                }
                else
                {
                    chkUrinalysis.Checked = true;
                    lbldonors.Visible = false;
                }

                LoadActivityDetails();
                LoadDocumentDetails();
                LoadAccountDetails(this.currentDonorId, this.currentTestInfoId);
                LoadTestHistoryDetails();

                //tcMain.SelectedTab = tabDonorInfo;
            }
            Program._logger.Debug("calling ResetControlsCauseValidation 2");
            ResetControlsCauseValidation();
            Program._logger.Debug("called ResetControlsCauseValidation 2");
        }

        //used to tell what type of report

        /// <summary>
        ///  Get notification information for DonorTestInfo object
        /// </summary>
        /// <param name="donorTestInfo"></param>
        private void LoadNotificationData(DonorTestInfo donorTestInfo)
        {
            notification = backendLogic.GetDonorNotification(this.currentTestInfoId);

            // No notification record, maybe legacy data
            btnSendNotification.Visible = notification.backend_notifications_id != 0;
            lblNotified.Visible = notification.backend_notifications_id != 0;
            string notificationSummary = string.Empty;

            if (notification.notified_by_email)
            {
                notificationSummary += $"Notified by email date: {notification.notified_by_email_timestamp}";
            }
            else if (notification.notification_email_exception > 0)
            {
                notificationSummary += $"Notification by email problem: {notification.notify_email_exception_timestamp}";
            }
            else
            {
                notificationSummary += "Not yet notified by email";
            }
            notificationSummary += " \t ";
            if (notification.notified_by_sms)
            {
                notificationSummary += $"Notified by sms date: {notification.notified_by_sms_timestamp}";
            }
            else if (notification.notification_sms_exception > 0)
            {
                notificationSummary += $"Notification by sms problem: {notification.notify_sms_exception_timestamp}";
            }
            else
            {
                notificationSummary += "Not yet notified by sms";
            }
            //if (notification.notified_by_email)
            //{
            //    btnSendNotification.Text = "Resend Notification";
            //}
            lblNotified.Text = notificationSummary;
            if (notification.notification_email_exception > 0)
            {
                lblNotified.ForeColor = Color.DarkRed;
            }
            else
            {
                lblNotified.ForeColor = SystemColors.ControlText;
            }
        }

        private void LoadTestInfoDetails(DonorTestInfo donorTestInfo)
        {
            //Donor Info

            if (donorTestInfo.IsWalkinDonor)
            {
                lbldonors.Visible = true;
                lbldonors.Text = "WALK-IN";
            }
            else
            {
                lbldonors.Visible = false;
            }

            if (donorTestInfo.PaymentStatus == PaymentStatus.Paid)
            {
                lblinfo.Visible = false;
            }
            else
            {
                lblinfo.Visible = true;
            }

            //Tab Header
            if (donorTestInfo.ScreeningTime != null)
            {
                lblTabHeaderSpecimenDate.Text = Convert.ToDateTime(donorTestInfo.ScreeningTime).ToString("MM/dd/yyyy");
            }

            //Donor Info Tab
            cmbClient.SelectedValue = donorTestInfo.ClientId;
            LoadClientDepartment();
            cmbDepartment.SelectedValue = donorTestInfo.ClientDepartmentId;

            if (cmbClient.SelectedIndex > 0)
            {
                lblTabHeaderClient.Text = cmbClient.Text.Trim();
                txtClient.Text = cmbClient.Text;
                txtClient.Visible = true;
                //cmbClient.Visible = false;
            }

            if (cmbDepartment.SelectedIndex > 0)
            {
                lblTabHeaderDepartment.Text = cmbDepartment.Text.Trim();
                txtDepartment.Text = cmbDepartment.Text;
                txtDepartment.Visible = true;
                //cmbDepartment.Visible = false;
            }

            lblStatus.Text = donorTestInfo.TestStatus.ToDescriptionString();

            //Test Info Tab

            if (donorTestInfo.IsWalkinDonor)
            {
                lblDonor.Visible = true;
                lblDonor.Text = "WALK-IN";
            }
            else
            {
                lblDonor.Visible = false;
            }
            //Instant Test

            if (donorTestInfo.IsInstantTest)
            {
                chkInstant.Checked = true;
                pnlTest.Visible = true;
            }
            else
            {
                chkInstant.Checked = false;
                chkInstant.Enabled = true;
                pnlTest.Visible = false;
            }

            //Instant Test Result
            if (donorTestInfo.InstantTestResult == InstantTestResult.Positive)
            {
                rbPositive.Checked = true;
                txtLabOverallResults.Text = string.Empty;
            }
            if (donorTestInfo.InstantTestResult == InstantTestResult.Negative)
            {
                rbNegative.Checked = true;
                txtLabOverallResults.Text = "NEGATIVE";
            }
            if (donorTestInfo.InstantTestResult == InstantTestResult.None)
            {
                rbPositive.Checked = false;
                rbNegative.Checked = false;
                txtLabOverallResults.Text = string.Empty;
            }
            ClientDepartment clientDepartment = clientBL.GetClientDepartment(donorTestInfo.ClientDepartmentId);
            Client client = clientBL.Get(donorTestInfo.ClientId);
            if (client.CanEditTestCategory == true)
            {
                foreach (DonorTestInfoTestCategories testCategory in donorTestInfo.TestInfoTestCategories)
                {
                    if (testCategory.TestCategoryId == TestCategories.UA)
                    {
                        chkUrinalysis.Enabled = true;
                        chkUrinalysis.Checked = true;

                        foreach (ClientDeptTestCategory testCategoryItem in clientDepartment.ClientDeptTestCategories)
                        {
                            if (testCategoryItem.TestCategoryId == TestCategories.UA)
                            {
                                TestPanelBL testPanelBL = new TestPanelBL();
                                List<TestPanel> testPanelUAList = testPanelBL.GetListByUA();
                                cmbUATestPanel.DataSource = testPanelUAList;
                                cmbUATestPanel.ValueMember = "TestPanelId";
                                cmbUATestPanel.DisplayMember = "TestPanelName";

                                break;
                            }
                        }

                        if (testCategory.TestPanelId != null)
                        {
                            cmbUATestPanel.SelectedValue = testCategory.TestPanelId;
                        }

                        txtUASpecimenId.Text = testCategory.SpecimenId;
                        UASpecimenId = testCategory.SpecimenId;
                    }

                    if (testCategory.TestCategoryId == TestCategories.Hair)
                    {
                        chkHair.Enabled = true;
                        chkHair.Checked = true;

                        foreach (ClientDeptTestCategory testCategoryItem in clientDepartment.ClientDeptTestCategories)
                        {
                            if (testCategoryItem.TestCategoryId == TestCategories.Hair)
                            {
                                TestPanelBL testPanelBL = new TestPanelBL();
                                List<TestPanel> testPanelHairList = testPanelBL.GetListByHair();
                                cmbHairTestPanel.DataSource = testPanelHairList;
                                cmbHairTestPanel.ValueMember = "TestPanelId";
                                cmbHairTestPanel.DisplayMember = "TestPanelName";

                                break;
                            }
                        }

                        if (testCategory.TestPanelId != null)
                        {
                            cmbHairTestPanel.SelectedValue = testCategory.TestPanelId;
                        }

                        if (testCategory.HairTestPanelDays != null)
                        {
                            cmbDays.SelectedIndex = Convert.ToInt32(testCategory.HairTestPanelDays) / 90;

                            if (donorTestInfo.PaymentStatus != PaymentStatus.Paid && Convert.ToInt32(donorTestInfo.TestStatus) < 6)
                            {
                                cmbDays.Enabled = true;
                            }
                            else
                            {
                                cmbDays.Enabled = false;
                            }
                        }

                        txtHairSpecimenId.Text = testCategory.SpecimenId;
                    }
                    if (testCategory.TestCategoryId == TestCategories.DNA)
                    {
                        chkDNA.Enabled = true;
                        chkDNA.Checked = true;
                    }
                    if (testCategory.TestCategoryId == TestCategories.BC)
                    {
                        //chkBC.Enabled = true;
                        //chkBC.Checked = true;
                    }
                }
            }
            if (client.CanEditTestCategory == false)
            {
                foreach (DonorTestInfoTestCategories testCategory in donorTestInfo.TestInfoTestCategories)
                {
                    if (testCategory.TestCategoryId == TestCategories.UA)
                    {
                        chkUrinalysis.Enabled = false;
                        chkUrinalysis.Checked = true;

                        foreach (ClientDeptTestCategory testCategoryItem in clientDepartment.ClientDeptTestCategories)
                        {
                            if (testCategoryItem.TestCategoryId == TestCategories.UA)
                            {
                                TestPanelBL testPanelBL = new TestPanelBL();
                                List<TestPanel> testPanelUAList = testPanelBL.GetListByUA();
                                cmbUATestPanel.DataSource = testPanelUAList;
                                cmbUATestPanel.ValueMember = "TestPanelId";
                                cmbUATestPanel.DisplayMember = "TestPanelName";

                                break;
                            }
                        }

                        if (testCategory.TestPanelId != null)
                        {
                            cmbUATestPanel.SelectedValue = testCategory.TestPanelId;
                        }

                        txtUASpecimenId.Text = testCategory.SpecimenId;
                        UASpecimenId = testCategory.SpecimenId;
                    }

                    if (testCategory.TestCategoryId == TestCategories.Hair)
                    {
                        chkHair.Enabled = false;
                        chkHair.Checked = true;

                        foreach (ClientDeptTestCategory testCategoryItem in clientDepartment.ClientDeptTestCategories)
                        {
                            if (testCategoryItem.TestCategoryId == TestCategories.Hair)
                            {
                                TestPanelBL testPanelBL = new TestPanelBL();
                                List<TestPanel> testPanelHairList = testPanelBL.GetListByHair();
                                cmbHairTestPanel.DataSource = testPanelHairList;
                                cmbHairTestPanel.ValueMember = "TestPanelId";
                                cmbHairTestPanel.DisplayMember = "TestPanelName";

                                break;
                            }
                        }

                        if (testCategory.TestPanelId != null)
                        {
                            cmbHairTestPanel.SelectedValue = testCategory.TestPanelId;
                        }

                        if (testCategory.HairTestPanelDays != null)
                        {
                            cmbDays.SelectedIndex = Convert.ToInt32(testCategory.HairTestPanelDays) / 90;

                            if (donorTestInfo.PaymentStatus != PaymentStatus.Paid && Convert.ToInt32(donorTestInfo.TestStatus) < 6)
                            {
                                cmbDays.Enabled = true;
                            }
                            else
                            {
                                cmbDays.Enabled = false;
                            }
                        }

                        txtHairSpecimenId.Text = testCategory.SpecimenId;
                    }
                    if (testCategory.TestCategoryId == TestCategories.DNA)
                    {
                        chkDNA.Enabled = false;
                        chkDNA.Checked = true;
                    }
                }
            }

            if (chkUrinalysis.Enabled && donorTestInfo.IsUA)
            {
                chkUrinalysis.Checked = true;
            }

            if (chkHair.Enabled && donorTestInfo.IsHair)
            {
                chkHair.Checked = true;
            }

            if (chkDNA.Enabled && donorTestInfo.IsDNA)
            {
                chkDNA.Checked = true;
            }
            if (chkBC.Enabled && donorTestInfo.IsBC)
            {
                chkBC.Checked = true;
            }

            if (donorTestInfo.IsInstantTest)
            {
                chkInstant.Checked = true;
            }
            if (testflag == true)
            {
                if (donorTestInfo.IsUA == false)
                {
                    chkUrinalysis.Enabled = false;
                }
                if (donorTestInfo.IsHair == false)
                {
                    chkHair.Enabled = false;
                }
                if (donorTestInfo.IsDNA == false)
                {
                    chkDNA.Enabled = false;
                }
                if (donorTestInfo.IsBC == false)
                {
                    chkBC.Enabled = false;
                }
            }

            //Reason for test
            if (donorTestInfo.ReasonForTestId != TestInfoReasonForTest.None)
            {
                if (donorTestInfo.ReasonForTestId == TestInfoReasonForTest.PreEmployment)
                {
                    rbPreEmployment.Checked = true;
                }
                else if (donorTestInfo.ReasonForTestId == TestInfoReasonForTest.Random)
                {
                    rbRandom.Checked = true;
                }
                else if (donorTestInfo.ReasonForTestId == TestInfoReasonForTest.ReasonableSuspicionCause)
                {
                    rbReasonable.Checked = true;
                }
                else if (donorTestInfo.ReasonForTestId == TestInfoReasonForTest.PostAccident)
                {
                    rbPostAccident.Checked = true;
                }
                else if (donorTestInfo.ReasonForTestId == TestInfoReasonForTest.ReturnToDuty)
                {
                    rbReturntoDuty.Checked = true;
                }
                else if (donorTestInfo.ReasonForTestId == TestInfoReasonForTest.FollowUp)
                {
                    rbFollowUp.Checked = true;
                }
                else if (donorTestInfo.ReasonForTestId == TestInfoReasonForTest.Other)
                {
                    rbOther.Checked = true;

                    txtReason.Visible = true;
                    txtReason.Text = donorTestInfo.OtherReason;
                }
            }

            //Specimen Cup
            if (donorTestInfo.SpecimenCollectionCupId != SpecimenCollectionCupType.None)
            {
                if (donorTestInfo.SpecimenCollectionCupId == SpecimenCollectionCupType.UrineSingle)
                {
                    rbUrineSingle.Checked = true;
                }
                else if (donorTestInfo.SpecimenCollectionCupId == SpecimenCollectionCupType.UrineSplit)
                {
                    rbUrineSplit.Checked = true;
                }
                else if (donorTestInfo.SpecimenCollectionCupId == SpecimenCollectionCupType.Saliva)
                {
                    rbSaliva.Checked = true;
                }
                else if (donorTestInfo.SpecimenCollectionCupId == SpecimenCollectionCupType.Blood)
                {
                    rbBlood.Checked = true;
                }
            }

            //Observed
            if (donorTestInfo.IsObserved != YesNo.None)
            {
                if (donorTestInfo.IsObserved == YesNo.Yes)
                {
                    rbObservedYes.Checked = true;
                }
                else if (donorTestInfo.IsObserved == YesNo.No)
                {
                    rbObservedNo.Checked = true;
                }
            }

            //Form Type
            if (donorTestInfo.FormTypeId != SpecimenFormType.None)
            {
                if (donorTestInfo.FormTypeId == SpecimenFormType.Federal)
                {
                    rbFederal.Checked = true;
                    LoadTestingAuthorityName("1");
                    cmbTestingAuthority.SelectedValue = donorTestInfo.TestingAuthorityId;
                }
                else if (donorTestInfo.FormTypeId == SpecimenFormType.NonFederal)
                {
                    rbNonFederal.Checked = true;
                }
            }

            //Temperature
            if (donorTestInfo.IsTemperatureInRange != YesNo.None)
            {
                if (donorTestInfo.IsTemperatureInRange == YesNo.Yes)
                {
                    rbTemperatureYes.Checked = true;
                }
                else if (donorTestInfo.IsTemperatureInRange == YesNo.No)
                {
                    rbTemperatureNo.Checked = true;
                }

                if (donorTestInfo.TemperatureOfSpecimen != null)
                {
                    txtTemperature.Text = donorTestInfo.TemperatureOfSpecimen.ToString();
                }
            }

            //Adulteration
            if (donorTestInfo.IsAdulterationSign != YesNo.None)
            {
                if (donorTestInfo.IsAdulterationSign == YesNo.Yes)
                {
                    rbAdulterationYes.Checked = true;
                }
                else if (donorTestInfo.IsAdulterationSign == YesNo.No)
                {
                    rbAdulterationNo.Checked = true;
                }
            }

            //QNS
            if (donorTestInfo.IsQuantitySufficient != YesNo.None)
            {
                if (donorTestInfo.IsQuantitySufficient == YesNo.Yes)
                {
                    rbQNSYes.Checked = true;
                }
                else if (donorTestInfo.IsQuantitySufficient == YesNo.No)
                {
                    rbQNSNo.Checked = true;
                }
            }

            //Donor Refueses
            if (donorTestInfo.IsDonorRefused != null)
            {
                chkDonorRefuses.Checked = (bool)donorTestInfo.IsDonorRefused;
            }

            //Collection Site Information
            int collectionSiteUserId = Program.currentUserId;

            if (donorTestInfo.CollectionSiteUserId != null)
            {
                collectionSiteUserId = Convert.ToInt32(donorTestInfo.CollectionSiteUserId);
            }

            UserBL userBL = new UserBL();
            User user = userBL.Get(collectionSiteUserId);
            if ((Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName == Program.superAdmin1 || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()) && (donorTestInfo.TestStatus == Enum.DonorRegistrationStatus.Processing || donorTestInfo.TestStatus == Enum.DonorRegistrationStatus.SuspensionQueue))
            {
                txtCollectorName.Text = user.UserFirstName + " " + user.UserLastName;
            }
            if (user != null)
            {
                if (user.UserType == UserType.Vendor)
                {
                    VendorBL vendorBL = new VendorBL();
                    Vendor vendor = vendorBL.Get(Convert.ToInt32(user.VendorId));
                    txtCollectorName.Text = user.UserFirstName + " " + user.UserLastName;
                    txtLocationName.Text = vendor.VendorName;
                    foreach (VendorAddress address in vendor.Addresses)
                    {
                        if (address.AddressTypeId == AddressTypes.PhysicalAddress1)
                        {
                            txtCollectionAddress1.Text = address.Address1;
                            txtCollectionAddress2.Text = address.Address2;
                            txtCollectionCity.Text = address.City;
                            txtCollectionState.Text = address.State;
                            txtCollectionZipCode.Text = address.ZipCode;
                            txtCollectionPhone.Text = address.Phone;
                            txtCollectionFax.Text = address.Fax;
                            txtCollectionEmail.Text = address.Email;

                            break;
                        }
                    }
                }
                else if (user.UserType == UserType.TPA)
                {
                    // txtLocationName.Text = "TPA";
                }
            }
            if (donorTestInfo.ScreeningTime != null && donorTestInfo.ScreeningTime != DateTime.MinValue)
            {
                txtScreeningDate.Text = Convert.ToDateTime(donorTestInfo.ScreeningTime).ToString("MM/dd/yyyy");
                txtScreeningTime.Text = Convert.ToDateTime(donorTestInfo.ScreeningTime).ToString("hh:mm tt");
            }
            else
            {
                //txtScreeningDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                //txtScreeningTime.Text = DateTime.Now.ToString("hh:mm tt");
            }

            //  DataTable dtReverseEntry = donorBL.GetIsReverseEntry(currentDonorId,currentTestInfoId);
            //
            if (!(donorTestInfo.TestStatus == DonorRegistrationStatus.Registered
                || donorTestInfo.TestStatus == DonorRegistrationStatus.InQueue
                || donorTestInfo.TestStatus == DonorRegistrationStatus.SuspensionQueue))
            {
                if (donorTestInfo.IsReverseEntry == true)
                {
                    //Payment
                    //if (donorTestInfo.PaymentStatus == PaymentStatus.None || donorTestInfo.PaymentStatus == PaymentStatus.Pending)
                    //{
                    //    // lblpaymsg.Visible = false;

                    //    if (donorTestInfo.PaymentMethodId == PaymentMethod.None)
                    //    {
                    //        rbtnPaymentCash.Checked = true;
                    //    }
                    //    if (donorTestInfo.PaymentTypeId == ClientPaymentTypes.InvoiceClient)
                    //    {
                    //        chkinvoiceclient.Checked = true;
                    //    }
                    //    else
                    //    {
                    //        chkinvoiceclient.Checked = false;
                    //    }

                    //    if (donorTestInfo.IsPaymentReceived == true)
                    //    {
                    //        chkPayment.Checked = true;
                    //    }
                    //    else
                    //    {
                    //        chkPayment.Checked = false;
                    //    }
                    //    //   txtPaymentNote.Text = donorTestInfo.PaymentNote;
                    //    if (SaveReversePaymentDetails(false, donorTestInfo))
                    //    {
                    //        // LoadTestInfoDetails(donorTestInfo);
                    //        if (donorTestInfo.PaymentStatus == PaymentStatus.Paid)
                    //        {
                    //            lblinfo.Visible = false;
                    //        }
                    //        LoadPaymentDetails(donorTestInfo);
                    //        LoadAccountDetails(this.currentDonorId, this.currentTestInfoId);
                    //    }
                    //}

                    chkUrinalysis.Enabled = false;
                    chkHair.Enabled = false;
                    chkDNA.Enabled = false;
                    pnlReason.Enabled = true;
                    cmbTestingAuthority.Enabled = false;
                    if (rbFederal.Checked == true)
                    {
                        cmbTestingAuthority.Visible = true;
                        lblTestingAuthority.Visible = true;
                        lblTestingAuthorityMan.Visible = true;
                        cmbTestingAuthority.Enabled = true;
                    }
                    else
                    {
                        cmbTestingAuthority.Visible = false;
                        lblTestingAuthority.Visible = false;
                        lblTestingAuthorityMan.Visible = false;
                    }
                    txtUASpecimenId.ReadOnly = true;
                    txtHairSpecimenId.ReadOnly = true;
                    pnlCup.Enabled = true;
                    pnlObserved.Enabled = true;
                    pnlFormType.Enabled = true;
                    pnlTemprature.Enabled = true;
                    pnlAdulteration.Enabled = true;
                    pnlQNS.Enabled = true;
                    chkDonorRefuses.Enabled = false;
                    chkInstant.Enabled = false;
                    rbPositive.Enabled = false;
                    rbNegative.Enabled = false;
                    lblLocationMan.Visible = true;
                    lblCollectorMan.Visible = true;
                    bool specimenEditFlag1 = false;

                    if (!specimenEditFlag1)
                    {
                        btnTestInfoSave.Enabled = true;
                    }
                    else
                    {
                        btnTestInfoSave.Enabled = false;
                    }

                    if (chkUrinalysis.Checked == true && chkDonorRefuses.Checked == false)
                    {
                        lblUASpecimenId.Visible = true;
                        lblUASpecimenMan.Visible = true;
                        txtUASpecimenId.Visible = true;

                        if (!specimenEditFlag1)
                        {
                            lblUASpecimenId.Enabled = true;
                            lblUASpecimenMan.Enabled = true;
                            txtUASpecimenId.Enabled = true;
                            txtUASpecimenId.ReadOnly = true;
                        }
                    }
                    if (chkHair.Checked == true && chkDonorRefuses.Checked == false)
                    {
                        lblHairSpecimenId.Visible = true;
                        lblHairSpecimenMan.Visible = true;
                        txtHairSpecimenId.Visible = true;

                        if (!specimenEditFlag1)
                        {
                            lblHairSpecimenId.Enabled = true;
                            lblHairSpecimenMan.Enabled = true;
                            txtHairSpecimenId.Enabled = true;
                            txtHairSpecimenId.ReadOnly = true;
                        }
                    }

                    cmbLocationName.Visible = true;
                    cmbCollectionName.Visible = true;
                    txtLocationName.Visible = false;
                    txtCollectorName.Visible = false;
                    LoadVendorsCombo(cmbLocationName);
                }
                else if (donorTestInfo.IsReverseEntry == false)
                {
                    cmbLocationName.Visible = false;
                    cmbCollectionName.Visible = false;
                    txtLocationName.Visible = true;
                    txtCollectorName.Visible = true;
                    chkUrinalysis.Enabled = false;
                    chkHair.Enabled = false;
                    chkDNA.Enabled = false;
                    pnlReason.Enabled = false;
                    cmbTestingAuthority.Enabled = false;
                    if (rbFederal.Checked == true)
                    {
                        cmbTestingAuthority.Visible = true;
                        lblTestingAuthority.Visible = true;
                        lblTestingAuthorityMan.Visible = true;
                    }
                    else
                    {
                        cmbTestingAuthority.Visible = false;
                        lblTestingAuthority.Visible = false;
                        lblTestingAuthorityMan.Visible = false;
                    }
                    txtUASpecimenId.ReadOnly = true;
                    txtHairSpecimenId.ReadOnly = true;
                    pnlCup.Enabled = false;
                    pnlObserved.Enabled = false;
                    pnlFormType.Enabled = false;
                    pnlTemprature.Enabled = false;
                    pnlAdulteration.Enabled = false;
                    pnlQNS.Enabled = false;
                    chkDonorRefuses.Enabled = false;
                    chkInstant.Enabled = false;
                    rbPositive.Enabled = false;
                    rbNegative.Enabled = false;

                    bool specimenEditFlag = false;

                    if (!specimenEditFlag)
                    {
                        btnTestInfoSave.Enabled = true;
                    }
                    else
                    {
                        btnTestInfoSave.Enabled = false;
                    }

                    if (chkUrinalysis.Checked == true && chkDonorRefuses.Checked == false)
                    {
                        lblUASpecimenId.Visible = true;
                        lblUASpecimenMan.Visible = true;
                        txtUASpecimenId.Visible = true;

                        if (!specimenEditFlag)
                        {
                            lblUASpecimenId.Enabled = true;
                            lblUASpecimenMan.Enabled = true;
                            txtUASpecimenId.Enabled = true;
                            txtUASpecimenId.ReadOnly = false;
                        }
                    }

                    if (chkHair.Checked == true && chkDonorRefuses.Checked == false)
                    {
                        lblHairSpecimenId.Visible = true;
                        lblHairSpecimenMan.Visible = true;
                        txtHairSpecimenId.Visible = true;

                        if (!specimenEditFlag)
                        {
                            lblHairSpecimenId.Enabled = true;
                            lblHairSpecimenMan.Enabled = true;
                            txtHairSpecimenId.Enabled = true;
                            txtHairSpecimenId.ReadOnly = false;
                        }
                    }
                }
                if (donorTestInfo.TestStatus == Enum.DonorRegistrationStatus.Processing || donorTestInfo.TestStatus == Enum.DonorRegistrationStatus.Completed)
                {
                    //btnActivationMailSend.Visible = false;
                }
            }
            else
            {
                txtUASpecimenId.ReadOnly = false;
                txtHairSpecimenId.ReadOnly = false;
                chkDonorRefuses.Enabled = true;
                btnTestInfoSave.Enabled = true;
            }
            if (donorTestInfo.PaymentStatus == PaymentStatus.Paid)
            {
                chkUrinalysis.Enabled = false;
                chkHair.Enabled = false;
                chkDNA.Enabled = false;
            }
            if (chkHair.Checked == false)
            {
                cmbDays.SelectedIndex = 0;
                cmbDays.Enabled = false;
            }

            if (donorTestInfo.TestStatus == Enum.DonorRegistrationStatus.InQueue)
            {
                //btnActivationMailSend.Visible = false;
            }

            if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
            {
                //DONOR_VIEW_TEST_INFO_TAB
                DataRow[] testInfoTab = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DONOR_VIEW_TEST_INFO_TAB.ToDescriptionString() + "'");

                if (testInfoTab.Length > 0)
                {
                    chkUrinalysis.Enabled = false;
                    cmbUATestPanel.Enabled = false;

                    chkHair.Enabled = false;
                    cmbHairTestPanel.Enabled = false;
                    cmbDays.Enabled = false;

                    chkDNA.Enabled = false;

                    chkInstant.Enabled = false;

                    pnlReason.Enabled = false;
                    pnlCup.Enabled = false;
                    pnlObserved.Enabled = false;
                    pnlFormType.Enabled = false;
                    pnlTemprature.Enabled = false;
                    pnlAdulteration.Enabled = false;
                    pnlQNS.Enabled = false;
                    chkDonorRefuses.Enabled = false;
                    btnTestInfoSave.Enabled = false;

                    txtUASpecimenId.Enabled = false;
                    txtHairSpecimenId.Enabled = false;
                    cmbTestingAuthority.Enabled = false;
                }

                //DONOR_EDIT_TEST_INFO_TAB
                DataRow[] testInfoEdit = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DONOR_EDIT_TEST_INFO_TAB.ToDescriptionString() + "'");

                if (testInfoEdit.Length > 0)
                {
                    if (!(donorTestInfo.TestStatus == DonorRegistrationStatus.Registered
               || donorTestInfo.TestStatus == DonorRegistrationStatus.InQueue
               || donorTestInfo.TestStatus == DonorRegistrationStatus.SuspensionQueue))
                    {
                        chkUrinalysis.Enabled = false;
                        chkHair.Enabled = false;
                        chkDNA.Enabled = false;
                        pnlReason.Enabled = false;
                        cmbTestingAuthority.Enabled = false;
                        if (rbFederal.Checked == true)
                        {
                            cmbTestingAuthority.Visible = true;
                            lblTestingAuthority.Visible = true;
                            lblTestingAuthorityMan.Visible = true;
                        }
                        else
                        {
                            cmbTestingAuthority.Visible = false;
                            lblTestingAuthority.Visible = false;
                            lblTestingAuthorityMan.Visible = false;
                        }
                        txtUASpecimenId.ReadOnly = true;
                        txtHairSpecimenId.ReadOnly = true;
                        pnlCup.Enabled = false;
                        pnlObserved.Enabled = false;
                        pnlFormType.Enabled = false;
                        pnlTemprature.Enabled = false;
                        pnlAdulteration.Enabled = false;
                        pnlQNS.Enabled = false;
                        chkDonorRefuses.Enabled = false;
                        chkInstant.Enabled = false;
                        rbPositive.Enabled = false;
                        rbNegative.Enabled = false;

                        bool specimenEditFlag = false;

                        if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                        {
                            //DONOR_EDIT_SPECIMEN_ID
                            DataRow[] specimenEdit = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DONOR_EDIT_SPECIMEN_ID.ToDescriptionString() + "'");

                            if (specimenEdit.Length > 0)
                            {
                                specimenEditFlag = true;
                            }
                        }
                        else
                        {
                            specimenEditFlag = true;
                        }

                        if (specimenEditFlag)
                        {
                            btnTestInfoSave.Enabled = true;
                        }
                        else
                        {
                            btnTestInfoSave.Enabled = false;
                        }

                        if (chkUrinalysis.Checked)
                        {
                            lblUASpecimenId.Visible = true;
                            lblUASpecimenMan.Visible = true;
                            txtUASpecimenId.Visible = true;

                            if (specimenEditFlag)
                            {
                                lblUASpecimenId.Enabled = true;
                                lblUASpecimenMan.Enabled = true;
                                txtUASpecimenId.Enabled = true;
                                txtUASpecimenId.ReadOnly = false;
                            }
                        }

                        if (chkHair.Checked)
                        {
                            lblHairSpecimenId.Visible = true;
                            lblHairSpecimenMan.Visible = true;
                            txtHairSpecimenId.Visible = true;

                            if (specimenEditFlag)
                            {
                                lblHairSpecimenId.Enabled = true;
                                lblHairSpecimenMan.Enabled = true;
                                txtHairSpecimenId.Enabled = true;
                                txtHairSpecimenId.ReadOnly = false;
                            }
                        }
                    }
                    else
                    {
                        //clientDepartment = clientBL.GetClientDepartment(donorTestInfo.ClientDepartmentId);
                        //client = clientBL.Get(donorTestInfo.ClientId);
                        if (client.CanEditTestCategory == true)
                        {
                            foreach (DonorTestInfoTestCategories testCategory in donorTestInfo.TestInfoTestCategories)
                            {
                                if (testCategory.TestCategoryId == TestCategories.UA)
                                {
                                    chkUrinalysis.Enabled = true;
                                    chkUrinalysis.Checked = true;

                                    foreach (ClientDeptTestCategory testCategoryItem in clientDepartment.ClientDeptTestCategories)
                                    {
                                        if (testCategoryItem.TestCategoryId == TestCategories.UA)
                                        {
                                            TestPanelBL testPanelBL = new TestPanelBL();
                                            List<TestPanel> testPanelUAList = testPanelBL.GetListByUA();
                                            cmbUATestPanel.DataSource = testPanelUAList;
                                            cmbUATestPanel.ValueMember = "TestPanelId";
                                            cmbUATestPanel.DisplayMember = "TestPanelName";

                                            break;
                                        }
                                    }

                                    if (testCategory.TestPanelId != null)
                                    {
                                        cmbUATestPanel.SelectedValue = testCategory.TestPanelId;
                                    }

                                    txtUASpecimenId.Text = testCategory.SpecimenId;
                                    UASpecimenId = testCategory.SpecimenId;
                                }

                                if (testCategory.TestCategoryId == TestCategories.Hair)
                                {
                                    chkHair.Enabled = true;
                                    chkHair.Checked = true;

                                    foreach (ClientDeptTestCategory testCategoryItem in clientDepartment.ClientDeptTestCategories)
                                    {
                                        if (testCategoryItem.TestCategoryId == TestCategories.Hair)
                                        {
                                            TestPanelBL testPanelBL = new TestPanelBL();
                                            List<TestPanel> testPanelHairList = testPanelBL.GetListByHair();
                                            cmbHairTestPanel.DataSource = testPanelHairList;
                                            cmbHairTestPanel.ValueMember = "TestPanelId";
                                            cmbHairTestPanel.DisplayMember = "TestPanelName";

                                            break;
                                        }
                                    }

                                    if (testCategory.TestPanelId != null)
                                    {
                                        cmbHairTestPanel.SelectedValue = testCategory.TestPanelId;
                                    }

                                    if (testCategory.HairTestPanelDays != null)
                                    {
                                        cmbDays.SelectedIndex = Convert.ToInt32(testCategory.HairTestPanelDays) / 90;

                                        if (donorTestInfo.PaymentStatus != PaymentStatus.Paid && Convert.ToInt32(donorTestInfo.TestStatus) < 6)
                                        {
                                            cmbDays.Enabled = true;
                                        }
                                        else
                                        {
                                            cmbDays.Enabled = false;
                                        }
                                    }

                                    txtHairSpecimenId.Text = testCategory.SpecimenId;
                                }
                                if (testCategory.TestCategoryId == TestCategories.DNA)
                                {
                                    chkDNA.Enabled = true;
                                    chkDNA.Checked = true;
                                }
                            }
                        }
                        if (client.CanEditTestCategory == false)
                        {
                            foreach (DonorTestInfoTestCategories testCategory in donorTestInfo.TestInfoTestCategories)
                            {
                                if (testCategory.TestCategoryId == TestCategories.UA)
                                {
                                    chkUrinalysis.Enabled = false;
                                    chkUrinalysis.Checked = true;

                                    foreach (ClientDeptTestCategory testCategoryItem in clientDepartment.ClientDeptTestCategories)
                                    {
                                        if (testCategoryItem.TestCategoryId == TestCategories.UA)
                                        {
                                            TestPanelBL testPanelBL = new TestPanelBL();
                                            List<TestPanel> testPanelUAList = testPanelBL.GetListByUA();
                                            cmbUATestPanel.DataSource = testPanelUAList;
                                            cmbUATestPanel.ValueMember = "TestPanelId";
                                            cmbUATestPanel.DisplayMember = "TestPanelName";

                                            break;
                                        }
                                    }

                                    if (testCategory.TestPanelId != null)
                                    {
                                        cmbUATestPanel.SelectedValue = testCategory.TestPanelId;
                                    }

                                    txtUASpecimenId.Text = testCategory.SpecimenId;
                                    UASpecimenId = testCategory.SpecimenId;
                                }

                                if (testCategory.TestCategoryId == TestCategories.Hair)
                                {
                                    chkHair.Enabled = false;
                                    chkHair.Checked = true;

                                    foreach (ClientDeptTestCategory testCategoryItem in clientDepartment.ClientDeptTestCategories)
                                    {
                                        if (testCategoryItem.TestCategoryId == TestCategories.Hair)
                                        {
                                            TestPanelBL testPanelBL = new TestPanelBL();
                                            List<TestPanel> testPanelHairList = testPanelBL.GetListByHair();
                                            cmbHairTestPanel.DataSource = testPanelHairList;
                                            cmbHairTestPanel.ValueMember = "TestPanelId";
                                            cmbHairTestPanel.DisplayMember = "TestPanelName";

                                            break;
                                        }
                                    }

                                    if (testCategory.TestPanelId != null)
                                    {
                                        cmbHairTestPanel.SelectedValue = testCategory.TestPanelId;
                                    }

                                    if (testCategory.HairTestPanelDays != null)
                                    {
                                        cmbDays.SelectedIndex = Convert.ToInt32(testCategory.HairTestPanelDays) / 90;

                                        if (donorTestInfo.PaymentStatus != PaymentStatus.Paid && Convert.ToInt32(donorTestInfo.TestStatus) < 6)
                                        {
                                            cmbDays.Enabled = true;
                                        }
                                        else
                                        {
                                            cmbDays.Enabled = false;
                                        }
                                    }

                                    txtHairSpecimenId.Text = testCategory.SpecimenId;
                                }
                                if (testCategory.TestCategoryId == TestCategories.DNA)
                                {
                                    chkDNA.Enabled = false;
                                    chkDNA.Checked = true;
                                }
                            }
                        }

                        if (chkUrinalysis.Enabled && donorTestInfo.IsUA)
                        {
                            chkUrinalysis.Checked = true;
                        }

                        if (chkHair.Enabled && donorTestInfo.IsHair)
                        {
                            chkHair.Checked = true;
                        }

                        if (chkDNA.Enabled && donorTestInfo.IsDNA)
                        {
                            chkDNA.Checked = true;
                        }
                        if (chkBC.Enabled && donorTestInfo.IsBC)
                        {
                            chkBC.Checked = true;
                        }

                        if (donorTestInfo.IsInstantTest)
                        {
                            chkInstant.Checked = true;
                        }
                        if (testflag == true)
                        {
                            if (donorTestInfo.IsUA == false)
                            {
                                chkUrinalysis.Enabled = false;
                            }
                            if (donorTestInfo.IsHair == false)
                            {
                                chkHair.Enabled = false;
                            }
                            if (donorTestInfo.IsDNA == false)
                            {
                                chkDNA.Enabled = false;
                            }
                            if (donorTestInfo.IsBC == false)
                            {
                                chkBC.Enabled = false;
                            }
                        }

                        if (donorTestInfo.PaymentStatus == PaymentStatus.Paid)
                        {
                            chkUrinalysis.Enabled = false;
                            chkHair.Enabled = false;
                            chkDNA.Enabled = false;
                        }
                        if (chkHair.Checked == false)
                        {
                            cmbDays.SelectedIndex = 0;
                            cmbDays.Enabled = false;
                        }

                        //if (chkUrinalysis.Checked == true)
                        //{
                        //    chkUrinalysis.Enabled = true;
                        //    cmbUATestPanel.Enabled = false;
                        //}
                        //else
                        //{
                        //    chkUrinalysis.Enabled = false;
                        //    cmbUATestPanel.Enabled = false;
                        //}

                        //if (chkHair.Checked == true)
                        //{
                        //    chkHair.Enabled = true;
                        //    cmbHairTestPanel.Enabled = false;
                        //    cmbDays.Enabled = true;
                        //}
                        //else
                        //{
                        //    chkHair.Enabled = false;
                        //    cmbHairTestPanel.Enabled = false;
                        //    cmbDays.Enabled = false;
                        //}

                        //if (chkDNA.Checked == true)
                        //{
                        //    chkDNA.Enabled = true;
                        //}
                        //else
                        //{
                        //    //    chkDNA.Enabled = false;
                        //}

                        chkInstant.Enabled = true;

                        pnlReason.Enabled = true;
                        rbRandom.Enabled = true;
                        pnlCup.Enabled = true;
                        pnlObserved.Enabled = true;
                        pnlFormType.Enabled = true;
                        pnlTemprature.Enabled = true;
                        pnlAdulteration.Enabled = true;
                        pnlQNS.Enabled = true;
                        chkDonorRefuses.Enabled = true;
                        btnTestInfoSave.Enabled = true;
                    }
                }
            }
        }

        private void LoadResultDetails(DonorTestInfo donorTestInfo)
        {
            LoadCRLReport();

            LoadQuestReport();

            LoadMROReport();
        }

        private void LoadActivityDetails()
        {
            lstActivityNoteHistory.Items.Clear();

            List<DonorActivityNote> donorActivityNoteList = donorBL.GetDonorActivityList(this.currentTestInfoId);

            lstActivityNoteHistory.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            lstActivityNoteHistory.MeasureItem += lst_ActivityNote_MeasureItem;
            lstActivityNoteHistory.DrawItem += lst_ActivityNote_DrawItem;

            if (donorActivityNoteList != null)
            {
                foreach (DonorActivityNote donorActivityNote in donorActivityNoteList)
                {
                    int userId = (int)donorActivityNote.ActivityUserId;
                    //if (Program.currentUserName.ToUpper() == donorActivityNote.ActivityUserName.ToUpper() || Program.currentUserName.ToUpper() == Program.adminUsername.ToUpper())
                    if ((Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()) || (Program.currentUserId == userId))
                    {
                        string activityNote = donorActivityNote.ActivityDateTime.ToString("MM/dd/yyyy hh:mm tt");
                        activityNote += "(" + donorActivityNote.ActivityUserName + "):";
                        activityNote += donorActivityNote.ActivityCategoryId.ToDescriptionString() + ":";
                        activityNote += (donorActivityNote.IsActivityVisible ? "Visible" : "Invisible") + ":";
                        activityNote += donorActivityNote.ActivityNote;

                        lstActivityNoteHistory.Items.Add(activityNote);
                    }
                    else     //if (Program.currentUserName.ToUpper()  == "Visible")
                    {
                        if (donorActivityNote.IsActivityVisible == true)
                        {
                            string activityNote = donorActivityNote.ActivityDateTime.ToString("MM/dd/yyyy hh:mm tt");
                            activityNote += "(" + donorActivityNote.ActivityUserName + "):";
                            activityNote += donorActivityNote.ActivityCategoryId.ToDescriptionString() + ":";
                            activityNote += (donorActivityNote.IsActivityVisible ? "Visible" : "Invisible") + ":";
                            activityNote += donorActivityNote.ActivityNote;

                            lstActivityNoteHistory.Items.Add(activityNote);
                        }
                    }
                }
            }
        }

        private void LoadLegalDetails(DonorTestInfo donorTestInfo)
        {
            if ((Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()) || viewflag == true)
            {
                //AttorneyInformation
                if (donorTestInfo.AttorneyId1 != null)
                {
                    cmbAttorneyName1.SelectedValue = donorTestInfo.AttorneyId1;
                }
                else
                {
                    cmbAttorneyName1.SelectedIndex = 0;
                }

                if (donorTestInfo.AttorneyId2 != null)
                {
                    cmbAttorneyName2.SelectedValue = donorTestInfo.AttorneyId2;
                }
                else
                {
                    cmbAttorneyName2.SelectedIndex = 0;
                }

                if (donorTestInfo.AttorneyId3 != null)
                {
                    cmbAttorneyName3.SelectedValue = donorTestInfo.AttorneyId3;
                }
                else
                {
                    cmbAttorneyName3.SelectedIndex = 0;
                }

                //ThirdPartyInfomation
                if (donorTestInfo.ThirdPartyInfoId1 != null)
                {
                    cmbThirdPartyInfo1Name.SelectedValue = donorTestInfo.ThirdPartyInfoId1;
                }
                else
                {
                    cmbThirdPartyInfo1Name.SelectedIndex = 0;
                }

                if (donorTestInfo.ThirdPartyInfoId2 != null)
                {
                    cmbThirdPartyInfo2Name.SelectedValue = donorTestInfo.ThirdPartyInfoId2;
                }
                else
                {
                    cmbThirdPartyInfo2Name.SelectedIndex = 0;
                }
                //Program details
                if (donorTestInfo.ProgramTypeId != ProgramType.None)
                {
                    if (donorTestInfo.ProgramTypeId == ProgramType.OneTimeTesting)
                    {
                        rbOneTimeTesting.Checked = true;
                    }
                    else if (donorTestInfo.ProgramTypeId == ProgramType.Random)
                    {
                        rbRandomTesting.Checked = true;
                    }
                }

                if (donorTestInfo.IsSurscanDeterminesDates == true)
                {
                    chkSurScanDates.Checked = true;
                }
                else
                {
                    chkSurScanDates.Checked = false;
                }

                if (donorTestInfo.IsTpDeterminesDates == true)
                {
                    chkThirdPartyDates.Checked = true;
                }
                else
                {
                    chkThirdPartyDates.Checked = false;
                }

                if (donorTestInfo.ProgramStartDate != null && donorTestInfo.ProgramStartDate != DateTime.MinValue)
                {
                    DateTime inActiveDate = Convert.ToDateTime(donorTestInfo.ProgramStartDate);
                    cmbLegalStartMonth.Text = inActiveDate.ToString("MM");
                    cmbLegalStartDate.Text = inActiveDate.ToString("dd");
                    cmbLegalStartYear.Text = inActiveDate.ToString("yyyy");
                    // txtStartDate.Text = Convert.ToDateTime(donorTestInfo.ProgramStartDate).ToString("MM/dd/yyyy");
                }
                else
                {
                    // txtStartDate.Text = string.Empty;
                    //DateTime startDate = DateTime.Now;
                    //cmbLegalStartMonth.Text = startDate.ToString("MM");
                    //cmbLegalStartDate.Text = startDate.ToString("dd");
                    //cmbLegalStartYear.Text = startDate.ToString("yyyy");
                    cmbLegalStartMonth.SelectedIndex = 0;
                    cmbLegalStartDate.SelectedIndex = 0;
                    cmbLegalStartYear.SelectedIndex = 0;
                }

                if (donorTestInfo.ProgramEndDate != null && donorTestInfo.ProgramEndDate != DateTime.MinValue)
                {
                    DateTime inActiveDate = Convert.ToDateTime(donorTestInfo.ProgramEndDate);
                    cmbLegalEndMonth.Text = inActiveDate.ToString("MM");
                    cmbLegalEndDate.Text = inActiveDate.ToString("dd");
                    cmbLegalEndYear.Text = inActiveDate.ToString("yyyy");
                    //txtEndDate.Text = Convert.ToDateTime(donorTestInfo.ProgramEndDate).ToString("MM/dd/yyyy");
                }
                else
                {
                    //txtEndDate.Text = string.Empty;
                    DateTime endDate = DateTime.Now;
                    cmbLegalEndMonth.SelectedIndex = 0;
                    cmbLegalEndDate.SelectedIndex = 0;
                    cmbLegalEndYear.SelectedIndex = 0;
                }

                //CourtDetails

                if (donorTestInfo.CaseNumber != null)
                {
                    txtCaseNumber.Text = donorTestInfo.CaseNumber;
                }
                else
                {
                    txtCaseNumber.Text = string.Empty;
                }

                if (donorTestInfo.CourtId != null)
                {
                    cmbCourtName.SelectedValue = donorTestInfo.CourtId;
                }

                //JudgeDetails
                if (donorTestInfo.JudgeId != null)
                {
                    cmbJudgeName.SelectedValue = donorTestInfo.JudgeId;
                }

                if (donorTestInfo.SpecialNotes != null)
                {
                    txtLegalInfoNotes.Text = donorTestInfo.SpecialNotes;

                    if (txtLegalInfoNotes.Text.Length > 1246)
                    {
                        txtLegalInfoNotes.ScrollBars = ScrollBars.Vertical;
                    }
                    else
                    {
                        txtLegalInfoNotes.ScrollBars = ScrollBars.None;
                    }
                }
                else
                {
                    txtLegalInfoNotes.Text = string.Empty;
                }
            }
        }

        private void LoadVendorDetails(DonorTestInfo donorTestInfo)
        {
            //VendorInformation
            if (donorTestInfo.CollectionSite1Id != 0 && donorTestInfo.CollectionSite1Id != null)
            {
                cmbVendor1Name.SelectedValue = donorTestInfo.CollectionSite1Id;
                if (cmbVendor1Name.SelectedValue != null)
                {
                    int vendorId = (int)cmbVendor1Name.SelectedValue;
                    Vendor vendor = vendorBL.Get(vendorId);

                    txtVendor1City.Text = vendor.VendorCity;
                    txtVendor1State.Text = vendor.VendorState;
                    txtVendor1Phone.Text = vendor.VendorPhone;
                    txtVendor1Fax.Text = vendor.VendorFax;
                }

                //  txtScheduleDate.Text = donorTestInfo.ScheduleDate.ToString();
                //DateTime inActiveDate = Convert.ToDateTime(donorTestInfo.ScheduleDate);
                //DateTime ActiveDate = Convert.ToDateTime(inActiveDate.ToString("MM/dd/yyyy"));
                //if (inActiveDate == DateTime.MinValue)
                //{
                //    cmbMonth.SelectedIndex = 0;
                //    cmbDate.SelectedIndex = 0;
                //    cmbYear.SelectedIndex = 0;

                //    //cmbDonorMonth.Visible = true;
                //    //cmbDonorDate.Visible = true;
                //    //cmbDonorYear.Visible = true;
                //    //txtDOB.Visible = false;
                //}
                //else
                //{
                //    cmbMonth.Text = ActiveDate.ToString("MM");
                //    cmbDate.Text = ActiveDate.ToString("dd");
                //    cmbYear.Text = ActiveDate.ToString("yyyy");

                //    //  txtDOB.Text = inActiveDate.ToString("MM/dd/yyyy");
                //}
            }

            if (donorTestInfo.CollectionSite2Id != 0 && donorTestInfo.CollectionSite2Id != null)
            {
                cmbVendor2Name.SelectedValue = donorTestInfo.CollectionSite2Id;
                if (cmbVendor2Name.SelectedValue != null)
                {
                    int vendorId = (int)cmbVendor2Name.SelectedValue;

                    Vendor vendor = vendorBL.Get(vendorId);

                    txtVendor2City.Text = vendor.VendorCity;
                    txtVendor2State.Text = vendor.VendorState;
                    txtVendor2Phone.Text = vendor.VendorPhone;
                    txtVendor2Fax.Text = vendor.VendorFax;
                }
                //  txtScheduleDate.Text = donorTestInfo.ScheduleDate.ToString();
                //DateTime inActiveDate = Convert.ToDateTime(donorTestInfo.ScheduleDate);
                //DateTime ActiveDate = Convert.ToDateTime(inActiveDate.ToString("MM/dd/yyyy"));
                //if (inActiveDate == DateTime.MinValue)
                //{
                //    cmbMonth.SelectedIndex = 0;
                //    cmbDate.SelectedIndex = 0;
                //    cmbYear.SelectedIndex = 0;
                //}
                //else
                //{
                //    cmbMonth.Text = ActiveDate.ToString("MM");
                //    cmbDate.Text = ActiveDate.ToString("dd");
                //    cmbYear.Text = ActiveDate.ToString("yyyy");
                //}
            }

            if (donorTestInfo.CollectionSite3Id != 0 && donorTestInfo.CollectionSite3Id != null)
            {
                cmbVendor3Name.SelectedValue = donorTestInfo.CollectionSite3Id;
                if (cmbVendor3Name.SelectedValue != null)
                {
                    int vendorId = (int)cmbVendor3Name.SelectedValue;

                    Vendor vendor = vendorBL.Get(vendorId);

                    txtVendor3City.Text = vendor.VendorCity;
                    txtVendor3State.Text = vendor.VendorState;
                    txtVendor3Phone.Text = vendor.VendorPhone;
                    txtVendor3Fax.Text = vendor.VendorFax;
                }
                ////   txtScheduleDate.Text = donorTestInfo.ScheduleDate.ToString();
                //DateTime inActiveDate = Convert.ToDateTime(donorTestInfo.ScheduleDate);
                //DateTime ActiveDate = Convert.ToDateTime(inActiveDate.ToString("MM/dd/yyyy"));
                //if (inActiveDate == DateTime.MinValue)
                //{
                //    cmbMonth.SelectedIndex = 0;
                //    cmbDate.SelectedIndex = 0;
                //    cmbYear.SelectedIndex = 0;
                //}
                //else
                //{
                //    cmbMonth.Text = ActiveDate.ToString("MM");
                //    cmbDate.Text = ActiveDate.ToString("dd");
                //    cmbYear.Text = ActiveDate.ToString("yyyy");

                //}
            }

            if (donorTestInfo.CollectionSite4Id != 0 && donorTestInfo.CollectionSite4Id != null)
            {
                cmbVendor4Name.SelectedValue = donorTestInfo.CollectionSite4Id;
                if (cmbVendor4Name.SelectedValue != null)
                {
                    int vendorId = (int)cmbVendor4Name.SelectedValue;

                    Vendor vendor = vendorBL.Get(vendorId);

                    txtVendor4City.Text = vendor.VendorCity;
                    txtVendor4State.Text = vendor.VendorState;
                    txtVendor4Phone.Text = vendor.VendorPhone;
                    txtVendor4Fax.Text = vendor.VendorFax;
                }
                //   txtScheduleDate.Text = donorTestInfo.ScheduleDate.ToString();
                //DateTime inActiveDate = Convert.ToDateTime(donorTestInfo.ScheduleDate);
                //DateTime ActiveDate = Convert.ToDateTime(inActiveDate.ToString("MM/dd/yyyy"));
                //if (inActiveDate == DateTime.MinValue)
                //{
                //    cmbMonth.SelectedIndex = 0;
                //    cmbDate.SelectedIndex = 0;
                //    cmbYear.SelectedIndex = 0;
                //}
                //else
                //{
                //    cmbMonth.Text = ActiveDate.ToString("MM");
                //    cmbDate.Text = ActiveDate.ToString("dd");
                //    cmbYear.Text = ActiveDate.ToString("yyyy");

                //}
            }
        }

        private void LoadDocumentDetails(bool ShowSystem = false)
        {
            if (this.currentDonorId > 0)
            {
                DonorBL donorBL = new DonorBL();
                List<DonorDocument> donorDocumentList = null;
                donorDocumentList = donorBL.GetDonorDocumentList(this.currentDonorId, ShowSystem);

                dgvDocuments.DataSource = donorDocumentList;

                if (dgvDocuments.Rows.Count > 0)
                {
                    btnDocumentSelectAll.Enabled = true;
                    btnDocumentDeselectAll.Enabled = true;
                    btnDocumentViewSelected.Enabled = true;
                    btnDocumentViewAll.Enabled = true;
                    btnDocumentExportSelected.Enabled = true;
                    btnDocumentExportAll.Enabled = true;
                    dgvDocuments.Focus();
                }
                else
                {
                    btnDocumentSelectAll.Enabled = false;
                    btnDocumentDeselectAll.Enabled = false;
                    btnDocumentViewSelected.Enabled = false;
                    btnDocumentViewAll.Enabled = false;
                    btnDocumentExportSelected.Enabled = false;
                    btnDocumentExportAll.Enabled = false;
                }

                if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                {
                    //DONOR_EDIT_DOCUMENT_TAB
                    DataRow[] donorDocumemntInfoEdit = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DONOR_EDIT_DOCUMENT_TAB.ToDescriptionString() + "'");

                    if (donorDocumemntInfoEdit.Length > 0)
                    {
                        if (dgvDocuments.Rows.Count > 0)
                        {
                            btnDocumentSelectAll.Enabled = true;
                            btnDocumentDeselectAll.Enabled = true;
                            btnDocumentViewSelected.Enabled = true;
                            btnDocumentViewAll.Enabled = true;
                            btnDocumentExportSelected.Enabled = true;
                            btnDocumentExportAll.Enabled = true;
                            dgvDocuments.Focus();
                        }
                        else
                        {
                            // dgvDocuments.DataSource = null;
                            btnDocumentUpload.Enabled = true;
                            btnDocumentSelectAll.Enabled = false;
                            btnDocumentDeselectAll.Enabled = false;
                            btnDocumentViewSelected.Enabled = false;
                            btnDocumentViewAll.Enabled = false;
                            btnDocumentExportSelected.Enabled = false;
                            btnDocumentExportAll.Enabled = false;
                        }
                    }
                    else
                    {
                        dgvDocuments.Enabled = false;
                        btnDocumentUpload.Enabled = false;
                        btnDocumentSelectAll.Enabled = false;
                        btnDocumentDeselectAll.Enabled = false;
                        btnDocumentViewSelected.Enabled = false;
                        btnDocumentViewAll.Enabled = false;
                        btnDocumentExportSelected.Enabled = false;
                        btnDocumentExportAll.Enabled = false;
                    }
                }
            }
        }

        private void LoadPaymentDetails(DonorTestInfo donorTestInfo)
        {
            if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
            {
                #region Load Payment Details

                //DONOR_COLLECT_PAYMENT
                DataRow[] collectPaymentTab = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DONOR_COLLECT_PAYMENT.ToDescriptionString() + "'");

                txtPaymentAmount.Text = string.Format("{0:0.00}", donorTestInfo.TotalPaymentAmount.ToString());

                if (donorTestInfo.PaymentStatus == PaymentStatus.Paid)
                {
                    lblpaymsg.Visible = false;
                    //dtpPaymentDate.Value = Convert.ToDateTime(donorTestInfo.PaymentDate);
                    DateTime inActiveDate = Convert.ToDateTime(donorTestInfo.PaymentDate);
                    DateTime ActiveDate = Convert.ToDateTime(inActiveDate.ToString("MM/dd/yyyy"));
                    if (inActiveDate == DateTime.MinValue)
                    {
                        cmbPaymentMonth.SelectedIndex = 0;
                        cmbPaymentDate.SelectedIndex = 0;
                        cmbPaymentYear.SelectedIndex = 0;
                    }
                    else
                    {
                        cmbPaymentMonth.Text = ActiveDate.ToString("MM");
                        cmbPaymentDate.Text = ActiveDate.ToString("dd");
                        cmbPaymentYear.Text = ActiveDate.ToString("yyyy");
                    }
                    if (donorTestInfo.PaymentTypeId == ClientPaymentTypes.InvoiceClient)
                    {
                        chkinvoiceclient.Checked = true;
                    }
                    else
                    {
                        chkinvoiceclient.Checked = false;
                    }

                    if (donorTestInfo.PaymentMethodId == PaymentMethod.Cash)
                    {
                        rbtnPaymentCash.Checked = true;
                    }
                    else if (donorTestInfo.PaymentMethodId == PaymentMethod.Card)
                    {
                        rbtnPaymentCard.Checked = true;
                    }
                    else
                    {
                        rbtnPaymentCash.Checked = false;
                        rbtnPaymentCard.Checked = false;
                    }
                    chkPayment.Checked = true;
                    txtPaymentNote.Text = donorTestInfo.PaymentNote;
                    if (collectPaymentTab.Length > 0)
                    {
                        // tcMain.TabPages.Add(tabPayment);
                        // tcMain.SelectedTab = tabPayment;

                        if (donorTestInfo.PaymentStatus == PaymentStatus.Paid)
                        {
                            // dtpPaymentDate.Enabled = false;
                            lblpaymsg.Visible = false;
                            cmbPaymentMonth.Enabled = false;
                            cmbPaymentDate.Enabled = false;
                            cmbPaymentYear.Enabled = false;

                            rbtnPaymentCash.Enabled = false;
                            rbtnPaymentCard.Enabled = false;
                            txtPaymentNote.Enabled = false;
                            chkPayment.Enabled = false;
                            btnPaymentSave.Enabled = false;
                            if (donorTestInfo.PaymentTypeId == ClientPaymentTypes.InvoiceClient)
                            {
                                chkinvoiceclient.Checked = true;
                            }
                            else
                            {
                                chkinvoiceclient.Checked = false;
                            }
                        }
                        else
                        {
                            //  dtpPaymentDate.Enabled = true;
                            lblpaymsg.Visible = true;
                            cmbPaymentMonth.Enabled = true;
                            cmbPaymentDate.Enabled = true;
                            cmbPaymentYear.Enabled = true;

                            rbtnPaymentCash.Enabled = true;
                            rbtnPaymentCard.Enabled = true;
                            txtPaymentNote.Enabled = true;
                            chkPayment.Enabled = true;
                            btnPaymentSave.Enabled = true;

                            if (donorTestInfo.PaymentTypeId == ClientPaymentTypes.InvoiceClient)
                            {
                                chkinvoiceclient.Checked = true;
                            }
                            else
                            {
                                chkinvoiceclient.Checked = false;
                            }
                        }
                    }
                    else
                    {
                        // dtpPaymentDate.Enabled = false;
                        lblpaymsg.Visible = false;
                        cmbPaymentMonth.Enabled = false;
                        cmbPaymentDate.Enabled = false;
                        cmbPaymentYear.Enabled = false;

                        rbtnPaymentCash.Enabled = false;
                        rbtnPaymentCard.Enabled = false;
                        txtPaymentNote.Enabled = false;
                        chkPayment.Enabled = false;
                        btnPaymentSave.Enabled = false;
                        if (donorTestInfo.PaymentTypeId == ClientPaymentTypes.InvoiceClient)
                        {
                            chkinvoiceclient.Checked = true;
                        }
                        else
                        {
                            chkinvoiceclient.Checked = false;
                        }
                    }
                }
                else if (donorTestInfo.PaymentStatus == PaymentStatus.None || donorTestInfo.PaymentStatus == PaymentStatus.Pending)
                {
                    if (collectPaymentTab.Length > 0)
                    {
                        //tcMain.TabPages.Add(tabPayment);
                        //tcMain.SelectedTab = tabPayment;

                        //    dtpPaymentDate.Enabled = true;
                        lblpaymsg.Visible = true;
                        cmbPaymentMonth.Enabled = true;
                        cmbPaymentDate.Enabled = true;
                        cmbPaymentYear.Enabled = true;

                        rbtnPaymentCash.Enabled = true;
                        rbtnPaymentCard.Enabled = true;
                        txtPaymentNote.Enabled = true;
                        chkPayment.Enabled = true;
                        btnPaymentSave.Enabled = true;

                        if (donorTestInfo.PaymentTypeId == ClientPaymentTypes.InvoiceClient)
                        {
                            chkinvoiceclient.Checked = true;
                        }
                        else
                        {
                            chkinvoiceclient.Checked = false;
                        }
                    }
                    else
                    {
                        //  dtpPaymentDate.Enabled = false;
                        lblpaymsg.Visible = false;
                        cmbPaymentMonth.Enabled = false;
                        cmbPaymentDate.Enabled = false;
                        cmbPaymentYear.Enabled = false;

                        rbtnPaymentCash.Enabled = false;
                        rbtnPaymentCard.Enabled = false;
                        txtPaymentNote.Enabled = false;
                        chkPayment.Enabled = false;
                        btnPaymentSave.Enabled = false;
                        if (donorTestInfo.PaymentTypeId == ClientPaymentTypes.InvoiceClient)
                        {
                            chkinvoiceclient.Checked = true;
                        }
                        else
                        {
                            chkinvoiceclient.Checked = false;
                        }
                    }
                }

                #endregion Load Payment Details
            }
            else
            {
                txtPaymentAmount.Text = string.Format("{0:0.00}", donorTestInfo.TotalPaymentAmount.ToString());

                if (donorTestInfo.PaymentStatus == PaymentStatus.Paid)
                {
                    // dtpPaymentDate.Value = Convert.ToDateTime(donorTestInfo.PaymentDate);
                    lblpaymsg.Visible = false;
                    DateTime inActiveDate = Convert.ToDateTime(donorTestInfo.PaymentDate);
                    DateTime ActiveDate = Convert.ToDateTime(inActiveDate.ToString("MM/dd/yyyy"));
                    if (inActiveDate == DateTime.MinValue)
                    {
                        cmbPaymentMonth.SelectedIndex = 0;
                        cmbPaymentDate.SelectedIndex = 0;
                        cmbPaymentYear.SelectedIndex = 0;
                    }
                    else
                    {
                        cmbPaymentMonth.Text = ActiveDate.ToString("MM");
                        cmbPaymentDate.Text = ActiveDate.ToString("dd");
                        cmbPaymentYear.Text = ActiveDate.ToString("yyyy");
                    }
                    if (donorTestInfo.PaymentTypeId == ClientPaymentTypes.InvoiceClient)
                    {
                        chkinvoiceclient.Checked = true;
                    }
                    else
                    {
                        chkinvoiceclient.Checked = false;
                    }

                    if (donorTestInfo.PaymentMethodId == PaymentMethod.Cash)
                    {
                        rbtnPaymentCash.Checked = true;
                    }
                    else if (donorTestInfo.PaymentMethodId == PaymentMethod.Card)
                    {
                        rbtnPaymentCard.Checked = true;
                    }
                    else
                    {
                        rbtnPaymentCash.Checked = false;
                        rbtnPaymentCard.Checked = false;
                    }
                    if (donorTestInfo.IsPaymentReceived == true)
                    {
                        chkPayment.Checked = true;
                    }
                    else
                    {
                        chkPayment.Checked = false;
                    }

                    txtPaymentNote.Text = donorTestInfo.PaymentNote;

                    //  dtpPaymentDate.Enabled = false;
                    cmbPaymentMonth.Enabled = false;
                    cmbPaymentDate.Enabled = false;
                    cmbPaymentYear.Enabled = false;
                    chkPayment.Enabled = false;

                    rbtnPaymentCash.Enabled = false;
                    rbtnPaymentCard.Enabled = false;
                    txtPaymentNote.Enabled = false;
                    btnPaymentSave.Enabled = false;
                }
                else if (donorTestInfo.PaymentStatus == PaymentStatus.None || donorTestInfo.PaymentStatus == PaymentStatus.Pending)
                {
                    lblpaymsg.Visible = true;
                    if (donorTestInfo.PaymentMethodId == PaymentMethod.Cash)
                    {
                        rbtnPaymentCash.Checked = true;
                    }
                    else if (donorTestInfo.PaymentMethodId == PaymentMethod.Card)
                    {
                        rbtnPaymentCard.Checked = true;
                    }
                    else
                    {
                        rbtnPaymentCash.Checked = false;
                        rbtnPaymentCard.Checked = false;
                    }
                    if (donorTestInfo.PaymentTypeId == ClientPaymentTypes.InvoiceClient)
                    {
                        chkinvoiceclient.Checked = true;
                    }
                    else
                    {
                        chkinvoiceclient.Checked = false;
                    }

                    if (donorTestInfo.IsPaymentReceived == true)
                    {
                        chkPayment.Checked = true;
                    }
                    else
                    {
                        chkPayment.Checked = false;
                    }
                    txtPaymentNote.Text = donorTestInfo.PaymentNote;
                    //  dtpPaymentDate.Enabled = true;
                    cmbPaymentMonth.Enabled = true;
                    cmbPaymentDate.Enabled = true;
                    cmbPaymentYear.Enabled = true;

                    rbtnPaymentCash.Enabled = true;
                    rbtnPaymentCard.Enabled = true;
                    txtPaymentNote.Enabled = true;
                    btnPaymentSave.Enabled = true;
                    chkPayment.Enabled = true;
                }
            }
        }

        private void LoadAccountDetails(int currentDonorId, int currentTestInfoId)
        {
            DonorAccounting donorAccounting = donorBL.GetAccountingDetails(this.currentDonorId, this.currentTestInfoId);

            txtAccSingleUARevenue.Text = Convert.ToDouble(donorAccounting.UARevenue).ToString();
            txtAccSingleHairRevenue.Text = Convert.ToDouble(donorAccounting.HairRevenue).ToString();
            txtAccSingleDNARevenue.Text = Convert.ToDouble(donorAccounting.DNARevenue).ToString();
            txtAccSingleTotalRevenue.Text = Convert.ToDouble(donorAccounting.TotalRevenue).ToString();
            txtAccSingleLabCost.Text = Convert.ToDouble(donorAccounting.LaboratoryCost).ToString();
            txtAccSingleMROCost.Text = Convert.ToDouble(donorAccounting.MROCost).ToString();
            //  txtAccSingleCupCost.Text = Convert.ToDouble(donorAccounting.CupCost).ToString();
            //   txtAccSingleShippingCost.Text = Convert.ToDouble(donorAccounting.ShippingCost).ToString();
            txtAccSingleVendorCost.Text = Convert.ToDouble(donorAccounting.VendorCost).ToString();
            txtAccSingleTotalCost.Text = Convert.ToDouble(donorAccounting.TotalCost).ToString();
            txtAccSingleGrossProfit.Text = Convert.ToDouble(donorAccounting.GrossProfit).ToString();

            txtAccConsolUARevenue.Text = Convert.ToDouble(donorAccounting.CumulativeUARevenue).ToString();
            txtAccConsolHairRevenue.Text = Convert.ToDouble(donorAccounting.CumulativeHairRevenue).ToString();
            txtAccConsolDNARevenue.Text = Convert.ToDouble(donorAccounting.CumulativeDNARevenue).ToString();
            txtAccConsolTotalRevenue.Text = Convert.ToDouble(donorAccounting.CumulativeTotalRevenue).ToString();
            txtAccConsolLabCost.Text = Convert.ToDouble(donorAccounting.CumulativeLaboratoryCost).ToString();
            txtAccConsolMROCost.Text = Convert.ToDouble(donorAccounting.CumulativeMROCost).ToString();
            //txtAccConsolCupCost.Text = Convert.ToDouble(donorAccounting.CumulativeCupCost).ToString();
            //txtAccConsolShippingCost.Text = Convert.ToDouble(donorAccounting.CumulativeShippingCost).ToString();
            txtAccConsolVendorCost.Text = Convert.ToDouble(donorAccounting.CumulativeVendorCost).ToString();
            txtAccConsolTotalCost.Text = Convert.ToDouble(donorAccounting.CumulativeTotalCost).ToString();
            txtAccConsolGrossProfit.Text = Convert.ToDouble(donorAccounting.CumulativeGrossProfit).ToString();
        }

        private void LoadTestHistoryDetails()
        {
            if (this.currentDonorId > 0)
            {
                Dictionary<string, string> searchParam = new Dictionary<string, string>();

                searchParam.Add("DonorId", this.currentDonorId.ToString());

                DonorBL donorBL = new DonorBL();

                DataTable dtDonors = donorBL.SearchDonorFromTestHistory(searchParam, Program.currentUserType, Program.currentUserId, Program.currentUserName);

                dgvTestHistory.DataSource = dtDonors;

                if (dgvTestHistory.Rows.Count > 0)
                {
                    btnTestHistorySelectAll.Enabled = true;
                    btnTestHistoryDeselectAll.Enabled = true;
                    btnTestHistoryViewSelected.Enabled = true;
                    btnTestHistoryViewAll.Enabled = true;
                    dgvTestHistory.Focus();
                }
                else
                {
                    btnTestHistorySelectAll.Enabled = false;
                    btnTestHistoryDeselectAll.Enabled = false;
                    btnTestHistoryViewSelected.Enabled = false;
                    btnTestHistoryViewAll.Enabled = false;
                }
            }
        }

        #endregion DB to UI

        #region UI Validation

        private bool ValidatePreRegisteredDonor()
        {
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
                    //if (!ValidateEmail())
                    //{
                    //    Cursor.Current = Cursors.Default;
                    //    MessageBox.Show("Email already exists.");
                    //    txtEmail.Focus();
                    //    return false;
                    //}
                }
            }
            return true;
        }

        private bool ValidateDonorDetails()
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

            if (rbtnMale.Checked == false && rbtnFemale.Checked == false)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Gender must be selected.");
                rbtnFemale.Focus();
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
                    //if (!ValidateEmail())
                    //{
                    //    Cursor.Current = Cursors.Default;
                    //    MessageBox.Show("Email already exists.");
                    //    txtEmail.Focus();
                    //    return false;
                    //}
                }
            }

            if (txtSSN.Tag.ToString().Replace("_", "").Replace("-", "").Trim() == string.Empty || txtSSN.Text.Trim() == string.Empty)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("SSN cannot be empty.");
                txtSSN.Focus();
                return false;
            }
            else
            {
                if (this.cmbClient.Visible)
                {
                    //We are not checking for existing as we are just making a change.
                }
                else
                {
                    //if (!ValidateSSN())
                    //{
                    //    Cursor.Current = Cursors.Default;
                    //    MessageBox.Show("SSN already exists.");
                    //    txtSSN.Focus();
                    //    return false;
                    //}
                }
            }

            if (txtSSN.Text.ToString() != string.Empty)
            {
                if (txtSSN.Text.Length == 9)
                {
                    // txtSSN.Tag = txtSSN.Text;
                    string NewSSN = string.Empty;
                    string NewSSN1 = string.Empty;
                    string NewSSN2 = string.Empty;
                    NewSSN = txtSSN.Text.Substring(0, 3);
                    NewSSN1 = txtSSN.Text.Substring(3, 2);
                    NewSSN2 = txtSSN.Text.Substring(5, 4);
                    string Unmask = NewSSN + "-" + NewSSN1 + "-" + NewSSN2;
                    txtSSN.Tag = Unmask.ToString();
                }
            }

            if (txtSSN.Text.ToString().Contains(" ") || txtSSN.Tag.ToString().Length < 11)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Invalid Format of SSN.");
                txtSSN.Focus();
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
            //    MessageBox.Show("Date of Birth cannot be empty.");
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

            if ((int)cmbDepartment.SelectedValue==0)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("A department must be selected.");
                cmbDepartment.Focus();
                return false;
            }

            return true;
        }

        private bool ValidateTestInfoDetails()
        {
            if (chkDonorRefuses.Checked)
            {
                return true;
            }

            if (!(chkUrinalysis.Checked || chkHair.Checked || chkDNA.Checked))
            {
                tcMain.SelectedTab = tabTestInfo;
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Test Category must be selected.");
                chkUrinalysis.Focus();
                return false;
            }

            if (chkUrinalysis.Checked || chkHair.Checked || chkDNA.Checked)
            {
                if (chkHair.Checked)
                {
                    if (cmbDays.SelectedIndex == 0)
                    {
                        tcMain.SelectedTab = tabTestInfo;
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("# of Days must be selected.");
                        cmbDays.Focus();
                        return false;
                    }
                }

                //Reason For Test
                if (!(rbPreEmployment.Checked
                    || rbRandom.Checked
                    || rbReasonable.Checked
                    || rbPostAccident.Checked
                    || rbReturntoDuty.Checked
                    || rbFollowUp.Checked
                    || rbOther.Checked))
                {
                    tcMain.SelectedTab = tabTestInfo;
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Reason For Test must be selected.");
                    rbPreEmployment.Focus();
                    return false;
                }

                if (rbOther.Checked && txtReason.Text.Trim() == string.Empty)
                {
                    tcMain.SelectedTab = tabTestInfo;
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Other Reason cannot be empty.");
                    txtReason.Focus();
                    return false;
                }

                //Specimen Collectin Cup
                if (!(rbUrineSingle.Checked
                    || rbUrineSplit.Checked
                    || rbSaliva.Checked
                    || rbBlood.Checked))
                {
                    tcMain.SelectedTab = tabTestInfo;
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Specimen Collectin Cup must be selected.");
                    return false;
                }

                //Observed
                if (!(rbObservedYes.Checked
                    || rbObservedNo.Checked))
                {
                    tcMain.SelectedTab = tabTestInfo;
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Observed must be selected.");
                    return false;
                }

                //Form Type
                if (!(rbFederal.Checked
                    || rbNonFederal.Checked))
                {
                    tcMain.SelectedTab = tabTestInfo;
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Form Type must be selected.");
                    return false;
                }

                //Testing Authority
                if (rbFederal.Checked && cmbTestingAuthority.SelectedIndex == 0)
                {
                    tcMain.SelectedTab = tabTestInfo;
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Testing Authority must be selected.");
                    return false;
                }

                //Temperature
                //if (chkUrinalysis.Checked)
                //{
                //if (!(rbTemperatureYes.Checked
                //    || rbTemperatureNo.Checked))
                //{
                //    tcMain.SelectedTab = tabTestInfo;
                //    Cursor.Current = Cursors.Default;
                //    MessageBox.Show("Specimen Temperature must be confirmed.");
                //    return false;
                //}

                if (rbTemperatureNo.Checked && txtTemperature.Text.Trim() == string.Empty)
                {
                    tcMain.SelectedTab = tabTestInfo;
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Temperature cannot be empty.");
                    txtTemperature.Focus();
                    return false;
                }

                if (txtTemperature.Text.Trim() != string.Empty)
                {
                    try
                    {
                        double temperature = Convert.ToDouble(txtTemperature.Text.Trim());

                        if (rbTemperatureNo.Checked)
                        {
                            if (temperature >= 90 && temperature <= 100)
                            {
                                tcMain.SelectedTab = tabTestInfo;
                                Cursor.Current = Cursors.Default;
                                MessageBox.Show("Invalid range of Temperature.");
                                txtTemperature.Focus();
                                return false;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        tcMain.SelectedTab = tabTestInfo;
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Invalid format of Temperature.");
                        txtTemperature.Focus();
                        return false;
                    }
                }

                //if (!(rbAdulterationYes.Checked
                //    || rbAdulterationNo.Checked))
                //{
                //    tcMain.SelectedTab = tabTestInfo;
                //    Cursor.Current = Cursors.Default;
                //    MessageBox.Show("Sign of Adulteration must be confirmed.");
                //    return false;
                //}
                //}

                //if (!(rbQNSYes.Checked
                //        || rbQNSNo.Checked))
                //{
                //    tcMain.SelectedTab = tabTestInfo;
                //    Cursor.Current = Cursors.Default;
                //    MessageBox.Show("Speciment Quantity must be confirmed.");
                //    return false;
                //}

                if (rbTemperatureYes.Checked
                    && rbAdulterationNo.Checked
                    && rbQNSYes.Checked)
                {
                    if ((!chkUrinalysis.Enabled) && chkUrinalysis.Checked)
                    {
                        if (txtUASpecimenId.Text.Trim() == string.Empty)
                        {
                            tcMain.SelectedTab = tabTestInfo;
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("UA Specimen Id cannot be empty.");
                            txtUASpecimenId.Focus();
                            return false;
                        }
                        if (txtUASpecimenId.Text.Trim() != string.Empty)
                        {
                            if (!UASpecimenValidation())
                            {
                                MessageBox.Show("UA Specimen Id already exists.");
                                return false;
                            }
                        }
                    }

                    if ((!chkHair.Enabled) && chkHair.Checked)
                    {
                        if (txtHairSpecimenId.Text.Trim() == string.Empty)
                        {
                            tcMain.SelectedTab = tabTestInfo;
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("Hair Specimen Id cannot be empty.");
                            txtUASpecimenId.Focus();
                            return false;
                        }
                        if (txtHairSpecimenId.Text.Trim() != string.Empty)
                        {
                            if (!HairSpecimenValidation())
                            {
                                MessageBox.Show(" Hair Specimen Id already exists.");
                                return false;
                            }
                        }
                    }
                    if (txtHairSpecimenId.Text.Trim() != string.Empty && txtUASpecimenId.Text.Trim() != string.Empty)
                    {
                        if (txtHairSpecimenId.Text.Trim() == txtUASpecimenId.Text.Trim())
                        {
                            MessageBox.Show("Specimen Id cannot be same.");
                            return false;
                        }
                    }
                }

                if (chkInstant.Checked == true)
                {
                    if (rbPositive.Checked == false && rbNegative.Checked == false)
                    {
                        MessageBox.Show("Test Result Must be Selected.");
                        return false;
                    }
                }
            }

            return true;
        }

        private bool DoSpecimenIdValidation()
        {
            DonorTestInfo donorTestInfo = new DonorTestInfo();
            donorTestInfo = donorBL.GetDonorTestInfoByDonorIdDonorTestInfoId(this.currentDonorId, this.currentTestInfoId);
            if (donorTestInfo != null)
            {
                if (donorTestInfo.IsReverseEntry == false)
                {
                    lblUASpecimenId.Visible = false;
                    lblUASpecimenMan.Visible = false;
                    txtUASpecimenId.Visible = false;
                    txtUASpecimenId.Enabled = false;

                    lblHairSpecimenId.Visible = false;
                    lblHairSpecimenMan.Visible = false;
                    txtHairSpecimenId.Visible = false;
                    txtHairSpecimenId.Enabled = false;

                    txtScreeningDate.Text = string.Empty;
                    txtScreeningTime.Text = string.Empty;

                    //Reason For Test
                    if (!(rbPreEmployment.Checked
                            || rbRandom.Checked
                            || rbReasonable.Checked
                            || rbPostAccident.Checked
                            || rbReturntoDuty.Checked
                            || rbFollowUp.Checked
                            || rbOther.Checked))
                    {
                        return false;
                    }

                    if (rbOther.Checked && txtReason.Text.Trim() == string.Empty)
                    {
                        return false;
                    }

                    //Specimen Collectin Cup
                    if (!(rbUrineSingle.Checked
                        || rbUrineSplit.Checked
                        || rbSaliva.Checked
                        || rbBlood.Checked))
                    {
                        return false;
                    }

                    //Observed
                    if (!(rbObservedYes.Checked
                        || rbObservedNo.Checked))
                    {
                        return false;
                    }

                    //Form Type
                    if (!(rbFederal.Checked
                        || rbNonFederal.Checked))
                    {
                        return false;
                    }

                    //Testing Authority
                    if (rbFederal.Checked && cmbTestingAuthority.SelectedIndex == 0)
                    {
                        return false;
                    }

                    if (!rbTemperatureYes.Checked)
                    {
                        return false;
                    }

                    if (!rbAdulterationNo.Checked)
                    {
                        return false;
                    }

                    if (!rbQNSYes.Checked)
                    {
                        return false;
                    }

                    if (chkUrinalysis.Checked)
                    {
                        lblUASpecimenId.Visible = true;
                        lblUASpecimenMan.Visible = true;
                        txtUASpecimenId.Visible = true;
                        txtUASpecimenId.Enabled = true;
                    }

                    if (chkHair.Checked)
                    {
                        lblHairSpecimenId.Visible = true;
                        lblHairSpecimenMan.Visible = true;
                        txtHairSpecimenId.Visible = true;
                        txtHairSpecimenId.Enabled = true;
                    }

                    txtScreeningDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    txtScreeningTime.Text = DateTime.Now.ToString("hh:mm tt");
                }
                else if (donorTestInfo.IsReverseEntry == true)
                {
                    lblUASpecimenId.Visible = false;
                    lblUASpecimenMan.Visible = false;
                    txtUASpecimenId.Visible = false;
                    txtUASpecimenId.Enabled = false;

                    lblHairSpecimenId.Visible = false;
                    lblHairSpecimenMan.Visible = false;
                    txtHairSpecimenId.Visible = false;
                    txtHairSpecimenId.Enabled = false;

                    txtScreeningDate.Text = string.Empty;
                    txtScreeningTime.Text = string.Empty;

                    //Reason For Test
                    if (!(rbPreEmployment.Checked
                            || rbRandom.Checked
                            || rbReasonable.Checked
                            || rbPostAccident.Checked
                            || rbReturntoDuty.Checked
                            || rbFollowUp.Checked
                            || rbOther.Checked))
                    {
                        return false;
                    }

                    if (rbOther.Checked && txtReason.Text.Trim() == string.Empty)
                    {
                        return false;
                    }

                    //Specimen Collectin Cup
                    if (!(rbUrineSingle.Checked
                        || rbUrineSplit.Checked
                        || rbSaliva.Checked
                        || rbBlood.Checked))
                    {
                        return false;
                    }

                    //Observed
                    if (!(rbObservedYes.Checked
                        || rbObservedNo.Checked))
                    {
                        return false;
                    }

                    //Form Type
                    if (!(rbFederal.Checked
                        || rbNonFederal.Checked))
                    {
                        return false;
                    }

                    //Testing Authority
                    if (rbFederal.Checked && cmbTestingAuthority.SelectedIndex == 0)
                    {
                        return false;
                    }

                    if (!rbTemperatureYes.Checked)
                    {
                        return false;
                    }

                    if (!rbAdulterationNo.Checked)
                    {
                        return false;
                    }

                    if (!rbQNSYes.Checked)
                    {
                        return false;
                    }

                    if (chkUrinalysis.Checked)
                    {
                        lblUASpecimenId.Visible = true;
                        lblUASpecimenMan.Visible = true;
                        txtUASpecimenId.Visible = true;
                        txtUASpecimenId.Enabled = true;
                    }

                    if (chkHair.Checked)
                    {
                        lblHairSpecimenId.Visible = true;
                        lblHairSpecimenMan.Visible = true;
                        txtHairSpecimenId.Visible = true;
                        txtHairSpecimenId.Enabled = true;
                    }
                    txtScreeningDate.Text = Convert.ToDateTime(donorTestInfo.ScreeningTime).ToString("MM/dd/yyyy");
                    txtScreeningTime.Text = Convert.ToDateTime(donorTestInfo.ScreeningTime).ToString("hh:mm tt");
                }
            }

            return true;
        }

        private bool ValidateActivityDetails()
        {
            return true;
        }

        private bool ValidateLegalDetails()
        {
            if (cmbAttorneyName1.SelectedValue.ToString() != "0")
            {
                if (cmbAttorneyName1.SelectedValue.ToString() == cmbAttorneyName2.SelectedValue.ToString())
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("This Attorney Name already Exists.");
                    cmbAttorneyName2.Focus();
                    return false;
                }
            }
            if (cmbAttorneyName1.SelectedValue.ToString() != "0")
            {
                if (!AttorneyActive(cmbAttorneyName1))
                {
                    return false;
                }
            }

            if (cmbAttorneyName2.SelectedValue.ToString() != "0")
            {
                if (cmbAttorneyName2.SelectedValue.ToString() == cmbAttorneyName3.SelectedValue.ToString())
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("This Attorney Name already Exists.");
                    cmbAttorneyName3.Focus();
                    return false;
                }
            }
            if (cmbAttorneyName2.SelectedValue.ToString() != "0")
            {
                if (!AttorneyActive(cmbAttorneyName2))
                {
                    return false;
                }
            }

            if (cmbAttorneyName3.SelectedValue.ToString() != "0")
            {
                if (cmbAttorneyName1.SelectedValue.ToString() == cmbAttorneyName3.SelectedValue.ToString())
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("This Attorney Name already Exists.");
                    cmbAttorneyName1.Focus();
                    return false;
                }
            }
            if (cmbAttorneyName3.SelectedValue.ToString() != "0")
            {
                if (!AttorneyActive(cmbAttorneyName3))
                {
                    return false;
                }
            }

            if (cmbThirdPartyInfo1Name.SelectedValue.ToString() != "0")
            {
                if (cmbThirdPartyInfo1Name.SelectedValue.ToString() == cmbThirdPartyInfo2Name.SelectedValue.ToString())
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("This ThirdParty Name already Exists.");
                    cmbThirdPartyInfo1Name.Focus();
                    return false;
                }
            }
            if (cmbThirdPartyInfo1Name.SelectedValue.ToString() != "0")
            {
                if (!ThirdPartyActive(cmbThirdPartyInfo1Name))
                {
                    return false;
                }
            }
            if (cmbThirdPartyInfo2Name.SelectedValue.ToString() != "0")
            {
                if (!ThirdPartyActive(cmbThirdPartyInfo2Name))
                {
                    return false;
                }
            }
            if (cmbCourtName.SelectedValue.ToString() != "0")
            {
                if (!CourtActive(cmbCourtName))
                {
                    return false;
                }
            }
            if (cmbJudgeName.SelectedValue.ToString() != "0")
            {
                if (!JudgeActive(cmbJudgeName))
                {
                    return false;
                }
            }
            if (cmbLegalStartDate.Text != "DD" || cmbLegalStartMonth.Text != "MM" || cmbLegalStartYear.Text != "YYYY" || cmbLegalEndDate.Text != "DD" || cmbLegalEndMonth.Text != "MM" || cmbLegalEndYear.Text != "YYYY")
            {
                if (cmbLegalStartDate.Text == "DD" || cmbLegalStartMonth.Text == "MM" || cmbLegalStartYear.Text == "YYYY" || cmbLegalEndDate.Text == "DD" || cmbLegalEndMonth.Text == "MM" || cmbLegalEndYear.Text == "YYYY")
                {
                    MessageBox.Show("Invalid format of date.");
                    return false;
                }
                else
                {
                    string inStartActiveDate = cmbLegalStartYear.Text + '-' + cmbLegalStartMonth.Text + '-' + cmbLegalStartDate.Text;
                    DateTime startDate = Convert.ToDateTime(inStartActiveDate);

                    string inEndActiveDate = cmbLegalEndYear.Text + '-' + cmbLegalEndMonth.Text + '-' + cmbLegalEndDate.Text;
                    DateTime endDate = Convert.ToDateTime(inEndActiveDate);

                    if (startDate < DateTime.Today)
                    {
                        MessageBox.Show("Invalid Date.");
                        return false;
                    }

                    if (endDate < startDate)
                    {
                        MessageBox.Show("Invalid Date.");
                        return false;
                    }
                }
            }

            return true;
        }

        private bool ValidateVendorDetails()
        {
            return true;
        }

        private bool ValidatePaymentDetails(DonorTestInfo donorTestInfo)
        {
            if (donorTestInfo.PaymentStatus == PaymentStatus.None || donorTestInfo.PaymentStatus == PaymentStatus.Pending)
            {
                if (txtPaymentAmount.Text.Trim() != string.Empty && Convert.ToDouble(txtPaymentAmount.Text.Trim()) > 0)
                {
                    if (rbtnPaymentCash.Checked == false && rbtnPaymentCard.Checked == false)
                    {
                        tcMain.SelectedTab = tabPayment;
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Payment Method must be selected.");
                        return false;
                    }

                    DateTime PaymentDate = Convert.ToDateTime(cmbPaymentMonth.Text + '-' + cmbPaymentDate.Text + '-' + cmbPaymentYear.Text);
                    if (PaymentDate < DateTime.Today)
                    {
                        tcMain.SelectedTab = tabPayment;
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Invalid Payment Date.");
                        return false;
                    }

                    if (chkPayment.Checked == false)
                    {
                        MessageBox.Show("Accept payment must be checked.");
                        return false;
                    }
                }
            }
            return true;
        }

        private bool ValidateReverseEntryTestInfoDetails()
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
                MessageBox.Show(" Donor Email cannot be empty.");
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
            if (txtSSN.Tag.ToString().Replace("_", "").Replace("-", "").Trim() == string.Empty || txtSSN.Text.Trim() == string.Empty)
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

            if (txtSSN.Text.ToString().Contains(" ") || txtSSN.Tag.ToString().Length < 11)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Invalid Format of SSN.");
                txtSSN.Focus();
                return false;
            }
            //if (rbtnFemale.Checked == false || rbtnFemale.Checked == false)
            //{
            //    Cursor.Current = Cursors.Default;
            //    MessageBox.Show("Gender cannot be empty.");
            //    //  txtSSN.Focus();
            //    return false;
            //}

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

            //TestInfo
            if (chkDonorRefuses.Checked)
            {
                return true;
            }

            if (!(chkUrinalysis.Checked || chkHair.Checked || chkDNA.Checked))
            {
                tcMain.SelectedTab = tabTestInfo;
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Test Category must be selected.");
                chkUrinalysis.Focus();
                return false;
            }

            if (chkUrinalysis.Checked || chkHair.Checked || chkDNA.Checked)
            {
                if (chkHair.Checked)
                {
                    if (cmbDays.SelectedIndex == 0)
                    {
                        tcMain.SelectedTab = tabTestInfo;
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("# of Days must be selected.");
                        cmbDays.Focus();
                        return false;
                    }
                }

                //Reason For Test
                if (!(rbPreEmployment.Checked
                    || rbRandom.Checked
                    || rbReasonable.Checked
                    || rbPostAccident.Checked
                    || rbReturntoDuty.Checked
                    || rbFollowUp.Checked
                    || rbOther.Checked))
                {
                    tcMain.SelectedTab = tabTestInfo;
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Reason For Test must be selected.");
                    rbPreEmployment.Focus();
                    return false;
                }

                if (rbOther.Checked && txtReason.Text.Trim() == string.Empty)
                {
                    tcMain.SelectedTab = tabTestInfo;
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Other Reason cannot be empty.");
                    txtReason.Focus();
                    return false;
                }

                //Specimen Collectin Cup
                if (!(rbUrineSingle.Checked
                    || rbUrineSplit.Checked
                    || rbSaliva.Checked
                    || rbBlood.Checked))
                {
                    tcMain.SelectedTab = tabTestInfo;
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Specimen Collectin Cup must be selected.");
                    return false;
                }

                //Observed
                if (!(rbObservedYes.Checked
                    || rbObservedNo.Checked))
                {
                    tcMain.SelectedTab = tabTestInfo;
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Observed must be selected.");
                    return false;
                }

                //Form Type
                if (!(rbFederal.Checked
                    || rbNonFederal.Checked))
                {
                    tcMain.SelectedTab = tabTestInfo;
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Form Type must be selected.");
                    return false;
                }

                //Testing Authority
                if (rbFederal.Checked && cmbTestingAuthority.SelectedIndex == 0)
                {
                    tcMain.SelectedTab = tabTestInfo;
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Testing Authority must be selected.");
                    return false;
                }

                //Temperature
                //if (chkUrinalysis.Checked)
                //{
                //if (!(rbTemperatureYes.Checked
                //    || rbTemperatureNo.Checked))
                //{
                //    tcMain.SelectedTab = tabTestInfo;
                //    Cursor.Current = Cursors.Default;
                //    MessageBox.Show("Specimen Temperature must be confirmed.");
                //    return false;
                //}

                if (rbTemperatureNo.Checked && txtTemperature.Text.Trim() == string.Empty)
                {
                    tcMain.SelectedTab = tabTestInfo;
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Temperature cannot be empty.");
                    txtTemperature.Focus();
                    return false;
                }

                if (txtTemperature.Text.Trim() != string.Empty)
                {
                    try
                    {
                        double temperature = Convert.ToDouble(txtTemperature.Text.Trim());

                        if (rbTemperatureNo.Checked)
                        {
                            if (temperature >= 90 && temperature <= 100)
                            {
                                tcMain.SelectedTab = tabTestInfo;
                                Cursor.Current = Cursors.Default;
                                MessageBox.Show("Invalid range of Temperature.");
                                txtTemperature.Focus();
                                return false;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        tcMain.SelectedTab = tabTestInfo;
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Invalid format of Temperature.");
                        txtTemperature.Focus();
                        return false;
                    }
                }

                if (rbTemperatureYes.Checked
                    && rbAdulterationNo.Checked
                    && rbQNSYes.Checked)
                {
                    if ((!chkUrinalysis.Enabled) && chkUrinalysis.Checked)
                    {
                        if (txtUASpecimenId.Text.Trim() == string.Empty)
                        {
                            tcMain.SelectedTab = tabTestInfo;
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("UA Specimen Id cannot be empty.");
                            txtUASpecimenId.Focus();
                            return false;
                        }
                        //if (txtUASpecimenId.Text.Trim() != string.Empty)
                        //{
                        //    if (!UASpecimenValidation())
                        //    {
                        //        MessageBox.Show("UA Specimen Id already exists.");
                        //        return false;

                        //    }
                        //}
                    }

                    if ((!chkHair.Enabled) && chkHair.Checked)
                    {
                        if (txtHairSpecimenId.Text.Trim() == string.Empty)
                        {
                            tcMain.SelectedTab = tabTestInfo;
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("Hair Specimen Id cannot be empty.");
                            txtUASpecimenId.Focus();
                            return false;
                        }
                        //if (txtHairSpecimenId.Text.Trim() != string.Empty)
                        //{
                        //    if (!HairSpecimenValidation())
                        //    {
                        //        MessageBox.Show(" Hair Specimen Id already exists.");
                        //        return false;

                        //    }
                        //}
                    }
                    if (txtHairSpecimenId.Text.Trim() != string.Empty && txtUASpecimenId.Text.Trim() != string.Empty)
                    {
                        if (txtHairSpecimenId.Text.Trim() == txtUASpecimenId.Text.Trim())
                        {
                            MessageBox.Show("Specimen Id cannot be same.");
                            return false;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Select a test questions.");
                    rbTemperatureYes.Focus();
                    return false;
                }

                if (chkInstant.Checked == true)
                {
                    if (rbPositive.Checked == false && rbNegative.Checked == false)
                    {
                        MessageBox.Show("Test Result Must be Selected.");
                        return false;
                    }
                }
                if (cmbLocationName.SelectedIndex == 0)
                {
                    tcMain.SelectedTab = tabTestInfo;
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Location Name must be selected.");
                    return false;
                }

                if (cmbCollectionName.SelectedIndex == 0)
                {
                    tcMain.SelectedTab = tabTestInfo;
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Collector Name must be selected.");
                    return false;
                }
            }

            return true;
        }

        #endregion UI Validation

        #region UI to DB

        private bool SavePaymentDetails(bool validateFlag, DonorTestInfo donorTestInfo)
        {
            if (!validateFlag)
            {
                if (!ValidatePaymentDetails(donorTestInfo))
                {
                    return false;
                }
            }

            if (donorTestInfo.PaymentStatus != PaymentStatus.Paid)
            {
                if (donorTestInfo.PaymentStatus == PaymentStatus.None || donorTestInfo.PaymentStatus == PaymentStatus.Pending)
                {
                    if (txtPaymentAmount.Text.Trim() != string.Empty && Convert.ToDouble(txtPaymentAmount.Text.Trim()) > 0)
                    {
                        //donorTestInfo.PaymentDate = dtpPaymentDate.Value;
                        string inPaymentDate = cmbPaymentMonth.Text + '-' + cmbPaymentDate.Text + '-' + cmbPaymentYear.Text;
                        donorTestInfo.PaymentDate = Convert.ToDateTime(inPaymentDate);

                        if (rbtnPaymentCash.Checked)
                        {
                            donorTestInfo.PaymentMethodId = PaymentMethod.Cash;
                        }
                        else if (rbtnPaymentCard.Checked)
                        {
                            donorTestInfo.PaymentMethodId = PaymentMethod.Card;
                        }
                        else
                        {
                            donorTestInfo.PaymentMethodId = PaymentMethod.None;
                        }

                        if (chkPayment.Checked == true)
                        {
                            donorTestInfo.IsPaymentReceived = true;
                        }
                        else
                        {
                            donorTestInfo.IsPaymentReceived = false;
                        }

                        donorTestInfo.PaymentNote = txtPaymentNote.Text.Trim();
                        donorTestInfo.LastModifiedBy = Program.currentUserName;

                        donorBL.SavePaymentDetails(donorTestInfo);

                        //Payment Confirmation Mail

                        // DonorBL donorBL = new DonorBL();

                        ClientBL clientBL = new ClientBL();
                        Donor donor = donorBL.Get(donorTestInfo.DonorId, "Desktop");

                        ClientDepartment clientDepartment = clientBL.GetClientDepartment(Convert.ToInt32(donorTestInfo.ClientDepartmentId));

                        Client client = clientBL.Get(clientDepartment.ClientId);

                        if (donor.DonorEmail != string.Empty)
                        {
                            try
                            {
                                MailManager mailManager = new MailManager();
                                string mailBody = mailManager.SendDonorPaymentConfirmationMail(donor, donorTestInfo, clientDepartment, client);
                                mailManager.SendMail(donor.DonorEmail, ConfigurationManager.AppSettings["PaymentConfirmationMailSubject"].ToString().Trim(), mailBody);
                            }
                            catch
                            {
                                throw new Exception("Not able to send mail.");
                            }
                        }

                        lblStatus.Text = donorTestInfo.TestStatus.ToDescriptionString();
                        // dtpPaymentDate.Enabled = false;
                        lblpaymsg.Visible = false;
                        cmbPaymentMonth.Enabled = false;
                        cmbPaymentDate.Enabled = false;
                        cmbPaymentYear.Enabled = false;
                        chkPayment.Enabled = false;
                        rbtnPaymentCash.Enabled = false;
                        rbtnPaymentCard.Enabled = false;
                        txtPaymentNote.Enabled = false;
                        btnPaymentSave.Enabled = false;

                        LoadActivityDetails();
                    }
                }
            }
            else
            {
                // dtpPaymentDate.Enabled = false;
                lblpaymsg.Visible = false;
                cmbPaymentMonth.Enabled = false;
                cmbPaymentDate.Enabled = false;
                cmbPaymentYear.Enabled = false;

                rbtnPaymentCash.Enabled = false;
                rbtnPaymentCard.Enabled = false;
                txtPaymentNote.Enabled = false;
                btnPaymentSave.Enabled = false;
                chkPayment.Enabled = false;
            }

            return true;
        }

        private bool SaveReversePaymentDetails(bool validateFlag, DonorTestInfo donorTestInfo)
        {
            //if (!validateFlag)
            //{
            //    if (!ValidatePaymentDetails(donorTestInfo))
            //    {
            //        return false;
            //    }
            //}

            if (donorTestInfo.PaymentStatus != PaymentStatus.Paid)
            {
                if (donorTestInfo.PaymentStatus == PaymentStatus.None || donorTestInfo.PaymentStatus == PaymentStatus.Pending)
                {
                    if (txtPaymentAmount.Text.Trim() != string.Empty && Convert.ToDouble(txtPaymentAmount.Text.Trim()) > 0)
                    {
                        //donorTestInfo.PaymentDate = dtpPaymentDate.Value;
                        string inPaymentDate = cmbPaymentMonth.Text + '-' + cmbPaymentDate.Text + '-' + cmbPaymentYear.Text;
                        donorTestInfo.PaymentDate = Convert.ToDateTime(inPaymentDate);

                        donorTestInfo.PaymentMethodId = PaymentMethod.Cash;

                        donorTestInfo.IsPaymentReceived = true;

                        donorTestInfo.PaymentNote = txtPaymentNote.Text.Trim();
                        donorTestInfo.LastModifiedBy = Program.currentUserName;

                        donorBL.SaveReversePaymentDetails(donorTestInfo);

                        // dtpPaymentDate.Enabled = false;
                        lblpaymsg.Visible = false;
                        cmbPaymentMonth.Enabled = false;
                        cmbPaymentDate.Enabled = false;
                        cmbPaymentYear.Enabled = false;
                        chkPayment.Enabled = false;
                        rbtnPaymentCash.Enabled = false;
                        rbtnPaymentCard.Enabled = false;
                        txtPaymentNote.Enabled = false;
                        btnPaymentSave.Enabled = false;

                        LoadActivityDetails();
                    }
                }
            }
            else
            {
                // dtpPaymentDate.Enabled = false;
                lblpaymsg.Visible = false;
                cmbPaymentMonth.Enabled = false;
                cmbPaymentDate.Enabled = false;
                cmbPaymentYear.Enabled = false;

                rbtnPaymentCash.Enabled = false;
                rbtnPaymentCard.Enabled = false;
                txtPaymentNote.Enabled = false;
                btnPaymentSave.Enabled = false;
                chkPayment.Enabled = false;
            }

            return true;
        }

        private bool SavePreregisteredDonorDetails(bool validateFlag)
        {
            if (!validateFlag)
            {
                if (!ValidatePreRegisteredDonor())
                {
                    return false;
                }
            }

            Donor donor = donorBL.Get(this.currentDonorId, "Desktop");

            donor.DonorEmail = txtEmail.Text.Trim();
            donor.LastModifiedBy = Program.currentUserName;

            int returnVal = 0;
            returnVal = donorBL.Save(donor);

            return true;
        }

        private bool SaveDonorDetails(bool validateFlag)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (!validateFlag)
            {
                if (!ValidateDonorDetails())
                {
                    return false;
                }
            }

            Donor donor = donorBL.Get(this.currentDonorId, "Desktop");

            donor.DonorFirstName = txtFirstName.Text.Trim();
            donor.DonorClearStarProfId = txtClearStarProfileId.Text.Trim();
            donor.DonorMI = txtMiddleInitial.Text.Trim();
            donor.DonorLastName = txtLastName.Text.Trim();
            donor.IsHiddenWeb = this.chkHideWeb.Checked;

            if (cmbSuffix.SelectedIndex != 0)
            {
                donor.DonorSuffix = cmbSuffix.Text.Trim();
            }

            if (!txtSSN.CausesValidation)
            {
                if (txtSSN.Text.Length >= 9)
                {
                    string Unmask1 = "***-**-" + txtSSN.Text.ToString().Substring(7);
                    string NewSSN = string.Empty;
                    string NewSSN1 = string.Empty;
                    string NewSSN2 = string.Empty;

                    if (txtSSN.Text.Length == 9)
                    {
                        if (txtSSN.Text.Trim() != string.Empty)
                        {
                            NewSSN = txtSSN.Text.Substring(0, 3);
                            NewSSN1 = txtSSN.Text.Substring(3, 2);
                            NewSSN2 = txtSSN.Text.Substring(5, 4);
                            string Unmask = NewSSN + "-" + NewSSN1 + "-" + NewSSN2;
                            //  Unmask = "***-**-" + Unmask.ToString().Substring(7);

                            donor.DonorSSN = Unmask.ToString();
                            txtSSN.Tag = donor.DonorSSN;
                        }
                    }
                    else
                    {
                        if (txtSSN.Text.Trim() != Unmask1)
                        {
                            donor.DonorSSN = txtSSN.Text.ToString();
                            txtSSN.Tag = donor.DonorSSN;
                        }
                        else
                        {
                            donor.DonorSSN = txtSSN.Tag.ToString();
                            txtSSN.Tag = donor.DonorSSN;
                        }
                    }

                    if (txtSSN.Text.Length >= 9)
                    {
                        donor.DonorSSN = txtSSN.Tag.ToString();
                        txtSSN.Tag = donor.DonorSSN;
                    }
                    else if (pbUnmaskSSN.Tag.ToString() == "Unmask SSN")
                    {
                        pbUnmaskSSN.Image = global::SurPath.Properties.Resources.unmask_SSN;
                        donor.DonorSSN = txtSSN.Tag.ToString().Trim();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Format of SSN.");
                    txtSSN.Focus();
                    return false;
                }
            }

            string inActiveDate = cmbDonorYear.Text + '-' + cmbDonorMonth.Text + '-' + cmbDonorDate.Text;
            try
            {
                //DateTime inactiveDate = Convert.ToDateTime(inActiveDate);
                donor.DonorDateOfBirth = Convert.ToDateTime(inActiveDate.Trim());
            }
            catch
            {
                MessageBox.Show("Invalid Date");
                cmbDonorMonth.Focus();
                return false;
            }
            DateTime inactiveDate = Convert.ToDateTime(inActiveDate.ToString());

            if (inactiveDate > DateTime.Today || inactiveDate < DateTime.Today.AddYears(-125))
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Invalid Date.");
                cmbDonorMonth.Focus();
                return false;
            }
            else
            {
                donor.DonorDateOfBirth = Convert.ToDateTime(inActiveDate.Trim());
            }

            //   donor.DonorDateOfBirth = Convert.ToDateTime(txtDOB.Text.Trim());
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
            donor.LastModifiedBy = Program.currentUserName;
            //bool _WasZeroDept = false;
            // If donor.DonorInitialDepartmentId is 0 - due to registration bug missing DonorInitialDepartmentId
            //if (donor.DonorInitialDepartmentId==0)
            //{
            //    _WasZeroDept = true;
            //    donor.DonorInitialDepartmentId = (int)cmbDepartment.SelectedValue;
            //}

            //Check To see if the original codes
            if ((donor.DonorInitialClientId != (int)cmbClient.SelectedValue) || (donor.DonorInitialDepartmentId != (int)cmbDepartment.SelectedValue))
            {
                donor.DonorInitialClientId = (int)cmbClient.SelectedValue;
                donor.DonorInitialDepartmentId = (int)cmbDepartment.SelectedValue;
                if (this.cmbDepartment.Visible && this.cmbClient.Visible)
                {
                    this.CreateNoteWithText(" " + "Change: ClientDept From:" + this.txtClient.Text + "/" + txtDepartment.Text);
                    this.CreateNoteWithText(" " + "To: " + this.cmbClient.Text + "/" + this.cmbDepartment.Text);
                }

                this.cmbClient.Visible = false;
                this.cmbDepartment.Visible = false;
                donorBL.UpdateTestToArchive(txtUASpecimenId.Text);

                //if (_WasZeroDept==false) donorBL.UpdateTestStatustoProcessing(donor.DonorId);
                //donor.fe
                //MessageBox.Show("The Client and Department have changed from what was originally entered");
            }
            //make a log entry

            int returnVal = 0;
            returnVal = donorBL.Save(donor);
            Program._logger.Debug("After save, calling LoadTabDetails");
            LoadDonorDetails();
            Cursor.Current = Cursors.Default;
            return true;
        }

        private bool SaveTestInfoDetails(bool validateFlag, DonorTestInfo donorTestInfo)
        {
            //Test Category & Test Panel
            int htPanelDays = 0;
            donorTestInfo.IsUA = false;
            donorTestInfo.IsHair = false;
            donorTestInfo.IsDNA = false;
            donorTestInfo.IsBC = false;

            bool canUpdateHTPDays = false;
            if (donorTestInfo.PaymentStatus != PaymentStatus.Paid)
            {
                if (chkUrinalysis.Enabled && chkUrinalysis.Checked)
                {
                    donorTestInfo.IsUA = true;
                }

                if (chkHair.Enabled && chkHair.Checked)
                {
                    donorTestInfo.IsHair = true;

                    foreach (DonorTestInfoTestCategories testCategory in donorTestInfo.TestInfoTestCategories)
                    {
                        if (testCategory.TestCategoryId == TestCategories.Hair)
                        {
                            testCategory.HairTestPanelDays = Convert.ToInt32(cmbDays.SelectedIndex) * 90;
                            break;
                        }
                    }
                }

                if (chkDNA.Enabled && chkDNA.Checked)
                {
                    donorTestInfo.IsDNA = true;
                }
                if (chkBC.Enabled && chkBC.Checked)
                {
                    donorTestInfo.IsBC = true;
                }
            }
            Client clients = clientBL.Get(donorTestInfo.ClientId);

            if (donorTestInfo.PaymentStatus == PaymentStatus.Paid || (donorTestInfo.PaymentStatus != PaymentStatus.Paid && clients.CanEditTestCategory == false))
            {
                if ((!chkUrinalysis.Enabled) && chkUrinalysis.Checked)
                {
                    donorTestInfo.IsUA = true;
                }

                if ((!chkHair.Enabled) && chkHair.Checked)
                {
                    donorTestInfo.IsHair = true;

                    foreach (DonorTestInfoTestCategories testCategory in donorTestInfo.TestInfoTestCategories)
                    {
                        if (testCategory.TestCategoryId == TestCategories.Hair)
                        {
                            testCategory.HairTestPanelDays = Convert.ToInt32(cmbDays.SelectedIndex) * 90;
                            break;
                        }
                    }
                }

                if ((!chkDNA.Enabled) && chkDNA.Checked)
                {
                    donorTestInfo.IsDNA = true;
                }
                if ((!chkBC.Enabled) && chkBC.Checked)
                {
                    donorTestInfo.IsBC = true;
                }
            }

            if (!(donorTestInfo.TestStatus == DonorRegistrationStatus.Registered
                    || donorTestInfo.TestStatus == DonorRegistrationStatus.InQueue
                    || donorTestInfo.TestStatus == DonorRegistrationStatus.SuspensionQueue))
            {
                if (chkUrinalysis.Checked)
                {
                    if (txtUASpecimenId.Text.Trim() == string.Empty)
                    {
                        tcMain.SelectedTab = tabTestInfo;
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("UA Specimen Id cannot be empty.");
                        txtUASpecimenId.Focus();
                        return false;
                    }
                    if (txtUASpecimenId.Text.Trim() != string.Empty)
                    {
                        if (!UASpecimenValidation())
                        {
                            MessageBox.Show(" UA Specimen Id already exists.");
                            return false;
                        }
                    }

                    foreach (DonorTestInfoTestCategories testCategory in donorTestInfo.TestInfoTestCategories)
                    {
                        if (testCategory.TestCategoryId == TestCategories.UA)
                        {
                            testCategory.SpecimenId = txtUASpecimenId.Text.Trim();
                            break;
                        }
                    }
                }

                if (chkHair.Checked)
                {
                    if (txtHairSpecimenId.Text.Trim() == string.Empty)
                    {
                        tcMain.SelectedTab = tabTestInfo;
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Hair Specimen Id cannot be empty.");
                        txtUASpecimenId.Focus();
                        return false;
                    }
                    if (txtHairSpecimenId.Text.Trim() != string.Empty)
                    {
                        if (!HairSpecimenValidation())
                        {
                            MessageBox.Show("Hair Specimen Id already exists.");
                            return false;
                        }
                    }

                    foreach (DonorTestInfoTestCategories testCategory in donorTestInfo.TestInfoTestCategories)
                    {
                        if (testCategory.TestCategoryId == TestCategories.Hair)
                        {
                            testCategory.SpecimenId = txtHairSpecimenId.Text.Trim();
                            break;
                        }
                    }
                }

                donorTestInfo.LastModifiedBy = Program.currentUserName;

                donorBL.UpdateTestInfoSpecimenIDs(donorTestInfo);
            }
            else
            {
                if (!validateFlag)
                {
                    if (donorTestInfo.PaymentStatus != PaymentStatus.Paid)
                    {
                        if (chkHair.Enabled && chkHair.Checked)
                        {
                            donorTestInfo.IsHair = true;

                            foreach (DonorTestInfoTestCategories testCategory in donorTestInfo.TestInfoTestCategories)
                            {
                                if (testCategory.TestCategoryId == TestCategories.Hair)
                                {
                                    testCategory.HairTestPanelDays = Convert.ToInt32(cmbDays.SelectedIndex) * 90;
                                    if (testCategory.HairTestPanelDays == 0)
                                    {
                                        MessageBox.Show("# of Days must be selected.");
                                        return false;
                                    }
                                    htPanelDays = Convert.ToInt32(testCategory.HairTestPanelDays);
                                    canUpdateHTPDays = true;
                                    break;
                                }
                            }
                        }
                    }
                    clients = clientBL.Get(donorTestInfo.ClientId);
                    if (donorTestInfo.PaymentStatus != PaymentStatus.Paid && clients.CanEditTestCategory == false)
                    {
                        if ((!chkHair.Enabled) && chkHair.Checked)
                        {
                            donorTestInfo.IsHair = true;

                            foreach (DonorTestInfoTestCategories testCategory in donorTestInfo.TestInfoTestCategories)
                            {
                                if (testCategory.TestCategoryId == TestCategories.Hair)
                                {
                                    testCategory.HairTestPanelDays = Convert.ToInt32(cmbDays.SelectedIndex) * 90;
                                    if (testCategory.HairTestPanelDays == 0)
                                    {
                                        MessageBox.Show("# of Days must be selected.");
                                        return false;
                                    }
                                    htPanelDays = Convert.ToInt32(testCategory.HairTestPanelDays);
                                    canUpdateHTPDays = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (donorTestInfo.PaymentStatus != PaymentStatus.Paid)
                    {
                        clientBL = new ClientBL();
                        //  ClientDepartment clientDepartment = clientBL.GetClientDepartment(Convert.ToInt32(donor.DonorInitialDepartmentId));

                        Client client = clientBL.Get(donorTestInfo.ClientId);
                        if (client.CanEditTestCategory == true)
                        {
                            donorBL.SaveTestInfoDetailsBeforPayment(donorTestInfo);
                            if (this.currentTestInfoId > 0)
                            {
                                donorTestInfo = donorBL.GetDonorTestInfo(this.currentTestInfoId);
                                if (donorTestInfo != null)
                                {
                                    testflag = true;
                                    LoadPaymentDetails(donorTestInfo);
                                    LoadTestInfoDetails(donorTestInfo);
                                }
                            }
                            MessageBox.Show("Test panel details updated. Note : Specimen collection process will not be initiated until the donor completes payment process.");
                            //return false;
                        }
                        else
                        {
                            //  MessageBox.Show("Specimen collection process will not be initiated until the donor completes payment process.");
                            //return false;
                        }
                        if (canUpdateHTPDays == true)
                        {
                            if (htPanelDays > 0 && donorTestInfo.PaymentStatus != PaymentStatus.Paid && donorTestInfo.PaymentTypeId == ClientPaymentTypes.DonorPays)
                            {
                                try
                                {
                                    donorBL.UpdateHairTestPanelDays(donorTestInfo);
                                    LoadPaymentDetails(donorTestInfo);
                                    LoadActivityDetails();
                                    if (client.CanEditTestCategory != true)
                                    {
                                        MessageBox.Show("Test panel details updated. Note : Specimen collection process will not be initiated until the donor completes payment process.");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Unable to update the hair test panel days.");
                                    //return false;
                                }
                            }
                        }
                        return false;
                    }
                    else
                    {
                        if (!ValidateTestInfoDetails())
                        {
                            return false;
                        }
                    }
                }

                string refuessReason = string.Empty;

                if (chkDonorRefuses.Checked)
                {
                    if (MessageBox.Show("Are you sure? The donor refuessed to take the test?", "SurPath Drug Testing", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                    {
                        return false;
                    }
                    else
                    {
                        //refuessReason =
                    }
                }

                //Reason for test
                donorTestInfo.OtherReason = null;
                if (rbPreEmployment.Checked)
                {
                    donorTestInfo.ReasonForTestId = TestInfoReasonForTest.PreEmployment;
                }
                else if (rbRandom.Checked)
                {
                    donorTestInfo.ReasonForTestId = TestInfoReasonForTest.Random;
                }
                else if (rbReasonable.Checked)
                {
                    donorTestInfo.ReasonForTestId = TestInfoReasonForTest.ReasonableSuspicionCause;
                }
                else if (rbPostAccident.Checked)
                {
                    donorTestInfo.ReasonForTestId = TestInfoReasonForTest.PostAccident;
                }
                else if (rbReturntoDuty.Checked)
                {
                    donorTestInfo.ReasonForTestId = TestInfoReasonForTest.ReturnToDuty;
                }
                else if (rbFollowUp.Checked)
                {
                    donorTestInfo.ReasonForTestId = TestInfoReasonForTest.FollowUp;
                }
                else if (rbOther.Checked)
                {
                    donorTestInfo.ReasonForTestId = TestInfoReasonForTest.Other;
                    donorTestInfo.OtherReason = txtReason.Text.Trim();
                }
                else
                {
                    donorTestInfo.ReasonForTestId = TestInfoReasonForTest.None;
                }

                //Specimen Collection Cup
                if (rbUrineSingle.Checked)
                {
                    donorTestInfo.SpecimenCollectionCupId = SpecimenCollectionCupType.UrineSingle;
                }
                else if (rbUrineSplit.Checked)
                {
                    donorTestInfo.SpecimenCollectionCupId = SpecimenCollectionCupType.UrineSplit;
                }
                else if (rbSaliva.Checked)
                {
                    donorTestInfo.SpecimenCollectionCupId = SpecimenCollectionCupType.Saliva;
                }
                else if (rbBlood.Checked)
                {
                    donorTestInfo.SpecimenCollectionCupId = SpecimenCollectionCupType.Blood;
                }
                else
                {
                    donorTestInfo.SpecimenCollectionCupId = SpecimenCollectionCupType.None;
                }

                //Observed
                if (rbObservedYes.Checked)
                {
                    donorTestInfo.IsObserved = YesNo.Yes;
                }
                else if (rbObservedNo.Checked)
                {
                    donorTestInfo.IsObserved = YesNo.No;
                }
                else
                {
                    donorTestInfo.IsObserved = YesNo.None;
                }

                //Form Type
                donorTestInfo.TestingAuthorityId = null;
                if (rbFederal.Checked)
                {
                    donorTestInfo.FormTypeId = SpecimenFormType.Federal;
                    donorTestInfo.TestingAuthorityId = Convert.ToInt32(cmbTestingAuthority.SelectedValue);
                    cmbTestingAuthority.Visible = true;
                }
                else if (rbNonFederal.Checked)
                {
                    donorTestInfo.FormTypeId = SpecimenFormType.NonFederal;
                }
                else
                {
                    donorTestInfo.FormTypeId = SpecimenFormType.None;
                }

                //Temperature
                if (rbTemperatureYes.Checked)
                {
                    donorTestInfo.IsTemperatureInRange = YesNo.Yes;
                }
                else if (rbTemperatureNo.Checked)
                {
                    donorTestInfo.IsTemperatureInRange = YesNo.No;
                    donorTestInfo.TemperatureOfSpecimen = Convert.ToDouble(txtTemperature.Text.Trim());
                }
                else
                {
                    donorTestInfo.IsTemperatureInRange = YesNo.None;
                    donorTestInfo.TemperatureOfSpecimen = null;
                }

                //Adulteration
                if (rbAdulterationYes.Checked)
                {
                    donorTestInfo.IsAdulterationSign = YesNo.Yes;
                }
                else if (rbAdulterationNo.Checked)
                {
                    donorTestInfo.IsAdulterationSign = YesNo.No;
                }
                else
                {
                    donorTestInfo.IsAdulterationSign = YesNo.None;
                }

                //Sufficient Quantity
                if (rbQNSYes.Checked)
                {
                    donorTestInfo.IsQuantitySufficient = YesNo.Yes;
                }
                else if (rbQNSNo.Checked)
                {
                    donorTestInfo.IsQuantitySufficient = YesNo.No;
                }
                else
                {
                    donorTestInfo.IsQuantitySufficient = YesNo.None;
                }

                //Test Info Status and final call
                donorTestInfo.IsDonorRefused = false;
                donorTestInfo.CollectionSiteUserId = null;
                donorTestInfo.CollectionSiteVendorId = null;
                donorTestInfo.CollectionSiteLocationId = null;

                if (chkDonorRefuses.Checked)
                {
                    donorTestInfo.IsDonorRefused = true;
                    donorTestInfo.TestStatus = DonorRegistrationStatus.Completed;
                }
                else if (rbTemperatureYes.Checked
                        && rbAdulterationNo.Checked
                        && rbQNSYes.Checked)
                {
                    if ((!chkUrinalysis.Enabled) && chkUrinalysis.Checked)
                    {
                        foreach (DonorTestInfoTestCategories testCategory in donorTestInfo.TestInfoTestCategories)
                        {
                            if (testCategory.TestCategoryId == TestCategories.UA)
                            {
                                testCategory.SpecimenId = txtUASpecimenId.Text.Trim();
                                break;
                            }
                        }
                    }

                    if ((!chkHair.Enabled) && chkHair.Checked)
                    {
                        foreach (DonorTestInfoTestCategories testCategory in donorTestInfo.TestInfoTestCategories)
                        {
                            if (testCategory.TestCategoryId == TestCategories.Hair)
                            {
                                testCategory.SpecimenId = txtHairSpecimenId.Text.Trim();
                                break;
                            }
                        }
                    }

                    donorTestInfo.CollectionSiteUserId = Program.currentUserId;

                    UserBL userBL = new UserBL();
                    User user = userBL.Get(Program.currentUserId);
                    if (user.UserType == UserType.Vendor)
                    {
                        donorTestInfo.CollectionSiteVendorId = user.VendorId;

                        VendorBL vendorBL = new VendorBL();
                        Vendor vendor = vendorBL.Get(Convert.ToInt32(user.VendorId));

                        foreach (VendorAddress address in vendor.Addresses)
                        {
                            if (address.AddressTypeId == AddressTypes.PhysicalAddress1)
                            {
                                donorTestInfo.CollectionSiteLocationId = address.AddressId;
                                break;
                            }
                        }
                    }

                    if (chkInstant.Checked && rbNegative.Checked)
                    {
                        donorTestInfo.TestStatus = DonorRegistrationStatus.Completed;
                    }
                    else
                    {
                        donorTestInfo.TestStatus = DonorRegistrationStatus.Processing;
                    }
                }
                else if (rbTemperatureNo.Checked
                        || rbAdulterationYes.Checked
                        || rbQNSNo.Checked)
                {
                    donorTestInfo.TestStatus = DonorRegistrationStatus.SuspensionQueue;
                }
                else if (chkDNA.Checked == true && chkHair.Checked == false && chkUrinalysis.Checked == false && rbTemperatureYes.Visible == false
                        || rbAdulterationYes.Visible == false
                        || rbQNSYes.Visible == false)
                {
                    donorTestInfo.CollectionSiteUserId = Program.currentUserId;

                    UserBL userBL = new UserBL();
                    User user = userBL.Get(Program.currentUserId);
                    if (user.UserType == UserType.Vendor)
                    {
                        donorTestInfo.CollectionSiteVendorId = user.VendorId;

                        VendorBL vendorBL = new VendorBL();
                        Vendor vendor = vendorBL.Get(Convert.ToInt32(user.VendorId));

                        foreach (VendorAddress address in vendor.Addresses)
                        {
                            if (address.AddressTypeId == AddressTypes.PhysicalAddress1)
                            {
                                donorTestInfo.CollectionSiteLocationId = address.AddressId;
                                break;
                            }
                        }
                    }

                    if (chkInstant.Checked && rbNegative.Checked)
                    {
                        donorTestInfo.TestStatus = DonorRegistrationStatus.Completed;
                    }
                    else
                    {
                        donorTestInfo.TestStatus = DonorRegistrationStatus.Processing;
                    }
                }

                if (chkInstant.Checked)
                {
                    donorTestInfo.IsInstantTest = true;
                }
                else
                {
                    donorTestInfo.IsInstantTest = false;
                }

                //if (rbPositive.Checked)
                //{
                //    donorTestInfo.InstantTestResult = InstantTestResult.Positive;

                //    MessageBox.Show("Take Regular Test.");
                //}
                //else if(rbNegative.Checked)
                //{
                //    donorTestInfo.InstantTestResult = InstantTestResult.Negative;

                //    MessageBox.Show("Test Completed.");
                //}
                if (rbPositive.Checked == false && rbNegative.Checked == false)
                {
                    donorTestInfo.InstantTestResult = InstantTestResult.None;
                }
                if (rbPositive.Checked == true)
                {
                    DialogResult userConfirmation = MessageBox.Show("Result is POSITVE. Do you want to save?", "SurPath Drug Testing", MessageBoxButtons.YesNoCancel);

                    if (userConfirmation == DialogResult.Cancel)
                    {
                        return false;
                    }
                    else if (userConfirmation == DialogResult.No)
                    {
                        return false;
                    }
                    else if (userConfirmation == DialogResult.Yes)
                    {
                        donorTestInfo.InstantTestResult = InstantTestResult.Positive;
                        MessageBox.Show("Take Regular Test.");
                    }
                }
                if (rbNegative.Checked == true)
                {
                    DialogResult userConfirmation = MessageBox.Show("Result is NEGATIVE. Do you want to save?", "SurPath Drug Testing", MessageBoxButtons.YesNoCancel);

                    if (userConfirmation == DialogResult.Cancel)
                    {
                        return false;
                    }
                    else if (userConfirmation == DialogResult.No)
                    {
                        return false;
                    }
                    else if (userConfirmation == DialogResult.Yes)
                    {
                        donorTestInfo.InstantTestResult = InstantTestResult.Negative;

                        //if (rbTemperatureYes.Checked && rbAdulterationNo.Checked && rbQNSYes.Checked)
                        //{
                        donorTestInfo.TestOverallResult = (int)OverAllTestResult.Negative;
                        //}
                        MessageBox.Show("Test Completed.");
                    }
                }

                //if (chkRegularTest.Checked)
                //{
                //    donorTestInfo.IsRegularAfterInstantTest = true;
                //}
                //else
                //{
                //    donorTestInfo.IsRegularAfterInstantTest = false;
                //}

                donorTestInfo.LastModifiedBy = Program.currentUserName;

                donorBL.SaveTestInfoDetails(donorTestInfo);

                lblStatus.Text = donorTestInfo.TestStatus.ToDescriptionString();

                if (!(donorTestInfo.TestStatus == DonorRegistrationStatus.Registered
                        || donorTestInfo.TestStatus == DonorRegistrationStatus.InQueue
                        || donorTestInfo.TestStatus == DonorRegistrationStatus.SuspensionQueue))
                {
                    chkUrinalysis.Enabled = false;
                    chkHair.Enabled = false;
                    chkDNA.Enabled = false;
                    pnlReason.Enabled = false;
                    cmbTestingAuthority.Enabled = false;
                    if (rbFederal.Checked == true)
                    {
                        cmbTestingAuthority.Visible = true;
                        lblTestingAuthority.Visible = true;
                        lblTestingAuthorityMan.Visible = true;
                    }
                    else
                    {
                        cmbTestingAuthority.Visible = false;
                        lblTestingAuthority.Visible = false;
                        lblTestingAuthorityMan.Visible = false;
                    }
                    //cmbTestingAuthority.Visible = false;
                    //lblTestingAuthority.Visible = false;
                    //lblTestingAuthorityMan.Visible = false;
                    txtUASpecimenId.ReadOnly = true;
                    txtHairSpecimenId.ReadOnly = true;
                    pnlCup.Enabled = false;
                    pnlObserved.Enabled = false;
                    pnlFormType.Enabled = false;
                    pnlTemprature.Enabled = false;
                    pnlAdulteration.Enabled = false;
                    pnlQNS.Enabled = false;
                    chkDonorRefuses.Enabled = false;
                    btnTestInfoSave.Enabled = false;
                    chkInstant.Enabled = false;
                    rbPositive.Enabled = false;
                    rbNegative.Enabled = false;
                }
            }

            LoadActivityDetails();
            LoadTestHistoryDetails();
            LoadAccountDetails(currentDonorId, currentTestInfoId);
            return true;
        }

        private bool SaveTestInfoReverseEntryDetails(bool validateFlag, DonorTestInfo donorTestInfo, Donor donor)
        {
            //Test Category & Test Panel
            int htPanelDays = 0;
            donorTestInfo.IsUA = false;
            donorTestInfo.IsHair = false;
            donorTestInfo.IsDNA = false;
            donorTestInfo.IsBC = false;

            bool canUpdateHTPDays = false;
            if (donorTestInfo.PaymentStatus != PaymentStatus.Paid)
            {
                if (chkUrinalysis.Enabled && chkUrinalysis.Checked)
                {
                    donorTestInfo.IsUA = true;
                }

                if (chkHair.Enabled && chkHair.Checked)
                {
                    donorTestInfo.IsHair = true;

                    foreach (DonorTestInfoTestCategories testCategory in donorTestInfo.TestInfoTestCategories)
                    {
                        if (testCategory.TestCategoryId == TestCategories.Hair)
                        {
                            testCategory.HairTestPanelDays = Convert.ToInt32(cmbDays.SelectedIndex) * 90;
                            break;
                        }
                    }
                }

                if (chkDNA.Enabled && chkDNA.Checked)
                {
                    donorTestInfo.IsDNA = true;
                }
                if (chkBC.Enabled && chkBC.Checked)
                {
                    donorTestInfo.IsBC = true;
                }
            }
            Client clients = clientBL.Get(donorTestInfo.ClientId);

            if (donorTestInfo.PaymentStatus == PaymentStatus.Paid || (donorTestInfo.PaymentStatus != PaymentStatus.Paid && clients.CanEditTestCategory == false))
            {
                if ((!chkUrinalysis.Enabled) && chkUrinalysis.Checked)
                {
                    donorTestInfo.IsUA = true;
                }

                if ((!chkHair.Enabled) && chkHair.Checked)
                {
                    donorTestInfo.IsHair = true;

                    foreach (DonorTestInfoTestCategories testCategory in donorTestInfo.TestInfoTestCategories)
                    {
                        if (testCategory.TestCategoryId == TestCategories.Hair)
                        {
                            testCategory.HairTestPanelDays = Convert.ToInt32(cmbDays.SelectedIndex) * 90;
                            break;
                        }
                    }
                }

                if ((!chkDNA.Enabled) && chkDNA.Checked)
                {
                    donorTestInfo.IsDNA = true;
                }
                if ((!chkBC.Enabled) && chkBC.Checked)
                {
                    donorTestInfo.IsBC = true;
                }
            }

            if (!(donorTestInfo.TestStatus == DonorRegistrationStatus.Registered
                    || donorTestInfo.TestStatus == DonorRegistrationStatus.InQueue
                    || donorTestInfo.TestStatus == DonorRegistrationStatus.SuspensionQueue))
            {
                //if (chkUrinalysis.Checked)
                //{
                //    if (txtUASpecimenId.Text.Trim() == string.Empty)
                //    {
                //        tcMain.SelectedTab = tabTestInfo;
                //        Cursor.Current = Cursors.Default;
                //        MessageBox.Show("UA Specimen Id cannot be empty.");
                //        txtUASpecimenId.Focus();
                //        return false;
                //    }
                //    //if (txtUASpecimenId.Text.Trim() != string.Empty)
                //    //{
                //    //    if (!UASpecimenValidation())
                //    //    {
                //    //        MessageBox.Show(" UA Specimen Id already exists.");
                //    //        return false;
                //    //    }
                //    //}

                //    foreach (DonorTestInfoTestCategories testCategory in donorTestInfo.TestInfoTestCategories)
                //    {
                //        if (testCategory.TestCategoryId == TestCategories.UA)
                //        {
                //            testCategory.SpecimenId = txtUASpecimenId.Text.Trim();
                //            break;
                //        }
                //    }
                //}

                //if (chkHair.Checked)
                //{
                //    if (txtHairSpecimenId.Text.Trim() == string.Empty)
                //    {
                //        tcMain.SelectedTab = tabTestInfo;
                //        Cursor.Current = Cursors.Default;
                //        MessageBox.Show("Hair Specimen Id cannot be empty.");
                //        txtUASpecimenId.Focus();
                //        return false;
                //    }
                //    //if (txtHairSpecimenId.Text.Trim() != string.Empty)
                //    //{
                //    //    if (!HairSpecimenValidation())
                //    //    {
                //    //        MessageBox.Show("Hair Specimen Id already exists.");
                //    //        return false;
                //    //    }
                //    //}

                //    foreach (DonorTestInfoTestCategories testCategory in donorTestInfo.TestInfoTestCategories)
                //    {
                //        if (testCategory.TestCategoryId == TestCategories.Hair)
                //        {
                //            testCategory.SpecimenId = txtHairSpecimenId.Text.Trim();
                //            break;
                //        }
                //    }
                //}

                //donorTestInfo.LastModifiedBy = Program.currentUserName;

                //donorBL.UpdateTestInfoSpecimenIDs(donorTestInfo);
                //}
                //else
                if (!validateFlag)
                {
                    if (donorTestInfo.PaymentStatus != PaymentStatus.Paid)
                    {
                        if (chkHair.Enabled && chkHair.Checked)
                        {
                            donorTestInfo.IsHair = true;

                            foreach (DonorTestInfoTestCategories testCategory in donorTestInfo.TestInfoTestCategories)
                            {
                                if (testCategory.TestCategoryId == TestCategories.Hair)
                                {
                                    testCategory.HairTestPanelDays = Convert.ToInt32(cmbDays.SelectedIndex) * 90;
                                    if (testCategory.HairTestPanelDays == 0)
                                    {
                                        MessageBox.Show("# of Days must be selected.");
                                        return false;
                                    }
                                    htPanelDays = Convert.ToInt32(testCategory.HairTestPanelDays);
                                    canUpdateHTPDays = true;
                                    break;
                                }
                            }
                        }
                    }
                    clients = clientBL.Get(donorTestInfo.ClientId);
                    if (donorTestInfo.PaymentStatus != PaymentStatus.Paid && clients.CanEditTestCategory == false)
                    {
                        if ((!chkHair.Enabled) && chkHair.Checked)
                        {
                            donorTestInfo.IsHair = true;

                            foreach (DonorTestInfoTestCategories testCategory in donorTestInfo.TestInfoTestCategories)
                            {
                                if (testCategory.TestCategoryId == TestCategories.Hair)
                                {
                                    testCategory.HairTestPanelDays = Convert.ToInt32(cmbDays.SelectedIndex) * 90;
                                    if (testCategory.HairTestPanelDays == 0)
                                    {
                                        MessageBox.Show("# of Days must be selected.");
                                        return false;
                                    }
                                    htPanelDays = Convert.ToInt32(testCategory.HairTestPanelDays);
                                    canUpdateHTPDays = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (donorTestInfo.PaymentStatus != PaymentStatus.Paid)
                    {
                        clientBL = new ClientBL();
                        //  ClientDepartment clientDepartment = clientBL.GetClientDepartment(Convert.ToInt32(donor.DonorInitialDepartmentId));

                        Client client = clientBL.Get(donorTestInfo.ClientId);
                        if (client.CanEditTestCategory == true)
                        {
                            donorBL.SaveTestInfoDetailsBeforPayment(donorTestInfo);
                            if (this.currentTestInfoId > 0)
                            {
                                donorTestInfo = donorBL.GetDonorTestInfo(this.currentTestInfoId);
                                if (donorTestInfo != null)
                                {
                                    testflag = true;
                                    LoadPaymentDetails(donorTestInfo);
                                    LoadTestInfoDetails(donorTestInfo);
                                }
                            }
                            MessageBox.Show("Test panel details updated. Note : Specimen collection process will not be initiated until the donor completes payment process.");
                            //return false;
                        }
                        else
                        {
                            //  MessageBox.Show("Specimen collection process will not be initiated until the donor completes payment process.");
                            //return false;
                        }
                        if (canUpdateHTPDays == true)
                        {
                            if (htPanelDays > 0 && donorTestInfo.PaymentStatus != PaymentStatus.Paid && donorTestInfo.PaymentTypeId == ClientPaymentTypes.DonorPays)
                            {
                                try
                                {
                                    donorBL.UpdateHairTestPanelDays(donorTestInfo);
                                    LoadPaymentDetails(donorTestInfo);
                                    LoadActivityDetails();
                                    if (client.CanEditTestCategory != true)
                                    {
                                        MessageBox.Show("Test panel details updated. Note : Specimen collection process will not be initiated until the donor completes payment process.");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Unable to update the hair test panel days.");
                                    //return false;
                                }
                            }
                        }
                        return false;
                    }
                    else
                    {
                        if (!ValidateReverseEntryTestInfoDetails())
                        {
                            return false;
                        }
                    }
                    //}

                    string refuessReason = string.Empty;

                    if (chkDonorRefuses.Checked)
                    {
                        if (MessageBox.Show("Are you sure? The donor refuessed to take the test?", "SurPath Drug Testing", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                        {
                            return false;
                        }
                        else
                        {
                            //refuessReason =
                        }
                    }

                    //Reason for test
                    donorTestInfo.OtherReason = null;
                    if (rbPreEmployment.Checked)
                    {
                        donorTestInfo.ReasonForTestId = TestInfoReasonForTest.PreEmployment;
                    }
                    else if (rbRandom.Checked)
                    {
                        donorTestInfo.ReasonForTestId = TestInfoReasonForTest.Random;
                    }
                    else if (rbReasonable.Checked)
                    {
                        donorTestInfo.ReasonForTestId = TestInfoReasonForTest.ReasonableSuspicionCause;
                    }
                    else if (rbPostAccident.Checked)
                    {
                        donorTestInfo.ReasonForTestId = TestInfoReasonForTest.PostAccident;
                    }
                    else if (rbReturntoDuty.Checked)
                    {
                        donorTestInfo.ReasonForTestId = TestInfoReasonForTest.ReturnToDuty;
                    }
                    else if (rbFollowUp.Checked)
                    {
                        donorTestInfo.ReasonForTestId = TestInfoReasonForTest.FollowUp;
                    }
                    else if (rbOther.Checked)
                    {
                        donorTestInfo.ReasonForTestId = TestInfoReasonForTest.Other;
                        donorTestInfo.OtherReason = txtReason.Text.Trim();
                    }
                    else
                    {
                        donorTestInfo.ReasonForTestId = TestInfoReasonForTest.None;
                    }

                    //Specimen Collection Cup
                    if (rbUrineSingle.Checked)
                    {
                        donorTestInfo.SpecimenCollectionCupId = SpecimenCollectionCupType.UrineSingle;
                    }
                    else if (rbUrineSplit.Checked)
                    {
                        donorTestInfo.SpecimenCollectionCupId = SpecimenCollectionCupType.UrineSplit;
                    }
                    else if (rbSaliva.Checked)
                    {
                        donorTestInfo.SpecimenCollectionCupId = SpecimenCollectionCupType.Saliva;
                    }
                    else if (rbBlood.Checked)
                    {
                        donorTestInfo.SpecimenCollectionCupId = SpecimenCollectionCupType.Blood;
                    }
                    else
                    {
                        donorTestInfo.SpecimenCollectionCupId = SpecimenCollectionCupType.None;
                    }

                    //Observed
                    if (rbObservedYes.Checked)
                    {
                        donorTestInfo.IsObserved = YesNo.Yes;
                    }
                    else if (rbObservedNo.Checked)
                    {
                        donorTestInfo.IsObserved = YesNo.No;
                    }
                    else
                    {
                        donorTestInfo.IsObserved = YesNo.None;
                    }

                    //Form Type
                    donorTestInfo.TestingAuthorityId = null;
                    if (rbFederal.Checked)
                    {
                        donorTestInfo.FormTypeId = SpecimenFormType.Federal;
                        donorTestInfo.TestingAuthorityId = Convert.ToInt32(cmbTestingAuthority.SelectedValue);
                        cmbTestingAuthority.Visible = true;
                    }
                    else if (rbNonFederal.Checked)
                    {
                        donorTestInfo.FormTypeId = SpecimenFormType.NonFederal;
                    }
                    else
                    {
                        donorTestInfo.FormTypeId = SpecimenFormType.None;
                    }

                    //Temperature
                    if (rbTemperatureYes.Checked)
                    {
                        donorTestInfo.IsTemperatureInRange = YesNo.Yes;
                    }
                    else if (rbTemperatureNo.Checked)
                    {
                        donorTestInfo.IsTemperatureInRange = YesNo.No;
                        donorTestInfo.TemperatureOfSpecimen = Convert.ToDouble(txtTemperature.Text.Trim());
                    }
                    else
                    {
                        donorTestInfo.IsTemperatureInRange = YesNo.None;
                        donorTestInfo.TemperatureOfSpecimen = null;
                    }

                    //Adulteration
                    if (rbAdulterationYes.Checked)
                    {
                        donorTestInfo.IsAdulterationSign = YesNo.Yes;
                    }
                    else if (rbAdulterationNo.Checked)
                    {
                        donorTestInfo.IsAdulterationSign = YesNo.No;
                    }
                    else
                    {
                        donorTestInfo.IsAdulterationSign = YesNo.None;
                    }

                    //Sufficient Quantity
                    if (rbQNSYes.Checked)
                    {
                        donorTestInfo.IsQuantitySufficient = YesNo.Yes;
                    }
                    else if (rbQNSNo.Checked)
                    {
                        donorTestInfo.IsQuantitySufficient = YesNo.No;
                    }
                    else
                    {
                        donorTestInfo.IsQuantitySufficient = YesNo.None;
                    }

                    //Test Info Status and final call
                    donorTestInfo.IsDonorRefused = false;
                    donorTestInfo.CollectionSiteUserId = null;
                    donorTestInfo.CollectionSiteVendorId = null;
                    donorTestInfo.CollectionSiteLocationId = null;

                    if (chkDonorRefuses.Checked)
                    {
                        donorTestInfo.IsDonorRefused = true;
                        donorTestInfo.TestStatus = DonorRegistrationStatus.Completed;
                    }
                    else if (rbTemperatureYes.Checked
                            && rbAdulterationNo.Checked
                            && rbQNSYes.Checked)
                    {
                        if ((!chkUrinalysis.Enabled) && chkUrinalysis.Checked)
                        {
                            foreach (DonorTestInfoTestCategories testCategory in donorTestInfo.TestInfoTestCategories)
                            {
                                if (testCategory.TestCategoryId == TestCategories.UA)
                                {
                                    testCategory.SpecimenId = txtUASpecimenId.Text.Trim();
                                    break;
                                }
                            }
                        }

                        if ((!chkHair.Enabled) && chkHair.Checked)
                        {
                            foreach (DonorTestInfoTestCategories testCategory in donorTestInfo.TestInfoTestCategories)
                            {
                                if (testCategory.TestCategoryId == TestCategories.Hair)
                                {
                                    testCategory.SpecimenId = txtHairSpecimenId.Text.Trim();
                                    break;
                                }
                            }
                        }

                        donorTestInfo.CollectionSiteUserId = Program.currentUserId;

                        UserBL userBL = new UserBL();
                        User user = userBL.Get(Program.currentUserId);
                        if (user.UserType == UserType.Vendor)
                        {
                            donorTestInfo.CollectionSiteVendorId = user.VendorId;

                            VendorBL vendorBL = new VendorBL();
                            Vendor vendor = vendorBL.Get(Convert.ToInt32(user.VendorId));

                            foreach (VendorAddress address in vendor.Addresses)
                            {
                                if (address.AddressTypeId == AddressTypes.PhysicalAddress1)
                                {
                                    donorTestInfo.CollectionSiteLocationId = address.AddressId;
                                    break;
                                }
                            }
                        }

                        //if (chkInstant.Checked && rbNegative.Checked)
                        //{
                        //    donorTestInfo.TestStatus = DonorRegistrationStatus.Completed;
                        //}
                        //else if (chkInstant.Checked && rbPositive.Checked)
                        //{
                        //    donorTestInfo.TestStatus = DonorRegistrationStatus.Completed;
                        //}
                        //else
                        //{
                        //     donorTestInfo.TestStatus = DonorRegistrationStatus.Processing;
                        //}
                    }
                    //else if (rbTemperatureNo.Checked
                    //        || rbAdulterationYes.Checked
                    //        || rbQNSNo.Checked)
                    //{
                    //    donorTestInfo.TestStatus = DonorRegistrationStatus.SuspensionQueue;
                    //}

                    //if (chkInstant.Checked)
                    //{
                    //    donorTestInfo.IsInstantTest = true;
                    //}
                    //else
                    //{
                    //    donorTestInfo.IsInstantTest = false;
                    //}

                    donorTestInfo.LastModifiedBy = Program.currentUserName;

                    donorTestInfo.CollectionSiteVendorId = Convert.ToInt32(cmbLocationName.SelectedValue);
                    donorTestInfo.CollectionSiteUserId = Convert.ToInt32(cmbCollectionName.SelectedValue);
                    // DateTime screenTime = Convert.ToDateTime(txtScreeningDate.Text + txtScreeningTime.Text);
                    // donorTestInfo.ScreeningTime = screenTime;
                    donorTestInfo.IsReverseEntry = false;

                    DialogResult userReverseConfirmation = MessageBox.Show("You cannot revert the change in test info details?", "SurPath Drug Testing", MessageBoxButtons.YesNoCancel);

                    lblStatus.Text = donorTestInfo.TestStatus.ToDescriptionString();

                    if (userReverseConfirmation == DialogResult.Cancel)
                    {
                        return false;
                    }
                    else if (userReverseConfirmation == DialogResult.No)
                    {
                        return false;
                    }
                    else if (userReverseConfirmation == DialogResult.Yes)
                    {
                        donorBL.SaveReverseEntryTestInfoDetails(donorTestInfo, donor);

                        if (!(donorTestInfo.TestStatus == DonorRegistrationStatus.Registered
                                || donorTestInfo.TestStatus == DonorRegistrationStatus.InQueue
                                || donorTestInfo.TestStatus == DonorRegistrationStatus.SuspensionQueue))
                        {
                            chkUrinalysis.Enabled = false;
                            chkHair.Enabled = false;
                            chkDNA.Enabled = false;
                            pnlReason.Enabled = false;
                            cmbTestingAuthority.Enabled = false;
                            if (rbFederal.Checked == true)
                            {
                                cmbTestingAuthority.Visible = true;
                                lblTestingAuthority.Visible = true;
                                lblTestingAuthorityMan.Visible = true;
                            }
                            else
                            {
                                cmbTestingAuthority.Visible = false;
                                lblTestingAuthority.Visible = false;
                                lblTestingAuthorityMan.Visible = false;
                            }
                            //cmbTestingAuthority.Visible = false;
                            //lblTestingAuthority.Visible = false;
                            //lblTestingAuthorityMan.Visible = false;
                            txtUASpecimenId.ReadOnly = true;
                            txtHairSpecimenId.ReadOnly = true;
                            pnlCup.Enabled = false;
                            pnlObserved.Enabled = false;
                            pnlFormType.Enabled = false;
                            pnlTemprature.Enabled = false;
                            pnlAdulteration.Enabled = false;
                            pnlQNS.Enabled = false;
                            chkDonorRefuses.Enabled = false;
                            btnTestInfoSave.Enabled = false;
                            chkInstant.Enabled = false;
                            rbPositive.Enabled = false;
                            rbNegative.Enabled = false;
                        }
                    }
                }
            }
            LoadActivityDetails();
            LoadTestHistoryDetails();
            LoadAccountDetails(currentDonorId, currentTestInfoId);
            return true;
        }

        private bool HairSpecimenValidation()
        {
            DonorTestInfo donorTestInfo = donorBL.GetDonorTestInfo(this.currentTestInfoId);

            DataTable dtspecimen = donorBL.GetSpecimenId(txtHairSpecimenId.Text.Trim());
            if (dtspecimen.Rows.Count > 1)
            {
                return false;
            }
            else if (dtspecimen.Rows.Count == 1)
            {
                if ((int)dtspecimen.Rows[0]["DonorTestInfoId"] != donorTestInfo.DonorTestInfoId)
                {
                    return false;
                }
            }
            return true;
        }

        private bool UASpecimenValidation()
        {
            DonorTestInfo donorTestInfo = donorBL.GetDonorTestInfo(this.currentTestInfoId);

            DataTable dtspecimen = donorBL.GetSpecimenId(txtUASpecimenId.Text.Trim());
            if (dtspecimen.Rows.Count > 1)
            {
                return false;
            }
            else if (dtspecimen.Rows.Count == 1)
            {
                if ((int)dtspecimen.Rows[0]["DonorTestInfoId"] != donorTestInfo.DonorTestInfoId)
                {
                    return false;
                }
            }
            return true;
        }

        private bool SaveLegalDetails(bool validateFlag, DonorTestInfo donorTestInfo, string SpecialNotes)
        {
            if (!validateFlag)
            {
                if (!ValidateLegalDetails())
                {
                    return false;
                }
            }

            //AttorneyInfo
            if (cmbAttorneyName1.SelectedIndex != 0)
            {
                donorTestInfo.AttorneyId1 = (int)cmbAttorneyName1.SelectedValue;
            }
            else
            {
                donorTestInfo.AttorneyId1 = null;
            }

            if (cmbAttorneyName2.SelectedIndex != 0)
            {
                donorTestInfo.AttorneyId2 = (int)cmbAttorneyName2.SelectedValue;
            }
            else
            {
                donorTestInfo.AttorneyId2 = null;
            }

            if (cmbAttorneyName3.SelectedIndex != 0)
            {
                donorTestInfo.AttorneyId3 = (int)cmbAttorneyName3.SelectedValue;
            }
            else
            {
                donorTestInfo.AttorneyId3 = null;
            }

            //ThirdPartyInformation

            if (cmbThirdPartyInfo1Name.SelectedIndex != 0)
            {
                donorTestInfo.ThirdPartyInfoId1 = (int)cmbThirdPartyInfo1Name.SelectedValue;
            }
            else
            {
                donorTestInfo.ThirdPartyInfoId1 = null;
            }

            if (cmbThirdPartyInfo2Name.SelectedIndex != 0)
            {
                donorTestInfo.ThirdPartyInfoId2 = (int)cmbThirdPartyInfo2Name.SelectedValue;
            }
            else
            {
                donorTestInfo.ThirdPartyInfoId2 = null;
            }

            //Program details
            if (rbOneTimeTesting.Checked)
            {
                donorTestInfo.ProgramTypeId = ProgramType.OneTimeTesting;
            }
            else if (rbRandomTesting.Checked)
            {
                donorTestInfo.ProgramTypeId = ProgramType.Random;
            }
            else
            {
                donorTestInfo.ProgramTypeId = ProgramType.None;
            }

            if (chkSurScanDates.Checked)
            {
                donorTestInfo.IsSurscanDeterminesDates = true;
            }
            else
            {
                donorTestInfo.IsSurscanDeterminesDates = false;
            }

            if (chkThirdPartyDates.Checked)
            {
                donorTestInfo.IsTpDeterminesDates = true;
            }
            else
            {
                donorTestInfo.IsTpDeterminesDates = false;
            }

            if (cmbLegalStartMonth.SelectedIndex != 0 && cmbLegalStartDate.SelectedIndex != 0 && cmbLegalStartYear.SelectedIndex != 0)
            {
                try
                {
                    string inStartActiveDate = cmbLegalStartYear.Text + '-' + cmbLegalStartMonth.Text + '-' + cmbLegalStartDate.Text;
                    donorTestInfo.ProgramStartDate = Convert.ToDateTime(inStartActiveDate);
                }
                catch (Exception ex)
                {
                    tcMain.SelectedTab = tabLegalInfo;
                    MessageBox.Show("Invalid format of date.");
                    cmbLegalStartMonth.Focus();
                    // txtStartDate.Focus();
                }
            }

            if (cmbLegalEndMonth.SelectedIndex != 0 && cmbLegalEndDate.SelectedIndex != 0 && cmbLegalEndYear.SelectedIndex != 0)
            {
                try
                {
                    string inEndActiveDate = cmbLegalEndYear.Text + '-' + cmbLegalEndMonth.Text + '-' + cmbLegalEndDate.Text;
                    donorTestInfo.ProgramEndDate = Convert.ToDateTime(inEndActiveDate);
                }
                catch (Exception ex)
                {
                    tcMain.SelectedTab = tabLegalInfo;
                    MessageBox.Show("Invalid format of date.");
                    cmbLegalEndMonth.Focus();
                    // txtEndDate.Focus();
                }
            }

            //if (txtStartDate.Text.Trim().Replace("/", "").Replace(" ", "") != string.Empty)
            //{
            //    try
            //    {
            //        donorTestInfo.ProgramStartDate = Convert.ToDateTime(txtStartDate.Text);
            //    }
            //    catch (Exception ex)
            //    {
            //        tcMain.SelectedTab = tabLegalInfo;
            //        MessageBox.Show("Invalid format of date.");
            //        txtStartDate.Focus();
            //    }
            //}

            //if (txtEndDate.Text.Trim().Replace("/", "").Replace(" ", "") != string.Empty)
            //{
            //    try
            //    {
            //        donorTestInfo.ProgramEndDate = Convert.ToDateTime(txtEndDate.Text);
            //    }
            //    catch (Exception ex)
            //    {
            //        tcMain.SelectedTab = tabLegalInfo;
            //        MessageBox.Show("Invalid format of date.");
            //        txtEndDate.Focus();
            //    }
            //}

            //CourtDetails
            if (txtCaseNumber.Text != string.Empty)
            {
                donorTestInfo.CaseNumber = txtCaseNumber.Text;
            }
            else
            {
                donorTestInfo.CaseNumber = string.Empty;
            }

            if (cmbCourtName.SelectedIndex != 0)
            {
                donorTestInfo.CourtId = (int)cmbCourtName.SelectedValue;
            }

            //JudgeDetails
            if (cmbJudgeName.SelectedIndex != 0)
            {
                donorTestInfo.JudgeId = (int)cmbJudgeName.SelectedValue;
            }

            //if (txtLegalInfoNotes.Text != string.Empty)
            //{
            //    // string SpecialNotes1 = donorTestInfo.SpecialNotes;
            //    if (SpecialNotes.Trim() != donorTestInfo.SpecialNotes.Trim())
            //    {
            //        donorTestInfo.SpecialNotes = txtLegalInfoNotes.Text;
            //        isValidString = true;
            //    }

            //    // donorTestInfo.SpecialNotes = txtLegalInfoNotes.Text;
            //}
            //else
            //{
            //    donorTestInfo.SpecialNotes = string.Empty;
            //}

            if (txtLegalInfoNotes.Text != string.Empty)
            {
                // string SpecialNotes1 = donorTestInfo.SpecialNotes;
                if (donorTestInfo.SpecialNotes != null)
                {
                    if (SpecialNotes.Trim() != donorTestInfo.SpecialNotes.Trim())
                    {
                        donorTestInfo.SpecialNotes = txtLegalInfoNotes.Text;
                        isValidString = true;
                    }
                }
                else
                {
                    donorTestInfo.SpecialNotes = txtLegalInfoNotes.Text;
                }

                // donorTestInfo.SpecialNotes = txtLegalInfoNotes.Text;
            }
            else
            {
                donorTestInfo.SpecialNotes = string.Empty;
            }

            donorTestInfo.LastModifiedBy = Program.currentUserName;

            UserBL userBL = new UserBL();
            User user = userBL.Get(Program.currentUserId);

            donorBL.SaveLegalInfoDetails(donorTestInfo, isValidString);
            isValidString = false;
            // btnLegalInfoSave.Enabled = false;
            LoadActivityDetails();
            return true;
        }

        #endregion UI to DB

        private void btnUnmaskSSN_MouseDown(object sender, MouseEventArgs e)
        {
            //btnUnmaskSSN.Image = global::SurPath.Properties.Resources.unmask_SSN_2;
            //if (txtSSN.Text != string.Empty)
            //{
            //   // btnUnmaskSSN.BackColor = Color.Silver;
            //    txtSSN.Text = txtSSN.Tag.ToString();
            //}
        }

        #endregion Private Methods

        #region Result Tab Methods

        private void LoadCRLReport()
        {
            bool defaultPositiveFlag = false;
            ReportType reportType = ReportType.LabReport;
            string specimenId = UASpecimenId;
            ReportInfo reportDetails = new ReportInfo();
            reportDetails = null;
            List<OBR_Info> obrList = null;

            RTFBuilderbase crlReport = null;

            //rtbFileContent.Text = string.Empty;
            //rtbCRLReport.Text = string.Empty;

            HL7ParserDao hl7ParserDao = new HL7ParserDao();
            hl7ParserDao.GetReportLAB(reportType, specimenId, reportDetails, ref obrList, ref crlReport);
            reportDetails = hl7ParserDao.GetReportDetails(reportType, specimenId, reportDetails);
            if (reportDetails != null)
            {
                txtLbReportedDate.Text = reportDetails.CreatedOn.ToString("MM/dd/yyyy");
                // txtLabOverallResults.Text = reportDetails.ReportStatus.ToDescriptionString();
            }

            dgvLabResult1.AutoGenerateColumns = false;
            dgvLabResult2.AutoGenerateColumns = false;
            dgvLabResult3.AutoGenerateColumns = false;
            //  dataGridView4.AutoGenerateColumns = false;

            dgvLabResult1.DataSource = null;
            dgvLabResult2.DataSource = null;
            dgvLabResult3.DataSource = null;
            //   dataGridView4.DataSource = null;

            dgvLabResult1.Columns[0].HeaderText = "Drug Name";
            dgvLabResult2.Columns[0].HeaderText = "Drug Name";
            dgvLabResult3.Columns[0].HeaderText = "Drug Name";
            // dataGridView4.Columns[0].HeaderText = "Test Panel";

            if (obrList != null)
            {
                if (obrList.Count <= 4)
                {
                    int i = 1;
                    foreach (OBR_Info obr in obrList)
                    {
                        txtReceivedDate.Text = obr.SpecimenReceivedDate.ToString();

                        if (i == 1)
                        {
                            dgvLabResult1.DataSource = obr.observatinos;
                            dgvLabResult1.Columns[0].HeaderText = obr.SectionHeader;
                        }
                        else if (i == 2)
                        {
                            dgvLabResult2.DataSource = obr.observatinos;
                            dgvLabResult2.Columns[0].HeaderText = obr.SectionHeader;
                        }
                        else if (i == 3)
                        {
                            dgvLabResult3.DataSource = obr.observatinos;
                            dgvLabResult3.Columns[0].HeaderText = obr.SectionHeader;
                        }

                        if (obr.SectionHeader.Contains("CONFIRMATION"))
                        {
                            foreach (OBX_Info obr1 in obr.observatinos)
                            {
                                if (obr1.Status.Contains("POSITIVE"))
                                {
                                    txtLabOverallResults.Text = "POSITIVE";
                                    defaultPositiveFlag = true;
                                }
                                else
                                {
                                    txtLabOverallResults.Text = "NEGATIVE";
                                }
                            }
                        }

                        i++;
                    }

                    if (obrList.Count == 2)
                    {
                        dgvLabResult3.Visible = false;
                        dgvLabResult2.Size = new System.Drawing.Size(379, 238);
                    }
                }

                //if (defaultPositiveFlag == true)
                //{
                //    txtLabOverallResults.Text = "POSITIVE";
                //}
                //else
                //{
                //    txtLabOverallResults.Text = "NEGATIVE";
                //}
            }
        }

        private void LoadQuestReport()
        {
            bool defaultPositiveFlag = false;
            ReportType reportType = ReportType.QuestLabReport;
            string specimenId = UASpecimenId;
            ReportInfo reportDetails = new ReportInfo();
            reportDetails = null;
            List<OBR_Info> obrList = null;

            RTFBuilderbase crlReport = null;

            //rtbFileContent.Text = string.Empty;
            //rtbCRLReport.Text = string.Empty;

            HL7ParserDao hl7ParserDao = new HL7ParserDao();
            hl7ParserDao.GetReportQuest(reportType, specimenId, reportDetails, ref obrList, ref crlReport);
            reportDetails = hl7ParserDao.GetReportDetails(reportType, specimenId, reportDetails);
            if (reportDetails != null)
            {
                txtLbReportedDate.Text = reportDetails.CreatedOn.ToString("MM/dd/yyyy");
                // txtLabOverallResults.Text = reportDetails.ReportStatus.ToDescriptionString();
            }

            dgvLabResult1.AutoGenerateColumns = false;
            dgvLabResult2.AutoGenerateColumns = false;
            dgvLabResult3.AutoGenerateColumns = false;
            //  dataGridView4.AutoGenerateColumns = false;

            dgvLabResult1.DataSource = null;
            dgvLabResult2.DataSource = null;
            dgvLabResult3.DataSource = null;
            //   dataGridView4.DataSource = null;

            dgvLabResult1.Columns[0].HeaderText = "Drug Name";
            dgvLabResult2.Columns[0].HeaderText = "Drug Name";
            dgvLabResult3.Columns[0].HeaderText = "Drug Name";
            // dataGridView4.Columns[0].HeaderText = "Test Panel";

            if (obrList != null)
            {
                if (obrList.Count <= 4)
                {
                    int i = 1;
                    foreach (OBR_Info obr in obrList)
                    {
                        txtReceivedDate.Text = obr.SpecimenReceivedDate.ToString();

                        if (i == 1)
                        {
                            dgvLabResult1.DataSource = obr.observatinos;
                            dgvLabResult1.Columns[0].HeaderText = obr.SectionHeader;
                        }
                        else if (i == 2)
                        {
                            dgvLabResult2.DataSource = obr.observatinos;
                            dgvLabResult2.Columns[0].HeaderText = obr.SectionHeader;
                        }
                        else if (i == 3)
                        {
                            dgvLabResult3.DataSource = obr.observatinos;
                            dgvLabResult3.Columns[0].HeaderText = obr.SectionHeader;
                        }

                        if (obr.SectionHeader.Contains("CONFIRMATION") || reportType == ReportType.QuestLabReport)
                        {
                            foreach (OBX_Info obr1 in obr.observatinos)
                            {
                                if (obr1.Status.Contains("POSITIVE"))
                                {
                                    defaultPositiveFlag = true;
                                }
                            }
                        }

                        i++;
                    }

                    if (obrList.Count == 2)
                    {
                        dgvLabResult3.Visible = false;
                        dgvLabResult2.Size = new System.Drawing.Size(379, 238);
                    }
                }

                if (defaultPositiveFlag == true)
                {
                    txtLabOverallResults.Text = "POSITIVE";
                }
                else
                {
                    txtLabOverallResults.Text = "NEGATIVE";
                }
            }
        }

        private void LoadMROReport()
        {
            bool defaultPositiveFlag = false;

            ReportType reportType = ReportType.MROReport;
            string specimenId = UASpecimenId;
            ReportInfo reportDetails = null;
            ReportInfo reports = null;
            List<OBR_Info> obrList = null;
            DonorBL donorBL = new DonorBL();

            RTFBuilderbase crlReport = null;

            HL7ParserDao hl7ParserDao = new HL7ParserDao();
            hl7ParserDao.GetReportMRO(reportType, specimenId, reportDetails, ref obrList, ref crlReport);
            reportDetails = hl7ParserDao.GetReportDetails(reportType, specimenId, reportDetails);

            reports = donorBL.GetMROReport(this.currentTestInfoId, reportType);
            if (reports != null)
            {
                documentReportId = reports.FinalReportId;

                if (documentReportId != 0)
                {
                    btnMRO.Visible = true;
                }
                else
                {
                    btnMRO.Visible = false;
                }
            }

            if (reportDetails != null)
            {
                txtMROReportedDate.Text = reportDetails.CreatedOn.ToString("MM/dd/yyyy");
            }

            if (crlReport != null)
            {
                //  rtbCRLReport.Rtf = crlReport.ToString();
            }

            dgvMROResult1.AutoGenerateColumns = false;
            dgvMROResult2.AutoGenerateColumns = false;
            dgvMROResult3.AutoGenerateColumns = false;
            //  dataGridView4.AutoGenerateColumns = false;

            dgvMROResult1.DataSource = null;
            dgvMROResult2.DataSource = null;
            dgvMROResult3.DataSource = null;
            //   dataGridView4.DataSource = null;

            dgvMROResult1.Columns[0].HeaderText = "Drug Name";
            dgvMROResult2.Columns[0].HeaderText = "Drug Name";
            dgvMROResult3.Columns[0].HeaderText = "Drug Name";
            // dataGridView4.Columns[0].HeaderText = "Test Panel";

            if (obrList != null)
            {
                if (obrList.Count <= 4)
                {
                    int i = 1;
                    foreach (OBR_Info obr in obrList)
                    {
                        txtReceivedDate.Text = obr.SpecimenReceivedDate.ToString();

                        if (i == 1)
                        {
                            dgvMROResult1.DataSource = obr.observatinos;
                            // dgvMROResult1.Columns[0].HeaderText = obr.SectionHeader;
                        }
                        else if (i == 2)
                        {
                            dgvMROResult2.DataSource = obr.observatinos;
                            //  dgvMROResult2.Columns[0].HeaderText = obr.SectionHeader;
                        }
                        else if (i == 3)
                        {
                            dgvMROResult3.DataSource = obr.observatinos;
                            // dgvMROResult3.Columns[0].HeaderText = obr.SectionHeader;
                        }

                        foreach (OBX_Info obr1 in obr.observatinos)
                        {
                            if (obr1.Status.Contains("POSITIVE"))
                            {
                                defaultPositiveFlag = true;
                            }
                        }

                        i++;
                    }

                    if (obrList.Count == 2)
                    {
                        dgvMROResult3.Visible = false;
                        dgvMROResult2.Size = new System.Drawing.Size(379, 238);
                    }
                    else if (obrList.Count == 1)
                    {
                        dgvMROResult3.Visible = false;
                        dgvMROResult2.Visible = false;
                        dgvMROResult1.Size = new System.Drawing.Size(379, 319);
                    }
                }
                if (defaultPositiveFlag == true)
                {
                    txtMROOverallResults.Text = "POSITIVE";
                }
                if (defaultPositiveFlag == false)
                {
                    txtMROOverallResults.Text = "NEGATIVE";
                }
            }
        }

        //private void LoadMROReport()
        //{
        //    bool defaultPositiveFlag = false;

        //    ReportType reportType = ReportType.MROReport;
        //    string specimenId = UASpecimenId;
        //    ReportInfo reportDetails = null;
        //    List<OBR_Info> obrList = null;

        //    RTFBuilderbase crlReport = null;

        //    //rtbFileContent.Text = string.Empty;
        //    //rtbCRLReport.Text = string.Empty;

        //    HL7ParserDao hl7ParserDao = new HL7ParserDao();
        //    hl7ParserDao.GetReport(reportType, specimenId, reportDetails, ref obrList, ref crlReport);
        //    reportDetails = hl7ParserDao.GetReportDetails(reportType, specimenId, reportDetails);
        //    if (reportDetails != null)
        //    {
        //        txtMROReportedDate.Text = reportDetails.CreatedOn.ToString("MM/dd/yyyy");
        //    }

        //    if (crlReport != null)
        //    {
        //        //  rtbCRLReport.Rtf = crlReport.ToString();
        //    }

        //    dgvMROResult1.AutoGenerateColumns = false;
        //    dgvMROResult2.AutoGenerateColumns = false;
        //    dgvMROResult3.AutoGenerateColumns = false;
        //    //  dataGridView4.AutoGenerateColumns = false;

        //    dgvMROResult1.DataSource = null;
        //    dgvMROResult2.DataSource = null;
        //    dgvMROResult3.DataSource = null;
        //    //   dataGridView4.DataSource = null;

        //    dgvMROResult1.Columns[0].HeaderText = "Test Panel";
        //    dgvMROResult2.Columns[0].HeaderText = "Test Panel";
        //    dgvMROResult3.Columns[0].HeaderText = "Test Panel";
        //    // dataGridView4.Columns[0].HeaderText = "Test Panel";

        //    if (obrList != null)
        //    {
        //        if (obrList.Count <= 4)
        //        {
        //            int i = 1;
        //            foreach (OBR_Info obr in obrList)
        //            {
        //                //if (!obr.SectionHeader.Contains("CONFIRMATION"))
        //                //{
        //                //    if (defaultPositiveFlag == false)
        //                //    {
        //                //        txtMROOverallResults.Text = "NEGATIVE";
        //                //    }
        //                //}
        //                if (obr.SectionHeader.Contains("CONFIRMATION"))
        //                {
        //                    if (obrList.Count == 4)
        //                    {
        //                        //dataGridView4.DataSource = obr.observatinos;
        //                        //dataGridView4.Columns[0].HeaderText = obr.SectionHeader;
        //                    }
        //                    else if (obrList.Count == 3)
        //                    {
        //                        dgvMROResult3.DataSource = obr.observatinos;
        //                        dgvMROResult3.Columns[0].HeaderText = obr.SectionHeader;
        //                        foreach (OBX_Info obr1 in obr.observatinos)
        //                        {
        //                            if (obr1.Status.Contains("POSITIVE"))
        //                            {
        //                                defaultPositiveFlag = true;
        //                            }
        //                        }

        //                    }

        //                    else if (obrList.Count == 2)
        //                    {
        //                        dgvMROResult2.DataSource = obr.observatinos;
        //                        dgvMROResult2.Columns[0].HeaderText = obr.SectionHeader;
        //                        dgvMROResult3.Visible = false;
        //                        dgvMROResult2.Size = new System.Drawing.Size(379, 238);
        //                        foreach (OBX_Info obr1 in obr.observatinos)
        //                        {
        //                            if (obr1.Status.Contains("POSITIVE"))
        //                            {
        //                                defaultPositiveFlag = true;
        //                            }
        //                        }
        //                        //if (defaultPositiveFlag == true)
        //                        //{
        //                        //    txtMROOverallResults.Text = "POSITIVE";
        //                        //}
        //                        //else
        //                        //{
        //                        //    txtMROOverallResults.Text = "NEGATIVE";
        //                        //}
        //                    }
        //                    //if (defaultPositiveFlag == true)
        //                    //{
        //                    //    txtMROOverallResults.Text = "POSITIVE";
        //                    //}
        //                    //else
        //                    //{
        //                    //    txtMROOverallResults.Text = "NEGATIVE";
        //                    //}
        //                }
        //                else if (obr.SectionHeader.Contains("INITIAL TEST"))
        //                {
        //                    if (obrList.Count == 4)
        //                    {
        //                        dgvMROResult3.DataSource = obr.observatinos;
        //                        dgvMROResult3.Columns[0].HeaderText = obr.SectionHeader;
        //                    }
        //                    else if (obrList.Count == 3)
        //                    {
        //                        dgvMROResult2.DataSource = obr.observatinos;
        //                        dgvMROResult2.Columns[0].HeaderText = obr.SectionHeader;
        //                    }
        //                    else if (obrList.Count == 2)
        //                    {
        //                        dgvMROResult1.DataSource = obr.observatinos;
        //                        dgvMROResult1.Columns[0].HeaderText = obr.SectionHeader;
        //                        dgvMROResult3.Visible = false;
        //                        dgvMROResult2.Size = new System.Drawing.Size(379, 238);
        //                    }
        //                }
        //                else
        //                {
        //                    if (i == 1)
        //                    {
        //                        dgvMROResult1.DataSource = obr.observatinos;
        //                        dgvMROResult1.Columns[0].HeaderText = obr.SectionHeader;
        //                    }
        //                    else if (i == 2)
        //                    {
        //                        dgvMROResult2.DataSource = obr.observatinos;
        //                        dgvMROResult2.Columns[0].HeaderText = obr.SectionHeader;
        //                        dgvMROResult3.Visible = false;
        //                        dgvMROResult2.Size = new System.Drawing.Size(379, 238);
        //                    }
        //                    else if (i == 3)
        //                    {
        //                        dgvMROResult3.DataSource = obr.observatinos;
        //                        dgvMROResult3.Columns[0].HeaderText = obr.SectionHeader;
        //                    }
        //                    else if (i == 4)
        //                    {
        //                        //dataGridView4.DataSource = obr.observatinos;
        //                        //dataGridView4.Columns[0].HeaderText = obr.SectionHeader;
        //                    }
        //                    i++;
        //                }
        //            }
        //        }
        //        if (defaultPositiveFlag == true)
        //        {
        //            txtMROOverallResults.Text = "POSITIVE";
        //        }
        //        if (defaultPositiveFlag == false)
        //        {
        //            txtMROOverallResults.Text = "NEGATIVE";
        //        }
        //    }
        //}

        private void dataGridViewReport_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Cells[1].Value != null && row.Cells[1].Value.ToString() != string.Empty)
                {
                    row.Cells[0].Value = row.Cells[1].Value.ToString() + " - " + row.Cells[2].Value.ToString();
                }
                else
                {
                    row.Cells[0].Value = row.Cells[2].Value;
                }

                if (row.Cells[7].Value != null && row.Cells[7].Value.ToString() != string.Empty)
                {
                    row.Cells[5].Value = row.Cells[7].Value.ToString() + " " + row.Cells[6].Value.ToString();
                }

                if (row.Cells[4].Value != null && row.Cells[4].Value.ToString() == "POS")
                {
                    row.Cells[4].Value = "POSITIVE";
                }
                else if (row.Cells[4].Value != null && row.Cells[4].Value.ToString() == "NEG")
                {
                    row.Cells[4].Value = "NEGATIVE";
                }
            }
        }

        #endregion Result Tab Methods

        private void btnLabReport_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Under Development.");

            ///Determine LabReportType

            HL7ParserDao hl7ParserDao = new HL7ParserDao();

            string specimenId = UASpecimenId;
            int rType = hl7ParserDao.DetermineLabReportType(specimenId);
            ReportInfo reportDetails = new ReportInfo();
            reportDetails = null;
            List<OBR_Info> obrList = null;

            RTFBuilderbase crlReport = null;

            ReportType reportType = ReportType.None;

            if (rType == 1)
            {
                reportType = ReportType.LabReport;
                hl7ParserDao.GetReportLAB(reportType, specimenId, reportDetails, ref obrList, ref crlReport);
            }
            if (rType == 3)
            {
                reportType = ReportType.QuestLabReport;
                hl7ParserDao.GetReportQuest(reportType, specimenId, reportDetails, ref obrList, ref crlReport);
            }

            if (rType == 2)
            {
                reportType = ReportType.MROReport;
                hl7ParserDao.GetReportMRO(reportType, specimenId, reportDetails, ref obrList, ref crlReport);
            }

            //hl7ParserDao.GetReportQuest(reportType, specimenId, reportDetails, ref obrList, ref crlReport);

            //setting back to labreport
            //reportType = ReportType.LabReport;

            reportDetails = hl7ParserDao.GetReportDetails(reportType, specimenId, reportDetails);
            if (crlReport != null)
            {
                string name;
                String timeStamp = GetTimestamp(DateTime.Now);

                name = reportDetails.DonorFirstName + " " + reportDetails.DonorLastName;
                string[] tmpSpeId = specimenId.Split(',');
                if (tmpSpeId.Count() >= 0)
                {
                    specimenId = tmpSpeId[0];
                }

                ReportInfo rtInfo = new ReportInfo();

                rtInfo = hl7ParserDao.GetReportDetails(reportType, specimenId, reportDetails);

                string reportTXT = WTFRTF.DeRtf(rtInfo.LabReport);



                if (reportType == ReportType.QuestLabReport)
                {
                    var fs20loc = rtInfo.LabReport.IndexOf("\\fs20");
                    StringBuilder sb = new StringBuilder();
                    int lengthPad = 20;
                    sb.Append("\n");
                    sb.Append("Name: ".PadRight(lengthPad) + $"{rtInfo.DonorFirstName} {rtInfo.DonorMI} {rtInfo.DonorLastName}");
                    sb.Append("\n");
                    sb.Append("Date Of Birth: ".PadRight(lengthPad) + rtInfo.DonorDOB);
                    sb.Append("\n");
                    sb.Append("Specimen ID: ".PadRight(lengthPad) + rtInfo.SpecimenId);
                    sb.Append("\n");
                    sb.Append("Date Of Collection: ".PadRight(lengthPad) + rtInfo.SpecimenCollectionDate);
                    sb.Append("\n");
                    sb.Append("\n");
                    reportTXT = sb.ToString() + reportTXT;
                    rtInfo.LabReport = rtInfo.LabReport.Insert(fs20loc + 8, sb.ToString());
                }

                rtInfo.LabReport = rtInfo.LabReport.Replace(@"\line", "");
                rtInfo.LabReport = rtInfo.LabReport.Replace(@"{", "");
                rtInfo.LabReport = rtInfo.LabReport.Replace(@"}", "");

                string Report = rtInfo.LabReport.Remove(0, 174);

                string folderName = "Text";
                var folder = Directory.CreateDirectory(folderName);
                // string outputfilename = path.Replace(".doc", ".pdf");
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Text\", name + "_" + timeStamp + ".txt");
                using (FileStream fs = File.Create(path))
                {
                    // Add some text to file
                    Byte[] title = new UTF8Encoding(true).GetBytes(reportTXT);
                    fs.Write(title, 0, title.Length);
                }
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "PDF Files (*.pdf)|*.pdf";
                sfd.FileName = name + "_" + timeStamp + ".pdf";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    String[] content = System.IO.File.ReadAllLines(path);

                    PrintPDF pdf = new PrintPDF();
                    pdf.SavePDF(sfd.FileName, content);

                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    System.Diagnostics.Process.Start(sfd.FileName);

                    //FontFactory.RegisterDirectories();
                    ////  iTextSharp.text.Font myfont = FontFactory.GetFont("Tahoma", BaseFont.IDENTITY_H, 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
                    //iTextSharp.text.Font myfonts = FontFactory.GetFont("Arial", BaseFont.IDENTITY_H, 8, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
                    ////Document pdfDoc = new Document(PageSize.A4,10f,10f,10f, 0f);
                    //Document pdfDoc = new Document(PageSize.A4, 25, 25, 40, 40);
                    //pdfDoc.Open();
                    //// MemoryStream ms = new MemoryStream();

                    //Paragraph Paragraph = new Paragraph(rtInfo.LabReport.ToString());
                    //PdfWriter wri = PdfWriter.GetInstance(pdfDoc, new FileStream(name, FileMode.Create));
                    //pdfDoc.Open();
                    //Paragraph.Font = myfonts;
                    //Paragraph.Alignment = Element.ALIGN_JUSTIFIED_ALL;
                    //pdfDoc.Add(Paragraph);
                    //pdfDoc.Close();
                    // System.Diagnostics.Process.Start(name);
                }
            }
            else
            {
                MessageBox.Show("Report not yet received.");
            }
        }

        private void txtSSN_MouseDown(object sender, MouseEventArgs e)
        {
            if (Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper())
            {
                if (txtSSN.Text != string.Empty)
                {
                    // txtSSN.Text = txtSSN.Tag.ToString();
                    // txtSSN.Text = txtSSN.Tag.ToString();
                    txtSSN.Text = string.Empty;
                }
            }
        }

        private void txtSSN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (txtSSN.Text.Length >= 9)
            {
                e.Handled = true;
            }
            else if (txtSSN.Text.Length <= 9)
            {
                e.Handled = false;
            }

            if (!(Char.IsDigit(e.KeyChar) || char.IsWhiteSpace(e.KeyChar) || char.IsControl(e.KeyChar)))
            {
                e.Handled = true;
            }

            if (e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space)
            {
                e.Handled = false;
            }
        }

        private void dgvTestHistory_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvTestHistory.Rows.Count > 0 && e.ColumnIndex >= 0 || e.ColumnIndex == -1)
            {
                if (e.RowIndex != -1 || e.ColumnIndex == 0)
                {
                    DataGridViewCheckBoxCell fieldSelection = (DataGridViewCheckBoxCell)dgvTestHistory.Rows[e.RowIndex].Cells["DonorSelection"];

                    if (Convert.ToBoolean(fieldSelection.Value))
                    {
                        fieldSelection.Value = false;
                    }
                    else
                    {
                        fieldSelection.Value = true;
                    }
                }
            }
        }

        private void dgvDocuments_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvDocuments.Rows.Count > 0 && e.ColumnIndex >= 0 || e.ColumnIndex == -1)
            {
                if (e.RowIndex != -1 || e.ColumnIndex == 0)
                {
                    DataGridViewCheckBoxCell fieldSelection = (DataGridViewCheckBoxCell)dgvDocuments.Rows[e.RowIndex].Cells["View"];

                    if (Convert.ToBoolean(fieldSelection.Value))
                    {
                        fieldSelection.Value = false;
                    }
                    else
                    {
                        fieldSelection.Value = true;
                    }
                }
            }
        }

        private void txtLegalInfoNotes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (txtLegalInfoNotes.Text.Length > 1246)
            {
                txtLegalInfoNotes.ScrollBars = ScrollBars.Vertical;
            }
            else
            {
                txtLegalInfoNotes.ScrollBars = ScrollBars.None;
            }
        }

        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }

        private void tcMain_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnMRO_Click(object sender, EventArgs e)
        {
            string path = string.Empty;
            DonorDocument donorDocument = donorBL.GetDonorDocument(documentReportId, false);
            if (!(donorDocument == null))
            {
                int donorDocumentId = donorDocument.DonorDocumentId;
                string fileName = donorDocument.FileName;
                string documentTitle = donorDocument.DocumentTitle;

                if (fileName != string.Empty && donorDocumentId != 0)
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "PDF Files (*.pdf)|*.pdf";
                    sfd.FileName = fileName;   //+"_" + timeStamp + ".pdf";
                                               // path = sfd.FileName;
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        //  path = sfd.FileName;
                        string targetFile = sfd.FileName; //+ "\\" + donorDocumentId + "_" + documentTitle + "." + fileName.Trim().Substring(fileName.Trim().LastIndexOf('.') + 1);

                        File.WriteAllBytes(targetFile, donorDocument.DocumentContent);
                        Process Proc = new Process();
                        Proc.StartInfo.FileName = targetFile;
                        Proc.Start();
                    }
                }
            }
            else
            {
                MessageBox.Show($"Document with ID {documentReportId} was not found!!");
            }

        }

        private void cmbLocationName_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<User> userList = new List<User>();

            if (cmbLocationName.SelectedIndex != 0)
            {
                VendorBL vendorBL = new VendorBL();
                Vendor vendor = vendorBL.Get(Convert.ToInt32(cmbLocationName.SelectedValue));

                txtLocationName.Text = vendor.VendorName;

                foreach (VendorAddress address in vendor.Addresses)
                {
                    if (address.AddressTypeId == AddressTypes.PhysicalAddress1)
                    {
                        txtCollectionAddress1.Text = address.Address1;
                        txtCollectionAddress2.Text = address.Address2;
                        txtCollectionCity.Text = address.City;
                        txtCollectionState.Text = address.State;
                        txtCollectionZipCode.Text = address.ZipCode;
                        txtCollectionPhone.Text = address.Phone;
                        txtCollectionFax.Text = address.Fax;
                        txtCollectionEmail.Text = address.Email;

                        break;
                    }
                }

                UserBL userBL = new UserBL();
                int vendorId = (int)cmbLocationName.SelectedValue;
                userList = userBL.GetVendorName(vendorId);

                User tmpUser = new User();
                tmpUser.UserId = 0;
                tmpUser.UserFirstName = "(Select Collector Name)";

                userList.Insert(0, tmpUser);
            }
            else
            {
                txtLocationName.Text = string.Empty;
                txtCollectionAddress1.Text = string.Empty;
                txtCollectionAddress2.Text = string.Empty;
                txtCollectionCity.Text = string.Empty;
                txtCollectionState.Text = string.Empty;
                txtCollectionZipCode.Text = string.Empty;
                txtCollectionPhone.Text = string.Empty;
                txtCollectionFax.Text = string.Empty;
                txtCollectionEmail.Text = string.Empty;
                txtScreeningDate.Text = string.Empty;
                txtScreeningTime.Text = string.Empty;
                txtCollectorName.Text = string.Empty;

                User tmpUser = new User();
                tmpUser.UserId = 0;
                tmpUser.UserFirstName = "(Select Collector Name)";

                userList.Insert(0, tmpUser);
            }

            cmbCollectionName.DataSource = userList;
            cmbCollectionName.ValueMember = "UserId";
            cmbCollectionName.DisplayMember = "UserDisplayName";
        }

        private void lst_ActivityNote_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = (int)e.Graphics.MeasureString(lstActivityNoteHistory.Items[e.Index].ToString(), lstActivityNoteHistory.Font, lstActivityNoteHistory.Width).Height;
        }

        private void lst_ActivityNote_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
            e.Graphics.DrawString(lstActivityNoteHistory.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds);
        }

        private void chkHideWeb_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkHideWeb.Checked)
            {
                DialogResult result = MessageBox.Show("Are you sure you would like to hide this Donor on the Web?", "Hide From Web", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    FrmHideWebInput frm = new FrmHideWebInput(currentTestInfoId, currentDonorId, donorBL, Program.currentUserId, true);
                    frm.ShowDialog();

                    // MessageBox.Show("Good Bopuasdf");
                }
            }
            else
            {
                DialogResult result2 = MessageBox.Show("Are you sure you would like to enable this user to be seen on the web?", "Show On Web", MessageBoxButtons.YesNo);
                if (result2 == DialogResult.Yes)
                {
                    FrmHideWebInput frm = new FrmHideWebInput(currentTestInfoId, currentDonorId, donorBL, Program.currentUserId, false);
                    frm.ShowDialog();

                    // MessageBox.Show("Good Bopuasdf");
                }
            }
        }

        private void chkHideWeb_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void btnChangeClient_Click(object sender, EventArgs e)
        {
            DialogResult result2 = MessageBox.Show("Are you sure you want to change the Client and Department?", "Change", MessageBoxButtons.YesNo);
            if (result2 == DialogResult.Yes)
            {
                this.cmbClient.Visible = true;
                this.cmbDepartment.Visible = true;
            }
        }

        private void txtClearStarProfileId_TextChanged(object sender, EventArgs e)
        {
        }

        private void btnSendNotification_Click(object sender, EventArgs e)
        {
            try
            {
                string mbMessage = string.Empty;
                List<int> donor_test_info_ids = new List<int>();
                donor_test_info_ids.Add(notification.donor_test_info_id);
                bool force_db = false;

                if ((int)notification.clinic_exception > 50 && (int)notification.clinic_exception < 60)
                {

                    var _ok = MessageBoxManager.OK;
                    var _can = MessageBoxManager.Cancel;


                    MessageBoxManager.OK = "Use FormFox";
                    MessageBoxManager.Cancel = "Use Database";


                    if (MessageBox.Show($"This has failed to send in via Form Fox. Try FormFox Again?", "Try FormFox Again", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        force_db = true;
                    }
                    MessageBoxManager.OK = _ok;
                    MessageBoxManager.Cancel = _can;

                }

                backendLogic.DoSendIn(donor_test_info_ids, this.radius, Program.currentUserId, force_db);

                //if (!notification.in_window)
                //{
                //    Tuple<bool, bool> confirm = backendLogic.ConfirmSendIn();
                //    Cursor.Current = Cursors.WaitCursor;
                //    if (confirm.Item1) // sched
                //    {
                //        backendLogic.SetDonorNotificationNextWindow(notification.donor_test_info_id, Program.currentUserId);
                //    }
                //    if (confirm.Item2) // now
                //    {
                //        backendLogic.SetDonorNotificationNow(notification.donor_test_info_id, Program.currentUserId);
                //    }
                //}
                //else
                //{
                //    Cursor.Current = Cursors.WaitCursor;
                //    backendLogic.SetDonorNotificationNextWindow(notification.donor_test_info_id, Program.currentUserId);
                //}

                //Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was a problem scheduling notifications!\r\nPlease copy all this content and communicate it to report the error.\r\n" + ex.Message, "Error");
                //throw;
            }

            //// Only show this if outside the window
            //if (MessageBox.Show("Notify Next Schedule for Departmenet?", "Schedule Notification", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            //{
            //    bool result = backendLogic.NotificationSetforNextWindowNotification(this.currentTestInfoId);
            //    if (result)
            //    {
            //        MessageBox.Show("Will notify immediately!", "Scheduled");
            //    }
            //    else
            //    {
            //        MessageBox.Show("Unable to Set!", "Not Scheduled");
            //    }
            //}
            //else if (MessageBox.Show("Notify Immediately?", "Schedule Notification", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            //{
            //    bool result = backendLogic.NotificationSetforImmediateNotification(this.currentTestInfoId);
            //    if (result)
            //    {
            //        MessageBox.Show("Schedule to notify!", "Scheduled");
            //    }
            //    else
            //    {
            //        MessageBox.Show("Unable to schedule!", "Not Scheduled");
            //    }
            //}
        }

        private void btnSendSMS_Click(object sender, EventArgs e)
        {
            FrmSmsSendForm frmSmsSendForm = new FrmSmsSendForm(Program._logger);
            DonorTestInfo donorTestInfo = donorBL.GetDonorTestInfo(this.currentTestInfoId);

            frmSmsSendForm.donor_test_info_id = currentTestInfoId;
            frmSmsSendForm.client_department_id = donorTestInfo.ClientDepartmentId;
            frmSmsSendForm.client_id = donorTestInfo.ClientId;
            frmSmsSendForm.donor_id = currentDonorId;
            frmSmsSendForm.activity_user_id = Program.currentUserId;
            DialogResult smsSendDR = frmSmsSendForm.ShowDialog(this);
            frmSmsSendForm.Dispose();
            if (smsSendDR == DialogResult.OK)
            {
                LoadActivityDetails();
            }
            else if (smsSendDR == DialogResult.Abort)
            {
                MessageBox.Show("There was problem sending an SMS");
            }
            frmSmsSendForm.Dispose();
        }

        private void chkShowSystemRecords_CheckedChanged(object sender, EventArgs e)
        {
            LoadDocumentDetails(chkShowSystemRecords.Checked);
        }
    }
}