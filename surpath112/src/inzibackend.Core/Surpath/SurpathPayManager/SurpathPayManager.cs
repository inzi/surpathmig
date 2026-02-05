using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.IdentityFramework;
using Abp.Linq;
using Abp.Notifications;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.AspNetCore.Identity;
using inzibackend.Authorization.Roles;
using inzibackend.Configuration;
using inzibackend.Debugging;
using inzibackend.MultiTenancy;
using inzibackend.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Localization;
using Abp.Organizations;
using Abp.Runtime.Caching;
using Abp.Threading;
using Abp.UI;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using inzibackend.Authorization.Roles;
using inzibackend.Configuration;
using inzibackend.Security;
using inzibackend.Authorization.Users;
using System.Transactions;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;
using Abp.Application.Services.Dto;
using Abp.Application.Features;
using Newtonsoft.Json;

namespace inzibackend.Surpath.SurpathPay
{
    public class SurpathPayManager : inzibackendDomainServiceBase
    {


        private readonly IRepository<Cohort, Guid> _cohortRepository;
        private readonly IRepository<CohortUser, Guid> _cohortUserRepository;
        private readonly IRepository<RecordState, Guid> _recordStateRepository;
        private readonly IRepository<Record, Guid> _recordRepository;
        private readonly IRepository<RecordNote, Guid> _recordNoteRepository;
        private readonly IRepository<RecordCategory, Guid> _recordCategoryRepository;
        private readonly IRepository<RecordRequirement, Guid> _recordRequirementRepository;
        private readonly IRepository<User, long> _userLookUpRepository;
        private readonly IRepository<PidType, Guid> _PidTypeRepository;
        private readonly IRepository<RecordStatus, Guid> _recordStatusRepository;
        private readonly IRepository<TenantDocumentCategory, Guid> _tenantDocumentCategoryRepository;
        private readonly IRepository<TenantDocument, Guid> _tenantDocumentRepository;

        private readonly IRepository<LedgerEntry, Guid> _ledgerEntryRepository;
        private readonly IRepository<TenantDocument, Guid> _tenantDocumentLookUpRepository;
        private readonly IRepository<Cohort, Guid> _lookup_cohortRepository;
        private readonly IRepository<LedgerEntryDetail, Guid> _ledgerEntryDetailRepository;
        private readonly IFeatureChecker _featureChecker;
        //private readonly IRepository<SurpathService, Guid> _surpathServiceRepository;
        private readonly IRepository<TenantSurpathService, Guid> _tenantSurpathServiceRepository;
        private string _SurpathRecordsRootFolder { get; set; }
        public SurpathPayManager(
          IRepository<User, long> lookup_userRepository,
          IRepository<LedgerEntry, Guid> ledgerEntryRepository,
          IRepository<LedgerEntryDetail, Guid> ledgerEntryDetailRepository,
        IRepository<TenantSurpathService, Guid> tenantSurpathServiceRepository
            , IFeatureChecker featureChecker
          //ILogger logger
          )
        {
            _userLookUpRepository = lookup_userRepository;

            _ledgerEntryRepository = ledgerEntryRepository;
            _ledgerEntryDetailRepository = ledgerEntryDetailRepository;
            _tenantSurpathServiceRepository = tenantSurpathServiceRepository;
            _featureChecker = featureChecker;

        }
        [UnitOfWork(IsolationLevel.ReadUncommitted)]
        public async Task<bool> UserIsPaid(long id)
        {
            // We have to get the user
            // get their departments

            var retval = false;

            Logger.Info("DonorPaid Check");
            //var filteredLedgerEntries = _ledgerEntryRepository.GetAll()
            //        .Where(l=>l.UserId == id);

            var ledgerdetailentries = _ledgerEntryRepository.GetAll().Where(l => l.UserId == id);

            //var sortedledgerdetailentries = ledgerdetailentries.OrderBy("CreationTime desc");

            var entry = from lde in ledgerdetailentries orderby lde.CreationTime descending select lde;

            var _entry = entry.FirstOrDefault();

            //return (_entry != null && _entry.ExpirationDate == null);
            return (_entry != null && _entry.ExpirationDate < DateTime.Now);

            //return retval;
        }

