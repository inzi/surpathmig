using Surpath.CSTest.Models;
using Surpath.CSTest.Profile;
using System;
using System.Configuration;
using System.Xml;

namespace Surpath.ClearStar.BL
{
    public class ProfileBL
    {
        public XmlNode CreateProfileForCountry(ProfileModel Prof, string CustId)
        {
            var cred = DefaultCredentialsBL.GetCredentials();
            Surpath.CSTest.Profile.Profile p = new Profile();
            Surpath.CSTest.Document.Document d = new CSTest.Document.Document();
            //Set all properties
            int PriorityId = 0;// ToDo: need values for the following
            string sPosition = string.Empty;

            string sAccountingCode = "";
            string sFromDate = DateTime.Now.ToShortDateString();
            string sSubjectType = "";
            string FolderId = "";
            bool bIsHighlighted = false;
            Prof.Suffix = "";

            XmlNode propresp = p.CreateProfile(cred.UserName, cred.Password, cred.BoID, CustId, FolderId, PriorityId, sPosition, sAccountingCode, Prof.SSN, Prof.LastName, Prof.FirstName, Prof.MiddleName, Prof.Suffix, Prof.RaceId, Prof.Sex, Prof.BirthDate, Prof.Height, Prof.Weight, Prof.Scars, Prof.Eyes, Prof.Address1, Prof.Address2, Prof.City, Prof.State, Prof.Zip, Prof.County, sFromDate, sSubjectType, bIsHighlighted, Prof.Comments);
            return propresp;
        }

        public void CreateInternalationalProfile(ProfileModel Prof, string CustId, string FolderId)
        {
            var cred = DefaultCredentialsBL.GetCredentials();
            Surpath.CSTest.Profile.Profile p = new Profile();

            int PriorityId = 0;// ToDo: need values for the following
            string sPosition = "";
            string sAccountingCode = "";
            string sFromDate = "";
            string sSubjectType = "";
            bool bIsHighlighted = false;

            XmlNode propresp = p.CreateInternationalProfile(cred.UserName, cred.Password, cred.BoID, CustId, FolderId, PriorityId, sPosition, sAccountingCode, Prof.SSN, Prof.LastName, Prof.FirstName, Prof.MiddleName, Prof.Suffix, Prof.RaceId, Prof.Sex, Prof.BirthDate, Prof.Height, Prof.Weight, Prof.Scars, Prof.Eyes, Prof.CountryId, Prof.Address1, Prof.Address2, Prof.City, Prof.State, Prof.Zip, Prof.County, sFromDate, sSubjectType, bIsHighlighted, Prof.Comments, Prof.Email, Prof.Phone, Prof.PassportNumber, Prof.PassportIssuingCountryId);

            //ToDo: not implemented yet.
        }

        public void NotificationRequest(string CustId, string ProfileNo)
        {
            var cred = DefaultCredentialsBL.GetCredentials();
            Surpath.CSTest.Profile.Profile p = new Profile();
            string sNotificationType = "";//Todo: need value here.
            string sNotificationValue = ""; //Todo: need value here.
            XmlNode propresp = p.NotificationRequest(cred.UserName, cred.Password, cred.BoID, CustId, ProfileNo, sNotificationType, sNotificationValue);

            //ToDo: not implemented yet.
        }

        public XmlNode AddServicetoProfile(string CustId, string ProfileNo, string ServiceNo)
        {
            var cred = DefaultCredentialsBL.GetCredentials();
            Surpath.CSTest.Profile.Profile p = new Profile();
            XmlNode propresp = p.AddServiceToProfile(cred.UserName, cred.Password, cred.BoID, CustId, ProfileNo, ServiceNo);

            return propresp;
        }

        public void AddOrderToProfile(string CustId, string ProfileNo, string ServiceNo)
        {
            var cred = DefaultCredentialsBL.GetCredentials();
            Surpath.CSTest.Profile.Profile p = new Profile();

            string sCountry = ""; //ToDo: need the following values
            string sState = "";
            string sCity = "";
            string sZip = "";
            string sInstruct = "";
            string sStage = "";
            int AliasId = 0;
            string sOrderFieldsXML = "";

            XmlNode propresp = p.AddOrderToProfile(cred.UserName, cred.Password, cred.BoID, CustId, ProfileNo, ServiceNo, sCountry, sState, sCountry, sCity, sZip, sInstruct, sStage, AliasId, sOrderFieldsXML);

            //ToDo: not implemented yet.
        }

        public XmlNode TransmitProfile(string CustId, string ProfileNo)
        {
            var cred = DefaultCredentialsBL.GetCredentials();
            Surpath.CSTest.Profile.Profile p = new Profile();

            XmlNode propresp = p.TransmitProfile(cred.UserName, cred.Password, cred.BoID, CustId, ProfileNo);

            return propresp;
        }

        public XmlNode CancelProfile(string CustId, string ProfileNo)
        {
            var cred = DefaultCredentialsBL.GetCredentials();
            Surpath.CSTest.Profile.Profile p = new Profile();

            XmlNode propresp = p.CancelProfile2(cred.UserName, cred.Password, cred.BoID, CustId, ProfileNo, "John Jackson", false, false);

            return propresp;
        }

        public byte[] GetProfileReport(string CustId, string ProfileNo)
        {
            var cred = DefaultCredentialsBL.GetCredentials();
            Surpath.CSTest.Profile.Profile p = new Profile();

            var report = p.GetProfileReport(cred.UserName, cred.Password, cred.BoID, CustId, ProfileNo, true);

            return report;
        }

        public string SaveProfileReport(string CustId, string ProfileNo, byte[] report)
        {
            bool success = false;
            var csreports = ConfigurationManager.AppSettings["ClearStarReports"].ToString().Trim();
            try
            {
                var exists = System.IO.File.Exists(csreports + ProfileNo + ".pdf");
                if (!exists)
                {
                    System.IO.File.WriteAllBytes(csreports + ProfileNo + ".pdf", report);

                    var file = System.IO.File.GetAttributes(csreports + ProfileNo + ".pdf");

                    success = true;
                }
                else
                {
                    success = true; //file already exists.
                }
            }
            catch (Exception ex)
            {
                success = false;

                //ToDO: need to write to logfile
            }
            //return System.IO.File.;

            //return FileStyleUriParser;
            return csreports + ProfileNo + ".pdf";
        }
    }
}