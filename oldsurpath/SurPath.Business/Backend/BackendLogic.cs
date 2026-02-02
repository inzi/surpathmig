using Newtonsoft.Json;
using Serilog;
using SurPath.Business;
using SurPath.Data;
using SurPath.Entity;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SurpathBackend
{
    public class BackendLogic
    {
        public PDFengine pdfEngine { get; }
        public string Name { get; } = "BackendLogic";
        public BackendData backendData { get; }
        private static ILogger _logger;
        public BackendFiles backendFiles;

        public static string currentUserName = "SYSTEM";
        //public static int currentUserId = 0;

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

        public BackendLogic(string _currentUserName = "", ILogger __logger = null, bool backendfilessync = true)
        {
            if (__logger != null)
            {
                _logger = __logger;
                if (_logger != null) _logger.Debug($"{this.Name} logger online");

                _logger.Debug("Path to this lib: " + AssemblyDirectory);
            }
            else
            {
                _logger = new LoggerConfiguration().CreateLogger();
                _logger.Information($"BackendLogic Logger Loaded because _logger was null");
            }
            backendData = new BackendData(null, null, _logger);
            backendFiles = new BackendFiles(null, _logger, backendfilessync);

            pdfEngine = new PDFengine(_logger);
            if (!string.IsNullOrEmpty(_currentUserName)) currentUserName = _currentUserName;
        }

        public bool ClinicExceptions()
        {
            return backendData.ExceptionClinicsExist();
        }

        public int TotalExceptions()
        {
            return backendData.TotalExceptions();
        }

        public bool NotificationSetforNextWindowNotification(int donor_test_info_id)
        {
            return NotificationSetForTransmission(donor_test_info_id, true, false);
        }

        public bool NotificationSetforImmediateNotification(int donor_test_info_id)
        {
            return NotificationSetForTransmission(donor_test_info_id, false, true);
        }

        public bool NotificationSetForTransmission(int donor_test_info_id, bool notify_next_window = false, bool notify_now = false, int currentUserId = 0)
        {
            bool retval = false;
            try
            {
                ParamSetDonorNotification p2 = new ParamSetDonorNotification();
                p2.notification = backendData.GetDonorNotification(new ParamGetDonorNotification() { donor_test_info_id = donor_test_info_id });
                if (p2.notification.backend_notifications_id == 0)
                {
                    // if we get an ID of 0, it's an empty notification
                    // So we create one with the donor info id, etc.
                    // It might be for a client without notification engine settings
                    // but we'll defer to capturing that we want this donor sent in.
                    // rather than alerting the user that the client / Dept doesn't
                    // have any settings
                    p2.notification = backendData.SetDonorNotification(new ParamSetDonorNotification() { notification = new Notification() { donor_test_info_id = donor_test_info_id, created_by = currentUserName } });
                    string activity_note = $"Created donor notification record, may have been old data";
                    backendData.SetDonorActivity(new ParamSetDonorActivity() { donor_test_info_id = donor_test_info_id, activity_category_id = (int)DonorActivityCategories.Information, activity_note = activity_note, activity_user_id = currentUserId });
                }
                p2.notification.last_modified_by = currentUserName;
                p2.notification.notify_next_window = notify_next_window;
                p2.notification.notify_now = notify_now;

                // If this donor has already been notified, we need to reset the notified_by 
                if ((p2.notification.notify_next_window == true ||
                    p2.notification.notify_now == true) && p2.notification.notified_by_email==true)
                {
                    string activity_note = $"Has been sent in before - resetting sent in flags.";
                    backendData.SetDonorActivity(new ParamSetDonorActivity() { donor_test_info_id = donor_test_info_id, activity_category_id = (int)DonorActivityCategories.Notification, activity_note = activity_note, activity_user_id = currentUserId });
                    p2.notification.notified_by_email = false;
                    p2.notification.notified_by_sms = false;
                }
                Notification rN = backendData.SetDonorNotification(p2);

                if (rN.notify_next_window == p2.notification.notify_next_window ||
                    rN.notify_now == p2.notification.notify_now)
                {
                    retval = true;
                    //DonorActivityNote donorActivityNote = new DonorActivityNote();
                    string activity_note = $"Set to notify - Next window? {rN.notify_next_window.ToString()} : Notify Now? {rN.notify_now.ToString()}";
                    backendData.SetDonorActivity(new ParamSetDonorActivity() { donor_test_info_id = donor_test_info_id, activity_category_id = (int)DonorActivityCategories.Notification, activity_note = activity_note, activity_user_id = currentUserId });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retval;
        }

        public bool OkToSend(ClientNotificationDataSettings c, DateTime TestTimeRun, DateTime TestRegistrationTime)
        {
            bool retval = false;
            bool InSendWindow = false;
            bool AfterDelay = false;
            ClientNotificationDataSettingsDay thisDaySettings = c.DaySettings.Where(x => x.DayOfWeek == (int)TestTimeRun.DayOfWeek).First();
            bool DayEnabled = thisDaySettings.Enabled;
            double SecondsFromMidnight = (TestTimeRun - new DateTime(TestTimeRun.Year, TestTimeRun.Month, TestTimeRun.Day, 0, 0, 0)).TotalSeconds;
            bool InDayWindow = ((SecondsFromMidnight >= thisDaySettings.send_time_start_seconds_from_midnight) && (SecondsFromMidnight <= thisDaySettings.send_time_stop_seconds_from_midnight));

            if (DayEnabled && InDayWindow && true)
            {
                InSendWindow = true;
            }

            // Check Options

            // ASAP checked is sample is required asap
            if (c.send_asap && true) //(c.override_day_schedule && c.send_asap && true)
            {
                return true;
            }

            if (TestTimeRun >= TestRegistrationTime.AddHours(c.delay_in_hours))
            {
                AfterDelay = true;
            }

            // Options checked - determine

            if (InSendWindow && AfterDelay && DayEnabled && true)
            {
                retval = true;
            }
            return retval;
        }

        public bool NotificationSetNotified(int donor_test_info_id, bool notified_by_email = false, bool notified_by_sms = false)
        {
            bool retval = false;
            try
            {
                ParamSetDonorNotification p2 = new ParamSetDonorNotification();
                p2.notification = backendData.GetDonorNotification(new ParamGetDonorNotification() { donor_test_info_id = donor_test_info_id });
                if (p2.notification.backend_notifications_id == 0)
                {
                    p2.notification = backendData.SetDonorNotification(new ParamSetDonorNotification()
                    {
                        notification = new Notification()
                        {
                            donor_test_info_id = donor_test_info_id,
                            created_by = currentUserName,
                            notified_by_email = notified_by_email == notified_by_email,
                            notified_by_sms = notified_by_sms == notified_by_sms,
                            notified_by_email_timestamp = DateTime.Now,
                            notified_by_sms_timestamp = DateTime.Now
                        }
                    });
                }
                else
                {
                    p2.notification.notified_by_email = notified_by_email;
                    p2.notification.notified_by_sms = notified_by_sms;
                    if (notified_by_email) p2.notification.notified_by_email_timestamp = DateTime.Now;
                    if (notified_by_sms) p2.notification.notified_by_sms_timestamp = DateTime.Now;
                    backendData.SetDonorNotification(p2);
                }

                retval = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
            return retval;
        }

        public void SendError(string msg)
        {
            MailManager m = new MailManager();
            m.SendMail("chris@inzi.com", "SURPATH ERROR", msg);
        }

        public Task<bool> SendNotification(int donor_test_info_id, int activity_user_id = 0, Notification oldnotification = null)
        {
            _logger.Debug($"Notifying {donor_test_info_id}");
            bool retval = false;
            try
            {
                bool isProduction = true;
                bool.TryParse(ConfigurationManager.AppSettings["Production"].ToString().Trim(), out isProduction);
                // Get notification data record
                Notification notification = backendData.GetDonorNotification(new ParamGetDonorNotification() { donor_test_info_id = donor_test_info_id });

                // Get the notification render information
                NotificationInformation notificationInformation = backendData.GetNotificationInfoForDonorInfoId(new ParamGetNotificationInfoForDonorInfoId() { donor_test_info_id = donor_test_info_id });
                if (notificationInformation.donor_id == 0) return Task.FromResult(false);
                // Build the PDF
                ClientNotificationDataSettings c = backendData.GetClientNotificationDataSettings(notificationInformation.client_id, notificationInformation.client_department_id);

                string _pdf_template_filename = pdfEngine.DefaultTemplate;
                // if the json specifies a different pdf template use that, otherwise use the default template.
                if (!string.IsNullOrEmpty(pdfEngine.RenderSettings.TemplateFileName)) _pdf_template_filename = pdfEngine.RenderSettings.TemplateFileName;
                pdfEngine.LoadTemplate(_pdf_template_filename);

                // This builds the pdf and will throw errors if data is not found, like files, etc.
                // returns false if it fails for clinics, sets the exception
                if (pdfEngine.PopulateRenderFromNotificationData(notificationInformation, notification))
                {
                    _logger.Debug($"Back from PopulateRenderFromNotificationData [1], true results");

                    // Build the email
                    MailManager m = new MailManager();
                    notificationInformation.notify_again = notification.notify_again;
                    string body = m.GenerateNotificationMail(notificationInformation);
                    // Send the email

                    string toEmail = notificationInformation.donor_email;
                    bool emailResult = false;

                    if (!isProduction) toEmail = ConfigurationManager.AppSettings["TestEmailAddress"].ToString().Trim();
                    notification.notification_sent_to_email = toEmail;

                    try
                    {
                        var __subject = "IMPORTANT - Drug Screen Notification";
                        if (!isProduction) __subject = "TEST EMAIL - IGNORE THIS EMAIL " + __subject;
                        m.SendMail(toEmail, __subject, body, pdfEngine.CurrentDocumentAsMemoryStreamBytes(), "application/pdf", "DRUG SCREEN NOTIFICATION.PDF");
                        notification.notified_by_email = true;
                        notification.notification_sent_to_email = toEmail;
                        emailResult = true;
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex.ToString());
                        if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                        if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                        notification.notified_by_email = false;
                        notification.notification_email_exception = (int)NotificationExceptions.InvalidEmail;
                    }

                    bool SMSNotifications = false; //<add key= "SMSNotifications" value= "true" />
                    bool.TryParse(ConfigurationManager.AppSettings["SMSNotifications"].ToString().Trim(), out SMSNotifications);
                    SMSNotifications = SMSNotifications && c.enable_sms;
                    string SMSNotificationText = ConfigurationManager.AppSettings["SMSNotificationText"].ToString().Trim();
                    if (string.IsNullOrEmpty(SMSNotificationText))
                    {
                        //TODO - this needs to use the email from webconfig
                        SMSNotificationText = $"Please check your email for an important email from {m.SMTPFromAddress} regarding drug screening.";
                    }
                    SMSNotificationText += $" ({c.client_name})";
                    bool smsResult = false;
                    if (SMSNotifications)
                    {
                        // Send the SMS
                        string _phone = ConfigurationManager.AppSettings["TestPhone"].ToString().Trim();

                        BackendSMS sms = new BackendSMS();
                        if (!isProduction)
                        {
                            SMSNotificationText = "(TEST IGNORE) " + SMSNotificationText;
                            smsResult = sms.SendSMS(c.client_sms_apikey, c.client_sms_token, c.client_sms_from_number, _phone, SMSNotificationText).GetAwaiter().GetResult();
                        }
                        else
                        {
                            smsResult = sms.SendSMS(c.client_sms_apikey, c.client_sms_token, c.client_sms_from_number, notificationInformation.donor_phone_1, SMSNotificationText).GetAwaiter().GetResult();
                        }
                        if (isProduction && !smsResult && !string.IsNullOrEmpty(notificationInformation.donor_phone_2))
                        {
                            // Try their other phone number
                            smsResult = sms.SendSMS(c.client_sms_apikey, c.client_sms_token, c.client_sms_from_number, notificationInformation.donor_phone_2, SMSNotificationText).GetAwaiter().GetResult();
                            notification.notification_sent_to_phone = notificationInformation.donor_phone_2;
                        }
                        else
                        {
                            notification.notification_sent_to_phone = notificationInformation.donor_phone_1;
                        }

                        // Log that it was sent
                        if (smsResult)
                        {
                            notification.notified_by_sms = true;
                        }
                        else
                        {
                            notification.notified_by_sms = false;
                            notification.notification_sms_exception = (int)NotificationExceptions.InvalidPhone;
                            string _BadSMSActivity = $"Notified by sms failed {notification.notification_sent_to_phone} ";

                            SetDonorActivity(donor_test_info_id, (int)DonorActivityCategories.Notification, _BadSMSActivity, activity_user_id);
                        }

                        if (!isProduction)
                        {
                            notification.notification_sent_to_phone = _phone;
                        }
                    }
                    string ActivityNote = string.Empty;
                    if (!isProduction) ActivityNote += "TEST: ";
                    string Again = " (again)";
                    if (!notification.notify_again) Again = string.Empty;

                    if (emailResult) ActivityNote += $"Notified by email{Again} {@notification.notification_sent_to_email} ";
                    if (smsResult) ActivityNote += $"Notified by sms {notification.notification_sent_to_phone} ";

                    notification.notify_again = false; // reset the send again
                    notification.notify_now = false; // reset the send now flag

                    backendData.SetDonorNotification(new ParamSetDonorNotification() { notification = notification });

                    SetDonorActivity(donor_test_info_id, (int)DonorActivityCategories.Notification, ActivityNote, activity_user_id);
                    retval = true;
                    if (pdfEngine.WasAnIssue == true)
                    {
                        try
                        {
                            pdfEngine.ThisSenderTracker.AddData("");
                            var ___msg = "This donor was sent in, but there was some kind of issue while doing so";
                            pdfEngine.ThisSenderTracker.AddData(___msg);
                            var __msg = ___msg + "\r\n\r\n";
                            __msg = __msg + pdfEngine.ThisSenderTracker.Report();
                            SendInFailureEmail(__msg);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error(ex.Message);
                            if (!(ex.InnerException == null)) _logger.Error(ex.InnerException.ToString());
                            notification.notified_by_email = false;
                            notification.notification_email_exception = (int)NotificationExceptions.InvalidEmail;
                            throw ex;
                        }
                    }
                }
                else
                {
                    _logger.Debug($"Back from PopulateRenderFromNotificationData [1], false results");
                    // This order failed, email it to ourselves
                    try
                    {
                        // compare this notification to the old one. 
                        // If metrics have not changed, don't send the email. This is a retry.
                        // Metrics = 
                        // clinic_range
                        // notify now
                        // clinic_exception
                        bool sendfailure = true;
                        if (!(oldnotification==null))
                        {

                            // we've tried this notification once since service started. See if anything has changed
                            if (notification.donor_test_info_id == oldnotification.donor_test_info_id &&
                                notification.clinic_exception==oldnotification.clinic_exception &&
                                notification.clinic_radius==oldnotification.clinic_radius &&
                                notification.notify_now == oldnotification.notify_now &&
                                notification.notify_reset_sendin == oldnotification.notify_reset_sendin &&
                                notification.notify_manual == oldnotification.notify_manual)
                            {
                                _logger.Debug($"Notification metrics match an already attempted send in, no need to resend failure");
                                _logger.Debug($"notification.clinic_exception {notification.clinic_exception}");
                                _logger.Debug($"notification.clinic_radius {notification.clinic_radius}");
                                _logger.Debug($"notification.notify_now {notification.notify_now}");
                                _logger.Debug($"notification.notify_reset_sendin {notification.notify_reset_sendin}");
                                _logger.Debug($"notification.notify_manual {notification.notify_manual}");
                                sendfailure = false;
                            }
                        }



                        if (sendfailure==true) SendInFailureEmail(pdfEngine.ThisSenderTracker.Report());
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex.Message);
                        if (!(ex.InnerException == null)) _logger.Error(ex.InnerException.ToString());
                        notification.notified_by_email = false;
                        notification.notification_email_exception = (int)NotificationExceptions.InvalidEmail;
                        throw ex;
                    }
                    retval = false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }

            return Task.FromResult(retval);
        }

        public List<ZipCodeDataRow> GetZipCodes()
        {
            return backendData.GetZipCodes();
        }

        public bool InsertZipCodes(List<ZipCodeDataRow> _zips)
        {
            return backendData.InsertZipCodes(_zips);
        }

        public byte[] CreatePDFFromText(string Text, string Title)
        {

            PDFengine pe = new PDFengine(_logger);
            return pe.StringToPDF(Text, Title);

        }

        public Task<bool> PreviewNotification(Notification notification, NotificationInformation notificationInformation, ClientNotificationDataSettings clientNotificationDataSettings, string _pdfRenderSettingsJSON)
        {
            bool retval = false;

            try
            {
                string _pdf_template_filename = pdfEngine.DefaultTemplate;
                // if the json specifies a different pdf template use that, otherwise use the default template.
                if (!string.IsNullOrEmpty(pdfEngine.RenderSettings.TemplateFileName)) _pdf_template_filename = pdfEngine.RenderSettings.TemplateFileName;
                pdfEngine.LoadTemplate(_pdf_template_filename);

                //pdfEngine.SetRenderSettingsFromJson(_pdfRenderSettingsJSON);

                // This builds the pdf and will throw errors if data is not found, like files, etc.
                // returns false if it fails for clinics, sets the exception
                if (pdfEngine.PopulateRenderFromNotificationData(notificationInformation, notification, clientNotificationDataSettings, _pdfRenderSettingsJSON))
                {
                    _logger.Debug($"Back from PopulateRenderFromNotificationData [2], true results");
                    if (!(string.IsNullOrEmpty(pdfEngine.PDFBase64))) // Failsafe. this is emptied before trying and if, for any reason, we get true back but no PDF, don't email
                    {
                        // Build the email
                        MailManager m = new MailManager();
                        notificationInformation.notify_again = notification.notify_again;
                        string body = m.GenerateNotificationMail(notificationInformation);
                        // Send the email

                        string toEmail = notificationInformation.donor_email;

                        notification.notification_sent_to_email = toEmail;

                        try
                        {
                            m.SendMail(toEmail, "IMPORTANT - Drug Screen Notification", body, pdfEngine.CurrentDocumentAsMemoryStreamBytes(), "application/pdf", "DRUG SCREEN NOTIFICATION.PDF");
                        }
                        catch (Exception ex)
                        {
                            _logger.Error(ex.Message);
                            if (!(ex.InnerException == null)) _logger.Error(ex.InnerException.ToString());
                            notification.notified_by_email = false;
                            notification.notification_email_exception = (int)NotificationExceptions.InvalidEmail;
                            throw ex;
                        }

                        bool SMSNotifications = false;
                        SMSNotifications = clientNotificationDataSettings.enable_sms;
                        string SMSNotificationText = string.Empty;
                        //if (ConfigurationManager.["SMSNotificationText"])
                        //    ConfigurationManager.AppSettings["SMSNotificationText"].ToString().Trim();
                        if (!(ConfigurationManager.AppSettings["SMSNotificationText"] == null))
                        {
                            SMSNotificationText = ConfigurationManager.AppSettings["SMSNotificationText"].ToString().Trim();
                        }
                        if (string.IsNullOrEmpty(SMSNotificationText))
                        {
                            SMSNotificationText = "Greetings Student, You have been sent a Drug Testing Order via email you gave for your school, check your spam &amp; junk mail as well -SurScan";
                        }
                        bool smsResult = false;
                        if (SMSNotifications)
                        {
                            // Send the SMS
                            BackendSMS sms = new BackendSMS();
                            smsResult = sms.SendSMS(clientNotificationDataSettings.client_sms_apikey, clientNotificationDataSettings.client_sms_token, clientNotificationDataSettings.client_sms_from_number, notificationInformation.donor_phone_1, SMSNotificationText).GetAwaiter().GetResult();
                        }

                        if (pdfEngine.WasAnIssue == true)
                        {
                            try
                            {
                                pdfEngine.ThisSenderTracker.AddData("");
                                var ___msg = "This donor was sent in, but there was some kind of issue while doing so";
                                pdfEngine.ThisSenderTracker.AddData(___msg);
                                var __msg = ___msg + "\r\n\r\n";
                                __msg = __msg + pdfEngine.ThisSenderTracker.Report();
                                SendInFailureEmail(__msg);
                            }
                            catch (Exception ex)
                            {
                                _logger.Error(ex.Message);
                                if (!(ex.InnerException == null)) _logger.Error(ex.InnerException.ToString());
                                notification.notified_by_email = false;
                                notification.notification_email_exception = (int)NotificationExceptions.InvalidEmail;
                                throw ex;
                            }
                        }

                        retval = true;
                    }
                    else
                    {
                        _logger.Error("We got a true result, but a blank PDF - we will consider this a failure and not send donor in");
                        // THIS IS NOT DRY
                        pdfEngine.ThisSenderTracker.AddData($"!!! True but empty PDF");

                        SendFailure(notification);

                        retval = false;
                    }


                }
                else
                {
                    _logger.Debug($"Back from PopulateRenderFromNotificationData [2], false results");
                    SendFailure(notification);

                    retval = false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                if (!(ex.InnerException == null)) _logger.Error(ex.InnerException.ToString());
                throw ex;
            }
            return Task.FromResult(retval);
        }

        private void SendFailure(Notification notification)
        {
            // This order failed, email it to ourselves
            try
            {
                SendInFailureEmail(pdfEngine.ThisSenderTracker.Report());
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                if (!(ex.InnerException == null)) _logger.Error(ex.InnerException.ToString());
                notification.notified_by_email = false;
                notification.notification_email_exception = (int)NotificationExceptions.InvalidEmail;
                throw ex;
            }

        }

        //public bool SavePDFGlobalsToConfig()
        //{
        //    bool retval = false;
        //    try
        //    {
        //        retval = pdfEngine.SavePDFGlobalsToConfig();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return retval;
        //}
        public void SendInFailureEmail(string _body)
        {
            _logger.Debug("Sending Failure Email");
            SmtpClient smtpClient = new SmtpClient();
            NetworkCredential basicCredential = new NetworkCredential(ConfigurationManager.AppSettings["SmtpFromAddress"].ToString().Trim(), ConfigurationManager.AppSettings["SmtpFromAddressPassword"].ToString().Trim());
            MailMessage message = new MailMessage();
            MailAddress fromAddress = new MailAddress(ConfigurationManager.AppSettings["SmtpFromAddress"].ToString().Trim());

            smtpClient.Host = ConfigurationManager.AppSettings["SmtpHost"].ToString().Trim();
            smtpClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"].ToString().Trim());
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = basicCredential;
            smtpClient.EnableSsl = true;

            message.From = fromAddress;
            //message.Subject = "BACKEND SERVICE START " + DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss");
            message.Subject = "BACKEND SERVICE SEND IN FAILURE REPORT " + DateTime.Now.ToString("yyyyy MMMM dd (dddd), HH:mm:ss");

            message.IsBodyHtml = false;
            string mailBody = string.Empty;
            mailBody += _body;
            message.Body = mailBody;
            message.To.Add("chris@inzi.com");
            //message.To.Add("david@surscan.com");

            try
            {
                smtpClient.Send(message);
            }
            catch (Exception ex2)
            {
                _logger.Error("Unable to send failure email");
                _logger.Error(ex2.Message);
                if (ex2.InnerException != null) _logger.Error(ex2.InnerException.ToString());
                _logger.Error(ex2.StackTrace);
                throw ex2;
                //
            }
        }
        #region Files

        public object SaveFile()
        {
            return new object();
        }

        public bool LoadFile()
        {
            return false;
        }

        #endregion Files

        #region sms

        public bool SetSMSActivity(SMSActivity smsActivity, int activity_user_id = 0)
        {
            if (smsActivity.donor_test_info_id == 0) return false;
            try
            {
                ParamSetSMSActivity paramSetSMSActivity = new ParamSetSMSActivity();

                paramSetSMSActivity.smsActivity = smsActivity;
                backendData.SetSMSActivity(paramSetSMSActivity);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public List<SMSActivity> GetSMSActivity(int donor_test_info_id, int activity_user_id = 0)
        {
            try
            {
                ParamGetSMSActivity paramGetSMSActivity = new ParamGetSMSActivity() { donor_test_info_id = donor_test_info_id };
                return backendData.GetSMSActivity(paramGetSMSActivity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SetDonorActivity(int donor_test_info_id, int activity_category_id, string activity_note, int activity_user_id = 0)
        {
            try
            {
                return backendData.SetDonorActivity(new ParamSetDonorActivity() { donor_test_info_id = donor_test_info_id, activity_category_id = activity_category_id, activity_note = activity_note.Trim(), activity_user_id = activity_user_id });
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        public bool SetBackend_client_edit_activity(int backend_notification_window_data_id, string activity_note, int activity_user_id = 0)
        {
            try
            {
                return backendData.SetBackend_client_edit_activity(new ParamSetBackend_client_edit_activity() { backend_notification_window_data_id = backend_notification_window_data_id, activity_note = activity_note.Trim(), activity_user_id = activity_user_id });
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }
        public List<UserActivity> GetUserActivities(int activity_user_id, int activity_user_category_id = 0, bool is_activity_visible = true)
        {
            try
            {
                return backendData.GetUserActivity(new ParamGetUserActivity() { activity_user_id = activity_user_id, activity_user_category_id = activity_user_category_id, is_activity_visible = is_activity_visible });
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
        }
        public bool SetUserActivity(int activity_user_id, int activity_user_category_id, string activity_note)
        {
            try
            {
                return backendData.SetUserActivity(new ParamSetUserActivity() { activity_user_category_id = activity_user_category_id, activity_note = activity_note.Trim(), activity_user_id = activity_user_id });
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
            return false;
        }


        public bool SetSMSActivityAsRead(int donor_test_info_id, int backend_sms_activity_id, int activity_user_id = 0)
        {
            try
            {
                ParamGetSMSActivity paramGetSMSActivity = new ParamGetSMSActivity() { donor_test_info_id = donor_test_info_id };
                List<SMSActivity> smsConversation = GetSMSActivity(donor_test_info_id);
                if (smsConversation.Where(x => x.backend_sms_activity_id == backend_sms_activity_id).ToList().Count > 0)
                {
                    SMSActivity smsActivity = smsConversation.Where(x => x.backend_sms_activity_id == backend_sms_activity_id).First();
                    smsActivity.reply_read = true;
                    smsActivity.reply_read_timestamp = DateTime.Now;
                    smsActivity.user_id = activity_user_id;
                    SetSMSActivity(smsActivity, activity_user_id);
                    SetDonorActivity(donor_test_info_id, (int)DonorActivityCategories.Notification, $"Marked as read: {smsActivity.reply_text}", activity_user_id);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        /// <summary>
        /// return the last smsactivity or a new one for the donor test info id
        /// </summary>
        /// <param name="donor_test_info_id"></param>
        /// <param name="activity_user_id"></param>
        /// <returns></returns>
        public SMSActivity GetSMSActivityLatest(int donor_test_info_id, int activity_user_id = 0)
        {
            try
            {
                List<SMSActivity> smsConversation = GetSMSActivity(donor_test_info_id);
                SMSActivity latestSMS = new SMSActivity();
                // Get the latest SMS activity if it exists
                if (smsConversation.Count > 0) latestSMS = smsConversation.First();
                // If the last activity had been read / seen, we create a new one.
                if (latestSMS.reply_read)
                {
                    // if we have a new activity, populate it

                    latestSMS = new SMSActivity();
                    latestSMS.donor_test_info_id = donor_test_info_id;
                    latestSMS.user_id = activity_user_id;
                }
                return latestSMS;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion sms

        #region NotificationDataSettings

        public List<ClientNotificationDataSettings> GetAllNotificaitonDataSettings()
        {
            try
            {
                return backendData.GetAllClientNotificationDataSettings();
            }
            catch (Exception)
            {
                return new List<ClientNotificationDataSettings>();
            }
        }

        ///// <summary>
        /////
        ///// </summary>
        //public void UpdateNotificationsOnSettingsChange(ClientNotificationDataSettings clientNotificationDataSettings)
        //{
        //    //List<Tuple<DateTime, int>> counts = new List<Tuple<DateTime, int>>();
        //    Dictionary<DateTime, int> counts = new Dictionary<DateTime, int>();

        //    List<Notification> notifications = backendData.GetReadyDonorNotifications(new ParamGetReadyDonorNotifications());
        //    // filter our any donor not related to this client
        //    notifications = notifications.Where(x => x.client_id == clientNotificationDataSettings.client_id && x.client_department_id == clientNotificationDataSettings.client_department_id).ToList();
        //    DateTime start = clientNotificationDataSettings.notification_start_date ?? DateTime.Now.AddYears(-1); // if not set, go into the past to avoid sending
        //    DateTime end = clientNotificationDataSettings.notification_stop_date ?? start.AddDays(1);
        //    DateTime sweep = clientNotificationDataSettings.notification_sweep_date ?? DateTime.Now.AddYears(1);
        //    // filter out any notifications not in sweep / end window.
        //    notifications = notifications.Where(x => x.created_on.Date >= sweep.Date && x.created_on.Date <= end.Date).ToList();

        //    // need to look at all dates between start and stop that are selected days of the week
        //    List<DateTime> window_dates = Enumerable.Range(0, 1 + end.Subtract(start).Days)
        //          .Select(offset => start.AddDays(offset))
        //          .ToList();
        //    // get rid of dates that aren't send in dates
        //    foreach (ClientNotificationDataSettingsDay ds in clientNotificationDataSettings.DaySettings)
        //    {
        //        if (ds.Enabled != true)
        //        {
        //            window_dates.RemoveAll(x => x.DayOfWeek == (DayOfWeek)ds.DayOfWeek);
        //        }
        //    }
        //    int t;
        //    // Get Counts
        //    window_dates.
        //        ForEach(d =>
        //            counts[d.Date] = notifications.Where(x => (DateTime)x.notify_after_timestamp == d.Date).Count()
        //        );

        //    // get rid of dates with send ins equal or above max send ins
        //    window_dates.RemoveAll(x => counts.Any(d => d.Value >= clientNotificationDataSettings.max_sendins));

        //    foreach (Notification n in notifications)
        //    {
        //        while (window_dates.Count > 0)
        //        {
        //            window_dates.RemoveAll(x => counts.Any(d => d.Value >= clientNotificationDataSettings.max_sendins));
        //            // shuffle dates to pick a random day with send ins being less than the max send ins
        //            window_dates = window_dates.OrderBy(emp => Guid.NewGuid()).ToList();
        //            // assign notification to this send in date
        //            n.notify_after_timestamp = window_dates.FirstOrDefault();
        //            n.notify_before_timestamp = end.Date;

        //        }

        //    }

        //}

        public int SetClientNotificationSettings(ClientNotificationDataSettings clientNotificationDataSettings)
        {
            int retval = 0;
            try
            {

                // UpdateNotificationsOnSettingsChange(clientNotificationDataSettings);

                ParamSetClientNotificationSettings p = new ParamSetClientNotificationSettings();
                p.clientNotificationDataSettings = clientNotificationDataSettings;

                //ClientNotificationDataSettings c = new ClientNotificationDataSettings();
                retval = backendData.SetClientNotificationSettings(p);



            }
            catch (Exception)
            {
                throw;
            }
            return retval;
        }

        public void SetDefaultClientNotificationSettings(int client_id, int client_department_id)
        {
            ParamSetClientNotificationSettings p = new ParamSetClientNotificationSettings();

            p.clientNotificationDataSettings.client_autoresponse = "";
            p.clientNotificationDataSettings.client_id = client_id;
            p.clientNotificationDataSettings.client_department_id = client_department_id;
            p.clientNotificationDataSettings.deadline_alert_in_days = 0;
            p.clientNotificationDataSettings.delay_in_hours = 0;
            p.clientNotificationDataSettings.send_asap = true;
            p.clientNotificationDataSettings.enable_sms = false;
            p.clientNotificationDataSettings.created_by = "SYSTEM";
            p.clientNotificationDataSettings.last_modified_by = "SYSTEM";

            p.clientNotificationDataSettings.pdf_render_settings_filename = pdfEngine.DefaultRenderSettingsFile;
            p.clientNotificationDataSettings.pdf_template_filename = pdfEngine.DefaultTemplate;

            p.clientNotificationDataSettings.notification_start_date = DateTime.Now;

            backendData.SetClientNotificationSettings(p);
        }

        public ClientNotificationDataSettings GetClientNotificationDataSettings(int client_id, int client_department_id)
        {
            ClientNotificationDataSettings retval = new ClientNotificationDataSettings();
            try
            {
                retval = backendData.GetClientNotificationDataSettings(client_id, client_department_id);
            }
            catch (Exception)
            {
                throw;
            }
            return retval;
        }

        public ClientNotificationDataSettings GetClientNotificationDataSettingsById(int backend_notification_window_data_id)
        {
            ClientNotificationDataSettings retval = new ClientNotificationDataSettings();
            try
            {
                retval = backendData.GetClientNotificationDataSettingsById(backend_notification_window_data_id);
            }
            catch (Exception)
            {
                throw;
            }
            return retval;
        }

        #endregion NotificationDataSettings

        #region NotificationDataForRender

        public NotificationInformation GetNotificationDataBy_DonorInfoId(int donor_test_info_id)
        {
            NotificationInformation retval = new NotificationInformation();

            try
            {
                retval = backendData.GetNotificationInfoForDonorInfoId(new ParamGetNotificationInfoForDonorInfoId() { donor_test_info_id = donor_test_info_id });
            }
            catch (Exception)
            {
                throw;
            }
            return retval;
        }

        #endregion NotificationDataForRender

        #region DonorNotifications

        public Notification GetDonorNotification(int donor_test_info_id)
        {
            Notification n = new Notification();
            try
            {
                n = backendData.GetDonorNotification(new ParamGetDonorNotification() { donor_test_info_id = donor_test_info_id });
            }
            catch (Exception)
            {
                throw;
            }
            return n;
        }

        public Notification SetDonorNotification(Notification n, int activity_user_id = 0)
        {
            try
            {
                ParamSetDonorNotification p = new ParamSetDonorNotification();
                p.notification = n;
                n = backendData.SetDonorNotification(p);
                string ActivityNote = "Notification data ";
                if (n.donor_test_info_id == 0) { ActivityNote += "created"; } else { ActivityNote += "update"; };
                SetDonorActivity(n.donor_test_info_id, (int)DonorActivityCategories.Notification, ActivityNote, activity_user_id);
            }
            catch (Exception)
            {
                throw;
            }
            return n;
        }

        public bool SetDonorNotificationNextWindow(int donor_info_test_id, int radius, int activity_user_id = 0, bool force_db = false)
        {
            bool retval = false;
            try
            {
                Notification n = GetDonorNotification(donor_info_test_id);// d.GetDonorNotification(new ParamGetDonorNotification() { donor_test_info_id = donor_info_test_id });
                if (n.notified_by_email == true) n.notify_again = true;
                n.notify_next_window = true;
                n.notify_after_timestamp = DateTime.Now;
                n.force_db = force_db;
                n.clinic_radius = radius;
                SetDonorNotification(n, activity_user_id);

                string ActivityNote = "Set to send in after " + n.notify_after_timestamp.Value.Date.ToString();
                SetDonorActivity(n.donor_test_info_id, (int)DonorActivityCategories.Notification, ActivityNote, activity_user_id);
            }
            catch (Exception)
            {
                retval = false;
                throw;
            }
            return retval;
        }

        public bool SetDonorNotificationNow(int donor_info_test_id, int radius, int activity_user_id = 0, bool force_db = false)
        {
            bool retval = false;
            try
            {
                Notification n = GetDonorNotification(donor_info_test_id);
                if (n.notified_by_email == true) n.notify_again = true;
                n.notify_next_window = false;
                n.notify_after_timestamp = DateTime.Now.AddDays(-1);
                n.notify_now = true;
                n.force_db = force_db;
                n.clinic_radius = radius;

                SetDonorNotification(n, activity_user_id);

                string ActivityNote = "Set to send in immediately";
                SetDonorActivity(n.donor_test_info_id, (int)DonorActivityCategories.Notification, ActivityNote, activity_user_id);
            }
            catch (Exception)
            {
                retval = false;
                throw;
            }
            return retval;
        }

        public bool SetDonorNotificationRange(int donor_info_test_id, int range, int activity_user_id = 0)
        {
            bool retval = false;
            try
            {
                Notification n = GetDonorNotification(donor_info_test_id);
                n.clinic_radius = range;
                if (n.notified_by_email == true) n.notify_again = true;
                SetDonorNotification(n, activity_user_id);

                string ActivityNote = "Set clinic range to " + range.ToString();

                SetDonorActivity(n.donor_test_info_id, (int)DonorActivityCategories.Notification, ActivityNote, activity_user_id);
            }
            catch (Exception)
            {
                retval = false;
                throw;
            }
            return retval;
        }

        
        #endregion DonorNotifications

        #region exceptions

        //public DataTable GetNotificationExceptionsByType(NotificationExceptions notificationExceptions)
        public void GetNotificationExceptionsByType(NotificationExceptions notificationExceptions)
        {
        }

        #endregion exceptions

        #region ActivityLog

        //public bool ActivityLog(int donor_test_info_id, string ActivityNote, int activity_user_id = 0)
        //{
        //    try
        //    {
        //        return d.SetDonorActivity(new ParamSetDonorActivity() { donor_test_info_id = donor_test_info_id, activity_category_id = (int)DonorActivityCategories.Notification, activity_note = ActivityNote.Trim(), activity_user_id = activity_user_id });

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        #endregion ActivityLog

        #region UIHelpers

        /// <summary>
        /// Universal confirmation method for send ins outside the scheduled window
        /// </summary>
        public Tuple<bool, bool> ConfirmSendIn(int donor_test_info_id = 0, bool multi = false)
        {
            bool sched = false;
            bool sendnow = false;
            string mbMessage = string.Empty;

            // Check if client / department is manual - if so we do send in immediately and change the dialogs to reflect
            //ClientNotificationDataSettings c = d.GetClientNotificationDataSettings(client_id, client_department_id);

            //if (!c.force_manual && MessageBox.Show("Send in on departments next scheduled window?", "Schedule Send In", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            if (MessageBox.Show("Send in on departments next scheduled window?", "Schedule Send In", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                sched = true;
                mbMessage = "Selected records will be scheduled!";
            }
            else if (MessageBox.Show("Send in immediately?", "Send In", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                sendnow = true;
                mbMessage = "Selected records will be sent in immediately!";
            }
            return Tuple.Create<bool, bool>(sched, sendnow);
        }

        public void DoSendIn(List<int> donor_test_info_ids, int radius, int currentUserId = 0, bool force_db = false)
        {
            bool sched = false;
            bool sendnow = false;
            string mbMessage = string.Empty;

            try
            {
                // Check if client / department is manual - if so we do send in immediately and change the dialogs to reflect
                //ClientNotificationDataSettings c = d.GetClientNotificationDataSettings(client_id, client_department_id);

                //if (!c.force_manual && MessageBox.Show("Send in on departments next scheduled window?", "Schedule Send In", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                foreach (int donor_test_info_id in donor_test_info_ids)
                {
                    NotificationInformation ni = GetNotificationDataBy_DonorInfoId(donor_test_info_id);

                    // if n.backend_notifications_id is 0, we have no notification record for this donor_test_info_id and need to create it.
                    if (ni.backend_notifications_id < 1)
                    {
                        Notification n = new Notification() { donor_test_info_id = donor_test_info_id };
                        if (n.notified_by_email == true) n.notify_again = true;
                        SetDonorNotification(n, currentUserId);
                        // retreive the notification info with the new notification record
                        ni = GetNotificationDataBy_DonorInfoId(donor_test_info_id);
                    }

                    // if there are not settings for this client / dept id, alert the user that the notification will not be sent
                    if (ni.backend_notification_window_data_id < 1)
                    {
                        if (MessageBox.Show($"Warning: {ni.client_name} {ni.department_name} does not have any notification settings!\r\n{ni.donor_full_name} will not be sent in until these are created.\r\nWould you like to create default settings now (SMS disabled, ASAP sample, Manual Send in only)?", "NO DEPARTMENTAL SETTINGS!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            SetDefaultClientNotificationSettings(ni.client_id, ni.client_department_id);
                        }
                    }

                    // We want the PDFRender settings for this client - to know if we need to set the distance on this notification
                    int ThisRadius = GetThisRadiusForSendIn(radius, ni.client_id, ni.client_department_id);
                    //int Radius = ni.dis
                    // if not manual and in window, we send in window - we just schedule to go in next window. user could have used Send In Now.
                    //if (!ni.force_manual && ni.in_window)
                    //{
                    //    Cursor.Current = Cursors.WaitCursor;

                    //SetDonorNotificationNextWindow(donor_test_info_id, ThisRadius, currentUserId, force_db);
                    //}
                    //else
                    //{
                    // Does this notification have a failed Send in? From FormFox?



                    // We're outside the window, so we prompt the user
                    if (!ni.force_manual && MessageBox.Show($"Send {ni.donor_full_name} in on next scheduled window?", "Schedule Send In", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                        {
                            sched = true;
                        }
                        else if (MessageBox.Show($"Send {ni.donor_full_name} in immediately?", "Send In", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                        {
                            // they didn't say yes, so give option to send in immediately
                            sendnow = true;
                        }
                        // if they canceled twice, we won't do anything.
                        if (sched) // sched
                        {
                            SetDonorNotificationNextWindow(donor_test_info_id, ThisRadius, currentUserId, force_db);
                        }
                        if (sendnow) // now
                        {
                            SetDonorNotificationNow(donor_test_info_id, ThisRadius, currentUserId, force_db);
                        }
                    //}

                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
        }

        /// <summary>
        /// Users could be sent in to either 3rd party (formfox) or from local list 
        /// via the exception report.
        /// </summary>
        /// <param name="donor_test_info_ids"></param>
        /// <param name="currentUserId"></param>
        public void DoExceptionSendIn(List<ExceptionSendIn> _EXSendIns, int currentUserId = 0)
        {
            bool sched = false;
            bool sendnow = false;
            string mbMessage = string.Empty;

            try
            {
                // Check if client / department is manual - if so we do send in immediately and change the dialogs to reflect
                //ClientNotificationDataSettings c = d.GetClientNotificationDataSettings(client_id, client_department_id);

                //if (!c.force_manual && MessageBox.Show("Send in on departments next scheduled window?", "Schedule Send In", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                foreach (ExceptionSendIn _EXSendIn in _EXSendIns)
                {
                    NotificationInformation ni = GetNotificationDataBy_DonorInfoId(_EXSendIn.donor_test_info_id);

                    // if n.backend_notifications_id is 0, we have no notification record for this donor_test_info_id and need to create it.
                    if (ni.backend_notifications_id < 1)
                    {
                        Notification n = new Notification() { donor_test_info_id = _EXSendIn.donor_test_info_id };
                        if (n.notified_by_email == true) n.notify_again = true;
                        n.clinic_radius = _EXSendIn.range;
                        //n.clinic_exception = 0;
                        //n.notification_email_exception = 0;
                        //n.notification_sms_exception = 0;
                        SetDonorNotification(n, currentUserId);
                        // retreive the notification info with the new notification record
                        ni = GetNotificationDataBy_DonorInfoId(_EXSendIn.donor_test_info_id);
                    }

                    // if there are not settings for this client / dept id, alert the user that the notification will not be sent
                    if (ni.backend_notification_window_data_id < 1)
                    {
                        if (MessageBox.Show($"Warning: {ni.client_name} {ni.department_name} does not have any notification settings!\r\n{ni.donor_full_name} will not be sent in until these are created.\r\nWould you like to create default settings now (SMS disabled, ASAP sample, Manual Send in only)?", "NO DEPARTMENTAL SETTINGS!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            SetDefaultClientNotificationSettings(ni.client_id, ni.client_department_id);
                        }
                    }

                    // We want the PDFRender settings for this client - to know if we need to set the distance on this notification
                    int ThisRadius = GetThisRadiusForSendIn(_EXSendIn.range, ni.client_id, ni.client_department_id);


                    // if not manual and in window, we send in window - we just schedule to go in next window. user could have used Send In Now.
                    //if (!ni.force_manual && ni.in_window)
                    //{
                    //    Cursor.Current = Cursors.WaitCursor;

                    //    SetDonorNotificationNextWindow(_EXSendIn.donor_test_info_id, ThisRadius, currentUserId, _EXSendIn.force_db);
                    //}
                    //else
                    //{
                        // We're outside the window, so we prompt the user
                        if (!ni.force_manual && MessageBox.Show($"Send {ni.donor_full_name} in on next scheduled window?", "Schedule Send In", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                        {
                            sched = true;
                        }
                        else if (MessageBox.Show($"Send {ni.donor_full_name} in immediately?", "Send In", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                        {
                            // they didn't say yes, so give option to send in immediately
                            sendnow = true;
                        }
                        // if they canceled twice, we won't do anything.
                        if (sched) // sched
                        {
                            SetDonorNotificationNextWindow(_EXSendIn.donor_test_info_id, ThisRadius, currentUserId, _EXSendIn.force_db);
                        }
                        if (sendnow) // now
                        {
                            SetDonorNotificationNow(_EXSendIn.donor_test_info_id, ThisRadius, currentUserId, _EXSendIn.force_db);
                        }
                    //}

                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
        }

        private int GetThisRadiusForSendIn(int radius, int client_id, int client_department_id)
        {
            int retval = 0;

            var _clientNotificationDataSettings = GetClientNotificationDataSettings(client_id, client_department_id);
            var _PDFSettings = pdfEngine.ReadPdfRenderSettings(_clientNotificationDataSettings.pdf_render_settings_filename);
            if (radius > _PDFSettings.Max_Clinic_Distance) retval = radius;
            return retval;
        }

        public void FlipFormFoxArchiveBit(List<string> ReferenceTestIDs, int currentUserId = 0)
        {
            _logger.Information($"FlipFormFoxArchiveBit called");
            try
            {
                foreach (string ReferenceTestID in ReferenceTestIDs)
                {
                    ParamGetformfoxorders paramGetformfoxorders = new ParamGetformfoxorders();
                    paramGetformfoxorders.formfoxorders.ReferenceTestID = ReferenceTestID;
                    formfoxorders ffo = backendData.GetFormFoxOrder(paramGetformfoxorders);
                    bool _archived = ffo.archived;
                    ffo.archived = !_archived;
                    //if (ffo.archived == true)
                    //{
                    //    ffo.archived = false;
                    //}else
                    //{
                    //    ffo.archived = true;
                    //}
                    //ffo.archived = !(ffo.archived);
                    //ffo.archived = false;
                    backendData.SetFormFoxOrder(new ParamSetformfoxorders() { formfoxorders = ffo });

                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
        }

        public ClientNotificationDataSettings CloneClientNotificationDataSettings(ClientNotificationDataSettings c)
        {
            try
            {
                //ClientNotificationDataSettings clientNotificationDataSettings = new ClientNotificationDataSettings();
                string _c = JsonConvert.SerializeObject(c, Formatting.Indented);
                return JsonConvert.DeserializeObject<ClientNotificationDataSettings>(_c);
            }
            catch (Exception)
            {
                _logger.Error("CloneClientNotificationDataSettings couldn't clone object");
                return new ClientNotificationDataSettings();
            }
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email)) return false;

                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^\s*(?:\+?(\d{1,3}))?([-. (]*(\d{3})[-. )]*)?((\d{3})[-. ]*(\d{2,4})(?:[-.x ]*(\d+))?)\s*$").Success;
        }

        public bool IsFiveDigitZip(string number)
        {
            return Regex.Match(number, @"^\d{5}(\-?\d{4})?$").Success;
        }

        #endregion UIHelpers



        #region Integrations
        public int SetIntegrationPartnerClient(IntegrationPartnerClient integrationPartnerClient)
        {
            int retval = 0;

            try
            {
                // get this integration partner if it exists
               

                backendData.SetIntegrationPartnerClient(new ParamSetIntegrationPartnerClient() { IntegrationPartnerClient = integrationPartnerClient });

            }
            catch (Exception ex)
            {

                throw;
            }

            return retval;
        }
       

        public IntegrationPartner GetIntegrationPartnersById(int _id)
        {
            List<IntegrationPartner> _ps = GetIntegrationPartners();
            if (_ps.Exists(p=>p.backend_integration_partner_id==_id))
            {
                return _ps.Where(p => p.backend_integration_partner_id == _id).First();
            }
            return new IntegrationPartner();
        }
        public List<IntegrationPartner> GetIntegrationPartners()
        {
            return backendData.GetIntegrationPartners(new ParamGetIntegrationPartners());
        }


        public IntegrationPartner GetIntegrationPartnerbyClientidDeptId(int client_id, int client_department_id = 0)
        {
            var retval = new IntegrationPartner();
            var res = backendData.GetIntegrationPartnerClientByClientAndDepartmentId(new ParamGetIntegrationPartnerClientByClientAndDepartmentId() { client_department_id = client_department_id, client_id = client_id });
            if (res.Exists(c => c.client_id == client_id && c.client_department_id == client_department_id))
            {
                IntegrationPartnerClient _ipc = res.Where(c => c.client_id == client_id && c.client_department_id == client_department_id).First();
                var _clients = GetIntegrationPartners();
                if (_clients.Exists(cl => cl.backend_integration_partner_id == _ipc.backend_integration_partner_id))
                {
                    retval = _clients.Where(cl => cl.backend_integration_partner_id == _ipc.backend_integration_partner_id).First();
                }
            }
            return retval;
        }


        //public IntegrationPartner GetIntegrationPartner(int backend_integration_partner_id)
        //{
        //    var res = backendData.GetIntegrationPartners(new ParamGetIntegrationPartners());
        //    if (res.Exists(c => c.backend_integration_partner_id == backend_integration_partner_id))
        //    {
        //        return res.Where(c => c.backend_integration_partner_id == backend_integration_partner_id).First();
        //    }
        //    else
        //    {
        //        return new IntegrationPartner();
        //    }
        //}
        public List<IntegrationPartnerClient> GetIntegrationPartnerClients(string _key)
        {
            return backendData.GetIntegrationPartnerClientsByPartnerKey(new ParamGetIntegrationPartnerClientsByPartnerKey() { partner_key = _key });
        }
        public List<IntegrationPartnerClient> GetIntegrationPartnerClientByClientAndDepartmentId(int client_id, int client_department_id)
        {
            return backendData.GetIntegrationPartnerClientByClientAndDepartmentId(new ParamGetIntegrationPartnerClientByClientAndDepartmentId() { client_department_id = client_department_id, client_id = client_id });
        }
      
        public IntegrationDonors GetIntegrationPartnerDonorsAndDocuments(IntegrationPartner integrationParter, ApiIntegrationFilter _filter)
        {
            return backendData.GetIntegrationPartnerDonorsAndDocuments(new ParamGetIntegrationPartnerDonorsAndDocuments() { backend_integration_partner_id = integrationParter.backend_integration_partner_id, apiIntegrationFilter = _filter });
        }

        public bool SetIntegrationPartnerRelease(IntegrationPartnerRelease integrationPartnerRelease, string last_modified_by, string released_by)
        {
            return backendData.SetIntegrationPartnerRelease(new ParamSetIntegrationPartnerRelease() { integrationPartnerRelease = integrationPartnerRelease, last_modified_by = last_modified_by, released_by = released_by });
        }

        public List<IntegrationPartnerRelease> GetIntegrationPartnerRelease(string partner_key, string partner_client_code, bool released_only = true, bool sent = false)
        {
            var _released_only = 1;

            if (released_only == false) _released_only = 0;

            var _sent = 0;

            if (sent == true) _sent = 1;

            return backendData.GetIntegrationPartnerRelease(new ParamGetIntegrationPartnerRelease() {
                partner_client_code = partner_client_code,
                partner_key = partner_key,
                released_only = _released_only,
                sent = _sent
            });
        }
        public IntegrationPartner GetIntegrationPartnerByPartnerClientCode(string partner_client_code)
        {
            return backendData.GetIntegrationPartnerByPartnerClientCode(new ParamGetIntegrationPartnerByPartnerClientCode() { partner_client_code = partner_client_code });
        }

        public int SetIntegrationPartners(IntegrationPartner integrationPartner)
        {
            if (integrationPartner.backend_integration_partner_id==0)
            {
                // we need a new pid type.
                var partners = GetIntegrationPartners();
                // get the max PID
                var _newPID = partners.Select(p => p.backend_integration_partners_pidtype).Max();
                if (_newPID < 10000)
                {
                    _newPID = _newPID + 10000;
                }
                _newPID++;
                integrationPartner.backend_integration_partners_pidtype = _newPID;
            }


            return backendData.SetIntegrationPartners(new ParamSetIntegrationPartners() { IntegrationPartner = integrationPartner });
        }
        #endregion Integrations

        #region Overrides
        public PaymentOverride OverrideDonorPay(string donor_email)
        {
            if (string.IsNullOrEmpty(donor_email)==false)
            {
                var overrides = backendData.GetOverRideList();

                return overrides.Where(o => o.Email.Equals(donor_email, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            }

            return null;
        }

        public bool BurnOverrideDonorPay(string donor_email)
        {
            if (string.IsNullOrEmpty(donor_email) == false)
            {
                return backendData.BurnOverrideDonorPay(donor_email);
            }

            return false;
        }

        #endregion Overrides
    }

    public class ScheduleChecker
    {
    }

    public static class IntegerExtensions
    {
        public static int ParseInt(this string value, int defaultIntValue = 0)
        {
            int parsedInt;
            if (int.TryParse(value, out parsedInt))
            {
                return parsedInt;
            }

            return defaultIntValue;
        }

        public static int SafeParseInt(object _value, int defaultIntValue = 0)
        {
            try
            {
                if (_value == null) return defaultIntValue;
                string testval = _value.ToString();
                if (string.IsNullOrEmpty(testval)) return defaultIntValue;
                return IntegerExtensions.ParseInt(_value.ToString());
            }
            catch (Exception)
            {
                return defaultIntValue;
            }
        }

        public static int? ParseNullableInt(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            return value.ParseInt();
        }

    }
}