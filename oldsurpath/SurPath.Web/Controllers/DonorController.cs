using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using SurPath.Business;
using SurPath.Entity;
using SurPath.Enum;
using SurPathWeb.Filters;
using SurPathWeb.Mailers;
using SurPathWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Mvc.Html;
using System.Net;
using Serilog;

namespace SurPathWeb.Controllers
{

    [SessionValidateAttribute]
    public partial class DonorController : Controller
    {
        #region Private Variables

        DonorBL donorBL = new DonorBL();
        ClientBL clientBL = new ClientBL();
        ILogger _logger = MvcApplication._logger;

        #endregion

        [HttpGet]
        [SessionValidateAttribute]
        public ActionResult ProgramSelection()
        {
            Session["FromRegistration"] = "";
            ViewBag.ProgramActive = "active";
            int donorId = Convert.ToInt32(Session["DonorId"]);

            DonorBL donorBL = new DonorBL();
            Donor donor = donorBL.Get(donorId, "Web");

            if (donor != null)
            {
                if (donor.DonorInitialClientId != null)
                {
                    PrepareClientDepartments(Convert.ToInt32(donor.DonorInitialClientId));
                    GetPeymentMethod();
                }
            }

            ViewBag.SuccessMsg = "";

            if (Session["SuccessMessage"] != null)
            {
                ViewBag.SuccessMsg = Session["SuccessMessage"].ToString();
                Session["SuccessMessage"] = "";
            }

            return View();
        }

        [HttpPost]
        [SessionValidateAttribute]
        public ActionResult ProgramSelection(string Program2)
        {
            ViewBag.ProgramActive = "active";
            Session["FromRegistration"] = "";
            int donorId = Convert.ToInt32(Session["DonorId"]);
            int currentUserId = Convert.ToInt32(Session["UserId"]);
            int clientDepartmentId = Convert.ToInt32(UserAuthentication.Decrypt(Program2, true));

            Donor donor = donorBL.Get(donorId, "Web");

            if (donor != null)
            {
                if (donor.DonorInitialClientId != null)
                {
                    PrepareClientDepartments(Convert.ToInt32(donor.DonorInitialClientId));
                }
            }

            ClientBL clientBL = new ClientBL();
            ClientDeptTestCategory testcategory = new ClientDeptTestCategory();
            DataTable dtclient = clientBL.ClientDepartment(clientDepartmentId);

            bool UAtestpanelidflag = true;
            bool Hairtestpanelidflag = true;

            foreach (DataRow dr in dtclient.Rows)
            {
                testcategory.TestCategoryId = (TestCategories)(dr["TestCategoryId"]);

                if (testcategory.TestCategoryId == TestCategories.UA)
                {
                    ClientDeptTestPanel clientDepartment = new ClientDeptTestPanel();

                    clientDepartment.TestPanelId = Convert.ToInt32(dr["TestPanelId"]);
                    if (clientDepartment.TestPanelId == 0)
                    {
                        UAtestpanelidflag = false;
                    }
                    else
                    {
                        UAtestpanelidflag = true;
                    }
                }


                if (testcategory.TestCategoryId == TestCategories.Hair)
                {
                    ClientDeptTestPanel clientDepartment = new ClientDeptTestPanel();

                    clientDepartment.TestPanelId = Convert.ToInt32(dr["TestPanelId"]);
                    if (clientDepartment.TestPanelId == 0)
                    {
                        Hairtestpanelidflag = false;

                    }
                    else
                    {
                        Hairtestpanelidflag = true;
                    }
                }

            }
            if (UAtestpanelidflag == false)
            {
                ViewBag.ServerErr = "You cannot select this department. Because the UA test Panel is not defined for this department.";
                return View();
            }
            if (Hairtestpanelidflag == false)
            {
                ViewBag.ServerErr = "You cannot select this department. Because the Hair test Panel is not defined for this department.";
                return View();
            }

            DonorTestInfo donorTestInfo = donorBL.GetDonorTestInfoByDonorId(donorId);
            if (donorTestInfo.PaymentStatus == PaymentStatus.Paid)
            {

                Donor donorReturn = donorBL.AddTest(donorId, clientDepartmentId, currentUserId);

                string Donortestinfoid = donorReturn.DonorTestInfoId.ToString();
                Session["DonorTestinfoId"] = UserAuthentication.Encrypt(Donortestinfoid.ToString(), true);

                if (donorReturn != null)
                {
                    RegistrationDataModel model = new RegistrationDataModel();

                    model.FirstName = donorReturn.DonorFirstName;
                    model.LastName = donorReturn.DonorLastName;
                    model.Amount = donorReturn.ProgramAmount.ToString("N2");
                    model.DonorEmail = donorReturn.DonorEmail;

                    if (donorReturn.DonorSSN.Length == 11)
                    {
                        model.DonorSSN = "xxx-xx-" + donorReturn.DonorSSN.Substring(7);
                    }

                    model.DonorDOB = donorReturn.DonorDateOfBirth.ToString("MM/dd/yyyy");
                    model.DonorCity = donorReturn.DonorCity;
                    model.DonorState = donorReturn.DonorState;
                    model.DonorZipCode = donorReturn.DonorZip;
                    model.DonorClearStarProfileId = donorReturn.DonorClearStarProfId;

                    //ClientDepartment clientDepartment = clientBL.GetClientDepartment(clientDepartmentId);
                    Client client = clientBL.Get(donorReturn.ClientDepartment.ClientId);

                    model.ClientCode = client.ClientCode;
                    model.ClientName = client.ClientName;
                    model.DepartmentName = donorReturn.ClientDepartment.DepartmentName;
                    //if (!string.IsNullOrEmpty(donorReturn.ClientDepartment.ClientEmail.ToString()))
                    //{
                    //    model.ClientEmail = donorReturn.ClientDepartment.ClientEmail;
                    //}
                    //else
                    //{
                    //    model.ClientEmail = client.ClientEmail;
                    //}

                    model.TPAEmail = ConfigurationManager.AppSettings["TPAProgramToMailAddress"].ToString().Trim();

                    IUserMailer mail = new RegistrationMailer(model);
                    mail.DonorProgramRegsitrationMail().Send();

                    if (!string.IsNullOrEmpty(client.ClientEmail.ToString()))
                    {
                        model.ClientEmail = client.ClientEmail;
                        // mail.ClientProgramRegsitrationMail().Send();
                    }

                    mail.TPAProgramRegsitrationMail().Send();

                    if (donorReturn.ClientDepartment.PaymentTypeId == ClientPaymentTypes.DonorPays)
                    {
                        return RedirectToAction("PaymentSelection");
                    }
                    else
                    {
                        return RedirectToAction("ProgramConfirmation");
                    }
                }
                else
                {
                    return View();
                }
            }
            else
            {
                ViewBag.TestAlreadyExists = "You cannot apply / take another test until existing dues / payments are settled.";
            }
            return View();
        }

