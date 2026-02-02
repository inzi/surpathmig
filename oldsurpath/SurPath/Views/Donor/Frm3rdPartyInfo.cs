using SurPath.Business;
using SurPath.Entity;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SurPath
{
    public partial class Frm3rdPartyInfo : Form
    {
        // private bool haveEditRights = false;

        #region Constructor

        public Frm3rdPartyInfo()
        {
            InitializeComponent();
        }

        public Frm3rdPartyInfo(int donorId)
        {
            InitializeComponent();
            this._donorId = donorId;
        }

        #endregion Constructor

        private int _donorId;

        #region Event Methods

        private void Frm3rdPartyInfo_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeControls();
                LoadThirdPartyInfo(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Frm3rdPartyInfo_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                Program.frmMain.frm3rdPartyInfo = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void tsbNew_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                FrmThirdPartyDetails frmThirdPartyDetails = new FrmThirdPartyDetails(Enum.OperationMode.New, this._donorId, 0);
                if (frmThirdPartyDetails.ShowDialog() == DialogResult.OK)
                {
                    LoadThirdPartyInfo(0);
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
                Cursor.Current = Cursors.WaitCursor;
                if (dgvThirdParty.SelectedRows.Count > 0)
                {
                    int selectedIndex = dgvThirdParty.SelectedRows[0].Index;
                    int thirdPartyId = (int)dgvThirdParty.SelectedRows[0].Cells["ThirdPartyId"].Value;

                    FrmThirdPartyDetails frmThirdPartyDetails = new FrmThirdPartyDetails(Enum.OperationMode.Edit, this._donorId, thirdPartyId);
                    if (frmThirdPartyDetails.ShowDialog() == DialogResult.OK)
                    {
                        LoadThirdPartyInfo(selectedIndex);
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

        private void tsbArchive_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (dgvThirdParty.Rows.Count > 0)
                {
                    if (MessageBox.Show("Are you sure? Do you want to archive the selected record?", "SurPath Drug Testing", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        int thirdPartyId = (int)dgvThirdParty.SelectedRows[0].Cells["ThirdPartyId"].Value;
                        ThirdPartyBL thirdPartyBL = new ThirdPartyBL();
                        int returnvalue = thirdPartyBL.Delete(thirdPartyId, Program.currentUserName);
                        //if (returnvalue == 0)
                        //{
                        //    MessageBox.Show("You cannot delete this record,since this is used in other process.");
                        //    return;
                        //}
                        //else
                        //{
                        LoadThirdPartyInfo(0);
                        //}
                    }
                }
                else
                {
                    MessageBox.Show("Please select a Record.");
                    LoadThirdPartyInfo(0);
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
                    //LoadThirdPartyInfo(0);
                    ThirdPartyBL thirdPartyBL = new ThirdPartyBL();
                    List<ThirdParty> thirdPartyList = null;

                    Dictionary<string, string> searchParam = new Dictionary<string, string>();
                    searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                    searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());

                    thirdPartyList = thirdPartyBL.GetList(this._donorId, searchParam);

                    dgvThirdParty.DataSource = thirdPartyList;

                    if (dgvThirdParty.Rows.Count > 0)
                    {
                        //if (selectedIndex > dgvThirdParty.Rows.Count - 1)
                        //{
                        //    selectedIndex = dgvThirdParty.Rows.Count - 1;
                        //}
                        dgvThirdParty.Rows[0].Selected = true;
                        dgvThirdParty.Focus();
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                //  LoadThirdPartyInfo(0);
                ThirdPartyBL thirdPartyBL = new ThirdPartyBL();
                List<ThirdParty> thirdPartyList = null;

                Dictionary<string, string> searchParam = new Dictionary<string, string>();
                searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());

                thirdPartyList = thirdPartyBL.GetList(this._donorId, searchParam);

                dgvThirdParty.DataSource = thirdPartyList;

                if (dgvThirdParty.Rows.Count > 0)
                {
                    //if (selectedIndex > dgvThirdParty.Rows.Count - 1)
                    //{
                    //    selectedIndex = dgvThirdParty.Rows.Count - 1;
                    //}
                    dgvThirdParty.Rows[0].Selected = true;
                    dgvThirdParty.Focus();
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

        private void dgvThirdParty_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (e.KeyData == Keys.Enter)
                {
                    e.SuppressKeyPress = false;
                    if (dgvThirdParty.SelectedRows.Count > 0)
                    {
                        int selectedIndex = dgvThirdParty.SelectedRows[0].Index;
                        int thirdPartyId = (int)dgvThirdParty.SelectedRows[0].Cells["ThirdPartyId"].Value;

                        FrmThirdPartyDetails frmThirdPartyDetails = new FrmThirdPartyDetails(Enum.OperationMode.Edit, this._donorId, thirdPartyId);
                        if (frmThirdPartyDetails.ShowDialog() == DialogResult.OK)
                        {
                            LoadThirdPartyInfo(selectedIndex);
                        }
                    }
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvThirdParty_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (e.RowIndex >= 0)
                {
                    int selectedIndex = e.RowIndex;
                    int thirdPartyId = (int)dgvThirdParty.Rows[e.RowIndex].Cells["ThirdPartyId"].Value;

                    FrmThirdPartyDetails frmThirdPartyDetails = new FrmThirdPartyDetails(Enum.OperationMode.Edit, this._donorId, thirdPartyId);
                    if (frmThirdPartyDetails.ShowDialog() == DialogResult.OK)
                    {
                        LoadThirdPartyInfo(selectedIndex);
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

        private void BtnShowAll_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                txtSearchKeyword.Text = string.Empty;
                LoadThirdPartyInfo(0);
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
                LoadThirdPartyInfo(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvThirdParty_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dgvThirdParty.Rows)
                {
                    bool thirdPartyStatus = Convert.ToBoolean(row.Cells["IsActive"].Value);
                    if (thirdPartyStatus.ToString() == "True")
                    {
                        row.Cells["Status"].Value = "Active";
                    }
                    else
                    {
                        row.Cells["Status"].Value = "Inactive";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion Event Methods

        #region Private Methods

        private void InitializeControls()
        {
            dgvThirdParty.AutoGenerateColumns = false;

            //if (!(Program.currentUserName.ToUpper() == Program.adminUsername.ToUpper() || Program.currentUserName.ToUpper() == Program.adminUsername1.ToUpper()))
            //{
            //    //THIRD_PARTY_ADD
            //    DataRow[] thirdPartyInfoAdd = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.THIRD_PARTY_ADD.ToDescriptionString() + "'");

            //    if (thirdPartyInfoAdd.Length > 0)
            //    {
            //        tsbNew.Visible = true;
            //    }
            //    else
            //    {
            //        tsbNew.Visible = false;
            //    }

            //    //THIRD_PARTY_EDIT
            //    DataRow[] thirdPartyInfoEdit = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.THIRD_PARTY_EDIT.ToDescriptionString() + "'");

            //    if (thirdPartyInfoEdit.Length > 0)
            //    {
            //        tsbEdit.Visible = true;
            //        haveEditRights = true;
            //    }
            //    else
            //    {
            //        tsbEdit.Visible = false;
            //    }

            //    //THIRD_PARTY_ARCHIVE
            //    DataRow[] thirdPartyInfoArchive = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.THIRD_PARTY_ARCHIVE.ToDescriptionString() + "'");

            //    if (thirdPartyInfoArchive.Length > 0)
            //    {
            //        tsbArchive.Visible = true;
            //    }
            //    else
            //    {
            //        tsbArchive.Visible = false;
            //    }
            //}
            //else
            //{
            //    haveEditRights = true;
            //}
        }

        private void LoadThirdPartyInfo(int selectedIndex)
        {
            try
            {
                ThirdPartyBL thirdPartyBL = new ThirdPartyBL();
                List<ThirdParty> thirdPartyList = null;

                Dictionary<string, string> searchParam = new Dictionary<string, string>();
                searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());

                thirdPartyList = thirdPartyBL.GetList(this._donorId, searchParam);

                dgvThirdParty.DataSource = thirdPartyList;

                if (dgvThirdParty.Rows.Count > 0)
                {
                    if (selectedIndex > dgvThirdParty.Rows.Count - 1)
                    {
                        selectedIndex = dgvThirdParty.Rows.Count - 1;
                    }
                    dgvThirdParty.Rows[selectedIndex].Selected = true;
                    dgvThirdParty.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion Private Methods

        private void dgvThirdParty_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            bool active = false;
            if (chkIncludeInactive.Checked == true)
            {
                active = true;
            }

            ThirdPartyBL thirdPartyBL = new ThirdPartyBL();
            List<ThirdParty> thirdPartyList = null;

            DataGridView dgv = sender as DataGridView;
            DataGridViewColumn col = dgv.Columns[e.ColumnIndex];
            if (col.Name == "ThirdPartyFirstName")
            {
                DataGridViewColumn FirstName = dgv.Columns["ThirdPartyFirstName"];
                string firstName = string.Empty;
                if (FirstName.HeaderCell.SortGlyphDirection == SortOrder.None || FirstName.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    thirdPartyList = thirdPartyBL.Sorting(this._donorId, "firstName", active, "1");
                    dgvThirdParty.DataSource = thirdPartyList;

                    FirstName.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    thirdPartyList = thirdPartyBL.Sorting(this._donorId, "firstName", active);
                    dgvThirdParty.DataSource = thirdPartyList;
                    FirstName.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "ThirdPartyLastName")
            {
                DataGridViewColumn LastName = dgv.Columns["ThirdPartyLastName"];
                string lastName = string.Empty;
                if (LastName.HeaderCell.SortGlyphDirection == SortOrder.None || LastName.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    thirdPartyList = thirdPartyBL.Sorting(this._donorId, "lastName", active, "1");
                    dgvThirdParty.DataSource = thirdPartyList;

                    LastName.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    thirdPartyList = thirdPartyBL.Sorting(this._donorId, "lastName", active);
                    dgvThirdParty.DataSource = thirdPartyList;
                    LastName.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "ThirdPartyCity")
            {
                DataGridViewColumn City = dgv.Columns["ThirdPartyCity"];
                string city = string.Empty;
                if (City.HeaderCell.SortGlyphDirection == SortOrder.None || City.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    thirdPartyList = thirdPartyBL.Sorting(this._donorId, "city", active, "1");
                    dgvThirdParty.DataSource = thirdPartyList;

                    City.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    thirdPartyList = thirdPartyBL.Sorting(this._donorId, "city", active);
                    dgvThirdParty.DataSource = thirdPartyList;
                    City.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "ThirdPartyState")
            {
                DataGridViewColumn State = dgv.Columns["ThirdPartyState"];
                string state = string.Empty;
                if (State.HeaderCell.SortGlyphDirection == SortOrder.None || State.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    thirdPartyList = thirdPartyBL.Sorting(this._donorId, "state", active, "1");
                    dgvThirdParty.DataSource = thirdPartyList;

                    State.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    thirdPartyList = thirdPartyBL.Sorting(this._donorId, "state", active);
                    dgvThirdParty.DataSource = thirdPartyList;
                    State.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "ThirdPartyEmail")
            {
                DataGridViewColumn Email = dgv.Columns["ThirdPartyEmail"];
                string email = string.Empty;
                if (Email.HeaderCell.SortGlyphDirection == SortOrder.None || Email.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    thirdPartyList = thirdPartyBL.Sorting(this._donorId, "email", active, "1");
                    dgvThirdParty.DataSource = thirdPartyList;

                    Email.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    thirdPartyList = thirdPartyBL.Sorting(this._donorId, "email", active);
                    dgvThirdParty.DataSource = thirdPartyList;
                    Email.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "Status")
            {
                DataGridViewColumn IsActive = dgv.Columns["IsActive"];
                string isActive = string.Empty;
                if (IsActive.HeaderCell.SortGlyphDirection == SortOrder.None || IsActive.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    thirdPartyList = thirdPartyBL.Sorting(this._donorId, "isActive", active, "1");
                    dgvThirdParty.DataSource = thirdPartyList;

                    IsActive.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    thirdPartyList = thirdPartyBL.Sorting(this._donorId, "isActive", active);
                    dgvThirdParty.DataSource = thirdPartyList;
                    IsActive.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
        }
    }
}