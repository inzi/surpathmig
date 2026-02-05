using SurPath.Business.Master;
using SurPath.Entity.Master;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmTestingAuthorityInfo : Form
    {
        private bool haveEditRights = false;

        #region Constructor

        public FrmTestingAuthorityInfo()
        {
            InitializeComponent();
        }

        #endregion Constructor

        #region Event Methods

        private void FrmTestingAuthorityInfo_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeControls();
                LoadTestingAuthorityInfo(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while loading the page.");
            }
        }

        private void FrmTestingAuthorityInfo_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.frmMain.frmTestingAuthorityInfo = null;
        }

        private void tsbNew_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                FrmTestingAuthorityDetails frmTestingAuthorityDetails = new FrmTestingAuthorityDetails(Enum.OperationMode.New, 0);
                if (frmTestingAuthorityDetails.ShowDialog() == DialogResult.OK)
                {
                    LoadTestingAuthorityInfo(0);
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void tsbEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (haveEditRights)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    if (dgvTestingAuthorityInfo.SelectedRows.Count > 0)
                    {
                        int selectedIndex = dgvTestingAuthorityInfo.SelectedRows[0].Index;
                        int testingAuthorityId = (int)dgvTestingAuthorityInfo.SelectedRows[0].Cells["TestingAuthorityId"].Value;

                        FrmTestingAuthorityDetails frmTestingAuthorityDetails = new FrmTestingAuthorityDetails(Enum.OperationMode.Edit, testingAuthorityId);
                        if (frmTestingAuthorityDetails.ShowDialog() == DialogResult.OK)
                        {
                            LoadTestingAuthorityInfo(selectedIndex);
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

        private void tsbArchive_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (dgvTestingAuthorityInfo.Rows.Count > 0)
                {
                    if (MessageBox.Show("Are you sure? Do you want to archive the selected record?", "SurPath Drug Testing", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        int testingAuthorityId = (int)dgvTestingAuthorityInfo.SelectedRows[0].Cells["TestingAuthorityId"].Value;
                        TestingAuthorityBL testingAuthorityBL = new TestingAuthorityBL();
                        int returnvalue = testingAuthorityBL.Delete(testingAuthorityId, Program.currentUserName);
                        if (returnvalue == 0)
                        {
                            MessageBox.Show("You cannot delete this record,since this is used in another process.");
                            return;
                        }
                        else
                        {
                            LoadTestingAuthorityInfo(0);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a Record.");
                    LoadTestingAuthorityInfo(0);
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void txtSearchKeyword_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.Enter)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    // LoadTestingAuthorityInfo(0);
                    TestingAuthorityBL testingAuthorityBL = new TestingAuthorityBL();
                    List<TestingAuthority> testingAuthorityList = null;
                    Dictionary<string, string> searchParam = new Dictionary<string, string>();
                    searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                    searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());
                    testingAuthorityList = testingAuthorityBL.GetList(searchParam);

                    dgvTestingAuthorityInfo.DataSource = testingAuthorityList;

                    if (dgvTestingAuthorityInfo.Rows.Count > 0)
                    {
                        //if (selectedIndex > dgvTestingAuthorityInfo.Rows.Count - 1)
                        //{
                        //    selectedIndex = dgvTestingAuthorityInfo.Rows.Count - 1;
                        //}
                        dgvTestingAuthorityInfo.Rows[0].Selected = true;
                        dgvTestingAuthorityInfo.Focus();
                    }
                    else
                    {
                        MessageBox.Show("No Records Found");
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                // LoadTestingAuthorityInfo(0);
                TestingAuthorityBL testingAuthorityBL = new TestingAuthorityBL();
                List<TestingAuthority> testingAuthorityList = null;
                Dictionary<string, string> searchParam = new Dictionary<string, string>();
                searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());
                testingAuthorityList = testingAuthorityBL.GetList(searchParam);

                dgvTestingAuthorityInfo.DataSource = testingAuthorityList;

                if (dgvTestingAuthorityInfo.Rows.Count > 0)
                {
                    //if (selectedIndex > dgvTestingAuthorityInfo.Rows.Count - 1)
                    //{
                    //    selectedIndex = dgvTestingAuthorityInfo.Rows.Count - 1;
                    //}
                    dgvTestingAuthorityInfo.Rows[0].Selected = true;
                    dgvTestingAuthorityInfo.Focus();
                }
                else
                {
                    MessageBox.Show("No Records Found");
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvTestingAuthorityInfo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (e.KeyData == Keys.Enter)
                {
                    e.SuppressKeyPress = false;
                    if (dgvTestingAuthorityInfo.SelectedRows.Count > 0)
                    {
                        int selectedIndex = dgvTestingAuthorityInfo.SelectedRows[0].Index;
                        int testingAuthorityId = (int)dgvTestingAuthorityInfo.SelectedRows[0].Cells["TestingAuthorityId"].Value;

                        if (haveEditRights)
                        {
                            FrmTestingAuthorityDetails frmTestingAuthorityDetails = new FrmTestingAuthorityDetails(Enum.OperationMode.Edit, testingAuthorityId);
                            if (frmTestingAuthorityDetails.ShowDialog() == DialogResult.OK)
                            {
                                LoadTestingAuthorityInfo(selectedIndex);
                            }
                        }

                        if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                        {
                            //TESTING_AUTHORITY_VIEW
                            DataRow[] testingAuthorityView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.TESTING_AUTHORITY_VIEW.ToDescriptionString() + "'");

                            if (testingAuthorityView.Length > 0)
                            {
                                FrmTestingAuthorityDetails frmTestingAuthorityDetails = new FrmTestingAuthorityDetails(Enum.OperationMode.Edit, testingAuthorityId);
                                if (frmTestingAuthorityDetails.ShowDialog() == DialogResult.OK)
                                {
                                    LoadTestingAuthorityInfo(selectedIndex);
                                }
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

        private void dgvTestingAuthorityInfo_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (e.RowIndex >= 0)
                {
                    int selectedIndex = e.RowIndex;
                    int testingAuthorityId = (int)dgvTestingAuthorityInfo.Rows[e.RowIndex].Cells["TestingAuthorityId"].Value;

                    if (haveEditRights)
                    {
                        FrmTestingAuthorityDetails frmTestingAuthorityDetails = new FrmTestingAuthorityDetails(Enum.OperationMode.Edit, testingAuthorityId);
                        if (frmTestingAuthorityDetails.ShowDialog() == DialogResult.OK)
                        {
                            LoadTestingAuthorityInfo(selectedIndex);
                        }
                    }

                    if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                    {
                        //TESTING_AUTHORITY_VIEW
                        DataRow[] testingAuthorityView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.TESTING_AUTHORITY_VIEW.ToDescriptionString() + "'");

                        if (testingAuthorityView.Length > 0)
                        {
                            FrmTestingAuthorityDetails frmTestingAuthorityDetails = new FrmTestingAuthorityDetails(Enum.OperationMode.Edit, testingAuthorityId);
                            if (frmTestingAuthorityDetails.ShowDialog() == DialogResult.OK)
                            {
                                LoadTestingAuthorityInfo(selectedIndex);
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

        private void chkIncludeInactive_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                LoadTestingAuthorityInfo(0);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                txtSearchKeyword.Text = string.Empty;
                LoadTestingAuthorityInfo(0);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvTestingAuthorityInfo_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvTestingAuthorityInfo.Rows)
            {
                bool testAuthorityStatus = Convert.ToBoolean(row.Cells["IsActive"].Value);
                if (testAuthorityStatus.ToString() == "True")
                {
                    row.Cells["Status"].Value = "Active";
                }
                else
                {
                    row.Cells["Status"].Value = "Inactive";
                }
            }
        }

        #endregion Event Methods

        #region Private Methods

        private void InitializeControls()
        {
            dgvTestingAuthorityInfo.AutoGenerateColumns = false;

            if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
            {
                //TESTING_AUTHORITY_ADD
                DataRow[] testingAuthorityInfoAdd = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.TESTING_AUTHORITY_ADD.ToDescriptionString() + "'");

                if (testingAuthorityInfoAdd.Length > 0)
                {
                    tsbNew.Visible = true;
                }
                else
                {
                    tsbNew.Visible = false;
                }

                //TESTING_AUTHORITY_EDIT
                DataRow[] testingAuthorityInfoEdit = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.TESTING_AUTHORITY_EDIT.ToDescriptionString() + "'");

                if (testingAuthorityInfoEdit.Length > 0)
                {
                    tsbEdit.Visible = true;
                    haveEditRights = true;
                }
                else
                {
                    tsbEdit.Visible = false;
                }

                //TESTING_AUTHORITY_ARCHIVE
                DataRow[] testingAuthorityInfoArchive = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.TESTING_AUTHORITY_ARCHIVE.ToDescriptionString() + "'");

                if (testingAuthorityInfoArchive.Length > 0)
                {
                    tsbArchive.Visible = true;
                }
                else
                {
                    tsbArchive.Visible = false;
                }
            }
            else
            {
                haveEditRights = true;
            }
        }

        private void LoadTestingAuthorityInfo(int selectedIndex)
        {
            try
            {
                TestingAuthorityBL testingAuthorityBL = new TestingAuthorityBL();
                List<TestingAuthority> testingAuthorityList = null;
                Dictionary<string, string> searchParam = new Dictionary<string, string>();
                searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());
                testingAuthorityList = testingAuthorityBL.GetList(searchParam);

                dgvTestingAuthorityInfo.DataSource = testingAuthorityList;

                if (dgvTestingAuthorityInfo.Rows.Count > 0)
                {
                    if (selectedIndex > dgvTestingAuthorityInfo.Rows.Count - 1)
                    {
                        selectedIndex = dgvTestingAuthorityInfo.Rows.Count - 1;
                    }
                    dgvTestingAuthorityInfo.Rows[selectedIndex].Selected = true;
                    dgvTestingAuthorityInfo.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion Private Methods

        private void dgvTestingAuthorityInfo_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            bool active = false;
            if (chkIncludeInactive.Checked == true)
            {
                active = true;
            }

            TestingAuthorityBL testingAuthorityBL = new TestingAuthorityBL();
            List<TestingAuthority> testingAuthorityList = null;

            DataGridView dgv = sender as DataGridView;
            DataGridViewColumn col = dgv.Columns[e.ColumnIndex];
            if (col.Name == "TestingAuthorityName")
            {
                DataGridViewColumn TestingAuthorityName = dgv.Columns["TestingAuthorityName"];
                string testingAuthorityName = string.Empty;
                if (TestingAuthorityName.HeaderCell.SortGlyphDirection == SortOrder.None || TestingAuthorityName.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    testingAuthorityList = testingAuthorityBL.Sorting("testingAuthorityName", active, "1");
                    dgvTestingAuthorityInfo.DataSource = testingAuthorityList;

                    TestingAuthorityName.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    testingAuthorityList = testingAuthorityBL.Sorting("testingAuthorityName", active);
                    dgvTestingAuthorityInfo.DataSource = testingAuthorityList;
                    TestingAuthorityName.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "Status")
            {
                DataGridViewColumn IsActive = dgv.Columns["IsActive"];
                string isActive = string.Empty;
                if (IsActive.HeaderCell.SortGlyphDirection == SortOrder.None || IsActive.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    testingAuthorityList = testingAuthorityBL.Sorting("isActive", active, "1");
                    dgvTestingAuthorityInfo.DataSource = testingAuthorityList;

                    IsActive.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    testingAuthorityList = testingAuthorityBL.Sorting("isActive", active);
                    dgvTestingAuthorityInfo.DataSource = testingAuthorityList;
                    IsActive.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
        }
    }
}