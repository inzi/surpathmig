using SurPathWeb.Filters;
using SurPathWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace SurPathWeb.Controllers
{
    public partial class DonorController : Controller
    {
        [HttpGet]
        [SessionValidateAttribute]
        public ActionResult UserProfile()
        {
            try
            {
                int donorID = Convert.ToInt32(Session["DonorId"].ToString());

                var donorData = donorBL.Get(donorID, "Web");

                if (donorData != null)
                {
                    var model = new DonorProfileDataModel
                    {
                        EmailID = donorData.DonorEmail,
                        SSN = donorData.DonorSSN,
                        FirstName = donorData.DonorFirstName,
                        MiddleInitial = donorData.DonorMI,
                        LastName = donorData.DonorLastName,
                        Suffix = donorData.DonorSuffix,
                        DonorDOBMonth = donorData.DonorDateOfBirth.Month.ToString(),
                        DonorDOBDate = donorData.DonorDateOfBirth.Day.ToString(),
                        DonorDOBYear = donorData.DonorDateOfBirth.Year.ToString(),
                        Address1 = donorData.DonorAddress1,
                        Address2 = donorData.DonorAddress2,
                        City = donorData.DonorCity,
                        State = donorData.DonorState,
                        ZipCode = donorData.DonorZip,
                        Phone1 = donorData.DonorPhone1,
                        Phone2 = donorData.DonorPhone2,
                        Gender = donorData.DonorGender
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
            int donorID = Convert.ToInt32(Session["DonorId"].ToString());

            var donorData = donorBL.Get(donorID, "Web");

            if (donorData != null)
            {

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
                    Session["UserName"] = donorData.DonorFirstName + " " + donorData.DonorLastName;
                    Session["SuccessMessage"] = "Your profile updated successfully";
                    return Redirect("ProgramSelection");
                }
                else
                {
                    ViewBag.ServerErr = "Unable to process your request";
                }
            }
            return View(model);
        }
    }
}