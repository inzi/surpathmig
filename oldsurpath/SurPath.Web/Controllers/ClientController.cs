using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using SurPath.Business;
using SurPath.Entity;
using System.Diagnostics;
using SurPath.Enum;
using SurPathWeb.Filters;
using SurPathWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.Mvc.Html;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Drawing;
using SurPath.Data;
using Surpath.ClearStar.BL;
using SurpathBackend;
using Serilog;

namespace SurPathWeb.Controllers
{
    public class ClientController : Controller
    {
        ClientBL clientBL = new ClientBL();
        BackendLogic backendLogic = new BackendLogic();
        DonorBL donorBL = new DonorBL();
        HL7ParserBL hl7Parser = new HL7ParserBL();
        ReportType reportTypeMRO = ReportType.MROReport;
        ReportInfo reports = null;
        ReportType reportTypeLab = ReportType.LabReport;
        ReportType reportTypeQuest = ReportType.QuestLabReport;
        ReportInfo reportsLab = null;
        ReportInfo reportsQuest = null;
        ReportInfo reporLabType = null;
        ILogger _logger = MvcApplication._logger;
        int FilterDayInterval = -30;

        [HttpGet]
        [SessionValidateAttribute]
        public ActionResult UserInfo()
        {
            try
            {
                Session["SortingValidation"] = null;
                Session["Download"] = null;
                Session["SortingValidation1"] = null;

                UserBL userBL = new UserBL();
                int UserId = Convert.ToInt32(Session["UserId"].ToString());

                var UserData = userBL.Get(UserId);

                if (UserData != null)
                {
                    var model = new DonorProfileDataModel
                    {
                        Username = UserData.Username,
                        EmailID = UserData.UserEmail,
                        FirstName = UserData.UserFirstName,
                        LastName = UserData.UserLastName,
                        Phone1 = UserData.UserPhoneNumber,
                        Fax = UserData.UserFax
                    };

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                //redirect to 404
            }
            return View();
        }

        [HttpPost]
        [SessionValidateAttribute]
        public ActionResult UserInfo(DonorProfileDataModel model)
        {
            Session["SortingValidation"] = null;
            Session["Download"] = null;
            Session["SortingValidation1"] = null;

            UserBL userBL = new UserBL();
            int UserId = Convert.ToInt32(Session["UserId"].ToString());

            User user = userBL.GetByUsernameOrEmail(model.EmailID.Trim());

            if (user == null || user.UserId == UserId)
            {
                var UserData = userBL.Get(UserId);

                if (UserData != null)
                {
                    UserData.UserFirstName = model.FirstName;
                    UserData.UserLastName = model.LastName;
                    //UserData.Username = model.Username;
                    UserData.UserPhoneNumber = model.Phone1;
                    UserData.UserFax = model.Fax;
                    UserData.UserEmail = model.EmailID;

                    int status = userBL.Save(UserData);

                    if (status == 1)
                    {
                        Session["UserName"] = UserData.UserFirstName + " " + UserData.UserLastName;
                        Session["SuccessMessage"] = "Your profile updated successfully";
                        return Redirect("Dashboard");
                    }
                    else
                    {
                        ViewBag.ServerErr = "Unable to process your request";
                    }
                }

                else
                {
                    ViewBag.emailerr = "Email address already exists.";
                }
            }
            else
            {
                ViewBag.emailerr = "Email address already exists.";
            }
            return View(model);

        }

        [HttpGet]
        [SessionValidateAttribute]
        public ActionResult Dashboard()
        {
            UserBL userBL = new UserBL();
            Session["SortingValidation"] = null;
            Session["Download"] = null;
            Session["SortingValidation1"] = null;

            int userid = Convert.ToInt32(Session["UserId"].ToString());
            DataTable departmentIDList = userBL.GetUserDepartment(userid);

            List<ClientDataModel> model = new List<ClientDataModel>();

            foreach (DataRow dr in departmentIDList.Rows)
            {
                string departmentID = dr["ClientDepartmentId"].ToString();
                DataTable data = clientBL.GetClientDashboard(departmentID);

                foreach (DataRow dr1 in data.Rows)
                {
                    ClientDataModel clientdata = new ClientDataModel();

                    clientdata.ClientDeparmentID = dr1["ClientDepartmentId"].ToString();
                    clientdata.ClientID = dr1["ClientId"].ToString();
                    clientdata.PreRegistration = dr1["PreregistrationCount"].ToString();
                    clientdata.Activated = dr1["ActivatedCount"].ToString();
                    clientdata.Registered = dr1["RegisteredCount"].ToString();
                    clientdata.InQueue = dr1["InQueueCount"].ToString();
                    clientdata.SuspensionQueue = dr1["SuspensionQueueCount"].ToString();
                    clientdata.Processing = dr1["ProcessingCount"].ToString();
                    clientdata.Complete = dr1["CompletedCount"].ToString();

                    Client client = clientBL.Get(Convert.ToInt32(clientdata.ClientID));

                    clientdata.ClientName = client.ClientName.ToString();

                    ClientDepartment clientDepartment = clientBL.GetClientDepartment(Convert.ToInt32(clientdata.ClientDeparmentID));

                    clientdata.ClientDeparmentName = clientDepartment.DepartmentName.ToString();

                    model.Add(clientdata);
                }

            }
            var items = model;

            ViewBag.SuccessMsg = "";

            if (Session["SuccessMessage"] != null)
            {
                ViewBag.SuccessMsg = Session["SuccessMessage"].ToString();
                Session["SuccessMessage"] = "";
            }

            return View(items);
        }

        [HttpGet]
        [SessionValidateAttribute]
        public ActionResult DonorInfo(string ClientID, string departmentID, string teststatus)
        {
            try
            {
                Session["Download"] = null;
                Session["SortingValidation"] = null;
                Session["SortingValidation1"] = null;
                ViewBag.teststatus = teststatus;
                DonorBL donorBL = new DonorBL();
                DataTable donorID = clientBL.GetDonorId(departmentID, teststatus);

                List<TestResultDataModel> model = new List<TestResultDataModel>();

                foreach (DataRow dr in donorID.Rows)
                {
                    TestResultDataModel testdata = new TestResultDataModel();

                    string DonorID = dr["DonorId"].ToString();
                    var donordata = donorBL.Get(Convert.ToInt32(DonorID), "Web");

                    if (donordata != null)
                    {
                        testdata.DonorTestInfoId = UserAuthentication.Encrypt(dr["DonorTestInfoId"].ToString(), true);
                        testdata.DonorId = donordata.DonorId.ToString();
                        testdata.ClientDepartmentId = dr["TestInfoDepartmentId"].ToString();
                        testdata.ClearStarCode = dr["ClearStarCode"].ToString();
                        testdata.TestStatus = dr["TestStatus"].ToString();
                        _logger.Debug($"TABLE: DonorTestInfoId {testdata.DonorTestInfoId} DonorId {testdata.DonorId}  ClientDepartmentId {testdata.ClientDepartmentId} ClearStarCode {testdata.ClearStarCode} TestStatus {testdata.TestStatus}");
                        if (dr["TestOverallResult"].ToString() != string.Empty)
                        {
                            OverAllTestResult result = (OverAllTestResult)(int)(dr["TestOverallResult"]);
                            if (result.ToString() != "None")
                            {
                                testdata.TestOverallResult = result.ToString();
                            }
                            else
                            {
                                testdata.TestOverallResult = "";
                            }
                        }
                        else
                        {
                            testdata.TestOverallResult = string.Empty;
                        }

                        DonorRegistrationStatus Status = (DonorRegistrationStatus)(int)(dr["TestStatus"]);
                        testdata.TestStatus = Status.ToDescriptionString();
                        _logger.Debug($"testdata.TestStatus {testdata.TestStatus }");
                        //try
                        //{
                        //    testdata.DonorClearStarProfId = (string)dr["donorClearStarProfId"];
                        //}
                        //catch(Exception ex)
                        //{

                        //}

                        //try
                        //{
                        //    object value2 = dr["donorClearStarProfId"];
                        //    if (value2 != DBNull.Value)
                        //    {
                        //        testdata.DonorClearStarProfId = (string)dr["donorClearStarProfId"];
                        //    }
                        //}
                        //catch (Exception ex)
                        //{
                        //    testdata.DonorClearStarProfId = string.Empty;
                        //}

                        if (!string.IsNullOrEmpty(donordata.DonorFirstName.ToString()))
                        {
                            testdata.FirstName = donordata.DonorFirstName;
                        }
                        else
                        {
                            testdata.FirstName = " ";
                        }

                        if (!string.IsNullOrEmpty(donordata.DonorLastName.ToString()))
                        {
                            testdata.LastName = donordata.DonorLastName;
                        }
                        else
                        {
                            testdata.LastName = " ";
                        }

                        if (!string.IsNullOrEmpty(donordata.DonorSSN))
                        {
                            testdata.SSN = donordata.DonorSSN;
                            if (testdata.SSN.ToString().Length == 11)
                            {
                                testdata.SSN = "XXX-XX-" + testdata.SSN.ToString().Substring(7);
                            }
                            else
                            {
                                testdata.SSN = "";
                            }
                        }
                        else
                        {
                            testdata.SSN = "";
                        }

                        if (dr["SpecimenDate"] != DBNull.Value)
                        {
                            var specimendate = dr["SpecimenDate"].ToString();

                            if (string.IsNullOrEmpty(specimendate.ToString()))
                            {
                                testdata.TestDate = specimendate.ToString();
                            }
                            else
                            {
                                if (Convert.ToDateTime(specimendate.ToString()) != DateTime.MinValue)
                                {
                                    testdata.TestDate = Convert.ToDateTime(specimendate.ToString()).ToString("MM/dd/yyyy");
                                }
                            }
                        }

                        ClientDepartment clientDepartment = clientBL.GetClientDepartment(Convert.ToInt32(testdata.ClientDepartmentId));
                        testdata.DepartmentName = clientDepartment.DepartmentName.ToString();

                        if (testdata.TestStatus.ToUpper() == "COMPLETED" && dr["IsDonorRefused"].ToString() != "1" || (testdata.TestStatus.ToUpper() == "PROCESSING"))
                        {
                            int MROValue = 0;
                            string MROType = dr["MROTypeId"].ToString();

                            if ((MROType.ToString() == "2") || (MROType.ToString() == "1"))// && testdata.TestOverallResult.ToString() == "Positive"))
                            {
                                int testInfoId = Convert.ToInt32(dr["DonorTestInfoId"].ToString());
                                reportsLab = donorBL.GetLabReport(testInfoId, reportTypeLab);
                                _logger.Debug($"reportsLab ID: {reportsLab.ReportId}");
                                reportsQuest = donorBL.GetLabReport(testInfoId, reportTypeQuest);
                                _logger.Debug($"reportsQuest ID: {reportsQuest.ReportId}");
                                reports = donorBL.GetMROReport(testInfoId, reportTypeMRO);
                                _logger.Debug($"reports ID: {reports.ReportId}");

                                if (reports != null)
                                {
                                    _logger.Debug($"reports is not null");

                                    int documentReportId = reports.FinalReportId;

                                    _logger.Debug($"Final Report ID: {documentReportId}");

                                    if (documentReportId != 0)
                                    {
                                        DonorDocument donorDocument = donorBL.GetDonorDocument(documentReportId);
                                        if (donorDocument != null)
                                        {
                                            int donorDocumentId = donorDocument.DonorDocumentId;
                                            string fileName = donorDocument.FileName;
                                            string documentTitle = donorDocument.DocumentTitle;
                                            byte[] content = donorDocument.DocumentContent;
                                            //testdata.MRODownloadLink = Helper.CreateActionLink(this).ActionLink("MRO Report", "ViewDocument", "Donor",
                                            //     new
                                            //     {
                                            //         documentID = donorDocumentId

                                            //     }, new { @target = "_blank", @class = "btn btn-primary" }).ToString();

                                            testdata.MRODownloadLink = Helper.CreateActionLink(this).ActionLink("MRO Report", "ViewDocument", "Donor",
                                            new
                                            {
                                                documentID = donorDocumentId

                                            }, new { @target = "_blank", @class = "btn btn-primary" }).ToString();
                                            _logger.Debug($"DownloadLink set 6");
                                            testdata.DownloadLink = Helper.CreateActionLink(this).Label("").ToString();

                                            testdata.MRODownloadLink = "";
                                            if (MROType.ToString() == "2")
                                            {
                                                MROValue = 0;
                                            }
                                            else
                                            {
                                                MROValue = 1;
                                            }
                                        }

                                    }
                                    else
                                    {
                                        testdata.MRODownloadLink = Helper.CreateActionLink(this).Label("").ToString();
                                    }

                                }
                                else
                                {
                                    _logger.Debug($"reports is null, creating MRO download link");
                                    testdata.MRODownloadLink = Helper.CreateActionLink(this).Label("").ToString();
                                }
                            }
                            else
                            {
                                testdata.MRODownloadLink = Helper.CreateActionLink(this).Label("").ToString();
                            }


                            if (MROValue == 0)
                            {
                                _logger.Debug($"MRO Value is null");
                                if ((MROType.ToString() == "2") || (MROType.ToString() == "1" && testdata.TestStatus.ToUpper() == "COMPLETED"))
                                {
                                    if (dr["IsInstantTest"].ToString() == "1" && dr["InstantTestResult"].ToString() == "2")
                                    {
                                        testdata.DownloadLink = Helper.CreateActionLink(this).Label("").ToString();
                                    }
                                    else
                                    {
                                        int testInfo = Convert.ToInt32(dr["DonorTestInfoId"].ToString());
                                        if (dr["SpecimenId"].ToString() != string.Empty)
                                        {
                                            testdata.SpecimenId = dr["SpecimenId"].ToString();
                                            if (testdata.SpecimenId.Contains(','))
                                            {
                                                string[] specimen = testdata.SpecimenId.Split(',');
                                                testdata.SpecimenId = specimen[0].Trim().ToString();
                                            }
                                        }
                                        //Query and assigs the reportLab Type 
                                        reporLabType = donorBL.GetLabReportType(testInfo, testdata.SpecimenId, reportTypeMRO);
                                        if (reporLabType == null)
                                        {
                                            if (reportsLab != null || reportsQuest != null)
                                            {
                                                if (MROType.ToString() == "1" && testdata.TestStatus.ToUpper() == "COMPLETED" && testdata.TestOverallResult.ToUpper().ToString() == "NEGATIVE" && (reportsLab != null && reportsLab.ReportType.ToString().ToUpper() == "LABREPORT"))
                                                {
                                                    testdata.DownloadLink = Helper.CreateActionLink(this).ActionLink("Lab Report", "Result", "Client",
                                                       new
                                                       {
                                                           specimenId = testdata.SpecimenId,
                                                           name = testdata.FirstName + "  " + testdata.LastName
                                                       }, new { @target = "_blank", @class = "btn btn-primary" }).ToString();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            testdata.DownloadLink = Helper.CreateActionLink(this).Label("").ToString();
                                        }

                                        if (MROType.ToString() == "2")
                                        {
                                            testdata.DownloadLink = Helper.CreateActionLink(this).ActionLink("Lab Report", "Result", "Client",
                                                    new
                                                    {
                                                        specimenId = testdata.SpecimenId,
                                                        name = testdata.FirstName + "  " + testdata.LastName
                                                    }, new { @target = "_blank", @class = "btn btn-primary" }).ToString();
                                        }

                                    }
                                }
                                else
                                {
                                    _logger.Debug($"MRO Value is not null");
                                    testdata.DownloadLink = Helper.CreateActionLink(this).Label("").ToString();
                                }
                            }

                        }
                        else
                        {
                            testdata.DownloadLink = Helper.CreateActionLink(this).Label("").ToString();
                            testdata.MRODownloadLink = Helper.CreateActionLink(this).Label("").ToString();
                        }

                        model.Add(testdata);
                    }
                }
                var items = model;
                return View(items);
            }

            catch (Exception ex)
            {
                //_logger.Error(ex.ToString());
                //if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                //if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                throw;
            }
        }

        public ActionResult Resetdonorsearch()
        {
            Session["SortingValidation"] = null;
            Session["Download"] = null;
            return RedirectToAction("DonorSearch");
        }

        [HttpGet]
        [SessionValidateAttribute]
        public ActionResult DonorSearch()
        {
            TestResultDataModel model = new TestResultDataModel();
            FormCollection frm = new FormCollection();

            List<TestResultDataModel> testResult = new List<TestResultDataModel>();
            if (Session["SortingValidation"] != null)
            {
                DonorSearch(model, frm);
                return View();
            }
            else
            {
                Session["Firstname"] = "";
                Session["LastName"] = "";
                Session["SSN"] = "";
                Session["dobday"] = "";
                Session["dobmonth"] = "";
                Session["dobyear"] = "";
                Session["client"] = "";
                Session["department"] = "";
                Session["SpecimenID"] = "";
                Session["Testreasons"] = "";
                Session["categorytest"] = "";
                Session["startdated"] = "";
                Session["startmonth"] = "";
                Session["startyear"] = "";
                Session["enddated"] = "";
                Session["endmonth"] = "";
                Session["endyear"] = "";
                Session["resulttest"] = "";
                Session["includearchived"] = false;
                Session["Day3"] = false;
                Session["Day7"] = false;
                Session["Day30"] = false;
                Session["Day60"] = false;
                Session["Day90"] = false;
                Session["Daterange"] = false;

                Session["Download"] = null;
                Session["SortingValidation"] = null;

                //Session["beforedated"] = "";
                //Session["beforemonth"] = "";
                //Session["beforeyear"] = "";
                //Session["afterdated"] = "";
                //Session["aftermonth"] = "";
                //Session["afteryear"] = "";
                if (ConfigurationManager.AppSettings["DateFilterDelta"] != null)
                {
                    var _interval = ConfigurationManager.AppSettings["DateFilterDelta"].ToString().Trim();
                    int.TryParse(_interval, out this.FilterDayInterval);
                }
                Session["beforeDateFilter"] = DateTime.Now.Date.AddDays(1).ToString("MM/dd/yyyy");
                Session["afterDateFilter"] = DateTime.Now.Date.AddDays(this.FilterDayInterval).ToString("MM/dd/yyyy");
                Session["DonorSearchFilterList"] = ((int)DonorSearchFilterList.None).ToString();

                ClientList();

                //var fla = Enum.GetValues(typeof(DonorSearchFilterList));
                //Session["DonorSearchFilterList"] = new SelectList(fla);

                return View(testResult);
            }
        }

        [HttpPost]
        [SessionValidateAttribute]
        public ActionResult DonorSearch(TestResultDataModel model, FormCollection frmCollection)
        {
            _logger.Debug($"Donor Search");
            Session["SortingValidation"] = frmCollection["searchvalue"] != null ? frmCollection["searchvalue"].ToString() : null;
            Session["SortingValidation1"] = frmCollection["searchvalue1"] != null ? frmCollection["searchvalue1"].ToString() : null;

            int userid = Convert.ToInt32(Session["UserId"].ToString());
            UserType usertype = (UserType)(int)(Session["UserType"]);

            Dictionary<string, string> searchParam = new Dictionary<string, string>();
            List<TestResultDataModel> testResultList = new List<TestResultDataModel>();


            if (Session["SortingValidation1"] != null)
            {
                #region model construction and assignment
                model.FirstName = frmCollection["FirstName"] != null ? frmCollection["FirstName"].ToString() : null;
                model.LastName = frmCollection["LastName"] != null ? frmCollection["LastName"].ToString() : null;
                model.SSN = frmCollection["SSN"] != null ? frmCollection["SSN"].ToString() : null;
                model.DOB = frmCollection["DOB"] != null ? frmCollection["DOB"].ToString() : null;
                model.ClientId = frmCollection["Clients"] != null ? frmCollection["Clients"].ToString() : null;
                model.ClientDepartmentId = frmCollection["Departments"] != null ? frmCollection["Departments"].ToString() : null;
                model.SpecimenId = frmCollection["SpecimenId"] != null ? frmCollection["SpecimenId"].ToString() : null;
                model.StrTestreason = frmCollection["reason"] != "" && frmCollection["reason"] != null ? frmCollection["reason"].ToString() : "0";
                model.StrTestType = frmCollection["TestType"] != "" && frmCollection["TestType"] != null ? frmCollection["TestType"].ToString() : "0";
                model.StartDate = frmCollection["started"] != null ? frmCollection["started"].ToString() : null;
                model.EndDate = frmCollection["ended"] != null ? frmCollection["ended"].ToString() : null;
                model.StrOverallResult = frmCollection["result"] != null && frmCollection["result"] != "" ? frmCollection["result"].ToString() : "0";
                model.IncludeArchived = frmCollection["IncludeArchived"] != null ? Convert.ToBoolean(frmCollection["IncludeArchived"].ToString()) != true ? false : true : false;

                _logger.Debug($"model info: {model.FirstName} {model.LastName}");

                bool days3 = false;
                bool days7 = false;
                bool days30 = false;
                bool days60 = false;
                bool days90 = false;
                bool daterange = false;
                try
                {
                    days3 = Convert.ToBoolean(frmCollection["3daytext"]) != true ? false : true;
                    days7 = Convert.ToBoolean(frmCollection["7daytext"]) != true ? false : true;
                    days30 = Convert.ToBoolean(frmCollection["30daytext"]) != true ? false : true;
                    days60 = Convert.ToBoolean(frmCollection["60daytext"]) != true ? false : true;
                    days90 = Convert.ToBoolean(frmCollection["90daytext"]) != true ? false : true;
                    daterange = Convert.ToBoolean(frmCollection["daterangetxt"]) != true ? false : true;
                }
                catch (Exception ex)
                {
                    var _ex = ex;
                }

                Session["Firstname"] = model.FirstName;
                Session["LastName"] = model.LastName;
                Session["SSN"] = model.SSN;
                if (model.DOB != "" && model.DOB != null)
                {
                    if (Convert.ToDateTime(model.DOB.ToString()) < DateTime.Now)
                    {
                        DateTime DOB_date = Convert.ToDateTime(model.DOB.ToString());
                        Session["dobday"] = DOB_date.Day.ToString();
                        Session["dobmonth"] = DOB_date.Month.ToString();
                        Session["dobyear"] = DOB_date.Year.ToString();
                    }
                    else
                    {
                        ViewBag.doberr = "Invalid date";
                        ClientList();
                        return View(testResultList);
                    }
                }
                else
                {
                    Session["dobday"] = "";
                    Session["dobmonth"] = "";
                    Session["dobyear"] = "";
                }
                
                Session["client"] = model.ClientId;
                Session["department"] = model.ClientDepartmentId;
                Session["SpecimenID"] = model.SpecimenId;
                Session["Testreasons"] = model.StrTestreason;
                Session["categorytest"] = model.StrTestType;


                if (model.StartDate != "" && model.EndDate != "" && model.StartDate != null && model.EndDate != null)
                {
                    if (Convert.ToDateTime(model.StartDate.ToString()) <= DateTime.Now && Convert.ToDateTime(model.EndDate.ToString()) <= DateTime.Now)
                    {
                        DateTime Start_date = Convert.ToDateTime(model.StartDate.ToString());
                        DateTime End_date = Convert.ToDateTime(model.EndDate.ToString());
                        Session["startdated"] = Start_date.Day.ToString();
                        Session["startmonth"] = Start_date.Month.ToString();
                        Session["startyear"] = Start_date.Year.ToString();
                        Session["enddated"] = End_date.Day.ToString();
                        Session["endmonth"] = End_date.Month.ToString();
                        Session["endyear"] = End_date.Year.ToString();
                    }
                    else
                    {
                        ViewBag.daterange = "Invalid date";
                        ClientList();
                        return View(testResultList);
                    }
                }
                else
                {
                    Session["startdated"] = "";
                    Session["startmonth"] = "";
                    Session["startyear"] = "";
                    Session["enddated"] = "";
                    Session["endmonth"] = "";
                    Session["endyear"] = "";
                }
                Session["resulttest"] = model.StrOverallResult;
                Session["includearchived"] = model.IncludeArchived;
                Session["Day3"] = days3;
                Session["Day7"] = days7;
                Session["Day30"] = days30;
                Session["Day60"] = days60;
                Session["Day90"] = days90;
                Session["Daterange"] = daterange;

                if (model.FirstName != string.Empty && model.FirstName != null)
                {
                    searchParam.Add("FirstName", "%" + model.FirstName + "%");
                }

                if (model.LastName != string.Empty && model.LastName != null)
                {
                    searchParam.Add("LastName", "%" + model.LastName + "%");
                }

                if (model.SSN != string.Empty && model.SSN != null)
                {
                    if (model.SSN.Replace("_", "").Replace("-", "").Trim() != string.Empty)
                    {
                        searchParam.Add("SSN", "%" + model.SSN + "%");
                    }
                }

                if (model.DOB != string.Empty && model.DOB != null)
                {
                    DateTime DOBdate = Convert.ToDateTime(model.DOB.ToString());
                    if (DOBdate > DateTime.Now)
                    {
                        return View();
                    }
                    searchParam.Add("DOB", model.DOB);
                }

                if (model.ClientId != "" && model.ClientId != null)
                {
                    string ClientID = UserAuthentication.Decrypt(model.ClientId.ToString(), true);
                    searchParam.Add("Client", ClientID.ToString());
                }

                if (model.ClientDepartmentId != "" && model.ClientDepartmentId != null)
                {
                    string DepartmentID = UserAuthentication.Decrypt(model.ClientDepartmentId.ToString(), true);
                    searchParam.Add("Department", DepartmentID.ToString());
                }

                if (model.SpecimenId != string.Empty && model.SpecimenId != null)
                {
                    searchParam.Add("SpecimenId", "%" + model.SpecimenId + "%");
                }

                if (Convert.ToInt32(model.StrTestreason.ToString()) != 0)
                {
                    searchParam.Add("TestReason", model.StrTestreason.ToString());
                }

                if (Convert.ToInt32(model.StrTestType.ToString()) != 0)
                {

                    searchParam.Add("TestCategory", model.StrTestType.ToString());
                }

                if (Convert.ToInt32(model.StrOverallResult.ToString()) != 0)
                {
                    searchParam.Add("TestResult", model.StrOverallResult.ToString());
                }

                searchParam.Add("IncludeArchive", model.IncludeArchived.ToString());

                if (model.StartDate != string.Empty && model.EndDate != string.Empty && model.StartDate != null && model.EndDate != null)
                {
                    DateTime StartDate = Convert.ToDateTime(model.StartDate.ToString());
                    DateTime EndDate = Convert.ToDateTime(model.EndDate.ToString());

                    searchParam.Add("NoOfDays", "DR#" + StartDate.ToString() + "#" + EndDate.ToString());
                }
                else if (days3 == true)
                {
                    searchParam.Add("NoOfDays", "3");
                }
                else if (days7 == true)
                {
                    searchParam.Add("NoOfDays", "7");
                }
                else if (days30 == true)
                {
                    searchParam.Add("NoOfDays", "30");
                }
                else if (days60 == true)
                {
                    searchParam.Add("NoOfDays", "60");
                }
                else if (days90 == true)
                {
                    searchParam.Add("NoOfDays", "90");
                }

                /// Filter - Persist settings
                Session["DonorSearchFilterList"] = frmCollection["DonorSearchFilterList"] != null ? frmCollection["DonorSearchFilterList"] : "0";
                Session["beforeDateFilter"] = frmCollection["beforeDateFilter"]; //!= null ? frmCollection["DonorSearchFilterList"] : "0";
                Session["afterDateFilter"] = frmCollection["afterDateFilter"]; // != null ? frmCollection["DonorSearchFilterList"] : "0";

                // int.TryParse((string)Session["DonorSearchFilterList"], out int _DonorSearchFilterList);
                searchParam.Add("DonorSearchFilterList", (string)Session["DonorSearchFilterList"]);
                //string _beforeDateFilter = frmCollection["beforeDateFilter"]; //!= null ? frmCollection["beforeDateFilter"] : DateTime.Now.Date.AddYears(-6).ToString();
                searchParam.Add("beforeDateFilter", (string)Session["beforeDateFilter"]);
                //string _afterDateFilter = frmCollection["afterDateFilter"]; // != null ? frmCollection["afterDateFilter"] : DateTime.Now.Date.ToString();
                searchParam.Add("afterDateFilter", (string)Session["afterDateFilter"]);


                //frmCollection["started"] != null ? frmCollection["started"].ToString() : null;
                #endregion //model construction
                DonorBL donorBL = new DonorBL();
                _logger.Debug("Searching");
                DataTable dtDonors = donorBL.SearchDonorByClient(searchParam, usertype, userid);
                List<TestResultDataModel> test = new List<TestResultDataModel>();
                var Vartesttype = "";
                //UserAuthRoles userAuthRoles = (UserAuthRoles)Session["UserAuthRoles"];
                UserAuthRoles userAuthRoles = (UserAuthRoles)Session["UserAuthRoles"] == null ? new UserAuthRoles() : (UserAuthRoles)Session["UserAuthRoles"]; // safe in case session isn't set.
                foreach (DataRow dr in dtDonors.Rows)
                {
                    _logger.Debug($"We got data back {dtDonors.Rows.Count.ToString()}");
                    TestResultDataModel testdata = new TestResultDataModel();

                    string DonorID = dr["DonorId"].ToString();

                    var donordata = donorBL.Get(Convert.ToInt32(DonorID), "Web");
                    try
                    {



                        testdata.backend_notifications_id = Convert.ToInt32((Int64)dr["backend_notifications_id"]);
                        testdata.Notified_by_email_timestamp = dr["Notified_by_email_timestamp"].ToString();
                        testdata.show_web_notify_button = Convert.ToBoolean(dr["show_web_notify_button"]);

                        if (DateTime.TryParse(testdata.Notified_by_email_timestamp, out DateTime dateValue))
                            testdata.has_been_notified = testdata.Notified_by_email_timestamp;
                        else
                            testdata.has_been_notified = String.Empty;

                        if (testdata.show_web_notify_button)
                        {
                            bool Dept_Show_Web_Notify_button = testdata.show_web_notify_button;
                            // set to false if the user isn't TPA type
                            // need to check user auth roles
                            testdata.show_web_notify_button = userAuthRoles.HasRole(AuthorizationRules.WEB_CAN_SEND_IN.ToString());
                            if ((UserType)Session["UserType"] == UserType.TPA && Dept_Show_Web_Notify_button == true)
                            {
                                // if the dept has the button enabled and this user is a TPA, 
                                // then we show the button regardless of the role
                                testdata.show_web_notify_button = false;
                            }

                        }
                        //testdata.show_web_notify_button = true;
                        ViewBag.ShowSendIn = testdata.show_web_notify_button.ToString();
                        if (donordata != null)
                        {
                            if (!string.IsNullOrEmpty(donordata.DonorFirstName.ToString()))
                            {
                                testdata.FirstName = donordata.DonorFirstName.ToString();
                            }
                            else
                            {
                                testdata.FirstName = "";
                            }

                            if (!string.IsNullOrEmpty(donordata.DonorLastName))
                            {
                                testdata.LastName = donordata.DonorLastName.ToString();
                            }
                            else
                            {
                                testdata.LastName = "";
                            }

                            if (!string.IsNullOrEmpty(donordata.DonorSSN))
                            {
                                testdata.SSN = donordata.DonorSSN.ToString();
                                testdata.SSNview = donordata.DonorSSN.ToString();
                                if (testdata.SSN.ToString().Length == 11)
                                {
                                    testdata.SSN = "XXX-XX-" + testdata.SSN.ToString().Substring(7);
                                }
                                else
                                {
                                    testdata.SSN = "";
                                }
                            }
                            else
                            {
                                testdata.SSN = "";
                            }

                            if (donordata.DonorDateOfBirth.ToString("MM/dd/yyyy") != "01/01/0001")
                            {
                                testdata.DOB = donordata.DonorDateOfBirth.ToString("MM/dd/yyyy");
                            }
                            else
                            {
                                testdata.DOB = "";
                            }

                            testdata.SpecimenId = dr["SpecimenId"].ToString();


                            _logger.Debug($"SpecimenID set to {testdata.SpecimenId}");
                            if (string.IsNullOrEmpty(testdata.SpecimenId))
                            {
                                _logger.Debug($"SpecimenID is empty!!");


                            }


                            testdata.DonorTestInfoId = UserAuthentication.Encrypt(dr["DonorTestInfoId"].ToString(), true);
                            testdata.DonorId = donordata.DonorId.ToString();
                            testdata.ClientDepartmentId = dr["TestInfoDepartmentId"].ToString();
                            testdata.ClearStarCode = dr["ClearStarCode"].ToString();
                            testdata.ClientId = dr["TestInfoClientId"].ToString();
                            testdata.ClientName = dr["ClientName"].ToString();
                            testdata.DepartmentName = dr["ClientDepartmentName"].ToString();
                            DonorRegistrationStatus testStatus = (DonorRegistrationStatus)(int)(dr["TestStatus"]);
                            testdata.TestStatus = testStatus.ToDescriptionString();
                            // Per david - if completed status, do not show button
                            if (testdata.TestStatus.Equals("Completed", StringComparison.InvariantCultureIgnoreCase)) testdata.show_web_notify_button = false;
                            testdata.isHiddenWeb = dr["IsHiddenWeb"].ToString();
                            var notapprovedCount = string.Empty;
                            if (!string.IsNullOrEmpty(donordata.NotApproved))
                            {
                                notapprovedCount = " (" + donordata.NotApproved + ")";
                                testdata.RecordKeepingLink = Helper.CreateActionLink(this).ActionLink("Documents" + notapprovedCount, "DocumentManage", "Donor",
                                               new
                                               {
                                                   donorId = testdata.DonorId,
                                                   clientId = testdata.ClientId


                                               }, new { @target = "_blank", @class = "btn btn-primary" }).ToString();

                            }
                            else
                            {
                                testdata.RecordKeepingLink = Helper.CreateActionLink(this).ActionLink("Documents", "DocumentManage", "Donor",
                                               new
                                               {
                                                   donorId = testdata.DonorId,
                                                   clientId = testdata.ClientId


                                               }, new { @target = "_blank", @class = "btn btn-primary" }).ToString();

                            }
                            //todo:Mk create different color buttons here for show
                            testdata.RecordKeepingLink = Helper.CreateActionLink(this).ActionLink("Documents " + notapprovedCount, "DocumentManage", "Donor",
                                                 new
                                                 {
                                                     donorId = testdata.DonorId,
                                                     clientId = testdata.ClientId


                                                 }, new { @target = "_blank", @class = "btn" }).ToString();

                            if (dr["SpecimenDate"] != DBNull.Value)
                            {
                                var specimendate = dr["SpecimenDate"].ToString();

                                if (string.IsNullOrEmpty(specimendate.ToString()))
                                {
                                    testdata.TestDate = specimendate.ToString();
                                }
                                else
                                {
                                    if (Convert.ToDateTime(specimendate.ToString()) != DateTime.MinValue)
                                    {
                                        testdata.TestDate = Convert.ToDateTime(specimendate.ToString()).ToString("MM/dd/yyyy");
                                    }
                                }
                            }

                            if (dr["ReasonForTestId"].ToString() != string.Empty)
                            {
                                TestInfoReasonForTest Status = (TestInfoReasonForTest)(int)(dr["ReasonForTestId"]);
                                testdata.StrTestreason = Helper.GetEnumDescription((TestInfoReasonForTest)(Convert.ToInt32(dr["ReasonForTestId"])));
                            }
                            else
                            {
                                testdata.StrTestreason = string.Empty;
                            }

                            if (dr["TestOverallResult"].ToString() != string.Empty)
                            {
                                OverAllTestResult result = (OverAllTestResult)(int)(dr["TestOverallResult"]);
                                if (result.ToString() != "None")
                                {
                                    testdata.StrOverallResult = result.ToString();
                                }
                                else
                                {
                                    testdata.StrOverallResult = "";
                                }
                            }
                            else
                            {
                                testdata.StrOverallResult = string.Empty;
                            }
                            if (dr["TestCategoryId"].ToString() != string.Empty)
                            {
                                string TestCategory = dr["TestCategoryId"].ToString();
                                Array Arytesttype = TestCategory.Split(',').Distinct().Select(r => (TestCategories)Enum.Parse(typeof(TestCategories), r)).ToArray();
                                string[] strtesttype = Arytesttype.OfType<object>().Select(o => o.ToString()).ToArray();
                                Vartesttype = strtesttype.Aggregate((x, y) => x + " & " + y);
                                testdata.StrTestType = Vartesttype;
                            }
                            else
                            {
                                testdata.StrTestType = string.Empty;
                            }

                            if (donordata.DonorClearStarProfId != null)// This checkes to see if there is a ClearStarID if so it sets it to inprogress
                            {
                                testdata.DonorClearStarProfId = "In Progress";
                                //testdata.SpecimenId = donordata.DonorClearStarProfId;
                                string sCustID = string.Empty;
                                var csreports = ConfigurationManager.AppSettings["ClearStarReports"].ToString().Trim();
                                try
                                {
                                    var creds = DefaultCredentialsBL.GetCredentials();
                                    //string sCustID = "SLSS_00008";
                                    sCustID = testdata.ClearStarCode;

                                    string ProfileNo = donordata.DonorClearStarProfId;//Becca D Test
                                    Surpath.ClearStar.BL.ProfileBL profile = new ProfileBL();

                                    var exists = System.IO.File.Exists(csreports + ProfileNo + ".pdf");
                                    if (!exists)
                                    {

                                        var result = profile.GetProfileReport(sCustID, ProfileNo);
                                        var file = profile.SaveProfileReport(sCustID, ProfileNo, result);
                                    }
                                }
                                catch (System.Exception ex)
                                {
                                    var blah = ex;
                                }
                                if (System.IO.File.Exists(csreports + donordata.DonorClearStarProfId + ".pdf"))
                                {
                                    testdata.DonorClearStarProfId = Helper.CreateActionLink(this).ActionLink("Complete", "ViewProfileDocument", "BackgroundCheck",
                                                     new
                                                     {
                                                         sCustId = sCustID,
                                                         profileId = donordata.DonorClearStarProfId



                                                     }, new { @target = "_blank", @class = "btn btn-profile" }).ToString();
                                }

                                //testdata.DonorClearStarProfId = "In Progress";

                                //testdata.MRODownloadLink = Helper.CreateActionLink(this).ActionLink("MRO Report", "ViewDocument", "Donor",
                                //                 new
                                //                 {
                                //                     documentID = donorDocumentId

                                //                 }, new { @target = "_blank", @class = "btn btn-primary" }).ToString();


                            }

                            _logger.Debug($"TABLE: DonorTestInfoId {testdata.DonorTestInfoId} DonorId{testdata.DonorId}  ClientDepartmentId {testdata.ClientDepartmentId} ClearStarCode {testdata.ClearStarCode} TestStatus {testdata.TestStatus}");

                            if (testdata.TestStatus.ToUpper() == "COMPLETED" && dr["IsDonorRefused"].ToString() != "1" || (testdata.TestStatus.ToUpper() == "PROCESSING"))
                            {
                                _logger.Debug("We have test data, status COMPLETE or PROCESSING");

                                int MROValue = 0;
                                string MROType = dr["MROTypeId"].ToString();

                                bool IsPositiveProcessing = false;
                                if (testdata.TestStatus.ToUpper() == "PROCESSING" && testdata.StrOverallResult.ToString().ToUpper() == "Positive".ToUpper())
                                {
                                    IsPositiveProcessing = true;

                                }

                                //Original Code what shows link.
                                if ((MROType.ToString() == "2") || (MROType.ToString() == "1")) // && testdata.StrOverallResult.ToString() == "Positive"))
                                {
                                    int testInfoId = Convert.ToInt32(dr["DonorTestInfoId"].ToString());
                                    reportsLab = donorBL.GetLabReport(testInfoId, reportTypeLab);
                                    reportsQuest = donorBL.GetLabReport(testInfoId, reportTypeQuest);
                                    reports = donorBL.GetMROReport(testInfoId, reportTypeMRO);
                                    if (reports != null && !IsPositiveProcessing)
                                    {
                                        int documentReportId = reports.FinalReportId;

                                        if (documentReportId != 0)
                                        {
                                            //DonorDocument donorDocument = donorBL.GetDonorDocument(documentReportId, false);
                                            DonorDocument donorDocument = donorBL.GetDonorDocument(documentReportId);
                                            int donorDocumentId = 0;
                                            try
                                            {
                                                if (!(donorDocument == null))
                                                {


                                                    donorDocumentId = donorDocument.DonorDocumentId;



                                                    string fileName = donorDocument.FileName;
                                                    string documentTitle = donorDocument.DocumentTitle;
                                                    byte[] content = donorDocument.DocumentContent;


                                                    testdata.MRODownloadLink = Helper.CreateActionLink(this).ActionLink("MRO Report", "ViewDocument", "Donor",
                                                     new
                                                     {
                                                         documentID = donorDocumentId

                                                     }, new { @target = "_blank", @class = "btn btn-primary" }).ToString();
                                                    testdata.DownloadLink = Helper.CreateActionLink(this).Label("").ToString();



                                                    if (MROType.ToString() == "2")
                                                    {
                                                        MROValue = 0;
                                                    }
                                                    else
                                                    {
                                                        MROValue = 1;
                                                    }
                                                }
                                            }
                                            catch (Exception)
                                            {


                                            }
                                        }
                                        else
                                        {
                                            testdata.MRODownloadLink = Helper.CreateActionLink(this).Label("").ToString();
                                        }

                                    }
                                    else
                                    {
                                        testdata.MRODownloadLink = Helper.CreateActionLink(this).Label("").ToString();
                                    }
                                }
                                else
                                {
                                    testdata.MRODownloadLink = Helper.CreateActionLink(this).Label("").ToString();
                                }


                                //New Code that fixes status.  //TODO: Mike Need to refactor this code when I care sometime - The above code duplicates this code below   but fixes overallstatus on positive
                                if (testdata.TestStatus.ToUpper() == "COMPLETED" && dr["IsDonorRefused"].ToString() != "1" || (testdata.TestStatus.ToUpper() == "PROCESSING"))
                                {

                                    if ((MROType.ToString() == "2") || (MROType.ToString() == "1") || (MROType.ToString() == "3")) // && testdata.StrOverallResult.ToString() == "Positive"))
                                    {

                                        if (testdata.StrOverallResult.ToString() == "Positive" && !IsPositiveProcessing)
                                        //if (testdata.StrOverallResult.ToString() == "")
                                        {
                                            int testInfoId = Convert.ToInt32(dr["DonorTestInfoId"].ToString());
                                            reportsLab = donorBL.GetLabReport(testInfoId, reportTypeLab);
                                            if (reportsLab != null) _logger.Debug($"reportsLab ID: {reportsLab.ReportId}");
                                            reportsQuest = donorBL.GetLabReport(testInfoId, reportTypeQuest);
                                            if (reportsQuest!=null) _logger.Debug($"reportsQuest ID: {reportsQuest.ReportId}");
                                            reports = donorBL.GetMROReport(testInfoId, reportTypeMRO);
                                            if (reports != null) _logger.Debug($"reports ID: {reports.ReportId}");

                                            testdata.MRODownloadLink = Helper.CreateActionLink(this).Label("").ToString();
                                            //testdata.StrOverallResult = "";
                                            //testdata.TestOverallResult = "";
                                        }
                                        else
                                        {
                                            testdata.MRODownloadLink = Helper.CreateActionLink(this).Label("").ToString();
                                        }
                                    }

                                    if (reports != null && !IsPositiveProcessing)
                                    {
                                        int documentReportId = reports.FinalReportId;

                                        if (documentReportId != 0)
                                        {
                                            //DonorDocument donorDocument = donorBL.GetDonorDocument(documentReportId);
                                            DonorDocument donorDocument = donorBL.GetDonorDocument(documentReportId, false);
                                            int donorDocumentId = 0;
                                            try
                                            {
                                                if (!(donorDocument == null))
                                                {
                                                    donorDocumentId = donorDocument.DonorDocumentId;
                                                    string fileName = donorDocument.FileName;
                                                    string documentTitle = donorDocument.DocumentTitle;
                                                    byte[] content = donorDocument.DocumentContent;
                                                    testdata.MRODownloadLink = Helper.CreateActionLink(this).ActionLink("MRO Report", "ViewDocument", "Donor",
                                                     new
                                                     {
                                                         documentID = donorDocumentId

                                                     }, new { @target = "_blank", @class = "btn btn-primary" }).ToString();
                                                    _logger.Debug($"DownloadLink set 1");
                                                    testdata.DownloadLink = Helper.CreateActionLink(this).Label("").ToString();

                                                    if (MROType.ToString() == "2")
                                                    {
                                                        MROValue = 0;
                                                    }
                                                    else
                                                    {
                                                        MROValue = 1;
                                                    }
                                                }

                                            }
                                            catch (Exception)
                                            {
                                            }
                                        }
                                        else
                                        {
                                            testdata.MRODownloadLink = Helper.CreateActionLink(this).Label("").ToString();
                                        }
                                    }
                                    else
                                    {

                                        if ((reportTypeMRO == ReportType.MROReport || reportTypeMRO == ReportType.QuestLabReport) && testdata.StrOverallResult.ToUpper() == "NEGATIVE")
                                        {
                                            testdata.MRODownloadLink = Helper.CreateActionLink(this).Label("").ToString();
                                            OverAllTestResult result = (OverAllTestResult)(int)(dr["TestOverallResult"]);


                                            testdata.StrOverallResult = result.ToString();
                                            testdata.TestOverallResult = "";
                                        }
                                        else
                                        {
                                            testdata.StrOverallResult = "";
                                            testdata.TestOverallResult = "";
                                        }
                                    }

                                }

                                //End of New Code Fix


                                _logger.Debug("After code fix");
                                _logger.Debug($"testdata.DownloadLink = {testdata.DownloadLink}");

                                _logger.Debug($"MROValue {MROValue}");
                                if (MROValue == 0)
                                {
                                    _logger.Debug($"MROValue is 0");
                                    if ((MROType.ToString() == "2") || (MROType.ToString() == "1" && testdata.TestStatus.ToUpper() == "COMPLETED"))
                                    {
                                        _logger.Debug($"DownloadLink set 6");

                                        if (dr["IsInstantTest"].ToString() == "1" && dr["InstantTestResult"].ToString() == "2")
                                        {
                                            _logger.Debug($"DownloadLink set 1");

                                            testdata.DownloadLink = Helper.CreateActionLink(this).Label("").ToString();
                                            _logger.Debug(testdata.DownloadLink);

                                        }
                                        else
                                        {
                                            _logger.Debug("Not IsInstantTest=1 and InstantTestResult=2");
                                            int testInfo = Convert.ToInt32(dr["DonorTestInfoId"].ToString());
                                            if (dr["SpecimenId"].ToString() != string.Empty)
                                            {
                                                testdata.SpecimenId = dr["SpecimenId"].ToString();
                                                if (testdata.SpecimenId.Contains(','))
                                                {
                                                    string[] specimen = testdata.SpecimenId.Split(',');
                                                    testdata.SpecimenId = specimen[0].Trim().ToString();
                                                }
                                            }

                                            reporLabType = donorBL.GetLabReportType(testInfo, testdata.SpecimenId, reportTypeMRO);
                                            
                                            if (reporLabType == null)
                                            {
                                                _logger.Debug("reporLabType is null");

                                                if (reportsLab != null || reportsQuest != null)
                                                {
                                                    _logger.Debug("reportsLab or reportsQuest is NOT null");
                                                    if (MROType.ToString() == "1" && testdata.TestStatus.ToUpper() == "COMPLETED" && testdata.StrOverallResult.ToUpper().ToString() == "NEGATIVE")// && reportsLab.ReportType.ToString().ToUpper() == "LABREPORT")
                                                    {
                                                        _logger.Debug($"DownloadLink set 2 - MROType.ToString() {MROType.ToString()} {testdata.TestStatus.ToUpper()} {testdata.StrOverallResult.ToUpper().ToString()} {reportsLab.ReportType.ToString()}");
                                                        testdata.DownloadLink = Helper.CreateActionLink(this).ActionLink("Lab Report", "Result", "Client",
                                                           new
                                                           {
                                                               specimenId = testdata.SpecimenId,
                                                               name = testdata.FirstName + "  " + testdata.LastName
                                                           }, new { @target = "_blank", @class = "btn btn-primary" }).ToString();
                                                        _logger.Debug(testdata.DownloadLink);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                testdata.DownloadLink = Helper.CreateActionLink(this).Label("").ToString();
                                            }

                                            if (MROType.ToString() == "2")
                                            {
                                                _logger.Debug($"DownloadLink set 3");
                                                testdata.DownloadLink = Helper.CreateActionLink(this).ActionLink("Lab Report", "Result", "Client",
                                                        new
                                                        {
                                                            specimenId = testdata.SpecimenId,
                                                            name = testdata.FirstName + "  " + testdata.LastName
                                                        }, new { @target = "_blank", @class = "btn btn-primary" }).ToString();
                                            }

                                        }
                                    }
                                    else
                                    {
                                        testdata.DownloadLink = Helper.CreateActionLink(this).Label("").ToString();
                                    }
                                    if ((MROType.ToString() == "2") || (MROType.ToString() == "1" && testdata.TestStatus.ToUpper() == "COMPLETED" && testdata.StrOverallResult.ToUpper().ToString() != "Positive" && (reportsLab != null || reportsQuest != null)))
                                    {
                                        if (dr["IsInstantTest"].ToString() == "1" && dr["InstantTestResult"].ToString() == "2")
                                        {
                                            _logger.Debug($"DownloadLink set 4");
                                            testdata.DownloadLink = Helper.CreateActionLink(this).Label("").ToString();
                                            _logger.Debug(testdata.DownloadLink);

                                        }
                                        else
                                        {
                                            if (dr["SpecimenId"].ToString() != string.Empty)
                                            {
                                                testdata.SpecimenId = dr["SpecimenId"].ToString();
                                            }
                                            _logger.Debug($"DownloadLink set 5");
                                            testdata.DownloadLink = Helper.CreateActionLink(this).ActionLink("Lab Report", "Result", "Client",
                                            new
                                            {
                                                specimenId = testdata.SpecimenId,
                                                name = testdata.FirstName + "  " + testdata.LastName
                                            }, new { @target = "_blank", @class = "btn btn-primary" }).ToString();
                                            _logger.Debug(testdata.DownloadLink);
                                        }
                                    }
                                    else
                                    {
                                        testdata.DownloadLink = Helper.CreateActionLink(this).Label("").ToString();
                                    }
                                }
                            }
                            else
                            {
                                _logger.Debug($"DownloadLink set 6");
                                testdata.DownloadLink = Helper.CreateActionLink(this).Label("").ToString();
                                _logger.Debug(testdata.DownloadLink);

                                testdata.MRODownloadLink = Helper.CreateActionLink(this).Label("").ToString();
                            }
                            if (dr["IsHiddenWeb"].ToString() == "1")
                            {
                                _logger.Debug($"DownloadLink set 6");
                                testdata.MRODownloadLink = "<span class='important'>Hidden</span>";
                                testdata.DownloadLink = "<span class='important'>Results:</span>"; //string.Empty;
                                _logger.Debug(testdata.DownloadLink);

                                testdata.StrOverallResult = "Hidden";
                                testdata.TestOverallResult = "Hidden";

                            }

                            if (testdata.MRODownloadLink.ToString().Length >= 1 && dr["IsHiddenWeb"].ToString() == "0")
                            {
                                testdata.DownloadLink = ""; //string.Empty;
                            }
                            if (testdata.TestStatus == DonorRegistrationStatus.Processing.ToString())
                            {
                                testdata.DownloadLink = ""; //string.Empty;
                            }
                            if (testdata.TestStatus == DonorRegistrationStatus.Completed.ToString() && testdata.StrOverallResult == OverAllTestResult.Positive.ToString())
                            {
                                //todo:mkearl Eventually this will code should not be needed as the HL7 process should clean up the reports
                                //this is a Hack and is basically our insurance policy.
                                testdata.DownloadLink = ""; //string.Empty;
                                if (string.IsNullOrEmpty(testdata.MRODownloadLink))
                                { testdata.MRODownloadLink = "<span class='notice'>Pending</span>"; }


                            }

                            test.Add(testdata);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex.ToString());
                        if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                        if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());

                    }

                }
                Session["Download"] = test;
                var items = test;
                Session["SortingValidation"] = "1";
                Session["SortingValidation1"] = null;
                ClientList();
                ViewBag.usertype = usertype;
                return View(items);
            }
            #region Sorting Validation
            else
            {
                List<TestResultDataModel> test = new List<TestResultDataModel>();
                test = (List<TestResultDataModel>)Session["Download"];
                var items = test;
                Session["SortingValidation"] = "1";
                Session["SortingValidation1"] = null;
                ClientList();
                return View(items);
            }
            #endregion//Sorting validation
        }

        [HttpPost]
        public JsonResult NotifyDonor(DonorNotifyJSONModel model)
        {
            int CurrentuserID = Convert.ToInt32(Session["UserId"]);
            string Username = (string)Session["UserLoginName"];
            Username = (string.IsNullOrEmpty(Username)) ? "User ID " + CurrentuserID.ToString() : Username;

            _logger.Debug("NotifyDonor called");

            // send it to the database
            bool doNow = model.NotifyNow;
            bool doNextWindow = false;
            doNextWindow = !doNow;
            int donor_test_info_id;
            int.TryParse((UserAuthentication.Decrypt(model.donor_test_info_id.ToString(), true)), out donor_test_info_id);
            _logger.Debug($"NotifyDonor donor_test_info_id={donor_test_info_id}");
            if (donor_test_info_id < 1)
            {
                model.Queued = false;
                model.Msg = "There's a problem notifying this user.";
                return Json(model);
            }
            else
            {
                bool result = backendLogic.NotificationSetForTransmission(donor_test_info_id, doNextWindow, doNow, CurrentuserID);
                model.Queued = true;
                model.Msg = "Notification Queued.";
            }
            model.donor_test_info_id = UserAuthentication.Encrypt(donor_test_info_id.ToString(), true);
            return Json(model);
        }

        [HttpPost]
        public JsonResult GetDepartments(string ClientName)
        {
            int CurrentuserID = Convert.ToInt32(Session["UserId"]);
            if (ClientName != null && ClientName != "")
            {
                int ClientId = Convert.ToInt32(UserAuthentication.Decrypt(ClientName, true));

                var ClientDepartments = clientBL.GetClientDepartmentList(ClientId);

                if (ClientDepartments != null)
                {
                    List<SelectListItem> departments = new List<SelectListItem>();

                    departments.Add(new SelectListItem { Text = "Select Program", Value = "", Selected = true });

                    foreach (var item in ClientDepartments)
                    {
                        departments.Add(new SelectListItem
                        {
                            Text = item.DepartmentName,
                            Value = UserAuthentication.Encrypt(item.ClientDepartmentId.ToString(), true)
                        });
                    }

                    ViewBag.Department = departments;
                    return Json(new { Success = 1, department = departments });
                }
                else
                {
                    return Json(new { Success = -1 });
                }
            }
            else
            {
                return Json(new { Success = -1 });
            }
        }

        public void ClientList()
        {
            UserBL userBL = new UserBL();
            string id = null;
            int userid = Convert.ToInt32(Session["UserId"].ToString());
            DataTable departmentIDList = userBL.GetUserDepartment(userid);

            List<SelectListItem> Clients = new List<SelectListItem>();
            List<SelectListItem> ReasonList = new List<SelectListItem>();
            List<SelectListItem> TestTypeList = new List<SelectListItem>();
            List<SelectListItem> ResultList = new List<SelectListItem>();

            Clients.Add(new SelectListItem { Text = "Select Client", Value = "", Selected = true });
            ReasonList.Add(new SelectListItem { Text = "Select Reason", Value = "", Selected = true });
            TestTypeList.Add(new SelectListItem { Text = "Select Test Category", Value = "", Selected = true });
            ResultList.Add(new SelectListItem { Text = "Select Test Result", Value = "", Selected = true });

            foreach (DataRow dr in departmentIDList.Rows)
            {
                string departmentID = dr["ClientDepartmentId"].ToString();
                DataTable data = clientBL.GetClientDashboard(departmentID);

                foreach (DataRow dr1 in data.Rows)
                {
                    TestResultDataModel clientdata = new TestResultDataModel();

                    clientdata.ClientDepartmentId = dr1["ClientDepartmentId"].ToString();
                    clientdata.ClientId = dr1["ClientId"].ToString();

                    if (clientdata.ClientId != null && id != clientdata.ClientId)
                    {
                        id = clientdata.ClientId;
                        Client client = clientBL.Get(Convert.ToInt32(clientdata.ClientId));
                        clientdata.ClientName = client.ClientName.ToString();

                        Clients.Add(new SelectListItem
                        {
                            Text = clientdata.ClientName,
                            Value = UserAuthentication.Encrypt(clientdata.ClientId.ToString(), true)
                        });
                    }
                }
            }

            foreach (string reason in Enum.GetNames(typeof(TestInfoReasonForTest)))
            {
                TestInfoReasonForTest testreason = (TestInfoReasonForTest)Enum.Parse(typeof(TestInfoReasonForTest), reason);
                string reasons = testreason.ToString();

                ReasonList.Add(new SelectListItem
                {
                    Text = reasons,
                    Value = ((int)testreason).ToString()
                });

            }

            foreach (string TestCategory in Enum.GetNames(typeof(TestCategories)))
            {
                TestCategories TestCategories = (TestCategories)Enum.Parse(typeof(TestCategories), TestCategory);
                string Category = TestCategories.ToString();

                TestTypeList.Add(new SelectListItem
                {
                    Text = Category,
                    Value = ((int)TestCategories).ToString()
                });

            }

            foreach (string result in Enum.GetNames(typeof(InstantTestResult)))
            {
                InstantTestResult TestResult = (InstantTestResult)Enum.Parse(typeof(InstantTestResult), result);
                string Results = TestResult.ToString();

                ResultList.Add(new SelectListItem
                {
                    Text = Results,
                    Value = ((int)TestResult).ToString()
                });

            }

            ViewBag.Clients = Clients;
            ViewBag.Reasons = ReasonList;
            ViewBag.Testcategory = TestTypeList;
            ViewBag.Testresult = ResultList;
        }

        [HttpGet]
        public ActionResult Export(string clientId, string donorid, string donortestinfoid, string ExportFormat, string firstname, string lastname)
        {
            TestResultDataModel model = new TestResultDataModel();
            List<TestResultDataModel> downloadtable = new List<TestResultDataModel>();

            string name = firstname + " " + lastname;

            if (donorid != null && donortestinfoid != null)
            {
                string donorTestinfoID = UserAuthentication.Decrypt(donortestinfoid.ToString(), true);
                model.DonorId = donorid;
                model.DonorTestInfoId = donortestinfoid;
            }

            model.Exportformat = ExportFormat != "0" && ExportFormat != null ? ExportFormat.ToString() : "0";

            model.ClientId = clientId != null ? clientId.ToString() : null;

            if (model.DonorId == null && model.DonorTestInfoId == null)
            {
                if (Session["Download"] != null)
                {
                    downloadtable = (List<TestResultDataModel>)Session["Download"];
                }
                else
                {
                    return RedirectToAction("DonorSearch", "Client");
                }
            }

            var items = downloadtable;
            download(model.Exportformat, items, name);

            return View();
        }

        [HttpGet]
        public ActionResult PrintPDFAll(string clientId, string donorid, string donortestinfoid, string ExportFormat, string firstname, string lastname)
        {
            TestResultDataModel model = new TestResultDataModel();
            List<TestResultDataModel> downloadtable = new List<TestResultDataModel>();


            string name = firstname + " " + lastname;

            if (donorid != null && donortestinfoid != null)
            {
                string donorTestinfoID = UserAuthentication.Decrypt(donortestinfoid.ToString(), true);
                model.DonorId = donorid;
                model.DonorTestInfoId = donortestinfoid;
            }

            model.Exportformat = ExportFormat != "0" && ExportFormat != null ? ExportFormat.ToString() : "0";

            model.ClientId = clientId != null ? clientId.ToString() : null;

            if (model.DonorId == null && model.DonorTestInfoId == null)
            {
                if (Session["Download"] != null)
                {
                    downloadtable = (List<TestResultDataModel>)Session["Download"];
                }
                else
                {
                    return RedirectToAction("DonorSearch", "Client");
                }
            }

            var items = downloadtable;


            var blah = string.Empty;
            downloadAllPdf(items);

            return View("Export");

        }

        public void download(string Exporttype, List<TestResultDataModel> trData, string name)
        {
            GridView gridvw = new GridView();
            List<ExportModel> testResultDataModel = new List<ExportModel>();
            ExportModel trDataModel = new ExportModel();

            foreach (TestResultDataModel trd in trData)
            {
                var testModel = new ExportModel
                {
                    Specimen_ID = trd.SpecimenId,
                    First_Name = trd.FirstName,
                    Last_Name = trd.LastName,
                    SSN = trd.SSNview,
                    DOB = trd.DOB,
                    Test_Status = trd.TestStatus,
                    Tested_Date = trd.TestDate,
                    Client_Name = trd.ClientName,
                    Program = trd.DepartmentName,
                    Final_Result = trd.StrOverallResult,
                    Test_Type = trd.StrTestType,
                    Test_Reason = trd.StrTestreason
                };
                testResultDataModel.Add(testModel);

            }

            var items = testResultDataModel;

            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gridvw.AllowPaging = false;
            gridvw.DataSource = items.ToList().Take(items.Count);

            gridvw.DataBind();


            if (items.Count != 0)
            {
                gridvw.HeaderRow.Style.Add("width", "17%");
                gridvw.HeaderRow.Style.Add("font-size", "11px");
                gridvw.Style.Add("text-decoration", "none");
                gridvw.Style.Add("font-family", "Arial, Helvetica, sans-serif");
                gridvw.Style.Add("font-size", "10px");
                for (int i = 0; i < gridvw.HeaderRow.Cells.Count; i++)
                {
                    gridvw.HeaderRow.Cells[i].Style.Add("background", "#CCCCFF");
                    gridvw.HeaderRow.Cells[i].Text = gridvw.HeaderRow.Cells[i].Text.Replace("_", " ");
                }
            }
            gridvw.RenderControl(hw);

            if (Exporttype == "0")
            {
                Response.ContentType = "application/pdf";
                if (items.Count != 0)
                {
                    Response.AddHeader("content-disposition", "attachment;filename=SurPath.pdf");
                }
                else
                {
                    Response.AddHeader("content-disposition", "attachment;filename=" + name + ".pdf");
                }
                Response.Cache.SetCacheability(HttpCacheability.NoCache);

                StringReader sr = new StringReader(sw.ToString());
                StreamReader reader = new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(sw.ToString())));
                Document pdfDoc = new Document(PageSize.A2, 7f, 7f, 7f, 0f);
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();
                htmlparser.Parse(reader);

                if (items.Count == 0)
                {
                    pdfDoc.NewPage();
                    pdfDoc.Add(new Paragraph(" "));
                }
                pdfDoc.Close();
                Response.Write(pdfDoc);
                Response.End();

            }
            else if (Exporttype == "1")
            {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=SurPath.doc");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-word ";

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            else if (Exporttype == "2")
            {
                gridvw.DataSource = items.ToList().Take(items.Count);
                gridvw.DataBind();
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment;filename=SurPath.xls");
                Response.ContentType = "application/excel";

                Response.Write(sw.ToString());
                Response.End();
            }
            else
            {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=SurPath.csv");
                Response.Charset = "";
                Response.ContentType = "application/text";

                StringBuilder sb = new StringBuilder();
                for (int k = 0; k < gridvw.Rows[0].Cells.Count; k++)
                {
                    sb.Append(gridvw.Rows[0].Cells[k].Text + ',');
                }
                sb.Append("\r\n");
                for (int i = 0; i < gridvw.Rows.Count; i++)
                {
                    for (int k = 0; k < gridvw.Rows[0].Cells.Count; k++)
                    {
                        sb.Append(gridvw.Rows[i].Cells[k].Text + ',');
                    }
                    sb.Append("\r\n");
                }
                Response.Output.Write(sb.ToString());
                Response.Flush();
                Response.End();

            }
        }

        public void downloadAllPdf(List<TestResultDataModel> trData)
        {

            GridView gridvw = new GridView();
            List<ExportModel> testResultDataModel = new List<ExportModel>();
            ExportModel trDataModel = new ExportModel();


            List<string> FileNames = new List<string>();

            foreach (TestResultDataModel trd in trData)
            {

                var fileName = string.Empty;
                if (trd.MRODownloadLink.Length >= 1 && trd.MRODownloadLink != "")
                {
                    fileName = trd.MRODownloadLink;
                    FileNames.Add(fileName);
                }
                if (trd.DownloadLink.Length >= 1 && trd.DownloadLink != "")
                {
                    fileName = trd.DownloadLink;
                    FileNames.Add(fileName);
                }
            }

            String[] Files = FileNames.ToArray();
            GetStrippedPDFFiles(Files);

        }

        public void GetStrippedPDFFiles(string[] files)
        {
            List<string> Filenames = new List<string>();
            foreach (string f in files)
            {
                var file = f;
                //LabReport Stripper
                if (f.Contains("<a class=\"btn btn-primary\" href=\"/Client/Result?"))
                {
                    file = f.Replace("<a class=\"btn btn-primary\" href=\"/Client/Result?", "");
                    file = file.Replace("\" target=\"_blank\">Lab Report</a>", "");
                    file = file.Replace("%20%20", " ");
                    file = file.Replace("&amp;", "");
                }
                //MROReport Stripper
                if (f.Contains("<a class=\"btn btn-primary\" href=\"/Donor/ViewDocument?"))
                {
                    file = f.Replace("<a class=\"btn btn-primary\" href=\"/Donor/ViewDocument?", "");
                    file = file.Replace("\" target=\"_blank\">MRO Report</a>", "");
                }
                Filenames.Add(file);


            }

            //String[] Files = @"c:\temp\1.pdf, c:\temp\2.pdf".Split(',');
            String[] Files = Filenames.ToArray();
            MergePDFs(Files);
        }

        public void MergePDFs(string[] sourceFiles)
        {
            String ts = GetTimestamp(DateTime.Now);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            string textReportPath = ConfigurationManager.AppSettings["LabReportFilePath"].ToString().Trim();
            string destinationFile = textReportPath + "all_" + ts + ".pdf";
            List<string> NewFileNames = new List<string>();
            foreach (string file in sourceFiles)
            {
                try
                {
                    if (file.Contains("specimenId"))
                    {
                        int name = file.IndexOf("name=");

                        string specimenId = file.Remove(name, file.Length - name);
                        specimenId = specimenId.Replace("specimenId=", "");
                        ReportInfo rtInfo = new ReportInfo();

                        rtInfo = hl7Parser.GetReportDetails(ReportType.LabReport, specimenId, rtInfo);

                        rtInfo.LabReport = rtInfo.LabReport.Replace(@"\line", "");
                        rtInfo.LabReport = rtInfo.LabReport.Replace(@"{", "");
                        rtInfo.LabReport = rtInfo.LabReport.Replace(@"}", "");

                        Response.Cache.SetCacheability(HttpCacheability.NoCache);

                        string Report = rtInfo.LabReport.Remove(0, 174);
                        String timeStamp = GetTimestamp(DateTime.Now);
                        string folderName = textReportPath;
                        string path = textReportPath + specimenId + "_" + timeStamp + ".pdf";

                        if (path != null)
                        {
                            Report = Report.Replace("\r\n", "\n");
                            String[] content = Report.Split('\n');

                            PrintPDF pdf = new PrintPDF();
                            pdf.SavePDF(path, content);


                            NewFileNames.Add(path);


                        }
                    }
                    if (file.Contains("documentID"))
                    {

                        String timeStamp = GetTimestamp(DateTime.Now);
                        string documentID = file.Replace("documentID=", "");

                        DonorDocument donorDocument = donorBL.GetDonorDocument(Convert.ToInt32(documentID));
                        DonorProfileDataModel document = new DonorProfileDataModel();

                        if (donorDocument != null)
                        {
                            document.filename = donorDocument.FileName;
                            document.documentContent = donorDocument.DocumentContent;
                            document.DocumentID = donorDocument.DonorDocumentId.ToString();
                            document.DocumentTitle = donorDocument.DocumentTitle;
                        }
                        string path = textReportPath + documentID + "_" + timeStamp + ".pdf";
                        System.IO.File.WriteAllBytes(path, document.documentContent);
                        NewFileNames.Add(path);
                    }
                }
                catch (Exception ex)
                {

                    Debug.WriteLine("Error Processing File" + ex.StackTrace);
                }

            }

            if (System.IO.File.Exists(destinationFile))
                System.IO.File.Delete(destinationFile);

            using (FileStream stream = new FileStream(destinationFile, FileMode.Create))
            using (Document doc = new Document())
            using (PdfCopy pdf = new PdfCopy(doc, stream))
            {
                doc.Open();

                PdfReader reader = null;
                PdfImportedPage page = null;

                //fixed typo
                NewFileNames.ForEach(file =>
                {
                    reader = new PdfReader(file);

                    for (int i = 0; i < reader.NumberOfPages; i++)
                    {
                        page = pdf.GetImportedPage(reader, i + 1);
                        pdf.AddPage(page);
                    }

                    pdf.FreeReader(reader);
                    reader.Close();
                    if (System.IO.File.Exists(file))
                    {
                        System.IO.File.Delete(file);
                    }
                    //file.Delete(file);
                });
            }
            try
            {
                Response.ContentType = "Application/pdf";
                //Response.AppendHeader("Content-Disposition", destinationFile);
                Response.AddHeader("Content-Disposition", "attachment; filename=" + destinationFile);
                Response.TransmitFile(destinationFile);
                //Response.BinaryWrite((byte[])destinationFile);


                Response.End();

                //Response.ClearContent();
                //Response.ContentType = "application/pdf";
                //Response.AddHeader("Content-Disposition", "inline; filename=" + destinationFile);
                //Response.AddHeader("Content-Length", docSize.ToString());
                //Response.BinaryWrite((byte[])docStream);
                //Response.End();


                //Response.ContentType = "application/pdf";


                //Response.AddHeader("content-disposition", "attachment;filename=" + destinationFile);

                //Response.Cache.SetCacheability(HttpCacheability.NoCache);

                //StringReader sr = new StringReader(sw.ToString());
                //StreamReader reader2 = new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(sw.ToString())));
                //Document pdfDoc = new Document(PageSize.A2, 7f, 7f, 7f, 0f);
                //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                //pdfDoc.Open();
                //htmlparser.Parse(reader2);

                //pdfDoc.NewPage();
                //pdfDoc.Add(new Paragraph(" "));            

                //pdfDoc.Close();
                //Response.Write(pdfDoc);
                //Response.End();

            }
            catch (Exception)
            {

                throw;
            }



            if (System.IO.File.Exists(destinationFile))
            {
                System.IO.File.Delete(destinationFile);
            }

        }
        public void Result(string specimenId, string name)
        {
            _logger.Debug("Result called");
            string textReportPath = ConfigurationManager.AppSettings["LabReportFilePath"].ToString().Trim();
            string[] tmpSpeId = specimenId.Split(',');
            if (tmpSpeId.Count() >= 0)
            {
                specimenId = tmpSpeId[0];
            }
            HL7ParserDao hl7ParserDao = new HL7ParserDao(_logger);

            SurPath.Data.HL7ParserDao dao = new HL7ParserDao(_logger);
            ReportInfo rtInfo = new ReportInfo();
            int rType = dao.DetermineLabReportType(specimenId);
            _logger.Debug($"rType = {rType}");
            ReportType reportType = ReportType.None;


            if (rType == 1)
            {
                reportType = ReportType.LabReport;
                rtInfo = hl7Parser.GetReportDetails(reportType, specimenId, rtInfo);
            }
            if (rType == 3)
            {
                reportType = ReportType.QuestLabReport;
                rtInfo = hl7Parser.GetReportDetails(reportType, specimenId, rtInfo);
            }

            if (rType == 2)
            {
                reportType = ReportType.MROReport;
                rtInfo = hl7Parser.GetReportDetails(reportType, specimenId, rtInfo);
            }


            if (reportType == ReportType.QuestLabReport)
            {
                var fs20loc = rtInfo.LabReport.IndexOf("\\fs20");
                StringBuilder sb = new StringBuilder();
                sb.Append("\n");
                sb.Append("Name: ");
                sb.Append(rtInfo.DonorFirstName);
                sb.Append(" ");
                sb.Append(rtInfo.DonorMI);
                sb.Append(" ");
                sb.Append(rtInfo.DonorLastName);
                sb.Append("\n");
                sb.Append("Date Of Birth: ");
                sb.Append(rtInfo.DonorDOB);
                sb.Append("\n");
                sb.Append("Specimen ID: ");
                sb.Append(rtInfo.SpecimenId);
                sb.Append("\n");
                sb.Append("Primary ID: ");
                sb.Append(rtInfo.SsnId);
                sb.Append("\n");
                sb.Append("Date Of Collection: ");
                sb.Append(rtInfo.SpecimenCollectionDate);
                sb.Append("\n");
                rtInfo.LabReport = rtInfo.LabReport.Insert(fs20loc + 8, sb.ToString());

            }







            rtInfo.LabReport = rtInfo.LabReport.Replace(@"\line", "");
            rtInfo.LabReport = rtInfo.LabReport.Replace(@"{", "");
            rtInfo.LabReport = rtInfo.LabReport.Replace(@"}", "");

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            string Report = rtInfo.LabReport.Remove(0, 174);
            String timeStamp = GetTimestamp(DateTime.Now);
            string folderName = textReportPath;
            string path = textReportPath + name + "_" + timeStamp + ".pdf";

            if (path != null)
            {
                Report = Report.Replace("\r\n", "\n");
                String[] content = Report.Split('\n');

                PrintPDF pdf = new PrintPDF();
                pdf.SavePDF(path, content);

                Response.ContentType = "Application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment; filename = " + name + ".pdf");
                Response.TransmitFile(textReportPath + name + "_" + timeStamp + ".pdf");
                Response.End();

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                string pdfPath = textReportPath + name + "_" + timeStamp + ".pdf";

                if (System.IO.File.Exists(pdfPath))
                {
                    System.IO.File.Delete(pdfPath);
                }
            }

            //MemoryStream ms = new MemoryStream();
            //Document document = new Document(PageSize.A4, 25, 25, 25, 25);

            //Paragraph Paragraph = new Paragraph(rtInfo.LabReport.ToString());

            //PdfWriter writer = PdfWriter.GetInstance(document, ms);

            //document.Open();
            //Paragraph.Alignment = Element.ALIGN_JUSTIFIED_ALL;
            //document.Add(Paragraph);
            //document.Close();
            //writer.Close();
            //ms.Close();
            //Response.ContentType = "pdf/application";
            //Response.AddHeader("content-disposition", "attachment;filename=" + name + ".pdf");
            //Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            //// Response.Write(document);
            //Response.End();

        }

        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }

        public FileStreamResult CreateFile(string report, string Name)
        {
            //ToDo: add some data from your database into that string:
            var Data = report;

            var byteArray = Encoding.ASCII.GetBytes(Data);
            var stream = new MemoryStream(byteArray);

            return File(stream, "text/plain", Name);
        }


    }
}