        [HttpGet]
        [SessionValidateAttribute]
        public ActionResult ProgramConfirmation()
        {
            ViewBag.ProgramActive = "active";

            return View();
        }

        [HttpGet]
        [SessionValidateAttribute]
        public ActionResult PaymentConfirmationCash()
        {
            GetPeymentMethod();
            return View();
        }

        [HttpGet]
        [SessionValidateAttribute]
        public ActionResult PaymentConfirmationCard()
        {
            string testInfoId = Session["DonorTestinfoId"].ToString();

            RegistrationDataModel model = new RegistrationDataModel();

            string donortestinfoid = UserAuthentication.Decrypt(testInfoId, true);

            DonorTestInfo donortestinfo = donorBL.GetDonorTestInfo(Convert.ToInt32(donortestinfoid));

            if (donortestinfo != null)
            {
                ClientDepartment clientdepartment = clientBL.GetClientDepartment(donortestinfo.ClientDepartmentId);
                Client client = clientBL.Get(donortestinfo.ClientId);

                model.DepartmentName = clientdepartment.DepartmentName;
                model.ClientName = client.ClientName;
                model.Amount = donortestinfo.TotalPaymentAmount.ToString();
            }

            DonorTestInfo donorTestInfo = donorBL.GetDonorTestInfo(Convert.ToInt32(donortestinfoid));
            if (donorTestInfo != null)
            {
                if (makePayment(donorTestInfo, model.Amount, "CARD", model.ClientName))
                {
                    //return Redirect("PaymentConfirmationCard");
                }
                else
                {
                    ViewBag.ServerErr = "Sorry an error occurred while processing your request...";
                }
            }

            GetPeymentMethod();
            return View();
        }

        [HttpGet]
        [SessionValidateAttribute]
        public ActionResult PaymentSelection(string testInfoId)
        {
            if (Session["DonorTestinfoId"].ToString() != "")
            {
                testInfoId = Session["DonorTestinfoId"].ToString();

                RegistrationDataModel model = new RegistrationDataModel();
                model.PaymentData = new PaymentDataModel();

                string donortestinfoid = UserAuthentication.Decrypt(testInfoId, true);

                DonorTestInfo donortestinfo = donorBL.GetDonorTestInfo(Convert.ToInt32(donortestinfoid));

                if (donortestinfo != null)
                {
                    ClientDepartment clientdepartment = clientBL.GetClientDepartment(donortestinfo.ClientDepartmentId);
                    Client client = clientBL.Get(donortestinfo.ClientId);

                    model.DepartmentName = clientdepartment.DepartmentName;
                    model.ClientName = client.ClientName;
                    model.Amount = donortestinfo.TotalPaymentAmount.ToString();
                    model.PaymentData.Amount = donortestinfo.TotalPaymentAmount.ToString();
                    model.PaymentData.Description = client.ClientName;
                    model.PaymentType = PaymentMethod.Card;
                }

                return View(model);
            }
            else
            {
                Session.Abandon();
                return RedirectToAction("Login", "Authentication");
            }
        }

