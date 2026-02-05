using inzibackend.Surpath;
using inzibackend.Surpath;
using inzibackend.Surpath;

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

namespace inzibackend.Surpath
{
    [AbpAuthorize(AppPermissions.Pages_LedgerEntryDetails)]
    public class LedgerEntryDetailsAppService : inzibackendAppServiceBase, ILedgerEntryDetailsAppService
    {
        private readonly IRepository<LedgerEntryDetail, Guid> _ledgerEntryDetailRepository;
        private readonly ILedgerEntryDetailsExcelExporter _ledgerEntryDetailsExcelExporter;
        private readonly IRepository<LedgerEntry, Guid> _ledgerEntryLookUpRepository;
        private readonly IRepository<SurpathService, Guid> _surpathServiceLookUpRepository;
        private readonly IRepository<TenantSurpathService, Guid> _tenantSurpathServiceLookUpRepository;

        public LedgerEntryDetailsAppService(IRepository<LedgerEntryDetail, Guid> ledgerEntryDetailRepository, ILedgerEntryDetailsExcelExporter ledgerEntryDetailsExcelExporter, IRepository<LedgerEntry, Guid> lookup_ledgerEntryRepository, IRepository<SurpathService, Guid> lookup_surpathServiceRepository, IRepository<TenantSurpathService, Guid> lookup_tenantSurpathServiceRepository)
        {
            _ledgerEntryDetailRepository = ledgerEntryDetailRepository;
            _ledgerEntryDetailsExcelExporter = ledgerEntryDetailsExcelExporter;
            _ledgerEntryLookUpRepository = lookup_ledgerEntryRepository;
            _surpathServiceLookUpRepository = lookup_surpathServiceRepository;
            _tenantSurpathServiceLookUpRepository = lookup_tenantSurpathServiceRepository;

        }

