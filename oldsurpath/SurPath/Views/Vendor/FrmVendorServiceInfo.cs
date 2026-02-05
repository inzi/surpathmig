using SurPath.Business;
using SurPath.Business.Master;
using SurPath.Entity;
using SurPath.Entity.Master;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmVendorServiceInfo : Form
    {
        private bool haveEditRights = false;

        #region Private Variables

        private int _vendorId;

        #endregion Private Variables

        #region Constructor

        public FrmVendorServiceInfo()
        {
            InitializeComponent();
        }

        public FrmVendorServiceInfo(int vendorId)
        {
            InitializeComponent();

            this._vendorId = vendorId;
        }

        #endregion Constructor

        #region Event Methods

        private void FrmVendorServiceInfo_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeControls();
                LoadVendorServiceInfo(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while loading the page.");
            }
        }

        private void FrmVendorServiceInfo_FormClosed(object sender, FormClosedEventArgs e)
        {
            //
        }

        private void tsbNew_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                FrmVendorServiceDetails frmVendorServiceDetails = new FrmVendorServiceDetails(Enum.OperationMode.New, this._vendorId, 0);
                if (frmVendorServiceDetails.ShowDialog() == DialogResult.OK)
                {
                    LoadVendorServiceInfo(0);
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
                    if (dgvVendorService.SelectedRows.Count > 0)
                    {
                        int selectedIndex = dgvVendorService.SelectedRows[0].Index;
                        int vendorServiceId = (int)dgvVendorService.SelectedRows[0].Cells["VendorServiceId"].Value;

                        FrmVendorServiceDetails frmVendorServiceDetails = new FrmVendorServiceDetails(Enum.OperationMode.Edit, this._vendorId, vendorServiceId);
                        if (frmVendorServiceDetails.ShowDialog() == DialogResult.OK)
                        {
                            LoadVendorServiceInfo(selectedIndex);
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
                if (dgvVendorService.SelectedRows.Count > 0)
                {
                    if (MessageBox.Show("Are you sure? Do you want to archive the selected record?", "SurPath Drug Testing", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        int vendorServiceId = (int)dgvVendorService.SelectedRows[0].Cells["VendorServiceId"].Value;
                        VendorServiceBL vendorServiceBL = new VendorServiceBL();
                        int returnvalue = vendorServiceBL.Delete(vendorServiceId, Program.currentUserName);
                        //if (returnvalue == 0)
                        //{
                        //    MessageBox.Show("You can not delete this record,since this is used in other process.");
                        //    return;
                        //}
                        //else
                        //{
                        LoadVendorServiceInfo(0);
                        // }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a Record.");
                    LoadVendorServiceInfo(0);
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
                //  LoadVendorServiceInfo(0);
                VendorServiceBL vendorServiceBL = new VendorServiceBL();
                List<VendorService> vendorServiceList = null;

                Dictionary<string, string> searchParam = new Dictionary<string, string>();
                searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());
                vendorServiceList = vendorServiceBL.GetList(this._vendorId, searchParam);

                dgvVendorService.DataSource = vendorServiceList;

                if (dgvVendorService.Rows.Count > 0)
                {
                    //if (selectedIndex > dgvVendorService.Rows.Count - 1)
                    //{
                    //    selectedIndex = dgvVendorService.Rows.Count - 1;
                    //  }
                    dgvVendorService.Rows[0].Selected = true;
                    dgvVendorService.Focus();
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
                    //   LoadVendorServiceInfo(0);
                    VendorServiceBL vendorServiceBL = new VendorServiceBL();
                    List<VendorService> vendorServiceList = null;

                    Dictionary<string, string> searchParam = new Dictionary<string, string>();
                    searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                    searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());
                    vendorServiceList = vendorServiceBL.GetList(this._vendorId, searchParam);

                    dgvVendorService.DataSource = vendorServiceList;

                    if (dgvVendorService.Rows.Count > 0)
                    {
                        //if (selectedIndex > dgvVendorService.Rows.Count - 1)
                        //{
                        //    selectedIndex = dgvVendorService.Rows.Count - 1;
                        //  }
                        dgvVendorService.Rows[0].Selected = true;
                        dgvVendorService.Focus();
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

        private void dgvVendorService_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (e.RowIndex >= 0)
                {
                    int selectedIndex = e.RowIndex;
                    int vendorServiceId = (int)dgvVendorService.Rows[e.RowIndex].Cells["VendorServiceId"].Value;
                    if (haveEditRights)
                    {
                        FrmVendorServiceDetails frmVendorServiceDetails = new FrmVendorServiceDetails(Enum.OperationMode.Edit, this._vendorId, vendorServiceId);
                        if (frmVendorServiceDetails.ShowDialog() == DialogResult.OK)
                        {
                            LoadVendorServiceInfo(selectedIndex);
                        }
                    }

                    if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                    {
                        //VENDOR_VENDOR_SERVICE_VIEW
                        DataRow[] vendorServiceView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.VENDOR_VENDOR_SERVICE_VIEW.ToDescriptionString() + "'");

                        if (vendorServiceView.Length > 0)
                        {
                            FrmVendorServiceDetails frmVendorServiceDetails = new FrmVendorServiceDetails(Enum.OperationMode.Edit, this._vendorId, vendorServiceId);
                            if (frmVendorServiceDetails.ShowDialog() == DialogResult.OK)
                            {
                                LoadVendorServiceInfo(selectedIndex);
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

        private void dgvVendorService_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.Enter)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    e.SuppressKeyPress = false;
                    if (dgvVendorService.SelectedRows.Count > 0)
                    {
                        int selectedIndex = dgvVendorService.SelectedRows[0].Index;
                        int vendorServiceId = (int)dgvVendorService.SelectedRows[0].Cells["VendorServiceId"].Value;
                        if (haveEditRights)
                        {
                            FrmVendorServiceDetails frmVendorServiceDetails = new FrmVendorServiceDetails(Enum.OperationMode.Edit, this._vendorId, vendorServiceId);
                            if (frmVendorServiceDetails.ShowDialog() == DialogResult.OK)
                            {
                                LoadVendorServiceInfo(selectedIndex);
                            }
                        }

                        if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                        {
                            //VENDOR_VENDOR_SERVICE_VIEW
                            DataRow[] vendorServiceView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.VENDOR_VENDOR_SERVICE_VIEW.ToDescriptionString() + "'");

                            if (vendorServiceView.Length > 0)
                            {
                                FrmVendorServiceDetails frmVendorServiceDetails = new FrmVendorServiceDetails(Enum.OperationMode.Edit, this._vendorId, vendorServiceId);
                                if (frmVendorServiceDetails.ShowDialog() == DialogResult.OK)
                                {
                                    LoadVendorServiceInfo(selectedIndex);
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

        private void dgvVendorService_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvVendorService.Rows)
            {
                TestCategories testCategory = (TestCategories)((int)row.Cells["TestCategoryId"].Value);
                row.Cells["TestCategory"].Value = testCategory.ToDescriptionString();

                YesNo yesNo = (YesNo)((int)row.Cells["ObservedType"].Value);
                if (yesNo.ToString() == "Yes")
                {
                    row.Cells["IsObserved"].Value = "Observed";
                }
                else
                {
                    row.Cells["IsObserved"].Value = "Unobserved";
                }

                SpecimenFormType specimenFormType = (SpecimenFormType)((int)row.Cells["FormTypeId"].Value);
                if (specimenFormType.ToString() == "Federal")
                {
                    row.Cells["FormType"].Value = "Federal";
                }
                else
                {
                    row.Cells["FormType"].Value = "Non Federal";
                }

                bool vendorServiceStatus = Convert.ToBoolean(row.Cells["IsActive"].Value);
                if (vendorServiceStatus.ToString() == "True")
                {
                    row.Cells["Status"].Value = "Active";
                }
                else
                {
                    row.Cells["Status"].Value = "Inactive";
                }
            }
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                txtSearchKeyword.Text = string.Empty;
                LoadVendorServiceInfo(0);
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
                LoadVendorServiceInfo(0);
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

        private void InitializeControls()
        {
            dgvVendorService.AutoGenerateColumns = false;

            if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
            {
                //VENDOR_VENDOR_SERVICE_ADD
                DataRow[] vendorServiceInfoAdd = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.VENDOR_VENDOR_SERVICE_ADD.ToDescriptionString() + "'");

                if (vendorServiceInfoAdd.Length > 0)
                {
                    tsbNew.Visible = true;
                }
                else
                {
                    tsbNew.Visible = false;
                }

                //VENDOR_VENDOR_SERVICE_EDIT
                DataRow[] vendorServiceInfoEdit = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.VENDOR_VENDOR_SERVICE_EDIT.ToDescriptionString() + "'");

                if (vendorServiceInfoEdit.Length > 0)
                {
                    tsbEdit.Visible = true;
                    haveEditRights = true;
                }
                else
                {
                    tsbEdit.Visible = false;
                }

                //VENDOR_VENDOR_SERVICE_ARCHIVE
                DataRow[] vendorServiceInfoArchive = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.VENDOR_VENDOR_SERVICE_ARCHIVE.ToDescriptionString() + "'");

                if (vendorServiceInfoArchive.Length > 0)
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

            if (this._vendorId != 0)
            {
                VendorBL vendorBL = new VendorBL();
                Vendor vendor = vendorBL.Get(this._vendorId);

                if (vendor != null)
                {
                    lblVendorName.Text = vendor.VendorName;
                }
            }
            else
            {
                lblVendorName.Text = string.Empty;
            }
        }

        private void LoadVendorServiceInfo(int selectedIndex)
        {
            VendorServiceBL vendorServiceBL = new VendorServiceBL();
            List<VendorService> vendorServiceList = null;

            Dictionary<string, string> searchParam = new Dictionary<string, string>();
            searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
            searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());
            vendorServiceList = vendorServiceBL.GetList(this._vendorId, searchParam);

            dgvVendorService.DataSource = vendorServiceList;

            if (dgvVendorService.Rows.Count > 0)
            {
                if (selectedIndex > dgvVendorService.Rows.Count - 1)
                {
                    selectedIndex = dgvVendorService.Rows.Count - 1;
                }
                dgvVendorService.Rows[selectedIndex].Selected = true;
                dgvVendorService.Focus();
            }
        }

        #endregion Private Methods

        private void dgvVendorService_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            bool active = false;
            if (chkIncludeInactive.Checked == true)
            {
                active = true;
            }

            VendorServiceBL vendorServiceBL = new VendorServiceBL();
            List<VendorService> vendorServiceList = null;

            DataGridView dgv = sender as DataGridView;
            DataGridViewColumn col = dgv.Columns[e.ColumnIndex];

            if (col.Name == "VendorServiceName")
            {
                DataGridViewColumn VendorServiceName = dgv.Columns["VendorServiceName"];
                string vendorServiceName = string.Empty;
                if (VendorServiceName.HeaderCell.SortGlyphDirection == SortOrder.None || VendorServiceName.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    vendorServiceList = vendorServiceBL.Sorting(this._vendorId, "vendorServiceName", active, "1");
                    dgvVendorService.DataSource = vendorServiceList;

                    VendorServiceName.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    vendorServiceList = vendorServiceBL.Sorting(this._vendorId, "vendorServiceName", active);
                    dgvVendorService.DataSource = vendorServiceList;
                    VendorServiceName.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "TestCategory")
            {
                DataGridViewColumn TestCategory = dgv.Columns["TestCategory"];
                string testCategory = string.Empty;
                if (TestCategory.HeaderCell.SortGlyphDirection == SortOrder.None || TestCategory.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    vendorServiceList = vendorServiceBL.Sorting(this._vendorId, "testCategory", active, "1");
                    dgvVendorService.DataSource = vendorServiceList;

                    TestCategory.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    vendorServiceList = vendorServiceBL.Sorting(this._vendorId, "testCategory", active);
                    dgvVendorService.DataSource = vendorServiceList;
                    TestCategory.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "IsObserved")
            {
                DataGridViewColumn IsObserved = dgv.Columns["IsObserved"];
                string isObserved = string.Empty;
                if (IsObserved.HeaderCell.SortGlyphDirection == SortOrder.None || IsObserved.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    vendorServiceList = vendorServiceBL.Sorting(this._vendorId, "isObserved", active, "1");
                    dgvVendorService.DataSource = vendorServiceList;

                    IsObserved.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    vendorServiceList = vendorServiceBL.Sorting(this._vendorId, "isObserved", active);
                    dgvVendorService.DataSource = vendorServiceList;
                    IsObserved.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "FormType")
            {
                DataGridViewColumn FormType = dgv.Columns["FormType"];
                string formType = string.Empty;
                if (FormType.HeaderCell.SortGlyphDirection == SortOrder.None || FormType.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    vendorServiceList = vendorServiceBL.Sorting(this._vendorId, "formType", active, "1");
                    dgvVendorService.DataSource = vendorServiceList;

                    FormType.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    vendorServiceList = vendorServiceBL.Sorting(this._vendorId, "formType", active);
                    dgvVendorService.DataSource = vendorServiceList;
                    FormType.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "Status")
            {
                DataGridViewColumn IsActive = dgv.Columns["Status"];
                string isActive = string.Empty;
                if (IsActive.HeaderCell.SortGlyphDirection == SortOrder.None || IsActive.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    vendorServiceList = vendorServiceBL.Sorting(this._vendorId, "isActive", active, "1");
                    dgvVendorService.DataSource = vendorServiceList;

                    IsActive.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    vendorServiceList = vendorServiceBL.Sorting(this._vendorId, "isActive", active);
                    dgvVendorService.DataSource = vendorServiceList;
                    IsActive.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
        }

        //#region Public Properties

        //public int VendorId
        //{
        //    get
        //    {
        //        return this._vendorId;
        //    }
        //    set
        //    {
        //        this._vendorId = value;
        //    }
        //}

        //#endregion
    }
}