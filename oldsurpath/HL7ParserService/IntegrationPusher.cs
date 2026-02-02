using MySql.Data.MySqlClient;
using Serilog;
using Surpath.ClearStar.BL;
using SurPath.Business;
using SurPath.Data;
using SurPath.Entity;
using SurPath.Enum;
// using SurPath.MySQLHelper;
using SurpathBackend;
//using SurPathWeb.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;
using Tamir.SharpSsh.java.io;
using Tamir.SharpSsh.jsch;

namespace HL7ParserService
{

    /// <summary>
    /// backend_integration_partner_release contains a row entry for all 
    /// </summary>
    public class IntegrationPusher
    {

        public bool transmitFile = false;

        public ILogger _logger;
        public string ConnectionString;
        public MySqlConnection conn;

        List<IntegrationPartner> partners = new List<IntegrationPartner>();
        List<IntegrationPartnerClient> allpartnerclients = new List<IntegrationPartnerClient>();
        List<IntegrationPush> pushes = new List<IntegrationPush>();
        //public List<IntegrationPushFile> IntegrationPushFiles = new List<IntegrationPushFile>();
        public List<IntegrationResultFile> IntegrationResultFiles = new List<IntegrationResultFile>();
        
        public List<Client> Clients = new List<Client>();

        

        private BackendLogic backendLogic; // = new BackendLogic(null, MvcApplication._logger);

        //public string StageFilesFolder;
        //public string LabReportFilePath;
        //public string MROReportFileInboundPath;
        //public string ClearStarFilePath;
        public IntegrationPusher(bool TransmitFiles, ILogger __logger = null)
        {
            if (__logger != null)
            {
                _logger = __logger;
            }
            else
            {
                _logger = new LoggerConfiguration().CreateLogger();
            }
            this.transmitFile = TransmitFiles;
            this.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString().Trim();
            this.conn = new MySqlConnection(this.ConnectionString);
            this.backendLogic = new BackendLogic(null, _logger);

            //this.StageFilesFolder = ConfigurationManager.AppSettings["StageFilesFolder"].ToString().Trim();
            //if (!this.StageFilesFolder.EndsWith("/")) this.StageFilesFolder = this.StageFilesFolder + "/";
            //this.MROReportFileInboundPath = ConfigurationManager.AppSettings["MROReportFileInboundPath"].ToString().Trim();
            //if (!this.MROReportFileInboundPath.EndsWith("/")) this.MROReportFileInboundPath = this.MROReportFileInboundPath + "/";
            //this.LabReportFilePath = ConfigurationManager.AppSettings["LabReportFilePath"].ToString().Trim();
            //if (!this.LabReportFilePath.EndsWith("/")) this.LabReportFilePath = this.LabReportFilePath + "/";

        }


        public void Push()
        {
            _logger.Debug("Pushing files");

            IntegrationResultFiles = new List<IntegrationResultFile>();
            pushes = new List<IntegrationPush>();
            partners = backendLogic.GetIntegrationPartners();
            allpartnerclients = new List<IntegrationPartnerClient>();
            // Get all the integration clients we're pushing updates to
            partners = partners.Where(p => p.partner_push == true).ToList();
            _logger.Debug($"{partners.Count} partner(s) loaded with push enabled");
            getUnSentFiles();
            if (pushes.Count>0)
            {
                SendUnSentFiles();
            }           
            _logger.Debug("Push finished");
        }


