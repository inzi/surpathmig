using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace SurPath.Entity
{
    #region SpecialPurpose
    /// <summary>
    /// This class is used by the exception report when doing send ins
    /// When locations are found they could be via 3rdParty (formfox, for example)
    /// </summary>
    public class ExceptionSendIn
    {
        public int donor_test_info_id { get; set; }
        public bool use_formfox { get; set; }
        public bool force_db { get; set; }
        public int range { get; set; }
    }
    #endregion SpecialPurpose


    #region Models

    //public class ClientAutoResponse
    //{
    //    public int backend_sms_autoresponses_id { get; set; }
    //    public int client_id { get; set; }
    //    public int client_sms_from_id { get; set; }
    //    public string client_sms_apikey { get; set; }
    //    public string client_sms_token { get; set; }
    //    public string reply { get; set; }
    //    public DateTime created_on { get; set; }
    //    public string created_by { get; set; } = "SYSTEM";
    //    public DateTime last_modified_on { get; set; }
    //    public string last_modified_by { get; set; } = "SYSTEM";
    //}

    public class UnsentSMS
    {
        public int backend_notifications_id { get; set; }
        public int donor_test_info_id { get; set; }
        public int donor_id { get; set; }
        public string donor_phone_1 { get; set; }
        public string donor_phone_2 { get; set; }
        public string client_sms_from_id { get; set; }
        public string client_sms_text { get; set; }
        public bool notified_by_sms { get; set; }
        public int client_id { get; set; }
        public int client_department_id { get; set; }
        public string client_sms_apikey { get; set; }
        public string client_sms_token { get; set; }
        public DateTime created_on { get; set; }
        public DateTime notified_by_sms_timestamp { get; set; }
    }

    public class CollectionFacility
    {
        public bool is_synchronized { get; set; }
        public int vendor_address_id { get; set; }
        public int vendor_id { get; set; }
        public int address_type_id { get; set; }
        public DateTime created_on { get; set; }
        public DateTime last_modified_on { get; set; }
        public string vendor_name { get; set; }
        public string vendor_zip { get; set; }
        public string vendor_state { get; set; }
        public string vendor_phone { get; set; }
        public string vendor_fax { get; set; }
        public string created_by { get; set; }
        public string last_modified_by { get; set; }
        public string vendor_address_1 { get; set; }
        public string vendor_address_2 { get; set; }
        public string vendor_city { get; set; }
        public string vendor_email { get; set; }
        public float d2c { get; set; }
    }

    public class ClientNotificationDataSettingsDay
    {
        public int DayOfWeek { get; set; }
        public bool Enabled { get; set; } = false;
        public int send_time_start_seconds_from_midnight { get; set; } = 0;
        public int send_time_stop_seconds_from_midnight { get; set; } = 0;
    }

    public class ClientNotificationDataSettings
    {
        public ClientNotificationDataSettings()
        {
            this.DaySettings = new List<ClientNotificationDataSettingsDay>();
            foreach (int dayInt in System.Enum.GetValues(typeof(DayOfWeekEnum)))
            {
                this.DaySettings.Add(new ClientNotificationDataSettingsDay()
                {
                    DayOfWeek = dayInt
                });
            }
        }

        public int backend_notification_window_data_id { get; set; }
        public int client_id { get; set; }
        public int client_department_id { get; set; }
        public string pdf_template_filename { get; set; }
        public string pdf_render_settings_filename { get; set; }
        public bool enable_sms { get; set; } = false;
        public bool force_manual { get; set; } = true;
        public bool use_formfox { get; set; } = false;
        public bool onsite_testing { get; set; } = false;
        public DateTime created_on { get; set; }
        public string created_by { get; set; } = "SYSTEM";
        public List<ClientNotificationDataSettingsDay> DaySettings { get; set; }

        //public bool sunday { get; set; }
        //public int sunday_send_time_start_seconds_from_midnight { get; set; }
        //public int sunday_send_time_stop_seconds_from_midnight { get; set; }
        //public bool monday { get; set; }
        //public int monday_send_time_start_seconds_from_midnight { get; set; }
        //public int monday_send_time_stop_seconds_from_midnight { get; set; }
        //public bool tuesday { get; set; }
        //public int tuesday_send_time_start_seconds_from_midnight { get; set; }
        //public int tuesday_send_time_stop_seconds_from_midnight { get; set; }
        //public bool wednesday { get; set; }
        //public int wednesday_send_time_start_seconds_from_midnight { get; set; }
        //public int wednesday_send_time_stop_seconds_from_midnight { get; set; }
        //public bool thursday { get; set; }
        //public int thursday_send_time_start_seconds_from_midnight { get; set; }
        //public int thursday_send_time_stop_seconds_from_midnight { get; set; }
        //public bool friday { get; set; }
        //public int friday_send_time_start_seconds_from_midnight { get; set; }
        //public int friday_send_time_stop_seconds_from_midnight { get; set; }
        //public bool saturday { get; set; }
        //public int saturday_send_time_start_seconds_from_midnight { get; set; }
        //public int saturday_send_time_stop_seconds_from_midnight { get; set; }
        public int delay_in_hours { get; set; } = 0;

        public bool send_asap { get; set; } = true;
        public int deadline_alert_in_days { get; set; } = 7;
        public int max_sendins { get; set; } = 5;
        public bool override_day_schedule { get; set; } = false;
        public DateTime last_modified_on { get; set; }

        public DateTime? notification_start_date { get; set; }
        public DateTime? notification_stop_date { get; set; }
        public DateTime? notification_sweep_date { get; set; }
        public string last_modified_by { get; set; } = "SYSTEM";

        public string client_name { get; set; } = string.Empty;
        public string department_name { get; set; } = string.Empty;

        public string client_sms_token { get; set; } = string.Empty;
        public string client_sms_apikey { get; set; } = string.Empty;
        public string client_autoresponse { get; set; } = string.Empty;
        public string client_sms_from_number { get; set; } = string.Empty;

        public bool show_web_notify_button { get; set; } = false;
    }

    public class BWDOW
    {
        private int _bwDOW = 0b0000000;
        private int _sunday = 1;
        private int _monday = 2;
        private int _tuesday = 4;
        private int _wednesday = 8;
        private int _thursday = 16;
        private int _friday = 32;
        private int _saturday = 64;

        public int setDOW
        {
            get
            {
                return this._bwDOW;
            }
            set
            {
                this._bwDOW = value;
            }
        }

        private bool isDaySet(int _val)
        {
            return (this._bwDOW & _val) > 0;
        }

        private void SetDay(int _val, bool _enabled)
        {
            if (_enabled == ((this._bwDOW & _val) > 0))
            {
                // bit already set bail
                return;
            }
            if (_enabled)
            {
                // We want the bit set
                this._bwDOW = this._bwDOW + _val;
            }
            else
            {
                // We do NOT want the bit set
                this._bwDOW = this._bwDOW - _val;
            }
        }

        public bool Monday
        {
            get
            {
                return isDaySet(this._monday);
            }
            set
            {
                SetDay(this._monday, value);
            }
        }

        public bool Tuesday
        {
            get
            {
                return isDaySet(this._tuesday);
            }
            set
            {
                SetDay(this._tuesday, value);
            }
        }

        public bool Wednesday
        {
            get
            {
                return isDaySet(this._wednesday);
            }
            set
            {
                SetDay(this._wednesday, value);
            }
        }

        public bool Thursday
        {
            get
            {
                return isDaySet(this._thursday);
            }
            set
            {
                SetDay(this._thursday, value);
            }
        }

        public bool Friday
        {
            get
            {
                return isDaySet(this._friday);
            }
            set
            {
                SetDay(this._friday, value);
            }
        }

        public bool Saturday
        {
            get
            {
                return isDaySet(this._saturday);
            }
            set
            {
                SetDay(this._saturday, value);
            }
        }

        public bool Sunday
        {
            get
            {
                return isDaySet(this._sunday);
            }
            set
            {
                SetDay(this._sunday, value);
            }
        }

        public bool Weekdays
        {
            get
            {
                return (
                    isDaySet(this._monday) &&
                    isDaySet(this._tuesday) &&
                    isDaySet(this._wednesday) &&
                    isDaySet(this._thursday) &&
                    isDaySet(this._friday) &&
                    true
                    );
            }
            set
            {
                SetDay(this._monday, value);
                SetDay(this._tuesday, value);
                SetDay(this._wednesday, value);
                SetDay(this._thursday, value);
                SetDay(this._friday, value);
            }
        }

        public bool Weekends
        {
            get
            {
                return (
                    isDaySet(this._saturday) &&
                    isDaySet(this._sunday) &&
                    true
                    );
            }
            set
            {
                SetDay(this._sunday, value);
                SetDay(this._saturday, value);
            }
        }
    }

    public class DonorActivity
    {
        public int donor_test_info_id { get; set; }
        public int activity_user_id { get; set; }
        public int activity_category_id { get; set; }
        public bool is_activity_visible { get; set; } = true;
        public bool is_synchronized { get; set; } = true;
        public string activity_note { get; set; }
    }

    public class UserActivity
    {
        public int user_activity_id { get; set; }
        public int activity_user_id { get; set; }
        public int activity_user_category_id { get; set; }
        public DateTime activity_datetime { get; set; }
        public bool is_activity_visible { get; set; } = true;
        public bool is_synchronized { get; set; } = false;
        public string activity_note { get; set; }
    }

    /// <summary>
    /// This class is used by the PDF & SMS engine to render and send a notification
    /// </summary>
    public class NotificationInformation
    {
        public int backend_notifications_id { get; set; } = 0;
        public int backend_notification_window_data_id { get; set; } = 0;
        public int donor_test_info_id { get; set; }
        public int client_id { get; set; }
        public int client_department_id { get; set; }
        public int donor_id { get; set; }
        public DateTime test_requested_date { get; set; }
        public string client_name { get; set; }
        public string department_name { get; set; }
        public string lab_code { get; set; }
        public int test_panel_id { get; set; }
        public string test_panel_name { get; set; }
        public string donor_phone_1 { get; set; }
        public string donor_phone_2 { get; set; }
        public string donor_zip { get; set; }
        public string donor_email { get; set; }
        public int test_category_id { get; set; }
        public string test_category_name { get; set; }
        public int reason_for_test_id { get; set; } = 2;
        public string other_reaon { get; set; }
        public bool send_asap { get; set; }
        public int delay_in_hours { get; set; }
        public int deadline_alert_in_days { get; set; }
        public bool force_manual { get; set; } = true;
        public bool in_window { get; set; } = false;
        public string donor_first_name { get; set; } = string.Empty;
        public string donor_last_name { get; set; } = string.Empty;
        public string donor_full_name { get; set; } = string.Empty;

        public bool notify_again { get; set; } = false;
    }

    public class ClientsWithSettings
    {
        public int backend_notification_window_data_id { get; set; }
        public string name { get; set; }
    }

    public class ZipCodeDataRow
    {
        public int zip_id { get; set; }
        public string zip { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string county { get; set; }
        public string type { get; set; }
        public int tz_std { get; set; }
        public int tz_dst { get; set; }
    }


    #region FormFox

    public class formfoxorders
    {
        public int backend_formfox_orders_id { get; set; }
        public int donor_test_info_id { get; set; }
        public string ReferenceTestID { get; set; }
        public DateTime createdON { get; set; }
        public DateTime updatedOn { get; set; }
        public string filename { get; set; }
        public DateTime deadline { get; set; }
        public string status { get; set; }
        public string sfcode { get; set; }
        public string sampletype { get; set; }
        public string testid { get; set; }
        public string SpecimenID { get; set; }
        public bool archived { get; set; }
    }



    #endregion FormFox

    #region IntegrationPartners
    public class IntegrationPartner
    {
        public int backend_integration_partner_id { get; set; }
        public string partner_name { get; set; }
        public string partner_key { get; set; }
        public string partner_crypto { get; set; }
        public int backend_integration_partners_pidtype { get; set; }
        public string html_instructions { get; set; } = string.Empty;
        public string login_url { get; set; } = string.Empty;
        public DateTime created_on { get; set; }
        public DateTime last_modified_on { get; set; }
        public string last_modified_by { get; set; }
        public bool active { get; set; }
        public bool require_login { get; set; } = false;
        public bool require_remote_login { get; set; } = false;

        public bool partner_push { get; set; } = false;
        public string partner_push_host { get; set; }
        public string partner_push_username { get; set; }
        public string partner_push_password { get; set; }
        public string partner_push_port { get; set; }
        public string partner_push_path { get; set; }


        private string b64encode(string b)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(b);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        private string b64decode(string b = "")
        {
            if (string.IsNullOrEmpty(b) == true) return string.Empty;
            var base64EncodedBytes = System.Convert.FromBase64String(b);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public IntegrationPartnerDTO integrationPartnerDTO
        {
            get
            {
                
                return new IntegrationPartnerDTO()
                {
                    backend_integration_partner_id = this.backend_integration_partner_id,
                    partner_name = this.partner_name,
                    partner_key = this.partner_key,
                    partner_crypto = this.partner_crypto,
                    html_instructions = EmptyOrBase64(this.html_instructions), // String.IsNullOrEmpty(this.html_instructions) ? string.Empty : b64encode(this.html_instructions),
                    login_url = EmptyOrBase64(this.login_url), // String.IsNullOrEmpty(this.login_url) ? string.Empty: b64encode(this.login_url),
                    partner_push = this.partner_push,
                    partner_push_host = EmptyOrBase64(this.partner_push_host), //this.partner_push_host,
                    partner_push_password = EmptyOrBase64(this.partner_push_password), //this.partner_push_password,
                    partner_push_path = EmptyOrBase64(this.partner_push_path), //this.partner_push_path,
                    partner_push_port = EmptyOrBase64(this.partner_push_port), //this.partner_push_port,
                    partner_push_username = EmptyOrBase64(this.partner_push_username), //this.partner_push_username
                };
            }
            set
            {
                this.partner_name = value.partner_name;
                this.partner_key = value.partner_key;
                this.partner_crypto = value.partner_crypto;
                this.html_instructions = b64decode(value.html_instructions);
                this.login_url = b64decode(value.login_url);
                this.partner_push = value.partner_push;
                this.partner_push_host = b64decode(value.partner_push_host);
                this.partner_push_password = b64decode(value.partner_push_password);
                this.partner_push_path = b64decode(value.partner_push_path);
                this.partner_push_port = b64decode(value.partner_push_port);
                this.partner_push_username = b64decode(value.partner_push_username);
            }   
        }

        private string EmptyOrBase64(string val = "")
        {
            return String.IsNullOrEmpty(val) ? string.Empty : b64encode(val);
        }
    }

    public class IntegrationPartnerDTO
    {
        public int backend_integration_partner_id { get; set; }
        public string partner_name { get; set; }
        public string partner_key { get; set; }
        public string partner_crypto { get; set; }
        public string html_instructions { get; set; }
        public string login_url { get; set; }
        public bool partner_push { get; set; }
        public string partner_push_host { get; set; }
        public string partner_push_username { get; set; }
        public string partner_push_password { get; set; }
        public string partner_push_port { get; set; }
        public string partner_push_path { get; set; }
    }

    //[Serializable]
    public class IntegrationPartnerClient
    {
        public int backend_integration_partner_client_map_id { get; set; }
        public Guid backend_integration_partner_client_map_GUID { get; set; } = Guid.NewGuid();
        public int backend_integration_partner_id { get; set; } // this is OUR id for the partner
        public int client_id { get; set; } // this is OUR client ID
        public int client_department_id { get; set; } // this is OUR client dept ID
        public string client_name { get; set; } // this is OUR client name
        public string partner_push_folder { get; set; } // this optional value for ftp pushes, a drop folder. If supplied will append to partner_push_path
        public string client_department_name { get; set; } // this is OUR client department name
        public string partner_client_id { get; set; } // this is the partner's ID for the client
        public string partner_client_code { get; set; } // this is the partners code for the client
        public DateTime created_on { get; set; }
        public DateTime last_modified_on { get; set; }
        public string last_modified_by { get; set; }
        public bool active { get; set; }
        public bool require_login { get; set; } = false;
        public bool require_remote_login { get; set; } = false;

        public IntegrationPartnerClientDTO integrationPartnerClientDTO
        {
            get {
                return new IntegrationPartnerClientDTO()
                {
                    backend_integration_partner_client_map_GUID = this.backend_integration_partner_client_map_GUID,
                    client_name = this.client_name,
                    client_department_name = this.client_department_name,
                    partner_push_folder = this.partner_push_folder,
                    partner_client_code = this.partner_client_code,
                    partner_client_id = this.partner_client_id,
                    created_on = this.created_on,
                    last_modified_by = this.last_modified_by,
                    last_modified_on = this.last_modified_on
                };
            }
        }
    }
    // 

    public class IntegrationPartnerClientListHelper
    {
        public List<IntegrationPartnerClient> clients = new List<IntegrationPartnerClient>();
        private List<IntegrationPartnerClientDTO> _clientsDTO;
        public List<IntegrationPartnerClientDTO> clientsDTO
        {
            get
            {
                _clientsDTO = new List<IntegrationPartnerClientDTO>();
                clients.ForEach(c => _clientsDTO.Add(c.integrationPartnerClientDTO));
                return _clientsDTO;
            }
        }

        public string ToJson()
        {
            string json = new JavaScriptSerializer().Serialize(this.clients);
            return json;
        }

        public string ToJsonDTO()
        {
            _clientsDTO = new List<IntegrationPartnerClientDTO>();
            clients.ForEach(c => _clientsDTO.Add(c.integrationPartnerClientDTO));
            string json = new JavaScriptSerializer().Serialize(this._clientsDTO);
            return json;
        }
    }
    #endregion IntegrationPartners

    #region OverRides
    public class PaymentOverride
    {
        //`TransID` varchar(32) DEFAULT NULL,
        //`SubmitDate` datetime DEFAULT NULL,
        //`LastName` varchar(128) DEFAULT NULL,
        //`FirstName` varchar(128) DEFAULT NULL,
        //`Phone` varchar(20) DEFAULT NULL,
        //`Email` varchar(255) DEFAULT NULL,
        //`Card` varchar(20) DEFAULT NULL,
        //`PaymentMethod` varchar(128) DEFAULT NULL,
        //`PaymentAmount` varchar(20) DEFAULT NULL,
        //`SettlementDate` datetime DEFAULT NULL,
        //`SettlementAmount` varchar(20) DEFAULT NULL,
        //`Used` int (11) DEFAULT '0',
        //`DateUsed` datetime DEFAULT NULL,
        //`InvoiceNumber` varchar(20) DEFAULT NULL,
        //`TransStatus` varchar(20) DEFAULT NULL
        public string TransID { get; set; }
        public DateTime SubmitDate { get; set; }
        public string LastName { get;set;}
        public string FirstName { get;set;}
        public string Phone { get;set;}
        public string Email { get;set;}
        public string Card { get;set;}
        public string PaymentMethod { get;set;}
        public string PaymentAmount { get;set;}
        public DateTime SettlementDate { get;set;}
        public string SettlementAmount { get;set;}
        public int Used { get; set; } = 0;
        public DateTime DateUsed { get;set;}
        public string InvoiceNumber { get;set;}
        public string TransStatus { get;set;}
       
    }
    #endregion OverRides

    #endregion Models
}