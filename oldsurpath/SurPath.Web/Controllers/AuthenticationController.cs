using Serilog;
using SurPath.Business;
using SurPath.Business.Master;
using SurPath.Entity;
using SurPath.Entity.Master;
using SurPath.Enum;
using SurpathBackend;
using SurPathWeb.Filters;
using SurPathWeb.Mailers;
using SurPathWeb.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
namespace SurPathWeb.Controllers
{
    public class AuthenticationController : Controller
    {
        #region Private Variables

        UserAuthentication userAuthentication = new UserAuthentication();
        AttorneyBL attorneyBL = new AttorneyBL();
        DonorBL donorBL = new DonorBL();
        ClientBL clientBL = new ClientBL();
        UserBL userBL = new UserBL();
        BackendLogic backendLogic = new BackendLogic();
        ILogger _logger = MvcApplication._logger;

        string donorStatus;


        #endregion

        public AuthenticationController()
        {
            _logger.Debug("AuthenticationController()");
        }

        [HttpGet]
        public ActionResult Login()
        {
            //Session.Abandon();

            try
            {
                return View();
            }
            catch (Exception ex)
            {
                MvcApplication.LogError(ex);
                throw;
            }
        }

        [HttpPost]
        public ActionResult Login(string userName, string password, string returnUrl)
        {
            _logger.Debug($"Login called {userName}, {password}, {returnUrl}");
            try
            {
                var _val = userAuthentication.ValidateUser(userName, password);
                bool IsValidated = _val.Item1;
                int user_id = _val.Item2;
                int donor_id = _val.Item3;
                if (IsValidated==true)
                {
                    // var userData = userAuthentication.GetByUsernameOrEmail(userName);
                    var userData = userAuthentication.GetByUserById(user_id);

              
                    if (userData != null)
                    {
                        _logger.Debug($"Found a user: user email {userData.UserEmail} donor id {userData.DonorId} {userData.DonorName}");
                        DataTable InActiveUser = donorBL.GetInActiveUser(userName, password);
                        if (InActiveUser.Rows.Count == 0)
                        {
                            _logger.Debug($"No inactive users found");
                            // Capture The Login
                            backendLogic.SetUserActivity(userData.UserId, (int)UserActivityCategories.Login, $"User {userData.UserId} {userData.UserEmail} logged in");
                            // Set the authroles
                            UserAuthRoles userAuthRoles = new UserAuthRoles();
                            userAuthRoles.FromDataTable(userAuthentication.GetUserAuthorizationRules(userData.Username));
                            Session["UserAuthRoles"] = userAuthRoles;

                            if (userData.UserType == SurPath.Enum.UserType.Donor)
                            {
                                _logger.Debug($"is donor");
                                Session["UserType"] = UserType.Donor;
                                Session["UserName"] = userData.UserFirstName + " " + userData.UserLastName;
                                Session["UserId"] = userData.UserId;
                                Session["DonorId"] = userData.DonorId;
                                Session["UserLoginName"] = userData.Username;

                                Donor donor = donorBL.Get(Convert.ToInt32(userData.DonorId), "Web");

                                //if (donor.DonorRegistrationStatusValue == SurPath.Enum.DonorRegistrationStatus.Activated)
                                //{
                                //    return RedirectToAction("ProfileUpdate", "Registration");
                                //}
                                //else 
                                if (donor.DonorRegistrationStatusValue == SurPath.Enum.DonorRegistrationStatus.PreRegistration)
                                {
                                    _logger.Debug($"Still Pending Activation");
                                    throw new Exception("Your activation is PENDING. Please open the email SurScan sent you. Use the temporary password provided in the email, and CLICK the activation link.");
                                }
                                if (userData.ChangePasswordRequired == true)
                                {
                                    _logger.Debug($"Change Password required");
                                    string userID = UserAuthentication.URLIDEncrypt(userData.UserId.ToString(), false);
                                    return RedirectToAction("Activation", new RouteValueDictionary(new { controller = "Authentication", action = "Activation", id = userID }));
                                }
                                else if (Session["IntegrationRegistration"]!=null && ((bool)Session["IntegrationRegistration"]==true))
                                {
                                    Session["SSNRequired"] = true;
                                    Session["PidTypeValues"] = donor.PidTypeValues;
                                    Session["PreRegistrationDonorObject"] = donor;
                                    return RedirectToAction("ProfileUpdate2", "Registration", donor);
                                }
                                else if (userData.ProgramExists.ToUpper() == "NO")
                                {
                                    _logger.Debug($"Program Exists no");
                                    Session["isProgramExists"] = userData.ProgramExists;
                                    return RedirectToAction("ProgramSelection", "Donor");
                                }
                                else
                                {
                                    // if the user is in-queue and the test doesn't have record keeping - send them to update their profile
                                    bool _RecordKeeping = false;
                                    if (donor.ClientDepartment!=null)
                                    {
                                        _RecordKeeping = donor.ClientDepartment.IsRecordKeeping;
                                    }
                                    else
                                    {
                                        // We need the client Dept.
                                        var _dti = donorBL.GetDonorTestInfoByDonorId(donor.DonorId);
                                        ClientDepartment clientDepartment = clientBL.GetClientDepartment(_dti.ClientDepartmentId);
                                        _RecordKeeping = clientDepartment.IsRecordKeeping;
                                    }

                                    if (donor.DonorRegistrationStatusValue == SurPath.Enum.DonorRegistrationStatus.InQueue && _RecordKeeping==false)
                                    {
                                        Session["SSNRequired"] = true;
                                        Session["PidTypeValues"] = donor.PidTypeValues;
                                        Session["PreRegistrationDonorObject"] = donor;
                                        return RedirectToAction("ProfileUpdate", "Registration",donor);
                                    }
                                    
                                    Session["isProgramExists"] = userData.ProgramExists;
                                    return RedirectToAction("DocumentUpload", "Donor");
                                }
                            }
                            else if (userData.UserType == SurPath.Enum.UserType.Attorney)
                            {
                                _logger.Debug($"Is Attorney");
                                Session["UserType"] = UserType.Attorney;
                                Session["UserName"] = userData.UserFirstName + " " + userData.UserLastName;
                                Session["UserId"] = userData.UserId;
                                Session["AttorneyId"] = userData.AttorneyId;
                                Session["UserLoginName"] = userData.Username;

                                //if (string.IsNullOrEmpty(returnUrl))
                                //{

                                //}

                                User user = userBL.Get(Convert.ToInt32(userData.UserId));

                                if (user.ChangePasswordRequired == false)
                                {
                                    return RedirectToAction("TestResult", "Attorney");
                                }
                                else
                                {
                                    throw new Exception("Your activation is PENDING. Please open the email SurScan sent you. Use the temporary password provided in the email, and CLICK the activation link.");
                                }

                            }


                            else if (userData.UserType == SurPath.Enum.UserType.Court)
                            {
                                Session["UserType"] = UserType.Court;
                                Session["UserName"] = userData.UserFirstName + " " + userData.UserLastName;
                                Session["UserId"] = userData.UserId;
                                Session["CourtId"] = userData.CourtId;
                                Session["UserLoginName"] = userData.Username;

                                //if (string.IsNullOrEmpty(returnUrl))
                                //{

                                //}

                                return RedirectToAction("TestResult", "Court");
                            }

                            else if (userData.UserType == SurPath.Enum.UserType.Judge)
                            {
                                _logger.Debug($"Is Judge");
                                Session["UserType"] = UserType.Judge;
                                Session["UserName"] = userData.UserFirstName + " " + userData.UserLastName;
                                Session["UserId"] = userData.UserId;
                                Session["JudgeId"] = userData.JudgeId;
                                Session["UserLoginName"] = userData.Username;

                                //if (string.IsNullOrEmpty(returnUrl))
                                //{

                                //}

                                return RedirectToAction("TestResult", "Judge");
                            }

                            else if (userData.UserType == SurPath.Enum.UserType.Vendor)
                            {
                                _logger.Debug($"Is Vendor");

                                Session["UserType"] = UserType.Vendor;
                                Session["UserName"] = userData.UserFirstName + " " + userData.UserLastName;
                                Session["UserId"] = userData.UserId;
                                Session["VendorId"] = userData.VendorId;
                                Session["UserLoginName"] = userData.Username;

                                //if (string.IsNullOrEmpty(returnUrl))
                                //{

                                //}

                                return RedirectToAction("VendorDashboard", "Vendor");
                                //return RedirectToAction("DonorSearch", "Vendor");
                            }
                            else if (userData.UserType == SurPath.Enum.UserType.Client)
                            {
                                Session["UserType"] = UserType.Client;
                                Session["UserName"] = userData.UserFirstName + " " + userData.UserLastName;
                                Session["UserId"] = userData.UserId;
                                Session["ClientId"] = userData.ClientId;
                                Session["UserLoginName"] = userData.Username;

                                //if (string.IsNullOrEmpty(returnUrl))
                                //{

                                //}
                                //return RedirectToAction("Dashboard", "Client");
                                return RedirectToAction("DonorSearch", "Client");
                            }


                        }
                        else
                        {
                            throw new Exception("Account is inactive,hence contact the administrator");
                        }
                    }
                }
                else
                {
                    throw new Exception("Email Address and Password does not match");
                }
            }
            catch (Exception ex)
            {
                MvcApplication.LogError(ex);

                ViewBag.ServerErr = ex.Message;
            }
            return View();
        }