        public bool getUnSentFiles()
        {

            DonorBL donorBL = new DonorBL(_logger);

            foreach (IntegrationPartner ip in partners)
            {
                _logger.Debug($"Getting files for {ip.partner_name}");
                // get all their clients
                List<IntegrationPartnerClient> partnerclients = backendLogic.GetIntegrationPartnerClients(ip.partner_key);
                allpartnerclients = allpartnerclients.Concat(partnerclients).ToList();

                foreach (IntegrationPartnerClient ipc in partnerclients)
                {
                    _logger.Debug($"Getting files for {ipc.partner_client_code}");
                    // IntegrationDonors donorsanddocs = backendLogic.GetIntegrationPartnerDonorsAndDocuments(ip, new ApiIntegrationFilter() { partner_client_code = ipc.partner_client_code });

                    List<IntegrationPartnerRelease> topush = backendLogic.GetIntegrationPartnerRelease(ip.partner_key, ipc.partner_client_code, false);

                    _logger.Debug($"Files to push: {topush.Count}");

                    foreach (IntegrationPartnerRelease ipr in topush)
                    {
                        int donor_test_info_id = ipr.donor_test_info_id;
                        int donor_id = ipr.donor_id;
                        int client_id = ipr.client_id;
                        int client_department_id = ipr.client_department_id;

                        _logger.Debug($"Checking donor_test_info_id: {donor_test_info_id} donor_id: {donor_id} client_id: {client_id} client_department_id: {client_department_id} for push");

                        // DonorTestInfo donorTestInfo = donorBL.GetDonorTestInfoByDonorIdDonorTestInfoId(donor_id, donor_test_info_id);

                        DonorTestInfo donorTestInfo = donorBL.GetDonorTestInfo(donor_test_info_id);
                        //donorTestInfo.TestInfoTestCategories.ForEach(tc => _logger.Debug($"Test category: {tc.TestCategoryId.ToDescriptionString()}"));
                        //ReportInfo reportInfo = donorBL.GetLabReport(donor_test_info_id, ReportType.);
                        _logger.Debug($"donorTestInfo.TestStatus: {donorTestInfo.TestStatus.ToDescriptionString()}");
                        _logger.Debug($"ipr.released: {ipr.released}");

                        // if a test is not complete, we don't send results
                        if (donorTestInfo.TestStatus == DonorRegistrationStatus.Completed && ipr.released == true)
                        {
                            _logger.Debug($"Test is completed");

                            IntegrationResultFile integrationResultFile = new IntegrationResultFile();
                            // get the donor
                            Donor donor = donorBL.Get(donor_id, "SYSTEM");

                            integrationResultFile = PopulateIntegrationResultFile(donor, ip.backend_integration_partners_pidtype);
                            integrationResultFile.partner_client_code = ipc.partner_client_code;

                            integrationResultFile.attention_flag = ((int)donorTestInfo.TestOverallResult != (int)OverAllTestResult.Negative || (int)donorTestInfo.TestOverallResult != (int)OverAllTestResult.Discard);

                            // Add drug test results
                            if (donorTestInfo.TestInfoTestCategories.Exists(t => t.TestCategoryId == TestCategories.UA))
                            {
                                var testOverallResultValue = (OverAllTestResult)donorTestInfo.TestOverallResult;
                                var testCat = donorTestInfo.TestInfoTestCategories.Where(t => t.TestCategoryId == TestCategories.UA).ToList().OrderByDescending(t=>t.LastModifiedOn).First();
                                if (testCat!=null)
                                {
                                    IntegrationResultData dtresultData = new IntegrationResultData();
                                    dtresultData.Category = TestCategories.UA.ToDescriptionString();
                                    dtresultData.CategoryID = (int)TestCategories.UA;
                                    dtresultData.Status = testOverallResultValue.ToDescriptionString();
                                    dtresultData.StatusID = (int)testOverallResultValue;
                                    dtresultData.DTInitiated = donorTestInfo.TestRequestedDate;
                                    dtresultData.DTTested = donorTestInfo.ScreeningTime;
                                    dtresultData.DTReceived = testCat.CreatedOn;
                                    dtresultData.id = testCat.SpecimenId;

                                    integrationResultFile.Results.Add(dtresultData);

                                }

                            }



                            // does this dept have background checks? if so, it'll have ClearStarCode property

                            if (donor.DonorClearStarProfId != null)// This checkes to see if there is a ClearStarID if so it sets it to inprogress
                            {
                                _logger.Debug($"Background check id exists: {donor.DonorClearStarProfId.ToString()}");
                                string sCustID = string.Empty;
                                var csreports = ConfigurationManager.AppSettings["ClearStarReports"].ToString().Trim();
                                _logger.Debug($"csreports: {csreports}");

                                try
                                {
                                    var creds = DefaultCredentialsBL.GetCredentials();
                                    sCustID = donor.ClientDepartment.ClearStarCode;
                                    _logger.Debug($"sCustID: {sCustID}");
                                    string ProfileNo = donor.DonorClearStarProfId;
                                    _logger.Debug($"ProfileNo: {ProfileNo}");

                                    Surpath.ClearStar.BL.ProfileBL profile = new ProfileBL();

                                    var exists = System.IO.File.Exists(csreports + ProfileNo + ".pdf");

                                    if (!exists)
                                    {
                                        _logger.Debug(csreports + ProfileNo + ".pdf doesn't exist, retreiving, calling GetProfileReport");
                                        var result = profile.GetProfileReport(sCustID, ProfileNo);
                                        _logger.Debug($"Saving result");
                                        var file = profile.SaveProfileReport(sCustID, ProfileNo, result);
                                    }
                                    else
                                    {
                                        _logger.Debug(csreports + ProfileNo + ".pdf exists");

                                    }
                                }
                                catch (System.Exception ex)
                                {
                                    _logger.Error(ex.Message);
                                    if (ex.InnerException != null)
                                    {
                                        _logger.Error(ex.InnerException.ToString());
                                    }

                                }
                                if (System.IO.File.Exists(csreports + donor.DonorClearStarProfId + ".pdf"))
                                {
                                    _logger.Debug($"Background check file exists");
                                    IntegrationResultData resultData = new IntegrationResultData();
                                    resultData.Category = TestCategories.BC.ToDescriptionString();
                                    resultData.CategoryID = (int)TestCategories.BC;
                                    resultData.Status = DonorRegistrationStatus.Completed.ToDescriptionString();
                                    resultData.StatusID = (int)DonorRegistrationStatus.Completed;


                                    // we've not sent BC
                                    //_i.category = TestCategories.BC.ToDescriptionString();
                                    //_i.status = DonorRegistrationStatus.Completed.ToDescriptionString();
                                    //TODO if include file - attach it 
                                    // ResultReportBase64

                                    integrationResultFile.Results.Add(resultData);
                                    _logger.Debug($"Added to integration result file");


                                }
                                else
                                {
                                    _logger.Debug($"Background check doesn't exist exists");

                                }
                            }

                            foreach (DonorTestInfoTestCategories _tc in donorTestInfo.TestInfoTestCategories)
                            {
                                _logger.Debug($"Checking test category {_tc.TestCategoryId.ToDescriptionString()}");

                                // BC
                                //if (ipr.background_check == false && _tc.TestCategoryId == TestCategories.BC)
                                //{

                                //    IntegrationPushFile _i = PopulateIntegrationPushFile(donor);
                                //    // we've not sent BC
                                //    _i.category = TestCategories.BC.ToDescriptionString();
                                //    _i.status = DonorRegistrationStatus.Completed.ToDescriptionString();
                                //    //TODO if include file - attach it 
                                //    // ResultReportBase64

                                //    IntegrationPushFiles.Add(_i);

                                //}
                                if (ipr.released == true && _tc.TestCategoryId != TestCategories.BC)
                                {
                                    _logger.Debug($"{_tc.TestCategoryId.ToDescriptionString()} released");
                                    IntegrationResultData resultData = new IntegrationResultData();
                                    resultData.Category = _tc.TestCategoryId.ToDescriptionString();
                                    resultData.CategoryID = (int)_tc.TestCategoryId;
                                    resultData.Status = DonorRegistrationStatus.Completed.ToDescriptionString();
                                    resultData.StatusID = (int)DonorRegistrationStatus.Completed;
                                    //TODO if include file - attach it 
                                    // ResultReportBase64
                                    integrationResultFile.Results.Add(resultData);
                                }

                            }


                            if (integrationResultFile.Results.Count > 0)
                            {
                                _logger.Debug($"Results exist, adding to push list");

                                IntegrationResultFiles.Add(integrationResultFile);

                                pushes.Add(new IntegrationPush()
                                {
                                    integrationResultFile = integrationResultFile,
                                    integrationPartnerRelease = ipr
                                });
                            }

                            //IntegrationPushFile integrationPushFile = new IntegrationPushFile();

                            //// status of dti?
                            //if (ipr.released == true)
                            //{
                            //    IntegrationPushFile _i = PopulateIntegrationPushFile(donor);
                            //    // we've not sent UA
                            //    _i.category = TestCategories.UA.ToDescriptionString();
                            //    _i.status = DonorRegistrationStatus.Completed.ToDescriptionString();
                            //    //TODO if include file - attach it 
                            //    // ResultReportBase64

                            //    IntegrationPushFiles.Add(_i);
                            //}

                            //Dictionary<string, string> searchParam = new Dictionary<string, string>();
                            //searchParam.Add("DonorId", donor_id.ToString());
                            //DataTable dtDonors = donorBL.SearchDonorByClient(searchParam, UserType.TPA, 0);

                        }


                    }
                }
            }

            return true;
        }

