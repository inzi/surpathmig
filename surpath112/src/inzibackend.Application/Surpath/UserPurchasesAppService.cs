using inzibackend.Authorization.Users;
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
    [AbpAuthorize(AppPermissions.Pages_Administration_Host_Dashboard)]
    public class UserPurchasesAppService : inzibackendAppServiceBase, IUserPurchasesAppService
    {
        private readonly IRepository<UserPurchase, Guid> _userPurchaseRepository;
        private readonly IUserPurchasesExcelExporter _userPurchasesExcelExporter;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<SurpathService, Guid> _surpathServiceRepository;
        private readonly IRepository<TenantSurpathService, Guid> _tenantSurpathServiceRepository;
        private readonly IRepository<Cohort, Guid> _cohortRepository;
        private readonly IRepository<LedgerEntry, Guid> _ledgerEntryRepository;

        public UserPurchasesAppService(
            IRepository<UserPurchase, Guid> userPurchaseRepository,
            IUserPurchasesExcelExporter userPurchasesExcelExporter,
            IRepository<User, long> userRepository,
            IRepository<SurpathService, Guid> surpathServiceRepository,
            IRepository<TenantSurpathService, Guid> tenantSurpathServiceRepository,
            IRepository<Cohort, Guid> cohortRepository,
            IRepository<LedgerEntry, Guid> ledgerEntryRepository)
        {
            _userPurchaseRepository = userPurchaseRepository;
            _userPurchasesExcelExporter = userPurchasesExcelExporter;
            _userRepository = userRepository;
            _surpathServiceRepository = surpathServiceRepository;
            _tenantSurpathServiceRepository = tenantSurpathServiceRepository;
            _cohortRepository = cohortRepository;
            _ledgerEntryRepository = ledgerEntryRepository;
        }

        public async Task<PagedResultDto<GetUserPurchaseForViewDto>> GetAll(GetAllUserPurchasesInput input)
        {
            if (AbpSession.TenantId == null)
            {
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            }

            var statusFilter = input.StatusFilter.HasValue
                ? (EnumPurchaseStatus)input.StatusFilter
                : default;
            var paymentPeriodTypeFilter = input.PaymentPeriodTypeFilter.HasValue
                ? (PaymentPeriodType)input.PaymentPeriodTypeFilter
                : default;

            var filteredUserPurchases = _userPurchaseRepository.GetAll()
                .Include(e => e.UserFk)
                .Include(e => e.SurpathServiceFk)
                .Include(e => e.TenantSurpathServiceFk)
                .Include(e => e.CohortFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false 
                    || e.Name.Contains(input.Filter) 
                    || e.Description.Contains(input.Filter) 
                    || e.Notes.Contains(input.Filter) 
                    || e.MetaData.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                .WhereIf(input.MinOriginalPriceFilter != null, e => e.OriginalPrice >= input.MinOriginalPriceFilter)
                .WhereIf(input.MaxOriginalPriceFilter != null, e => e.OriginalPrice <= input.MaxOriginalPriceFilter)
                .WhereIf(input.MinAdjustedPriceFilter != null, e => e.AdjustedPrice >= input.MinAdjustedPriceFilter)
                .WhereIf(input.MaxAdjustedPriceFilter != null, e => e.AdjustedPrice <= input.MaxAdjustedPriceFilter)
                .WhereIf(input.MinDiscountAmountFilter != null, e => e.DiscountAmount >= input.MinDiscountAmountFilter)
                .WhereIf(input.MaxDiscountAmountFilter != null, e => e.DiscountAmount <= input.MaxDiscountAmountFilter)
                .WhereIf(input.MinAmountPaidFilter != null, e => e.AmountPaid >= input.MinAmountPaidFilter)
                .WhereIf(input.MaxAmountPaidFilter != null, e => e.AmountPaid <= input.MaxAmountPaidFilter)
                .WhereIf(input.PaymentPeriodTypeFilter.HasValue && input.PaymentPeriodTypeFilter > -1, e => e.PaymentPeriodType == paymentPeriodTypeFilter)
                .WhereIf(input.MinPurchaseDateFilter != null, e => e.PurchaseDate >= input.MinPurchaseDateFilter)
                .WhereIf(input.MaxPurchaseDateFilter != null, e => e.PurchaseDate <= input.MaxPurchaseDateFilter)
                .WhereIf(input.MinExpirationDateFilter != null, e => e.ExpirationDate >= input.MinExpirationDateFilter)
                .WhereIf(input.MaxExpirationDateFilter != null, e => e.ExpirationDate <= input.MaxExpirationDateFilter)
                .WhereIf(input.IsRecurringFilter.HasValue && input.IsRecurringFilter > -1, e => (input.IsRecurringFilter == 1 && e.IsRecurring) || (input.IsRecurringFilter == 0 && !e.IsRecurring))
                .WhereIf(!string.IsNullOrWhiteSpace(input.NotesFilter), e => e.Notes.Contains(input.NotesFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                .WhereIf(!(input.UserIdFilter == null), e => e.UserFk != null && e.UserFk.Id == input.UserIdFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.SurpathServiceNameFilter), e => e.SurpathServiceFk != null && e.SurpathServiceFk.Name == input.SurpathServiceNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.TenantSurpathServiceNameFilter), e => e.TenantSurpathServiceFk != null && e.TenantSurpathServiceFk.Name == input.TenantSurpathServiceNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.CohortNameFilter), e => e.CohortFk != null && e.CohortFk.Name == input.CohortNameFilter);

            var pagedAndFilteredUserPurchases = filteredUserPurchases
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var userPurchases = from o in pagedAndFilteredUserPurchases
                                join o1 in _userRepository.GetAll() on o.UserId equals o1.Id into j1
                                from s1 in j1.DefaultIfEmpty()

                                join o2 in _surpathServiceRepository.GetAll() on o.SurpathServiceId equals o2.Id into j2
                                from s2 in j2.DefaultIfEmpty()

                                join o3 in _tenantSurpathServiceRepository.GetAll() on o.TenantSurpathServiceId equals o3.Id into j3
                                from s3 in j3.DefaultIfEmpty()

                                join o4 in _cohortRepository.GetAll() on o.CohortId equals o4.Id into j4
                                from s4 in j4.DefaultIfEmpty()

                                select new GetUserPurchaseForViewDto
                                {
                                    UserPurchase = new UserPurchaseDto
                                    {
                                        Name = o.Name,
                                        Description = o.Description,
                                        Status = o.Status,
                                        OriginalPrice = o.OriginalPrice,
                                        AdjustedPrice = o.AdjustedPrice,
                                        DiscountAmount = o.DiscountAmount,
                                        AmountPaid = o.AmountPaid,
                                        PaymentPeriodType = o.PaymentPeriodType,
                                        PurchaseDate = o.PurchaseDate,
                                        ExpirationDate = o.ExpirationDate,
                                        IsRecurring = o.IsRecurring,
                                        Notes = o.Notes,
                                        MetaData = o.MetaData,
                                        BalanceDue = o.AdjustedPrice - o.AmountPaid,
                                        UserId = o.UserId,
                                        SurpathServiceId = o.SurpathServiceId,
                                        TenantSurpathServiceId = o.TenantSurpathServiceId,
                                        CohortId = o.CohortId,
                                        Id = o.Id
                                    },
                                    UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                    SurpathServiceName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                    TenantSurpathServiceName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                                    CohortName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                                };

            var totalCount = await filteredUserPurchases.CountAsync();

            return new PagedResultDto<GetUserPurchaseForViewDto>(
                totalCount,
                await userPurchases.ToListAsync()
            );
        }

        public async Task<PagedResultDto<GetUserPurchaseForViewDto>> GetAllForUserId(GetAllUserPurchasesInput input)
        {
            if (input.UserIdFilter == null)
            {
                throw new UserFriendlyException(L("UserIdRequired"));
            }

            if (AbpSession.TenantId == null)
            {
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            }

            var statusFilter = input.StatusFilter.HasValue
                ? (EnumPurchaseStatus)input.StatusFilter
                : default;
            var paymentPeriodTypeFilter = input.PaymentPeriodTypeFilter.HasValue
                ? (PaymentPeriodType)input.PaymentPeriodTypeFilter
                : default;

            var filteredUserPurchases = _userPurchaseRepository.GetAll()
                .Include(e => e.UserFk)
                .Include(e => e.SurpathServiceFk)
                .Include(e => e.TenantSurpathServiceFk)
                .Include(e => e.CohortFk)
                .Where(e => e.UserId == input.UserIdFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false
                    || e.Name.Contains(input.Filter)
                    || e.Description.Contains(input.Filter)
                    || e.Notes.Contains(input.Filter)
                    || e.MetaData.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                .WhereIf(input.MinOriginalPriceFilter != null, e => e.OriginalPrice >= input.MinOriginalPriceFilter)
                .WhereIf(input.MaxOriginalPriceFilter != null, e => e.OriginalPrice <= input.MaxOriginalPriceFilter)
                .WhereIf(input.MinAdjustedPriceFilter != null, e => e.AdjustedPrice >= input.MinAdjustedPriceFilter)
                .WhereIf(input.MaxAdjustedPriceFilter != null, e => e.AdjustedPrice <= input.MaxAdjustedPriceFilter)
                .WhereIf(input.MinDiscountAmountFilter != null, e => e.DiscountAmount >= input.MinDiscountAmountFilter)
                .WhereIf(input.MaxDiscountAmountFilter != null, e => e.DiscountAmount <= input.MaxDiscountAmountFilter)
                .WhereIf(input.MinAmountPaidFilter != null, e => e.AmountPaid >= input.MinAmountPaidFilter)
                .WhereIf(input.MaxAmountPaidFilter != null, e => e.AmountPaid <= input.MaxAmountPaidFilter)
                .WhereIf(input.PaymentPeriodTypeFilter.HasValue && input.PaymentPeriodTypeFilter > -1, e => e.PaymentPeriodType == paymentPeriodTypeFilter)
                .WhereIf(input.MinPurchaseDateFilter != null, e => e.PurchaseDate >= input.MinPurchaseDateFilter)
                .WhereIf(input.MaxPurchaseDateFilter != null, e => e.PurchaseDate <= input.MaxPurchaseDateFilter)
                .WhereIf(input.MinExpirationDateFilter != null, e => e.ExpirationDate >= input.MinExpirationDateFilter)
                .WhereIf(input.MaxExpirationDateFilter != null, e => e.ExpirationDate <= input.MaxExpirationDateFilter)
                .WhereIf(input.IsRecurringFilter.HasValue && input.IsRecurringFilter > -1, e => (input.IsRecurringFilter == 1 && e.IsRecurring) || (input.IsRecurringFilter == 0 && !e.IsRecurring))
                .WhereIf(!string.IsNullOrWhiteSpace(input.NotesFilter), e => e.Notes.Contains(input.NotesFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SurpathServiceNameFilter), e => e.SurpathServiceFk != null && e.SurpathServiceFk.Name == input.SurpathServiceNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.TenantSurpathServiceNameFilter), e => e.TenantSurpathServiceFk != null && e.TenantSurpathServiceFk.Name == input.TenantSurpathServiceNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.CohortNameFilter), e => e.CohortFk != null && e.CohortFk.Name == input.CohortNameFilter);

            var pagedAndFilteredUserPurchases = filteredUserPurchases
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var userPurchases = from o in pagedAndFilteredUserPurchases
                                join o1 in _userRepository.GetAll() on o.UserId equals o1.Id into j1
                                from s1 in j1.DefaultIfEmpty()

                                join o2 in _surpathServiceRepository.GetAll() on o.SurpathServiceId equals o2.Id into j2
                                from s2 in j2.DefaultIfEmpty()

                                join o3 in _tenantSurpathServiceRepository.GetAll() on o.TenantSurpathServiceId equals o3.Id into j3
                                from s3 in j3.DefaultIfEmpty()

                                join o4 in _cohortRepository.GetAll() on o.CohortId equals o4.Id into j4
                                from s4 in j4.DefaultIfEmpty()

                                select new GetUserPurchaseForViewDto
                                {
                                    UserPurchase = new UserPurchaseDto
                                    {
                                        Name = o.Name,
                                        Description = o.Description,
                                        Status = o.Status,
                                        OriginalPrice = o.OriginalPrice,
                                        AdjustedPrice = o.AdjustedPrice,
                                        DiscountAmount = o.DiscountAmount,
                                        AmountPaid = o.AmountPaid,
                                        PaymentPeriodType = o.PaymentPeriodType,
                                        PurchaseDate = o.PurchaseDate,
                                        ExpirationDate = o.ExpirationDate,
                                        IsRecurring = o.IsRecurring,
                                        Notes = o.Notes,
                                        MetaData = o.MetaData,
                                        BalanceDue = o.AdjustedPrice - o.AmountPaid,
                                        UserId = o.UserId,
                                        SurpathServiceId = o.SurpathServiceId,
                                        TenantSurpathServiceId = o.TenantSurpathServiceId,
                                        CohortId = o.CohortId,
                                        Id = o.Id
                                    },
                                    UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                    SurpathServiceName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                    TenantSurpathServiceName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                                    CohortName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                                };

            var totalCount = await filteredUserPurchases.CountAsync();

            return new PagedResultDto<GetUserPurchaseForViewDto>(
                totalCount,
                await userPurchases.ToListAsync()
            );
        }

        public async Task<GetUserPurchaseForViewDto> GetUserPurchaseForView(Guid id)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var userPurchase = await _userPurchaseRepository.GetAsync(id);

            var output = new GetUserPurchaseForViewDto 
            { 
                UserPurchase = ObjectMapper.Map<UserPurchaseDto>(userPurchase) 
            };

            output.UserPurchase.BalanceDue = userPurchase.AdjustedPrice - userPurchase.AmountPaid;

            if (output.UserPurchase.UserId != null)
            {
                var _lookupUser = await _userRepository.FirstOrDefaultAsync((long)output.UserPurchase.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.UserPurchase.SurpathServiceId != null)
            {
                var _lookupSurpathService = await _surpathServiceRepository.FirstOrDefaultAsync((Guid)output.UserPurchase.SurpathServiceId);
                output.SurpathServiceName = _lookupSurpathService?.Name?.ToString();
            }

            if (output.UserPurchase.TenantSurpathServiceId != null)
            {
                var _lookupTenantSurpathService = await _tenantSurpathServiceRepository.FirstOrDefaultAsync((Guid)output.UserPurchase.TenantSurpathServiceId);
                output.TenantSurpathServiceName = _lookupTenantSurpathService?.Name?.ToString();
            }

            if (output.UserPurchase.CohortId != null)
            {
                var _lookupCohort = await _cohortRepository.FirstOrDefaultAsync((Guid)output.UserPurchase.CohortId);
                output.CohortName = _lookupCohort?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Host_Dashboard)]
        public async Task<GetUserPurchaseForEditOutput> GetUserPurchaseForEdit(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var userPurchase = await _userPurchaseRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetUserPurchaseForEditOutput { UserPurchase = ObjectMapper.Map<CreateOrEditUserPurchaseDto>(userPurchase) };

            if (output.UserPurchase.UserId != null)
            {
                var _lookupUser = await _userRepository.FirstOrDefaultAsync((long)output.UserPurchase.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.UserPurchase.SurpathServiceId != null)
            {
                var _lookupSurpathService = await _surpathServiceRepository.FirstOrDefaultAsync((Guid)output.UserPurchase.SurpathServiceId);
                output.SurpathServiceName = _lookupSurpathService?.Name?.ToString();
            }

            if (output.UserPurchase.TenantSurpathServiceId != null)
            {
                var _lookupTenantSurpathService = await _tenantSurpathServiceRepository.FirstOrDefaultAsync((Guid)output.UserPurchase.TenantSurpathServiceId);
                output.TenantSurpathServiceName = _lookupTenantSurpathService?.Name?.ToString();
            }

            if (output.UserPurchase.CohortId != null)
            {
                var _lookupCohort = await _cohortRepository.FirstOrDefaultAsync((Guid)output.UserPurchase.CohortId);
                output.CohortName = _lookupCohort?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditUserPurchaseDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_Host_Dashboard)]
        protected virtual async Task Create(CreateOrEditUserPurchaseDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var userPurchase = ObjectMapper.Map<UserPurchase>(input);

            if (AbpSession.TenantId != null)
            {
                userPurchase.TenantId = (int?)AbpSession.TenantId;
            }

            await _userPurchaseRepository.InsertAsync(userPurchase);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Host_Dashboard)]
        protected virtual async Task Update(CreateOrEditUserPurchaseDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            
            var userPurchase = await _userPurchaseRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, userPurchase);
            
            // Update user purchase status based on balance
            UpdatePurchaseStatus(userPurchase);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Host_Dashboard)]
        public async Task Delete(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            
            await _userPurchaseRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetUserPurchasesToExcel(GetAllUserPurchasesForExcelInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            
            var statusFilter = input.StatusFilter.HasValue
                ? (EnumPurchaseStatus)input.StatusFilter
                : default;
            var paymentPeriodTypeFilter = input.PaymentPeriodTypeFilter.HasValue
                ? (PaymentPeriodType)input.PaymentPeriodTypeFilter
                : default;

            var filteredUserPurchases = _userPurchaseRepository.GetAll()
                .Include(e => e.UserFk)
                .Include(e => e.SurpathServiceFk)
                .Include(e => e.TenantSurpathServiceFk)
                .Include(e => e.CohortFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false
                    || e.Name.Contains(input.Filter)
                    || e.Description.Contains(input.Filter)
                    || e.Notes.Contains(input.Filter)
                    || e.MetaData.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                .WhereIf(input.MinOriginalPriceFilter != null, e => e.OriginalPrice >= input.MinOriginalPriceFilter)
                .WhereIf(input.MaxOriginalPriceFilter != null, e => e.OriginalPrice <= input.MaxOriginalPriceFilter)
                .WhereIf(input.MinAdjustedPriceFilter != null, e => e.AdjustedPrice >= input.MinAdjustedPriceFilter)
                .WhereIf(input.MaxAdjustedPriceFilter != null, e => e.AdjustedPrice <= input.MaxAdjustedPriceFilter)
                .WhereIf(input.MinDiscountAmountFilter != null, e => e.DiscountAmount >= input.MinDiscountAmountFilter)
                .WhereIf(input.MaxDiscountAmountFilter != null, e => e.DiscountAmount <= input.MaxDiscountAmountFilter)
                .WhereIf(input.MinAmountPaidFilter != null, e => e.AmountPaid >= input.MinAmountPaidFilter)
                .WhereIf(input.MaxAmountPaidFilter != null, e => e.AmountPaid <= input.MaxAmountPaidFilter)
                .WhereIf(input.PaymentPeriodTypeFilter.HasValue && input.PaymentPeriodTypeFilter > -1, e => e.PaymentPeriodType == paymentPeriodTypeFilter)
                .WhereIf(input.MinPurchaseDateFilter != null, e => e.PurchaseDate >= input.MinPurchaseDateFilter)
                .WhereIf(input.MaxPurchaseDateFilter != null, e => e.PurchaseDate <= input.MaxPurchaseDateFilter)
                .WhereIf(input.MinExpirationDateFilter != null, e => e.ExpirationDate >= input.MinExpirationDateFilter)
                .WhereIf(input.MaxExpirationDateFilter != null, e => e.ExpirationDate <= input.MaxExpirationDateFilter)
                .WhereIf(input.IsRecurringFilter.HasValue && input.IsRecurringFilter > -1, e => (input.IsRecurringFilter == 1 && e.IsRecurring) || (input.IsRecurringFilter == 0 && !e.IsRecurring))
                .WhereIf(!string.IsNullOrWhiteSpace(input.NotesFilter), e => e.Notes.Contains(input.NotesFilter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                .WhereIf(!(input.UserIdFilter == null), e => e.UserFk != null && e.UserFk.Id == input.UserIdFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.SurpathServiceNameFilter), e => e.SurpathServiceFk != null && e.SurpathServiceFk.Name == input.SurpathServiceNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.TenantSurpathServiceNameFilter), e => e.TenantSurpathServiceFk != null && e.TenantSurpathServiceFk.Name == input.TenantSurpathServiceNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.CohortNameFilter), e => e.CohortFk != null && e.CohortFk.Name == input.CohortNameFilter);

            var query = from o in filteredUserPurchases
                        join o1 in _userRepository.GetAll() on o.UserId equals o1.Id into j1
                        from s1 in j1.DefaultIfEmpty()

                        join o2 in _surpathServiceRepository.GetAll() on o.SurpathServiceId equals o2.Id into j2
                        from s2 in j2.DefaultIfEmpty()

                        join o3 in _tenantSurpathServiceRepository.GetAll() on o.TenantSurpathServiceId equals o3.Id into j3
                        from s3 in j3.DefaultIfEmpty()

                        join o4 in _cohortRepository.GetAll() on o.CohortId equals o4.Id into j4
                        from s4 in j4.DefaultIfEmpty()

                        select new GetUserPurchaseForViewDto
                        {
                            UserPurchase = new UserPurchaseDto
                            {
                                Name = o.Name,
                                Description = o.Description,
                                Status = o.Status,
                                OriginalPrice = o.OriginalPrice,
                                AdjustedPrice = o.AdjustedPrice,
                                DiscountAmount = o.DiscountAmount,
                                AmountPaid = o.AmountPaid,
                                PaymentPeriodType = o.PaymentPeriodType,
                                PurchaseDate = o.PurchaseDate,
                                ExpirationDate = o.ExpirationDate,
                                IsRecurring = o.IsRecurring,
                                Notes = o.Notes,
                                MetaData = o.MetaData,
                                BalanceDue = o.AdjustedPrice - o.AmountPaid,
                                UserId = o.UserId,
                                SurpathServiceId = o.SurpathServiceId,
                                TenantSurpathServiceId = o.TenantSurpathServiceId,
                                CohortId = o.CohortId,
                                Id = o.Id
                            },
                            UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                            SurpathServiceName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                            TenantSurpathServiceName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                            CohortName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                        };

            var userPurchaseListDtos = await query.ToListAsync();

            return _userPurchasesExcelExporter.ExportToFile(userPurchaseListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Host_Dashboard)]
        public async Task<PagedResultDto<UserPurchaseUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            
            var query = _userRepository.GetAll().WhereIf(
                !string.IsNullOrWhiteSpace(input.Filter),
                e => e.Name != null && e.Name.Contains(input.Filter)
            );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<UserPurchaseUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new UserPurchaseUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.Name?.ToString()
                });
            }

            return new PagedResultDto<UserPurchaseUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Host_Dashboard)]
        public async Task<PagedResultDto<UserPurchaseSurpathServiceLookupTableDto>> GetAllSurpathServiceForLookupTable(GetAllForLookupTableInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            
            var query = _surpathServiceRepository.GetAll().WhereIf(
                !string.IsNullOrWhiteSpace(input.Filter),
                e => e.Name != null && e.Name.Contains(input.Filter)
            );

            var totalCount = await query.CountAsync();

            var surpathServiceList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<UserPurchaseSurpathServiceLookupTableDto>();
            foreach (var surpathService in surpathServiceList)
            {
                lookupTableDtoList.Add(new UserPurchaseSurpathServiceLookupTableDto
                {
                    Id = surpathService.Id.ToString(),
                    DisplayName = surpathService.Name?.ToString()
                });
            }

            return new PagedResultDto<UserPurchaseSurpathServiceLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Host_Dashboard)]
        public async Task<PagedResultDto<UserPurchaseTenantSurpathServiceLookupTableDto>> GetAllTenantSurpathServiceForLookupTable(GetAllForLookupTableInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            
            var query = _tenantSurpathServiceRepository.GetAll().WhereIf(
                !string.IsNullOrWhiteSpace(input.Filter),
                e => e.Name != null && e.Name.Contains(input.Filter)
            );

            var totalCount = await query.CountAsync();

            var tenantSurpathServiceList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<UserPurchaseTenantSurpathServiceLookupTableDto>();
            foreach (var tenantSurpathService in tenantSurpathServiceList)
            {
                lookupTableDtoList.Add(new UserPurchaseTenantSurpathServiceLookupTableDto
                {
                    Id = tenantSurpathService.Id.ToString(),
                    DisplayName = tenantSurpathService.Name?.ToString()
                });
            }

            return new PagedResultDto<UserPurchaseTenantSurpathServiceLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Host_Dashboard)]
        public async Task<PagedResultDto<UserPurchaseCohortLookupTableDto>> GetAllCohortForLookupTable(GetAllForLookupTableInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            
            var query = _cohortRepository.GetAll().WhereIf(
                !string.IsNullOrWhiteSpace(input.Filter),
                e => e.Name != null && e.Name.Contains(input.Filter)
            );

            var totalCount = await query.CountAsync();

            var cohortList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<UserPurchaseCohortLookupTableDto>();
            foreach (var cohort in cohortList)
            {
                lookupTableDtoList.Add(new UserPurchaseCohortLookupTableDto
                {
                    Id = cohort.Id.ToString(),
                    DisplayName = cohort.Name?.ToString()
                });
            }

            return new PagedResultDto<UserPurchaseCohortLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        // Payment tracking methods
        [AbpAuthorize(AppPermissions.Pages_Administration_Host_Dashboard)]
        public async Task<GetUserPurchaseForViewDto> GetUserPurchaseBalance(Guid id)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            
            return await GetUserPurchaseForView(id);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Host_Dashboard)]
        public async Task<PagedResultDto<GetUserPurchaseForViewDto>> GetAllWithBalances(GetAllUserPurchasesInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            
            return await GetAll(input);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Host_Dashboard)]
        public async Task ApplyPayment(Guid userPurchaseId, double amount, string paymentMethod, string notes)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            
            var userPurchase = await _userPurchaseRepository.GetAsync(userPurchaseId);
            
            if (userPurchase == null)
            {
                throw new UserFriendlyException(L("UserPurchaseNotFound"));
            }
            
            if (amount <= 0)
            {
                throw new UserFriendlyException(L("PaymentAmountMustBeGreaterThanZero"));
            }
            
            // Update the amount paid
            userPurchase.AmountPaid += amount;
            
            // Create a ledger entry for this payment
            var paymentAmount = Convert.ToDecimal(amount);
            var ledgerEntry = new LedgerEntry
            {
                Name = $"Payment for {userPurchase.Name}",
                Amount = paymentAmount,
                TotalPrice = paymentAmount,
                TransactionName = $"Payment - {paymentMethod}",
                Note = notes,
                Settled = true,
                UserId = userPurchase.UserId,
                CohortId = userPurchase.CohortId,
                TenantId = userPurchase.TenantId,
                UserPurchaseId = userPurchase.Id,
                PaymentDate = DateTime.Now,
                PaymentMethod = paymentMethod
            };
            
            await _ledgerEntryRepository.InsertAsync(ledgerEntry);
            
            // Update user purchase status based on balance
            UpdatePurchaseStatus(userPurchase);
            
            await _userPurchaseRepository.UpdateAsync(userPurchase);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Host_Dashboard)]
        public async Task AdjustPrice(Guid userPurchaseId, double adjustedPrice, string reason)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            
            var userPurchase = await _userPurchaseRepository.GetAsync(userPurchaseId);
            
            if (userPurchase == null)
            {
                throw new UserFriendlyException(L("UserPurchaseNotFound"));
            }
            
            if (adjustedPrice < 0)
            {
                throw new UserFriendlyException(L("AdjustedPriceMustBeGreaterThanZero"));
            }
            
            // Store the original price before adjustment
            double originalAdjustedPrice = userPurchase.AdjustedPrice;
            
            // Update the adjusted price
            userPurchase.AdjustedPrice = adjustedPrice;
            
            // Add a note about the price adjustment
            string note = string.IsNullOrEmpty(userPurchase.Notes) ? "" : userPurchase.Notes + "\n";
            note += $"Price adjusted from ${originalAdjustedPrice} to ${adjustedPrice} on {DateTime.Now}. Reason: {reason}";
            userPurchase.Notes = note;
            
            // Update user purchase status based on balance
            UpdatePurchaseStatus(userPurchase);
            
            await _userPurchaseRepository.UpdateAsync(userPurchase);
        }
        
        private void UpdatePurchaseStatus(UserPurchase userPurchase)
        {
            // Calculate balance
            double balance = userPurchase.AdjustedPrice - userPurchase.AmountPaid;
            
            // Determine status based on balance and expiration
            if (userPurchase.ExpirationDate.HasValue && userPurchase.ExpirationDate.Value < DateTime.Now)
            {
                userPurchase.Status = EnumPurchaseStatus.Expired;
            }
            else if (balance < -0.01) // Small threshold for refund due (accounting for floating point precision)
            {
                userPurchase.Status = EnumPurchaseStatus.RefundDue;
            }
            else if (Math.Abs(balance) < 0.01) // Considered paid in full if within a penny
            {
                userPurchase.Status = EnumPurchaseStatus.PaidInFull;
            }
            else if (userPurchase.AmountPaid > 0)
            {
                userPurchase.Status = EnumPurchaseStatus.PartiallyPaid;
            }
            else
            {
                userPurchase.Status = EnumPurchaseStatus.New;
            }
        }
    }
}