        [HttpPost]
        [SessionValidateAttribute]
        public ActionResult PaymentSelection(RegistrationDataModel regDataModel, string testInfoId, string paymentType)
        {
            testInfoId = Session["DonorTestinfoId"].ToString();

            //RegistrationDataModel model = new RegistrationDataModel();
            ViewBag.PayType = paymentType;


            string donortestinfoid = UserAuthentication.Decrypt(testInfoId, true);

            DonorTestInfo donorTestInfo = donorBL.GetDonorTestInfo(Convert.ToInt32(donortestinfoid));

            //if (donorTestInfo != null)
            //{
            //    ClientDepartment clientdepartment = clientBL.GetClientDepartment(donorTestInfo.ClientDepartmentId);
            //    Client client = clientBL.Get(donorTestInfo.ClientId);

            //    regDataModel.DepartmentName = clientdepartment.DepartmentName;
            //    regDataModel.ClientName = client.ClientName;
            //    regDataModel.Amount = donorTestInfo.TotalPaymentAmount.ToString();
            //}

            if (paymentType.ToUpper() == "CARD")
            {
                string authReturn = AuthorizePayment(regDataModel);
                if (authReturn != "1")
                {
                    ViewBag.AuthError = authReturn;
                    ViewBag.Timeout = "1";
                    return View(regDataModel);
                }
            }
            if (donorTestInfo != null)
            {
                if (makePayment(donorTestInfo, regDataModel.Amount, paymentType, regDataModel.ClientName))
                {
                    regDataModel.FirstName = Session["UserName"].ToString();
                    regDataModel.Amount = donorTestInfo.TotalPaymentAmount.ToString();
                    regDataModel.DonorEmail = Session["UserLoginName"].ToString();

                    IUserMailer mail = new RegistrationMailer(regDataModel);
                    // mail.PaymentConformationMail().Send();
                    if (paymentType.ToUpper() == "CARD")
                    {
                        mail.CardPaymentConformationMail().Send();
                        return Redirect("PaymentConfirmationCard");
                    }
                    else
                    {
                        mail.PaymentConformationMail().Send();
                        return Redirect("PaymentConfirmationCash");
                    }
                }
                else
                {
                    ViewBag.ServerErr = "Sorry an error occurred while processing your request...";
                }
            }

            return View(regDataModel);
        }

        [HttpGet]
        [SessionValidateAttribute]
        public ActionResult DocumentUpload()
        {
            try
            {
                ViewBag.DocumentActive = "active";
                int donorId = Convert.ToInt32(Session["DonorId"]);
                var donorinfo = donorBL.Get(donorId, "Web");
                var donortestinfo = donorBL.GetDonorTestInfoByDonorId(donorId);
                var ClientDept = donorinfo.ClientDepartment;
                //var docs = clientBL.GetClientDepartmentDocTypes(164);
                var docs = clientBL.GetClientDepartmentDocTypes(donortestinfo.ClientDepartmentId);
                ViewBag.DonorName = donorinfo.DonorFirstName + " " + donorinfo.DonorLastName;
                ViewBag.ClientDeptDocs = docs.AsEnumerable();


                List<DonorDocument> documentlist = new List<DonorDocument>();
                List<DonorProfileDataModel> document = new List<DonorProfileDataModel>();
                var items = donorBL.GetDonorDocumentList(donorId);
                foreach (DonorDocument dd in items)
                {
                    var documentdata = new DonorProfileDataModel
                    {
                        DocumentID = dd.DonorDocumentId.ToString(),
                        DocumentTitle = dd.DocumentTitle.ToString().Trim(';'),
                        uploaddate = Convert.ToDateTime(dd.DocumentUploadTime).ToString("MM/dd/yyyy"),
                        uploadtime = dd.DocumentUploadTime.TimeOfDay,
                        Rejected = dd.IsRejected,
                        Approved = dd.IsApproved
                    };
                    document.Add(documentdata);
                }
                GetPeymentMethod();
                return View(document);

            }
            catch (Exception ex)
            {
                MvcApplication.LogError(ex);
                return RedirectToAction("Login", "Authentication");

            }


        }

