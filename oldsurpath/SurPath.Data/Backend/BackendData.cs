using MySql.Data.MySqlClient;
using Serilog;
using Surpath.ClearStar.BL;
using SurPath.Entity;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

/// <summary>
/// For time zone data - it'd be best to use an API so they maintain it
/// list of services: https://stackoverflow.com/questions/16086962/how-to-get-a-time-zone-from-a-location-using-latitude-and-longitude-coordinates
/// https://timezonedb.com/ is $50 a year
/// </summary>

namespace SurPath.Data
{
    public class BackendData : IBackendData
    {
        //#####################################
        //#####################################
        //#####################################
        private static ILogger _logger;

        public bool SetParameters(MySqlCommand _cmd, IBackendDataParamObject p, bool DoLogging = true)
        {
            //CALL prcInsertStuff(
            //@paramName1 := nameValue1
            //, @paramValue1:= paramValue1
            //);

            bool retval = false;
            if (DoLogging) _logger.Debug(new string('#', 20) + $" Database Call" + new string('#', 20));
            if (DoLogging) _logger.Debug($"Executing {p.sp()}");
            if (DoLogging) _logger.Debug($"params:");
            string __cmd = $"{p.sp()} (";

            foreach (MySqlParameter param in p.Parameters())
            {
                if (DoLogging) _logger.Debug($"{param.ParameterName} = {param.Value} ({param.DbType})  {param.Direction}");
                _cmd.Parameters.Add(param);
                retval = true;
                string _type = param.DbType.ToString();
                string _out = "out ";
                if (!param.Direction.ToString().Equals("out", StringComparison.InvariantCultureIgnoreCase)) _out = "";
                if (new[] { "Int", "SByte" }.Any(_type.Contains))
                {
                    __cmd += $"{_out} {param.ParameterName} := {param.Value},";
                }
                else
                {
                    __cmd += $"{_out} {param.ParameterName} := \"{param.Value}\",";
                }
            }
            __cmd.TrimEnd(',');
            __cmd += ");";
            if (DoLogging) _logger.Debug($"");
            if (DoLogging) _logger.Debug(__cmd);
            if (DoLogging) _logger.Debug($"");

            return retval;
        }

        public string ConnectionString { get; set; }
        public CultureInfo Culture { get; set; }
        public MySqlTransaction TransactionObject { get; set; }
        public MySqlConnection conn { get; set; }
        //public List<UnsentSMS> Notification { get; private set; }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public BackendData(string _ConnectionString = null, MySqlTransaction trans = null, ILogger __logger = null)
        {
            if (__logger != null)
            {
                _logger = __logger;
                _logger.Debug("BackendData logger online");
                _logger.Debug("Path to this lib: " + AssemblyDirectory);
            }
            else
            {
                _logger = new LoggerConfiguration().CreateLogger();
            }

            this.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString().Trim();
            string cultureSetting = ConfigurationManager.AppSettings["Culture"]?.ToString()?.Trim();
            this.Culture = !string.IsNullOrEmpty(cultureSetting) ? new CultureInfo(cultureSetting) : new CultureInfo("en-US");

            if (!string.IsNullOrEmpty(_ConnectionString)) ConnectionString = _ConnectionString;
            if (trans != null)
            {
                TransactionObject = trans;
                this.conn = trans.Connection;
            }
            else
            {
                this.conn = new MySqlConnection(this.ConnectionString);
            }
        }

        //#####################################
        //#####################################
        //#####################################

        //#####################################

        #region Service

        public List<Notification> GetReadyDonorNotifications(ParamGetReadyDonorNotifications p)
        {
            try
            {
                DataTable dataTable = new DataTable();

                if (this.conn.State == ConnectionState.Closed) conn.Open();
                MySqlCommand cmd = new MySqlCommand(p.sp(), conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    dataTable.Load(reader);
                }

                if (this.TransactionObject == null) conn.Close();
                if (dataTable.Rows.Count < 1) return new List<Notification>();
                List<Notification> ret = dataTable.Rows.OfType<DataRow>().Select(dr =>

                    populateNotificationFromDataRow(dr)
                ).ToList();

                return ret;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Error(ex.InnerException.ToString());
                throw;
            }
        }

        #endregion Service

        //#####################################

        #region sms

        public int LogSMSReply(ParamLogSMSReply p)
        {
            int retval = 0;
            DataTable dataTable = new DataTable();
            if (this.conn.State == ConnectionState.Closed) if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SetParameters(cmd, p);
            cmd.ExecuteNonQuery();
            p.backend_sms_replies_id = (int)cmd.Parameters[p.outputID].Value;

            conn.Close();

            return retval;
        }

        /// <summary>
        /// Add an SMS to be sent to the database
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool QueueSMS(ParamQueueSMS p)
        {
            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SetParameters(cmd, p);
            cmd.ExecuteNonQuery();
            p.backend_sms_queue_id = (int)cmd.Parameters[p.outputID].Value;
            if (this.TransactionObject == null) conn.Close();

            return true;
        }

        public SMSActivity SetSMSActivity(ParamSetSMSActivity p)
        {
            DataTable dataTable = new DataTable();

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            SetParameters(cmd, p);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }

            if (this.TransactionObject == null) conn.Close();
            if (dataTable.Rows.Count == 0) return new SMSActivity();
            SMSActivity ret = dataTable.Rows.OfType<DataRow>().Select(dr =>

                populateSMSActivityFromDataRow(dr)
            ).First();

            return ret;
        }

        public List<SMSActivity> GetSMSActivity(ParamGetSMSActivity p)
        {
            DataTable dataTable = new DataTable();

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            SetParameters(cmd, p);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }

            if (this.TransactionObject == null) conn.Close();
            if (dataTable.Rows.Count == 0) return new List<SMSActivity>();

            List<SMSActivity> ret = dataTable.Rows.OfType<DataRow>().Select(dr =>

                    populateSMSActivityFromDataRow(dr)
             ).ToList();

