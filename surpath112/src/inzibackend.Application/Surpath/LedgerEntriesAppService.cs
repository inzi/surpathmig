using inzibackend.Authorization.Users;
using inzibackend.Surpath;
using inzibackend.Surpath;

using inzibackend.MultiTenancy.Payments;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using inzibackend.Surpath.Exporting;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using Abp.Application.Services.Dto;
using inzibackend.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using inzibackend.Storage;
using Abp.Domain.Uow;
using inzibackend.Authorization.Users.Dto;

namespace inzibackend.Surpath
{
    [AbpAuthorize(AppPermissions.Pages_LedgerEntries)]
    public class LedgerEntriesAppService : inzibackendAppServiceBase, ILedgerEntriesAppService
    {
        private readonly IRepository<LedgerEntry, Guid> _ledgerEntryRepository;
        private readonly ILedgerEntriesExcelExporter _ledgerEntriesExcelExporter;
        private readonly IRepository<User, long> _userLookUpRepository;
        private readonly IRepository<TenantDocument, Guid> _tenantDocumentLookUpRepository;
        private readonly IRepository<Cohort, Guid> _cohortLookUpRepository;

        public LedgerEntriesAppService(IRepository<LedgerEntry, Guid> ledgerEntryRepository, ILedgerEntriesExcelExporter ledgerEntriesExcelExporter, IRepository<User, long> lookup_userRepository, IRepository<TenantDocument, Guid> lookup_tenantDocumentRepository, IRepository<Cohort, Guid> lookup_cohortRepository)
        {
            _ledgerEntryRepository = ledgerEntryRepository;
            _ledgerEntriesExcelExporter = ledgerEntriesExcelExporter;
            _userLookUpRepository = lookup_userRepository;
            _tenantDocumentLookUpRepository = lookup_tenantDocumentRepository;
            _cohortLookUpRepository = lookup_cohortRepository;

        }

