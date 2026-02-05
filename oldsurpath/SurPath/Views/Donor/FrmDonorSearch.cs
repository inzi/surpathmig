using SurPath.Business;
using SurPath.Data;
using SurPath.Entity;
using SurPath.Enum;
using SurpathBackend;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmDonorSearch : Form
    {
        #region Private Variables

        private ClientBL clientBL = new ClientBL();

        // FrmAddRemoveColumns frmAddRemoveColumns();

        private Dictionary<string, string> searchParamExport = new Dictionary<string, string>();

        private string fieldList = string.Empty;
        private bool showAll = false;

        private BackendData backendData = new BackendData();
        private BackendLogic backendLogic = new BackendLogic(Program.currentUserName);
        private int NumberOfRowsChecked = 0;
        private SelectedCounter selectedCounter;

        private int FilterDayInterval = -90;
        #endregion Private Variables

        #region Constructor

        public FrmDonorSearch()
        {
            InitializeComponent();
        }

        #endregion Constructor

        #region Event Methods

        private void FrmDonorSearch_Load(object sender, EventArgs e)
        {
            Program._logger.Debug("Search loaded");
            try
            {
                if (ConfigurationManager.AppSettings["DateFilterDelta"] != null)
                {
                    var _interval = ConfigurationManager.AppSettings["DateFilterDelta"].ToString().Trim();
                    int.TryParse(_interval, out this.FilterDayInterval);
                }
                selectedCounter = new SelectedCounter();
                Cursor.Current = Cursors.WaitCursor;
                InitializeControls();
                Cursor.Current = Cursors.Default;



            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmDonorSearch_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                Program.frmMain.frmDonorSearch = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void rbtnDateRange_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnDateRange.Checked)
            {
                dtpFromDate.Visible = true;
                dtpToDate.Visible = true;
            }
            else
            {
                dtpFromDate.Visible = false;
                dtpToDate.Visible = false;
            }
        }

        private void txtSSN_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void txtDOB_MouseClick(object sender, MouseEventArgs e)
        {
            //  SendKeys.Send("{HOME}");
        }

        private void txtZipCode_MouseClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void cmbClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                LoadClientDepartment();
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
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                DoSearch();
                stopWatch.Stop();
                Program._logger.Debug($"Donor search time in seconds: {stopWatch.Elapsed.TotalSeconds.ToString()}");

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (dgvSearchResult.Rows.Count > 0)
                {
                    for (int i = 0; i < dgvSearchResult.Rows.Count; i++)
                    {
                        DataGridViewCheckBoxCell donorSelection = (DataGridViewCheckBoxCell)dgvSearchResult.Rows[i].Cells["DonorSelection"];
                        donorSelection.Value = true;
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

        private void btnDeselectAll_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (dgvSearchResult.Rows.Count > 0)
                {
                    for (int i = 0; i < dgvSearchResult.Rows.Count; i++)
                    {
                        DataGridViewCheckBoxCell donorSelection = (DataGridViewCheckBoxCell)dgvSearchResult.Rows[i].Cells["DonorSelection"];
                        donorSelection.Value = false;
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

        private void btnViewSelected_Click(object sender, EventArgs e)
        {
            try
            {
                int count1 = 0;
                if (dgvSearchResult.Rows.Count > 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    for (int i = 0; i < dgvSearchResult.Rows.Count; i++)
                    {
                        DataGridViewCheckBoxCell donorSelection = (DataGridViewCheckBoxCell)dgvSearchResult.Rows[i].Cells["DonorSelection"];

                        if (Convert.ToBoolean(donorSelection.Value) == true)
                        {
                            count1++;
                        }
                    }
                }
                this.NumberOfRowsChecked = count1;
                if (count1 > 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    if (dgvSearchResult.Rows.Count > 0)
                    {
                        Dictionary<string, string> tabPageList = new Dictionary<string, string>();

                        for (int i = 0; i < dgvSearchResult.Rows.Count; i++)
                        {
                            DataGridViewCheckBoxCell donorSelection = (DataGridViewCheckBoxCell)dgvSearchResult.Rows[i].Cells["DonorSelection"];

                            if (Convert.ToBoolean(donorSelection.Value))
                            {
                                string donorId = dgvSearchResult.Rows[i].Cells["DonorId"].Value.ToString();
                                string donorTestInfoId = dgvSearchResult.Rows[i].Cells["DonorTestInfoId"].Value.ToString();
                                string donorFistName = dgvSearchResult.Rows[i].Cells["DonorFirstName"].Value.ToString();
                                string donorLastName = dgvSearchResult.Rows[i].Cells["DonorLastName"].Value.ToString();

                                donorTestInfoId = donorTestInfoId != string.Empty ? donorTestInfoId : "0";

                                if (donorId != string.Empty && donorTestInfoId != string.Empty)
                                {
                                    string key = donorId + "#" + donorTestInfoId;
                                    string value = donorFistName + " " + donorLastName;
                                    if (!tabPageList.ContainsKey(key))
                                    {
                                        tabPageList.Add(key, value);
                                    }
                                }
                            }
                        }

                        if (tabPageList.Count > 0)
                        {
                            LoadDonorDetails(tabPageList);
                        }
                    }
                }
                else
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Select a donors.");
                    return;
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void btnViewAll_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (dgvSearchResult.Rows.Count > 0)
                {
                    Dictionary<string, string> tabPageList = new Dictionary<string, string>();

                    for (int i = 0; i < dgvSearchResult.Rows.Count; i++)
                    {
                        string donorId = dgvSearchResult.Rows[i].Cells["DonorId"].Value.ToString();
                        string donorTestInfoId = dgvSearchResult.Rows[i].Cells["DonorTestInfoId"].Value.ToString();
                        string donorFistName = dgvSearchResult.Rows[i].Cells["DonorFirstName"].Value.ToString();
                        string donorLastName = dgvSearchResult.Rows[i].Cells["DonorLastName"].Value.ToString();

                        donorTestInfoId = donorTestInfoId != string.Empty ? donorTestInfoId : "0";

                        if (donorId != string.Empty && donorTestInfoId != string.Empty)
                        {
                            string key = donorId + "#" + donorTestInfoId;
                            string value = donorFistName + " " + donorLastName;
                            if (!tabPageList.ContainsKey(key))
                            {
                                tabPageList.Add(key, value);
                            }
                        }
                    }

                    if (tabPageList.Count > 0)
                    {
                        LoadDonorDetails(tabPageList);
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

        public void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSearchResult.Rows.Count > 0)
                {
                    FrmDisplayColumns frmDisplayColumns = new FrmDisplayColumns(searchParamExport);
                    frmDisplayColumns.ShowDialog();
                }
                else
                {
                    MessageBox.Show("No Records Found");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dgvSearchResult_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //Cursor.Current = Cursors.WaitCursor;
                //if (e.ColumnIndex == 0)
                //{
                //    if (e.RowIndex != -1)
                //    {
                //        DataGridViewCheckBoxCell donorSelection = (DataGridViewCheckBoxCell)dgvSearchResult.Rows[e.RowIndex].Cells["DonorSelection"];
                //        if (Convert.ToBoolean(donorSelection.Value))
                //        {
                //            donorSelection.Value = false;
                //        }
                //        else
                //        {
                //            donorSelection.Value = true;
                //        }
                //    }
                //}
                //Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvSearchResult_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvSearchResult.Rows)
            {
                if (row.Cells["DonorSSN"].Value != null && row.Cells["DonorSSN"].Value.ToString() != string.Empty)
                {
                    if (row.Cells["DonorSSN"].Value.ToString().Length == 11)
                    {
                        row.Cells["SSN"].Value = "***-**-" + row.Cells["DonorSSN"].Value.ToString().Substring(7);
                        row.Cells["SSN"].Tag = "DonorSSN";
                    }
                }

                if (row.Cells["DonorDateOfBirth"].Value != null && row.Cells["DonorDateOfBirth"].Value.ToString() != string.Empty)
                {
                    DateTime dob = Convert.ToDateTime(row.Cells["DonorDateOfBirth"].Value);
                    if (dob != DateTime.MinValue)
                    {
                        row.Cells["DOB"].Value = dob.ToString("MM/dd/yyyy");
                    }
                }

                if (row.Cells["SpecimenDate"].Value != null && row.Cells["SpecimenDate"].Value.ToString() != string.Empty)
                {
                    DateTime specimenDate = Convert.ToDateTime(row.Cells["SpecimenDate"].Value);
                    if (specimenDate != DateTime.MinValue)
                    {
                        row.Cells["SpecimenDateValue"].Value = specimenDate.ToString("MM/dd/yyyy");
                    }
                }

                if (row.Cells["MROTypeId"].Value != null && row.Cells["MROTypeId"].Value.ToString() != string.Empty)
                {
                    ClientMROTypes clientMRoTypes = (ClientMROTypes)((int)row.Cells["MROTypeId"].Value);
                    row.Cells["MROType"].Value = clientMRoTypes.ToString();
                }

                if (row.Cells["PaymentTypeId"].Value != null && row.Cells["PaymentTypeId"].Value.ToString() != string.Empty)
                {
                    ClientPaymentTypes clientPaymentTypes = (ClientPaymentTypes)((int)row.Cells["PaymentTypeId"].Value);
                    row.Cells["PaymentType"].Value = clientPaymentTypes.ToDescriptionString();
                }

                if (row.Cells["ReasonForTestId"].Value != null && row.Cells["ReasonForTestId"].Value.ToString() != string.Empty)
                {
                    TestInfoReasonForTest testInfoReasonForTest = (TestInfoReasonForTest)((int)row.Cells["ReasonForTestId"].Value);
                    row.Cells["TestReason"].Value = testInfoReasonForTest.ToDescriptionString();
                }
                if (row.Cells["TestOverallResult"].Value != null && row.Cells["TestOverallResult"].Value.ToString() != string.Empty)
                {
                    OverAllTestResult result = (OverAllTestResult)((int)row.Cells["TestOverallResult"].Value);
                    if (result.ToString() != "None")
                    {
                        row.Cells["Result"].Value = result.ToDescriptionString();
                    }
                    else
                    {
                        row.Cells["Result"].Value = " ";
                    }
                }

                if (row.Cells["PaymentReceived"].Value.ToString() == "1")  //&& row.Cells["DonorRegistrationStatus"].Value != "Pre-Registered"
                {
                    if (row.Cells["PaymentMethodId"].Value != null && row.Cells["PaymentMethodId"].Value.ToString() != string.Empty)
                    {
                        PaymentMethod paymentMethod = (PaymentMethod)((int)row.Cells["PaymentMethodId"].Value);
                        row.Cells["PaymentMode"].Value = paymentMethod.ToString();
                    }
                }

                if (row.Cells["DonorTestRegisteredDate"].Value != null && row.Cells["DonorTestRegisteredDate"].Value.ToString() != string.Empty)
                {
                    DateTime paymentDate = Convert.ToDateTime(row.Cells["DonorTestRegisteredDate"].Value);
                    if (paymentDate != DateTime.MinValue)
                    {
                        row.Cells["DonorTestRegisteredDateValue"].Value = paymentDate.ToString("MM/dd/yyyy");
                    }
                }

                //if (row.Cells["notified_by_email_timestamp"].Value != null && row.Cells["notified_by_email_timestamp"].Value.ToString() != string.Empty)
                //{
                //    if (row.Cells["notified_by_email_timestamp"].Value.ToString().Length == 11)
                //    {
                //        row.Cells["DonorDateNotifiedData"].Value = row.Cells["notified_by_email_timestamp"].Value.ToString();
                //    }
                //}
                if (row.Cells["PaymentDate"].Value != null && row.Cells["PaymentDate"].Value.ToString() != string.Empty)
                {
                    DateTime paymentDate = Convert.ToDateTime(row.Cells["PaymentDate"].Value);
                    if (paymentDate != DateTime.MinValue)
                    {
                        row.Cells["PaymentDateValue"].Value = paymentDate.ToString("MM/dd/yyyy");
                    }
                }
                //else
                //{
                //    row.Cells["PaymentMode"].Value = "";
                //}

                if (row.Cells["TestStatus"].Value != null && row.Cells["TestStatus"].Value.ToString() != string.Empty)
                {
                    DonorRegistrationStatus status = (DonorRegistrationStatus)((int)row.Cells["TestStatus"].Value);
                    row.Cells["Status"].Value = status.ToDescriptionString();
                }
                else if (row.Cells["DonorRegistrationStatus"].Value != null && row.Cells["DonorRegistrationStatus"].Value.ToString() != string.Empty)
                {
                    DonorRegistrationStatus status = (DonorRegistrationStatus)((int)row.Cells["DonorRegistrationStatus"].Value);
                    if (status == Enum.DonorRegistrationStatus.PreRegistration)
                    {
                        row.Cells["Status"].Value = "Pre-Registered";
                    }
                    else
                    {
                        row.Cells["Status"].Value = status.ToDescriptionString();
                    }
                }
            }

            this.selectedCounter.RowsAvailable = dgvSearchResult.Rows.Count > 0;
        }

        private void dgvSearchResult_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (e.RowIndex >= 0)
                {
                    Dictionary<string, string> tabPageList = new Dictionary<string, string>();

                    string donorId = dgvSearchResult.Rows[e.RowIndex].Cells["DonorId"].Value.ToString();
                    string donorTestInfoId = dgvSearchResult.Rows[e.RowIndex].Cells["DonorTestInfoId"].Value.ToString();
                    string donorFistName = dgvSearchResult.Rows[e.RowIndex].Cells["DonorFirstName"].Value.ToString();
                    string donorLastName = dgvSearchResult.Rows[e.RowIndex].Cells["DonorLastName"].Value.ToString();

                    donorTestInfoId = donorTestInfoId != string.Empty ? donorTestInfoId : "0";

                    if (donorId != string.Empty && donorTestInfoId != string.Empty)
                    {
                        string key = donorId + "#" + donorTestInfoId;
                        string value = donorFistName + " " + donorLastName;
                        if (!tabPageList.ContainsKey(key))
                        {
                            tabPageList.Add(key, value);
                        }

                        DataGridViewCheckBoxCell fieldSelection = (DataGridViewCheckBoxCell)dgvSearchResult.Rows[e.RowIndex].Cells["DonorSelection"];

                        if (Convert.ToBoolean(fieldSelection.Value))
                        {
                            fieldSelection.Value = false;
                        }
                        else
                        {
                            fieldSelection.Value = true;
                        }
                    }
                    Program._logger.Debug("Cell double click, opening..");
                    LoadDonorDetails(tabPageList);
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvSearchResult_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    if (dgvSearchResult.Rows.Count > 0)
                    {
                        if (dgvSearchResult.CurrentRow.Index >= 0)
                        {
                            Dictionary<string, string> tabPageList = new Dictionary<string, string>();

                            string donorId = dgvSearchResult.Rows[dgvSearchResult.CurrentRow.Index].Cells["DonorId"].Value.ToString();
                            string donorTestInfoId = dgvSearchResult.Rows[dgvSearchResult.CurrentRow.Index].Cells["DonorTestInfoId"].Value.ToString();
                            string donorFistName = dgvSearchResult.Rows[dgvSearchResult.CurrentRow.Index].Cells["DonorFirstName"].Value.ToString();
                            string donorLastName = dgvSearchResult.Rows[dgvSearchResult.CurrentRow.Index].Cells["DonorLastName"].Value.ToString();

                            donorTestInfoId = donorTestInfoId != string.Empty ? donorTestInfoId : "0";

                            if (donorId != string.Empty && donorTestInfoId != string.Empty)
                            {
                                string key = donorId + "#" + donorTestInfoId;
                                string value = donorFistName + " " + donorLastName;
                                if (!tabPageList.ContainsKey(key))
                                {
                                    tabPageList.Add(key, value);
                                }
                            }

                            LoadDonorDetails(tabPageList);
                        }
                    }
                    DoSearch();
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Program.EmailError(ex);
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            InitializeControls();
        }

        private void cmbSearchMonth_TextChanged(object sender, EventArgs e)
        {
            cmbSearchMonth.CausesValidation = false;
        }

        private void cmbSearchDate_TextChanged(object sender, EventArgs e)
        {
            cmbSearchDate.CausesValidation = false;
        }

        private void cmbSearchYear_TextChanged(object sender, EventArgs e)
        {
            cmbSearchYear.CausesValidation = false;
        }

        private void dgvSearchResult_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            DataGridViewColumn col = dgv.Columns[e.ColumnIndex];

            if (col.Name == "DOB")
            {
                DataGridViewColumn BirthDate = dgv.Columns["DonorDateOfBirth"];

                if (BirthDate.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    dgv.Sort(BirthDate, ListSortDirection.Descending);
                    BirthDate.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                    col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
                else
                {
                    dgv.Sort(BirthDate, ListSortDirection.Ascending);
                    BirthDate.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                    col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
            }
            else if (col.Name == "SSN")
            {
                DataGridViewColumn SSN = dgv.Columns["DonorSSN"];

                if (SSN.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    dgv.Sort(SSN, ListSortDirection.Descending);
                    SSN.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                    col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
                else
                {
                    dgv.Sort(SSN, ListSortDirection.Ascending);
                    SSN.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                    col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
            }
            else if (col.Name == "SpecimenDateValue")
            {
                DataGridViewColumn SpecimenDate = dgv.Columns["SpecimenDate"];

                if (SpecimenDate.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    dgv.Sort(SpecimenDate, ListSortDirection.Descending);
                    SpecimenDate.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                    col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
                else
                {
                    dgv.Sort(SpecimenDate, ListSortDirection.Ascending);
                    SpecimenDate.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                    col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
            }
            else if (col.Name == "PaymentDateValue")
            {
                DataGridViewColumn PaymentDateValue = dgv.Columns["PaymentDate"];

                if (PaymentDateValue.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    dgv.Sort(PaymentDateValue, ListSortDirection.Descending);
                    PaymentDateValue.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                    col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
                else
                {
                    dgv.Sort(PaymentDateValue, ListSortDirection.Ascending);
                    PaymentDateValue.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                    col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
            }
            else if (col.Name == "DonorTestRegisteredDateValue")
            {
                DataGridViewColumn DonorTestRegisteredDateValue = dgv.Columns["DonorTestRegisteredDate"];

                if (DonorTestRegisteredDateValue.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    dgv.Sort(DonorTestRegisteredDateValue, ListSortDirection.Descending);
                    DonorTestRegisteredDateValue.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                    col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
                else
                {
                    dgv.Sort(DonorTestRegisteredDateValue, ListSortDirection.Ascending);
                    DonorTestRegisteredDateValue.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                    col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
            }
            //else if (col.Name == "DonorDateNotifiedData")
            //{
            //    DataGridViewColumn DonorTestRegisteredDateValue = dgv.Columns["DonorDateNotifiedData"];

            //    if (DonorDateNotifiedData.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
            //    {
            //        dgv.Sort(DonorDateNotifiedData, ListSortDirection.Descending);
            //        DonorDateNotifiedData.HeaderCell.SortGlyphDirection = SortOrder.Descending;
            //        col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
            //    }
            //    else
            //    {
            //        dgv.Sort(DonorDateNotifiedData, ListSortDirection.Ascending);
            //        DonorDateNotifiedData.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
            //        col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
            //    }
            //}
            else if (col.Name == "MROType")
            {
                DataGridViewColumn MROType = dgv.Columns["MROTypeId"];

                if (MROType.HeaderCell.SortGlyphDirection == SortOrder.Descending)
                {
                    dgv.Sort(MROType, ListSortDirection.Ascending);
                    MROType.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                    col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    dgv.Sort(MROType, ListSortDirection.Descending);
                    MROType.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                    col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else if (col.Name == "PaymentType")
            {
                DataGridViewColumn PaymentType = dgv.Columns["PaymentTypeId"];

                if (PaymentType.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    dgv.Sort(PaymentType, ListSortDirection.Descending);
                    PaymentType.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                    col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
                else
                {
                    dgv.Sort(PaymentType, ListSortDirection.Ascending);
                    PaymentType.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                    col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
            }
            else if (col.Name == "TestReason")
            {
                DataGridViewColumn ReasonForTest = dgv.Columns["ReasonForTestId"];

                if (ReasonForTest.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    dgv.Sort(ReasonForTest, ListSortDirection.Descending);
                    ReasonForTest.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                    col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
                else
                {
                    dgv.Sort(ReasonForTest, ListSortDirection.Ascending);
                    ReasonForTest.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                    col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
            }
            else if (col.Name == "PaymentMode")
            {
                DataGridViewColumn PaymentMode = dgv.Columns["PaymentMethodId"];

                if (PaymentMode.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    dgv.Sort(PaymentMode, ListSortDirection.Descending);
                    PaymentMode.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                    col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
                else
                {
                    dgv.Sort(PaymentMode, ListSortDirection.Ascending);
                    PaymentMode.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                    col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
            }
            else if (col.Name == "Result")
            {
                DataGridViewColumn Results = dgv.Columns["TestOverallResult"];

                if (Results.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    dgv.Sort(Results, ListSortDirection.Descending);
                    Results.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                    col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
                else
                {
                    dgv.Sort(Results, ListSortDirection.Ascending);
                    Results.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                    col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
            }
            else if (col.Name == "Status")
            {
                DataGridViewColumn TestStatus = dgv.Columns["TestStatus"];

                if (TestStatus.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    dgv.Sort(TestStatus, ListSortDirection.Descending);
                    TestStatus.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                    col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
                else
                {
                    dgv.Sort(TestStatus, ListSortDirection.Ascending);
                    TestStatus.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                    col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
            }
            else if (col.Name == "Status")
            {
                DataGridViewColumn DonorRegistrationStatus = dgv.Columns["DonorRegistrationStatus"];

                if (DonorRegistrationStatus.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    dgv.Sort(DonorRegistrationStatus, ListSortDirection.Descending);
                    DonorRegistrationStatus.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                    col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
                else
                {
                    dgv.Sort(DonorRegistrationStatus, ListSortDirection.Ascending);
                    DonorRegistrationStatus.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                    col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
            }
        }

        private void btnAddRemove_Click(object sender, EventArgs e)
        {
            // InitializeControls();
            FrmAddRemoveColumns frmAddRemoveColumns = new FrmAddRemoveColumns(this);
            AddRemoves();
            frmAddRemoveColumns.Show();
            //if (frmAddRemoveColumns.IsFormValidate == false)
            //{
            //    //  MessageBox.Show("Under Development");
            //    AddRemoves();
            //    frmAddRemoveColumns.Show();
            //}
            //else if (frmAddRemoveColumns.IsFormValidate == true)
            //{
            //    // MessageBox.Show("Under Development_1");
            //    AddRemoves();
            //    frmAddRemoveColumns.Close();
            //}
            // MessageBox.Show("Under Development");
        }

        private void dgvSearchResult_MouseDown(object sender, MouseEventArgs e)
        {
            //try
            //{
            //    dgvSearchResult.DoDragDrop(dgvSearchResult, DragDropEffects.Copy);

            //    // Get the index of the item the mouse is below.
            //    var hittestInfo = dgvSearchResult.HitTest(e.X, e.Y);

            //    if (hittestInfo.RowIndex == -1 && hittestInfo.ColumnIndex != -1)
            //    {
            //        if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            //        {
            //            if (hittestInfo.ColumnIndex != 0)
            //            {
            //                if (dgvSearchResult.Columns["DonorFirstName"].Index == hittestInfo.ColumnIndex)
            //                {
            //                    if (dgvSearchResult.Columns[1].Name == "DonorFirstName")
            //                    {
            //                        dgvSearchResult.Columns[1].Visible = false;
            //                    }
            //                }

            //                if (dgvSearchResult.Columns["DonorLastName"].Index == hittestInfo.ColumnIndex)
            //                {
            //                    if (dgvSearchResult.Columns[2].Name == "DonorLastName")
            //                    {
            //                        dgvSearchResult.Columns[2].Visible = false;
            //                    }
            //                }

            //                if (dgvSearchResult.Columns["SSN"].Index == hittestInfo.ColumnIndex)
            //                {
            //                    if (dgvSearchResult.Columns[3].Name == "SSN")
            //                    {
            //                        dgvSearchResult.Columns[3].Visible = false;
            //                    }
            //                }

            //                if (dgvSearchResult.Columns["DOB"].Index == hittestInfo.ColumnIndex)
            //                {
            //                    if (dgvSearchResult.Columns[4].Name == "DOB")
            //                    {
            //                        dgvSearchResult.Columns[4].Visible = false;
            //                    }
            //                }

            //                if (dgvSearchResult.Columns["SpecimenID"].Index == hittestInfo.ColumnIndex)
            //                {
            //                    if (dgvSearchResult.Columns[5].Name == "SpecimenID")
            //                    {
            //                        dgvSearchResult.Columns[5].Visible = false;
            //                    }
            //                }

            //                if (dgvSearchResult.Columns["ClientName"].Index == hittestInfo.ColumnIndex)
            //                {
            //                    if (dgvSearchResult.Columns[7].Name == "ClientName")
            //                    {
            //                        dgvSearchResult.Columns[7].Visible = false;
            //                    }
            //                }

            //                if (dgvSearchResult.Columns["DepartmentName"].Index == hittestInfo.ColumnIndex)
            //                {
            //                    if (dgvSearchResult.Columns[8].Name == "DepartmentName")
            //                    {
            //                        dgvSearchResult.Columns[8].Visible = false;
            //                    }
            //                }

            //                if (dgvSearchResult.Columns["Status"].Index == hittestInfo.ColumnIndex)
            //                {
            //                    if (dgvSearchResult.Columns[9].Name == "Status")
            //                    {
            //                        dgvSearchResult.Columns[9].Visible = false;
            //                    }
            //                }

            //                if (dgvSearchResult.Columns["TestReason"].Index == hittestInfo.ColumnIndex)
            //                {
            //                    if (dgvSearchResult.Columns[14].Name == "TestReason")
            //                    {
            //                        dgvSearchResult.Columns[14].Visible = false;
            //                    }
            //                }

            //                if (dgvSearchResult.Columns["DonorCity"].Index == hittestInfo.ColumnIndex)
            //                {
            //                    if (dgvSearchResult.Columns[15].Name == "DonorCity")
            //                    {
            //                        dgvSearchResult.Columns[15].Visible = false;
            //                    }
            //                }

            //                if (dgvSearchResult.Columns["ZipCode"].Index == hittestInfo.ColumnIndex)
            //                {
            //                    if (dgvSearchResult.Columns[16].Name == "ZipCode")
            //                    {
            //                        dgvSearchResult.Columns[16].Visible = false;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message.ToString());
            //    Cursor.Current = Cursors.Default;
            //}
        }

        private void dgvSearchResult_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                //// The mouse locations are relative to the screen, so they must be
                //// converted to client coordinates.
                Point clientPoint = dgvSearchResult.PointToClient(new Point(e.X, e.Y));

                // If the drag operation was a copy then add the row to the other control.
                if (e.Effect == DragDropEffects.Copy)
                {
                    if (e.Data.GetDataPresent(typeof(System.String)))
                    {
                        string cellvalue = e.Data.GetData(typeof(string)) as string;

                        var hittest = dgvSearchResult.HitTest(clientPoint.X, clientPoint.Y);

                        if (hittest.RowIndex == -1 && hittest.ColumnIndex != -1)
                        {
                            if (cellvalue == "First Name")
                            {
                                if (dgvSearchResult.Columns[1].Name == "DonorFirstName")
                                {
                                    dgvSearchResult.Columns[1].Visible = true;
                                }
                            }

                            if (cellvalue == "Last Name")
                            {
                                if (dgvSearchResult.Columns[2].Name == "DonorLastName")
                                {
                                    dgvSearchResult.Columns[2].Visible = true;
                                }
                            }

                            if (cellvalue.Trim() == "SSN")
                            {
                                if (dgvSearchResult.Columns[3].Name == "SSN")
                                {
                                    dgvSearchResult.Columns[3].Visible = true;
                                }
                            }

                            if (cellvalue.Trim() == "DOB")
                            {
                                if (dgvSearchResult.Columns[4].Name == "DOB")
                                {
                                    dgvSearchResult.Columns[4].Visible = true;
                                }
                            }

                            if (cellvalue.Trim() == "Specimen ID")
                            {
                                if (dgvSearchResult.Columns[5].Name == "SpecimenID")
                                {
                                    dgvSearchResult.Columns[5].Visible = true;
                                }
                            }

                            if (cellvalue.Trim() == "Specimen Date")
                            {
                                if (dgvSearchResult.Columns[6].Name == "SpecimenDateValue")
                                {
                                    dgvSearchResult.Columns[6].Visible = true;
                                }
                            }

                            if (cellvalue.Trim() == "Client")
                            {
                                if (dgvSearchResult.Columns[7].Name == "ClientName")
                                {
                                    dgvSearchResult.Columns[7].Visible = true;
                                }
                            }

                            if (cellvalue.Trim() == "Department")
                            {
                                if (dgvSearchResult.Columns[8].Name == "DepartmentName")
                                {
                                    dgvSearchResult.Columns[8].Visible = true;
                                }
                            }

                            if (cellvalue.Trim() == "Status")
                            {
                                if (dgvSearchResult.Columns[9].Name == "Status")
                                {
                                    dgvSearchResult.Columns[9].Visible = true;
                                }
                            }

                            if (cellvalue.Trim() == "Payment Mode")
                            {
                                if (dgvSearchResult.Columns[10].Name == "PaymentMode")
                                {
                                    dgvSearchResult.Columns[10].Visible = true;
                                }
                            }

                            if (cellvalue.Trim() == "Amount")
                            {
                                if (dgvSearchResult.Columns[12].Name == "PaymentAmount")
                                {
                                    dgvSearchResult.Columns[12].Visible = true;
                                }
                            }

                            if (cellvalue.Trim() == "Result")
                            {
                                if (dgvSearchResult.Columns[13].Name == "Result")
                                {
                                    dgvSearchResult.Columns[13].Visible = true;
                                }
                            }

                            if (cellvalue.Trim() == "Test Reason")
                            {
                                if (dgvSearchResult.Columns[14].Name == "TestReason")
                                {
                                    dgvSearchResult.Columns[14].Visible = true;
                                }
                            }

                            if (cellvalue.Trim() == "Donor City")
                            {
                                if (dgvSearchResult.Columns[15].Name == "DonorCity")
                                {
                                    dgvSearchResult.Columns[15].Visible = true;
                                }
                            }

                            if (cellvalue.Trim() == "Zip Code")
                            {
                                if (dgvSearchResult.Columns[16].Name == "ZipCode")
                                {
                                    dgvSearchResult.Columns[16].Visible = true;
                                }
                            }

                            if (cellvalue.Trim() == "MRO Type")
                            {
                                if (dgvSearchResult.Columns[17].Name == "MROType")
                                {
                                    dgvSearchResult.Columns[17].Visible = true;
                                }
                            }
                            if (cellvalue.Trim() == "Payment Type")
                            {
                                if (dgvSearchResult.Columns[18].Name == "PaymentType")
                                {
                                    dgvSearchResult.Columns[18].Visible = true;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Drag to column header");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                Cursor.Current = Cursors.Default;
            }
        }

        private void dgvSearchResult_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void dgvSearchResult_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void lblFirstName_MouseDown(object sender, MouseEventArgs e)
        {
            //if (lblFirstName.Text != string.Empty)
            //{
            //    lblFirstName.DoDragDrop(lblFirstName.Text, DragDropEffects.Copy);
            //}
        }

        #endregion Event Methods

        #region Private Methods

        private void InitializeControls()
        {
            try
            {
                BindField(this.btnNotify, "Enabled", this.selectedCounter, "Enabled");
                BindField(this.btnNotifyNow, "Enabled", this.selectedCounter, "Enabled");
                BindField(this.btnSetNotified, "Enabled", this.selectedCounter, "Enabled");
                BindField(this.btnSelectRandom, "Enabled", this.selectedCounter, "RowsAvailable");

                dgvSearchResult.AutoGenerateColumns = false;
                dgvSearchResult.DataSource = null;

                txtFirstName.Text = string.Empty;
                txtLastName.Text = string.Empty;
                txtSSN.Text = string.Empty;
                // txtDOB.Text = string.Empty;
                // ----Donor Search-----//
                cmbSearchYear.Items.Clear();
                var myDate = DateTime.Now;
                var newDate = myDate.AddYears(-125).Year;
                for (int i = newDate; i <= DateTime.Now.Year; i++)
                {
                    cmbSearchYear.Items.Add(i);
                }
                cmbSearchYear.Items.Insert(0, "YYYY");
                cmbSearchMonth.SelectedIndex = 0;
                cmbSearchDate.SelectedIndex = 0;
                cmbSearchYear.SelectedIndex = 0;

                txtCity.Text = string.Empty;
                txtZipCode.Text = string.Empty;
                txtSpecimenId.Text = string.Empty;

                cmbTestReason.SelectedIndex = 0;
                cmbClient.SelectedIndex = 0;
                cmbDepartment.SelectedIndex = 0;
                cmbStatus.SelectedIndex = 0;
                cmbTestType.SelectedIndex = 0;
                rbtnNone.Checked = true;
                chkIncludeArchived.Checked = false;
                chkwalkin.Checked = false;
                chkShowAll.Checked = false;

                dtpFromDate.Visible = false;
                dtpToDate.Visible = false;
                dtpFromDate.Value = DateTime.Today;
                dtpToDate.Value = DateTime.Today;

                LoadClient();
                LoadClientDepartment();

                // DoSearch();
                for (int j = 0; j < dgvSearchResult.Columns.Count; j++)
                {
                    dgvSearchResult.Columns[0].Visible = true;

                    if (dgvSearchResult.Columns[j].HeaderText == "First Name")
                    {
                        dgvSearchResult.Columns[j].Visible = true;
                    }

                    if (dgvSearchResult.Columns[j].HeaderText == "Last Name")
                    {
                        dgvSearchResult.Columns[j].Visible = true;
                    }

                    if (dgvSearchResult.Columns[j].HeaderText == "SSN")
                    {
                        dgvSearchResult.Columns[j].Visible = true;
                    }

                    if (dgvSearchResult.Columns[j].HeaderText == "DOB")
                    {
                        dgvSearchResult.Columns[j].Visible = true;
                    }

                    if (dgvSearchResult.Columns[j].HeaderText == "Specimen ID")
                    {
                        dgvSearchResult.Columns[j].Visible = true;
                    }

                    if (dgvSearchResult.Columns[j].HeaderText == "Specimen Date")
                    {
                        dgvSearchResult.Columns[j].Visible = true;
                    }

                    if (dgvSearchResult.Columns[j].HeaderText == "Client")
                    {
                        dgvSearchResult.Columns[j].Visible = true;
                    }

                    if (dgvSearchResult.Columns[j].HeaderText == "Department")
                    {
                        dgvSearchResult.Columns[j].Visible = true;
                    }

                    if (dgvSearchResult.Columns[j].HeaderText == "Status")
                    {
                        dgvSearchResult.Columns[j].Visible = true;
                    }

                    if (dgvSearchResult.Columns[j].HeaderText == "Payment Mode")
                    {
                        dgvSearchResult.Columns[j].Visible = true;
                    }

                    if (dgvSearchResult.Columns[j].HeaderText == "Amount")
                    {
                        dgvSearchResult.Columns[j].Visible = true;
                    }

                    if (dgvSearchResult.Columns[j].HeaderText == "Result")
                    {
                        dgvSearchResult.Columns[j].Visible = true;
                    }

                    if (dgvSearchResult.Columns[j].HeaderText == "Test Reason")
                    {
                        dgvSearchResult.Columns[j].Visible = true;
                    }

                    if (dgvSearchResult.Columns[j].HeaderText == "Donor City")
                    {
                        dgvSearchResult.Columns[j].Visible = true;
                    }

                    if (dgvSearchResult.Columns[j].HeaderText == "Zip Code")
                    {
                        dgvSearchResult.Columns[j].Visible = true;
                    }

                    if (dgvSearchResult.Columns[j].HeaderText == "MRO Type")
                    {
                        dgvSearchResult.Columns[j].Visible = true;
                    }

                    if (dgvSearchResult.Columns[j].HeaderText == "Payment Type")
                    {
                        dgvSearchResult.Columns[j].Visible = true;
                    }

                    // Filters:

                    dtBeforeFilter.Value = DateTime.Now.Date.AddDays(1);
                    dtAfterFilter.Value = DateTime.Now.Date.AddDays(this.FilterDayInterval);
                    cbFilter.SelectedIndex = cbFilter.FindString("None");
                    cbPresets.SelectedIndex = 0;



                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadClient()
        {
            try
            {
                List<Client> clientList = clientBL.GetList("1");

                Client tmpClient = new Client();
                tmpClient.ClientId = 0;
                tmpClient.ClientName = "(Select Client)";

                clientList.Insert(0, tmpClient);

                cmbClient.DataSource = clientList;
                cmbClient.ValueMember = "ClientId";
                cmbClient.DisplayMember = "ClientName";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadClientDepartment()
        {
            try
            {
                List<ClientDepartment> clientDepartmentList = new List<ClientDepartment>();

                if (cmbClient.SelectedIndex > 0)
                {
                    clientDepartmentList = clientBL.GetClientDepartmentList(Convert.ToInt32(cmbClient.SelectedValue), "1");
                }

                ClientDepartment tmpclientDepartment = new ClientDepartment();
                tmpclientDepartment.ClientDepartmentId = 0;
                tmpclientDepartment.DepartmentName = "(Select Department)";

                clientDepartmentList.Insert(0, tmpclientDepartment);

                cmbDepartment.DataSource = clientDepartmentList;
                cmbDepartment.ValueMember = "ClientDepartmentId";
                cmbDepartment.DisplayMember = "DepartmentName";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DoSearch()
        {
            try
            {
                Dictionary<string, string> searchParam = new Dictionary<string, string>();
                // Filters

                searchParam.Add("cbFilter", cbFilter.Items[cbFilter.SelectedIndex].ToString());
                searchParam.Add("dtAfterFilter", dtAfterFilter.Value.ToString("yyyy-MM-dd"));
                searchParam.Add("dtBeforeFilter", dtBeforeFilter.Value.ToString("yyyy-MM-dd"));




                if (txtFirstName.Text.Trim() != string.Empty)
                {
                    searchParam.Add("FirstName", "%" + txtFirstName.Text.Trim() + "%");
                }

                if (txtLastName.Text.Trim() != string.Empty)
                {
                    searchParam.Add("LastName", "%" + txtLastName.Text.Trim() + "%");
                }

                if (txtSSN.Text.Trim() != string.Empty)
                {
                    if (txtSSN.Text.Replace("_", "").Replace("-", "").Trim() != string.Empty)
                    {
                        searchParam.Add("SSN", "%" + txtSSN.Text.Trim() + "%");
                    }
                }

                if (cmbSearchYear.SelectedIndex != 0 && cmbSearchMonth.SelectedIndex != 0 && cmbSearchDate.SelectedIndex != 0)
                {
                    string donorDOB = cmbSearchYear.Text + '-' + cmbSearchMonth.Text + '-' + cmbSearchDate.Text;
                    if (donorDOB != null && donorDOB != "--")
                    {
                        try
                        {
                            DateTime dt = Convert.ToDateTime(donorDOB.ToString());
                            if (dt > DateTime.Now)
                            {
                                Cursor.Current = Cursors.Default;
                                MessageBox.Show("Invalid DOB.");
                                cmbSearchMonth.Focus();
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("Invalid format of DOB.");
                            cmbSearchMonth.Focus();
                            return;
                        }
                        searchParam.Add("DOB", donorDOB.Trim());
                    }
                }

                //if (txtDOB.Text.Replace("_", "").Replace("/", "").Trim() != string.Empty)
                //{
                //    try
                //    {
                //        DateTime dt = Convert.ToDateTime(txtDOB.Text);
                //        if (dt > DateTime.Now)
                //        {
                //            Cursor.Current = Cursors.Default;
                //            MessageBox.Show("Invalid DOB.");
                //            txtDOB.Focus();
                //            return;
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Cursor.Current = Cursors.Default;
                //        MessageBox.Show("Invalid format of DOB.");
                //        txtDOB.Focus();
                //        return;
                //    }
                //    searchParam.Add("DOB", txtDOB.Text.Trim());
                //}

                if (txtCity.Text.Trim() != string.Empty)
                {
                    searchParam.Add("City", "%" + txtCity.Text.Trim() + "%");
                }

                if (txtZipCode.Text.Trim() != string.Empty)
                {
                    string ZipCode = txtZipCode.Text.Trim();
                    ZipCode = ZipCode.EndsWith("-") ? ZipCode.Replace("-", "") : ZipCode;

                    if (!Program.regexZipCode.IsMatch(ZipCode))
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Invalid format of Zip Code.");
                        txtZipCode.Focus();
                        return;
                    }
                    else
                    {
                        searchParam.Add("ZipCode", txtZipCode.Text.Trim());
                    }
                }

                if (txtSpecimenId.Text.Trim() != string.Empty)
                {
                    searchParam.Add("SpecimenId", "%" + txtSpecimenId.Text.Trim() + "%");
                }

                if (cmbTestReason.SelectedIndex > 0)
                {
                    searchParam.Add("TestReason", cmbTestReason.SelectedIndex.ToString());
                }

                if (cmbClient.SelectedIndex > 0)
                {
                    searchParam.Add("Client", cmbClient.SelectedValue.ToString());
                }

                if (cmbDepartment.SelectedIndex > 0)
                {
                    searchParam.Add("Department", cmbDepartment.SelectedValue.ToString());
                }

                if (cmbStatus.SelectedIndex > 0)
                {
                    searchParam.Add("Status", cmbStatus.SelectedIndex.ToString());
                }

                if (cmbTestType.SelectedIndex > 0)
                {
                    searchParam.Add("TestCategory", cmbTestType.SelectedIndex.ToString());
                }

                searchParam.Add("IncludeArchive", chkIncludeArchived.Checked.ToString());

                searchParam.Add("Walkin", chkwalkin.Checked.ToString());

                searchParam.Add("ShowAll", chkShowAll.Checked.ToString());

                if (chkShowAll.Checked == true)
                {
                    showAll = true;
                }
                else
                {
                    showAll = false;
                }

                if (rbtnDateRange.Checked)
                {
                    DateTime fromDate = dtpFromDate.Value;
                    DateTime toDate = dtpToDate.Value;

                    if (fromDate > toDate)
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Invalid range of dates.");
                        dtpFromDate.Focus();
                        return;
                    }
                    if (fromDate > DateTime.Today)
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Invalid date.");
                        dtpFromDate.Focus();
                        return;
                    }

                    if (toDate > DateTime.Today)
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Invalid date.");
                        dtpToDate.Focus();
                        return;
                    }
                    searchParam.Add("NoOfDays", "DR#" + fromDate.ToString("MM/dd/yyyy") + "#" + toDate.ToString("MM/dd/yyyy"));
                }
                else if (rbtnLast3Days.Checked)
                {
                    searchParam.Add("NoOfDays", "3");
                }
                else if (rbtnLast7Days.Checked)
                {
                    searchParam.Add("NoOfDays", "7");
                }
                else if (rbtnLast30Days.Checked)
                {
                    searchParam.Add("NoOfDays", "30");
                }
                else if (rbtnLast60Days.Checked)
                {
                    searchParam.Add("NoOfDays", "60");
                }
                else if (rbtnLast90Days.Checked)
                {
                    searchParam.Add("NoOfDays", "90");
                }

                searchParamExport = searchParam;
                DonorBL donorBL = new DonorBL(Program._logger);
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                DataTable dtDonors = donorBL.SearchDonor(searchParam, Program.currentUserType, Program.currentUserId, Program.currentUserName, showAll);
                stopWatch.Stop();
                Program._logger.Debug($"DT Table return in seconds: {stopWatch.Elapsed.TotalSeconds.ToString()}");
                dgvSearchResult.DataSource = dtDonors;

                if (dgvSearchResult.Rows.Count > 0)
                {
                    btnSelectAll.Enabled = true;
                    btnDeselectAll.Enabled = true;
                    btnViewSelected.Enabled = true;
                    btnViewAll.Enabled = true;
                    btnExport.Enabled = true;
                    btnAddRemove.Enabled = true;

                    dgvSearchResult.Focus();
                }
                else
                {
                    btnSelectAll.Enabled = false;
                    btnDeselectAll.Enabled = false;
                    btnViewSelected.Enabled = false;
                    btnViewAll.Enabled = false;
                    btnExport.Enabled = false;
                    btnAddRemove.Enabled = false;

                    btnSearch.Focus();
                    MessageBox.Show("No Records Found");
                }
            }
            catch (Exception ex)
            {
                Program._logger.Error(ex.ToString());
                if (ex.InnerException != null) Program._logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) Program._logger.Error(ex.StackTrace.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadDonorDetails(Dictionary<string, string> tabPageList)
        {
            try
            {
                string firstTabTag = string.Empty;

                foreach (KeyValuePair<string, string> item in tabPageList)
                {
                    firstTabTag = item.Key;
                    break;
                }
                if (Program.frmMain.frmDonorDetails == null)
                {
                    Program._logger.Debug($"opening frmDonorDetails from LoadDonorDetails in Donor Search");

                    Program.frmMain.frmDonorDetails = new FrmDonorDetails();
                    Program.frmMain.frmDonorDetails.MdiParent = Program.frmMain;
                }

                Program.frmMain.frmDonorDetails.Show();

                int index = 0;

                foreach (KeyValuePair<string, string> tabPage in tabPageList)
                {
                    int tabIndex = Program.frmMain.frmDonorDetails.AddDonorNamesTab(tabPage.Value, tabPage.Key);

                    if (index == 0)
                    {
                        index = tabIndex;
                    }
                }
                if (index == 0)
                {
                    Program.frmMain.frmDonorDetails.LoadTabDetails(index);
                    Program.frmMain.frmDonorDetails.BringToFront();
                }
                else
                {
                    Program.frmMain.frmDonorDetails.LoadTabDetails(index - 1);
                    Program.frmMain.frmDonorDetails.BringToFront();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void AddRemoves()
        {
            InitializeControls();
            bool IsMinOneAvail = false;
            FrmAddRemoveColumns frmAddRemoveColumns = new FrmAddRemoveColumns(this);
            if (frmAddRemoveColumns.IsFormValidate == false)
            {
                if (frmAddRemoveColumns.IsValidate == true)
                {
                    DonorBL donorBL = new DonorBL();
                    DataTable dtcolumns = donorBL.GetColumnName();

                    if (dtcolumns.Rows.Count < 17)
                    {
                        for (int i = 0; i < dtcolumns.Rows.Count; i++)
                        {
                            for (int j = 0; j < dgvSearchResult.Columns.Count; j++)
                            {
                                if (dtcolumns.Rows[i]["ColumnName"].ToString() == "First Name")
                                {
                                    if (dgvSearchResult.Columns[j].Name == "DonorFirstName")
                                    {
                                        dgvSearchResult.Columns[j].Visible = false;
                                        IsMinOneAvail = true;
                                    }
                                }

                                if (dtcolumns.Rows[i]["ColumnName"].ToString() == "Last Name")
                                {
                                    if (dgvSearchResult.Columns[j].Name == "DonorLastName")
                                    {
                                        dgvSearchResult.Columns[j].Visible = false;
                                        IsMinOneAvail = true;
                                    }
                                    // dgvSearchResult.Columns["Last Name"].Visible = false;
                                }

                                if (dtcolumns.Rows[i]["ColumnName"].ToString() == "SSN")
                                {
                                    if (dgvSearchResult.Columns[j].Name == "SSN")
                                    {
                                        dgvSearchResult.Columns[j].Visible = false;
                                        IsMinOneAvail = true;
                                    }
                                    // dgvSearchResult.Columns["SSN"].Visible = false;
                                }

                                if (dtcolumns.Rows[i]["ColumnName"].ToString() == "DOB")
                                {
                                    if (dgvSearchResult.Columns[j].Name == "DOB")
                                    {
                                        dgvSearchResult.Columns[j].Visible = false;
                                        IsMinOneAvail = true;
                                    }
                                    //dgvSearchResult.Columns["DOB"].Visible = false;
                                }

                                if (dtcolumns.Rows[i]["ColumnName"].ToString() == "Specimen ID")
                                {
                                    if (dgvSearchResult.Columns[j].Name == "SpecimenID")
                                    {
                                        dgvSearchResult.Columns[j].Visible = false;
                                        IsMinOneAvail = true;
                                    }
                                    // dgvSearchResult.Columns["SpecimenID"].Visible = false;
                                }

                                if (dtcolumns.Rows[i]["ColumnName"].ToString() == "Specimen Date")
                                {
                                    if (dgvSearchResult.Columns[j].Name == "SpecimenDateValue")
                                    {
                                        dgvSearchResult.Columns[j].Visible = false;
                                        IsMinOneAvail = true;
                                    }
                                    //  dgvSearchResult.Columns["Specimen Date"].Visible = false;
                                }

                                if (dtcolumns.Rows[i]["ColumnName"].ToString() == "Client")
                                {
                                    if (dgvSearchResult.Columns[j].Name == "ClientName")
                                    {
                                        dgvSearchResult.Columns[j].Visible = false;
                                        IsMinOneAvail = true;
                                    }
                                    //dgvSearchResult.Columns["Client"].Visible = false;
                                }

                                if (dtcolumns.Rows[i]["ColumnName"].ToString() == "Department")
                                {
                                    if (dgvSearchResult.Columns[j].Name == "DepartmentName")
                                    {
                                        dgvSearchResult.Columns[j].Visible = false;
                                        IsMinOneAvail = true;
                                    }
                                    //  dgvSearchResult.Columns["Department"].Visible = false;
                                }

                                if (dtcolumns.Rows[i]["ColumnName"].ToString() == "Status")
                                {
                                    if (dgvSearchResult.Columns[j].Name == "Status")
                                    {
                                        dgvSearchResult.Columns[j].Visible = false;
                                        IsMinOneAvail = true;
                                    }
                                    // dgvSearchResult.Columns["Status"].Visible = false;
                                }

                                if (dtcolumns.Rows[i]["ColumnName"].ToString() == "Payment Mode")
                                {
                                    if (dgvSearchResult.Columns[j].Name == "PaymentMode")
                                    {
                                        dgvSearchResult.Columns[j].Visible = false;
                                        IsMinOneAvail = true;
                                    }
                                    // dgvSearchResult.Columns["Payment Mode"].Visible = false;
                                }

                                if (dtcolumns.Rows[i]["ColumnName"].ToString() == "Amount")
                                {
                                    if (dgvSearchResult.Columns[j].Name == "PaymentAmount")
                                    {
                                        dgvSearchResult.Columns[j].Visible = false;
                                        IsMinOneAvail = true;
                                    }
                                    // dgvSearchResult.Columns["Amount"].Visible = false;
                                }

                                if (dtcolumns.Rows[i]["ColumnName"].ToString() == "Result")
                                {
                                    if (dgvSearchResult.Columns[j].Name == "Result")
                                    {
                                        dgvSearchResult.Columns[j].Visible = false;
                                        IsMinOneAvail = true;
                                    }
                                    // dgvSearchResult.Columns["Result"].Visible = false;
                                }

                                if (dtcolumns.Rows[i]["ColumnName"].ToString() == "Test Reason")
                                {
                                    if (dgvSearchResult.Columns[j].Name == "TestReason")
                                    {
                                        dgvSearchResult.Columns[j].Visible = false;
                                        IsMinOneAvail = true;
                                    }

                                    // dgvSearchResult.Columns["Test Reason"].Visible = false;
                                }

                                if (dtcolumns.Rows[i]["ColumnName"].ToString() == "Donor City")
                                {
                                    if (dgvSearchResult.Columns[j].Name == "DonorCity")
                                    {
                                        dgvSearchResult.Columns[j].Visible = false;
                                        IsMinOneAvail = true;
                                    }
                                    // dgvSearchResult.Columns["Donor City"].Visible = false;
                                }

                                if (dtcolumns.Rows[i]["ColumnName"].ToString() == "Zip Code")
                                {
                                    if (dgvSearchResult.Columns[j].Name == "ZipCode")
                                    {
                                        dgvSearchResult.Columns[j].Visible = false;
                                        IsMinOneAvail = true;
                                    }

                                    // dgvSearchResult.Columns["Zip Code"].Visible = false;
                                }

                                if (dtcolumns.Rows[i]["ColumnName"].ToString() == "MRO Type")
                                {
                                    if (dgvSearchResult.Columns[j].Name == "MROType")
                                    {
                                        dgvSearchResult.Columns[j].Visible = false;
                                        IsMinOneAvail = true;
                                    }

                                    //  dgvSearchResult.Columns["MRO Type"].Visible = false;
                                }

                                if (dtcolumns.Rows[i]["ColumnName"].ToString() == "Payment Type")
                                {
                                    if (dgvSearchResult.Columns[j].Name == "PaymentType")
                                    {
                                        dgvSearchResult.Columns[j].Visible = false;
                                        IsMinOneAvail = true;
                                    }

                                    // dgvSearchResult.Columns["Payment Type"].Visible = false;
                                }
                            }
                        }
                        DoSearch();
                    }

                    if (IsMinOneAvail == false)
                    {
                        if (dgvSearchResult.Columns[0].Name == "DonorSelection")
                        {
                            //dgvSearchResult.Columns[0].Visible = false;
                            dgvSearchResult.Columns[0].Visible = true;
                            dgvSearchResult.Columns[0].ReadOnly = true;

                            for (int j = 0; j < dgvSearchResult.Columns.Count; j++)
                            {
                                if (dgvSearchResult.Columns[j].HeaderText == "First Name")
                                {
                                    dgvSearchResult.Columns[j].Visible = false;
                                }

                                if (dgvSearchResult.Columns[j].HeaderText == "Last Name")
                                {
                                    dgvSearchResult.Columns[j].Visible = false;
                                }

                                if (dgvSearchResult.Columns[j].HeaderText == "SSN")
                                {
                                    dgvSearchResult.Columns[j].Visible = false;
                                }

                                if (dgvSearchResult.Columns[j].HeaderText == "DOB")
                                {
                                    dgvSearchResult.Columns[j].Visible = false;
                                }

                                if (dgvSearchResult.Columns[j].HeaderText == "Specimen ID")
                                {
                                    dgvSearchResult.Columns[j].Visible = false;
                                }

                                if (dgvSearchResult.Columns[j].HeaderText == "Specimen Date")
                                {
                                    dgvSearchResult.Columns[j].Visible = false;
                                }

                                if (dgvSearchResult.Columns[j].HeaderText == "Client")
                                {
                                    dgvSearchResult.Columns[j].Visible = false;
                                }

                                if (dgvSearchResult.Columns[j].HeaderText == "Department")
                                {
                                    dgvSearchResult.Columns[j].Visible = false;
                                }

                                if (dgvSearchResult.Columns[j].HeaderText == "Status")
                                {
                                    dgvSearchResult.Columns[j].Visible = false;
                                }

                                if (dgvSearchResult.Columns[j].HeaderText == "Payment Mode")
                                {
                                    dgvSearchResult.Columns[j].Visible = false;
                                }

                                if (dgvSearchResult.Columns[j].HeaderText == "Amount")
                                {
                                    dgvSearchResult.Columns[j].Visible = false;
                                }

                                if (dgvSearchResult.Columns[j].HeaderText == "Result")
                                {
                                    dgvSearchResult.Columns[j].Visible = false;
                                }

                                if (dgvSearchResult.Columns[j].HeaderText == "Test Reason")
                                {
                                    dgvSearchResult.Columns[j].Visible = false;
                                }

                                if (dgvSearchResult.Columns[j].HeaderText == "Donor City")
                                {
                                    dgvSearchResult.Columns[j].Visible = false;
                                }

                                if (dgvSearchResult.Columns[j].HeaderText == "Zip Code")
                                {
                                    dgvSearchResult.Columns[j].Visible = false;
                                }

                                if (dgvSearchResult.Columns[j].HeaderText == "MRO Type")
                                {
                                    dgvSearchResult.Columns[j].Visible = false;
                                }

                                if (dgvSearchResult.Columns[j].HeaderText == "Payment Type")
                                {
                                    dgvSearchResult.Columns[j].Visible = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (dgvSearchResult.Columns[0].Name == "DonorSelection")
                        {
                            dgvSearchResult.Columns[0].Visible = true;
                        }
                    }
                    // }
                }
            }
            else
            {
                DoSearch();
            }
        }

        #endregion Private Methods

        private void dgvSearchResult_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvSearchResult.Rows.Count > 0 && e.ColumnIndex >= 0 || e.ColumnIndex == -1)
            {
                if (e.RowIndex != -1 || e.ColumnIndex == 0)
                {
                    if (e.RowIndex != -1)
                    {
                        DataGridViewCheckBoxCell fieldSelection = (DataGridViewCheckBoxCell)dgvSearchResult.Rows[e.RowIndex].Cells["DonorSelection"];

                        if (Convert.ToBoolean(fieldSelection.Value))
                        {
                            fieldSelection.Value = false;
                            selectedCounter.SelectedCount--;
                        }
                        else
                        {
                            selectedCounter.SelectedCount++;
                            fieldSelection.Value = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Send donor(s) in 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNotify_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> donor_test_info_ids = new List<int>();

                for (int id = 0; id < dgvSearchResult.Rows.Count; id++)
                {
                    DataGridViewCheckBoxCell donorSelection = (DataGridViewCheckBoxCell)dgvSearchResult.Rows[id].Cells["DonorSelection"];
                    int donor_test_info_id = (int)dgvSearchResult.Rows[id].Cells["DonorTestInfoId"].Value;
                    int backend_notification_window_data_id = (int)(Int64)dgvSearchResult.Rows[id].Cells["backend_notification_window_data_id"].Value;

                    if (Convert.ToBoolean(donorSelection.Value) == true)
                    {
                        donor_test_info_ids.Add(donor_test_info_id);
                    }
                }

                //TODO - Need to verify if we want to force_db on this....
                backendLogic.DoSendIn(donor_test_info_ids, 0, Program.currentUserId, false);


                Cursor.Current = Cursors.WaitCursor;
                DoSearch();
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                //_logger.Error(ex.ToString());
                //if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                //if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dgvSearchResult.Rows.Count < 1) return;

            // Randomly select nudNumberToSelect donors for notification
            int numberOfRandom = 0;
            int.TryParse(nudNumberToSelect.Value.ToString(), out numberOfRandom);

            if (numberOfRandom > dgvSearchResult.Rows.Count) numberOfRandom = dgvSearchResult.Rows.Count;

            Random rnd = new Random();
            // generate numberOfRandom unique integers
            List<int> ids = new List<int>();

            while (ids.Count < numberOfRandom)
            {
                int RowToSelect = rnd.Next(1, dgvSearchResult.Rows.Count);
                if (!ids.Contains(RowToSelect)) ids.Add(RowToSelect);
            }

            for (int i = 0; i < dgvSearchResult.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell donorSelection = (DataGridViewCheckBoxCell)dgvSearchResult.Rows[i].Cells["DonorSelection"];
                donorSelection.Value = false;
            }

            foreach (int id in ids)
            {
                DataGridViewCheckBoxCell donorSelection = (DataGridViewCheckBoxCell)dgvSearchResult.Rows[id].Cells["DonorSelection"];
                donorSelection.Value = true;
                selectedCounter.SelectedCount++;
            }

            for (int i = 0; i < dgvSearchResult.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell donorSelection = (DataGridViewCheckBoxCell)dgvSearchResult.Rows[i].Cells["DonorSelection"];
                dgvSearchResult.Rows[i].Selected = (bool)donorSelection.Value == true;
            }

            //int checked = dgvSearchResult.Rows.DonorSelection
            //while (dgvSearchResult.SelectedRows.Count < numberOfRandom)
            //{
            //    int RowToSelect = rnd.Next(1, dgvSearchResult.Rows.Count);
            //    if (!dgvSearchResult.Rows[RowToSelect].Selected)
            //    {
            //        dgvSearchResult.Rows[RowToSelect].Selected = true;
            //    }

            //}

            //for (int rowid = 0; rowid <= numberOfRandom; rowid++)
            //{
            //}
        }

        private void btnSetNotified_Click(object sender, EventArgs e)
        {
            try
            {
                // get the row selected
                //backendLogic.NotificationSetNotified(this.DonorTestInfoId.ValueType);
                Cursor.Current = Cursors.WaitCursor;
                for (int id = 0; id < dgvSearchResult.Rows.Count; id++)
                {
                    DataGridViewCheckBoxCell donorSelection = (DataGridViewCheckBoxCell)dgvSearchResult.Rows[id].Cells["DonorSelection"];

                    if (Convert.ToBoolean(donorSelection.Value) == true)
                    {
                        int _DonorTestInfoId = (int)dgvSearchResult.Rows[id].Cells["DonorTestInfoId"].Value;
                        backendLogic.NotificationSetNotified(_DonorTestInfoId, true);

                        backendLogic.SetDonorActivity(_DonorTestInfoId, (int)DonorActivityCategories.Notification, $"Manually Sent In set by {Program.currentUserName}", Program.currentUserId);
                    }
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnNotifyNow_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure [Notify Now]?", "Notify now", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    for (int id = 0; id < dgvSearchResult.Rows.Count; id++)
                    {
                        DataGridViewCheckBoxCell donorSelection = (DataGridViewCheckBoxCell)dgvSearchResult.Rows[id].Cells["DonorSelection"];

                        if (Convert.ToBoolean(donorSelection.Value) == true)
                        {
                            int _DonorTestInfoId = (int)dgvSearchResult.Rows[id].Cells["DonorTestInfoId"].Value;
                            backendLogic.NotificationSetforImmediateNotification(_DonorTestInfoId);
                        }
                    }
                    Cursor.Current = Cursors.Default;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //private int GetColumnIndexByName(DataGridView grid, string name)
        //{
        //    foreach (DataColumn col in grid.Columns)
        //    {
        //        if (col.ColumnName.ToLower().Trim() == name.ToLower().Trim()) return col.Ordinal;
        //    }
        //    return null;
        //}
        public static void BindField(Control control, string propertyName, object dataSource, string dataMember)
        {
            Binding bd;

            for (int index = control.DataBindings.Count - 1; (index == 0); index--)
            {
                bd = control.DataBindings[index];
                if (bd.PropertyName == propertyName)
                    control.DataBindings.Remove(bd);
            }
            control.DataBindings.Add(propertyName, dataSource, dataMember, false, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // get the row selected
                //backendLogic.NotificationSetNotified(this.DonorTestInfoId.ValueType);
                Cursor.Current = Cursors.WaitCursor;
                for (int id = 0; id < dgvSearchResult.Rows.Count; id++)
                {
                    DataGridViewCheckBoxCell donorSelection = (DataGridViewCheckBoxCell)dgvSearchResult.Rows[id].Cells["DonorSelection"];

                    if (Convert.ToBoolean(donorSelection.Value) == true)
                    {
                        int _DonorTestInfoId = (int)dgvSearchResult.Rows[id].Cells["DonorTestInfoId"].Value;
                        backendLogic.NotificationSetNotified(_DonorTestInfoId, false);

                        backendLogic.SetDonorActivity(_DonorTestInfoId, (int)DonorActivityCategories.Notification, $"Sent In cleared set by {Program.currentUserName}", Program.currentUserId);
                    }
                }
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void dtAfterFilter_ValueChanged(object sender, EventArgs e)
        {
            if (dtBeforeFilter.Value <= dtAfterFilter.Value)
            {
                dtBeforeFilter.Value = dtAfterFilter.Value.AddDays(1);
            }
            dtBeforeFilter.MinDate = dtAfterFilter.Value.AddDays(1);
        }

        private void dtBeforeFilter_ValueChanged(object sender, EventArgs e)
        {
            if (dtAfterFilter.Value >= dtBeforeFilter.Value)
            {
                dtAfterFilter.Value = dtBeforeFilter.Value.AddDays(-1);
            }
            dtAfterFilter.MaxDate = dtBeforeFilter.Value.AddDays(-1);
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbFilter.SelectedIndex == 0)
                {
                    lblAfter.Visible = false;
                    lblBefore.Visible = false;
                    lblPresets.Visible = false;
                    dtAfterFilter.Visible = false;
                    dtBeforeFilter.Visible = false;
                    cbPresets.Visible = false;
                }
                else
                {
                    lblAfter.Visible = true;
                    lblBefore.Visible = true;
                    lblPresets.Visible = true;
                    dtAfterFilter.Visible = true;
                    dtBeforeFilter.Visible = true;
                    cbPresets.Visible = true;
                }
            }
            catch (Exception ex)
            {

                Program._logger.Error(ex.ToString());
                if (ex.InnerException != null) Program._logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) Program._logger.Error(ex.StackTrace.ToString());
                MessageBox.Show(ex.Message);

            }
        }

        private void cbPresets_SelectedIndexChanged(object sender, EventArgs e)
        {
            //dtBeforeFilter.Value = DateTime.Now.Date.AddDays(1);
            //dtAfterFilter.Value = DateTime.Now.Date.AddDays(this.FilterDayInterval);
            //cbFilter.SelectedIndex = cbFilter.FindString("None");
            int _presetSel = cbPresets.SelectedIndex;
            int _FilterDayInterval = this.FilterDayInterval;
            if (_presetSel > 0)
            {
                switch (_presetSel)
                {
                    case 1: // 3 days
                        _FilterDayInterval = -3;
                        break;
                    case 2: // 7 days
                        _FilterDayInterval = -7;
                        break;
                    case 3: // 30 days
                        _FilterDayInterval = -30;
                        break;
                    case 4: // 60 days
                        _FilterDayInterval = -60;
                        break;
                    case 5: // 90 days
                        _FilterDayInterval = -90;
                        break;
                    default:
                        break;
                }

                dtBeforeFilter.Value = DateTime.Now.Date.AddDays(1);
                dtAfterFilter.Value = DateTime.Now.Date.AddDays(_FilterDayInterval);
                cbPresets.SelectedIndex = 0; // cbFilter.FindString("None");
            }

        }
    }
}