            return ret;
        }

        public List<SMSActivity> GetUnreadSMSActivity()
        {
            ParamGeneric p = new ParamGeneric() { StoreProc = "backend_get_sms_activity_unread" };
            DataTable dataTable = new DataTable();
            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            SetParameters(cmd, p);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }

            if (this.TransactionObject == null) conn.Close();
            if (dataTable.Rows.Count == 0) return new List<SMSActivity>();

            List<SMSActivity> ret = dataTable.Rows.OfType<DataRow>().Select(dr =>

                    populateSMSActivityFromDataRow(dr)
             ).ToList();

            return ret;
        }

        public List<UnsentSMS> GetUnSentSMS()
        {
            DataTable dataTable = new DataTable();

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand("backend_get_unsent_sms", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }

            if (this.TransactionObject == null) conn.Close();

            List<UnsentSMS> unsentSMs = dataTable.Rows.OfType<DataRow>().Select(dr =>

                new UnsentSMS()
                {
                    backend_notifications_id = dr.Field<int>("backend_notifications_id"),
                    donor_test_info_id = dr.Field<int>("donor_test_info_id"),
                    client_id = dr.Field<int>("client_id"),
                    client_department_id = dr.Field<int>("client_department_id"),
                    donor_id = dr.Field<int>("donor_id"),
                    donor_phone_1 = dr.Field<string>("donor_phone_1"),
                    donor_phone_2 = dr.Field<string>("donor_phone_2"),
                    client_sms_from_id = dr.Field<string>("client_sms_from_id"),
                    client_sms_text = dr.Field<string>("client_sms_text"),
                    client_sms_apikey = dr.Field<string>("client_sms_apikey"),
                    client_sms_token = dr.Field<string>("client_sms_token"),
                    created_on = dr.Field<DateTime>("created_on"),
                    notified_by_sms_timestamp = dr.Field<DateTime>("notified_by_sms_timestamp")
                }
            ).ToList();

            return unsentSMs;
        }

        /// <summary>
        /// Saves an SMS reply to the database - TODO refactor to be autoresponse and match table name
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>

        public bool MarkSMSAsSent(int donor_test_info_id)
        {
            bool retval = false;

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand("backend_set_sms_sent", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter("@donor_test_info_id", donor_test_info_id));
            cmd.ExecuteNonQuery();
            retval = true;
            if (this.TransactionObject == null) conn.Close();

            return retval;
        }

        public DataTable GetSMSActivityUnread()
        {
            DataTable dataTable = new DataTable();

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand("backend_get_sms_activity_unread_dataview", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }
            if (this.TransactionObject == null) conn.Close();

            if (dataTable.Rows.Count == 0) return new DataTable();

            return dataTable;
        }

        public DataTable GetSendInScheduleExceptions()
        {
            DataTable dataTable = new DataTable();

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand("backend_get_Send_in_Schedule_Exceptions_dataview", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }
            if (this.TransactionObject == null) conn.Close();

            if (dataTable.Rows.Count == 0) return new DataTable();

            return dataTable;
        }

        public DataTable GetDeadlineDonors()
        {
            DataTable dataTable = new DataTable();

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand("backend_get_deadline_donors_dataview", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }
            if (this.TransactionObject == null) conn.Close();

            if (dataTable.Rows.Count == 0) return new DataTable();

            return dataTable;
        }

        #endregion sms

        #region formfox

        public DataTable GetFormFoxOverdue(int daysAgo, bool includeArchived = false)
        {
            DataTable dataTable = new DataTable();

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand("backend_get_formfoxorders_overdue_datagrid", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter("@daysAgo", daysAgo));

            if (includeArchived == true)
            {
                cmd.Parameters.Add(new MySqlParameter("@includeArchived", 1));
            }
            else
            {
                cmd.Parameters.Add(new MySqlParameter("@includeArchived", 0));
            }
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }
            if (this.TransactionObject == null) conn.Close();

            if (dataTable.Rows.Count == 0) return new DataTable();

            return dataTable;
        }

        #endregion formfox

        //#####################################

        #region UI_Checks

        public bool ExceptionClinicsExist()
        {
            DataTable dataTable = new DataTable();

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand("backend_get_clinic_exception_count", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }

            if (this.TransactionObject == null) conn.Close();

            return dataTable.Rows.OfType<DataRow>().First().Field<Int64>("clinic_exception_count") > 0;
        }

        public int TotalExceptions()
        {
            //DataTable dataTable = new DataTable();

            //if (this.conn.State == ConnectionState.Closed) conn.Open();
            //MySqlCommand cmd = new MySqlCommand("backend_get_exception_count", conn);
            //cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //using (MySqlDataReader reader = cmd.ExecuteReader())
            //{
            //    dataTable.Load(reader);

            //}

            //if (this.TransactionObject == null) conn.Close();

            //return dataTable.Rows.OfType<DataRow>().First().Field<Int32>("ExceptionCount");
            ExceptionCounts e = GetExceptionData();
            return e.ExceptionCount;
        }

        public ExceptionCounts GetExceptionData()
        {
            ParamGeneric p = new ParamGeneric() { StoreProc = "backend_get_exception_count", DoLogging = false };
            DataTable dataTable = new DataTable();
            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            SetParameters(cmd, p, p.DoLogging);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }

            if (this.TransactionObject == null) conn.Close();
            if (dataTable.Rows.Count == 0) return new ExceptionCounts();
            ExceptionCounts res = new ExceptionCounts();
            res.ExceptionCount = (int)dataTable.Rows.OfType<DataRow>().First().Field<Int32>("ExceptionCount");
            res.clinic_exception_count = (int)dataTable.Rows.OfType<DataRow>().First().Field<Int32>("clinic_exception_count");
            res.sms_count = (int)dataTable.Rows.OfType<DataRow>().First().Field<Int32>("sms_count");
            res.sis_count = (int)dataTable.Rows.OfType<DataRow>().First().Field<Int32>("sis_count");
            res.did_count = (int)dataTable.Rows.OfType<DataRow>().First().Field<Int32>("did_count");
            res.ffo_count = (int)dataTable.Rows.OfType<DataRow>().First().Field<Int32>("ffo_count");
            conn.Close();
            return res;
        }

        #endregion UI_Checks

        //#####################################

        #region Donor

        public List<PIDTypeValue> GetDonorPIDS(int donor_id)
        {
            DataTable dataTable = new DataTable();
            if (this.conn.State == ConnectionState.Closed) conn.Open();
            string _sql = "select * from individual_pids where donor_id = @donor_id;";
            ParamHelper p = new ParamHelper();
            p.Param = new MySqlParameter("@donor_id", donor_id);

            // MySqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sqlQuery, p.Params);

            MySqlCommand cmd = new MySqlCommand(_sql, conn);
            foreach (MySqlParameter parameter in p.ParmList)
            {
                cmd.Parameters.Add(parameter);
            }

            // cmd.CommandType = System.Data.CommandType.Text;

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }

            if (this.TransactionObject == null) conn.Close();
            if (dataTable.Rows.Count == 0) return new List<PIDTypeValue>();

            List<PIDTypeValue> ret = dataTable.Rows.OfType<DataRow>().Select(dr =>
                    new PIDTypeValue()
                    {
                        PIDType = varchartoint(dr.Field<string>("pid_type_id")),
                        Err = "",
                        required = false,
                        PIDValue = dr.Field<string>("pid")
                    }
             ).ToList();

            return ret;
        }

        public bool QueueDonorNotification(ParamQueueDonorNotification p)
        {
            if (this.conn.State == ConnectionState.Closed) conn.Open();
            //MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SetParameters(cmd, p);
            cmd.ExecuteNonQuery();
            p.backend_notifications_id = (int)cmd.Parameters[p.outputID].Value;
            if (this.TransactionObject == null) conn.Close();

            return true;
        }

        /// <summary>
        ///  This sets a donor notification - when a donor is registered, this is called
        ///
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Notification SetDonorNotification(ParamSetDonorNotification p)
        {
            try
            {
                _logger.Debug($"SetDonorNotification called");
                if (p.notification.notify_reset_sendin == true)
                {
                    _logger.Debug($"notify_reset_sendin is true");
                    p.notification = UpdateNotificationSchedule(p.notification);
                }

                DataTable dataTable = new DataTable();

                if (this.conn.State == ConnectionState.Closed) conn.Open();
                MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SetParameters(cmd, p);
                //cmd.ExecuteNonQuery();
                //if (this.TransactionObject == null) conn.Close();

                //return true;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    dataTable.Load(reader);
                }
                if (dataTable.Rows.Count < 1) return new Notification();
                if (this.TransactionObject == null) conn.Close();

                Notification ret = dataTable.Rows.OfType<DataRow>().Select(dr =>

                    populateNotificationFromDataRow(dr)
                ).First();

                return ret;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Error(ex.InnerException.ToString());
                throw;
            }
        }

        public Notification UpdateNotificationSchedule(Notification n)
        {
            try
            {
                _logger.Debug($"UpdateNotificationSchedule {n.client_id} {n.client_department_id}");
                ClientNotificationDataSettings clientNotificationDataSettings = GetClientNotificationDataSettings(n.client_id, n.client_department_id);
                _logger.Debug($"clientNotificationDataSettings retrieved");
                ScheduleLoadHelper scheduleLoadHelper = GetClientNotificationDateList(clientNotificationDataSettings);
                _logger.Debug($"scheduleLoadHelper retrieved");
                scheduleLoadHelper.window_dates = scheduleLoadHelper.window_dates.OrderBy(x => Guid.NewGuid()).ToList();
                // assign notification to this send in date
                _logger.Debug($"Asssigning dates");

                n.notify_after_timestamp = scheduleLoadHelper.window_dates.FirstOrDefault();
                n.notify_before_timestamp = scheduleLoadHelper.end.Date;
                _logger.Debug($"returning");

                return n;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Error(ex.InnerException.ToString());
                throw;
            }
        }

        public ScheduleLoadHelper GetClientNotificationDateList(ClientNotificationDataSettings clientNotificationDataSettings, List<Notification> notifications = null)
        {
            try
            {
                _logger.Debug($"GetClientNotificationDateList called - clientNotificationDataSettings.client_id {clientNotificationDataSettings.client_id} clientNotificationDataSettings.client_department_id {clientNotificationDataSettings.client_department_id}");

                ScheduleLoadHelper scheduleLoadHelper = new ScheduleLoadHelper();
                _logger.Debug($"scheduleLoadHelper created");

                if (notifications == null)
                {
                    _logger.Debug($"notifications is null, calling GetReadyDonorNotifications");

                    notifications = GetReadyDonorNotifications(new ParamGetReadyDonorNotifications());
                    _logger.Debug($"GetReadyDonorNotifications returned");
                }
                notifications.RemoveAll(x => x.client_id != clientNotificationDataSettings.client_id);
                notifications.RemoveAll(x => x.client_department_id != clientNotificationDataSettings.client_department_id);

                scheduleLoadHelper.start = clientNotificationDataSettings.notification_start_date ?? DateTime.Now.AddYears(-1); // if not set, go into the past to avoid sending
                scheduleLoadHelper.end = clientNotificationDataSettings.notification_stop_date ?? scheduleLoadHelper.start.AddDays(1);
                scheduleLoadHelper.sweep = clientNotificationDataSettings.notification_sweep_date ?? DateTime.Now.AddYears(1);
                scheduleLoadHelper.max_sendins = clientNotificationDataSettings.max_sendins;
                // filter out any notifications not in sweep / end window.
                notifications = notifications.Where(x => x.created_on.Date >= scheduleLoadHelper.sweep.Date && x.created_on.Date <= scheduleLoadHelper.end.Date).ToList();

                // need to look at all dates between start and stop that are selected days of the week
                scheduleLoadHelper.window_dates = Enumerable.Range(0, 1 + scheduleLoadHelper.end.Subtract(scheduleLoadHelper.start).Days)
                      .Select(offset => scheduleLoadHelper.start.AddDays(offset))
                      .ToList();
                // get rid of dates that aren't send in dates
                foreach (ClientNotificationDataSettingsDay ds in clientNotificationDataSettings.DaySettings)
                {
                    if (ds.Enabled != true)
                    {
                        scheduleLoadHelper.window_dates.RemoveAll(x => x.DayOfWeek == (DayOfWeek)ds.DayOfWeek);
                    }
                }
                int t;
                // Get Counts
                scheduleLoadHelper.window_dates.
                    ForEach(d =>
                        scheduleLoadHelper.counts[d.Date] = notifications.Where(x => ((DateTime)x.notify_after_timestamp).Date == d.Date).Count()
                    );

                // get rid of dates with send ins equal or above max send ins
                scheduleLoadHelper.window_dates.RemoveAll(x => scheduleLoadHelper.counts.Any(d => d.Value >= scheduleLoadHelper.max_sendins && x.Date == d.Key.Date));

                // Get rid of any dates in the past.
                scheduleLoadHelper.window_dates.RemoveAll(x => x.Date <= DateTime.Now.Date);

                return scheduleLoadHelper;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Error(ex.InnerException.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// Updates notifications waiting so they get scheduled / rescheduled if client notificaiton settings are changed.
        /// </summary>
        /// <param name="clientNotificationDataSettings"></param>
        public void UpdateNotificationsOnSettingsChange(ClientNotificationDataSettings clientNotificationDataSettings)
        {
            // We only need to do this if it's a schedule window
            if (clientNotificationDataSettings.force_manual) return;

            List<Notification> notifications = GetReadyDonorNotifications(new ParamGetReadyDonorNotifications());
            // notifications will be filtered after this call
            ScheduleLoadHelper scheduleLoadHelper = GetClientNotificationDateList(clientNotificationDataSettings, notifications);

            // Have each notification update it's send in data
            foreach (Notification n in notifications)
            {
                // SetDonorNotification gets an available date

                ////scheduleLoadHelper.window_dates.RemoveAll(x => scheduleLoadHelper.counts.Any(d => d.Value >= scheduleLoadHelper.max_sendins));
                ////// we don't have any days left to add people to
                ////if (scheduleLoadHelper.window_dates.Count < 1) break;

                ////// shuffle dates to pick a random day with send ins being less than the max send ins
                ////scheduleLoadHelper.window_dates = scheduleLoadHelper.window_dates.OrderBy(x => Guid.NewGuid()).ToList();
                ////// assign notification to this send in date
                ////n.notify_after_timestamp = scheduleLoadHelper.window_dates.FirstOrDefault();
                ////n.notify_before_timestamp = scheduleLoadHelper.end.Date;
                n.notify_reset_sendin = true;
                SetDonorNotification(new ParamSetDonorNotification() { notification = n });
            }
        }

        public bool SetDonorActivity(ParamSetDonorActivity p)
        {
            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SetParameters(cmd, p);
            cmd.ExecuteNonQuery();
            p.donor_test_activity_id = (int)cmd.Parameters[p.outputID].Value;
            if (this.TransactionObject == null) conn.Close();

            return true;
        }

        public bool SetBackend_client_edit_activity(ParamSetBackend_client_edit_activity p)
        {
            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SetParameters(cmd, p);
            cmd.ExecuteNonQuery();
            p.backend_client_edit_activity_id = (int)cmd.Parameters[p.outputID].Value;
            if (this.TransactionObject == null) conn.Close();

            return true;
        }

        // ParamSetBackend_client_edit_activity

        public bool SetUserActivity(ParamSetUserActivity p)
        {
            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SetParameters(cmd, p);
            cmd.ExecuteNonQuery();
            p.user_activity_id = (int)cmd.Parameters[p.outputID].Value;
            if (this.TransactionObject == null) conn.Close();

            return true;
        }

        public List<UserActivity> GetUserActivity(ParamGetUserActivity p)
        {
            DataTable dataTable = new DataTable();

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SetParameters(cmd, p);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }
            if (this.TransactionObject == null) conn.Close();
            if (dataTable.Rows.Count == 0) return new List<UserActivity>();

            List<UserActivity> userActivities = dataTable.Rows.OfType<DataRow>().Select(dr =>

                new UserActivity()
                {
                    is_synchronized = dr.Field<ulong>("is_synchronized") == 1,
                    activity_datetime = dr.Field<DateTime>("activity_datetime"),
                    activity_user_category_id = dr.Field<int>("activity_user_category_id"),
                    user_activity_id = dr.Field<int>("user_activity_id"),
                    activity_note = dr.Field<string>("activity_note"),
                    activity_user_id = dr.Field<int>("activity_user_id")
                }
            ).ToList();

            return userActivities;
        }

        public List<CollectionFacility> GetClinicsForZip(ParamGetClinicsForZip p)
        {
            DataTable dataTable = new DataTable();

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SetParameters(cmd, p);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }
            if (this.TransactionObject == null) conn.Close();
            if (dataTable.Rows.Count == 0) return new List<CollectionFacility>();

            List<CollectionFacility> collectionFacilities = dataTable.Rows.OfType<DataRow>().Select(dr =>

                new CollectionFacility()
                {
                    is_synchronized = dr.Field<ulong>("is_synchronized") == 1,
                    vendor_name = dr.Field<string>("vendor_name"),
                    vendor_address_id = dr.Field<int>("vendor_address_id"),
                    vendor_id = dr.Field<int>("vendor_id"),
                    address_type_id = dr.Field<int>("address_type_id"),
                    created_on = dr.Field<DateTime>("created_on"),
                    last_modified_on = dr.Field<DateTime>("last_modified_on"),
                    vendor_zip = dr.Field<string>("vendor_zip"),
                    vendor_state = dr.Field<string>("vendor_state"),
                    vendor_phone = dr.Field<string>("vendor_phone"),
                    vendor_fax = dr.Field<string>("vendor_fax"),
                    created_by = dr.Field<string>("created_by"),
                    last_modified_by = dr.Field<string>("last_modified_by"),
                    vendor_address_1 = dr.Field<string>("vendor_address_1"),
                    vendor_address_2 = dr.Field<string>("vendor_address_2"),
                    vendor_city = dr.Field<string>("vendor_city"),
                    vendor_email = dr.Field<string>("vendor_email"),
                    d2c = dr.Field<float>("d2c")
                }
            ).ToList();

            return collectionFacilities;
        }

        public List<CollectionFacility> GetClinicsForDonor(ParamGetClinicsForDonor p)
        {
            DataTable dataTable = new DataTable();

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SetParameters(cmd, p);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }
            if (this.TransactionObject == null) conn.Close();
            if (dataTable.Rows.Count == 0) return new List<CollectionFacility>();

            List<CollectionFacility> collectionFacilities = dataTable.Rows.OfType<DataRow>().Select(dr =>

                new CollectionFacility()
                {
                    is_synchronized = dr.Field<ulong>("is_synchronized") == 1,
                    vendor_name = dr.Field<string>("vendor_name"),
                    vendor_address_id = dr.Field<int>("vendor_address_id"),
                    vendor_id = dr.Field<int>("vendor_id"),
                    address_type_id = dr.Field<int>("address_type_id"),
                    created_on = dr.Field<DateTime>("created_on"),
                    last_modified_on = dr.Field<DateTime>("last_modified_on"),
                    vendor_zip = dr.Field<string>("vendor_zip"),
                    vendor_state = dr.Field<string>("vendor_state"),
                    vendor_phone = dr.Field<string>("vendor_phone"),
                    vendor_fax = dr.Field<string>("vendor_fax"),
                    created_by = dr.Field<string>("created_by"),
                    last_modified_by = dr.Field<string>("last_modified_by"),
                    vendor_address_1 = dr.Field<string>("vendor_address_1"),
                    vendor_address_2 = dr.Field<string>("vendor_address_2"),
                    vendor_city = dr.Field<string>("vendor_city"),
                    vendor_email = dr.Field<string>("vendor_email"),
                    d2c = dr.Field<float>("d2c")
                }
            ).ToList();

            return collectionFacilities;
        }

        public List<CollectionFacility> GetClinicForDonorMinNumber(ParamGetClinicForDonorMinNumber p)
        {
            DataTable dataTable = new DataTable();

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SetParameters(cmd, p);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }
            if (this.TransactionObject == null) conn.Close();
            if (dataTable.Rows.Count == 0) return new List<CollectionFacility>();
            List<CollectionFacility> collectionFacilities = dataTable.Rows.OfType<DataRow>().Select(dr =>

                new CollectionFacility()
                {
                    is_synchronized = dr.Field<ulong>("is_synchronized") == 1,
                    vendor_name = dr.Field<string>("vendor_name"),
                    vendor_address_id = dr.Field<int>("vendor_address_id"),
                    vendor_id = dr.Field<int>("vendor_id"),
                    address_type_id = dr.Field<int>("address_type_id"),
                    created_on = dr.Field<DateTime>("created_on"),
                    last_modified_on = dr.Field<DateTime>("last_modified_on"),
                    vendor_zip = dr.Field<string>("vendor_zip"),
                    vendor_state = dr.Field<string>("vendor_state"),
                    vendor_phone = dr.Field<string>("vendor_phone"),
                    vendor_fax = dr.Field<string>("vendor_fax"),
                    created_by = dr.Field<string>("created_by"),
                    last_modified_by = dr.Field<string>("last_modified_by"),
                    vendor_address_1 = dr.Field<string>("vendor_address_1"),
                    vendor_address_2 = dr.Field<string>("vendor_address_2"),
                    vendor_city = dr.Field<string>("vendor_city"),
                    vendor_email = dr.Field<string>("vendor_email"),
                    d2c = dr.Field<float>("d2c")
                }
            ).ToList();

            return collectionFacilities;
        }

        public DataTable GetNotificationExceptionsByType(NotificationExceptions notificationExceptions)
        {
            DataTable dataTable = new DataTable();

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand("backend_get_notification_exceptions_dataview", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }
            if (this.TransactionObject == null) conn.Close();

            if (dataTable.Rows.Count == 0) return new DataTable();
            DataView dv = new DataView(dataTable);
            // filter the results
            switch (notificationExceptions)
            {
                case NotificationExceptions.NoClinics:
                    dv.RowFilter = "clinic_exception > 0";
                    break;
            }

            return dv.ToTable();
        }

        public List<Notification> GetNotificationExceptions()
        {
            DataTable dataTable = new DataTable();

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand("backend_get_notification_exceptions", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }

            if (this.TransactionObject == null) conn.Close();
            if (dataTable.Rows.Count == 0) return new List<Notification>();
            List<Notification> ret = dataTable.Rows.OfType<DataRow>().Select(dr =>

                populateNotificationFromDataRow(dr)
            ).ToList();

            return ret;
        }

        public NotificationInformation GetNotificationInfoForDonorInfoId(ParamGetNotificationInfoForDonorInfoId p)
        {
            DataTable dataTable = new DataTable();

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            SetParameters(cmd, p);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }

            if (this.TransactionObject == null) conn.Close();

            if (dataTable.Rows.Count == 0) return new NotificationInformation();
            NotificationInformation ret = dataTable.Rows.OfType<DataRow>().Select(dr =>
               // = dr.Field<>(""),
               populateNotificationInformationFromDataRow(dr)
            ).First();

            return ret;
        }

        public Notification GetDonorNotification(ParamGetDonorNotification p)
        {
            DataTable dataTable = new DataTable();

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            SetParameters(cmd, p);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }

            if (this.TransactionObject == null) conn.Close();
            if (dataTable.Rows.Count < 1) return new Notification();
            Notification ret = dataTable.Rows.OfType<DataRow>().Select(dr =>

                populateNotificationFromDataRow(dr)
            ).First();

            return ret;
        }

        #endregion Donor

        //#####################################

        #region Notification_Window

        public int SetClientNotificationSettings(ParamSetClientNotificationSettings p)
        {
            // Update all the donors scheduling window values for notifications
            UpdateNotificationsOnSettingsChange(p.clientNotificationDataSettings);

            int retval = 0;
            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SetParameters(cmd, p);
            cmd.ExecuteNonQuery();
            retval = (int)cmd.Parameters[p.outputID].Value;

            if (this.TransactionObject == null) conn.Close();

            return retval;
        }

        public bool RemoveClientNotificationSettings(int client_id, int client_department_id)
        {
            // "backend_remove_sms_client_data"
            bool retval = false;
            DataTable dataTable = new DataTable();

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand("backend_remove_notification_window_data", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter("@client_id", client_id));
            cmd.Parameters.Add(new MySqlParameter("@client_department_id", client_department_id));
            cmd.ExecuteNonQuery();
            retval = true;
            if (this.TransactionObject == null) conn.Close();

            return retval;
        }

        #endregion Notification_Window

        //#####################################

        #region Notification_Settings

        public bool RemoveClientSMSData(int client_id, int client_department_id)
        {
            // "backend_remove_sms_client_data"
            bool retval = false;
            DataTable dataTable = new DataTable();

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand("backend_remove_sms_client_data", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter("@client_id", client_id));
            cmd.Parameters.Add(new MySqlParameter("@client_department_id", client_department_id));
            cmd.ExecuteNonQuery();
            retval = true;
            if (this.TransactionObject == null) conn.Close();

            return retval;
        }

        /// <summary>
        /// TODO refactor to be autoresponse and match table name
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public ClientNotificationDataSettings GetClientNotificationDataSettingsByPhone(ParamGetClientNotificationDataSettingsByPhone p)
        {
            ClientNotificationDataSettings clientNotificationDataSettings = new ClientNotificationDataSettings();
            DataTable dataTable = new DataTable();

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand(p.sp(), conn);
            SetParameters(cmd, p);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }

            if (this.TransactionObject == null) conn.Close();
            if (dataTable != null && dataTable.Rows.OfType<DataRow>().Count() > 0)
            {
                var dr = dataTable.Rows.OfType<DataRow>().First();
                clientNotificationDataSettings = populateClientNotificationDataSettings(dr);
            }
            return clientNotificationDataSettings;
        }

        public ClientNotificationDataSettings GetClientNotificationDataSettings(int client_id, int client_department_id)
        {
            try
            {
                ClientNotificationDataSettings clientNotificationDataSettings = new ClientNotificationDataSettings();
                DataTable dataTable = new DataTable();

                if (this.conn.State == ConnectionState.Closed) conn.Open();
                MySqlCommand cmd = new MySqlCommand("backend_get_notification_window_data", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("@client_id", client_id));
                cmd.Parameters.Add(new MySqlParameter("@client_department_id", client_department_id));
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    dataTable.Load(reader);
                }

                if (this.TransactionObject == null) conn.Close();
                if (dataTable != null && dataTable.Rows.OfType<DataRow>().Count() > 0)
                {
                    var dr = dataTable.Rows.OfType<DataRow>().First();
                    clientNotificationDataSettings = populateClientNotificationDataSettings(dr);
                }
                return clientNotificationDataSettings;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Error(ex.InnerException.ToString());
                throw ex;
            }
        }

        public ClientNotificationDataSettings GetClientNotificationDataSettingsById(int backend_notification_window_data_id)
        {
            ClientNotificationDataSettings clientNotificationDataSettings = new ClientNotificationDataSettings();
            DataTable dataTable = new DataTable();

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand("backend_get_notification_window_data_by_id", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter("@backend_notification_window_data_id", backend_notification_window_data_id));
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }

            if (this.TransactionObject == null) conn.Close();
            if (dataTable != null && dataTable.Rows.OfType<DataRow>().Count() > 0)
            {
                var dr = dataTable.Rows.OfType<DataRow>().First();
                clientNotificationDataSettings = populateClientNotificationDataSettings(dr);
            }
            return clientNotificationDataSettings;
        }

        public List<ClientNotificationDataSettings> GetAllClientNotificationDataSettings()
        {
            List<ClientNotificationDataSettings> allNotificationDataSettings = new List<ClientNotificationDataSettings>();
            ClientNotificationDataSettings clientNotificationDataSettings = new ClientNotificationDataSettings();
            DataTable dataTable = new DataTable();

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand("backend_get_all_notification_window_data", conn);
            _logger.Debug($"Calling backend_get_all_notification_window_data");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }

            if (this.TransactionObject == null) conn.Close();

            if (dataTable != null && dataTable.Rows.OfType<DataRow>().Count() > 0)
            {
                foreach (DataRow dr in dataTable.Rows)
                {
                    allNotificationDataSettings.Add(populateClientNotificationDataSettings(dr));
                }
            }
            return allNotificationDataSettings;
        }

        #endregion Notification_Settings

        //#####################################

        #region PDF

        public List<NotificationInformation> GetNotificationInformationList()
        {
            DataTable dataTable = new DataTable();

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand("backend_get_notification_data_list", conn);
            //SetParameters(cmd, p);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }

            if (this.TransactionObject == null) conn.Close();

            List<NotificationInformation> ret = dataTable.Rows.OfType<DataRow>().Select(dr =>
             // = dr.Field<>(""),
             populateNotificationInformationFromDataRow(dr)
            ).ToList();

            return ret;
        }

        #endregion PDF

        //#####################################

        #region FormFox

        /// <summary>
        /// Set a formfox order
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public object SetFormFoxOrder(ParamSetformfoxorders p)
        {
            int retval = 0;
            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SetParameters(cmd, p);
            cmd.ExecuteNonQuery();
            retval = (int)cmd.Parameters[p.outputID].Value;

            if (this.TransactionObject == null) conn.Close();

            return retval;
        }

        public formfoxorders GetFormFoxOrder(ParamGetformfoxorders p)
        {
            DataTable dataTable = new DataTable();

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand(p.sp(), conn);
            SetParameters(cmd, p);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }
            if (this.TransactionObject == null) conn.Close();

            if (dataTable.Rows.Count == 0) return new formfoxorders();

            formfoxorders ret = dataTable.Rows.OfType<DataRow>().ToList().Select(dr =>
                    new formfoxorders()
                    {
                        backend_formfox_orders_id = dr.Field<int>("backend_formfox_orders_id"),
                        ReferenceTestID = dr.Field<string>("ReferenceTestID"),
                        donor_test_info_id = dr.Field<int>("donor_test_info_id"),
                        deadline = dr.Field<DateTime>("deadline"),
                        createdON = dr.Field<DateTime>("createdON"),
                        filename = dr.Field<string>("filename"),
                        updatedOn = dr.Field<DateTime>("updatedOn"),
                        status = dr.Field<string>("status"),
                        sfcode = dr.Field<string>("sfcode"),
                        sampletype = dr.Field<string>("sampletype"),
                        testid = dr.Field<string>("testid"),
                        SpecimenID = dr.Field<string>("SpecimenID"),
                        archived = dr.Field<sbyte>("archived") > 0
                    }
             ).First();

            return ret;
        }

        #endregion FormFox

        //#####################################

        #region Integrations

        public bool CheckSurpathKey(ParamCheckPartner_key p)
        {
            if (this.conn.State == ConnectionState.Closed) conn.Open();
            //MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SetParameters(cmd, p);
            cmd.ExecuteNonQuery();
            p.backend_integration_partners_id = (int)cmd.Parameters[p.outputID].Value;
            if (this.TransactionObject == null) conn.Close();

            return true;
        }

        public IntegrationPartner GetIntegrationPartnerByPartnerKey(ParamGetIntegrationPartnerByPartnerKey p)
        {
            DataTable dataTable = new DataTable();

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            SetParameters(cmd, p);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }

            if (this.TransactionObject == null) conn.Close();
            if (dataTable.Rows.Count == 0) return new IntegrationPartner();
            IntegrationPartner ret = dataTable.Rows.OfType<DataRow>().Select(dr =>

                populateIntegrationPartnerFromDataRow(dr)
            ).First();

            return ret;
        }

        public IntegrationPartner GetIntegrationPartnerByPartnerClientCode(ParamGetIntegrationPartnerByPartnerClientCode p)
        {
            DataTable dataTable = new DataTable();

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            SetParameters(cmd, p);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }

            if (this.TransactionObject == null) conn.Close();
            if (dataTable.Rows.Count == 0) return new IntegrationPartner();
            IntegrationPartner ret = dataTable.Rows.OfType<DataRow>().Select(dr =>

                populateIntegrationPartnerFromDataRow(dr)
            ).First();

            return ret;
        }

        public IntegrationPartnerClient GetIntegrationPartnerClientByPartnerClientId(ParamGetIntegrationPartnerClientByPartnerClientId p)
        {
            DataTable dataTable = new DataTable();

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            SetParameters(cmd, p);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }

            if (this.TransactionObject == null) conn.Close();
            if (dataTable.Rows.Count == 0) return new IntegrationPartnerClient();
            IntegrationPartnerClient ret = dataTable.Rows.OfType<DataRow>().Select(dr =>

               populateIntegrationPartnerClientFromDataRow(dr)

            ).First();

            return ret;
        }

        public List<IntegrationPartnerClient> GetIntegrationPartnerClientsByPartnerKey(ParamGetIntegrationPartnerClientsByPartnerKey p)
        {
            DataTable dataTable = new DataTable();

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            SetParameters(cmd, p);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }

            if (this.TransactionObject == null) conn.Close();
            if (dataTable.Rows.Count == 0) return new List<IntegrationPartnerClient>();
            List<IntegrationPartnerClient> ret = dataTable.Rows.OfType<DataRow>().Select(dr =>

                populateIntegrationPartnerClientFromDataRow(dr)

            ).ToList();

            return ret;
        }

        public List<IntegrationPartnerClient> GetIntegrationPartnerClientByClientAndDepartmentId(ParamGetIntegrationPartnerClientByClientAndDepartmentId p)
        {
            DataTable dataTable = new DataTable();

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            SetParameters(cmd, p);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }

            if (this.TransactionObject == null) conn.Close();
            if (dataTable.Rows.Count == 0) return new List<IntegrationPartnerClient>();
            List<IntegrationPartnerClient> ret = dataTable.Rows.OfType<DataRow>().Select(dr =>

                populateIntegrationPartnerClientFromDataRow(dr)

            ).ToList();

            return ret;
        }

        public List<IntegrationPartnerRelease> GetIntegrationPartnerRelease(ParamGetIntegrationPartnerRelease p)
        {
            DataTable dataTable = new DataTable();

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            SetParameters(cmd, p);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }

            if (this.TransactionObject == null) conn.Close();
            if (dataTable.Rows.Count == 0) return new List<IntegrationPartnerRelease>();
            List<IntegrationPartnerRelease> ret = dataTable.Rows.OfType<DataRow>().Select(dr =>
                new IntegrationPartnerRelease()
                {
                    backend_integration_partner_release_id = dr.Field<int>("backend_integration_partner_release_id"),
                    backend_integration_partner_release_GUID = dr.Field<string>("backend_integration_partner_release_GUID"),
                    background_check = dr.Field<int>("background_check") > 0,
                    created_on = dr.Field<DateTime>("created_on"),
                    last_modified_on = dr.Field<DateTime>("last_modified_on"),
                    sent_on = dr.IsNull("sent_on") ? DateTime.MinValue : dr.Field<DateTime>("sent_on"),
                    donor_document_id = dr.Field<int>("donor_document_id"),
                    donor_test_test_category_id = dr.Field<int>("donor_test_test_category_id"),
                    donor_test_info_id = dr.Field<int>("donor_test_info_id"),
                    released = dr.Field<sbyte>("released") > 0,
                    sent = dr.Field<sbyte>("sent") > 0,
                    last_modified_by = dr.Field<string>("last_modified_by"),
                    released_by = dr.Field<string>("released_by"),
                    report_info_id = dr.Field<int>("report_info_id"),
                    donor_id = dr.Field<int>("donor_id"),
                    client_department_id = dr.Field<int>("client_department_id"),
                    client_id = dr.Field<int>("client_id"),
                }
            ).ToList();

            return ret;
        }

        public bool SetIntegrationPartnerRelease(ParamSetIntegrationPartnerRelease p)
        {
            if (this.conn.State == ConnectionState.Closed) conn.Open();
            //MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SetParameters(cmd, p);
            cmd.ExecuteNonQuery();
            if (this.TransactionObject == null) conn.Close();
            return true;
        }

        public IntegrationDonors GetIntegrationPartnerDonorsAndDocuments(ParamGetIntegrationPartnerDonorsAndDocuments p)
        {
            _logger.Debug("GetIntegrationPartnerDonorsAndDocuments");
            DataTable dataTable = new DataTable();

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            SetParameters(cmd, p);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }

            if (this.TransactionObject == null) conn.Close();
            IntegrationDonors ret = new IntegrationDonors();
            if (dataTable.Rows.Count == 0) return ret;
            // we're going to get non-unique rows
            // so for this object, we have to get all the distinct donors
            // then populate the list for the related documents

            // get all the rows
            var _dr = dataTable.Rows.OfType<DataRow>().Select(dr =>

                populateIntegrationDonorsDataRow(dr)

               ).ToList();

            ret.donors = _dr.Select(dr =>
                    new IntegrationDonor()
                    {
                        id = dr.donorid,
                        partner_donor_id = dr.donoraltid
                    }
                ).Distinct().ToList();

            foreach (IntegrationDonor _d in ret.donors)
            {
                _d.tests = _dr.Where(dr => dr.donoraltid == _d.partner_donor_id).ToList().Select(dr =>
                    new IntegrationDonorTests()
                    {
                        id = dr.donortestid,
                        created_on = dr.testcreated_on,
                        test_requested_date = dr.test_requested_date,
                        category = dr.testcategory,
                        categoryid = dr.testcategoryid,
                        bccode = dr.donorClearstarProfId,
                        status = dr.teststatus,
                        statusid = dr.teststatusid
                    }).ToList();

                //foreach(IntegrationDonorTests _t in _d.tests)
                _d.tests.ForEach(_t =>
                {
                    if (_t.statusid == (int)DonorRegistrationStatus.Completed)
                    {
                        if (_dr.Exists(dr => dr.donoraltid == _d.partner_donor_id && dr.testcategoryid == _t.categoryid))
                        {
                            //var _thisRow = _dr.Where(dr => dr.donoraltid == _d.altid && dr.testcategoryid==_t.categoryid && dr.teststatusid ==_t.statusid && dr.)
                            var _thisRow = _dr.Where(dr => dr.donoraltid == _d.partner_donor_id && dr.testcategoryid == _t.categoryid).First();
                            _t.document = new IntegrationDonorDocument();

                            _t.document.received_on = _thisRow.received_on;
                            _t.document.report_type = _thisRow.report_type.ToString();
                            _t.document.report_typeid = _thisRow.report_typeid;
                            _t.document.created_on = _thisRow.doccreated_on;
                            _t.document.id = _thisRow.docid;
                            _t.document.test_requested_date = _thisRow.doctest_requested_date;

                            if (_t.categoryid == (int)TestCategories.UA)
                            {
                                _t.document.filename = _thisRow.filename;
                                if (p.WithBase64 == true) _t.document.base64 = _thisRow.docbase64;
                            }

                            if (_t.categoryid == (int)TestCategories.BC)
                            {
                                BackendBCReport backendBCReport = new BackendBCReport();
                                var res = backendBCReport.ViewProfileDocument(_thisRow.deptclearstarcode, _thisRow.donorClearstarProfId);
                                var file = res.Item1;
                                var bytes = res.Item2;

                                _t.document.filename = file;
                                if (p.WithBase64 == true) _t.document.base64 = Convert.ToBase64String(bytes);
                            }
                        }
                    }
                });
            }

            var _data = dataTable.Rows.OfType<DataRow>().Select(dr =>
                new IntegrationDonorTests()
                {
                }
            );

            //ret.donors = dataTable.Rows.OfType<DataRow>().Select(dr =>

            //    new IntegrationPartnerClient()
            //    {
            //        backend_integration_partner_client_map_id = dr.Field<int>("backend_integration_partner_client_map_id"),
            //        backend_integration_partner_id = dr.Field<int>("backend_integration_partner_id"),
            //        partner_client_code = dr.Field<string>("partner_client_code"),
            //        client_id = dr.Field<int>("client_id"),
            //        client_department_id = dr.Field<int>("client_department_id"),
            //        partner_client_id = dr.Field<string>("partner_client_id"),
            //        created_on = dr.Field<DateTime>("created_on"),
            //        last_modified_by = dr.Field<string>("last_modified_by"),
            //        last_modified_on = dr.Field<DateTime>("last_modified_on"),
            //        active = dr.Field<sbyte>("active") > 0,
            //    }

            //).ToList();

            return ret;
        }

        public ApiIntegrationMatchResults GetDonorMatch(ParamGetDonorMatch p)
        {
            _logger.Debug("GetDonorMatch");
            DataTable dataTable = new DataTable();

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            SetParameters(cmd, p);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }

            if (this.TransactionObject == null) conn.Close();
            ApiIntegrationMatchResults ret = new ApiIntegrationMatchResults();
            if (dataTable.Rows.Count == 0)
            {
                ret.result = false;
                ret.message = "No matches found";
                return ret;
            }

            ret.possible_matchs = dataTable.Rows.OfType<DataRow>().Select(dr =>

                new ApiIntegrationMatchResult()
                {
                    donor_id = dr.Field<int>("donor_id"),
                    donor_first_name = dr.Field<string>("donor_first_name").ToString(),
                    donor_last_name = dr.Field<string>("donor_last_name").ToString(),
                    partner_donor_id = dr.Field<string>("pid").ToString(),
                    partner_client_code = dr.Field<string>("partner_client_code").ToString(),
                    partner_client_id = dr.Field<string>("partner_client_id").ToString(),
                    partner_client_name = dr.Field<string>("client_name").ToString(),
                    donor_email = dr.Field<string>("donor_email").ToString(),
                }

            ).ToList();
            var _idxs = new List<int>();
            // see if any match the filter *exactly*
            ret.possible_matchs.ForEach(am =>

                {
                    if (
                       am.donor_email.Equals(p.apiIntegrationMatch.donor_email, StringComparison.InvariantCultureIgnoreCase)
                    && am.partner_donor_id.Equals(p.apiIntegrationMatch.partner_donor_id, StringComparison.InvariantCultureIgnoreCase)
                    && am.donor_first_name.Equals(p.apiIntegrationMatch.donor_first_name, StringComparison.InvariantCultureIgnoreCase)
                    && am.donor_last_name.Equals(p.apiIntegrationMatch.donor_last_name, StringComparison.InvariantCultureIgnoreCase)
                    && am.partner_client_name.Equals(p.apiIntegrationMatch.partner_client_name, StringComparison.InvariantCultureIgnoreCase)
                    )
                    {
                        ret.exact_matchs.Add(am);
                        _idxs.Add(ret.possible_matchs.IndexOf(am));
                    }
                }

             );
            foreach (int _idx in _idxs)
            {
                ret.possible_matchs.RemoveAt(_idx);
            }
            if (p.apiIntegrationMatch.exact_only == true)
            {
                ret.possible_matchs = new List<ApiIntegrationMatchResult>();
            }

            return ret;
        }

        public ApiIntegrationMatchResults VerifyIntegrationPid(ParamGetDonorMatch p)
        {
            _logger.Debug("GetDonorMatch");
            DataTable dataTable = new DataTable();

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            SetParameters(cmd, p);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }

            if (this.TransactionObject == null) conn.Close();
            ApiIntegrationMatchResults ret = new ApiIntegrationMatchResults();
            if (dataTable.Rows.Count == 0)
            {
                ret.result = false;
                ret.message = "No matches found";
                return ret;
            }

            ret.possible_matchs = dataTable.Rows.OfType<DataRow>().Select(dr =>

                new ApiIntegrationMatchResult()
                {
                    donor_id = dr.Field<int>("donor_id"),
                    donor_first_name = dr.Field<string>("donor_first_name").ToString(),
                    donor_last_name = dr.Field<string>("donor_last_name").ToString(),
                    partner_donor_id = dr.Field<string>("pid").ToString(),
                    partner_client_code = dr.Field<string>("partner_client_code").ToString(),
                    partner_client_id = dr.Field<string>("partner_client_id").ToString(),
                    partner_client_name = dr.Field<string>("client_name").ToString(),
                    donor_email = dr.Field<string>("donor_email").ToString(),
                }

            ).ToList();
            var _idxs = new List<int>();
            // see if any match the filter *exactly*
            ret.possible_matchs.ForEach(am =>

            {
                if (
                    am.partner_client_code.Equals(p.apiIntegrationMatch.partner_client_code, StringComparison.InvariantCultureIgnoreCase)
                    && am.partner_donor_id.Equals(p.apiIntegrationMatch.partner_donor_id, StringComparison.InvariantCultureIgnoreCase)

                )
                {
                    ret.exact_matchs.Add(am);
                    _idxs.Add(ret.possible_matchs.IndexOf(am));
                }
            }

             );
            foreach (int _idx in _idxs)
            {
                ret.possible_matchs.RemoveAt(_idx);
            }
            if (p.apiIntegrationMatch.exact_only == true)
            {
                ret.possible_matchs = new List<ApiIntegrationMatchResult>();
            }

            return ret;
        }

        public bool SetIntegrationPartnerClient(ParamSetIntegrationPartnerClient p)
        {
            if (this.conn.State == ConnectionState.Closed) conn.Open();
            //MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SetParameters(cmd, p);
            cmd.ExecuteNonQuery();
            p.backend_integration_partner_client_map_id = (int)cmd.Parameters[p.outputID].Value;
            if (this.TransactionObject == null) conn.Close();

            return true;
        }

        public List<IntegrationPartner> GetIntegrationPartners(ParamGetIntegrationPartners p)
        {
            DataTable dataTable = new DataTable();

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            SetParameters(cmd, p);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }

            if (this.TransactionObject == null) conn.Close();
            if (dataTable.Rows.Count == 0) return new List<IntegrationPartner>();
            List<IntegrationPartner> ret = dataTable.Rows.OfType<DataRow>().Select(dr =>

                populateIntegrationPartnerFromDataRow(dr)
            //new IntegrationPartner()
            //{
            //    backend_integration_partner_id = dr.Field<int>("backend_integration_partner_id"),
            //    partner_name = dr.Field<string>("partner_name"),
            //    partner_key = dr.Field<string>("partner_key"),
            //    partner_crypto = dr.Field<string>("partner_crypto"),
            //    backend_integration_partners_pidtype = dr.Field<int>("backend_integration_partners_pidtype"),
            //    active = dr.Field<sbyte>("active")>0,
            //    created_on = dr.Field<DateTime>("created_on"),
            //    last_modified_by = dr.Field<string>("last_modified_by"),
            //    last_modified_on = dr.Field<DateTime>("last_modified_on"),
            //    login_url = dr.Field<string>("login_url"),
            //    partner_push =
            //    html_instructions = LongBlobToASCIIString(dr, "html_instructions") // dr.Field<byte>("html_instructions").ToString()
            //}

            ).ToList();
            conn.Close();
            return ret;
        }

        public string LongBlobToASCIIString(DataRow dr, String field)
        {
            string retval = String.Empty;
            byte[] _bytes = dr.Field<byte[]>(field);
            retval = Encoding.ASCII.GetString(_bytes, 0, _bytes.Length);

            return retval;
        }

        public int SetIntegrationPartners(ParamSetIntegrationPartners p)
        {
            if (this.conn.State == ConnectionState.Closed) conn.Open();
            //MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            MySqlCommand cmd = new MySqlCommand(p.sp(), conn, this.TransactionObject);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SetParameters(cmd, p);
            cmd.ExecuteNonQuery();
            p.IntegrationPartner.backend_integration_partner_id = (int)cmd.Parameters[p.outputID].Value;
            if (this.TransactionObject == null) conn.Close();

            return p.IntegrationPartner.backend_integration_partner_id;
        }

        #endregion Integrations

        //#####################################

        public object GetGenericObject(ParamGeneric p)
        {
            DataTable dataTable = new DataTable();

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            MySqlCommand cmd = new MySqlCommand(p.sp(), conn);
            SetParameters(cmd, p);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }
            if (this.TransactionObject == null) conn.Close();

            if (!string.IsNullOrEmpty(p.outputID))
            {
                if (cmd.Parameters[p.outputID].Value == DBNull.Value)
                {
                    return 0;
                }
                else
                {
                    return cmd.Parameters[p.outputID].Value;
                }
            }
            else
            {
                return dataTable;
            }
        }

        public List<ZipCodeDataRow> GetZipCodes()
        {
            DataTable dataTable = new DataTable();
            if (this.conn.State == ConnectionState.Closed) conn.Open();
            string _sql = "select * from zip_codes;";

            MySqlCommand cmd = new MySqlCommand(_sql, conn);

            cmd.CommandType = System.Data.CommandType.Text;

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }

            if (this.TransactionObject == null) conn.Close();
            if (dataTable.Rows.Count == 0) return new List<ZipCodeDataRow>();

            List<ZipCodeDataRow> ret = dataTable.Rows.OfType<DataRow>().Select(dr =>
                    new ZipCodeDataRow()
                    {
                        zip_id = dr.Field<int>("zip_id"),
                        tz_std = dr.Field<int>("tz_std"),
                        tz_dst = dr.Field<int>("tz_dst"),
                        zip = dr.Field<string>("zip"),
                        latitude = dr.Field<string>("latitude"),
                        longitude = dr.Field<string>("longitude"),
                        city = dr.Field<string>("city"),
                        state = dr.Field<string>("state"),
                        county = dr.Field<string>("county"),
                        type = dr.Field<string>("type"),
                    }
             ).ToList();

            return ret;
        }

        public bool InsertZipCodes(List<ZipCodeDataRow> _zips)
        {
            bool retval = false;
            try
            {
                if (this.conn.State == ConnectionState.Closed) conn.Open();
                MySqlTransaction transaction = conn.BeginTransaction();

                //Obtain a dataset, obviously a "select *" is not the best way...
                var mySqlDataAdapterSelect = new MySqlDataAdapter("select * from zip_codes", conn);

                var ds = new DataSet();

                mySqlDataAdapterSelect.Fill(ds, "zip_codes");

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter();
                string _insertCommand =
@"INSERT INTO surpathlive.zip_codes
(zip, latitude, longitude, city, state, county, type, tz_std, tz_dst)
VALUES
(@zip, @latitude, @longitude, @city, @state, @county, @type, @tz_std, @tz_dst);";

                mySqlDataAdapter.InsertCommand = new MySqlCommand(_insertCommand, conn);

                mySqlDataAdapter.InsertCommand.Parameters.Add("@zip", MySqlDbType.VarChar, 255, "zip");
                mySqlDataAdapter.InsertCommand.Parameters.Add("@latitude", MySqlDbType.VarChar, 255, "latitude");
                mySqlDataAdapter.InsertCommand.Parameters.Add("@longitude", MySqlDbType.VarChar, 255, "longitude");
                mySqlDataAdapter.InsertCommand.Parameters.Add("@city", MySqlDbType.VarChar, 255, "city");
                mySqlDataAdapter.InsertCommand.Parameters.Add("@state", MySqlDbType.VarChar, 255, "state");
                mySqlDataAdapter.InsertCommand.Parameters.Add("@county", MySqlDbType.VarChar, 255, "county");
                mySqlDataAdapter.InsertCommand.Parameters.Add("@type", MySqlDbType.VarChar, 255, "type");
                mySqlDataAdapter.InsertCommand.Parameters.Add("@tz_std", MySqlDbType.Int32, 11, "tz_std");
                mySqlDataAdapter.InsertCommand.Parameters.Add("@tz_dst", MySqlDbType.Int32, 11, "tz_dst");
                mySqlDataAdapter.InsertCommand.UpdatedRowSource = UpdateRowSource.None;

                foreach (ZipCodeDataRow z in _zips)
                {
                    DataRow row = ds.Tables["zip_codes"].NewRow();
                    row["zip"] = z.zip;
                    row["latitude"] = z.latitude;
                    row["longitude"] = z.longitude;
                    row["city"] = z.city;
                    row["state"] = z.state;
                    row["county"] = z.county;
                    row["type"] = z.type;
                    row["tz_std"] = z.tz_std;
                    row["tz_dst"] = z.tz_dst;
                    ds.Tables["zip_codes"].Rows.Add(row);
                }

                mySqlDataAdapter.UpdateBatchSize = _zips.Count();
                mySqlDataAdapter.Update(ds, "zip_codes");

                transaction.Commit();

                retval = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Error(ex.InnerException.ToString());
                throw ex;
            }
            return retval;
        }

        public List<PaymentOverride> GetOverRideList()
        {
            DataTable dataTable = new DataTable();
            if (this.conn.State == ConnectionState.Closed) conn.Open();
            string _sql = "select * from overridedonorpay where used = 0;";
            ParamHelper p = new ParamHelper();

            MySqlCommand cmd = new MySqlCommand(_sql, conn);
            foreach (MySqlParameter parameter in p.ParmList)
            {
                cmd.Parameters.Add(parameter);
            }

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                dataTable.Load(reader);
            }

            if (this.TransactionObject == null) conn.Close();
            if (dataTable.Rows.Count == 0) return new List<PaymentOverride>();

            List<PaymentOverride> ret = dataTable.Rows.OfType<DataRow>().Select(dr =>
                    new PaymentOverride()
                    {
                        TransID = dr.Field<string>("TransID"),
                        SubmitDate = dr.Field<DateTime>("SubmitDate"),
                        LastName = dr.Field<string>("LastName"),
                        FirstName = dr.Field<string>("FirstName"),
                        Phone = dr.Field<string>("Phone"),
                        Email = dr.Field<string>("Email"),
                        Card = dr.Field<string>("Card"),
                        PaymentMethod = dr.Field<string>("PaymentMethod"),
                        PaymentAmount = dr.Field<string>("PaymentAmount"),
                        SettlementDate = dr.Field<DateTime>("SettlementDate"),
                        SettlementAmount = dr.Field<string>("SettlementAmount"),
                        Used = dr.Field<int>("Used"),
                        // DateUsed = dr.Field<DateTime>("DateUsed"),
                        InvoiceNumber = dr.Field<string>("InvoiceNumber"),
                        TransStatus = dr.Field<string>("TransStatus"),
                    }
             ).ToList();

            return ret;
        }

        public bool BurnOverrideDonorPay(string donor_email)
        {
            try
            {
                DataTable dataTable = new DataTable();
                if (this.conn.State == ConnectionState.Closed) conn.Open();
                string _sql = "update overridedonorpay set Used = 1, DateUsed = CURRENT_TIMESTAMP() where Email = @donor_email;";
                ParamHelper p = new ParamHelper();
                p.Param = new MySqlParameter("@donor_email", donor_email);

                MySqlCommand cmd = new MySqlCommand(_sql, conn);
                foreach (MySqlParameter parameter in p.ParmList)
                {
                    cmd.Parameters.Add(parameter);
                }
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //#####################################

        #region Private_Functions

        private int varchartoint(string val)
        {
            int retval = 0;
            int.TryParse(val, out retval);
            return retval;
        }

        private NotificationInformation populateNotificationInformationFromDataRow(DataRow dr)
        {
            NotificationInformation n = new NotificationInformation()
            {
                client_id = dr.Field<int>("client_id"),
                client_department_id = dr.Field<int>("client_department_id"),
                donor_id = dr.Field<int>("donor_id"),
                test_requested_date = dr.Field<DateTime>("test_requested_date"),
                test_panel_id = dr.Field<int>("test_panel_id"),
                test_category_id = dr.Field<int>("test_category_id"),
                department_name = dr.Field<string>("department_name"),
                client_name = dr.Field<string>("client_name"),
                lab_code = dr.Field<string>("lab_code"),
                test_panel_name = dr.Field<string>("test_panel_name"),
                donor_email = dr.Field<string>("donor_email"),
                donor_phone_1 = dr.Field<string>("donor_phone_1"),
                donor_phone_2 = dr.Field<string>("donor_phone_2"),
                donor_zip = dr.Field<string>("donor_zip"),
                test_category_name = dr.Field<string>("test_category_name"),
                donor_test_info_id = dr.Field<int>("donor_test_info_id"),
                in_window = dr.Field<int>("in_window") > 0,
                force_manual = dr.Field<int>("force_manual") > 0,
                donor_first_name = dr.Field<string>("donor_first_name"),
                donor_last_name = dr.Field<string>("donor_last_name"),
                donor_full_name = dr.Field<string>("donor_full_name"),
                backend_notifications_id = (int)dr.Field<long>("backend_notifications_id"),
                backend_notification_window_data_id = (int)dr.Field<long>("backend_notification_window_data_id"),
                deadline_alert_in_days = (int)dr.Field<long>("deadline_alert_in_days"),
                delay_in_hours = (int)dr.Field<long>("delay_in_hours"),
            };
            if (!dr.IsNull("reason_for_test_id")) n.reason_for_test_id = dr.Field<int>("reason_for_test_id");
            //reason_for_test_id = (dr.IsNull("reason_for_test_id")) ? -1 : dr.Field<int>("reason_for_test_id")
            return n;
        }

        private Notification populateNotificationFromDataRow(DataRow dr)
        {
            Notification n = new Notification()
            {
                backend_notifications_id = dr.Field<int>("backend_notifications_id"),
                notified_by_email = dr.Field<sbyte>("notified_by_email") > 0,
                notified_by_sms = dr.Field<sbyte>("notified_by_sms") > 0,
                notification_email_exception = dr.Field<int>("notification_email_exception"),
                notification_sms_exception = dr.Field<int>("notification_sms_exception"),
                donor_test_info_id = dr.Field<int>("donor_test_info_id"),
                is_archived = dr.Field<sbyte>("is_archived") > 0,
                notify_now = dr.Field<sbyte>("notify_now") > 0,
                notify_manual = dr.Field<sbyte>("notify_manual") > 0,
                force_db = dr.Field<sbyte>("force_db") > 0,
                notify_next_window = dr.Field<sbyte>("notify_next_window") > 0,
                clinic_exception = dr.Field<int>("clinic_exception"),
                clinic_radius = dr.Field<int>("clinic_radius"),
                client_department_id = dr.Field<int>("client_department_id"),
                client_id = dr.Field<int>("client_id"),
                created_by = dr.Field<string>("created_by"),
                notification_sent_to_email = dr.Field<string>("notification_sent_to_email"),
                notification_sent_to_phone = dr.Field<string>("notification_sent_to_phone"),
                last_modified_by = dr.Field<string>("last_modified_by"),
                in_window = dr.Field<int>("in_window") > 0,
                notify_again = dr.Field<int>("notify_again") > 0,
            };

            if (!dr.IsNull("created_on")) n.created_on = dr.Field<DateTime>("created_on");
            if (!dr.IsNull("last_modified_on")) n.last_modified_on = dr.Field<DateTime>("last_modified_on");
            if (!dr.IsNull("notified_by_email_timestamp")) n.notified_by_email_timestamp = dr.Field<DateTime>("notified_by_email_timestamp");
            if (!dr.IsNull("notified_by_sms_timestamp")) n.notified_by_sms_timestamp = dr.Field<DateTime>("notified_by_sms_timestamp");
            if (!dr.IsNull("notify_email_exception_timestamp")) n.notify_email_exception_timestamp = dr.Field<DateTime>("notify_email_exception_timestamp");
            if (!dr.IsNull("notify_sms_exception_timestamp")) n.notify_sms_exception_timestamp = dr.Field<DateTime>("notify_sms_exception_timestamp");
            if (!dr.IsNull("clinic_exception_timestamp")) n.clinic_exception_timestamp = dr.Field<DateTime>("clinic_exception_timestamp");
            if (!dr.IsNull("notify_after_timestamp")) n.notify_after_timestamp = dr.Field<DateTime>("notify_after_timestamp");
            if (!dr.IsNull("notify_before_timestamp")) n.notify_before_timestamp = dr.Field<DateTime>("notify_before_timestamp");
            return n;
        }

        private ClientNotificationDataSettings populateClientNotificationDataSettings(DataRow dr)
        {
            ClientNotificationDataSettings o = new ClientNotificationDataSettings();

            try
            {
                List<ClientNotificationDataSettingsDay> listDaySettings = new List<ClientNotificationDataSettingsDay>();
                foreach (int dayInt in System.Enum.GetValues(typeof(DayOfWeekEnum)))
                {
                    string dayname = System.Enum.GetName(typeof(DayOfWeekEnum), dayInt).ToLower();
                    ClientNotificationDataSettingsDay thisDay = new ClientNotificationDataSettingsDay();
                    listDaySettings.Add(new ClientNotificationDataSettingsDay()
                    {
                        DayOfWeek = dayInt,
                        Enabled = Convert.ToBoolean(dr.Field<sbyte>(dayname)),
                        send_time_start_seconds_from_midnight = dr.Field<int>($"{dayname}_send_time_start_seconds_from_midnight"),
                        send_time_stop_seconds_from_midnight = dr.Field<int>($"{dayname}_send_time_stop_seconds_from_midnight")
                    });
                }
                o = new ClientNotificationDataSettings()
                {
                    backend_notification_window_data_id = dr.Field<int>("backend_notification_window_data_id"),
                    client_id = dr.Field<int>("client_id"),
                    client_department_id = dr.Field<int>("client_department_id"),
                    created_on = dr.Field<DateTime>("created_on"),
                    created_by = dr.Field<string>("created_by"),
                    DaySettings = listDaySettings,
                    delay_in_hours = dr.Field<int>("delay_in_hours"),
                    send_asap = Convert.ToBoolean(dr.Field<sbyte>("send_asap")),
                    enable_sms = Convert.ToBoolean(dr.Field<sbyte>("enable_sms")),
                    use_formfox = Convert.ToBoolean(dr.Field<sbyte>("use_formfox")),
                    force_manual = Convert.ToBoolean(dr.Field<sbyte>("force_manual")),
                    onsite_testing = Convert.ToBoolean(dr.Field<sbyte>("onsite_testing")),
                    deadline_alert_in_days = dr.Field<int>("deadline_alert_in_days"),
                    override_day_schedule = Convert.ToBoolean(dr.Field<sbyte>("override_day_schedule")),
                    last_modified_on = dr.Field<DateTime>("last_modified_on"),
                    last_modified_by = dr.Field<string>("last_modified_by"),
                    client_name = dr.Field<string>("client_name"),
                    department_name = dr.Field<string>("department_name"),
                    client_sms_token = dr.Field<string>("client_sms_token"),
                    client_sms_apikey = dr.Field<string>("client_sms_apikey"),
                    client_autoresponse = dr.Field<string>("client_autoresponse"),
                    client_sms_from_number = dr.Field<string>("client_sms_from_number"),
                    show_web_notify_button = Convert.ToBoolean(dr.Field<sbyte>("show_web_notify_button")),
                    max_sendins = dr.Field<int>("max_sendins"),
                    pdf_render_settings_filename = dr.Field<string>("pdf_render_settings_filename")
                };

                if (!dr.IsNull("notification_start_date")) o.notification_start_date = dr.Field<DateTime>("notification_start_date");
                if (!dr.IsNull("notification_stop_date")) o.notification_stop_date = dr.Field<DateTime>("notification_stop_date");
                if (!dr.IsNull("notification_sweep_date")) o.notification_sweep_date = dr.Field<DateTime>("notification_sweep_date");
                return o;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private SMSActivity populateSMSActivityFromDataRow(DataRow dr)
        {
            SMSActivity o = new SMSActivity()
            {
                last_modified_by = dr.Field<string>("last_modified_by"),
                created_by = dr.Field<string>("created_by"),
                reply_text = dr.Field<string>("reply_text"),
                sent_text = dr.Field<string>("sent_text"),
                auto_reply_text = dr.Field<string>("auto_reply_text"),

                user_id = dr.Field<int>("user_id"),
                reply_read = dr.Field<int>("reply_read") > 0,
                donor_test_info_id = dr.Field<int>("donor_test_info_id"),
                backend_sms_activity_id = dr.Field<int>("backend_sms_activity_id"),
            };

            if (!dr.IsNull("last_modified_on")) o.last_modified_on = dr.Field<DateTime>("last_modified_on");
            if (!dr.IsNull("created_on")) o.created_on = dr.Field<DateTime>("created_on");

            if (!dr.IsNull("reply_read_timestamp")) o.reply_read_timestamp = dr.Field<DateTime>("reply_read_timestamp");
            if (!dr.IsNull("dt_reply_received")) o.dt_reply_received = dr.Field<DateTime>("dt_reply_received");
            if (!dr.IsNull("dt_sms_sent")) o.dt_sms_sent = dr.Field<DateTime>("dt_sms_sent");

            return o;
        }

        private IntegrationPartnerClient populateIntegrationPartnerClientFromDataRow(DataRow dr)
        {
            IntegrationPartnerClient o = new IntegrationPartnerClient()
            {
                backend_integration_partner_client_map_id = dr.Field<int>("backend_integration_partner_client_map_id"),
                backend_integration_partner_client_map_GUID = dr.Field<Guid>("backend_integration_partner_client_map_GUID"),
                backend_integration_partner_id = dr.Field<int>("backend_integration_partner_id"),
                partner_client_code = dr.Field<string>("partner_client_code"),
                client_id = dr.Field<int>("client_id"),
                client_department_id = dr.Field<int>("client_department_id"),
                partner_client_id = dr.Field<string>("partner_client_id"),
                partner_push_folder = dr.Field<string>("partner_push_folder"),
                created_on = dr.Field<DateTime>("created_on"),
                last_modified_by = dr.Field<string>("last_modified_by"),
                last_modified_on = dr.Field<DateTime>("last_modified_on"),
                active = dr.Field<sbyte>("active") > 0,
                require_remote_login = dr.Field<sbyte>("require_remote_login") > 0,
                require_login = dr.Field<sbyte>("require_login") > 0,
            };

            if (!dr.IsNull("last_modified_on")) o.last_modified_on = dr.Field<DateTime>("last_modified_on");
            if (!dr.IsNull("created_on")) o.created_on = dr.Field<DateTime>("created_on");

            return o;
        }

        private IntegrationDonorsDataRow populateIntegrationDonorsDataRow(DataRow dr)
        {
            //IntegrationDonorsDataRow o = new IntegrationDonorsDataRow()
            //{
            //    donorid = dr.Field<int>("donor_id").ToString(),
            //    donoraltid = dr.Field<string>("pid"),
            //    donorClearstarProfId = dr.Field<string>("donorClearstarProfId") != null ? dr.Field<string>("donorClearstarProfId") : "",
            //    donortestid = dr.Field<int>("donor_test_info_id").ToString(),
            //    deptclearstarcode = dr.Field<string>("clearstarcode") != null ? dr.Field<string>("clearstarcode") : "",
            //    testcategory = ((TestCategories)dr.Field<int>("test_category_id")).ToString(),
            //    testcategoryid = dr.Field<int>("test_category_id"),
            //    testcreated_on = dr.Field<DateTime>("dti_created_on"),
            //    teststatus = ((DonorRegistrationStatus)dr.Field<int>("test_status")).ToString(),
            //    teststatusid = dr.Field<int>("test_status"),
            //    test_requested_date = dr.Field<DateTime>("test_requested_date"),
            //    doccreated_on = dr.Field<DateTime>("doc_created_on"),
            //    //docid = dr.Field<int>("report_info_id") != null ? dr.Field<int>("report_info_id") : 0,
            //    docid = dr.Field<int>("report_info_id"),
            //    docbase64 = BackendStatics.Base64EncodeBytes(dr.Field<byte[]>("lab_report")),
            //    doctest_requested_date = dr.Field<DateTime>("test_requested_date"),
            //    filename = dr.Field<string>("lab_report_source_filename"),
            //    received_on = dr.Field<DateTime>("doc_received_on"),
            //    report_type = dr.Field<int>("report_type"),
            //    final_report_id = dr.Field<int>("final_report_id"),
            //    document_content = BackendStatics.Base64EncodeBytes(dr.Field<byte[]>("document_content")),
            //    document_title = dr.Field<string>("document_title"),
            //    document_upload_time = dr.Field<DateTime>("document_upload_time"),
            //    donor_document_id = dr.IsNull("donor_document_id") ? 0 : dr.Field<int>("donor_document_id"),
            //    donor_document_typeId = dr.IsNull("donor_document_typeId") ? 0 : dr.Field<int>("donor_document_typeId"),
            //};
            IntegrationDonorsDataRow o = new IntegrationDonorsDataRow();
            o.donorid = dr.Field<int>("donor_id").ToString();
            o.donoraltid = dr.Field<string>("pid");
            o.donorClearstarProfId = dr.Field<string>("donorClearstarProfId") != null ? dr.Field<string>("donorClearstarProfId") : "";
            o.donortestid = dr.Field<int>("donor_test_info_id").ToString();
            o.deptclearstarcode = dr.Field<string>("clearstarcode") != null ? dr.Field<string>("clearstarcode") : "";
            o.testcategory = ((TestCategories)dr.Field<int>("test_category_id")).ToString();
            o.testcategoryid = dr.Field<int>("test_category_id");
            o.testcreated_on = dr.Field<DateTime>("dti_created_on");
            o.teststatus = ((DonorRegistrationStatus)dr.Field<int>("test_status")).ToString();
            o.teststatusid = dr.Field<int>("test_status");
            o.test_requested_date = dr.Field<DateTime>("test_requested_date");
            o.doccreated_on = dr.Field<DateTime>("doc_created_on");
            //docid = dr.Field<int>("report_info_id") != null ? dr.Field<int>("report_info_id") : 0;
            o.docid = dr.Field<int>("report_info_id");
            o.docbase64 = BackendStatics.Base64EncodeBytes(dr.Field<byte[]>("lab_report"));
            o.doctest_requested_date = dr.Field<DateTime>("test_requested_date");
            o.filename = dr.Field<string>("lab_report_source_filename");
            o.received_on = dr.Field<DateTime>("doc_received_on");
            o.report_type = ((ReportType)dr.Field<int>("report_type")).ToString(); // dr.Field<int>("report_type");
            o.report_typeid = dr.Field<int>("report_type");
            o.final_report_id = (int)dr.Field<long>("final_report_id");
            o.document_content = dr.IsNull("document_content") ? "" : BackendStatics.Base64EncodeBytes(dr.Field<byte[]>("document_content"));
            o.document_title = dr.Field<string>("document_title");
            o.document_upload_time = dr.IsNull("document_upload_time") ? DateTime.MinValue : dr.Field<DateTime>("document_upload_time");
            o.donor_document_id = dr.IsNull("donor_document_id") ? 0 : dr.Field<int>("donor_document_id");
            o.donor_document_typeId = dr.IsNull("donor_document_typeId") ? 0 : dr.Field<int>("donor_document_typeId");
            return o;
        }

        private IntegrationPartner populateIntegrationPartnerFromDataRow(DataRow dr)
        {
            IntegrationPartner o = new IntegrationPartner()
            {
                last_modified_by = dr.Field<string>("last_modified_by"),
                backend_integration_partner_id = dr.Field<int>("backend_integration_partner_id"),
                partner_name = dr.Field<string>("partner_name"),
                partner_crypto = dr.Field<string>("partner_crypto"),
                partner_key = dr.Field<string>("partner_key"),
                partner_push = dr.Field<sbyte>("partner_push") > 0,
                partner_push_host = dr.Field<string>("partner_push_host"),
                partner_push_password = dr.Field<string>("partner_push_password"),
                partner_push_path = dr.Field<string>("partner_push_path"),
                partner_push_port = dr.Field<string>("partner_push_port"),
                partner_push_username = dr.Field<string>("partner_push_username"),
                backend_integration_partners_pidtype = dr.Field<int>("backend_integration_partners_pidtype"),
                active = dr.Field<sbyte>("active") > 0,
                created_on = dr.Field<DateTime>("created_on"),
                last_modified_on = dr.Field<DateTime>("last_modified_on"),
                login_url = dr.Field<string>("login_url"),
                html_instructions = LongBlobToASCIIString(dr, "html_instructions") // dr.Field<byte>("html_instructions").ToString()
            };

            if (!dr.IsNull("last_modified_on")) o.last_modified_on = dr.Field<DateTime>("last_modified_on");
            if (!dr.IsNull("created_on")) o.created_on = dr.Field<DateTime>("created_on");

            return o;
        }

        private IntegrationPartnerRelease populateIntegrationPartnerReleaseFromDataRow(DataRow dr)
        {
            IntegrationPartnerRelease o = new IntegrationPartnerRelease()
            {
                backend_integration_partner_release_id = dr.Field<int>("backend_integration_partner_release_id"),
                backend_integration_partner_release_GUID = dr.Field<string>("backend_integration_partner_release_GUID"),
                background_check = dr.Field<int>("background_check") > 0,
                created_on = dr.Field<DateTime>("created_on"),
                last_modified_on = dr.Field<DateTime>("last_modified_on"),
                sent_on = dr.IsNull("sent_on") ? DateTime.MinValue : dr.Field<DateTime>("sent_on"),
                donor_document_id = dr.Field<int>("donor_document_id"),
                donor_test_test_category_id = dr.Field<int>("donor_test_test_category_id"),
                donor_test_info_id = dr.Field<int>("donor_test_info_id "),
                released = dr.Field<sbyte>("released") > 0,
                sent = dr.Field<sbyte>("sent") > 0,
                last_modified_by = dr.Field<string>("last_modified_by"),
                released_by = dr.Field<string>("released_by"),
                report_info_id = dr.Field<int>("report_info_id"),
                donor_id = dr.Field<int>("donor_id"),
                client_department_id = dr.Field<int>("client_department_id"),
                client_id = dr.Field<int>("client_id"),
            };

            if (!dr.IsNull("last_modified_on")) o.last_modified_on = dr.Field<DateTime>("last_modified_on");
            if (!dr.IsNull("created_on")) o.created_on = dr.Field<DateTime>("created_on");

            return o;
        }

        private void FieldToNullDateTime(DataRow dr)
        {
        }

        public List<T> DataReaderMapToList<T>(IDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (!object.Equals(dr[prop.Name], DBNull.Value))
                    {
                        prop.SetValue(obj, dr[prop.Name], null);
                    }
                }
                list.Add(obj);
            }
            return list;
        }

        #endregion Private_Functions
    }

    public static class DateTimeNullHelper
    {
        public static object DTOrDBNull(DateTime? dt)
        {
            if (dt == null) return DBNull.Value;
            return dt;
        }

        public static DateTime? DBNullorDT(object o)
        {
            if (o == null) return null;
            return (DateTime?)o;
        }
    }

    public class ScheduleLoadHelper
    {
        public List<DateTime> window_dates { get; set; } = new List<DateTime>();
        public Dictionary<DateTime, int> counts { get; set; } = new Dictionary<DateTime, int>();

        public DateTime start { get; set; } = new DateTime();
        public DateTime end { get; set; } = new DateTime();
        public DateTime sweep { get; set; } = new DateTime();
        public int max_sendins { get; set; } = 5;
        //public DateTime GetASendInDate()
        //{
        //    if (this.window_dates.Count < 1) return DateTime.MinValue;

        //    this.window_dates.RemoveAll(x => this.counts.Any(d => d.Value >= this.max_sendins));
        //    // shuffle dates to pick a random day with send ins being less than the max send ins
        //    this.window_dates = this.window_dates.OrderBy(x => Guid.NewGuid()).ToList();
        //    return this.window_dates.FirstOrDefault();
        //}
    }

    public static class BackendStatics
    {
        public static string Base64EncodeBytes(byte[] bytes)
        {
            //var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(bytes);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        //Decode
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string Base64URLDecode(string base64URLEncodedData)
        {
            // Add needed characters for URL encoded (which is always missing = at end)
            // https://stackoverflow.com/questions/1228701/code-for-decoding-encoding-a-modified-base64-url

            base64URLEncodedData = base64URLEncodedData.PadRight(base64URLEncodedData.Length + (4 - base64URLEncodedData.Length % 4) % 4, '=');

            var base64EncodedBytes = System.Convert.FromBase64String(base64URLEncodedData);

            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}