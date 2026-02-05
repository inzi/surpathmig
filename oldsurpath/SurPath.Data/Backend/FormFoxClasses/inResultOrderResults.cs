using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SurPath.Data.Backend
{
    #region  ResultOrderResults
    /* 
     Licensed under the Apache License, Version 2.0

     http://www.apache.org/licenses/LICENSE-2.0
     */

    [XmlRoot(ElementName = "PersonName", Namespace = "https://FormFox.com")]
    public class ResultPersonName
    {
        [XmlElement(ElementName = "GivenName", Namespace = "https://FormFox.com")]
        public string GivenName { get; set; }
        [XmlElement(ElementName = "MiddleName", Namespace = "https://FormFox.com")]
        public string MiddleName { get; set; }
        [XmlElement(ElementName = "FamilyName", Namespace = "https://FormFox.com")]
        public string FamilyName { get; set; }
    }

    [XmlRoot(ElementName = "Gender", Namespace = "https://FormFox.com")]
    public class Gender
    {
        [XmlElement(ElementName = "IdValue", Namespace = "https://FormFox.com")]
        public string IdValue { get; set; }
    }

    [XmlRoot(ElementName = "Telephone", Namespace = "https://FormFox.com")]
    public class ResultTelephone
    {
        [XmlElement(ElementName = "FormattedNumber", Namespace = "https://FormFox.com")]
        public string FormattedNumber { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
        [XmlElement(ElementName = "IdValue", Namespace = "https://FormFox.com")]
        public string IdValue { get; set; }
    }

    [XmlRoot(ElementName = "ContactMethod", Namespace = "https://FormFox.com")]
    public class ResultContactMethod
    {
        [XmlElement(ElementName = "Telephone", Namespace = "https://FormFox.com")]
        public List<ResultTelephone> Telephone { get; set; } = new List<ResultTelephone>();
    }

    [XmlRoot(ElementName = "DeliveryAddress", Namespace = "https://FormFox.com")]
    public class ResultDeliveryAddress
    {
        [XmlElement(ElementName = "AddressLine", Namespace = "https://FormFox.com")]
        public List<string> AddressLine { get; set; }
    }

    [XmlRoot(ElementName = "PostalAddress", Namespace = "https://FormFox.com")]
    public class ResultPostalAddress
    {
        [XmlElement(ElementName = "PostalCode", Namespace = "https://FormFox.com")]
        public string PostalCode { get; set; }
        [XmlElement(ElementName = "Region", Namespace = "https://FormFox.com")]
        public string Region { get; set; }
        [XmlElement(ElementName = "Municipality", Namespace = "https://FormFox.com")]
        public string Municipality { get; set; }
        [XmlElement(ElementName = "DeliveryAddress", Namespace = "https://FormFox.com")]
        public ResultDeliveryAddress DeliveryAddress { get; set; } = new ResultDeliveryAddress();
        [XmlElement(ElementName = "CountryCode", Namespace = "https://FormFox.com")]
        public string CountryCode { get; set; }
    }

    [XmlRoot(ElementName = "Company", Namespace = "https://FormFox.com")]
    public class ResultCompany
    {
        [XmlElement(ElementName = "IdValue", Namespace = "https://FormFox.com")]
        public string IdValue { get; set; }
        [XmlElement(ElementName = "IdName", Namespace = "https://FormFox.com")]
        public string IdName { get; set; }
    }

    [XmlRoot(ElementName = "Location", Namespace = "https://FormFox.com")]
    public class Location
    {
        [XmlElement(ElementName = "IdValue", Namespace = "https://FormFox.com")]
        public string IdValue { get; set; }
        [XmlElement(ElementName = "IdName", Namespace = "https://FormFox.com")]
        public string IdName { get; set; }
    }

    [XmlRoot(ElementName = "DemographicDetail", Namespace = "https://FormFox.com")]
    public class ResultDemographicDetail
    {
        [XmlElement(ElementName = "Company", Namespace = "https://FormFox.com")]
        public ResultCompany Company { get; set; } = new ResultCompany();
        [XmlElement(ElementName = "Location", Namespace = "https://FormFox.com")]
        public Location Location { get; set; }
        [XmlElement(ElementName = "User1", Namespace = "https://FormFox.com")]
        public string User1 { get; set; }
        [XmlElement(ElementName = "User2", Namespace = "https://FormFox.com")]
        public string User2 { get; set; }
        [XmlElement(ElementName = "User3", Namespace = "https://FormFox.com")]
        public string User3 { get; set; }
        [XmlElement(ElementName = "User4", Namespace = "https://FormFox.com")]
        public string User4 { get; set; }
    }

    [XmlRoot(ElementName = "PersonalData", Namespace = "https://FormFox.com")]
    public class ResultPersonalData
    {
        [XmlElement(ElementName = "PrimaryId", Namespace = "https://FormFox.com")]
        public string PrimaryId { get; set; }
        [XmlElement(ElementName = "PrimaryIdType", Namespace = "https://FormFox.com")]
        public string PrimaryIdType { get; set; }
        [XmlElement(ElementName = "AlternateID", Namespace = "https://FormFox.com")]
        public string AlternateID { get; set; }
        [XmlElement(ElementName = "AltIDType", Namespace = "https://FormFox.com")]
        public string AltIDType { get; set; }
        [XmlElement(ElementName = "PersonName", Namespace = "https://FormFox.com")]
        public ResultPersonName PersonName { get; set; } = new ResultPersonName();
        [XmlElement(ElementName = "Gender", Namespace = "https://FormFox.com")]
        public Gender Gender { get; set; }
        [XmlElement(ElementName = "DateofBirth", Namespace = "https://FormFox.com")]
        public string DateofBirth { get; set; }
        [XmlElement(ElementName = "ContactMethod", Namespace = "https://FormFox.com")]
        public ResultContactMethod ContactMethod { get; set; } = new ResultContactMethod();
        [XmlElement(ElementName = "PostalAddress", Namespace = "https://FormFox.com")]
        public ResultPostalAddress PostalAddress { get; set; } = new ResultPostalAddress();
        [XmlElement(ElementName = "DemographicDetail", Namespace = "https://FormFox.com")]
        public ResultDemographicDetail DemographicDetail { get; set; } = new ResultDemographicDetail();
    }

    [XmlRoot(ElementName = "CollectionSite", Namespace = "https://FormFox.com")]
    public class CollectionSite
    {
        [XmlElement(ElementName = "SiteID", Namespace = "https://FormFox.com")]
        public string SiteID { get; set; }
        [XmlElement(ElementName = "FFSiteCode", Namespace = "https://FormFox.com")]
        public string FFSiteCode { get; set; }
        [XmlElement(ElementName = "CollectorName", Namespace = "https://FormFox.com")]
        public string CollectorName { get; set; }
        [XmlElement(ElementName = "CollectorID", Namespace = "https://FormFox.com")]
        public string CollectorID { get; set; }
        [XmlElement(ElementName = "SiteName", Namespace = "https://FormFox.com")]
        public string SiteName { get; set; }
        [XmlElement(ElementName = "SiteTimeZone", Namespace = "https://FormFox.com")]
        public string SiteTimeZone { get; set; }
        [XmlElement(ElementName = "PostalAddress", Namespace = "https://FormFox.com")]
        public PostalAddress PostalAddress { get; set; }
        [XmlElement(ElementName = "Telephone", Namespace = "https://FormFox.com")]
        public Telephone Telephone { get; set; }
    }

    [XmlRoot(ElementName = "ReasonForTest", Namespace = "https://FormFox.com")]
    public class ResultReasonForTest
    {
        [XmlElement(ElementName = "IdValue", Namespace = "https://FormFox.com")]
        public string IdValue { get; set; }
        [XmlElement(ElementName = "IdName", Namespace = "https://FormFox.com")]
        public string IdName { get; set; }
    }

    [XmlRoot(ElementName = "TestProcedure", Namespace = "https://FormFox.com")]
    public class ResultTestProcedure
    {
        [XmlElement(ElementName = "IdSampleType", Namespace = "https://FormFox.com")]
        public string IdSampleType { get; set; }
        [XmlElement(ElementName = "IdTestMethod", Namespace = "https://FormFox.com")]
        public string IdTestMethod { get; set; }
    }

    [XmlRoot(ElementName = "UnitCodes", Namespace = "https://FormFox.com")]
    public class ResultUnitCodes
    {
        [XmlElement(ElementName = "IdValue", Namespace = "https://FormFox.com")]
        public string IdValue { get; set; }
    }

    [XmlRoot(ElementName = "ImageData", Namespace = "https://FormFox.com")]
    public class ResultImageData
    {
        [XmlElement(ElementName = "ImageName", Namespace = "https://FormFox.com")]
        public string ImageName { get; set; }
        [XmlElement(ElementName = "ImageData", Namespace = "https://FormFox.com")]
        public string ImageData { get; set; }
    }

    [XmlRoot(ElementName = "ImagesData", Namespace = "https://FormFox.com")]
    public class ImagesData
    {
        [XmlElement(ElementName = "ImageData", Namespace = "https://FormFox.com")]
        public ResultImageData ImageData { get; set; }
    }

    [XmlRoot(ElementName = "Screening", Namespace = "https://FormFox.com")]
    public class Screening
    {
        [XmlElement(ElementName = "DOTTest", Namespace = "https://FormFox.com")]
        public string DOTTest { get; set; }
        [XmlElement(ElementName = "TestingAuthority", Namespace = "https://FormFox.com")]
        public string TestingAuthority { get; set; }
        [XmlElement(ElementName = "RequestSplitSample", Namespace = "https://FormFox.com")]
        public string RequestSplitSample { get; set; }
        [XmlElement(ElementName = "RequestObservation", Namespace = "https://FormFox.com")]
        public string RequestObservation { get; set; }
        [XmlElement(ElementName = "OrderCommentsToCollector", Namespace = "https://FormFox.com")]
        public string OrderCommentsToCollector { get; set; }
        [XmlElement(ElementName = "TestProcedure", Namespace = "https://FormFox.com")]
        public ResultTestProcedure TestProcedure { get; set; } = new ResultTestProcedure();
        [XmlElement(ElementName = "UnitCodes", Namespace = "https://FormFox.com")]
        public ResultUnitCodes UnitCodes { get; set; } = new ResultUnitCodes();
        [XmlElement(ElementName = "LaboratoryID", Namespace = "https://FormFox.com")]
        public string LaboratoryID { get; set; }
        [XmlElement(ElementName = "LaboratoryAccount", Namespace = "https://FormFox.com")]
        public string LaboratoryAccount { get; set; }
        [XmlElement(ElementName = "VerifiedBy", Namespace = "https://FormFox.com")]
        public string VerifiedBy { get; set; }
        [XmlElement(ElementName = "DateCollected", Namespace = "https://FormFox.com")]
        public string DateCollected { get; set; }
        [XmlElement(ElementName = "CollectionStatus", Namespace = "https://FormFox.com")]
        public string CollectionStatus { get; set; }
        [XmlElement(ElementName = "SpecimenID", Namespace = "https://FormFox.com")]
        public string SpecimenID { get; set; }
        [XmlElement(ElementName = "FormNumber", Namespace = "https://FormFox.com")]
        public string FormNumber { get; set; }
        [XmlElement(ElementName = "SplitSample", Namespace = "https://FormFox.com")]
        public string SplitSample { get; set; }
        [XmlElement(ElementName = "Observed", Namespace = "https://FormFox.com")]
        public string Observed { get; set; }
        [XmlElement(ElementName = "ScreenResult", Namespace = "https://FormFox.com")]
        public string ScreenResult { get; set; }
        [XmlElement(ElementName = "TempInRange", Namespace = "https://FormFox.com")]
        public string TempInRange { get; set; }
        [XmlElement(ElementName = "CollectorComments", Namespace = "https://FormFox.com")]
        public string CollectorComments { get; set; }
        [XmlElement(ElementName = "ImagesData", Namespace = "https://FormFox.com")]
        public ImagesData ImagesData { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
    }

    [XmlRoot(ElementName = "Screenings", Namespace = "https://FormFox.com")]
    public class Screenings
    {
        [XmlElement(ElementName = "WhoOrderedTest", Namespace = "https://FormFox.com")]
        public string WhoOrderedTest { get; set; }
        [XmlElement(ElementName = "DateOrdered", Namespace = "https://FormFox.com")]
        public string DateOrdered { get; set; }
        [XmlElement(ElementName = "ScheduledDate", Namespace = "https://FormFox.com")]
        public string ScheduledDate { get; set; }
        [XmlElement(ElementName = "CollectionSite", Namespace = "https://FormFox.com")]
        public CollectionSite CollectionSite { get; set; }
        [XmlElement(ElementName = "ReasonForTest", Namespace = "https://FormFox.com")]
        public ResultReasonForTest ReasonForTest { get; set; } = new ResultReasonForTest();
        [XmlElement(ElementName = "CSOs", Namespace = "https://FormFox.com")]
        public string CSOs { get; set; }
        [XmlElement(ElementName = "ArrivalDate", Namespace = "https://FormFox.com")]
        public string ArrivalDate { get; set; }
        [XmlElement(ElementName = "BillingParty", Namespace = "https://FormFox.com")]
        public string BillingParty { get; set; }
        [XmlElement(ElementName = "Screening", Namespace = "https://FormFox.com")]
        public List<Screening> Screening { get; set; } = new List<Screening>();
    }

    [XmlRoot(ElementName = "ResultOrderResults", Namespace = "https://FormFox.com")]
    public class ResultOrderResults
    {
        [XmlElement(ElementName = "SendingFacility", Namespace = "https://FormFox.com")]
        public string SendingFacility { get; set; }
        [XmlElement(ElementName = "SendingFacilityTimeZone", Namespace = "https://FormFox.com")]
        public string SendingFacilityTimeZone { get; set; }
        [XmlElement(ElementName = "ClientReferenceID", Namespace = "https://FormFox.com")]
        public string ClientReferenceID { get; set; }
        [XmlElement(ElementName = "MailID", Namespace = "https://FormFox.com")]
        public string MailID { get; set; }
        [XmlElement(ElementName = "ReferenceTestID", Namespace = "https://FormFox.com")]
        public string ReferenceTestID { get; set; }
        [XmlElement(ElementName = "PersonalData", Namespace = "https://FormFox.com")]
        public ResultPersonalData PersonalData { get; set; } = new ResultPersonalData();
        [XmlElement(ElementName = "Screenings", Namespace = "https://FormFox.com")]
        public Screenings Screenings { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }


    #endregion ResultOrderResults


}