        [HttpGet]
        [SessionValidateAttribute]
        public ActionResult DocumentManage(int DonorId =0, int ClientId = 0)
        {

            try
            {
                if (DonorId==0 || ClientId==0)
                {
                    return RedirectToAction("Login", "Authentication");
                }

                ViewBag.DocumentActive = "active";
                int donorId = DonorId;// Convert.ToInt32(Session["DonorId"]);
                TempData["DonorId"] = DonorId;
                TempData["ClientDepartment"] = ClientId;
                List<DonorDocument> documentlist = new List<DonorDocument>();
                List<DonorProfileDataModel> document = new List<DonorProfileDataModel>();
                var donorinfo = donorBL.Get(donorId, "Web");
                var donortestinfo = donorBL.GetDonorTestInfoByDonorId(donorId);
                //var ClientDept = donorinfo.ClientDepartment.ClientDepartmentId;
                var items = donorBL.GetDonorDocumentList(donorId);
                var docs = clientBL.GetClientDepartmentDocTypes(donortestinfo.ClientDepartmentId);
                ViewBag.DonorName = donorinfo.DonorFirstName + " " + donorinfo.DonorLastName;
                ViewBag.ClientDeptDocs = docs.AsEnumerable();
                foreach (DonorDocument dd in items)
                {
                    var documentdata = new DonorProfileDataModel
                    {
                        DocumentID = dd.DonorDocumentId.ToString(),
                        DocumentTitle = dd.DocumentTitle.ToString().Trim(';'),
                        uploaddate = Convert.ToDateTime(dd.DocumentUploadTime).ToString("MM/dd/yyyy"),
                        uploadtime = dd.DocumentUploadTime.TimeOfDay,
                        NeedsApproval = dd.IsNeedsApproval,
                        Rejected = dd.IsRejected,
                        Approved = dd.IsApproved



                    };
                    document.Add(documentdata);
                }
                GetPeymentMethod();
                return View(document);
            }
            catch (Exception ex)
            {
                MvcApplication.LogError(ex);
                // throw;
                return RedirectToAction("Login", "Authentication");
            }
        }

        [HttpPost]
        [SessionValidateAttribute]
        public ActionResult DocumentUpload(string[] documentType, IEnumerable<HttpPostedFileBase> fileName)
        {
            _logger.Debug("DocumentUpload called");
            List<DonorDocument> documentlist = new List<DonorDocument>();
            _logger.Debug($"documentlist length (should be zero at this point): {documentlist.Count}");
            List<DonorProfileDataModel> document = new List<DonorProfileDataModel>();
            try
            {
                ViewBag.DocumentActive = "active";
                int donorId = Convert.ToInt32(Session["DonorId"]);
                _logger.Debug($"DocumentUpload donorId: {donorId}");
                Donor donor = donorBL.Get(donorId, "Web");
                _logger.Debug($"DocumentUpload got donor");
                var donorinfo = donorBL.GetDonorTestInfoByDonorId(donorId);
                _logger.Debug($"DocumentUpload got test info");
                var donortestinfo = donorBL.GetDonorTestInfoByDonorId(donorId);
                _logger.Debug($"DocumentUpload got test info (again)");

                var ClientDept = donorinfo.ClientDepartmentId;
                _logger.Debug($"DocumentUpload got ClientDept");
                //var docs = clientBL.GetClientDepartmentDocTypes(164);
                var docs = clientBL.GetClientDepartmentDocTypes(donortestinfo.ClientDepartmentId);
                _logger.Debug($"DocumentUpload GetClientDepartmentDocTypes called");

                ViewBag.DonorName = donorinfo.DonorFirstName + " " + donorinfo.DonorLastName;
                ViewBag.ClientDeptDocs = docs.AsEnumerable();

                byte[] fileContent = new byte[] { };

                var n = documentType.Length;
                _logger.Debug($"DocumentUpload documentType.Length {n.ToString()}");

                var i = 0;
                _logger.Debug($"Donor {donorId} {donor.DonorLastName},{donor.DonorFirstName}");
                foreach (HttpPostedFileBase file in fileName)
                {
                    try
                    {
                        if (!(string.IsNullOrEmpty(file.FileName)))
                        {
                            _logger.Debug($"uploaded file: FileName: {file.FileName.ToString()}");
                        }
                        else
                        {
                            _logger.Debug($"Filename is empty");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex.Message);
                    }


                    _logger.Debug($"Testing {file.FileName}");
                    var supportedTypes = new[] { "jpg", "jpeg", "png", "gif", "txt", "ppt", "doc", "docx", "xls", "xlsx", "ods", "pdf" };
                    var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                    //if (!supportedTypes.Any(s=>s.Equals(fileExt,StringComparison.InvariantCultureIgnoreCase)))
                    //{
                    //    fileExt = fileExt + string.Empty;
                    //    _logger.Debug($"Unsupported file type {fileExt}");
                    //    return View();
                    //}
                    if (!supportedTypes.Contains(fileExt, StringComparer.InvariantCultureIgnoreCase))
                    {
                        fileExt = fileExt + string.Empty;
                        _logger.Debug($"Unsupported file type {fileExt}");
                        return View();
                    }

                    if (i < n)
                    {
                        using (var stream = new BinaryReader(file.InputStream))
                        {
                            fileContent = stream.ReadBytes(file.ContentLength);
                        }

                        DonorDocument donordocument = new DonorDocument();

                        donordocument.DonorId = donorId;
                        donordocument.DocumentTitle = documentType[i].Trim(';');
                        donordocument.FileName = file.FileName;
                        donordocument.DocumentContent = fileContent;
                        donordocument.UploadedBy = donor.DonorEmail;

                        i++;

                        var returnvalue = donorBL.UploadDonorDocument(donordocument);
                        if (returnvalue > 0)
                        {
                            donordocument.DonorDocumentId = returnvalue;
                        }
                        _logger.Debug("File uploaded");

                    }
                }

                var items = donorBL.GetDonorDocumentList(donorId);
                foreach (DonorDocument dd in items)
                {
                    var documentdata = new DonorProfileDataModel
                    {
                        DocumentID = dd.DonorDocumentId.ToString(),
                        DocumentTitle = dd.DocumentTitle.ToString().Trim(';'),
                        uploaddate = Convert.ToDateTime(dd.DocumentUploadTime).ToString("MM/dd/yyyy"),
                        uploadtime = dd.DocumentUploadTime.TimeOfDay
                    };
                    document.Add(documentdata);
                }


                ViewBag.SuccessMsg = "File(s) has been uploaded successfully.";
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                ViewBag.ServerErr = ex.Message;
            }
            return View(document);
        }

