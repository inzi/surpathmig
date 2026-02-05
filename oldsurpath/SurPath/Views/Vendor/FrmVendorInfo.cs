using SurPath.Business;
using SurPath.Entity;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmVendorInfo : Form
    {
        private bool haveEditRights = false;

        #region Constructor

        public FrmVendorInfo()
        {
            InitializeComponent();
        }

        #endregion Constructor

        #region Event Methods

        private void FrmVendorInfo_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeControls();
                LoadVendorInfo(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while loading the page.");
            }
        }

        private void FrmVendorInfo_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.frmMain.frmVendorInfo = null;
        }

        private void tsbNew_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                FrmVendorDetails frmVendorDetails = new FrmVendorDetails(Enum.OperationMode.New, 0);
                if (frmVendorDetails.ShowDialog() == DialogResult.OK)
                {
                    LoadVendorInfo(0);
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
                    if (dgvVendorInfo.SelectedRows.Count > 0)
                    {
                        int selectedIndex = dgvVendorInfo.SelectedRows[0].Index;
                        int vendorId = (int)dgvVendorInfo.SelectedRows[0].Cells["VendorId"].Value;

                        FrmVendorDetails frmVendorDetails = new FrmVendorDetails(Enum.OperationMode.Edit, vendorId);
                        if (frmVendorDetails.ShowDialog() == DialogResult.OK)
                        {
                            LoadVendorInfo(selectedIndex);
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
                if (dgvVendorInfo.SelectedRows.Count > 0)
                {
                    if (MessageBox.Show("Are you sure? Do you want to archive the selected record?", "SurPath Drug Testing", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        int vendorId = (int)dgvVendorInfo.SelectedRows[0].Cells["VendorId"].Value;
                        VendorBL vendorBL = new VendorBL();
                        int returnvalue = vendorBL.Delete(vendorId, Program.currentUserName);
                        if (returnvalue == 0)
                        {
                            MessageBox.Show("You cannot delete this record,since this is used in other process.");
                            return;
                        }
                        else
                        {
                            LoadVendorInfo(0);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a Record.");
                    LoadVendorInfo(0);
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
                //    LoadVendorInfo(0);
                VendorBL vendorBL = new VendorBL();
                List<Vendor> vendorList = null;
                Dictionary<string, string> searchParam = new Dictionary<string, string>();
                searchParam.Add("VendorType", cmbVendorType.Text);
                searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());

                vendorList = vendorBL.GetList(searchParam);
                dgvVendorInfo.DataSource = vendorList;

                if (dgvVendorInfo.Rows.Count > 0)
                {
                    //if (selectedIndex > dgvVendorInfo.Rows.Count - 1)
                    //{
                    //    selectedIndex = dgvVendorInfo.Rows.Count - 1;
                    //}
                    dgvVendorInfo.Rows[0].Selected = true;
                    dgvVendorInfo.Focus();
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
                    //   LoadVendorInfo(0);
                    VendorBL vendorBL = new VendorBL();
                    List<Vendor> vendorList = null;
                    Dictionary<string, string> searchParam = new Dictionary<string, string>();
                    searchParam.Add("VendorType", cmbVendorType.Text);
                    searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                    searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());

                    vendorList = vendorBL.GetList(searchParam);
                    dgvVendorInfo.DataSource = vendorList;

                    if (dgvVendorInfo.Rows.Count > 0)
                    {
                        //if (selectedIndex > dgvVendorInfo.Rows.Count - 1)
                        //{
                        //    selectedIndex = dgvVendorInfo.Rows.Count - 1;
                        //}
                        dgvVendorInfo.Rows[0].Selected = true;
                        dgvVendorInfo.Focus();
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

        private void dgvVendorInfo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.Enter)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    e.SuppressKeyPress = false;
                    if (dgvVendorInfo.SelectedRows.Count > 0)
                    {
                        int selectedIndex = dgvVendorInfo.SelectedRows[0].Index;
                        int vendorId = (int)dgvVendorInfo.SelectedRows[0].Cells["VendorId"].Value;
                        if (haveEditRights)
                        {
                            FrmVendorDetails frmVendorDetails = new FrmVendorDetails(OperationMode.Edit, vendorId);
                            if (frmVendorDetails.ShowDialog() == DialogResult.OK)
                            {
                                LoadVendorInfo(selectedIndex);
                            }
                        }

                        if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                        {
                            //VENDOR_VIEW
                            DataRow[] vendorView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.VENDOR_VIEW.ToDescriptionString() + "'");

                            if (vendorView.Length > 0)
                            {
                                FrmVendorDetails frmVendorDetails = new FrmVendorDetails(OperationMode.Edit, vendorId);
                                if (frmVendorDetails.ShowDialog() == DialogResult.OK)
                                {
                                    LoadVendorInfo(selectedIndex);
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

        private void dgvVendorInfo_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (e.RowIndex >= 0)
                {
                    int selectedIndex = e.RowIndex;
                    int vendorId = (int)dgvVendorInfo.Rows[e.RowIndex].Cells["VendorId"].Value;
                    if (haveEditRights)
                    {
                        FrmVendorDetails frmVendorDetails = new FrmVendorDetails(OperationMode.Edit, vendorId);
                        if (frmVendorDetails.ShowDialog() == DialogResult.OK)
                        {
                            LoadVendorInfo(selectedIndex);
                        }
                    }
                    if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                    {
                        //VENDOR_VIEW
                        DataRow[] vendorView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.VENDOR_VIEW.ToDescriptionString() + "'");

                        if (vendorView.Length > 0)
                        {
                            FrmVendorDetails frmVendorDetails = new FrmVendorDetails(OperationMode.Edit, vendorId);
                            if (frmVendorDetails.ShowDialog() == DialogResult.OK)
                            {
                                LoadVendorInfo(selectedIndex);
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

        private void dgvVendorInfo_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvVendorInfo.Rows)
            {
                VendorTypes vendorTypes = (VendorTypes)((int)row.Cells["VendorTypeId"].Value);
                row.Cells["VendorTypes"].Value = vendorTypes.ToDescriptionString();

                VendorStatus vendorStatus = (VendorStatus)((int)row.Cells["VendorStatus"].Value);
                row.Cells["Status"].Value = vendorStatus.ToDescriptionString();
            }
            foreach (DataGridViewColumn column in dgvVendorInfo.Columns)
            {
                //  column.SortMode = DataGridViewColumnSortMode.Programmatic;
                column.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
        }

        #endregion Event Methods

        #region Private Methods

        private void InitializeControls()
        {
            dgvVendorInfo.AutoGenerateColumns = false;
            cmbVendorType.SelectedIndex = 0;
            chkIncludeInactive.Checked = false;

            if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
            {
                //VENDOR_ADD
                DataRow[] vendorInfoAdd = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.VENDOR_ADD.ToDescriptionString() + "'");

                if (vendorInfoAdd.Length > 0)
                {
                    tsbNew.Visible = true;
                }
                else
                {
                    tsbNew.Visible = false;
                }

                //VENDOR_EDIT
                DataRow[] vendorInfoEdit = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.VENDOR_EDIT.ToDescriptionString() + "'");

                if (vendorInfoEdit.Length > 0)
                {
                    tsbEdit.Visible = true;
                    haveEditRights = true;
                }
                else
                {
                    tsbEdit.Visible = false;
                }

                //VENDOR_ARCHIVE
                DataRow[] vendorInfoArchive = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.VENDOR_ARCHIVE.ToDescriptionString() + "'");

                if (vendorInfoArchive.Length > 0)
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

        private void LoadVendorInfo(int selectedIndex)
        {
            VendorBL vendorBL = new VendorBL();
            List<Vendor> vendorList = null;
            Dictionary<string, string> searchParam = new Dictionary<string, string>();
            searchParam.Add("VendorType", cmbVendorType.Text);
            searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
            searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());

            vendorList = vendorBL.GetList(searchParam);
            dgvVendorInfo.DataSource = vendorList;

            if (dgvVendorInfo.Rows.Count > 0)
            {
                if (selectedIndex > dgvVendorInfo.Rows.Count - 1)
                {
                    selectedIndex = dgvVendorInfo.Rows.Count - 1;
                }
                dgvVendorInfo.Rows[selectedIndex].Selected = true;
                dgvVendorInfo.Focus();
            }
        }

        #endregion Private Methods

        private void chkIncludeInactive_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                LoadVendorInfo(0);
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
                LoadVendorInfo(0);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbVendorType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                LoadVendorInfo(0);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvVendorInfo_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            bool active = false;
            string VendorType;
            if (chkIncludeInactive.Checked == true)
            {
                active = true;
            }

            if (cmbVendorType.SelectedIndex != 0)
            {
                VendorType = cmbVendorType.Text;
            }
            else
            {
                VendorType = cmbVendorType.Text;
            }

            VendorBL vendorBL = new VendorBL();
            List<Vendor> vendorList = null;

            DataGridView dgv = sender as DataGridView;
            DataGridViewColumn col = dgv.Columns[e.ColumnIndex];
            if (col.Name == "VendorName")
            {
                DataGridViewColumn VendorName = dgv.Columns["VendorName"];
                string vendorName = string.Empty;
                if (VendorName.HeaderCell.SortGlyphDirection == SortOrder.None || VendorName.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    vendorList = vendorBL.Sorting(VendorType, "vendorName", active, "1");
                    dgvVendorInfo.DataSource = vendorList;

                    VendorName.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    vendorList = vendorBL.Sorting(VendorType, "vendorName", active);
                    dgvVendorInfo.DataSource = vendorList;
                    VendorName.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            if (col.Name == "VendorTypes")
            {
                DataGridViewColumn VendorTypes = dgv.Columns["VendorTypes"];
                string vendorTypes = string.Empty;
                if (VendorType == "All")
                {
                    if (VendorTypes.HeaderCell.SortGlyphDirection == SortOrder.None || VendorTypes.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                    {
                        vendorList = vendorBL.Sorting(VendorType, "vendorTypes", active, "1");
                        dgvVendorInfo.DataSource = vendorList;

                        VendorTypes.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                    }
                    else
                    {
                        vendorList = vendorBL.Sorting(VendorType, "vendorTypes", active);
                        dgvVendorInfo.DataSource = vendorList;
                        VendorTypes.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                    }
                }
            }
            else if (col.Name == "VendorMainContact")
            {
                DataGridViewColumn VendorMainName = dgv.Columns["VendorMainContact"];
                string vendorMainName = string.Empty;
                if (VendorMainName.HeaderCell.SortGlyphDirection == SortOrder.None || VendorMainName.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    vendorList = vendorBL.Sorting(VendorType, "vendorMainName", active, "1");
                    dgvVendorInfo.DataSource = vendorList;

                    VendorMainName.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    vendorList = vendorBL.Sorting(VendorType, "vendorMainName", active);
                    dgvVendorInfo.DataSource = vendorList;
                    VendorMainName.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "VendorEmail")
            {
                DataGridViewColumn VendorEmail = dgv.Columns["VendorEmail"];
                string vendorEmail = string.Empty;
                if (VendorEmail.HeaderCell.SortGlyphDirection == SortOrder.None || VendorEmail.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    vendorList = vendorBL.Sorting(VendorType, "vendorEmail", active, "1");
                    dgvVendorInfo.DataSource = vendorList;

                    VendorEmail.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    vendorList = vendorBL.Sorting(VendorType, "vendorEmail", active);
                    dgvVendorInfo.DataSource = vendorList;
                    VendorEmail.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "City")
            {
                DataGridViewColumn City = dgv.Columns["City"];
                string city = string.Empty;
                if (City.HeaderCell.SortGlyphDirection == SortOrder.None || City.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    vendorList = vendorBL.Sorting(VendorType, "city", active, "1");
                    dgvVendorInfo.DataSource = vendorList;

                    City.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    vendorList = vendorBL.Sorting(VendorType, "city", active);
                    dgvVendorInfo.DataSource = vendorList;
                    City.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "State")
            {
                DataGridViewColumn State = dgv.Columns["State"];
                string state = string.Empty;
                if (State.HeaderCell.SortGlyphDirection == SortOrder.None || State.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    vendorList = vendorBL.Sorting(VendorType, "state", active, "1");
                    dgvVendorInfo.DataSource = vendorList;

                    State.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    vendorList = vendorBL.Sorting(VendorType, "state", active);
                    dgvVendorInfo.DataSource = vendorList;
                    State.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "Status")
            {
                DataGridViewColumn IsActive = dgv.Columns["Status"];
                string isActive = string.Empty;
                if (IsActive.HeaderCell.SortGlyphDirection == SortOrder.None || IsActive.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    vendorList = vendorBL.Sorting(VendorType, "isActive", active, "1");
                    dgvVendorInfo.DataSource = vendorList;

                    IsActive.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    vendorList = vendorBL.Sorting(VendorType, "isActive", active);
                    dgvVendorInfo.DataSource = vendorList;
                    IsActive.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
        }
    }
}