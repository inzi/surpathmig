using Microsoft.VisualStudio.TestTools.UnitTesting;
using Surpath.ClearStar.BL;
using System.IO;
using System.Xml;

namespace Surpath.CSTest.Tests
{
    [TestClass()]
    public class ProfileBLTests
    {
        [TestMethod()]
        public void AddProfileToCustomer()
        {
            var creds = DefaultCredentialsBL.GetCredentials();
            string sCustID = "SLSS_00001";

            Models.ProfileModel p = new Models.ProfileModel();

            p.FirstName = "Michael";
            p.LastName = "Kearl";
            p.Address1 = "4805 Carolina Circle";
            p.BirthDate = "04/07/1968";
            p.City = "McKinney";
            p.Zip = "75071";
            p.SSN = "646050676";
            p.State = "TX";
            p.Sex = "M";
            p.Phone = "8014049676";
            p.MiddleName = "Leroy";
            p.Weight = "230";
            p.Eyes = "Blue";
            p.Comments = "none";
            p.RaceId = "";
            p.Height = "";
            p.Scars = "";
            p.Address2 = "";
            p.County = "";

            Surpath.ClearStar.BL.ProfileBL profile = new ProfileBL();
            XmlNode node = profile.CreateProfileForCountry(p, sCustID);

            Assert.AreEqual("SLSS_00001", sCustID);
        }

        [TestMethod()]
        public void AddServiceToProfile()
        {
            var creds = DefaultCredentialsBL.GetCredentials();
            string sCustID = "SLSS_00001";
            string ProfileNo = "2018070902779947";
            //ProfileNo = string.Empty;
            string ServiceNo = "SLSS_10092";//background check

            //string ServiceNo = "SLSS_00001";//background check
            Surpath.ClearStar.BL.ProfileBL profile = new ProfileBL();
            XmlNode node = profile.AddServicetoProfile(sCustID, ProfileNo, ServiceNo);

            Assert.AreEqual("SLSS_00001", sCustID);
        }

        [TestMethod]
        public void TransmitProfile()
        {
            var creds = DefaultCredentialsBL.GetCredentials();
            string sCustID = "SLSS_00001";
            string ProfileNo = "2018070902779947";
            Surpath.ClearStar.BL.ProfileBL profile = new ProfileBL();
            XmlNode node = profile.TransmitProfile(sCustID, ProfileNo);

            Assert.AreEqual("SLSS_00001", sCustID);
        }

        [TestMethod]
        public void UploadProfileDocument()
        {
            var cred = DefaultCredentialsBL.GetCredentials();
            string sCustID = "SLSS_00001";
            string ProfileNo = "2018070902779947";
            ClearStar.BL.DocumentBL d = new DocumentBL();
            Models.DocumentModel doc = new Models.DocumentModel();

            doc.FileId = 1;
            doc.sOrderIdDocCopiedTo = "73017312";
            doc.FileType = "Profile";
            doc.ContentType = "application/pdf";
            doc.SecurityLevel = 1;
            doc.Description = "Concent Document";
            doc.IncludeInReport = "";
            doc.FileName = "Concent.pdf";
            bool exists = File.Exists(@"c:\temp\1.pdf");

            if (exists)
            {
                byte[] bytes = File.ReadAllBytes(@"c:\temp\1.pdf");
                doc.bytes = bytes;
            }

            XmlNode docresp = d.UploadProfileDocument3(doc, sCustID, ProfileNo);

            Assert.AreEqual("SLSS_00001", sCustID);
        }

        [TestMethod]
        public void CancelProfile()
        {
            var creds = DefaultCredentialsBL.GetCredentials();
            string sCustID = "SLSS_00001";
            string ProfileNo = "2018070872692290";
            Surpath.ClearStar.BL.ProfileBL profile = new ProfileBL();
            XmlNode node = profile.CancelProfile(sCustID, ProfileNo);

            Assert.AreEqual("SLSS_00001", sCustID);
        }

        [TestMethod()]
        public void UpdateService()
        {
            var creds = DefaultCredentialsBL.GetCredentials();
            string sCustID = "SLSS_00001";
            string ProfileNo = "2018070881661510";
            //ProfileNo = string.Empty;
            string ServiceNo = "SLSS_10092";//background check

            //string ServiceNo = "SLSS_00001";//background check
            Surpath.ClearStar.BL.ProfileBL profile = new ProfileBL();
            XmlNode node = profile.AddServicetoProfile(sCustID, ProfileNo, ServiceNo);

            Assert.AreEqual("SLSS_00001", sCustID);
        }

        [TestMethod()]
        public void GetDocument()
        {
            var creds = DefaultCredentialsBL.GetCredentials();
            string sCustID = "SLSS_00001";
            string ProfileNo = "2018072452862163";//Becca D Test
            //ProfileNo = string.Empty;
            string ServiceNo = "SLSS_10092";//background check

            //string ServiceNo = "SLSS_00001";//background check
            Surpath.ClearStar.BL.ProfileBL profile = new ProfileBL();
            var result = profile.GetProfileReport(sCustID, ProfileNo);
            var success = profile.SaveProfileReport(sCustID, ProfileNo, result);

            Assert.AreEqual(success, true);
        }
    }
}