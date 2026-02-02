using BackendHelpers;
using HL7ParserService;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RTF;
using Serilog;
using SurPath.Business;
using SurPath.Data;
using SurPath.Data.Backend;
using SurPath.Entity;
using SurPath.Enum;
using SurPath.MySQLHelper;
using SurpathBackend;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace SurPath.Backend.Tests
{
    [TestClass]
    public class BackendTests
    {
        private int client_id = 110;
        private int donor_id = 77957;
        private int client_department_id = 372;
        private int backend_sms_queue_id;
        private int donor_test_info_id;
        private string created_by = "unit test";
        private string last_modified_by;
        private string ConnString = "server = 127.0.0.1; port = 3306; Username = surpath; Password = z24XrByS1; database = surpathlive; convert zero datetime=True";
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
        public void NotificationDataSettings_Set()
        {
            DateTime dateTimeStart = new DateTime(2019, 1, 1, 5, 30, 0); // 5:30 am
            DateTime dateTimeStop = new DateTime(2019, 1, 1, 15, 30, 0); // 12:30 pm
            DateTime TestTimeRun = new DateTime(2019, 1, 1, 12, 0, 0); // noon
            ParamSetClientNotificationSettings p = new ParamSetClientNotificationSettings();
            Assert.IsTrue(p.clientNotificationDataSettings != null);

            p.clientNotificationDataSettings.client_autoresponse = "AutoResponse";
            p.clientNotificationDataSettings.client_id = client_id;
            p.clientNotificationDataSettings.client_department_id = client_department_id;
            p.clientNotificationDataSettings.deadline_alert_in_days = 0;
            p.clientNotificationDataSettings.delay_in_hours = 0;
            p.clientNotificationDataSettings.send_asap = true;
            p.clientNotificationDataSettings.enable_sms = true;
            p.clientNotificationDataSettings.created_by = created_by;
            p.clientNotificationDataSettings.last_modified_by = last_modified_by;
            p.clientNotificationDataSettings.pdf_render_settings_filename = "settings.json";
            p.clientNotificationDataSettings.pdf_template_filename = "template.pdf";
            p.clientNotificationDataSettings.DaySettings.Where(x => x.DayOfWeek == (int)DayOfWeekEnum.Monday).First().Enabled = true;
            p.clientNotificationDataSettings.DaySettings.Where(x => x.DayOfWeek == (int)DayOfWeekEnum.Monday).First().send_time_start_seconds_from_midnight = SecondsFromMidnight(dateTimeStart);
            p.clientNotificationDataSettings.DaySettings.Where(x => x.DayOfWeek == (int)DayOfWeekEnum.Monday).First().send_time_stop_seconds_from_midnight = SecondsFromMidnight(dateTimeStop);
            p.clientNotificationDataSettings.DaySettings.Where(x => x.DayOfWeek == (int)DayOfWeekEnum.Tuesday).First().Enabled = true;
            p.clientNotificationDataSettings.DaySettings.Where(x => x.DayOfWeek == (int)DayOfWeekEnum.Tuesday).First().send_time_start_seconds_from_midnight = SecondsFromMidnight(dateTimeStart);
            p.clientNotificationDataSettings.DaySettings.Where(x => x.DayOfWeek == (int)DayOfWeekEnum.Tuesday).First().send_time_stop_seconds_from_midnight = SecondsFromMidnight(dateTimeStop);
            p.clientNotificationDataSettings.DaySettings.Where(x => x.DayOfWeek == (int)DayOfWeekEnum.Wednesday).First().Enabled = false;
            p.clientNotificationDataSettings.DaySettings.Where(x => x.DayOfWeek == (int)DayOfWeekEnum.Wednesday).First().send_time_start_seconds_from_midnight = SecondsFromMidnight(dateTimeStart);
            p.clientNotificationDataSettings.DaySettings.Where(x => x.DayOfWeek == (int)DayOfWeekEnum.Wednesday).First().send_time_stop_seconds_from_midnight = SecondsFromMidnight(dateTimeStop);
            p.clientNotificationDataSettings.notification_start_date = DateTime.Now;
            Assert.IsTrue(p.clientNotificationDataSettings.DaySettings.Where(x => x.DayOfWeek == (int)DayOfWeekEnum.Monday).First().Enabled == true);

            ClientNotificationDataSettings c = new ClientNotificationDataSettings();

            d.SetClientNotificationSettings(p);
        }

        [TestMethod]
        public void NotificationDataSettings_Get()
        {
            ClientNotificationDataSettings c = d.GetClientNotificationDataSettings(client_id, client_department_id);
            Assert.IsTrue(c.client_department_id == client_department_id);
            Assert.IsTrue(c.client_id == client_id);
            Assert.IsTrue(c.enable_sms == false);
            Assert.IsTrue(c.delay_in_hours > 0);
            // Get Settings where there are none in database
            c = d.GetClientNotificationDataSettings(client_id + 1, client_department_id + 1);
            Assert.IsTrue(c.enable_sms == false);
            Assert.IsTrue(c.delay_in_hours == 0);
            Assert.IsTrue(c.send_asap == true);
        }

        [TestMethod]
        public void TestNullLogger()
        {
            ILogger _logger = new LoggerConfiguration().CreateLogger();
            Thread.Sleep(5000);
            for (int x = 0; x < 1000000; x++)
            {
                _logger.Debug("Test");
            }
        }

        [TestMethod]
        public void PIDHelperTest()
        {

            string JsonParserMasks;
            PIDHelper pidHelper = new PIDHelper();

            var pid = "N203147004";

            List<PidMask> pid4Masks = pidHelper.Evaluate(pid);

            var x = 1;
        }

        [TestMethod]
        public void AddPidForNewDonorTest()
        {

            // private void AddPidForNewDonorID(int donor_id, string PID, int pid_type_id, MySqlTransaction trans)

            int donor_id = 500;
            string PID = "test";
            int pid_type_id = 7000;

            string ConnectionString = "server=127.0.0.1;port=3310;Username=surpath;Password=z24XrByS1;database=surpathlive; convert zero datetime=True";

            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    if (PID.Equals("N/S", StringComparison.InvariantCultureIgnoreCase)) return;
                    if (PID.Equals("ILLEGIBLE", StringComparison.InvariantCultureIgnoreCase)) return;
                    if (string.IsNullOrEmpty(PID)) return;
                    if (donor_id < 1) return;
                    if (pid_type_id < 1) return; // don't insert invalid PIDs


                    int existingRowID = Convert.ToInt32(
                            SqlHelper.ExecuteScalar(
                                trans,
                                CommandType.Text,
                                "select individual_pid_id from individual_pids where donor_id=@donor_id AND pid_type_id = @pid_type_id;",
                                  new List<MySqlParameter>()
                                    {
                               new MySqlParameter("@donor_id", donor_id),
                               new MySqlParameter("@pid_type_id", pid_type_id),
                                    }.ToArray()
                                )
                            );

                    if (existingRowID < 1)
                    {// insert
                        SqlHelper.ExecuteNonQuery(
                            trans,
                            CommandType.Text,
                            @"INSERT INTO individual_pids(
                           donor_id
                          ,pid
                          ,pid_type_id
                          ,mask_pid
                          ,validated
                          ,individual_pid_type_description
                        ) VALUES (
                           @donor_id -- donor_id - IN int(11)
                          ,@pid -- pid - IN varchar(255)
                          ,@pid_type_id -- pid_type_id - IN varchar(10)
                          ,@mask_pid
                          ,@validated
                          ,@pid_type_name -- individual_pid_type_description - IN varchar(1024)
                        )",
                                new List<MySqlParameter>()
                                {
                           new MySqlParameter("@donor_id", donor_id),
                            new MySqlParameter("@pid", PID),
                            new MySqlParameter("@pid_type_id", pid_type_id),
                            new MySqlParameter("@pid_type_name", ((PidTypes)pid_type_id).ToString() ),
                            new MySqlParameter("@mask_pid", pid_type_id==(int)PidTypes.SSN? 1 :0 ),
                            new MySqlParameter("@validated",  (sbyte)0 ),
                                }.ToArray()
                            );
                    }
                    //else
                    //{// update
                    //    SqlHelper.ExecuteNonQuery(
                    //        trans,
                    //        CommandType.Text,
                    //        "update individual_pids set donor_id = @donor_id, pid = @pid, pid_type_id = @pid_type_id WHE;",
                    //            new List<MySqlParameter>()
                    //            {
                    //               new MySqlParameter("@donor_id", donor_id),
                    //                new MySqlParameter("@pid", PID),
                    //                new MySqlParameter("@pid_type_id", pid_type_id),
                    //            }.ToArray()
                    //        );

                    //}
                }
                catch (Exception ex)
                {
                    //_logger.Error(ex.Message);
                    //_logger.Error(ex.InnerException.ToString());
                    throw;
                }
            }

       
        }


        [TestMethod]
        public void FilesTest()
        {
            BackendFiles backendFiles = new BackendFiles(ConnString);

            string file_content = "this is a test.";
            string file_name = "unit_test.txt";
            string file_contentOut = string.Empty;

            byte[] byteArray = Encoding.ASCII.GetBytes(file_content);
            MemoryStream stream = new MemoryStream(byteArray);

            backendFiles.databaseFilePut(stream, file_name);

            // now load it.
            using (MemoryStream streamOut = backendFiles.databaseFileRead(file_name))
            using (StreamReader reader = new StreamReader(streamOut))
            {
                reader.DiscardBufferedData();        // reader now reading from position 0

                file_contentOut = reader.ReadToEnd();

                Assert.IsTrue(file_contentOut.Equals(file_content));
            };
            // convert stream to string

            // now update it.
            file_content = "updated test text.";
            byteArray = Encoding.ASCII.GetBytes(file_content);
            stream = new MemoryStream(byteArray);

            backendFiles.databaseFilePut(stream, file_name);

            // now load it again.

            using (MemoryStream streamOut = backendFiles.databaseFileRead(file_name))
            using (StreamReader reader = new StreamReader(streamOut))
            {
                // convert stream to string
                reader.DiscardBufferedData();        // reader now reading from position 0
                file_contentOut = reader.ReadToEnd();
            };

            Assert.IsTrue(file_contentOut.Equals(file_content));
        }

        [TestMethod]
        public void FilesTextFilesTest()
        {
            BackendFiles backendFiles = new BackendFiles(ConnString);

            string file_content = "this is a test.";
            string file_name = "unit_test.txt";

            int id = backendFiles.SaveTextFile(file_name, file_content);
            Assert.IsTrue(id > 0);

            string _out = backendFiles.ReadTextFile(file_name);
            Assert.IsTrue(_out.Equals(file_content));

            file_content = "updated";
            int id2 = backendFiles.SaveTextFile(file_name, file_content);
            Assert.IsTrue(id2 > 0);
            Assert.IsTrue(id == id2);
            string _out2 = backendFiles.ReadTextFile(file_name);
            Assert.IsTrue(_out2.Equals(file_content));
            Assert.IsTrue(_out2 != _out);
        }

        [TestMethod]
        public void FilesFolderSync()
        {
            string test_filename = "test.txt";
            string test_folder = @"c:\allfiles\";

            using (System.IO.StreamWriter file =
                       new System.IO.StreamWriter(test_folder + test_filename))
            {
                file.Write("1");
            }

            string test_string = string.Empty;
            BackendFiles backendFiles = new BackendFiles();
            backendFiles.SyncFolderToDatabase(test_folder);

            backendFiles.SyncFolderFromDatabase(test_folder);
            test_string = backendFiles.ReadTextFile(test_filename);
            Assert.IsTrue(test_string == "1");
            backendFiles.SaveTextFile(test_filename, "0");
            backendFiles.SyncFolderToDatabase(test_folder);
            test_string = backendFiles.ReadTextFile(test_filename);
            Assert.IsTrue(test_string == "0");
            backendFiles.SyncFolderToDatabase(test_folder, true);
            test_string = backendFiles.ReadTextFile(test_filename);
            Assert.IsTrue(test_string == "0");
            Assert.IsTrue(test_string == "1");

            // C:\Dev\Techknowlegable\Surpath\Surpath.Backend\SurpathBackend\PDFConfig
        }

        //[TestMethod]
        //public void ServiceJob()
        //{
        //    BackendLogic backendLogic = new BackendLogic();
        //    // get all the notification window data
        //    List<ClientNotificationDataSettings> clientNotificationDataSettings = d.GetAllNotificaitonDataSettings();
        //    if (clientNotificationDataSettings.Count < 1) Assert.IsTrue(true);  //Task.FromResult(true);

        //    // get ready notifications
        //    List<Notification> notifications = d.GetReadyDonorNotifications(new ParamGetReadyDonorNotifications());
        //    if (notifications.Count < 1) Assert.IsTrue(true); // Task.FromResult(true);

        //    // Get the notifications for this department if

        //    List<Notification> _thisDeptNotifications;
        //    bool result = false;
        //    foreach (ClientNotificationDataSettings clientNotificationDataSetting in clientNotificationDataSettings)
        //    {
        //        _thisDeptNotifications = notifications.Where(x => x.client_id == clientNotificationDataSetting.client_id && x.client_department_id == clientNotificationDataSetting.client_department_id).ToList();
        //        if (_thisDeptNotifications.Count < 1) continue;
        //        foreach (Notification notification in _thisDeptNotifications)
        //        {
        //            if (notification.notify_now)
        //            {
        //                // if now, send it

        //                result = backendLogic.SendNotification(notification.donor_test_info_id).GetAwaiter().GetResult();
        //            }
        //            else
        //            {
        //                // are we in the window?
        //                // if next windows and in window send it

        //                if (backendLogic.OkToSend(clientNotificationDataSetting, DateTime.Now, notification.created_on))
        //                {
        //                    result = backendLogic.SendNotification(notification.donor_test_info_id).GetAwaiter().GetResult();

        //                }
        //            }
        //        }
        //    }
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void Scratch()
        //{
        //    string s = "not_Sunday_day";
        //    string[] sa = s.Split('_');

        //    var x = 1;

        //    s = "sldkfjsdfkl";
        //    sa = s.Split('_');

        //    x = 1;

        //    var y = System.Enum.GetValues(typeof(DayOfWeekEnum));

        //    x = 1;

        //}

        //[TestMethod]
        //public void BackendLogicSendNotification()
        //{
        //    BackendLogic b = new BackendLogic();

        //    bool result = b.SendNotification(93987).GetAwaiter().GetResult();

        //    Assert.IsTrue(result);

        //}

        ////[TestMethod]
        ////public void SendSMS()
        ////{
        ////    bool doTest = false;
        ////    if (doTest)
        ////    {
        ////        BackendSMS sms = new BackendSMS();

        ////        bool result = sms.SendSMS("", "", "", "+12148019441", "Unit Test Test Message").GetAwaiter().GetResult();

        ////        Assert.IsTrue(result);
        ////    }
        ////    else
        ////    {
        ////        Assert.IsTrue(true);
        ////    }
        ////}

        ////[TestMethod]
        ////public void FindDonorsToNotify()
        ////{
        ////    // when a donor registers, put them in the backend_donor_notify table
        ////    //    add the other data at that time - donor_id, client_id, dep id, reason, etc.
        ////    //    if deadline, put that deadline + 7 days (a setting? global or dept?)
        ////    //    if asap, leave deadline blank or set a bit
        ////    //
        ////    // get the donor, the dept (it has lab_code) and the dept notification settings.
        ////    // If we can notify, we notify and set the donor_test_info to notified
        ////    //   Is this a status? Donor Reg status maybe we need to add "notified"? Check with David
        ////    // get clinics for donor
        ////    // build the PDF
        ////    // send the sms
        ////    // update sms sent info
        ////    // log notification to actity

        ////}

        //[TestMethod]
        //public void GetClinicsForDonor()
        //{
        //    ParamGetClinicsForDonor p = new ParamGetClinicsForDonor();

        //    p.donor_id = 6;
        //    p._dist = 30;
        //    List<CollectionFacility> collectionFacilities = d.GetClinicsForDonor(p);
        //    Assert.IsTrue(collectionFacilities.Count > 0);

        //}

        //[TestMethod]
        //public void daysofweek()
        //{
        //    BWDOW bwdow = new BWDOW();

        //    bwdow.Monday = true;
        //    Assert.IsTrue(bwdow.Monday);
        //    bwdow.setDOW = 5;
        //    Assert.IsTrue(bwdow.Sunday);
        //    Assert.IsTrue(bwdow.Tuesday);
        //    Assert.IsFalse(bwdow.Friday);
        //    bwdow.Weekdays = true;
        //    bwdow.Weekends = false;
        //    Assert.IsTrue(bwdow.Friday);
        //    Assert.IsFalse(bwdow.Sunday);
        //    bwdow.Weekends = false;
        //    bwdow.Weekdays = false;
        //    Assert.IsFalse(bwdow.Thursday);
        //    bwdow.Weekends = true;
        //    Assert.IsTrue(bwdow.Saturday);

        //}

        //[TestMethod]
        //public void ServiceScheduleTests()
        //{
        //    DateTime runMoment;
        //    runMoment = DateTime.Now;
        //    int Year = runMoment.Year;
        //    int Month = runMoment.Month;
        //    int Day = runMoment.Day;
        //    int Hour = runMoment.Hour;
        //    int Minute = runMoment.Minute;
        //    int Second = runMoment.Second;

        //    runMoment = new DateTime(Year, Month, Day, Hour, Minute, Second);

        //    Assert.IsTrue(false);
        //}

        //[TestMethod]
        //public void NotificationDataSettings_GetAll()
        //{
        //    List<ClientNotificationDataSettings> c = d.GetAllNotificaitonDataSettings();
        //    Assert.IsTrue(c.Count > 0);
        //    Assert.IsTrue(c.First().client_department_id == client_department_id);
        //    Assert.IsTrue(c.First().client_id == client_id);
        //    Assert.IsTrue(c.First().enable_sms == true);

        //}

        //[TestMethod]
        //public void NotificationDataSettings_Remove()
        //{
        //    d.RemoveClientNotificationSettings(client_id, client_department_id);
        //    ClientNotificationDataSettings c = d.GetClientNotificationDataSettings(client_id, client_department_id);
        //    Assert.IsTrue(c.client_department_id == 0);
        //    Assert.IsTrue(c.client_id == 0);
        //}

        //[TestMethod]
        //public void NotificationSetDonorNotification()
        //{
        //    //MySqlTransaction trans = conn.BeginTransaction();

        //    donor_test_info_id = 93987;
        //    //ParamQueueDonorNotification p = new ParamQueueDonorNotification();
        //    //p.donor_test_info_id = donor_test_info_id;
        //    //p.created_by = created_by;
        //    //p.last_modified_by = last_modified_by;
        //    //d.QueueDonorNotification(p);
        //    //Assert.IsTrue(p.backend_notifications_id > 0);

        //    // set it as notified by email, and sms, with no exeption
        //    ParamSetDonorNotification p2 = new ParamSetDonorNotification();
        //    p2.notification.donor_test_info_id = donor_test_info_id;
        //    p2.notification.notified_by_email = true;
        //    p2.notification.notified_by_sms = true;
        //    Notification n = d.SetDonorNotification(p2);
        //    Assert.IsTrue(n.notified_by_email_timestamp.Date == DateTime.Today.Date);

        //    // Reset the notification
        //    //d.QueueDonorNotification(p);

        //    // set it with email notification, but sms exception
        //    p2.notification.notified_by_sms = false;
        //    p2.notification.notification_sms_exception = (int)NotificationExceptions.SMSBlocked;
        //    n = d.SetDonorNotification(p2);
        //    Assert.IsTrue(n.notify_sms_exception_timestamp.Date == DateTime.Today.Date);

        //    // get the notification for a donor_test_info_id

        //}

        [TestMethod]
        public void GetNotificationDataBy_DonorInfoId()
        {
            donor_test_info_id = 0; // 93987;
            NotificationInformation n = d.GetNotificationInfoForDonorInfoId(new ParamGetNotificationInfoForDonorInfoId() { donor_test_info_id = donor_test_info_id });
            Assert.IsTrue(n.donor_id == 0);
            donor_test_info_id = 93996;
            n = d.GetNotificationInfoForDonorInfoId(new ParamGetNotificationInfoForDonorInfoId() { donor_test_info_id = donor_test_info_id });
            Assert.IsTrue(n.donor_id > 0);
        }

        //[TestMethod]
        //public void GetNotificationExceptions()
        //{
        //    donor_test_info_id = 93987;
        //    ParamQueueDonorNotification p = new ParamQueueDonorNotification();
        //    p.donor_test_info_id = donor_test_info_id;
        //    p.created_by = created_by;
        //    p.last_modified_by = last_modified_by;
        //    d.QueueDonorNotification(p);
        //    Assert.IsTrue(p.backend_notifications_id > 0);

        //    ParamSetDonorNotification p2 = new ParamSetDonorNotification();
        //    p2.notification.donor_test_info_id = p.donor_test_info_id;
        //    // set it with email notification, but sms exception
        //    p2.notification.notified_by_sms = false;
        //    p2.notification.notification_sms_exception = (int)NotificationExceptions.SMSBlocked;
        //    Notification n = d.SetDonorNotification(p2);
        //    Assert.IsTrue(n.notify_sms_exception_timestamp.Date == DateTime.Today.Date);

        //    List<Notification> list = d.GetNotificationExceptions();

        //    Assert.IsTrue(list.Count > 0);
        //    Assert.IsTrue(list.First().notification_sms_exception == (int)NotificationExceptions.SMSBlocked);

        //}

        [TestMethod]
        public void GenericDonorInfoId_Get()
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

        [TestMethod]
        public void DonorNotification_GetSet()
        {
            donor_test_info_id = 93987;
            Notification n = new Notification();
            n = b.GetDonorNotification(donor_test_info_id);
            Assert.IsTrue(n.backend_notifications_id > 0);
            n.is_archived = true;
            n = b.SetDonorNotification(n);
            n.notify_reset_sendin = true;
            n.is_archived = false;
            n = b.SetDonorNotification(n);
        }

        //
        [TestMethod]
        public void DonorStatusTest()
        {
            var donor_id = 93482;
            DonorBL donorBL = new DonorBL();
            Donor donor = donorBL.Get(donor_id, "test");
            var status = "Activated";
            donor.DonorRegistrationStatusValue = DonorRegistrationStatus.Registered;
            if ((int)donor.DonorRegistrationStatusValue >= (int)DonorRegistrationStatus.Registered)
            {
                status = donor.DonorRegistrationStatusValue.ToString();
            }
            Assert.IsTrue(status == DonorRegistrationStatus.Registered.ToString());
        }

        //[TestMethod]
        //public void TestOkToSendAction()
        //{
        //    DateTime dateTimeStart = new DateTime(2019, 1, 1, 5, 30, 0); // 5:30 am
        //    DateTime dateTimeStop = new DateTime(2019, 1, 1, 15, 30, 0); // 12:30 pm
        //    DateTime TestTimeRun = new DateTime(2019, 1, 1, 12, 0, 0); // noon
        //    DateTime TestRegistrationTime = new DateTime(2019, 1, 1, 10, 0, 0); // 10 AM registration
        //    int DayInt = (int)DayOfWeekEnum.Tuesday;
        //    ClientNotificationDataSettings c = new ClientNotificationDataSettings();

        //    Assert.IsTrue(c != null);

        //    c.client_id = client_id;
        //    c.client_department_id = client_department_id;
        //    c.created_by = created_by;
        //    c.deadline_alert_in_days = 0;
        //    c.delay_in_hours = 0;
        //    c.DaySettings.Where(x => x.DayOfWeek == DayInt).First().Enabled = true;
        //    c.DaySettings.Where(x => x.DayOfWeek == DayInt).First().send_time_start_seconds_from_midnight = SecondsFromMidnight(dateTimeStart);
        //    c.DaySettings.Where(x => x.DayOfWeek == DayInt).First().send_time_stop_seconds_from_midnight = SecondsFromMidnight(dateTimeStop);
        //    Assert.IsTrue(c.DaySettings.Where(x => x.DayOfWeek == DayInt).First().Enabled);
        //    Assert.IsFalse(c.DaySettings.Where(x => x.DayOfWeek == (int)DayOfWeekEnum.Monday).First().Enabled);

        //    Assert.IsTrue(OkToSend(c, TestTimeRun, TestRegistrationTime));

        //}

        //[TestMethod]
        //public void GetFormDetailsBasedOnZip()
        //{
        //    string TestZipCode = "76210";

        //    Assert.IsTrue(false);

        //}

        //public bool OkToSend(ClientNotificationDataSettings c, DateTime TestTimeRun, DateTime TestRegistrationTime)
        //{
        //    bool retval = false;
        //    bool InSendWindow = false;
        //    bool AfterDelay = false;
        //    ClientNotificationDataSettingsDay thisDaySettings = c.DaySettings.Where(x => x.DayOfWeek == (int)TestTimeRun.DayOfWeek).First();
        //    bool DayEnabled = thisDaySettings.Enabled;
        //    double SecondsFromMidnight = (TestTimeRun - new DateTime(TestTimeRun.Year, TestTimeRun.Month, TestTimeRun.Day, 0, 0, 0)).TotalSeconds;
        //    bool InDayWindow = ((SecondsFromMidnight >= thisDaySettings.send_time_start_seconds_from_midnight) && (SecondsFromMidnight <= thisDaySettings.send_time_stop_seconds_from_midnight));

        //    if (DayEnabled && InDayWindow && true)
        //    {
        //        InSendWindow = true;
        //    }

        //    // Check Options

        //    // If send asap is enable and override is set to true, then return oktosend as true
        //    if (c.send_asap && true) //(c.override_day_schedule && c.send_asap && true)
        //    {
        //        return true;
        //    }

        //    if (TestTimeRun >= TestRegistrationTime.AddHours(c.delay_in_hours))
        //    {
        //        AfterDelay = true;
        //    }

        //    // Options checked - determine

        //    if (InSendWindow && AfterDelay && DayEnabled && true)
        //    {
        //        retval = true;
        //    }
        //    return retval;
        //}

        public int MinutesToSeconds(int min)
        {
            return min * 60;
        }

        public int HoursToMin(int hour)
        {
            return hour * 60;
        }

        public int HoursToSec(int hour)
        {
            return hour * 60 * 60;
        }

        public int SecondsFromMidnight(DateTime dateTime)
        {
            int retval = 0;
            retval += HoursToSec(dateTime.Hour);
            retval += MinutesToSeconds(dateTime.Minute);
            return retval;
        }
    }

    public class ServiceScheduleChecker
    {
        public DateTime datetime { get; set; }

        /// <summary>
        /// Is True if rules are in alignment
        /// </summary>
        /// <returns></returns>
        public bool OkToExecute()
        {
            bool retval = false;

            return retval;
        }
    }



    [TestClass]
    public class ScratchTests
    {

        [TestMethod]
        public void TestSafeSSNTest()
        {
            string v = "088724939";
            string TempSSNID = new String(v.Where(Char.IsDigit).ToArray());
            TempSSNID = new string('0', 9) + TempSSNID;
            TempSSNID = TempSSNID.Substring(TempSSNID.Length - 9);
            PIDHelper pidHelper = new PIDHelper();
            List<PidMask> pidMasks = pidHelper.Evaluate(v);
            Assert.IsTrue(true == true);
        }

        [TestMethod]
        public void Report_info()
        {
            HL7ParserBL hl7Parser = new HL7ParserBL();
            ReportType reportType = ReportType.QuestLabReport;
            string specimenId = "1511217603639116";
            specimenId = "2050001640";
            //report_info_id report_type specimen_id lab_sample_id   ssn_id donor_pid   donor_pid_type_id donor_last_name donor_first_name donor_mi    donor_dob donor_gender    lab_report lab_report_checksum lab_report_source_filename donor_test_info_id  report_status is_synchronized is_archived created_on  created_by last_modified_on    last_modified_by final_report_id lab_name lab_code    lab_report_date donor_driving_license   test_panel_code test_panel_name report_note tpa_code    region_code client_code deaprtment_code donor_id    client_id client_department_id    screening_time
            //105 2   2026195916  96753162    37456554            MORRISON SAVANNAH        10 / 22 / 1996          2993129fc9bc4da620cba0767792e2c10368fba8        0   0   0   1   2 / 28 / 2020 8:56:16 PM SYSTEM  1 / 12 / 2017 9:42:10 AM SYSTEM  1030
            ReportInfo reportDetails = new ReportInfo();
            ReportInfo reportInfo = hl7Parser.GetReportDetails(reportType, specimenId, reportDetails);

            var x = reportInfo.SpecimenCollectionDate;
        }

        [TestMethod]
        public void testDecryptOfHandoff()
        {

            var payloads = new string[]{
                "K2J4a3RwVm9JaU4vVEJkMy9GM3lJVUl3TGdaM2pmL0toZWp5SytIcVdPNnZOaWdrSTU5cGR3VTYrR1hETUV0M2lydmxpQkxZU3Z6bzRqN0h4Y3o1ZDJoWWJyY1ZRTVo1MTQreUZzSGZSVFdlaGJGQUZRcmRHVUNORHhtRGF5NzYzNUlzUXNxUjh3dHN4bEQ1L016MHQzZ2VGY0t6SHg5WEg5a01GZUJkTHRkOWF5aHVUWHpYM3FIbk43cWU0aTB3MytvMkZ5UDRRUGxIYU8weEtLNm1mOW9Qc2gvTVVvOFhZcjJzZk9Wc0s3ZDJvR1hKLzg3MDh5NS9wU1Nac1U1bi9UU3dQY1pZTGlGZkROVVRzMGxpTG03aS9KUXo0aWp3WTJkWTR0eFErQTE0a1ZjOElmTzhNZjAwc0QzR1dDNGhQRjlaVlVHdXlZSFhDMjFVMCt4R2Y1YjFpazhZKzl5TUNPalE0TUE3bmtoYnJnek1ka2JWMGdqbzBPREFPNTVJS0JsQWpUU2hhakVZQTNzdk9sY2VzU0ZOQk5HZWhldnRDT2pRNE1BN25raEhwM2xZOFkyQVIxNU9Gd3lna2VCZlJuSUZQb2JBTVdIMloxWWZWV0FYTmgyTmFIamdRUzg1WThnUE9lVC8vaE5lY1ZZRW1WcE5RZGxnSVFwcW1OYnBqNXVidmtQV0J5TWMzWU54WEp3WUlERTB4Q3ZTTWxxZ0NQZCtiWjhDc001clpEb3k3ZUJGYUE9PQ",
"K2J4a3RwVm9JaU4vVEJkMy9GM3lJVUl3TGdaM2pmL0srNHljVTBFOTJoUDNNQU43TDhxTGp3VTYrR1hETUV0M2lydmxpQkxZU3Z6bzRqN0h4Y3o1ZDJoWWJyY1ZRTVo1MTQreUZzSGZSVFdlaGJGQUZRcmRHVUNORHhtRGF5NzYzNUlzUXNxUjh3dHN4bEQ1L016MHQzZ2VGY0t6SHg5WDlNK1VNSDRxTHhwZU5xdmZwQTEvdkIwNE9nV3JBQlFOTCs5U1ZqMlkvNWpIc2xMdUZRdnBpb3FoQlQ0QUZSK2NBdGZmTEtHS3JxVHZuZ0djMUI0RHFaZzRBTHRRQVVHZUEwOVl5NHd4WE5SUHBQc2QrNnVXQ0c3aS9KUXo0aWp3WTJkWTR0eFErQTE0a1ZjOElmTzhNZjAwc0QzR1dDNGg2OGlLaEYybjA3Tld1ZHdnRlhXbkpYdm1IR1k2MjRWc0F3bW5ROHI4KzF3Sm1vOUduLzM0N3IrWmQ1dU9QRXRhSlgwYjBBZGExMmhXdWR3Z0ZYV25KYytSMHF1YlNidGF1K0ZUd0h3dEVaMzdyUkJmeHE4V0NEOEJDQWpnQzRQRmw3WlQwWHlleGNvUjdybFhvR2lzUkpuK2FaK3hKMUpZYnVMOGxEUGlLUER5cmE3b3lHVnQrZ01KcDBQSy9QdGNkY0hBSVZ6M3hmUTFMdTBjYktKS3NXY0xmM0RlUkxnc1JBdUw1aGFlVUpuM3UzaTNwNnpNV2hJQUZjY0dqemU5amNnRmNRZEpYMEs0eFdCWVlvRXBwYUk1UVEzOHU3aUlZNThqWXZhdU53WXZ5a0JDemg4MkdnPT0",
"K2J4a3RwVm9JaU4vVEJkMy9GM3lJVUl3TGdaM2pmL0tmMTVzRUlXSmhMTG0vTEQ3RWE3QmJzVXV1ZDFaVG11Zmt4Qnc1VUtaekhCQzZjdjRwZEVpdDI3aS9KUXo0aWp3MDZzTllncnh3bWVIWG1SWEVYT3Flb2xVdTFycmU1SS9mbnZ6Q2pncHllTW9SUkJXVUpnbVZqaS9STkM3R0ZLLzRWSjRFb09xSkhlWGNaOEJXQ3NURXBqdWE2MmREMWFGSm9JaW1zNVM0Kzh2MzdOS2I5b2Q4MG00cjVkUDVwSEhQWHVidVVlRkNlQkpNZ2dBZFQzdGpRRWhpT0tGYWduZFQ2K2VCVTFqeUN3RVk3VVY3L2g5b3JueXNzSEk3R3BEZXd1K1lLUXlZYko2VHBhbE00Y2VjRnpwZ2c2ZUR5NWVBd21uUThyOCsxd2xPdHFhZGxTcE9Ic0x2bUNrTW1HeWF0NXVwOVFqRjlEVzRnODZKK0NhL0JZbjc1UzBnUVI4YnVMOGxEUGlLUEFLN3Nxbi8wbGpPbTdpL0pRejRpanc4WXJOd216Sm9GZFd1ZHdnRlhXbkpYakpybmVQbjBzWWJ1TDhsRFBpS1BDTE9Va3NRY2xWNFFNSnAwUEsvUHRjUVYyQWhVVjJybkNOYU4yTUVndC9HdmU3ZUxlbnJNeGFvUlh3TnVXZW93QURDYWREeXZ6N1hIWEJ3Q0ZjOThYMFZZazBOaXI2cjhtS1R0ZDdzSVZMT1ZEYVRuNHNKcnd1WlNLWEhOeFBOM2M9",
"K2J4a3RwVm9JaU4vVEJkMy9GM3lJVUl3TGdaM2pmL0tGekhXRFlBVU03OU5mVXpmYm1JT3FBVTYrR1hETUV0M2lydmxpQkxZU3Z6bzRqN0h4Y3o1ZDJoWWJyY1ZRTVo1MTQreUZzSGZSVFdlaGJGQUZRcmRHVUNORHhtRGF5NzYzNUlzUXNxUjh3dHN4bEQ1L016MHQzZ2VGY0t6SHg5WGVpR0N2bnorRXR0OWF5aHVUWHpYM2sxMGRzRjVPdlJFc2Q0aDZxN0xUYWVDYmxlRUVQa25GTm9Qc2gvTVVvOFhZcjJzZk9Wc0s3ZDJvR1hKLzg3MDh5NS9wU1Nac1U1bi9UU3dQY1pZTGlHNThsNDB5TzFuRGo4MVpTOXEweVFjMmQrSmo2MXBrREZXdWR3Z0ZYV25KYk9qT0hqODYzcFhPUmE2VGk1YjdQRHh2QVRkd0ZNQzlldGhmczhQV2xoRnNOTTUyUmtBUlNOV3Vkd2dGWFduSlUvRklrUXFZNkcrR0ZPMHRzNnh2M2RhZytNVEZNSFgvSU03ckx6WDZSY3dNNzVqdjF0S2RSd3ZhTGh4aktidng1akZxQXFqSndQYitZYzliYVFrV2pkR2NnVStoc0F4WVJvR2o3RFIrdUJJSFkxb2VPQkJMemtLbzR6YVZLeVZuNkRGd0tNREpQMzZ1YVU5MisrQnRaaVEyekZ3UDhOWmVJN0NmcXdDaHhRelBtOHd5aitDdFVodTR2eVVNK0lvOEl2SmdBSm9JdzhmclU5V0FOdFp2UVJXLzBHc1J5b0V5dkVqMk45OUd3dTRVeGVIdnRTLzR4UT0",
"K2J4a3RwVm9JaU4vVEJkMy9GM3lJVUl3TGdaM2pmL0tyTjJSa3NaV1hnTTNZT1U3YXAzQy84QTR3U1YxMDRzU3dlVERwMjNzVVlkS21OL3dqRUh1OWdJdWk0dVVoQ2loODA0V1c0cHMyWDZVUTZRSUJYVDByamU3bHNzTnY0cUh2ckpITGhhbzRvczRwK3dPaWgraUU4Y2FMaG9IN0V3ZzNDUHo0cXpUSGhNd3VJNE9NOW9PeVlxNUdMQUs3emx6T0FibnpMYjMwaWZUSjEzNlQ3K21pYTBOYU1iK3VKbU0zVUhBd2JYbGhaaHI4SHhnWFEvNFJKalY2UVhjWkpWaTk1dldHa3VOakVjUFZpeEc5cXlnZHZ4V3NrcHh0VHB6bURnQXUxQUJRWjREVDFqTGpERmMxSW5FZlRVbitZOHdNRVRmQ1R5eVlCazhrWit5ZXU5dUFaMnV5VU9KSmxTWkdIaUh4RkcrTkZNRFQxakxqREZjMUh2TjZadDJiZHFNMXVJUE9pZmdtdndXSisrVXRJRUVmRzdpL0pRejRpandDdTdLcC85Sll6cHU0dnlVTStJbzhQR0t6Y0pzeWFCWFZybmNJQlYxcHlWNHlhNTNqNTlMR0c3aS9KUXo0aWp3aXpsSkxFSEpWZUVEQ2FkRHl2ejdYRUZkZ0lWRmRxNXdqV2pkakJJTGZ4cjN1M2kzcDZ6TVdxRVY4RGJsbnFNQUF3bW5ROHI4KzF4MXdjQWhYUGZGOUZXSk5EWXErcS9KaWs3WGU3Q0ZTemxRMms1K0xDYThMbVVpbHh6Y1R6ZDM",
"K2J4a3RwVm9JaU4vVEJkMy9GM3lJVUl3TGdaM2pmL0tWZE1JV2NaMzB1S1hjQW1lSytSRVM4amxWSU1FaHRkRzZsZDk1cjJyZGdwa2JmZDhXL3cyeGtaeUJUNkd3REZoTkp5Lzg2ekhXWU9ZV2QwdUVWaU1SRmxVZ1dIOUpKeXFQdkY1UmQyUWl2b3JHRnMwSDF1eEpMc2U5SklxMzFYWU9rRmIwdHRNT0kzVUZ6eGVERDA1djlIRUFYRWJtdWpQS0lzOE9iRWNaU3dyOE9UUERCRkxrd0NnY2FIUmlZNStFRWE2cHFtVVRYQzViYVZERXk4bjVlSFR5aUpsY2paVW5WMzk0Rk9iS0h0dTR2eVVNK0lvOEVacWZSU0had2QxUFh6WTVrY0tuSHdHVnkwOXVjRmtDTVFsbTgyc3dMbjJDT2pRNE1BN25raUVqQTZsZFF0Qm4yL1pVNFE2N3BPZnFjYytEUURwTFZoV3Vkd2dGWFduSlh2bUhHWTYyNFZzQXdtblE4cjgrMXdKbW85R24vMzQ3Z01KcDBQSy9QdGNnenVzdk5mcEZ6QUk2TkRnd0R1ZVNQdXRFRi9HcnhZSUF3bW5ROHI4KzF4QlhZQ0ZSWGF1Y0lwUi8zSjVxZWJ3MzJQVEJ5WVNMZHd0cG9oTzhua09xVmE1M0NBVmRhY2wrNjhVc0htL05SeUtVZjl5ZWFubThQZTdlTGVuck14YUVnQVZ4d2FQTjcwazBiOTMyWUxUWEtJNVFRMzh1N2lJWTU4all2YXVOd1l2eWtCQ3poODJHZz09",
"K2J4a3RwVm9JaU4vVEJkMy9GM3lJVUl3TGdaM2pmL0tkLytlZUp6MExPd1VpU0l2ei9QVWFBVTYrR1hETUV0M2lydmxpQkxZU3Z6bzRqN0h4Y3o1ZDJoWWJyY1ZRTVo1MTQreUZzSGZSVFdlaGJGQUZRcmRHVUNORHhtRGF5NzYzNUlzUXNxUjh3dHN4bEQ1L016MHQzZ2VGY0t6SHg5WHJDOWtCSjJXbmpsOWF5aHVUWHpYM200MmF3ZzJ6UWJUTG1CejJaYUVYdVVRcEZVaUkwSWxCZG9Qc2gvTVVvOFhZcjJzZk9Wc0s3ZDJvR1hKLzg3MDh5NS9wU1Nac1U1bi9UU3dQY1pZTGlIZEJFK281cmdud0lHWVBvMldhYmhGdHhyczlvL0lLUmNkaldoNDRFRXZPU21xWXdQbTFGK2lFeEdmc3Z3bTVMSHdnZUZ4THhXMVRldGhmczhQV2xoRnNOTTUyUmtBUlNOV3Vkd2dGWFduSlUvRklrUXFZNkcrTzZqcjV4ZDNJbTdyWVg3UEQxcFlSY0lqVG8va1NVMkRrVHRRVGFFWmhyQTNVenpCTGJHdE8wR1ZZejNDZU9yWXdpdFExSzFXRXl4QlhZQ0ZSWGF1Y0lwUi8zSjVxZWJ3MzJQVEJ5WVNMZHd0cG9oTzhua09xVmE1M0NBVmRhY2wrNjhVc0htL05SeXVadVdMbUFWUDMwYWw4cHNyVVFpbzg2OFJBNmowRDVqWllDRUthcGpXNlkrYm03NUQxZ2NqSE4yRGNWeWNHQ0F4Tk1RcjBqSmFvQWozZm0yZkFyRE9hMlE2TXUzZ1JXZz0",
"K2J4a3RwVm9JaU4vVEJkMy9GM3lJVUl3TGdaM2pmL0t2Q08zbGFsZlg4VVpCODRmNnVVeTZPdGdEaHNrSnpIS0dYZWNnNUIyS2JWQTljRWVHOXdqTXpCQ3hXbENPMGlWQWpTRzRsUStuQmZjcU5HOC9sL05UeVZqVXRZVnNxclJkUlp4OWY1SjFaQnpuU0JHL1NWYUpzUVd2Z3NGRjdxTGlCSnFteDZWS3VsRTFYY0ZpNWsxVkpmVHdZWTlKY05WcnA3QkZBTS9uTWlJOEJvRk9lZ0FRWjNqUjBlVGdzOXhWUTcvNytDOWlmcHU0dnlVTStJbzhFWnFmUlNIWndkMVB6cTZwN1lpODVUb1EzTWRFUUhpUVZ6cGdnNmVEeTVlQXdtblE4cjgrMXdsT3RxYWRsU3BPSHNMdm1Da01tR3kzNnh2Vm5kaXRnMFBIeVlJT29weUxlWjVmRkNZS21kTEJ3OUVlZUJoa29XYXIwdEowVlhkeUpmcG5OSzNHL3NWNjJGK3p3OWFXRVZvcUNHL2tleFpwMTR3bXdJZkRSb3ZOMU04d1MyeHJUdE9TcEtETnVYMDFnbmEvZE9GWFNZUFFWMkFoVVYycm5BaFMyZGJ5K0ZHdTRuY1dhU1JLV3FIN01Zc00xdys2c0lLbzR6YVZLeVZuNjZiUXRrTjc3YXFXWmVENS8xRW5pSld1ZHdnRlhXbkpmdXZGTEI1dnpVY3FHMGUxOU1OdEpKak1ZSGI3b1d1ZnYrNlE5dHBrbkhxVnJuY0lCVjFweVg3cnhTd2ViODFISTFvM1l3U0MzOGF2OVQ1WEZ0SjV2UEIvQUUrZ0c5TFFpUVlReWVXU0RESQ",
"K2J4a3RwVm9JaU4vVEJkMy9GM3lJVUl3TGdaM2pmL0tJOG5GV0FDYWxIY3dWSEhCd1d1SWhRVTYrR1hETUV0M2lydmxpQkxZU3Z6bzRqN0h4Y3o1ZDJoWWJyY1ZRTVo1MTQreUZzSGZSVFdlaGJGQUZRcmRHVUNORHhtRGF5NzYzNUlzUXNxUjh3dHN4bEQ1L016MHQzZ2VGY0t6SHg5WEdHY1RYKzgwY0VMVVBLSVdReDlTNndJQlBLdGRZZEV6ZlYzOFdtY2U5aDV4VGNuK2FsUWNxWXFoQlQ0QUZSK2NBdGZmTEtHS3JxVHZuZ0djMUI0RHFaZzRBTHRRQVVHZUEwOVl5NHd4WE5TeUtzdnEyU1JhRFR5Um43SjY3MjRCbmE3SlE0a21WSmtZZUlmRVViNDBVd05QV011TU1WelU4akVPK1BpWHlqMkZlc3hkRGNyc1J1dGhmczhQV2xoRnNOTTUyUmtBUlNOV3Vkd2dGWFduSlUvRklrUXFZNkcrbElkbkp1alNHaVF4d3VIMVBIbUxESzZQZ0NLU0gveW1tNFVxWU14Snc5b2hUUVRSbm9YcjdkQlpyeDhXMk9XVnJ3SmFndGJxc3JQZlk5TUhKaEl0M05wQXFzZWRVQmxZVnJuY0lCVjFweVVKQlZjQ2c2TTlsUWpvME9EQU81NUlscFdOWGlHazdRU3pDc3d3TUpnZkVQVmQ0cDN3eFRUSjZZdCtIQThTejdLNXBUM2I3NEcxbUJ5eERuQ2xBR1FBWThNR1l1eXVtbVBSYkRmNGZubEdubEJVbCt3K3FpeWVOeGlEaE1Ia29Sbz0"
};
            var key = "242575a2-d320-11eb-b1e0-7c4c58ced981";
            var crypto = "9edef9be-4ec6-ad3f-75e9-45d031d51741";
            var partner_client_code = "154"; // Guid.NewGuid().ToString(); 

            //string b = BackendStatics.Base64URLDecode(dtostring);

            SurpathHandoffURLGenerator surpathHandoffURLGenerator =
                new SurpathHandoffURLGenerator(key, crypto, partner_client_code, "http://localhost:1481/registration/handoff", true);
            foreach (var dtostring in  payloads)
            {
               

                var _dto = BackendStatics.Base64URLDecode(dtostring);
                //var descriptedstring = SurpathHandoffURLGenerator.DecryptWithKey(cypherstring, crypto);
                string _json = UserAuthentication.DecryptWithKey(_dto, crypto);
                IntegrationHandOffDTO integrationHandOffDTO = JsonConvert.DeserializeObject<IntegrationHandOffDTO>(_json);
                Debug.Print($"{integrationHandOffDTO.integration_id} {integrationHandOffDTO.donor_first_name} {integrationHandOffDTO.donor_last_name} {integrationHandOffDTO.donor_email}");

            }
            var y = 1;

        }

        [TestMethod]
        public void testURLBuilderHandOff()
        {
            var key = "242575a2-d320-11eb-b1e0-7c4c58ced981";
            var crypto = "9edef9be-4ec6-ad3f-75e9-45d031d51741";
            var partner_client_code = "115"; // Guid.NewGuid().ToString();

            SurpathHandoffURLGenerator surpathHandoffURLGenerator =
                new SurpathHandoffURLGenerator(key, crypto, partner_client_code, "http://localhost:1481/registration/handoff", true);


            //        SurpathHandoffURLGenerator surpathHandoffURLGenerator =
            //new SurpathHandoffURLGenerator(key, crypto, partner_client_code, "https://stage.surpath.com/registration/handoff", true);

            IntegrationHandOffDTO integrationHandOffDTO = new IntegrationHandOffDTO();
            integrationHandOffDTO.donor_email = "chris@inzi.com";
            integrationHandOffDTO.partner_client_code = partner_client_code;
            integrationHandOffDTO.donor_first_name = "Chris";
            integrationHandOffDTO.donor_last_name = "Norman";
            integrationHandOffDTO.donor_phone_1 = "2148019441";
            integrationHandOffDTO.donor_state = "TX";
            integrationHandOffDTO.integration_id = "pcid55";

            var URL = surpathHandoffURLGenerator.GetHandoffURL(integrationHandOffDTO).ToString();

            // split the url into a list
            List<string> _parts = URL.Split('/').ToList();
            // get rid of empty values.
            _parts = _parts.Where(p => string.IsNullOrEmpty(p) == false).ToList();
            // get the index of 'handoff'
            var _handoffindex = _parts.IndexOf("handoff");

            var base64clientcode = _parts[_handoffindex + 1];

            var decodedClientCode = Base64UrlEncoder.Decode(base64clientcode);

            var _mte1 = Base64UrlEncoder.Decode("MTE1");

            Assert.IsTrue(partner_client_code == decodedClientCode);

        }

        [TestMethod]
        public void integrationPusherTest()
        {
            var data = new string('=', 1024);


            IntegrationPusher integrationPusher = new IntegrationPusher(false);

            //byte[] byteArray = Encoding.ASCII.GetBytes(data);
            //MemoryStream stream = new MemoryStream(byteArray);
            //integrationPusher.SendFile(stream, "inbound/test.enc", "ext.projectconcert.com", 22, "surscan_test", "9ub2Vxvmd3FEd68s");

            integrationPusher.Push();

            var x = 1;
        }


        [TestMethod]
        public void GetEncryptedHandOff()
        {
            var id = "c5f20d02-d8e5-11eb-8329-001c42da525b";
            var crypto = "9edef9be-4ec6-ad3f-75e9-45d031d51741";
            IntegrationHandOffDTO integrationHandOffDTO = new IntegrationHandOffDTO();
            integrationHandOffDTO.donor_email = "chris@inzi.com";
            integrationHandOffDTO.partner_client_code = "115";
            integrationHandOffDTO.integration_id = "pcid55";
            string _auth = UserAuthentication.EncryptWithKey(integrationHandOffDTO.partner_client_code + integrationHandOffDTO.donor_email, id);
            integrationHandOffDTO.auth = _auth;
            var _json = JsonConvert.SerializeObject(integrationHandOffDTO);
            var encobj = UserAuthentication.EncryptWithKey(_json, crypto);
            var base64 = Base64UrlEncoder.Encode(encobj);


            Assert.IsTrue(true == true);
        }


        public static string Base64ForUrlDecode(string str)
        {
            if (str.Contains("="))
            {
                str = str.Replace("=", "/");
            }
            byte[] decbuff = HttpServerUtility.UrlTokenDecode(str);
            return Encoding.UTF8.GetString(decbuff);
        }

        [TestMethod]
        public void TestActivationDonorIdDecrypt()
        {
            string _enc = "LVgzT2wxQ3ZHQnMs0";

            int _donorid = Convert.ToInt32(UserAuthentication.URLIDDecrypt(Base64ForUrlDecode(_enc.ToString()), true));

            Assert.IsTrue(_donorid == 129940);

        }

        [TestMethod]
        public void TestEnum()
        {
            foreach (var val in System.Enum.GetValues(typeof(ZipRowIDs)))
            {
                var x = val;
                Debug.WriteLine(x);
            }

            //Array _ids = System.Enum.GetValues(typeof(ZipRowIDs));

            //foreach (System.EMyEnum val in System.Enum.GetValues(typeof(ZipRowIDs)))
            //    {
            //       Console.WriteLine(val);
            //    }

            //foreach (string _id in _ids)
            //{
            //}
        }



        //        public class FFMPSearchResultTest

        //        {
        //            public string Code { get; set; }
        //            public string Name { get; set; }
        //            public string City { get; set; }
        //            public string State { get; set; }
        //            public string Zip { get; set; }
        //            public string Address1 { get; set; }
        //            public string Address2 { get; set; }
        //            public int MasterAccount { get; set; }
        //            public string TimeZone { get; set; }
        //            public int TZOffset { get; set; }
        //            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //            public double Distance { get; set; } = -1;
        //            public bool MarketplaceEnabled { get; set; }
        //            public List<FFMPService> Services { get; set; }
        //            public List<object> Amenities { get; set; }
        //            public bool NoMarketplaceId { get; set; } = false;
        //        }

        //        [TestMethod]
        //        public void pullSitesNullDistace()
        //        {
        //            var json = @"
        //[
        //  {
        //    ""Code"": ""FF00090142"",
        //    ""Name"": ""SpotOn Drug and Alcohol Testing"",
        //    ""City"": ""Cypress"",
        //    ""State"": ""TX"",
        //    ""Zip"": ""77429-5212"",
        //    ""Address1"": ""12617 Louetta Rd Ste 212"",
        //    ""Address2"": """",
        //    ""Phone"": ""8324223596"",
        //    ""Fax"": ""8324223637"",
        //    ""HoursOfOperation"": ""M  08:00 - 19:00  T  08:00 - 19:00  W  08:00 - 19:00  TH  08:00 - 19:00  F  08:00 - 18:00  Su  01:00 - 23:30"",
        //    ""MasterAccount"": 2627,
        //    ""TimeZone"": ""CDT"",
        //    ""TZOffset"": -5,
        //    ""Distance"": 22.3,
        //    ""FormFoxEnabled"": true,
        //    ""MarketplaceEnabled"": true,
        //    ""Rating"": 5,
        //    ""Services"": [
        //      {
        //        ""Code"": ""NDOTU"",
        //        ""Rank"": 1.985,
        //        ""PriceList"": ""Gold""
        //      }
        //    ],
        //    ""Amenities"": []
        //  },
        //  {
        //    ""Code"": ""FF00067887"",
        //    ""Name"": ""Porter Drug and Alcohol Screens - Porter"",
        //    ""City"": ""Porter"",
        //    ""State"": ""TX"",
        //    ""Zip"": ""77365-5490"",
        //    ""Address1"": ""24420 FM 1314 Rd Ste 101"",
        //    ""Address2"": """",
        //    ""Phone"": ""2813546010"",
        //    ""Fax"": ""2813546012"",
        //    ""HoursOfOperation"": ""M  08:00 - 04:30  T  08:00 - 04:30  W  08:00 - 04:30  TH  08:00 - 04:30  F  08:00 - 04:30"",
        //    ""MasterAccount"": 1263,
        //    ""TimeZone"": ""CDT"",
        //    ""TZOffset"": -5,
        //    ""Distance"": 22.9,
        //    ""FormFoxEnabled"": true,
        //    ""MarketplaceEnabled"": true,
        //    ""Rating"": 5,
        //    ""Services"": [
        //      {
        //        ""Code"": ""NDOTU"",
        //        ""Rank"": 4.9895,
        //        ""PriceList"": ""Platinum""
        //      }
        //    ],
        //    ""Amenities"": []
        //  },
        //  {
        //    ""Code"": ""FF00087141"",
        //    ""Name"": ""34566 Concentra Houston Intercontinental"",
        //    ""City"": ""Houston"",
        //    ""State"": ""TX"",
        //    ""Zip"": ""77060-2101"",
        //    ""Address1"": ""401 Greens Rd"",
        //    ""Address2"": """",
        //    ""Phone"": ""2818730111"",
        //    ""Fax"": ""2818730660"",
        //    ""HoursOfOperation"": ""M  07:00 - 17:00  T  07:00 - 17:00  W  07:00 - 17:00  TH  07:00 - 17:00  F  07:00 - 17:00"",
        //    ""MasterAccount"": 1516,
        //    ""TimeZone"": ""CDT"",
        //    ""TZOffset"": -5,
        //    ""Distance"": 23.8,
        //    ""FormFoxEnabled"": true,
        //    ""MarketplaceEnabled"": true,
        //    ""Rating"": 5,
        //    ""Services"": [
        //      {
        //        ""Code"": ""NDOTU"",
        //        ""Rank"": 1.985,
        //        ""PriceList"": ""Gold""
        //      }
        //    ],
        //    ""Amenities"": []
        //  },
        //  {
        //    ""Code"": ""FF00067431"",
        //    ""Name"": ""Direct Occupational Centers"",
        //    ""City"": ""Houston"",
        //    ""State"": ""TX"",
        //    ""Zip"": ""77073-5121"",
        //    ""Address1"": ""1920 Rankin Rd Ste 130"",
        //    ""Address2"": """",
        //    ""Phone"": ""2816452781"",
        //    ""Fax"": ""2816452785"",
        //    ""HoursOfOperation"": ""M  08:00 - 16:00  T  08:00 - 16:00  W  08:00 - 16:00  TH  08:00 - 16:00  F  08:00 - 16:30"",
        //    ""MasterAccount"": 1221,
        //    ""TimeZone"": ""CDT"",
        //    ""TZOffset"": -5,
        //    ""Distance"": 24,
        //    ""FormFoxEnabled"": true,
        //    ""MarketplaceEnabled"": true,
        //    ""Rating"": 5,
        //    ""Services"": [
        //      {
        //        ""Code"": ""NDOTU"",
        //        ""Rank"": 4.989,
        //        ""PriceList"": ""Platinum""
        //      }
        //    ],
        //    ""Amenities"": []
        //  },
        //  {
        //    ""Code"": ""FF00092166"",
        //    ""Name"": ""37304 Concentra Jersey Village"",
        //    ""City"": ""Jersey Village"",
        //    ""State"": ""TX"",
        //    ""Zip"": ""77040-1002"",
        //    ""Address1"": ""17410 Northwest Fwy"",
        //    ""Address2"": """",
        //    ""Phone"": ""7134660044"",
        //    ""Fax"": ""7134660106"",
        //    ""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00"",
        //    ""MasterAccount"": 1516,
        //    ""TimeZone"": ""CDT"",
        //    ""TZOffset"": -5,
        //    ""Distance"": 34.1,
        //    ""FormFoxEnabled"": true,
        //    ""MarketplaceEnabled"": true,
        //    ""Rating"": 5,
        //    ""Services"": [
        //      {
        //        ""Code"": ""NDOTU"",
        //        ""Rank"": 1.985,
        //        ""PriceList"": ""Gold""
        //      }
        //    ],
        //    ""Amenities"": []
        //  },
        //  {
        //    ""Code"": ""FF00087142"",
        //    ""Name"": ""34567 Concentra Houston Northwest 290 "",
        //    ""City"": ""Houston"",
        //    ""State"": ""TX"",
        //    ""Zip"": ""77041-5165"",
        //    ""Address1"": ""6360 W Sam Houston Pkwy N Ste 200"",
        //    ""Address2"": """",
        //    ""Phone"": ""7132800400"",
        //    ""Fax"": ""7138960702"",
        //    ""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00"",
        //    ""MasterAccount"": 1516,
        //    ""TimeZone"": ""CDT"",
        //    ""TZOffset"": -5,
        //    ""Distance"": 37.3,
        //    ""FormFoxEnabled"": true,
        //    ""MarketplaceEnabled"": true,
        //    ""Rating"": 5,
        //    ""Services"": [
        //      {
        //        ""Code"": ""NDOTU"",
        //        ""Rank"": 1.985,
        //        ""PriceList"": ""Gold""
        //      }
        //    ],
        //    ""Amenities"": []
        //  },
        //  {
        //    ""Code"": ""FF00087691"",
        //    ""Name"": ""Nationwide Testing Systems - 4141"",
        //    ""City"": ""Houston"",
        //    ""State"": ""TX"",
        //    ""Zip"": ""77092-8743"",
        //    ""Address1"": ""4141 Directors Row Ste A-B"",
        //    ""Address2"": """",
        //    ""Phone"": ""7138698378"",
        //    ""Fax"": ""7138699649"",
        //    ""HoursOfOperation"": ""M  08:30 - 16:30  T  08:30 - 16:30  W  08:30 - 16:30  TH  08:30 - 16:30  F  08:30 - 16:30  S  09:00 - 12:30"",
        //    ""MasterAccount"": 2073,
        //    ""TimeZone"": ""CDT"",
        //    ""TZOffset"": -5,
        //    ""Distance"": 37.9,
        //    ""FormFoxEnabled"": true,
        //    ""MarketplaceEnabled"": true,
        //    ""Rating"": 5,
        //    ""Services"": [
        //      {
        //        ""Code"": ""NDOTU"",
        //        ""Rank"": 1.985,
        //        ""PriceList"": ""Gold""
        //      }
        //    ],
        //    ""Amenities"": []
        //  },
        //  {
        //    ""Code"": ""FF00087143"",
        //    ""Name"": ""34561 Concentra Houston Post Oak"",
        //    ""City"": ""Houston"",
        //    ""State"": ""TX"",
        //    ""Zip"": ""77055-7232"",
        //    ""Address1"": ""1000 N Post Oak Rd"",
        //    ""Address2"": ""Bldg G #100"",
        //    ""Phone"": ""7136864868"",
        //    ""Fax"": ""7136865127"",
        //    ""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00"",
        //    ""MasterAccount"": 1516,
        //    ""TimeZone"": ""CDT"",
        //    ""TZOffset"": -5,
        //    ""Distance"": 39,
        //    ""FormFoxEnabled"": true,
        //    ""MarketplaceEnabled"": true,
        //    ""Rating"": 5,
        //    ""Services"": [
        //      {
        //        ""Code"": ""NDOTU"",
        //        ""Rank"": 1.985,
        //        ""PriceList"": ""Gold""
        //      }
        //    ],
        //    ""Amenities"": []
        //  },
        //  {
        //    ""Code"": ""FF00092167"",
        //    ""Name"": ""37305 Concentra Hempstead"",
        //    ""City"": ""Houston"",
        //    ""State"": ""TX"",
        //    ""Zip"": ""77008-6053"",
        //    ""Address1"": ""9200 Hempstead Rd Ste 137"",
        //    ""Address2"": """",
        //    ""Phone"": ""7138809800"",
        //    ""Fax"": ""7138803330"",
        //    ""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00"",
        //    ""MasterAccount"": 1516,
        //    ""TimeZone"": ""CDT"",
        //    ""TZOffset"": -5,
        //    ""Distance"": 39.1,
        //    ""FormFoxEnabled"": true,
        //    ""MarketplaceEnabled"": true,
        //    ""Rating"": 5,
        //    ""Services"": [
        //      {
        //        ""Code"": ""NDOTU"",
        //        ""Rank"": 1.985,
        //        ""PriceList"": ""Gold""
        //      }
        //    ],
        //    ""Amenities"": []
        //  },
        //  {
        //    ""Code"": ""FF00087144"",
        //    ""Name"": ""35327 Concentra West Houston Katy Freeway"",
        //    ""City"": ""Houston"",
        //    ""State"": ""TX"",
        //    ""Zip"": ""77079-1503"",
        //    ""Address1"": ""12345 Katy Fwy"",
        //    ""Address2"": """",
        //    ""Phone"": ""2816795600"",
        //    ""Fax"": ""2816796510"",
        //    ""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00  S  09:00 - 16:00"",
        //    ""MasterAccount"": 1516,
        //    ""TimeZone"": ""CDT"",
        //    ""TZOffset"": -5,
        //    ""Distance"": 41.2,
        //    ""FormFoxEnabled"": true,
        //    ""MarketplaceEnabled"": true,
        //    ""Rating"": 5,
        //    ""Services"": [
        //      {
        //        ""Code"": ""NDOTU"",
        //        ""Rank"": 1.985,
        //        ""PriceList"": ""Gold""
        //      }
        //    ],
        //    ""Amenities"": []
        //  },
        //  {
        //    ""Code"": ""FF00093055"",
        //    ""Name"": ""Westfield Urgent Care"",
        //    ""City"": ""Houston"",
        //    ""State"": ""TX"",
        //    ""Zip"": ""77073-2404"",
        //    ""Address1"": ""2010 FM 1960 Rd"",
        //    ""Address2"": """",
        //    ""Phone"": ""2818218200"",
        //    ""Fax"": ""2818213692"",
        //    ""HoursOfOperation"": ""M  09:00 - 18:00  T  09:00 - 18:00  W  09:00 - 18:00  TH  09:00 - 18:00  F  09:00 - 18:00  S  09:00 - 14:00"",
        //    ""MasterAccount"": 3021,
        //    ""TimeZone"": ""CDT"",
        //    ""TZOffset"": -5,
        //    ""Distance"": null,
        //    ""FormFoxEnabled"": true,
        //    ""MarketplaceEnabled"": true,
        //    ""Rating"": 4,
        //    ""Services"": [
        //      {
        //        ""Code"": ""NDOTU"",
        //        ""Rank"": 4.99,
        //        ""PriceList"": ""Platinum""
        //      }
        //    ],
        //    ""Amenities"": []
        //  },
        //  {
        //    ""Code"": ""FF00091915"",
        //    ""Name"": ""Fastest Labs of Humble"",
        //    ""City"": ""Humble"",
        //    ""State"": ""TX"",
        //    ""Zip"": ""77338-3606"",
        //    ""Address1"": ""244 FM 1960 Bypass Rd E"",
        //    ""Address2"": """",
        //    ""Phone"": ""8326445785"",
        //    ""Fax"": ""8326445545"",
        //    ""HoursOfOperation"": ""M  09:00 - 17:00  T  09:00 - 17:00  W  09:00 - 17:00  TH  09:00 - 17:00  F  09:00 - 17:00"",
        //    ""MasterAccount"": 1235,
        //    ""TimeZone"": ""CDT"",
        //    ""TZOffset"": -5,
        //    ""Distance"": null,
        //    ""FormFoxEnabled"": true,
        //    ""MarketplaceEnabled"": true,
        //    ""Rating"": 4,
        //    ""Services"": [
        //      {
        //        ""Code"": ""NDOTU"",
        //        ""Rank"": 4.99,
        //        ""PriceList"": ""Platinum""
        //      }
        //    ],
        //    ""Amenities"": []
        //  },
        //  {
        //    ""Code"": ""FF00087263"",
        //    ""Name"": ""Fastest Labs of Houston NW"",
        //    ""City"": ""Houston"",
        //    ""State"": ""TX"",
        //    ""Zip"": ""77041-5056"",
        //    ""Address1"": ""11050 W Little York Rd Bldg A7"",
        //    ""Address2"": """",
        //    ""Phone"": ""7134663278"",
        //    ""Fax"": ""7134661158"",
        //    ""HoursOfOperation"": ""M  08:00 - 18:00  T  08:00 - 18:00  W  08:00 - 18:00  TH  08:00 - 18:00  F  08:00 - 17:00"",
        //    ""MasterAccount"": 1235,
        //    ""TimeZone"": ""CDT"",
        //    ""TZOffset"": -5,
        //    ""Distance"": null,
        //    ""FormFoxEnabled"": true,
        //    ""MarketplaceEnabled"": true,
        //    ""Rating"": 4,
        //    ""Services"": [
        //      {
        //        ""Code"": ""NDOTU"",
        //        ""Rank"": 1.9825,
        //        ""PriceList"": ""Gold""
        //      }
        //    ],
        //    ""Amenities"": []
        //  },
        //  {
        //    ""Code"": ""FF00093822"",
        //    ""Name"": ""WellNow Health - Jersey Village"",
        //    ""City"": ""Jersey Village"",
        //    ""State"": ""TX"",
        //    ""Zip"": ""77040-1114"",
        //    ""Address1"": ""17376 Northwest Fwy"",
        //    ""Address2"": """",
        //    ""Phone"": ""8329198484"",
        //    ""Fax"": ""8329198446"",
        //    ""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00"",
        //    ""MasterAccount"": 2210,
        //    ""TimeZone"": ""CDT"",
        //    ""TZOffset"": -5,
        //    ""Distance"": null,
        //    ""FormFoxEnabled"": true,
        //    ""MarketplaceEnabled"": true,
        //    ""Rating"": 3,
        //    ""Services"": [
        //      {
        //        ""Code"": ""NDOTU"",
        //        ""Rank"": 4.99,
        //        ""PriceList"": ""Platinum""
        //      }
        //    ],
        //    ""Amenities"": []
        //  },
        //  {
        //    ""Code"": ""FF00093873"",
        //    ""Name"": ""S.A.F.E. Drug Testing - Montgomery"",
        //    ""City"": ""Montgomery"",
        //    ""State"": ""TX"",
        //    ""Zip"": ""77356-1984"",
        //    ""Address1"": ""19380 Highway 105 W Ste 511"",
        //    ""Address2"": """",
        //    ""Phone"": ""9364488155"",
        //    ""Fax"": ""9364488177"",
        //    ""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00"",
        //    ""MasterAccount"": 1164,
        //    ""TimeZone"": ""CDT"",
        //    ""TZOffset"": -5,
        //    ""Distance"": null,
        //    ""FormFoxEnabled"": true,
        //    ""MarketplaceEnabled"": true,
        //    ""Rating"": 2,
        //    ""Services"": [
        //      {
        //        ""Code"": ""NDOTU"",
        //        ""Rank"": 1.985,
        //        ""PriceList"": ""Gold""
        //      }
        //    ],
        //    ""Amenities"": []
        //  },
        //  {
        //    ""Code"": ""FF00095556"",
        //    ""Name"": ""Finest Blood LLC"",
        //    ""City"": ""Spring"",
        //    ""State"": ""TX"",
        //    ""Zip"": ""77386-1518"",
        //    ""Address1"": ""25003 Pitkin Rd Ste D600"",
        //    ""Address2"": """",
        //    ""Phone"": ""8326086058"",
        //    ""Fax"": ""8325503838"",
        //    ""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00  S  08:30 - 13:30"",
        //    ""MasterAccount"": 3150,
        //    ""TimeZone"": ""CDT"",
        //    ""TZOffset"": -5,
        //    ""Distance"": null,
        //    ""FormFoxEnabled"": true,
        //    ""MarketplaceEnabled"": true,
        //    ""Services"": [
        //      {
        //        ""Code"": ""NDOTU"",
        //        ""Rank"": 4.99,
        //        ""PriceList"": ""Platinum""
        //      }
        //    ],
        //    ""Amenities"": []
        //  },
        //  {
        //    ""Code"": ""FF00093463"",
        //    ""Name"": ""Simmons and Arnold Services"",
        //    ""City"": ""Houston"",
        //    ""State"": ""TX"",
        //    ""Zip"": ""77090-2538"",
        //    ""Address1"": ""1000 Cypress Creek Pkwy Ste 218"",
        //    ""Address2"": """",
        //    ""Phone"": ""2818365647"",
        //    ""Fax"": ""2819666963"",
        //    ""HoursOfOperation"": ""M  10:00 - 17:00  T  10:00 - 17:00  W  10:00 - 17:00  TH  10:00 - 17:00  F  10:00 - 17:00"",
        //    ""MasterAccount"": 3151,
        //    ""TimeZone"": ""CDT"",
        //    ""TZOffset"": -5,
        //    ""Distance"": null,
        //    ""FormFoxEnabled"": true,
        //    ""MarketplaceEnabled"": true,
        //    ""Services"": [
        //      {
        //        ""Code"": ""NDOTU"",
        //        ""Rank"": 4.989,
        //        ""PriceList"": ""Platinum""
        //      }
        //    ],
        //    ""Amenities"": []
        //  },
        //  {
        //    ""Code"": ""FF00093648"",
        //    ""Name"": ""AFC Urgent Care Cypress"",
        //    ""City"": ""Cypress"",
        //    ""State"": ""TX"",
        //    ""Zip"": ""77433-1973"",
        //    ""Address1"": ""9740 Barker Cypress Rd # 1088"",
        //    ""Address2"": """",
        //    ""Phone"": ""2819448013"",
        //    ""Fax"": ""8326748895"",
        //    ""HoursOfOperation"": ""M  08:00 - 20:00  T  08:00 - 20:00  W  08:00 - 20:00  TH  08:00 - 20:00  F  08:00 - 20:00  S  08:00 - 20:00  Su  08:00 - 20:00"",
        //    ""MasterAccount"": 2955,
        //    ""TimeZone"": ""CDT"",
        //    ""TZOffset"": -5,
        //    ""Distance"": null,
        //    ""FormFoxEnabled"": true,
        //    ""MarketplaceEnabled"": false,
        //    ""Services"": [
        //      {
        //        ""Code"": ""NDOTU"",
        //        ""Rank"": 4.989,
        //        ""PriceList"": ""Platinum""
        //      }
        //    ],
        //    ""Amenities"": []
        //  },
        //  {
        //    ""Code"": ""FF00094117"",
        //    ""Name"": ""Elite Pro-Lab Mobile Services"",
        //    ""City"": ""Houston"",
        //    ""State"": ""TX"",
        //    ""Zip"": ""77095-1463"",
        //    ""Address1"": ""16506 FM 529 Rd Ste 115"",
        //    ""Address2"": """",
        //    ""Phone"": ""8324276743"",
        //    ""Fax"": ""8322017383"",
        //    ""HoursOfOperation"": ""M  08:00 - 12:00  13:00 - 15:00  T  08:00 - 12:00  13:00 - 15:00  W  08:00 - 12:00  13:00 - 15:00  TH  08:00 - 12:00  13:00 - 15:00  F  08:00 - 12:00  13:00 - 15:00"",
        //    ""MasterAccount"": 3369,
        //    ""TimeZone"": ""CDT"",
        //    ""TZOffset"": -5,
        //    ""Distance"": null,
        //    ""FormFoxEnabled"": true,
        //    ""MarketplaceEnabled"": true,
        //    ""Services"": [
        //      {
        //        ""Code"": ""NDOTU"",
        //        ""Rank"": 4.989,
        //        ""PriceList"": ""Platinum""
        //      }
        //    ],
        //    ""Amenities"": []
        //  },
        //  {
        //    ""Code"": ""FF00094388"",
        //    ""Name"": ""Porter Drug and Alcohol Screens - Cleveland"",
        //    ""City"": ""Cleveland"",
        //    ""State"": ""TX"",
        //    ""Zip"": ""77327-4689"",
        //    ""Address1"": ""618 E Houston St"",
        //    ""Address2"": """",
        //    ""Phone"": ""2817616323"",
        //    ""Fax"": ""2817616321"",
        //    ""HoursOfOperation"": ""M  08:00 - 16:00  T  08:00 - 16:00  W  08:00 - 16:00  TH  08:00 - 16:00  F  08:00 - 16:00"",
        //    ""MasterAccount"": 1263,
        //    ""TimeZone"": ""CDT"",
        //    ""TZOffset"": -5,
        //    ""Distance"": null,
        //    ""FormFoxEnabled"": true,
        //    ""MarketplaceEnabled"": true,
        //    ""Services"": [
        //      {
        //        ""Code"": ""NDOTU"",
        //        ""Rank"": 4.99,
        //        ""PriceList"": ""Platinum""
        //      }
        //    ],
        //    ""Amenities"": []
        //  }
        //]
        //";

        //            //// AnalyzeSitesFromFormfox(sitesString, ref __Sites, ref _Sites, ref sendertracker);
        //            //var __Sites = JsonConvert.DeserializeObject<List<FFMPSearchResultTest>>(json);

        //            //var y = 1;


        //            FFMPSearch _f = new FFMPSearch();
        //            SenderTracker _s = new SenderTracker();
        //            List<FFMPSearchResult> __Sites = new List<FFMPSearchResult>();
        //            List<FFMPSearchResult> _Sites = new List<FFMPSearchResult>();
        //            _f.AnalyzeSitesFromFormfox(json, ref __Sites, ref _Sites, ref _s);

        //            var x = 1;

        //        }


        [TestMethod]
        public void pullSites()
        {
            bool ffSiteFound = false;
            FFMPSearch _f = new FFMPSearch();
            SenderTracker _s = new SenderTracker();
            _f.FFMPSearchSearchType = 2;
            _f.PullSites("5901 112th St", "Lubbock", "TX", "79424", 30, ref _s);
            string _pricetier = string.Empty;
            var _sitesFound = 0;
            foreach (string _pt in _f.PriceTiers)
            {
                if (_f.Sites.Exists(s => s.PriceTier.Equals(_pt, StringComparison.InvariantCultureIgnoreCase)))
                {
                    var _sitelistCount = _f.Sites.Where(s => s.PriceTier.Equals(_pt, StringComparison.InvariantCultureIgnoreCase)).ToList().First().SiteList.Count();
                    // var _count = _f.Sites.Where(s => s.PriceTier.Equals(_pt, StringComparison.InvariantCultureIgnoreCase)).ToList().Count();

                    _sitesFound = _sitesFound + _sitelistCount; // ffco.fFMPSearch.Sites.Where(s => s.PriceTier.Equals(_pt, StringComparison.InvariantCultureIgnoreCase)).ToList().Count();
                }

            }
            ffSiteFound = _sitesFound > 0;



            var x = 1;

        }


        public class FFMPSearchResultTest

        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Zip { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public int MasterAccount { get; set; }
            public string TimeZone { get; set; }
            public int TZOffset { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public double Distance { get; set; }
            public bool MarketplaceEnabled { get; set; }
            public List<FFMPService> Services { get; set; }
            public List<object> Amenities { get; set; }
            public bool NoMarketplaceId { get; set; } = false;
        }

        [TestMethod]
        public void pullSitesNullDistace()
        {
            var json = @"
[
  {
    ""Code"": ""FF00090142"",
    ""Name"": ""SpotOn Drug and Alcohol Testing"",
    ""City"": ""Cypress"",
    ""State"": ""TX"",
    ""Zip"": ""77429-5212"",
    ""Address1"": ""12617 Louetta Rd Ste 212"",
    ""Address2"": """",
    ""Phone"": ""8324223596"",
    ""Fax"": ""8324223637"",
    ""HoursOfOperation"": ""M  08:00 - 19:00  T  08:00 - 19:00  W  08:00 - 19:00  TH  08:00 - 19:00  F  08:00 - 18:00  Su  01:00 - 23:30"",
    ""MasterAccount"": 2627,
    ""TimeZone"": ""CDT"",
    ""TZOffset"": -5,
    ""Distance"": 22.3,
    ""FormFoxEnabled"": true,
    ""MarketplaceEnabled"": true,
    ""Rating"": 5,
    ""Services"": [
      {
        ""Code"": ""NDOTU"",
        ""Rank"": 1.985,
        ""PriceList"": ""Gold""
      }
    ],
    ""Amenities"": []
  },
  {
    ""Code"": ""FF00067887"",
    ""Name"": ""Porter Drug and Alcohol Screens - Porter"",
    ""City"": ""Porter"",
    ""State"": ""TX"",
    ""Zip"": ""77365-5490"",
    ""Address1"": ""24420 FM 1314 Rd Ste 101"",
    ""Address2"": """",
    ""Phone"": ""2813546010"",
    ""Fax"": ""2813546012"",
    ""HoursOfOperation"": ""M  08:00 - 04:30  T  08:00 - 04:30  W  08:00 - 04:30  TH  08:00 - 04:30  F  08:00 - 04:30"",
    ""MasterAccount"": 1263,
    ""TimeZone"": ""CDT"",
    ""TZOffset"": -5,
    ""Distance"": 22.9,
    ""FormFoxEnabled"": true,
    ""MarketplaceEnabled"": true,
    ""Rating"": 5,
    ""Services"": [
      {
        ""Code"": ""NDOTU"",
        ""Rank"": 4.9895,
        ""PriceList"": ""Platinum""
      }
    ],
    ""Amenities"": []
  },
  {
    ""Code"": ""FF00087141"",
    ""Name"": ""34566 Concentra Houston Intercontinental"",
    ""City"": ""Houston"",
    ""State"": ""TX"",
    ""Zip"": ""77060-2101"",
    ""Address1"": ""401 Greens Rd"",
    ""Address2"": """",
    ""Phone"": ""2818730111"",
    ""Fax"": ""2818730660"",
    ""HoursOfOperation"": ""M  07:00 - 17:00  T  07:00 - 17:00  W  07:00 - 17:00  TH  07:00 - 17:00  F  07:00 - 17:00"",
    ""MasterAccount"": 1516,
    ""TimeZone"": ""CDT"",
    ""TZOffset"": -5,
    ""Distance"": 23.8,
    ""FormFoxEnabled"": true,
    ""MarketplaceEnabled"": true,
    ""Rating"": 5,
    ""Services"": [
      {
        ""Code"": ""NDOTU"",
        ""Rank"": 1.985,
        ""PriceList"": ""Gold""
      }
    ],
    ""Amenities"": []
  },
  {
    ""Code"": ""FF00067431"",
    ""Name"": ""Direct Occupational Centers"",
    ""City"": ""Houston"",
    ""State"": ""TX"",
    ""Zip"": ""77073-5121"",
    ""Address1"": ""1920 Rankin Rd Ste 130"",
    ""Address2"": """",
    ""Phone"": ""2816452781"",
    ""Fax"": ""2816452785"",
    ""HoursOfOperation"": ""M  08:00 - 16:00  T  08:00 - 16:00  W  08:00 - 16:00  TH  08:00 - 16:00  F  08:00 - 16:30"",
    ""MasterAccount"": 1221,
    ""TimeZone"": ""CDT"",
    ""TZOffset"": -5,
    ""Distance"": 24,
    ""FormFoxEnabled"": true,
    ""MarketplaceEnabled"": true,
    ""Rating"": 5,
    ""Services"": [
      {
        ""Code"": ""NDOTU"",
        ""Rank"": 4.989,
        ""PriceList"": ""Platinum""
      }
    ],
    ""Amenities"": []
  },
  {
    ""Code"": ""FF00092166"",
    ""Name"": ""37304 Concentra Jersey Village"",
    ""City"": ""Jersey Village"",
    ""State"": ""TX"",
    ""Zip"": ""77040-1002"",
    ""Address1"": ""17410 Northwest Fwy"",
    ""Address2"": """",
    ""Phone"": ""7134660044"",
    ""Fax"": ""7134660106"",
    ""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00"",
    ""MasterAccount"": 1516,
    ""TimeZone"": ""CDT"",
    ""TZOffset"": -5,
    ""Distance"": 34.1,
    ""FormFoxEnabled"": true,
    ""MarketplaceEnabled"": true,
    ""Rating"": 5,
    ""Services"": [
      {
        ""Code"": ""NDOTU"",
        ""Rank"": 1.985,
        ""PriceList"": ""Gold""
      }
    ],
    ""Amenities"": []
  },
  {
    ""Code"": ""FF00087142"",
    ""Name"": ""34567 Concentra Houston Northwest 290 "",
    ""City"": ""Houston"",
    ""State"": ""TX"",
    ""Zip"": ""77041-5165"",
    ""Address1"": ""6360 W Sam Houston Pkwy N Ste 200"",
    ""Address2"": """",
    ""Phone"": ""7132800400"",
    ""Fax"": ""7138960702"",
    ""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00"",
    ""MasterAccount"": 1516,
    ""TimeZone"": ""CDT"",
    ""TZOffset"": -5,
    ""Distance"": 37.3,
    ""FormFoxEnabled"": true,
    ""MarketplaceEnabled"": true,
    ""Rating"": 5,
    ""Services"": [
      {
        ""Code"": ""NDOTU"",
        ""Rank"": 1.985,
        ""PriceList"": ""Gold""
      }
    ],
    ""Amenities"": []
  },
  {
    ""Code"": ""FF00087691"",
    ""Name"": ""Nationwide Testing Systems - 4141"",
    ""City"": ""Houston"",
    ""State"": ""TX"",
    ""Zip"": ""77092-8743"",
    ""Address1"": ""4141 Directors Row Ste A-B"",
    ""Address2"": """",
    ""Phone"": ""7138698378"",
    ""Fax"": ""7138699649"",
    ""HoursOfOperation"": ""M  08:30 - 16:30  T  08:30 - 16:30  W  08:30 - 16:30  TH  08:30 - 16:30  F  08:30 - 16:30  S  09:00 - 12:30"",
    ""MasterAccount"": 2073,
    ""TimeZone"": ""CDT"",
    ""TZOffset"": -5,
    ""Distance"": 37.9,
    ""FormFoxEnabled"": true,
    ""MarketplaceEnabled"": true,
    ""Rating"": 5,
    ""Services"": [
      {
        ""Code"": ""NDOTU"",
        ""Rank"": 1.985,
        ""PriceList"": ""Gold""
      }
    ],
    ""Amenities"": []
  },
  {
    ""Code"": ""FF00087143"",
    ""Name"": ""34561 Concentra Houston Post Oak"",
    ""City"": ""Houston"",
    ""State"": ""TX"",
    ""Zip"": ""77055-7232"",
    ""Address1"": ""1000 N Post Oak Rd"",
    ""Address2"": ""Bldg G #100"",
    ""Phone"": ""7136864868"",
    ""Fax"": ""7136865127"",
    ""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00"",
    ""MasterAccount"": 1516,
    ""TimeZone"": ""CDT"",
    ""TZOffset"": -5,
    ""Distance"": 39,
    ""FormFoxEnabled"": true,
    ""MarketplaceEnabled"": true,
    ""Rating"": 5,
    ""Services"": [
      {
        ""Code"": ""NDOTU"",
        ""Rank"": 1.985,
        ""PriceList"": ""Gold""
      }
    ],
    ""Amenities"": []
  },
  {
    ""Code"": ""FF00092167"",
    ""Name"": ""37305 Concentra Hempstead"",
    ""City"": ""Houston"",
    ""State"": ""TX"",
    ""Zip"": ""77008-6053"",
    ""Address1"": ""9200 Hempstead Rd Ste 137"",
    ""Address2"": """",
    ""Phone"": ""7138809800"",
    ""Fax"": ""7138803330"",
    ""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00"",
    ""MasterAccount"": 1516,
    ""TimeZone"": ""CDT"",
    ""TZOffset"": -5,
    ""Distance"": 39.1,
    ""FormFoxEnabled"": true,
    ""MarketplaceEnabled"": true,
    ""Rating"": 5,
    ""Services"": [
      {
        ""Code"": ""NDOTU"",
        ""Rank"": 1.985,
        ""PriceList"": ""Gold""
      }
    ],
    ""Amenities"": []
  },
  {
    ""Code"": ""FF00087144"",
    ""Name"": ""35327 Concentra West Houston Katy Freeway"",
    ""City"": ""Houston"",
    ""State"": ""TX"",
    ""Zip"": ""77079-1503"",
    ""Address1"": ""12345 Katy Fwy"",
    ""Address2"": """",
    ""Phone"": ""2816795600"",
    ""Fax"": ""2816796510"",
    ""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00  S  09:00 - 16:00"",
    ""MasterAccount"": 1516,
    ""TimeZone"": ""CDT"",
    ""TZOffset"": -5,
    ""Distance"": 41.2,
    ""FormFoxEnabled"": true,
    ""MarketplaceEnabled"": true,
    ""Rating"": 5,
    ""Services"": [
      {
        ""Code"": ""NDOTU"",
        ""Rank"": 1.985,
        ""PriceList"": ""Gold""
      }
    ],
    ""Amenities"": []
  },
  {
    ""Code"": ""FF00093055"",
    ""Name"": ""Westfield Urgent Care"",
    ""City"": ""Houston"",
    ""State"": ""TX"",
    ""Zip"": ""77073-2404"",
    ""Address1"": ""2010 FM 1960 Rd"",
    ""Address2"": """",
    ""Phone"": ""2818218200"",
    ""Fax"": ""2818213692"",
    ""HoursOfOperation"": ""M  09:00 - 18:00  T  09:00 - 18:00  W  09:00 - 18:00  TH  09:00 - 18:00  F  09:00 - 18:00  S  09:00 - 14:00"",
    ""MasterAccount"": 3021,
    ""TimeZone"": ""CDT"",
    ""TZOffset"": -5,
    ""Distance"": null,
    ""FormFoxEnabled"": true,
    ""MarketplaceEnabled"": true,
    ""Rating"": 4,
    ""Services"": [
      {
        ""Code"": ""NDOTU"",
        ""Rank"": 4.99,
        ""PriceList"": ""Platinum""
      }
    ],
    ""Amenities"": []
  },
  {
    ""Code"": ""FF00091915"",
    ""Name"": ""Fastest Labs of Humble"",
    ""City"": ""Humble"",
    ""State"": ""TX"",
    ""Zip"": ""77338-3606"",
    ""Address1"": ""244 FM 1960 Bypass Rd E"",
    ""Address2"": """",
    ""Phone"": ""8326445785"",
    ""Fax"": ""8326445545"",
    ""HoursOfOperation"": ""M  09:00 - 17:00  T  09:00 - 17:00  W  09:00 - 17:00  TH  09:00 - 17:00  F  09:00 - 17:00"",
    ""MasterAccount"": 1235,
    ""TimeZone"": ""CDT"",
    ""TZOffset"": -5,
    ""Distance"": null,
    ""FormFoxEnabled"": true,
    ""MarketplaceEnabled"": true,
    ""Rating"": 4,
    ""Services"": [
      {
        ""Code"": ""NDOTU"",
        ""Rank"": 4.99,
        ""PriceList"": ""Platinum""
      }
    ],
    ""Amenities"": []
  },
  {
    ""Code"": ""FF00087263"",
    ""Name"": ""Fastest Labs of Houston NW"",
    ""City"": ""Houston"",
    ""State"": ""TX"",
    ""Zip"": ""77041-5056"",
    ""Address1"": ""11050 W Little York Rd Bldg A7"",
    ""Address2"": """",
    ""Phone"": ""7134663278"",
    ""Fax"": ""7134661158"",
    ""HoursOfOperation"": ""M  08:00 - 18:00  T  08:00 - 18:00  W  08:00 - 18:00  TH  08:00 - 18:00  F  08:00 - 17:00"",
    ""MasterAccount"": 1235,
    ""TimeZone"": ""CDT"",
    ""TZOffset"": -5,
    ""Distance"": null,
    ""FormFoxEnabled"": true,
    ""MarketplaceEnabled"": true,
    ""Rating"": 4,
    ""Services"": [
      {
        ""Code"": ""NDOTU"",
        ""Rank"": 1.9825,
        ""PriceList"": ""Gold""
      }
    ],
    ""Amenities"": []
  },
  {
    ""Code"": ""FF00093822"",
    ""Name"": ""WellNow Health - Jersey Village"",
    ""City"": ""Jersey Village"",
    ""State"": ""TX"",
    ""Zip"": ""77040-1114"",
    ""Address1"": ""17376 Northwest Fwy"",
    ""Address2"": """",
    ""Phone"": ""8329198484"",
    ""Fax"": ""8329198446"",
    ""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00"",
    ""MasterAccount"": 2210,
    ""TimeZone"": ""CDT"",
    ""TZOffset"": -5,
    ""Distance"": null,
    ""FormFoxEnabled"": true,
    ""MarketplaceEnabled"": true,
    ""Rating"": 3,
    ""Services"": [
      {
        ""Code"": ""NDOTU"",
        ""Rank"": 4.99,
        ""PriceList"": ""Platinum""
      }
    ],
    ""Amenities"": []
  },
  {
    ""Code"": ""FF00093873"",
    ""Name"": ""S.A.F.E. Drug Testing - Montgomery"",
    ""City"": ""Montgomery"",
    ""State"": ""TX"",
    ""Zip"": ""77356-1984"",
    ""Address1"": ""19380 Highway 105 W Ste 511"",
    ""Address2"": """",
    ""Phone"": ""9364488155"",
    ""Fax"": ""9364488177"",
    ""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00"",
    ""MasterAccount"": 1164,
    ""TimeZone"": ""CDT"",
    ""TZOffset"": -5,
    ""Distance"": null,
    ""FormFoxEnabled"": true,
    ""MarketplaceEnabled"": true,
    ""Rating"": 2,
    ""Services"": [
      {
        ""Code"": ""NDOTU"",
        ""Rank"": 1.985,
        ""PriceList"": ""Gold""
      }
    ],
    ""Amenities"": []
  },
  {
    ""Code"": ""FF00095556"",
    ""Name"": ""Finest Blood LLC"",
    ""City"": ""Spring"",
    ""State"": ""TX"",
    ""Zip"": ""77386-1518"",
    ""Address1"": ""25003 Pitkin Rd Ste D600"",
    ""Address2"": """",
    ""Phone"": ""8326086058"",
    ""Fax"": ""8325503838"",
    ""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00  S  08:30 - 13:30"",
    ""MasterAccount"": 3150,
    ""TimeZone"": ""CDT"",
    ""TZOffset"": -5,
    ""Distance"": null,
    ""FormFoxEnabled"": true,
    ""MarketplaceEnabled"": true,
    ""Services"": [
      {
        ""Code"": ""NDOTU"",
        ""Rank"": 4.99,
        ""PriceList"": ""Platinum""
      }
    ],
    ""Amenities"": []
  },
  {
    ""Code"": ""FF00093463"",
    ""Name"": ""Simmons and Arnold Services"",
    ""City"": ""Houston"",
    ""State"": ""TX"",
    ""Zip"": ""77090-2538"",
    ""Address1"": ""1000 Cypress Creek Pkwy Ste 218"",
    ""Address2"": """",
    ""Phone"": ""2818365647"",
    ""Fax"": ""2819666963"",
    ""HoursOfOperation"": ""M  10:00 - 17:00  T  10:00 - 17:00  W  10:00 - 17:00  TH  10:00 - 17:00  F  10:00 - 17:00"",
    ""MasterAccount"": 3151,
    ""TimeZone"": ""CDT"",
    ""TZOffset"": -5,
    ""Distance"": null,
    ""FormFoxEnabled"": true,
    ""MarketplaceEnabled"": true,
    ""Services"": [
      {
        ""Code"": ""NDOTU"",
        ""Rank"": 4.989,
        ""PriceList"": ""Platinum""
      }
    ],
    ""Amenities"": []
  },
  {
    ""Code"": ""FF00093648"",
    ""Name"": ""AFC Urgent Care Cypress"",
    ""City"": ""Cypress"",
    ""State"": ""TX"",
    ""Zip"": ""77433-1973"",
    ""Address1"": ""9740 Barker Cypress Rd # 1088"",
    ""Address2"": """",
    ""Phone"": ""2819448013"",
    ""Fax"": ""8326748895"",
    ""HoursOfOperation"": ""M  08:00 - 20:00  T  08:00 - 20:00  W  08:00 - 20:00  TH  08:00 - 20:00  F  08:00 - 20:00  S  08:00 - 20:00  Su  08:00 - 20:00"",
    ""MasterAccount"": 2955,
    ""TimeZone"": ""CDT"",
    ""TZOffset"": -5,
    ""Distance"": null,
    ""FormFoxEnabled"": true,
    ""MarketplaceEnabled"": false,
    ""Services"": [
      {
        ""Code"": ""NDOTU"",
        ""Rank"": 4.989,
        ""PriceList"": ""Platinum""
      }
    ],
    ""Amenities"": []
  },
  {
    ""Code"": ""FF00094117"",
    ""Name"": ""Elite Pro-Lab Mobile Services"",
    ""City"": ""Houston"",
    ""State"": ""TX"",
    ""Zip"": ""77095-1463"",
    ""Address1"": ""16506 FM 529 Rd Ste 115"",
    ""Address2"": """",
    ""Phone"": ""8324276743"",
    ""Fax"": ""8322017383"",
    ""HoursOfOperation"": ""M  08:00 - 12:00  13:00 - 15:00  T  08:00 - 12:00  13:00 - 15:00  W  08:00 - 12:00  13:00 - 15:00  TH  08:00 - 12:00  13:00 - 15:00  F  08:00 - 12:00  13:00 - 15:00"",
    ""MasterAccount"": 3369,
    ""TimeZone"": ""CDT"",
    ""TZOffset"": -5,
    ""Distance"": null,
    ""FormFoxEnabled"": true,
    ""MarketplaceEnabled"": true,
    ""Services"": [
      {
        ""Code"": ""NDOTU"",
        ""Rank"": 4.989,
        ""PriceList"": ""Platinum""
      }
    ],
    ""Amenities"": []
  },
  {
    ""Code"": ""FF00094388"",
    ""Name"": ""Porter Drug and Alcohol Screens - Cleveland"",
    ""City"": ""Cleveland"",
    ""State"": ""TX"",
    ""Zip"": ""77327-4689"",
    ""Address1"": ""618 E Houston St"",
    ""Address2"": """",
    ""Phone"": ""2817616323"",
    ""Fax"": ""2817616321"",
    ""HoursOfOperation"": ""M  08:00 - 16:00  T  08:00 - 16:00  W  08:00 - 16:00  TH  08:00 - 16:00  F  08:00 - 16:00"",
    ""MasterAccount"": 1263,
    ""TimeZone"": ""CDT"",
    ""TZOffset"": -5,
    ""Distance"": null,
    ""FormFoxEnabled"": true,
    ""MarketplaceEnabled"": true,
    ""Services"": [
      {
        ""Code"": ""NDOTU"",
        ""Rank"": 4.99,
        ""PriceList"": ""Platinum""
      }
    ],
    ""Amenities"": []
  }
]
";


            var __Sites = JsonConvert.DeserializeObject<List<FFMPSearchResultTest>>(json);

            var y = 1;

        }

        [TestMethod]
        public void pullReport()
        {
            // public bool GetReportByID(int report_info_id, ReportInfo reportDetails, ref List<OBR_Info> obrList, ref RTFBuilderbase crlReport)
            ReportType reportType = ReportType.LabReport;
            int report_info_id = 4064971;
            ReportInfo reportDetails = null;
            List<OBR_Info> obrList = null;

            RTFBuilderbase crlReport = null;

            HL7ParserDao hl7ParserDao = new HL7ParserDao();
            hl7ParserDao.GetReportByID(report_info_id, reportDetails, ref obrList, ref crlReport);

            string Rtf = crlReport.ToString();
            string TXTversion = WTFRTF.DeRtf(Rtf);
            Rtf = WTFRTF.DeInceptionRTFString(Rtf);
            byte[] bytes = Encoding.ASCII.GetBytes(Rtf);
            string filename = "c:\\logs2\\report.rtf";

            if (File.Exists(filename)) File.Delete(filename);
            FileInfo fileInfo = new FileInfo(filename);
            FileStream fileStream = fileInfo.Open(FileMode.Create,
                FileAccess.Write);

            // Write bytes to file
            fileStream.Write(bytes,
                0, bytes.Length);
            fileStream.Flush();
            fileStream.Close();

            // write out the text version
            filename = "c:\\logs2\\report.txt";
            bytes = Encoding.ASCII.GetBytes(TXTversion);
            if (File.Exists(filename)) File.Delete(filename);
            fileInfo = new FileInfo(filename);
            fileStream = fileInfo.Open(FileMode.Create,
                FileAccess.Write);

            // Write bytes to file
            fileStream.Write(bytes,
                0, bytes.Length);
            fileStream.Flush();
            fileStream.Close();
        }


        [TestMethod]
        public void TestZipUpdater()
        {
            ZipUpdater zipUpdater = new ZipUpdater();

            zipUpdater.DoWork(true);
        }


        [TestMethod]
        public void HL7StageDebug()
        {
            ILogger _logger = new LoggerConfiguration().CreateLogger();
            HL7Stage h = new HL7Stage(_logger);


            h.Gen();

            var x = 1;
        }

        [TestMethod]
        public void HL7StageDebugSpecificDonor()
        {
            ILogger _logger = new LoggerConfiguration().CreateLogger();
            HL7Stage h = new HL7Stage(_logger);

            // Test with specific donor ID
            int donorId = 12345; // Replace with actual test donor ID
            h.Gen(donorId);

            var x = 1;
        }


        [TestMethod]
        public void jsonTypeTest()
        {
            string sitesString = @"[{ 	""Code"": ""FF00086443"", 	""Name"": ""DOT Medical and Drug Testing Services - Grand Prairie"", 	""City"": ""Grand Prairie"", 	""State"": ""TX"", 	""Zip"": ""75050-4229"", 	""Address1"": ""3435 Roy Orr Blvd Ste 200"", 	""Address2"": """", 	""Phone"": ""9725915931"", 	""Fax"": ""2142388091"", 	""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00"", 	""MasterAccount"": 1801, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": 2.2, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 5, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.989, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00087652"", 	""Name"": ""34519 Concentra Las Colinas"", 	""City"": ""Irving"", 	""State"": ""TX"", 	""Zip"": ""75039-3886"", 	""Address1"": ""5910 N MacArthur Blvd Ste 133"", 	""Address2"": """", 	""Phone"": ""9725548494"", 	""Fax"": ""9724384647"", 	""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00"", 	""MasterAccount"": 1516, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": 7.1, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 5, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 1.985, 		""PriceList"": ""Gold"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00068000"", 	""Name"": ""ARCpoint Labs of Irving"", 	""City"": ""Irving"", 	""State"": ""TX"", 	""Zip"": ""75063-2503"", 	""Address1"": ""8925 Sterling St Ste 255"", 	""Address2"": """", 	""Phone"": ""4692097400"", 	""Fax"": ""4692097388"", 	""HoursOfOperation"": ""M  08:30 - 16:00  T  08:30 - 16:30  W  08:30 - 16:30  TH  08:30 - 16:30  F  08:30 - 16:00"", 	""MasterAccount"": 1272, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": 8.9, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 5, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.989, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00087625"", 	""Name"": ""34517 Concentra Arlington North"", 	""City"": ""Arlington"", 	""State"": ""TX"", 	""Zip"": ""76006-7408"", 	""Address1"": ""2160 E Lamar Blvd"", 	""Address2"": """", 	""Phone"": ""9729880441"", 	""Fax"": ""9726410054"", 	""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00  S  09:00 - 16:00"", 	""MasterAccount"": 1516, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": 9.3, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 5, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 1.985, 		""PriceList"": ""Gold"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00092173"", 	""Name"": ""37310 Concentra Empire Central"", 	""City"": ""Dallas"", 	""State"": ""TX"", 	""Zip"": ""75247-4081"", 	""Address1"": ""1450 Empire Central Dr Ste 100"", 	""Address2"": """", 	""Phone"": ""2149055000"", 	""Fax"": ""2149055015"", 	""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00"", 	""MasterAccount"": 1516, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": 10.8, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 5, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 1.985, 		""PriceList"": ""Gold"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00087640"", 	""Name"": ""34520 Concentra Stemmons"", 	""City"": ""Dallas"", 	""State"": ""TX"", 	""Zip"": ""75247-6103"", 	""Address1"": ""2920 N Stemmons Fwy"", 	""Address2"": """", 	""Phone"": ""2146302331"", 	""Fax"": ""2149051323"", 	""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00  S  09:00 - 17:00"", 	""MasterAccount"": 1516, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": 11.3, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 5, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 1.985, 		""PriceList"": ""Gold"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00087634"", 	""Name"": ""34525 Concentra Carrollton"", 	""City"": ""Carrollton"", 	""State"": ""TX"", 	""Zip"": ""75006-6891"", 	""Address1"": ""1345 Valwood Pkwy Ste 306"", 	""Address2"": """", 	""Phone"": ""9724846435"", 	""Fax"": ""9724846785"", 	""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00"", 	""MasterAccount"": 1516, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": 12.7, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 5, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 1.985, 		""PriceList"": ""Gold"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00092172"", 	""Name"": ""37309 Concentra Frankford"", 	""City"": ""Carrollton"", 	""State"": ""TX"", 	""Zip"": ""75007-4643"", 	""Address1"": ""1837 W Frankford Rd Ste 116"", 	""Address2"": """", 	""Phone"": ""9722361941"", 	""Fax"": ""9722361955"", 	""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00"", 	""MasterAccount"": 1516, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": 14.6, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 5, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 1.985, 		""PriceList"": ""Gold"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00087626"", 	""Name"": ""34521 Concentra Arlington South"", 	""City"": ""Arlington"", 	""State"": ""TX"", 	""Zip"": ""76018-1126"", 	""Address1"": ""511 E Interstate 20"", 	""Address2"": """", 	""Phone"": ""8172615166"", 	""Fax"": ""8172755432"", 	""HoursOfOperation"": ""M  08:00 - 18:00  T  08:00 - 18:00  W  08:00 - 18:00  TH  08:00 - 18:00  F  08:00 - 18:00"", 	""MasterAccount"": 1516, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": 16.7, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 5, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 1.985, 		""PriceList"": ""Gold"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00000498"", 	""Name"": ""Any Lab Test Now - Arlington"", 	""City"": ""Arlington"", 	""State"": ""TX"", 	""Zip"": ""76018-1148"", 	""Address1"": ""4645 Matlock Rd Ste 103"", 	""Address2"": """", 	""Phone"": ""8177840100"", 	""Fax"": ""8173856384"", 	""HoursOfOperation"": ""M  09:00 - 18:00  T  09:00 - 18:00  W  09:00 - 18:00  TH  09:00 - 18:00  F  09:00 - 18:00  S  10:00 - 14:00"", 	""MasterAccount"": 430, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": 18.0, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 5, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.99, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00087651"", 	""Name"": ""35371 Concentra Lewisville"", 	""City"": ""Lewisville"", 	""State"": ""TX"", 	""Zip"": ""75067-2314"", 	""Address1"": ""2403 S Stemmons Fwy Ste 103"", 	""Address2"": """", 	""Phone"": ""9728292999"", 	""Fax"": ""9724597929"", 	""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00"", 	""MasterAccount"": 1516, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 5, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 1.985, 		""PriceList"": ""Gold"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00087647"", 	""Name"": ""34535 Concentra Redbird"", 	""City"": ""Dallas"", 	""State"": ""TX"", 	""Zip"": ""75237-1800"", 	""Address1"": ""5520 S Westmoreland Rd Ste 200"", 	""Address2"": """", 	""Phone"": ""2144678210"", 	""Fax"": ""2144678192"", 	""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00"", 	""MasterAccount"": 1516, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 5, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 1.985, 		""PriceList"": ""Gold"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00089387"", 	""Name"": ""Fastest Labs of Addison"", 	""City"": ""Addison"", 	""State"": ""TX"", 	""Zip"": ""75001-4381"", 	""Address1"": ""4021 Belt Line Rd Ste 207"", 	""Address2"": """", 	""Phone"": ""9726852795"", 	""Fax"": ""9726852692"", 	""HoursOfOperation"": ""M  09:00 - 17:30  T  09:00 - 17:30  W  09:00 - 17:30  TH  09:00 - 17:30  F  09:00 - 17:00"", 	""MasterAccount"": 1235, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 5, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.9895, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00087619"", 	""Name"": ""34536 Concentra Addison-DFW"", 	""City"": ""Addison"", 	""State"": ""TX"", 	""Zip"": ""75001-4259"", 	""Address1"": ""15810 Midway Rd"", 	""Address2"": """", 	""Phone"": ""9724588111"", 	""Fax"": ""9724587776"", 	""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00"", 	""MasterAccount"": 1516, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 5, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 1.985, 		""PriceList"": ""Gold"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00087638"", 	""Name"": ""34530 Concentra Upper Greenville"", 	""City"": ""Dallas"", 	""State"": ""TX"", 	""Zip"": ""75206-2912"", 	""Address1"": ""5601 Greenville Ave"", 	""Address2"": """", 	""Phone"": ""2148216007"", 	""Fax"": ""2148216149"", 	""HoursOfOperation"": ""M  08:00 - 20:00  T  08:00 - 20:00  W  08:00 - 20:00  TH  08:00 - 20:00  F  08:00 - 20:00  S  09:00 - 16:00"", 	""MasterAccount"": 1516, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 5, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 1.985, 		""PriceList"": ""Gold"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00087657"", 	""Name"": ""34533 Concentra Ft. Worth Fossil Creek"", 	""City"": ""Fort Worth"", 	""State"": ""TX"", 	""Zip"": ""76137-2422"", 	""Address1"": ""4060 Sandshell Dr"", 	""Address2"": """", 	""Phone"": ""8173069777"", 	""Fax"": ""8173069780"", 	""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00"", 	""MasterAccount"": 1516, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 5, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 1.985, 		""PriceList"": ""Gold"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00087030"", 	""Name"": ""AccuScreen Drug and Alcohol Testing - DeSoto"", 	""City"": ""Desoto"", 	""State"": ""TX"", 	""Zip"": ""75115-2417"", 	""Address1"": ""1607 Falcon Dr Ste 102"", 	""Address2"": """", 	""Phone"": ""9729577539"", 	""Fax"": ""2149604221"", 	""HoursOfOperation"": ""M  08:30 - 17:00  T  08:30 - 17:00  W  08:30 - 17:00  TH  08:30 - 17:00  F  08:30 - 17:00"", 	""MasterAccount"": 307, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 5, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.99, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00086810"", 	""Name"": ""BlueStar Diagnostics - White Rock Lake"", 	""City"": ""Dallas"", 	""State"": ""TX"", 	""Zip"": ""75218-2612"", 	""Address1"": ""10611 Garland Rd Ste 112"", 	""Address2"": """", 	""Phone"": ""4697785222"", 	""Fax"": ""4697785373"", 	""HoursOfOperation"": ""M  08:30 - 16:30  T  08:30 - 16:30  W  08:30 - 16:30  TH  08:30 - 16:30  F  08:30 - 16:30"", 	""MasterAccount"": 1674, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 5, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.9895, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00087656"", 	""Name"": ""34518 Concentra Garland"", 	""City"": ""Garland"", 	""State"": ""TX"", 	""Zip"": ""75042-7793"", 	""Address1"": ""1621 S Jupiter Rd Ste 101"", 	""Address2"": """", 	""Phone"": ""2143407555"", 	""Fax"": ""2143403980"", 	""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00"", 	""MasterAccount"": 1516, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 5, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 1.985, 		""PriceList"": ""Gold"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00087649"", 	""Name"": ""34526 Concentra Mesquite"", 	""City"": ""Mesquite"", 	""State"": ""TX"", 	""Zip"": ""75149-1027"", 	""Address1"": ""4928 Samuell Blvd"", 	""Address2"": """", 	""Phone"": ""2143281400"", 	""Fax"": ""2143282884"", 	""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00"", 	""MasterAccount"": 1516, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 5, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 1.985, 		""PriceList"": ""Gold"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00087658"", 	""Name"": ""34531 Concentra Ft. Worth Forest Park"", 	""City"": ""Fort Worth"", 	""State"": ""TX"", 	""Zip"": ""76102"", 	""Address1"": ""2500 West Freeway (I30)"", 	""Address2"": ""Ste. 100"", 	""Phone"": ""8178828700"", 	""Fax"": ""8178828707"", 	""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00  S  08:00 - 16:00"", 	""MasterAccount"": 1516, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 5, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 1.985, 		""PriceList"": ""Gold"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00087648"", 	""Name"": ""34528 Concentra Plano"", 	""City"": ""Plano"", 	""State"": ""TX"", 	""Zip"": ""75074-1009"", 	""Address1"": ""1300 N Central Expy"", 	""Address2"": """", 	""Phone"": ""9725782212"", 	""Fax"": ""9728817666"", 	""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00"", 	""MasterAccount"": 1516, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 5, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 1.985, 		""PriceList"": ""Gold"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00086811"", 	""Name"": ""BlueStar Diagnostics - Plano"", 	""City"": ""Plano"", 	""State"": ""TX"", 	""Zip"": ""75074-8844"", 	""Address1"": ""720 E Park Blvd Ste 102"", 	""Address2"": """", 	""Phone"": ""4696567999"", 	""Fax"": ""4693314177"", 	""HoursOfOperation"": ""M  08:30 - 17:00  T  08:30 - 17:00  W  08:00 - 17:00  TH  08:30 - 17:00  F  08:30 - 17:00  S  08:30 - 12:30"", 	""MasterAccount"": 1674, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 5, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.9895, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00087251"", 	""Name"": ""BlueStar Diagnostics - Frisco"", 	""City"": ""Frisco"", 	""State"": ""TX"", 	""Zip"": ""75035-5719"", 	""Address1"": ""9300 John Hickman Pkwy Ste 901"", 	""Address2"": """", 	""Phone"": ""4692941744"", 	""Fax"": ""4692941799"", 	""HoursOfOperation"": ""M  08:30 - 16:30  T  08:30 - 16:30  W  08:30 - 16:30  TH  08:30 - 16:30  F  08:30 - 16:30"", 	""MasterAccount"": 1674, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 5, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.9895, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00087659"", 	""Name"": ""35319 Concentra Frisco"", 	""City"": ""Frisco"", 	""State"": ""TX"", 	""Zip"": ""75034-4416"", 	""Address1"": ""8756 Teel Pkwy Ste 350"", 	""Address2"": """", 	""Phone"": ""9727125454"", 	""Fax"": ""9727125442"", 	""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00"", 	""MasterAccount"": 1516, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 5, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 1.985, 		""PriceList"": ""Gold"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00087633"", 	""Name"": ""34524 Concentra Burleson"", 	""City"": ""Burleson"", 	""State"": ""TX"", 	""Zip"": ""76028-2664"", 	""Address1"": ""811 NE Alsbury Blvd Ste 800"", 	""Address2"": """", 	""Phone"": ""8172937311"", 	""Fax"": ""8175511066"", 	""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00"", 	""MasterAccount"": 1516, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 5, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 1.985, 		""PriceList"": ""Gold"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00088231"", 	""Name"": ""Any Lab Test Now - Irving"", 	""City"": ""Irving"", 	""State"": ""TX"", 	""Zip"": ""75062-5243"", 	""Address1"": ""2540N N Belt Line Rd"", 	""Address2"": """", 	""Phone"": ""9728875023"", 	""Fax"": ""4695864509"", 	""HoursOfOperation"": ""M  08:30 - 18:30  T  08:30 - 18:30  W  08:30 - 18:30  TH  08:30 - 18:30  F  08:30 - 18:30  S  09:00 - 14:00"", 	""MasterAccount"": 430, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 4, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.989, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00012108"", 	""Name"": ""Any Lab Test Now - Euless"", 	""City"": ""Euless"", 	""State"": ""TX"", 	""Zip"": ""76039-3366"", 	""Address1"": ""1060 N Main St Ste 106"", 	""Address2"": """", 	""Phone"": ""8173548378"", 	""Fax"": ""8173548379"", 	""HoursOfOperation"": ""M  12:00 - 16:00  T  12:00 - 16:00  W  12:00 - 16:00  TH  12:00 - 16:00  F  12:00 - 16:00"", 	""MasterAccount"": 430, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 4, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.989, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00067496"", 	""Name"": ""AccuScreen - Dallas"", 	""City"": ""Dallas"", 	""State"": ""TX"", 	""Zip"": ""75235-2207"", 	""Address1"": ""1720 Regal Row Ste 238"", 	""Address2"": """", 	""Phone"": ""2147160619"", 	""Fax"": ""2149604221"", 	""HoursOfOperation"": ""M  08:30 - 17:00  T  08:30 - 17:00  W  08:30 - 17:00  TH  08:30 - 17:00  F  08:30 - 17:00  S  09:00 - 13:00"", 	""MasterAccount"": 307, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 4, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.99, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00000458"", 	""Name"": ""Allied Compliance Services, Inc - Hurst"", 	""City"": ""Hurst"", 	""State"": ""TX"", 	""Zip"": ""76053-4849"", 	""Address1"": ""951 W Pipeline Rd Ste 320"", 	""Address2"": """", 	""Phone"": ""8175899998"", 	""Fax"": ""8175890809"", 	""HoursOfOperation"": ""M  08:00 - 15:00  T  08:00 - 15:00  W  08:00 - 15:00  TH  08:00 - 15:00  F  08:00 - 15:00"", 	""MasterAccount"": 129, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 4, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.99, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00086151"", 	""Name"": ""Drug Test First"", 	""City"": ""Dallas"", 	""State"": ""TX"", 	""Zip"": ""75237-2467"", 	""Address1"": ""4343 W Camp Wisdom Rd Ste 213"", 	""Address2"": """", 	""Phone"": ""9722834300"", 	""Fax"": ""9722834302"", 	""HoursOfOperation"": ""M  08:00 - 18:00  T  08:00 - 18:00  W  08:00 - 18:00  TH  08:00 - 18:00  F  08:00 - 18:00  S  08:00 - 13:00"", 	""MasterAccount"": 1724, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 4, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.989, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00000387"", 	""Name"": ""AccuTrace Testing Inc."", 	""City"": ""Arlington"", 	""State"": ""TX"", 	""Zip"": ""76016-2791"", 	""Address1"": ""3921 W Green Oaks Blvd Ste D"", 	""Address2"": """", 	""Phone"": ""8174961600"", 	""Fax"": ""8174969475"", 	""HoursOfOperation"": ""M  08:00 - 16:30  T  08:00 - 16:30  W  08:00 - 16:30  TH  08:00 - 16:30  F  08:00 - 16:30"", 	""MasterAccount"": 250, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 4, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 3.988, 		""PriceList"": ""Gold Plus"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00093899"", 	""Name"": ""ArcPoint Labs of Fort Worth - Downtown"", 	""City"": ""Fort Worth"", 	""State"": ""TX"", 	""Zip"": ""76111-2333"", 	""Address1"": ""2757 Airport Fwy"", 	""Address2"": """", 	""Phone"": ""6827075474"", 	""Fax"": ""6822138889"", 	""HoursOfOperation"": ""M  08:30 - 17:00  T  08:30 - 17:00  W  08:30 - 17:00  TH  08:30 - 17:00  F  08:30 - 17:00"", 	""MasterAccount"": 3299, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 4, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 3.987, 		""PriceList"": ""Gold Plus"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00089168"", 	""Name"": ""Any Lab Test Now - North Ft Worth Alliance"", 	""City"": ""Fort Worth"", 	""State"": ""TX"", 	""Zip"": ""76131-4300"", 	""Address1"": ""2700 Western Center Blvd Ste 100"", 	""Address2"": """", 	""Phone"": ""8173490991"", 	""Fax"": ""8173490937"", 	""HoursOfOperation"": ""M  08:00 - 18:00  T  08:00 - 18:00  W  08:00 - 18:00  TH  08:00 - 18:00  F  08:00 - 18:00  S  10:00 - 14:00"", 	""MasterAccount"": 430, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 4, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 2.986, 		""PriceList"": ""Gold"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00089749"", 	""Name"": ""Test First Drug & Alcohol Testing - Dallas"", 	""City"": ""Dallas"", 	""State"": ""TX"", 	""Zip"": ""75228-7137"", 	""Address1"": ""6300 Samuell Blvd Ste 115"", 	""Address2"": """", 	""Phone"": ""2142756300"", 	""Fax"": ""2142756302"", 	""HoursOfOperation"": ""M  09:00 - 18:00  T  09:00 - 18:00  W  09:00 - 18:00  TH  09:00 - 18:00  F  09:00 - 18:00  S  09:00 - 13:00"", 	""MasterAccount"": 2491, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 4, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.99, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00000499"", 	""Name"": ""Any Lab Test Now - Ft Worth"", 	""City"": ""Ft Worth"", 	""State"": ""TX"", 	""Zip"": ""76109-5800"", 	""Address1"": ""5512 Bellaire Dr S Ste J"", 	""Address2"": """", 	""Phone"": ""8173774555"", 	""Fax"": ""8173774558"", 	""HoursOfOperation"": ""M  09:00 - 15:30  T  09:00 - 15:30  W  09:00 - 15:30  TH  09:00 - 15:30  F  09:00 - 15:30  S  10:00 - 12:00"", 	""MasterAccount"": 430, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 4, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.99, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00086145"", 	""Name"": ""ARCpoint Labs of Denton"", 	""City"": ""Denton"", 	""State"": ""TX"", 	""Zip"": ""76207-3408"", 	""Address1"": ""4234 I-35 Frontage Rd"", 	""Address2"": """", 	""Phone"": ""9403802988"", 	""Fax"": ""9403804088"", 	""HoursOfOperation"": ""M  08:30 - 05:00  T  08:30 - 05:00  W  08:30 - 05:00  TH  08:30 - 05:00  F  08:30 - 05:00"", 	""MasterAccount"": 1722, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 4, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.9895, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00000142"", 	""Name"": ""Reliance Healthcare - Grand Prairie"", 	""City"": ""Grand Prairie"", 	""State"": ""TX"", 	""Zip"": ""75050-1033"", 	""Address1"": ""2100 N State Highway 360 Ste 1207"", 	""Address2"": """", 	""Phone"": ""9722061909"", 	""Fax"": ""9726061479"", 	""HoursOfOperation"": ""M  10:00 - 16:00  T  10:00 - 16:00  W  10:00 - 16:00  TH  10:00 - 16:00  F  10:00 - 16:00"", 	""MasterAccount"": 57, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 3, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.989, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00092830"", 	""Name"": ""DOT Medical and Drug Testing Services - Dallas"", 	""City"": ""Dallas"", 	""State"": ""TX"", 	""Zip"": ""75247-3625"", 	""Address1"": ""1430 Regal Row Ste 300"", 	""Address2"": """", 	""Phone"": ""9728439494"", 	""Fax"": ""9728439493"", 	""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00"", 	""MasterAccount"": 1801, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 3, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.989, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00079535"", 	""Name"": ""Safe at Work"", 	""City"": ""Arlington"", 	""State"": ""TX"", 	""Zip"": ""76010-1047"", 	""Address1"": ""623 W Main St"", 	""Address2"": """", 	""Phone"": ""8177958300"", 	""Fax"": ""8177958303"", 	""HoursOfOperation"": ""M  09:00 - 12:00  13:30 - 16:30  T  09:00 - 12:00  13:30 - 16:30  W  09:00 - 12:00  13:30 - 16:30  TH  09:00 - 12:00  13:30 - 16:30  F  09:00 - 12:00  13:30 - 16:30"", 	""MasterAccount"": 1361, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 3, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 3.987, 		""PriceList"": ""Gold Plus"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00000143"", 	""Name"": ""Reliance Healthcare - Fort Worth"", 	""City"": ""Fort Worth"", 	""State"": ""TX"", 	""Zip"": ""76137-1940"", 	""Address1"": ""3629 Western Center Blvd Ste 221"", 	""Address2"": """", 	""Phone"": ""8178690006"", 	""Fax"": ""8178690009"", 	""HoursOfOperation"": ""M  10:30 - 16:00  T  10:30 - 16:00  W  10:30 - 16:00  TH  10:30 - 16:00  F  10:30 - 16:00"", 	""MasterAccount"": 57, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 3, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.989, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00086751"", 	""Name"": ""AccuTrace Testing Inc. - Fort Worth"", 	""City"": ""Fort Worth"", 	""State"": ""TX"", 	""Zip"": ""76102-2245"", 	""Address1"": ""515 E Weatherford St"", 	""Address2"": """", 	""Phone"": ""8173329911"", 	""Fax"": ""8173329933"", 	""HoursOfOperation"": ""M  08:30 - 16:00  T  08:30 - 16:00  W  08:30 - 16:00  TH  08:30 - 16:00  F  08:30 - 16:00"", 	""MasterAccount"": 250, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 3, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 3.988, 		""PriceList"": ""Gold Plus"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00088922"", 	""Name"": ""Fastest Labs of Plano"", 	""City"": ""Plano"", 	""State"": ""TX"", 	""Zip"": ""75074-8592"", 	""Address1"": ""1104 Summit Ave Ste 101"", 	""Address2"": """", 	""Phone"": ""4696619020"", 	""Fax"": ""4696619313"", 	""HoursOfOperation"": ""M  08:30 - 17:00  T  08:30 - 17:00  W  08:30 - 17:00  TH  08:30 - 17:00  F  08:30 - 17:00"", 	""MasterAccount"": 1235, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 3, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.989, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00094034"", 	""Name"": ""Any Lab Test Now - Burleson"", 	""City"": ""Burleson"", 	""State"": ""TX"", 	""Zip"": ""76028-2660"", 	""Address1"": ""671 NE Alsbury Blvd Ste A"", 	""Address2"": """", 	""Phone"": ""8177448801"", 	""Fax"": ""8177447988"", 	""HoursOfOperation"": ""M  08:30 - 17:30  T  08:30 - 17:30  W  08:30 - 17:30  TH  08:30 - 17:30  F  08:30 - 17:30  S  10:00 - 14:00"", 	""MasterAccount"": 430, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 3, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.99, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00093931"", 	""Name"": ""ARCpoint Labs of Southwest Fort Worth"", 	""City"": ""Fort Worth"", 	""State"": ""TX"", 	""Zip"": ""76132-3911"", 	""Address1"": ""7100 Oakmont Blvd Ste 104"", 	""Address2"": """", 	""Phone"": ""8554272768"", 	""Fax"": ""8179350487"", 	""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00"", 	""MasterAccount"": 3311, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 3, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.989, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00085462"", 	""Name"": ""Assure Testing of Texas, Inc"", 	""City"": ""Crowley"", 	""State"": ""TX"", 	""Zip"": ""76036-2600"", 	""Address1"": ""200 E Main St Ste B"", 	""Address2"": """", 	""Phone"": ""8174264994"", 	""Fax"": ""8174264992"", 	""HoursOfOperation"": ""M  08:00 - 15:00  T  08:00 - 15:00  W  08:00 - 15:00  TH  08:00 - 15:00  F  08:00 - 15:00"", 	""MasterAccount"": 1567, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 3, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.989, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00084792"", 	""Name"": ""North Texas Screening, LLC"", 	""City"": ""Denton"", 	""State"": ""TX"", 	""Zip"": ""76209-4276"", 	""Address1"": ""604 N Bell Ave"", 	""Address2"": """", 	""Phone"": ""9405663388"", 	""Fax"": ""4694641780"", 	""HoursOfOperation"": ""M  09:00 - 17:00  T  09:00 - 17:00  W  09:00 - 17:00  TH  09:00 - 17:00  F  09:00 - 16:00"", 	""MasterAccount"": 1483, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 3, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.99, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00092348"", 	""Name"": ""MedWayOHS. Inc. - Arlington"", 	""City"": ""Arlington"", 	""State"": ""TX"", 	""Zip"": ""76011-4612"", 	""Address1"": ""110 W Randol Mill Rd Ste 120C"", 	""Address2"": """", 	""Phone"": ""8175067998"", 	""Fax"": ""8175063294"", 	""HoursOfOperation"": ""M  09:00 - 17:00  T  09:00 - 17:00  W  09:00 - 17:00  TH  09:00 - 17:00  F  09:00 - 17:00"", 	""MasterAccount"": 2844, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 2, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 3.987, 		""PriceList"": ""Gold Plus"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00090989"", 	""Name"": ""USA Mobile Drug Testing of Dallas"", 	""City"": ""Mesquite"", 	""State"": ""TX"", 	""Zip"": ""75150-7009"", 	""Address1"": ""4200 Gus Thomasson Rd Ste 107"", 	""Address2"": """", 	""Phone"": ""2142840695"", 	""Fax"": """", 	""HoursOfOperation"": ""M  09:00 - 17:00  T  09:00 - 17:00  W  09:00 - 17:00  TH  09:00 - 17:00  F  09:00 - 17:00"", 	""MasterAccount"": 2639, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 2, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.99, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00086813"", 	""Name"": ""SurScan, Inc."", 	""City"": ""Plano"", 	""State"": ""TX"", 	""Zip"": ""75074-5565"", 	""Address1"": ""2030 G Ave Ste 1102"", 	""Address2"": """", 	""Phone"": ""9726331388"", 	""Fax"": ""7753703031"", 	""HoursOfOperation"": ""M  09:00 - 12:00  13:00 - 16:00  T  09:00 - 12:00  13:00 - 16:00  W  09:00 - 12:00  13:00 - 16:00  TH  09:00 - 12:00  13:00 - 16:00  F  09:00 - 12:00  13:00 - 16:00"", 	""MasterAccount"": 1913, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Rating"": 1, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.99, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00092349"", 	""Name"": ""Lyrad Health LLC"", 	""City"": ""Grand Prairie"", 	""State"": ""TX"", 	""Zip"": ""75051-8386"", 	""Address1"": ""2701 Osler Dr Ste 2"", 	""Address2"": """", 	""Phone"": ""9726393992"", 	""Fax"": ""8336286624"", 	""HoursOfOperation"": ""M  09:00 - 16:00  T  09:00 - 16:00  W  09:00 - 16:00  TH  09:00 - 16:00  F  09:00 - 16:00"", 	""MasterAccount"": 2845, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 3.987, 		""PriceList"": ""Gold Plus"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00095308"", 	""Name"": ""JTD Services, Inc"", 	""City"": ""Fort Worth"", 	""State"": ""TX"", 	""Zip"": ""76180-7611"", 	""Address1"": ""8713 Airport Fwy Ste 318"", 	""Address2"": """", 	""Phone"": ""8174287795"", 	""Fax"": """", 	""HoursOfOperation"": null, 	""MasterAccount"": 3841, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.99, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00095855"", 	""Name"": ""Any Lab Test Now - Southlake"", 	""City"": ""Southlake"", 	""State"": ""TX"", 	""Zip"": ""76092-6163"", 	""Address1"": ""500 W Southlake Blvd Ste 134"", 	""Address2"": """", 	""Phone"": ""6822685522"", 	""Fax"": ""8178873824"", 	""HoursOfOperation"": ""M  08:00 - 18:00  T  08:00 - 18:00  W  08:00 - 18:00  TH  08:00 - 18:00  F  08:00 - 18:00  S  10:00 - 14:00"", 	""MasterAccount"": 430, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.989, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00095777"", 	""Name"": ""Staff Labs, LLC"", 	""City"": ""Fort Worth"", 	""State"": ""TX"", 	""Zip"": ""76112-6500"", 	""Address1"": ""6001 Rich St"", 	""Address2"": """", 	""Phone"": ""8177641999"", 	""Fax"": ""8175346101"", 	""HoursOfOperation"": ""M  09:00 - 17:00  T  09:00 - 17:00  W  09:00 - 17:00  TH  09:00 - 17:00  F  09:00 - 17:00"", 	""MasterAccount"": 4071, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": false, 	""IsMPOnly"": false, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.99, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00086303"", 	""Name"": ""Watchdog Screening and HR Solutions-TE195"", 	""City"": ""Lewisville"", 	""State"": ""TX"", 	""Zip"": ""75067-4200"", 	""Address1"": ""105 Kathryn Dr Ste D"", 	""Address2"": """", 	""Phone"": ""8009722054"", 	""Fax"": ""2142795032"", 	""HoursOfOperation"": ""M  09:00 - 18:00  T  09:00 - 18:00  W  09:00 - 18:00  TH  09:00 - 18:00  F  09:00 - 18:00  S  09:00 - 18:00"", 	""MasterAccount"": 1776, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.99, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00095818"", 	""Name"": ""Axis Occupational Health"", 	""City"": ""Dallas"", 	""State"": ""TX"", 	""Zip"": ""75230-2009"", 	""Address1"": ""12700 Hillcrest Rd Ste 125"", 	""Address2"": """", 	""Phone"": ""4696467246"", 	""Fax"": ""4696467246"", 	""HoursOfOperation"": ""M  10:00 - 18:00  T  10:00 - 18:00  W  13:00 - 18:00  TH  10:00 - 18:00  F  10:00 - 18:00"", 	""MasterAccount"": 4089, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.989, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00095374"", 	""Name"": ""DOT Medical and Drug Testing Services - Haltom City"", 	""City"": ""Haltom City"", 	""State"": ""TX"", 	""Zip"": ""76117-1720"", 	""Address1"": ""4017 Clay Ave Ste F"", 	""Address2"": """", 	""Phone"": ""8179848001"", 	""Fax"": ""8179848002"", 	""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00"", 	""MasterAccount"": 1801, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.989, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00094740"", 	""Name"": ""ProSalutem"", 	""City"": ""Dallas"", 	""State"": ""TX"", 	""Zip"": ""75243-0586"", 	""Address1"": ""11882 Greenville Ave Ste B100"", 	""Address2"": """", 	""Phone"": ""4692942183"", 	""Fax"": """", 	""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00"", 	""MasterAccount"": 3633, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 3.988, 		""PriceList"": ""Gold Plus"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00095820"", 	""Name"": ""DOT Medical and Drug Testing Services - Lancaster"", 	""City"": ""Lancaster"", 	""State"": ""TX"", 	""Zip"": ""75134-1519"", 	""Address1"": ""3241 Danieldale Rd Ste 100"", 	""Address2"": """", 	""Phone"": ""4697223610"", 	""Fax"": ""4697223611"", 	""HoursOfOperation"": ""M  08:00 - 17:00  T  08:00 - 17:00  W  08:00 - 17:00  TH  08:00 - 17:00  F  08:00 - 17:00"", 	""MasterAccount"": 1801, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.989, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00095300"", 	""Name"": ""Fastest Labs of North Fort Worth"", 	""City"": ""Saginaw"", 	""State"": ""TX"", 	""Zip"": ""76131-1438"", 	""Address1"": ""1200 S Blue Mound Rd Ste 120"", 	""Address2"": """", 	""Phone"": ""8179687626"", 	""Fax"": """", 	""HoursOfOperation"": ""M  09:00 - 12:00  13:00 - 17:00  T  09:00 - 12:00  13:00 - 17:00  W  09:00 - 12:00  13:00 - 17:00  TH  09:00 - 12:00  13:00 - 17:00  F  09:00 - 12:00  13:00 - 17:00"", 	""MasterAccount"": 1235, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.99, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00010554"", 	""Name"": ""Any Lab Test Now - Plano"", 	""City"": ""Plano"", 	""State"": ""TX"", 	""Zip"": ""75093-2326"", 	""Address1"": ""4701 W Park Blvd Ste 206"", 	""Address2"": """", 	""Phone"": ""9725966181"", 	""Fax"": ""9725966191"", 	""HoursOfOperation"": ""M  08:30 - 17:30  T  08:30 - 17:30  W  08:30 - 17:30  TH  08:30 - 17:30  F  08:30 - 17:30  Su  13:00 - 16:00"", 	""MasterAccount"": 430, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.9895, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00095444"", 	""Name"": ""Premise Medical Center"", 	""City"": ""Lancaster"", 	""State"": ""TX"", 	""Zip"": ""75146-1069"", 	""Address1"": ""3250 W Pleasant Run Rd Ste 130"", 	""Address2"": """", 	""Phone"": ""4692975222"", 	""Fax"": ""8556510605"", 	""HoursOfOperation"": ""M  09:00 - 16:00  T  09:00 - 16:00  W  09:00 - 16:00  TH  09:00 - 16:00  F  09:00 - 16:00"", 	""MasterAccount"": 3912, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.989, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""CHEMCHEK"", 	""Name"": ""Chem Chek, Inc."", 	""City"": ""Richardson"", 	""State"": ""TX"", 	""Zip"": ""75081-1863"", 	""Address1"": ""1750 Alma Rd Ste 108"", 	""Address2"": """", 	""Phone"": ""9729189300"", 	""Fax"": ""9729180020"", 	""HoursOfOperation"": ""M  09:00 - 16:00  T  09:00 - 16:00  W  09:00 - 16:00  TH  09:00 - 16:00  F  09:00 - 14:30"", 	""MasterAccount"": 27, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.989, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00094445"", 	""Name"": ""1st Care Medical Testing"", 	""City"": ""Midlothian"", 	""State"": ""TX"", 	""Zip"": ""76065-4105"", 	""Address1"": ""5790 W Highway 287"", 	""Address2"": """", 	""Phone"": ""9727239411"", 	""Fax"": ""9727239411"", 	""HoursOfOperation"": ""M  09:00 - 17:30  T  09:00 - 17:30  W  09:00 - 12:00  TH  09:00 - 17:30  F  09:00 - 17:30"", 	""MasterAccount"": 3535, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 1.98201, 		""PriceList"": ""Gold"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00088731"", 	""Name"": ""Any Lab Test Now - Frisco, TX"", 	""City"": ""Frisco"", 	""State"": ""TX"", 	""Zip"": ""75034-9488"", 	""Address1"": ""3520 Preston Rd Ste 113A"", 	""Address2"": """", 	""Phone"": ""9725698564"", 	""Fax"": ""9728101522"", 	""HoursOfOperation"": ""M  08:30 - 17:30  T  08:30 - 17:30  W  08:30 - 17:30  TH  08:30 - 17:30  F  08:30 - 17:30  Su  13:00 - 16:00"", 	""MasterAccount"": 430, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.9895, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00095329"", 	""Name"": ""1 Stop Drugscreening"", 	""City"": ""Midlothian"", 	""State"": ""TX"", 	""Zip"": ""76065-3361"", 	""Address1"": ""200 S 14th St Ste 130"", 	""Address2"": """", 	""Phone"": ""4694508895"", 	""Fax"": ""4695195682"", 	""HoursOfOperation"": ""M  10:00 - 16:00  T  10:00 - 16:00  W  10:00 - 16:00  TH  10:00 - 16:00  F  10:00 - 16:00  S  09:00 - 20:00"", 	""MasterAccount"": 3857, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.99, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }, { 	""Code"": ""FF00093783"", 	""Name"": ""Watchdog Solutions - TX078"", 	""City"": ""Fort Worth"", 	""State"": ""TX"", 	""Zip"": ""76132-4236"", 	""Address1"": ""7535 Oakmont Blvd"", 	""Address2"": """", 	""Phone"": ""8009722054"", 	""Fax"": ""2142795032"", 	""HoursOfOperation"": ""M  09:00 - 18:00  T  09:00 - 18:00  W  09:00 - 18:00  TH  09:00 - 18:00  F  09:00 - 18:00  S  09:00 - 18:00"", 	""MasterAccount"": 1776, 	""TimeZone"": ""CST"", 	""TZOffset"": -6, 	""Distance"": null, 	""FormFoxEnabled"": true, 	""MarketplaceEnabled"": true, 	""IsMPOnly"": false, 	""Services"": [{ 		""Code"": ""NDOTU"", 		""Rank"": 4.99, 		""PriceList"": ""Platinum"" 	}], 	""Amenities"": [] }]";

            string testfile = "./jsonFiles/clinicarray.json";
            string filetxt = File.ReadAllText(testfile);

            var obj = JToken.Parse(filetxt);

            if (obj.Type == JTokenType.Array)
            {
                // Do Analysis:
                var y = 2;
            }
            else
            {
                var z = 3;
            }

            var x = 1;

        }


        [TestMethod]
        public void testExtensionValidation()
        {

            var supportedTypes = new[] { "jpg", "jpeg", "png", "gif", "txt", "ppt", "doc", "docx", "xls", "xlsx", "ods", "pdf" };
            var FileName = "IMG_2464.PNG";
            var fileExt = System.IO.Path.GetExtension(FileName).Substring(1);
            Debug.WriteLine(supportedTypes.Contains(fileExt));
            Debug.WriteLine(supportedTypes.Contains(fileExt,StringComparer.InvariantCultureIgnoreCase));
        }

    }
}