        public bool SendUnSentFiles()
        {
            _logger.Debug($"Sending unsent files");
            
            foreach (IntegrationResultFile integrationResultFile in this.IntegrationResultFiles)
            {
                // get this client
                IntegrationPartner ip = backendLogic.GetIntegrationPartnerByPartnerClientCode(integrationResultFile.partner_client_code);
                IntegrationPartnerClient ipc = allpartnerclients.Where(cp => cp.partner_client_code == integrationResultFile.partner_client_code).ToList().First();


                string integrationResultFileJSON = new JavaScriptSerializer().Serialize(integrationResultFile);

                string integrationResultFileJSONEncrypted = EncryptWithKey(integrationResultFileJSON, ip.partner_crypto);
                string _dt = DateTime.Now.ToUniversalTime().ToString("yyyyMMddHHmmssfff");
                string dst = $"{_dt}.{integrationResultFile.partner_client_code}.{integrationResultFile.integration_id}.enc";
                _logger.Debug($"partner_push_path = {ip.partner_push_path}");
                if (!(string.IsNullOrEmpty(ip.partner_push_path)))
                {
                    _logger.Debug($"partner_push_path = {ip.partner_push_path}");
                    var partner_push_path = ip.partner_push_path;
                    partner_push_path = partner_push_path.TrimEnd('/') + "/";
                    // add the subfolder if value
                    if (!(string.IsNullOrEmpty(ipc.partner_push_folder)))
                    {
                        _logger.Debug($"partner_push_folder = {ipc.partner_push_folder}");

                        partner_push_path = partner_push_path + ipc.partner_push_folder;
                        partner_push_path = partner_push_path.TrimEnd('/') + "/";
                    }

                    dst = partner_push_path + dst;
                    _logger.Debug($"dst now {dst}");

                }

                
                Int32.TryParse(ip.partner_push_port, out int port);

                byte[] byteArray = Encoding.ASCII.GetBytes(integrationResultFileJSONEncrypted);
                MemoryStream stream = new MemoryStream(byteArray);
                _logger.Debug($"Sending file to {ip.partner_name} -> {dst}; {ip.partner_push_host},{port}, {ip.partner_push_username}, {ip.partner_push_password}");
                SendFile(stream, dst, ip.partner_push_host, port, ip.partner_push_username, ip.partner_push_password);
                _logger.Debug($"Sent file to {ip.partner_name} -> {dst}");

                MarkFileAsSent(integrationResultFile);

                //JSch jsch = new JSch();

                //String host = ip.partner_push_host;
                //String user = ip.partner_push_username;
                //Int32.TryParse(ip.partner_push_port, out int port);
                //Session session = jsch.getSession(user, host, port);

                //UserInfo ui = new MyUserInfo(ip.partner_push_password);

                //session.setUserInfo(ui);
                //session.connect();

                //Channel channel = session.openChannel("sftp");
                //channel.connect();
                //ChannelSftp chan = (ChannelSftp)channel;

                //String pwd = chan.pwd();


                //// var ba = new ByteArrayInputStream(integrationResultFileJSONEncrypted.getBytes();
                //chan.setInputStream(stream);

                //chan.put(dst);
                ////String lpwd = c.lpwd();

                ////foreach (string reportFile in Directory.EnumerateFiles(sourcePath))
                ////{
                ////    c.put(reportFile, pwd);
                ////    _logger.Information(reportFile.Substring(reportFile.LastIndexOf('\\') + 1));
                ////}

                //chan.quit();
                //session.disconnect();

                // var ip = partners.Where(p=>p == integrationResultFile.partner_client_code)



            }


            return true;
        }

