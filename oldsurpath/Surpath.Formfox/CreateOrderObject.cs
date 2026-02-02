/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */
using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Surpath.Formfox
{
    [XmlRoot(ElementName = "PersonName")]
    public class PersonName
    {
        [XmlElement(ElementName = "GivenName")]
        public string GivenName { get; set; }
        [XmlElement(ElementName = "MiddleName")]
        public string MiddleName { get; set; }
        [XmlElement(ElementName = "FamilyName")]
        public string FamilyName { get; set; }
    }

    [XmlRoot(ElementName = "Gender")]
    public class Gender
    {
        [XmlElement(ElementName = "IdValue")]
        public string IdValue { get; set; }
    }

    [XmlRoot(ElementName = "Telephone")]
    public class Telephone
    {
        [XmlElement(ElementName = "FormattedNumber")]
        public string FormattedNumber { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
    }

    [XmlRoot(ElementName = "ContactMethod")]
    public class ContactMethod
    {
        [XmlElement(ElementName = "Telephone")]
        public List<Telephone> Telephone { get; set; }
        [XmlElement(ElementName = "email")]
        public string Email { get; set; }
        [XmlElement(ElementName = "ContactName")]
        public string ContactName { get; set; }
        [XmlElement(ElementName = "Fax")]
        public Fax Fax { get; set; }
    }

    [XmlRoot(ElementName = "DeliveryAddress")]
    public class DeliveryAddress
    {
        [XmlElement(ElementName = "AddressLine")]
        public List<string> AddressLine { get; set; }
    }

    [XmlRoot(ElementName = "PostalAddress")]
    public class PostalAddress
    {
        [XmlElement(ElementName = "CountryCode")]
        public string CountryCode { get; set; }
        [XmlElement(ElementName = "PostalCode")]
        public string PostalCode { get; set; }
        [XmlElement(ElementName = "Region")]
        public string Region { get; set; }
        [XmlElement(ElementName = "Municipality")]
        public string Municipality { get; set; }
        [XmlElement(ElementName = "DeliveryAddress")]
        public DeliveryAddress DeliveryAddress { get; set; }
    }

    [XmlRoot(ElementName = "Fax")]
    public class Fax
    {
        [XmlElement(ElementName = "FormattedNumber")]
        public string FormattedNumber { get; set; }
    }

    [XmlRoot(ElementName = "Company")]
    public class Company
    {
        [XmlElement(ElementName = "IdValue")]
        public string IdValue { get; set; }
        [XmlElement(ElementName = "IdName")]
        public string IdName { get; set; }
        [XmlElement(ElementName = "PostalAddress")]
        public PostalAddress PostalAddress { get; set; }
        [XmlElement(ElementName = "ContactMethod")]
        public ContactMethod ContactMethod { get; set; }
        [XmlElement(ElementName = "FormFoxID")]
        public string FormFoxID { get; set; }
    }

    [XmlRoot(ElementName = "Location")]
    public class Location
    {
        [XmlElement(ElementName = "IdValue")]
        public string IdValue { get; set; }
        [XmlElement(ElementName = "IdName")]
        public string IdName { get; set; }
    }

    [XmlRoot(ElementName = "DemographicDetail")]
    public class DemographicDetail
    {
        [XmlElement(ElementName = "Company")]
        public Company Company { get; set; }
        [XmlElement(ElementName = "Location")]
        public Location Location { get; set; }
        [XmlElement(ElementName = "User1")]
        public string User1 { get; set; }
        [XmlElement(ElementName = "User2")]
        public string User2 { get; set; }
        [XmlElement(ElementName = "User3")]
        public string User3 { get; set; }
        [XmlElement(ElementName = "User4")]
        public string User4 { get; set; }
        [XmlElement(ElementName = "TestingOversightID")]
        public string TestingOversightID { get; set; }
    }

    [XmlRoot(ElementName = "PersonalData")]
    public class PersonalData
    {
        [XmlElement(ElementName = "PrimaryID")]
        public string PrimaryID { get; set; }
        [XmlElement(ElementName = "PrimaryIDType")]
        public string PrimaryIDType { get; set; }
        [XmlElement(ElementName = "PrimaryIDExpirationDate")]
        public string PrimaryIDExpirationDate { get; set; }
        [XmlElement(ElementName = "AlternateID")]
        public string AlternateID { get; set; }
        [XmlElement(ElementName = "AltIDType")]
        public string AltIDType { get; set; }
        [XmlElement(ElementName = "AltIDExpirationDate")]
        public string AltIDExpirationDate { get; set; }
        [XmlElement(ElementName = "DriverLicClass")]
        public string DriverLicClass { get; set; }
        [XmlElement(ElementName = "DriverLicStateofIssue")]
        public string DriverLicStateofIssue { get; set; }
        [XmlElement(ElementName = "UseStateDLIDDrugTestOnly")]
        public string UseStateDLIDDrugTestOnly { get; set; }
        [XmlElement(ElementName = "PersonName")]
        public PersonName PersonName { get; set; }
        [XmlElement(ElementName = "Gender")]
        public Gender Gender { get; set; }
        [XmlElement(ElementName = "DateofBirth")]
        public string DateofBirth { get; set; }
        [XmlElement(ElementName = "ContactMethod")]
        public ContactMethod ContactMethod { get; set; }
        [XmlElement(ElementName = "PostalAddress")]
        public PostalAddress PostalAddress { get; set; }
        [XmlElement(ElementName = "DemographicDetail")]
        public DemographicDetail DemographicDetail { get; set; }
    }

    [XmlRoot(ElementName = "AuthorizationLetter")]
    public class AuthorizationLetter
    {
        [XmlElement(ElementName = "SendToDonoremail")]
        public string SendToDonoremail { get; set; }
        [XmlElement(ElementName = "SendToDonorMobile")]
        public string SendToDonorMobile { get; set; }
    }

    [XmlRoot(ElementName = "DonorNotification")]
    public class DonorNotification
    {
        [XmlElement(ElementName = "AuthorizationLetter")]
        public AuthorizationLetter AuthorizationLetter { get; set; }
    }

    [XmlRoot(ElementName = "Account")]
    public class Account
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "level")]
        public string Level { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "AccountStructure")]
    public class AccountStructure
    {
        [XmlElement(ElementName = "Account")]
        public List<Account> Account { get; set; }
    }

    [XmlRoot(ElementName = "Marketplace")]
    public class Marketplace
    {
        [XmlElement(ElementName = "AccountStructure")]
        public AccountStructure AccountStructure { get; set; }
    }

    [XmlRoot(ElementName = "ReasonForTest")]
    public class ReasonForTest
    {
        [XmlElement(ElementName = "IdValue")]
        public string IdValue { get; set; }
        [XmlElement(ElementName = "IdName")]
        public string IdName { get; set; }
    }

    [XmlRoot(ElementName = "CSO")]
    public class CSO
    {
        [XmlElement(ElementName = "CSOnumber")]
        public string CSOnumber { get; set; }
        [XmlElement(ElementName = "CSOprompt")]
        public string CSOprompt { get; set; }
        [XmlElement(ElementName = "CSOtext")]
        public string CSOtext { get; set; }
    }

    [XmlRoot(ElementName = "CSOs")]
    public class CSOs
    {
        [XmlElement(ElementName = "CSO")]
        public CSO CSO { get; set; }
    }

    [XmlRoot(ElementName = "TestProcedure")]
    public class TestProcedure
    {
        [XmlElement(ElementName = "IdSampleType")]
        public string IdSampleType { get; set; }
        [XmlElement(ElementName = "IdTestMethod")]
        public string IdTestMethod { get; set; }
        [XmlElement(ElementName = "UseReader")]
        public string UseReader { get; set; }
        [XmlElement(ElementName = "DeviceNumber")]
        public string DeviceNumber { get; set; }
        [XmlElement(ElementName = "PositiveCutoffLevel")]
        public string PositiveCutoffLevel { get; set; }
        [XmlElement(ElementName = "ConfirmationWaitTime")]
        public string ConfirmationWaitTime { get; set; }
    }

    [XmlRoot(ElementName = "UnitCodes")]
    public class UnitCodes
    {
        [XmlElement(ElementName = "IdValue")]
        public string IdValue { get; set; }
    }

    [XmlRoot(ElementName = "LabReasonForTest")]
    public class LabReasonForTest
    {
        [XmlElement(ElementName = "IdValue")]
        public string IdValue { get; set; }
        [XmlElement(ElementName = "IdName")]
        public string IdName { get; set; }
    }

    [XmlRoot(ElementName = "Service")]
    public class Service
    {
        [XmlElement(ElementName = "DOTTest")]
        public string DOTTest { get; set; }
        [XmlElement(ElementName = "CollectorName")]
        public string CollectorName { get; set; }
        [XmlElement(ElementName = "TestingAuthority")]
        public string TestingAuthority { get; set; }
        [XmlElement(ElementName = "RequestObservation")]
        public string RequestObservation { get; set; }
        [XmlElement(ElementName = "RequestSplitSample")]
        public string RequestSplitSample { get; set; }
        [XmlElement(ElementName = "TestProcedure")]
        public TestProcedure TestProcedure { get; set; }
        [XmlElement(ElementName = "UnitCodes")]
        public UnitCodes UnitCodes { get; set; }
        [XmlElement(ElementName = "ConfirmationOnly")]
        public string ConfirmationOnly { get; set; }
        [XmlElement(ElementName = "LaboratoryID")]
        public string LaboratoryID { get; set; }
        [XmlElement(ElementName = "LaboratoryAccount")]
        public string LaboratoryAccount { get; set; }
        [XmlElement(ElementName = "LabReasonForTest")]
        public LabReasonForTest LabReasonForTest { get; set; }
        [XmlElement(ElementName = "OrderCommentsToCollector")]
        public string OrderCommentsToCollector { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
        [XmlElement(ElementName = "agreeToPay")]
        public List<string> AgreeToPay { get; set; }
        [XmlElement(ElementName = "DISAClientPolicyID")]
        public string DISAClientPolicyID { get; set; }
        [XmlElement(ElementName = "Exam")]
        public Exam Exam { get; set; }
        [XmlElement(ElementName = "IdValue")]
        public IdValue IdValue { get; set; }
        [XmlElement(ElementName = "CertificationType")]
        public string CertificationType { get; set; }
        [XmlElement(ElementName = "OrderCommentsToProvider")]
        public string OrderCommentsToProvider { get; set; }
        [XmlElement(ElementName = "TestCodes")]
        public TestCodes TestCodes { get; set; }
    }

    [XmlRoot(ElementName = "Exam")]
    public class Exam
    {
        [XmlElement(ElementName = "TestingAuthority")]
        public string TestingAuthority { get; set; }
        [XmlElement(ElementName = "CertificationType")]
        public string CertificationType { get; set; }
        [XmlElement(ElementName = "OrderCommentsToExaminer")]
        public string OrderCommentsToExaminer { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
    }

    [XmlRoot(ElementName = "IdValue")]
    public class IdValue
    {
        [XmlAttribute(AttributeName = "codeSet")]
        public string CodeSet { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "TestCodes")]
    public class TestCodes
    {
        [XmlElement(ElementName = "IdValue")]
        public string IdValue { get; set; }
    }

    [XmlRoot(ElementName = "Services")]
    public class Services
    {
        [XmlElement(ElementName = "Marketplace")]
        public Marketplace Marketplace { get; set; }
        [XmlElement(ElementName = "WhoOrderedTest")]
        public string WhoOrderedTest { get; set; }
        [XmlElement(ElementName = "DateOrdered")]
        public string DateOrdered { get; set; }
        [XmlElement(ElementName = "ScheduledDate")]
        public string ScheduledDate { get; set; }
        [XmlElement(ElementName = "ExpirationDate")]
        public string ExpirationDate { get; set; }
        [XmlElement(ElementName = "HardExpire")]
        public string HardExpire { get; set; }
        [XmlElement(ElementName = "ServiceProvider")]
        public string ServiceProvider { get; set; }
        [XmlElement(ElementName = "LockSiteChoice")]
        public string LockSiteChoice { get; set; }
        [XmlElement(ElementName = "ReasonForTest")]
        public ReasonForTest ReasonForTest { get; set; }
        [XmlElement(ElementName = "CSOs")]
        public CSOs CSOs { get; set; }
        [XmlElement(ElementName = "Service")]
        public List<Service> Service { get; set; }
    }

    [XmlRoot(ElementName = "CreateOrderTest")]
    public class CreateOrderTest
    {
        [XmlElement(ElementName = "SendingFacility")]
        public string SendingFacility { get; set; }
        [XmlElement(ElementName = "SendingFacilityID")]
        public string SendingFacilityID { get; set; }
        [XmlElement(ElementName = "SendingFacilityTimeZone")]
        public string SendingFacilityTimeZone { get; set; }
        [XmlElement(ElementName = "ClientReferenceID")]
        public string ClientReferenceID { get; set; }
        [XmlElement(ElementName = "ReferenceTestID")]
        public string ReferenceTestID { get; set; }
        [XmlElement(ElementName = "IsTemplateOrder")]
        public string IsTemplateOrder { get; set; }
        [XmlElement(ElementName = "PersonalData")]
        public PersonalData PersonalData { get; set; }
        [XmlElement(ElementName = "DonorNotification")]
        public DonorNotification DonorNotification { get; set; }
        [XmlElement(ElementName = "Services")]
        public Services Services { get; set; }
        [XmlElement(ElementName = "BillingInformation")]
        public string BillingInformation { get; set; }
    }

}