        [HttpGet]
        public ActionResult Signout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Authentication");
        }

        [HttpGet]
        public ActionResult Activation(string donorid, string id="")
        {
            if (string.IsNullOrEmpty(donorid) && string.IsNullOrEmpty(id)) return RedirectToAction("Login", "Authentication");
            if (string.IsNullOrEmpty(donorid) && !string.IsNullOrEmpty(id)) donorid = id;
            

            ViewBag.userId = donorid;
            _logger.Information("Activation Link Called");
            int UserID;
            try
            {
                UserID = Convert.ToInt32(UserAuthentication.URLIDDecrypt(donorid.ToString(), false));
                try
                {

                    UserBL userBL = new UserBL();
                    _logger.Information($"Activation Link Called for UserID {UserID.ToString()}");
                    var userData = userBL.Get(UserID);

                    if ((userData != null && !userData.ChangePasswordRequired))
                    {
                        _logger.Information($"Activation ChangePasswordRequired not set for {UserID.ToString()}");
                        _logger.Information("Redirecting to login");
                        return RedirectToAction("Login", "Authentication");
                    }
                }
                catch (Exception ex)
                {
                    MvcApplication.LogError(ex);

                    ViewBag.ServerErr = "Unable to process your request.";
                    //_logger.Error(ex.Message);
                    //if (string.IsNullOrEmpty(ex.InnerException.ToString())) _logger.Error(ex.InnerException.ToString());
                }
            }
            catch (Exception ex)
            {
                MvcApplication.LogError(ex);

                ViewBag.ServerErr = "Please check your URL and try again. This URL is invalid.";
                //_logger.Error(ex.Message);
                //if (string.IsNullOrEmpty(ex.InnerException.ToString())) _logger.Error(ex.InnerException.ToString());
            }

            return View();

        }

