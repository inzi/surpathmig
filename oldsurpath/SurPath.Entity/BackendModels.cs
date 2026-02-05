using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;

namespace SurPath.Entity
{
    public class BackendFile
    {
        public int file_id { get; set; }
        public DateTime file_update_time { get; set; }
        public MemoryStream file_content { get; set; }
        public int file_size { get; set; }
        public string file_content_type { get; set; }
        public string file_name { get; set; }
    }

    public class ExceptionCounts
    {
        public int ExceptionCount { get; set; } = 0;
        public int clinic_exception_count { get; set; } = 0;
        public int sms_count { get; set; } = 0;
        public int sis_count { get; set; } = 0;
        public int did_count { get; set; } = 0;
        public int ffo_count { get; set; } = 0;
    }

    public class SMSActivity
    {
        public int backend_sms_activity_id { get; set; }
        public string created_by { get; set; } = "SYSTEM";
        public int donor_test_info_id { get; set; }
        public DateTime? dt_reply_received { get; set; }
        public DateTime? dt_sms_sent { get; set; }
        public string last_modified_by { get; set; } = "SYSTEM";
        public DateTime last_modified_on { get; set; }
        public DateTime created_on { get; set; }
        public string reply_text { get; set; }
        public bool reply_read { get; set; }
        public DateTime? reply_read_timestamp { get; set; }
        public string sent_text { get; set; }
        public int user_id { get; set; }
        public string auto_reply_text { get; set; } = string.Empty;

        //public int id { get; set; }
        //public string to { get; set; }

        //public string from { get; set; }
        //public string apikey { get; set; }
        //public string text { get; set; }
        //public bool sent { get; set; } = false;
        //public DateTime dt_entered { get; set; }
        //public DateTime dt_sent { get; set; }
    }

    public class PDFEngineSettings
    {
        public NameValueCollection Settings { get; set; } = new NameValueCollection();

        public string Json(bool handleMultipleValuesPerKey = false)
        {
            // Note - NameValueCollections allow multiple keys
            var result = new Dictionary<string, object>();
            foreach (string key in this.Settings.Keys)
            {
                if (handleMultipleValuesPerKey)
                {
                    string[] values = this.Settings.GetValues(key);
                    if (values.Length == 1)
                    {
                        result.Add(key, HttpUtility.UrlEncode(values[0]));
                    }
                    else
                    {
                        result.Add(key, HttpUtility.UrlEncode(values.ToString()));
                    }
                }
                else
                {
                    result.Add(key, this.Settings[key]);
                }
            }
            string json = new JavaScriptSerializer().Serialize(result);
            return json;
        }

        public void FromJson(string json)
        {
            Settings = new NameValueCollection();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            Dictionary<string, object> _settings = (Dictionary<string, object>)json_serializer.DeserializeObject(json);

            foreach (string key in _settings.Keys)
            {
                string[] values = _settings[key].ToString().Split(',');
                if (values.Length > 1)
                {
                    foreach (var _val in values)
                    {
                        Settings.Add(key, System.Web.HttpUtility.UrlDecode(_val));
                    }
                }
                else
                {
                    Settings.Add(key, values[0]);
                }
            }
        }
    }

    public static class AppWideSettings
    {
    }

