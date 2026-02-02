using SurPath.Business;
using SurPath.Business.Master;
using SurPath.Entity;
using SurPath.Entity.Master;
using SurPath.Enum;
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
using Surpath.CSTest;
using System.Web.Configuration;
using System.Xml;
using System.Xml.Serialization;


namespace SurPathWeb.Areas.Background.Controllers
{
    public class BackgroundAuthController : Controller
    {
        #region Private Variables

        UserAuthentication userAuthentication = new UserAuthentication();
        AttorneyBL attorneyBL = new AttorneyBL();
        DonorBL donorBL = new DonorBL();
        ClientBL clientBL = new ClientBL();
        UserBL userBL = new UserBL();

        string donorStatus;


        #endregion

        [HttpGet]
        public ActionResult Login()
        {
            //Session.Abandon();
        
            
           
            return View();
        }
        public ActionResult Index()
        {
            //Session.Abandon();

            //string sUserName = WebConfigurationManager.AppSettings["ProdLoginName"];
            //string sPassword = WebConfigurationManager.AppSettings["ProdPassword"];

            string sUserName = WebConfigurationManager.AppSettings["TestLoginName"];
            string sPassword = WebConfigurationManager.AppSettings["TestPassword"];
            int iBOID = int.Parse(WebConfigurationManager.AppSettings["BoID"]);

            string sCustID = "SLSS_00001";
            
            ////Button say create clearstar customer
            
            //return back custid
            //store custid in our db

            

            //string sProfNo = "2018031442776347";

            string sProfNo = "2018041204567257";


            byte[] bytes = null;

                

            string mike = "mike";
            int nodecount = 0;
            using (Surpath.CSTest.Profile.Profile gwProfile = new Surpath.CSTest.Profile.Profile())
            {
                try
                {
                    //bytes = gwProfile.GetProfileReport(sUserName, sPassword, iBOID, sCustID, sProfNo, false);
                    //bytes = gwProfile.GetListOfProfiles(sUserName, sPassword, iBOID, sCustID,,false, , sProfNo, false);
                    XmlNode result = gwProfile.GetProfileDetail(sUserName, sPassword, iBOID, sCustID, sProfNo);
                    nodecount = result.ChildNodes.Count;

                    XmlNode error =  result.SelectSingleNode("ErrorStatus/Message");

                    if (!string.IsNullOrEmpty(error.InnerText))
                    {
                        //If Error node is not empty that means some error occurred, handle it in this area
                    }
                    else
                    {
                       mike = result.InnerText.ToString();
                        mike = result.InnerXml.ToString();
                        foreach (var node in result.InnerXml)
                        {
                            
                        }

                    }


                }
                catch (System.Web.Services.Protocols.SoapException ex)
                {
                    var error = ex.StackTrace.ToString() + "----" + ex.Message.ToString();
                }
                //if (bytes.Length > 0)
                //{
                //    Response.BufferOutput = true;
                //    Response.AddHeader("Content-Disposition", "inline");
                //    Response.ContentType = "application/pdf";
                //    Response.BinaryWrite(bytes);
                //}
                Response.Write(mike);

                
                string blah = mike;
                

                
        



            }
            //System.Xml.XmlNode x = xml;
            return View();
        }
        public ActionResult Buttons()
        {
            //Session.Abandon();
            return View();
        }

        [HttpPost]
        public ActionResult Login(string userName, string password, string returnUrl)
        {
            try
            {
                if (userAuthentication.ValidateUser(userName, password))
                {
                    var userData = userAuthentication.GetByUsernameOrEmail(userName);

                    if (userData != null)
                    {
                        DataTable InActiveUser = donorBL.GetInActiveUser(userName, password);
                        if (InActiveUser.Rows.Count == 0)
                        {
                            if (userData.UserType == SurPath.Enum.UserType.Donor)
                            {
                                Session["UserType"] = UserType.Donor;
                                Session["UserName"] = userData.UserFirstName + " " + userData.UserLastName;
                                Session["UserId"] = userData.UserId;
                                Session["DonorId"] = userData.DonorId;
                                Session["UserLoginName"] = userData.Username;

                                //if (string.IsNullOrEmpty(returnUrl))
                                //{

                                //}

                                Donor donor = donorBL.Get(Convert.ToInt32(userData.DonorId), "Web");

                                if (donor.DonorRegistrationStatusValue == SurPath.Enum.DonorRegistrationStatus.Activated)
                                {
                                    return RedirectToAction("ProfileUpdate", "Registration");
                                }
                                else if (donor.DonorRegistrationStatusValue == SurPath.Enum.DonorRegistrationStatus.PreRegistration)
                                {
                                    throw new Exception("Your activation is PENDING. Please open the email SurScan sent you. Use the temporary password provided in the email, and CLICK the activation link.");
                                }
                                if (userData.ChangePasswordRequired == true)
                                {
                                    string userID = UserAuthentication.URLIDEncrypt(userData.UserId.ToString(), false);
                                    return RedirectToAction("Activation", new RouteValueDictionary(new { controller = "Authentication", action = "Activation", id = userID }));
                                }
                                else if (userData.ProgramExists.ToUpper() == "NO")
                                {
                                    Session["isProgramExists"] = userData.ProgramExists;
                                    return RedirectToAction("ProgramSelection", "Donor");
                                }
                                else
                                {
                                    Session["isProgramExists"] = userData.ProgramExists;
                                    return RedirectToAction("DocumentUpload", "Donor");
                                }
                            }
                            else if (userData.UserType == SurPath.Enum.UserType.Attorney)
                            {
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
        public ActionResult Activation(string id)
        {
            ViewBag.userId = id;

            try
            {
                int UserID = Convert.ToInt32(UserAuthentication.URLIDDecrypt(id.ToString(), false));
                UserBL userBL = new UserBL();

                var userData = userBL.Get(UserID);

                if ((userData != null && !userData.ChangePasswordRequired))
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
                ViewBag.UserID = id;

                int userID = Convert.ToInt32(UserAuthentication.URLIDDecrypt(id.ToString(), false));

                UserBL userBL = new UserBL();
                User user = userBL.Get(userID);

                if (user == null)
                {
                    throw new Exception("User does not exists.");
                }

                else
                {
                    if (user.UserPassword != UserAuthentication.Encrypt(oldPassword, true))
                    {
                        throw new Exception("The old password does not match with the database value.");
                    }
                }
                string NewPassword = UserAuthentication.Encrypt(newPassword, true);
                if (user.UserType != UserType.Donor)
                {
                    userBL.ChangePassword(user.Username, NewPassword);
                }
                else
                {
                    int donorid = Convert.ToInt32(user.DonorId);
                    Donor donor = new Donor();
                    donor = donorBL.Get(donorid, "Web");
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
                    donorBL.DoDonorInQueueUpdate(donorid, oldPassword, newPassword,donorStatus);
                }

            }
            catch (Exception e)
            {
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
                    userBL.ChangePassword(attorney.AttorneyEmail, NewPassword);
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
                    userBL.ChangePassword(court.CourtUsername, NewPassword);
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
                    userBL.ChangePassword(judge.JudgeUsername, NewPassword);
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
                    userBL.ChangePassword(user.Username, NewPassword);
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
                    userBL.ChangePassword(user.Username, NewPassword);
                }

            }
            catch (Exception e)
            {
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
            catch
            {
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
