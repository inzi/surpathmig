using Serilog;
using SurPath.Data;
using SurPath.Entity;
using SurPath.Enum;
using SurpathBackend;
using System;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;

namespace SurPath
{
    public partial class FrmSmsSendForm : Form
    {
        public string sms_to_send { get; set; } = string.Empty;
        public int donor_test_info_id { get; set; } = 0;
        public int donor_id { get; set; } = 0;
        public int client_id { get; set; } = 0;
        public int client_department_id { get; set; } = 0;
        public int activity_user_id { get; set; } = 0;

        private BackendLogic backendLogic;
        private BackendSMS backendSMS;
        private static ILogger _logger;

        public FrmSmsSendForm(ILogger __logger)
        {
            _logger = __logger;


            backendLogic = new BackendLogic("", __logger);
            backendSMS = new BackendSMS(_logger);

            InitializeComponent();
        }

        private void FrmSmsSendForm_Load(object sender, EventArgs e)
        {
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            this.sms_to_send = txtSMS.Text;

            bool smsSent = sendSMS(); //.GetAwaiter().GetResult();
            this.Hide();
            if (smsSent) { this.DialogResult = DialogResult.OK; } else { this.DialogResult = DialogResult.Abort; }
            this.Close();
        }


        private string PrePhoneNumber(String phone_in)
        {
            string retval = string.Empty;
            try
            {
                string justNumbers = new String(phone_in.Where(Char.IsDigit).ToArray());
            }
            catch (Exception)
            {

                throw;
            }

            return retval;

        }

        private bool sendSMS()
        {
            bool retval = false;
            if (!string.IsNullOrEmpty(sms_to_send))
            {
                // We have something to send via backendLogic...
                _logger.Debug($"FrmSmsSendForm - Preparing to try and send SMS message");
                if (donor_test_info_id > 0 && client_id > 0 && client_department_id > 0)
                {
                    //// we have a valid test info id
                    //// we can mark a reply



                    bool isProduction = true;
                    bool.TryParse(ConfigurationManager.AppSettings["Production"].ToString().Trim(), out isProduction);
                    // Get notification data record
                    Notification notification = backendLogic.GetDonorNotification(donor_test_info_id);
                    // Get the notification render information
                    NotificationInformation notificationInformation = backendLogic.GetNotificationDataBy_DonorInfoId(donor_test_info_id);

                    ClientNotificationDataSettings c = backendLogic.GetClientNotificationDataSettings(notificationInformation.client_id, notificationInformation.client_department_id);
                    if (notificationInformation.donor_id == 0) return retval;

                    // Send the SMS

                    bool smsResult = false;
                    string _phone = "";
                    //DonorBL donorBL = new DonorBL();
                    // Task.Run( async() => await donorBL.DonorSendSMS(donor_id, "test using donorbl")).GetAwaiter().GetResult();
                    if (!isProduction)
                    {
                        _phone = ConfigurationManager.AppSettings["TestPhone"].ToString().Trim();
                        smsResult = backendSMS.SendSMS(c.client_sms_apikey, c.client_sms_token, c.client_sms_from_number, _phone, sms_to_send).GetAwaiter().GetResult();
                    }
                    else
                    {
                        _phone = notificationInformation.donor_phone_1.ToString().Trim();
                        smsResult = backendSMS.SendSMS(c.client_sms_apikey, c.client_sms_token, c.client_sms_from_number, notificationInformation.donor_phone_1, sms_to_send).GetAwaiter().GetResult();

                    }
                    if (isProduction && !smsResult && !string.IsNullOrEmpty(notificationInformation.donor_phone_2))
                    {
                        _phone = notificationInformation.donor_phone_2.ToString().Trim();
                        smsResult = backendSMS.SendSMS(c.client_sms_apikey, c.client_sms_token, c.client_sms_from_number, notificationInformation.donor_phone_2, sms_to_send).GetAwaiter().GetResult();
                    }
                    retval = smsResult;
                    if (smsResult == true)
                    {
                        _logger.Debug($"SMS Notification Sent:  Phone - {_phone}, Text - {sms_to_send}, Result - {smsResult.ToString()}");

                        SMSActivity smsActivity = backendLogic.GetSMSActivityLatest(donor_test_info_id, Program.currentUserId);
                        smsActivity.sent_text = sms_to_send;
                        smsActivity.dt_sms_sent = DateTime.Now;
                        if (!string.IsNullOrEmpty(smsActivity.reply_text))
                        {
                            smsActivity.reply_read = true;
                            smsActivity.reply_read_timestamp = DateTime.Now;
                        }
                        _logger.Debug($"Setting SMS entry into backend SMS Activity:  sent_text - {smsActivity.sent_text}, dt_sms_sent - {smsActivity.dt_sms_sent.ToString()}, Marking last SMS as read (this is a reply) - {smsActivity.reply_read.ToString()}");

                        backendLogic.SetSMSActivity(smsActivity, this.activity_user_id);
                        _logger.Debug($"SMS Activity captured... continuing to adding user activity note");
                        string ActivityNote = string.Empty;
                        if (!isProduction) ActivityNote += "TEST: ";
                        if (smsResult) ActivityNote += $"SMS sent to {notification.notification_sent_to_phone} : {sms_to_send}";
                        _logger.Debug($"Adding Activity Note: donor_test_info_id - {donor_test_info_id}, Note - {ActivityNote.Trim()}, activity_user_id - {activity_user_id}");
                        backendLogic.SetDonorActivity(donor_test_info_id, (int)DonorActivityCategories.Notification, ActivityNote.Trim(), activity_user_id);
                        retval = true;
                    }
                    else
                    {
                        _logger.Error($"SMS Notification NOT SENT!!!!! - Attampt info:  Phone - {_phone}, Text - {sms_to_send}, Result - {smsResult.ToString()}");

                    }



                }
            }
            return retval;
        }
    }
}