using SurPath.Enum;
using System;
using System.Collections.Generic;

namespace SurPath.Entity
{
    public class DonorTestInfo
    {
        #region Private Variables

        private int _donorTestInfoId;
        private int _donorId;
        private int _clientId;
        private int _clientDepartmentId;
        private ClientMROTypes _mroTypeId;
        private ClientPaymentTypes _paymentTypeId;
        private DateTime _testRequestedDate;
        private int _testRequestedBy;
        private bool _isUA;
        private bool _isHair;
        private bool _isDNA;
        private bool _isBC;
        private TestInfoReasonForTest _reasonForTestId;
        private string _otherReason;
        private YesNo _isTemperatureInRange;
        private double? _temperatureOfSpecimen;
        private int? _testingAuthorityId;
        private SpecimenCollectionCupType _specimenCollectionCupId;
        private YesNo _isObserved;
        private SpecimenFormType _formTypeId;
        private YesNo _isAdulterationSign;
        private YesNo _isQuantitySufficient;
        private int? _collectionSiteVendorId;
        private int? _collectionSiteLocationId;
        private int? _collectionSiteUserId;
        private DateTime? _screeningTime;
        private bool? _isDonorRefused;
        private string _collectionSiteRemarks;
        private int? _laboratoryVendorId;
        private int? _mroVendorId;
        private int? _testOverallResult;
        private DonorRegistrationStatus _testStatus;
        private ProgramType _programTypeId;
        private bool? _isSurscanDeterminesDates;
        private bool? _isTpDeterminesDates;
        private DateTime? _programStartDate;
        private DateTime? _programEndDate;
        private string _caseNumber;
        private int? _courtId;
        private int? _judgeId;
        private double? _totalPaymentAmount;
        private DateTime? _paymentDate;
        private PaymentMethod _paymentMethodId;
        private string _paymentNote;
        private PaymentStatus _paymentStatus;
        private double? _laboratoryCost;
        private double? _mroCost;
        private double? _cupCost;
        private double? _shippingCost;
        private double _vendorCost;
        private int? _collectionsite1id;
        private int? _collectionsite2id;
        private int? _collectionsite3id;
        private int? _collectionsite4id;
        private DateTime? _scheduleDate;
        private bool _isSynchronized;
        private DateTime _createdOn;
        private string _createdBy;
        private DateTime _lastModifiedOn;
        private string _lastModifiedBy;

        private List<DonorTestInfoTestCategories> _testInfoTestCategories = new List<DonorTestInfoTestCategories>();

        private int? _attorneyId1;
        private int? _attorneyId2;
        private int? _attorneyId3;
        private int? _thirdPartyInfoId1;
        private int? _thirdPartyInfoId2;

        private string _specialNotes;

        private string _clientName;
        private string _clientDepartmentName;
        private int? _preRegistration;
        private int? _activated;
        private int? _registered;
        private int? _inQueue;
        private int? _suspensionQueue;
        private int? _processing;
        private int? _completed;

        private string _donorFirstName;
        private string _donorLastName;

        private bool _isWalkinDonor;
        private bool _isInstantTest;
        private InstantTestResult? _instantTestResult;
        private bool _isRegularAfterInstantTest;
        private string _testingAuthorityName;
        private bool _isPaymentReceived;

        private bool _isReverseEntry;

        private OverAllTestResult? _reportStatus;

        #endregion Private Variables

        #region Public Properties

        public int DonorTestInfoId
        {
            get
            {
                return this._donorTestInfoId;
            }
            set
            {
                this._donorTestInfoId = value;
            }
        }

        public int DonorId
        {
            get
            {
                return this._donorId;
            }
            set
            {
                this._donorId = value;
            }
        }

        public int ClientId
        {
            get
            {
                return this._clientId;
            }
            set
            {
                this._clientId = value;
            }
        }

        public int ClientDepartmentId
        {
            get
            {
                return this._clientDepartmentId;
            }
            set
            {
                this._clientDepartmentId = value;
            }
        }

        public ClientMROTypes MROTypeId
        {
            get
            {
                return this._mroTypeId;
            }
            set
            {
                this._mroTypeId = value;
            }
        }

        public ClientPaymentTypes PaymentTypeId
        {
            get
            {
                return this._paymentTypeId;
            }
            set
            {
                this._paymentTypeId = value;
            }
        }

