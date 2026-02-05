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
    public partial class FrmExceptions : Form
    {
        static ILogger _logger = Program._logger;

        BackendData backendData = new BackendData();
        BackendLogic backendLogic = new BackendLogic(Program.currentUserName, _logger);
        RangeHolder rangeHolder;
        bool GridsUpdating = false;
       
        private Timer exceptionTimer;
        //private SelectedCounter dgvHelpers["dgvClinicExceptions"].SelectedCounter;
        //private Dictionary<string, string> dgvHelpers["dgvClinicExceptions"].CellMatch;
        //private List<string> dgvHelpers["dgvClinicExceptions"].KnownCells;

        //private DGVHelper dgvHelpers["dgvClinicExceptions"] = new DGVHelper();
        //private DGVHelper dgvHelpers["dgvSMSReplies"] = new DGVHelper();
        private Dictionary<string, DGVHelper> dgvHelpers = new Dictionary<string, DGVHelper>();

        public FrmExceptions(ILogger __logger = null)
        {
            _logger.Debug("FrmExceptions loaded");
            InitializeComponent();
            exceptionTimer = new Timer();
            exceptionTimer.Interval = (30 * 1000); // 30 seconds
            exceptionTimer.Tick += new EventHandler(OnTimedEvent);
            exceptionTimer.Start();

        }
        private void OnTimedEvent(object source, EventArgs e)
        {
            _logger.Debug("OnTimeEvent Fired");
            SetTabs();
        }
        /// <summary>
        /// Set tabs flag icon
        /// </summary>
        private void SetTabs()
        {
            ExceptionCounts exceptionCounts = backendData.GetExceptionData();

            // Clinic Exceptions
            if (exceptionCounts.clinic_exception_count > 0)
            {
                tabExceptionNotifications.ImageIndex = 1;
            }
            else
            {
                tabExceptionNotifications.ImageIndex = 2;
            }
            if (exceptionCounts.sms_count > 0)
            {
                tabSMSReplies.ImageIndex = 1;
                _logger.Debug("refreshing sms");
                dgvHelpers["dgvSMSReplies"].SetTabs = false;
                PopulateSMSReplies();
            }
            else
            {
                tabSMSReplies.ImageIndex = 2;
            }

            if (exceptionCounts.sis_count > 0)
            {
                tabClientScheduleExpired.ImageIndex = 1;
            }
            else
            {
                tabClientScheduleExpired.ImageIndex = 2;
            }

            if (exceptionCounts.did_count > 0)
            {
                tabDeadlineDonors.ImageIndex = 1;
            }
            else
            {
                tabDeadlineDonors.ImageIndex = 2;
            }

            if (exceptionCounts.ffo_count > 0)
            {
                tabFormFox.ImageIndex = 1;
            }
            else
            {
                tabFormFox.ImageIndex = 2;
            }
            Program.frmMain.updateExceptionFlags();

        }

        #region form

        private void FrmExceptions_Load(object sender, EventArgs e)
        {
            try
            {
                rangeHolder = new RangeHolder();
                //dgvHelpers["dgvClinicExceptions"].CellMatch = new Dictionary<string, string>();
                //dgvHelpers["dgvClinicExceptions"].KnownCells = new List<string>();
                //dgvHelpers["dgvClinicExceptions"].SelectedCounter = new SelectedCounter();

                dgvHelpers["dgvClinicExceptions"] = new DGVHelper();
                dgvHelpers["dgvSMSReplies"] = new DGVHelper() { colID = "_sms" };
                dgvHelpers["dgvDeadlineDonors"] = new DGVHelper() { colID = "_dld" };
                dgvHelpers["dgvSendInScheduler"] = new DGVHelper() { colID = "_sis" };
                dgvHelpers["dgvFormFox"] = new DGVHelper() { colID = "_ffo" };

                Cursor.Current = Cursors.WaitCursor;
                InitializeControls();
                // PopulateClinicExceptions();

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message, "Error in FrmExceptions_Load");
            }
        }

        private void FrmExceptions_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.frmMain.frmExceptions = null;
        }

        private void tabsExceptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.tabsExceptions.SelectedIndex)
            {
                case 0: // Clinic exceptions
                    PopulateClinicExceptions();
                    break;

                case 1: // SMS replies
                    PopulateSMSReplies();
                    break;

                case 2: // send in scheduler
                    PopulateSendInScheduleExceptions();
                    break;

                case 3: // deadline donors
                    PopuldateDeadlineDonors();
                    break;

                case 4: // FormFox overuse
                    PopulateFormFoxStatus();
                    break;
            }
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
        }

        private void PopulateClinicColumnMapper()
        {
            // public Dictionary<string, int> ColumnMapper = new Dictionary<string, int>();
            Dictionary<string, int> columnMap = new Dictionary<string, int>()
            {
                { "key", 0}
            };
        }

        private void InitializeControls()
        {
            try
            {
                //dgvClinicExceptions.AutoGenerateColumns = false;
                //dgvSMSReplies.AutoGenerateColumns = false;
                //dgvDeadlineDonors.AutoGenerateColumns = false;
                //dgvSendInScheduler.AutoGenerateColumns = false;
                //dgvFormFox.AutoGenerateColumns = false;

                PopulateKnownCells(dgvClinicExceptions);
                PopulateKnownCells(dgvSMSReplies);
                PopulateKnownCells(dgvDeadlineDonors);
                PopulateKnownCells(dgvSendInScheduler);
                PopulateKnownCells(dgvFormFox);

                // make smsreplies rows not show to selection bar
                dgvSMSReplies.RowsDefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;

                BindField(this.numRangeInMiles, "Value", rangeHolder, "Range");
                BindField(this.trackBarMiles, "Value", rangeHolder, "Range");
                string helperid = "dgvClinicExceptions";

                BindField(this.btnNotify, "Enabled", dgvHelpers[helperid].SelectedCounter, "Enabled");
                BindField(this.numRangeInMiles, "Enabled", dgvHelpers[helperid].SelectedCounter, "Enabled");
                BindField(this.trackBarMiles, "Enabled", dgvHelpers[helperid].SelectedCounter, "Enabled");
                BindField(this.btnRandom, "Enabled", dgvHelpers[helperid].SelectedCounter, "RowsAvailable");

                //helperid = "dgvSMSReplies";
                //BindField(this.btnNotify, "Enabled", dgvHelpers[helperid].SelectedCounter, "Enabled");
                //BindField(this.numRangeInMiles, "Enabled", dgvHelpers[helperid].SelectedCounter, "Enabled");
                //BindField(this.trackBarMiles, "Enabled", dgvHelpers[helperid].SelectedCounter, "Enabled");
                //BindField(this.btnRandom, "Enabled", dgvHelpers[helperid].SelectedCounter, "RowsAvailable");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
        }

        private void FrmExceptions_Shown(object sender, EventArgs e)
        {
            Application.DoEvents();
            PopulateClinicExceptions();
            //var x = 1;
            // clear all selections just in case there's only one and
            // when doing miles searches, only selected rows are
            // updated
            dgvClinicExceptions.ClearSelection();
        }

        #endregion form

        #region clinics

        private void button3_Click(object sender, EventArgs e)
        {
            if (dgvClinicExceptions.Rows.Count < 1) return;

            // Randomly select nudNumberToSelect donors for notification
            int numberOfRandom = 0;
            int.TryParse(nudNumberToSelect.Value.ToString(), out numberOfRandom);

            if (numberOfRandom > dgvClinicExceptions.Rows.Count) numberOfRandom = dgvClinicExceptions.Rows.Count;

            Random rnd = new Random();
            // generate numberOfRandom unique integers
            List<int> ids = new List<int>();

            while (ids.Count < numberOfRandom)
            {
                int RowToSelect = rnd.Next(1, dgvClinicExceptions.Rows.Count);
                if (!ids.Contains(RowToSelect)) ids.Add(RowToSelect);
            }

            for (int i = 0; i < dgvClinicExceptions.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell donorSelection = (DataGridViewCheckBoxCell)dgvClinicExceptions.Rows[i].Cells["DonorSelection"];
                donorSelection.Value = false;
            }

            foreach (int id in ids)
            {
                DataGridViewCheckBoxCell donorSelection = (DataGridViewCheckBoxCell)dgvClinicExceptions.Rows[id - 1].Cells["DonorSelection"];
                donorSelection.Value = true;
                dgvHelpers["dgvClinicExceptions"].SelectedCounter.SelectedCount++;
            }

            for (int i = 0; i < dgvClinicExceptions.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell donorSelection = (DataGridViewCheckBoxCell)dgvClinicExceptions.Rows[i].Cells["DonorSelection"];
                dgvClinicExceptions.Rows[i].Selected = (bool)donorSelection.Value == true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //string mbMessage = string.Empty;
                _logger.Debug($"Send in [exceptions] clicked");
                List<int> donor_test_info_ids = new List<int>();
                List<ExceptionSendIn> _EXSendIns = new List<ExceptionSendIn>();

                for (int id = 0; id < dgvClinicExceptions.Rows.Count; id++)
                {
                    DataGridViewCheckBoxCell donorSelection = (DataGridViewCheckBoxCell)dgvClinicExceptions.Rows[id].Cells["DonorSelection"];

                    if (Convert.ToBoolean(donorSelection.Value) == true)
                    {
                        _logger.Debug($"Row id {id} selected");
                        int donor_test_info_id = (int)dgvClinicExceptions.Rows[id].Cells["donor_test_info_id"].Value;
                        _logger.Debug($"Row donor_test_info_id {donor_test_info_id} selected");

                        _logger.Debug($"Checking force_db");

                        int _int_force_db = 0;
                        int.TryParse(dgvClinicExceptions.Rows[id].Cells["force_db"].Value.ToString(), out _int_force_db);
                        _logger.Debug($"force_db = {_int_force_db}");

                        donor_test_info_ids.Add(donor_test_info_id);
                        _EXSendIns.Add(new ExceptionSendIn() { donor_test_info_id = donor_test_info_id, force_db = _int_force_db > 0, range = (int)numRangeInMiles.Value });

                    }
                }
                _logger.Debug($"Calling exception send ins");

                backendLogic.DoExceptionSendIn(_EXSendIns, Program.currentUserId);

            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());

                MessageBox.Show("There was a problem scheduling notifications!\r\nPlease copy all this content and communicate it to report the error.\r\n" + ex.Message, "Error");
                throw;
            }
            Cursor.Current = Cursors.Default;
            //PopulateClinicExceptions();
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void RangeSearchToggle()
        {
            this.numRangeInMiles.Enabled = !this.numRangeInMiles.Enabled;
            this.trackBarMiles.Enabled = !this.trackBarMiles.Enabled;
        }

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

        private void PopulateClinicExceptions()
        {
            GridsUpdating = true;
            //DataTable dtDonors = donorBL.SearchDonor(searchParam, Program.currentUserType, Program.currentUserId, Program.currentUserName, showAll);
            DataTable dtClinicExceptions = backendData.GetNotificationExceptionsByType(Enum.NotificationExceptions.NoClinics);
            dgvClinicExceptions.DataSource = dtClinicExceptions;
            PopulateClinicsInDistance();
        }

        private void PopulateSMSReplies()
        {
            Dictionary<string, int> order = new Dictionary<string, int>();
            foreach (DataGridViewColumn column in dgvSMSReplies.Columns)
            {
                order[column.Name] = column.DisplayIndex;
            }

            GridsUpdating = true;
            //DataTable dtDonors = donorBL.SearchDonor(searchParam, Program.currentUserType, Program.currentUserId, Program.currentUserName, showAll);
            DataTable dtSMSReplies = backendData.GetSMSActivityUnread();
            dgvSMSReplies.DataSource = dtSMSReplies;

            if (dgvSMSReplies.RowCount < 1)
            {
                foreach (DataGridViewColumn column in dgvHelpers[dgvSMSReplies.Name].Columns)
                {
                    if (!dgvSMSReplies.Columns.Contains(column)) this.dgvSMSReplies.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { column });
                }
            }
            //    foreach (DataGridViewColumn column in dgvSMSReplies.Columns)
            //    {
            //        this.dgvSMSReplies.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            //this.DonorSelection_sms,
            //this.DonorFirstName_sms,
            //this.DonorLastName_sms,
            //this.SMSReply_sms,
            //this.SSN_sms,
            //this.ClientName_sms,
            //this.DepartmentName_sms,
            //this.DonorTestRegisteredDate_sms,
            //this.DateDonorNotified_sms,
            //this.DonorCity_sms,
            //this.ZipCode_sms,
            //this.auto_reply_text_sms,
            //this.ReplyToSms_sms,
            //this.MarkAsRead_sms});
            //}

            //if (dgvSMSReplies.RowCount>0)
            //{
            //    foreach (DataGridViewColumn column in dgvSMSReplies.Columns)
            //    {
            //        //order[column.Name] = column.DisplayIndex;
            //        if (order.ContainsKey(column.Name))
            //        {
            //            column.DisplayIndex = order[column.Name];
            //        }
            //    }
            //}
        }

        private void PopulateFormFoxStatus()
        {
            GridsUpdating = true;
            DataTable dtFormFoxOverdue = backendData.GetFormFoxOverdue((int)ffDaysAgo.Value, ffIncludeArchived.Checked);
            dgvFormFox.DataSource = dtFormFoxOverdue;

            if (dgvFormFox.RowCount < 1)
            {
                foreach (DataGridViewColumn column in dgvHelpers[dgvFormFox.Name].Columns)
                {
                    if (!dgvFormFox.Columns.Contains(column)) this.dgvFormFox.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { column });
                }
            }
        }
        private void PopulateSendInScheduleExceptions()
        {
            GridsUpdating = true;
            DataTable dtSendInScheduleExceptions = backendData.GetSendInScheduleExceptions();
            dgvSendInScheduler.DataSource = dtSendInScheduleExceptions;
        }

        private void PopuldateDeadlineDonors()
        {
            GridsUpdating = true;
            DataTable dtDeadlineDonors = backendData.GetDeadlineDonors();
            dgvDeadlineDonors.DataSource = dtDeadlineDonors;
        }

        private void DeSelectAllClinicExceptions()
        {
            for (int id = 0; id < dgvClinicExceptions.Rows.Count; id++)
            {
                dgvClinicExceptions.Rows[id].Cells["DonorSelection"].Selected = false;
            }
        }

        private void PopulateClinicsInDistance(bool useslider = false)
        {
            // for every row, we need fire a task to go get clinics in range of setting:
            //ParamGetClinicsForDonor p = new ParamGetClinicsForDonor();

            //p.donor_id = 6;
            //p._dist = 30;
            //List<CollectionFacility> collectionFacilities = d.GetClinicsForDonor(p);
            //Assert.IsTrue(collectionFacilities.Count > 0);

            //TODO



            try
            {
                string lblSearchingText = lblSearching.Text;
                RangeSearchToggle();
                lblSearching.Visible = true;

                DonorBL donorBL = new DonorBL(_logger);

                Cursor.Current = Cursors.WaitCursor;

                lblSearching.Text = "Searching FormFox";

                for (int id = 0; id < dgvClinicExceptions.Rows.Count; id++)
                {
                    if (dgvClinicExceptions.Rows[id].Cells["DonorSelection"].Selected)
                    {

                        dgvClinicExceptions.Rows[id].Selected = false;
                        dgvClinicExceptions.Rows[id].Cells["ClinicsInRange"].Value = "fetching (FormFox)...";

                        FFMPSearch ffApi = new FFMPSearch(_logger);
                        int donor_id = (int)dgvClinicExceptions.Rows[id].Cells["DonorId"].Value;
                        Donor donor = donorBL.Get(donor_id, "backend");
                        int range = (int)numRangeInMiles.Value;
                        int maxRange = (int)numMaxRange.Value;
                        if (useslider == true)
                        {
                            maxRange = (int)numRangeInMiles.Value;
                        }
                        int count = 0;

                        /// This needs to search FormFox and none are found in FormFox, THEN search local database.
                        SenderTracker senderTracker = new SenderTracker();
                        ffApi.PullSites(donor.DonorAddress1, donor.DonorCity, donor.DonorState, donor.DonorZip, maxRange, ref senderTracker);

                        FFMPSearchResult _site = new FFMPSearchResult();
                        string _pricetier = string.Empty;
                        ffApi.Sites.ForEach(s => count = count + s.SiteList.Where(_s => _s.Distance < maxRange).Count());

                        if (count < 1) // No formfox sites found.
                        {
                            dgvClinicExceptions.Rows[id].Cells["force_db"].Value = "1";
                            lblSearching.Text = "Searching DB";
                            // Find some in our database
                            count = GetClinicsForDonorIdInRangeOf(donor_id, range);
                        }
                        else
                        {
                            // We found formfox sites within this range, so we can go ahead and use FormFox
                            dgvClinicExceptions.Rows[id].Cells["force_db"].Value = "0";
                        }


                        dgvClinicExceptions.Rows[id].Cells["ClinicsInRange"].Value = count.ToString();

                        dgvClinicExceptions.Rows[id].Cells["MaxMiles"].Value = range.ToString();

                        if (count == 0)
                        {
                            dgvClinicExceptions.Rows[id].Cells["DonorSelection"].ReadOnly = true;
                        }
                        else
                        {
                            dgvClinicExceptions.Rows[id].Cells["DonorSelection"].ReadOnly = false;
                            dgvClinicExceptions.Rows[id].Cells["DonorSelection"].Selected = true;
                        }



                    }
                }
                Cursor.Current = Cursors.Default;
                lblSearching.Text = lblSearchingText;
                lblSearching.Visible = false;
                RangeSearchToggle();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
        }

        private void PopulateClinicsByMinimumNumber()
        {
            // for every row, we need fire a task to go get clinics in range of setting:
            //ParamGetClinicsForDonor p = new ParamGetClinicsForDonor();

            //p.donor_id = 6;
            //p._dist = 30;
            //List<CollectionFacility> collectionFacilities = d.GetClinicsForDonor(p);
            //Assert.IsTrue(collectionFacilities.Count > 0);
            try
            {
                string lblSearchingText = lblSearching.Text;
                RangeSearchToggle();
                lblSearching.Visible = true;
                DonorBL donorBL = new DonorBL(_logger);

                Cursor.Current = Cursors.WaitCursor;
                for (int id = 0; id < dgvClinicExceptions.Rows.Count; id++)
                {
                    dgvClinicExceptions.Rows[id].Cells["ClinicsInRange"].Value = "fetching...";

                    lblSearching.Text = "Searching FormFox";
                    FFMPSearch ffApi = new FFMPSearch(_logger);
                    int donor_id = (int)dgvClinicExceptions.Rows[id].Cells["DonorId"].Value;
                    Donor donor = donorBL.Get(donor_id, "backend");
                    int range = (int)numRangeInMiles.Value;
                    int maxRange = (int)numMaxRange.Value;
                    int count = 0;
                    int donor_test_info_id = (int)dgvClinicExceptions.Rows[id].Cells["donor_test_info_id"].Value;

                    int MaxDistance = 0;
                    /// This needs to search FormFox and none are found in FormFox, THEN search local database.
                    SenderTracker senderTracker = new SenderTracker();
                    ffApi.PullSites(donor.DonorAddress1, donor.DonorCity, donor.DonorState, donor.DonorZip, maxRange, ref senderTracker);

                    FFMPSearchResult _site = new FFMPSearchResult();
                    string _pricetier = string.Empty;
                    ffApi.Sites.ForEach(s =>
                        count = count + s.SiteList.Where(_s => _s.Distance < maxRange
                    ).Count());
                    ffApi.Sites.ForEach(s =>
                    {
                        int m = (int)Math.Ceiling(s.SiteList.Max(sl => sl.Distance));
                        if (MaxDistance < m) MaxDistance = m;
                    }
                    );


                    if (count < 1)
                    {
                        lblSearching.Text = "Searching Database";
                        ParamGetClinicForDonorMinNumber p = new ParamGetClinicForDonorMinNumber();
                        p.donor_id = (int)dgvClinicExceptions.Rows[id].Cells["DonorId"].Value;
                        p.min_count = (int)numMinClinics.Value;
                        p.max_miles = (int)numMaxRange.Value;
                        p.start_miles = 0;
                        List<CollectionFacility> facilities = backendData.GetClinicForDonorMinNumber(p);
                        count = facilities.Count();
                        if (count > 0)
                        {
                            MaxDistance = (int)Math.Ceiling(facilities.Max(x => x.d2c));
                        }
                        dgvClinicExceptions.Rows[id].Cells["force_db"].Value = "1";

                    }
                    else
                    {
                        dgvClinicExceptions.Rows[id].Cells["force_db"].Value = "0";

                    }
                    if (count > 0)
                    {
                        dgvClinicExceptions.Rows[id].Cells["MaxMiles"].Value = MaxDistance;
                        dgvClinicExceptions.Rows[id].Cells["DonorSelection"].ReadOnly = false;
                        backendLogic.SetDonorNotificationRange(donor_test_info_id, MaxDistance, Program.currentUserId);
                    }
                    else
                    {
                        dgvClinicExceptions.Rows[id].Cells["MaxMiles"].Value = "No Matches";
                        //(DataGridViewCheckBoxCell)
                        dgvClinicExceptions.Rows[id].Cells["DonorSelection"].Selected = false;
                        dgvClinicExceptions.Rows[id].Cells["DonorSelection"].ReadOnly = true;
                    }
                    dgvClinicExceptions.Rows[id].Cells["ClinicsInRange"].Value = count;
                }
                lblSearching.Text = lblSearchingText;
                lblSearching.Visible = false;
                Cursor.Current = Cursors.Default;
                RangeSearchToggle();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
        }

        private int GetClinicsForDonorIdInRangeOf(int donor_id, int range_in_miles)
        {
            ParamGetClinicsForDonor p = new ParamGetClinicsForDonor();

            p.donor_id = donor_id;
            p._dist = range_in_miles;
            return backendData.GetClinicsForDonor(p).Count;
        }

        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            rangeHolder.dragging = false;
            PopulateClinicsInDistance(true);
        }

        private void numRangeInMiles_ValueChanged(object sender, EventArgs e)
        {
            if (rangeHolder.dragging) return;
            PopulateClinicsInDistance();
        }

        private void trackBar1_MouseDown(object sender, MouseEventArgs e)
        {
            rangeHolder.dragging = true;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PopulateClinicsByMinimumNumber();
        }

        private void dgvClinicExceptions_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ////// if it's the checkbox, add it to the selected rows
            ////if (dgvClinicExceptions.Rows.Count > 0 && e.ColumnIndex >= 0 || e.ColumnIndex == -1)
            ////{
            ////    if (e.RowIndex != -1 || e.ColumnIndex == 0)
            ////    {
            ////        if (e.RowIndex != -1)
            ////        {
            ////            if (e.ColumnIndex == 0)
            ////            {
            ////                // first column
            ////                checkDGVRows(sender, e, dgvClinicExceptions, dgvHelpers["dgvClinicExceptions"].SelectedCounter, true);
            ////            }
            ////        }
            ////    }
            ////}

            //checkDGVRows(sender, e, dgvClinicExceptions, dgvHelpers["dgvClinicExceptions"].SelectedCounter);

            //if (dgvClinicExceptions.Rows.Count > 0 && e.ColumnIndex >= 0 || e.ColumnIndex == -1)
            //{
            //    if (e.RowIndex != -1 || e.ColumnIndex == 0)
            //    {
            //        if (e.RowIndex != -1)
            //        {
            //            //if (IntegerExtensions.SafeParseInt(dgvClinicExceptions.Rows[e.RowIndex].Cells["ClinicsInRange"].Value) < 1)
            //            //{
            //            //    // MessageBox.Show("Please ensure the selected rows has clinics within range before selecting!");
            //            //}
            //            //else
            //            //{
            //            DataGridViewCheckBoxCell fieldSelection = (DataGridViewCheckBoxCell)dgvClinicExceptions.Rows[e.RowIndex].Cells["DonorSelection"];

            //            if (Convert.ToBoolean(fieldSelection.Value))
            //            {
            //                dgvHelpers["dgvClinicExceptions"].SelectedCounter.SelectedCount = dgvHelpers["dgvClinicExceptions"].SelectedCounter.SelectedCount - 1;
            //                dgvClinicExceptions.Rows[e.RowIndex].Selected = false;
            //                fieldSelection.Value = false;
            //            }
            //            else
            //            {
            //                dgvHelpers["dgvClinicExceptions"].SelectedCounter.SelectedCount = dgvHelpers["dgvClinicExceptions"].SelectedCounter.SelectedCount + 1;
            //                dgvClinicExceptions.Rows[e.RowIndex].Selected = true;
            //                fieldSelection.Value = true;
            //            }
            //            //}

            //        }
            //    }
            //}
        }

        private void DeselectRowsWithNoClinics()
        {
            for (int id = 0; id < dgvClinicExceptions.Rows.Count; id++)
            {
                // don't allow selection of a row without some clinics being available.
                if (IntegerExtensions.SafeParseInt((string)dgvClinicExceptions.Rows[id].Cells["ClinicsInRange"].Value) < 1)
                {
                    dgvHelpers["dgvClinicExceptions"].SelectedCounter.SelectedCount = dgvHelpers["dgvClinicExceptions"].SelectedCounter.SelectedCount - 1;
                    dgvClinicExceptions.Rows[id].Selected = false;
                    DataGridViewCheckBoxCell fieldSelection = (DataGridViewCheckBoxCell)dgvClinicExceptions.Rows[id].Cells["DonorSelection"];
                    fieldSelection.Value = false;
                }
            }
        }

        private void dgvClinicExceptions_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (GridsUpdating) return;
                Cursor.Current = Cursors.WaitCursor;
                if (e.RowIndex >= 0)
                {
                    Dictionary<string, string> tabPageList = new Dictionary<string, string>();

                    string donorId = dgvClinicExceptions.Rows[e.RowIndex].Cells["DonorId"].Value.ToString();
                    string donorTestInfoId = dgvClinicExceptions.Rows[e.RowIndex].Cells["DonorTestInfoId"].Value.ToString();
                    string donorFistName = dgvClinicExceptions.Rows[e.RowIndex].Cells["DonorFirstName"].Value.ToString();
                    string donorLastName = dgvClinicExceptions.Rows[e.RowIndex].Cells["DonorLastName"].Value.ToString();

                    donorTestInfoId = donorTestInfoId != string.Empty ? donorTestInfoId : "0";

                    if (donorId != string.Empty && donorTestInfoId != string.Empty)
                    {
                        string key = donorId + "#" + donorTestInfoId;
                        string value = donorFistName + " " + donorLastName;
                        if (!tabPageList.ContainsKey(key))
                        {
                            tabPageList.Add(key, value);
                        }

                        DataGridViewCheckBoxCell fieldSelection = (DataGridViewCheckBoxCell)dgvClinicExceptions.Rows[e.RowIndex].Cells["DonorSelection"];

                        if (Convert.ToBoolean(fieldSelection.Value))
                        {
                            fieldSelection.Value = false;
                        }
                        else
                        {
                            fieldSelection.Value = true;
                        }
                    }

                    LoadDonorDetails(tabPageList);
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                Cursor.Current = Cursors.Default;
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
                    Program.frmMain.frmDonorDetails.LoadTabDetails(index, (int)numRangeInMiles.Value);
                    Program.frmMain.frmDonorDetails.BringToFront();
                }
                else
                {
                    Program.frmMain.frmDonorDetails.LoadTabDetails(index - 1, (int)numRangeInMiles.Value);
                    Program.frmMain.frmDonorDetails.BringToFront();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        //private void dgvClinicExceptions_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //    DataGridView dgv = sender as DataGridView;
        //    DataGridViewColumn col = dgv.Columns[e.ColumnIndex];
        //    string cellName = col.Name;

        //    if (dgvHelpers["dgvClinicExceptions"].CellMatch.ContainsKey(col.Name)) cellName = dgvHelpers["dgvClinicExceptions"].CellMatch[col.Name];

        //    DataGridViewColumn _SortCol = dgv.Columns[cellName];

        //    if (_SortCol.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
        //    {
        //        dgv.Sort(_SortCol, ListSortDirection.Descending);
        //        _SortCol.HeaderCell.SortGlyphDirection = SortOrder.Descending;
        //        col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
        //    }
        //    else
        //    {
        //        dgv.Sort(_SortCol, ListSortDirection.Ascending);
        //        _SortCol.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
        //        col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
        //    }

        //    //if (col.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
        //    //{
        //    //    dgv.Sort(col, ListSortDirection.Descending);
        //    //    col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
        //    //    col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
        //    //}
        //    //else
        //    //{
        //    //    dgv.Sort(col, ListSortDirection.Ascending);
        //    //    col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
        //    //    col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
        //    //}

        //}

        private bool CellHasData(DataGridViewCell cell)
        {
            //return !string.IsNullOrEmpty(row.Cells[_name].Value.ToString());
            return (cell.Value != null && cell.Value.ToString() != string.Empty);
        }

        private void dgvClinicExceptions_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv_DataBindingComplete(sender, e);
            SetTabs();

            //foreach (DataGridViewRow row in dgvClinicExceptions.Rows)
            //{
            //    if (row.Cells["DonorSSN"].Value != null && row.Cells["DonorSSN"].Value.ToString() != string.Empty)
            //    {
            //        if (row.Cells["DonorSSN"].Value.ToString().Length == 11)
            //        {
            //            row.Cells["SSN"].Value = "***-**-" + row.Cells["DonorSSN"].Value.ToString().Substring(7);
            //            dgvHelpers["dgvClinicExceptions"].SetCellMatch("SSN", "DonorSSN");
            //        }
            //    }

            //    if (row.Cells["DonorDateOfBirth"].Value != null && row.Cells["DonorDateOfBirth"].Value.ToString() != string.Empty)
            //    {
            //        DateTime dob = Convert.ToDateTime(row.Cells["DonorDateOfBirth"].Value);
            //        if (dob != DateTime.MinValue)
            //        {
            //            row.Cells["DOB"].Value = dob.ToString("MM/dd/yyyy");
            //            dgvHelpers["dgvClinicExceptions"].SetCellMatch("DOB", "DonorDateOfBirth");

            //        }
            //    }

            //    if (row.Cells["SpecimenDate"].Value != null && row.Cells["SpecimenDate"].Value.ToString() != string.Empty)
            //    {
            //        DateTime specimenDate = Convert.ToDateTime(row.Cells["SpecimenDate"].Value);
            //        if (specimenDate != DateTime.MinValue)
            //        {
            //            row.Cells["SpecimenDateValue"].Value = specimenDate.ToString("MM/dd/yyyy");
            //            dgvHelpers["dgvClinicExceptions"].SetCellMatch("SpecimenDateValue", "SpecimenDate");

            //        }
            //    }

            //    if (row.Cells["MROTypeId"].Value != null && row.Cells["MROTypeId"].Value.ToString() != string.Empty)
            //    {
            //        ClientMROTypes clientMRoTypes = (ClientMROTypes)((int)row.Cells["MROTypeId"].Value);
            //        row.Cells["MROType"].Value = clientMRoTypes.ToString();
            //        dgvHelpers["dgvClinicExceptions"].SetCellMatch("MROType", "MROTypeId");

            //    }

            //    if (row.Cells["PaymentTypeId"].Value != null && row.Cells["PaymentTypeId"].Value.ToString() != string.Empty)
            //    {
            //        ClientPaymentTypes clientPaymentTypes = (ClientPaymentTypes)((int)row.Cells["PaymentTypeId"].Value);
            //        row.Cells["PaymentType"].Value = clientPaymentTypes.ToDescriptionString();
            //        dgvHelpers["dgvClinicExceptions"].SetCellMatch("PaymentType", "PaymentTypeId");

            //    }

            //    if (row.Cells["ReasonForTestId"].Value != null && row.Cells["ReasonForTestId"].Value.ToString() != string.Empty)
            //    {
            //        TestInfoReasonForTest testInfoReasonForTest = (TestInfoReasonForTest)((int)row.Cells["ReasonForTestId"].Value);
            //        row.Cells["TestReason"].Value = testInfoReasonForTest.ToDescriptionString();
            //        dgvHelpers["dgvClinicExceptions"].SetCellMatch("TestReason", "ReasonForTestId");

            //    }
            //    if (row.Cells["TestOverallResult"].Value != null && row.Cells["TestOverallResult"].Value.ToString() != string.Empty)
            //    {
            //        OverAllTestResult result = (OverAllTestResult)((int)row.Cells["TestOverallResult"].Value);
            //        if (result.ToString() != "None")
            //        {
            //            row.Cells["Result"].Value = result.ToDescriptionString();
            //        }
            //        else
            //        {
            //            row.Cells["Result"].Value = " ";
            //        }
            //        dgvHelpers["dgvClinicExceptions"].SetCellMatch("Result", "TestOverallResult");

            //    }

            //    if (row.Cells["PaymentReceived"].Value.ToString() == "1")  //&& row.Cells["DonorRegistrationStatus"].Value != "Pre-Registered"
            //    {
            //        if (row.Cells["PaymentMethodId"].Value != null && row.Cells["PaymentMethodId"].Value.ToString() != string.Empty)
            //        {
            //            PaymentMethod paymentMethod = (PaymentMethod)((int)row.Cells["PaymentMethodId"].Value);
            //            row.Cells["PaymentMode"].Value = paymentMethod.ToString();
            //            dgvHelpers["dgvClinicExceptions"].SetCellMatch("PaymentMode", "PaymentMethodId");

            //        }
            //    }

            //    if (row.Cells["DonorTestRegisteredDate"].Value != null && row.Cells["DonorTestRegisteredDate"].Value.ToString() != string.Empty)
            //    {
            //        DateTime paymentDate = Convert.ToDateTime(row.Cells["DonorTestRegisteredDate"].Value);
            //        if (paymentDate != DateTime.MinValue)
            //        {
            //            row.Cells["DonorTestRegisteredDateValue"].Value = paymentDate.ToString("MM/dd/yyyy");
            //            dgvHelpers["dgvClinicExceptions"].SetCellMatch("DonorTestRegisteredDateValue", "DonorTestRegisteredDate");

            //        }
            //    }

            //    //if (row.Cells["notified_by_email_timestamp"].Value != null && row.Cells["notified_by_email_timestamp"].Value.ToString() != string.Empty)
            //    //{
            //    //    if (row.Cells["notified_by_email_timestamp"].Value.ToString().Length == 11)
            //    //    {
            //    //        row.Cells["DonorDateNotifiedData"].Value = row.Cells["notified_by_email_timestamp"].Value.ToString();
            //    //    }
            //    //}
            //    if (row.Cells["PaymentDate"].Value != null && row.Cells["PaymentDate"].Value.ToString() != string.Empty)
            //    {
            //        DateTime paymentDate = Convert.ToDateTime(row.Cells["PaymentDate"].Value);
            //        if (paymentDate != DateTime.MinValue)
            //        {
            //            row.Cells["PaymentDateValue"].Value = paymentDate.ToString("MM/dd/yyyy");
            //            dgvHelpers["dgvClinicExceptions"].SetCellMatch("PaymentDateValue", "PaymentDate");

            //        }
            //    }
            //    //else
            //    //{
            //    //    row.Cells["PaymentMode"].Value = "";
            //    //}

            //    if (row.Cells["TestStatus"].Value != null && row.Cells["TestStatus"].Value.ToString() != string.Empty)
            //    {
            //        DonorRegistrationStatus status = (DonorRegistrationStatus)((int)row.Cells["TestStatus"].Value);
            //        row.Cells["Status"].Value = status.ToDescriptionString();
            //        dgvHelpers["dgvClinicExceptions"].SetCellMatch("Status", "TestStatus");

            //    }

            //    else if (row.Cells["DonorRegistrationStatus"].Value != null && row.Cells["DonorRegistrationStatus"].Value.ToString() != string.Empty)
            //    {
            //        DonorRegistrationStatus status = (DonorRegistrationStatus)((int)row.Cells["DonorRegistrationStatus"].Value);
            //        if (status == Enum.DonorRegistrationStatus.PreRegistration)
            //        {
            //            row.Cells["Status"].Value = "Pre-Registered";
            //        }
            //        else
            //        {
            //            row.Cells["Status"].Value = status.ToDescriptionString();
            //        }
            //        dgvHelpers["dgvClinicExceptions"].SetCellMatch("Status", "TestStatus");

            //    }
            //}

            //// hid any rows not defined at design time
            //foreach (DataGridViewColumn column in dgvClinicExceptions.Columns)
            //{
            //    if (!dgvHelpers["dgvClinicExceptions"].KnownCells.Contains(column.Name)) column.Visible = false;
            //}

            //dgvHelpers["dgvClinicExceptions"].SelectedCounter.RowsAvailable = dgvClinicExceptions.Rows.Count > 0;
            //this.nudNumberToSelect.Maximum = dgvClinicExceptions.Rows.Count;

            //SetTabs();
        }

        private void dgvClinicExceptions_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
        }

        private void dgvClinicExceptions_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            //if (e.ColumnIndex == 0) return;
            //checkDGVRows(sender, e, dgvClinicExceptions, dgvHelpers["dgvClinicExceptions"].SelectedCounter);
            checkDGVRows(sender, e, dgvHelpers["dgvClinicExceptions"].SelectedCounter);
        }

        #endregion clinics

        #region genericHandlers

        /// <summary>
        /// This is an attempt at a generic handler for datagrids
        /// TODO - make this loop through all rows and if checked, highlight the row
        /// and if the row is selected check the box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="dgv"></param>
        /// <param name="selCounter"></param>
        //private void checkDGVRows(object sender, DataGridViewCellMouseEventArgs e, DataGridView dgv, SelectedCounter selCounter)
        private void checkDGVRows(object sender, DataGridViewCellMouseEventArgs e, SelectedCounter selCounter)
        {
            DataGridView dgv = sender as DataGridView;
            string c = dgvHelpers[dgv.Name].colID;

            if (dgv.Rows.Count > 0 && e.ColumnIndex >= 0 || e.ColumnIndex == -1)
            {
                if (e.RowIndex != -1 || e.ColumnIndex == 0)
                {
                    if (e.RowIndex != -1)
                    {

                        selCounter.SelectedCount = 0;

                        DataGridViewCheckBoxCell rowCheckbox = (DataGridViewCheckBoxCell)dgv.Rows[e.RowIndex].Cells["DonorSelection" + c];

                        bool RowSelectedVal = Convert.ToBoolean(rowCheckbox.Value);
                        dgv.Rows[e.RowIndex].Selected = !RowSelectedVal;
                        rowCheckbox.Value = !RowSelectedVal;
                        //if (Convert.ToBoolean(rowCheckbox.Value))
                        //{
                        //    //dgvHelpers[dgv.Name].SelectedCounter.SelectedCount = dgvHelpers[dgv.Name].SelectedCounter.SelectedCount - 1;
                        //    dgv.Rows[e.RowIndex].Selected = false;
                        //    //rowCheckbox.Value = false;

                        //}
                        //else
                        //{
                        //    //dgvHelpers[dgv.Name].SelectedCounter.SelectedCount = dgvHelpers[dgv.Name].SelectedCounter.SelectedCount + 1;
                        //    dgv.Rows[e.RowIndex].Selected = true;
                        //    //rowCheckbox.Value = true;
                        //}

                        //if (e.ColumnIndex == 0) // The checkbox was clicked
                        //{
                        //    DataGridViewCheckBoxCell fieldSelection = (DataGridViewCheckBoxCell)dgv.Rows[e.RowIndex].Cells["DonorSelection" + c];

                        //    if (Convert.ToBoolean(fieldSelection.Value))
                        //    {
                        //        dgvHelpers[dgv.Name].SelectedCounter.SelectedCount = dgvHelpers[dgv.Name].SelectedCounter.SelectedCount - 1;
                        //        dgv.Rows[e.RowIndex].Selected = false;
                        //        fieldSelection.Value = false;
                        //    }
                        //    else
                        //    {
                        //        dgvHelpers[dgv.Name].SelectedCounter.SelectedCount = dgvHelpers[dgv.Name].SelectedCounter.SelectedCount + 1;
                        //        dgv.Rows[e.RowIndex].Selected = true;
                        //        fieldSelection.Value = true;
                        //    }
                        //    //for (int i = 0; i < dgv.Rows.Count; i++)
                        //    //{
                        //    //    fieldSelection = (DataGridViewCheckBoxCell)dgv.Rows[i].Cells["DonorSelection" + c];
                        //    //    if (Convert.ToBoolean(fieldSelection.Value) == true)
                        //    //    {
                        //    //        dgv.Rows[i].Selected = true;
                        //    //        fieldSelection.Value = true;
                        //    //    }
                        //    //}
                        //}
                        //else

                        //{
                        //    for (int i = 0; i < dgv.Rows.Count; i++)
                        //    {
                        //        DataGridViewCheckBoxCell fieldSelection = (DataGridViewCheckBoxCell)dgv.Rows[i].Cells["DonorSelection" + c];

                        //        if (!dgv.Rows[i].Selected)
                        //        {
                        //            //selCounter.SelectedCount = selCounter.SelectedCount - 1;
                        //            dgv.Rows[i].Selected = false;
                        //            fieldSelection.Value = false;
                        //        }
                        //        else
                        //        {
                        //            selCounter.SelectedCount = selCounter.SelectedCount + 1;
                        //            dgv.Rows[i].Selected = true;
                        //            fieldSelection.Value = true;
                        //        }
                        //    }
                        //}
                        for (int i = 0; i < dgv.Rows.Count; i++)
                        {
                            DataGridViewCheckBoxCell fieldSelection = (DataGridViewCheckBoxCell)dgv.Rows[i].Cells["DonorSelection" + c];

                            if (!(fieldSelection.Value == null) && (bool)fieldSelection.Value == true)
                            {
                                selCounter.SelectedCount = selCounter.SelectedCount + 1;
                                dgv.Rows[i].Selected = true;
                            }
                            //if (!dgv.Rows[i].Selected)
                            //{
                            //    //selCounter.SelectedCount = selCounter.SelectedCount - 1;
                            //    dgv.Rows[i].Selected = false;
                            //    fieldSelection.Value = false;
                            //}
                            //else
                            //{
                            //    selCounter.SelectedCount = selCounter.SelectedCount + 1;
                            //    dgv.Rows[i].Selected = true;
                            //    fieldSelection.Value = true;
                            //}
                        }


                    }
                }
            }
        }

        private void setDGVSelected(object sender, SelectedCounter selCounter)
        {
            DataGridView dgv = sender as DataGridView;
            string c = dgvHelpers[dgv.Name].colID;

            foreach (DataGridViewRow r in dgv.Rows)
            {
                selCounter.SelectedCount = 0;

                DataGridViewCheckBoxCell rowCheckbox = (DataGridViewCheckBoxCell)dgv.Rows[r.Index].Cells["DonorSelection" + c];

                if (!(rowCheckbox.Value == null) && (bool)rowCheckbox.Value == true)
                {
                    selCounter.SelectedCount = selCounter.SelectedCount + 1;
                    r.Selected = true;
                }
                else
                {
                    r.Selected = false;
                }
            }

        }


        private void dgv_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            DataGridViewColumn col = dgv.Columns[e.ColumnIndex];

            // if this is a DataGridViewButtonColumn we can't sort, so bail.
            if (dgv.Columns[e.ColumnIndex].CellType.Name.Equals("DataGridViewButtonCell", StringComparison.InvariantCultureIgnoreCase)) return;
            string cellName = col.Name;

            if (dgvHelpers[dgv.Name].CellMatch.ContainsKey(col.Name)) cellName = dgvHelpers[dgv.Name].CellMatch[col.Name];

            DataGridViewColumn _SortCol = dgv.Columns[cellName];

            if (_SortCol.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
            {
                dgv.Sort(_SortCol, ListSortDirection.Descending);
                _SortCol.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
            }
            else
            {
                dgv.Sort(_SortCol, ListSortDirection.Ascending);
                _SortCol.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
            }
        }

        private void dgv_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            //if (e.ColumnIndex == 0) return;
            DataGridView dgv = sender as DataGridView;
            checkDGVRows(sender, e, dgvHelpers[dgv.Name].SelectedCounter);
        }

        //private void dgv_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e, DGVHelper dgvHelper)
        private void dgv_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            //DGVHelper dgvHelper = new DGVHelper();
            //switch (dgv.Name)
            //{
            //    case "dgvSMSReplies":
            //        dgvHelper = ref dgvHelpers["dgvSMSReplies"];
            //        break;

            //    case "dgvClinicExceptions":
            //        dgvHelper = ref dgvHelpers["dgvSMSReplies"];
            //        break;
            //}
            List<string> _rowCellNames = new List<string>();
            foreach (DataGridViewRow row in dgv.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    _rowCellNames.Add(cell.OwningColumn.DataPropertyName);
                }
            }

            //List<string> _colNames = new List<string>();
            //foreach(var _c in dgv.Columns)
            //{
            //    _colNames.Add(dgv.Columns[_c.ToString()].own);
            //}

            foreach (DataGridViewRow row in dgv.Rows)
            {
                string c = dgvHelpers[dgv.Name].colID;
                if (dgv.Columns.Contains("DonorSSN") && dgv.Columns.Contains("SSN" + c) && row.Cells["DonorSSN"].Value != null && row.Cells["DonorSSN"].Value.ToString() != string.Empty)
                {
                    if (row.Cells["DonorSSN"].Value.ToString().Length == 11)
                    {
                        row.Cells["SSN" + c].Value = "***-**-" + row.Cells["DonorSSN"].Value.ToString().Substring(7);
                        dgvHelpers[dgv.Name].SetCellMatch("SSN" + c, "DonorSSN");
                    }
                }

                // formfox specific
                //if (c=="_ffo")
                //{
                //    if (dgv.Columns.Contains("donor_ssn_ffo") && row.Cells["donor_ssn_ffo"].Value.ToString() != string.Empty)
                //    {
                //        if (row.Cells["donor_ssn_ffo"].Value.ToString().Length == 11)
                //        {
                //            row.Cells["donor_ssn_ffo"].Value = " * **-**-" + row.Cells["donor_ssn_ffo"].Value.ToString().Substring(7);
                //            //dgvHelpers[dgv.Name].SetCellMatch("SSN" + c, "DonorSSN");
                //        }
                //    }
                //}


                if (dgv.Columns.Contains("DonorDateOfBirth") && dgv.Columns.Contains("DOB" + c) && row.Cells["DonorDateOfBirth"].Value != null && row.Cells["DonorDateOfBirth"].Value.ToString() != string.Empty)
                {
                    DateTime dob = Convert.ToDateTime(row.Cells["DonorDateOfBirth"].Value);
                    if (dob != DateTime.MinValue)
                    {
                        row.Cells["DOB" + c].Value = dob.ToString("MM/dd/yyyy");
                        dgvHelpers[dgv.Name].SetCellMatch("DOB" + c, "DonorDateOfBirth");
                    }
                }

                if (dgv.Columns.Contains("SpecimenDate") && dgv.Columns.Contains("SpecimenDateValue" + c) && row.Cells["SpecimenDate"].Value != null && row.Cells["SpecimenDate"].Value.ToString() != string.Empty)
                {
                    DateTime specimenDate = Convert.ToDateTime(row.Cells["SpecimenDate"].Value);
                    if (specimenDate != DateTime.MinValue)
                    {
                        row.Cells["SpecimenDateValue" + c].Value = specimenDate.ToString("MM/dd/yyyy");
                        dgvHelpers[dgv.Name].SetCellMatch("SpecimenDateValue" + c, "SpecimenDate");
                    }
                }

                if (dgv.Columns.Contains("MROTypeId") && dgv.Columns.Contains("MROType" + c) && dgv.Columns.Contains("MROType" + c) && row.Cells["MROTypeId"].Value != null && row.Cells["MROTypeId"].Value.ToString() != string.Empty)
                {
                    ClientMROTypes clientMRoTypes = (ClientMROTypes)((int)row.Cells["MROTypeId"].Value);
                    row.Cells["MROType" + c].Value = clientMRoTypes.ToString();
                    dgvHelpers[dgv.Name].SetCellMatch("MROType" + c, "MROTypeId");
                }




                if (dgv.Columns.Contains("PaymentTypeId") && dgv.Columns.Contains("PaymentType" + c) && row.Cells["PaymentTypeId"].Value != null && row.Cells["PaymentTypeId"].Value.ToString() != string.Empty)
                {
                    ClientPaymentTypes clientPaymentTypes = (ClientPaymentTypes)((int)row.Cells["PaymentTypeId"].Value);
                    row.Cells["PaymentType" + c].Value = clientPaymentTypes.ToDescriptionString();
                    dgvHelpers[dgv.Name].SetCellMatch("PaymentType" + c, "PaymentTypeId");
                }

                if (dgv.Columns.Contains("ReasonForTestId") && dgv.Columns.Contains("TestReason" + c) && row.Cells["ReasonForTestId"].Value != null && row.Cells["ReasonForTestId"].Value.ToString() != string.Empty)
                {
                    TestInfoReasonForTest testInfoReasonForTest = (TestInfoReasonForTest)((int)row.Cells["ReasonForTestId"].Value);
                    row.Cells["TestReason" + c].Value = testInfoReasonForTest.ToDescriptionString();
                    dgvHelpers[dgv.Name].SetCellMatch("TestReason" + c, "ReasonForTestId");
                }
                if (dgv.Columns.Contains("TestOverallResult") && dgv.Columns.Contains("Result" + c) && row.Cells["TestOverallResult"].Value != null && row.Cells["TestOverallResult"].Value.ToString() != string.Empty)
                {
                    OverAllTestResult result = (OverAllTestResult)((int)row.Cells["TestOverallResult"].Value);
                    if (result.ToString() != "None")
                    {
                        row.Cells["Result" + c].Value = result.ToDescriptionString();
                    }
                    else
                    {
                        row.Cells["Result" + c].Value = " ";
                    }
                    dgvHelpers[dgv.Name].SetCellMatch("Result" + c, "TestOverallResult");
                }

                if (dgv.Columns.Contains("PaymentReceived") && dgv.Columns.Contains("PaymentMode" + c) && row.Cells["PaymentReceived"].Value.ToString() == "1")  //&& row.Cells["DonorRegistrationStatus"].Value != "Pre-Registered"
                {
                    if (row.Cells["PaymentMethodId"].Value != null && row.Cells["PaymentMethodId"].Value.ToString() != string.Empty)
                    {
                        PaymentMethod paymentMethod = (PaymentMethod)((int)row.Cells["PaymentMethodId"].Value);
                        row.Cells["PaymentMode" + c].Value = paymentMethod.ToString();
                        dgvHelpers[dgv.Name].SetCellMatch("PaymentMode" + c, "PaymentMethodId");
                    }
                }

                if (dgv.Columns.Contains("DonorTestRegisteredDate") && dgv.Columns.Contains("DonorTestRegisteredDateValue" + c) && row.Cells["DonorTestRegisteredDate"].Value != null && row.Cells["DonorTestRegisteredDate"].Value.ToString() != string.Empty)
                {
                    DateTime paymentDate = Convert.ToDateTime(row.Cells["DonorTestRegisteredDate"].Value);
                    if (paymentDate != DateTime.MinValue)
                    {
                        row.Cells["DonorTestRegisteredDateValue" + c].Value = paymentDate.ToString("MM/dd/yyyy");
                        dgvHelpers[dgv.Name].SetCellMatch("DonorTestRegisteredDateValue" + c, "DonorTestRegisteredDate");
                    }
                }

                if (dgv.Columns.Contains("PaymentDate") && dgv.Columns.Contains("PaymentDateValue" + c) && row.Cells["PaymentDate"].Value != null && row.Cells["PaymentDate"].Value.ToString() != string.Empty)
                {
                    DateTime paymentDate = Convert.ToDateTime(row.Cells["PaymentDate"].Value);
                    if (paymentDate != DateTime.MinValue)
                    {
                        row.Cells["PaymentDateValue" + c].Value = paymentDate.ToString("MM/dd/yyyy");
                        dgvHelpers[dgv.Name].SetCellMatch("PaymentDateValue" + c, "PaymentDate");
                    }
                }
                // the column TestStatus is hidden & databound, the colum Status is visible, the cell is what is shown
                if (dgv.Columns.Contains("TestStatus") && dgv.Columns.Contains("Status" + c) && row.Cells["TestStatus"].Value != null && row.Cells["TestStatus"].Value.ToString() != string.Empty)
                {
                    DonorRegistrationStatus status = (DonorRegistrationStatus)((int)row.Cells["TestStatus"].Value);
                    row.Cells["Status" + c].Value = status.ToDescriptionString();
                    dgvHelpers[dgv.Name].SetCellMatch("Status" + c, "TestStatus");
                }
                else if (dgv.Columns.Contains("DonorRegistrationStatus") && dgv.Columns.Contains("Status" + c) && row.Cells["DonorRegistrationStatus"].Value != null && row.Cells["DonorRegistrationStatus"].Value.ToString() != string.Empty)
                {
                    DonorRegistrationStatus status = (DonorRegistrationStatus)((int)row.Cells["DonorRegistrationStatus"].Value);
                    if (status == Enum.DonorRegistrationStatus.PreRegistration)
                    {
                        row.Cells["Status" + c].Value = "Pre-Registered";
                    }
                    else
                    {
                        row.Cells["Status" + c].Value = status.ToDescriptionString();
                    }
                    dgvHelpers[dgv.Name].SetCellMatch("Status" + c, "TestStatus");
                }

                //FormFox specific

                //if (dgv.Columns.Contains("IsArchived") && row.Cells["archived" + c].Value != null)
                if (dgv.Columns.Contains("IsArchived" + c) && row.Cells["archived" + c].Value != null)
                {
                    bool _archived = Convert.ToBoolean(row.Cells["archived" + c].Value);
                    row.Cells["IsArchived" + c].Value = _archived.ToString();
                    //dgvHelpers[dgv.Name].SetCellMatch("MROType" + c, "MROTypeId");
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
            //foreach (DataGridViewColumn column in dgv.Columns)
            //{
            //    Debug.WriteLine($"Checking column {column.Name}");
            //    if (!dgvHelpers[dgv.Name].KnownCells.Contains(column.Name))
            //    {
            //        Debug.WriteLine($"-- Hiding {column.Name}");
            //        column.Visible = false;
            //    }
            //    else
            //    {
            //        column.Visible = true;
            //    }
            //}

            foreach (DataGridViewColumn column in dgv.Columns)
            {
                //dgv.Columns[column.Name].DisplayIndex = column.Index;
            }

            dgvHelpers[dgv.Name].SelectedCounter.RowsAvailable = dgv.Rows.Count > 0;


            GridsUpdating = false;
        }

        #endregion genericHandlers

        #region smsreplies

        private void dgvSMSReplies_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            _logger.Debug("dmd replies data binding finished");
            dgv_DataBindingComplete(sender, e);
            if (dgvHelpers["dgvSMSReplies"].SetTabs == true)
            {
                _logger.Debug("sms setting tabs");
                SetTabs();
            }
            else
            {
                _logger.Debug("sms skipping setting tabs");
            }
            dgvHelpers["dgvSMSReplies"].SetTabs = true;
            _logger.Debug("sms set tabs reverted to true");

            //DataGridView dgv = sender as DataGridView;
            //DGVHelper dgvHelper = new DGVHelper();
            //switch(dgv.Name)
            //{
            //    case "dgvSMSReplies":
            //        dgvHelper = dgvHelpers["dgvSMSReplies"];
            //        break;

            //    case "dgvClinicExceptions":
            //        dgvHelper = dgvHelpers["dgvSMSReplies"];
            //        break;
            //}
            //foreach (DataGridViewRow row in dgvSMSReplies.Rows)
            //{
            //    if (dgv.Columns.Contains("DonorSSN") && row.Cells["DonorSSN"].Value != null && row.Cells["DonorSSN"].Value.ToString() != string.Empty)
            //    {
            //        if (row.Cells["DonorSSN"].Value.ToString().Length == 11)
            //        {
            //            row.Cells["SSN"].Value = "***-**-" + row.Cells["DonorSSN"].Value.ToString().Substring(7);
            //            dgvHelper.SetCellMatch("SSN", "DonorSSN");
            //        }
            //    }

            //    if (dgv.Columns.Contains("DonorDateOfBirth") && row.Cells["DonorDateOfBirth"].Value != null && row.Cells["DonorDateOfBirth"].Value.ToString() != string.Empty)
            //    {
            //        DateTime dob = Convert.ToDateTime(row.Cells["DonorDateOfBirth"].Value);
            //        if (dob != DateTime.MinValue)
            //        {
            //            row.Cells["DOB"].Value = dob.ToString("MM/dd/yyyy");
            //            dgvHelper.SetCellMatch("DOB", "DonorDateOfBirth");

            //        }
            //    }

            //    if (dgv.Columns.Contains("SpecimenDate") && row.Cells["SpecimenDate"].Value != null && row.Cells["SpecimenDate"].Value.ToString() != string.Empty)
            //    {
            //        DateTime specimenDate = Convert.ToDateTime(row.Cells["SpecimenDate"].Value);
            //        if (specimenDate != DateTime.MinValue)
            //        {
            //            row.Cells["SpecimenDateValue"].Value = specimenDate.ToString("MM/dd/yyyy");
            //            dgvHelper.SetCellMatch("SpecimenDateValue", "SpecimenDate");

            //        }
            //    }

            //    if (dgv.Columns.Contains("MROTypeId") && row.Cells["MROTypeId"].Value != null && row.Cells["MROTypeId"].Value.ToString() != string.Empty)
            //    {
            //        ClientMROTypes clientMRoTypes = (ClientMROTypes)((int)row.Cells["MROTypeId"].Value);
            //        row.Cells["MROType"].Value = clientMRoTypes.ToString();
            //        dgvHelper.SetCellMatch("MROType", "MROTypeId");

            //    }

            //    if (dgv.Columns.Contains("PaymentTypeId") && row.Cells["PaymentTypeId"].Value != null && row.Cells["PaymentTypeId"].Value.ToString() != string.Empty)
            //    {
            //        ClientPaymentTypes clientPaymentTypes = (ClientPaymentTypes)((int)row.Cells["PaymentTypeId"].Value);
            //        row.Cells["PaymentType"].Value = clientPaymentTypes.ToDescriptionString();
            //        dgvHelper.SetCellMatch("PaymentType", "PaymentTypeId");

            //    }

            //    if (dgv.Columns.Contains("ReasonForTestId") && row.Cells["ReasonForTestId"].Value != null && row.Cells["ReasonForTestId"].Value.ToString() != string.Empty)
            //    {
            //        TestInfoReasonForTest testInfoReasonForTest = (TestInfoReasonForTest)((int)row.Cells["ReasonForTestId"].Value);
            //        row.Cells["TestReason"].Value = testInfoReasonForTest.ToDescriptionString();
            //        dgvHelper.SetCellMatch("TestReason", "ReasonForTestId");

            //    }
            //    if (dgv.Columns.Contains("TestOverallResult") && row.Cells["TestOverallResult"].Value != null && row.Cells["TestOverallResult"].Value.ToString() != string.Empty)
            //    {
            //        OverAllTestResult result = (OverAllTestResult)((int)row.Cells["TestOverallResult"].Value);
            //        if (result.ToString() != "None")
            //        {
            //            row.Cells["Result"].Value = result.ToDescriptionString();
            //        }
            //        else
            //        {
            //            row.Cells["Result"].Value = " ";
            //        }
            //        dgvHelper.SetCellMatch("Result", "TestOverallResult");

            //    }

            //    if (dgv.Columns.Contains("PaymentReceived") && row.Cells["PaymentReceived"].Value.ToString() == "1")  //&& row.Cells["DonorRegistrationStatus"].Value != "Pre-Registered"
            //    {
            //        if (row.Cells["PaymentMethodId"].Value != null && row.Cells["PaymentMethodId"].Value.ToString() != string.Empty)
            //        {
            //            PaymentMethod paymentMethod = (PaymentMethod)((int)row.Cells["PaymentMethodId"].Value);
            //            row.Cells["PaymentMode"].Value = paymentMethod.ToString();
            //            dgvHelper.SetCellMatch("PaymentMode", "PaymentMethodId");

            //        }
            //    }

            //    if (dgv.Columns.Contains("DonorTestRegisteredDate") && row.Cells["DonorTestRegisteredDate"].Value != null && row.Cells["DonorTestRegisteredDate"].Value.ToString() != string.Empty)
            //    {
            //        DateTime paymentDate = Convert.ToDateTime(row.Cells["DonorTestRegisteredDate"].Value);
            //        if (paymentDate != DateTime.MinValue)
            //        {
            //            row.Cells["DonorTestRegisteredDateValue"].Value = paymentDate.ToString("MM/dd/yyyy");
            //            dgvHelper.SetCellMatch("DonorTestRegisteredDateValue", "DonorTestRegisteredDate");

            //        }
            //    }

            //    if (dgv.Columns.Contains("PaymentDate") && row.Cells["PaymentDate"].Value != null && row.Cells["PaymentDate"].Value.ToString() != string.Empty)
            //    {
            //        DateTime paymentDate = Convert.ToDateTime(row.Cells["PaymentDate"].Value);
            //        if (paymentDate != DateTime.MinValue)
            //        {
            //            row.Cells["PaymentDateValue"].Value = paymentDate.ToString("MM/dd/yyyy");
            //            dgvHelper.SetCellMatch("PaymentDateValue", "PaymentDate");

            //        }
            //    }

            //    if (dgv.Columns.Contains("TestStatus") && row.Cells["TestStatus"].Value != null && row.Cells["TestStatus"].Value.ToString() != string.Empty)
            //    {
            //        DonorRegistrationStatus status = (DonorRegistrationStatus)((int)row.Cells["TestStatus"].Value);
            //        row.Cells["Status"].Value = status.ToDescriptionString();
            //        dgvHelper.SetCellMatch("Status", "TestStatus");

            //    }

            //    else if (dgv.Columns.Contains("DonorRegistrationStatus") && row.Cells["DonorRegistrationStatus"].Value != null && row.Cells["DonorRegistrationStatus"].Value.ToString() != string.Empty)
            //    {
            //        DonorRegistrationStatus status = (DonorRegistrationStatus)((int)row.Cells["DonorRegistrationStatus"].Value);
            //        if (status == Enum.DonorRegistrationStatus.PreRegistration)
            //        {
            //            row.Cells["Status"].Value = "Pre-Registered";
            //        }
            //        else
            //        {
            //            row.Cells["Status"].Value = status.ToDescriptionString();
            //        }
            //        dgvHelper.SetCellMatch("Status", "TestStatus");

            //    }
            //}

            //// hid any rows not defined at design time
            //foreach (DataGridViewColumn column in dgvSMSReplies.Columns)
            //{
            //    if (!dgvHelper.KnownCells.Contains(column.Name)) column.Visible = false;
            //}

            //dgvHelper.SelectedCounter.RowsAvailable = dgvClinicExceptions.Rows.Count > 0;

            //SetTabs();
        }

        private void dgvSMSReplies_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
        }

        private void dgvSMSReplies_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (GridsUpdating) return;
            if (dgvSMSReplies.Columns[e.ColumnIndex].CellType.Name == "DataGridViewButtonCell") return;
            try
            {
                string c = dgvHelpers[dgvSMSReplies.Name].colID;
                Cursor.Current = Cursors.WaitCursor;

                if (e.RowIndex >= 0)
                {
                    Dictionary<string, string> tabPageList = new Dictionary<string, string>();

                    string donor_id = dgvSMSReplies.Rows[e.RowIndex].Cells["DonorId"].Value.ToString();
                    string donor_test_info_id = dgvClinicExceptions.Rows[e.RowIndex].Cells["DonorTestInfoId"].Value.ToString();
                    string donorFistName = dgvSMSReplies.Rows[e.RowIndex].Cells["DonorFirstName" + c].Value.ToString();
                    string donorLastName = dgvSMSReplies.Rows[e.RowIndex].Cells["DonorLastName" + c].Value.ToString();

                    donor_test_info_id = donor_test_info_id != string.Empty ? donor_test_info_id : "0";

                    if (donor_id != string.Empty && donor_test_info_id != string.Empty)
                    {
                        string key = donor_id + "#" + donor_test_info_id;
                        string value = donorFistName + " " + donorLastName;
                        if (!tabPageList.ContainsKey(key))
                        {
                            tabPageList.Add(key, value);
                        }

                        //DataGridViewCheckBoxCell fieldSelection = (DataGridViewCheckBoxCell)dgvSMSReplies.Rows[e.RowIndex].Cells["DonorSelection"+c];

                        //if (Convert.ToBoolean(fieldSelection.Value))
                        //{
                        //    fieldSelection.Value = false;
                        //}
                        //else
                        //{
                        //    fieldSelection.Value = true;
                        //}
                    }

                    LoadDonorDetails(tabPageList);
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvSMSReplies_SelectionChanged(object sender, EventArgs e)
        {
            dgvSMSReplies.ClearSelection();
        }

        private void dgvSMSReplies_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (GridsUpdating) return;
            GridsUpdating = true;
            if (e.RowIndex < 0) return;
            DataGridView dgv = sender as DataGridView;
            object o = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (dgv != null)
            {
                if (o.GetType().Name.Equals("DataGridViewButtonCell", StringComparison.InvariantCultureIgnoreCase))
                {
                    int donor_test_info_id = 0;
                    int client_id = 0;
                    int client_department_id = 0;
                    int donor_id = 0;
                    int.TryParse(dgv.Rows[e.RowIndex].Cells["DonorId"].Value.ToString(), out donor_id);
                    int.TryParse(dgv.Rows[e.RowIndex].Cells["donor_test_info_id"].Value.ToString(), out donor_test_info_id);
                    int.TryParse(dgv.Rows[e.RowIndex].Cells["client_id"].Value.ToString(), out client_id);
                    int.TryParse(dgv.Rows[e.RowIndex].Cells["client_department_id"].Value.ToString(), out client_department_id);

                    DataGridViewButtonCell b = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewButtonCell;
                    if (dgv.Columns[e.ColumnIndex].Name.Equals("ReplyToSms_sms", StringComparison.InvariantCultureIgnoreCase))
                    {
                        FrmSmsSendForm frmSmsSendForm = new FrmSmsSendForm(_logger);

                        frmSmsSendForm.donor_test_info_id = donor_test_info_id;
                        frmSmsSendForm.client_department_id = client_department_id;
                        frmSmsSendForm.client_id = client_id;
                        frmSmsSendForm.donor_id = donor_id;
                        frmSmsSendForm.activity_user_id = Program.currentUserId;
                        DialogResult smsSendDR = frmSmsSendForm.ShowDialog(this);
                        frmSmsSendForm.Dispose();
                        if (smsSendDR == DialogResult.OK)
                        {
                            int backend_sms_activity_id = 0;
                            int.TryParse(dgv.Rows[e.RowIndex].Cells["backend_sms_activity_id"].Value.ToString(), out backend_sms_activity_id);
                            if (backend_sms_activity_id > 0 && donor_test_info_id > 0)
                            {
                                _logger.Debug($"Setting sms as read for backend_sms_activity_id {backend_sms_activity_id}");
                                backendLogic.SetSMSActivityAsRead(donor_test_info_id, backend_sms_activity_id, Program.currentUserId);
                                dgv.Rows.Remove(dgv.Rows[e.RowIndex]);
                            }
                        }
                        else if (smsSendDR == DialogResult.Abort)
                        {
                            MessageBox.Show("There was problem sending an SMS");
                        }
                        frmSmsSendForm.Dispose();
                    }
                    if (dgv.Columns[e.ColumnIndex].Name.Equals("MarkAsRead_sms", StringComparison.InvariantCultureIgnoreCase))
                    {
                        //MarkAsRead_sms
                        int backend_sms_activity_id = 0;
                        int.TryParse(dgv.Rows[e.RowIndex].Cells["backend_sms_activity_id"].Value.ToString(), out backend_sms_activity_id);
                        if (backend_sms_activity_id > 0 && donor_test_info_id > 0)
                        {
                            _logger.Debug($"Setting sms as read for backend_sms_activity_id {backend_sms_activity_id}");
                            backendLogic.SetSMSActivityAsRead(donor_test_info_id, backend_sms_activity_id, Program.currentUserId);
                            dgv.Rows.Remove(dgv.Rows[e.RowIndex]);
                        }
                        
                    }
                    dgvHelpers["dgvSMSReplies"].SetTabs = false;
                    PopulateSMSReplies();
                }
            }
        }

        #endregion smsreplies

        #region formfox
        private void dgvFormFox_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv_DataBindingComplete(sender, e);
            SetTabs();

        }

        private void dgvFormFox_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
        }

        private void dgvFormFox_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Debug.WriteLine("dgvFormFox_CellMouseDoubleClick");
            if (GridsUpdating) return;
            DataGridView dgv = sender as DataGridView;
            string c = dgvHelpers[dgv.Name].colID;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (e.RowIndex >= 0)
                {
                    Dictionary<string, string> tabPageList = new Dictionary<string, string>();

                    string donorId = dgv.Rows[e.RowIndex].Cells["donor_id"].Value.ToString();
                    string donorTestInfoId = dgv.Rows[e.RowIndex].Cells["donor_test_info_id"].Value.ToString();
                    string donorFistName = dgv.Rows[e.RowIndex].Cells.GetCellValueFromColumnHeader("First Name").ToString();
                    string donorLastName = dgv.Rows[e.RowIndex].Cells.GetCellValueFromColumnHeader("Last Name").ToString();

                    donorTestInfoId = donorTestInfoId != string.Empty ? donorTestInfoId : "0";

                    if (donorId != string.Empty && donorTestInfoId != string.Empty)
                    {
                        string key = donorId + "#" + donorTestInfoId;
                        string value = donorFistName + " " + donorLastName;
                        if (!tabPageList.ContainsKey(key))
                        {
                            tabPageList.Add(key, value);
                        }

                        //DataGridViewCheckBoxCell fieldSelection = (DataGridViewCheckBoxCell)dgv.Rows[e.RowIndex].Cells["DonorSelection"+c];

                        //if (Convert.ToBoolean(fieldSelection.Value))
                        //{
                        //    fieldSelection.Value = false;
                        //}
                        //else
                        //{
                        //    fieldSelection.Value = true;
                        //}
                    }

                    LoadDonorDetails(tabPageList);
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }
        private void dgvFormFox_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            Debug.WriteLine("dgvFormFox_CellMouseUp");

            checkDGVRows(sender, e, dgvHelpers["dgvFormFox"].SelectedCounter);

        }
        private void dgvFormFox_SelectionChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("dgvFormFox_SelectionChanged");

            //dgvSMSReplies.ClearSelection();
        }

        private void dgvFormFox_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Debug.WriteLine("dgvFormFox_CellClick");
            //if (GridsUpdating) return;
            //GridsUpdating = true;
            //if (e.RowIndex < 0) return;
            //DataGridView dgv = sender as DataGridView;
            //object o = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
            //if (dgv != null)
            //{
            //    if (o.GetType().Name.Equals("DataGridViewButtonCell", StringComparison.InvariantCultureIgnoreCase))
            //    {
            //        int donor_test_info_id = 0;
            //        int client_id = 0;
            //        int client_department_id = 0;
            //        int donor_id = 0;
            //        int.TryParse(dgv.Rows[e.RowIndex].Cells["DonorId"].Value.ToString(), out donor_id);
            //        int.TryParse(dgv.Rows[e.RowIndex].Cells["donor_test_info_id"].Value.ToString(), out donor_test_info_id);
            //        int.TryParse(dgv.Rows[e.RowIndex].Cells["client_id"].Value.ToString(), out client_id);
            //        int.TryParse(dgv.Rows[e.RowIndex].Cells["client_department_id"].Value.ToString(), out client_department_id);

            //        DataGridViewButtonCell b = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewButtonCell;
            //        if (dgv.Columns[e.ColumnIndex].Name.Equals("ReplyToSms_sms", StringComparison.InvariantCultureIgnoreCase))
            //        {
            //            FrmSmsSendForm frmSmsSendForm = new FrmSmsSendForm(_logger);

            //            frmSmsSendForm.donor_test_info_id = donor_test_info_id;
            //            frmSmsSendForm.client_department_id = client_department_id;
            //            frmSmsSendForm.client_id = client_id;
            //            frmSmsSendForm.donor_id = donor_id;
            //            frmSmsSendForm.activity_user_id = Program.currentUserId;
            //            DialogResult smsSendDR = frmSmsSendForm.ShowDialog(this);
            //            frmSmsSendForm.Dispose();
            //            if (smsSendDR == DialogResult.OK)
            //            {
            //            }
            //            else if (smsSendDR == DialogResult.Abort)
            //            {
            //                MessageBox.Show("There was problem sending an SMS");
            //            }
            //            frmSmsSendForm.Dispose();
            //        }
            //        if (dgv.Columns[e.ColumnIndex].Name.Equals("MarkAsRead_sms", StringComparison.InvariantCultureIgnoreCase))
            //        {
            //            //MarkAsRead_sms
            //            int backend_sms_activity_id = 0;
            //            int.TryParse(dgv.Rows[e.RowIndex].Cells["backend_sms_activity_id"].Value.ToString(), out backend_sms_activity_id);
            //            if (backend_sms_activity_id > 0 && donor_test_info_id > 0)
            //            {
            //                backendLogic.SetSMSActivityAsRead(donor_test_info_id, backend_sms_activity_id, Program.currentUserId);
            //            }
            //        }

            //        PopulateSMSReplies();
            //    }
            //}
        }

        #endregion formfox


        private void dgvDeadlineDonors_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv_DataBindingComplete(sender, e);
            SetTabs();

        }

        private void dgvDeadlineDonors_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (GridsUpdating) return;
            DataGridView dgv = sender as DataGridView;
            string c = dgvHelpers[dgv.Name].colID;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (e.RowIndex >= 0)
                {
                    Dictionary<string, string> tabPageList = new Dictionary<string, string>();

                    string donorId = dgv.Rows[e.RowIndex].Cells["DonorId"].Value.ToString();
                    string donorTestInfoId = dgv.Rows[e.RowIndex].Cells["DonorTestInfoId"].Value.ToString();
                    string donorFistName = dgv.Rows[e.RowIndex].Cells["DonorFirstName" + c].Value.ToString();
                    string donorLastName = dgv.Rows[e.RowIndex].Cells["DonorLastName" + c].Value.ToString();

                    donorTestInfoId = donorTestInfoId != string.Empty ? donorTestInfoId : "0";

                    if (donorId != string.Empty && donorTestInfoId != string.Empty)
                    {
                        string key = donorId + "#" + donorTestInfoId;
                        string value = donorFistName + " " + donorLastName;
                        if (!tabPageList.ContainsKey(key))
                        {
                            tabPageList.Add(key, value);
                        }

                        //DataGridViewCheckBoxCell fieldSelection = (DataGridViewCheckBoxCell)dgv.Rows[e.RowIndex].Cells["DonorSelection"+c];

                        //if (Convert.ToBoolean(fieldSelection.Value))
                        //{
                        //    fieldSelection.Value = false;
                        //}
                        //else
                        //{
                        //    fieldSelection.Value = true;
                        //}
                    }

                    LoadDonorDetails(tabPageList);
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvSendInScheduler_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv_DataBindingComplete(sender, e);
            SetTabs();

        }

        private void dgvSendInScheduler_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridView dgv = sender as DataGridView;
                if (e.RowIndex >= 0)
                {
                    int selectedIndex = e.RowIndex;
                    int clientDepartmentId = (int)dgv.Rows[e.RowIndex].Cells["client_department_id"].Value;
                    int clientId = (int)dgv.Rows[e.RowIndex].Cells["client_id"].Value;
                    string clientName = (string)dgv.Rows[e.RowIndex].Cells["client_name_sis"].Value;
                    FrmClientDepartmentDetails frmClientDepartmentDetails = new FrmClientDepartmentDetails(Enum.OperationMode.Edit, clientId, clientDepartmentId);
                    frmClientDepartmentDetails.Tag = ClientName;
                    frmClientDepartmentDetails.ShowDialog();

                    PopulateSendInScheduleExceptions();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void tabExceptionNotifications_Click(object sender, EventArgs e)
        {
        }

        private void trackBarMiles_Scroll(object sender, EventArgs e)
        {

        }

        private void ffIncludeArchived_CheckedChanged(object sender, EventArgs e)
        {
            PopulateFormFoxStatus();
        }

        private void ffRefresh_Click(object sender, EventArgs e)
        {
            PopulateFormFoxStatus();
        }

        private void tabFormFox_Click(object sender, EventArgs e)
        {

        }

        private void ffFlipArchive_Click(object sender, EventArgs e)
        {
            //checkDGVRows(dgvFormFox, e, dgvHelpers[dgvFormFox.Name].SelectedCounter);
            setDGVSelected(dgvFormFox, dgvHelpers[dgvFormFox.Name].SelectedCounter);
            try
            {

                List<string> _ids = new List<string>();

                for (int id = 0; id < dgvFormFox.Rows.Count; id++)
                {

                    if (dgvFormFox.Rows[id].Selected == true)
                    {
                        string ReferenceTestID = dgvFormFox.Rows[id].Cells["ReferenceTestID_ffo"].Value.ToString().Trim();
                        _ids.Add(ReferenceTestID);
                    }
                }
                backendLogic.FlipFormFoxArchiveBit(_ids, Program.currentUserId);

                PopulateFormFoxStatus();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                MessageBox.Show("There was a problem archiving / unarchiving FormFox orders!\r\nPlease copy all this content and communicate it to report the error.\r\n" + ex.Message, "Error");
                //                throw;

            }
        }


        //private void dgvClinicExceptions_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //    //if (e.ColumnIndex == 0) return;
        //    //checkDGVRows(sender, e, dgvClinicExceptions, dgvHelpers["dgvClinicExceptions"].SelectedCounter);
        //    checkDGVRows(sender, e, dgvHelpers["dgvClinicExceptions"].SelectedCounter);
        //}





        //private void dgvFormFox_CellClick(object sender, DataGridViewCellEventArgs e)
        //{

        //}

        //public void ShowMyDialogBox()
        //{
        //    Form2 testDialog = new Form2();

        //    // Show testDialog as a modal dialog and determine if DialogResult = OK.
        //    if (testDialog.ShowDialog(this) == DialogResult.OK)
        //    {
        //        // Read the contents of testDialog's TextBox.
        //        this.txtResult.Text = testDialog.TextBox1.Text;
        //    }
        //    else
        //    {
        //        this.txtResult.Text = "Cancelled";
        //    }
        //    testDialog.Dispose();
        //}
    }

    public class RangeHolder
    {
        public decimal Range { get; set; } = 50;
        public bool dragging { get; set; } = false;
    }

    public class SelectedCounter : INotifyPropertyChanged
    {
        private int _count = 0;
        private bool _RowsAvailable = false;
        public bool Enabled { get; set; } = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public int SelectedCount
        {
            get { return _count; }
            set
            {
                if (_count != value)
                {
                    _count = value;
                    Enabled = _count > 0;
                    if (!(PropertyChanged == null)) PropertyChanged(this, new PropertyChangedEventArgs("Enabled"));
                }
            }
        }

        public bool RowsAvailable
        {
            get { return _RowsAvailable; }
            set
            {
                if (_RowsAvailable != value)
                {
                    _RowsAvailable = value;

                    if (!(PropertyChanged == null)) PropertyChanged(this, new PropertyChangedEventArgs("RowsAvailable"));
                }
            }
        }

        //dgvSearchResult.Rows.Count
    }

    public class ValueOverZero : INotifyPropertyChanged
    {
        private int _count = 0;
        public bool Enabled { get; set; } = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Count
        {
            get { return _count; }
            set
            {
                if (_count != value)
                {
                    _count = value;
                    Enabled = _count > 0;
                    if (!(PropertyChanged == null)) PropertyChanged(this, new PropertyChangedEventArgs("Enabled"));
                }
            }
        }

        //dgvSearchResult.Rows.Count
    }

    public class CheckBoxReverseChecked : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _Enabled = false;

        public CheckBox checkBox
        {
            set
            {
                Binding bd;

                for (int index = value.DataBindings.Count - 1; (index == 0); index--)
                {
                    bd = value.DataBindings[index];
                    if (bd.PropertyName == "Checked")
                        value.DataBindings.Remove(bd);
                }
                value.DataBindings.Add("Checked", this, "Enabled", false, DataSourceUpdateMode.OnPropertyChanged);
            }
        }

        public bool Enabled
        {
            get
            {
                return this._Enabled;
            }
            set
            {
                if (this._Enabled != value)
                {
                    this._Enabled = value;

                    if (!(PropertyChanged == null)) PropertyChanged(this, new PropertyChangedEventArgs("RowsAvailable"));
                }
            }
        }

        public bool NotEnabled
        {
            get { return !this.Enabled; }
        }

        //dgvSearchResult.Rows.Count
    }

    public class DGVHelper
    {
        public SelectedCounter SelectedCounter { get; set; } = new SelectedCounter();
        public Dictionary<string, string> CellMatch { get; set; } = new Dictionary<string, string>();
        public List<string> KnownCells { get; set; } = new List<string>();
        public List<DataGridViewColumn> Columns { get; set; } = new List<DataGridViewColumn>();
        public bool SetTabs { get; set; } = true;
        public void SetCellMatch(string key, string value)
        {
            this.CellMatch[key] = value;
        }

        public string colID { get; set; } = string.Empty;

        private Dictionary<string, int> _colOrder = new Dictionary<string, int>();
        public void ColumnOrder(ref DataGridView dgv, bool set = false)
        {
          // holds the column order and either captures or sets it
          if (set==true)
            {
                _colOrder = new Dictionary<string, int>();

                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    _colOrder.Add(col.Name, col.DisplayIndex);
                }
            }
          else
            {
                foreach(KeyValuePair<string, int> keyValuePair in _colOrder)
                {
                    dgv.Columns[keyValuePair.Key].DisplayIndex = keyValuePair.Value;
                }

            }
        }
    }

    public static class DataGridHelper
    {
        public static object GetCellValueFromColumnHeader(this DataGridViewCellCollection CellCollection, string HeaderText)
        {
            return CellCollection.Cast<DataGridViewCell>().First(c => c.OwningColumn.HeaderText == HeaderText).Value;
        }
    }
}