        [HttpPost]
        public ActionResult Activation(string id, string oldPassword, string newPassword)
        {
            try
            {
                ViewBag.UserID = id;

                int userID = Convert.ToInt32(UserAuthentication.URLIDDecrypt(id.ToString(), false));
                _logger.Debug($"Activation for user id: {userID}");
                UserBL userBL = new UserBL();
                User user = userBL.Get(userID);

                if (user == null)
                {
                    _logger.Debug($"User does not exists.");

                    throw new Exception("User does not exists.");
                }

                else
                {
                    if (user.UserPassword != UserAuthentication.Encrypt(oldPassword, true))
                    {
                        _logger.Debug($"Bad Password");

                        throw new Exception("The old password does not match with the database value.");
                    }
                }
                string NewPassword = UserAuthentication.Encrypt(newPassword, true);
                if (user.UserType != UserType.Donor)
                {
                    _logger.Debug($"Changing user Password");

                    userBL.ChangePassword(user.Username, NewPassword);
                }
                else
                {
                    _logger.Debug($"Changing Donor Password");
                    if (string.IsNullOrEmpty(donorStatus))
                    {
                        donorStatus = string.Empty;
                    }
                    int donorid = Convert.ToInt32(user.DonorId);
                    Donor donor = new Donor();
                    donor = donorBL.Get(donorid, "Web");
                    if (donor.DonorRegistrationStatusValue == DonorRegistrationStatus.PreRegistration)
                    {
                        donorStatus = "PreRegistration";
                    }
                    else if (donor.DonorRegistrationStatusValue == DonorRegistrationStatus.Registered || donor.DonorRegistrationStatusValue == DonorRegistrationStatus.Activated)
                    {
                        donorStatus = "Registered";
                    }
                    else if (donor.DonorRegistrationStatusValue == DonorRegistrationStatus.InQueue)
                    {
                        donorStatus = "InQueue";
                    }
                    _logger.Debug($"Setting donor status to {donorStatus}");
                    donorBL.DoDonorInQueueUpdate(donorid, oldPassword, newPassword, donorStatus);
                }

            }
            catch (Exception e)
            {
                MvcApplication.LogError(e);
                string[] error = new string[] { "User does not exists.", "The old password does not match with the database value." };

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

            return RedirectToAction("ChangePasswordConfirmation");
        }

        [HttpGet]
        [SessionValidateAttribute]
        public ActionResult ChangePassword()
        {
            if ((UserType)Session["UserType"] == UserType.Donor)
            {
                ViewBag.Layout = "~/Views/Shared/_DonorLayout.cshtml";
                ViewBag.Id = UserAuthentication.Encrypt(Session["DonorId"].ToString(), true);
            }
            else if ((UserType)Session["UserType"] == UserType.Attorney)
            {
                ViewBag.Layout = "~/Views/Shared/_AttorneyLayout.cshtml";
                ViewBag.Id = UserAuthentication.Encrypt(Session["AttorneyId"].ToString(), true);
            }
            else if ((UserType)Session["UserType"] == UserType.Court)
            {
                ViewBag.Layout = "~/Views/Shared/_CourtLayout.cshtml";
                ViewBag.Id = UserAuthentication.Encrypt(Session["CourtId"].ToString(), true);
            }
            else if ((UserType)Session["UserType"] == UserType.Judge)
            {
                ViewBag.Layout = "~/Views/Shared/_JudgeLayout.cshtml";
                ViewBag.Id = UserAuthentication.Encrypt(Session["JudgeId"].ToString(), true);
            }
            else if ((UserType)Session["UserType"] == UserType.Client)
            {
                ViewBag.Layout = "~/Views/Shared/_ClientLayout.cshtml";
                ViewBag.Id = UserAuthentication.Encrypt(Session["UserId"].ToString(), true);
            }
            else if ((UserType)Session["UserType"] == UserType.Vendor)
            {
                ViewBag.Layout = "~/Views/Shared/_VendorLayout.cshtml";
                ViewBag.Id = UserAuthentication.Encrypt(Session["UserId"].ToString(), true);
            }

            return View();
        }

        private void LogUserPWChange(User user, UserType userType)
        {
            // Capture The PW Change
            backendLogic.SetUserActivity(user.UserId, (int)UserActivityCategories.Security, $"User {user.UserId} {user.UserEmail} of type {((UserType)Session["UserType"]).ToString()} successfully changed password");

        }

        [HttpPost]
        [SessionValidateAttribute]
        public ActionResult ChangePassword(string Id, string oldPassword, string newPassword)
        {
            ViewBag.Id = Id;
            try
            {
                if ((UserType)Session["UserType"] == UserType.Donor)
                {
                    ViewBag.Layout = "~/Views/Shared/_DonorLayout.cshtml";
                    ViewBag.DonorID = Id;

                    int donorID = Convert.ToInt32(UserAuthentication.Decrypt(Id.ToString(), true));

                    UserBL userBL = new UserBL();

                    userBL.ChangeDonorPassword(donorID, oldPassword, newPassword);
                }
                else if ((UserType)Session["UserType"] == UserType.Attorney)
                {
                    ViewBag.Layout = "~/Views/Shared/_AttorneyLayout.cshtml";
                    ViewBag.AttorneyID = Id;

                    int attorneyID = Convert.ToInt32(UserAuthentication.Decrypt(Id.ToString(), true));

                    AttorneyBL attorneyBL = new AttorneyBL();
                    UserBL userBL = new UserBL();
                    Attorney attorney = attorneyBL.Get(attorneyID);

                    if (attorney == null)
                    {
                        throw new Exception("User does not exists.");
                    }

                    User user = userBL.GetByUsernameOrEmail(attorney.AttorneyEmail);

                    if (user != null)
                    {
                        if (user.UserPassword != UserAuthentication.Encrypt(oldPassword, true))
                        {
                            throw new Exception("The old password does not match with the database value.");
                        }
                    }
                    string NewPassword = UserAuthentication.Encrypt(newPassword, true);
                    if (userBL.ChangePassword(attorney.AttorneyEmail, NewPassword) > 0)
                    {
                        LogUserPWChange(user, (UserType)Session["UserType"]);
                    }
                }
                else if ((UserType)Session["UserType"] == UserType.Court)
                {
                    ViewBag.Layout = "~/Views/Shared/_CourtLayout.cshtml";
                    ViewBag.CourtID = Id;

                    int courtID = Convert.ToInt32(UserAuthentication.Decrypt(Id.ToString(), true));

                    CourtBL courtBL = new CourtBL();
                    UserBL userBL = new UserBL();
                    Court court = courtBL.Get(courtID);

                    if (court == null)
                    {
                        throw new Exception("User does not exists.");
                    }

                    User user = userBL.GetByUsernameOrEmail(court.CourtUsername);

                    if (user != null)
                    {
                        if (user.UserPassword != UserAuthentication.Encrypt(oldPassword, true))
                        {
                            throw new Exception("The old password does not match with the database value.");
                        }
                    }
                    string NewPassword = UserAuthentication.Encrypt(newPassword, true);
                    if (userBL.ChangePassword(court.CourtUsername, NewPassword)>0)
                    {
                        LogUserPWChange(user, (UserType)Session["UserType"]);

                    }
                }
                else if ((UserType)Session["UserType"] == UserType.Judge)
                {
                    ViewBag.Layout = "~/Views/Shared/_JudgeLayout.cshtml";
                    ViewBag.JudgeID = Id;

                    int judgeID = Convert.ToInt32(UserAuthentication.Decrypt(Id.ToString(), true));

                    JudgeBL judgeBL = new JudgeBL();
                    UserBL userBL = new UserBL();
                    Judge judge = judgeBL.Get(judgeID);

                    if (judge == null)
                    {
                        throw new Exception("User does not exists.");
                    }

                    User user = userBL.GetByUsernameOrEmail(judge.JudgeUsername);

                    if (user != null)
                    {
                        if (user.UserPassword != UserAuthentication.Encrypt(oldPassword, true))
                        {
                            throw new Exception("The old password does not match with the database value.");
                        }
                    }
                    string NewPassword = UserAuthentication.Encrypt(newPassword, true);
                    if(userBL.ChangePassword(judge.JudgeUsername, NewPassword) > 0)
                    {
                        LogUserPWChange(user, (UserType)Session["UserType"]);

                    }
                }
                else if ((UserType)Session["UserType"] == UserType.Client)
                {
                    ViewBag.Layout = "~/Views/Shared/_ClientLayout.cshtml";
                    ViewBag.UserID = Id;

                    int UserId = Convert.ToInt32(UserAuthentication.Decrypt(Id.ToString(), true));

                    UserBL userBL = new UserBL();
                    User user = userBL.Get(UserId);

                    if (user == null)
                    {
                        throw new Exception("User does not exists.");
                    }

                    //User user = userBL.GetByUsernameOrEmail(judge.JudgeUsername);

                    if (user != null)
                    {
                        if (user.UserPassword != UserAuthentication.Encrypt(oldPassword, true))
                        {
                            throw new Exception("The old password does not match with the database value.");
                        }
                    }
                    string NewPassword = UserAuthentication.Encrypt(newPassword, true);
                    if(userBL.ChangePassword(user.Username, NewPassword) > 0)
                    {
                        LogUserPWChange(user, (UserType)Session["UserType"]);

                    }
                }
                else if ((UserType)Session["UserType"] == UserType.Vendor)
                {
                    ViewBag.Layout = "~/Views/Shared/_VendorLayout.cshtml";
                    ViewBag.UserID = Id;

                    int UserId = Convert.ToInt32(UserAuthentication.Decrypt(Id.ToString(), true));

                    UserBL userBL = new UserBL();
                    User user = userBL.Get(UserId);

                    if (user == null)
                    {
                        throw new Exception("User does not exists.");
                    }

                    //User user = userBL.GetByUsernameOrEmail(judge.JudgeUsername);

                    if (user != null)
                    {
                        if (user.UserPassword != UserAuthentication.Encrypt(oldPassword, true))
                        {
                            throw new Exception("The old password does not match with the database value.");
                        }
                    }
                    string NewPassword = UserAuthentication.Encrypt(newPassword, true);
                    if(userBL.ChangePassword(user.Username, NewPassword) > 0)
                    {
                        LogUserPWChange(user, (UserType)Session["UserType"]);

                    }
                }
                
            }
            catch (Exception e)
            {
                MvcApplication.LogError(e);

                string[] error = new string[] { "User does not exists.", "The old password does not match with the database value." };

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

            return RedirectToAction("ChangePasswordConfirmation");
        }

        [HttpGet]
        public ActionResult ChangePasswordConfirmation()
        {
            Session.Abandon();
            return View();
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string userName)
        {
            try
            {
                UserAuthentication userAuth = new UserAuthentication();

                User user = userAuth.SendForgotPasswordWeb(userName);

                if (user != null)
                {
                    RegistrationDataModel model = new RegistrationDataModel();

                    model.FirstName = user.UserFirstName;
                    model.LastName = user.UserLastName;
                    model.Username = user.Username;
                    model.TemporaryPassword = UserAuthentication.Decrypt(user.UserPassword, true);

                    if (user.UserEmail.Trim() != string.Empty)
                    {
                        model.EmailID = user.UserEmail;

                        IUserMailer mail = new RegistrationMailer(model);
                        mail.ForgotPasswordMail().Send();

                        ViewBag.ServerErr = "The temporary password has been mailed to your Email Address.";
                    }
                }
            }
            catch (Exception ex)
            {
                MvcApplication.LogError(ex);

                ViewBag.ServerErr = ex.Message;
            }

            return View();
        }

        [HttpGet]
        public ActionResult SessionExpired(string returnUrl)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Activations(string id)
        {

            ViewBag.AttorneyId = id;

            try
            {
                int AttorneyID = Convert.ToInt32(UserAuthentication.Decrypt(id.ToString(), true));
                // if (!(donorData != null && donorData.DonorRegistrationStatusValue == DonorRegistrationStatus.PreRegistration))

                var AttorneyData = userBL.Get(AttorneyID);

                if (AttorneyData != null)
                {
                    return RedirectToAction("Login", "Authentication");
                }
            }
            catch(Exception e)
            {
                MvcApplication.LogError(e);

                ViewBag.ServerErr = "Unable to process your request.";
            }
            return View();
        }

        [HttpGet]
        [SessionValidateAttribute]
        public ActionResult SessionExpire()
        {
            return View();
        }
        //[HttpGet]
        //public ActionResult LoadSurScan()
        //{
        //    Dictionary<string, object> postData = new Dictionary<string, object>();
        //    postData.Add("id", "SlZzL0NTamQybkU90");
        //    postData.Add("second", "someValueTwo");

        //    return this.RedirectAndPost("http://23.253.81.99/SurPath/Registration/Activation/", values);
        //}
    }
}
