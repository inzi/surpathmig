using SurPath.Business.Master;
using SurPath.Entity.Master;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmCourtInfo : Form
    {
        private bool haveEditRights = false;

        #region Constructor

        public FrmCourtInfo()
        {
            InitializeComponent();
        }

        #endregion Constructor

        #region Event Methods

        private void FrmCourtInfo_Load(object sender, System.EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                InitializeControls();
                LoadCourtInfo(0);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmCourtInfo_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                Program.frmMain.frmCourtInfo = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void tsbNew_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                FrmCourtDetails frmCourtDetails = new FrmCourtDetails(Enum.OperationMode.New, 0);
                if (frmCourtDetails.ShowDialog() == DialogResult.OK)
                {
                    LoadCourtInfo(0);
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
                    if (dgvCourt.SelectedRows.Count > 0)
                    {
                        int selectedIndex = dgvCourt.SelectedRows[0].Index;
                        int courtId = (int)dgvCourt.SelectedRows[0].Cells["CourtId"].Value;

                        FrmCourtDetails frmCourtDetails = new FrmCourtDetails(Enum.OperationMode.Edit, courtId);
                        frmCourtDetails.Size = new System.Drawing.Size(499, 293);
                        frmCourtDetails.btnOK.Location = new System.Drawing.Point(159, 225);
                        frmCourtDetails.btnClose.Location = new System.Drawing.Point(248, 225);
                        if (frmCourtDetails.ShowDialog() == DialogResult.OK)
                        {
                            LoadCourtInfo(selectedIndex);
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
                if (dgvCourt.Rows.Count > 0)
                {
                    if (MessageBox.Show("Are you sure? Do you want to archive the selected record?", "SurPath CourtInfo", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        int courtId = (int)dgvCourt.SelectedRows[0].Cells["CourtId"].Value;
                        CourtBL courtBL = new CourtBL();
                        int returnvalue = courtBL.Delete(courtId, Program.currentUserName);
                        if (returnvalue == 0)
                        {
                            MessageBox.Show("You cannot delete this record,since this is used in another process.");
                            return;
                        }
                        else
                        {
                            LoadCourtInfo(0);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a Record.");
                    LoadCourtInfo(0);
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
                // LoadCourtInfo(0);
                CourtBL courtBL = new CourtBL();
                List<Court> courtList = null;
                Dictionary<string, string> searchParam = new Dictionary<string, string>();
                searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());

                courtList = courtBL.GetList(searchParam);

                dgvCourt.DataSource = courtList;

                if (dgvCourt.Rows.Count > 0)
                {
                    //if (selectedIndex > dgvCourt.Rows.Count - 1)
                    //{
                    //    selectedIndex = dgvCourt.Rows.Count - 1;
                    //}
                    dgvCourt.Rows[0].Selected = true;
                    dgvCourt.Focus();
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
                    // LoadCourtInfo(0);

                    CourtBL courtBL = new CourtBL();
                    List<Court> courtList = null;
                    Dictionary<string, string> searchParam = new Dictionary<string, string>();
                    searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                    searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());

                    courtList = courtBL.GetList(searchParam);

                    dgvCourt.DataSource = courtList;

                    if (dgvCourt.Rows.Count > 0)
                    {
                        //if (selectedIndex > dgvCourt.Rows.Count - 1)
                        //{
                        //    selectedIndex = dgvCourt.Rows.Count - 1;
                        //}
                        dgvCourt.Rows[0].Selected = true;
                        dgvCourt.Focus();
                    }
                    else
                    {
                        MessageBox.Show("No Records Found");
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

        private void dgvCourt_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (e.KeyData == Keys.Enter)
                {
                    e.SuppressKeyPress = false;
                    if (dgvCourt.SelectedRows.Count > 0)
                    {
                        int selectedIndex = dgvCourt.SelectedRows[0].Index;
                        int courtId = (int)dgvCourt.SelectedRows[0].Cells["CourtId"].Value;
                        if (haveEditRights)
                        {
                            FrmCourtDetails frmCourtDetails = new FrmCourtDetails(Enum.OperationMode.Edit, courtId);
                            frmCourtDetails.Size = new System.Drawing.Size(499, 293);
                            frmCourtDetails.btnOK.Location = new System.Drawing.Point(159, 225);
                            frmCourtDetails.btnClose.Location = new System.Drawing.Point(248, 225);
                            if (frmCourtDetails.ShowDialog() == DialogResult.OK)
                            {
                                LoadCourtInfo(selectedIndex);
                            }
                        }

                        if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                        {
                            //COURT_VIEW
                            DataRow[] courtView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.COURT_VIEW.ToDescriptionString() + "'");

                            if (courtView.Length > 0)
                            {
                                FrmCourtDetails frmCourtDetails = new FrmCourtDetails(Enum.OperationMode.Edit, courtId);
                                frmCourtDetails.Size = new System.Drawing.Size(530, 293);
                                frmCourtDetails.btnOK.Location = new System.Drawing.Point(159, 225);
                                frmCourtDetails.btnClose.Location = new System.Drawing.Point(248, 225);
                                if (frmCourtDetails.ShowDialog() == DialogResult.OK)
                                {
                                    LoadCourtInfo(selectedIndex);
                                }
                            }
                        }
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvCourt_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (e.RowIndex >= 0)
                {
                    int selectedIndex = e.RowIndex;
                    int courtId = (int)dgvCourt.Rows[e.RowIndex].Cells["CourtId"].Value;
                    if (haveEditRights)
                    {
                        FrmCourtDetails frmCourtDetails = new FrmCourtDetails(Enum.OperationMode.Edit, courtId);
                        frmCourtDetails.Size = new System.Drawing.Size(530, 293);
                        frmCourtDetails.btnOK.Location = new System.Drawing.Point(159, 225);
                        frmCourtDetails.btnClose.Location = new System.Drawing.Point(248, 225);
                        if (frmCourtDetails.ShowDialog() == DialogResult.OK)
                        {
                            LoadCourtInfo(selectedIndex);
                        }
                    }

                    if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                    {
                        //COURT_VIEW
                        DataRow[] courtView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.COURT_VIEW.ToDescriptionString() + "'");

                        if (courtView.Length > 0)
                        {
                            FrmCourtDetails frmCourtDetails = new FrmCourtDetails(Enum.OperationMode.Edit, courtId);
                            frmCourtDetails.Size = new System.Drawing.Size(530, 293);
                            frmCourtDetails.btnOK.Location = new System.Drawing.Point(159, 225);
                            frmCourtDetails.btnClose.Location = new System.Drawing.Point(248, 225);
                            if (frmCourtDetails.ShowDialog() == DialogResult.OK)
                            {
                                LoadCourtInfo(selectedIndex);
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
            LoadCourtInfo(0);
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                txtSearchKeyword.Text = string.Empty;
                LoadCourtInfo(0);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvCourt_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvCourt.Rows)
            {
                bool courtStatus = Convert.ToBoolean(row.Cells["IsActive"].Value);
                if (courtStatus.ToString() == "True")
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
            try
            {
                dgvCourt.AutoGenerateColumns = false;

                if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                {
                    //COURT_ADD
                    DataRow[] courtInfoAdd = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.COURT_ADD.ToDescriptionString() + "'");

                    if (courtInfoAdd.Length > 0)
                    {
                        tsbNew.Visible = true;
                    }
                    else
                    {
                        tsbNew.Visible = false;
                    }

                    //COURT_EDIT
                    DataRow[] courtInfoEdit = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.COURT_EDIT.ToDescriptionString() + "'");

                    if (courtInfoEdit.Length > 0)
                    {
                        tsbEdit.Visible = true;
                        haveEditRights = true;
                    }
                    else
                    {
                        tsbEdit.Visible = false;
                    }

                    //COURT_ARCHIVE
                    DataRow[] courtInfoArchive = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.COURT_ARCHIVE.ToDescriptionString() + "'");

                    if (courtInfoArchive.Length > 0)
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadCourtInfo(int selectedIndex)
        {
            try
            {
                CourtBL courtBL = new CourtBL();
                List<Court> courtList = null;
                Dictionary<string, string> searchParam = new Dictionary<string, string>();
                searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());

                courtList = courtBL.GetList(searchParam);

                dgvCourt.DataSource = courtList;

                if (dgvCourt.Rows.Count > 0)
                {
                    if (selectedIndex > dgvCourt.Rows.Count - 1)
                    {
                        selectedIndex = dgvCourt.Rows.Count - 1;
                    }
                    dgvCourt.Rows[selectedIndex].Selected = true;
                    dgvCourt.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion Private Methods

        private void dgvCourt_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            bool active = false;
            if (chkIncludeInactive.Checked == true)
            {
                active = true;
            }

            CourtBL courtBL = new CourtBL();
            List<Court> courtList = null;

            DataGridView dgv = sender as DataGridView;
            DataGridViewColumn col = dgv.Columns[e.ColumnIndex];
            if (col.Name == "CourtName")
            {
                DataGridViewColumn CourtName = dgv.Columns["CourtName"];
                string courtName = string.Empty;
                if (CourtName.HeaderCell.SortGlyphDirection == SortOrder.None || CourtName.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    courtList = courtBL.Sorting("courtName", active, "1");
                    dgvCourt.DataSource = courtList;

                    CourtName.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    courtList = courtBL.Sorting("courtName", active);
                    dgvCourt.DataSource = courtList;
                    CourtName.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "CourtUsername")
            {
                DataGridViewColumn CourtUserName = dgv.Columns["CourtUsername"];
                string courtUserName = string.Empty;
                if (CourtUserName.HeaderCell.SortGlyphDirection == SortOrder.None || CourtUserName.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    courtList = courtBL.Sorting("courtUserName", active, "1");
                    dgvCourt.DataSource = courtList;

                    CourtUserName.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    courtList = courtBL.Sorting("courtUserName", active);
                    dgvCourt.DataSource = courtList;
                    CourtUserName.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "CourtCity")
            {
                DataGridViewColumn City = dgv.Columns["CourtCity"];
                string city = string.Empty;
                if (City.HeaderCell.SortGlyphDirection == SortOrder.None || City.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    courtList = courtBL.Sorting("city", active, "1");
                    dgvCourt.DataSource = courtList;

                    City.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    courtList = courtBL.Sorting("city", active);
                    dgvCourt.DataSource = courtList;
                    City.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "CourtState")
            {
                DataGridViewColumn State = dgv.Columns["CourtState"];
                string state = string.Empty;
                if (State.HeaderCell.SortGlyphDirection == SortOrder.None || State.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    courtList = courtBL.Sorting("state", active, "1");
                    dgvCourt.DataSource = courtList;

                    State.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    courtList = courtBL.Sorting("state", active);
                    dgvCourt.DataSource = courtList;
                    State.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "Status")
            {
                DataGridViewColumn IsActive = dgv.Columns["IsActive"];
                string isActive = string.Empty;
                if (IsActive.HeaderCell.SortGlyphDirection == SortOrder.None || IsActive.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    courtList = courtBL.Sorting("isActive", active, "1");
                    dgvCourt.DataSource = courtList;

                    IsActive.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    courtList = courtBL.Sorting("isActive", active);
                    dgvCourt.DataSource = courtList;
                    IsActive.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
        }
    }
}