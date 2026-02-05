using SurPath.Business.Master;
using SurPath.Entity.Master;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmAttorneyInfo : Form
    {
        private bool haveEditRights = false;

        #region Constructor

        public FrmAttorneyInfo()
        {
            InitializeComponent();
        }

        #endregion Constructor

        #region Event Methods

        private void FrmAttorneyInfo_Load(object sender, System.EventArgs e)
        {
            try
            {
                InitializeControls();
                LoadAttorneyInfo(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmAttorneyInfo_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                Program.frmMain.frmAttorneyInfo = null;
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
                FrmAttorneyDetails frmAttorneyDetails = new FrmAttorneyDetails(Enum.OperationMode.New, 0);
                if (frmAttorneyDetails.ShowDialog() == DialogResult.OK)
                {
                    LoadAttorneyInfo(0);
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
                    if (dgvAttorney.SelectedRows.Count > 0)
                    {
                        int selectedIndex = dgvAttorney.SelectedRows[0].Index;
                        int attorneyId = (int)dgvAttorney.SelectedRows[0].Cells["AttorneyId"].Value;

                        FrmAttorneyDetails frmAttorneyDetails = new FrmAttorneyDetails(Enum.OperationMode.Edit, attorneyId);
                        if (frmAttorneyDetails.ShowDialog() == DialogResult.OK)
                        {
                            LoadAttorneyInfo(selectedIndex);
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
                if (dgvAttorney.Rows.Count > 0)
                {
                    if (MessageBox.Show("Are you sure? Do you want to archive the selected record?", "SurPath Drug Testing", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        int attorneyId = (int)dgvAttorney.SelectedRows[0].Cells["AttorneyId"].Value;
                        AttorneyBL attorneyBL = new AttorneyBL();
                        int returnvalue = attorneyBL.Delete(attorneyId, Program.currentUserName);
                        if (returnvalue == 0)
                        {
                            MessageBox.Show("You cannot delete this record,since this is used in another process.");
                            return;
                        }
                        else
                        {
                            LoadAttorneyInfo(0);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a Record.");
                    LoadAttorneyInfo(0);
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
                // LoadAttorneyInfo(0);
                AttorneyBL attorneyBL = new AttorneyBL();
                List<Attorney> attorneyList = null;

                Dictionary<string, string> searchParam = new Dictionary<string, string>();
                searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());
                attorneyList = attorneyBL.GetList(searchParam);

                dgvAttorney.DataSource = attorneyList;

                if (dgvAttorney.Rows.Count > 0)
                {
                    //if (selectedIndex > dgvAttorney.Rows.Count - 1)
                    //{
                    //    selectedIndex = dgvAttorney.Rows.Count - 1;
                    //}
                    dgvAttorney.Rows[0].Selected = true;
                    dgvAttorney.Focus();
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
                Cursor.Current = Cursors.WaitCursor;
                if (e.KeyData == Keys.Enter)
                {
                    // LoadAttorneyInfo(0);
                    AttorneyBL attorneyBL = new AttorneyBL();
                    List<Attorney> attorneyList = null;

                    Dictionary<string, string> searchParam = new Dictionary<string, string>();
                    searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                    searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());
                    attorneyList = attorneyBL.GetList(searchParam);

                    dgvAttorney.DataSource = attorneyList;

                    if (dgvAttorney.Rows.Count > 0)
                    {
                        //if (selectedIndex > dgvAttorney.Rows.Count - 1)
                        //{
                        //    selectedIndex = dgvAttorney.Rows.Count - 1;
                        //}
                        dgvAttorney.Rows[0].Selected = true;
                        dgvAttorney.Focus();
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

        private void dgvAttorney_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (e.KeyData == Keys.Enter)
                {
                    e.SuppressKeyPress = false;
                    if (dgvAttorney.SelectedRows.Count > 0)
                    {
                        int selectedIndex = dgvAttorney.SelectedRows[0].Index;
                        int attorneyId = (int)dgvAttorney.SelectedRows[0].Cells["AttorneyId"].Value;
                        if (haveEditRights)
                        {
                            FrmAttorneyDetails frmAttorneyDetails = new FrmAttorneyDetails(Enum.OperationMode.Edit, attorneyId);
                            if (frmAttorneyDetails.ShowDialog() == DialogResult.OK)
                            {
                                LoadAttorneyInfo(selectedIndex);
                            }
                        }

                        if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                        {
                            //ATTORNEY_VIEW
                            DataRow[] attorneyView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.ATTORNEY_VIEW.ToDescriptionString() + "'");

                            if (attorneyView.Length > 0)
                            {
                                FrmAttorneyDetails frmAttorneyDetails = new FrmAttorneyDetails(Enum.OperationMode.Edit, attorneyId);
                                if (frmAttorneyDetails.ShowDialog() == DialogResult.OK)
                                {
                                    LoadAttorneyInfo(selectedIndex);
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

        private void dgvAttorney_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (e.RowIndex >= 0)
                {
                    int selectedIndex = e.RowIndex;
                    int attorneyId = (int)dgvAttorney.Rows[e.RowIndex].Cells["AttorneyId"].Value;

                    if (haveEditRights)
                    {
                        FrmAttorneyDetails frmAttorneyDetails = new FrmAttorneyDetails(Enum.OperationMode.Edit, attorneyId);
                        if (frmAttorneyDetails.ShowDialog() == DialogResult.OK)
                        {
                            LoadAttorneyInfo(selectedIndex);
                        }
                    }
                    if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                    {
                        //ATTORNEY_VIEW
                        DataRow[] attorneyView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.ATTORNEY_VIEW.ToDescriptionString() + "'");

                        if (attorneyView.Length > 0)
                        {
                            FrmAttorneyDetails frmAttorneyDetails = new FrmAttorneyDetails(Enum.OperationMode.Edit, attorneyId);
                            if (frmAttorneyDetails.ShowDialog() == DialogResult.OK)
                            {
                                LoadAttorneyInfo(selectedIndex);
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

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                txtSearchKeyword.Text = string.Empty;
                LoadAttorneyInfo(0);
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
                LoadAttorneyInfo(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvAttorney_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvAttorney.Rows)
            {
                bool attorneyStatus = Convert.ToBoolean(row.Cells["IsActive"].Value);
                if (attorneyStatus.ToString() == "True")
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
                dgvAttorney.AutoGenerateColumns = false;

                if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                {
                    //ATTORNEY_ADD
                    DataRow[] attorneyInfoAdd = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.ATTORNEY_ADD.ToDescriptionString() + "'");

                    if (attorneyInfoAdd.Length > 0)
                    {
                        tsbNew.Visible = true;
                    }
                    else
                    {
                        tsbNew.Visible = false;
                    }

                    //ATTORNEY_EDIT
                    DataRow[] attorneyInfoEdit = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.ATTORNEY_EDIT.ToDescriptionString() + "'");

                    if (attorneyInfoEdit.Length > 0)
                    {
                        tsbEdit.Visible = true;
                        haveEditRights = true;
                    }
                    else
                    {
                        tsbEdit.Visible = false;
                    }

                    //ATTORNEY_ARCHIVE
                    DataRow[] attorneyInfoArchive = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.ATTORNEY_ARCHIVE.ToDescriptionString() + "'");

                    if (attorneyInfoArchive.Length > 0)
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

        private void LoadAttorneyInfo(int selectedIndex)
        {
            try
            {
                AttorneyBL attorneyBL = new AttorneyBL();
                List<Attorney> attorneyList = null;

                Dictionary<string, string> searchParam = new Dictionary<string, string>();
                searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());
                attorneyList = attorneyBL.GetList(searchParam);

                dgvAttorney.DataSource = attorneyList;

                if (dgvAttorney.Rows.Count > 0)
                {
                    if (selectedIndex > dgvAttorney.Rows.Count - 1)
                    {
                        selectedIndex = dgvAttorney.Rows.Count - 1;
                    }
                    dgvAttorney.Rows[selectedIndex].Selected = true;
                    dgvAttorney.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion Private Methods

        private void dgvAttorney_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            bool active = false;
            if (chkIncludeInactive.Checked == true)
            {
                active = true;
            }

            AttorneyBL attorneyBL = new AttorneyBL();
            List<Attorney> attorneyList = null;

            DataGridView dgv = sender as DataGridView;
            DataGridViewColumn col = dgv.Columns[e.ColumnIndex];
            if (col.Name == "AttorneyFirstName")
            {
                DataGridViewColumn FirstName = dgv.Columns["AttorneyFirstName"];
                string firstName = string.Empty;
                if (FirstName.HeaderCell.SortGlyphDirection == SortOrder.None || FirstName.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    attorneyList = attorneyBL.Sorting("firstName", active, "1");
                    dgvAttorney.DataSource = attorneyList;

                    FirstName.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    attorneyList = attorneyBL.Sorting("firstName", active);
                    dgvAttorney.DataSource = attorneyList;
                    FirstName.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "AttorneyLastName")
            {
                DataGridViewColumn LastName = dgv.Columns["AttorneyLastName"];
                string lastName = string.Empty;
                if (LastName.HeaderCell.SortGlyphDirection == SortOrder.None || LastName.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    attorneyList = attorneyBL.Sorting("lastName", active, "1");
                    dgvAttorney.DataSource = attorneyList;

                    LastName.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    attorneyList = attorneyBL.Sorting("lastName", active);
                    dgvAttorney.DataSource = attorneyList;
                    LastName.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "AttorneyCity")
            {
                DataGridViewColumn City = dgv.Columns["AttorneyCity"];
                string city = string.Empty;
                if (City.HeaderCell.SortGlyphDirection == SortOrder.None || City.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    attorneyList = attorneyBL.Sorting("city", active, "1");
                    dgvAttorney.DataSource = attorneyList;

                    City.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    attorneyList = attorneyBL.Sorting("city", active);
                    dgvAttorney.DataSource = attorneyList;
                    City.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "AttorneyState")
            {
                DataGridViewColumn State = dgv.Columns["AttorneyState"];
                string state = string.Empty;
                if (State.HeaderCell.SortGlyphDirection == SortOrder.None || State.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    attorneyList = attorneyBL.Sorting("state", active, "1");
                    dgvAttorney.DataSource = attorneyList;

                    State.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    attorneyList = attorneyBL.Sorting("state", active);
                    dgvAttorney.DataSource = attorneyList;
                    State.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "AttorneyEmail")
            {
                DataGridViewColumn Email = dgv.Columns["AttorneyEmail"];
                string email = string.Empty;
                if (Email.HeaderCell.SortGlyphDirection == SortOrder.None || Email.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    attorneyList = attorneyBL.Sorting("email", active, "1");
                    dgvAttorney.DataSource = attorneyList;

                    Email.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    attorneyList = attorneyBL.Sorting("email", active);
                    dgvAttorney.DataSource = attorneyList;
                    Email.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "Status")
            {
                DataGridViewColumn IsActive = dgv.Columns["IsActive"];
                string isActive = string.Empty;
                if (IsActive.HeaderCell.SortGlyphDirection == SortOrder.None || IsActive.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    attorneyList = attorneyBL.Sorting("isActive", active, "1");
                    dgvAttorney.DataSource = attorneyList;

                    IsActive.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    attorneyList = attorneyBL.Sorting("isActive", active);
                    dgvAttorney.DataSource = attorneyList;
                    IsActive.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
        }
    }
}