        public bool MarkFileAsSent(IntegrationResultFile integrationResultFile)
        {

            if (pushes.Exists(p=>p.integrationResultFile == integrationResultFile))
            {
                IntegrationPartnerRelease i = pushes.Where(p => p.integrationResultFile == integrationResultFile).FirstOrDefault().integrationPartnerRelease;
                i.sent_on = DateTime.Now;
                i.sent = true;


                backendLogic.SetIntegrationPartnerRelease(i, "SYSTEM", i.released_by);
            }
            else
            {

            }

            return true;
        }


        public bool SendFile(MemoryStream stream, string dst, string host, int port, string user, string pw)
        {
            JSch jsch = new JSch();


            //string tempFolderPath = Path.GetTempPath();
            //tempFolderPath = tempFolderPath.TrimEnd('/') + '/';
            string randomTempFileName = Path.GetTempFileName();
            //string _tempFile = tempFolderPath + randomTempFileName;
            // System.IO.File.WriteAllText(randomTempFileName, stream.ToString());
            // System.IO.FileStream fileStream = new FileStream()
            _logger.Debug($"Writing to temp file: {randomTempFileName}");
            using (var fs = new FileStream(randomTempFileName, FileMode.Create, FileAccess.Write))
            {
                stream.WriteTo(fs);
            }
            //String host = ip.partner_push_host;
            //String user = ip.partner_push_username;
            //Int32.TryParse(port, out int _port);
            Session session = jsch.getSession(user, host, port);
            _logger.Debug($"Session retrieved");
            UserInfo ui = new MyUserInfo(pw);

            session.setUserInfo(ui);
            _logger.Debug($"connecting");
            session.connect();
            _logger.Debug($"connected");
            Channel channel = session.openChannel("sftp");
            channel.connect();
            _logger.Debug($"connected sftp channel");
            ChannelSftp chan = (ChannelSftp)channel;

            String pwd = chan.pwd();
            _logger.Debug($"sending {randomTempFileName} -> {dst}");
            chan.put(randomTempFileName, dst);
            _logger.Debug($"Sent {randomTempFileName} -> {dst}");
            chan.quit();
            session.disconnect();
            _logger.Debug($"disconnected, deleting {randomTempFileName}");

            System.IO.File.Delete(randomTempFileName);

            return true;

        }

