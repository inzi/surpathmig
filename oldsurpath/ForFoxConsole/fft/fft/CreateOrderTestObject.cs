/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace fft
{
    [XmlRoot(ElementName = "PersonName")]
    public class PersonName
    {
        [XmlElement(ElementName = "GivenName")]
        public string GivenName { get; set; }
        [XmlElement(ElementName = "FamilyName")]
        public string FamilyName { get; set; }
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
        public List<Telephone> Telephone { get; set; } = new List<Telephone>();
    }

    [XmlRoot(ElementName = "PostalAddress")]
    public class PostalAddress
    {
        [XmlElement(ElementName = "PostalCode")]
        public string PostalCode { get; set; }
        [XmlElement(ElementName = "Region")]
        public string Region { get; set; }
        [XmlElement(ElementName = "Municipality")]
        public string Municipality { get; set; }
        [XmlElement(ElementName = "DeliveryAddress")]
        public DeliveryAddress DeliveryAddress { get; set; } = new DeliveryAddress();
    }

    [XmlRoot(ElementName = "DeliveryAddress")]
    public class DeliveryAddress
    {
        [XmlElement(ElementName = "AddressLine")]
        public string AddressLine { get; set; }
    }

    [XmlRoot(ElementName = "Company")]
    public class Company
    {
        [XmlElement(ElementName = "IdValue")]
        public string IdValue { get; set; }
        [XmlElement(ElementName = "IdName")]
        public string IdName { get; set; }
        [XmlElement(ElementName = "PostalAddress")]
        public PostalAddress PostalAddress { get; set; } = new PostalAddress();
        [XmlElement(ElementName = "ContactMethod")]
        public string ContactMethod { get; set; }
    }

    [XmlRoot(ElementName = "DemographicDetail")]
    public class DemographicDetail
    {
        [XmlElement(ElementName = "Company")]
        public Company Company { get; set; } = new Company();
        [XmlElement(ElementName = "Location")]
        public string Location { get; set; }
    }

    [XmlRoot(ElementName = "PersonalData")]
    public class PersonalData
    {
        [XmlElement(ElementName = "PrimaryID")]
        public string PrimaryID { get; set; }
        [XmlElement(ElementName = "PrimaryIDType")]
        public string PrimaryIDType { get; set; }
        [XmlElement(ElementName = "AlternateID")]
        public string AlternateID { get; set; }
        [XmlElement(ElementName = "AltIDType")]
        public string AltIDType { get; set; }
        [XmlElement(ElementName = "PersonName")]
        public PersonName PersonName { get; set; } = new PersonName();
        [XmlElement(ElementName = "Gender")]
        public string Gender { get; set; }
        [XmlElement(ElementName = "DateofBirth")]
        public string DateofBirth { get; set; }
        [XmlElement(ElementName = "ContactMethod")]
        public ContactMethod ContactMethod { get; set; } = new ContactMethod();
        [XmlElement(ElementName = "PostalAddress")]
        public PostalAddress PostalAddress { get; set; } = new PostalAddress();
        [XmlElement(ElementName = "DemographicDetail")]
        public DemographicDetail DemographicDetail { get; set; } = new DemographicDetail();
    }

    [XmlRoot(ElementName = "ReasonForTest")]
    public class ReasonForTest
    {
        [XmlElement(ElementName = "IdValue")]
        public string IdValue { get; set; }
        [XmlElement(ElementName = "IdName")]
        public string IdName { get; set; }
    }

    [XmlRoot(ElementName = "TestProcedure")]
    public class TestProcedure
    {
        [XmlElement(ElementName = "IdSampleType")]
        public string IdSampleType { get; set; }
        [XmlElement(ElementName = "IdTestMethod")]
        public string IdTestMethod { get; set; }
    }

    [XmlRoot(ElementName = "UnitCodes")]
    public class UnitCodes
    {
        [XmlElement(ElementName = "IdValue")]
        public string IdValue { get; set; }
    }

    [XmlRoot(ElementName = "Service")]
    public class Service
    {
        [XmlElement(ElementName = "DOTTest")]
        public string DOTTest { get; set; }
        [XmlElement(ElementName = "TestingAuthority")]
        public string TestingAuthority { get; set; }
        [XmlElement(ElementName = "TestProcedure")]
        public TestProcedure TestProcedure { get; set; } = new TestProcedure();
        [XmlElement(ElementName = "UnitCodes")]
        public UnitCodes UnitCodes { get; set; } = new UnitCodes();
        [XmlElement(ElementName = "LaboratoryID")]
        public string LaboratoryID { get; set; }
        [XmlElement(ElementName = "LaboratoryAccount")]
        public string LaboratoryAccount { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
    }

    [XmlRoot(ElementName = "Services")]
    public class Services
    {
        [XmlElement(ElementName = "DateOrdered")]
        public string DateOrdered { get; set; }
        [XmlElement(ElementName = "ScheduledDate")]
        public string ScheduledDate { get; set; }
        [XmlElement(ElementName = "ExpirationDate")]
        public string ExpirationDate { get; set; }
        [XmlElement(ElementName = "CollectionSiteID")]
        public string CollectionSiteID { get; set; }
        [XmlElement(ElementName = "ReasonForTest")]
        public ReasonForTest ReasonForTest { get; set; } = new ReasonForTest();
        [XmlElement(ElementName = "Service")]
        public Service Service { get; set; } = new Service();
    }

    [XmlRoot(ElementName = "CreateOrderTest")]
    public class CreateOrderTest
    {
        [XmlElement(ElementName = "SendingFacility")]
        public string SendingFacility { get; set; }
        [XmlElement(ElementName = "SendingFacilityTimeZone")]
        public string SendingFacilityTimeZone { get; set; }
        [XmlElement(ElementName = "ProcessType")]
        public string ProcessType { get; set; }
        [XmlElement(ElementName = "ClientReferenceID")]
        public string ClientReferenceID { get; set; }
        [XmlElement(ElementName = "PersonalData")]
        public PersonalData PersonalData { get; set; } = new PersonalData();
        [XmlElement(ElementName = "Services")]
        public Services Services { get; set; } = new Services();
    }


    #region CreateOrderResponse

    [XmlRoot(ElementName = "CreateOrderResponse", Namespace = "https://www.formfox.com/v2/ffordersvc")]
    public class CreateOrderResponse
    {
        [XmlElement(ElementName = "CreateOrderResult", Namespace = "https://www.formfox.com/v2/ffordersvc")]
        public string CreateOrderResult { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }

    [XmlRoot(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Body
    {
        [XmlElement(ElementName = "CreateOrderResponse", Namespace = "https://www.formfox.com/v2/ffordersvc")]
        public CreateOrderResponse CreateOrderResponse { get; set; } = new CreateOrderResponse();
    }

    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Envelope
    {
        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public Body Body { get; set; } = new Body();
        [XmlAttribute(AttributeName = "soap", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Soap { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsd { get; set; }
    }


    #region innerxml
    [XmlRoot(ElementName = "Service")]
    public class ServiceResult
    {
        [XmlElement(ElementName = "IdValue")]
        public string IdValue { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
    }

    [XmlRoot(ElementName = "OrderTestResults")]
    public class OrderTestResults
    {
        [XmlElement(ElementName = "ClientReferenceID")]
        public string ClientReferenceID { get; set; }
        [XmlElement(ElementName = "RequestStatus")]
        public string RequestStatus { get; set; }
        [XmlElement(ElementName = "ReferenceTestID")]
        public string ReferenceTestID { get; set; }
        [XmlElement(ElementName = "Service")]
        public ServiceResult Service { get; set; } = new ServiceResult();
    }
    #endregion innerxml

    #endregion CreateOrderResponse

}
