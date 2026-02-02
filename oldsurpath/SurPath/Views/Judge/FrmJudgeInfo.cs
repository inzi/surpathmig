using SurPath.Business.Master;
using SurPath.Entity.Master;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmJudgeInfo : Form
    {
        private bool haveEditRights = false;

        #region Constructor

        public FrmJudgeInfo()
        {
            InitializeComponent();
        }

        #endregion Constructor

        #region Event Methods

        private void FrmJudgeInfo_Load(object sender, System.EventArgs e)
        {
            try
            {
                InitializeControls();
                LoadJudgeInfo(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while loading the page.");
            }
        }

        private void FrmJudgeInfo_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.frmMain.frmJudgeInfo = null;
        }

        private void tsbNew_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                FrmJudgeDetails frmJudgeDetails = new FrmJudgeDetails(Enum.OperationMode.New, 0);
                if (frmJudgeDetails.ShowDialog() == DialogResult.OK)
                {
                    LoadJudgeInfo(0);
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
                    if (dgvJudge.SelectedRows.Count > 0)
                    {
                        int selectedIndex = dgvJudge.SelectedRows[0].Index;
                        int judgeId = (int)dgvJudge.SelectedRows[0].Cells["JudgeId"].Value;
                        FrmJudgeDetails frmJudgeDetails = new FrmJudgeDetails(Enum.OperationMode.Edit, judgeId);
                        frmJudgeDetails.Size = new System.Drawing.Size(721, 288);
                        frmJudgeDetails.btnOK.Location = new System.Drawing.Point(271, 222);
                        frmJudgeDetails.btnClose.Location = new System.Drawing.Point(358, 222);
                        if (frmJudgeDetails.ShowDialog() == DialogResult.OK)
                        {
                            LoadJudgeInfo(selectedIndex);
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
                if (dgvJudge.Rows.Count > 0)
                {
                    if (MessageBox.Show("Are you sure? Do you want to archive the selected record?", "SurPath Drug Testing", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        int JudgeId = (int)dgvJudge.SelectedRows[0].Cells["JudgeId"].Value;
                        JudgeBL judgeBL = new JudgeBL();
                        int returnvalue = judgeBL.Delete(JudgeId, Program.currentUserName);
                        if (returnvalue == 0)
                        {
                            MessageBox.Show("You cannot delete this record,since this is used in another process.");
                            return;
                        }
                        else
                        {
                            LoadJudgeInfo(0);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a Record.");
                    LoadJudgeInfo(0);
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
                //  LoadJudgeInfo(0);
                JudgeBL judgeBL = new JudgeBL();
                List<Judge> judgeList = null;

                Dictionary<string, string> searchParam = new Dictionary<string, string>();
                searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());
                judgeList = judgeBL.GetList(searchParam);
                dgvJudge.DataSource = judgeList;

                if (dgvJudge.Rows.Count > 0)
                {
                    //if (selectedIndex > dgvJudge.Rows.Count - 1)
                    //{
                    //    selectedIndex = dgvJudge.Rows.Count - 1;
                    //}

                    dgvJudge.Rows[0].Selected = true;
                    dgvJudge.Focus();
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

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                txtSearchKeyword.Text = string.Empty;
                LoadJudgeInfo(0);
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
                LoadJudgeInfo(0);
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
                Cursor.Current = Cursors.WaitCursor;
                if (e.KeyData == Keys.Enter)
                {
                    // LoadJudgeInfo(0);

                    JudgeBL judgeBL = new JudgeBL();
                    List<Judge> judgeList = null;

                    Dictionary<string, string> searchParam = new Dictionary<string, string>();
                    searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                    searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());
                    judgeList = judgeBL.GetList(searchParam);
                    dgvJudge.DataSource = judgeList;

                    if (dgvJudge.Rows.Count > 0)
                    {
                        //if (selectedIndex > dgvJudge.Rows.Count - 1)
                        //{
                        //    selectedIndex = dgvJudge.Rows.Count - 1;
                        //}

                        dgvJudge.Rows[0].Selected = true;
                        dgvJudge.Focus();
                    }
                    else
                    {
                        MessageBox.Show("No Records Found");
                    }
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvJudge_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (e.RowIndex >= 0)
                {
                    int selectedIndex = e.RowIndex;

                    int judgeId = (int)dgvJudge.Rows[e.RowIndex].Cells["JudgeId"].Value;
                    if (haveEditRights)
                    {
                        FrmJudgeDetails frmJudgeDetails = new FrmJudgeDetails(Enum.OperationMode.Edit, judgeId);
                        frmJudgeDetails.Size = new System.Drawing.Size(721, 288);
                        frmJudgeDetails.btnOK.Location = new System.Drawing.Point(271, 222);
                        frmJudgeDetails.btnClose.Location = new System.Drawing.Point(358, 222);
                        if (frmJudgeDetails.ShowDialog() == DialogResult.OK)
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            LoadJudgeInfo(selectedIndex);
                        }
                    }

                    if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                    {
                        //JUDGE_VIEW
                        DataRow[] judgeView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.JUDGE_VIEW.ToDescriptionString() + "'");

                        if (judgeView.Length > 0)
                        {
                            FrmJudgeDetails frmJudgeDetails = new FrmJudgeDetails(Enum.OperationMode.Edit, judgeId);
                            frmJudgeDetails.Size = new System.Drawing.Size(721, 288);
                            frmJudgeDetails.btnOK.Location = new System.Drawing.Point(271, 222);
                            frmJudgeDetails.btnClose.Location = new System.Drawing.Point(358, 222);
                            if (frmJudgeDetails.ShowDialog() == DialogResult.OK)
                            {
                                Cursor.Current = Cursors.WaitCursor;
                                LoadJudgeInfo(selectedIndex);
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

        private void dgvJudge_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.Enter)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    e.SuppressKeyPress = false;
                    if (dgvJudge.SelectedRows.Count > 0)
                    {
                        int selectedIndex = dgvJudge.SelectedRows[0].Index;
                        int judgeId = (int)dgvJudge.SelectedRows[0].Cells["JudgeId"].Value;

                        if (haveEditRights)
                        {
                            FrmJudgeDetails frmJudgeDetails = new FrmJudgeDetails(Enum.OperationMode.Edit, judgeId);
                            frmJudgeDetails.Size = new System.Drawing.Size(721, 288);
                            frmJudgeDetails.btnOK.Location = new System.Drawing.Point(271, 222);
                            frmJudgeDetails.btnClose.Location = new System.Drawing.Point(358, 222);
                            if (frmJudgeDetails.ShowDialog() == DialogResult.OK)
                            {
                                Cursor.Current = Cursors.WaitCursor;
                                LoadJudgeInfo(selectedIndex);
                            }
                        }

                        if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                        {
                            //JUDGE_VIEW
                            DataRow[] judgeView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.JUDGE_VIEW.ToDescriptionString() + "'");

                            if (judgeView.Length > 0)
                            {
                                FrmJudgeDetails frmJudgeDetails = new FrmJudgeDetails(Enum.OperationMode.Edit, judgeId);
                                frmJudgeDetails.Size = new System.Drawing.Size(721, 288);
                                frmJudgeDetails.btnOK.Location = new System.Drawing.Point(271, 222);
                                frmJudgeDetails.btnClose.Location = new System.Drawing.Point(358, 222);
                                if (frmJudgeDetails.ShowDialog() == DialogResult.OK)
                                {
                                    Cursor.Current = Cursors.WaitCursor;
                                    LoadJudgeInfo(selectedIndex);
                                }
                            }
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

        private void dgvJudge_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvJudge.Rows)
            {
                bool judgeStatus = Convert.ToBoolean(row.Cells["IsActive"].Value);
                if (judgeStatus.ToString() == "True")
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
            dgvJudge.AutoGenerateColumns = false;

            if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
            {
                //JUDGE_ADD
                DataRow[] judgeInfoAdd = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.JUDGE_ADD.ToDescriptionString() + "'");

                if (judgeInfoAdd.Length > 0)
                {
                    tsbNew.Visible = true;
                }
                else
                {
                    tsbNew.Visible = false;
                }

                //JUDGE_EDIT
                DataRow[] judgeInfoEdit = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.JUDGE_EDIT.ToDescriptionString() + "'");

                if (judgeInfoEdit.Length > 0)
                {
                    tsbEdit.Visible = true;
                    haveEditRights = true;
                }
                else
                {
                    tsbEdit.Visible = false;
                }

                //JUDGE_ARCHIVE
                DataRow[] judgeInfoArchive = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.JUDGE_ARCHIVE.ToDescriptionString() + "'");

                if (judgeInfoArchive.Length > 0)
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

        private void LoadJudgeInfo(int selectedIndex)
        {
            try
            {
                JudgeBL judgeBL = new JudgeBL();
                List<Judge> judgeList = null;

                Dictionary<string, string> searchParam = new Dictionary<string, string>();
                searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());
                judgeList = judgeBL.GetList(searchParam);
                dgvJudge.DataSource = judgeList;

                if (dgvJudge.Rows.Count > 0)
                {
                    if (selectedIndex > dgvJudge.Rows.Count - 1)
                    {
                        selectedIndex = dgvJudge.Rows.Count - 1;
                    }

                    dgvJudge.Rows[selectedIndex].Selected = true;
                    dgvJudge.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion Private Methods

        private void dgvJudge_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            bool active = false;
            if (chkIncludeInactive.Checked == true)
            {
                active = true;
            }

            JudgeBL judgeBL = new JudgeBL();
            List<Judge> judgeList = null;

            DataGridView dgv = sender as DataGridView;
            DataGridViewColumn col = dgv.Columns[e.ColumnIndex];
            if (col.Name == "JudgeFirstName")
            {
                DataGridViewColumn FirstName = dgv.Columns["JudgeFirstName"];
                string firstName = string.Empty;
                if (FirstName.HeaderCell.SortGlyphDirection == SortOrder.None || FirstName.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    judgeList = judgeBL.Sorting("firstName", active, "1");
                    dgvJudge.DataSource = judgeList;

                    FirstName.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    judgeList = judgeBL.Sorting("firstName", active);
                    dgvJudge.DataSource = judgeList;
                    FirstName.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "JudgeLastName")
            {
                DataGridViewColumn LastName = dgv.Columns["JudgeLastName"];
                string lastName = string.Empty;
                if (LastName.HeaderCell.SortGlyphDirection == SortOrder.None || LastName.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    judgeList = judgeBL.Sorting("lastName", active, "1");
                    dgvJudge.DataSource = judgeList;

                    LastName.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    judgeList = judgeBL.Sorting("lastName", active);
                    dgvJudge.DataSource = judgeList;
                    LastName.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "JudgeSuffix")
            {
                DataGridViewColumn Suffix = dgv.Columns["JudgeSuffix"];
                string suffix = string.Empty;
                if (Suffix.HeaderCell.SortGlyphDirection == SortOrder.None || Suffix.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    judgeList = judgeBL.Sorting("suffix", active, "1");
                    dgvJudge.DataSource = judgeList;

                    Suffix.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    judgeList = judgeBL.Sorting("suffix", active);
                    dgvJudge.DataSource = judgeList;
                    Suffix.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "JudgeUsername")
            {
                DataGridViewColumn UserName = dgv.Columns["JudgeUsername"];
                string userName = string.Empty;
                if (UserName.HeaderCell.SortGlyphDirection == SortOrder.None || UserName.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    judgeList = judgeBL.Sorting("userName", active, "1");
                    dgvJudge.DataSource = judgeList;

                    UserName.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    judgeList = judgeBL.Sorting("userName", active);
                    dgvJudge.DataSource = judgeList;
                    UserName.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "JudgeCity")
            {
                DataGridViewColumn City = dgv.Columns["JudgeCity"];
                string city = string.Empty;
                if (City.HeaderCell.SortGlyphDirection == SortOrder.None || City.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    judgeList = judgeBL.Sorting("city", active, "1");
                    dgvJudge.DataSource = judgeList;

                    City.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    judgeList = judgeBL.Sorting("city", active);
                    dgvJudge.DataSource = judgeList;
                    City.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "JudgeState")
            {
                DataGridViewColumn State = dgv.Columns["JudgeState"];
                string state = string.Empty;
                if (State.HeaderCell.SortGlyphDirection == SortOrder.None || State.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    judgeList = judgeBL.Sorting("state", active, "1");
                    dgvJudge.DataSource = judgeList;

                    State.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    judgeList = judgeBL.Sorting("state", active);
                    dgvJudge.DataSource = judgeList;
                    State.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "Status")
            {
                DataGridViewColumn IsActive = dgv.Columns["IsActive"];
                string isActive = string.Empty;
                if (IsActive.HeaderCell.SortGlyphDirection == SortOrder.None || IsActive.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    judgeList = judgeBL.Sorting("isActive", active, "1");
                    dgvJudge.DataSource = judgeList;

                    IsActive.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    judgeList = judgeBL.Sorting("isActive", active);
                    dgvJudge.DataSource = judgeList;
                    IsActive.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
        }
    }
}