using SurPath.Business;
using SurPath.Entity;
using SurPath.Enum;
using System;
using System.Data;
using System.Windows.Forms;
using SurpathBackend;
using Serilog;
using System.Diagnostics;

namespace SurPath
{
    public partial class FrmMain : Form
    {
        #region Public Variables
        static ILogger _logger = Program._logger;
        public FrmDrugNames frmDrugNames;
        public FrmTestPanel frmTestPanel;
        public FrmAttorneyInfo frmAttorneyInfo;
        public FrmCourtInfo frmCourtInfo;
        public FrmJudgeInfo frmJudgeInfo;
        public FrmPartners frmPartners;
        //public FrmPart frmJudgeInfo;
        public FrmDepartmentInfo frmDepartmentInfo;
        public FrmTestingAuthorityInfo frmTestingAuthorityInfo;
        public FrmExceptions frmExceptions;
        public Frm3rdPartyInfo frm3rdPartyInfo;
        public FrmZipCodeDetails frmZipCodeDetails;

        public FrmClientInfo frmClientInfo;
        public FrmVendorInfo frmVendorInfo;
        public FrmDonorPaymentInfo frmDonorPaymentInfo;
        public FrmUserInfo frmUserInfo;

        public FrmDonorDetails frmDonorDetails;
        public FrmDonorSearch frmDonorSearch;
        public FrmDashboard frmDashboard;

        public FrmHelp frmHelp;
        public ValueOverZero valueOverZero;
        private BackendLogic surpathBackend;

        private Timer exceptionTimer;
        #endregion Public Variables

        #region Constructor

        public FrmMain()
        {
            InitializeComponent();
            _logger.Debug("FrmMain loaded");

        }

        #endregion Constructor

        #region Event Methods

        private void FrmMain_Load(object sender, EventArgs e)
        {
            try
            {
                valueOverZero = new ValueOverZero();
                btnExceptionFlag.Visible = true;

                BindField(btnExceptionFlag, "Visible", valueOverZero, "Enabled");

                if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                {
                    DoMenuProcess();
                }
                surpathBackend = new BackendLogic(Program.currentUserName);

                exceptionTimer = new Timer();
                exceptionTimer.Interval = (30 * 1000); // 30 seconds
                exceptionTimer.Tick += new EventHandler(OnTimedEvent);
                exceptionTimer.Start();

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                _logger.Debug(ex.StackTrace);
                MessageBox.Show(ex.Message, "FrmMain_Load");
            }
        }
        /// <summary>
        /// this updates the notication flags every time the form timer fires
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnTimedEvent(object source, EventArgs e)
        {
            _logger.Debug($"OnTimedEvent Exception Timer Fired");
            updateExceptionFlags();
        }
        public void updateExceptionFlags()
        {
            valueOverZero.Count = surpathBackend.TotalExceptions();
        }