        public bool SendFile(string filename, string dst, string host, int port, string user, string pw)
        {
            JSch jsch = new JSch();

            //String host = ip.partner_push_host;
            //String user = ip.partner_push_username;
            //Int32.TryParse(port, out int _port);
            Session session = jsch.getSession(user, host, port);

            UserInfo ui = new MyUserInfo(pw);

            session.setUserInfo(ui);
            session.connect();

            Channel channel = session.openChannel("sftp");
            channel.connect();
            ChannelSftp chan = (ChannelSftp)channel;

            String pwd = chan.pwd();

            chan.put(filename, dst);

            chan.quit();
            session.disconnect();

            return true;

        }






        //private IntegrationPushFile PopulateIntegrationPushFile(Donor donor)
        //{
        //    IntegrationPushFile _i = new IntegrationPushFile();
        //    _i.donor_email = donor.DonorEmail;
        //    _i.donor_first_name = donor.DonorFirstName;
        //    _i.donor_last_name = donor.DonorLastName;
        //    _i.donor_mi = donor.DonorMI;
        //    // _i.integration_id = donor.PidTypeValues.Where()

        //    return _i;
        //}

        private IntegrationResultFile PopulateIntegrationResultFile(Donor donor, int PIDType)
        {
            IntegrationResultFile _i = new IntegrationResultFile();
            _i.donor_email = donor.DonorEmail;
            _i.donor_first_name = donor.DonorFirstName;
            _i.donor_last_name = donor.DonorLastName;
            _i.donor_mi = donor.DonorMI;
            // _i.integration_id = donor.PidTypeValues.Where()
            if (donor.PidTypeValues.Exists(pt=>pt.PIDType==PIDType))
            {
                _i.integration_id = donor.PidTypeValues.Where(pt => pt.PIDType == PIDType).ToList().First().PIDValue;
            }
            return _i;
        }