        [HttpPost]
        [SessionValidateAttribute]
        public ActionResult DocumentManageUpload(string[] documentType, IEnumerable<HttpPostedFileBase> fileName)
        {
            int donorId = 0;
            int clientdept = 0;
            List<DonorDocument> documentlist = new List<DonorDocument>();
            List<DonorProfileDataModel> document = new List<DonorProfileDataModel>();
            try
            {
                //ViewBag.DocumentActive = "active";
                //int donorId = 74974; // Convert.ToInt32(Session["DonorId"]);

                donorId = Convert.ToInt32(TempData["DonorId"]); // Convert.ToInt32(Session["DonorId"]);
                Donor donor = donorBL.Get(donorId, "Web");
                clientdept = Convert.ToInt32(TempData["ClientDepartment"]);
                byte[] fileContent = new byte[] { };

                var n = documentType.Length;
                var i = 0;

                foreach (var file in fileName)
                {
                    var supportedTypes = new[] { "jpg", "jpeg", "png", "gif", "txt", "ppt", "doc", "docx", "xls", "xlsx", "ods", "pdf" };
                    var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                    if (!supportedTypes.Contains(fileExt))
                    {
                        return View();
                    }

                    if (i < n)
                    {
                        using (var stream = new BinaryReader(file.InputStream))
                        {
                            fileContent = stream.ReadBytes(file.ContentLength);
                        }

                        DonorDocument donordocument = new DonorDocument();

                        donordocument.DonorId = donorId;
                        donordocument.DocumentTitle = documentType[i].Trim(';');
                        donordocument.FileName = file.FileName;
                        donordocument.DocumentContent = fileContent;
                        donordocument.UploadedBy = donor.DonorEmail;


                        i++;

                        var returnvalue = donorBL.UploadDonorDocument(donordocument);
                        if (returnvalue > 0)
                        {
                            donordocument.DonorDocumentId = returnvalue;
                        }
                    }
                }

                var items = donorBL.GetDonorDocumentList(donorId);
                foreach (DonorDocument dd in items)
                {
                    var documentdata = new DonorProfileDataModel
                    {
                        DocumentID = dd.DonorDocumentId.ToString(),
                        DocumentTitle = dd.DocumentTitle.ToString().Trim(';'),
                        uploaddate = Convert.ToDateTime(dd.DocumentUploadTime).ToString("MM/dd/yyyy"),
                        uploadtime = dd.DocumentUploadTime.TimeOfDay
                    };
                    document.Add(documentdata);
                }

                ViewBag.SuccessMsg = "File(s) has been uploaded successfully.";

            }
            catch (Exception ex)
            {
                ViewBag.ServerErr = ex.Message;
            }


            return RedirectToAction("DocumentManage", "Donor", new { donorId = donorId, clientId = clientdept });
            //return View("DocumentManage",);
        }