        public async Task<PagedResultDto<GetLedgerEntryDetailForViewDto>> GetAll(GetAllLedgerEntryDetailsInput input)
        {
            if (AbpSession.TenantId == null)
            {
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            }

            var filteredLedgerEntryDetails = _ledgerEntryDetailRepository.GetAll()
                        .Include(e => e.LedgerEntryFk)
                        .Include(e => e.SurpathServiceFk)
                        .Include(e => e.TenantSurpathServiceFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Note.Contains(input.Filter) || e.MetaData.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NoteFilter), e => e.Note.Contains(input.NoteFilter))
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(input.MinDiscountFilter != null, e => e.Discount >= input.MinDiscountFilter)
                        .WhereIf(input.MaxDiscountFilter != null, e => e.Discount <= input.MaxDiscountFilter)
                        .WhereIf(input.MinDiscountAmountFilter != null, e => e.DiscountAmount >= input.MinDiscountAmountFilter)
                        .WhereIf(input.MaxDiscountAmountFilter != null, e => e.DiscountAmount <= input.MaxDiscountAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MetaDataFilter), e => e.MetaData.Contains(input.MetaDataFilter))
                        .WhereIf(input.MinAmountPaidFilter != null, e => e.AmountPaid >= input.MinAmountPaidFilter)
                        .WhereIf(input.MaxAmountPaidFilter != null, e => e.AmountPaid <= input.MaxAmountPaidFilter)
                        .WhereIf(input.MinDatePaidOnFilter != null, e => e.DatePaidOn >= input.MinDatePaidOnFilter)
                        .WhereIf(input.MaxDatePaidOnFilter != null, e => e.DatePaidOn <= input.MaxDatePaidOnFilter)
                        .WhereIf(input.LedgerEntryIdFilter.HasValue, e => false || e.LedgerEntryId == input.LedgerEntryIdFilter.Value)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LedgerEntryTransactionIdFilter), e => e.LedgerEntryFk != null && e.LedgerEntryFk.TransactionId == input.LedgerEntryTransactionIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SurpathServiceNameFilter), e => e.SurpathServiceFk != null && e.SurpathServiceFk.Name == input.SurpathServiceNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TenantSurpathServiceNameFilter), e => e.TenantSurpathServiceFk != null && e.TenantSurpathServiceFk.Name == input.TenantSurpathServiceNameFilter);

            var pagedAndFilteredLedgerEntryDetails = filteredLedgerEntryDetails
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var ledgerEntryDetails = from o in pagedAndFilteredLedgerEntryDetails
                                     join o1 in _ledgerEntryLookUpRepository.GetAll() on o.LedgerEntryId equals o1.Id into j1
                                     from s1 in j1.DefaultIfEmpty()

                                     join o2 in _surpathServiceLookUpRepository.GetAll() on o.SurpathServiceId equals o2.Id into j2
                                     from s2 in j2.DefaultIfEmpty()

                                     join o3 in _tenantSurpathServiceLookUpRepository.GetAll() on o.TenantSurpathServiceId equals o3.Id into j3
                                     from s3 in j3.DefaultIfEmpty()

                                     select new
                                     {

                                         o.Note,
                                         o.Amount,
                                         o.Discount,
                                         o.DiscountAmount,
                                         o.MetaData,
                                         o.AmountPaid,
                                         o.DatePaidOn,
                                         Id = o.Id,
                                         LedgerEntryTransactionId = s1 == null || s1.TransactionId == null ? "" : s1.TransactionId.ToString(),
                                         SurpathServiceName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                         TenantSurpathServiceName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                                     };

            var totalCount = await filteredLedgerEntryDetails.CountAsync();

            var dbList = await ledgerEntryDetails.ToListAsync();
            var results = new List<GetLedgerEntryDetailForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetLedgerEntryDetailForViewDto()
                {
                    LedgerEntryDetail = new LedgerEntryDetailDto
                    {

                        Note = o.Note,
                        Amount = o.Amount,
                        Discount = o.Discount,
                        DiscountAmount = o.DiscountAmount,
                        MetaData = o.MetaData,
                        AmountPaid = o.AmountPaid,
                        DatePaidOn = o.DatePaidOn,
                        Id = o.Id,
                    },
                    LedgerEntryTransactionId = o.LedgerEntryTransactionId,
                    SurpathServiceName = o.SurpathServiceName,
                    TenantSurpathServiceName = o.TenantSurpathServiceName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetLedgerEntryDetailForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetLedgerEntryDetailForViewDto> GetLedgerEntryDetailForView(Guid id)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var ledgerEntryDetail = await _ledgerEntryDetailRepository.GetAsync(id);

            var output = new GetLedgerEntryDetailForViewDto { LedgerEntryDetail = ObjectMapper.Map<LedgerEntryDetailDto>(ledgerEntryDetail) };

            if (output.LedgerEntryDetail.LedgerEntryId != null)
            {
                var _lookupLedgerEntry = await _ledgerEntryLookUpRepository.FirstOrDefaultAsync((Guid)output.LedgerEntryDetail.LedgerEntryId);
                output.LedgerEntryTransactionId = _lookupLedgerEntry?.TransactionId?.ToString();
            }

            if (output.LedgerEntryDetail.SurpathServiceId != null)
            {
                var _lookupSurpathService = await _surpathServiceLookUpRepository.FirstOrDefaultAsync((Guid)output.LedgerEntryDetail.SurpathServiceId);
                output.SurpathServiceName = _lookupSurpathService?.Name?.ToString();
            }

            if (output.LedgerEntryDetail.TenantSurpathServiceId != null)
            {
                var _lookupTenantSurpathService = await _tenantSurpathServiceLookUpRepository.FirstOrDefaultAsync((Guid)output.LedgerEntryDetail.TenantSurpathServiceId);
                output.TenantSurpathServiceName = _lookupTenantSurpathService?.Name?.ToString();
            }

            return output;

        }

        [AbpAuthorize(AppPermissions.Pages_LedgerEntryDetails_Edit)]
        public async Task<GetLedgerEntryDetailForEditOutput> GetLedgerEntryDetailForEdit(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var ledgerEntryDetail = await _ledgerEntryDetailRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetLedgerEntryDetailForEditOutput { LedgerEntryDetail = ObjectMapper.Map<CreateOrEditLedgerEntryDetailDto>(ledgerEntryDetail) };

            if (output.LedgerEntryDetail.LedgerEntryId != null)
            {
                var _lookupLedgerEntry = await _ledgerEntryLookUpRepository.FirstOrDefaultAsync((Guid)output.LedgerEntryDetail.LedgerEntryId);
                output.LedgerEntryTransactionId = _lookupLedgerEntry?.TransactionId?.ToString();
            }

            if (output.LedgerEntryDetail.SurpathServiceId != null)
            {
                var _lookupSurpathService = await _surpathServiceLookUpRepository.FirstOrDefaultAsync((Guid)output.LedgerEntryDetail.SurpathServiceId);
                output.SurpathServiceName = _lookupSurpathService?.Name?.ToString();
            }

            if (output.LedgerEntryDetail.TenantSurpathServiceId != null)
            {
                var _lookupTenantSurpathService = await _tenantSurpathServiceLookUpRepository.FirstOrDefaultAsync((Guid)output.LedgerEntryDetail.TenantSurpathServiceId);
                output.TenantSurpathServiceName = _lookupTenantSurpathService?.Name?.ToString();
            }

            return output;

        }

        public async Task CreateOrEdit(CreateOrEditLedgerEntryDetailDto input)
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

        [AbpAuthorize(AppPermissions.Pages_LedgerEntryDetails_Create)]
        protected virtual async Task Create(CreateOrEditLedgerEntryDetailDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var ledgerEntryDetail = ObjectMapper.Map<LedgerEntryDetail>(input);

            if (AbpSession.TenantId != null)
            {
                ledgerEntryDetail.TenantId = (int?)AbpSession.TenantId;
            }

            await _ledgerEntryDetailRepository.InsertAsync(ledgerEntryDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_LedgerEntryDetails_Edit)]
        protected virtual async Task Update(CreateOrEditLedgerEntryDetailDto input)
        {

            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var ledgerEntryDetail = await _ledgerEntryDetailRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, ledgerEntryDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_LedgerEntryDetails_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            await _ledgerEntryDetailRepository.DeleteAsync(input.Id);

        }

        public async Task<FileDto> GetLedgerEntryDetailsToExcel(GetAllLedgerEntryDetailsForExcelInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var filteredLedgerEntryDetails = _ledgerEntryDetailRepository.GetAll()
                        .Include(e => e.LedgerEntryFk)
                        .Include(e => e.SurpathServiceFk)
                        .Include(e => e.TenantSurpathServiceFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Note.Contains(input.Filter) || e.MetaData.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NoteFilter), e => e.Note.Contains(input.NoteFilter))
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(input.MinDiscountFilter != null, e => e.Discount >= input.MinDiscountFilter)
                        .WhereIf(input.MaxDiscountFilter != null, e => e.Discount <= input.MaxDiscountFilter)
                        .WhereIf(input.MinDiscountAmountFilter != null, e => e.DiscountAmount >= input.MinDiscountAmountFilter)
                        .WhereIf(input.MaxDiscountAmountFilter != null, e => e.DiscountAmount <= input.MaxDiscountAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MetaDataFilter), e => e.MetaData.Contains(input.MetaDataFilter))
                        .WhereIf(input.MinAmountPaidFilter != null, e => e.AmountPaid >= input.MinAmountPaidFilter)
                        .WhereIf(input.MaxAmountPaidFilter != null, e => e.AmountPaid <= input.MaxAmountPaidFilter)
                        .WhereIf(input.MinDatePaidOnFilter != null, e => e.DatePaidOn >= input.MinDatePaidOnFilter)
                        .WhereIf(input.MaxDatePaidOnFilter != null, e => e.DatePaidOn <= input.MaxDatePaidOnFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LedgerEntryTransactionIdFilter), e => e.LedgerEntryFk != null && e.LedgerEntryFk.TransactionId == input.LedgerEntryTransactionIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SurpathServiceNameFilter), e => e.SurpathServiceFk != null && e.SurpathServiceFk.Name == input.SurpathServiceNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TenantSurpathServiceNameFilter), e => e.TenantSurpathServiceFk != null && e.TenantSurpathServiceFk.Name == input.TenantSurpathServiceNameFilter);

            var query = (from o in filteredLedgerEntryDetails
                         join o1 in _ledgerEntryLookUpRepository.GetAll() on o.LedgerEntryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _surpathServiceLookUpRepository.GetAll() on o.SurpathServiceId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _tenantSurpathServiceLookUpRepository.GetAll() on o.TenantSurpathServiceId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetLedgerEntryDetailForViewDto()
                         {
                             LedgerEntryDetail = new LedgerEntryDetailDto
                             {
                                 Note = o.Note,
                                 Amount = o.Amount,
                                 Discount = o.Discount,
                                 DiscountAmount = o.DiscountAmount,
                                 MetaData = o.MetaData,
                                 AmountPaid = o.AmountPaid,
                                 DatePaidOn = o.DatePaidOn,
                                 Id = o.Id
                             },
                             LedgerEntryTransactionId = s1 == null || s1.TransactionId == null ? "" : s1.TransactionId.ToString(),
                             SurpathServiceName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             TenantSurpathServiceName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                         });

            var ledgerEntryDetailListDtos = await query.ToListAsync();

            return _ledgerEntryDetailsExcelExporter.ExportToFile(ledgerEntryDetailListDtos);

        }

        [AbpAuthorize(AppPermissions.Pages_LedgerEntryDetails)]
        public async Task<PagedResultDto<LedgerEntryDetailLedgerEntryLookupTableDto>> GetAllLedgerEntryForLookupTable(GetAllForLookupTableInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var query = _ledgerEntryLookUpRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.TransactionId != null && e.TransactionId.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var ledgerEntryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<LedgerEntryDetailLedgerEntryLookupTableDto>();
            foreach (var ledgerEntry in ledgerEntryList)
            {
                lookupTableDtoList.Add(new LedgerEntryDetailLedgerEntryLookupTableDto
                {
                    Id = ledgerEntry.Id.ToString(),
                    DisplayName = ledgerEntry.TransactionId?.ToString()
                });
            }

            return new PagedResultDto<LedgerEntryDetailLedgerEntryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );

        }

        [AbpAuthorize(AppPermissions.Pages_LedgerEntryDetails)]
        public async Task<PagedResultDto<LedgerEntryDetailSurpathServiceLookupTableDto>> GetAllSurpathServiceForLookupTable(GetAllForLookupTableInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var query = _surpathServiceLookUpRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var surpathServiceList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<LedgerEntryDetailSurpathServiceLookupTableDto>();
            foreach (var surpathService in surpathServiceList)
            {
                lookupTableDtoList.Add(new LedgerEntryDetailSurpathServiceLookupTableDto
                {
                    Id = surpathService.Id.ToString(),
                    DisplayName = surpathService.Name?.ToString()
                });
            }

            return new PagedResultDto<LedgerEntryDetailSurpathServiceLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );

        }

        [AbpAuthorize(AppPermissions.Pages_LedgerEntryDetails)]
        public async Task<PagedResultDto<LedgerEntryDetailTenantSurpathServiceLookupTableDto>> GetAllTenantSurpathServiceForLookupTable(GetAllForLookupTableInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var query = _tenantSurpathServiceLookUpRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var tenantSurpathServiceList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<LedgerEntryDetailTenantSurpathServiceLookupTableDto>();
            foreach (var tenantSurpathService in tenantSurpathServiceList)
            {
                lookupTableDtoList.Add(new LedgerEntryDetailTenantSurpathServiceLookupTableDto
                {
                    Id = tenantSurpathService.Id.ToString(),
                    DisplayName = tenantSurpathService.Name?.ToString()
                });
            }

            return new PagedResultDto<LedgerEntryDetailTenantSurpathServiceLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );

        }

    }
}