using SurPathWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaptchaMvc;
using CaptchaMvc.HtmlHelpers;
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
using System.Security.Authentication;
using System.Diagnostics;
using System.IO;
using Serilog;
using System.Text.RegularExpressions;
using SurpathBackend;
using SurPath.Data;
using Newtonsoft.Json;

namespace SurPathWeb.Controllers
{
    public class RegistrationController : Controller
    {
        #region Private Variables
        private BackendLogic backendLogic;
        private BackendData d;

        DonorBL donorBL; // = new DonorBL();
        ClientBL clientBL; // = new ClientBL();
        string donorStatus;
        //ILogger _logger = MvcApplication._logger;
        UserAuthentication userAuthentication = new UserAuthentication();


        #endregion


        public RegistrationController()
        {
            //this._logger = MvcApplication._logger;
            logDebug("New registrationController invoked");
            this.backendLogic = new BackendLogic(null, MvcApplication._logger);
            this.d = new BackendData(null, null, MvcApplication._logger);
            this.donorBL = new DonorBL();
            donorBL._logger = MvcApplication._logger;

            this.clientBL = new ClientBL();
            setDonorBLSession();

        }

        #region publicMethods
        [HttpGet]
        public ActionResult PreRegistration()
        {
            //if (Session != null)
            //{
            //    logDebug($"Session exists, preregistration called, abandoning session");
            //    Session.Abandon();
            //}
            logDebug("RegistrationController");
            //Session["IntegrationRegistration"] = false;
            setDonorBLSession();
            //if (string.IsNullOrEmpty(donorBL.SessionID) && Session.SessionID != null)
            //    donorBL = new DonorBL(_logger, Session.SessionID.ToString());
            return View();
            //return RedirectToAction("ProfileUpdate2");
            //return RedirectToAction("ProgramSelection");
        }

        [HttpPost]
        public ActionResult PreRegistration(RegistrationDataModel model)
        {
            setDonorBLSession();

            Donor donor = new Donor();
            if (this.IsCaptchaValid("The code you entered is not valid.") || MvcApplication.Production == false)
            {

                Client client = clientBL.Get(model.ClientCode.Trim());
                donor.DonorInitialClientId = client.ClientId;

                donor.DonorFirstName = model.FirstName;
                donor.DonorLastName = model.LastName;
                donor.DonorEmail = model.EmailID;
                donor.DonorRegistrationStatusValue = DonorRegistrationStatus.PreRegistration;

                Session["PreRegistrationClientCode"] = model.ClientCode.Trim();
                Session["PreRegistrationDonorObject"] = donor;

                Session["AlreadyCommitted"] = false;

            }
            else
            {
                ViewBag.ServerError = "TRUE";
                return View();
            }

            //return RedirectToAction("ProfileUpdate2", donor);
            return RedirectToAction("ProgramSelection", donor);

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

                // Since we're getting the client - we'll store if they have recordkeeping in session state
                // We're goign to store this in a session variable to avoid another trip to the database
                // TODO Move Session state to database or something else


                // Is this an integration parter? If so, we need to require a login
                // Return it here in case we decide to do it at first pass

                if (client.IntegrationPartner == true && client.require_remote_login == true)
                {
                    return Json(new { Success = 0, ErrorMsg = $"You must login at <a href='{client.login_url}'>{client.login_url}</a> to register with this client code", Url = client.login_url });
                }


                Session["Registration_Client_Has_Recordkeeping"] = client.ClientDepartments.SelectMany(d => d.ClientDeptTestCategories).Select(t => t.TestCategoryId == SurPath.Enum.TestCategories.RC).Where(r => r == true).ToList().Count() > 0;




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
        public ActionResult Activation(string donorid)
        {
            setDonorBLSession();
            ViewBag.DonorID = donorid;

            try
            {
                int donorID = Convert.ToInt32(UserAuthentication.URLIDDecrypt(Helper.Base64ForUrlDecode(donorid.ToString()), true));

                var donorData = donorBL.Get(donorID, "Web");

                // Per phone converastion with Jon - sep 23 2:50 PM - we're going to check the associated user account
                // and if the reset password flag is set, we're going to proceed with activation
                // but we if they're in-queue, we don't want to go backwards

                UserBL userBL = new UserBL();

                User user = userBL.GetByDonorId(donorID);

                bool bGotoLogin = false;

                if (donorData == null) bGotoLogin = true;

                if (donorData.DonorRegistrationStatusValue == DonorRegistrationStatus.PreRegistration && user.ChangePasswordRequired == false) bGotoLogin = true;

                if (bGotoLogin) return RedirectToAction("Login", "Authentication");



                // This is negative logic - hard to read - replacing with simplier logic
                //if (!(donorData != null && donorData.DonorRegistrationStatusValue == DonorRegistrationStatus.PreRegistration))
                //{
                //    return RedirectToAction("Login", "Authentication");
                //}
            }
            catch
            {
                ViewBag.ServerErr = "Unable to process your request.";
            }
            return View();
        }

        [HttpPost]
        public ActionResult Activation(string donorid, string oldPassword, string newPassword)
        {
            try
            {
                setDonorBLSession();
                ViewBag.DonorID = donorid;

                int _donorid = Convert.ToInt32(UserAuthentication.URLIDDecrypt(Helper.Base64ForUrlDecode(donorid.ToString()), true));
                Donor donor = new Donor();
                donor = donorBL.Get(_donorid, "Web");
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

                donorBL.DoDonorInQueueUpdate(_donorid, oldPassword, newPassword, donorStatus, true);
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
                    ViewBag.ServerErr = "Unable to process your request.\n" + e.Message;
                }
                return View();
            }
            return RedirectToAction("PasswordResetConfirmation");
        }

        public ActionResult PasswordResetConfirmation()
        {
            return View();
        }

        //// This is from prod merge

        //[HttpGet]
        //[SessionValidateAttribute]
        //public ActionResult ProfileUpdate()
        //{
        //    try
        //    {
        //        setDonorBLSession();
        //        //int donorID = Convert.ToInt32(Session["DonorId"].ToString());

        //        var donorPreRegDonorObject = (Donor)Session["PreRegistrationDonorObject"];

        //        if (donorPreRegDonorObject != null)
        //        {
        //            DonorProfileDataModel model = new DonorProfileDataModel();

        //            model.EmailID = donorPreRegDonorObject.DonorEmail;
        //            model.SSN = donorPreRegDonorObject.DonorSSN;
        //            model.FirstName = donorPreRegDonorObject.DonorFirstName;
        //            model.MiddleInitial = donorPreRegDonorObject.DonorMI;
        //            model.LastName = donorPreRegDonorObject.DonorLastName;
        //            model.Suffix = donorPreRegDonorObject.DonorSuffix;
        //            if (donorPreRegDonorObject.DonorDateOfBirth.ToString("MM/dd/yyyy") != "01/01/0001")
        //            {
        //                model.DonorDOBMonth = donorPreRegDonorObject.DonorDateOfBirth.Month.ToString();
        //                model.DonorDOBDate = donorPreRegDonorObject.DonorDateOfBirth.Day.ToString();
        //                model.DonorDOBYear = donorPreRegDonorObject.DonorDateOfBirth.Year.ToString();
        //            }
        //            else
        //            {
        //                model.DonorDOBDate = "";
        //                model.DonorDOBMonth = "";
        //                model.DonorDOBYear = "";
        //            }
        //            model.Address1 = donorPreRegDonorObject.DonorAddress1;
        //            model.Address2 = donorPreRegDonorObject.DonorAddress2;
        //            model.City = donorPreRegDonorObject.DonorCity;
        //            model.State = donorPreRegDonorObject.DonorState;
        //            model.ZipCode = donorPreRegDonorObject.DonorZip;
        //            model.Phone1 = donorPreRegDonorObject.DonorPhone1;
        //            model.Phone2 = donorPreRegDonorObject.DonorPhone2;
        //            model.Gender = donorPreRegDonorObject.DonorGender;

        //            return View(model);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //redirect to 404
        //    }
        //    return View();
        //}

        ///// <summary>
        ///// Step 3
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[HttpPost]
        ////[SessionValidateAttribute]
        //public ActionResult ProfileUpdate(DonorProfileDataModel model)
        //{
        //    setDonorBLSession();

        //    if (string.IsNullOrEmpty(model.SSN)) model.SSN = string.Empty;

        //    if (model.PIDTypeValues.Exists(x => x.PIDType == (int)PidTypes.SSN))
        //    {
        //        if (!string.IsNullOrEmpty(model.PIDTypeValues.Where(x => x.PIDType == (int)PidTypes.SSN).FirstOrDefault().PIDValue))
        //        {
        //            model.SSN = model.PIDTypeValues.Where(x => x.PIDType == (int)PidTypes.SSN).FirstOrDefault().PIDValue.Trim();
        //        }
        //    }

        //    var donorSSNDetails = donorBL.GetBySSN(model.SSN.Trim(), "Web");

        //    if (donorSSNDetails == null)
        //    {
        //        Donor donor = (Donor)Session["PreRegistrationDonorObject"];

        //        if (donor != null)
        //        {

        //            donor.DonorSSN = model.SSN;
        //            donor.DonorFirstName = model.FirstName;
        //            donor.DonorMI = model.MiddleInitial;
        //            donor.DonorLastName = model.LastName;
        //            donor.DonorSuffix = model.Suffix;
        //            DateTime DOB = Convert.ToDateTime(model.DonorDOBMonth.ToString() + '-' + model.DonorDOBDate.ToString() + '-' + model.DonorDOBYear.ToString());
        //            if (Convert.ToDateTime(DOB.ToString()) < DateTime.Now)
        //            {
        //                donor.DonorDateOfBirth = Convert.ToDateTime(DOB.ToString());
        //            }
        //            else
        //            {
        //                ViewBag.DOBERR = "Invalid D.O.B";
        //                return View(model);
        //            }
        //            donor.DonorAddress1 = model.Address1;
        //            donor.DonorAddress2 = model.Address2;
        //            donor.DonorCity = model.City;

        //            if (model.State != null && model.State != "")
        //            {
        //                donor.DonorState = model.State.Substring(0, 2);
        //            }
        //            else
        //            {
        //                donor.DonorState = model.State;
        //            }

        //            donor.DonorZip = model.ZipCode;
        //            donor.DonorPhone1 = model.Phone1;
        //            donor.DonorPhone2 = model.Phone2;

        //            donor.LastModifiedBy = donor.DonorEmail;
        //            donor.DonorGender = model.Gender;



        //            /// TODO - Here - we could check First Name, last name, SSN, and DOB and see if there's an existing DONOR / USER
        //            /// If so - we'd stick that into Session
        //            /// Then, when registration is compleate, we'd add an update to to the profile / user in BL & DAO
        //            /// Then use that donor id, user id, client id, dept and create test info records.
        //            /// See the finalize function for next steps



        //            return View("ProgramSelection");


        //        }
        //        //return View(model);
        //        return View("ProfileUpdate", model);

        //    }

        //    ViewBag.SSNErr = "SSN already exist";
        //    //return View(model);
        //    return View("ProfileUpdate", model);
        //}