    [Serializable]
    public class PDFRenderClinicElement
    {
        public string Text { get; set; }
        public string FontName { get; set; } = "Verdana";
        public int FontSize { get; set; } = 12;
        public bool Bold { get; set; } = false;
        public int TextColorRed { get; set; } = 0;
        public int TextColorGreen { get; set; } = 0;
        public int TextColorBlue { get; set; } = 0;
        public bool DrawBox { get; set; } = false;
        public bool DrawBoxSolid { get; set; } = false;
        public int BoxPadding { get; set; } = 6;
        public int BoxColorRed { get; set; } = 0;
        public int BoxColorGreen { get; set; } = 0;
        public int BoxColorBlue { get; set; } = 0;
        public int BoxAlpha { get; set; } = 128;
        public int Top { get; set; }
        public int Left { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    [Serializable]
    public class PDFRenderElement
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public string FontName { get; set; } = "Verdana";
        public int FontSize { get; set; } = 12;
        public bool Bold { get; set; } = false;
        public int TextColorRed { get; set; } = 0;
        public int TextColorGreen { get; set; } = 0;
        public int TextColorBlue { get; set; } = 0;
        public bool DrawBox { get; set; } = false;
        public bool DrawBoxSolid { get; set; } = false;
        public int BoxPadding { get; set; } = 6;
        public int BoxColorRed { get; set; } = 0;
        public int BoxColorGreen { get; set; } = 0;
        public int BoxColorBlue { get; set; } = 0;
        public int BoxAlpha { get; set; } = 128;
        public int Top { get; set; }
        public int Left { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    [Serializable]
    public class PdfRenderSettings
    {
        public List<PDFRenderElement> Elements { get; set; } = new List<PDFRenderElement>();
        public PDFRenderClinicElement ClinicDefaultElement { get; set; } = new PDFRenderClinicElement();
        public string TemplateFileName { get; set; }
        public int CollectionSiteNumberOfColumns { get; set; } = 2;
        public int Max_Clinic_Distance { get; set; } = 30;
        public string Specimen_Referral { get; set; } = String.Empty;
        public int Default_Reason_Code { get; set; } = 2;
        public string MROName { get; set; }
        public string ASAPText { get; set; } = String.Empty;
        public string ExpirationDateTimeOrNote { get; set; } = String.Empty;
        public Dictionary<string, string> Service_Test_Mapping { get; set; }
    }

    /// <summary>
    /// This class represents the backend_notifications table
    /// </summary>
    public class Notification
    {
        public int backend_notifications_id { get; set; }
        public string created_by { get; set; } = "SYSTEM";
        public DateTime created_on { get; set; }
        public int donor_test_info_id { get; set; }
        public string last_modified_by { get; set; } = "SYSTEM";
        public DateTime last_modified_on { get; set; }
        public bool notified_by_email { get; set; }
        public bool notified_by_sms { get; set; }
        public int clinic_exception { get; set; }
        public bool is_archived { get; set; }
        public bool notify_now { get; set; } = false;
        public bool notify_again { get; set; } = false;
        public bool notify_manual { get; set; } = false; // 
        public bool force_db { get; set; } = false; // ignore third party, use local clinic database
        public bool notify_next_window { get; set; } = false;
        public int notification_email_exception { get; set; }
        public int notification_sms_exception { get; set; }
        public string notification_sent_to_email { get; set; }
        public string notification_sent_to_phone { get; set; }
        public int clinic_radius { get; set; } = 0;
        public int client_id { get; set; } = 0;
        public int client_department_id { get; set; } = 0;

        public DateTime? notified_by_email_timestamp { get; set; }
        public DateTime? notified_by_sms_timestamp { get; set; }
        public DateTime? notify_email_exception_timestamp { get; set; }
        public DateTime? notify_sms_exception_timestamp { get; set; }
        public DateTime? clinic_exception_timestamp { get; set; }
        public DateTime? notify_after_timestamp { get; set; }
        public DateTime? notify_before_timestamp { get; set; }

        public bool in_window { get; set; } = false;
        public bool notify_reset_sendin { get; set; } = false;

    }

    [Serializable]
    public class PIDTypeValue
    {
        public int PIDType { get; set; }
        public string PIDValue { get; set; }
        public bool required { get; set; } = false;
        public string Err { get; set; } = string.Empty;
        public bool isReadOnly {get;set;} = false;
        public bool validated { get; set; } = false;
        public bool mask { get; set; } = false;
    }


    // To serialize the settings - see https://stackoverflow.com/questions/7003740/how-to-convert-namevaluecollection-to-json-string

    ///  The example from SO

    //public class StackOverflow_7003740
    //{
    //    static Dictionary<string, object> NvcToDictionary(NameValueCollection nvc, bool handleMultipleValuesPerKey)
    //    {
    //        var result = new Dictionary<string, object>();
    //        foreach (string key in nvc.Keys)
    //        {
    //            if (handleMultipleValuesPerKey)
    //            {
    //                string[] values = nvc.GetValues(key);
    //                if (values.Length == 1)
    //                {
    //                    result.Add(key, values[0]);
    //                }
    //                else
    //                {
    //                    result.Add(key, values);
    //                }
    //            }
    //            else
    //            {
    //                result.Add(key, nvc[key]);
    //            }
    //        }

    //        return result;
    //    }

    //    public static void Test()
    //    {
    //        NameValueCollection nvc = new NameValueCollection();
    //        nvc.Add("foo", "bar");
    //        nvc.Add("multiple", "first");
    //        nvc.Add("multiple", "second");

    //        foreach (var handleMultipleValuesPerKey in new bool[] { false, true })
    //        {
    //            if (handleMultipleValuesPerKey)
    //            {
    //                Console.WriteLine("Using special handling for multiple values per key");
    //            }
    //            var dict = NvcToDictionary(nvc, handleMultipleValuesPerKey);
    //            string json = new JavaScriptSerializer().Serialize(dict);
    //            Console.WriteLine(json);
    //            Console.WriteLine();
    //        }
    //    }
    //}
}