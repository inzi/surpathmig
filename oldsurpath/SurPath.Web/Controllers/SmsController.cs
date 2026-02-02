using SurPath.Data;
using SurPath.Entity;
using SurpathBackend;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Twilio.AspNet.Common;
using Twilio.AspNet.Mvc;
using Twilio.TwiML;
using SurPath.Business;
using SurPath.Enum;
using MySql.Data.MySqlClient;
using Serilog;

namespace SurPathWeb.Controllers
{
    public class SmsController : TwilioController
    {
        public BackendData d = new BackendData();
        public BackendLogic backendLogic = new BackendLogic("SYSTEM", MvcApplication._logger);

        private static ILogger _logger = MvcApplication._logger;


        public TwiMLResult ReplyHandler(SmsRequest incomingMessage)
        {
            _logger.Debug("ReplyHandler");

            MessagingResponse messagingResponse = new MessagingResponse();
            // Get the default response
            string sent_text = "Thank you for your message. Please visit https://surpath.com for details.";
            if (ConfigurationManager.AppSettings["DefaultSMSResponse"] != null)
            {
                sent_text = ConfigurationManager.AppSettings["DefaultSMSResponse"].ToString().Trim();

            }
            try
            {

                // get the text & phone number that was sent to us
                string reply_text = incomingMessage.Body;
                string phone_no = incomingMessage.From;
                _logger.Debug($"Inbound From: {incomingMessage.From} Msg: {incomingMessage.Body}");
                if (!string.IsNullOrEmpty(phone_no) || !string.IsNullOrEmpty(reply_text))
                {
 
                    // get the client response, with donor id, 
                    // from the phone number, identify the donor, and the auto response
                    ParamGetClientNotificationDataSettingsByPhone p = new ParamGetClientNotificationDataSettingsByPhone() { phone_number = phone_no };
                    // Clean up the phone number, get rid of +1
                    if (p.phone_number.Length > 10)
                    {
                        p.phone_number = p.phone_number.Substring(p.phone_number.Length - 10);
                    }

                    // get the donor_test_info_id if there is one.
                    ParamGeneric paramGeneric = new ParamGeneric()
                    {
                        StoreProc = "backend_get_donor_info_id_by_phone",
                        outputID = "@donor_test_info_id"
                    };
                    paramGeneric.AddParam("@phone_number", p.phone_number);

                    paramGeneric.AddParam("@donor_test_info_id", MySqlDbType.Int32);

                    int _donor_test_info_id = (int)d.GetGenericObject(paramGeneric);

                    // if get back a zero, there's no donor_test_info or donor with this phone number
                    if (_donor_test_info_id < 1)
                    {
                        messagingResponse.Message(sent_text);
                    }
                    else
                    {
                        ClientNotificationDataSettings clientNotificationDataSettings = d.GetClientNotificationDataSettingsByPhone(p);
                        // get the conversation for this donor test info id
                        List<SMSActivity> smsConversation = backendLogic.GetSMSActivity(_donor_test_info_id);
                        // Create activity record
                        SMSActivity latestSMS = new SMSActivity();
                        // Get the latest SMS activity if it exists
                        if (smsConversation.Count > 0) latestSMS = smsConversation.First();
                        // If the last activity had been read / replied to, etc, we need to create a new one.
                        if (latestSMS.reply_read) latestSMS = new SMSActivity();

                        // if we have a new activity, populate it
                        if (latestSMS.backend_sms_activity_id == 0)
                        {
                            latestSMS.donor_test_info_id = _donor_test_info_id;
                            latestSMS.reply_text = reply_text;
                            latestSMS.user_id = 0;
                            latestSMS.dt_reply_received = DateTime.Now;
                        }

                        // Construct what we'll send back
                        if (!string.IsNullOrEmpty(clientNotificationDataSettings.client_autoresponse))
                        {
                            //messagingResponse.Message(s.reply);
                            sent_text = (clientNotificationDataSettings.client_autoresponse);
                        }

                        // Construct the response
                        messagingResponse.Message(sent_text);
                        backendLogic.SetDonorActivity(_donor_test_info_id, (int)DonorActivityCategories.Notification, $"Auto Reply Sent:  {sent_text}", 0);

                        // if this were an actual reply, we'd populate reply
                        // but since it's automated, we won't
                        // leaving code here in case we want to get these every time
                        //latestSMS.sent_text = sent_text;
                        //latestSMS.dt_sms_sent = DateTime.Now;

                        // Send to the database
                        d.SetSMSActivity(new ParamSetSMSActivity() { smsActivity = latestSMS });
                        // Log activity
                        d.SetDonorActivity(new ParamSetDonorActivity() { donor_test_info_id = _donor_test_info_id, activity_category_id = (int)DonorActivityCategories.Notification, activity_note = "SMS received - " + reply_text, activity_user_id = 0 });
                        backendLogic.SetDonorActivity(_donor_test_info_id, (int)DonorActivityCategories.Notification, $"SMS received:  {reply_text}", 0);
                    }
                    // Send the response to twillio

                    
                }


            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                //throw;                   
                Response.StatusCode = 500;
                Response.End();

            }
            return TwiML(messagingResponse);

        }
    }
}