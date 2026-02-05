using GridMvc.DataAnnotations;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SurPathWeb.Models
{
    public class TestInfoDataModel
    {
        public int DonorTestInfoId { get; set; }

        public int DonorId { get; set; }

        public int ClientId { get; set; }

        public int ClientDepartmentId { get; set; }

        public ClientMROTypes MROTypeId { get; set; }

        public ClientPaymentTypes PaymentTypeId { get; set; }

        public DateTime TestRequestedDate { get; set; }

        public int TestRequestedBy { get; set; }

        public bool IsUA { get; set; }

        public bool IsHair { get; set; }

        public bool IsDNA { get; set; }

        public TestInfoReasonForTest ReasonForTestId { get; set; }

        public string OtherReason { get; set; }

        public YesNo IsTemperatureInRange { get; set; }

        public double? TemperatureOfSpecimen { get; set; }

        public int? TestingAuthorityId { get; set; }

        public SpecimenCollectionCupType SpecimenCollectionCupId { get; set; }

        public YesNo IsObserved { get; set; }

        public SpecimenFormType FormTypeId { get; set; }

        public YesNo IsAdulterationSign { get; set; }

        public YesNo IsQuantitySufficient { get; set; }

        public int? CollectionSiteVendorId { get; set; }

        public int? CollectionSiteLocationId { get; set; }

        public int? CollectionSiteUserId { get; set; }

        public DateTime? ScreeningTime { get; set; }

        public bool? IsDonorRefused { get; set; }

        public string CollectionSiteRemarks { get; set; }

        public int? LaboratoryVendorId { get; set; }

        public int? MROVendorId { get; set; }

        public int? TestOverallResult { get; set; }

        public DonorRegistrationStatus TestStatus { get; set; }

        public ProgramType ProgramTypeId { get; set; }

        public bool? IsSurscanDeterminesDates { get; set; }

        public bool? IsTpDeterminesDates { get; set; }

        public DateTime? ProgramStartDate { get; set; }

        public DateTime? ProgramEndDate { get; set; }

        public string CaseNumber { get; set; }

        public int? CourtId { get; set; }

        public int? JudgeId { get; set; }

        public double? TotalPaymentAmount { get; set; }

        public DateTime? PaymentDate { get; set; }

        public PaymentMethod PaymentMethodId { get; set; }

        public string PaymentNote { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public double? LaboratoryCost { get; set; }

        public double? MROCost { get; set; }

        public double? CupCost { get; set; }

        public double? ShippingCost { get; set; }

        public double VendorCost { get; set; }

        public int? CollectionSite1Id { get; set; }

        public int? CollectionSite2Id { get; set; }

        public int? CollectionSite3Id { get; set; }

        public int? CollectionSite4Id { get; set; }

        public DateTime? ScheduleDate { get; set; }

        public bool IsSynchronized { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime LastModifiedOn { get; set; }

        public string LastModifiedBy { get; set; }

        public List<TestInfoTestCategoriesDataModel> TestInfoTestCategories { get; set; }

        public int? AttorneyId1 { get; set; }

        public int? AttorneyId2 { get; set; }

        public int? AttorneyId3 { get; set; }

        public int? ThirdPartyInfoId1 { get; set; }

        public int? ThirdPartyInfoId2 { get; set; }

        public string SpecialNotes { get; set; }

        public int? PreRegistration { get; set; }

        public int? Activated { get; set; }

        public int? Registered { get; set; }

        public int? InQueue { get; set; }

        public int? SuspensionQueue { get; set; }

        public int? Processing { get; set; }

        public int? Completed { get; set; }

        public string ClientName { get; set; }

        public string ClientDeparmentName { get; set; }

        public string DonorFirstName { get; set; }

        public string CollectionAddress1 { get; set; }

        public string CollectionAddress2 { get; set; }

        public string CollectionCity { get; set; }

        public string CollectionState { get; set; }

        public string CollectionZipCode { get; set; }

        public string CollectionPhone { get; set; }

        public string CollectionFax { get; set; }

        public string CollectionEmail { get; set; }

        public DateTime ScreeningDate { get; set; }       

        public string CollectorName { get; set; }

        public string LocationName { get; set; }

        public bool IsInstantTest { get; set; }

        public InstantTestResult InstantTestResult { get; set; }

        public string StrReasonForTest { get; set; }

        public string StrSpecimenCollectionCup { get; set; }

        public string StrFormType { get; set; }

        public string TestingAuthorityName { get; set; }
    }
}