        private void BtnQuickLinks_Click(object sender, EventArgs e)
        {
            try
            {
                Button btnQuickLink = sender as Button;
                Cursor.Current = Cursors.WaitCursor;
                if (btnQuickLink.Tag.ToString() == "QuickLinksHead")
                {
                    if (BtnQuickLinksHead.CausesValidation == false)
                    {
                        PnlToolBar.Width = 42;

                        BtnQuickLinkLogo.Width = 42;
                        BtnQuickLinkLogo.BackgroundImage = global::SurPath.Properties.Resources.Logo_Small4;

                        BtnQuickLinksHead.Width = 42;
                        BtnQuickLinksHead.BackgroundImage = global::SurPath.Properties.Resources._0_1_Quick_Links;

                        BtnQuickLinkAddDonor.Width = 42;
                        BtnQuickLinkAddDonor.BackgroundImage = global::SurPath.Properties.Resources._1_1_Add_Donor;

                        BtnQuickLinkAddDonor.Width = 42;
                        BtnQuickLinkAddTest.BackgroundImage = global::SurPath.Properties.Resources._1_2_Add_Test;

                        BtnQuickLinkSearch.Width = 42;
                        BtnQuickLinkSearch.BackgroundImage = global::SurPath.Properties.Resources._3_1_Search;

                        BtnQuickLinkDashboard.Width = 42;
                        BtnQuickLinkDashboard.BackgroundImage = global::SurPath.Properties.Resources._4_1_Dashboard;

                        BtnQuickLinkClient.Width = 42;
                        BtnQuickLinkClient.BackgroundImage = global::SurPath.Properties.Resources._5_1_Clients;

                        BtnQuickLinkVendor.Width = 42;
                        BtnQuickLinkVendor.BackgroundImage = global::SurPath.Properties.Resources._6_1_Vendor;

                        BtnQuickLinkUser.Width = 42;
                        BtnQuickLinkUser.BackgroundImage = global::SurPath.Properties.Resources._7_1_User;

                        BtnQuickLinkDrugNames.Width = 42;
                        BtnQuickLinkDrugNames.BackgroundImage = global::SurPath.Properties.Resources._8_1_Drug_Name;

                        BtnQuickLinkTestPanel.Width = 42;
                        BtnQuickLinkTestPanel.BackgroundImage = global::SurPath.Properties.Resources._9_1_Test_Panel;

                        BtnQuickLinkAttorneyInfo.Width = 42;
                        BtnQuickLinkAttorneyInfo.BackgroundImage = global::SurPath.Properties.Resources._10_1_Attorney;

                        BtnQuickLinkCourtInfo.Width = 42;
                        BtnQuickLinkCourtInfo.BackgroundImage = global::SurPath.Properties.Resources._11_1_Court;

                        BtnQuickLinkJudgeInfo.Width = 42;
                        BtnQuickLinkJudgeInfo.BackgroundImage = global::SurPath.Properties.Resources._12_1_Judge;

                        BtnQuickLinkDepartmentInfo.Width = 42;
                        BtnQuickLinkDepartmentInfo.BackgroundImage = global::SurPath.Properties.Resources._13_1_Department;

                        BtnQuickLinkTestingAuthority.Width = 42;
                        BtnQuickLinkTestingAuthority.BackgroundImage = global::SurPath.Properties.Resources._14_1_Testing_Authority1;

                        BtnQuickLinkExceptions.Width = 42;
                        BtnQuickLinkExceptions.BackgroundImage = global::SurPath.Properties.Resources.Exceptions_Small;

                        BtnQuickLinkPartners.Width = 42;
                        BtnQuickLinkPartners.BackgroundImage = global::SurPath.Properties.Resources.Partner_small;

                        // BtnQuickLinksHead.CausesValidation = false;
                        BtnQuickLinksHead.CausesValidation = true;
                    }
                    else
                    {
                        PnlToolBar.Width = 121;

                        BtnQuickLinkLogo.Width = 121;
                        BtnQuickLinkLogo.BackgroundImage = global::SurPath.Properties.Resources.Logo_large;

                        BtnQuickLinksHead.Width = 121;
                        BtnQuickLinksHead.BackgroundImage = global::SurPath.Properties.Resources._0_Quick_Links;

                        BtnQuickLinkAddDonor.Width = 121;
                        BtnQuickLinkAddDonor.BackgroundImage = global::SurPath.Properties.Resources._1_Add_Donor;

                        BtnQuickLinkAddTest.Width = 121;
                        BtnQuickLinkAddTest.BackgroundImage = global::SurPath.Properties.Resources._2_Add_Test;

                        BtnQuickLinkSearch.Width = 121;
                        BtnQuickLinkSearch.BackgroundImage = global::SurPath.Properties.Resources._3_Search;

                        BtnQuickLinkDashboard.Width = 121;
                        BtnQuickLinkDashboard.BackgroundImage = global::SurPath.Properties.Resources._4_Dashboard;

                        BtnQuickLinkClient.Width = 121;
                        BtnQuickLinkClient.BackgroundImage = global::SurPath.Properties.Resources._5_Clients;

                        BtnQuickLinkVendor.Width = 121;
                        BtnQuickLinkVendor.BackgroundImage = global::SurPath.Properties.Resources._6_Vendor;

                        BtnQuickLinkUser.Width = 121;
                        BtnQuickLinkUser.BackgroundImage = global::SurPath.Properties.Resources._7_User;

                        BtnQuickLinkDrugNames.Width = 121;
                        BtnQuickLinkDrugNames.BackgroundImage = global::SurPath.Properties.Resources._8_Drug_Name;

                        BtnQuickLinkTestPanel.Width = 121;
                        BtnQuickLinkTestPanel.BackgroundImage = global::SurPath.Properties.Resources._9_Test_Panel;

                        BtnQuickLinkAttorneyInfo.Width = 121;
                        BtnQuickLinkAttorneyInfo.BackgroundImage = global::SurPath.Properties.Resources._10_Attorney;

                        BtnQuickLinkCourtInfo.Width = 121;
                        BtnQuickLinkCourtInfo.BackgroundImage = global::SurPath.Properties.Resources._11_Court;

                        BtnQuickLinkJudgeInfo.Width = 121;
                        BtnQuickLinkJudgeInfo.BackgroundImage = global::SurPath.Properties.Resources._12_Judge;

                        BtnQuickLinkDepartmentInfo.Width = 121;
                        BtnQuickLinkDepartmentInfo.BackgroundImage = global::SurPath.Properties.Resources._13_Department;

                        BtnQuickLinkTestingAuthority.Width = 121;
                        BtnQuickLinkTestingAuthority.BackgroundImage = global::SurPath.Properties.Resources._14_Testing_Authority1;

                        BtnQuickLinkExceptions.Width = 121;
                        BtnQuickLinkExceptions.BackgroundImage = global::SurPath.Properties.Resources.Exceptions;

                        BtnQuickLinkPartners.Width = 121;
                        BtnQuickLinkPartners.BackgroundImage = global::SurPath.Properties.Resources.Partner_large;

                        // BtnQuickLinksHead.CausesValidation = true;
                        BtnQuickLinksHead.CausesValidation = false;
                    }
                }
                else if (btnQuickLink.Tag.ToString() == "QuickLinkAddDonor")
                {
                    AddDonor();
                }
                else if (btnQuickLink.Tag.ToString() == "QuickLinkAddTest")
                {
                    AddTest();
                }
                else if (btnQuickLink.Tag.ToString() == "QuickLinkSearch")
                {
                    if (frmDonorSearch == null)
                    {
                        frmDonorSearch = new FrmDonorSearch();
                        //this.ShowIcon = false;
                        //this.ShowInTaskbar = false;
                        //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
                        frmDonorSearch.MdiParent = this;
                    }
                    frmDonorSearch.Show();
                    frmDonorSearch.BringToFront();
                }
                else if (btnQuickLink.Tag.ToString() == "QuickLinkDashboard")
                {
                    if (frmDashboard == null)
                    {
                        frmDashboard = new FrmDashboard();
                        frmDashboard.MdiParent = this;
                    }
                    frmDashboard.Show();
                    frmDashboard.BringToFront();
                }
                else if (btnQuickLink.Tag.ToString() == "QuickLinkClient")
                {
                    if (frmClientInfo == null)
                    {
                        frmClientInfo = new FrmClientInfo();
                        frmClientInfo.MdiParent = this;
                    }
                    frmClientInfo.Show();
                    frmClientInfo.BringToFront();
                }
                else if (btnQuickLink.Tag.ToString() == "QuickLinkVendor")
                {
                    if (frmVendorInfo == null)
                    {
                        frmVendorInfo = new FrmVendorInfo();
                        frmVendorInfo.MdiParent = this;
                    }
                    frmVendorInfo.Show();
                    frmVendorInfo.BringToFront();
                }
                else if (btnQuickLink.Tag.ToString() == "QuickLinkUser")
                {
                    if (frmUserInfo == null)
                    {
                        frmUserInfo = new FrmUserInfo();
                        frmUserInfo.MdiParent = this;
                    }
                    frmUserInfo.Show();
                    frmUserInfo.BringToFront();
                }
                else if (btnQuickLink.Tag.ToString() == "QuickLinkDrugNames")
                {
                    if (frmDrugNames == null)
                    {
                        frmDrugNames = new FrmDrugNames();
                        frmDrugNames.MdiParent = this;
                    }
                    frmDrugNames.Show();
                    frmDrugNames.BringToFront();
                }
                else if (btnQuickLink.Tag.ToString() == "QuickLinkTestPanel")
                {
                    if (frmTestPanel == null)
                    {
                        frmTestPanel = new FrmTestPanel();
                        frmTestPanel.MdiParent = this;
                    }
                    frmTestPanel.Show();
                    frmTestPanel.BringToFront();
                }
                else if (btnQuickLink.Tag.ToString() == "QuickLinkAttorneyInfo")
                {
                    if (frmAttorneyInfo == null)
                    {
                        frmAttorneyInfo = new FrmAttorneyInfo();
                        frmAttorneyInfo.MdiParent = this;
                    }
                    frmAttorneyInfo.Show();
                    frmAttorneyInfo.BringToFront();
                }
                else if (btnQuickLink.Tag.ToString() == "QuickLinkCourtInfo")
                {
                    if (frmCourtInfo == null)
                    {
                        frmCourtInfo = new FrmCourtInfo();
                        frmCourtInfo.MdiParent = this;
                    }
                    frmCourtInfo.Show();
                    frmCourtInfo.BringToFront();
                }
                else if (btnQuickLink.Tag.ToString() == "QuickLinkJudgeInfo")
                {
                    if (frmJudgeInfo == null)
                    {
                        frmJudgeInfo = new FrmJudgeInfo();
                        frmJudgeInfo.MdiParent = this;
                    }
                    frmJudgeInfo.Show();
                    frmJudgeInfo.BringToFront();
                }
                else if (btnQuickLink.Tag.ToString() == "QuickLinkDepartmentInfo")
                {
                    if (frmDepartmentInfo == null)
                    {
                        frmDepartmentInfo = new FrmDepartmentInfo();
                        frmDepartmentInfo.MdiParent = this;
                    }
                    frmDepartmentInfo.Show();
                    frmDepartmentInfo.BringToFront();
                }
                else if (btnQuickLink.Tag.ToString() == "QuickLinkTestingAuthority")
                {
                    if (frmTestingAuthorityInfo == null)
                    {
                        frmTestingAuthorityInfo = new FrmTestingAuthorityInfo();
                        frmTestingAuthorityInfo.MdiParent = this;
                    }
                    frmTestingAuthorityInfo.Show();
                    frmTestingAuthorityInfo.BringToFront();
                }
                else if (btnQuickLink.Tag.ToString() == "QuickLinkExceptions")
                {
                    if (frmExceptions == null)
                    {
                        frmExceptions = new FrmExceptions(_logger);
                        frmExceptions.MdiParent = this;
                    }
                    frmExceptions.Show();
                    frmExceptions.BringToFront();
                }
                else if (btnQuickLink.Tag.ToString() == "QuickLinkPartners")
                {
                    LoadPartnersForm();
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void tsmiDrugNames_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (frmDrugNames == null)
                {
                    frmDrugNames = new FrmDrugNames();
                    frmDrugNames.MdiParent = this;
                }
                frmDrugNames.Show();
                frmDrugNames.BringToFront();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void tsmiTestPanel_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (frmTestPanel == null)
                {
                    frmTestPanel = new FrmTestPanel();
                    frmTestPanel.MdiParent = this;
                }
                frmTestPanel.Show();
                frmTestPanel.BringToFront();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void tsmiAttorneyInfo_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (frmAttorneyInfo == null)
                {
                    frmAttorneyInfo = new FrmAttorneyInfo();
                    frmAttorneyInfo.MdiParent = this;
                }
                frmAttorneyInfo.Show();
                frmAttorneyInfo.BringToFront();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void tsmiCourtInfo_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (frmCourtInfo == null)
                {
                    frmCourtInfo = new FrmCourtInfo();
                    frmCourtInfo.MdiParent = this;
                }
                frmCourtInfo.Show();
                frmCourtInfo.BringToFront();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void tsmiJudgeInfo_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (frmJudgeInfo == null)
                {
                    frmJudgeInfo = new FrmJudgeInfo();
                    frmJudgeInfo.MdiParent = this;
                }
                frmJudgeInfo.Show();
                frmJudgeInfo.BringToFront();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void tsmiUserInfo_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (frmUserInfo == null)
                {
                    frmUserInfo = new FrmUserInfo();
                    frmUserInfo.MdiParent = this;
                }
                frmUserInfo.Show();
                frmUserInfo.BringToFront();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void tsmiVendorInfo_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (frmVendorInfo == null)
                {
                    frmVendorInfo = new FrmVendorInfo();
                    frmVendorInfo.MdiParent = this;
                }
                frmVendorInfo.Show();
                frmVendorInfo.BringToFront();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void tsmiClientInfo_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (frmClientInfo == null)
                {
                    frmClientInfo = new FrmClientInfo();
                    frmClientInfo.MdiParent = this;
                }
                frmClientInfo.Show();
                frmClientInfo.BringToFront();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void tsmiDonorSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (frmDonorSearch == null)
                {
                    frmDonorSearch = new FrmDonorSearch();
                    frmDonorSearch.MdiParent = this;
                }
                frmDonorSearch.Show();
                frmDonorSearch.BringToFront();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void tsmiDashboard_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (frmDashboard == null)
                {
                    frmDashboard = new FrmDashboard();
                    frmDashboard.MdiParent = this;
                }
                frmDashboard.Show();
                frmDashboard.BringToFront();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void tsmiUserProfile_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                FrmUserProfile frmUserProfile = new FrmUserProfile();
                frmUserProfile.ShowDialog();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void tsmiCloseAllWindow_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                foreach (Form frm in this.MdiChildren)
                {
                    frm.Close();
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                Application.Exit();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void tsmiDepartmentInfo_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (frmDepartmentInfo == null)
                {
                    frmDepartmentInfo = new FrmDepartmentInfo();
                    frmDepartmentInfo.MdiParent = this;
                }
                frmDepartmentInfo.Show();
                frmDepartmentInfo.BringToFront();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void testingAuthorityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (frmTestingAuthorityInfo == null)
                {
                    frmTestingAuthorityInfo = new FrmTestingAuthorityInfo();
                    frmTestingAuthorityInfo.MdiParent = this;
                }
                frmTestingAuthorityInfo.Show();
                frmTestingAuthorityInfo.BringToFront();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void tsmiAddDonor_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                AddDonor();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void tsmiChartOfAccount_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                MessageBox.Show("Under Development.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void tsmiAbout_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                //  MessageBox.Show("Under Development.");
                if (frmHelp == null)
                {
                    frmHelp = new FrmHelp();
                    frmHelp.MdiParent = this;
                }
                frmHelp.Show();
                frmHelp.BringToFront();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void tsmiAddTest_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                AddTest();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void tsmiDonorPaymentInfo_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (frmDonorPaymentInfo == null)
                {
                    frmDonorPaymentInfo = new FrmDonorPaymentInfo();
                    frmDonorPaymentInfo.MdiParent = this;
                }
                frmDonorPaymentInfo.Show();
                frmDonorPaymentInfo.BringToFront();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        #endregion Event Methods

        #region Private Methods

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

        private void DoMenuProcess()
        {
            #region Initialize Menu visibility

            //Default menus
            tsmiFile.Visible = true;
            tsmiWindow.Visible = true;
            tsmiCloseAllWindow.Visible = true;
            tsmiHelp.Visible = true;
            tsmiAbout.Visible = true;
            tsmiUserProfile.Visible = true;
            toolStripMenuItem2.Visible = true;
            tsmiExit.Visible = true;

            //Authorization based menus
            BtnQuickLinkDashboard.Visible = false;
            tsmiDashboard.Visible = false;
            toolStripMenuItem1.Visible = false;

            tsmiDonor.Visible = false;

            BtnQuickLinkAddDonor.Visible = false;
            BtnQuickLinkAddTest.Visible = false;
            tsmiAddDonor.Visible = false;
            tsmiAddTest.Visible = false;
            toolStripMenuItem3.Visible = false;
            BtnQuickLinkSearch.Visible = false;
            tsmiDonorSearch.Visible = false;

            BtnQuickLinkClient.Visible = false;
            tsmiClient.Visible = false;
            tsmiClientInfo.Visible = false;

            BtnQuickLinkVendor.Visible = false;
            tsmiVendor.Visible = false;
            tsmiVendorInfo.Visible = false;

            BtnQuickLinkUser.Visible = false;
            tsmiUser.Visible = false;
            tsmiUserInfo.Visible = false;

            BtnQuickLinkDrugNames.Visible = false;
            tsmiDrugNames.Visible = false;

            BtnQuickLinkTestPanel.Visible = false;
            tsmiTestPanel.Visible = false;

            BtnQuickLinkAttorneyInfo.Visible = false;
            tsmiAttorneyInfo.Visible = false;

            BtnQuickLinkCourtInfo.Visible = false;
            tsmiCourtInfo.Visible = false;

            BtnQuickLinkJudgeInfo.Visible = false;
            tsmiJudgeInfo.Visible = false;

            BtnQuickLinkDepartmentInfo.Visible = false;
            tsmiDepartmentInfo.Visible = false;

            BtnQuickLinkTestingAuthority.Visible = false;
            tsmiTestingAuthority.Visible = false;

            tsmiMaster.Visible = false;

            tsmiDrugNames.Visible = false;
            tsmiTestPanel.Visible = false;
            tsmiAttorneyInfo.Visible = false;
            tsmiCourtInfo.Visible = false;
            tsmiJudgeInfo.Visible = false;
            tsmiDepartmentInfo.Visible = false;
            tsmiTestingAuthority.Visible = false;

            tsmiAccounts.Visible = false;
            tsmiChartOfAccount.Visible = false;

            #endregion Initialize Menu visibility

            #region Menu visibility based on the rights

            //SS_DASHBOARD
            DataRow[] ssDashboard = Program.dtUserAuthRules.Select("AuthRuleCategoryInternalName = '" + AuthorizationCategories.SS_DASHBOARD.ToDescriptionString() + "'");

            if (ssDashboard.Length > 0)
            {
                BtnQuickLinkDashboard.Visible = true;
                tsmiDashboard.Visible = true;
                toolStripMenuItem1.Visible = true;
            }

            //DONOR_TAB
            DataRow[] donor = Program.dtUserAuthRules.Select("AuthRuleCategoryInternalName = '" + AuthorizationCategories.DONOR_TAB.ToDescriptionString() + "'");

            if (donor.Length > 0)
            {
                bool seperatorFlag1 = false;
                bool seperatorFlag2 = false;

                DataRow[] addDonor = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DONOR_ADD.ToDescriptionString() + "'");

                if (addDonor.Length > 0)
                {
                    BtnQuickLinkAddDonor.Visible = true;
                    BtnQuickLinkAddTest.Visible = true;
                    tsmiAddDonor.Visible = true;
                    tsmiAddTest.Visible = true;
                    seperatorFlag1 = true;
                }

                //   DataRow[] addTest = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DONOR_TEST.ToDescriptionString() + "'");

                //if (addTest.Length > 0)
                //{
                //    BtnQuickLinkAddTest.Visible = true;
                //    tsmiAddDonor.Visible = false;
                //    tsmiAddTest.Visible = true;
                //    seperatorFlag2 = true;
                //}

                DataRow[] searchDonor = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DONOR_SEARCH_VIEW.ToDescriptionString() + "'");

                if (searchDonor.Length > 0)
                {
                    BtnQuickLinkSearch.Visible = true;
                    tsmiDonorSearch.Visible = true;
                    seperatorFlag2 = true;
                }

                if (seperatorFlag1 && seperatorFlag2)
                {
                    toolStripMenuItem3.Visible = true;
                }

                if (seperatorFlag1 || seperatorFlag2)
                {
                    tsmiDonor.Visible = true;
                }
            }
            else
            {
                BtnQuickLinkAddDonor.Visible = false;
                tsmiAddDonor.Visible = false;
                tsmiAddTest.Visible = false;
                toolStripMenuItem3.Visible = false;
                BtnQuickLinkSearch.Visible = false;
                tsmiDonorSearch.Visible = false;
                tsmiDonor.Visible = false;
            }

            //Client
            DataRow[] client = Program.dtUserAuthRules.Select("AuthRuleCategoryInternalName = '" + AuthorizationCategories.CLIENT_SETUP.ToDescriptionString() + "'");

            if (client.Length > 0)
            {
                tsmiClientInfo.Visible = true;
                tsmiClient.Visible = true;
                BtnQuickLinkClient.Visible = true;
            }

            //Vendor
            DataRow[] vendor = Program.dtUserAuthRules.Select("AuthRuleCategoryInternalName = '" + AuthorizationCategories.VENDOR_SETUP.ToDescriptionString() + "'");

            if (vendor.Length > 0)
            {
                tsmiVendorInfo.Visible = true;
                tsmiVendor.Visible = true;
                BtnQuickLinkVendor.Visible = true;
            }

            //User
            DataRow[] user = Program.dtUserAuthRules.Select("AuthRuleCategoryInternalName = '" + AuthorizationCategories.USER_SETUP.ToDescriptionString() + "'");

            if (user.Length > 0)
            {
                tsmiUserInfo.Visible = true;
                tsmiUser.Visible = true;
                BtnQuickLinkUser.Visible = true;
            }

            //General Setup
            bool setupMenu = false;

            //DRUG_NAMES_SETUP
            DataRow[] drugNames = Program.dtUserAuthRules.Select("AuthRuleCategoryInternalName = '" + AuthorizationCategories.DRUG_NAMES_SETUP.ToDescriptionString() + "'");

            if (drugNames.Length > 0)
            {
                tsmiDrugNames.Visible = true;
                setupMenu = true;
                BtnQuickLinkDrugNames.Visible = true;
            }

            //TEST_PANEL_SETUP
            DataRow[] testPanel = Program.dtUserAuthRules.Select("AuthRuleCategoryInternalName = '" + AuthorizationCategories.TEST_PANEL_SETUP.ToDescriptionString() + "'");

            if (testPanel.Length > 0)
            {
                tsmiTestPanel.Visible = true;
                setupMenu = true;
                BtnQuickLinkTestPanel.Visible = true;
            }

            //ATTORNEY_INFO_SETUP
            DataRow[] attorneyInfo = Program.dtUserAuthRules.Select("AuthRuleCategoryInternalName = '" + AuthorizationCategories.ATTORNEY_INFO_SETUP.ToDescriptionString() + "'");

            if (attorneyInfo.Length > 0)
            {
                tsmiAttorneyInfo.Visible = true;
                setupMenu = true;
                BtnQuickLinkAttorneyInfo.Visible = true;
            }

            //COURT_INFO_SETUP
            DataRow[] courtInfo = Program.dtUserAuthRules.Select("AuthRuleCategoryInternalName = '" + AuthorizationCategories.COURT_INFO_SETUP.ToDescriptionString() + "'");

            if (courtInfo.Length > 0)
            {
                tsmiCourtInfo.Visible = true;
                setupMenu = true;
                BtnQuickLinkCourtInfo.Visible = true;
            }

            //JUDGE_INFO_SETUP
            DataRow[] judgeInfo = Program.dtUserAuthRules.Select("AuthRuleCategoryInternalName = '" + AuthorizationCategories.JUDGE_INFO_SETUP.ToDescriptionString() + "'");

            if (judgeInfo.Length > 0)
            {
                tsmiJudgeInfo.Visible = true;
                setupMenu = true;
                BtnQuickLinkJudgeInfo.Visible = true;
            }

            //DEPARTMENT_INFO_SETUP
            DataRow[] departmentInfo = Program.dtUserAuthRules.Select("AuthRuleCategoryInternalName = '" + AuthorizationCategories.DEPARTMENT_INFO_SETUP.ToDescriptionString() + "'");

            if (departmentInfo.Length > 0)
            {
                tsmiDepartmentInfo.Visible = true;
                setupMenu = true;
                BtnQuickLinkDepartmentInfo.Visible = true;
            }

            //DEPARTMENT_INFO_SETUP
            DataRow[] testingAuthority = Program.dtUserAuthRules.Select("AuthRuleCategoryInternalName = '" + AuthorizationCategories.TESTING_AUTHORITY_SETUP.ToDescriptionString() + "'");

            if (testingAuthority.Length > 0)
            {
                tsmiTestingAuthority.Visible = true;
                setupMenu = true;
                BtnQuickLinkTestingAuthority.Visible = true;
            }

            if (setupMenu)
            {
                tsmiMaster.Visible = true;
            }

            //CAN VIEW ONLY

            DataRow[] canViewOnly = Program.dtUserAuthRules.Select("AuthRuleCategoryInternalName = '" + AuthorizationCategories.CAN_VIEW_ONLY.ToDescriptionString() + "'");

            if (canViewOnly.Length > 0)
            {
                //DRUG_NAMES_VIEW
                DataRow[] drugNamesView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DRUG_NAMES_VIEW.ToDescriptionString() + "'");

                if (drugNamesView.Length > 0)
                {
                    tsmiDrugNames.Visible = true;
                    BtnQuickLinkDrugNames.Visible = true;
                }
                //TEST_PANEL_VIEW
                DataRow[] testPanelView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.TEST_PANEL_VIEW.ToDescriptionString() + "'");

                if (testPanelView.Length > 0)
                {
                    tsmiTestPanel.Visible = true;
                    BtnQuickLinkTestPanel.Visible = true;
                }
                //ATTORNEY_VIEW
                DataRow[] attorneyView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.ATTORNEY_VIEW.ToDescriptionString() + "'");

                if (attorneyView.Length > 0)
                {
                    tsmiAttorneyInfo.Visible = true;
                    BtnQuickLinkAttorneyInfo.Visible = true;
                }
                //COURT_VIEW
                DataRow[] courtView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.COURT_VIEW.ToDescriptionString() + "'");

                if (courtView.Length > 0)
                {
                    tsmiCourtInfo.Visible = true;
                    BtnQuickLinkCourtInfo.Visible = true;
                }
                //JUDGE_VIEW
                DataRow[] judgeView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.JUDGE_VIEW.ToDescriptionString() + "'");

