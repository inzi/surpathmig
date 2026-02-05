using SurPath.Business;
using SurPath.Business.Master;
using SurPath.Enum;
using SurPathWeb.Filters;
using SurPathWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using SurPath.Entity;

namespace SurPathWeb.Controllers
{
    public class AttorneyController : Controller
    {
        [HttpGet]
        [SessionValidateAttribute]
        public ActionResult TestResult()
        {
            ViewBag.TestResultActive = "active";

            Dictionary<string, string> searchParam = new Dictionary<string, string>();

            searchParam.Add("AttorneyId", Session["AttorneyId"].ToString());

            DonorBL donorBL = new DonorBL();
            ReportType reportType = ReportType.MROReport;
            ReportType reportTypeLab = ReportType.LabReport;
            ReportInfo reportsLab = null;
            ReportInfo reporLabType = null;
            DataTable dtDonors = donorBL.GetDonorsByAttorney(Convert.ToInt32(Session["AttorneyId"]));

            List<TestResultDataModel> testResultList = new List<TestResultDataModel>();

            foreach (DataRow dr in dtDonors.Rows)
            {
                TestResultDataModel testResult = new TestResultDataModel();
                testResult.ClientId = dr["TestInfoClientId"].ToString();
                testResult.DonorTestInfoId = UserAuthentication.Encrypt(dr["DonorTestInfoId"].ToString(), true);
                if (!string.IsNullOrEmpty(dr["DonorFirstName"].ToString()))
                {
                    testResult.FirstName = dr["DonorFirstName"].ToString();
                }
                else
                {
                    testResult.FirstName = "";
                }

                if (!string.IsNullOrEmpty(dr["DonorLastName"].ToString()))
                {
                    testResult.LastName = dr["DonorLastName"].ToString();
                }
                else
                {
                    testResult.LastName = "";
                }

                if (!string.IsNullOrEmpty(dr["DonorSSN"].ToString()))
                {
                    testResult.SSN = dr["DonorSSN"].ToString();
                    if (testResult.SSN.Length == 11)
                    {
                        testResult.SSN = "XXX-XX-" + testResult.SSN.Substring(7);
                    }
                    else
                    {
                        testResult.SSN = "";
                    }
                }
                else
                {
                    testResult.SSN = "";
                }

                if (!string.IsNullOrEmpty(dr["DonorDateOfBirth"].ToString()))
                {
                    testResult.DOB = Convert.ToDateTime(dr["DonorDateOfBirth"]).ToString("MM/dd/yyyy");
                }
                else
                {
                    testResult.DOB = "";
                }

                DonorRegistrationStatus Status = (DonorRegistrationStatus)(int)(dr["TestStatus"]);
                testResult.TestStatus = Status.ToDescriptionString();
                //testResult.TestDate = dr["SpecimenDate"].ToString();

                if (dr["SpecimenDate"] != DBNull.Value)
                {
                    if (Convert.ToDateTime(dr["SpecimenDate"]) != DateTime.MinValue)
                    {
                        testResult.TestDate = dr["SpecimenDate"].ToString();
                    }
                }

                if (dr["TestOverallResult"].ToString() != string.Empty)
                {
                    OverAllTestResult result = (OverAllTestResult)(int)(dr["TestOverallResult"]);
                    if (result.ToString() != "None")
                    {
                        testResult.TestOverallResult = result.ToString();
                    }
                    else
                    {
                        testResult.TestOverallResult = "";
                    }
                }
                else
                {
                    testResult.TestOverallResult = string.Empty;
                }

                if (testResult.TestStatus.ToUpper() == "COMPLETED" && dr["IsDonorRefused"].ToString() != "1" || (testResult.TestStatus.ToUpper() == "PROCESSING"))
                {

                    int MROValue = 0;
                    string MROType = dr["MROTypeId"].ToString();

                    if ((MROType.ToString() == "2") || (MROType.ToString() == "1" || (MROType.ToString() == "3")))// && testResult.TestOverallResult.ToString() == "Positive"))
                    {
                        int testInfoId = Convert.ToInt32(dr["DonorTestInfoId"].ToString());
                        ReportInfo reports = null;
                        reportsLab = donorBL.GetLabReport(testInfoId, reportTypeLab);
                        reports = donorBL.GetMROReport(testInfoId, reportType);
                        if (reports != null)
                        {
                            int documentReportId = reports.FinalReportId;

                            if (documentReportId != 0)
                            {
                                DonorDocument donorDocument = donorBL.GetDonorDocument(documentReportId);
                                int donorDocumentId = donorDocument.DonorDocumentId;
                                string fileName = donorDocument.FileName;
                                string documentTitle = donorDocument.DocumentTitle;
                                byte[] content = donorDocument.DocumentContent;

                                testResult.MRODownloadLink = Helper.CreateActionLink(this).ActionLink("MRO Report", "ViewDocument", "Donor",
                                     new
                                     {
                                         documentID = donorDocumentId

                                     }, new { @target = "_blank", @class = "btn btn-primary" }).ToString();
                                testResult.DownloadLink = Helper.CreateActionLink(this).Label("").ToString();

                                if (MROType.ToString() == "2")
                                {
                                    MROValue = 0;
                                }
                                else
                                {
                                    MROValue = 1;
                                }
                            }
                            else
                            {
                                testResult.MRODownloadLink = Helper.CreateActionLink(this).Label("").ToString();
                            }

                        }
                        else
                        {
                            testResult.MRODownloadLink = Helper.CreateActionLink(this).Label("").ToString();
                        }
                    }
                    else
                    {
                        testResult.MRODownloadLink = Helper.CreateActionLink(this).Label("").ToString();
                    }


                    //testResult.DownloadLink = Helper.CreateActionLink(this).ActionLink("Download", "_TestResultView", "Donor",
                    //    new
                    //    {
                    //        clientId = testResult.ClientId,
                    //        donorid = testResult.DonorId,
                    //        donortestinfoid = testResult.DonorTestInfoId,
                    //        firstname = testResult.FirstName,
                    //        lastname = testResult.LastName,
                    //        ExportFormat = "0"
                    //    }, new { @target = "_blank" }).ToString();

                    if (MROValue == 0)
                    {
                        if ((MROType.ToString() == "2") || (MROType.ToString() == "1" && testResult.TestStatus.ToUpper() == "COMPLETED"))// && testResult.TestOverallResult.ToUpper().ToString() != "Positive" && reportsLab != null))
                        {
                            if (dr["IsInstantTest"].ToString() == "1" && dr["InstanTestResult"].ToString() == "2")
                            {
                                testResult.DownloadLink = Helper.CreateActionLink(this).Label("").ToString();
                            }
                            else
                            {
                                int testInfo = Convert.ToInt32(dr["DonorTestInfoId"].ToString());
                                if (dr["SpecimenId"].ToString() != string.Empty)
                                {
                                    testResult.SpecimenId = dr["SpecimenId"].ToString();
                                    if (testResult.SpecimenId.Contains(','))
                                    {
                                        string[] specimen = testResult.SpecimenId.Split(',');
                                        testResult.SpecimenId = specimen[0].Trim().ToString();
                                    }
                                }
                                reporLabType = donorBL.GetLabReportType(testInfo, testResult.SpecimenId, reportType);
                                if (reporLabType == null)
                                {
                                    if (reportsLab != null)
                                    {
                                        if (MROType.ToString() == "1" && testResult.TestStatus.ToUpper() == "COMPLETED" && testResult.TestOverallResult.ToUpper().ToString() == "NEGATIVE" && reportsLab.ReportType.ToString().ToUpper() == "LABREPORT")
                                        {
                                            testResult.DownloadLink = Helper.CreateActionLink(this).ActionLink("Lab Report", "Result", "Client",
                                               new
                                               {
                                                   specimenId = testResult.SpecimenId,
                                                   name = testResult.FirstName + "  " + testResult.LastName
                                               }, new { @target = "_blank", @class = "btn btn-primary" }).ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    testResult.DownloadLink = Helper.CreateActionLink(this).Label("").ToString();
                                }

                                if (MROType.ToString() == "2")
                                {
                                    testResult.DownloadLink = Helper.CreateActionLink(this).ActionLink("Lab Report", "Result", "Client",
                                            new
                                            {
                                                specimenId = testResult.SpecimenId,
                                                name = testResult.FirstName + "  " + testResult.LastName
                                            }, new { @target = "_blank", @class = "btn btn-primary" }).ToString();
                                }
                             
                            }
                        }
                        else
                        {
                            testResult.DownloadLink = Helper.CreateActionLink(this).Label("").ToString();
                        }
                    }
                }
                else
                {
                    testResult.DownloadLink = Helper.CreateActionLink(this).Label("").ToString();
                    testResult.MRODownloadLink = Helper.CreateActionLink(this).Label("").ToString();
                }
                testResultList.Add(testResult);
            }

            var items = testResultList;

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
        public ActionResult UserProfile()
        {
            try
            {
                AttorneyBL at = new AttorneyBL();
                UserBL userbl = new UserBL();
                int userid = Convert.ToInt32(Session["UserId"].ToString());

                var AttorneyData = userbl.Get(userid);

                if (AttorneyData != null)
                {
                    var model = new DonorProfileDataModel
                    {
                        Username = AttorneyData.Username,
                        EmailID = AttorneyData.UserEmail,
                        FirstName = AttorneyData.UserFirstName,
                        LastName = AttorneyData.UserLastName,
                        Phone1 = AttorneyData.UserPhoneNumber,
                        Fax = AttorneyData.UserFax
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
        public ActionResult UserProfile(DonorProfileDataModel model)
        {
            UserBL userbl = new UserBL();
            int userid = Convert.ToInt32(Session["UserId"].ToString());

            User user = userbl.GetByUsernameOrEmail(model.EmailID.Trim());

            if (user == null || user.UserId == userid)
            {
                var AttorneyData = userbl.Get(userid);

                if (AttorneyData != null)
                {
                    //AttorneyData.Username = model.Username;
                    AttorneyData.UserEmail = model.EmailID;
                    AttorneyData.UserFirstName = model.FirstName;
                    AttorneyData.UserLastName = model.LastName;
                    AttorneyData.UserPhoneNumber = model.Phone1;
                    AttorneyData.UserFax = model.Fax;

                    int status = userbl.Save(AttorneyData);

                    if (status == 1)
                    {
                        Session["UserName"] = AttorneyData.UserFirstName + " " + AttorneyData.UserLastName;
                        Session["SuccessMessage"] = "Your profile updated successfully";
                        return Redirect("TestResult");
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
    }

}