        [Route("DocumentReject/{docID}", Name = "DocumentReject")]
        [SessionValidateAttribute]
        public ActionResult DocumentReject(int docID)
        {
            DonorDocument donorDocument = donorBL.GetDonorDocument(Convert.ToInt32(docID));
            DonorProfileDataModel document = new DonorProfileDataModel();
            var donorId = Convert.ToInt32(TempData["DonorId"]);
            var donorinfo = donorBL.Get(donorId, "Web");
            var donortestinfo = donorBL.GetDonorTestInfoByDonorId(donorId);
            var clientdept = Convert.ToInt32(TempData["ClientDepartment"]);
            var docs = clientBL.GetClientDepartmentDocTypes(donortestinfo.ClientDepartmentId);
            ViewBag.DonorName = donorinfo.DonorFirstName + " " + donorinfo.DonorLastName;
            ViewBag.ClientDeptDocs = docs.AsEnumerable();
            //var clientid = 5
            if (donorDocument != null)
            {
                document.filename = donorDocument.FileName;
                document.documentContent = donorDocument.DocumentContent;
                document.DocumentID = donorDocument.DonorDocumentId.ToString();
                document.DocumentTitle = donorDocument.DocumentTitle.Trim(';');
            }

            var returnvalue = donorBL.DonorDocumentReject(docID);
            //donorBL.DonorSendSMS(donorId, "Your document has been rejected." + donorDocument.DocumentTitle).Wait();
            return RedirectToAction("DocumentManage", "Donor", new { donorId = donorId, clientId = clientdept });

        }

        [Route("DocumentApprove/{docID}", Name = "DocumentApprove")]
        //[HttpPost] // Matches '/Products/Edit/{id}'
        [SessionValidateAttribute]
        public ActionResult DocumentApprove(int docID)
        {
            
            DonorDocument donorDocument = donorBL.GetDonorDocument(Convert.ToInt32(docID));
            DonorProfileDataModel document = new DonorProfileDataModel();
            var donorId = Convert.ToInt32(TempData["DonorId"]);
            var clientdept = Convert.ToInt32(TempData["ClientDepartment"]);

            if (donorDocument != null)
            {
                document.filename = donorDocument.FileName;
                document.documentContent = donorDocument.DocumentContent;
                document.DocumentID = donorDocument.DonorDocumentId.ToString();
                document.DocumentTitle = donorDocument.DocumentTitle.Trim(';');
            }
            //return View();


            var returnvalue = donorBL.DonorDocumentApprove(docID);
            donorBL.DonorSendSMS(donorId, "Your document has been approved." + donorDocument.DocumentTitle);
            return RedirectToAction("DocumentManage", "Donor", new { donorId = donorId, clientId = clientdept });


        }


        [HttpGet]
        [SessionValidateAttribute]
        public ActionResult ViewDocument(int documentID)
        {
            _logger.Debug($"ViewDocument {documentID}");

            DonorDocument donorDocument = donorBL.GetDonorDocument(Convert.ToInt32(documentID), false);
            DonorProfileDataModel document = new DonorProfileDataModel();

            if (donorDocument != null)
            {
                document.filename = donorDocument.FileName;
                document.documentContent = donorDocument.DocumentContent;
                document.DocumentID = donorDocument.DonorDocumentId.ToString();
                //donorDocument.DocumentTitle.Trim(';');
                string _title = donorDocument.DocumentTitle.Trim(';');
                _title = _title.Replace(", ", "--");
                _title = _title.Replace(',', '-');
                _title = _title.Replace(' ', '_');


                document.DocumentTitle = _title; 
            }

            var strType = System.IO.Path.GetExtension(document.filename).Substring(1);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            //string _DocumentTitle = document.DocumentTitle;
            //_DocumentTitle = _DocumentTitle.Replace(" ", "_");
            //_DocumentTitle = _DocumentTitle.Replace(",", "_");
            //string _attachment = $"attachment; filename=\"{ document.DocumentID }_{_DocumentTitle}.{strType}\"";
            //MvcApplication._logger.Debug($"Sending file: {_attachment}");
            
            //Response.AddHeader("content-disposition", "attachment; filename=" + _attachment);

            Response.AddHeader("content-disposition", "attachment; filename=" + document.DocumentID + "_" + document.DocumentTitle + "." + strType);

            Response.ContentType = strType;
            this.Response.BinaryWrite(document.documentContent);
            this.Response.End();

            return View();
        }

        [HttpGet]
        [SessionValidateAttribute]
        public ActionResult _TestResultView(string firstname, string lastname)
        {
            string name = firstname + " " + lastname;
            GridView gridvw = new GridView();
            List<TestResultDataModel> download = new List<TestResultDataModel>();
            var items = download;

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + name + ".pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gridvw.AllowPaging = false;
            gridvw.DataSource = items.ToList().Take(items.Count);
            gridvw.DataBind();
            gridvw.RenderControl(hw);

            StringReader sr = new StringReader(sw.ToString());
            StreamReader reader = new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(sw.ToString())));
            Document pdfDoc = new Document(PageSize.A2, 7f, 7f, 7f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(reader);

            pdfDoc.NewPage();
            pdfDoc.Add(new Paragraph(" "));

            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();

            return View();
        }

