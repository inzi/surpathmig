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

namespace SurPathWeb.Controllers
{
    
    public class DocController : Controller
    {
        DonorBL donorBL = new DonorBL();
        ClientBL clientBL = new ClientBL();

        [HttpGet]
        [ActionName("DocumentManage")]
        [SessionValidateAttribute]
        public ActionResult DocumentManage(int DonorId, int ClientId, bool DocumentManager)
        {
            ViewBag.DocumentActive = "active";
            int donorId = DonorId;// Convert.ToInt32(Session["DonorId"]);

            List<DonorDocument> documentlist = new List<DonorDocument>();
            List<DonorProfileDataModel> document = new List<DonorProfileDataModel>();
            // Maybe check if logged in user is donor, if so hide system

            bool HideFilesFromSystem = true;
            // Session["UserType"] = UserType.Donor;
            if (UserType.Donor == (UserType)Session["UserType"])
            {
             //   HideFilesFromSystem = false;

            }
            var items = donorBL.GetDonorDocumentList(donorId, HideFilesFromSystem);
            foreach (DonorDocument dd in items)
            {
                var documentdata = new DonorProfileDataModel
                {
                    DocumentID = dd.DonorDocumentId.ToString(),
                    DocumentTitle = dd.DocumentTitle.ToString(),
                    uploaddate = Convert.ToDateTime(dd.DocumentUploadTime).ToString("MM/dd/yyyy"),
                    uploadtime = dd.DocumentUploadTime.TimeOfDay
                };
                document.Add(documentdata);
            }
            GetPeymentMethod();
            return View(document);
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
    }
}