        public DateTime TestRequestedDate
        {
            get
            {
                return this._testRequestedDate;
            }
            set
            {
                this._testRequestedDate = value;
            }
        }

        public int TestRequestedBy
        {
            get
            {
                return this._testRequestedBy;
            }
            set
            {
                this._testRequestedBy = value;
            }
        }

        public bool IsUA
        {
            get
            {
                return this._isUA;
            }
            set
            {
                this._isUA = value;
            }
        }

        public bool IsHair
        {
            get
            {
                return this._isHair;
            }
            set
            {
                this._isHair = value;
            }
        }

        public bool IsDNA
        {
            get
            {
                return this._isDNA;
            }
            set
            {
                this._isDNA = value;
            }
        }

        public bool IsBC
        {
            get
            {
                return this._isBC;
            }
            set
            {
                this._isBC = value;
            }
        }

        public TestInfoReasonForTest ReasonForTestId
        {
            get
            {
                return this._reasonForTestId;
            }
            set
            {
                this._reasonForTestId = value;
            }
        }

        public string OtherReason
        {
            get
            {
                return this._otherReason;
            }
            set
            {
                this._otherReason = value;
            }
        }

        public YesNo IsTemperatureInRange
        {
            get
            {
                return this._isTemperatureInRange;
            }
            set
            {
                this._isTemperatureInRange = value;
            }
        }

        public double? TemperatureOfSpecimen
        {
            get
            {
                return this._temperatureOfSpecimen;
            }
            set
            {
                this._temperatureOfSpecimen = value;
            }
        }

        public int? TestingAuthorityId
        {
            get
            {
                return this._testingAuthorityId;
            }
            set
            {
                this._testingAuthorityId = value;
            }
        }

        public SpecimenCollectionCupType SpecimenCollectionCupId
        {
            get
            {
                return this._specimenCollectionCupId;
            }
            set
            {
                this._specimenCollectionCupId = value;
            }
        }

        public YesNo IsObserved
        {
            get
            {
                return this._isObserved;
            }
            set
            {
                this._isObserved = value;
            }
        }

        public SpecimenFormType FormTypeId
        {
            get
            {
                return this._formTypeId;
            }
            set
            {
                this._formTypeId = value;
            }
        }

        public YesNo IsAdulterationSign
        {
            get
            {
                return this._isAdulterationSign;
            }
            set
            {
                this._isAdulterationSign = value;
            }
        }

        public YesNo IsQuantitySufficient
        {
            get
            {
                return this._isQuantitySufficient;
            }
            set
            {
                this._isQuantitySufficient = value;
            }
        }

        public int? CollectionSiteVendorId
        {
            get
            {
                return this._collectionSiteVendorId;
            }
            set
            {
                this._collectionSiteVendorId = value;
            }
        }

        public int? CollectionSiteLocationId
        {
            get
            {
                return this._collectionSiteLocationId;
            }
            set
            {
                this._collectionSiteLocationId = value;
            }
        }

        public int? CollectionSiteUserId
        {
            get
            {
                return this._collectionSiteUserId;
            }
            set
            {
                this._collectionSiteUserId = value;
            }
        }

        public DateTime? ScreeningTime
        {
            get
            {
                return this._screeningTime;
            }
            set
            {
                this._screeningTime = value;
            }
        }

        public bool? IsDonorRefused
        {
            get
            {
                return this._isDonorRefused;
            }
            set
            {
                this._isDonorRefused = value;
            }
        }

        public string CollectionSiteRemarks
        {
            get
            {
                return this._collectionSiteRemarks;
            }
            set
            {
                this._collectionSiteRemarks = value;
            }
        }

        public int? LaboratoryVendorId
        {
            get
            {
                return this._laboratoryVendorId;
            }
            set
            {
                this._laboratoryVendorId = value;
            }
        }

        public int? MROVendorId
        {
            get
            {
                return this._mroVendorId;
            }
            set
            {
                this._mroVendorId = value;
            }
        }

        public int? TestOverallResult
        {
            get
            {
                return this._testOverallResult;
            }
            set
            {
                this._testOverallResult = value;
            }
        }

        public DonorRegistrationStatus TestStatus
        {
            get
            {
                return this._testStatus;
            }
            set
            {
                this._testStatus = value;
            }
        }

