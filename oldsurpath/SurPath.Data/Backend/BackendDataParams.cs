using MySql.Data.MySqlClient;
using SurPath.Entity;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SurPath.Data
{
    #region params

    #region Generics

    /// <summary>
    /// This is a generic class for stored procs to get data back.
    /// if no output id, it returns DataTable of results
    ///
    /// ParamGeneric paramGeneric = new ParamGeneric()
    ///{
    ///    StoreProc = "backend_get_donor_info_id_by_phone",
    ///    outputID = "@donor_test_info_id"
    ///};
    ///paramGeneric.AddParam("@phone_number", "2148019441");
    ///paramGeneric.AddParam("@donor_test_info_id", MySqlDbType.Int32);
    ///int _donor_test_info_id = (int)d.GetGenericObject(paramGeneric);
    /// </summary>
    public class ParamGeneric : IBackendDataParamObject
    {
        private List<MySqlParameter> _params = new List<MySqlParameter>();
        public bool DoLogging = true;

        public bool AddParam(string parameterName, object value)
        {
            bool retval = false;
            try
            {
                this._params.Add(new MySqlParameter(parameterName, value));
                retval = true;
            }
            catch (Exception)
            {
                throw;
            }
            return retval;
        }

        public MySqlParameter[] Parameters()
        {
            if (this._params.Count < 1) return new List<MySqlParameter>().ToArray();
            if (!string.IsNullOrEmpty(this.outputID)) _params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.Output;
            return _params.ToArray();
        }

        public string StoreProc { get; set; } = string.Empty;

        public string sp()
        {
            return this.StoreProc;
        }

        public string outputID = "";
    }

    public class ParamGenericSQL : IBackendDataParamObject
    {
        private List<MySqlParameter> _params = new List<MySqlParameter>();
        public bool DoLogging = true;

        public bool AddParam(string parameterName, object value)
        {
            bool retval = false;
            try
            {
                this._params.Add(new MySqlParameter(parameterName, value));
                retval = true;
            }
            catch (Exception)
            {
                throw;
            }
            return retval;
        }

        public MySqlParameter[] Parameters()
        {
            if (this._params.Count < 1) return new List<MySqlParameter>().ToArray();
            if (!string.IsNullOrEmpty(this.outputID)) _params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.Output;
            return _params.ToArray();
        }

        public string sql { get; set; } = string.Empty;

        public string sp()
        {
            return string.Empty;
        }

        public string query()
        {
            return this.sql;
        }

        public string outputID = "";
    }

    #endregion Generics

    public class ParamQueueDonorNotification : IBackendDataParamObject
    {
        public int donor_test_info_id { get; set; }
        public DateTime created_on { get; set; }
        public string created_by { get; set; } = "SYSTEM";
        public DateTime last_modified_on { get; set; }
        public string last_modified_by { get; set; } = "SYSTEM";
        public int backend_notifications_id { get; set; } = 0;

        public string sp()
        {
            return "backend_queue_donor_notification";
        }

        public string outputID = "@backend_notifications_id";

        public MySqlParameter[] Parameters()
        {
            //in sendTo varchar(45),
            //in sendFrom varchar(45),
            //in textToSend varchar(150),
            //in client_id int,
            //in donor_id int,
            //in apikey varchar(45),
            //out param_backend_sms_queue_id int
            // BACKEND_QUEUE_SMS('2148019441','2148019441','TEST SEND',1,1,'APIKEY', @sms_id);
            List<MySqlParameter> _params = new List<MySqlParameter>();

            _params.Add(new MySqlParameter("@donor_test_info_id", this.donor_test_info_id));
            _params.Add(new MySqlParameter("@created_by", this.created_by));
            _params.Add(new MySqlParameter("@last_modified_by", this.last_modified_by));
            _params.Add(new MySqlParameter("@backend_notifications_id", MySqlDbType.Int32));
            if (!string.IsNullOrEmpty(this.outputID)) _params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.Output;
            return _params.ToArray();
        }
    }

    public class ParamSetDonorActivity : IBackendDataParamObject
    {
        public int donor_test_info_id { get; set; }
        public int activity_user_id { get; set; }
        public int activity_category_id { get; set; }
        public bool is_activity_visible { get; set; } = true;
        public bool is_synchronized { get; set; } = false;
        public string activity_note { get; set; }
        public int donor_test_activity_id { get; set; }
        public string sp()
        {
            return "backend_set_donor_test_activity";
        }

        public string outputID = "@donor_test_activity_id";

        public MySqlParameter[] Parameters()
        {
            //in sendTo varchar(45),
            //in sendFrom varchar(45),
            //in textToSend varchar(150),
            //in client_id int,
            //in donor_id int,
            //in apikey varchar(45),
            //out param_backend_sms_queue_id int
            // BACKEND_QUEUE_SMS('2148019441','2148019441','TEST SEND',1,1,'APIKEY', @sms_id);
            List<MySqlParameter> _params = new List<MySqlParameter>();

            _params.Add(new MySqlParameter("@donor_test_info_id", this.donor_test_info_id));
            _params.Add(new MySqlParameter("@activity_user_id", this.activity_user_id));
            _params.Add(new MySqlParameter("@activity_category_id", this.activity_category_id));
            _params.Add(new MySqlParameter("@is_activity_visible", this.is_activity_visible));
            _params.Add(new MySqlParameter("@is_synchronized", this.is_synchronized));
            _params.Add(new MySqlParameter("@activity_note", this.activity_note));
            _params.Add(new MySqlParameter("@donor_test_activity_id", MySqlDbType.Int32));
            if (!string.IsNullOrEmpty(this.outputID)) _params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.Output;
            return _params.ToArray();
        }
    }

    public class ParamSetBackend_client_edit_activity : IBackendDataParamObject
    {
        public int backend_notification_window_data_id { get; set; }
        public int activity_user_id { get; set; }
        public int activity_category_id { get; set; }
        public string activity_note { get; set; }
        public int backend_client_edit_activity_id { get; set; }
        public string sp()
        {
            return "backend_set_client_edit_activity";
        }

        public string outputID = "@backend_client_edit_activity_id";

        public MySqlParameter[] Parameters()
        {
            //in sendTo varchar(45),
            //in sendFrom varchar(45),
            //in textToSend varchar(150),
            //in client_id int,
            //in donor_id int,
            //in apikey varchar(45),
            //out param_backend_sms_queue_id int
            // BACKEND_QUEUE_SMS('2148019441','2148019441','TEST SEND',1,1,'APIKEY', @sms_id);
            List<MySqlParameter> _params = new List<MySqlParameter>();

            _params.Add(new MySqlParameter("@donor_test_info_id", this.backend_notification_window_data_id));
            _params.Add(new MySqlParameter("@activity_user_id", this.activity_user_id));
            _params.Add(new MySqlParameter("@activity_category_id", this.activity_category_id));
            _params.Add(new MySqlParameter("@activity_note", this.activity_note));
            _params.Add(new MySqlParameter("@backend_client_edit_activity_id", MySqlDbType.Int32));
            if (!string.IsNullOrEmpty(this.outputID)) _params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.Output;
            return _params.ToArray();
        }
    }
    public class ParamSetUserActivity : IBackendDataParamObject
    {
        public int activity_user_id { get; set; }
        public int activity_user_category_id { get; set; }
        public bool is_activity_visible { get; set; } = true;
        public bool is_synchronized { get; set; } = true;
        public string activity_note { get; set; }
        public int user_activity_id { get; set; }

        public string sp()
        {
            return "backend_set_user_activity";
        }

        public string outputID = "@user_activity_id";

        public MySqlParameter[] Parameters()
        {
            //in sendTo varchar(45),
            //in sendFrom varchar(45),
            //in textToSend varchar(150),
            //in client_id int,
            //in donor_id int,
            //in apikey varchar(45),
            //out param_backend_sms_queue_id int
            // BACKEND_QUEUE_SMS('2148019441','2148019441','TEST SEND',1,1,'APIKEY', @sms_id);
            List<MySqlParameter> _params = new List<MySqlParameter>();

            _params.Add(new MySqlParameter("@activity_user_id", this.activity_user_id));
            _params.Add(new MySqlParameter("@activity_user_category_id", this.activity_user_category_id));
            _params.Add(new MySqlParameter("@is_activity_visible", this.is_activity_visible));
            _params.Add(new MySqlParameter("@is_synchronized", this.is_synchronized));
            _params.Add(new MySqlParameter("@activity_note", this.activity_note));
            _params.Add(new MySqlParameter("@user_activity_id", MySqlDbType.Int32));
            if (!string.IsNullOrEmpty(this.outputID)) _params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.Output;
            return _params.ToArray();
        }
    }
    public class ParamGetUserActivity : IBackendDataParamObject
    {
        public int activity_user_id { get; set; }
        public int activity_user_category_id { get; set; }
        public bool is_activity_visible { get; set; } = true;

        public string sp()
        {
            return "backend_get_user_activity";
        }

        public string outputID = "";

        public MySqlParameter[] Parameters()
        {
            //in sendTo varchar(45),
            //in sendFrom varchar(45),
            //in textToSend varchar(150),
            //in client_id int,
            //in donor_id int,
            //in apikey varchar(45),
            //out param_backend_sms_queue_id int
            // BACKEND_QUEUE_SMS('2148019441','2148019441','TEST SEND',1,1,'APIKEY', @sms_id);
            List<MySqlParameter> _params = new List<MySqlParameter>();

            _params.Add(new MySqlParameter("@activity_user_id", this.activity_user_id));
            _params.Add(new MySqlParameter("@activity_user_category_id", this.activity_user_category_id));
            _params.Add(new MySqlParameter("@is_activity_visible", this.is_activity_visible));
            if (!string.IsNullOrEmpty(this.outputID)) _params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.Output;
            return _params.ToArray();
        }
    }

    public class ParamSetDonorNotification : IBackendDataParamObject
    {
        //public int backend_notifications_id { get; set; } = 0;
        //public int donor_test_info_id { get; set; } = 0;
        //public bool notified_by_email { get; set; } = false;
        //public bool notified_by_sms { get; set; } = false;
        //public int notification_email_exception { get; set; } = 0;
        //public int notification_sms_exception { get; set; } = 0;
        //public int clinic_exception { get; set; } = 0;
        //public int is_archived { get; set; } = 0;
        //public bool notify_now { get; set; } = false;
        //public bool notify_next_window { get; set; } = false;
        //public string notification_sent_to_email { get; set; } = String.Empty;
        //public string notification_sent_to_phone { get; set; } = String.Empty;
        //public string created_by { get; set; } = "SYSTEM";
        //public string last_modified_by { get; set; } = "SYSTEM";

        public Notification notification { get; set; } = new Notification();

        public string sp()
        {
            return "backend_set_donor_notification";
        }

        public string outputID = "";

        public MySqlParameter[] Parameters()
        {
            List<MySqlParameter> _params = new List<MySqlParameter>();
            _params.Add(new MySqlParameter("@donor_test_info_id", this.notification.donor_test_info_id));
            _params.Add(new MySqlParameter("@notified_by_email", this.notification.notified_by_email));
            _params.Add(new MySqlParameter("@notified_by_sms", this.notification.notified_by_sms));
            _params.Add(new MySqlParameter("@notification_email_exception", this.notification.notification_email_exception));
            _params.Add(new MySqlParameter("@notification_sms_exception", this.notification.notification_sms_exception));
            _params.Add(new MySqlParameter("@notification_sent_to_email", this.notification.notification_sent_to_email));
            _params.Add(new MySqlParameter("@notification_sent_to_phone", this.notification.notification_sent_to_phone));
            _params.Add(new MySqlParameter("@notify_now", this.notification.notify_now));
            _params.Add(new MySqlParameter("@notify_again", this.notification.notify_again));
            _params.Add(new MySqlParameter("@force_db", this.notification.force_db));
            _params.Add(new MySqlParameter("@notify_manual", this.notification.notify_manual));
            _params.Add(new MySqlParameter("@notify_next_window", this.notification.notify_next_window));
            _params.Add(new MySqlParameter("@is_archived", this.notification.is_archived));
            _params.Add(new MySqlParameter("@clinic_exception", this.notification.clinic_exception));
            _params.Add(new MySqlParameter("@clinic_radius", this.notification.clinic_radius));
            _params.Add(new MySqlParameter("@created_by", this.notification.created_by));
            _params.Add(new MySqlParameter("@last_modified_by", this.notification.last_modified_by));
            _params.Add(new MySqlParameter("@backend_notifications_id", this.notification.backend_notifications_id));
            _params.Add(new MySqlParameter("@notify_after_timestamp", DateTimeNullHelper.DTOrDBNull(this.notification.notify_after_timestamp)));
            _params.Add(new MySqlParameter("@notify_before_timestamp", DateTimeNullHelper.DTOrDBNull(this.notification.notify_before_timestamp)));
            _params.Add(new MySqlParameter("@notify_reset_sendin", this.notification.notify_reset_sendin));

            if (!string.IsNullOrEmpty(this.outputID)) _params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.Output;
            return _params.ToArray();
        }
    }

    public class ParamGetDonorNotification : IBackendDataParamObject
    {
        public int donor_test_info_id { get; set; } = 0;

        public string sp()
        {
            return "backend_get_donor_notification";
        }

        public string outputID = "";

        public MySqlParameter[] Parameters()
        {
            //in sendTo varchar(45),
            //in sendFrom varchar(45),
            //in textToSend varchar(150),
            //in client_id int,
            //in donor_id int,
            //in apikey varchar(45),
            //out param_backend_sms_queue_id int
            // BACKEND_QUEUE_SMS('2148019441','2148019441','TEST SEND',1,1,'APIKEY', @sms_id);
            List<MySqlParameter> _params = new List<MySqlParameter>();
            _params.Add(new MySqlParameter("@donor_test_info_id", this.donor_test_info_id));
            //_params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.Output;
            return _params.ToArray();
        }
    }

    #region SMS

    public class ParamSetSMSActivity : IBackendDataParamObject
    {
        public SMSActivity smsActivity { get; set; } = new SMSActivity();

        public string sp()
        {
            return "backend_set_sms_activity";
        }

        public string outputID = "";

        public MySqlParameter[] Parameters()
        {
            MySqlParameter mySqlParameter;
            List<MySqlParameter> _params = new List<MySqlParameter>();

            SMSActivity _model = this.smsActivity;

            _params.Add(new MySqlParameter("@backend_sms_activity_id", _model.backend_sms_activity_id));
            _params.Add(new MySqlParameter("@donor_test_info_id", _model.donor_test_info_id));
            _params.Add(new MySqlParameter("@sent_text", _model.sent_text));
            _params.Add(new MySqlParameter("@dt_sms_sent", DateTimeNullHelper.DTOrDBNull(_model.dt_sms_sent)));
            _params.Add(new MySqlParameter("@reply_text", _model.reply_text));
            _params.Add(new MySqlParameter("@auto_reply_text", _model.auto_reply_text));
            _params.Add(new MySqlParameter("@dt_reply_received", DateTimeNullHelper.DTOrDBNull(_model.dt_reply_received)));
            _params.Add(new MySqlParameter("@reply_read", _model.reply_read));
            _params.Add(new MySqlParameter("@user_id", _model.user_id));
            _params.Add(new MySqlParameter("@created_by", _model.created_by));
            _params.Add(new MySqlParameter("@last_modified_by", _model.last_modified_by));
            _params.Add(new MySqlParameter("@reply_read_timestamp", DateTimeNullHelper.DTOrDBNull(_model.reply_read_timestamp)));

            if (!string.IsNullOrEmpty(this.outputID)) _params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.Output;
            return _params.ToArray();
        }
    }

    public class ParamGetSMSActivity : IBackendDataParamObject
    {
        public int donor_test_info_id { get; set; } = 0;

        public string sp()
        {
            return "backend_get_sms_activity";
        }

        public string outputID = "";

        public MySqlParameter[] Parameters()
        {
            MySqlParameter mySqlParameter;
            List<MySqlParameter> _params = new List<MySqlParameter>();

            _params.Add(new MySqlParameter("@donor_test_info_id", this.donor_test_info_id));

            if (!string.IsNullOrEmpty(this.outputID)) _params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.Output;
            return _params.ToArray();
        }
    }

    #endregion SMS

    public class ParamGetReadyDonorNotifications : IBackendDataParamObject
    {
        public string sp()
        {
            return "backend_get_ready_donor_notifications";
        }

        public string outputID = "";

        public MySqlParameter[] Parameters()
        {
            List<MySqlParameter> _params = new List<MySqlParameter>();
            if (!string.IsNullOrEmpty(this.outputID)) _params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.Output;
            return _params.ToArray();
        }
    }

    public class ParamQueueSMS : IBackendDataParamObject
    {
        public int client_id { get; set; }
        public int client_department_id { get; set; }
        public int donor_id { get; set; }
        public string sendTo { get; set; }
        public string textToSend { get; set; }
        public string created_by { get; set; } = "SYSTEM";
        public string last_modified_by { get; set; } = "SYSTEM";
        public int backend_sms_queue_id { get; set; } = 0;

        public string sp()
        {
            return "BACKEND_QUEUE_SMS";
        }

        public string outputID = "@backend_sms_queue_id";

        public MySqlParameter[] Parameters()
        {
            //in sendTo varchar(45),
            //in sendFrom varchar(45),
            //in textToSend varchar(150),
            //in client_id int,
            //in donor_id int,
            //in apikey varchar(45),
            //out param_backend_sms_queue_id int
            // BACKEND_QUEUE_SMS('2148019441','2148019441','TEST SEND',1,1,'APIKEY', @sms_id);
            List<MySqlParameter> _params = new List<MySqlParameter>();
            _params.Add(new MySqlParameter("@sendTo", this.sendTo));
            _params.Add(new MySqlParameter("@textToSend", this.textToSend));
            _params.Add(new MySqlParameter("@created_by", this.created_by));
            _params.Add(new MySqlParameter("@last_modified_by", this.last_modified_by));
            _params.Add(new MySqlParameter("@client_id", this.client_id));
            _params.Add(new MySqlParameter("@client_department_id", this.client_id));
            _params.Add(new MySqlParameter("@donor_id", this.donor_id));
            _params.Add(new MySqlParameter("@backend_sms_queue_id", MySqlDbType.Int32));
            _params.Where(x => x.ParameterName == "@backend_sms_queue_id").First().Direction = System.Data.ParameterDirection.Output;
            return _params.ToArray();
        }
    }

    public class ParamLogSMSReply : IBackendDataParamObject
    {
        public int donor_test_info_id { get; set; }
        public int backend_sms_replies_id { get; set; }
        public string reply { get; set; }
        public string created_by { get; set; } = "SYSTEM";
        public string last_modified_by { get; set; } = "SYSTEM";

        public string sp()
        {
            return "backend_log_sms_reply";
        }

        public string outputID = "@backend_sms_replies_id";

        public MySqlParameter[] Parameters()
        {
            //CREATE PROCEDURE surpathlive.`backend_set_sms_reply`(
            //in backend_sms_queue_id int,
            //in reply varchar(150),
            //out backend_sms_replies_id int
            //)
            //call backend_set_sms_reply(@sms_id, 'test reply', @sms_replyid );
            List<MySqlParameter> _params = new List<MySqlParameter>();
            _params.Add(new MySqlParameter("@donor_test_info_id", this.donor_test_info_id));
            _params.Add(new MySqlParameter("@reply", this.reply));
            _params.Add(new MySqlParameter("@backend_sms_replies_id", MySqlDbType.Int32));
            _params.Add(new MySqlParameter("@created_by", this.created_by));
            _params.Add(new MySqlParameter("@last_modified_by", this.last_modified_by));
            if (!string.IsNullOrEmpty(this.outputID)) _params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.Output;
            return _params.ToArray();
        }
    }

    /// <summary>
    /// This param class uses an object as a parameter
    /// because of the large number of properties, it makes the code more readable.
    /// TODO - update other param classes to use their retur model for parameters
    /// as this is more elegant
    /// </summary>

    public class ParamSetClientNotificationSettings : IBackendDataParamObject
    {
        public ParamSetClientNotificationSettings()
        {
            this.clientNotificationDataSettings = new ClientNotificationDataSettings();
        }

        public ClientNotificationDataSettings clientNotificationDataSettings { get; set; }

        public MySqlParameter[] Parameters()
        {
            //DateTimeNullHelper.DTOrDBNull

            List<MySqlParameter> _params = new List<MySqlParameter>();
            _params.Add(new MySqlParameter("@client_id", this.clientNotificationDataSettings.client_id));
            _params.Add(new MySqlParameter("@client_department_id", this.clientNotificationDataSettings.client_department_id));
            _params.Add(new MySqlParameter("@pdf_template_filename", this.clientNotificationDataSettings.pdf_template_filename));
            _params.Add(new MySqlParameter("@pdf_render_settings_filename", this.clientNotificationDataSettings.pdf_render_settings_filename));
            _params.Add(new MySqlParameter("@send_asap", this.clientNotificationDataSettings.send_asap));
            _params.Add(new MySqlParameter("@override_day_schedule", this.clientNotificationDataSettings.override_day_schedule));
            _params.Add(new MySqlParameter("@created_by", this.clientNotificationDataSettings.created_by));
            _params.Add(new MySqlParameter("@notification_start_date", DateTimeNullHelper.DTOrDBNull(this.clientNotificationDataSettings.notification_start_date)));
            _params.Add(new MySqlParameter("@notification_stop_date", DateTimeNullHelper.DTOrDBNull(this.clientNotificationDataSettings.notification_stop_date)));
            _params.Add(new MySqlParameter("@notification_sweep_date", this.clientNotificationDataSettings.notification_sweep_date));
            _params.Add(new MySqlParameter("@last_modified_by", this.clientNotificationDataSettings.last_modified_by));
            _params.Add(new MySqlParameter("@deadline_alert_in_days", this.clientNotificationDataSettings.deadline_alert_in_days));
            _params.Add(new MySqlParameter("@max_sendins", this.clientNotificationDataSettings.max_sendins));
            _params.Add(new MySqlParameter("@delay_in_hours", this.clientNotificationDataSettings.delay_in_hours));
            _params.Add(new MySqlParameter("@enable_sms", this.clientNotificationDataSettings.enable_sms));
            _params.Add(new MySqlParameter("@onsite_testing", this.clientNotificationDataSettings.onsite_testing));
            _params.Add(new MySqlParameter("@force_manual", this.clientNotificationDataSettings.force_manual));
            _params.Add(new MySqlParameter("@use_formfox", this.clientNotificationDataSettings.use_formfox));
            _params.Add(new MySqlParameter("@backend_notification_window_data_id", this.clientNotificationDataSettings.backend_notification_window_data_id));

            _params.Add(new MySqlParameter("@client_sms_token", this.clientNotificationDataSettings.client_sms_token));
            _params.Add(new MySqlParameter("@client_sms_apikey", this.clientNotificationDataSettings.client_sms_apikey));
            _params.Add(new MySqlParameter("@client_autoresponse", this.clientNotificationDataSettings.client_autoresponse));
            _params.Add(new MySqlParameter("@client_sms_from_number", this.clientNotificationDataSettings.client_sms_from_number));
            _params.Add(new MySqlParameter("@show_web_notify_button", this.clientNotificationDataSettings.show_web_notify_button));

            foreach (int dayInt in System.Enum.GetValues(typeof(DayOfWeekEnum)))
            {
                string dayname = System.Enum.GetName(typeof(DayOfWeekEnum), dayInt).ToLower();
                ClientNotificationDataSettingsDay thisDay = this.clientNotificationDataSettings.DaySettings.Where(x => x.DayOfWeek == dayInt).First();

                _params.Add(new MySqlParameter($"@{dayname}", thisDay.Enabled));
                _params.Add(new MySqlParameter($"@{dayname}_send_time_start_seconds_from_midnight", thisDay.send_time_start_seconds_from_midnight));
                _params.Add(new MySqlParameter($"@{dayname}_send_time_stop_seconds_from_midnight", thisDay.send_time_stop_seconds_from_midnight));
            }

            //_params.Add(new MySqlParameter("@client_id", this.clientNotificationDataSettings.friday));
            //_params.Add(new MySqlParameter("@client_id", this.clientNotificationDataSettings.friday_send_time_start_seconds_from_midnight));
            //_params.Add(new MySqlParameter("@client_id", this.clientNotificationDataSettings.friday_send_time_stop_seconds_from_midnight));
            //_params.Add(new MySqlParameter("@client_id", this.clientNotificationDataSettings.last_modified_by));
            //_params.Add(new MySqlParameter("@client_id", this.clientNotificationDataSettings.monday));
            //_params.Add(new MySqlParameter("@client_id", this.clientNotificationDataSettings.monday_send_time_start_seconds_from_midnight));
            //_params.Add(new MySqlParameter("@client_id", this.clientNotificationDataSettings.monday_send_time_stop_seconds_from_midnight));
            //_params.Add(new MySqlParameter("@client_id", this.clientNotificationDataSettings.override_day_schedule));
            //_params.Add(new MySqlParameter("@client_id", this.clientNotificationDataSettings.saturday));
            //_params.Add(new MySqlParameter("@client_id", this.clientNotificationDataSettings.saturday_send_time_start_seconds_from_midnight));
            //_params.Add(new MySqlParameter("@client_id", this.clientNotificationDataSettings.saturday_send_time_stop_seconds_from_midnight));
            //_params.Add(new MySqlParameter("@client_id", this.clientNotificationDataSettings.send_asap));
            //_params.Add(new MySqlParameter("@client_id", this.clientNotificationDataSettings.sunday));
            //_params.Add(new MySqlParameter("@client_id", this.clientNotificationDataSettings.sunday_send_time_start_seconds_from_midnight));
            //_params.Add(new MySqlParameter("@client_id", this.clientNotificationDataSettings.sunday_send_time_stop_seconds_from_midnight));
            //_params.Add(new MySqlParameter("@client_id", this.clientNotificationDataSettings.thursday));
            //_params.Add(new MySqlParameter("@client_id", this.clientNotificationDataSettings.thursday_send_time_start_seconds_from_midnight));
            //_params.Add(new MySqlParameter("@client_id", this.clientNotificationDataSettings.thursday_send_time_stop_seconds_from_midnight));
            //_params.Add(new MySqlParameter("@client_id", this.clientNotificationDataSettings.tuesday));
            //_params.Add(new MySqlParameter("@client_id", this.clientNotificationDataSettings.tuesday_send_time_start_seconds_from_midnight));
            //_params.Add(new MySqlParameter("@client_id", this.clientNotificationDataSettings.tuesday_send_time_stop_seconds_from_midnight));
            //_params.Add(new MySqlParameter("@client_id", this.clientNotificationDataSettings.wednesday));
            //_params.Add(new MySqlParameter("@client_id", this.clientNotificationDataSettings.wednesday_send_time_start_seconds_from_midnight));
            //_params.Add(new MySqlParameter("@client_id", this.clientNotificationDataSettings.wednesday_send_time_stop_seconds_from_midnight));

            if (!string.IsNullOrEmpty(this.outputID)) _params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.Output;
            return _params.ToArray();
        }

        public string sp()
        {
            return "backend_set_notification_window_data";
        }

        public string outputID = "@backend_notification_window_data_id";
    }

    public class ParamGetClinicsForZip : IBackendDataParamObject
    {
        public ParamGetClinicsForZip()
        {
            this.CollectionFacilities = new List<CollectionFacility>();
        }

        public string _zip { get; set; }
        public int _dist { get; set; }
        public List<CollectionFacility> CollectionFacilities { get; set; }

        public MySqlParameter[] Parameters()
        {
            List<MySqlParameter> _params = new List<MySqlParameter>();
            _params.Add(new MySqlParameter("@_zip", this._zip));
            _params.Add(new MySqlParameter("@_dist", this._dist));
            //_params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.Output;
            return _params.ToArray();
        }

        public string sp()
        {
            return "backend_Get_Clinics_In_Range_Of_Zip";
        }

        public string outputID = "";
    }

    public class ParamGetClinicsForDonor : IBackendDataParamObject
    {
        public ParamGetClinicsForDonor()
        {
            this.CollectionFacilities = new List<CollectionFacility>();
        }

        public int donor_id { get; set; }
        public int _dist { get; set; }
        public List<CollectionFacility> CollectionFacilities { get; set; }

        public MySqlParameter[] Parameters()
        {
            List<MySqlParameter> _params = new List<MySqlParameter>();
            _params.Add(new MySqlParameter("@donor_id", this.donor_id));
            _params.Add(new MySqlParameter("@_dist", this._dist));
            //_params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.Output;
            return _params.ToArray();
        }

        public string sp()
        {
            return "Get_Clinics_For_Donor";
        }

        public string outputID = "";
    }

    public class ParamGetClientNotificationDataSettingsByPhone : IBackendDataParamObject
    {
        public string phone_number { get; set; } = string.Empty;

        public string sp()
        {
            return "backend_get_notification_window_data_by_donor_phone";
        }

        public string outputID = "";

        public MySqlParameter[] Parameters()
        {
            List<MySqlParameter> _params = new List<MySqlParameter>();

            _params.Add(new MySqlParameter("@phone_number", this.phone_number));
            if (!string.IsNullOrEmpty(this.outputID)) _params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.Output;
            return _params.ToArray();
        }
    }

    public class ParamGetClinicForDonorMinNumber : IBackendDataParamObject
    {
        public ParamGetClinicForDonorMinNumber()
        {
            this.CollectionFacilities = new List<CollectionFacility>();
        }

        public int donor_id { get; set; }
        public int min_count { get; set; }
        public int start_miles { get; set; }
        public int max_miles { get; set; }
        public List<CollectionFacility> CollectionFacilities { get; set; }

        public MySqlParameter[] Parameters()
        {
            List<MySqlParameter> _params = new List<MySqlParameter>();
            _params.Add(new MySqlParameter("@donor_id", this.donor_id));
            _params.Add(new MySqlParameter("@min_count", this.min_count));
            _params.Add(new MySqlParameter("@start_miles", this.start_miles));
            _params.Add(new MySqlParameter("@max_miles", this.max_miles));
            if (!string.IsNullOrEmpty(this.outputID)) _params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.Output;
            return _params.ToArray();
        }

        public string sp()
        {
            return "Find_X_Clinics_For_Donor";
        }

        public string outputID = "";
    }

    /// <summary>
    /// This pulls in data with for a Notification record, sweeping up
    /// details used to populate the PDF
    /// </summary>
    public class ParamGetNotificationInfoForDonorInfoId : IBackendDataParamObject
    {
        public int donor_test_info_id { get; set; }
        // public List<CollectionFacility> CollectionFacilities { get; set; }

        public MySqlParameter[] Parameters()
        {
            List<MySqlParameter> _params = new List<MySqlParameter>();
            _params.Add(new MySqlParameter("@donor_test_info_id", this.donor_test_info_id));
            //_params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.Output;
            return _params.ToArray();
        }

        public string sp()
        {
            return "backend_get_notification_information_for_donor_info_id";
        }

        public string outputID = "";
    }


    #region FormFox
    // backend_get_formfoxorders_overdue
    //public class ParamSetformfoxordersoverdue

    public class ParamSetformfoxorders : IBackendDataParamObject
    {
        public ParamSetformfoxorders()
        {
            this.formfoxorders = new formfoxorders();
        }

        public formfoxorders formfoxorders { get; set; }

        public MySqlParameter[] Parameters()
        {

            List<MySqlParameter> _params = new List<MySqlParameter>();
            _params.Add(new MySqlParameter("@donor_test_info_id", this.formfoxorders.donor_test_info_id));
            _params.Add(new MySqlParameter("@filename", this.formfoxorders.filename));
            _params.Add(new MySqlParameter("@ReferenceTestID", this.formfoxorders.ReferenceTestID));
            _params.Add(new MySqlParameter("@deadline", this.formfoxorders.deadline));
            _params.Add(new MySqlParameter("@status", this.formfoxorders.status));
            _params.Add(new MySqlParameter("@sfcode", this.formfoxorders.sfcode));
            _params.Add(new MySqlParameter("@sampletype", this.formfoxorders.sampletype));
            _params.Add(new MySqlParameter("@testid", this.formfoxorders.testid));
            _params.Add(new MySqlParameter("@SpecimenID", this.formfoxorders.SpecimenID));
            _params.Add(new MySqlParameter("@archived", this.formfoxorders.archived));

            _params.Add(new MySqlParameter("@backend_formfox_orders_id", this.formfoxorders.backend_formfox_orders_id));

            if (!string.IsNullOrEmpty(this.outputID)) _params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.Output;
            return _params.ToArray();
        }

        public string sp()
        {
            return "backend_set_formfoxorders";
        }

        public string outputID = "@backend_formfox_orders_id";
    }

    public class ParamGetformfoxorders : IBackendDataParamObject
    {
        public ParamGetformfoxorders()
        {
            this.formfoxorders = new formfoxorders();
        }

        public formfoxorders formfoxorders { get; set; }

        public MySqlParameter[] Parameters()
        {

            List<MySqlParameter> _params = new List<MySqlParameter>();
            _params.Add(new MySqlParameter("@ReferenceTestID", this.formfoxorders.ReferenceTestID));
            _params.Add(new MySqlParameter("@SpecimenID", this.formfoxorders.SpecimenID));

            if (!string.IsNullOrEmpty(this.outputID)) _params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.Output;
            return _params.ToArray();
        }

        public string sp()
        {
            return "backend_get_formfoxorders";
        }

        public string outputID = "";
    }
    #endregion FormFox



    #region Integration
    public class ParamCheckPartner_key : IBackendDataParamObject
    {
        public string partner_key { get; set; }
        public int backend_integration_partners_id { get; set; }
        public string sp()
        {
            return "backend_get_partner_key";
        }

        public string outputID = "@backend_integration_partners_id";

        public MySqlParameter[] Parameters()
        {
            List<MySqlParameter> _params = new List<MySqlParameter>();

            _params.Add(new MySqlParameter("@partner_key", this.partner_key));

            _params.Add(new MySqlParameter("@backend_integration_partners_id", MySqlDbType.Int32));

            if (!string.IsNullOrEmpty(this.outputID)) _params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.Output;
            return _params.ToArray();
        }
    }


    public class ParamGetIntegrationPartnerByPartnerKey : IBackendDataParamObject
    {
        public string partner_key { get; set; }
        public IntegrationPartner IntegrationPartner { get; set; }
        public string sp()
        {
            return "backend_get_integration_partner_by_key";
        }

        public string outputID = "";

        public MySqlParameter[] Parameters()
        {
            List<MySqlParameter> _params = new List<MySqlParameter>();

            _params.Add(new MySqlParameter("@partner_key", this.partner_key));
            if (!string.IsNullOrEmpty(this.outputID)) _params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.Output;
            return _params.ToArray();
        }
    }

    public class ParamGetIntegrationPartnerByPartnerClientCode : IBackendDataParamObject
    {
        public string partner_client_code { get; set; }
        public IntegrationPartner IntegrationPartner { get; set; }
        public string sp()
        {
            return "backend_get_integration_partner_by_partner_client_code";
        }

        public string outputID = "";

        public MySqlParameter[] Parameters()
        {
            List<MySqlParameter> _params = new List<MySqlParameter>();

            _params.Add(new MySqlParameter("@partner_client_code", this.partner_client_code));
            if (!string.IsNullOrEmpty(this.outputID)) _params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.Output;
            return _params.ToArray();
        }
    }

    //


    public class ParamSetIntegrationPartnerByKey : IBackendDataParamObject
    {
        public string partner_key { get; set; }
        public int backend_integration_partner_id { get; set; }
        public IntegrationPartner IntegrationPartner { get; set; }
        public string sp()
        {
            return "backend_set_integration_partner";
        }

        public string outputID = "@backend_integration_partner_id";

        public MySqlParameter[] Parameters()
        {

            List<MySqlParameter> _params = new List<MySqlParameter>();
            _params.Add(new MySqlParameter("@partner_name", this.IntegrationPartner.partner_name));
            _params.Add(new MySqlParameter("@partner_key", this.IntegrationPartner.partner_key));
            _params.Add(new MySqlParameter("@partner_crypto", this.IntegrationPartner.partner_crypto));
            _params.Add(new MySqlParameter("@backend_integration_partners_pidtype", this.IntegrationPartner.backend_integration_partners_pidtype));
            _params.Add(new MySqlParameter("@last_modified_by", this.IntegrationPartner.last_modified_by));

            _params.Add(new MySqlParameter("@backend_integration_partner_id", MySqlDbType.Int32));

            if (!string.IsNullOrEmpty(this.outputID)) _params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.Output;
            return _params.ToArray();
        }
    }

    public class ParamSetIntegrationPartnerClient : IBackendDataParamObject
    {
        public string partner_key { get; set; }
        public IntegrationPartnerClient IntegrationPartnerClient { get; set; }
        public int backend_integration_partner_client_map_id { get; set; }
        public string sp()
        {
            return "backend_set_integration_partner_client";
        }

        public string outputID = "@backend_integration_partner_client_map_id";

        public MySqlParameter[] Parameters()
        {

            List<MySqlParameter> _params = new List<MySqlParameter>();
            _params.Add(new MySqlParameter("@client_id", this.IntegrationPartnerClient.client_id));
            _params.Add(new MySqlParameter("@client_department_id", this.IntegrationPartnerClient.client_department_id));
            _params.Add(new MySqlParameter("@backend_integration_partner_id", this.IntegrationPartnerClient.backend_integration_partner_id));
            _params.Add(new MySqlParameter("@guid", this.IntegrationPartnerClient.backend_integration_partner_client_map_GUID.ToString()));
            _params.Add(new MySqlParameter("@partner_client_id", this.IntegrationPartnerClient.partner_client_id));
            _params.Add(new MySqlParameter("@partner_client_code", this.IntegrationPartnerClient.partner_client_code));
            _params.Add(new MySqlParameter("@last_modified_by", this.IntegrationPartnerClient.last_modified_by));
            _params.Add(new MySqlParameter("@partner_key", this.partner_key));
            _params.Add(new MySqlParameter("@active", this.IntegrationPartnerClient.active));
            _params.Add(new MySqlParameter("@require_login", this.IntegrationPartnerClient.require_login));
            _params.Add(new MySqlParameter("@partner_push_folder", this.IntegrationPartnerClient.partner_push_folder));
            _params.Add(new MySqlParameter("@require_remote_login", this.IntegrationPartnerClient.require_remote_login));


            _params.Add(new MySqlParameter("@backend_integration_partner_client_map_id", MySqlDbType.Int32));

            if (!string.IsNullOrEmpty(this.outputID)) _params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.Output;
            return _params.ToArray();
        }
    }

    public class ParamGetIntegrationPartnerClientsByPartnerKey : IBackendDataParamObject
    {
        public string partner_key { get; set; }
        public IntegrationPartner IntegrationPartner { get; set; }
        public string sp()
        {
            return "backend_get_integration_clients_by_key";
        }

        public string outputID = "";

        public MySqlParameter[] Parameters()
        {
            List<MySqlParameter> _params = new List<MySqlParameter>();

            _params.Add(new MySqlParameter("@partner_key", this.partner_key));

            if (!string.IsNullOrEmpty(this.outputID)) _params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.Output;
            return _params.ToArray();
        }
    }
    public class ParamGetIntegrationPartnerClientByPartnerClientId : IBackendDataParamObject
    {
        public IntegrationPartnerClientDTO IntegrationPartnerClientDTO { get; set; }
        public string sp()
        {
            return "backend_get_integration_clients_by_partner_client_id";
        }

        public string outputID = "";

        public MySqlParameter[] Parameters()
        {
            List<MySqlParameter> _params = new List<MySqlParameter>();

            _params.Add(new MySqlParameter("@partner_client_id", IntegrationPartnerClientDTO.partner_client_id));

            if (!string.IsNullOrEmpty(this.outputID)) _params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.Output;
            return _params.ToArray();
        }
    }

    public class ParamGetIntegrationPartners : IBackendDataParamObject
    {
        public List<IntegrationPartner> IntegrationPartners { get; set; }
        public string sp()
        {
            return "backend_get_integration_partners";
        }

        public string outputID = "";

        public MySqlParameter[] Parameters()
        {
            List<MySqlParameter> _params = new List<MySqlParameter>();

            if (!string.IsNullOrEmpty(this.outputID)) _params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.Output;
            return _params.ToArray();
        }
    }

    public class ParamGetIntegrationPartnerClientByClientAndDepartmentId : IBackendDataParamObject
    {
        public int client_id { get; set; }
        public int client_department_id { get; set; }
        public IntegrationPartner IntegrationPartner { get; set; }
        public string sp()
        {
            return "backend_get_integration_clients_by_client_and_dept_id";
        }

        public string outputID = "";

        public MySqlParameter[] Parameters()
        {
            List<MySqlParameter> _params = new List<MySqlParameter>();

            _params.Add(new MySqlParameter("@client_department_id", this.client_department_id));
            _params.Add(new MySqlParameter("@client_id", this.client_id));

            if (!string.IsNullOrEmpty(this.outputID)) _params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.Output;
            return _params.ToArray();
        }
    }

    public class ParamSetIntegrationPartners : IBackendDataParamObject
    {
        public IntegrationPartner IntegrationPartner { get; set; }
        public string sp()
        {
            return "backend_set_integration_partners";
        }

        public string outputID = "@backend_integration_partner_id";

        public MySqlParameter[] Parameters()
        {

            List<MySqlParameter> _params = new List<MySqlParameter>();
            _params.Add(new MySqlParameter("@backend_integration_partner_id", this.IntegrationPartner.backend_integration_partner_id));
            _params.Add(new MySqlParameter("@partner_name", this.IntegrationPartner.partner_name));
            _params.Add(new MySqlParameter("@partner_key", this.IntegrationPartner.partner_key));
            _params.Add(new MySqlParameter("@active", this.IntegrationPartner.active));
            //_params.Add(new MySqlParameter("@require_login", this.IntegrationPartner.require_login));
            _params.Add(new MySqlParameter("@partner_crypto", this.IntegrationPartner.partner_crypto));
            _params.Add(new MySqlParameter("@backend_integration_partners_pidtype", this.IntegrationPartner.backend_integration_partners_pidtype));
            _params.Add(new MySqlParameter("@last_modified_by", this.IntegrationPartner.last_modified_by));
            _params.Add(new MySqlParameter("@html_instructions", Encoding.ASCII.GetBytes(this.IntegrationPartner.html_instructions)));
            _params.Add(new MySqlParameter("@login_url", Encoding.ASCII.GetBytes(this.IntegrationPartner.login_url)));

            _params.Add(new MySqlParameter("@partner_push", this.IntegrationPartner.partner_push));
            _params.Add(new MySqlParameter("@partner_push_host", this.IntegrationPartner.partner_push_host));
            _params.Add(new MySqlParameter("@partner_push_username", this.IntegrationPartner.partner_push_username));
            _params.Add(new MySqlParameter("@partner_push_password", this.IntegrationPartner.partner_push_password));
            _params.Add(new MySqlParameter("@partner_push_port", this.IntegrationPartner.partner_push_port));
            _params.Add(new MySqlParameter("@partner_push_path", this.IntegrationPartner.partner_push_path));

            if (!string.IsNullOrEmpty(this.outputID)) _params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.InputOutput;
            return _params.ToArray();
        }
    }

    public class ParamGetIntegrationPartnerDonorsAndDocuments : IBackendDataParamObject
    {
        public ApiIntegrationFilter apiIntegrationFilter { get; set; }
        public int backend_integration_partner_id { get; set; } = 0;

        public bool WithBase64 { get; set; } = false;
        public string sp()
        {
            return "BACKEND_GET_PARTNER_DONORS_AND_DOCUMENTS";
        }

        public string outputID = "";

        public MySqlParameter[] Parameters()
        {


            //public DateTime fromDateTime { get; set; }
            //public DateTime toDateTime { get; set; }
            //public int maxResults { get; set; } = 0;
            ////        public bool assendingResults { get; set; } = false;
            //public string partner_client_id { get; set; }
            //public string partner_client_code { get; set; }
            //public string partner_donor_id { get; set; }

            //in fromDateTime timestamp,
            //in toDateTime timestamp,
            //in partner_client_id nvarchar(255),
            //in partner_client_code nvarchar(255),
            //in partner_donor_id nvarchar(255),
            //in maxResults int  backend_integration_partner_id
            
            if (this.apiIntegrationFilter.fromDateTime==DateTime.MinValue)
            {
                this.apiIntegrationFilter.fromDateTime = DateTime.Now.AddYears(-10);
            }

            if (this.apiIntegrationFilter.toDateTime == DateTime.MinValue)
            {
                this.apiIntegrationFilter.toDateTime = DateTime.Now.AddDays(1);
            }

            List<MySqlParameter> _params = new List<MySqlParameter>();
            _params.Add(new MySqlParameter("@backend_integration_partner_id", this.backend_integration_partner_id));
            _params.Add(new MySqlParameter("@fromDateTime", this.apiIntegrationFilter.fromDateTime));
            _params.Add(new MySqlParameter("@toDateTime", this.apiIntegrationFilter.toDateTime));
            _params.Add(new MySqlParameter("@partner_client_id", this.apiIntegrationFilter.partner_client_id));
            _params.Add(new MySqlParameter("@partner_client_code", this.apiIntegrationFilter.partner_client_code));
            _params.Add(new MySqlParameter("@partner_donor_id", this.apiIntegrationFilter.partner_donor_id));
            _params.Add(new MySqlParameter("@maxResults", this.apiIntegrationFilter.maxResults));

            if (!string.IsNullOrEmpty(this.outputID)) _params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.InputOutput;
            return _params.ToArray();
        }
    }

    public class ParamGetDonorMatch : IBackendDataParamObject
    {
        public ApiIntegrationMatch apiIntegrationMatch { get; set; }
        public int backend_integration_partner_id { get; set; } = 0;

        public string sp()
        {
            return "BACKEND_GET_DONOR_MATCHES";
        }

        public string outputID = "";

        public MySqlParameter[] Parameters()
        {
//            CREATE PROCEDURE surpathlive.`BACKEND_GET_DONOR_MATCHES`(
//   IN backend_integration_partner_id   int,
//   IN donor_first_name NVARCHAR(255),
//   IN donor_last_name                  NVARCHAR(255),
//   IN PARTNER_DONOR_ID                 NVARCHAR(255),
//   IN partner_client_code              NVARCHAR(255),
//   IN partner_client_name              NVARCHAR(255),
//   IN MAXRESULTS                       INT)
//BEGIN

            List<MySqlParameter> _params = new List<MySqlParameter>();
            _params.Add(new MySqlParameter("@backend_integration_partner_id", this.backend_integration_partner_id));
            _params.Add(new MySqlParameter("@partner_client_id", this.apiIntegrationMatch.partner_client_id));
            _params.Add(new MySqlParameter("@partner_client_code", this.apiIntegrationMatch.partner_client_code));
            _params.Add(new MySqlParameter("@partner_client_name", this.apiIntegrationMatch.partner_client_name));
            _params.Add(new MySqlParameter("@partner_donor_id", this.apiIntegrationMatch.partner_donor_id));
            _params.Add(new MySqlParameter("@donor_first_name", this.apiIntegrationMatch.donor_first_name));
            _params.Add(new MySqlParameter("@donor_last_name", this.apiIntegrationMatch.donor_last_name));
            _params.Add(new MySqlParameter("@maxResults", this.apiIntegrationMatch.maxResults));

            if (!string.IsNullOrEmpty(this.outputID)) _params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.InputOutput;
            return _params.ToArray();
        }
    }

    public class ParamSetDonorPidValidated : IBackendDataParamObject
    {
        public ApiIntegrationMatch apiIntegrationMatch { get; set; }
        public int backend_integration_partner_id { get; set; } = 0;

        public string sp()
        {
            return "backend_set_donor_pid_validated";
        }

        public string outputID = "";

        public MySqlParameter[] Parameters()
        {
            //            CREATE PROCEDURE surpathlive.`BACKEND_GET_DONOR_MATCHES`(
            //   IN backend_integration_partner_id   int,
            //   IN donor_first_name NVARCHAR(255),
            //   IN donor_last_name                  NVARCHAR(255),
            //   IN PARTNER_DONOR_ID                 NVARCHAR(255),
            //   IN partner_client_code              NVARCHAR(255),
            //   IN partner_client_name              NVARCHAR(255),
            //   IN MAXRESULTS                       INT)
            //BEGIN

            List<MySqlParameter> _params = new List<MySqlParameter>();
            _params.Add(new MySqlParameter("@backend_integration_partner_id", this.backend_integration_partner_id));
            _params.Add(new MySqlParameter("@partner_client_id", this.apiIntegrationMatch.partner_client_id));
            _params.Add(new MySqlParameter("@partner_client_code", this.apiIntegrationMatch.partner_client_code));
            _params.Add(new MySqlParameter("@partner_client_name", this.apiIntegrationMatch.partner_client_name));
            _params.Add(new MySqlParameter("@partner_donor_id", this.apiIntegrationMatch.partner_donor_id));
            _params.Add(new MySqlParameter("@donor_first_name", this.apiIntegrationMatch.donor_first_name));
            _params.Add(new MySqlParameter("@donor_last_name", this.apiIntegrationMatch.donor_last_name));
            _params.Add(new MySqlParameter("@maxResults", this.apiIntegrationMatch.maxResults));

            if (!string.IsNullOrEmpty(this.outputID)) _params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.InputOutput;
            return _params.ToArray();
        }
    }


    public class ParamGetIntegrationPartnerRelease : IBackendDataParamObject
    {
        // public IntegrationPartnerRelease integrationPartnerRelease { get; set; }
        public string partner_key { get; set; }
        public string partner_client_code { get; set; }
        public int released_only { get; set; } = 1;
        public int sent { get; set; } = 0;
        public string sp()
        {
            return "backend_get_partner_release";
        }

        public string outputID = "";

        public MySqlParameter[] Parameters()
        {

            //          IN partner_key                                    varchar(200),
            //IN partner_client_code                           varchar(200),
            //IN released
            
            List<MySqlParameter> _params = new List<MySqlParameter>();
            _params.Add(new MySqlParameter("@partner_key", this.partner_key));
            _params.Add(new MySqlParameter("@partner_client_code", this.partner_client_code));
            _params.Add(new MySqlParameter("@released_only", this.released_only));
            _params.Add(new MySqlParameter("@sent", this.sent));

            if (!string.IsNullOrEmpty(this.outputID)) _params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.InputOutput;
            return _params.ToArray();
        }
    }

    public class ParamSetIntegrationPartnerRelease : IBackendDataParamObject
    {
        public IntegrationPartnerRelease integrationPartnerRelease { get; set; }
        public string last_modified_by { get; set; }
        public string released_by { get; set; }

        public string sp()
        {
            return "backend_set_partner_release";
        }

        public string outputID = "";

        public MySqlParameter[] Parameters()
        {
            List<MySqlParameter> _params = new List<MySqlParameter>();
            _params.Add(new MySqlParameter("@backend_integration_partner_release_id", this.integrationPartnerRelease.backend_integration_partner_release_id));
            _params.Add(new MySqlParameter("@released", this.integrationPartnerRelease.released));
            _params.Add(new MySqlParameter("@sent", this.integrationPartnerRelease.sent));
            _params.Add(new MySqlParameter("@last_modified_by", this.last_modified_by));
            _params.Add(new MySqlParameter("@released_by", this.released_by));

            if (!string.IsNullOrEmpty(this.outputID)) _params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.InputOutput;
            return _params.ToArray();
        }
    }



    #endregion Integration


    #region OverRides
    public class ParamGetOverRideList : IBackendDataParamObject
    {
        public IntegrationPartnerRelease integrationPartnerRelease { get; set; }
        public string last_modified_by { get; set; }
        public string released_by { get; set; }

        public string sp()
        {
            return "";
        }
        
        public string outputID = "";

        public MySqlParameter[] Parameters()
        {
            List<MySqlParameter> _params = new List<MySqlParameter>();
           
            if (!string.IsNullOrEmpty(this.outputID)) _params.Where(x => x.ParameterName == this.outputID).First().Direction = System.Data.ParameterDirection.InputOutput;
            return _params.ToArray();
        }
    }


    #endregion OverRides
    #endregion params
}