        public static string EncryptWithKey(string toEncrypt, string key)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public static string DecryptWithKey(string cipherString, string key)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

    }

    //public class IntegrationPushFile
    //{
    //    public string donor_email { get; set; } = string.Empty; // Donor Email
    //    public string donor_password { get; set; } = string.Empty; // Surpath use only, interal donor Password
    //    public string partner_client_code { get; set; } = string.Empty; // YOUR code for the client. You can set this to whatever you want.
    //    public string integration_id { get; set; } = string.Empty; // YOUR internal ID for the donor. Always treated as a string, convert / cast as needed.
    //    public string donor_first_name { get; set; } = string.Empty; // When donor registers, they must fill out contact info, this value will be pre-populated for the donor
    //    public string donor_mi { get; set; } = string.Empty; // Prepopulate donor's middle initial during registration
    //    public string donor_last_name { get; set; } = string.Empty; // prepopulate donor's last name


    //    public string category { get; set; }
    //    public string status { get; set; }
    //}
    public class MyUserInfo : UserInfo
    {
        public MyUserInfo(string password)
        {
            passwd = password;
        }

        public String getPassword()
        {
            return passwd;
        }

        public bool promptYesNo(String str)
        {
            return true;
        }

        private String passwd;

        public String getPassphrase()
        {
            return null;
        }

        public bool promptPassphrase(String message)
        {
            return true;
        }

        public bool promptPassword(String message)
        {
            if (passwd != string.Empty)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void showMessage(string message)
        {
            throw new System.NotImplementedException();
        }
    }

    public class IntegrationPush
    {
        public IntegrationResultFile integrationResultFile { get; set; }
        public IntegrationPartnerRelease integrationPartnerRelease { get; set; }
    }

    public class IntegrationResultFile
    {
        public string donor_email { get; set; } = string.Empty; // Donor Email
        public string donor_password { get; set; } = string.Empty; // Surpath use only, interal donor Password
        public string partner_client_code { get; set; } = string.Empty; // YOUR code for the client. You can set this to whatever you want.
        public string integration_id { get; set; } = string.Empty; // YOUR internal ID for the donor. Always treated as a string, convert / cast as needed.
        public string donor_first_name { get; set; } = string.Empty; // When donor registers, they must fill out contact info, this value will be pre-populated for the donor
        public string donor_mi { get; set; } = string.Empty; // Prepopulate donor's middle initial during registration
        public string donor_last_name { get; set; } = string.Empty; // prepopulate donor's last name
        public bool attention_flag { get; set; } = false; // Will be true if our overall test result is not negative.
        public List<IntegrationResultData> Results { get; set; } = new List<IntegrationResultData>();
    }
    public class IntegrationResultData
    {
        public string id { get; set; } = "";  // this is the result id
        public string Category { get; set; } // this is the result category
        public int CategoryID { get; set; } // this is the result category (int)
        public string Status { get; set; } // this is the status of this category's result
        public int StatusID { get; set; } // this is the status of this category's result (int)
        public string ResultReportBase64 { get; set; } // this is for the report file. it is not used currently
        public DateTime? DTInitiated { get; set; }
        public DateTime? DTProvided { get; set; }
        public DateTime? DTTested { get; set; }
        public DateTime? DTReceived { get; set; }
    }
}