        [HttpGet]
        //[SessionValidateAttribute]
        public ActionResult ProfileUpdate2(Donor donor)
        {
            logDebug("ProfileUpdate2 Get");
            setDonorBLSession();
            DonorProfileDataModel model = new DonorProfileDataModel();
            if (Session["DonorProfileDataModel"] != null)
            {
                logDebug("DonorProfileDataModel in session, returning view");

                model = (DonorProfileDataModel)Session["DonorProfileDataModel"];

                if (Session["PidTypeValues"] != null)
                {
                    logDebug("PidTypeValues in session");
                    model.PIDTypeValues = (List<PIDTypeValue>)Session["PidTypeValues"];
                }
                else
                {
                    logDebug("PidTypeValues not in session");
                }
                return View("ProfileUpdate2", model);

            }
            try
            {
                ViewBag.IntegrationPidType = 0;
                if (donor != null)
                {

                    var client_id = (int)Session["IntegrationClientId"];
                    var client_department_id = (int)Session["IntegrationClientDeptId"];

                    ClientDepartment clientDepartment = clientBL.GetClientDepartment(client_department_id);
                    if (clientDepartment.integrationPartner == true)
                    {
                        var _ip = backendLogic.GetIntegrationPartnerbyClientidDeptId(client_id);
                        ViewBag.IntegrationPidType = _ip.backend_integration_partners_pidtype;
                    }




                    if (Session["PidTypeValues"] != null)
                    {
                        logDebug("PidTypeValues in session");
                        donor.PidTypeValues = (List<PIDTypeValue>)Session["PidTypeValues"];
                        if (donor.PidTypeValues.Count > 0)
                        {
                            // we have values to carry over.
                            foreach (PIDTypeValue pv in donor.PidTypeValues)
                            {

                                if (!model.PIDTypeValues.Exists(_pv => _pv.PIDType == pv.PIDType))
                                {
                                    model.PIDTypeValues.Add(pv);
                                }
                                else
                                {
                                    var _p = model.PIDTypeValues.Where(_pv => _pv.PIDType == pv.PIDType).First();
                                    var _i = model.PIDTypeValues.IndexOf(_p);
                                    model.PIDTypeValues[_i] = pv;
                                }
                            }
                        }
                    }
                    else
                    {
                        logDebug("PidTypeValues not in session");
                    }

                    model.EmailID = donor.DonorEmail;
                    model.SSN = donor.DonorSSN;
                    model.FirstName = donor.DonorFirstName;
                    model.MiddleInitial = donor.DonorMI;
                    model.LastName = donor.DonorLastName;
                    model.Suffix = donor.DonorSuffix;
                    if (donor.DonorDateOfBirth.ToString("MM/dd/yyyy") != "01/01/0001")
                    {
                        model.DonorDOBMonth = donor.DonorDateOfBirth.Month.ToString();
                        model.DonorDOBDate = donor.DonorDateOfBirth.Day.ToString();
                        model.DonorDOBYear = donor.DonorDateOfBirth.Year.ToString();
                    }
                    else
                    {
                        model.DonorDOBDate = "";
                        model.DonorDOBMonth = "";
                        model.DonorDOBYear = "";
                    }
                    model.Address1 = donor.DonorAddress1;
                    model.Address2 = donor.DonorAddress2;
                    model.City = donor.DonorCity;
                    model.State = donor.DonorState;
                    model.ZipCode = donor.DonorZip;
                    model.Phone1 = donor.DonorPhone1;
                    model.Phone2 = donor.DonorPhone2;
                    model.Gender = donor.DonorGender;

                    bool SSNRequired = (bool)Session["SSNRequired"];
                    if (SSNRequired == true)
                    {
                        model.PIDTypeValues.Where(x => x.PIDType == (int)PidTypes.SSN).FirstOrDefault().required = true;
                        //if (!model.PIDTypeValues.Exists(x => x.PIDType == (int)PidTypes.SSN))
                        //{
                        //    model.PIDTypeValues.Add(new PIDTypeValue() { required=true, Err="", isReadOnly=false, PIDType= (int)PidTypes.SSN , PIDValue=""}); 
                        //}
                        //else
                        //{
                        //    var _p = model.PIDTypeValues.Where(x => x.PIDType == (int)PidTypes.SSN).First();
                        //    var _i = model.PIDTypeValues.IndexOf(_p);
                        //    _p.required = true;
                        //    model.PIDTypeValues[_i] = _p;
                        //}
                    }
                    logDebug("DonorProfileDataModel not in session, populated from donor object, returning view");
                    return View("ProfileUpdate2", model);
                }
            }
            catch (Exception ex)
            {
                //redirect to 404
            }
            //return View();
            return View("ProfileUpdate2", model);
            //return RedirectToAction("Activation");
        }


        [HttpPost]
        //[SessionValidateAttribute]
        public ActionResult ProfileUpdate2(DonorProfileDataModel model)
        {
            logDebug("ProfileUpdate2 Post");
            setDonorBLSession();

            model.SSN = (string)model.PIDTypeValues.Where(x => x.PIDType == (int)PidTypes.SSN).FirstOrDefault().PIDValue;
            model.SSN = getDashedSSN(model.SSN);
            if (string.IsNullOrEmpty(model.SSN)) model.SSN = string.Empty;

            Session["isPaymentStatus"] = "No";
            Donor donor = (Donor)Session["PreRegistrationDonorObject"];
            Session["PreRegistrationDonorData"] = model;
            var donorSSNDetails = donorBL.GetBySSN(model.SSN.Trim(), "Web");

            // Overridable in web.config using ForceUniqueSSN key
            bool ForceUniqueSSN = true;
            bool.TryParse(ConfigurationManager.AppSettings["ForceUniqueSSN"].ToString().Trim(), out ForceUniqueSSN);
            if (donorSSNDetails != null && ForceUniqueSSN == true)
            {
                ViewBag.SSNErr = "SSN already exist";
                //return View(model);
                return View("ProfileUpdate2", model);
            }

            if (!backendLogic.IsValidEmail(model.EmailID))
            {
                ViewBag.ServerErr = "Invalid Email";
                //return View(model);
                return View("ProfileUpdate2", model);
            }

            //if (donorSSNDetails == null || donorSSNDetails.DonorId == donorID)


            if (donor != null)
            {
                if (!model.EmailID.Equals(donor.DonorEmail, StringComparison.InvariantCultureIgnoreCase))
                {
                    donor.DonorEmail = model.EmailID;
                }
                donor.DonorSSN = model.SSN;
                donor.DonorFirstName = model.FirstName;
                donor.DonorMI = model.MiddleInitial;
                donor.DonorLastName = model.LastName;
                donor.DonorSuffix = model.Suffix;
                DateTime DOB = Convert.ToDateTime(model.DonorDOBMonth.ToString() + '-' + model.DonorDOBDate.ToString() + '-' + model.DonorDOBYear.ToString());
                if (Convert.ToDateTime(DOB.ToString()) < DateTime.Now)
                {
                    donor.DonorDateOfBirth = Convert.ToDateTime(DOB.ToString());
                }
                else
                {
                    ViewBag.DOBERR = "Invalid D.O.B";
                    return View(model);
                }
                donor.DonorAddress1 = model.Address1;
                donor.DonorAddress2 = model.Address2;
                donor.DonorCity = model.City;

                if (model.State != null && model.State != "")
                {
                    donor.DonorState = model.State.Substring(0, 2);
                }
                else
                {
                    donor.DonorState = model.State;
                }

                donor.DonorZip = model.ZipCode;
                donor.DonorPhone1 = model.Phone1;
                donor.DonorPhone2 = model.Phone2;

                donor.LastModifiedBy = donor.DonorEmail;
                donor.DonorGender = model.Gender;

                if (donor.PidTypeValues.Count > 0)
                {
                    // we have values to carry over.
                    foreach (PIDTypeValue pv in donor.PidTypeValues)
                    {

                        if (!model.PIDTypeValues.Exists(_pv => _pv.PIDType == pv.PIDType))
                        {
                            model.PIDTypeValues.Add(pv);
                        }
                        else
                        {
                            var _p = model.PIDTypeValues.Where(_pv => _pv.PIDType == pv.PIDType).First();
                            var _i = model.PIDTypeValues.IndexOf(_p);
                            model.PIDTypeValues[_i] = pv;
                        }
                    }
                }

                donor.PidTypeValues = new List<PIDTypeValue>();
                foreach (PIDTypeValue pv in model.PIDTypeValues)
                {
                    donor.PidTypeValues.Add(pv);
                }

                Session["PreRegistrationDonorObject"] = donor;
                //
                //
                //
                RegistrationDataModel model2 = new RegistrationDataModel();

                model2.FirstName = donor.DonorFirstName;
                Session["UserName"] = donor.DonorFirstName;

                model2.LastName = donor.DonorLastName;
                model2.Amount = donor.ProgramAmount.ToString("N2");
                model2.DonorEmail = donor.DonorEmail;

                if (donor.DonorSSN.Length == 11)
                {
                    model2.DonorSSN = "XXX-XX-" + donor.DonorSSN.Substring(7);
                }

                model2.DonorDOB = donor.DonorDateOfBirth.ToString("MM/dd/yyyy");
                model2.DonorCity = donor.DonorCity;
                model2.DonorState = donor.DonorState;
                model2.DonorZipCode = donor.DonorZip;

                //ClientDepartment clientDepartment = clientBL.GetClientDepartment(clientDepartmentId);
                //if (donor.ClientDepartment == null)
                //{
                //    if (Session["IntegrationClientDeptId"] != null)
                //    {
                //        donor.ClientDepartment = clientBL.GetClientDepartment((int)Session["IntegrationClientDeptId"]);
                //    }
                //}
                Client client = clientBL.Get(donor.ClientDepartment.ClientId);

                model2.ClientCode = client.ClientCode;
                model2.ClientName = client.ClientName;
                model2.DepartmentName = donor.ClientDepartment.DepartmentName;

                Session["PreRegistrationDonorObject"] = donor;
                Session["PreRegistrationDonorData"] = model;

                model2.TPAEmail = ConfigurationManager.AppSettings["TPAProgramToMailAddress"].ToString().Trim();

                IUserMailer mail = new RegistrationMailer(model2);
                //mail.DonorProgramRegsitrationMail().Send();
                if (!string.IsNullOrEmpty(client.ClientEmail.ToString()))
                {
                    model2.ClientEmail = client.ClientEmail;
                    //mail.ClientProgramRegsitrationMail().Send();
                }
                //
                //
                //
                // Make sure at least one of the PIDs is not null
                bool GoodPids = true;
                bool AllRequiredPidsProvided = model.PIDTypeValues.Where(x => x.required == true && !(string.IsNullOrEmpty(x.PIDValue))).Count() == model.PIDTypeValues.Where(x => x.required == true).Count();

                bool PidProvided = model.PIDTypeValues.Where(x => !(string.IsNullOrEmpty(x.PIDValue))).Count() > 0;

                if (AllRequiredPidsProvided == false)
                {
                    GoodPids = false;
                    // we're missing a required pid
                    foreach (PIDTypeValue pv in model.PIDTypeValues)
                    {
                        if (pv.required == true && string.IsNullOrEmpty(pv.PIDValue))
                        {
                            pv.Err = "This is required for this program.";
                            model.PidErrorMesages.Add("Please complete all required personal identification fields");
                        }
                    }
                }
                if (PidProvided == false)
                {
                    GoodPids = false;
                    model.PidErrorMesages.Add("Please complete at at least one personal identification field.");
                }

                Session["DonorProfileDataModel"] = model;
                if (GoodPids == false)
                {
                    return RedirectToAction("ProfileUpdate2", donor);

                }
                Session["PIDTypeValues"] = model.PIDTypeValues;
                Session["PidErrorMesages"] = model.PidErrorMesages;
                return RedirectToAction("ProfileReview", model);

                //return RedirectToAction("ProgramSelection");

            }
            else
            {
                return RedirectToAction("PreRegistration");
            }

        }


        [HttpGet]
        public ActionResult ProfileReview()
        {
            setDonorBLSession();
            DonorProfileDataModel model = new DonorProfileDataModel();
            if (Session["DonorProfileDataModel"] != null)
            {
                model = (DonorProfileDataModel)Session["DonorProfileDataModel"];

            }
            else
            {
                return View("ProfileUpdate2", model);

            }

            model.PidErrorMesages = (List<string>)Session["PidErrorMesages"];
            model.PIDTypeValues = (List<PIDTypeValue>)Session["PIDTypeValues"];

            //Session["PIDTypeValues"] = model.PIDTypeValues;
            //Session["PidErrorMesages"] = model.PidErrorMesages;


            //try
            //{

            //    if (donor != null)
            //    {


            //        model.EmailID = donor.DonorEmail;
            //        model.SSN = donor.DonorSSN;
            //        model.FirstName = donor.DonorFirstName;
            //        model.MiddleInitial = donor.DonorMI;
            //        model.LastName = donor.DonorLastName;
            //        model.Suffix = donor.DonorSuffix;
            //        if (donor.DonorDateOfBirth.ToString("MM/dd/yyyy") != "01/01/0001")
            //        {
            //            model.DonorDOBMonth = donor.DonorDateOfBirth.Month.ToString();
            //            model.DonorDOBDate = donor.DonorDateOfBirth.Day.ToString();
            //            model.DonorDOBYear = donor.DonorDateOfBirth.Year.ToString();
            //        }
            //        else
            //        {
            //            model.DonorDOBDate = "";
            //            model.DonorDOBMonth = "";
            //            model.DonorDOBYear = "";
            //        }
            //        model.Address1 = donor.DonorAddress1;
            //        model.Address2 = donor.DonorAddress2;
            //        model.City = donor.DonorCity;
            //        model.State = donor.DonorState;
            //        model.ZipCode = donor.DonorZip;
            //        model.Phone1 = donor.DonorPhone1;
            //        model.Phone2 = donor.DonorPhone2;
            //        model.Gender = donor.DonorGender;

            //        bool SSNRequired = (bool)Session["SSNRequired"];
            //        if (SSNRequired == true)
            //        {
            //            model.PIDTypeValues.Where(x => x.PIDType == (int)PidTypes.SSN).FirstOrDefault().required = true;
            //        }

            //        return View("ProfileUpdate", model);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    //redirect to 404
            //}
            //return View();
            return View("ProfileReview", model);
            //return RedirectToAction("Activation");
        }

        [HttpPost]
        public ActionResult ProfileReview(DonorProfileDataModel model, string button)
        {

            setDonorBLSession();
            model = new DonorProfileDataModel();
            if (Session["DonorProfileDataModel"] != null)
            {
                model = (DonorProfileDataModel)Session["DonorProfileDataModel"];

            }
            else
            {
                return View("ProfileUpdate2", model);

            }

            model.PidErrorMesages = (List<string>)Session["PidErrorMesages"];
            model.PIDTypeValues = (List<PIDTypeValue>)Session["PIDTypeValues"];

            Donor donor = (Donor)Session["PreRegistrationDonorObject"];

            if (button.Equals("Edit", StringComparison.InvariantCultureIgnoreCase))
            {
                return RedirectToAction("ProfileUpdate2", donor);
            }
            logDebug($"Donor registered - {donor.DonorFirstName} {donor.DonorLastName}");

            // check if this donor is in the pay override table.

            var OverRideDonorPay = backendLogic.OverrideDonorPay(donor.DonorEmail);

            // this logic needs to be on the final step
            if (donor.ClientDepartment.PaymentTypeId == ClientPaymentTypes.DonorPays && OverRideDonorPay == null)
            {
                logDebug($"Donor pay, redirecting... {donor.DonorFirstName} {donor.DonorLastName}");
                return RedirectToAction("PaymentSelection");
            }
            else
            {
                if (OverRideDonorPay != null)
                {
                    // burn the payment override
                    if (DoOverridePayment(OverRideDonorPay))
                    {
                        backendLogic.BurnOverrideDonorPay(donor.DonorEmail);
                    }
                }


                logDebug($"Client pay, committing... {donor.DonorFirstName} {donor.DonorLastName}");
                // Since client pay, we commit here
                if (Session == null)
                {
                    {
                        logDebug("Session is null, can't call CommitRegistrationToDatabase, sending to login");
                        return RedirectToAction("Login", "Authentication");

                    }
                }
                CommitRegistrationToDatabase();
                return RedirectToAction("ProgramConfirmation");
            }
        }