                if (judgeView.Length > 0)
                {
                    tsmiJudgeInfo.Visible = true;
                    BtnQuickLinkJudgeInfo.Visible = true;
                }
                //DEPARTMENT_VIEW
                DataRow[] departmentView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DEPARTMENT_VIEW.ToDescriptionString() + "'");

                if (departmentView.Length > 0)
                {
                    tsmiDepartmentInfo.Visible = true;
                    BtnQuickLinkDepartmentInfo.Visible = true;
                }
                //TESTING_AUTHORITY_VIEW
                DataRow[] testingAuthorityView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.TESTING_AUTHORITY_VIEW.ToDescriptionString() + "'");

                if (testingAuthorityView.Length > 0)
                {
                    tsmiTestingAuthority.Visible = true;
                    BtnQuickLinkTestingAuthority.Visible = true;
                }
                //CLIENT_VIEW
                DataRow[] clientView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.CLIENT_VIEW.ToDescriptionString() + "'");

                if (clientView.Length > 0)
                {
                    tsmiClientInfo.Visible = true;
                    tsmiClient.Visible = true;
                    BtnQuickLinkClient.Visible = true;
                }
                //VENDOR_VIEW
                DataRow[] vendorView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.VENDOR_VIEW.ToDescriptionString() + "'");

                if (vendorView.Length > 0)
                {
                    tsmiVendorInfo.Visible = true;
                    tsmiVendor.Visible = true;
                    BtnQuickLinkVendor.Visible = true;
                }
                //USER_VIEW
                DataRow[] userView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.USER_VIEW.ToDescriptionString() + "'");
                if (userView.Length > 0)
                {
                    tsmiUserInfo.Visible = true;
                    tsmiUser.Visible = true;
                    BtnQuickLinkUser.Visible = true;
                }
            }

            //Accounts

            #endregion Menu visibility based on the rights

            #region Repositioning the quick link buttons

            int topPosition = 90;

            if (BtnQuickLinkDashboard.Visible)
            {
                BtnQuickLinkDashboard.Top = topPosition;
                topPosition += 30;
            }

            if (BtnQuickLinkSearch.Visible)
            {
                BtnQuickLinkSearch.Top = topPosition;
                topPosition += 30;
            }

            if (BtnQuickLinkAddDonor.Visible)
            {
                BtnQuickLinkAddDonor.Top = topPosition;
                topPosition += 30;
            }

            if (BtnQuickLinkAddTest.Visible)
            {
                BtnQuickLinkAddTest.Top = topPosition;
                topPosition += 30;
            }

            if (BtnQuickLinkTestPanel.Visible)
            {
                BtnQuickLinkTestPanel.Top = topPosition;
                topPosition += 30;
            }

            if (BtnQuickLinkDrugNames.Visible)
            {
                BtnQuickLinkDrugNames.Top = topPosition;
                topPosition += 30;
            }

            if (BtnQuickLinkClient.Visible)
            {
                BtnQuickLinkClient.Top = topPosition;
                topPosition += 30;
            }

            if (BtnQuickLinkDepartmentInfo.Visible)
            {
                BtnQuickLinkDepartmentInfo.Top = topPosition;
                topPosition += 30;
            }

            if (BtnQuickLinkVendor.Visible)
            {
                BtnQuickLinkVendor.Top = topPosition;
                topPosition += 30;
            }

            if (BtnQuickLinkUser.Visible)
            {
                BtnQuickLinkUser.Top = topPosition;
                topPosition += 30;
            }

            if (BtnQuickLinkAttorneyInfo.Visible)
            {
                BtnQuickLinkAttorneyInfo.Top = topPosition;
                topPosition += 30;
            }
            if (BtnQuickLinkCourtInfo.Visible)
            {
                BtnQuickLinkCourtInfo.Top = topPosition;
                topPosition += 30;
            }

            if (BtnQuickLinkJudgeInfo.Visible)
            {
                BtnQuickLinkJudgeInfo.Top = topPosition;
                topPosition += 30;
            }

            if (BtnQuickLinkTestingAuthority.Visible)
            {
                BtnQuickLinkTestingAuthority.Top = topPosition;
                topPosition += 30;
            }

            #endregion Repositioning the quick link buttons
        }

        private void AddDonor()
        {
            try
            {
                FrmDonorRegistraion frmDonorRegistraion = new FrmDonorRegistraion(OperationMode.New);

                frmDonorRegistraion.lblPageHeader.Text = "Donor Registration";
                frmDonorRegistraion.Size = new System.Drawing.Size(851, 412);
                frmDonorRegistraion.gboxDonorDetails.Location = new System.Drawing.Point(12, 38);
                frmDonorRegistraion.gboxClientDetails.Location = new System.Drawing.Point(12, 257);
                frmDonorRegistraion.gboxDonorSearch.Visible = false;
                frmDonorRegistraion.dgvSearch.Visible = false;
                frmDonorRegistraion.ssndup.Visible = false;
                frmDonorRegistraion.btnOK.Location = new System.Drawing.Point(334, 342);
                frmDonorRegistraion.btnClose.Location = new System.Drawing.Point(425, 342);

                if (frmDonorRegistraion.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    LoadDonorInfoTab(frmDonorRegistraion);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddTest()
        {
            try
            {
                FrmDonorRegistraion frmDonorRegistraion = new FrmDonorRegistraion(OperationMode.Edit);

                frmDonorRegistraion.lblPageHeader.Text = "Test Registration";
                frmDonorRegistraion.Size = new System.Drawing.Size(849, 651);
                frmDonorRegistraion.gboxDonorDetails.Location = new System.Drawing.Point(12, 280);
                frmDonorRegistraion.gboxClientDetails.Location = new System.Drawing.Point(12, 501);
                frmDonorRegistraion.gboxDonorSearch.Visible = true;
                frmDonorRegistraion.dgvSearch.Visible = true;
                frmDonorRegistraion.txtSSN.Visible = false;
                frmDonorRegistraion.btnOK.Location = new System.Drawing.Point(334, 580);
                frmDonorRegistraion.btnClose.Location = new System.Drawing.Point(425, 580);
                frmDonorRegistraion.AcceptButton = frmDonorRegistraion.btnSearch;

                if (frmDonorRegistraion.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    LoadDonorInfoTab(frmDonorRegistraion);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadDonorInfoTab(FrmDonorRegistraion frmDonorRegistraion)
        {
            if (Program.frmMain.frmDonorDetails == null)
            {
                Program.frmMain.frmDonorDetails = new FrmDonorDetails();
                Program.frmMain.frmDonorDetails.MdiParent = Program.frmMain;
            }

            string tabTag = frmDonorRegistraion.DonorId.ToString() + "#" + frmDonorRegistraion.DonorTestInfoId.ToString();
            DonorBL donorBL = new DonorBL();
            Donor donor = donorBL.Get(frmDonorRegistraion.DonorId, "Desktop");

            if (donor != null)
            {
                string tabText = donor.DonorFirstName + " " + donor.DonorLastName;

                Program.frmMain.frmDonorDetails.Show();

                int tabIndex = Program.frmMain.frmDonorDetails.AddDonorNamesTab(tabText, tabTag);
                Program.frmMain.frmDonorDetails.LoadTabDetails(tabIndex);

                Program.frmMain.frmDonorDetails.BringToFront();
            }
        }

        #endregion Private Methods

        private void tsmiZipCodeDetails_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (frmZipCodeDetails == null)
                {
                    frmZipCodeDetails = new FrmZipCodeDetails();
                    frmZipCodeDetails.MdiParent = this;
                }
                frmZipCodeDetails.Show();
                frmZipCodeDetails.BringToFront();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void tsmiMaster_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// This iterates the menu items and finds the max right of all visibile
        /// menu items, then moves btnExceptionFlag to be just to the right of them.
        /// </summary>
        private void alignbtnExceptionFlag()
        {
            btnExceptionFlag.Left = this.ClientSize.Width - btnExceptionFlag.Width;
            int intNotificationLeft = 0;

            foreach (ToolStripMenuItem menuItem in menuStrip1.Items)
            {
                if (menuItem.Visible)
                {
                    int rightEdge = menuItem.Bounds.Location.X + menuItem.Width;
                    //int rightEdge = menuItem.DropDownLocation.X + menuItem.Width;
                    if (rightEdge > intNotificationLeft) intNotificationLeft = rightEdge;
                }
            }
            btnExceptionFlag.Left = intNotificationLeft + 10;
        }

        private void btnExceptionFlag_Paint(object sender, PaintEventArgs e)
        {
            alignbtnExceptionFlag();
        }

        private void FrmMain_ResizeEnd(object sender, EventArgs e)
        {
            alignbtnExceptionFlag();
        }

        private void btnExceptionFlag_VisibleChanged(object sender, EventArgs e)
        {
            // We'll use this event to set the button on the left.

            if (btnExceptionFlag.Visible == true)
            {
                //BtnQuickLinkExceptions.Width = 42;
                //BtnQuickLinkExceptions.BackgroundImage = global::SurPath.Properties.Resources.Exceptions_Small;

                //BtnQuickLinkExceptions.Width = 121;
                //BtnQuickLinkExceptions.BackgroundImage = global::SurPath.Properties.Resources.Exceptions;

                if (BtnQuickLinkExceptions.Width > 100)
                {
                    BtnQuickLinkExceptions.BackgroundImage = global::SurPath.Properties.Resources.Exceptions;
                }
                else
                {
                    BtnQuickLinkExceptions.BackgroundImage = global::SurPath.Properties.Resources.Exceptions_Small;
                }
            }
            else
            {
                if (BtnQuickLinkExceptions.Width > 100)
                {
                    BtnQuickLinkExceptions.BackgroundImage = global::SurPath.Properties.Resources.Exceptions_Large_Green;
                }
                else
                {
                    BtnQuickLinkExceptions.BackgroundImage = global::SurPath.Properties.Resources.Exceptions_Small_Green;
                }
            }
        }

        private void partnersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadPartnersForm();
        }

        private void LoadPartnersForm()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (frmPartners == null)
                {
                    frmPartners = new FrmPartners(_logger);
                    frmPartners.MdiParent = this;
                }
                frmPartners.Show();
                frmPartners.BringToFront();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            //this.exceptionTimer.Stop();
            //this.exceptionTimer.Dispose();
            _logger.Debug($"FrmMain Closed");
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            MessageBoxManager.Unregister(); // for custom message boxes
            this.exceptionTimer.Stop();
            this.exceptionTimer.Dispose();
            _logger.Debug($"FrmMain Closing");

        }

        private void userQuickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (frmUserInfo == null)
                {
                    frmUserInfo = new FrmUserInfo();
                    frmUserInfo.SkipSearchOnLoad = true;
                    frmUserInfo.MdiParent = this;
                }
                frmUserInfo.Show();
                frmUserInfo.BringToFront();
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