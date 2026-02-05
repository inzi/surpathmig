using SurPath.Business;
using SurPath.Business.Master;
using SurPath.Entity;
using SurPath.Entity.Master;
using SurPath.Enum;
using SurpathBackend;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmUserDetails : Form
    {
        #region Private Variables

        private OperationMode _mode = OperationMode.None;
        private int _userId = 0;
        private int Id = 0;
        private ClientBL clientBL = new ClientBL();

        private UserBL userBL = new UserBL();
        private bool viewFlag = false;

        #endregion Private Variables

        #region Constructor

        public FrmUserDetails()
        {
            InitializeComponent();
        }

        public FrmUserDetails(OperationMode mode, int userId)
        {
            InitializeComponent();

            this._mode = mode;
            this._userId = userId;
        }

        #endregion Constructor

        #region Event Methods

        private void FrmUserDetails_Load(object sender, EventArgs e)
        {
            cmbUserType.SelectedIndex = 0;

            try
            {
                InitializeControls();

                if (this._mode != OperationMode.None && this._userId != 0)
                {
                    LoadData();

                    rbtnSurScan.Enabled = false;
                    rbtnDonor.Enabled = false;
                    rbtnClient.Enabled = false;
                    rbtnVendor.Enabled = false;
                    rbtnAttorney.Enabled = false;
                    rbtnCourt.Enabled = false;
                    rbtnJudge.Enabled = false;
                    // cmbUserType.Enabled = false;
                }
                else if (this._mode == OperationMode.New && this._userId == 0)
                {
                    rbtnDonor.Enabled = false;
                    rbtnAttorney.Enabled = false;
                    rbtnCourt.Enabled = false;
                    rbtnJudge.Enabled = false;

                    if (rbtnSurScan.Checked == true)
                    {
                        //for(int i=0;i<tvClientRule.Nodes.Count;i++)
                        //{
                        // tvClientRule.Nodes[i].Checked = true;
                        CheckAllNodes(tvClientRule.Nodes);
                        // }
                    }

                    if (rbtnDonor.Checked == true)
                    {
                        //for (int i = 0; i < tvClientRule.Nodes.Count; i++)
                        //{
                        //    tvClientRule.Nodes[i].Checked = false;
                        //}
                        UncheckAllNodes(tvClientRule.Nodes);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while loading the page.");
            }
        }

        private void FrmUserDetails_FormClosing(object sender, FormClosingEventArgs e)
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

        private void btnUserClose_Click(object sender, EventArgs e)
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
                    MessageBox.Show("Username cannot be empty.");
                    txtUsername.Focus();
                }
                else
                {
                    Cursor.Current = Cursors.WaitCursor;
                    if (ValidateUsername())
                    {
                        MessageBox.Show("Username is available.");
                    }
                    else
                    {
                        MessageBox.Show("Username is not available.");
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

        private void rbtnSurScan_CheckedChanged(object sender, EventArgs e)
        {
            rbtnSurScan.CausesValidation = false;
            LoadDepartments();
            lblDepartmentName.Visible = true;
            lblDepartmentName.Text = "Department Name";
            lblUserTypeDonor.Visible = false;
            cmbUserType.Visible = true;
            lblUserTypeStar.Visible = true;
            lblClient.Visible = true;
            tvClientRule.Visible = true;
            lblAuthorization.Visible = true;
            tvAuthorizationRule.Visible = true;
            lblUserTypeStar.Left = lblDepartmentName.Left + lblDepartmentName.Width;

            CheckAllNodes(tvClientRule.Nodes);
        }

        private void rbtnDonor_CheckedChanged(object sender, EventArgs e)
        {
            rbtnDonor.CausesValidation = false;
            LoadDonor();
            lblUserTypeDonor.Visible = true;
            cmbUserType.Visible = false;
            lblDepartmentName.Visible = true;
            lblDepartmentName.Text = "Donor Name";
            lblUserTypeStar.Visible = false;
            lblClient.Visible = true;
            tvClientRule.Visible = true;
            lblAuthorization.Visible = true;
            tvAuthorizationRule.Visible = true;

            UncheckAllNodes(tvClientRule.Nodes);
        }

        private void rbtnClient_CheckedChanged(object sender, EventArgs e)
        {
            rbtnClient.CausesValidation = false;
            LoadClients();
            lblUserTypeDonor.Visible = false;
            lblDepartmentName.Visible = false;
            lblDepartmentName.Text = "Client Name";
            cmbUserType.Visible = false;
            lblUserTypeStar.Visible = false;
            lblClient.Visible = true;
            tvClientRule.Visible = true;
            lblAuthorization.Visible = true;
            tvAuthorizationRule.Visible = true;

            UncheckAllNodes(tvClientRule.Nodes);

            lblUserTypeStar.Left = lblDepartmentName.Left + lblDepartmentName.Width;
        }

        private void rbtnVendor_CheckedChanged(object sender, EventArgs e)
        {
            rbtnVendor.CausesValidation = false;
            LoadVendors();
            lblUserTypeDonor.Visible = false;
            lblDepartmentName.Visible = true;
            lblDepartmentName.Text = "Vendor Name";
            cmbUserType.Visible = true;
            lblUserTypeStar.Visible = true;
            lblClient.Visible = true;
            tvClientRule.Visible = true;
            lblAuthorization.Visible = true;
            tvAuthorizationRule.Visible = true;

            lblUserTypeStar.Left = lblDepartmentName.Left + lblDepartmentName.Width;

            CheckAllNodes(tvClientRule.Nodes);
        }

        private void rbtnAttorney_CheckedChanged(object sender, EventArgs e)
        {
            rbtnAttorney.CausesValidation = false;
            LoadAttorney();
            lblUserTypeAttorney.Visible = true;
            cmbUserType.Visible = false;
            lblDepartmentName.Visible = true;
            lblDepartmentName.Text = "Attorney Name";
            lblUserTypeStar.Visible = false;
            lblClient.Visible = true;
            tvClientRule.Visible = true;
            lblAuthorization.Visible = true;
            tvAuthorizationRule.Visible = true;

            CheckAllNodes(tvClientRule.Nodes);
        }

        private void rbtnCourt_CheckedChanged(object sender, EventArgs e)
        {
            rbtnCourt.CausesValidation = false;
            LoadCourt();
            lblUserTypeCourt.Visible = true;
            cmbUserType.Visible = false;
            lblDepartmentName.Visible = true;
            lblDepartmentName.Text = "Court Name";
            lblUserTypeStar.Visible = false;
            lblClient.Visible = true;
            tvClientRule.Visible = true;
            lblAuthorization.Visible = true;
            tvAuthorizationRule.Visible = true;

            CheckAllNodes(tvClientRule.Nodes);
        }

        private void rbtnJudge_CheckedChanged(object sender, EventArgs e)
        {
            rbtnJudge.CausesValidation = false;
            LoadJudge();
            lblUserTypeJudge.Visible = true;
            cmbUserType.Visible = false;
            lblDepartmentName.Visible = true;
            lblDepartmentName.Text = "Judge Name";
            lblUserTypeStar.Visible = false;
            lblClient.Visible = true;
            tvClientRule.Visible = true;
            lblAuthorization.Visible = true;
            tvAuthorizationRule.Visible = true;

            CheckAllNodes(tvClientRule.Nodes);
        }

        private void chkActive_CheckedChanged(object sender, EventArgs e)
        {
            chkActive.CausesValidation = false;
        }

        private void tvClientRule_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //
        }

        private void tvClientRule_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //
        }

        private void txtPhone_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void txtFax_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        #endregion Event Methods

        #region Public Methods

        public void LoadData()
        {
            try
            {
                if (this._mode == OperationMode.Edit)
                {
                    txtUsername.ReadOnly = true;

                    User user = userBL.Get(this._userId);

                    if (user != null)
                    {
                        txtFirstName.Text = user.UserFirstName;
                        txtLastName.Text = user.UserLastName;
                        txtPhone.Text = user.UserPhoneNumber;
                        txtFax.Text = user.UserFax;
                        txtEmail.Text = user.UserEmail;

                        chkActive.Checked = user.IsUserActive;
                        //if (user.Username.ToUpper() != "ADMIN")
                        if (!(user.Username.ToUpper() == Program.superAdmin.ToUpper() || user.Username.ToUpper() == Program.superAdmin1.ToUpper() || user.Username.ToUpper() == Program.superAdmin2.ToUpper()))
                        {
                            if (user.UserType == UserType.TPA)
                            {
                                rbtnSurScan.Checked = true;
                                LoadDepartments();

                                LoadClientName();
                                cmbUserType.SelectedValue = user.DepartmentId;
                                cmbUserType.Enabled = true;
                            }
                            else if (user.UserType == UserType.Donor)
                            {
                                rbtnDonor.Checked = true;
                                LoadDonor();
                                LoadClientName();
                            }
                            else if (user.UserType == UserType.Client)
                            {
                                rbtnClient.Checked = true;
                                // LoadClients();

                                LoadClientName();
                                cmbUserType.Text = string.Empty;
                                cmbUserType.Enabled = false;
                                //cmbUserType.SelectedValue = user.ClientId;
                            }
                            else if (user.UserType == UserType.Vendor)
                            {
                                rbtnVendor.Checked = true;
                                LoadVendors();

                                LoadClientName();
                                cmbUserType.SelectedValue = user.VendorId;
                                cmbUserType.Enabled = true;
                            }
                            else if (user.UserType == UserType.Attorney)
                            {
                                rbtnAttorney.Checked = true;
                                LoadAttorney();
                                LoadClientName();
                            }
                            else if (user.UserType == UserType.Court)
                            {
                                rbtnCourt.Checked = true;
                                lblLastMan.Visible = false;
                                lblEmailMan.Visible = false;
                                LoadCourt();
                                LoadClientName();
                            }
                            else if (user.UserType == UserType.Judge)
                            {
                                lblEmailMan.Visible = false;
                                rbtnJudge.Checked = true;
                                LoadJudge();
                                LoadClientName();
                            }
                        }
                        else
                        {
                            user.UserType = UserType.TPA;
                            lblUserType.Enabled = false;
                            lblDepartmentName.Enabled = false;
                            lblDepartmentName.Text = "Department Name";
                            lblUserTypeDonor.Enabled = false;
                            cmbUserType.Enabled = false;
                            lblUserTypeStar.Enabled = false;
                            lblClient.Enabled = false;
                            tvClientRule.Enabled = false;
                            lblAuthorization.Enabled = false;
                            tvAuthorizationRule.Enabled = false;
                            btnClientCollapseAll.Enabled = false;
                            btnClientExpandAll.Enabled = false;
                            btnAuthExpandAll.Enabled = false;
                            btnAuthCollapseAll.Enabled = false;
                            lblUserTypeStar.Left = lblDepartmentName.Left + lblDepartmentName.Width;
                            if (user.UserType == UserType.TPA)
                            {
                                if (user.DepartmentId != null)
                                {
                                    cmbUserType.SelectedValue = user.DepartmentId;
                                }
                                else
                                {
                                    cmbUserType.SelectedValue = 0;
                                }
                            }
                            //btnResetPassword.Visible = false;
                            //btnOK.Enabled = false;
                            //txtEmail.Enabled = false;
                            //txtFirstName.Enabled = false;
                            //txtLastName.Enabled = false;
                            //txtPhone.Enabled = false;
                            //txtFax.Enabled = false;
                            //chkActive.Enabled = false;
                        }

                        if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()) && viewFlag == true)
                        {
                            cmbUserType.Enabled = false;
                        }

                        txtUsername.Text = user.Username;
                        txtPassword.Text = UserAuthentication.Decrypt(user.UserPassword, true);
                        chkActive.Checked = user.IsUserActive;

                        LoadClientName();
                        LoadAuthorization();

                        //
                        if (user.ClientDepartmentList.Count > 0)
                        {
                            foreach (TreeNode clientNode in tvClientRule.Nodes)
                            {
                                if (clientNode.Nodes.Count > 0)
                                {
                                    int count = 0;
                                    foreach (TreeNode deptNode in clientNode.Nodes)
                                    {
                                        if (deptNode.Tag.ToString().StartsWith("Department"))
                                        {
                                            string[] dept = deptNode.Tag.ToString().Split('#');
                                            if (dept.Length == 3)
                                            {
                                                foreach (int clientDepartmentId in user.ClientDepartmentList)
                                                {
                                                    if (clientDepartmentId == Convert.ToInt32(dept[1]))
                                                    {
                                                        deptNode.Checked = true;
                                                        count++;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (clientNode.Nodes.Count == count)
                                    {
                                        clientNode.Checked = true;
                                    }
                                }
                            }
                        }

                        //Auth Rules
                        if (user.AuthRuleList.Count > 0)
                        {
                            foreach (TreeNode level1Node in tvAuthorizationRule.Nodes)
                            {
                                if (level1Node.Nodes.Count > 0)
                                {
                                    int parentCount = 0;
                                    foreach (TreeNode level2Node in level1Node.Nodes)
                                    {
                                        if (level2Node.Nodes.Count > 0)
                                        {
                                            int count = 0;
                                            foreach (TreeNode level3Node in level2Node.Nodes)
                                            {
                                                if (level3Node.Tag.ToString().StartsWith("AuthRule"))
                                                {
                                                    string[] authRule = level3Node.Tag.ToString().Split('#');
                                                    if (authRule.Length == 3)
                                                    {
                                                        foreach (int authRuleId in user.AuthRuleList)
                                                        {
                                                            if (authRuleId == Convert.ToInt32(authRule[1]))
                                                            {
                                                                level3Node.Checked = true;
                                                                count++;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            if (level2Node.Nodes.Count == count)
                                            {
                                                level2Node.Checked = true;
                                                parentCount++;
                                            }
                                        }
                                        else
                                        {
                                            if (level2Node.Tag.ToString().StartsWith("AuthRule"))
                                            {
                                                string[] authRule = level2Node.Tag.ToString().Split('#');
                                                if (authRule.Length == 3)
                                                {
                                                    foreach (int authRuleId in user.AuthRuleList)
                                                    {
                                                        if (authRuleId == Convert.ToInt32(authRule[1]))
                                                        {
                                                            level2Node.Checked = true;
                                                            parentCount++;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (level1Node.Nodes.Count == parentCount)
                                    {
                                        level1Node.Checked = true;
                                    }
                                }
                            }
                        }

                        //
                        ResetControlsCauseValidation();

                        UserHistory();


                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UserHistory()
        {
            // Get History
            BackendLogic backendLogic = new BackendLogic();
            List<UserActivity> userActivities = backendLogic.GetUserActivities(this._userId);
            // get the last 10 activities
            userActivities = userActivities.Take(10).ToList();

            // populate listUserActivity
            foreach(UserActivity activity in userActivities)
            {
                string[] data = new string[] { ((UserActivityCategories)activity.activity_user_category_id).ToString(), activity.activity_datetime.ToString("MM/dd/yyyy hh:mm tt"), activity.activity_note };
                ListViewItem item = new ListViewItem(data);
                listUserActivity.Items.Add(item);
            }

        }

        #endregion Public Methods

        #region Private Methods

        private void InitializeControls()
        {
            RandomStringGenerator rsg = new RandomStringGenerator(true, false, true, false);
            string newPassword = rsg.Generate(6, 8);

            if (this._mode == OperationMode.Edit)
            {
                btnAvailability.Visible = false;
            }

            if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
            {
                //USER_VIEW
                DataRow[] userView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.USER_VIEW.ToDescriptionString() + "'");

                if (userView.Length > 0)
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
                }
            }

            rbtnSurScan.Checked = true;
            lblDepartmentName.Visible = true;
            lblUserTypeStar.Visible = true;

            chkActive.Checked = true;
            txtPassword.Text = newPassword;

            tvClientRule.Nodes.Clear();

            LoadDepartments();

            LoadClientName();

            LoadAuthorization();

            tvClientRule.CollapseAll();
            tvAuthorizationRule.CollapseAll();

            if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
            {
                //USER_VIEW_PASSWORD
                DataRow[] userViewPassword = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.USER_VIEW_PASSWORD.ToDescriptionString() + "'");

                if (userViewPassword.Length > 0)
                {
                    chkShowPassword.Visible = true;
                }
                else
                {
                    chkShowPassword.Visible = false;
                }
            }

            ResetControlsCauseValidation();
        }

        private void LoadDepartments()
        {
            DepartmentBL departmentBL = new DepartmentBL();
            List<Department> departmentList = departmentBL.GetList();

            Department tmpDepartment = new Department();
            tmpDepartment.DepartmentId = 0;
            tmpDepartment.DepartmentNameValue = "(Select Department)";

            departmentList.Insert(0, tmpDepartment);

            cmbUserType.DataSource = departmentList;
            cmbUserType.ValueMember = "DepartmentId";
            cmbUserType.DisplayMember = "DepartmentNameValue";
        }

        private void LoadDonor()
        {
            try
            {
                DonorBL donorBL = new DonorBL();
                User user = userBL.Get(this._userId);
                Id = (int)user.DonorId;
                Donor tmpdonor = new Donor();
                tmpdonor = donorBL.Get(Id, "Desktop");
                int donorId = tmpdonor.DonorId;
                string Name = tmpdonor.DonorFirstName + " " + tmpdonor.DonorLastName;
                lblUserTypeDonor.Text = Name.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadClients()
        {
            try
            {
                ClientBL clientBL = new ClientBL();
                List<Client> clientList = clientBL.GetList();

                Client tmpClient = new Client();
                tmpClient.ClientId = 0;
                tmpClient.ClientName = "(Select Client)";

                clientList.Insert(0, tmpClient);

                cmbUserType.DataSource = clientList;
                cmbUserType.ValueMember = "ClientId";
                cmbUserType.DisplayMember = "ClientName";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadVendors()
        {
            try
            {
                VendorBL vendorBL = new VendorBL();
                List<Vendor> vendorList = vendorBL.GetList();

                Vendor tmpVendor = new Vendor();
                tmpVendor.VendorId = 0;
                tmpVendor.VendorName = "(Select Vendor)";

                vendorList.Insert(0, tmpVendor);

                cmbUserType.DataSource = vendorList;
                cmbUserType.ValueMember = "VendorId";
                cmbUserType.DisplayMember = "VendorName";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadAttorney()
        {
            try
            {
                AttorneyBL attorneyBL = new AttorneyBL();
                User user = userBL.Get(this._userId);
                Id = (int)user.AttorneyId;
                Attorney tmpattorney = new Attorney();
                tmpattorney = attorneyBL.Get(Id);
                int attorneyId = tmpattorney.AttorneyId;
                string Name = tmpattorney.AttorneyFirstName + " " + tmpattorney.AttorneyLastName;
                lblUserTypeAttorney.Text = Name.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadCourt()
        {
            try
            {
                CourtBL courtBL = new CourtBL();
                User user = userBL.Get(this._userId);
                Id = (int)user.CourtId;
                Court tmpcourt = new Court();
                tmpcourt = courtBL.Get(Id);
                int courtId = tmpcourt.CourtId;
                string Name = tmpcourt.CourtName;
                lblUserTypeCourt.Text = Name.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadJudge()
        {
            try
            {
                JudgeBL judgeBL = new JudgeBL();
                User user = userBL.Get(this._userId);
                Id = (int)user.JudgeId;
                Judge tmpjudge = new Judge();
                tmpjudge = judgeBL.Get(Id);
                int judgeId = tmpjudge.JudgeId;
                string Name = tmpjudge.JudgeFirstName + " " + tmpjudge.JudgeLastName;
                lblUserTypeJudge.Text = Name.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadClientName()
        {
            try
            {
                tvClientRule.Nodes.Clear();

                DataTable dtClients = clientBL.GetListDT();
                DataTable dtDepartments = clientBL.GetClientDepartmentList();

                foreach (DataRow drClient in dtClients.Rows)
                {
                    List<TreeNode> clientNameNodeList = new List<TreeNode>();

                    DataRow[] departments = dtDepartments.Select("ClientId = " + drClient["ClientId"].ToString() + "");

                    if (departments.Length > 0)
                    {
                        foreach (DataRow drDepartment in departments)
                        {
                            TreeNode DeptNameNode = new TreeNode(drDepartment["DepartmentName"].ToString());

                            DeptNameNode.Text = drDepartment["DepartmentName"].ToString();
                            DeptNameNode.ToolTipText = drDepartment["DepartmentName"].ToString();
                            DeptNameNode.Tag = "Department#" + drDepartment["ClientDepartmentId"].ToString() + "#" + drDepartment["ClientId"].ToString();

                            clientNameNodeList.Add(DeptNameNode);
                        }
                    }

                    TreeNode clientNameNode = new TreeNode(drClient["ClientName"].ToString(), clientNameNodeList.ToArray<TreeNode>());
                    clientNameNode.Text = drClient["ClientName"].ToString();
                    clientNameNode.ToolTipText = drClient["ClientName"].ToString();
                    clientNameNode.Tag = "Client#" + drClient["ClientId"].ToString();

                    tvClientRule.Nodes.Add(clientNameNode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadAuthorization()
        {
            try
            {
                tvAuthorizationRule.Nodes.Clear();

                DataTable dtAuthCategories = userBL.GetUserAuthCategories();

                DataTable dtAuthRules = userBL.GetUserAuthRules();

                foreach (DataRow drCategory in dtAuthCategories.Rows)
                {
                    if (drCategory["AuthRuleCategoryParentId"].ToString() == string.Empty)
                    {
                        List<TreeNode> authRuleCategoryNodeList = new List<TreeNode>();

                        DataRow[] childCategory = dtAuthCategories.Select("AuthRuleCategoryParentId = " + drCategory["AuthRuleCategoryId"].ToString() + "");

                        if (childCategory.Length > 0)
                        {
                            foreach (DataRow drChildCategory in childCategory)
                            {
                                List<TreeNode> authRuleChildCategoryNodeList = new List<TreeNode>();

                                DataRow[] authRules = dtAuthRules.Select("AuthRuleCategoryId = " + drChildCategory["AuthRuleCategoryId"].ToString() + "");

                                if (authRules.Length > 0)
                                {
                                    foreach (DataRow drRule in authRules)
                                    {
                                        TreeNode authRuleNode = new TreeNode(drRule["AuthRuleName"].ToString());

                                        authRuleNode.Text = drRule["AuthRuleName"].ToString();
                                        authRuleNode.ToolTipText = drRule["AuthRuleName"].ToString();
                                        authRuleNode.Tag = "AuthRule#" + drRule["AuthRuleId"].ToString() + "#" + drRule["AuthRuleCategoryId"].ToString();

                                        authRuleChildCategoryNodeList.Add(authRuleNode);
                                    }
                                }

                                TreeNode authChildCategoryNode = new TreeNode(drChildCategory["AuthRuleCategoryName"].ToString(), authRuleChildCategoryNodeList.ToArray<TreeNode>());

                                authChildCategoryNode.Text = drChildCategory["AuthRuleCategoryName"].ToString();
                                authChildCategoryNode.ToolTipText = drChildCategory["AuthRuleCategoryName"].ToString();
                                authChildCategoryNode.Tag = "AuthCategory#" + drChildCategory["AuthRuleCategoryId"].ToString();

                                authRuleCategoryNodeList.Add(authChildCategoryNode);
                            }

                            TreeNode authCategoryNode = new TreeNode(drCategory["AuthRuleCategoryName"].ToString(), authRuleCategoryNodeList.ToArray<TreeNode>());

                            authCategoryNode.Text = drCategory["AuthRuleCategoryName"].ToString();
                            authCategoryNode.ToolTipText = drCategory["AuthRuleCategoryName"].ToString();
                            authCategoryNode.Tag = "AuthCategory#" + drCategory["AuthRuleCategoryId"].ToString();

                            tvAuthorizationRule.Nodes.Add(authCategoryNode);
                        }
                        else
                        {
                            DataRow[] authRules = dtAuthRules.Select("AuthRuleCategoryId = " + drCategory["AuthRuleCategoryId"].ToString() + "");

                            if (authRules.Length > 0)
                            {
                                foreach (DataRow drRule in authRules)
                                {
                                    TreeNode authRuleNode = new TreeNode(drRule["AuthRuleName"].ToString());

                                    authRuleNode.Text = drRule["AuthRuleName"].ToString();
                                    authRuleNode.ToolTipText = drRule["AuthRuleName"].ToString();
                                    authRuleNode.Tag = "AuthRule#" + drRule["AuthRuleId"].ToString() + "#" + drRule["AuthRuleCategoryId"].ToString();

                                    authRuleCategoryNodeList.Add(authRuleNode);
                                }
                            }

                            TreeNode authCategoryNode = new TreeNode(drCategory["AuthRuleCategoryName"].ToString(), authRuleCategoryNodeList.ToArray<TreeNode>());

                            authCategoryNode.Text = drCategory["AuthRuleCategoryName"].ToString();
                            authCategoryNode.ToolTipText = drCategory["AuthRuleCategoryName"].ToString();
                            authCategoryNode.Tag = "AuthCategory#" + drCategory["AuthRuleCategoryId"].ToString();

                            tvAuthorizationRule.Nodes.Add(authCategoryNode);
                        }
                    }
                }
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

                User user = null;

                if (this._mode == OperationMode.New)
                {
                    user = new User();
                    user.UserId = 0;
                    user.CreatedBy = Program.currentUserName;
                }
                else if (this._mode == OperationMode.Edit)
                {
                    user = userBL.Get(this._userId);
                }

                user.UserFirstName = txtFirstName.Text.Trim();
                user.UserLastName = txtLastName.Text.Trim();
                user.UserPhoneNumber = txtPhone.Text.Trim();
                user.UserFax = txtFax.Text.Trim();
                user.UserEmail = txtEmail.Text.Trim();
                user.IsUserActive = chkActive.Checked;
                user.Username = txtUsername.Text;
                user.DepartmentId = null;
                user.DonorId = null;
                user.ClientId = null;
                user.VendorId = null;
                user.AttorneyId = null;
                user.CourtId = null;
                user.JudgeId = null;

                if (rbtnSurScan.Checked)
                {
                    if (!(user.Username.ToUpper() == Program.superAdmin.ToUpper() || user.Username.ToUpper() == Program.superAdmin1.ToUpper() || user.Username.ToUpper() == Program.superAdmin2.ToUpper()))
                    {
                        user.UserType = UserType.TPA;
                        user.DepartmentId = Convert.ToInt32(cmbUserType.SelectedValue);
                    }
                    else
                    {
                        user.UserType = UserType.TPA;
                        if (cmbUserType.SelectedIndex != 0)
                        {
                            user.DepartmentId = Convert.ToInt32(cmbUserType.SelectedValue);
                        }
                        else
                        {
                            user.DepartmentId = null;
                        }
                    }
                }
                else if (rbtnDonor.Checked)
                {
                    user.UserType = UserType.Donor;
                    user.DonorId = Convert.ToInt32(Id);
                }
                else if (rbtnClient.Checked)
                {
                    user.UserType = UserType.Client;
                    // user.ClientId = Convert.ToInt32(cmbUserType.SelectedValue);
                    user.ClientId = null;
                }
                else if (rbtnVendor.Checked)
                {
                    user.UserType = UserType.Vendor;
                    user.VendorId = Convert.ToInt32(cmbUserType.SelectedValue);
                }
                else if (rbtnAttorney.Checked)
                {
                    user.UserType = UserType.Attorney;
                    user.AttorneyId = Convert.ToInt32(Id);
                }
                else if (rbtnCourt.Checked)
                {
                    user.UserType = UserType.Court;
                    user.CourtId = Convert.ToInt32(Id);
                }
                else if (rbtnJudge.Checked)
                {
                    user.UserType = UserType.Judge;
                    user.JudgeId = Convert.ToInt32(Id);
                }

                user.Username = txtUsername.Text.Trim();

                if (user.UserPassword != UserAuthentication.Encrypt(txtPassword.Text, true))
                {
                    user.UserPassword = UserAuthentication.Encrypt(txtPassword.Text, true);
                    user.ChangePasswordRequired = true;
                }

                user.LastModifiedBy = Program.currentUserName;

                //Client Departments
                user.ClientDepartmentList = new List<int>();
                foreach (TreeNode clientNode in tvClientRule.Nodes)
                {
                    if (clientNode.Nodes.Count > 0)
                    {
                        foreach (TreeNode deptNode in clientNode.Nodes)
                        {
                            if (deptNode.Checked)
                            {
                                if (deptNode.Tag.ToString().StartsWith("Department"))
                                {
                                    string[] dept = deptNode.Tag.ToString().Split('#');
                                    if (dept.Length == 3)
                                    {
                                        user.ClientDepartmentList.Add(Convert.ToInt32(dept[1]));
                                    }
                                }
                            }
                        }
                    }
                }

                //Auth Rules
                user.AuthRuleList = new List<int>();
                foreach (TreeNode level1Node in tvAuthorizationRule.Nodes)
                {
                    if (level1Node.Nodes.Count > 0)
                    {
                        foreach (TreeNode level2Node in level1Node.Nodes)
                        {
                            if (level2Node.Nodes.Count > 0)
                            {
                                foreach (TreeNode level3Node in level2Node.Nodes)
                                {
                                    if (level3Node.Checked)
                                    {
                                        if (level3Node.Tag.ToString().StartsWith("AuthRule"))
                                        {
                                            string[] authRule = level3Node.Tag.ToString().Split('#');
                                            if (authRule.Length == 3)
                                            {
                                                user.AuthRuleList.Add(Convert.ToInt32(authRule[1]));
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (level2Node.Checked)
                                {
                                    if (level2Node.Tag.ToString().StartsWith("AuthRule"))
                                    {
                                        string[] authRule = level2Node.Tag.ToString().Split('#');
                                        if (authRule.Length == 3)
                                        {
                                            user.AuthRuleList.Add(Convert.ToInt32(authRule[1]));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                int returnVal = userBL.Save(user);

                if (returnVal > 0)
                {
                    if (this._mode == OperationMode.New)
                    {
                        user.UserId = returnVal;
                    }

                    LoadClientName();
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
                MessageBox.Show("First Name cannot be empty.");
                txtFirstName.Focus();
                return false;
            }
            if (rbtnSurScan.Checked == true || rbtnDonor.Checked == true || rbtnClient.Checked == true || rbtnVendor.Checked == true || rbtnAttorney.Checked == true || rbtnJudge.Checked == true)
            {
                if (txtLastName.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Last Name cannot be empty.");
                    txtLastName.Focus();
                    return false;
                }
            }
            if (rbtnSurScan.Checked == true || rbtnDonor.Checked == true || rbtnClient.Checked == true || rbtnVendor.Checked == true || rbtnAttorney.Checked == true)
            {
                if (txtEmail.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Email cannot be empty.");
                    txtEmail.Focus();
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
                else
                {
                    //if (!ValidateEmail())
                    //{
                    //    MessageBox.Show("Email Already Exists.");
                    //    txtEmail.Focus();
                    //    return false;
                    //}
                }
            }

            if (rbtnSurScan.Checked == false && rbtnDonor.Checked == false && rbtnClient.Checked == false && rbtnVendor.Checked == false && rbtnAttorney.Checked == false && rbtnCourt.Checked == false && rbtnJudge.Checked == false)
            {
                MessageBox.Show("User Type must be selected.");
                rbtnSurScan.Focus();
                return false;
            }

            if (rbtnSurScan.Checked)
            {
                if (!(txtUsername.Text.Trim().ToUpper() == Program.superAdmin.ToUpper() || txtUsername.Text.Trim().ToUpper() == Program.superAdmin1.ToUpper() || txtUsername.Text.Trim().ToUpper() == Program.superAdmin2.ToUpper())) //if (txtUsername.Text.Trim().ToUpper() != "ADMIN")
                {
                    if (cmbUserType.SelectedIndex == 0)
                    {
                        MessageBox.Show("Department cannot be empty.");
                        cmbUserType.Focus();
                        return false;
                    }
                    else
                    {
                        if (!DepartmentActive(cmbUserType))
                        {
                            return false;
                        }
                    }
                }
            }
            else if (rbtnDonor.Checked)
            {
                //
            }
            else if (rbtnClient.Checked)
            {
                //if (cmbUserType.SelectedIndex == 0)
                //{
                //    MessageBox.Show("Client can not be empty.");
                //    cmbUserType.Focus();
                //    return false;
                //}
            }
            else if (rbtnVendor.Checked)
            {
                if (cmbUserType.SelectedIndex == 0)
                {
                    MessageBox.Show("Vendor cannot be empty.");
                    cmbUserType.Focus();
                    return false;
                }
                else
                {
                    if (!VendorActive(cmbUserType))
                    {
                        return false;
                    }
                }
            }

            if (txtUsername.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Username cannot be empty.");
                txtUsername.Focus();
                return false;
            }
            else
            {
                if (this._mode == OperationMode.New)
                {
                    if (!ValidateUsername())
                    {
                        MessageBox.Show("Username is not available.");
                        txtUsername.Focus();
                        return false;
                    }
                }
            }

            if (txtPhone.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") != string.Empty)
            {
                if (!Program.regexPhoneNumber.IsMatch(txtPhone.Text.Trim()))
                {
                    MessageBox.Show("Invalid format of Phone number.");
                    txtPhone.Focus();
                    return false;
                }
            }

            if (txtFax.Text.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") != string.Empty)
            {
                if (!Program.regexPhoneNumber.IsMatch(txtFax.Text.Trim()))
                {
                    MessageBox.Show("Invalid format of Fax number.");
                    txtFax.Focus();
                    return false;
                }
            }

            //if (txtEmail.Text.Trim() != string.Empty)
            //{
            //    List<User> userList = userBL.GetList(txtEmail.Text.Trim());
            //    if ((userList.Count > 0 && this._mode == OperationMode.New) || (userList.Count > 1 && this._mode == OperationMode.Edit))
            //    {
            //        MessageBox.Show("The email provided is already exists in database.");
            //        txtEmail.Focus();
            //        return false;
            //    }
            //}

            return true;
        }

        private bool DepartmentActive(ComboBox cmb)
        {
            DepartmentBL departmentBL = new DepartmentBL();

            int departmentId = 0;

            if (cmb.SelectedIndex > 0)
            {
                departmentId = Convert.ToInt32(cmb.SelectedValue);
                Department department = departmentBL.Get(departmentId);
                if (department.IsActive == false)
                {
                    MessageBox.Show("Department is inactive. Select some other department.");
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

        private bool ValidateUsername()
        {
            User user = userBL.Get(txtUsername.Text.Trim());

            if (user != null)
            {
                return false;
            }

            return true;
        }

        private void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;
                if (node.Nodes.Count > 0)
                {
                    this.CheckAllChildNodes(node, nodeChecked);
                }
            }
        }

        private void CheckAllNodes(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                node.Checked = true;
                CheckChildren(node, true);
            }
        }

        private void UncheckAllNodes(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                node.Checked = false;
                CheckChildren(node, false);
            }
        }

        private void CheckChildren(TreeNode rootNode, bool isChecked)
        {
            foreach (TreeNode node in rootNode.Nodes)
            {
                CheckChildren(node, isChecked);
                node.Checked = isChecked;
            }
        }

        private bool ValidateEmail()
        {
            try
            {
                DataTable user = userBL.GetByEmail(txtEmail.Text.Trim());

                if (user.Rows.Count > 0 && this._mode == OperationMode.New)
                {
                    return false;
                }
                else if (user.Rows.Count > 1 && this._mode == OperationMode.Edit)
                {
                    return false;
                }
                else if (user.Rows.Count == 1 && this._mode == OperationMode.Edit)
                {
                    if ((int)user.Rows[0]["UserId"] != this._userId)
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

        public int UserId
        {
            get
            {
                return this._userId;
            }
            set
            {
                this._userId = value;
            }
        }

        #endregion Public Properties

        private void tvClientRule_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // The code only executes if the user caused the checked state to change.
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node.Nodes.Count > 0)
                {
                    /* Calls the CheckAllChildNodes method, passing in the current
                    Checked value of the TreeNode whose checked state changed. */
                    this.CheckAllChildNodes(e.Node, e.Node.Checked);
                }

                if (e.Node.Parent != null && e.Node.Parent.Nodes.Count > 0)
                {
                    bool flag = true;
                    foreach (TreeNode childNode in e.Node.Parent.Nodes)
                    {
                        if (!childNode.Checked)
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        e.Node.Parent.Checked = true;
                    }
                    else
                    {
                        e.Node.Parent.Checked = false;
                    }
                }
            }
        }

        private void tvAuthorizationRule_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // The code only executes if the user caused the checked state to change.
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node.Nodes.Count > 0)
                {
                    /* Calls the CheckAllChildNodes method, passing in the current
                    Checked value of the TreeNode whose checked state changed. */
                    this.CheckAllChildNodes(e.Node, e.Node.Checked);
                }

                if (e.Node.Parent != null && e.Node.Parent.Nodes.Count > 0)
                {
                    bool flag = true;
                    foreach (TreeNode childNode in e.Node.Parent.Nodes)
                    {
                        if (!childNode.Checked)
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        e.Node.Parent.Checked = true;
                    }
                    else
                    {
                        e.Node.Parent.Checked = false;
                    }
                }
            }
        }

        private void btnClientCollapseAll_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                tvClientRule.CollapseAll();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClientExpandAll_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                tvClientRule.ExpandAll();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAuthCollapseAll_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                tvAuthorizationRule.CollapseAll();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAuthExpandAll_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                tvAuthorizationRule.ExpandAll();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbUserType_TextChanged(object sender, EventArgs e)
        {
            cmbUserType.CausesValidation = false;
        }
    }
}