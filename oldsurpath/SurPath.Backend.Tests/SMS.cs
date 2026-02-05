using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using SurPath.Data;
using SurPath.Entity;
using SurpathBackend;
using System;
using System.Linq;

namespace SurPath.Backend.Tests
{
    /// <summary>
    /// Summary description for SMS
    /// </summary>
    [TestClass]
    public class SMS
    {
        private int client_id = 110;
        private int donor_id = 77946;
        private int client_department_id = 372;
        private int backend_sms_queue_id;
        private int donor_test_info_id = 93987;
        private string created_by = "unit test";
        private string last_modified_by;
        private string ConnString = "server = 127.0.0.1; port = 3306; Username = surpath; Password = z24XrByS1; database = surpathlive; convert zero datetime=True";
        private string FromID = "+13252081790";
        private string ApiKey = "AC71c8ef0bcf11e3a2c65f4bab577a2e1e";
        private string Token = "ce5bf1314d9e6d4e36a825e0efc4be19";
        private BackendData d;
        private BackendLogic b;

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [TestInitialize]
        public void TestInit()
        {
            last_modified_by = created_by;
            d = new BackendData(ConnString);
            b = new BackendLogic();
        }

        [TestMethod]
        public void Set_SMSActivity()
        {
            SMSActivity latestSMS = new SMSActivity();
            donor_test_info_id = 93987;
            // if we have a new activity, populate it
            if (latestSMS.backend_sms_activity_id == 0)
            {
                latestSMS.donor_test_info_id = donor_test_info_id;
                latestSMS.reply_text = "unit test inbound text";
                latestSMS.user_id = 0;
                latestSMS.dt_reply_received = DateTime.Now;
                latestSMS.auto_reply_text = "unit test auto reply";
            }

            // Send to the database
            d.SetSMSActivity(new ParamSetSMSActivity() { smsActivity = latestSMS });
        }

        [TestMethod]
        public void GetDonorInfoIdByPhone()
        {
            ParamGeneric paramGeneric = new ParamGeneric()
            {
                StoreProc = "backend_get_donor_info_id_by_phone",
                outputID = "@donor_test_info_id"
            };
            paramGeneric.AddParam("@phone_number", "2148019441");

            paramGeneric.AddParam("@donor_test_info_id", MySqlDbType.Int32);
            int _donor_test_info_id = (int)d.GetGenericObject(paramGeneric);
            Assert.IsTrue(_donor_test_info_id > 0);
        }

        //#region smstests

        ////[TestMethod]
        ////public void SMSCreateMessageInDBQueue()
        ////{
        ////    ParamQueueSMS p = new ParamQueueSMS();

        ////    p.client_id = client_id;
        ////    p.client_department_id = client_department_id;
        ////    p.donor_id = donor_id;
        ////    p.textToSend = "Unit test text " + RandomString(5);
        ////    p.sendTo = "2148019441";
        ////    d.QueueSMS(p);
        ////    backend_sms_queue_id = p.backend_sms_queue_id;
        ////    Assert.IsTrue(p.backend_sms_queue_id > 0);

        ////}

        //[TestMethod]
        //public void SMSSetClientAutoresponse()
        //{
        //    ParamSetClientSMSAutoResponse p = new ParamSetClientSMSAutoResponse();

        //    p.client_id = client_id;
        //    p.client_department_id = client_department_id;
        //    p.client_sms_apikey = ApiKey;
        //    p.client_sms_from_id = "+13252081790";
        //    p.client_sms_token = Token;
        //    p.reply = "SMS Autoresponse " + RandomString(5);
        //    p.last_modified_by = "unit test";
        //    p.created_by = "unit test";
        //    int new_id = d.SetClientSMSAutoResponse(p);
        //    Assert.IsTrue(p.backend_sms_autoresponses_id > 0);
        //}

        //[TestMethod]
        //public void SMSLogSMSReply()
        //{
        //    ParamLogSMSReply p = new ParamLogSMSReply();
        //    p.donor_test_info_id = 93986;
        //    p.reply = "Unit Test Reply";
        //    d.LogSMSReply(p);
        //    Assert.IsTrue(p.backend_sms_replies_id > 0);

        //}
        //[TestMethod]
        //public void SMSSendAutoresponse()
        //{
        //    string phone_no = "9728359507";
        //    ParamGetSMSReplyToMessage p = new ParamGetSMSReplyToMessage();
        //    BackendSMS sms = new BackendSMS();
        //    p.phone_number = phone_no;
        //    SMSClientResponse s = d.GetSMSReplyToMessage(p);
        //    Assert.IsTrue(!string.IsNullOrEmpty(s.reply));

        //    bool result = sms.SendSMS(s.client_sms_apikey, s.client_sms_token, s.client_sms_from_id, "2148019441", s.reply).GetAwaiter().GetResult();

        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void SMSSendFromDB()
        //{
        //    // Create a valid SMS entry in the database
        //    SMSSetClientData();

        //    //ClientSMSData clientSMSData = d.GetClientSMSData(client_id);