        public ProgramType ProgramTypeId
        {
            get
            {
                return this._programTypeId;
            }
            set
            {
                this._programTypeId = value;
            }
        }

        public bool? IsSurscanDeterminesDates
        {
            get
            {
                return this._isSurscanDeterminesDates;
            }
            set
            {
                this._isSurscanDeterminesDates = value;
            }
        }

        public bool? IsTpDeterminesDates
        {
            get
            {
                return this._isTpDeterminesDates;
            }
            set
            {
                this._isTpDeterminesDates = value;
            }
        }

        public DateTime? ProgramStartDate
        {
            get
            {
                return this._programStartDate;
            }
            set
            {
                this._programStartDate = value;
            }
        }

        public DateTime? ProgramEndDate
        {
            get
            {
                return this._programEndDate;
            }
            set
            {
                this._programEndDate = value;
            }
        }

        public string CaseNumber
        {
            get
            {
                return this._caseNumber;
            }
            set
            {
                this._caseNumber = value;
            }
        }

        public int? CourtId
        {
            get
            {
                return this._courtId;
            }
            set
            {
                this._courtId = value;
            }
        }

        public int? JudgeId
        {
            get
            {
                return this._judgeId;
            }
            set
            {
                this._judgeId = value;
            }
        }

        public double? TotalPaymentAmount
        {
            get
            {
                return this._totalPaymentAmount;
            }
            set
            {
                this._totalPaymentAmount = value;
            }
        }

        public DateTime? PaymentDate
        {
            get
            {
                return this._paymentDate;
            }
            set
            {
                this._paymentDate = value;
            }
        }

        public PaymentMethod PaymentMethodId
        {
            get
            {
                return this._paymentMethodId;
            }
            set
            {
                this._paymentMethodId = value;
            }
        }

        public string PaymentNote
        {
            get
            {
                return this._paymentNote;
            }
            set
            {
                this._paymentNote = value;
            }
        }

        public PaymentStatus PaymentStatus
        {
            get
            {
                return this._paymentStatus;
            }
            set
            {
                this._paymentStatus = value;
            }
        }

        public double? LaboratoryCost
        {
            get
            {
                return this._laboratoryCost;
            }
            set
            {
                this._laboratoryCost = value;
            }
        }

        public double? MROCost
        {
            get
            {
                return this._mroCost;
            }
            set
            {
                this._mroCost = value;
            }
        }

        public double? CupCost
        {
            get
            {
                return this._cupCost;
            }
            set
            {
                this._cupCost = value;
            }
        }

        public double? ShippingCost
        {
            get
            {
                return this._shippingCost;
            }
            set
            {
                this._shippingCost = value;
            }
        }

        public double VendorCost
        {
            get
            {
                return this._vendorCost;
            }
            set
            {
                this._vendorCost = value;
            }
        }

        public int? CollectionSite1Id
        {
            get
            {
                return this._collectionsite1id;
            }
            set
            {
                this._collectionsite1id = value;
            }
        }

        public int? CollectionSite2Id
        {
            get
            {
                return this._collectionsite2id;
            }
            set
            {
                this._collectionsite2id = value;
            }
        }

        public int? CollectionSite3Id
        {
            get
            {
                return this._collectionsite3id;
            }
            set
            {
                this._collectionsite3id = value;
            }
        }

        public int? CollectionSite4Id
        {
            get
            {
                return this._collectionsite4id;
            }
            set
            {
                this._collectionsite4id = value;
            }
        }

        public DateTime? ScheduleDate
        {
            get
            {
                return this._scheduleDate;
            }
            set
            {
                this._scheduleDate = value;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return this._isSynchronized;
            }
            set
            {
                this._isSynchronized = value;
            }
        }

        public DateTime CreatedOn
        {
            get
            {
                return this._createdOn;
            }
            set
            {
                this._createdOn = value;
            }
        }

        public string CreatedBy
        {
            get
            {
                return this._createdBy;
            }
            set
            {
                this._createdBy = value;
            }
        }

        public DateTime LastModifiedOn
        {
            get
            {
                return this._lastModifiedOn;
            }
            set
            {
                this._lastModifiedOn = value;
            }
        }

