using Serilog;
using SurPath.Business;
using SurPath.Data;
using SurPath.Data.Backend;
using SurPath.Entity;
using SurPath.Enum;
using SurpathBackend;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmPartners : Form
    {

        public List<IntegrationPartner> partners = new List<IntegrationPartner>();
        private BackendLogic backendLogic = new BackendLogic(Program.currentUserName, _logger);
        private Dictionary<string, DGVHelper> dgvHelpers = new Dictionary<string, DGVHelper>();
        private bool includeInactive = false;
        static ILogger _logger = Program._logger;

        public FrmPartners(ILogger __logger = null)
        {
            if (__logger != null) _logger = __logger;
            _logger.Debug("FrmPartners loaded");
            InitializeComponent();
        }

        private void frmPartners_Load(object sender, EventArgs e)
        {
            chkIncludeInactive.Checked = includeInactive;
            dgvHelpers["dgvPartners"] = new DGVHelper() { colID = "" };
            PopulateKnownCells(dgvPartners);

            LoadData();
            LoadGrid(0);
        }

        private void frmPartners_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.dgvPartners = null;
            Program.frmMain.frmPartners = null;
        }


        #region data
        private void LoadData()
        {
            partners = backendLogic.GetIntegrationPartners();

        }

        #endregion data

        #region datagrid

        private void LoadGrid(int selectedIndex)
        {
            DataGridView _dgv = dgvPartners;
            List<IntegrationPartner> _partners = this.partners;
            if (includeInactive==false)
            {
                _partners = _partners.Where(p => p.active == true).ToList();
            }
            _dgv.DataSource = _partners;

            if (_dgv.Rows.Count > 0)
            {
                if (selectedIndex > _dgv.Rows.Count - 1)
                {
                    selectedIndex = _dgv.Rows.Count - 1;
                }

                _dgv.Rows[selectedIndex].Selected = true;
                _dgv.Focus();
            }

            dgvHelpers["dgvPartners"].ColumnOrder(ref dgvPartners);
        }
        private void dgvPartners_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv_DataBindingComplete(sender, e);
            
        }

        private void dgv_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            List<string> _rowCellNames = new List<string>();
            foreach (DataGridViewRow row in dgv.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    _rowCellNames.Add(cell.OwningColumn.DataPropertyName);
                }
            }

            //hide any rows not defined at design time
            // hid any rows not defined at design time
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                if (!dgvHelpers[dgv.Name].KnownCells.Contains(column.Name))
                {
                    Debug.WriteLine($"{column.Name} hidden");
                    column.Visible = false;
                }
                else
                {
                    Debug.WriteLine($"{column.Name} shown");
                }
            }


            dgvHelpers[dgv.Name].SelectedCounter.RowsAvailable = dgv.Rows.Count > 0;

        }
        private void PopulateKnownCells(DataGridView dgv)
        {
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                Debug.WriteLine($"{column.Name} - di {column.DisplayIndex}");
                dgvHelpers[dgv.Name].KnownCells.Add(column.Name);
                dgv.Columns[column.Name].DisplayIndex = column.Index;
                dgvHelpers[dgv.Name].Columns.Add(column);
            }
            dgvHelpers["dgvPartners"].ColumnOrder(ref dgvPartners, true);
        }
        #endregion datagrid

        private void dgvPartners_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                DataGridView dgv = sender as DataGridView;

                Cursor.Current = Cursors.WaitCursor;
                if (e.RowIndex >= 0)
                {
                    int selectedIndex = e.RowIndex;

                    int _id = (int)dgv.Rows[e.RowIndex].Cells["backend_integration_partner_id"].Value;
                    bool haveEditRights = true;
                    if (haveEditRights)
                    {
                        //FrmPartnerDetails frmPartnerDetails = new FrmPartnerDetails(Enum.OperationMode.Edit, judgeId);
                        FrmPartnerDetails frmPartnerDetails = new FrmPartnerDetails();
                        frmPartnerDetails.integrationParter = partners.Where(p => p.backend_integration_partner_id == _id).First();
                        //frmPartnerDetails.Size = new System.Drawing.Size(721, 449);
                        //frmPartnerDetails.btnOK.Location = new System.Drawing.Point(270, 375);
                        //frmPartnerDetails.btnClose.Location = new System.Drawing.Point(357, 375);
                        if (frmPartnerDetails.ShowDialog() == DialogResult.OK)
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            LoadData();
                            LoadGrid(0);
                        }
                    }

                    //if (haveEditRights)
                    //{
                    //    FrmJudgeDetails frmJudgeDetails = new FrmJudgeDetails(Enum.OperationMode.Edit, judgeId);
                    //    frmJudgeDetails.Size = new System.Drawing.Size(721, 288);
                    //    frmJudgeDetails.btnOK.Location = new System.Drawing.Point(271, 222);
                    //    frmJudgeDetails.btnClose.Location = new System.Drawing.Point(358, 222);
                    //    if (frmJudgeDetails.ShowDialog() == DialogResult.OK)
                    //    {
                    //        Cursor.Current = Cursors.WaitCursor;
                    //        LoadJudgeInfo(selectedIndex);
                    //    }
                    //}

                    //if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                    //{
                    //    //JUDGE_VIEW
                    //    DataRow[] judgeView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.JUDGE_VIEW.ToDescriptionString() + "'");

                    //    if (judgeView.Length > 0)
                    //    {
                    //        FrmJudgeDetails frmJudgeDetails = new FrmJudgeDetails(Enum.OperationMode.Edit, judgeId);
                    //        frmJudgeDetails.Size = new System.Drawing.Size(721, 288);
                    //        frmJudgeDetails.btnOK.Location = new System.Drawing.Point(271, 222);
                    //        frmJudgeDetails.btnClose.Location = new System.Drawing.Point(358, 222);
                    //        if (frmJudgeDetails.ShowDialog() == DialogResult.OK)
                    //        {
                    //            Cursor.Current = Cursors.WaitCursor;
                    //            LoadJudgeInfo(selectedIndex);
                    //        }
                    //    }
                    //}

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
            this.includeInactive = ((CheckBox)sender).Checked;
            LoadData();
            LoadGrid(0);
        }

        private void dgvPartners_ColumnDisplayIndexChanged(object sender, DataGridViewColumnEventArgs e)
        {
            dgvHelpers["dgvPartners"].ColumnOrder(ref dgvPartners, true);
        }

        private void tsbNew_Click(object sender, EventArgs e)
        {
            FrmPartnerDetails frmPartnerDetails = new FrmPartnerDetails();
            IntegrationPartner _integrationParter = new IntegrationPartner();
            _integrationParter.login_url = "http://surscan.com";
            _integrationParter.partner_key = Guid.NewGuid().ToString();
            _integrationParter.partner_crypto = Guid.NewGuid().ToString();
            frmPartnerDetails.integrationParter = _integrationParter;
            //frmPartnerDetails.Size = new System.Drawing.Size(721, 449);
            //frmPartnerDetails.btnOK.Location = new System.Drawing.Point(270, 375);
            //frmPartnerDetails.btnClose.Location = new System.Drawing.Point(357, 375);
            if (frmPartnerDetails.ShowDialog() == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;
                LoadData();
                LoadGrid(0);
            }
        }

        private void tsbArchive_Click(object sender, EventArgs e)
        {
            Int32 selectedCellCount = dgvPartners.GetCellCount(DataGridViewElementStates.Selected);
            if (selectedCellCount>0)
            {
                for (int i =0; i< selectedCellCount; i++)
                {
                    var rowIdx = dgvPartners.SelectedCells[i].RowIndex;
                    int _id = (int)dgvPartners.Rows[rowIdx].Cells["backend_integration_partner_id"].Value;
                    IntegrationPartner integrationPartner = backendLogic.GetIntegrationPartnersById(_id);
                    if (integrationPartner.backend_integration_partner_id==_id)
                    {
                        integrationPartner.active = false;
                    }
                    backendLogic.SetIntegrationPartners(integrationPartner);
                }
            }
            LoadData();
            LoadGrid(0);
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            if (this.includeInactive==true)
            {
                this.includeInactive = false;
            }
            else
            {
                this.includeInactive = true;
            }
            LoadData();
            LoadGrid(0);
        }
    }
}
