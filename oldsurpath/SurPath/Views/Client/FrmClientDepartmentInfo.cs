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
    public partial class FrmClientDepartmentInfo : Form
    {
        private bool haveEditRights = false;

        #region Private Variables
        static ILogger _logger = Program._logger;

        private int _clientId;

        #endregion Private Variables

        #region Constructor

        public FrmClientDepartmentInfo()
        {
            _logger.Debug("FrmClientDepartmentInfo loaded");
            InitializeComponent();
        }

        public FrmClientDepartmentInfo(int clientId, ILogger __logger = null)
        {
            _logger.Debug("FrmClientDepartmentInfo loaded");
            InitializeComponent();

            this._clientId = clientId;
        }

        #endregion Constructor

        #region Event Methods

        private void FrmClientDepartmentInfo_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                InitializeControls();
                LoadClientDepartmentInfo(0);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmClientDepartmentInfo_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
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
                FrmClientDepartmentDetails frmClientDepartmentDetails = new FrmClientDepartmentDetails(Enum.OperationMode.New, this._clientId, 0);
                if (frmClientDepartmentDetails.ShowDialog() == DialogResult.OK)
                {
                    LoadClientDepartmentInfo(0);
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
                    if (dgvClientDepartment.SelectedRows.Count > 0)
                    {
                        int selectedIndex = dgvClientDepartment.SelectedRows[0].Index;
                        int clientDepartmentId = (int)dgvClientDepartment.SelectedRows[0].Cells["ClientDepartmentId"].Value;

                        FrmClientDepartmentDetails frmClientDepartmentDetails = new FrmClientDepartmentDetails(Enum.OperationMode.Edit, this._clientId, clientDepartmentId);
                        if (frmClientDepartmentDetails.ShowDialog() == DialogResult.OK)
                        {
                            LoadClientDepartmentInfo(selectedIndex);
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
                if (dgvClientDepartment.Rows.Count > 0)
                {
                    if (MessageBox.Show("Are you sure? Do you want to archive the selected record?", "SurPath Drug Testing", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        int clientDepartmentId = (int)dgvClientDepartment.SelectedRows[0].Cells["ClientDepartmentId"].Value;
                        ClientBL clientBL = new ClientBL();
                        int returnvalue = clientBL.DeleteClientDepartment(clientDepartmentId, Program.currentUserName);
                        if (returnvalue == 0)
                        {
                            MessageBox.Show("You cannot delete this record,since this is used in another process.");
                            return;
                        }
                        else
                        {
                            LoadClientDepartmentInfo(0);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a Record.");
                    LoadClientDepartmentInfo(0);
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

                    //LoadClientDepartmentInfo(0);
                    ClientBL clientBL = new ClientBL();
                    List<ClientDepartment> clientDepartmentList = null;
                    Dictionary<string, string> searchParam = new Dictionary<string, string>();
                    searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                    searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());
                    clientDepartmentList = clientBL.GetClientDepartmentList(this._clientId, searchParam);

                    dgvClientDepartment.DataSource = clientDepartmentList;

                    if (dgvClientDepartment.Rows.Count > 0)
                    {
                        //if (selectedIndex > dgvClientDepartment.Rows.Count - 1)
                        //{
                        //    selectedIndex = dgvClientDepartment.Rows.Count - 1;
                        //}

                        dgvClientDepartment.Rows[0].Selected = true;
                        dgvClientDepartment.Focus();
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                //   LoadClientDepartmentInfo(0);
                ClientBL clientBL = new ClientBL();
                List<ClientDepartment> clientDepartmentList = null;
                Dictionary<string, string> searchParam = new Dictionary<string, string>();
                searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());
                clientDepartmentList = clientBL.GetClientDepartmentList(this._clientId, searchParam);

                dgvClientDepartment.DataSource = clientDepartmentList;

                if (dgvClientDepartment.Rows.Count > 0)
                {
                    //if (selectedIndex > dgvClientDepartment.Rows.Count - 1)
                    //{
                    //    selectedIndex = dgvClientDepartment.Rows.Count - 1;
                    //}

                    dgvClientDepartment.Rows[0].Selected = true;
                    dgvClientDepartment.Focus();
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

        private void dgvClientDepartment_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (e.RowIndex >= 0)
                {
                    int selectedIndex = e.RowIndex;
                    int clientDepartmentId = (int)dgvClientDepartment.Rows[e.RowIndex].Cells["ClientDepartmentId"].Value;
                    if (haveEditRights)
                    {
                        FrmClientDepartmentDetails frmClientDepartmentDetails = new FrmClientDepartmentDetails(Enum.OperationMode.Edit, this._clientId, clientDepartmentId);
                        frmClientDepartmentDetails.Tag = this.lblClientName.Text;
                        if (frmClientDepartmentDetails.ShowDialog() == DialogResult.OK)
                        {
                            LoadClientDepartmentInfo(selectedIndex);
                        }
                    }

                    if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                    {
                        //CLIENT_DEPARTMENT_VIEW
                        DataRow[] clientDepartmentView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.CLIENT_DEPARTMENT_VIEW.ToDescriptionString() + "'");

                        if (clientDepartmentView.Length > 0)
                        {
                            FrmClientDepartmentDetails frmClientDepartmentDetails = new FrmClientDepartmentDetails(Enum.OperationMode.Edit, this._clientId, clientDepartmentId);
                            if (frmClientDepartmentDetails.ShowDialog() == DialogResult.OK)
                            {
                                LoadClientDepartmentInfo(selectedIndex);
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
                _logger.Error(this.Name + "Error");
                _logger.Error(ex.Message);
                _logger.Error(ex.InnerException.ToString());

            }
        }

        private void dgvClientDepartment_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (e.KeyData == Keys.Enter)
                {
                    e.SuppressKeyPress = false;
                    if (dgvClientDepartment.SelectedRows.Count > 0)
                    {
                        int selectedIndex = dgvClientDepartment.SelectedRows[0].Index;
                        int clientDepartmentId = (int)dgvClientDepartment.SelectedRows[0].Cells["ClientDepartmentId"].Value;
                        if (haveEditRights)
                        {
                            FrmClientDepartmentDetails frmClientDepartmentDetails = new FrmClientDepartmentDetails(Enum.OperationMode.Edit, this._clientId, clientDepartmentId);
                            if (frmClientDepartmentDetails.ShowDialog() == DialogResult.OK)
                            {
                                LoadClientDepartmentInfo(selectedIndex);
                            }
                        }

                        if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                        {
                            //CLIENT_DEPARTMENT_VIEW
                            DataRow[] clientDepartmentView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.CLIENT_DEPARTMENT_VIEW.ToDescriptionString() + "'");

                            if (clientDepartmentView.Length > 0)
                            {
                                FrmClientDepartmentDetails frmClientDepartmentDetails = new FrmClientDepartmentDetails(Enum.OperationMode.Edit, this._clientId, clientDepartmentId);
                                if (frmClientDepartmentDetails.ShowDialog() == DialogResult.OK)
                                {
                                    LoadClientDepartmentInfo(selectedIndex);
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

        private void dgvClientDepartment_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvClientDepartment.Rows)
            {
                bool isUA = (bool)row.Cells["IsUA"].Value;

                if (isUA)
                {
                    row.Cells["UA"].Value = "Yes";
                }
                else
                {
                    row.Cells["UA"].Value = "No";
                }

                bool isHair = (bool)row.Cells["IsHair"].Value;
                if (isHair)
                {
                    row.Cells["Hair"].Value = "Yes";
                }
                else
                {
                    row.Cells["Hair"].Value = "No";
                }

                bool isDNA = (bool)row.Cells["IsDNA"].Value;
                if (isDNA)
                {
                    row.Cells["DNA"].Value = "Yes";
                }
                else
                {
                    row.Cells["DNA"].Value = "No";
                }

                bool isBC = (bool)row.Cells["IsBC"].Value;
                if (isBC)
                {
                    row.Cells["BC"].Value = "Yes";
                }
                else
                {
                    row.Cells["BC"].Value = "No";
                }

                bool isRC = (bool)row.Cells["IsRC"].Value;
                if (isRC)
                {
                    row.Cells["RC"].Value = "Yes";
                }
                else
                {
                    row.Cells["RC"].Value = "No";
                }

                ClientMROTypes mroType = (ClientMROTypes)((int)row.Cells["MROTypeId"].Value);
                row.Cells["MROType"].Value = mroType.ToString();

                ClientPaymentTypes paymentType = (ClientPaymentTypes)((int)row.Cells["PaymentTypeId"].Value);
                row.Cells["PaymentType"].Value = paymentType.ToString();

                bool clientDepartmentStatus = Convert.ToBoolean(row.Cells["IsDepartmentActive"].Value);
                if (clientDepartmentStatus.ToString() == "True")
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
                LoadClientDepartmentInfo(0);
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
            LoadClientDepartmentInfo(0);
        }

        #endregion Event Methods

        #region Private Methods

        private void InitializeControls()
        {
            try
            {
                dgvClientDepartment.AutoGenerateColumns = false;

                if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                {
                    //CLIENT_DEPARTMENT_ADD
                    DataRow[] clientDepartmentAdd = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.CLIENT_DEPARTMENT_ADD.ToDescriptionString() + "'");

                    if (clientDepartmentAdd.Length > 0)
                    {
                        tsbNew.Visible = true;
                    }
                    else
                    {
                        tsbNew.Visible = false;
                    }

                    //CLIENT_DEPARTMENT_EDIT
                    DataRow[] clientDepartmentEdit = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.CLIENT_DEPARTMENT_EDIT.ToDescriptionString() + "'");

                    if (clientDepartmentEdit.Length > 0)
                    {
                        tsbEdit.Visible = true;
                        haveEditRights = true;
                    }
                    else
                    {
                        tsbEdit.Visible = false;
                    }

                    //CLIENT_DEPARTMENT_ARCHIVE
                    DataRow[] clientDepartmentArchive = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.CLIENT_DEPARTMENT_ARCHIVE.ToDescriptionString() + "'");

                    if (clientDepartmentArchive.Length > 0)
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

            LoadClientDetails();
        }

        private void LoadClientDepartmentInfo(int selectedIndex)
        {
            try
            {
                ClientBL clientBL = new ClientBL();
                List<ClientDepartment> clientDepartmentList = null;
                Dictionary<string, string> searchParam = new Dictionary<string, string>();
                searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());
                clientDepartmentList = clientBL.GetClientDepartmentList(this._clientId, searchParam);

                dgvClientDepartment.DataSource = clientDepartmentList;

                if (dgvClientDepartment.Rows.Count > 0)
                {
                    if (selectedIndex > dgvClientDepartment.Rows.Count - 1)
                    {
                        selectedIndex = dgvClientDepartment.Rows.Count - 1;
                    }

                    dgvClientDepartment.Rows[selectedIndex].Selected = true;
                    dgvClientDepartment.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadClientDetails()
        {
            try
            {
                ClientBL clientBL = new ClientBL();
                Client client = clientBL.Get(this._clientId);

                lblClientName.Text = client.ClientName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion Private Methods

        private void dgvClientDepartment_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            bool active = false;
            if (chkIncludeInactive.Checked == true)
            {
                active = true;
            }

            ClientBL clientBL = new ClientBL();
            List<ClientDepartment> clientDepartmentList = null;

            DataGridView dgv = sender as DataGridView;
            DataGridViewColumn col = dgv.Columns[e.ColumnIndex];
            if (col.Name == "DepartmentName")
            {
                DataGridViewColumn DepartmentName = dgv.Columns["DepartmentName"];
                string clientDepartmentName = string.Empty;
                if (DepartmentName.HeaderCell.SortGlyphDirection == SortOrder.None || DepartmentName.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    clientDepartmentList = clientBL.SortingClientDepartment(this._clientId, "clientDepartmentName", active, "1");
                    dgvClientDepartment.DataSource = clientDepartmentList;

                    DepartmentName.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    clientDepartmentList = clientBL.SortingClientDepartment(this._clientId, "clientDepartmentName", active);
                    dgvClientDepartment.DataSource = clientDepartmentList;
                    DepartmentName.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            //else if (col.Name == "UA")
            //{
            //    DataGridViewColumn UA = dgv.Columns["UA"];
            //    string isUA = string.Empty;
            //    if (UA.HeaderCell.SortGlyphDirection == SortOrder.None || UA.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
            //    {
            //        clientDepartmentList = clientBL.SortingClientDepartment(this._clientId, "isUA", active, "1");
            //        dgvClientDepartment.DataSource = clientDepartmentList;

            //        UA.HeaderCell.SortGlyphDirection = SortOrder.Descending;

            //    }
            //    else
            //    {
            //        clientDepartmentList = clientBL.SortingClientDepartment(this._clientId, "isUA", active);
            //        dgvClientDepartment.DataSource = clientDepartmentList;
            //        UA.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
            //    }
            //}
            //else if (col.Name == "Hair")
            //{
            //    DataGridViewColumn Hair = dgv.Columns["Hair"];
            //    string isHair = string.Empty;
            //    if (Hair.HeaderCell.SortGlyphDirection == SortOrder.None || Hair.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
            //    {
            //        clientDepartmentList = clientBL.SortingClientDepartment(this._clientId, "isHair", active, "1");
            //        dgvClientDepartment.DataSource = clientDepartmentList;

            //        Hair.HeaderCell.SortGlyphDirection = SortOrder.Descending;

            //    }
            //    else
            //    {
            //        clientDepartmentList = clientBL.SortingClientDepartment(this._clientId, "isHair", active);
            //        dgvClientDepartment.DataSource = clientDepartmentList;
            //        Hair.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
            //    }
            //}
            //else if (col.Name == "DNA")
            //{
            //    DataGridViewColumn DNA = dgv.Columns["DNA"];
            //    string isDNA = string.Empty;
            //    if (DNA.HeaderCell.SortGlyphDirection == SortOrder.None || DNA.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
            //    {
            //        clientDepartmentList = clientBL.SortingClientDepartment(this._clientId, "isDNA", active, "1");
            //        dgvClientDepartment.DataSource = clientDepartmentList;

            //        DNA.HeaderCell.SortGlyphDirection = SortOrder.Descending;

            //    }
            //    else
            //    {
            //        clientDepartmentList = clientBL.SortingClientDepartment(this._clientId, "isDNA", active);
            //        dgvClientDepartment.DataSource = clientDepartmentList;
            //        DNA.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
            //    }
            //}
            else if (col.Name == "MROType")
            {
                DataGridViewColumn MROType = dgv.Columns["MROType"];
                string isMROType = string.Empty;
                if (MROType.HeaderCell.SortGlyphDirection == SortOrder.None || MROType.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    clientDepartmentList = clientBL.SortingClientDepartment(this._clientId, "isMROType", active, "1");
                    dgvClientDepartment.DataSource = clientDepartmentList;

                    MROType.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    clientDepartmentList = clientBL.SortingClientDepartment(this._clientId, "isMROType", active);
                    dgvClientDepartment.DataSource = clientDepartmentList;
                    MROType.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "PaymentType")
            {
                DataGridViewColumn PaymentType = dgv.Columns["PaymentType"];
                string isPaymentType = string.Empty;
                if (PaymentType.HeaderCell.SortGlyphDirection == SortOrder.None || PaymentType.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    clientDepartmentList = clientBL.SortingClientDepartment(this._clientId, "isPaymentType", active, "1");
                    dgvClientDepartment.DataSource = clientDepartmentList;

                    PaymentType.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    clientDepartmentList = clientBL.SortingClientDepartment(this._clientId, "isPaymentType", active);
                    dgvClientDepartment.DataSource = clientDepartmentList;
                    PaymentType.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "Status")
            {
                DataGridViewColumn Status = dgv.Columns["Status"];
                string isDepartmentActive = string.Empty;
                if (Status.HeaderCell.SortGlyphDirection == SortOrder.None || Status.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    clientDepartmentList = clientBL.SortingClientDepartment(this._clientId, "isDepartmentActive", active, "1");
                    dgvClientDepartment.DataSource = clientDepartmentList;

                    Status.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    clientDepartmentList = clientBL.SortingClientDepartment(this._clientId, "isDepartmentActive", active);
                    dgvClientDepartment.DataSource = clientDepartmentList;
                    Status.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
        }
    }
}