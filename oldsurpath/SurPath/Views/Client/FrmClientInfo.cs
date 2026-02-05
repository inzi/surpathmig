using SurPath.Business;
using SurPath.Entity;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Serilog;

namespace SurPath
{
    public partial class FrmClientInfo : Form
    {
        private bool haveEditRights = false;
        static ILogger _logger = Program._logger;

        #region Constructor

        public FrmClientInfo()
        {
            InitializeComponent();
        }

        #endregion Constructor

        #region Event Methods

        private void FrmClientInfo_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                InitializeControls();
                LoadClientInfo(0);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Error occurred while loading the page.");
            }
        }

        private void FrmClientInfo_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                Program.frmMain.frmClientInfo = null;
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
                FrmClientDetails frmclientDetails = new FrmClientDetails(Enum.OperationMode.New, 0);
                if (frmclientDetails.ShowDialog() == DialogResult.OK)
                {
                    LoadClientInfo(0);
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
                    if (dgvClientInfo.SelectedRows.Count > 0)
                    {
                        int selectedIndex = dgvClientInfo.SelectedRows[0].Index;
                        int clientId = (int)dgvClientInfo.SelectedRows[0].Cells["ClientId"].Value;

                        FrmClientDetails frmclientDetails = new FrmClientDetails(Enum.OperationMode.Edit, clientId);
                        if (frmclientDetails.ShowDialog() == DialogResult.OK)
                        {
                            LoadClientInfo(selectedIndex);
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
                if (dgvClientInfo.Rows.Count > 0)
                {
                    if (MessageBox.Show("Are you sure? Do you want to archive the selected record?", "SurPath Drug Testing", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        int clientId = (int)dgvClientInfo.SelectedRows[0].Cells["ClientId"].Value;
                        ClientBL clientBL = new ClientBL();
                        int returnvalue = clientBL.Delete(clientId, Program.currentUserName);
                        if (returnvalue == 0)
                        {
                            MessageBox.Show("You cannot delete this record,since this is used in another process.");
                            return;
                        }
                        else
                        {
                            LoadClientInfo(0);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a Record.");
                    LoadClientInfo(0);
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
                //   LoadClientInfo(0);
                ClientBL clientBL = new ClientBL();
                List<Client> clientList = null;

                Dictionary<string, string> searchParam = new Dictionary<string, string>();

                searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());

                clientList = clientBL.GetList(searchParam);

                dgvClientInfo.DataSource = clientList;

                if (dgvClientInfo.Rows.Count > 0)
                {
                    //if (selectedIndex > dgvClientInfo.Rows.Count - 1)
                    //{
                    //    selectedIndex = dgvClientInfo.Rows.Count - 1;
                    //}
                    dgvClientInfo.Rows[0].Selected = true;
                    dgvClientInfo.Focus();
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
                    // LoadClientInfo(0);

                    ClientBL clientBL = new ClientBL();
                    List<Client> clientList = null;

                    Dictionary<string, string> searchParam = new Dictionary<string, string>();

                    searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                    searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());

                    clientList = clientBL.GetList(searchParam);

                    dgvClientInfo.DataSource = clientList;

                    if (dgvClientInfo.Rows.Count > 0)
                    {
                        //if (selectedIndex > dgvClientInfo.Rows.Count - 1)
                        //{
                        //    selectedIndex = dgvClientInfo.Rows.Count - 1;
                        //}
                        dgvClientInfo.Rows[0].Selected = true;
                        dgvClientInfo.Focus();
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

        private void dgvClientInfo_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (e.RowIndex >= 0)
                {
                    int selectedIndex = e.RowIndex;
                    int clientId = (int)dgvClientInfo.Rows[e.RowIndex].Cells["ClientId"].Value;

                    if (haveEditRights)
                    {
                        FrmClientDetails frmClientDetails = new FrmClientDetails(OperationMode.Edit, clientId);
                        if (frmClientDetails.ShowDialog() == DialogResult.OK)
                        {
                            LoadClientInfo(selectedIndex);
                        }
                    }

                    if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                    {
                        //CLIENT_VIEW
                        DataRow[] clientView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.CLIENT_VIEW.ToDescriptionString() + "'");

                        if (clientView.Length > 0)
                        {
                            FrmClientDetails frmClientDetails = new FrmClientDetails(OperationMode.Edit, clientId);
                            if (frmClientDetails.ShowDialog() == DialogResult.OK)
                            {
                                LoadClientInfo(selectedIndex);
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

        private void dgvClientInfo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (e.KeyData == Keys.Enter)
                {
                    e.SuppressKeyPress = false;
                    if (dgvClientInfo.SelectedRows.Count > 0)
                    {
                        int selectedIndex = dgvClientInfo.SelectedRows[0].Index;
                        int clientId = (int)dgvClientInfo.SelectedRows[0].Cells["ClientId"].Value;

                        if (haveEditRights)
                        {
                            FrmClientDetails frmClientDetails = new FrmClientDetails(OperationMode.Edit, clientId);
                            if (frmClientDetails.ShowDialog() == DialogResult.OK)
                            {
                                LoadClientInfo(selectedIndex);
                            }
                        }
                        if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                        {
                            //CLIENT_VIEW
                            DataRow[] clientView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.CLIENT_VIEW.ToDescriptionString() + "'");

                            if (clientView.Length > 0)
                            {
                                FrmClientDetails frmClientDetails = new FrmClientDetails(OperationMode.Edit, clientId);
                                if (frmClientDetails.ShowDialog() == DialogResult.OK)
                                {
                                    LoadClientInfo(selectedIndex);
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

        private void chkIncludeInactive_CheckedChanged(object sender, EventArgs e)
        {
            LoadClientInfo(0);
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                txtSearchKeyword.Text = string.Empty;
                LoadClientInfo(0);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvClientInfo_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvClientInfo.Rows)
            {
                bool clientStatus = Convert.ToBoolean(row.Cells["IsClientActive"].Value);
                if (clientStatus.ToString() == "True")
                {
                    row.Cells["Active"].Value = "Active";
                }
                else
                {
                    row.Cells["Active"].Value = "Inactive";
                }
            }
        }

        #endregion Event Methods

        #region Private Methods

        private void InitializeControls()
        {
            try
            {
                dgvClientInfo.AutoGenerateColumns = false;

                if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                {
                    //CLIENT_ADD
                    DataRow[] clientAdd = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.CLIENT_ADD.ToDescriptionString() + "'");

                    if (clientAdd.Length > 0)
                    {
                        tsbNew.Visible = true;
                    }
                    else
                    {
                        tsbNew.Visible = false;
                    }

                    //CLIENT_EDIT
                    DataRow[] clientEdit = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.CLIENT_EDIT.ToDescriptionString() + "'");

                    if (clientEdit.Length > 0)
                    {
                        tsbEdit.Visible = true;
                        haveEditRights = true;
                    }
                    else
                    {
                        tsbEdit.Visible = false;
                    }

                    //CLIENT_ARCHIVE
                    DataRow[] clientArchive = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.CLIENT_ARCHIVE.ToDescriptionString() + "'");

                    if (clientArchive.Length > 0)
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

        private void LoadClientInfo(int selectedIndex)
        {
            try
            {
                ClientBL clientBL = new ClientBL();
                List<Client> clientList = null;

                Dictionary<string, string> searchParam = new Dictionary<string, string>();

                searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());

                clientList = clientBL.GetList(searchParam);

                dgvClientInfo.DataSource = clientList;

                if (dgvClientInfo.Rows.Count > 0)
                {
                    if (selectedIndex > dgvClientInfo.Rows.Count - 1)
                    {
                        selectedIndex = dgvClientInfo.Rows.Count - 1;
                    }

                    dgvClientInfo.Rows[selectedIndex].Selected = true;
                    dgvClientInfo.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion Private Methods

        private void dgvClientInfo_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            bool active = false;
            if (chkIncludeInactive.Checked == true)
            {
                active = true;
            }

            ClientBL clientBL = new ClientBL();
            List<Client> clientList = null;

            DataGridView dgv = sender as DataGridView;
            DataGridViewColumn col = dgv.Columns[e.ColumnIndex];
            if (col.Name == "ClientName")
            {
                DataGridViewColumn ClientName = dgv.Columns["ClientName"];
                string clientName = string.Empty;
                if (ClientName.HeaderCell.SortGlyphDirection == SortOrder.None || ClientName.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    clientList = clientBL.Sorting("clientName", active, "1");
                    dgvClientInfo.DataSource = clientList;

                    ClientName.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    clientList = clientBL.Sorting("clientName", active);
                    dgvClientInfo.DataSource = clientList;
                    ClientName.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "Active")
            {
                DataGridViewColumn IsClientActive = dgv.Columns["IsClientActive"];
                string isActive = string.Empty;
                if (IsClientActive.HeaderCell.SortGlyphDirection == SortOrder.None || IsClientActive.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    clientList = clientBL.Sorting("isActive", active, "1");
                    dgvClientInfo.DataSource = clientList;

                    IsClientActive.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    clientList = clientBL.Sorting("isActive", active);
                    dgvClientInfo.DataSource = clientList;
                    IsClientActive.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
        }
    }
}