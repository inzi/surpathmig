using SurPath.Business.Master;
using SurPath.Entity.Master;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmDrugNames : Form
    {
        private bool haveEditRights = false;

        #region Constructor

        public FrmDrugNames()
        {
            InitializeComponent();
        }

        #endregion Constructor

        #region Event Methods

        private void FrmDrugNames_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor; InitializeControls();
                LoadDrugNames(0);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Error occurred while loading the page.");
            }
        }

        private void FrmDrugNames_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                Program.frmMain.frmDrugNames = null;
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
                FrmDrugNameDetails frmDrugNameDetails = new FrmDrugNameDetails(Enum.OperationMode.New, 0);
                if (frmDrugNameDetails.ShowDialog() == DialogResult.OK)
                {
                    LoadDrugNames(0);
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
                    if (dgvDrugNames.SelectedRows.Count > 0)
                    {
                        int selectedIndex = dgvDrugNames.SelectedRows[0].Index;
                        int drugNameId = (int)dgvDrugNames.SelectedRows[0].Cells["DrugNameId"].Value;

                        FrmDrugNameDetails frmDrugNameDetails = new FrmDrugNameDetails(Enum.OperationMode.Edit, drugNameId);
                        if (frmDrugNameDetails.ShowDialog() == DialogResult.OK)
                        {
                            LoadDrugNames(selectedIndex);
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
                if (dgvDrugNames.Rows.Count > 0)
                {
                    if (MessageBox.Show("Are you sure? Do you want to archive the selected record?", "SurPath Drug Testing", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        int drugNameId = (int)dgvDrugNames.SelectedRows[0].Cells["DrugNameId"].Value;
                        DrugNameBL drugNameBL = new DrugNameBL();
                        int returnvalue = drugNameBL.Delete(drugNameId, Program.currentUserName);
                        if (returnvalue == 0)
                        {
                            MessageBox.Show("You cannot delete this record,since this is used in another process.");
                            return;
                        }
                        else
                        {
                            LoadDrugNames(0);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a Record.");
                    LoadDrugNames(0);
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
                // LoadDrugNames(0);
                DrugNameBL drugNameBL = new DrugNameBL();
                List<DrugName> drugNameList = null;

                Dictionary<string, string> searchParam = new Dictionary<string, string>();
                searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());

                drugNameList = drugNameBL.GetList(searchParam);
                dgvDrugNames.DataSource = drugNameList;

                if (dgvDrugNames.Rows.Count > 0)
                {
                    //if (selectedIndex > dgvDrugNames.Rows.Count - 1)
                    //{
                    //    selectedIndex = dgvDrugNames.Rows.Count - 1;
                    //}
                    dgvDrugNames.Rows[0].Selected = true;
                    dgvDrugNames.Focus();
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
                    // LoadDrugNames(0);

                    DrugNameBL drugNameBL = new DrugNameBL();
                    List<DrugName> drugNameList = null;

                    Dictionary<string, string> searchParam = new Dictionary<string, string>();
                    searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                    searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());

                    drugNameList = drugNameBL.GetList(searchParam);
                    dgvDrugNames.DataSource = drugNameList;

                    if (dgvDrugNames.Rows.Count > 0)
                    {
                        //if (selectedIndex > dgvDrugNames.Rows.Count - 1)
                        //{
                        //    selectedIndex = dgvDrugNames.Rows.Count - 1;
                        //}
                        dgvDrugNames.Rows[0].Selected = true;
                        dgvDrugNames.Focus();
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

        private void dgvDrugNames_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (e.RowIndex >= 0)
                {
                    int selectedIndex = e.RowIndex;
                    int drugNameId = (int)dgvDrugNames.Rows[e.RowIndex].Cells["DrugNameId"].Value;

                    if (haveEditRights)
                    {
                        FrmDrugNameDetails frmDrugNameDetails = new FrmDrugNameDetails(Enum.OperationMode.Edit, drugNameId);
                        if (frmDrugNameDetails.ShowDialog() == DialogResult.OK)
                        {
                            LoadDrugNames(selectedIndex);
                        }
                    }

                    if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                    {
                        //DRUG_NAMES_VIEW
                        DataRow[] drugNamesView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DRUG_NAMES_VIEW.ToDescriptionString() + "'");

                        if (drugNamesView.Length > 0)
                        {
                            FrmDrugNameDetails frmDrugNameDetails = new FrmDrugNameDetails(Enum.OperationMode.Edit, drugNameId);
                            if (frmDrugNameDetails.ShowDialog() == DialogResult.OK)
                            {
                                LoadDrugNames(selectedIndex);
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

        private void dgvDrugNames_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (e.KeyData == Keys.Enter)
                {
                    e.SuppressKeyPress = false;

                    if (dgvDrugNames.SelectedRows.Count > 0)
                    {
                        int selectedIndex = dgvDrugNames.SelectedRows[0].Index;
                        int drugNameId = (int)dgvDrugNames.SelectedRows[0].Cells["DrugNameId"].Value;

                        if (haveEditRights)
                        {
                            FrmDrugNameDetails frmDrugNameDetails = new FrmDrugNameDetails(Enum.OperationMode.Edit, drugNameId);
                            if (frmDrugNameDetails.ShowDialog() == DialogResult.OK)
                            {
                                LoadDrugNames(selectedIndex);
                            }
                        }

                        if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                        {
                            //DRUG_NAMES_VIEW
                            DataRow[] drugNamesView = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DRUG_NAMES_VIEW.ToDescriptionString() + "'");

                            if (drugNamesView.Length > 0)
                            {
                                FrmDrugNameDetails frmDrugNameDetails = new FrmDrugNameDetails(Enum.OperationMode.Edit, drugNameId);
                                if (frmDrugNameDetails.ShowDialog() == DialogResult.OK)
                                {
                                    LoadDrugNames(selectedIndex);
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

        private void chkIncludeInactive_CheckedChanged(object sender, EventArgs e)
        {
            LoadDrugNames(0);
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                txtSearchKeyword.Text = string.Empty;
                LoadDrugNames(0);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvDrugNames_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //    foreach (DataGridViewColumn column in dgvDrugNames.Columns)
            //    {
            //        column.SortMode = DataGridViewColumnSortMode.Programmatic;
            //    }

            foreach (DataGridViewRow row in dgvDrugNames.Rows)
            {
                if (row.Cells["IsUA"].Value.ToString().ToUpper() == "TRUE")
                {
                    row.Cells["HaveUA"].Value = "Yes";
                }
                else
                {
                    row.Cells["HaveUA"].Value = "No";
                }

                if (row.Cells["IsHair"].Value.ToString().ToUpper() == "TRUE")
                {
                    row.Cells["HaveHair"].Value = "Yes";
                }
                else
                {
                    row.Cells["HaveHair"].Value = "No";
                }

                bool drugStatus = Convert.ToBoolean(row.Cells["IsActive"].Value);

                if (drugStatus.ToString() == "True")
                {
                    row.Cells["Status"].Value = "Active";
                }
                else
                {
                    row.Cells["Status"].Value = "Inactive";
                }
            }
            foreach (DataGridViewColumn column in dgvDrugNames.Columns)
            {
                //  column.SortMode = DataGridViewColumnSortMode.Programmatic;
                column.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
        }

        //int cntr = 0;
        private void dgvDrugNames_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            bool active = false;
            if (chkIncludeInactive.Checked == true)
            {
                active = true;
            }

            DrugNameBL drugNameBL = new DrugNameBL();
            List<DrugName> drugList = null;

            DataGridView dgv = sender as DataGridView;
            DataGridViewColumn col = dgv.Columns[e.ColumnIndex];

            if (col.Name == "DrugNameValue")
            {
                DataGridViewColumn DrugName = dgv.Columns["DrugNameValue"];
                string drugName = string.Empty;
                if (DrugName.HeaderCell.SortGlyphDirection == SortOrder.None || DrugName.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    drugList = drugNameBL.Sorting("drugName", active, "1");
                    dgvDrugNames.DataSource = drugList;

                    DrugName.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    drugList = drugNameBL.Sorting("drugName", active);
                    dgvDrugNames.DataSource = drugList;
                    DrugName.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            //if (col.Name == "HaveUA")
            //{
            //    DataGridViewColumn UA = dgv.Columns["IsUA"];
            //    string isUA = string.Empty;
            //    if (UA.HeaderCell.SortGlyphDirection == SortOrder.None || UA.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
            //    {
            //        drugList = drugNameBL.Sorting("isUA", active, "1");
            //        dgvDrugNames.DataSource = drugList;

            //        UA.HeaderCell.SortGlyphDirection = SortOrder.Descending;

            //    }
            //    else
            //    {
            //        drugList = drugNameBL.Sorting("isUA", active);
            //        dgvDrugNames.DataSource = drugList;
            //        UA.HeaderCell.SortGlyphDirection = SortOrder.Ascending;

            //    }
            //}
            //if (col.Name == "HaveHair")
            //{
            //    DataGridViewColumn Hair = dgv.Columns["IsHair"];
            //    string isHair = string.Empty;
            //    if (Hair.HeaderCell.SortGlyphDirection == SortOrder.None || Hair.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
            //    {
            //        drugList = drugNameBL.Sorting("isHair", active, "1");
            //        dgvDrugNames.DataSource = drugList;

            //        Hair.HeaderCell.SortGlyphDirection = SortOrder.Descending;

            //    }
            //    else
            //    {
            //        drugList = drugNameBL.Sorting("isHair", active);
            //        dgvDrugNames.DataSource = drugList;
            //        Hair.HeaderCell.SortGlyphDirection = SortOrder.Ascending;

            //    }
            //}
            else if (col.Name == "Status")
            {
                DataGridViewColumn IsActive = dgv.Columns["IsActive"];
                string isActive = string.Empty;
                if (IsActive.HeaderCell.SortGlyphDirection == SortOrder.None || IsActive.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    drugList = drugNameBL.Sorting("isActive", active, "1");
                    dgvDrugNames.DataSource = drugList;

                    IsActive.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                    //  col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
                else
                {
                    drugList = drugNameBL.Sorting("isActive", active);
                    dgvDrugNames.DataSource = drugList;
                    IsActive.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                    //   col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
            }
            //if (cntr % 2 == 0)
            //    dgvDrugNames.Sort(dgvDrugNames.Columns[e.ColumnIndex], ListSortDirection.Ascending);
            //else
            //    dgvDrugNames.Sort(dgvDrugNames.Columns[e.ColumnIndex], ListSortDirection.Descending);
            //cntr++;
        }

        #endregion Event Methods

        #region Private Methods

        private void InitializeControls()
        {
            try
            {
                dgvDrugNames.AutoGenerateColumns = false;

                if (!(Program.currentUserName.ToUpper() == Program.superAdmin.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin1.ToUpper() || Program.currentUserName.ToUpper() == Program.superAdmin2.ToUpper()))
                {
                    //DRUG_NAMES_ADD
                    DataRow[] drugNamesAdd = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DRUG_NAMES_ADD.ToDescriptionString() + "'");

                    if (drugNamesAdd.Length > 0)
                    {
                        tsbNew.Visible = true;
                    }
                    else
                    {
                        tsbNew.Visible = false;
                    }

                    //DRUG_NAMES_EDIT
                    DataRow[] drugNamesEdit = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DRUG_NAMES_EDIT.ToDescriptionString() + "'");

                    if (drugNamesEdit.Length > 0)
                    {
                        tsbEdit.Visible = true;
                        haveEditRights = true;
                    }
                    else
                    {
                        tsbEdit.Visible = false;
                    }

                    //DRUG_NAMES_ARCHIVE
                    DataRow[] drugNamesArchive = Program.dtUserAuthRules.Select("AuthRuleInternalName = '" + AuthorizationRules.DRUG_NAMES_ARCHIVE.ToDescriptionString() + "'");

                    if (drugNamesArchive.Length > 0)
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

        private void LoadDrugNames(int selectedIndex)
        {
            try
            {
                DrugNameBL drugNameBL = new DrugNameBL();
                List<DrugName> drugNameList = null;

                Dictionary<string, string> searchParam = new Dictionary<string, string>();
                searchParam.Add("GeneralSearch", "%" + txtSearchKeyword.Text.Trim() + "%");
                searchParam.Add("IncludeInactive", chkIncludeInactive.Checked.ToString());

                drugNameList = drugNameBL.GetList(searchParam);
                dgvDrugNames.DataSource = drugNameList;

                if (dgvDrugNames.Rows.Count > 0)
                {
                    if (selectedIndex > dgvDrugNames.Rows.Count - 1)
                    {
                        selectedIndex = dgvDrugNames.Rows.Count - 1;
                    }
                    dgvDrugNames.Rows[selectedIndex].Selected = true;
                    dgvDrugNames.Focus();
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