        #region Private Methods

        private void PrepareClientDepartments(int clientId)
        {
            var clientDepartments = clientBL.GetClientDepartmentList(clientId);
            var clientDepartmentsShuffled = clientBL.GetClientDepartmentList(clientId);
            clientDepartmentsShuffled.Shuffle();

            if (clientDepartments != null)
            {
                List<SelectListItem> departments = new List<SelectListItem>();
                List<SelectListItem> departmentsShuffled = new List<SelectListItem>();

                departments.Add(new SelectListItem { Text = "Select Program", Value = "0", Selected = true });
                departmentsShuffled.Add(new SelectListItem { Text = "Select Program", Value = "0", Selected = true });


                foreach (var item in clientDepartments)
                {
                    departments.Add(new SelectListItem
                    {
                        Text = item.DepartmentName,
                        Value = UserAuthentication.Encrypt(item.ClientDepartmentId.ToString(), true)
                    });
                }

                foreach (var item in clientDepartmentsShuffled)
                {
                    departmentsShuffled.Add(new SelectListItem
                    {
                        Text = item.DepartmentName,
                        Value = UserAuthentication.Encrypt(item.ClientDepartmentId.ToString(), true)
                    });
                }

                ViewBag.Department = departments;
                ViewBag.ShuffledDepartment = departmentsShuffled;
            }
        }

        private bool makePayment(DonorTestInfo donorTestInfo, string amt, string mode, string description)
        {
            if (donorTestInfo.PaymentStatus == PaymentStatus.None || donorTestInfo.PaymentStatus == PaymentStatus.Pending)
            {
                if (amt.Trim() != string.Empty && Convert.ToDouble(amt.Trim()) > 0)
                {
                    donorTestInfo.PaymentDate = DateTime.Now;
                    if (mode.ToUpper() == "CASH")
                    {
                        donorTestInfo.PaymentMethodId = PaymentMethod.Cash;
                    }
                    else if (mode.ToUpper() == "CARD")
                    {
                        donorTestInfo.PaymentMethodId = PaymentMethod.Card;
                    }
                    else
                    {
                        donorTestInfo.PaymentMethodId = PaymentMethod.None;
                    }
                    donorTestInfo.PaymentNote = "";// description.Trim();
                    donorTestInfo.LastModifiedBy = Session["UserLoginName"].ToString();

                    donorBL.SavePaymentDetails(donorTestInfo, "Yes");


                }
            }

            return true;
        }