        [UnitOfWork]
        public async Task<List<TenantSurpathService>> GetSurpathServicesForTenant(int tenantId)
        {
            
            //var retval = new List<TenantSurpathService>();
            // Don't filter by IsEnabled here - let hierarchical pricing determine if service should be included
            var _retval = _tenantSurpathServiceRepository.GetAll().IgnoreQueryFilters().Include(r=>r.SurpathServiceFk).Where(s => s.TenantId == tenantId && s.IsDeleted == false).ToList();
            //if (_retval.Count == 0) return retval;
            return _retval;
        }

        //public async Task CreateLedgerEntry(transactionResponse _transactionResponse, long userid, int? tenantid, decimal amount, List<EntityDto<Guid>> surpathServiceIds)
        public async Task CreateLedgerEntry(transactionResponse _transactionResponse, long userid, int? tenantid, decimal amount, AuthNetSubmit authNetSubmit, decimal _TotalPrice = 0) // List<Guid> tenantSurpathServiceIds)
        {
            CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var amountDue = _TotalPrice - amount;
            if (amountDue < 0)
            {
                amountDue = 0;
            }
            var _ledgerEntry = new LedgerEntry()
            {
                TenantId = tenantid,
                UserId = userid,
                AccountNumber = _transactionResponse.accountNumber,
                AuthCode = _transactionResponse.authCode,
                AmountDue = amountDue,
                AccountType = _transactionResponse.accountType,
                Amount = amount,
                PaidAmount = amount,
                TotalPrice = _TotalPrice,
                ReferenceTransactionId = _transactionResponse.refTransID,
                //AuthNetCustomerAddressId = _transactionResponse.cu
                AuthNetNetworkTransId = _transactionResponse.transId,
                AuthNetCustomerProfileId = _transactionResponse.profile==null? null : _transactionResponse.profile.customerProfileId,
                CardLastFour = authNetSubmit.CardLastFour,
                AuthNetCustomerPaymentProfileId = _transactionResponse.profile == null ? null : _transactionResponse.profile.customerPaymentProfileId,
                AuthNetCustomerAddressId = _transactionResponse.profile == null ? null : _transactionResponse.profile.customerAddressId,
                AuthNetTransHashSha2 = _transactionResponse.transHashSha2,
                CardNameOnCard = authNetSubmit.CardNameOnCard,
                CardZipCode = authNetSubmit.BillingZipCode
            };

            _ledgerEntry.TransactionMessage = JsonConvert.SerializeObject(_transactionResponse);
            _ledgerEntry.Settled = true;

            var _userRegDate = _userLookUpRepository.Get(userid).CreationTime;
            var _yearsSince = DateTime.Now.Year - _userRegDate.Year;
            _ledgerEntry.ExpirationDate = _userRegDate.AddYears(1 + _yearsSince);

            _ledgerEntryRepository.Insert(_ledgerEntry);
            var _tranServices = _tenantSurpathServiceRepository.GetAll().Include(s => s.SurpathServiceFk).Where(s => authNetSubmit.TenantSurpathServiceIds.Contains(s.Id)).ToList();

            var paidTally = _TotalPrice;
            _tranServices.ForEach(s =>
            {
                if (paidTally >= 0)
                {
                    var servicePrice = Convert.ToDecimal(s.Price);
                    var _leDetail = new LedgerEntryDetail()
                    {
                        TenantId = tenantid,
                        LedgerEntryId = _ledgerEntry.Id,
                        SurpathServiceId = (Guid)s.SurpathServiceId,
                        TenantSurpathServiceId = s.Id,
                        Amount = servicePrice,
                        AmountPaid = servicePrice,
                        //Discount = s.SurpathServiceFk.Discount,
                        Discount = 0,
                        DiscountAmount = 0 // servicePrice // (decimal)( servicePrice * (decimal)s.Discount)
                    };
                    _ledgerEntryDetailRepository.Insert(_leDetail);
                    paidTally -= servicePrice;
                }
               
            });

            

            CurrentUnitOfWork.EnableFilter(AbpDataFilters.MayHaveTenant);

        }
    }
}
