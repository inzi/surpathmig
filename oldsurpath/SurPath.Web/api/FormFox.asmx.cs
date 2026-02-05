using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using System.Xml.Serialization;
using Serilog;
using SurPath.Business;
using SurPath.Data;
using SurPath.Data.Backend;
using SurPath.Entity;
using SurPath.Enum;
using SurpathBackend;

namespace SurPathWeb.api.formfox
{
    /// <summary>
    /// Summary description for FormFox
    /// </summary>
    [WebService(Namespace = "http://surpath.com/api/formfox")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class FormFox : System.Web.Services.WebService
    {
        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethod("updateOrderStatus")]
        public string updateOrderStatus(string Username, string Password, string ReferenceTestId, string Status, string Notes, string SFCode, string SampleType, string TestID)
        {

            // From FormFox
            //Collection Status will be one of the following codes:
            //OK – Test has been Complete This is what will be the status of a completed collection event, and will include all collection data and CCF image
            //REFU – Donor Refused Test When a donor refuses to provide a sample
            //UNAB – Donor Unable to void When a donor has been given the allotted three hours to drink water, yet still cannot provide the required sample volume
            //PEND – Test is Pending collectionStatus when an order has been placed
            //SCHD – Test is Scheduled Status when an order has been placed
            //CANC – Test has been CanceledStatus when you remove an order via the API
            //ONSITE – Donor is On SiteStatus if the donor has been checked in at the front desk of the clinic
            //SUSP – Test has been SuspendedUsually happens when a donor cannot provide a sufficient sample and is sent back to the waiting area to drink water
            //INPRC – Test is in Process Status when the collection workflow has been started in Formfox
            //NCOM – Incomplete Status if the collector stops the collection process in Formfox, before completing the service, and without setting it to suspended or some other status.
            if (!Secure(Username, Password))
            {
                _logger.Debug($"updateOrderStatus unauthorized: {Username}, {Password}");
                return "401 Unauthorized access";
            }
            try
            {


                _logger.Debug("FormFox API updateOrderStatus START");
                _logger.Debug("Username");
                _logger.Debug(Username);
                _logger.Debug("Password");
                _logger.Debug(Password);
                _logger.Debug("ReferenceTestId");
                _logger.Debug(ReferenceTestId);
                _logger.Debug("Status");
                _logger.Debug(Status);
                _logger.Debug("Notes");
                _logger.Debug(Notes);
                _logger.Debug("SFCode");
                _logger.Debug(SFCode);
                _logger.Debug("SampleType");
                _logger.Debug(SampleType);
                _logger.Debug("TestID");
                _logger.Debug(TestID);
                _logger.Debug("FormFox API updateOrderStatus END");


                ParamGetformfoxorders paramGetformfoxorders = new ParamGetformfoxorders();
                ParamSetformfoxorders paramSetformfoxorders = new ParamSetformfoxorders();

                paramGetformfoxorders.formfoxorders.ReferenceTestID = ReferenceTestId;
                _logger.Debug($"Getting {ReferenceTestId}");

                paramSetformfoxorders.formfoxorders = data.GetFormFoxOrder(paramGetformfoxorders);
                if (paramSetformfoxorders.formfoxorders == null)
                {
                    _logger.Error($"{ReferenceTestId} NOT FOUND IN DATABASE");
                }
                else
                {
                    if (paramSetformfoxorders.formfoxorders.ReferenceTestID.Equals(ReferenceTestId, StringComparison.InvariantCultureIgnoreCase) == true)
                    {
                        paramSetformfoxorders.formfoxorders.status = Status;
                        paramSetformfoxorders.formfoxorders.sampletype = SampleType;
                        paramSetformfoxorders.formfoxorders.testid = TestID;
                        int _id = (int)data.SetFormFoxOrder(paramSetformfoxorders);
                        if (_id > 0)
                        {
                            // add an activity note on this event
                            var _dtiid = paramSetformfoxorders.formfoxorders.donor_test_info_id;
                            var ActivityNote = $"FormFox Status Update Received. Reference Test ID: {paramSetformfoxorders.formfoxorders.ReferenceTestID} donor_test_info_id: {paramSetformfoxorders.formfoxorders.donor_test_info_id} Status: {Status} SampleType {SampleType}";
                            //  ActivityNote = $"FormFox set test result.  Status: {_formfoxorders.status} SpecimenID: {_specimenID}";
                            _logger.Debug($"Adding Activity Note: donor_test_info_id - {_dtiid}, Note - {ActivityNote.Trim()}");
                            BackendLogic backendLogic = new BackendLogic("formfoxapi", _logger);
                            backendLogic.SetDonorActivity(_dtiid, (int)DonorActivityCategories.Information, ActivityNote.Trim());
                            return "Success";
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MvcApplication.LogError(ex);
            }
            _logger.Debug($"Failed to update {ReferenceTestId}");
            return "Failure";

        }
        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethod("updateOrderResult")]
        public string updateOrderResult(string Username, string Password, string ResultXML)
        {
            if (!Secure(Username, Password))
            {
                _logger.Debug($"updateOrderResult unauthorized: {Username}, {Password}");
                return "401 Unauthorized access";
            }

            try
            {


                _logger.Debug("FormFox API updateOrderResult START");

                _logger.Debug("Username");
                _logger.Debug(Username);
                _logger.Debug("Password");
                _logger.Debug(Password);
                _logger.Debug("ResultXML");
                _logger.Debug(ResultXML);
                _logger.Debug("FormFox API updateOrderResult END");

                XmlSerializer ResultOrderResultsSerializer = new XmlSerializer(typeof(ResultOrderResults));
                ResultOrderResults resultOrderResults = new ResultOrderResults();

                using (TextReader reader = new StringReader(ResultXML.ToString()))
                {
                    _logger.Debug("Attempting to deserialize XML");
                    resultOrderResults = (ResultOrderResults)ResultOrderResultsSerializer.Deserialize(reader);
                }

                if (resultOrderResults.Screenings.Screening.Where(sc => sc.Type == "Drug").Count() > 0)
                {
                    //XDocument ResultXMLDoc = XDocument.Load(new StringReader(ResultXML));
                    _logger.Debug("Found Drug Screening");

                    var _drugTestResult = resultOrderResults.Screenings.Screening.Where(sc => sc.Type == "Drug").ToList().First();
                    string _specimenID = _drugTestResult.SpecimenID;
                    _logger.Debug($"SpecimenID: {_specimenID}");
                    var FormFoxPDFFolder = ConfigurationManager.AppSettings["FormFoxPDFFolder"].ToString().Trim();
                    Directory.CreateDirectory(FormFoxPDFFolder);
                    var FormFoxDumpFolder = ConfigurationManager.AppSettings["FormFoxDumpFolder"].ToString().Trim();
                    Directory.CreateDirectory(FormFoxDumpFolder);

                    //var _ImagesData = ResultXMLDoc.Descendants("ImagesData").First().Descendants("ImageData").Descendants("ImageData").First().Value;
                    //var _ImagesDataFilename = ResultXMLDoc.Descendants("ImagesData").First().Descendants("ImageData").Descendants("ImageName").First().Value;

                    var _ImagesDataFilename = _drugTestResult.ImagesData.ImageData.ImageName;
                    var _ImagesData = _drugTestResult.ImagesData.ImageData.ImageData;

                    //byte[] bytes = Convert.FromBase64String(_ImagesData);
                    FormFoxDumpFolder = (FormFoxDumpFolder + Path.DirectorySeparatorChar);
                    System.Byte[] FileByteArray = System.Convert.FromBase64String(_ImagesData);
                    using (System.IO.FileStream __stream = System.IO.File.Create(FormFoxDumpFolder + _ImagesDataFilename))
                    {

                        __stream.Write(FileByteArray, 0, FileByteArray.Length);
                    }
                    _logger.Debug($"Document for SpecimenID: {_specimenID} saved to {FormFoxDumpFolder + _ImagesDataFilename}");


                    // the document we get from FormFox is a chain of custody document, 
                    // so we'll add it to donor documents as per conversation with David 2/14/21 at 5:56 PM

                    ParamGetformfoxorders paramGetformfoxorders = new ParamGetformfoxorders();

                    paramGetformfoxorders.formfoxorders.ReferenceTestID = resultOrderResults.ReferenceTestID;
                    formfoxorders _formfoxorders = data.GetFormFoxOrder(paramGetformfoxorders);

                    if (_formfoxorders.donor_test_info_id > 0)
                    {
                        // add an activity note on this event
                        //var _dtiid = _formfoxorders.donor_test_info_id;
                        // get the donor test info record
                        DonorBL donorBL = new DonorBL(_logger);

                        //DonorTestInfo _dti = donorBL.GetDonorTestInfo(_dtiid);
                        //Donor donor = donorBL.Get(_dti.DonorId, "Desktop");
                        BackendLogic backendLogic = new BackendLogic("formfoxapi", _logger);

                        _logger.Debug($"formfoxorders ID: {_formfoxorders.backend_formfox_orders_id.ToString()}");

                        _formfoxorders.SpecimenID = _specimenID;
                        _logger.Debug($"Setting SpecimenID {_formfoxorders.SpecimenID} for FormFox Order {_formfoxorders.backend_formfox_orders_id}");

                        data.SetFormFoxOrder(new ParamSetformfoxorders() { formfoxorders = _formfoxorders });
                        var ActivityNote = $"FormFox set test result. Reference Test ID: {_formfoxorders.ReferenceTestID} donor_test_info_id: {_formfoxorders.donor_test_info_id} Status: {_formfoxorders.status} SpecimenID: {_specimenID}";
                        _logger.Debug($"Adding Activity Note: donor_test_info_id - {_formfoxorders.donor_test_info_id}, Note - {ActivityNote.Trim()}");
                        backendLogic.SetDonorActivity(_formfoxorders.donor_test_info_id, (int)DonorActivityCategories.Information, ActivityNote.Trim());
                        //DonorBL donorBL = new DonorBL();
                        DonorTestInfo donorTestInfo = donorBL.GetDonorTestInfo(_formfoxorders.donor_test_info_id);
                        _logger.Debug($"donorTestInfo ID: {_formfoxorders.donor_test_info_id.ToString()}");
                        DonorDocument donorDocument = new DonorDocument();

                        donorDocument.DonorDocumentId = 0;
                        donorDocument.DonorId = donorTestInfo.DonorId;
                        donorDocument.DocumentTitle = _ImagesDataFilename;


                        donorDocument.DocumentContent = FileByteArray;
                        donorDocument.Source = "FormFox";
                        donorDocument.UploadedBy = "API";
                        donorDocument.FileName = _ImagesDataFilename;

                        HL7ParserDao hl7ParserDao = new HL7ParserDao();
                        _logger.Debug($"Uploading document for user {donorTestInfo.DonorId}");

                        hl7ParserDao.UploadPDFChainOfCustodyReport(_formfoxorders.SpecimenID, donorDocument, resultOrderResults, _drugTestResult, donorTestInfo);
                        ActivityNote = $"FormFox chain of custody document uploaded. Reference Test ID: {_formfoxorders.ReferenceTestID} donor_test_info_id: {_formfoxorders.donor_test_info_id} SpecimenID: {_specimenID}";
                        _logger.Debug($"Adding Activity Note: donor_test_info_id - {_formfoxorders.donor_test_info_id}, Note - {ActivityNote.Trim()}");
                        backendLogic.SetDonorActivity(_formfoxorders.donor_test_info_id, (int)DonorActivityCategories.Information, ActivityNote.Trim());
                       
                    }

                }




                return "Success";
            }
            catch (Exception ex)
            {
                MvcApplication.LogError(ex);

                return "Failure";
            }
        }


        //[System.Web.Script.Services.ScriptService]
        // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
        // [System.Web.Script.Services.ScriptService]
        BackendData data = new BackendData();
        ffAuthProvider AuthProvider = new ffAuthProvider();
        ILogger _logger = MvcApplication._logger;

        public FormFox()
        {
            _logger.Debug("FormFox API INVOKED");

            // get auths from FormFoxAuthAccounts
            var _auths = ConfigurationManager.AppSettings["FormFoxAuthAccounts"].ToString().Trim();
            string[] _authPairsArray = _auths.Split(';');
            foreach (string _a in _authPairsArray)
            {
                string[] _authsArry = _a.Split(',');
                var _u = Base64Decode(_authsArry[0]);
                var _p = Base64Decode(_authsArry[1]);
                AuthProvider.Accounts.Add(new ffAuths() { username = _u, password = _p });
            }

        }
        class ffAuths
        {
            public string username { get; set; }
            public string password { get; set; }

        }
        class ffAuthProvider
        {
            public List<ffAuths> Accounts { get; set; } = new List<ffAuths>();
        }


        // Secure
        bool Secure(string u, string p)
        {
            bool retval = false;

            if (AuthProvider.Accounts.Exists(a => a.username == u && a.password == p)) retval = true;


            return retval;
        }

        // Encode
        static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        //Decode
        static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