        private string AuthorizePayment(RegistrationDataModel regDataModel)
        {
            string AuthNetVersion = "3.1"; // Contains CCV support
            string AuthNetLoginID = System.Configuration.ConfigurationManager.AppSettings["Payment.AuthLoginID"].ToString();
            string AuthNetTransKey = System.Configuration.ConfigurationManager.AppSettings["Payment.AuthTransactionKey"].ToString();
            string AuthNetMode = System.Configuration.ConfigurationManager.AppSettings["Payment.TestMode"].ToString();

            WebClient objRequest = new WebClient();
            System.Collections.Specialized.NameValueCollection objInf = new System.Collections.Specialized.NameValueCollection(30);
            System.Collections.Specialized.NameValueCollection objRetInf = new System.Collections.Specialized.NameValueCollection(30);
            byte[] objRetBytes;
            string[] objRetVals;
            string retMessage;

            objInf.Add("x_version", AuthNetVersion);
            objInf.Add("x_delim_data", "True");
            objInf.Add("x_login", AuthNetLoginID);
            // objInf.Add("x_password", AuthNetPassword);
            objInf.Add("x_tran_key", AuthNetTransKey);
            objInf.Add("x_relay_response", "False");

            // Switch this to False once you go live
            objInf.Add("x_test_request", AuthNetMode);

            objInf.Add("x_delim_char", ",");
            objInf.Add("x_encap_char", "|");

            // Billing Address
            objInf.Add("x_first_name", regDataModel.PaymentData.FirstName);
            objInf.Add("x_last_name", regDataModel.PaymentData.LastName);
            objInf.Add("x_address", regDataModel.PaymentData.Address);
            objInf.Add("x_city", regDataModel.PaymentData.City);
            objInf.Add("x_state", regDataModel.PaymentData.State);
            objInf.Add("x_zip", regDataModel.PaymentData.ZIP);
            objInf.Add("x_country", regDataModel.PaymentData.Country);
            objInf.Add("x_email", regDataModel.PaymentData.Email);
            objInf.Add("x_fax", regDataModel.PaymentData.Fax);
            objInf.Add("x_phone", regDataModel.PaymentData.Phone);

            objInf.Add("x_description", regDataModel.PaymentData.Description);

            // Card Details
            objInf.Add("x_card_num", regDataModel.PaymentData.CardNumber);
            objInf.Add("x_exp_date", regDataModel.PaymentData.CardExpiryDate);

            // Authorisation code of the card (CCV)
            objInf.Add("x_card_code", regDataModel.PaymentData.CCV);

            objInf.Add("x_method", "CC");
            objInf.Add("x_type", "AUTH_CAPTURE");
            objInf.Add("x_amount", regDataModel.PaymentData.Amount);

            // Currency setting. Check the guide for other supported currencies
            objInf.Add("x_currency_code", "USD");

            try
            {
                // Pure Test Server
                objRequest.BaseAddress = System.Configuration.ConfigurationManager.AppSettings["Payment.URL"].ToString().Trim();

                objRetBytes = objRequest.UploadValues(objRequest.BaseAddress, "POST", objInf);
                objRetVals = System.Text.Encoding.ASCII.GetString(objRetBytes).Split(",".ToCharArray());

                if (objRetVals[0].Trim(char.Parse("|")) == "1")
                {
                    // Returned Authorisation Code
                    ViewBag.AuthNetCode = objRetVals[4].Trim(char.Parse("|"));
                    // Returned Transaction ID
                    ViewBag.AuthNetTransID = objRetVals[6].Trim(char.Parse("|"));
                    return retMessage = "1";
                }
                else
                {
                    // Error!
                    retMessage = objRetVals[3].Trim(char.Parse("|")) + " (" + objRetVals[2].Trim(char.Parse("|")) + ")";

                    if (objRetVals[2].Trim(char.Parse("|")) == "44")
                    {
                        // CCV transaction decline
                        retMessage += "Our Card Code Verification (CCV) returned the following error: ";

                        switch (objRetVals[38].Trim(char.Parse("|")))
                        {
                            case "N":
                                retMessage += "Card Code does not match.";
                                break;
                            case "P":
                                retMessage += "Card Code was not processed.";
                                break;
                            case "S":
                                retMessage += "Card Code should be on card but was not indicated.";
                                break;
                            case "U":
                                retMessage += "Issuer was not certified for Card Code.";
                                break;
                        }
                    }

                    if (objRetVals[2].Trim(char.Parse("|")) == "45")
                    {
                        if (retMessage.Length > 1)
                            retMessage += "<br />n";

                        // AVS transaction decline
                        retMessage += "Our Address Verification System (AVS) returned the following error: ";

                        switch (objRetVals[5].Trim(char.Parse("|")))
                        {
                            case "A":
                                retMessage += " the zip code entered does not match the billing address.";
                                break;
                            case "B":
                                retMessage += " no information was provided for the AVS check.";
                                break;
                            case "E":
                                retMessage += " a general error occurred in the AVS system.";
                                break;
                            case "G":
                                retMessage += " the credit card was issued by a non-US bank.";
                                break;
                            case "N":
                                retMessage += " neither the entered street address nor zip code matches the billing address.";
                                break;
                            case "P":
                                retMessage += " AVS is not applicable for this transaction.";
                                break;
                            case "R":
                                retMessage += " please retry the transaction; the AVS system was unavailable or timed out.";
                                break;
                            case "S":
                                retMessage += " the AVS service is not supported by your credit card issuer.";
                                break;
                            case "U":
                                retMessage += " address information is unavailable for the credit card.";
                                break;
                            case "W":
                                retMessage += " the 9 digit zip code matches, but the street address does not.";
                                break;
                            case "Z":
                                retMessage += " the zip code matches, but the address does not.";
                                break;
                        }
                    }

                    // strError contains the actual error
                    ViewBag.ErrorMsg = retMessage;
                    return retMessage;
                }
            }
            catch (Exception ex)
            {
                return retMessage = ex.Message;
            }
        }

        private void GetPeymentMethod()
        {
            DonorTestInfo donorTestInfo = donorBL.GetDonorTestInfoByDonorId(Convert.ToInt32(Session["DonorId"]));
            //if (donorTestInfo.PaymentMethodId != PaymentMethod.None)
            if (donorTestInfo.PaymentStatus == PaymentStatus.Paid)
            {
                Session["DonorTestinfoId"] = "";
                Session["isPaymentStatus"] = "Yes";
            }
            else
            {
                Session["DonorTestinfoId"] = UserAuthentication.Encrypt(donorTestInfo.DonorTestInfoId.ToString(), true);
                Session["isPaymentStatus"] = "No";
            }
        }

        #endregion
    }
}