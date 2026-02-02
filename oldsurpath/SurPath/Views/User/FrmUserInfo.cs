using SurPath.Business;
using SurPath.Entity;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmUserInfo : Form
    {
        private bool haveEditRights = false;

        private User user = new User();

        public bool SkipSearchOnLoad = false;
        #region Constructor

        public FrmUserInfo()
        {
            InitializeComponent();
        }

        #endregion Constructor

        #region Event Methods

        private void FrmUserInfo_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeControls();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while loading the page.");
            }
        }

        private void FrmUserInfo_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.frmMain.frmUserInfo = null;
        }

        private void tsbNew_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                FrmUserDetails frmUserDetails = new FrmUserDetails(Enum.OperationMode.New, 0);
                if (frmUserDetails.ShowDialog() == DialogResult.OK)
                {
                    LoadUserInfo(0);
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
                    if (dgvUserInfo.SelectedRows.Count > 0)
                    {
                        int selectedIndex = dgvUserInfo.SelectedRows[0].Index;
                        int userId = (int)dgvUserInfo.SelectedRows[0].Cells["UserId"].Value;

                        string userName = (string)dgvUserInfo.SelectedRows[0].Cells["UserName"].Value;
                        bool isAdmin = false;
                        bool canView = false;

                        if ((Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() ||
                            Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() ||
                            Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                        {
                            isAdmin = true;
                            canView = true;
                        }
                        else
                        {
                            if (((Program.currentUserName.ToUpper() == userName.ToUpper() && Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() ||
                                Program.currentUserName.ToUpper() == userName.ToUpper() && Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() ||
                                Program.currentUserName.ToUpper() == userName.ToUpper() && Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper())))
                            {
                                isAdmin = true;
                                canView = true;
                            }
                            if (isAdmin == false)
                            {
                                if (Program.currentUserName.ToUpper() == userName.ToUpper())
                                {
                                    isAdmin = false;
                                    canView = false;
                                }
                                else
                                {
                                    if (((userName.ToUpper() == Program.superAdmin.ToUpper() ||
                                userName.ToUpper() == Program.superAdmin1.ToUpper() ||
                                userName.ToUpper() == Program.superAdmin2.ToUpper())))
                                    {
                                        isAdmin = true;
                                        canView = false;
                                    }
                                    else
                                    {
                                        isAdmin = false;
                                        canView = true;
                                    }
                                }
                            }
                        }
                        if (canView == false)
                        {
                            MessageBox.Show("You cannot edit this user details.");
                            return;
                        }
                        //if ((userId == 1 && !(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper())) || (Program.currentUserId == userId && userId != 1))
                        //{
                        //    MessageBox.Show("You cannot edit this user details.");
                        //    return;
                        //}

                        FrmUserDetails frmUserDetails = new FrmUserDetails(Enum.OperationMode.Edit, userId);
                        if (frmUserDetails.ShowDialog() == DialogResult.OK)
                        {
                            LoadUserInfo(selectedIndex);
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
                int userId = (int)dgvUserInfo.SelectedRows[0].Cells["UserId"].Value;
                string userName = (string)dgvUserInfo.SelectedRows[0].Cells["UserName"].Value;
                // if (userId == 1)
                if (userName.ToUpper() == Program.superAdmin.ToUpper() || userName.ToUpper() == Program.superAdmin1.ToUpper() || userName.ToUpper() == Program.superAdmin2.ToUpper())
                {
                    Cursor.Current = Cursors.WaitCursor;
                    MessageBox.Show("You cannot archive this user details.");
                    return;
                }
                if (dgvUserInfo.SelectedRows.Count > 0)
                {
                    if (MessageBox.Show("Are you sure? Do you want to archive the selected record?", "SurPath Drug Testing", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        UserBL userBL = new UserBL();
                        int returnvalue = userBL.Delete(userId, Program.currentUserName);
                        //if (returnvalue == 0)
                        //{
                        //    MessageBox.Show("Can't Delete");
                        //    return;
                        //}
                        //else
                        //{
                        LoadUserInfo(0);
                        //}
                    }
                }
                else
                {
                    MessageBox.Show("Please select a Record.");
                    LoadUserInfo(0);
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
                //    LoadUserInfo(0);
                UserBL userBL = new UserBL();
                List<User> userList = null;

                Dictionary<string, string> searchParam = new Dictionary<string, string>();
                searchParam.Add("UserType", cmbUserType.Text);
                searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());

                userList = userBL.GetList(searchParam);

                dgvUserInfo.DataSource = userList;

                if (dgvUserInfo.Rows.Count > 0)
                {
                    //if (selectedIndex > dgvUserInfo.Rows.Count - 1)
                    //{
                    //    selectedIndex = dgvUserInfo.Rows.Count - 1;
                    //}
                    dgvUserInfo.Rows[0].Selected = true;
                    dgvUserInfo.Focus();
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
                    //  LoadUserInfo(0);
                    UserBL userBL = new UserBL();
                    List<User> userList = null;

                    Dictionary<string, string> searchParam = new Dictionary<string, string>();
                    searchParam.Add("UserType", cmbUserType.Text);
                    searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                    searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());

                    userList = userBL.GetList(searchParam);

                    dgvUserInfo.DataSource = userList;

                    if (dgvUserInfo.Rows.Count > 0)
                    {
                        //if (selectedIndex > dgvUserInfo.Rows.Count - 1)
                        //{
                        //    selectedIndex = dgvUserInfo.Rows.Count - 1;
                        //}
                        dgvUserInfo.Rows[0].Selected = true;
                        dgvUserInfo.Focus();
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

        private void dgvUserInfo_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (e.RowIndex >= 0)
                {
                    int selectedIndex = e.RowIndex;
                    int userId = (int)dgvUserInfo.Rows[e.RowIndex].Cells["UserId"].Value;
                    string userName = (string)dgvUserInfo.Rows[e.RowIndex].Cells["UserName"].Value;
                    bool isAdmin = false;
                    bool canView = false;

                    if ((Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() ||
                        Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() ||
                        Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                    {
                        isAdmin = true;
                        canView = true;
                    }
                    else
                    {
                        if (((Program.currentUserName.ToUpper() == userName.ToUpper() && Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() ||
                            Program.currentUserName.ToUpper() == userName.ToUpper() && Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() ||
                            Program.currentUserName.ToUpper() == userName.ToUpper() && Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper())))
                        {
                            isAdmin = true;
                            canView = true;
                        }
                        if (isAdmin == false)
                        {
                            if (Program.currentUserName.ToUpper() == userName.ToUpper())
                            {
                                isAdmin = false;
                                canView = false;
                            }
                            else
                            {
                                if (((userName.ToUpper() == Program.superAdmin.ToUpper() ||
                            userName.ToUpper() == Program.superAdmin1.ToUpper() ||
                            userName.ToUpper() == Program.superAdmin2.ToUpper())))
                                {
                                    isAdmin = true;
                                    canView = false;
                                }
                                else
                                {
                                    isAdmin = false;
                                    canView = true;
                                }
                            }
                        }
                    }
                    if (canView == false)
                    {
                        MessageBox.Show("You cannot edit this user details.");
                        return;
                    }
                    if (haveEditRights)
                    {
                        FrmUserDetails frmUserDetails = new FrmUserDetails(Enum.OperationMode.Edit, userId);
                        if (frmUserDetails.ShowDialog() == DialogResult.OK)
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            LoadUserInfo(selectedIndex);
                        }
                    }

                    if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                    {
                        //USER_VIEW
                        DataRow[] userView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.USER_VIEW.ToDescriptionString() + "'");

                        if (userView.Length > 0)
                        {
                            FrmUserDetails frmUserDetails = new FrmUserDetails(Enum.OperationMode.Edit, userId);
                            if (frmUserDetails.ShowDialog() == DialogResult.OK)
                            {
                                Cursor.Current = Cursors.WaitCursor;
                                LoadUserInfo(selectedIndex);
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

        private void dgvUserInfo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.Enter)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    e.SuppressKeyPress = false;
                    if (dgvUserInfo.SelectedRows.Count > 0)
                    {
                        int selectedIndex = dgvUserInfo.SelectedRows[0].Index;
                        int userId = (int)dgvUserInfo.SelectedRows[0].Cells["UserId"].Value;

                        //if ((userId == 1 && !(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper())) || (Program.currentUserId == userId && userId != 1))
                        //{
                        //    MessageBox.Show("You cannot edit this user details.");
                        //    return;
                        //}

                        string userName = (string)dgvUserInfo.SelectedRows[0].Cells["UserName"].Value;
                        bool isAdmin = false;
                        bool canView = false;

                        if ((Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() ||
                            Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() ||
                            Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                        {
                            isAdmin = true;
                            canView = true;
                        }
                        else
                        {
                            if (((Program.currentUserName.ToUpper() == userName.ToUpper() && Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() ||
                                Program.currentUserName.ToUpper() == userName.ToUpper() && Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() ||
                                Program.currentUserName.ToUpper() == userName.ToUpper() && Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper())))
                            {
                                isAdmin = true;
                                canView = true;
                            }
                            if (isAdmin == false)
                            {
                                if (Program.currentUserName.ToUpper() == userName.ToUpper())
                                {
                                    isAdmin = false;
                                    canView = false;
                                }
                                else
                                {
                                    if (((userName.ToUpper() == Program.superAdmin.ToUpper() ||
                                userName.ToUpper() == Program.superAdmin1.ToUpper() ||
                                userName.ToUpper() == Program.superAdmin2.ToUpper())))
                                    {
                                        isAdmin = true;
                                        canView = false;
                                    }
                                    else
                                    {
                                        isAdmin = false;
                                        canView = true;
                                    }
                                }
                            }
                        }
                        if (canView == false)
                        {
                            MessageBox.Show("You cannot edit this user details.");
                            return;
                        }

                        if (haveEditRights)
                        {
                            FrmUserDetails frmUserDetails = new FrmUserDetails(Enum.OperationMode.Edit, userId);
                            if (frmUserDetails.ShowDialog() == DialogResult.OK)
                            {
                                Cursor.Current = Cursors.WaitCursor;
                                LoadUserInfo(selectedIndex);
                            }
                        }

                        if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                        {
                            //USER_VIEW
                            DataRow[] userView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.USER_VIEW.ToDescriptionString() + "'");

                            if (userView.Length > 0)
                            {
                                FrmUserDetails frmUserDetails = new FrmUserDetails(Enum.OperationMode.Edit, userId);
                                if (frmUserDetails.ShowDialog() == DialogResult.OK)
                                {
                                    Cursor.Current = Cursors.WaitCursor;
                                    LoadUserInfo(selectedIndex);
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

        private void dgvUserInfo_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvUserInfo.Rows)
            {
                bool userStatus = Convert.ToBoolean(row.Cells["IsUserActive"].Value);
                if (userStatus.ToString() == "True")
                {
                    row.Cells["IsActive"].Value = "Active";
                }
                else
                {
                    row.Cells["IsActive"].Value = "Inactive";
                }
            }
            foreach (DataGridViewColumn column in dgvUserInfo.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Programmatic;
            }
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                txtSearchKeyword.Text = string.Empty;
                LoadUserInfo(0);
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
                LoadUserInfo(0);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbUserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                LoadUserInfo(0);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvUserInfo_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            bool active = false;
            if (chkIncludeInactive.Checked == true)
            {
                active = true;
            }

            string user = cmbUserType.Text;

            UserBL userBL = new UserBL();
            List<User> userList = null;
            DataGridView dgv = sender as DataGridView;
            DataGridViewColumn col = dgv.Columns[e.ColumnIndex];
            if (col.Name == "FirstName")
            {
                DataGridViewColumn FirstName = dgv.Columns["FirstName"];
                string firstName = string.Empty;
                if (FirstName.HeaderCell.SortGlyphDirection == SortOrder.None || FirstName.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    userList = userBL.Sorting("firstName", user, active, "1");
                    dgvUserInfo.DataSource = userList;

                    FirstName.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                    // col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
                else
                {
                    userList = userBL.Sorting("firstName", user, active);
                    dgvUserInfo.DataSource = userList;
                    FirstName.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                    // col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
            }
            else if (col.Name == "LastName")
            {
                DataGridViewColumn LastName = dgv.Columns["LastName"];
                string lastName = string.Empty;
                if (LastName.HeaderCell.SortGlyphDirection == SortOrder.None || LastName.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    userList = userBL.Sorting("lastName", user, active, "1");
                    dgvUserInfo.DataSource = userList;

                    LastName.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                    //col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
                else
                {
                    userList = userBL.Sorting("lastName", user, active);
                    dgvUserInfo.DataSource = userList;
                    LastName.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                    //col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
            }
            else if (col.Name == "UserName")
            {
                DataGridViewColumn UserName = dgv.Columns["UserName"];
                string userName = string.Empty;
                if (UserName.HeaderCell.SortGlyphDirection == SortOrder.None || UserName.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    userList = userBL.Sorting("userName", user, active, "1");
                    dgvUserInfo.DataSource = userList;

                    UserName.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                    // col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
                else
                {
                    userList = userBL.Sorting("userName", user, active);
                    dgvUserInfo.DataSource = userList;
                    UserName.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                    // col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
            }
            else if (col.Name == "UserType")
            {
                DataGridViewColumn UserType = dgv.Columns["UserType"];
                string userType = string.Empty;
                if (user == "All")
                {
                    if (UserType.HeaderCell.SortGlyphDirection == SortOrder.None || UserType.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                    {
                        userList = userBL.Sorting("userType", user, active, "1");
                        dgvUserInfo.DataSource = userList;

                        UserType.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                        //col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                    }
                    else
                    {
                        userList = userBL.Sorting("userType", user, active);
                        dgvUserInfo.DataSource = userList;
                        UserType.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                        //col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                    }
                }
            }

            //else if (col.Name == "UserTypesNames")
            //{
            //    DataGridViewColumn UserTypesNames = dgv.Columns["UserTypesNames"];
            //    string userTypeNames = string.Empty;
            //    if (UserTypesNames.HeaderCell.SortGlyphDirection == SortOrder.None || UserTypesNames.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
            //    {
            //        userList = userBL.Sorting("userTypeNames", user , active, "1");
            //        dgvUserInfo.DataSource = userList;

            //        UserTypesNames.HeaderCell.SortGlyphDirection = SortOrder.Descending;
            //        // col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
            //    }
            //    else
            //    {
            //        userList = userBL.Sorting("userTypesNames", user , active);
            //        dgvUserInfo.DataSource = userList;
            //        UserTypesNames.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
            //        // col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
            //    }
            //}
            else if (col.Name == "IsActive")
            {
                DataGridViewColumn IsActive = dgv.Columns["IsActive"];
                string isActive = string.Empty;

                if (IsActive.HeaderCell.SortGlyphDirection == SortOrder.None || IsActive.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    userList = userBL.Sorting("isActive", user, active, "1");
                    dgvUserInfo.DataSource = userList;

                    IsActive.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                    //  col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
                else
                {
                    userList = userBL.Sorting("isActive", user, active);
                    dgvUserInfo.DataSource = userList;
                    IsActive.HeaderCell.SortGlyphDirection = SortOrder.Ascending; ;
                    //   col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
            }
        }

        #endregion Event Methods

        #region Private Methods

        private void InitializeControls()
        {
            dgvUserInfo.AutoGenerateColumns = false;
            cmbUserType.SelectedIndex = 0;
            chkIncludeInactive.Checked = false;

            if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
            {
                //USER_ADD
                DataRow[] userInfoAdd = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.USER_ADD.ToDescriptionString() + "'");

                if (userInfoAdd.Length > 0)
                {
                    tsbNew.Visible = true;
                }
                else
                {
                    tsbNew.Visible = false;
                }

                //USER_EDIT
                DataRow[] userInfoEdit = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.USER_EDIT.ToDescriptionString() + "'");

                if (userInfoEdit.Length > 0)
                {
                    tsbEdit.Visible = true;
                    haveEditRights = true;
                }
                else
                {
                    tsbEdit.Visible = false;
                }

                //USER_ARCHIVE
                DataRow[] userInfoArchive = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.USER_ARCHIVE.ToDescriptionString() + "'");

                if (userInfoArchive.Length > 0)
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

        public void LoadUserInfo(int selectedIndex)
        {
            try
            {
                if (this.SkipSearchOnLoad==false)
                {
                    UserBL userBL = new UserBL();
                    List<User> userList = null;

                    Dictionary<string, string> searchParam = new Dictionary<string, string>();
                    searchParam.Add("UserType", cmbUserType.Text);
                    searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                    searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());

                    userList = userBL.GetList(searchParam);

                    dgvUserInfo.DataSource = userList;

                    if (dgvUserInfo.Rows.Count > 0)
                    {
                        if (selectedIndex > dgvUserInfo.Rows.Count - 1)
                        {
                            selectedIndex = dgvUserInfo.Rows.Count - 1;
                        }
                        dgvUserInfo.Rows[selectedIndex].Selected = true;
                        dgvUserInfo.Focus();
                    } 
                }
                else
                {
                    this.SkipSearchOnLoad = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion Private Methods
    }
}