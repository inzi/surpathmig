using SurPath.Business;
using SurPath.Business.Master;
using SurPath.Entity;
using SurPath.Enum;
using SurPathWeb.Filters;
using SurPathWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using SurpathBackend;

namespace SurPathWeb.Controllers
{
    public class VendorController : Controller
    {
        DonorBL donorBL = new DonorBL();
        ClientBL clientBL = new ClientBL();
        BackendLogic backendLogic = new BackendLogic();

        [HttpGet]
        [SessionValidateAttribute]
        public ActionResult UserProfile()
        {
            Session["testResultList"] = null;
            Session["SortingValidation"] = null;
            Session["SortingValidation1"] = null;

            try
            {
                UserBL userBL = new UserBL();
                int UserId = Convert.ToInt32(Session["UserId"].ToString());

                var Userdata = userBL.Get(UserId);

                if (Userdata != null)
                {
                    var model = new DonorProfileDataModel
                    {
                        Username = Userdata.Username,
                        EmailID = Userdata.UserEmail,
                        FirstName = Userdata.UserFirstName,
                        LastName = Userdata.UserLastName,
                        Phone1 = Userdata.UserPhoneNumber,
                        Fax = Userdata.UserFax
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
            Session["testResultList"] = null;
            Session["SortingValidation"] = null;
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
                        return Redirect("VendorDashboard");
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
        public ActionResult DonorInfo(string donorId, string donorTestInfoId)
        {
            Session["testResultList"] = null;
            Session["SortingValidation"] = null;
            Session["SortingValidation1"] = null;
            Session["model.DonorTestInfoId"] = null;
            if (string.IsNullOrEmpty(donorId))
            {
                return RedirectToAction("VendorDashboard", "Vendor");
            }

            try
            {
                DonorBL donorBL = new DonorBL();
                int donorID = Convert.ToInt32(UserAuthentication.Decrypt(donorId, true));
                var donorData = donorBL.Get(donorID, "Web");

                if (donorData != null)
                {
                    DonorProfileDataModel model = new DonorProfileDataModel();

                    model.FirstName = donorData.DonorFirstName;
                    model.LastName = donorData.DonorLastName;
                    model.DonorDOBMonth = donorData.DonorDateOfBirth.Month.ToString();
                    model.DonorDOBDate = donorData.DonorDateOfBirth.Day.ToString();
                    model.DonorDOBYear = donorData.DonorDateOfBirth.Year.ToString();
                    ViewBag.SSNSession = donorData.DonorSSN;
                    if (donorData.DonorSSN.ToString().Length == 11)
                    {
                        model.SSN = "XXX-XX-" + donorData.DonorSSN.ToString().Substring(7);
                    }
                    model.DonorTestInfoId = donorTestInfoId;
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
        public ActionResult DonorInfo(DonorProfileDataModel model)
        {
            Session["testResultList"] = null;
            Session["SortingValidation"] = null;
            Session["SortingValidation1"] = null;
            //  int donorID = Convert.ToInt32("153");
            //string sst = UserAuthentication.Encrypt(dr["DonorId"].ToString(), true);
            var donorData = donorBL.Get(Convert.ToInt32((UserAuthentication.Decrypt(model.DonorId.ToString(), true))), "Web");

            if (donorData != null)
            {
                donorData.DonorFirstName = model.FirstName;
                donorData.DonorLastName = model.LastName;
                donorData.DonorDateOfBirth = Convert.ToDateTime(model.DonorDOBMonth.ToString() + '-' + model.DonorDOBDate.ToString() + '-' + model.DonorDOBYear.ToString());
                donorData.DonorSSN = model.SSN;

                int status = donorBL.Save(donorData);
                if (status == 1)
                {
                    Session["model.DonorTestInfoId"] = model.DonorTestInfoId;
                    return RedirectToAction("TestInfo", "Vendor");

                }
                else
                {
                    ViewBag.ServerErr = "Unable to process your request";
                }
            }
            return View(model);
        }


        //[HttpPost]
        //public JsonResult NotifyDonor(DonorNotifyJSONModel model)
        //{
        //    // send it to the database
        //    bool doNow = model.NotifyNow;
        //    bool doNextWindow = false;
        //    doNextWindow = !doNow;
        //    int donor_test_info_id = model.donor_test_info_id;
            

        //    if (donor_test_info_id<0)
        //    {
        //        model.Queued = false;
        //        model.Msg = "There's a problem notifying this user.";
        //        return Json(model);
        //    }
        //    else
        //    {
        //        bool result = backendLogic.NotificationSetForTransmission(donor_test_info_id, doNextWindow, doNow);
        //        model.Queued = false;
        //        model.Msg = "Notification Queued.";
        //    }
        //    return Json(model);
        //}

        [HttpGet]
        [SessionValidateAttribute]
        public ActionResult VendorSearch()
        {
            //TestResultDataModel model = new TestResultDataModel();
            FormCollection frm = new FormCollection();
            string searchkey = null;

            ViewBag.SuccessMsg = "";
            if (Session["TestInfoSuccess"] != null)
            {
                if (!string.IsNullOrEmpty(Session["TestInfoSuccess"].ToString()))
                {
                    ViewBag.SuccessMsg = "Test info updated successfully";
                    Session["TestInfoSuccess"] = "";
                }
            }
            List<TestResultDataModel> testResultList = new List<TestResultDataModel>();
            if (Session["SortingValidation"] != null)
            {
                VendorSearch(searchkey, frm);
                return View();
            }
            else
            {
                Session["testResultList"] = null;
                Session["SortingValidation"] = null;
                LoadSearchKey();
                return View(testResultList);
            }
        }

        [HttpPost]
        [SessionValidateAttribute]
        public ActionResult VendorSearch(string searchKey, FormCollection frmCollection)
        {
            string searchValue;
            if (frmCollection["hidSearchValue"] != null)
            {
                searchValue = frmCollection["hidSearchValue"].ToString();
            }
            else
            {
                searchValue = null;
            }
            Session["SortingValidation"] = frmCollection["searchvalue"] != null ? frmCollection["searchvalue"].ToString() : null;
            Session["SortingValidation1"] = frmCollection["searchvalue1"] != null ? frmCollection["searchvalue1"].ToString() : null;
            Dictionary<string, string> searchPrm = new Dictionary<string, string>();

            List<TestResultDataModel> testResultList = new List<TestResultDataModel>();

            if (Session["SortingValidation1"] != null)
            {
                string sql = string.Empty;
                if (searchKey == "1")
                {
                    sql = "donors.donor_first_name like '" + "%" + searchValue + "%" + "'";
                    ViewBag.searchvalue = searchValue;
                }
                else if (searchKey == "2")
                {
                    sql = "donors.donor_last_name like '" + "%" + searchValue + "%" + "'";
                    ViewBag.searchvalue = searchValue;
                }
                else if (searchKey == "3")
                {
                    sql = "donors.donor_date_of_birth like '" + searchValue + "'";
                    DateTime search = Convert.ToDateTime(searchValue);
                    ViewBag.year = search.Year.ToString();
                    ViewBag.month = search.Month.ToString();
                    ViewBag.date = search.Day.ToString();
                }
                else if (searchKey == "4")
                {
                    sql = "donors.donor_city like '" + "%" + searchValue + "%" + "'";
                    ViewBag.searchvalue = searchValue;
                }
                else if (searchKey == "5")
                {
                    sql = "donors.donor_zip like '" + searchValue + "'";
                    ViewBag.searchvalue = searchValue;
                }
                else if (searchKey == "6")
                {
                    sql = "donors.donor_email like '" + "%" + searchValue + "%" + "'";
                    ViewBag.searchvalue = searchValue;
                }
                else
                {
                    sql = " 1=1 ";
                }

                searchPrm.Add("searchSql", sql);

                DataTable dtDonors = donorBL.SearchFromVendorDashboard(searchPrm);
                var op_Dtt = "";
                foreach (DataRow dr in dtDonors.Rows)
                {
                    if (dr["DonorId"].ToString() != null)
                    {
                        if (dr["DonorId"].ToString() != "0")
                        {
                            TestResultDataModel testResult = new TestResultDataModel();

                            testResult.DonorId = UserAuthentication.Encrypt(dr["DonorId"].ToString(), true);
                            testResult.DonorTestInfoId = UserAuthentication.Encrypt(dr["DonorTestInfoId"].ToString(), true);
                            testResult.FirstName = dr["DonorFirstName"].ToString();
                            testResult.LastName = dr["DonorLastName"].ToString();
                            string dtt = dr["DonorTestType"].ToString();

                            Array dttResult = dtt.Split(',').Distinct().Select(r => (TestCategories)Enum.Parse(typeof(TestCategories), r)).ToArray();
                            string[] dtT = dttResult.OfType<object>().Select(o => o.ToString()).ToArray();
                            op_Dtt = dtT.Aggregate((x, y) => x + " & " + y);
                            testResult.StrTestType = op_Dtt;

                            //testResult.TestType = ParseFlagsEnum(dr["DonorTestType"].ToString()); 
                            testResult.TestType = TestCategories.None;// dr["DonorTestType"] != DBNull.Value ? (TestCategories)(Convert.ToInt32(dr["DonorTestType"].ToString())) : TestCategories.None;
                            testResult.FormType = dr["DonorFormType"] != DBNull.Value ? (SpecimenFormType)(Convert.ToInt32(dr["DonorFormType"].ToString())) : SpecimenFormType.None;
                            testResult.StrFormType = dr["DonorFormType"] != DBNull.Value ? Helper.GetEnumDescription((SpecimenFormType)(Convert.ToInt32(dr["DonorFormType"]))) : Helper.GetEnumDescription((SpecimenFormType)0);
                            testResultList.Add(testResult);
                        }
                    }
                }
                Session["testResultList"] = testResultList;
                Session["SortingValidation"] = "1";
                Session["SortingValidation1"] = null;
                LoadSearchKey();
                return View(testResultList);
            }
            else
            {
                testResultList = (List<TestResultDataModel>)Session["testResultList"];
                Session["SortingValidation"] = "1";
                Session["SortingValidation1"] = null;
                LoadSearchKey();
                return View(testResultList);
            }
        }

        [HttpGet]
        [SessionValidateAttribute]
        public ActionResult VendorDashboard()
        {
            Session["testResultList"] = null;
            Session["SortingValidation"] = null;
            Session["SortingValidation1"] = null;
            //string searchValue = frmCollection["hidSearchValue"].ToString();
            //string searchKey = frmCollection["hidSearchKey"].ToString();            

            Dictionary<string, string> searchPrm = new Dictionary<string, string>();

            List<TestResultDataModel> testResultList = new List<TestResultDataModel>();

            string sql = string.Empty;
            //if (searchKey == "1")
            //{
            //    sql = "donor_test_info.schedule_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
            //}
            //else if (searchKey == "2")
            //{
            //    sql = "donor_test_info.schedule_date = '" + searchValue + "'";
            //}
            //else
            //{


            int collectionSiteUserId = Convert.ToInt32(Convert.ToInt32(Session["VendorId"].ToString()));


            sql = " users.vendor_id = " + collectionSiteUserId + " ";
            //}
            searchPrm.Add("searchSql", sql);

            DataTable dtDonors = donorBL.SearchFromVendorDashboard(searchPrm);
            var op_Dtt = "";
            foreach (DataRow dr in dtDonors.Rows)
            {
                if (dr["DonorId"].ToString() != null)
                {
                    if (dr["DonorId"].ToString() != "0")
                    {
                        TestResultDataModel testResult = new TestResultDataModel();

                        testResult.DonorId = UserAuthentication.Encrypt(dr["DonorId"].ToString(), true);
                        testResult.DonorTestInfoId = UserAuthentication.Encrypt(dr["DonorTestInfoId"].ToString(), true);
                        testResult.FirstName = dr["DonorFirstName"].ToString();
                        testResult.LastName = dr["DonorLastName"].ToString();
                        string dtt = dr["DonorTestType"].ToString();
                        Array dttResult = dtt.Split(',').Distinct().Select(r => (TestCategories)Enum.Parse(typeof(TestCategories), r)).ToArray();
                        string[] dtT = dttResult.OfType<object>().Select(o => o.ToString()).ToArray();
                        op_Dtt = dtT.Aggregate((x, y) => x + " & " + y);
                        testResult.StrTestType = op_Dtt;

                        //testResult.TestType = ParseFlagsEnum(dr["DonorTestType"].ToString()); 
                        //testResult.TestType = TestCategories.None;// dr["DonorTestType"] != DBNull.Value ? (TestCategories)(Convert.ToInt32(dr["DonorTestType"].ToString())) : TestCategories.None;
                        testResult.FormType = dr["DonorFormType"] != DBNull.Value ? (SpecimenFormType)(Convert.ToInt32(dr["DonorFormType"].ToString())) : SpecimenFormType.None;
                        testResult.StrFormType = dr["DonorFormType"] != DBNull.Value ? Helper.GetEnumDescription((SpecimenFormType)(Convert.ToInt32(dr["DonorFormType"]))) : Helper.GetEnumDescription((SpecimenFormType)0);
                        testResultList.Add(testResult);
                    }
                }
            }

            ViewBag.SuccessMsg = "";

            if (Session["SuccessMessage"] != null)
            {
                ViewBag.SuccessMsg = Session["SuccessMessage"].ToString();
                Session["SuccessMessage"] = "";
            }
            if (Session["TestInfoSuccess"] != null)
            {
                if (!string.IsNullOrEmpty(Session["TestInfoSuccess"].ToString()))
                {
                    ViewBag.SuccessMsg = "Test info updated successfully";
                    Session["TestInfoSuccess"] = "";
                }
            }
            return View(testResultList);
        }

        [HttpPost]
        [SessionValidateAttribute]
        public ActionResult VendorDashboard(FormCollection frmCollection)
        {
            Session["testResultList"] = null;
            Session["SortingValidation"] = null;
            Session["SortingValidation1"] = null;
            string searchValue = frmCollection["hidSearchValue"].ToString();
            string searchKey = frmCollection["hidSearchKey"].ToString();
            Dictionary<string, string> searchPrm = new Dictionary<string, string>();

            List<TestResultDataModel> testResultList = new List<TestResultDataModel>();

            string sql = string.Empty;
            if (searchKey == "1")
            {
                sql = "donor_test_info.schedule_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
            }
            else if (searchKey == "2")
            {
                sql = "donor_test_info.schedule_date = '" + searchValue + "'";
            }
            else
            {
                sql = " 1=1 ";
            }
            searchPrm.Add("searchSql", sql);

            DataTable dtDonors = donorBL.SearchFromVendorDashboard(searchPrm);
            var op_Dtt = "";
            foreach (DataRow dr in dtDonors.Rows)
            {
                if (dr["DonorId"].ToString() != null)
                {
                    if (dr["DonorId"].ToString() != "0")
                    {
                        TestResultDataModel testResult = new TestResultDataModel();

                        testResult.DonorId = UserAuthentication.Encrypt(dr["DonorId"].ToString(), true);
                        testResult.DonorTestInfoId = UserAuthentication.Encrypt(dr["DonorTestInfoId"].ToString(), true);
                        testResult.FirstName = dr["DonorFirstName"].ToString();
                        testResult.LastName = dr["DonorLastName"].ToString();
                        string dtt = dr["DonorTestType"].ToString();
                        Array dttResult = dtt.Split(',').Distinct().Select(r => (TestCategories)Enum.Parse(typeof(TestCategories), r)).ToArray();
                        string[] dtT = dttResult.OfType<object>().Select(o => o.ToString()).ToArray();
                        op_Dtt = dtT.Aggregate((x, y) => x + " & " + y);
                        testResult.StrTestType = op_Dtt;

                        //testResult.TestType = ParseFlagsEnum(dr["DonorTestType"].ToString()); 
                        //testResult.TestType = TestCategories.None;// dr["DonorTestType"] != DBNull.Value ? (TestCategories)(Convert.ToInt32(dr["DonorTestType"].ToString())) : TestCategories.None;
                        testResult.FormType = dr["DonorFormType"] != DBNull.Value ? (SpecimenFormType)(Convert.ToInt32(dr["DonorFormType"].ToString())) : SpecimenFormType.None;
                        testResultList.Add(testResult);
                    }
                }
            }

            return View(testResultList);
        }

        [HttpGet]
        [SessionValidateAttribute]
        public ActionResult TestInfo(string donorTestInfoId)
        {
            Session["testResultList"] = null;
            Session["SortingValidation"] = null;
            Session["SortingValidation1"] = null;
            PrepareTestingAuthority();
            string strDonorTestInfo = Session["model.DonorTestInfoId"] != null ? Session["model.DonorTestInfoId"].ToString() : "";
            if (string.IsNullOrEmpty(strDonorTestInfo))
            {
                return RedirectToAction("VendorDashboard", "Vendor");
            }

            int intDonorTestInfoId = !string.IsNullOrEmpty(strDonorTestInfo) ? Convert.ToInt32(UserAuthentication.Decrypt(strDonorTestInfo, true)) : 0;
            var donorTestInfo = donorBL.GetDonorTestInfo(intDonorTestInfoId);

            if (donorTestInfo != null)
            {
                var model = new TestInfoDataModel
                {
                    DonorTestInfoId = donorTestInfo.DonorTestInfoId,
                    DonorId = donorTestInfo.DonorId,
                    ClientId = donorTestInfo.ClientId,
                    ClientDepartmentId = donorTestInfo.ClientDepartmentId,
                    MROTypeId = donorTestInfo.MROTypeId,
                    PaymentTypeId = donorTestInfo.PaymentTypeId,
                    TestRequestedDate = donorTestInfo.TestRequestedDate,
                    TestRequestedBy = donorTestInfo.TestRequestedBy,
                    IsUA = donorTestInfo.IsUA,
                    IsHair = donorTestInfo.IsHair,
                    IsDNA = donorTestInfo.IsDNA,
                    ReasonForTestId = donorTestInfo.ReasonForTestId,
                    OtherReason = donorTestInfo.OtherReason,
                    IsTemperatureInRange = donorTestInfo.IsTemperatureInRange,
                    TemperatureOfSpecimen = donorTestInfo.TemperatureOfSpecimen,
                    TestingAuthorityId = donorTestInfo.TestingAuthorityId,
                    SpecimenCollectionCupId = donorTestInfo.SpecimenCollectionCupId,
                    IsObserved = donorTestInfo.IsObserved,
                    FormTypeId = donorTestInfo.FormTypeId,
                    IsAdulterationSign = donorTestInfo.IsAdulterationSign,
                    IsQuantitySufficient = donorTestInfo.IsQuantitySufficient,
                    CollectionSiteVendorId = donorTestInfo.CollectionSiteVendorId,
                    CollectionSiteLocationId = donorTestInfo.CollectionSiteLocationId,
                    CollectionSiteUserId = donorTestInfo.CollectionSiteUserId,
                    ScreeningTime = donorTestInfo.ScreeningTime,
                    IsDonorRefused = donorTestInfo.IsDonorRefused,
                    LaboratoryVendorId = donorTestInfo.LaboratoryVendorId,
                    MROVendorId = donorTestInfo.MROVendorId,
                    TestStatus = donorTestInfo.TestStatus,
                    IsSurscanDeterminesDates = donorTestInfo.IsSurscanDeterminesDates,
                    IsTpDeterminesDates = donorTestInfo.IsTpDeterminesDates,
                    ProgramStartDate = donorTestInfo.ProgramStartDate,
                    ProgramEndDate = donorTestInfo.ProgramEndDate,
                    CaseNumber = donorTestInfo.CaseNumber,
                    CourtId = donorTestInfo.CourtId,
                    JudgeId = donorTestInfo.JudgeId,
                    SpecialNotes = donorTestInfo.SpecialNotes,
                    TotalPaymentAmount = donorTestInfo.TotalPaymentAmount,
                    PaymentDate = donorTestInfo.PaymentDate,
                    PaymentMethodId = donorTestInfo.PaymentMethodId,
                    PaymentNote = donorTestInfo.PaymentNote,
                    PaymentStatus = donorTestInfo.PaymentStatus,
                    LaboratoryCost = donorTestInfo.LaboratoryCost,
                    MROCost = donorTestInfo.MROCost,
                    CupCost = donorTestInfo.CupCost,
                    ShippingCost = donorTestInfo.ShippingCost,
                    VendorCost = donorTestInfo.VendorCost,
                    CollectionSite1Id = donorTestInfo.CollectionSite1Id,
                    CollectionSite2Id = donorTestInfo.CollectionSite2Id,
                    CollectionSite3Id = donorTestInfo.CollectionSite3Id,
                    CollectionSite4Id = donorTestInfo.CollectionSite4Id,
                    ScheduleDate = donorTestInfo.ScheduleDate,
                    IsSynchronized = donorTestInfo.IsSynchronized,
                    CreatedOn = donorTestInfo.CreatedOn,
                    CreatedBy = donorTestInfo.CreatedBy,
                    LastModifiedOn = donorTestInfo.LastModifiedOn,
                    LastModifiedBy = donorTestInfo.LastModifiedBy,
                    StrReasonForTest = donorTestInfo.ReasonForTestId != null ? Helper.GetEnumDescription((TestInfoReasonForTest)donorTestInfo.ReasonForTestId) : Helper.GetEnumDescription((SpecimenFormType)0),
                    StrSpecimenCollectionCup = donorTestInfo.SpecimenCollectionCupId != null ? Helper.GetEnumDescription((SpecimenCollectionCupType)donorTestInfo.SpecimenCollectionCupId) : Helper.GetEnumDescription((SpecimenCollectionCupType)0),
                    StrFormType = donorTestInfo.FormTypeId != null ? Helper.GetEnumDescription((SpecimenFormType)donorTestInfo.FormTypeId) : Helper.GetEnumDescription((SpecimenFormType)0),
                    TestingAuthorityName = donorTestInfo.TestingAuthorityName
                };

                //Collection Site Information
                int collectionSiteUserId = Convert.ToInt32(Session["UserId"].ToString());

                if (donorTestInfo.CollectionSiteUserId != null)
                {
                    collectionSiteUserId = Convert.ToInt32(donorTestInfo.CollectionSiteUserId);
                }

                UserBL userBL = new UserBL();
                User user = userBL.Get(collectionSiteUserId);

                model.CollectorName = user.UserFirstName + " " + user.UserLastName;

                if (user.UserType == UserType.Vendor)
                {
                    VendorBL vendorBL = new VendorBL();
                    Vendor vendor = vendorBL.Get(Convert.ToInt32(user.VendorId));
                    if (vendor != null)
                    {
                        model.LocationName = vendor.VendorName;
                        foreach (VendorAddress address in vendor.Addresses)
                        {
                            if (address.AddressTypeId == AddressTypes.PhysicalAddress1)
                            {
                                model.CollectionAddress1 = address.Address1;
                                model.CollectionAddress2 = address.Address2;
                                model.CollectionCity = address.City;
                                model.CollectionState = address.State;
                                model.CollectionZipCode = address.ZipCode;
                                model.CollectionPhone = address.Phone;
                                model.CollectionFax = address.Fax;
                                model.CollectionEmail = address.Email;

                                break;
                            }
                        }
                    }
                }

                model.TestInfoTestCategories = new List<TestInfoTestCategoriesDataModel>();
                foreach (DonorTestInfoTestCategories testCategory in donorTestInfo.TestInfoTestCategories)
                {

                    var testModel = new TestInfoTestCategoriesDataModel
                    {
                        DonorTestTestCategoryId = testCategory.DonorTestTestCategoryId,
                        DonorTestInfoId = testCategory.DonorTestInfoId,
                        TestCategoryId = testCategory.TestCategoryId,
                        TestPanelId = testCategory.TestPanelId,
                        SpecimenId = testCategory.SpecimenId,
                        HairTestPanelDays = testCategory.HairTestPanelDays,
                        TestPanelResult = testCategory.TestPanelResult,
                        TestPanelStatus = testCategory.TestPanelStatus,
                        TestPanelName = testCategory.TestPanelName,
                        TestPanelCost = testCategory.TestPanelCost,
                        TestPanelPrice = testCategory.TestPanelPrice
                    };

                    model.TestInfoTestCategories.Add(testModel);
                }
                return View(model);
            }

            return View();
        }

        [HttpPost]
        [SessionValidateAttribute]
        public ActionResult TestInfo(TestInfoDataModel donorTestInfo, FormCollection frmCollection)
        {
            Session["testResultList"] = null;
            Session["SortingValidation"] = null;
            Session["SortingValidation1"] = null;
            PrepareTestingAuthority();

            string temperatureOfSpecimen = frmCollection["rdTemperatureOfSpecimen"] != null ? frmCollection["rdTemperatureOfSpecimen"].ToString() : "";
            string adulteration = frmCollection["rdAdulteration"] != null ? frmCollection["rdAdulteration"].ToString() : "";
            string sufficent = frmCollection["rdSufficent"] != null ? frmCollection["rdSufficent"].ToString() : "";

            string hidDonorRefused = frmCollection["hidDonorRefused"] != null ? frmCollection["hidDonorRefused"].ToString() : "";

            string refuessReason = string.Empty;


            ////Temperature
            if (temperatureOfSpecimen == "temperatureOfSpecimenYes")
            {
                donorTestInfo.IsTemperatureInRange = YesNo.Yes;
            }
            else if (temperatureOfSpecimen == "temperatureOfSpecimenNo")
            {
                donorTestInfo.IsTemperatureInRange = YesNo.No;
                donorTestInfo.TemperatureOfSpecimen = Convert.ToDouble(donorTestInfo.TemperatureOfSpecimen);
            }
            else
            {
                donorTestInfo.IsTemperatureInRange = YesNo.None;
                donorTestInfo.TemperatureOfSpecimen = null;
            }

            ////Adulteration
            if (adulteration == "adulterationYes")
            {
                donorTestInfo.IsAdulterationSign = YesNo.Yes;
            }
            else if (adulteration == "adulterationNo")
            {
                donorTestInfo.IsAdulterationSign = YesNo.No;
            }
            else
            {
                donorTestInfo.IsAdulterationSign = YesNo.None;
            }

            ////Sufficient Quantity
            if (sufficent == "QuantitySufficentYes")
            {
                donorTestInfo.IsQuantitySufficient = YesNo.Yes;
            }
            else if (sufficent == "QuantitySufficentNo")
            {
                donorTestInfo.IsQuantitySufficient = YesNo.No;
            }
            else
            {
                donorTestInfo.IsQuantitySufficient = YesNo.None;
            }

            ////Test Info Status and final call
            donorTestInfo.IsDonorRefused = false;
            donorTestInfo.CollectionSiteUserId = null;
            donorTestInfo.CollectionSiteVendorId = null;
            donorTestInfo.CollectionSiteLocationId = null;

            donorTestInfo.CollectionSiteUserId = Convert.ToInt32(Session["UserId"].ToString());

            UserBL userBL = new UserBL();
            User user = userBL.Get(Convert.ToInt32(Session["UserId"].ToString()));
            if (user.UserType == UserType.Vendor)
            {
                donorTestInfo.CollectionSiteVendorId = user.VendorId;

                VendorBL vendorBL = new VendorBL();
                Vendor vendor = vendorBL.Get(Convert.ToInt32(user.VendorId));

                foreach (VendorAddress address in vendor.Addresses)
                {
                    if (address.AddressTypeId == AddressTypes.PhysicalAddress1)
                    {
                        donorTestInfo.CollectionAddress1 = address.Address1;
                        donorTestInfo.CollectionAddress2 = address.Address2;
                        donorTestInfo.CollectionCity = address.City;
                        donorTestInfo.CollectionState = address.State;
                        donorTestInfo.CollectionZipCode = address.ZipCode;
                        donorTestInfo.CollectionPhone = address.Phone;
                        donorTestInfo.CollectionFax = address.Fax;
                        donorTestInfo.CollectionEmail = address.Email;
                        donorTestInfo.CollectionSiteLocationId = address.AddressId;
                        break;
                    }
                }
            }

            if (hidDonorRefused == "Yes")
            {
                donorTestInfo.IsDonorRefused = true;
                donorTestInfo.TestStatus = DonorRegistrationStatus.Completed;
            }
            else if (temperatureOfSpecimen == "temperatureOfSpecimenYes" && adulteration == "adulterationNo" && sufficent == "QuantitySufficentYes")
            {
                //Test Category & Test Panel
                bool IsSpecimenIdExists = false;
                string strSpecimenIdExists = string.Empty;

                foreach (TestInfoTestCategoriesDataModel testCategory in donorTestInfo.TestInfoTestCategories)
                {
                    if (testCategory.TestCategoryId == TestCategories.UA)
                    {
                        donorTestInfo.IsUA = true;
                        if (!SpecimenValidation(testCategory.SpecimenId))
                        {
                            IsSpecimenIdExists = true;
                            strSpecimenIdExists = "UA";
                        }
                    }
                    else if (testCategory.TestCategoryId == TestCategories.Hair)
                    {
                        if (!SpecimenValidation(testCategory.SpecimenId))
                        {
                            if (IsSpecimenIdExists)
                            {                               
                                strSpecimenIdExists = strSpecimenIdExists + " Hair";
                            }
                            else
                            {
                                strSpecimenIdExists = strSpecimenIdExists + ", Hair";
                            }
                            IsSpecimenIdExists = true;
                        }
                        donorTestInfo.IsHair = true;
                    }
                    else if (testCategory.TestCategoryId == TestCategories.DNA)
                    {
                        donorTestInfo.IsDNA = true;
                    }                    
                }
                if (IsSpecimenIdExists == true)
                {
                    ViewBag.SpecimenIdExists = strSpecimenIdExists + " Specimen Id already exists";
                    return View(donorTestInfo);
                }

                donorTestInfo.TestStatus = DonorRegistrationStatus.Processing;

            }
            else if (temperatureOfSpecimen == "temperatureOfSpecimenNo" || adulteration == "adulterationYes" || sufficent == "QuantitySufficentNo")
            {
                donorTestInfo.TestStatus = DonorRegistrationStatus.SuspensionQueue;
            }
            donorTestInfo.IsInstantTest = false;

            donorTestInfo.InstantTestResult = InstantTestResult.None;

            donorTestInfo.LastModifiedBy = Session["UserLoginName"].ToString();

            //  Convert from data model to entity

            DonorTestInfo donorTestInfoEntity = new DonorTestInfo();

            donorTestInfoEntity.DonorTestInfoId = donorTestInfo.DonorTestInfoId;
            donorTestInfoEntity.DonorId = donorTestInfo.DonorId;
            donorTestInfoEntity.ClientDepartmentId = donorTestInfo.ClientDepartmentId;
            donorTestInfoEntity.IsUA = donorTestInfo.IsUA;
            donorTestInfoEntity.IsHair = donorTestInfo.IsHair;
            donorTestInfoEntity.IsDNA = donorTestInfo.IsDNA;
            donorTestInfoEntity.ReasonForTestId = donorTestInfo.ReasonForTestId;
            donorTestInfoEntity.OtherReason = donorTestInfo.OtherReason;
            donorTestInfoEntity.IsTemperatureInRange = donorTestInfo.IsTemperatureInRange;
            donorTestInfoEntity.TemperatureOfSpecimen = donorTestInfo.TemperatureOfSpecimen;
            donorTestInfoEntity.TestingAuthorityId = donorTestInfo.TestingAuthorityId;
            donorTestInfoEntity.SpecimenCollectionCupId = donorTestInfo.SpecimenCollectionCupId;
            donorTestInfoEntity.IsObserved = donorTestInfo.IsObserved;
            donorTestInfoEntity.FormTypeId = donorTestInfo.FormTypeId;
            donorTestInfoEntity.IsAdulterationSign = donorTestInfo.IsAdulterationSign;
            donorTestInfoEntity.IsQuantitySufficient = donorTestInfo.IsQuantitySufficient;
            donorTestInfoEntity.CollectionSiteVendorId = donorTestInfo.CollectionSiteVendorId;
            donorTestInfoEntity.CollectionSiteLocationId = donorTestInfo.CollectionSiteLocationId;
            donorTestInfoEntity.CollectionSiteUserId = donorTestInfo.CollectionSiteUserId;
            donorTestInfoEntity.TotalPaymentAmount = donorTestInfo.TotalPaymentAmount;
            donorTestInfoEntity.ScreeningTime = DateTime.Now;
            donorTestInfoEntity.IsDonorRefused = donorTestInfo.IsDonorRefused;
            donorTestInfoEntity.TestStatus = donorTestInfo.TestStatus;
            donorTestInfoEntity.LastModifiedBy = donorTestInfo.LastModifiedBy;

            if (hidDonorRefused == "Yes")
            {
                donorTestInfo.IsDonorRefused = true;
                donorTestInfo.TestStatus = DonorRegistrationStatus.Completed;
            }
            else if (temperatureOfSpecimen == "temperatureOfSpecimenYes" && adulteration == "adulterationNo" && sufficent == "QuantitySufficentYes")
            {
                donorTestInfoEntity.TestInfoTestCategories = new List<DonorTestInfoTestCategories>();
                foreach (TestInfoTestCategoriesDataModel testCategory in donorTestInfo.TestInfoTestCategories)
                {

                    var testModel = new DonorTestInfoTestCategories
                    {
                        DonorTestTestCategoryId = testCategory.DonorTestTestCategoryId,
                        DonorTestInfoId = testCategory.DonorTestInfoId,
                        TestCategoryId = testCategory.TestCategoryId,
                        TestPanelId = testCategory.TestPanelId,
                        SpecimenId = testCategory.SpecimenId,
                        TestPanelCost = testCategory.TestPanelCost,
                        TestPanelPrice = testCategory.TestPanelPrice,
                        HairTestPanelDays = testCategory.HairTestPanelDays
                    };

                    donorTestInfoEntity.TestInfoTestCategories.Add(testModel);
                }
            }
            try
            {
                donorBL.SaveTestInfoDetails(donorTestInfoEntity);
            }
            catch (Exception ex)
            {
                ViewBag.ServerErr = "Sorry an error occurred while processing your request...";
                return View(donorTestInfo);
            }
            Session["TestInfoSuccess"] = "TNSuccess";
            return RedirectToAction("VendorDashboard", "Vendor");
        }

        private void PrepareTestingAuthority()
        {
            TestingAuthorityBL testingAuthorityBL = new TestingAuthorityBL();

            var testingAuthority = testingAuthorityBL.GetList();

            if (testingAuthority != null)
            {
                List<SelectListItem> listTestingAuthority = new List<SelectListItem>();

                listTestingAuthority.Add(new SelectListItem { Text = "Select Authority", Value = "0", Selected = true });

                foreach (var item in testingAuthority)
                {
                    listTestingAuthority.Add(new SelectListItem
                    {
                        Text = item.TestingAuthorityName,
                        Value = UserAuthentication.Encrypt(item.TestingAuthorityId.ToString(), true)
                    });
                }

                ViewBag.TestingAuthority = listTestingAuthority;
            }

            List<SelectListItem> listNoOfDays = new List<SelectListItem>();

            listNoOfDays.Add(new SelectListItem { Text = "Select Days", Value = "0", Selected = false });
            listNoOfDays.Add(new SelectListItem { Text = "90", Value = "90", Selected = true });
            listNoOfDays.Add(new SelectListItem { Text = "180", Value = "180", Selected = false });
            listNoOfDays.Add(new SelectListItem { Text = "270", Value = "270", Selected = false });
            listNoOfDays.Add(new SelectListItem { Text = "360", Value = "360", Selected = false });
            listNoOfDays.Add(new SelectListItem { Text = "450", Value = "450", Selected = false });
            listNoOfDays.Add(new SelectListItem { Text = "540", Value = "540", Selected = false });
            listNoOfDays.Add(new SelectListItem { Text = "630", Value = "630", Selected = false });
            listNoOfDays.Add(new SelectListItem { Text = "720", Value = "720", Selected = false });

            ViewBag.ListNoOfDays = listNoOfDays;
        }

        private void LoadSearchKey()
        {
            List<SelectListItem> searchKey = new List<SelectListItem>();
            searchKey.Add(new SelectListItem { Text = "Select", Value = "0", Selected = true });
            searchKey.Add(new SelectListItem { Text = "First Name", Value = "1" });
            searchKey.Add(new SelectListItem { Text = "Last Name", Value = "2" });
            searchKey.Add(new SelectListItem { Text = "D.O.B", Value = "3" });
            searchKey.Add(new SelectListItem { Text = "City", Value = "4" });
            searchKey.Add(new SelectListItem { Text = "Zip Code", Value = "5" });
            searchKey.Add(new SelectListItem { Text = "Email Address", Value = "6" });
            ViewBag.searchKey = searchKey;
        }

        private bool SpecimenValidation(string specimenId)
        {
            DataTable dtspecimen = donorBL.GetSpecimenId(specimenId);
            if (dtspecimen.Rows.Count >= 1)
            {
                return false;
            }           
            return true;
        }
    }


}

















































































































