using SurPath.Business.Master;
using SurPath.Entity.Master;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmTestPanel : Form
    {
        private bool haveEditRights = false;

        # region Constructor

        public FrmTestPanel()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Methods

        private void FrmTestPanel_Load(object sender, System.EventArgs e)
        {
            try
            {
                InitializeControls();
                LoadTestPanel(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmTestPanel_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.frmMain.frmTestPanel = null;
        }

        private void tsbNew_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                FrmTestPanelDetails frmTestPanelDetails = new FrmTestPanelDetails(OperationMode.New, 0);
                if (frmTestPanelDetails.ShowDialog() == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    LoadTestPanel(0);
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void tsbEdit_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (haveEditRights)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    if (dgvTestPanel.SelectedRows.Count > 0)
                    {
                        int selectedIndex = dgvTestPanel.SelectedRows[0].Index;
                        int testPanelId = (int)dgvTestPanel.SelectedRows[0].Cells["TestPanelId"].Value;

                        FrmTestPanelDetails frmTestPanelDetails = new FrmTestPanelDetails(OperationMode.Edit, testPanelId);
                        if (frmTestPanelDetails.ShowDialog() == DialogResult.OK)
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            LoadTestPanel(selectedIndex);
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

        private void tsbArchive_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (dgvTestPanel.SelectedRows.Count > 0)
                {
                    if (MessageBox.Show("Are you sure? Do you want to archive the selected record?", "SurPath TestPanel", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        int testPanelId = (int)dgvTestPanel.SelectedRows[0].Cells["TestPanelId"].Value;

                        TestPanelBL testPanelBL = new TestPanelBL();
                        int returnvalue = testPanelBL.Delete(testPanelId, Program.currentUserName);
                        if (returnvalue == 0)
                        {
                            MessageBox.Show("You cannot delete this record,since this is used in another process.");
                            return;
                        }
                        else
                        {
                            LoadTestPanel(0);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a Record.");
                    LoadTestPanel(0);
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                //   LoadTestPanel(0);
                TestPanelBL testPanelBL = new TestPanelBL();
                List<TestPanel> testPanelList = null;

                Dictionary<string, string> searchParam = new Dictionary<string, string>();
                searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());
                testPanelList = testPanelBL.GetList(searchParam);

                dgvTestPanel.DataSource = testPanelList;

                if (dgvTestPanel.Rows.Count > 0)
                {
                    //if (selectedIndex > dgvTestPanel.Rows.Count - 1)
                    //{
                    //    selectedIndex = dgvTestPanel.Rows.Count - 1;
                    //}
                    dgvTestPanel.Rows[0].Selected = true;
                    dgvTestPanel.Focus();
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

        private void txtSearchKeyword_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.Enter)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    // LoadTestPanel(0);
                    TestPanelBL testPanelBL = new TestPanelBL();
                    List<TestPanel> testPanelList = null;

                    Dictionary<string, string> searchParam = new Dictionary<string, string>();
                    searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                    searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());
                    testPanelList = testPanelBL.GetList(searchParam);

                    dgvTestPanel.DataSource = testPanelList;

                    if (dgvTestPanel.Rows.Count > 0)
                    {
                        //if (selectedIndex > dgvTestPanel.Rows.Count - 1)
                        //{
                        //    selectedIndex = dgvTestPanel.Rows.Count - 1;
                        //}
                        dgvTestPanel.Rows[0].Selected = true;
                        dgvTestPanel.Focus();
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

        private void dgvTestPanel_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.Enter)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    e.SuppressKeyPress = false;

                    if (dgvTestPanel.SelectedRows.Count > 0)
                    {
                        int selectedIndex = dgvTestPanel.SelectedRows[0].Index;
                        int testPanelId = (int)dgvTestPanel.SelectedRows[0].Cells["TestPanelId"].Value;

                        if (haveEditRights)
                        {
                            FrmTestPanelDetails frmTestPanelDetails = new FrmTestPanelDetails(OperationMode.Edit, testPanelId);
                            if (frmTestPanelDetails.ShowDialog() == DialogResult.OK)
                            {
                                LoadTestPanel(selectedIndex);
                            }
                        }

                        if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                        {
                            //TEST_PANEL_VIEW
                            DataRow[] testPanelView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.TEST_PANEL_VIEW.ToDescriptionString() + "'");

                            if (testPanelView.Length > 0)
                            {
                                FrmTestPanelDetails frmTestPanelDetails = new FrmTestPanelDetails(OperationMode.Edit, testPanelId);
                                if (frmTestPanelDetails.ShowDialog() == DialogResult.OK)
                                {
                                    LoadTestPanel(selectedIndex);
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

        private void dgvTestPanel_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (e.RowIndex >= 0)
                {
                    int selectedIndex = e.RowIndex;
                    int testPanelId = (int)dgvTestPanel.Rows[e.RowIndex].Cells["TestPanelId"].Value;

                    if (haveEditRights)
                    {
                        FrmTestPanelDetails frmTestPanelDetails = new FrmTestPanelDetails(OperationMode.Edit, testPanelId);
                        if (frmTestPanelDetails.ShowDialog() == DialogResult.OK)
                        {
                            LoadTestPanel(selectedIndex);
                        }
                    }

                    if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                    {
                        //TEST_PANEL_VIEW
                        DataRow[] testPanelView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.TEST_PANEL_VIEW.ToDescriptionString() + "'");

                        if (testPanelView.Length > 0)
                        {
                            FrmTestPanelDetails frmTestPanelDetails = new FrmTestPanelDetails(OperationMode.Edit, testPanelId);
                            if (frmTestPanelDetails.ShowDialog() == DialogResult.OK)
                            {
                                LoadTestPanel(selectedIndex);
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

        private void dgvTestPanel_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvTestPanel.Rows)
            {
                TestCategories testCategory = (TestCategories)((int)row.Cells["TestCategoryId"].Value);
                row.Cells["Category"].Value = testCategory.ToDescriptionString();

                bool testPanelStatus = Convert.ToBoolean(row.Cells["IsActive"].Value);
                if (testPanelStatus.ToString() == "True")
                {
                    row.Cells["Status"].Value = "Active";
                }
                else
                {
                    row.Cells["Status"].Value = "Inactive";
                }
            }
        }

        # endregion

        # region Private Methods

        private void InitializeControls()
        {
            dgvTestPanel.AutoGenerateColumns = false;

            if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
            {
                //TEST_PANEL_ADD
                DataRow[] testpanelInfoAdd = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.TEST_PANEL_ADD.ToDescriptionString() + "'");

                if (testpanelInfoAdd.Length > 0)
                {
                    tsbNew.Visible = true;
                }
                else
                {
                    tsbNew.Visible = false;
                }

                //TEST_PANEL_EDIT
                DataRow[] testpanelInfoEdit = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.TEST_PANEL_EDIT.ToDescriptionString() + "'");

                if (testpanelInfoEdit.Length > 0)
                {
                    tsbEdit.Visible = true;
                    haveEditRights = true;
                }
                else
                {
                    tsbEdit.Visible = false;
                }

                //TEST_PANEL_ARCHIVE
                DataRow[] testpanelInfoArchive = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.TEST_PANEL_ARCHIVE.ToDescriptionString() + "'");

                if (testpanelInfoArchive.Length > 0)
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

        private void LoadTestPanel(int selectedIndex)
        {
            try
            {
                TestPanelBL testPanelBL = new TestPanelBL();
                List<TestPanel> testPanelList = null;

                Dictionary<string, string> searchParam = new Dictionary<string, string>();
                searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());
                testPanelList = testPanelBL.GetList(searchParam);

                dgvTestPanel.DataSource = testPanelList;

                if (dgvTestPanel.Rows.Count > 0)
                {
                    if (selectedIndex > dgvTestPanel.Rows.Count - 1)
                    {
                        selectedIndex = dgvTestPanel.Rows.Count - 1;
                    }
                    dgvTestPanel.Rows[selectedIndex].Selected = true;
                    dgvTestPanel.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                txtSearchKeyword.Text = string.Empty;
                LoadTestPanel(0);
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
                LoadTestPanel(0);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvTestPanel_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            bool active = false;
            if (chkIncludeInactive.Checked == true)
            {
                active = true;
            }

            TestPanelBL testPanelBL = new TestPanelBL();
            List<TestPanel> testPanelList = null;

            DataGridView dgv = sender as DataGridView;
            DataGridViewColumn col = dgv.Columns[e.ColumnIndex];

            if (col.Name == "TestPanelName")
            {
                DataGridViewColumn TestPanelName = dgv.Columns["TestPanelName"];
                string testPanelName = string.Empty;
                if (TestPanelName.HeaderCell.SortGlyphDirection == SortOrder.None || TestPanelName.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    testPanelList = testPanelBL.Sorting("testPanelName", active, "1");
                    dgvTestPanel.DataSource = testPanelList;

                    TestPanelName.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    testPanelList = testPanelBL.Sorting("testPanelName", active);
                    dgvTestPanel.DataSource = testPanelList;
                    TestPanelName.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "Category")
            {
                DataGridViewColumn TestCategory = dgv.Columns["TestCategoryId"];
                string testCategory = string.Empty;
                if (TestCategory.HeaderCell.SortGlyphDirection == SortOrder.None || TestCategory.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    testPanelList = testPanelBL.Sorting("testCategory", active, "1");
                    dgvTestPanel.DataSource = testPanelList;

                    TestCategory.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    testPanelList = testPanelBL.Sorting("testCategory", active);
                    dgvTestPanel.DataSource = testPanelList;
                    TestCategory.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "Status")
            {
                DataGridViewColumn IsActive = dgv.Columns["IsActive"];
                string isActive = string.Empty;
                if (IsActive.HeaderCell.SortGlyphDirection == SortOrder.None || IsActive.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    testPanelList = testPanelBL.Sorting("isActive", active, "1");
                    dgvTestPanel.DataSource = testPanelList;

                    IsActive.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    testPanelList = testPanelBL.Sorting("isActive", active);
                    dgvTestPanel.DataSource = testPanelList;
                    IsActive.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
        }
    }
}