        [HttpGet]
        //[SessionValidateAttribute]
        public ActionResult ProgramSelection()
        {
            Session["isPaymentStatus"] = "No";

            Donor donor = (Donor)Session["PreRegistrationDonorObject"];
            setDonorBLSession();
            if (donor != null)
            {
                if (donor.DonorInitialClientId != null)
                {
                    PrepareClientDepartments(Convert.ToInt32(donor.DonorInitialClientId));
                    GetPaymentMethod();
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

            logDebug($"ProgramSelection HttpPost: {Program2}");
            if (Session == null)
            {
                logDebug("Session is null, sending to login");
                return RedirectToAction("Login", "Authentication");
            }
            setDonorBLSession();
            logDebug($"Getting program");
            int clientDepartmentId = Convert.ToInt32(UserAuthentication.Decrypt(Program2, true));
            logDebug($"Getting donor from PreRegistrationDonorObject");
            Donor donor = (Donor)Session["PreRegistrationDonorObject"];

            if (donor != null)
            {
                logDebug($"Donor not null");
                if (donor.DonorInitialClientId != null)
                {
                    logDebug($"DonorInitialClientId not null ({donor.DonorInitialClientId})");
                    PrepareClientDepartments(Convert.ToInt32(donor.DonorInitialClientId));
                }
            }


            ClientBL clientBL = new ClientBL();
            ClientDeptTestCategory testcategory = new ClientDeptTestCategory();
            DataTable dtclient = clientBL.ClientDepartment(clientDepartmentId);
            ClientDepartment clientDepartment = clientBL.GetClientDepartment(clientDepartmentId);
            logDebug($"Dept Loaded");

            bool UAtestpanelidflag = true;
            bool Hairtestpanelidflag = true;
            foreach (DataRow dr in dtclient.Rows)
            {
                testcategory.TestCategoryId = (TestCategories)(dr["TestCategoryId"]);

                if (testcategory.TestCategoryId == TestCategories.UA)
                {
                    ClientDeptTestPanel deptTestPanel = new ClientDeptTestPanel();

                    deptTestPanel.TestPanelId = Convert.ToInt32(dr["TestPanelId"]);
                    if (deptTestPanel.TestPanelId == 0)
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
                    ClientDeptTestPanel deptTestPanel = new ClientDeptTestPanel();

                    deptTestPanel.TestPanelId = Convert.ToInt32(dr["TestPanelId"]);
                    if (deptTestPanel.TestPanelId == 0)
                    {
                        Hairtestpanelidflag = false;

                    }
                    else
                    {
                        Hairtestpanelidflag = true;
                    }
                }


            }

            Session["SSNRequired"] = false;
            if (clientDepartment.ClientDeptTestCategories.Exists(x => x.TestCategoryId == TestCategories.BC)) Session["SSNRequired"] = true;
            Session["SSNRequired"] = true; // Require all SSN for now

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

            DonorTestInfo donorTestInfo;
            donorTestInfo = CreatePreRegistrationDonorTestObject(clientDepartment);
            Session["donorTestInfo"] = donorTestInfo;

            //if ((donorTestInfo.ClientDepartmentId.ToString() != "0" && donorTestInfo.PaymentStatus == PaymentStatus.Paid) || (donorTestInfo.ClientDepartmentId.ToString() == "0"))
            //{

            Session["DonorTestinfoId"] = "";
            Session["isPaymentStatus"] = "Yes";

            //Donor donorReturn = donorBL.DoDonorRegistrationTestRequest(donorId, clientDepartmentId, true);
            donorBL.DoDonorPreRegistrationTestRequest(donor, clientDepartmentId);


            string Donortestinfoid = donor.DonorTestInfoId.ToString();
            Session["DonorTestinfoId"] = UserAuthentication.Encrypt(Donortestinfoid.ToString(), true);





            if (donor != null)
            {
                RegistrationDataModel model = new RegistrationDataModel();

                model.FirstName = donor.DonorFirstName;
                Session["UserName"] = donor.DonorFirstName;

                model.LastName = donor.DonorLastName;
                model.Amount = donor.ProgramAmount.ToString("N2");
                model.DonorEmail = donor.DonorEmail;


                model.DonorDOB = donor.DonorDateOfBirth.ToString("MM/dd/yyyy");
                model.DonorCity = donor.DonorCity;
                model.DonorState = donor.DonorState;
                model.DonorZipCode = donor.DonorZip;

                //ClientDepartment clientDepartment = clientBL.GetClientDepartment(clientDepartmentId);
                Client client = clientBL.Get(donor.ClientDepartment.ClientId);

                model.ClientCode = client.ClientCode;
                model.ClientName = client.ClientName;
                model.DepartmentName = donor.ClientDepartment.DepartmentName;
                // added to address processing issue.
                donor.DonorInitialDepartmentId = clientDepartmentId;

                Session["PreRegistrationDonorObject"] = donor;

                model.TPAEmail = ConfigurationManager.AppSettings["TPAProgramToMailAddress"].ToString().Trim();

                IUserMailer mail = new RegistrationMailer(model);
                //mail.DonorProgramRegsitrationMail().Send();
                if (!string.IsNullOrEmpty(client.ClientEmail.ToString()))
                {
                    model.ClientEmail = client.ClientEmail;
                    //mail.ClientProgramRegsitrationMail().Send();
                }
                //mail.TPAProgramRegsitrationMail().Send();


                // here is wehre we need to go to demographics
                Session["IntegrationClientId"] = clientDepartment.ClientId;
                Session["IntegrationClientDeptId"] = clientDepartment.ClientDepartmentId;


                //// ********** Integration login check
                logDebug($"Integration check: {clientDepartment.integrationPartner}");
                if (clientDepartment.integrationPartner == true)
                {
                    if (clientDepartment.requireLogin == true)
                    {
                        logDebug($"Login required.");

                        if (Session["UserId"] == null)
                        {
                            logDebug($"Session UserID is null");
                        }
                        else
                        {
                            logDebug($"Session UserID is {(int)Session["UserId"]}");
                        }

                        // if the donor is not logged in, we need to require that they do so.
                        if (Session["UserId"] == null || (int)Session["UserId"] < 1)
                        {
                            logDebug("Redirecting to create account");

                            Session["IntegrationRegistration"] = true;

                            //return RedirectToAction("Login", "Authentication");
                            return RedirectToAction("CreateAccount", donor);
                        }
                    }
                    if (clientDepartment.require_remote_login == true)
                    {

                        logDebug("Remote login required");

                        if (string.IsNullOrEmpty(clientDepartment.login_url))
                        {
                            LogAnError(new Exception($"Invalid URL ({clientDepartment.login_url}), redirecting to login"));
                            return RedirectToAction("Login", "Authentication");
                        }

                        logDebug($"Login URL: {clientDepartment.login_url}");
                        if (Session["ValidHandoff"] == null || (bool)Session["ValidHandoff"] == false)
                        {
                            Uri uriResult;
                            bool result = Uri.TryCreate(clientDepartment.login_url, UriKind.Absolute, out uriResult)
                                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                            if (result == true)
                            {
                                logDebug($"Redirecting to {clientDepartment.login_url}");
                                return Redirect(clientDepartment.login_url);

                            }
                        }


                        //if (Session["ValidHandoff"]!=null)
                        //{
                        //    if ((bool)Session["ValidHandoff"]==true)
                        //    {

                        //    }
                        //}
                        //else
                        //{
                        //    Uri uriResult;
                        //    bool result = Uri.TryCreate(clientDepartment.login_url, UriKind.Absolute, out uriResult)
                        //        && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                        //    if (result == true)
                        //    {
                        //        logDebug($"Redirecting to {clientDepartment.login_url}");
                        //        return Redirect(clientDepartment.login_url);

                        //    }
                        //}




                    }
                }

                return RedirectToAction("ProfileUpdate2", donor);

            }
            else
            {
                return View();
            }
            //}
            //else
            //{
            //    Session["DonorTestinfoId"] = UserAuthentication.Encrypt(donorTestInfo.DonorTestInfoId.ToString(), true);
            //    Session["isPaymentStatus"] = "No";
            //    //ViewBag.TestAlreadyExists = "You cannot apply / take another test until existing dues / payments are settled.";
            //}
            //return View();
        }

        [HttpGet]
        [SessionValidateAttribute]
        public ActionResult ProgramConfirmation(DonorProfileDataModel model)
        {
            setDonorBLSession();
            //if (Session != null) Session.Abandon();

            return View();

        }

        [HttpGet]
        //[SessionValidateAttribute]
        public ActionResult PaymentSelection(string testInfoId)
        {
            try
            {
                // We're in preregistration - so we don't have DonorTestinfoId
                // because the record doesn't exist
                // We need to create RegistrationDataModel.PaymentData
                // and populate it
                logDebug($"PaymentSelection HttpGet...");
                setDonorBLSession();
                DonorTestInfo donorTestInfo = (DonorTestInfo)Session["donorTestInfo"];
                Donor donor = (Donor)Session["PreRegistrationDonorObject"];

                if (donorTestInfo != null)
                {
                    logDebug($"PaymentSelection using donorTestInfo {donorTestInfo.DonorTestInfoId.ToString()} {donor.DonorFirstName} {donor.DonorLastName}");

                    //testInfoId = Session["DonorTestinfoId"].ToString();

                    RegistrationDataModel model = new RegistrationDataModel();
                    model.PaymentData = new PaymentDataModel();

                    //string donortestinfoid = UserAuthentication.Decrypt(testInfoId, true);

                    //DonorTestInfo donortestinfo = donorBL.GetDonorTestInfo(Convert.ToInt32(donortestinfoid));

                    ClientDepartment clientdepartment = clientBL.GetClientDepartment(donorTestInfo.ClientDepartmentId);
                    Client client = clientBL.Get(donorTestInfo.ClientId);


                    model.DepartmentName = clientdepartment.DepartmentName;
                    model.ClientName = client.ClientName;
                    model.Amount = donor.ProgramAmount.ToString(); //donortestinfo.TotalPaymentAmount.ToString();
                    model.PaymentData.Amount = donor.ProgramAmount.ToString(); //donortestinfo.TotalPaymentAmount.ToString();
                    model.PaymentData.Description = client.ClientName;
                    model.PaymentType = PaymentMethod.Card;
                    model.PaymentData.Email = donor.DonorEmail;
                    logDebug($"PaymentSelection model: model.DepartmentName {model.DepartmentName}, model.ClientName {model.ClientName}, model.Amount {donor.ProgramAmount.ToString()}, model.PaymentData.Description {model.PaymentData.Description}, model.PaymentData.Email {model.PaymentData.Email}");
                    return View(model);
                }
                else
                {
                    logDebug($"donorTestInfo is null, abandoning this session");

                    Session.Abandon();
                    return RedirectToAction("Login", "Authentication");
                }
            }
            catch (Exception ex)
            {
                logDebug($"An error was thrown while trying to process a credit card!! PaymentSelection [GET]");
                LogAnError(ex);
                return RedirectToAction("Login", "Authentication");
            }

        }

        private bool DoOverridePayment(PaymentOverride paymentOverride)
        {
            try
            {
                logDebug($"DoOverridePayment called...");
                if (Session == null) return false;
                //RegistrationDataModel model = new RegistrationDataModel();
                //ViewBag.PayType = paymentType;
                //ViewBag.PayType = "CARD";
                var paymentType = "CARD";
                DonorTestInfo donorTestInfo = (DonorTestInfo)Session["donorTestInfo"];
                logDebug($"OverridePayment donorTestInfo {donorTestInfo.DonorTestInfoId}");
                Client client = clientBL.Get(donorTestInfo.ClientId);
                ClientDepartment clientdepartment = clientBL.GetClientDepartment(donorTestInfo.ClientDepartmentId);
                Donor donor = (Donor)Session["PreRegistrationDonorObject"];
                logDebug($"OverridePayment PreRegistrationDonorObject donor {donor.DonorId}, donor.DonorEmail {donor.DonorEmail} {donor.DonorFirstName} {donor.DonorLastName}");

                /// Update scope'd objects 
                // donorTestInfo = (DonorTestInfo)Session["donorTestInfo"];
                donorTestInfo.LastModifiedBy = donor.DonorEmail;
                donorTestInfo.CreatedBy = donor.DonorEmail;


                //donor = (Donor)Session["PreRegistrationDonorObject"];
                logDebug($"Overriding...");
                paymentOverride.PaymentAmount = paymentOverride.PaymentAmount.Replace("USD ", "");
                paymentOverride.SettlementAmount = paymentOverride.SettlementAmount.Replace("USD ", "");
                /// This is where the actual payment was made
                RegistrationDataModel regDataModel = new RegistrationDataModel()
                {
                    Amount = paymentOverride.PaymentAmount
                };

                regDataModel.PaymentData = new PaymentDataModel();

                regDataModel.DepartmentName = clientdepartment.DepartmentName;
                regDataModel.ClientName = client.ClientName;
                regDataModel.Amount = donor.ProgramAmount.ToString(); //donortestinfo.TotalPaymentAmount.ToString();
                regDataModel.PaymentData.Amount = paymentOverride.PaymentAmount.ToString(); //donortestinfo.TotalPaymentAmount.ToString();
                regDataModel.PaymentData.Description = client.ClientName;
                regDataModel.PaymentType = PaymentMethod.Card;
                regDataModel.PaymentData.Email = donor.DonorEmail;
                logDebug($"DoOverridePayment model: model.DepartmentName {regDataModel.DepartmentName}, model.ClientName {regDataModel.ClientName}, model.Amount {donor.ProgramAmount.ToString()}, model.PaymentData.Description {regDataModel.PaymentData.Description}, model.PaymentData.Email {regDataModel.PaymentData.Email}");

                donorTestInfo.TotalPaymentAmount = double.Parse(regDataModel.PaymentData.Amount, System.Globalization.CultureInfo.InvariantCulture);
                donorTestInfo.PaymentDate = paymentOverride.SubmitDate;
                donorTestInfo.PaymentMethodId = PaymentMethod.Card;
                donorTestInfo.PaymentStatus = PaymentStatus.Paid;

                /// Payment has been made - it is now time to commit all our stuff to the database
                try
                {
                    logDebug($"OverRide Burned, committing to database");
                    if (Session == null)
                    {

                        logDebug("Session is null, can't call CommitRegistrationToDatabase, sending to login");

                    }
                    CommitRegistrationToDatabase();
                    //throw new Exception();
                }
                catch (Exception ex)
                {
                    LogAnError(ex);
                    string sErrorFilePath = System.Configuration.ConfigurationManager.AppSettings["ErrorDumpPath"].ToString();
                    if (string.IsNullOrEmpty(sErrorFilePath)) sErrorFilePath = @"d:\logs\";
                    string fullPath = Path.GetFullPath(sErrorFilePath);

                    sErrorFilePath = fullPath
                        .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                        + Path.DirectorySeparatorChar;

                    string ErrInfo = string.Empty;
                    ErrInfo += ex.Message + System.Environment.NewLine;
                    //var stackTrace = new StackTrace(true);
                    //foreach (var r in stackTrace.GetFrames())
                    //{
                    //    ErrInfo += string.Format("Filename: {0} Method: {1} Line: {2} Column: {3}  ",
                    //        r.GetFileName(), r.GetMethod(), r.GetFileLineNumber(),
                    //        r.GetFileColumnNumber()) + System.Environment.NewLine;
                    //}
                    string ErrFileName = sErrorFilePath + string.Format("ErrOInfo-{0:yyyy-MM-dd_hh-mm-ss-tt}.bin", DateTime.Now);

                    using (StreamWriter writer = new StreamWriter(ErrFileName, true))
                    {
                        writer.WriteLine("-----------------------------------------------------------------------------");
                        writer.WriteLine("Date : " + DateTime.Now.ToString());
                        writer.WriteLine();

                        while (ex != null)
                        {
                            writer.WriteLine(ex.GetType().FullName);
                            writer.WriteLine("Message : " + ex.Message);
                            writer.WriteLine("StackTrace : " + ex.StackTrace);

                            ex = ex.InnerException;
                        }
                    }

                    //FileStream fs = new FileStream(ErrFileName, FileMode.CreateNew, FileAccess.Write, FileShare.Write);
                    //fs.Close();
                    //StreamWriter sw = new StreamWriter(ErrFileName, true, Encoding.ASCII);
                    //string NextLine = "This is the appended line.";
                    //sw.Write(NextLine);
                    //sw.Close();


                }



                if (donorTestInfo != null)
                {
                    DonorTestInfo _donorTestInfo = (DonorTestInfo)Session["donorTestInfo"];

                    donorTestInfo.DonorTestInfoId = _donorTestInfo.DonorTestInfoId;

                    /// makePayment sets the payment details and saves them to the database
                    /// The actual payment is above this
                    if (makePayment(donorTestInfo, regDataModel.Amount, paymentType, client.ClientName, paymentOverride))
                    {
                        regDataModel.FirstName = Session["UserName"].ToString();
                        //regDataModel.FirstName = regDataModel.FirstName;
                        regDataModel.Amount = donorTestInfo.TotalPaymentAmount.ToString();
                        regDataModel.DonorEmail = donorTestInfo.CreatedBy;//Session["UserLoginName"].ToString();

                        IUserMailer mail = new RegistrationMailer(regDataModel);
                        // mail.PaymentConformationMail().Send();
                        //if (paymentType.ToUpper() == "CARD")
                        //{
                        mail.CardPaymentConformationMail().Send();
                        //    return Redirect("PaymentConfirmationCard");
                        //}
                        //else
                        //{ // We made this the same as we no longer accept cash payments : Mike Kearl/Jon Jackson
                        //  //mail.PaymentConformationMail().Send();
                        //    mail.CardPaymentConformationMail().Send();
                        //    //return Redirect("PaymentConfirmationCash");
                        //    paymentType = "CARD";
                        //    return Redirect("PaymentConfirmationCard");

                        //}
                    }
                    else
                    {
                        ViewBag.ServerErr = "Sorry an error occurred while processing your request...";
                    }
                }

            }
            catch (Exception ex)
            {
                ViewBag.ServerErr = "Sorry an error occurred while processing your request...";
                logDebug("An error was thrown in PaymentSelection POST!");
                LogAnError(ex);
            }
            // return View(regDataModel);
            return true;
        }


        [HttpPost]
        //[SessionValidateAttribute]
        public ActionResult PaymentSelection(RegistrationDataModel regDataModel, string testInfoId, string paymentType)
        {
            try
            {
                logDebug($"PaymentSelection HttpPost...");
                if (Session == null)
                {
                    logDebug("Session is null, sending to login");
                    return RedirectToAction("Login", "Authentication");
                }
                setDonorBLSession();
                if (Session == null) return RedirectToAction("Login", "Authentication");
                //RegistrationDataModel model = new RegistrationDataModel();
                ViewBag.PayType = paymentType;
                ViewBag.PayType = "CARD";

                DonorTestInfo donorTestInfo = (DonorTestInfo)Session["donorTestInfo"];
                logDebug($"PaymentSelection donorTestInfo {donorTestInfo.DonorTestInfoId}");

                Donor donor = (Donor)Session["PreRegistrationDonorObject"];
                logDebug($"PaymentSelection PreRegistrationDonorObject donor {donor.DonorId}, donor.DonorEmail {donor.DonorEmail} {donor.DonorFirstName} {donor.DonorLastName}");

                /// Update scope'd objects 
                //donorTestInfo = (DonorTestInfo)Session["donorTestInfo"];
                donorTestInfo.LastModifiedBy = donor.DonorEmail;
                donorTestInfo.CreatedBy = donor.DonorEmail;


                //donor = (Donor)Session["PreRegistrationDonorObject"];
                paymentType = "CARD";
                if (paymentType.ToUpper() == "CARD")
                {
                    logDebug($"Authorizing...");

                    /// This is where the actual payment is made
                    string authReturn = AuthorizeNetPayment(regDataModel);
                    if (authReturn != "1")
                    {
                        logDebug($"Error with auth");
                        ViewBag.AuthError = authReturn;
                        ViewBag.Timeout = "1";
                        return View(regDataModel);
                    }
                    logDebug($"Auth Complete");

                }
                donorTestInfo.TotalPaymentAmount = double.Parse(regDataModel.PaymentData.Amount, System.Globalization.CultureInfo.InvariantCulture);

                /// Payment has been made - it is now time to commit all our stuff to the database
                try
                {
                    logDebug($"Payment Made, committing to database");
                    if (Session == null)
                    {

                        logDebug("Session is null, can't call CommitRegistrationToDatabase, sending to login");

                    }
                    //Session["donorTestInfo"] = donorTestInfo;
                    CommitRegistrationToDatabase();
                    //throw new Exception();
                }
                catch (Exception ex)
                {
                    LogAnError(ex);
                    string sErrorFilePath = System.Configuration.ConfigurationManager.AppSettings["ErrorDumpPath"].ToString();
                    if (string.IsNullOrEmpty(sErrorFilePath)) sErrorFilePath = @"d:\logs\";
                    string fullPath = Path.GetFullPath(sErrorFilePath);

                    sErrorFilePath = fullPath
                        .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                        + Path.DirectorySeparatorChar;

                    string ErrInfo = string.Empty;
                    ErrInfo += ex.Message + System.Environment.NewLine;
                    //var stackTrace = new StackTrace(true);
                    //foreach (var r in stackTrace.GetFrames())
                    //{
                    //    ErrInfo += string.Format("Filename: {0} Method: {1} Line: {2} Column: {3}  ",
                    //        r.GetFileName(), r.GetMethod(), r.GetFileLineNumber(),
                    //        r.GetFileColumnNumber()) + System.Environment.NewLine;
                    //}
                    string ErrFileName = sErrorFilePath + string.Format("ErrOInfo-{0:yyyy-MM-dd_hh-mm-ss-tt}.bin", DateTime.Now);

                    using (StreamWriter writer = new StreamWriter(ErrFileName, true))
                    {
                        writer.WriteLine("-----------------------------------------------------------------------------");
                        writer.WriteLine("Date : " + DateTime.Now.ToString());
                        writer.WriteLine();

                        while (ex != null)
                        {
                            writer.WriteLine(ex.GetType().FullName);
                            writer.WriteLine("Message : " + ex.Message);
                            writer.WriteLine("StackTrace : " + ex.StackTrace);

                            ex = ex.InnerException;
                        }
                    }

                    //FileStream fs = new FileStream(ErrFileName, FileMode.CreateNew, FileAccess.Write, FileShare.Write);
                    //fs.Close();
                    //StreamWriter sw = new StreamWriter(ErrFileName, true, Encoding.ASCII);
                    //string NextLine = "This is the appended line.";
                    //sw.Write(NextLine);
                    //sw.Close();


                }



                if (donorTestInfo != null)
                {
                    /// makePayment sets the payment details and saves them to the database
                    /// The actual payment is above this
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

            }
            catch (Exception ex)
            {
                ViewBag.ServerErr = "Sorry an error occurred while processing your request...";
                logDebug("An error was thrown in PaymentSelection POST!");
                LogAnError(ex);
            }
            return View(regDataModel);

        }

        [HttpGet]
        [SessionValidateAttribute]
        public ActionResult PaymentConfirmationCash()
        {
            setDonorBLSession();
            GetPaymentMethod();
            return View();
        }

        /// <summary>
        /// This is the final step - here is where we update the records in the database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SessionValidateAttribute]
        public ActionResult PaymentConfirmationCard()
        {
            string testInfoId = Session["DonorTestinfoId"].ToString();
            setDonorBLSession();
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
            GetPaymentMethod();

            return View();
        }


        [HttpPost]
        public JsonResult SendVerifyEmail(AcccountExistsJSONModel model)
        {
            RegistrationDataModel registrationDataModel = new RegistrationDataModel();
            registrationDataModel.EmailID = model.userName;
            registrationDataModel.DonorEmail = model.userName;
            RandomStringGenerator rsg = new RandomStringGenerator(true, false, true, false);
            string verificationCode = rsg.Generate(6, 8);
            if ((string)Session["verificationCode"] == null)
            {
                Session["verificationCode"] = verificationCode;
                registrationDataModel.TemporaryPassword = verificationCode;
            }
            else
            {
                verificationCode = (string)Session["verificationCode"];
            }

            Session["verifiedemail"] = false;

            try
            {
                // if not production, return it's sent and add code to json msg
                if (MvcApplication.Dev == true)
                {
                    return Json(new { Success = 1, ErrorMsg = "Email Sent.", Code = verificationCode });
                }


                IUserMailer mail = new RegistrationMailer(registrationDataModel);
                mail.VerifyEmail().Send();
                logDebug($"Verification Email sent to {registrationDataModel.DonorEmail}");
                return Json(new { Success = 1, ErrorMsg = "Email Sent." });

            }
            catch (Exception ex)
            {
                MvcApplication.LogError(ex);
                return Json(new { Success = -1, ErrorMsg = "Unable to send email." });

            }
        }

        [HttpPost]
        public JsonResult CheckVerifyEmail(AcccountExistsJSONModel model)
        {
            try
            {
                Session["verifiedemail"] = false;

                if (Session["verificationCode"] == null)
                {
                    return Json(new { Success = 0, ErrorMsg = "" });
                }

                string verificationCode = (string)Session["verificationCode"];
                if (model.verificationCode.Equals(verificationCode, StringComparison.CurrentCultureIgnoreCase))
                {
                    Session["verifiedemail"] = true;
                    return Json(new { Success = 1, ErrorMsg = "Success, verified." });
                }
                else
                {
                    return Json(new { Success = -1, ErrorMsg = "Incorrect code, please verify." });
                }
            }
            catch (Exception ex)
            {
                MvcApplication.LogError(ex);
                return Json(new { Success = -1, ErrorMsg = "Unable to process" });

            }

        }

        [HttpPost]
        public JsonResult accountavailableForProgram(AcccountExistsJSONModel model)
        {

            if (!backendLogic.IsValidEmail(model.userName))
            {
                return Json(new { Success = -1, ErrorMsg = "Invalid Email" });
            }

            if (string.IsNullOrEmpty(model.userName))
            {
                return Json(new { Success = -1, ErrorMsg = "Username required" });
            }

            Donor donor = (Donor)Session["PreRegistrationDonorObject"];

            var department_name = donor.ClientDepartment.DepartmentName;

            Donor verifyDonor = donorBL.GetByLoginAndProgram(model.userName, department_name);

            if (verifyDonor == null)
            {
                return Json(new { Success = 1, ErrorMsg = "" });

            }
            else
            {
                return Json(new { Success = -1, ErrorMsg = "Account Already Exists, please login" });

            }

            //if (!string.IsNullOrEmpty(model.userName) && !string.IsNullOrEmpty(model.programId))
            //{
            //    Donor donor = (Donor)Session["PreRegistrationDonorObject"];

            //    // See if this donor exists with this pid
            //    Donor verifyDonor = donorBL.GetByPIDAndEmail(model.userName, model.programId);

            //    if (verifyDonor == null)
            //    {
            //        return Json(new { Success = 1, ErrorMsg = "" });

            //    }
            //    else
            //    {
            //        return Json(new { Success = -1, ErrorMsg = "Account Already Exists" });

            //    }
            //}
            //else
            //{

            //    if (string.IsNullOrEmpty(model.programId))
            //    {
            //        return Json(new { Success = -1, ErrorMsg = "Program id required" });
            //    }
            //    return Json(new { Success = 0, ErrorMsg = "" });
            //}

        }

        //[HttpPost]
        //public ActionResult LoginDonor(FormCollection collection)
        //{


        //    Donor donor = (Donor)Session["PreRegistrationDonorObject"];

        //    Session["SSNRequired"] = true;
        //    Session["PidTypeValues"] = donor.PidTypeValues;
        //    Session["PreRegistrationDonorObject"] = donor;
        //    //return RedirectToAction("ProfileUpdate2", "Registration", donor);


        //    //Donor donor = (Donor)Session["PreRegistrationDonorObject"];
        //    ViewBag.ServerErr = "";
        //    // See if this donor exists with this pid
        //    var programid = collection["programid"];
        //    var email = collection["userName"];
        //    var password = collection["password"];

        //    if (!backendLogic.IsValidEmail(email))
        //    {
        //        ViewBag.ServerErr = "Email is not valid.";
        //        RedirectToAction("CreateAccount", "Registration", donor);
        //    }

        //    Tuple<bool, int, int> result = userAuthentication.AuthDonorByUsernamePasswordAndDepartmentName(email, password, programid);

        //    // wrong password
        //    if (result.Item1 == false && result.Item2 == 0)
        //    {

        //    }

        //    // does this account exist for this dept?
        //    if (result.Item1 == false && result.Item2==0 )
        //    {
        //        // didn't autenticate and there's no user id here
        //        // create this donor account
        //        Session["ActivateAccount"] = true;
        //        // Add this pid type for this integration partner
        //        if (Session["IntegrationClientId"] == null || Session["IntegrationClientDeptId"] == null) return RedirectToAction("Login", "Authentication");
        //        var client_id = (int)Session["IntegrationClientId"];
        //        var client_department_id = (int)Session["IntegrationClientDeptId"];
        //        IntegrationPartner _ip;

        //        _ip = backendLogic.GetIntegrationPartnerbyClientidDeptId(client_id, client_department_id);

        //        donor.PidTypeValues.Add(new PIDTypeValue() { PIDValue = programid, PIDType = _ip.backend_integration_partners_pidtype, required = true, Err = "" });
        //        Session["PidTypeValues"] = donor.PidTypeValues;
        //        return RedirectToAction("ProfileUpdate2", donor);

        //    }
        //    else
        //    {


        //    }


        //    //return RedirectToAction("ProfileUpdate2", donor);

        //    Donor verifyDonor = donorBL.GetByPIDAndEmail(email, programid);
        //    Session["ActivateAccount"] = false;
        //    if (verifyDonor == null)
        //    {

        //    }
        //    else
        //    {
        //        ViewBag.ServerErr("This account already exists!");
        //    }
        //    return View();
        //}


        [HttpPost]
        public JsonResult LoginRegistrant(LoginRegistrantJSONModel model)
        {

            if (!backendLogic.IsValidEmail(model.userName))
            {
                return Json(new { Success = -1, ErrorMsg = "Invalid Email" });
            }

            if (string.IsNullOrEmpty(model.userName))
            {
                return Json(new { Success = -1, ErrorMsg = "Username required" });
            }

            if (string.IsNullOrEmpty(model.password))
            {
                return Json(new { Success = -1, ErrorMsg = "Password required" });
            }

            Donor donor = (Donor)Session["PreRegistrationDonorObject"];

            var department_name = donor.ClientDepartment.DepartmentName;
            var username = model.userName;
            var password = model.password;
            // authenticate the donor using GetUserByUsernamePasswordAndDepartmentName

            Tuple<bool, int, int> result = userAuthentication.AuthDonorByUsernamePasswordAndDepartmentName(username, password, department_name);

            // var t = UserAuthentication.Encrypt(department_name, true);


            if (result.Item1 == true)
            {
                return Json(new { Success = 1, ErrorMsg = "" });

            }
            else
            {
                return Json(new { Success = 0, ErrorMsg = "Authentication failed. Check login and password" });
            }
        }

        [HttpPost]
        public JsonResult validEmail(AcccountExistsJSONModel model)
        {

            if (!backendLogic.IsValidEmail(model.userName))
            {
                return Json(new { Success = -1, ErrorMsg = "Invalid Email" });
            }


            Donor donor = (Donor)Session["PreRegistrationDonorObject"];

            var department_name = donor.ClientDepartment.DepartmentName;
            var username = model.userName;

            // authenticate the donor using GetUserByUsernamePasswordAndDepartmentName

            Tuple<bool, int, int> result = userAuthentication.AuthDonorByUsernamePasswordAndDepartmentName(username, "", department_name);

            if (result.Item2 == 0) return Json(new { Success = 0, ErrorMsg = "" });

            return Json(new { Success = 1, ErrorMsg = "Account Exists, please login." });

        }

        [HttpPost]
        public JsonResult accountavailable(AcccountExistsJSONModel model)
        {

            if (!backendLogic.IsValidEmail(model.userName))
            {
                return Json(new { Success = -1, ErrorMsg = "Invalid Email" });
            }

            if (string.IsNullOrEmpty(model.userName))
            {
                return Json(new { Success = -1, ErrorMsg = "Username required" });
            }


            if (!string.IsNullOrEmpty(model.userName) && !string.IsNullOrEmpty(model.programId))
            {
                Donor donor = (Donor)Session["PreRegistrationDonorObject"];

                // See if this donor exists with this pid
                Donor verifyDonor = donorBL.GetByPIDAndEmail(model.userName, model.programId);

                if (verifyDonor == null)
                {
                    return Json(new { Success = 1, ErrorMsg = "" });

                }
                else
                {
                    return Json(new { Success = -1, ErrorMsg = "Account Already Exists" });

                }
            }
            else
            {

                if (string.IsNullOrEmpty(model.programId))
                {
                    return Json(new { Success = -1, ErrorMsg = "Program id required" });
                }
                return Json(new { Success = 0, ErrorMsg = "" });
            }

        }



        /// <summary>
        /// Setup donor model for CreateAccount
        /// </summary>
        /// <returns></returns>
        //private DonorProfileDataModel CreateAccountModel()
        //{

        //}

        /// <summary>
        /// Create an account for Donors for integrations requiring user accounts
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateAccount(Donor donor)
        {
            setDonorBLSession();
            ViewBag.ServerErr = "";
            DonorProfileDataModel model = new DonorProfileDataModel();
            if (Session["DonorProfileDataModel"] != null)
            {
                model = (DonorProfileDataModel)Session["DonorProfileDataModel"];
                return View("ProfileUpdate2", model);

            }
            try
            {
                Session["verifiedemail"] = false;
                if (donor != null)
                {
                    model.EmailID = donor.DonorEmail;
                    model.SSN = donor.DonorSSN;
                    model.FirstName = donor.DonorFirstName;
                    model.MiddleInitial = donor.DonorMI;
                    model.LastName = donor.DonorLastName;
                    model.Suffix = donor.DonorSuffix;
                    if (donor.DonorDateOfBirth.ToString("MM/dd/yyyy") != "01/01/0001")
                    {
                        model.DonorDOBMonth = donor.DonorDateOfBirth.Month.ToString();
                        model.DonorDOBDate = donor.DonorDateOfBirth.Day.ToString();
                        model.DonorDOBYear = donor.DonorDateOfBirth.Year.ToString();
                    }
                    else
                    {
                        model.DonorDOBDate = "";
                        model.DonorDOBMonth = "";
                        model.DonorDOBYear = "";
                    }
                    model.Address1 = donor.DonorAddress1;
                    model.Address2 = donor.DonorAddress2;
                    model.City = donor.DonorCity;
                    model.State = donor.DonorState;
                    model.ZipCode = donor.DonorZip;
                    model.Phone1 = donor.DonorPhone1;
                    model.Phone2 = donor.DonorPhone2;
                    model.Gender = donor.DonorGender;

                    bool SSNRequired = false;
                    if (Session["SSNRequired"] != null) SSNRequired = (bool)Session["SSNRequired"];
                    if (SSNRequired == true)
                    {
                        model.PIDTypeValues.Where(x => x.PIDType == (int)PidTypes.SSN).FirstOrDefault().required = true;
                    }

                    //if (Session["IntegrationClientId"] == null || Session["IntegrationClientDeptId"] == null) return RedirectToAction("Login", "Authentication");
                    if (Session["IntegrationClientId"] == null) return RedirectToAction("Login", "Authentication");
                    var client_id = (int)Session["IntegrationClientId"];
                    //var client_department_id = (int)Session["IntegrationClientDeptId"];
                    // get the instructions for this integration client
                    var _ip = backendLogic.GetIntegrationPartnerbyClientidDeptId(client_id);

                    ViewBag.Instructions = _ip.html_instructions;

                    Session["CreateAccountDonorProfileDataModel"] = model;
                    //return View("CreateAccount", model);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                //redirect to 404
            }
            return RedirectToAction("Login", "Authentication");
        }



        /// <summary>
        /// Setup user / donor account and continue to confirmation
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateAccount(FormCollection collection)
        {
            bool _verifiedemail = false;
            var programid = collection["programid"];
            var email = collection["userName"];
            var password = collection["password"];

            DonorProfileDataModel model = new DonorProfileDataModel();
            if (Session["CreateAccountDonorProfileDataModel"] == null)
            {
                return RedirectToAction("Login", "Authentication");
            }

            model = (DonorProfileDataModel)Session["CreateAccountDonorProfileDataModel"];

            if (Session["verifiedemail"] != null)
            {
                _verifiedemail = (bool)Session["verifiedemail"];
            }

            if (_verifiedemail == false && string.IsNullOrEmpty(password))
            {
                ViewBag.ServerErr = "Email is not verified.";
                return View(model);
            }
            Donor donor = (Donor)Session["PreRegistrationDonorObject"];
            ViewBag.ServerErr = "";
            // See if this donor exists with this pid

            if (!backendLogic.IsValidEmail(email))
            {
                ViewBag.ServerErr = "Email is not valid.";
                return View(model);
            }
            var department_name = donor.ClientDepartment.DepartmentName;
            Tuple<bool, int, int> result = userAuthentication.AuthDonorByUsernamePasswordAndDepartmentName(email, password, department_name);
            Session["ActivateAccount"] = false;

            // wrong password
            if (result.Item1 == false && result.Item2 != 0)
            {
                ViewBag.ServerErr = "Authentication failed. Check username and password.";
                ViewBag.RetryLogin = true;
                return View(model);
            }


            var client_id = (int)Session["IntegrationClientId"];
            var client_department_id = (int)Session["IntegrationClientDeptId"];
            IntegrationPartner _ip;

            _ip = backendLogic.GetIntegrationPartnerbyClientidDeptId(client_id);

            if (result.Item1 == false && result.Item2 == 0)
            {

                // What if we have a user but no donor record? maybe this -> ((result.Item2 == 0) || (result.Item2 != 0 && result.Item3==0))) but have to check donor creation logic to see if we need to create donor
                // and associate it with an existing user


                // this user doesn't exist, we will create it:
                Session["ActivateAccount"] = true;

                // donor.PidTypeValues.Add(new PIDTypeValue() { PIDValue = programid, PIDType = _ip.backend_integration_partners_pidtype, required = true, Err = "" });

                Session["PidTypeValues"] = donor.PidTypeValues;
                return RedirectToAction("ProfileUpdate2", donor);
            }

            if (result.Item1 == true && result.Item2 != 0)
            {
                // we got a password and it matches the donor for the department
                Session["ActivateAccount"] = false;

                if (result.Item3 == 0)
                {
                    ViewBag.ServerErr = "Donor not found - error - please contact SurScan.";
                    return View(model);
                }
                // load the donor

                Donor _dbDonor = donorBL.Get(donor.DonorId, "Web");
                logDebug($"Pulled {donor.DonorId} from database for login to register");
                Session["PreRegistrationDonorObject"] = _dbDonor;
                Session["PidTypeValues"] = donor.PidTypeValues;
                return RedirectToAction("ProfileUpdate2", donor);

            }

            return View();

        }

        /// <summary>
        /// Confirm account and continue with registration
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [SessionValidateAttribute]
        public ActionResult VerifyAccount()
        {
            return View();
        }


        /// <summary>
        /// Profile Update - donors are sent here when registering after activation. Why it is here and not in authentication is beyond me.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SessionValidateAttribute]
        public ActionResult ProfileUpdate(Donor donor)
        {
            try
            {
                setDonorBLSession();
                //int donorID = Convert.ToInt32(Session["DonorId"].ToString());

                var donorPreRegDonorObject = (Donor)Session["PreRegistrationDonorObject"];

                if (donorPreRegDonorObject != null)
                {
                    DonorProfileDataModel model = new DonorProfileDataModel();





                    if (Session["PidTypeValues"] != null)
                    {
                        model.PIDTypeValues = (List<PIDTypeValue>)Session["PidTypeValues"];

                    }
                    if (donor.DonorInitialDepartmentId != null)
                    {
                        ClientDepartment clientDepartment = clientBL.GetClientDepartment((int)donor.DonorInitialDepartmentId);
                        // Open up integration PIDTypes
                        if (clientDepartment.integrationPartner == true)
                        {
                            //var _ip = backendLogic.GetIntegrationPartnerbyClientidDeptId((int)donor.DonorInitialClientId, (int)donor.DonorInitialDepartmentId);
                            var _ip = backendLogic.GetIntegrationPartnerbyClientidDeptId((int)donor.DonorInitialClientId);
                            var _ipPidType = _ip.backend_integration_partners_pidtype;
                            ViewBag.IntegrationPidType = _ipPidType;

                            if (!model.PIDTypeValues.Exists(pt => pt.PIDType == _ipPidType))
                            {
                                model.PIDTypeValues.Add(new PIDTypeValue()
                                {
                                    PIDType = _ipPidType,
                                    PIDValue = ""
                                });
                            }
                        }
                    }


                    bool SSNRequired = (bool)Session["SSNRequired"];
                    if (SSNRequired == true)
                    {
                        model.PIDTypeValues.Where(x => x.PIDType == (int)PidTypes.SSN).FirstOrDefault().required = true;
                    }





                    model.EmailID = donorPreRegDonorObject.DonorEmail;
                    model.SSN = donorPreRegDonorObject.DonorSSN;
                    model.FirstName = donorPreRegDonorObject.DonorFirstName;
                    model.MiddleInitial = donorPreRegDonorObject.DonorMI;
                    model.LastName = donorPreRegDonorObject.DonorLastName;
                    model.Suffix = donorPreRegDonorObject.DonorSuffix;
                    if (donorPreRegDonorObject.DonorDateOfBirth.ToString("MM/dd/yyyy") != "01/01/0001")
                    {
                        model.DonorDOBMonth = donorPreRegDonorObject.DonorDateOfBirth.Month.ToString();
                        model.DonorDOBDate = donorPreRegDonorObject.DonorDateOfBirth.Day.ToString();
                        model.DonorDOBYear = donorPreRegDonorObject.DonorDateOfBirth.Year.ToString();
                    }
                    else
                    {
                        model.DonorDOBDate = "";
                        model.DonorDOBMonth = "";
                        model.DonorDOBYear = "";
                    }
                    model.Address1 = donorPreRegDonorObject.DonorAddress1;
                    model.Address2 = donorPreRegDonorObject.DonorAddress2;
                    model.City = donorPreRegDonorObject.DonorCity;
                    model.State = donorPreRegDonorObject.DonorState;
                    model.ZipCode = donorPreRegDonorObject.DonorZip;
                    model.Phone1 = donorPreRegDonorObject.DonorPhone1;
                    model.Phone2 = donorPreRegDonorObject.DonorPhone2;
                    model.Gender = donorPreRegDonorObject.DonorGender;

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                //redirect to 404
            }
            return View();
        }


        /// <summary>
        /// Profile Update - donors are sent here when registering after activation. Why it is here and not in authentication is beyond me.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>




        //if (donor.PidTypeValues.Count > 0)
        //{
        //    // we have values to carry over.
        //    foreach (PIDTypeValue pv in donor.PidTypeValues)
        //    {

        //        if (!model.PIDTypeValues.Exists(_pv => _pv.PIDType == pv.PIDType))
        //        {
        //            model.PIDTypeValues.Add(pv);
        //        }
        //        else
        //        {
        //            var _p = model.PIDTypeValues.Where(_pv => _pv.PIDType == pv.PIDType).First();
        //            var _i = model.PIDTypeValues.IndexOf(_p);
        //            model.PIDTypeValues[_i] = pv;
        //        }
        //    }
        //}

        //donor.PidTypeValues = new List<PIDTypeValue>();
        //foreach (PIDTypeValue pv in model.PIDTypeValues)
        //{
        //    donor.PidTypeValues.Add(pv);
        //}

        [HttpPost]
        //[SessionValidateAttribute]
        public ActionResult ProfileUpdate(DonorProfileDataModel model)
        {
            setDonorBLSession();

            if (string.IsNullOrEmpty(model.SSN)) model.SSN = string.Empty;

            if (model.PIDTypeValues.Exists(x => x.PIDType == (int)PidTypes.SSN))
            {
                if (!string.IsNullOrEmpty(model.PIDTypeValues.Where(x => x.PIDType == (int)PidTypes.SSN).FirstOrDefault().PIDValue))
                {
                    model.SSN = model.PIDTypeValues.Where(x => x.PIDType == (int)PidTypes.SSN).FirstOrDefault().PIDValue.Trim();
                }
            }


            //var donor_id = Convert.ToInt32(model.DonorId);

            ////var donorSSNDetails = donorBL.GetBySSN(model.SSN.Trim(), "Web");
            //var donorSSNDetails = donorBL.Get(donor_id, "Web");

            //if (donorSSNDetails == null)
            //{
            //    Donor donor = (Donor)Session["PreRegistrationDonorObject"];

            //    if (donor != null)
            //    {

            //        donor.DonorSSN = model.SSN;
            //        donor.DonorFirstName = model.FirstName;
            //        donor.DonorMI = model.MiddleInitial;
            //        donor.DonorLastName = model.LastName;
            //        donor.DonorSuffix = model.Suffix;
            //        DateTime DOB = Convert.ToDateTime(model.DonorDOBMonth.ToString() + '-' + model.DonorDOBDate.ToString() + '-' + model.DonorDOBYear.ToString());
            //        if (Convert.ToDateTime(DOB.ToString()) < DateTime.Now)
            //        {
            //            donor.DonorDateOfBirth = Convert.ToDateTime(DOB.ToString());
            //        }
            //        else
            //        {
            //            ViewBag.DOBERR = "Invalid D.O.B";
            //            return View(model);
            //        }
            //        donor.DonorAddress1 = model.Address1;
            //        donor.DonorAddress2 = model.Address2;
            //        donor.DonorCity = model.City;

            //        if (model.State != null && model.State != "")
            //        {
            //            donor.DonorState = model.State.Substring(0, 2);
            //        }
            //        else
            //        {
            //            donor.DonorState = model.State;
            //        }

            //        donor.DonorZip = model.ZipCode;
            //        donor.DonorPhone1 = model.Phone1;
            //        donor.DonorPhone2 = model.Phone2;

            //        donor.LastModifiedBy = donor.DonorEmail;
            //        donor.DonorGender = model.Gender;





            //        /// TODO - Here - we could check First Name, last name, SSN, and DOB and see if there's an existing DONOR / USER
            //        /// If so - we'd stick that into Session
            //        /// Then, when registration is compleate, we'd add an update to to the profile / user in BL & DAO
            //        /// Then use that donor id, user id, client id, dept and create test info records.
            //        /// See the finalize function for next steps



            //        return View("ProgramSelection");


            //    }
            //    //return View(model);
            //    return View("ProfileUpdate", model);

            //}

            if (!string.IsNullOrEmpty(model.DonorId))
            {
                // editing a donor
                var donor_id = Convert.ToInt32(model.DonorId);
                Donor donor = donorBL.Get(donor_id, "Web");

                if (donor != null)
                {
                    // carry over values and save

                    if (!model.EmailID.Equals(donor.DonorEmail, StringComparison.InvariantCultureIgnoreCase))
                    {
                        donor.DonorEmail = model.EmailID;
                    }
                    donor.DonorSSN = model.SSN;
                    donor.DonorFirstName = model.FirstName;
                    donor.DonorMI = model.MiddleInitial;
                    donor.DonorLastName = model.LastName;
                    donor.DonorSuffix = model.Suffix;
                    DateTime DOB = Convert.ToDateTime(model.DonorDOBMonth.ToString() + '-' + model.DonorDOBDate.ToString() + '-' + model.DonorDOBYear.ToString());
                    if (Convert.ToDateTime(DOB.ToString()) < DateTime.Now)
                    {
                        donor.DonorDateOfBirth = Convert.ToDateTime(DOB.ToString());
                    }
                    else
                    {
                        ViewBag.DOBERR = "Invalid D.O.B";
                        return View(model);
                    }
                    donor.DonorAddress1 = model.Address1;
                    donor.DonorAddress2 = model.Address2;
                    donor.DonorCity = model.City;

                    if (model.State != null && model.State != "")
                    {
                        donor.DonorState = model.State.Substring(0, 2);
                    }
                    else
                    {
                        donor.DonorState = model.State;
                    }

                    donor.DonorZip = model.ZipCode;
                    donor.DonorPhone1 = model.Phone1;
                    donor.DonorPhone2 = model.Phone2;

                    donor.LastModifiedBy = donor.DonorEmail;
                    donor.DonorGender = model.Gender;



                    //if (model.PIDTypeValues.Count > 0)
                    //{
                    //    foreach (PIDTypeValue pv in model.PIDTypeValues)
                    //    {

                    //        if (!donor.PidTypeValues.Exists(_pv => _pv.PIDType == pv.PIDType))
                    //        {
                    //            model.PIDTypeValues.Add(pv);
                    //        }
                    //        else
                    //        {
                    //            var _p = model.PIDTypeValues.Where(_pv => _pv.PIDType == pv.PIDType).First();
                    //            var _i = model.PIDTypeValues.IndexOf(_p);
                    //            model.PIDTypeValues[_i] = pv;
                    //        }
                    //    }
                    //}
                    //if (donor.PidTypeValues.Count > 0)
                    //{
                    //    // we have values to carry over.
                    //    foreach (PIDTypeValue pv in donor.PidTypeValues)
                    //    {

                    //        if (!model.PIDTypeValues.Exists(_pv => _pv.PIDType == pv.PIDType))
                    //        {
                    //            model.PIDTypeValues.Add(pv);
                    //        }
                    //        else
                    //        {
                    //            var _p = model.PIDTypeValues.Where(_pv => _pv.PIDType == pv.PIDType).First();
                    //            var _i = model.PIDTypeValues.IndexOf(_p);
                    //            model.PIDTypeValues[_i] = pv;
                    //        }
                    //    }

                    //    // get rid of empty pids
                    model.PIDTypeValues = model.PIDTypeValues.Where(_p => !string.IsNullOrEmpty(_p.PIDValue)).ToList();
                    //}

                    donor.PidTypeValues = new List<PIDTypeValue>();
                    foreach (PIDTypeValue pv in model.PIDTypeValues)
                    {
                        donor.PidTypeValues.Add(pv);
                    }

                    Session["PreRegistrationDonorObject"] = donor;

                    donorBL.Save(donor);
                    donorBL.UpdateDonorPids(donor);
                    Session["PidTypeValues"] = donor.PidTypeValues;
                    return RedirectToAction("ProfileUpdate", "Registration", donor);


                    //
                    //RegistrationDataModel model2 = new RegistrationDataModel();

                    //model2.FirstName = donor.DonorFirstName;
                    //Session["UserName"] = donor.DonorFirstName;

                    //model2.LastName = donor.DonorLastName;
                    //model2.Amount = donor.ProgramAmount.ToString("N2");
                    //model2.DonorEmail = donor.DonorEmail;

                    //if (donor.DonorSSN.Length == 11)
                    //{
                    //    model2.DonorSSN = "XXX-XX-" + donor.DonorSSN.Substring(7);
                    //}

                    //model2.DonorDOB = donor.DonorDateOfBirth.ToString("MM/dd/yyyy");
                    //model2.DonorCity = donor.DonorCity;
                    //model2.DonorState = donor.DonorState;
                    //model2.DonorZipCode = donor.DonorZip;

                    ////ClientDepartment clientDepartment = clientBL.GetClientDepartment(clientDepartmentId);
                    ////if (donor.ClientDepartment == null)
                    ////{
                    ////    if (Session["IntegrationClientDeptId"] != null)
                    ////    {
                    ////        donor.ClientDepartment = clientBL.GetClientDepartment((int)Session["IntegrationClientDeptId"]);
                    ////    }
                    ////}
                    //Client client = clientBL.Get(donor.ClientDepartment.ClientId);

                    //model2.ClientCode = client.ClientCode;
                    //model2.ClientName = client.ClientName;
                    //model2.DepartmentName = donor.ClientDepartment.DepartmentName;

                    //Session["PreRegistrationDonorObject"] = donor;
                    //Session["PreRegistrationDonorData"] = model;

                    //model2.TPAEmail = ConfigurationManager.AppSettings["TPAProgramToMailAddress"].ToString().Trim();

                    //IUserMailer mail = new RegistrationMailer(model2);
                    ////mail.DonorProgramRegsitrationMail().Send();
                    //if (!string.IsNullOrEmpty(client.ClientEmail.ToString()))
                    //{
                    //    model2.ClientEmail = client.ClientEmail;
                    //    //mail.ClientProgramRegsitrationMail().Send();
                    //}
                    ////
                    ////
                    ////
                    //// Make sure at least one of the PIDs is not null
                    //bool GoodPids = true;
                    //bool AllRequiredPidsProvided = model.PIDTypeValues.Where(x => x.required == true && !(string.IsNullOrEmpty(x.PIDValue))).Count() == model.PIDTypeValues.Where(x => x.required == true).Count();

                    //bool PidProvided = model.PIDTypeValues.Where(x => !(string.IsNullOrEmpty(x.PIDValue))).Count() > 0;

                    //if (AllRequiredPidsProvided == false)
                    //{
                    //    GoodPids = false;
                    //    // we're missing a required pid
                    //    foreach (PIDTypeValue pv in model.PIDTypeValues)
                    //    {
                    //        if (pv.required == true && string.IsNullOrEmpty(pv.PIDValue))
                    //        {
                    //            pv.Err = "This is required for this program.";
                    //            model.PidErrorMesages.Add("Please complete all required personal identification fields");
                    //        }
                    //    }
                    //}
                    //if (PidProvided == false)
                    //{
                    //    GoodPids = false;
                    //    model.PidErrorMesages.Add("Please complete at at least one personal identification field.");
                    //}

                    //Session["DonorProfileDataModel"] = model;
                    //if (GoodPids == false)
                    //{
                    //    return RedirectToAction("ProfileUpdate2", donor);

                    //}
                    //Session["PIDTypeValues"] = model.PIDTypeValues;
                    //Session["PidErrorMesages"] = model.PidErrorMesages;
                    //return RedirectToAction("ProfileReview", model);

                    ////return RedirectToAction("ProgramSelection");

                }
            }

            //ViewBag.SSNErr = "SSN already exist";
            ////return View(model);
            //return View("ProfileUpdate", model);
            return RedirectToAction("Login", "Authentication");
        }




        #endregion

        #region handoff
        [HttpGet]
        [Route("Registration/handoff/{id}/{dto}", Name = "handoff")]
        public ActionResult handoff(string id, string dto)
        {
            /// we take the ID [partner key], pull the encryption, 
            /// and take the DTO string and desrialize to the object
            /// if it fails 
            logDebug($"handoff called");
            /// 
            try
            {
                if (Session != null)
                {
                    logDebug($"Session exists, handoff called, abandoning session");

                    //Session.Abandon();
                }
                Session["DonorProfileDataModel"] = null;
                logDebug($"handoff called. id = {id}");

                if (!string.IsNullOrEmpty(id))
                {
                    var _partner_client_code = BackendStatics.Base64URLDecode(id);
                    logDebug($"Decoded partner code: {_partner_client_code}");
                    IntegrationPartner integrationPartner = d.GetIntegrationPartnerByPartnerClientCode(new ParamGetIntegrationPartnerByPartnerClientCode() { partner_client_code = _partner_client_code });
                    if (integrationPartner.backend_integration_partner_id > 0)
                    {
                        //dto = dto.PadRight(dto.Length + (4 - dto.Length % 4) % 4, '='); // https://stackoverflow.com/questions/1228701/code-for-decoding-encoding-a-modified-base64-url
                        //var base64EncodedBytes = Convert.FromBase64String(dto);
                        //var _dto = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
                        var _dto = BackendStatics.Base64URLDecode(dto);

                        string _json = UserAuthentication.DecryptWithKey(_dto, integrationPartner.partner_crypto);
                        IntegrationHandOffDTO integrationHandOffDTO = JsonConvert.DeserializeObject<IntegrationHandOffDTO>(_json);
                        if (!backendLogic.IsValidEmail(integrationHandOffDTO.donor_email))
                        {
                            return Json(new { Success = -1, ErrorMsg = "HandOff failed [Invalid Email]" });
                        }
                        string _auth = UserAuthentication.EncryptWithKey(integrationHandOffDTO.partner_client_code + integrationHandOffDTO.donor_email, integrationPartner.partner_key);
                        if (integrationHandOffDTO.auth.ToString() == _auth)
                        {
                            logDebug($"Auth valid");
                            // get the client

                            List<IntegrationPartnerClient> _clients = d.GetIntegrationPartnerClientsByPartnerKey(new ParamGetIntegrationPartnerClientsByPartnerKey() { partner_key = integrationPartner.partner_key });

                            if (_clients.Exists(_c => _c.partner_client_code.Equals(integrationHandOffDTO.partner_client_code)))
                            {
                                logDebug($"Found client");
                                Donor donor = new Donor();
                                IntegrationPartnerClient _client = _clients.Where(_c => _c.partner_client_code.Equals(integrationHandOffDTO.partner_client_code)).First();
                                // IntegrationPartner ip = d.GetIntegrationPartnerByPartnerKey(new ParamGetIntegrationPartnerByPartnerKey() { partner_key = integrationPartner.partner_key });
                                //var dept = clientBL.GetClientDepartment(_client.client_department_id);
                                var __client = clientBL.Get(_client.client_id);
                                Session["IntegrationClientId"] = _client.client_id;
                                // Key matches ID

                                // Build the registration process and redirect to profileupdate2
                                //setDonorBLSession();
                                ////Client client = clientBL.Get(model.ClientCode.Trim());
                                donor.DonorInitialClientId = _client.client_id;
                                donor.DonorFirstName = "";
                                donor.DonorLastName = "";
                                donor.TemporaryPassword = integrationHandOffDTO.auth;
                                donor.DonorEmail = integrationHandOffDTO.donor_email;
                                donor.DonorRegistrationStatusValue = DonorRegistrationStatus.PreRegistration;
                                donor.DonorFirstName = integrationHandOffDTO.donor_first_name;
                                donor.DonorLastName = integrationHandOffDTO.donor_last_name;
                                donor.DonorMI = integrationHandOffDTO.donor_mi;
                                donor.DonorCity = integrationHandOffDTO.donor_city;
                                donor.DonorState = integrationHandOffDTO.donor_state;
                                donor.DonorZip = integrationHandOffDTO.donor_zip;
                                donor.DonorAddress1 = integrationHandOffDTO.donor_address_1;
                                donor.DonorAddress2 = integrationHandOffDTO.donor_address_2;
                                donor.DonorPhone1 = String.Format("{0:(###) ###-####}", String.Concat(integrationHandOffDTO.donor_phone_1.Where(Char.IsDigit)));
                                donor.DonorPhone2 = String.Format("{0:(###) ###-####}", String.Concat(integrationHandOffDTO.donor_phone_2.Where(Char.IsDigit)));

                                logDebug($"Adding PIDTypeValue: PIDType: {integrationPartner.backend_integration_partners_pidtype} Value: {integrationHandOffDTO.integration_id.ToString()}");
                                donor.PidTypeValues = new List<PIDTypeValue>();
                                donor.PidTypeValues.Add(new PIDTypeValue() { Err = "", isReadOnly = true, PIDValue = integrationHandOffDTO.integration_id.ToString(), required = true, PIDType = integrationPartner.backend_integration_partners_pidtype });
                                Session["PIDTypeValues"] = donor.PidTypeValues;
                                //Session["IntegrationClientDeptId"] = _client.client_department_id;
                                Session["PreRegistrationClientCode"] = __client.ClientCode;
                                Session["PreRegistrationDonorObject"] = donor;
                                Session["AlreadyCommitted"] = false;
                                Session["SSNRequired"] = true;
                                Session["Registration_Client_Has_Recordkeeping"] = false; // dept.ClientDeptTestCategories.Select(t => t.TestCategoryId == SurPath.Enum.TestCategories.RC).Where(r => r == true).ToList().Count() > 0;

                                DonorTestInfo donorTestInfo;
                                //donorTestInfo = CreatePreRegistrationDonorTestObject(dept);
                                //Session["donorTestInfo"] = donorTestInfo;


                                logDebug($"handing off to Program Selection");
                                Session["ValidHandoff"] = true;
                                return RedirectToAction("ProgramSelection");
                            }
                            else
                            {
                                logDebug($"{id} HandOff failed [Client Code Not Found]");
                                //return Json(new { Success = -1, ErrorMsg = "HandOff failed [Client Code Not Found]" });

                            }

                        }
                        else
                        {
                            logDebug($"{id} HandOff failed [ID mismatch]");
                            //return Json(new { Success = -1, ErrorMsg = "HandOff failed [ID mismatch]" });
                        }
                    }
                    else
                    {
                        logDebug($"{id} HandOff failed [ID not found]");

                        //return Json(new { Success = -1, ErrorMsg = "HandOff failed [ID not found]" });
                    }
                }
                else
                {
                    logDebug($"{id} HandOff failed [Invalid ID]");

                    //return Json(new { Success = -1, ErrorMsg = "HandOff failed [Invalid ID]" });
                }
            }
            catch (Exception ex)
            {
                MvcApplication.LogError(ex);

            }
            return RedirectToAction("Login", "Authentication");

            // return Json(new { Success = -1, ErrorMsg = "HandOff failed" });
        }
        #endregion handoff

        #region Private Methods

        private static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

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

        private bool makePayment(DonorTestInfo donorTestInfo, string amt, string mode, string description, PaymentOverride paymentOverride = null)
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

                    donorBL.SavePaymentDetails(donorTestInfo, "From Web");


                }
            }
            if (paymentOverride !=null)
            {
                
                donorTestInfo.TestStatus = DonorRegistrationStatus.InQueue;
                donorBL.SavePaymentDetails(donorTestInfo, "From Web", paymentOverride);
            }
            return true;
        }

        private string AuthorizeNetPayment(RegistrationDataModel regDataModel)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

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

        private void GetPaymentMethod()
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

        /// <summary>
        /// This DB Read Only method will generate a DonorTestInfo object for use during pre-registration checkout
        /// </summary>
        /// <param name="clientDepartment"></param>
        /// <returns></returns>
        private DonorTestInfo CreatePreRegistrationDonorTestObject(ClientDepartment clientDepartment)
        {
            logDebug("CreatePreRegistrationDonorTestObject");

            DonorTestInfo donorTestInfo = new DonorTestInfo();

            // We need to build a test_info model

            string sqlQuery = string.Empty;


            donorTestInfo.DonorId = 0;
            donorTestInfo.ClientId = clientDepartment.ClientId;
            donorTestInfo.ClientDepartmentId = clientDepartment.ClientDepartmentId;
            donorTestInfo.ClientDeparmentName = clientDepartment.DepartmentName;
            donorTestInfo.MROTypeId = clientDepartment.MROTypeId;
            donorTestInfo.PaymentTypeId = clientDepartment.PaymentTypeId;

            return donorTestInfo;
        }

        /// <summary>
        /// This function commits a registration to the database
        /// then emails the user if they have recordkeeping enabled
        /// 
        /// </summary>
        private bool CommitRegistrationToDatabase()
        {
            logDebug("CommitRegistrationToDatabase");

            if ((bool)Session["AlreadyCommitted"] == true)
            {
                logDebug("CommitRegistrationToDatabase AlreadyCommitted is true");

                return false;
            }

            setDonorBLSession();



            DonorTestInfo donorTestInfo = (DonorTestInfo)Session["donorTestInfo"];
            Donor donor = (Donor)Session["PreRegistrationDonorObject"];
            string ClientCode = (string)Session["PreRegistrationClientCode"];
            logDebug($"Committing to database {ClientCode}");

            /// TODO - Here - we could check session for a First Name, last name, SSN, and DOB match
            /// If so - we'll use that user & donor id
            /// We'll update the database instead of creating new user / donor rows.
            /// Then using that donor id, user id, we use the selected client id, dept and create test info records.
            /// See the finalize function for next steps

            //******* Pregestration Steps
            // Do Pregistration Step:
            int DonorId = PreRegStep1(donor, ClientCode);
            logDebug($"PreRegStep1 complete: DonorID = {DonorId} {donor.DonorFirstName} {donor.DonorLastName} {ClientCode}");

            if (DonorId == 0)
            {
                logDebug($"NO DONOR ID FROM STEP 1!");
                logDebug("");
                logDebug("donorTestInfo:");
                logDebug(donorTestInfo.ToString());
                logDebug("");
                logDebug("donor:");
                logDebug(donor.ToString());
                logDebug("");
                logDebug("ClientCode:");
                logDebug(ClientCode.ToString());
                throw new Exception();
                // Something went wrong, bail early
            }
            // Now that we have an actual donor in the database - we'll get it
            Donor _dbDonor = donorBL.Get(donor.DonorId, "Web");
            logDebug($"Pulled {donor.DonorId} from database");

            // we need the DonorInitialDepartmentId before saving
            //_donor.DonorInitialDepartmentId = donorReturn.ClientDepartment.ClientDepartmentId;

            // On creation, the function doesn't map all fields
            // Additionally - new Donor sets string values to null, so nulls for contact info must be set to an empty string
            // Fill out the contact details:

            _dbDonor.DonorAddress1 = DonorPropertySafeStringCheck(donor.DonorAddress1);
            _dbDonor.DonorAddress2 = DonorPropertySafeStringCheck(donor.DonorAddress2);
            _dbDonor.DonorCity = DonorPropertySafeStringCheck(donor.DonorCity);
            _dbDonor.DonorDateOfBirth = donor.DonorDateOfBirth; // required and already verified
            _dbDonor.DonorGender = donor.DonorGender;
            _dbDonor.DonorMI = DonorPropertySafeStringCheck(donor.DonorMI);
            _dbDonor.DonorPhone1 = DonorPropertySafeStringCheck(donor.DonorPhone1);
            _dbDonor.DonorPhone2 = DonorPropertySafeStringCheck(donor.DonorPhone2);
            _dbDonor.DonorSSN = DonorPropertySafeStringCheck(donor.DonorSSN);
            _dbDonor.DonorState = DonorPropertySafeStringCheck(donor.DonorState);
            _dbDonor.DonorSuffix = DonorPropertySafeStringCheck(donor.DonorSuffix);
            _dbDonor.DonorZip = DonorPropertySafeStringCheck(donor.DonorZip);
            _dbDonor.DonorInitialDepartmentId = donor.DonorInitialDepartmentId;
            _dbDonor.DonorInitialClientId = donor.DonorInitialClientId;
            _dbDonor.ClientDepartment = donor.ClientDepartment;
            logDebug($"_dbDonor cloned to donor");

            donor = _dbDonor;
            // We're done with the dbdonor
            _dbDonor = null;



            // With our DB donor, it's empty - so we'll copy fields from our phantom donor
            // and save it. Step 2 is where user populates details.
            Session["DonorId"] = DonorId;
            logDebug($"Session DonorID = {Session["DonorId"].ToString()}");

            // Continue to Step 2
            if (PreRegStep2(donor) == 0)
            {
                // Something went wrong, bail early
                logDebug($"PreRegStep2 result is 0! Something went wrong");


            }


            // Continue to Step 3

            if (PreRegStep3(donor, donorTestInfo) == 0)
            {
                // Something went wrong, bail early
                logDebug($"PreRegStep3 result is 0! Something went wrong");
            }


            // Data creation complete - 
            // Update Session Variables with DB driven objects

            donorTestInfo = donorBL.GetDonorTestInfoByDonorId(DonorId);
            logDebug($"Session[\"DonorTestinfoId\"] is {Session["DonorTestinfoId"]} (Encrypted)");
            Session["donorTestInfo"] = donorTestInfo;

            logDebug($"Session[\"donorTestInfo\"] set to {donorTestInfo}");
            logDebug($"Session[\"DonorTestinfoId\"] is now {Session["DonorTestinfoId"]}");

            //Session["donorTestInfo"] = donorTestInfo;
            Session["PreRegistrationDonorObject"] = donor;

            Session["AlreadyCommitted"] = true;

            // Lastly, send email if needed
            if (PreRegStep4(donor) == 0)
            {
                // Something went wrong, bail early
                logDebug($"PreRegStep4 result is 0! Something went wrong");
            }

            return true;
        }

        private int PreRegStep1(Donor _donor, String ClientCode)
        {
            try
            {
                logDebug($"PreRegStep1 {ClientCode}");

                RegistrationDataModel registrationDataModel = new RegistrationDataModel();
                registrationDataModel.FirstName = _donor.DonorFirstName;
                registrationDataModel.LastName = _donor.DonorLastName;
                registrationDataModel.EmailID = _donor.DonorEmail;

                //DonorBL donorBL = new DonorBL(_logger, Session.SessionID);

                // Find existing donor with these details
                //int ExistingDonorId = donorBL.MatchDonorIdByDetails(_donor.DonorFirstName, _donor.DonorLastName, _donor.DonorSSN, _donor.DonorDateOfBirth.ToString("yyyy-MM-dd"));

                //if (ExistingDonorId > 1)
                //{
                //    _donor.DonorId = ExistingDonorId;
                //}
                logDebug($"Calling  DoDonorPreRegisteration");

                int status = donorBL.DoDonorPreRegisteration(_donor, ClientCode);
                logDebug($"DoDonorPreRegisteration result {status.ToString()}");

                // Sending the email needs to be PreRegStep4



            }
            catch (Exception ex)
            {
                logDebug($"An error occured trying to add {_donor.DonorFirstName} {_donor.DonorLastName}");
                LogAnError(ex);
                throw;
            }
            return _donor.DonorId;
        }

        private int PreRegStep2(Donor _donor)
        {
            try
            {
                logDebug($"PreRegStep2");

                int donorID = Convert.ToInt32(_donor.DonorId);
                logDebug($"Saving donor");

                int retval = donorBL.Save(_donor);

                logDebug($"Donor Saved result: {retval.ToString()}");

                return retval;
            }
            catch (Exception ex)
            {
                logDebug($"An error occured trying to add {_donor.DonorFirstName} {_donor.DonorLastName}");
                LogAnError(ex);
                throw;
            }
        }

        private int PreRegStep3(Donor _donor, DonorTestInfo donorTestInfo)
        {
            logDebug($"PreRegStep3");

            int retval = 0;
            int donorId = _donor.DonorId;
            int clientDepartmentId = (int)donorTestInfo.ClientDepartmentId;
            logDebug($"clientDepartmentId = {clientDepartmentId}");

            try
            {
                donorTestInfo = donorBL.GetDonorTestInfoByDonorId(donorId);
                logDebug($"donorTestInfo = {donorTestInfo}");

                if ((donorTestInfo.ClientDepartmentId.ToString() != "0" && donorTestInfo.PaymentStatus == PaymentStatus.Paid) || (donorTestInfo.ClientDepartmentId.ToString() == "0"))
                {

                    Session["DonorTestinfoId"] = "";
                    Session["isPaymentStatus"] = "Yes";

                    logDebug($"Calling DoDonorRegistrationTestRequest");

                    Donor donorReturn = donorBL.DoDonorRegistrationTestRequest(donorId, clientDepartmentId);
                    logDebug($"Back from DoDonorRegistrationTestRequest - donorReturn {donorReturn.DonorId.ToString()}, donorReturn.DonorTestInfoId {donorReturn.DonorTestInfoId.ToString()}");

                    string Donortestinfoid = donorReturn.DonorTestInfoId.ToString();
                    Session["DonorTestinfoId"] = UserAuthentication.Encrypt(Donortestinfoid.ToString(), true);
                    logDebug($"Session[\"DonorTestinfoId\"] set to {Session["DonorTestinfoId"]} (Encrypted)");

                    if (donorReturn != null)
                    {
                        RegistrationDataModel model = new RegistrationDataModel();

                        Session["UserName"] = donorReturn.DonorFirstName;
                        logDebug($"Session[\"UserName\"] set to {Session["UserName"]}");

                        Client client = clientBL.Get(donorReturn.ClientDepartment.ClientId);
                        logDebug($"Loaded client {client.ClientName}");

                        if (MvcApplication.Production==true)
                        {
                            if (!string.IsNullOrEmpty(donorReturn.ClientDepartment.ClearStarCode) &&
                                donorReturn.ClientDepartment.ClearStarCode.Length > 2 &&
                                _donor.ClientDepartment.ClientDeptTestCategories.Where(x => x.TestCategoryId == TestCategories.BC).ToList().Count() == 1
                                )
                            {
                                logDebug($"Client has ClearStarCode");

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

                                if (donorReturn.ClientDepartment.ClearStarCode.Length > 0
                                    &&
                                    donorReturn.ClientDepartment.ClientDeptTestCategories.Where(x => x.TestCategoryId == TestCategories.BC).ToList().Count() == 1
                                    )

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
                                                donorReturn.DonorClearStarProfId = ProfileNo;
                                                donorReturn.DonorInitialDepartmentId = donorReturn.ClientDepartment.ClientDepartmentId;
                                                //donorReturn.ClientDepartment = donorReturn.ClientDepartment;
                                                int status = donorBL.Save(donorReturn);

                                            }
                                        }
                                    }
                                    catch (System.Exception ex)
                                    {
                                        logDebug($"An error occured trying to add {_donor.DonorFirstName} {_donor.DonorLastName}");
                                        LogAnError(ex);
                                    }
                                    logDebug($"Client ClearStar complete");

                                }
                                #endregion
                            }
                        }

                        retval = 1;

                    }
                }
            }
            catch (Exception ex)
            {
                logDebug($"An error occured trying to add {_donor.DonorFirstName} {_donor.DonorLastName}");
                LogAnError(ex);
                throw;
            }
            return retval;
        }

        private int PreRegStep4(Donor _donor)
        {
            RegistrationDataModel registrationDataModel = new RegistrationDataModel();
            registrationDataModel.FirstName = _donor.DonorFirstName;
            registrationDataModel.LastName = _donor.DonorLastName;
            registrationDataModel.EmailID = _donor.DonorEmail;

            int retval = 0;
            if (_donor.DonorId > 0)
            {
                UserBL userBL = new UserBL();

                User user = userBL.GetByDonorId(_donor.DonorId);
                var _pw = UserAuthentication.Decrypt(user.UserPassword, true);
                _donor.TemporaryPassword = _pw;

                logDebug($"_donor.DonorId = {_donor.DonorId.ToString()}");

                string activationLink = ConfigurationManager.AppSettings["ActivationLink"].ToString().Trim();
                if (!activationLink.EndsWith("/"))
                {
                    activationLink += "/";
                }

                activationLink += Helper.Base64ForUrlEncode(UserAuthentication.URLIDEncrypt(_donor.DonorId.ToString(), true));

                registrationDataModel.TemporaryPassword = _donor.TemporaryPassword;
                registrationDataModel.ActivationLink = activationLink;

                // Only Send if Record Keeping is enabled. - Setting: OnlyActiveWithRecordKeeping
                bool bOnlyActiveWithRecordKeeping = true;
                bool bSendActivationEmail = false;
                bool.TryParse(ConfigurationManager.AppSettings["OnlyActiveWithRecordKeeping"].ToString().Trim(), out bOnlyActiveWithRecordKeeping);

                if (!bOnlyActiveWithRecordKeeping)
                {
                    bSendActivationEmail = true;
                }
                else
                {
                    if ((bool)Session["Registration_Client_Has_Recordkeeping"] == true) bSendActivationEmail = true;
                    logDebug($"Send Activation Email (Has Record Keeping) {bSendActivationEmail.ToString()}");

                }

                if (Session["ActivateAccount"] != null)
                {
                    if (bSendActivationEmail == false)
                    {
                        bSendActivationEmail = (bool)Session["ActivateAccount"];
                    }
                }

                if (bSendActivationEmail)
                {
                    try
                    {
                        IUserMailer mail = new RegistrationMailer(registrationDataModel);
                        mail.PreRegistrationMail().Send();
                        logDebug($"Activation Email sent to {registrationDataModel.Username} - {registrationDataModel.DonorEmail}");

                    }
                    catch (Exception ex)
                    {
                        logDebug($"An error occured trying to send registration email to {registrationDataModel.DonorEmail}, {registrationDataModel.Username} - {_donor.DonorFirstName} {_donor.DonorLastName}");
                        LogAnError(ex);
                    }
                }

            }

            return retval;
        }

        private string DonorPropertySafeStringCheck(string val)
        {
            return (val == null) ? string.Empty : val;
        }

        private void logDebug(string _t)
        {
            if (Session != null)
            {
                MvcApplication._logger.Debug($"{Session.SessionID} - RegistrationController - {_t.ToString()}");
            }
            else
            {
                MvcApplication._logger.Debug($"NO SESSION - RegistrationController - {_t.ToString()}");
            }

        }

        private void LogAnError(Exception ex)
        {
            MvcApplication._logger.Error($"{Session.SessionID}- RegistrationController - ERROR");

            MvcApplication._logger.Error(ex.Message);
            if (!(ex.InnerException == null)) MvcApplication._logger.Error(ex.InnerException.ToString());
        }
        private void setDonorBLSession()
        {

            try
            {

                if (Session != null)
                {
                    if (donorBL != null)
                    {
                        this.donorBL.SessionID = Session.SessionID.ToString();
                        this.donorBL.Logger = MvcApplication._logger;
                    }
                }
                else
                {
                    logDebug("setDonorBLSession has no session");
                }
            }
            catch (Exception ex)
            {
                LogAnError(ex);
            }
        }

        private string getDashedSSN(string _ssn)
        {
            if (_ssn.Length != 9) return _ssn;
            string retval = _ssn;

            Regex regex;
            regex = new Regex("^\\d{3}-\\d{2}-\\d{4}$");
            bool _PIDHasDashes = regex.IsMatch(_ssn);
            if (!_PIDHasDashes)
            {
                retval = _ssn.Insert(5, "-").Insert(3, "-");
            }
            return retval;

        }
        #endregion
    }
}