        public string LastModifiedBy
        {
            get
            {
                return this._lastModifiedBy;
            }
            set
            {
                this._lastModifiedBy = value;
            }
        }

        public List<DonorTestInfoTestCategories> TestInfoTestCategories
        {
            get
            {
                return this._testInfoTestCategories;
            }
            set
            {
                this._testInfoTestCategories = value;
            }
        }

        public int? AttorneyId1
        {
            get
            {
                return this._attorneyId1;
            }
            set
            {
                this._attorneyId1 = value;
            }
        }

        public int? AttorneyId2
        {
            get
            {
                return this._attorneyId2;
            }
            set
            {
                this._attorneyId2 = value;
            }
        }

        public int? AttorneyId3
        {
            get
            {
                return this._attorneyId3;
            }
            set
            {
                this._attorneyId3 = value;
            }
        }

        public int? ThirdPartyInfoId1
        {
            get
            {
                return this._thirdPartyInfoId1;
            }
            set
            {
                this._thirdPartyInfoId1 = value;
            }
        }

        public int? ThirdPartyInfoId2
        {
            get
            {
                return this._thirdPartyInfoId2;
            }
            set
            {
                this._thirdPartyInfoId2 = value;
            }
        }

        public string SpecialNotes
        {
            get
            {
                return this._specialNotes;
            }
            set
            {
                this._specialNotes = value;
            }
        }

        public int? PreRegistration
        {
            get
            {
                return this._preRegistration;
            }
            set
            {
                this._preRegistration = value;
            }
        }

        public int? Activated
        {
            get
            {
                return this._activated;
            }
            set
            {
                this._activated = value;
            }
        }

        public int? Registered
        {
            get
            {
                return this._registered;
            }
            set
            {
                this._registered = value;
            }
        }

        public int? InQueue
        {
            get
            {
                return this._inQueue;
            }
            set
            {
                this._inQueue = value;
            }
        }

        public int? SuspensionQueue
        {
            get
            {
                return this._suspensionQueue;
            }
            set
            {
                this._suspensionQueue = value;
            }
        }

        public int? Processing
        {
            get
            {
                return this._processing;
            }
            set
            {
                this._processing = value;
            }
        }

        public int? Completed
        {
            get
            {
                return this._completed;
            }
            set
            {
                this._completed = value;
            }
        }

        public string ClientName
        {
            get
            {
                return this._clientName;
            }
            set
            {
                this._clientName = value;
            }
        }

        public string ClientDeparmentName
        {
            get
            {
                return this._clientDepartmentName;
            }
            set
            {
                this._clientDepartmentName = value;
            }
        }

        public string DonorFirstName
        {
            get
            {
                return this._donorFirstName;
            }
            set
            {
                this._donorFirstName = value;
            }
        }

        public string DonorLastName
        {
            get
            {
                return this._donorLastName;
            }
            set
            {
                this._donorLastName = value;
            }
        }

        public bool IsWalkinDonor
        {
            get
            {
                return this._isWalkinDonor;
            }
            set
            {
                this._isWalkinDonor = value;
            }
        }

        public bool IsInstantTest
        {
            get
            {
                return this._isInstantTest;
            }
            set
            {
                this._isInstantTest = value;
            }
        }

        public InstantTestResult? InstantTestResult
        {
            get
            {
                return this._instantTestResult;
            }
            set
            {
                this._instantTestResult = value;
            }
        }

        public bool IsRegularAfterInstantTest
        {
            get
            {
                return this._isRegularAfterInstantTest;
            }
            set
            {
                this._isRegularAfterInstantTest = value;
            }
        }

        public string TestingAuthorityName
        {
            get
            {
                return this._testingAuthorityName;
            }
            set
            {
                this._testingAuthorityName = value;
            }
        }

        public OverAllTestResult? ReportStatus
        {
            get
            {
                return this._reportStatus;
            }
            set
            {
                this._reportStatus = value;
            }
        }

        public bool IsPaymentReceived
        {
            get
            {
                return this._isPaymentReceived;
            }
            set
            {
                this._isPaymentReceived = value;
            }
        }

        public bool IsReverseEntry
        {
            get
            {
                return this._isReverseEntry;
            }
            set
            {
                this._isReverseEntry = value;
            }
        }

        #endregion Public Properties
    }
}