        //    BackendSMS sms = new BackendSMS();
        //    bool result = sms.SendSMS("", "", "", "+12148019441", "Unit Test Test Message").GetAwaiter().GetResult();
        //    //SMSCreateMessageInDBQueue();
        //    //result = sms.SendSMS(unsentSMS.client_sms_apikey, unsentSMS.client_sms_token, unsentSMS.client_sms_from_id, unsentSMS.donor_phone_1, unsentSMS.client_sms_text).GetAwaiter().GetResult();

        //    List<UnsentSMS> unsentSMs = d.GetUnSentSMS();
        //    Assert.IsTrue(unsentSMs.Count > 0);
        //    //bool result;
        //    foreach (UnsentSMS unsentSMS in unsentSMs)
        //    {
        //        result = false;
        //        //bool result = sms.SendSMS("", "", "", "+12148019441", "Unit Test Test Message").GetAwaiter().GetResult();
        //       // result = sms.SendSMS(unsentSMS.client_sms_apikey, unsentSMS.client_sms_token, unsentSMS.client_sms_from_id, unsentSMS.donor_phone_1, unsentSMS.client_sms_text).GetAwaiter().GetResult();

        //        Assert.IsTrue(result);
        //        Assert.IsTrue(d.MarkSMSAsSent(unsentSMS.backend_notifications_id));
        //        break;
        //    }
        //}

        //[TestMethod]
        //public void SMSSetClientData()
        //{
        //    ClientSMSData c = new ClientSMSData()
        //    {
        //        client_id = client_id,
        //        client_department_id = client_department_id,
        //        client_sms_from_id = "+13252081790",
        //        client_sms_apikey = "AC71c8ef0bcf11e3a2c65f4bab577a2e1e",
        //        client_sms_token = "ce5bf1314d9e6d4e36a825e0efc4be19",
        //        client_sms_text = "Drug Screening Notification - Please check your email for details.",
        //        client_sms_autoresponse_text = "Thank you for your reply. Please see your email for more details."
        //    };
        //    ParamSetClientSMSData p = new ParamSetClientSMSData()
        //    {
        //        ClientSMSData = c
        //    };

        //    d.SetClientSMSData(p);
        //    Assert.IsTrue(p.ClientSMSData.backend_sms_client_data_id > 0);

        //}

        //[TestMethod]
        //public void SMSGetClientData()
        //{
        //    SMSSetClientData();

        //    ClientSMSData clientSMSData = d.GetClientSMSData(client_id, client_department_id);
        //    Assert.IsTrue(clientSMSData.backend_sms_client_data_id > 0);

        //}

        //[TestMethod]
        //public void SMSRemoveClientData()
        //{
        //    bool result = true; //= d.RemoveClientSMSData(client_id, client_department_id);
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void SMSGetUnsent()
        //{
        //    //ParamQueueSMS p = new ParamQueueSMS();

        //    //p.client_id = client_id;
        //    //p.client_department_id = client_department_id;
        //    //p.donor_id = donor_id;
        //    //p.created_by = created_by;
        //    //p.last_modified_by = last_modified_by;
        //    //p.textToSend = "Unit test text " + RandomString(5);
        //    //p.sendTo = "2148019441";
        //    //d.QueueSMS(p);
        //    List<UnsentSMS> unsentSMs = d.GetUnSentSMS();
        //    Assert.IsTrue(unsentSMs.Count > 0);

        //}

        //[TestMethod]
        //public void SMSMarkSent()
        //{
        //    List<UnsentSMS> unsentSMs;
        //    //ParamQueueSMS p = new ParamQueueSMS();

        //    //p.client_id = client_id;
        //    //p.client_department_id = client_department_id;
        //    //p.donor_id = donor_id;

        //    //p.textToSend = "Unit test text" + RandomString(5);
        //    //p.sendTo = "2148019441";
        //    //d.QueueSMS(p);
        //    int donor_test_info_id = 93991;
        //    unsentSMs = d.GetUnSentSMS();
        //    Assert.IsTrue(unsentSMs.Where(x => x.donor_test_info_id == donor_test_info_id).ToArray().Length > 0);
        //    d.MarkSMSAsSent(donor_test_info_id);
        //    unsentSMs = d.GetUnSentSMS();
        //    Assert.IsFalse(unsentSMs.Where(x => x.donor_test_info_id == donor_test_info_id).ToArray().Length > 0);

        //}

        ////[TestMethod]
        ////public void SMSRemoveClientAutoresponse()
        ////{
        ////    ParamRemoveClientSMSAutoResponse p = new ParamRemoveClientSMSAutoResponse();

        ////    p.client_id = client_id;
        ////    p.client_department_id = client_department_id;

        ////    Assert.IsTrue(d.RemoveClientSMSAutoResponse(p));
        ////}

        ////[TestMethod]
        ////public void SMSAddReplyToMessage()
        ////{
        ////    SMSCreateMessageInDBQueue();
        ////    ParamSetSMSReplyToMessage p = new ParamSetSMSReplyToMessage();

        ////    p.backend_sms_queue_id = backend_sms_queue_id;
        ////    p.reply = "this is a reply";
        ////    d.SetReplyToSMSMessage(p);
        ////    Assert.IsTrue(p.backend_sms_replies_id > 0);

        ////}

        ////[TestMethod]
        ////public void SMSHandleReply()
        ////{
        ////    // start web service
        ////    // send text
        ////    // verify reply is logged

        ////}

        //#endregion smstests
    }
}