using SurPath.Business.Master;
using SurPath.Entity.Master;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmDepartmentInfo : Form
    {
        private bool haveEditRights = false;

        #region Constructor

        public FrmDepartmentInfo()
        {
            InitializeComponent();
        }

        #endregion Constructor

        #region Event Methods

        private void FrmDepartmentInfo_Load(object sender, System.EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                InitializeControls();
                LoadDepartmentInfo(0);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Error occurred while loading the page.");
            }
        }

        private void FrmDepartmentInfo_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                Program.frmMain.frmDepartmentInfo = null;
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
                FrmDepartmentDetails frmDepartmentDetails = new FrmDepartmentDetails(Enum.OperationMode.New, 0);
                if (frmDepartmentDetails.ShowDialog() == DialogResult.OK)
                {
                    LoadDepartmentInfo(0);
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
                    if (dgvDepartmentInfo.SelectedRows.Count > 0)
                    {
                        int selectedIndex = dgvDepartmentInfo.SelectedRows[0].Index;
                        int departmentId = (int)dgvDepartmentInfo.SelectedRows[0].Cells["DepartmentId"].Value;

                        FrmDepartmentDetails frmDepartmentDetails = new FrmDepartmentDetails(Enum.OperationMode.Edit, departmentId);
                        if (frmDepartmentDetails.ShowDialog() == DialogResult.OK)
                        {
                            LoadDepartmentInfo(selectedIndex);
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
                if (dgvDepartmentInfo.Rows.Count > 0)
                {
                    if (MessageBox.Show("Are you sure? Do you want to archive the selected record?", "SurPath Drug Testing", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        int departmentId = (int)dgvDepartmentInfo.SelectedRows[0].Cells["DepartmentId"].Value;
                        DepartmentBL departmentBL = new DepartmentBL();
                        int returnvalue = departmentBL.Delete(departmentId, Program.currentUserName);
                        if (returnvalue == 0)
                        {
                            MessageBox.Show("You cannot delete this record,since this is used in another process.");
                            return;
                        }
                        else
                        {
                            LoadDepartmentInfo(0);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a Record.");
                    LoadDepartmentInfo(0);
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
                // LoadDepartmentInfo(0);
                DepartmentBL departmentBL = new DepartmentBL();
                List<Department> departmentList = null;

                Dictionary<string, string> searchParam = new Dictionary<string, string>();
                searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());
                departmentList = departmentBL.GetList(searchParam);

                dgvDepartmentInfo.DataSource = departmentList;

                if (dgvDepartmentInfo.Rows.Count > 0)
                {
                    //if (selectedIndex > dgvDepartmentInfo.Rows.Count - 1)
                    //{
                    //    selectedIndex = dgvDepartmentInfo.Rows.Count - 1;
                    //}
                    dgvDepartmentInfo.Rows[0].Selected = true;
                    dgvDepartmentInfo.Focus();
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
                    // LoadDepartmentInfo(0);
                    DepartmentBL departmentBL = new DepartmentBL();
                    List<Department> departmentList = null;

                    Dictionary<string, string> searchParam = new Dictionary<string, string>();
                    searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                    searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());
                    departmentList = departmentBL.GetList(searchParam);

                    dgvDepartmentInfo.DataSource = departmentList;

                    if (dgvDepartmentInfo.Rows.Count > 0)
                    {
                        //if (selectedIndex > dgvDepartmentInfo.Rows.Count - 1)
                        //{
                        //    selectedIndex = dgvDepartmentInfo.Rows.Count - 1;
                        //}
                        dgvDepartmentInfo.Rows[0].Selected = true;
                        dgvDepartmentInfo.Focus();
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

        private void dgvDepartmentInfo_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (e.RowIndex >= 0)
                {
                    int selectedIndex = e.RowIndex;
                    int departmentId = (int)dgvDepartmentInfo.Rows[e.RowIndex].Cells["DepartmentId"].Value;
                    if (haveEditRights)
                    {
                        FrmDepartmentDetails frmDepartmentDetails = new FrmDepartmentDetails(Enum.OperationMode.Edit, departmentId);
                        if (frmDepartmentDetails.ShowDialog() == DialogResult.OK)
                        {
                            LoadDepartmentInfo(selectedIndex);
                        }
                    }

                    if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                    {
                        //DEPARTMENT_VIEW
                        DataRow[] departmentView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DEPARTMENT_VIEW.ToDescriptionString() + "'");

                        if (departmentView.Length > 0)
                        {
                            FrmDepartmentDetails frmDepartmentDetails = new FrmDepartmentDetails(Enum.OperationMode.Edit, departmentId);
                            if (frmDepartmentDetails.ShowDialog() == DialogResult.OK)
                            {
                                LoadDepartmentInfo(selectedIndex);
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

        private void dgvDepartmentInfo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (e.KeyData == Keys.Enter)
                {
                    e.SuppressKeyPress = false;
                    if (dgvDepartmentInfo.SelectedRows.Count > 0)
                    {
                        int selectedIndex = dgvDepartmentInfo.SelectedRows[0].Index;
                        int departmentId = (int)dgvDepartmentInfo.SelectedRows[0].Cells["DepartmentId"].Value;
                        if (haveEditRights)
                        {
                            FrmDepartmentDetails frmDepartmentDetails = new FrmDepartmentDetails(Enum.OperationMode.Edit, departmentId);
                            if (frmDepartmentDetails.ShowDialog() == DialogResult.OK)
                            {
                                LoadDepartmentInfo(selectedIndex);
                            }
                        }

                        if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                        {
                            //DEPARTMENT_VIEW
                            DataRow[] departmentView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DEPARTMENT_VIEW.ToDescriptionString() + "'");

                            if (departmentView.Length > 0)
                            {
                                FrmDepartmentDetails frmDepartmentDetails = new FrmDepartmentDetails(Enum.OperationMode.Edit, departmentId);
                                if (frmDepartmentDetails.ShowDialog() == DialogResult.OK)
                                {
                                    LoadDepartmentInfo(selectedIndex);
                                }
                            }
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

        private void dgvDepartmentInfo_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvDepartmentInfo.Rows)
            {
                bool departmentStatus = Convert.ToBoolean(row.Cells["IsActive"].Value);
                if (departmentStatus.ToString() == "True")
                {
                    row.Cells["Status"].Value = "Active";
                }
                else
                {
                    row.Cells["Status"].Value = "Inactive";
                }
            }
        }

        private void chkIncludeInactive_CheckedChanged(object sender, EventArgs e)
        {
            LoadDepartmentInfo(0);
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                txtSearchKeyword.Text = string.Empty;
                LoadDepartmentInfo(0);
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
            try
            {
                dgvDepartmentInfo.AutoGenerateColumns = false;

                if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                {
                    //DEPARTMENT_ADD
                    DataRow[] departmentInfoAdd = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DEPARTMENT_ADD.ToDescriptionString() + "'");

                    if (departmentInfoAdd.Length > 0)
                    {
                        tsbNew.Visible = true;
                    }
                    else
                    {
                        tsbNew.Visible = false;
                    }

                    //DEPARTMENT_EDIT
                    DataRow[] departmentInfoEdit = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DEPARTMENT_EDIT.ToDescriptionString() + "'");

                    if (departmentInfoEdit.Length > 0)
                    {
                        tsbEdit.Visible = true;
                        haveEditRights = true;
                    }
                    else
                    {
                        tsbEdit.Visible = false;
                    }

                    //DEPARTMENT_ARCHIVE
                    DataRow[] departmentInfoArchive = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DEPARTMENT_ARCHIVE.ToDescriptionString() + "'");

                    if (departmentInfoArchive.Length > 0)
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

        private void LoadDepartmentInfo(int selectedIndex)
        {
            try
            {
                DepartmentBL departmentBL = new DepartmentBL();
                List<Department> departmentList = null;

                Dictionary<string, string> searchParam = new Dictionary<string, string>();
                searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());
                departmentList = departmentBL.GetList(searchParam);

                dgvDepartmentInfo.DataSource = departmentList;

                if (dgvDepartmentInfo.Rows.Count > 0)
                {
                    if (selectedIndex > dgvDepartmentInfo.Rows.Count - 1)
                    {
                        selectedIndex = dgvDepartmentInfo.Rows.Count - 1;
                    }
                    dgvDepartmentInfo.Rows[selectedIndex].Selected = true;
                    dgvDepartmentInfo.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SearchDepartmentInfo(string searchKeyword)
        {
            //
        }

        #endregion Private Methods

        private void dgvDepartmentInfo_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            bool active = false;
            if (chkIncludeInactive.Checked == true)
            {
                active = true;
            }

            DepartmentBL departmentBL = new DepartmentBL();
            List<Department> departmentList = null;

            DataGridView dgv = sender as DataGridView;
            DataGridViewColumn col = dgv.Columns[e.ColumnIndex];
            if (col.Name == "DepartmentName")
            {
                DataGridViewColumn DepartmentName = dgv.Columns["DepartmentName"];
                string departmentName = string.Empty;
                if (DepartmentName.HeaderCell.SortGlyphDirection == SortOrder.None || DepartmentName.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    departmentList = departmentBL.Sorting("departmentName", active, "1");
                    dgvDepartmentInfo.DataSource = departmentList;

                    DepartmentName.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    departmentList = departmentBL.Sorting("departmentName", active);
                    dgvDepartmentInfo.DataSource = departmentList;
                    DepartmentName.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "Status")
            {
                DataGridViewColumn IsActive = dgv.Columns["IsActive"];
                string isActive = string.Empty;
                if (IsActive.HeaderCell.SortGlyphDirection == SortOrder.None || IsActive.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    departmentList = departmentBL.Sorting("isActive", active, "1");
                    dgvDepartmentInfo.DataSource = departmentList;

                    IsActive.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    departmentList = departmentBL.Sorting("isActive", active);
                    dgvDepartmentInfo.DataSource = departmentList;
                    IsActive.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
        }
    }
}