using SurPathWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using CaptchaMvc;
//using CaptchaMvc.HtmlHelpers;
using SurPath.Business;
using SurPath.Entity;
using SurPath.Enum;
using SurPathWeb.Mailers;
using System.Configuration;
using SurPathWeb.Filters;
using System.Security.Cryptography;
using System.Data;
using System.Net;
using Surpath.ClearStar.BL;
using System.Xml;

namespace SurPathWeb.Controllers
{
    public class BackgroundCheckController : Controller
    {
        #region Private Variables

        DonorBL donorBL = new DonorBL();
        ClientBL clientBL = new ClientBL();
        string donorStatus;
        #endregion

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
            //return RedirectToAction("ProfileUpdate2");
            //return RedirectToAction("ProgramSelection");

        }

        [HttpPost]
        public ActionResult Registration(RegistrationDataModel model)
        {
            Donor donor = new Donor();
            //if (this.IsCaptchaValid("The code you entered is not valid."))
            //{


            donor.DonorFirstName = model.FirstName;
            donor.DonorLastName = model.LastName;
            donor.DonorEmail = model.EmailID;

            donor.DonorRegistrationStatusValue = DonorRegistrationStatus.PreRegistration;
            int status = donorBL.DoDonorPreRegisteration(donor, model.ClientCode);

            if (donor.DonorId > 0)
            {
                //string activationLink = ConfigurationManager.AppSettings["MvcMailer.BaseURL"].ToString().Trim();

                //if (!activationLink.EndsWith("/"))
                //{
                //    activationLink += "/";
                //}

                //activationLink += "Registration/Activation/" + Helper.Base64ForUrlEncode(UserAuthentication.URLIDEncrypt(donor.DonorId.ToString(), true));

                //model.TemporaryPassword = donor.TemporaryPassword;
                //model.ActivationLink = activationLink;

                //IUserMailer mail = new RegistrationMailer(model);
                //mail.PreRegistrationMail().Send();
            }
            //}
            //else
            //{
            //    ViewBag.ServerError = "TRUE";
            //    return View();
            //}

            //return RedirectToAction("Confirmation");
            return RedirectToAction("ProfileUpdate2", donor);

        }

        public ActionResult Confirmation()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CheckClientCode(string clientCode)
        {
            Client client = clientBL.Get(clientCode.Trim());

            if (client != null)
            {
                return Json(new { Success = 1, ErrorMsg = "" });
            }

            return Json(new { Success = -1, ErrorMsg = "Client Code does not exist" });
        }

        [HttpPost]
        public JsonResult CheckDonorEmail(string donorEmail)
        {
            UserBL userBL = new UserBL();

            User user = userBL.GetByUsernameOrEmail(donorEmail.Trim());

            if (user == null)
            {
                return Json(new { Success = 1 });
            }
            return Json(new { Success = 1 });
            //return Json(new { Success = -1, ErrorMsg = "Email address already exist" });
        }

        [HttpGet]
        public ActionResult Activation(string id)
        {
            ViewBag.DonorID = id;

            try
            {
                int donorID = Convert.ToInt32(UserAuthentication.URLIDDecrypt(Helper.Base64ForUrlDecode(id.ToString()), true));

                var donorData = donorBL.Get(donorID, "Web");

                if (!(donorData != null && donorData.DonorRegistrationStatusValue == DonorRegistrationStatus.PreRegistration))
                {
                    return RedirectToAction("Login", "Authentication");
                }
            }
            catch
            {
                ViewBag.ServerErr = "Unable to process your request.";
            }
            return View();
        }

        [HttpPost]
        public ActionResult Activation(string id, string oldPassword, string newPassword)
        {
            try
            {
                ViewBag.DonorID = id;

                int donorID = Convert.ToInt32(UserAuthentication.URLIDDecrypt(Helper.Base64ForUrlDecode(id.ToString()), true));
                Donor donor = new Donor();
                donor = donorBL.Get(donorID, "Web");
                if (donor.DonorRegistrationStatusValue == DonorRegistrationStatus.PreRegistration)
                {
                    donorStatus = "PreRegistration";
                }
                else if (donor.DonorRegistrationStatusValue == DonorRegistrationStatus.Registered)
                {
                    donorStatus = "Registered";
                }
                else if (donor.DonorRegistrationStatusValue == DonorRegistrationStatus.InQueue)
                {
                    donorStatus = "InQueue";
                }
                donorBL.DoDonorInQueueUpdate(donorID, oldPassword, newPassword, donorStatus);
            }
            catch (Exception e)
            {
                string[] error = new string[] { "User does not exist.", "The old password does not match with the database value." };

                if (error.Contains(e.Message))
                {
                    ViewBag.ServerErr = e.Message;
                }
                else
                {
                    ViewBag.ServerErr = "Unable to process your request.";
                }
                return View();
            }
            return RedirectToAction("PasswordResetConfirmation");
        }