        public async Task<PagedResultDto<GetLedgerEntryForViewDto>> GetAll(GetAllLedgerEntriesInput input)
        {
            if (AbpSession.TenantId == null)
            {
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            }

            var serviceTypeFilter = input.ServiceTypeFilter.HasValue
                        ? (EnumServiceType)input.ServiceTypeFilter
                        : default;
            var paymentPeriodTypeFilter = input.PaymentPeriodTypeFilter.HasValue
                ? (PaymentPeriodType)input.PaymentPeriodTypeFilter
                : default;

            var filteredLedgerEntries = _ledgerEntryRepository.GetAll()
                        .Include(e => e.UserFk)
                        .Include(e => e.TenantDocumentFk)
                        .Include(e => e.CohortFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.TransactionName.Contains(input.Filter) || e.TransactionKey.Contains(input.Filter) || e.TransactionId.Contains(input.Filter) || e.PaymentToken.Contains(input.Filter) || e.AuthNetCustomerProfileId.Contains(input.Filter) || e.AuthNetCustomerPaymentProfileId.Contains(input.Filter) || e.AuthNetCustomerAddressId.Contains(input.Filter) || e.AccountNumber.Contains(input.Filter) || e.Note.Contains(input.Filter) || e.MetaData.Contains(input.Filter) || e.AuthCode.Contains(input.Filter) || e.ReferenceTransactionId.Contains(input.Filter) || e.TransactionHash.Contains(input.Filter) || e.AccountType.Contains(input.Filter) || e.TransactionCode.Contains(input.Filter) || e.TransactionMessage.Contains(input.Filter) || e.AuthNetTransHashSha2.Contains(input.Filter) || e.AuthNetNetworkTransId.Contains(input.Filter) || e.CardNameOnCard.Contains(input.Filter) || e.CardZipCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.ServiceTypeFilter.HasValue && input.ServiceTypeFilter > -1, e => e.ServiceType == serviceTypeFilter)
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(input.MinDiscountAmountFilter != null, e => e.DiscountAmount >= input.MinDiscountAmountFilter)
                        .WhereIf(input.MaxDiscountAmountFilter != null, e => e.DiscountAmount <= input.MaxDiscountAmountFilter)
                        .WhereIf(input.MinTotalPriceFilter != null, e => e.TotalPrice >= input.MinTotalPriceFilter)
                        .WhereIf(input.MaxTotalPriceFilter != null, e => e.TotalPrice <= input.MaxTotalPriceFilter)
                        .WhereIf(input.PaymentPeriodTypeFilter.HasValue && input.PaymentPeriodTypeFilter > -1, e => e.PaymentPeriodType == paymentPeriodTypeFilter)
                        .WhereIf(input.MinExpirationDateFilter != null, e => e.ExpirationDate >= input.MinExpirationDateFilter)
                        .WhereIf(input.MaxExpirationDateFilter != null, e => e.ExpirationDate <= input.MaxExpirationDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TransactionNameFilter), e => e.TransactionName.Contains(input.TransactionNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TransactionKeyFilter), e => e.TransactionKey.Contains(input.TransactionKeyFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TransactionIdFilter), e => e.TransactionId.Contains(input.TransactionIdFilter))
                        .WhereIf(input.SettledFilter.HasValue && input.SettledFilter > -1, e => (input.SettledFilter == 1 && e.Settled) || (input.SettledFilter == 0 && !e.Settled))
                        .WhereIf(input.MinAmountDueFilter != null, e => e.AmountDue >= input.MinAmountDueFilter)
                        .WhereIf(input.MaxAmountDueFilter != null, e => e.AmountDue <= input.MaxAmountDueFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaymentTokenFilter), e => e.PaymentToken.Contains(input.PaymentTokenFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AuthNetCustomerProfileIdFilter), e => e.AuthNetCustomerProfileId.Contains(input.AuthNetCustomerProfileIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AuthNetCustomerPaymentProfileIdFilter), e => e.AuthNetCustomerPaymentProfileId.Contains(input.AuthNetCustomerPaymentProfileIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AuthNetCustomerAddressIdFilter), e => e.AuthNetCustomerAddressId.Contains(input.AuthNetCustomerAddressIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccountNumberFilter), e => e.AccountNumber.Contains(input.AccountNumberFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NoteFilter), e => e.Note.Contains(input.NoteFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MetaDataFilter), e => e.MetaData.Contains(input.MetaDataFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AuthCodeFilter), e => e.AuthCode.Contains(input.AuthCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReferenceTransactionIdFilter), e => e.ReferenceTransactionId.Contains(input.ReferenceTransactionIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TransactionHashFilter), e => e.TransactionHash.Contains(input.TransactionHashFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccountTypeFilter), e => e.AccountType.Contains(input.AccountTypeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TransactionCodeFilter), e => e.TransactionCode.Contains(input.TransactionCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TransactionMessageFilter), e => e.TransactionMessage.Contains(input.TransactionMessageFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AuthNetTransHashSha2Filter), e => e.AuthNetTransHashSha2.Contains(input.AuthNetTransHashSha2Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AuthNetNetworkTransIdFilter), e => e.AuthNetNetworkTransId.Contains(input.AuthNetNetworkTransIdFilter))
                        .WhereIf(input.MinPaidAmountFilter != null, e => e.PaidAmount >= input.MinPaidAmountFilter)
                        .WhereIf(input.MaxPaidAmountFilter != null, e => e.PaidAmount <= input.MaxPaidAmountFilter)
                        .WhereIf(input.MinPaidInCashFilter != null, e => e.PaidInCash >= input.MinPaidInCashFilter)
                        .WhereIf(input.MaxPaidInCashFilter != null, e => e.PaidInCash <= input.MaxPaidInCashFilter)
                        .WhereIf(input.MinAvailableUserBalanceFilter != null, e => e.AvailableUserBalance >= input.MinAvailableUserBalanceFilter)
                        .WhereIf(input.MaxAvailableUserBalanceFilter != null, e => e.AvailableUserBalance <= input.MaxAvailableUserBalanceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CardNameOnCardFilter), e => e.CardNameOnCard.Contains(input.CardNameOnCardFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CardZipCodeFilter), e => e.CardZipCode.Contains(input.CardZipCodeFilter))
                        .WhereIf(input.MinBalanceForwardFilter != null, e => e.BalanceForward >= input.MinBalanceForwardFilter)
                        .WhereIf(input.MaxBalanceForwardFilter != null, e => e.BalanceForward <= input.MaxBalanceForwardFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TenantDocumentNameFilter), e => e.TenantDocumentFk != null && e.TenantDocumentFk.Name == input.TenantDocumentNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CohortNameFilter), e => e.CohortFk != null && e.CohortFk.Name == input.CohortNameFilter);

            var pagedAndFilteredLedgerEntries = filteredLedgerEntries
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var ledgerEntries = from o in pagedAndFilteredLedgerEntries
                                join o1 in _userLookUpRepository.GetAll() on o.UserId equals o1.Id into j1
                                from s1 in j1.DefaultIfEmpty()

                                join o2 in _tenantDocumentLookUpRepository.GetAll() on o.TenantDocumentId equals o2.Id into j2
                                from s2 in j2.DefaultIfEmpty()

                                join o3 in _cohortLookUpRepository.GetAll() on o.CohortId equals o3.Id into j3
                                from s3 in j3.DefaultIfEmpty()

                                select new
                                {

                                    o.Name,
                                    o.ServiceType,
                                    o.Amount,
                                    o.DiscountAmount,
                                    o.TotalPrice,
                                    o.PaymentPeriodType,
                                    o.ExpirationDate,
                                    o.TransactionName,
                                    o.TransactionKey,
                                    o.TransactionId,
                                    o.Settled,
                                    o.AmountDue,
                                    o.PaymentToken,
                                    o.AuthNetCustomerProfileId,
                                    o.AuthNetCustomerPaymentProfileId,
                                    o.AuthNetCustomerAddressId,
                                    o.AccountNumber,
                                    o.Note,
                                    o.MetaData,
                                    o.AuthCode,
                                    o.ReferenceTransactionId,
                                    o.TransactionHash,
                                    o.AccountType,
                                    o.TransactionCode,
                                    o.TransactionMessage,
                                    o.AuthNetTransHashSha2,
                                    o.AuthNetNetworkTransId,
                                    o.PaidAmount,
                                    o.PaidInCash,
                                    o.AvailableUserBalance,
                                    o.CardNameOnCard,
                                    o.CardZipCode,
                                    o.BalanceForward,
                                    Id = o.Id,
                                    UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                    TenantDocumentName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                    CohortName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                                };

            var totalCount = await filteredLedgerEntries.CountAsync();

            var dbList = await ledgerEntries.ToListAsync();
            var results = new List<GetLedgerEntryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetLedgerEntryForViewDto()
                {
                    LedgerEntry = new LedgerEntryDto
                    {

                        Name = o.Name,
                        ServiceType = o.ServiceType,
                        Amount = o.Amount,
                        DiscountAmount = o.DiscountAmount,
                        TotalPrice = o.TotalPrice,
                        PaymentPeriodType = o.PaymentPeriodType,
                        ExpirationDate = o.ExpirationDate,
                        TransactionName = o.TransactionName,
                        TransactionKey = o.TransactionKey,
                        TransactionId = o.TransactionId,
                        Settled = o.Settled,
                        AmountDue = o.AmountDue,
                        PaymentToken = o.PaymentToken,
                        AuthNetCustomerProfileId = o.AuthNetCustomerProfileId,
                        AuthNetCustomerPaymentProfileId = o.AuthNetCustomerPaymentProfileId,
                        AuthNetCustomerAddressId = o.AuthNetCustomerAddressId,
                        AccountNumber = o.AccountNumber,
                        Note = o.Note,
                        MetaData = o.MetaData,
                        AuthCode = o.AuthCode,
                        ReferenceTransactionId = o.ReferenceTransactionId,
                        TransactionHash = o.TransactionHash,
                        AccountType = o.AccountType,
                        TransactionCode = o.TransactionCode,
                        TransactionMessage = o.TransactionMessage,
                        AuthNetTransHashSha2 = o.AuthNetTransHashSha2,
                        AuthNetNetworkTransId = o.AuthNetNetworkTransId,
                        PaidAmount = o.PaidAmount,
                        PaidInCash = o.PaidInCash,
                        AvailableUserBalance = o.AvailableUserBalance,
                        CardNameOnCard = o.CardNameOnCard,
                        CardZipCode = o.CardZipCode,
                        BalanceForward = o.BalanceForward,
                        Id = o.Id,
                    },
                    UserName = o.UserName,
                    TenantDocumentName = o.TenantDocumentName,
                    CohortName = o.CohortName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetLedgerEntryForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<PagedResultDto<GetLedgerEntryForViewDto>> GetAllForUserId(GetAllLedgerEntriesInput input)
        {
            if (AbpSession.TenantId == null)
            {
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            }

            var serviceTypeFilter = input.ServiceTypeFilter.HasValue
                        ? (EnumServiceType)input.ServiceTypeFilter
                        : default;
            var paymentPeriodTypeFilter = input.PaymentPeriodTypeFilter.HasValue
                ? (PaymentPeriodType)input.PaymentPeriodTypeFilter
                : default;

            var filteredLedgerEntries = _ledgerEntryRepository.GetAll()
                        .Include(e => e.UserFk)
                        .Include(e => e.TenantDocumentFk)
                        .Include(e => e.CohortFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.TransactionName.Contains(input.Filter) || e.TransactionKey.Contains(input.Filter) || e.TransactionId.Contains(input.Filter) || e.PaymentToken.Contains(input.Filter) || e.AuthNetCustomerProfileId.Contains(input.Filter) || e.AuthNetCustomerPaymentProfileId.Contains(input.Filter) || e.AuthNetCustomerAddressId.Contains(input.Filter) || e.AccountNumber.Contains(input.Filter) || e.Note.Contains(input.Filter) || e.MetaData.Contains(input.Filter) || e.AuthCode.Contains(input.Filter) || e.ReferenceTransactionId.Contains(input.Filter) || e.TransactionHash.Contains(input.Filter) || e.AccountType.Contains(input.Filter) || e.TransactionCode.Contains(input.Filter) || e.TransactionMessage.Contains(input.Filter) || e.AuthNetTransHashSha2.Contains(input.Filter) || e.AuthNetNetworkTransId.Contains(input.Filter) || e.CardNameOnCard.Contains(input.Filter) || e.CardZipCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.ServiceTypeFilter.HasValue && input.ServiceTypeFilter > -1, e => e.ServiceType == serviceTypeFilter)
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(input.MinDiscountAmountFilter != null, e => e.DiscountAmount >= input.MinDiscountAmountFilter)
                        .WhereIf(input.MaxDiscountAmountFilter != null, e => e.DiscountAmount <= input.MaxDiscountAmountFilter)
                        .WhereIf(input.MinTotalPriceFilter != null, e => e.TotalPrice >= input.MinTotalPriceFilter)
                        .WhereIf(input.MaxTotalPriceFilter != null, e => e.TotalPrice <= input.MaxTotalPriceFilter)
                        .WhereIf(input.PaymentPeriodTypeFilter.HasValue && input.PaymentPeriodTypeFilter > -1, e => e.PaymentPeriodType == paymentPeriodTypeFilter)
                        .WhereIf(input.MinExpirationDateFilter != null, e => e.ExpirationDate >= input.MinExpirationDateFilter)
                        .WhereIf(input.MaxExpirationDateFilter != null, e => e.ExpirationDate <= input.MaxExpirationDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TransactionNameFilter), e => e.TransactionName.Contains(input.TransactionNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TransactionKeyFilter), e => e.TransactionKey.Contains(input.TransactionKeyFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TransactionIdFilter), e => e.TransactionId.Contains(input.TransactionIdFilter))
                        .WhereIf(input.SettledFilter.HasValue && input.SettledFilter > -1, e => (input.SettledFilter == 1 && e.Settled) || (input.SettledFilter == 0 && !e.Settled))
                        .WhereIf(input.MinAmountDueFilter != null, e => e.AmountDue >= input.MinAmountDueFilter)
                        .WhereIf(input.MaxAmountDueFilter != null, e => e.AmountDue <= input.MaxAmountDueFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaymentTokenFilter), e => e.PaymentToken.Contains(input.PaymentTokenFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AuthNetCustomerProfileIdFilter), e => e.AuthNetCustomerProfileId.Contains(input.AuthNetCustomerProfileIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AuthNetCustomerPaymentProfileIdFilter), e => e.AuthNetCustomerPaymentProfileId.Contains(input.AuthNetCustomerPaymentProfileIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AuthNetCustomerAddressIdFilter), e => e.AuthNetCustomerAddressId.Contains(input.AuthNetCustomerAddressIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccountNumberFilter), e => e.AccountNumber.Contains(input.AccountNumberFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NoteFilter), e => e.Note.Contains(input.NoteFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MetaDataFilter), e => e.MetaData.Contains(input.MetaDataFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AuthCodeFilter), e => e.AuthCode.Contains(input.AuthCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReferenceTransactionIdFilter), e => e.ReferenceTransactionId.Contains(input.ReferenceTransactionIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TransactionHashFilter), e => e.TransactionHash.Contains(input.TransactionHashFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccountTypeFilter), e => e.AccountType.Contains(input.AccountTypeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TransactionCodeFilter), e => e.TransactionCode.Contains(input.TransactionCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TransactionMessageFilter), e => e.TransactionMessage.Contains(input.TransactionMessageFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AuthNetTransHashSha2Filter), e => e.AuthNetTransHashSha2.Contains(input.AuthNetTransHashSha2Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AuthNetNetworkTransIdFilter), e => e.AuthNetNetworkTransId.Contains(input.AuthNetNetworkTransIdFilter))
                        .WhereIf(input.MinPaidAmountFilter != null, e => e.PaidAmount >= input.MinPaidAmountFilter)
                        .WhereIf(input.MaxPaidAmountFilter != null, e => e.PaidAmount <= input.MaxPaidAmountFilter)
                        .WhereIf(input.MinPaidInCashFilter != null, e => e.PaidInCash >= input.MinPaidInCashFilter)
                        .WhereIf(input.MaxPaidInCashFilter != null, e => e.PaidInCash <= input.MaxPaidInCashFilter)
                        .WhereIf(input.MinAvailableUserBalanceFilter != null, e => e.AvailableUserBalance >= input.MinAvailableUserBalanceFilter)
                        .WhereIf(input.MaxAvailableUserBalanceFilter != null, e => e.AvailableUserBalance <= input.MaxAvailableUserBalanceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CardNameOnCardFilter), e => e.CardNameOnCard.Contains(input.CardNameOnCardFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CardZipCodeFilter), e => e.CardZipCode.Contains(input.CardZipCodeFilter))
                        .WhereIf(input.MinBalanceForwardFilter != null, e => e.BalanceForward >= input.MinBalanceForwardFilter)
                        .WhereIf(input.MaxBalanceForwardFilter != null, e => e.BalanceForward <= input.MaxBalanceForwardFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                        .WhereIf(!(input.UserIdFilter==null), e => e.UserFk != null && e.UserFk.Id == input.UserIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TenantDocumentNameFilter), e => e.TenantDocumentFk != null && e.TenantDocumentFk.Name == input.TenantDocumentNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CohortNameFilter), e => e.CohortFk != null && e.CohortFk.Name == input.CohortNameFilter);

            var pagedAndFilteredLedgerEntries = filteredLedgerEntries
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var ledgerEntries = from o in pagedAndFilteredLedgerEntries
                                join o1 in _userLookUpRepository.GetAll() on o.UserId equals o1.Id into j1
                                from s1 in j1.DefaultIfEmpty()

                                join o2 in _tenantDocumentLookUpRepository.GetAll() on o.TenantDocumentId equals o2.Id into j2
                                from s2 in j2.DefaultIfEmpty()

                                join o3 in _cohortLookUpRepository.GetAll() on o.CohortId equals o3.Id into j3
                                from s3 in j3.DefaultIfEmpty()

                                select new
                                {

                                    o.Name,
                                    o.ServiceType,
                                    o.Amount,
                                    o.DiscountAmount,
                                    o.TotalPrice,
                                    o.PaymentPeriodType,
                                    o.ExpirationDate,
                                    o.TransactionName,
                                    o.TransactionKey,
                                    o.TransactionId,
                                    o.Settled,
                                    o.AmountDue,
                                    o.PaymentToken,
                                    o.AuthNetCustomerProfileId,
                                    o.AuthNetCustomerPaymentProfileId,
                                    o.AuthNetCustomerAddressId,
                                    o.AccountNumber,
                                    o.Note,
                                    o.MetaData,
                                    o.AuthCode,
                                    o.ReferenceTransactionId,
                                    o.TransactionHash,
                                    o.AccountType,
                                    o.TransactionCode,
                                    o.TransactionMessage,
                                    o.AuthNetTransHashSha2,
                                    o.AuthNetNetworkTransId,
                                    o.PaidAmount,
                                    o.PaidInCash,
                                    o.AvailableUserBalance,
                                    o.CardNameOnCard,
                                    o.CardZipCode,
                                    o.BalanceForward,
                                    Id = o.Id,
                                    UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                    TenantDocumentName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                    CohortName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                                };

            var totalCount = await filteredLedgerEntries.CountAsync();

            var dbList = await ledgerEntries.ToListAsync();
            var results = new List<GetLedgerEntryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetLedgerEntryForViewDto()
                {
                    LedgerEntry = new LedgerEntryDto
                    {

                        Name = o.Name,
                        ServiceType = o.ServiceType,
                        Amount = o.Amount,
                        DiscountAmount = o.DiscountAmount,
                        TotalPrice = o.TotalPrice,
                        PaymentPeriodType = o.PaymentPeriodType,
                        ExpirationDate = o.ExpirationDate,
                        TransactionName = o.TransactionName,
                        TransactionKey = o.TransactionKey,
                        TransactionId = o.TransactionId,
                        Settled = o.Settled,
                        AmountDue = o.AmountDue,
                        PaymentToken = o.PaymentToken,
                        AuthNetCustomerProfileId = o.AuthNetCustomerProfileId,
                        AuthNetCustomerPaymentProfileId = o.AuthNetCustomerPaymentProfileId,
                        AuthNetCustomerAddressId = o.AuthNetCustomerAddressId,
                        AccountNumber = o.AccountNumber,
                        Note = o.Note,
                        MetaData = o.MetaData,
                        AuthCode = o.AuthCode,
                        ReferenceTransactionId = o.ReferenceTransactionId,
                        TransactionHash = o.TransactionHash,
                        AccountType = o.AccountType,
                        TransactionCode = o.TransactionCode,
                        TransactionMessage = o.TransactionMessage,
                        AuthNetTransHashSha2 = o.AuthNetTransHashSha2,
                        AuthNetNetworkTransId = o.AuthNetNetworkTransId,
                        PaidAmount = o.PaidAmount,
                        PaidInCash = o.PaidInCash,
                        AvailableUserBalance = o.AvailableUserBalance,
                        CardNameOnCard = o.CardNameOnCard,
                        CardZipCode = o.CardZipCode,
                        BalanceForward = o.BalanceForward,
                        Id = o.Id,
                    },
                    UserName = o.UserName,
                    TenantDocumentName = o.TenantDocumentName,
                    CohortName = o.CohortName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetLedgerEntryForViewDto>(
                totalCount,
                results
            );

        }


        public async Task<GetLedgerEntryForViewDto> GetLedgerEntryForView(Guid id)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var ledgerEntry = await _ledgerEntryRepository.GetAsync(id);

            var output = new GetLedgerEntryForViewDto { LedgerEntry = ObjectMapper.Map<LedgerEntryDto>(ledgerEntry) };

            if (output.LedgerEntry.UserId != null)
            {
                var _lookupUser = await _userLookUpRepository.FirstOrDefaultAsync((long)output.LedgerEntry.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.LedgerEntry.TenantDocumentId != null)
            {
                var _lookupTenantDocument = await _tenantDocumentLookUpRepository.FirstOrDefaultAsync((Guid)output.LedgerEntry.TenantDocumentId);
                output.TenantDocumentName = _lookupTenantDocument?.Name?.ToString();
            }

            if (output.LedgerEntry.CohortId != null)
            {
                var _lookupCohort = await _cohortLookUpRepository.FirstOrDefaultAsync((Guid)output.LedgerEntry.CohortId);
                output.CohortName = _lookupCohort?.Name?.ToString();
            }

            return output;

        }

        [AbpAuthorize(AppPermissions.Pages_LedgerEntries_Edit)]
        public async Task<GetLedgerEntryForEditOutput> GetLedgerEntryForEdit(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var ledgerEntry = await _ledgerEntryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetLedgerEntryForEditOutput { LedgerEntry = ObjectMapper.Map<CreateOrEditLedgerEntryDto>(ledgerEntry) };

            if (output.LedgerEntry.UserId != null)
            {
                var _lookupUser = await _userLookUpRepository.FirstOrDefaultAsync((long)output.LedgerEntry.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.LedgerEntry.TenantDocumentId != null)
            {
                var _lookupTenantDocument = await _tenantDocumentLookUpRepository.FirstOrDefaultAsync((Guid)output.LedgerEntry.TenantDocumentId);
                output.TenantDocumentName = _lookupTenantDocument?.Name?.ToString();
            }

            if (output.LedgerEntry.CohortId != null)
            {
                var _lookupCohort = await _cohortLookUpRepository.FirstOrDefaultAsync((Guid)output.LedgerEntry.CohortId);
                output.CohortName = _lookupCohort?.Name?.ToString();
            }

            return output;

        }

        public async Task CreateOrEdit(CreateOrEditLedgerEntryDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }

        }

        [AbpAuthorize(AppPermissions.Pages_LedgerEntries_Create)]
        protected virtual async Task Create(CreateOrEditLedgerEntryDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var ledgerEntry = ObjectMapper.Map<LedgerEntry>(input);

            if (AbpSession.TenantId != null)
            {
                ledgerEntry.TenantId = (int?)AbpSession.TenantId;
            }

            await _ledgerEntryRepository.InsertAsync(ledgerEntry);

        }

        [AbpAuthorize(AppPermissions.Pages_LedgerEntries_Edit)]
        protected virtual async Task Update(CreateOrEditLedgerEntryDto input)
        {

            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var ledgerEntry = await _ledgerEntryRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, ledgerEntry);

        }

        [AbpAuthorize(AppPermissions.Pages_LedgerEntries_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            await _ledgerEntryRepository.DeleteAsync(input.Id);

        }

        public async Task<FileDto> GetLedgerEntriesToExcel(GetAllLedgerEntriesForExcelInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var serviceTypeFilter = input.ServiceTypeFilter.HasValue
                        ? (EnumServiceType)input.ServiceTypeFilter
                        : default;
            var paymentPeriodTypeFilter = input.PaymentPeriodTypeFilter.HasValue
                ? (PaymentPeriodType)input.PaymentPeriodTypeFilter
                : default;

            var filteredLedgerEntries = _ledgerEntryRepository.GetAll()
                        .Include(e => e.UserFk)
                        .Include(e => e.TenantDocumentFk)
                        .Include(e => e.CohortFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.TransactionName.Contains(input.Filter) || e.TransactionKey.Contains(input.Filter) || e.TransactionId.Contains(input.Filter) || e.PaymentToken.Contains(input.Filter) || e.AuthNetCustomerProfileId.Contains(input.Filter) || e.AuthNetCustomerPaymentProfileId.Contains(input.Filter) || e.AuthNetCustomerAddressId.Contains(input.Filter) || e.AccountNumber.Contains(input.Filter) || e.Note.Contains(input.Filter) || e.MetaData.Contains(input.Filter) || e.AuthCode.Contains(input.Filter) || e.ReferenceTransactionId.Contains(input.Filter) || e.TransactionHash.Contains(input.Filter) || e.AccountType.Contains(input.Filter) || e.TransactionCode.Contains(input.Filter) || e.TransactionMessage.Contains(input.Filter) || e.AuthNetTransHashSha2.Contains(input.Filter) || e.AuthNetNetworkTransId.Contains(input.Filter) || e.CardNameOnCard.Contains(input.Filter) || e.CardZipCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.ServiceTypeFilter.HasValue && input.ServiceTypeFilter > -1, e => e.ServiceType == serviceTypeFilter)
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(input.MinDiscountAmountFilter != null, e => e.DiscountAmount >= input.MinDiscountAmountFilter)
                        .WhereIf(input.MaxDiscountAmountFilter != null, e => e.DiscountAmount <= input.MaxDiscountAmountFilter)
                        .WhereIf(input.MinTotalPriceFilter != null, e => e.TotalPrice >= input.MinTotalPriceFilter)
                        .WhereIf(input.MaxTotalPriceFilter != null, e => e.TotalPrice <= input.MaxTotalPriceFilter)
                        .WhereIf(input.PaymentPeriodTypeFilter.HasValue && input.PaymentPeriodTypeFilter > -1, e => e.PaymentPeriodType == paymentPeriodTypeFilter)
                        .WhereIf(input.MinExpirationDateFilter != null, e => e.ExpirationDate >= input.MinExpirationDateFilter)
                        .WhereIf(input.MaxExpirationDateFilter != null, e => e.ExpirationDate <= input.MaxExpirationDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TransactionNameFilter), e => e.TransactionName.Contains(input.TransactionNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TransactionKeyFilter), e => e.TransactionKey.Contains(input.TransactionKeyFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TransactionIdFilter), e => e.TransactionId.Contains(input.TransactionIdFilter))
                        .WhereIf(input.SettledFilter.HasValue && input.SettledFilter > -1, e => (input.SettledFilter == 1 && e.Settled) || (input.SettledFilter == 0 && !e.Settled))
                        .WhereIf(input.MinAmountDueFilter != null, e => e.AmountDue >= input.MinAmountDueFilter)
                        .WhereIf(input.MaxAmountDueFilter != null, e => e.AmountDue <= input.MaxAmountDueFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaymentTokenFilter), e => e.PaymentToken.Contains(input.PaymentTokenFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AuthNetCustomerProfileIdFilter), e => e.AuthNetCustomerProfileId.Contains(input.AuthNetCustomerProfileIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AuthNetCustomerPaymentProfileIdFilter), e => e.AuthNetCustomerPaymentProfileId.Contains(input.AuthNetCustomerPaymentProfileIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AuthNetCustomerAddressIdFilter), e => e.AuthNetCustomerAddressId.Contains(input.AuthNetCustomerAddressIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccountNumberFilter), e => e.AccountNumber.Contains(input.AccountNumberFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NoteFilter), e => e.Note.Contains(input.NoteFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MetaDataFilter), e => e.MetaData.Contains(input.MetaDataFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AuthCodeFilter), e => e.AuthCode.Contains(input.AuthCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReferenceTransactionIdFilter), e => e.ReferenceTransactionId.Contains(input.ReferenceTransactionIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TransactionHashFilter), e => e.TransactionHash.Contains(input.TransactionHashFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccountTypeFilter), e => e.AccountType.Contains(input.AccountTypeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TransactionCodeFilter), e => e.TransactionCode.Contains(input.TransactionCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TransactionMessageFilter), e => e.TransactionMessage.Contains(input.TransactionMessageFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AuthNetTransHashSha2Filter), e => e.AuthNetTransHashSha2.Contains(input.AuthNetTransHashSha2Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AuthNetNetworkTransIdFilter), e => e.AuthNetNetworkTransId.Contains(input.AuthNetNetworkTransIdFilter))
                        .WhereIf(input.MinPaidAmountFilter != null, e => e.PaidAmount >= input.MinPaidAmountFilter)
                        .WhereIf(input.MaxPaidAmountFilter != null, e => e.PaidAmount <= input.MaxPaidAmountFilter)
                        .WhereIf(input.MinPaidInCashFilter != null, e => e.PaidInCash >= input.MinPaidInCashFilter)
                        .WhereIf(input.MaxPaidInCashFilter != null, e => e.PaidInCash <= input.MaxPaidInCashFilter)
                        .WhereIf(input.MinAvailableUserBalanceFilter != null, e => e.AvailableUserBalance >= input.MinAvailableUserBalanceFilter)
                        .WhereIf(input.MaxAvailableUserBalanceFilter != null, e => e.AvailableUserBalance <= input.MaxAvailableUserBalanceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CardNameOnCardFilter), e => e.CardNameOnCard.Contains(input.CardNameOnCardFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CardZipCodeFilter), e => e.CardZipCode.Contains(input.CardZipCodeFilter))
                        .WhereIf(input.MinBalanceForwardFilter != null, e => e.BalanceForward >= input.MinBalanceForwardFilter)
                        .WhereIf(input.MaxBalanceForwardFilter != null, e => e.BalanceForward <= input.MaxBalanceForwardFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TenantDocumentNameFilter), e => e.TenantDocumentFk != null && e.TenantDocumentFk.Name == input.TenantDocumentNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CohortNameFilter), e => e.CohortFk != null && e.CohortFk.Name == input.CohortNameFilter);

            var query = (from o in filteredLedgerEntries
                         join o1 in _userLookUpRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _tenantDocumentLookUpRepository.GetAll() on o.TenantDocumentId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _cohortLookUpRepository.GetAll() on o.CohortId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetLedgerEntryForViewDto()
                         {
                             LedgerEntry = new LedgerEntryDto
                             {
                                 Name = o.Name,
                                 ServiceType = o.ServiceType,
                                 Amount = o.Amount,
                                 DiscountAmount = o.DiscountAmount,
                                 TotalPrice = o.TotalPrice,
                                 PaymentPeriodType = o.PaymentPeriodType,
                                 ExpirationDate = o.ExpirationDate,
                                 TransactionName = o.TransactionName,
                                 TransactionKey = o.TransactionKey,
                                 TransactionId = o.TransactionId,
                                 Settled = o.Settled,
                                 AmountDue = o.AmountDue,
                                 PaymentToken = o.PaymentToken,
                                 AuthNetCustomerProfileId = o.AuthNetCustomerProfileId,
                                 AuthNetCustomerPaymentProfileId = o.AuthNetCustomerPaymentProfileId,
                                 AuthNetCustomerAddressId = o.AuthNetCustomerAddressId,
                                 AccountNumber = o.AccountNumber,
                                 Note = o.Note,
                                 MetaData = o.MetaData,
                                 AuthCode = o.AuthCode,
                                 ReferenceTransactionId = o.ReferenceTransactionId,
                                 TransactionHash = o.TransactionHash,
                                 AccountType = o.AccountType,
                                 TransactionCode = o.TransactionCode,
                                 TransactionMessage = o.TransactionMessage,
                                 AuthNetTransHashSha2 = o.AuthNetTransHashSha2,
                                 AuthNetNetworkTransId = o.AuthNetNetworkTransId,
                                 PaidAmount = o.PaidAmount,
                                 PaidInCash = o.PaidInCash,
                                 AvailableUserBalance = o.AvailableUserBalance,
                                 CardNameOnCard = o.CardNameOnCard,
                                 CardZipCode = o.CardZipCode,
                                 BalanceForward = o.BalanceForward,
                                 Id = o.Id
                             },
                             UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             TenantDocumentName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             CohortName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                         });

            var ledgerEntryListDtos = await query.ToListAsync();

            return _ledgerEntriesExcelExporter.ExportToFile(ledgerEntryListDtos);

        }

        [AbpAuthorize(AppPermissions.Pages_LedgerEntries)]
        public async Task<PagedResultDto<LedgerEntryUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var query = _userLookUpRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<LedgerEntryUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new LedgerEntryUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.Name?.ToString()
                });
            }

            return new PagedResultDto<LedgerEntryUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );

        }

        [AbpAuthorize(AppPermissions.Pages_LedgerEntries)]
        public async Task<PagedResultDto<LedgerEntryTenantDocumentLookupTableDto>> GetAllTenantDocumentForLookupTable(GetAllForLookupTableInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var query = _tenantDocumentLookUpRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var tenantDocumentList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<LedgerEntryTenantDocumentLookupTableDto>();
            foreach (var tenantDocument in tenantDocumentList)
            {
                lookupTableDtoList.Add(new LedgerEntryTenantDocumentLookupTableDto
                {
                    Id = tenantDocument.Id.ToString(),
                    DisplayName = tenantDocument.Name?.ToString()
                });
            }

            return new PagedResultDto<LedgerEntryTenantDocumentLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );

        }

        [AbpAuthorize(AppPermissions.Pages_LedgerEntries)]
        public async Task<PagedResultDto<LedgerEntryCohortLookupTableDto>> GetAllCohortForLookupTable(GetAllForLookupTableInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var query = _cohortLookUpRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var cohortList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<LedgerEntryCohortLookupTableDto>();
            foreach (var cohort in cohortList)
            {
                lookupTableDtoList.Add(new LedgerEntryCohortLookupTableDto
                {
                    Id = cohort.Id.ToString(),
                    DisplayName = cohort.Name?.ToString()
                });
            }

            return new PagedResultDto<LedgerEntryCohortLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );

        }

        [AbpAuthorize(AppPermissions.Pages_LedgerEntries)]
        public async Task<UserEditDto> GetUserDetails(long userId)
        {
            if (AbpSession.TenantId == null)
            {
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            }

            var user = await _userLookUpRepository.FirstOrDefaultAsync(userId);
            var userEditDto = ObjectMapper.Map<UserEditDto>(user);
            return userEditDto;
        }

    }
}