        public ActionResult PasswordResetConfirmation()
        {
            return View();
        }

        [HttpGet]
        [SessionValidateAttribute]
        public ActionResult ProfileUpdate()
        {
            try
            {
                int donorID = Convert.ToInt32(Session["DonorId"].ToString());

                var donorData = donorBL.Get(donorID, "Web");

                if (donorData != null)
                {
                    DonorProfileDataModel model = new DonorProfileDataModel();

                    model.EmailID = donorData.DonorEmail;
                    model.SSN = donorData.DonorSSN;
                    model.FirstName = donorData.DonorFirstName;
                    model.MiddleInitial = donorData.DonorMI;
                    model.LastName = donorData.DonorLastName;
                    model.Suffix = donorData.DonorSuffix;
                    if (donorData.DonorDateOfBirth.ToString("MM/dd/yyyy") != "01/01/0001")
                    {
                        model.DonorDOBMonth = donorData.DonorDateOfBirth.Month.ToString();
                        model.DonorDOBDate = donorData.DonorDateOfBirth.Day.ToString();
                        model.DonorDOBYear = donorData.DonorDateOfBirth.Year.ToString();
                    }
                    else
                    {
                        model.DonorDOBDate = "";
                        model.DonorDOBMonth = "";
                        model.DonorDOBYear = "";
                    }
                    model.Address1 = donorData.DonorAddress1;
                    model.Address2 = donorData.DonorAddress2;
                    model.City = donorData.DonorCity;
                    model.State = donorData.DonorState;
                    model.ZipCode = donorData.DonorZip;
                    model.Phone1 = donorData.DonorPhone1;
                    model.Phone2 = donorData.DonorPhone2;
                    model.Gender = donorData.DonorGender;

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                //redirect to 404
            }
            return View();
        }

        [HttpGet]
        //[SessionValidateAttribute]
        public ActionResult ProfileUpdate2(Donor donor)
        {
            //Create a new profile - 
            //Donor donor = new Donor();

            //donor.DonorFirstName = "";
            //donor.DonorLastName = "";
            //donor.DonorEmail = "mike.kearl9@gmail.com";

            //donor.DonorRegistrationStatusValue = DonorRegistrationStatus.Registered;



            //int status = donorBL.DoDonorPreRegisteration(donor, "Legal");//This is the initial ClientID and DepartmentID 
            //Todo: need to talk to david to see if there is a better code to use than "Legal".
            DonorProfileDataModel model = new DonorProfileDataModel();
            try
            {
                //int donorID = Convert.ToInt32(Session["DonorId"].ToString());
                int donorID = donor.DonorId;

                var donorData = donorBL.Get(donorID, "Web");

                if (donorData != null)
                {


                    model.EmailID = donorData.DonorEmail;
                    model.SSN = donorData.DonorSSN;
                    model.FirstName = donorData.DonorFirstName;
                    model.MiddleInitial = donorData.DonorMI;
                    model.LastName = donorData.DonorLastName;
                    model.Suffix = donorData.DonorSuffix;
                    if (donorData.DonorDateOfBirth.ToString("MM/dd/yyyy") != "01/01/0001")
                    {
                        model.DonorDOBMonth = donorData.DonorDateOfBirth.Month.ToString();
                        model.DonorDOBDate = donorData.DonorDateOfBirth.Day.ToString();
                        model.DonorDOBYear = donorData.DonorDateOfBirth.Year.ToString();
                    }
                    else
                    {
                        model.DonorDOBDate = "";
                        model.DonorDOBMonth = "";
                        model.DonorDOBYear = "";
                    }
                    model.Address1 = donorData.DonorAddress1;
                    model.Address2 = donorData.DonorAddress2;
                    model.City = donorData.DonorCity;
                    model.State = donorData.DonorState;
                    model.ZipCode = donorData.DonorZip;
                    model.Phone1 = donorData.DonorPhone1;
                    model.Phone2 = donorData.DonorPhone2;
                    model.Gender = donorData.DonorGender;
                    model.DonorClearStarProfId = donorData.DonorClearStarProfId;

                    return View("ProfileUpdate", model);
                }
            }
            catch (Exception ex)
            {
                var blah = ex;//redirect to 404
            }
            //return View();
            return View("ProfileUpdate", model);
            //return RedirectToAction("Activation");
        }


        [HttpPost]
        //[SessionValidateAttribute]
        public ActionResult ProfileUpdate(DonorProfileDataModel model)
        {
            int donorID = Convert.ToInt32(Session["DonorId"].ToString());

            var donorSSNDetails = donorBL.GetBySSN(model.SSN.Trim(), "Web");

            if (donorSSNDetails == null || donorSSNDetails.DonorId == donorID)
            {
                var donorData = donorBL.Get(donorID, "Web");

                if (donorData != null)
                {

                    donorData.DonorSSN = model.SSN;
                    donorData.DonorFirstName = model.FirstName;
                    donorData.DonorMI = model.MiddleInitial;
                    donorData.DonorLastName = model.LastName;
                    donorData.DonorSuffix = model.Suffix;
                    DateTime DOB = Convert.ToDateTime(model.DonorDOBMonth.ToString() + '-' + model.DonorDOBDate.ToString() + '-' + model.DonorDOBYear.ToString());
                    if (Convert.ToDateTime(DOB.ToString()) < DateTime.Now)
                    {
                        donorData.DonorDateOfBirth = Convert.ToDateTime(DOB.ToString());
                    }
                    else
                    {
                        ViewBag.DOBERR = "Invalid D.O.B";
                        return View(model);
                    }
                    donorData.DonorAddress1 = model.Address1;
                    donorData.DonorAddress2 = model.Address2;
                    donorData.DonorCity = model.City;

                    if (model.State != null && model.State != "")
                    {
                        donorData.DonorState = model.State.Substring(0, 2);
                    }
                    else
                    {
                        donorData.DonorState = model.State;
                    }

                    donorData.DonorZip = model.ZipCode;
                    donorData.DonorPhone1 = model.Phone1;
                    donorData.DonorPhone2 = model.Phone2;

                    donorData.LastModifiedBy = donorData.DonorEmail;
                    donorData.DonorGender = model.Gender;
                    donorData.DonorClearStarProfId = model.DonorClearStarProfId;

                    int status = donorBL.Save(donorData);

                    if (status == 1)
                    {
                        //Session["SuccessMessage"] = "Your profile updated successfully";
                        return Redirect("ProgramSelection");
                    }
                    else
                    {
                        ViewBag.ServerErr = "Unable to process your request";
                    }

                }
                //return View(model);
                return View("ProfileUpdate", model);

            }

            ViewBag.SSNErr = "SSN already exist";
            //return View(model);
            return View("ProfileUpdate", model);
        }

        [HttpPost]
        //[SessionValidateAttribute]
        public ActionResult ProfileUpdate2(DonorProfileDataModel model)
        {
            Session["isPaymentStatus"] = "No";
            //int donorID = Convert.ToInt32(Session["DonorId"].ToString());

            int donorID = Convert.ToInt32(model.DonorId);
            Session["DonorId"] = model.DonorId;

            var donorSSNDetails = donorBL.GetBySSN(model.SSN.Trim(), "Web");

            //if (donorSSNDetails == null || donorSSNDetails.DonorId == donorID)
            if (donorID > 1)
            {
                var donorData = donorBL.Get(donorID, "Web");

                if (donorData != null)
                {

                    donorData.DonorSSN = model.SSN;
                    donorData.DonorFirstName = model.FirstName;
                    donorData.DonorMI = model.MiddleInitial;
                    donorData.DonorLastName = model.LastName;
                    donorData.DonorSuffix = model.Suffix;
                    DateTime DOB = Convert.ToDateTime(model.DonorDOBMonth.ToString() + '-' + model.DonorDOBDate.ToString() + '-' + model.DonorDOBYear.ToString());
                    if (Convert.ToDateTime(DOB.ToString()) < DateTime.Now)
                    {
                        donorData.DonorDateOfBirth = Convert.ToDateTime(DOB.ToString());
                    }
                    else
                    {
                        ViewBag.DOBERR = "Invalid D.O.B";
                        return View(model);
                    }
                    donorData.DonorAddress1 = model.Address1;
                    donorData.DonorAddress2 = model.Address2;
                    donorData.DonorCity = model.City;

                    if (model.State != null && model.State != "")
                    {
                        donorData.DonorState = model.State.Substring(0, 2);
                    }
                    else
                    {
                        donorData.DonorState = model.State;
                    }

                    donorData.DonorZip = model.ZipCode;
                    donorData.DonorPhone1 = model.Phone1;
                    donorData.DonorPhone2 = model.Phone2;

                    donorData.LastModifiedBy = donorData.DonorEmail;
                    donorData.DonorGender = model.Gender;

                    int status = donorBL.Save(donorData);

                    if (status == 1)
                    {
                        //Session["SuccessMessage"] = "Your profile updated successfully";
                        return Redirect("ProgramSelection");
                    }
                    else
                    {
                        ViewBag.ServerErr = "Unable to process your request";
                    }

                }
                //return View(model);
                return View("ProfileUpdate", model);

            }

            ViewBag.SSNErr = "SSN already exist";
            //return View(model);
            return View("ProfileUpdate", model);
        }

        [HttpGet]
        //[SessionValidateAttribute]
        public ActionResult ProgramSelection()
        {

            int donorId = Convert.ToInt32(Session["DonorId"]);

            Donor donor = donorBL.Get(donorId, "Web");

            if (donor != null)
            {
                if (donor.DonorInitialClientId != null)
                {
                    PrepareClientDepartments(Convert.ToInt32(donor.DonorInitialClientId));
                    GetPeymentMethod();
                }
            }
            if (Session["SuccessMessage"] != null)
            {
                ViewBag.SuccessMsg = Session["SuccessMessage"].ToString();
                Session["SuccessMessage"] = "";
            }
            return View();
        }

        [HttpPost]
        //[SessionValidateAttribute]
        public ActionResult ProgramSelection(string Program2)
        {

            int donorId = Convert.ToInt32(Session["DonorId"]);

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
            DataTable dtclient = clientBL.BCClientDepartment(clientDepartmentId);

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
            if ((donorTestInfo.ClientDepartmentId.ToString() != "0" && donorTestInfo.PaymentStatus == PaymentStatus.Paid) || (donorTestInfo.ClientDepartmentId.ToString() == "0"))
            {

                Session["DonorTestinfoId"] = "";
                Session["isPaymentStatus"] = "Yes";

                Donor donorReturn = donorBL.DoDonorRegistrationTestRequest(donorId, clientDepartmentId);

                string Donortestinfoid = donorReturn.DonorTestInfoId.ToString();
                Session["DonorTestinfoId"] = UserAuthentication.Encrypt(Donortestinfoid.ToString(), true);

                if (donorReturn != null)
                {
                    RegistrationDataModel model = new RegistrationDataModel();

                    model.FirstName = donorReturn.DonorFirstName;
                    Session["UserName"] = donorReturn.DonorFirstName;

                    model.LastName = donorReturn.DonorLastName;
                    model.Amount = donorReturn.ProgramAmount.ToString("N2");
                    model.DonorEmail = donorReturn.DonorEmail;

                    if (donorReturn.DonorSSN.Length == 11)
                    {
                        model.DonorSSN = "XXX-XX-" + donorReturn.DonorSSN.Substring(7);
                    }

                    model.DonorDOB = donorReturn.DonorDateOfBirth.ToString("MM/dd/yyyy");
                    model.DonorCity = donorReturn.DonorCity;
                    model.DonorState = donorReturn.DonorState;
                    model.DonorZipCode = donorReturn.DonorZip;

                    //ClientDepartment clientDepartment = clientBL.GetClientDepartment(clientDepartmentId);
                    Client client = clientBL.Get(donorReturn.ClientDepartment.ClientId);

                    model.ClientCode = client.ClientCode;
                    model.ClientName = client.ClientName;
                    model.DepartmentName = donorReturn.ClientDepartment.DepartmentName;


                    #region Background Check Information. Send()
                    ///Send Background Check Information.
                    ///--------------------------------------------------------------
                    ///
                    var creds = DefaultCredentialsBL.GetCredentials();
                    Surpath.ClearStar.BL.CustomerBL service = new CustomerBL();
                    Surpath.ClearStar.BL.ProfileBL pservice = new Surpath.ClearStar.BL.ProfileBL();


                    string sCustID = donorReturn.ClientDepartment.ClearStarCode;  //   "SLSS_00050";
                    Surpath.CSTest.Models.ProfileModel p = new Surpath.CSTest.Models.ProfileModel();
                    p.FirstName = donorReturn.DonorFirstName;
                    p.LastName = donorReturn.DonorLastName;
                    p.SSN = donorReturn.DonorSSN;
                    p.MiddleName = donorReturn.DonorMI;
                    p.Email = donorReturn.DonorEmail;
                    p.City = donorReturn.DonorCity;
                    p.Address1 = donorReturn.DonorAddress1;
                    p.Address2 = donorReturn.DonorAddress2;
                    p.State = donorReturn.DonorState;
                    p.Zip = donorReturn.DonorZip;
                    p.Phone = donorReturn.DonorPhone1;
                    p.BirthDate = donorReturn.DonorDateOfBirth.ToShortDateString();
                    p.Sex = donorReturn.DonorGender.ToString();
                    p.Weight = "";
                    p.Eyes = "";
                    p.Comments = "Web Registration";
                    p.RaceId = "";
                    p.Height = "";
                    p.Scars = "";
                    p.Address2 = "";
                    p.County = "";

                    if (donorReturn.ClientDepartment.ClearStarCode.Length > 0)

                    {
                        System.Xml.XmlNode profile = pservice.CreateProfileForCountry(p, sCustID);

                        //string ProfileNo = "2018070902779947";
                        //ProfileNo = string.Empty;
                        string ServiceNo = "SLSS_10092";//background check
                        string ProfileNo = string.Empty;
                        try

                        {
                            foreach (XmlNode node2 in profile["Profile"].ChildNodes)
                            {
                                if (node2.InnerText.Contains("10ErrorSQL:Cannot insert duplicate key row in object"))
                                {
                                    //return cust;
                                }
                                else
                                {
                                    //Debug.WriteLine("Node:" + node.Name + " " + node.InnerText);
                                    if (node2.Name == "Prof_No")
                                    { ProfileNo = node2.InnerText; }
                                    var newprofile = pservice.AddServicetoProfile(sCustID, ProfileNo, ServiceNo);
                                    //need to set the ClearStart Id in the Donor
                                    model.DonorClearStarProfileId = ProfileNo;
                                    donor.DonorClearStarProfId = ProfileNo;
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //return cust;
                        }

                    }
                    #endregion

                    model.TPAEmail = ConfigurationManager.AppSettings["TPAProgramToMailAddress"].ToString().Trim();

                    IUserMailer mail = new RegistrationMailer(model);
                    //mail.DonorProgramRegsitrationMail().Send();
                    if (!string.IsNullOrEmpty(client.ClientEmail.ToString()))
                    {
                        model.ClientEmail = client.ClientEmail;
                        mail.ClientProgramRegsitrationMail().Send();

                        //ToDo: Send Registration Complete Email. for background
                    }
                    //Send Donor Email?????
                    if (!string.IsNullOrEmpty(donor.DonorEmail.ToString()))
                    {
                        //model.ClientEmail = client.ClientEmail;
                        mail.DonorProgramRegsitrationMail().Send();
                        //mail.ClientProgramRegsitrationMail().Send();

                        //ToDo: Send Registration Complete Email. for background
                    }
                    //mail.TPAProgramRegsitrationMail().Send();
                    donorBL.Save(donor);
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
                Session["DonorTestinfoId"] = UserAuthentication.Encrypt(donorTestInfo.DonorTestInfoId.ToString(), true);
                Session["isPaymentStatus"] = "No";
                //ViewBag.TestAlreadyExists = "You cannot apply / take another test until existing dues / payments are settled.";
            }
            return View();
        }


        [HttpGet]
        [SessionValidateAttribute]
        public ActionResult ProgramConfirmation()
        {
            return View();
        }

        [HttpGet]
        //[SessionValidateAttribute]
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

                    try
                    {
                        model.DepartmentName = clientdepartment.DepartmentName;
                    }
                    catch (Exception)
                    {
                        model.DepartmentName = "Not available";
                    }
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
        //[SessionValidateAttribute]
        public ActionResult PaymentSelection(RegistrationDataModel regDataModel, string testInfoId, string paymentType)
        {
            testInfoId = Session["DonorTestinfoId"].ToString();

            //RegistrationDataModel model = new RegistrationDataModel();
            ViewBag.PayType = paymentType;
            ViewBag.PayType = "CARD";
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
            paymentType = "CARD";
            if (paymentType.ToUpper() == "CARD")
            {
                string authReturn = AuthorizeNetPayment(regDataModel);
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
                    //regDataModel.FirstName = regDataModel.FirstName;
                    regDataModel.Amount = donorTestInfo.TotalPaymentAmount.ToString();
                    regDataModel.DonorEmail = donorTestInfo.CreatedBy;//Session["UserLoginName"].ToString();

                    IUserMailer mail = new RegistrationMailer(regDataModel);
                    // mail.PaymentConformationMail().Send();
                    if (paymentType.ToUpper() == "CARD")
                    {
                        mail.CardPaymentConformationMail().Send();
                        return Redirect("PaymentConfirmationCard");
                    }
                    else
                    { // We made this the same as we no longer accept cash payments : Mike Kearl/Jon Jackson
                        //mail.PaymentConformationMail().Send();
                        mail.CardPaymentConformationMail().Send();
                        //return Redirect("PaymentConfirmationCash");
                        paymentType = "CARD";
                        return Redirect("PaymentConfirmationCard");

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
                    //donorTestInfo.LastModifiedBy = Session["UserLoginName"].ToString();

                    donorBL.SavePaymentDetails(donorTestInfo, "Yes");


                }
            }

            return true;
        }

        private string AuthorizeNetPayment(RegistrationDataModel regDataModel)
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
            if ((donorTestInfo.ClientDepartmentId.ToString() != "0" && donorTestInfo.PaymentStatus == PaymentStatus.Paid) || (donorTestInfo.ClientDepartmentId.ToString() == "0"))
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

        [HttpGet]
        [SessionValidateAttribute]
        public ActionResult ViewProfileDocument(string sCustID, string profileId)
        {

            //Check Local File 

            var csreports = ConfigurationManager.AppSettings["ClearStarReports"].ToString().Trim();
            try
            {
                BackendBCReport backendBCReport = new BackendBCReport();
                var res = backendBCReport.ViewProfileDocument(sCustID, profileId);
                var file = res.Item1;
                var bytes = res.Item2;
                var strType = System.IO.Path.GetExtension(file).Substring(1);

                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("content-disposition", "attachment; filename=" + file + "." + strType);
                Response.ContentType = strType;
                this.Response.BinaryWrite(bytes);
                this.Response.End();

                //var exists = System.IO.File.Exists(csreports + profileId + ".pdf");
                //if (!exists)
                //{


                //    var creds = DefaultCredentialsBL.GetCredentials();

                //    string ProfileNo = profileId;

                //    Surpath.ClearStar.BL.ProfileBL profile = new ProfileBL();

                //    var result = profile.GetProfileReport(sCustID, ProfileNo);

                //    var file = profile.SaveProfileReport(sCustID, ProfileNo, result);

                //    if (System.IO.File.Exists(file))
                //    {
                //        var document = System.IO.File.GetAttributes(file);

                //    }



                //    var strType = System.IO.Path.GetExtension(file).Substring(1);
                //    Response.Clear();
                //    Response.ClearContent();
                //    Response.ClearHeaders();
                //    Response.AddHeader("content-disposition", "attachment; filename=" + file + "." + strType);
                //    Response.ContentType = strType;
                //    this.Response.BinaryWrite(System.IO.File.ReadAllBytes(file));
                //    this.Response.End();
                //}
                //else
                //{
                //    var file = csreports + profileId + ".pdf";
                //    var strType = System.IO.Path.GetExtension(file).Substring(1);
                //    Response.Clear();
                //    Response.ClearContent();
                //    Response.ClearHeaders();
                //    Response.AddHeader("content-disposition", "attachment; filename=" + file + "." + strType);
                //    Response.ContentType = strType;
                //    this.Response.BinaryWrite(System.IO.File.ReadAllBytes(file));
                //    this.Response.End();
                //}
            }
            catch (Exception)
            { }

            return View();
        }

        #endregion
    }
}
