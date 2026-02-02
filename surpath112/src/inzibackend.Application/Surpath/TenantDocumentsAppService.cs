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
    [AbpAuthorize(AppPermissions.Pages_TenantDocuments)]
    public class TenantDocumentsAppService : inzibackendAppServiceBase, ITenantDocumentsAppService
    {
        private readonly IRepository<TenantDocument, Guid> _tenantDocumentRepository;
        private readonly ITenantDocumentsExcelExporter _tenantDocumentsExcelExporter;
        private readonly IRepository<TenantDocumentCategory, Guid> _tenantDocumentCategoryLookUpRepository;
        private readonly IRepository<Record, Guid> _recordLookUpRepository;

        public TenantDocumentsAppService(IRepository<TenantDocument, Guid> tenantDocumentRepository, ITenantDocumentsExcelExporter tenantDocumentsExcelExporter, IRepository<TenantDocumentCategory, Guid> lookup_tenantDocumentCategoryRepository, IRepository<Record, Guid> lookup_recordRepository)
        {
            _tenantDocumentRepository = tenantDocumentRepository;
            _tenantDocumentsExcelExporter = tenantDocumentsExcelExporter;
            _tenantDocumentCategoryLookUpRepository = lookup_tenantDocumentCategoryRepository;
            _recordLookUpRepository = lookup_recordRepository;

        }

        public async Task<PagedResultDto<GetTenantDocumentForViewDto>> GetAll(GetAllTenantDocumentsInput input)
        {
                if (AbpSession.TenantId == null)
                {
                    CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                }

                var filteredTenantDocuments = _tenantDocumentRepository.GetAll()
                            .Include(e => e.TenantDocumentCategoryFk)
                            .Include(e => e.RecordFk)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                            .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                            .WhereIf(input.AuthorizedOnlyFilter.HasValue && input.AuthorizedOnlyFilter > -1, e => (input.AuthorizedOnlyFilter == 1 && e.AuthorizedOnly) || (input.AuthorizedOnlyFilter == 0 && !e.AuthorizedOnly))
                            .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.TenantDocumentCategoryNameFilter), e => e.TenantDocumentCategoryFk != null && e.TenantDocumentCategoryFk.Name == input.TenantDocumentCategoryNameFilter)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.TenantDocumentCategoryIdFilter.ToString()), e => e.TenantDocumentCategoryFk != null && e.TenantDocumentCategoryFk.Id == input.TenantDocumentCategoryIdFilter)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.RecordfilenameFilter), e => e.RecordFk != null && e.RecordFk.filename == input.RecordfilenameFilter);

                var pagedAndFilteredTenantDocuments = filteredTenantDocuments
                    .OrderBy(input.Sorting ?? "name asc")
                    .PageBy(input);

                var tenantDocuments = from o in pagedAndFilteredTenantDocuments
                                      join o1 in _tenantDocumentCategoryLookUpRepository.GetAll() on o.TenantDocumentCategoryId equals o1.Id into j1
                                      from s1 in j1.DefaultIfEmpty()

                                      join o2 in _recordLookUpRepository.GetAll() on o.RecordId equals o2.Id into j2
                                      from s2 in j2.DefaultIfEmpty()

                                      select new
                                      {

                                          o.Name,
                                          o.AuthorizedOnly,
                                          o.Description,
                                          Id = o.Id,
                                          TenantDocumentCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                          Recordfilename = s2 == null || s2.filename == null ? "" : s2.filename.ToString()
                                      };

                var totalCount = await filteredTenantDocuments.CountAsync();

                var dbList = await tenantDocuments.ToListAsync();
                var results = new List<GetTenantDocumentForViewDto>();

                foreach (var o in dbList)
                {
                    var res = new GetTenantDocumentForViewDto()
                    {
                        TenantDocument = new TenantDocumentDto
                        {

                            Name = o.Name,
                            AuthorizedOnly = o.AuthorizedOnly,
                            Description = o.Description,
                            Id = o.Id,
                        },
                        TenantDocumentCategoryName = o.TenantDocumentCategoryName,
                        Recordfilename = o.Recordfilename
                    };

                    results.Add(res);
                }

                return new PagedResultDto<GetTenantDocumentForViewDto>(
                    totalCount,
                    results
                );
        }

        public async Task<GetTenantDocumentForViewDto> GetTenantDocumentForView(Guid id)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var tenantDocument = await _tenantDocumentRepository.GetAsync(id);

                var output = new GetTenantDocumentForViewDto { TenantDocument = ObjectMapper.Map<TenantDocumentDto>(tenantDocument) };

                if (output.TenantDocument.TenantDocumentCategoryId != null)
                {
                    var _lookupTenantDocumentCategory = await _tenantDocumentCategoryLookUpRepository.FirstOrDefaultAsync((Guid)output.TenantDocument.TenantDocumentCategoryId);
                    output.TenantDocumentCategoryName = _lookupTenantDocumentCategory?.Name?.ToString();
                }

                if (output.TenantDocument.RecordId != null)
                {
                    var _lookupRecord = await _recordLookUpRepository.FirstOrDefaultAsync((Guid)output.TenantDocument.RecordId);
                    output.Recordfilename = _lookupRecord?.filename?.ToString();
                    output.TenantDocument.BinaryObjId = _lookupRecord.BinaryObjId;
                }

                return output;
        }

        [AbpAuthorize(AppPermissions.Pages_TenantDocuments_Edit)]
        public async Task<GetTenantDocumentForEditOutput> GetTenantDocumentForEdit(EntityDto<Guid> input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var tenantDocument = await _tenantDocumentRepository.FirstOrDefaultAsync(input.Id);

                var output = new GetTenantDocumentForEditOutput { TenantDocument = ObjectMapper.Map<CreateOrEditTenantDocumentDto>(tenantDocument) };

                if (output.TenantDocument.TenantDocumentCategoryId != null)
                {
                    var _lookupTenantDocumentCategory = await _tenantDocumentCategoryLookUpRepository.FirstOrDefaultAsync((Guid)output.TenantDocument.TenantDocumentCategoryId);
                    output.TenantDocumentCategoryName = _lookupTenantDocumentCategory?.Name?.ToString();
                }

                if (output.TenantDocument.RecordId != null)
                {
                    var _lookupRecord = await _recordLookUpRepository.FirstOrDefaultAsync((Guid)output.TenantDocument.RecordId);
                    output.Recordfilename = _lookupRecord?.filename?.ToString();
                }

                return output;
        }

        public async Task CreateOrEdit(CreateOrEditTenantDocumentDto input)
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

        [AbpAuthorize(AppPermissions.Pages_TenantDocuments_Create)]
        protected virtual async Task Create(CreateOrEditTenantDocumentDto input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var tenantDocument = ObjectMapper.Map<TenantDocument>(input);

                if (AbpSession.TenantId != null)
                {
                    tenantDocument.TenantId = (int?)AbpSession.TenantId;
                }

                await _tenantDocumentRepository.InsertAsync(tenantDocument);

        }

        [AbpAuthorize(AppPermissions.Pages_TenantDocuments_Edit)]
        protected virtual async Task Update(CreateOrEditTenantDocumentDto input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                var tenantDocument = await _tenantDocumentRepository.FirstOrDefaultAsync((Guid)input.Id);
                ObjectMapper.Map(input, tenantDocument);

        }

        [AbpAuthorize(AppPermissions.Pages_TenantDocuments_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                await _tenantDocumentRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetTenantDocumentsToExcel(GetAllTenantDocumentsForExcelInput input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var filteredTenantDocuments = _tenantDocumentRepository.GetAll()
                            .Include(e => e.TenantDocumentCategoryFk)
                            .Include(e => e.RecordFk)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                            .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                            .WhereIf(input.AuthorizedOnlyFilter.HasValue && input.AuthorizedOnlyFilter > -1, e => (input.AuthorizedOnlyFilter == 1 && e.AuthorizedOnly) || (input.AuthorizedOnlyFilter == 0 && !e.AuthorizedOnly))
                            .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.TenantDocumentCategoryNameFilter), e => e.TenantDocumentCategoryFk != null && e.TenantDocumentCategoryFk.Name == input.TenantDocumentCategoryNameFilter)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.RecordfilenameFilter), e => e.RecordFk != null && e.RecordFk.filename == input.RecordfilenameFilter);

                var query = (from o in filteredTenantDocuments
                             join o1 in _tenantDocumentCategoryLookUpRepository.GetAll() on o.TenantDocumentCategoryId equals o1.Id into j1
                             from s1 in j1.DefaultIfEmpty()

                             join o2 in _recordLookUpRepository.GetAll() on o.RecordId equals o2.Id into j2
                             from s2 in j2.DefaultIfEmpty()

                             select new GetTenantDocumentForViewDto()
                             {
                                 TenantDocument = new TenantDocumentDto
                                 {
                                     Name = o.Name,
                                     AuthorizedOnly = o.AuthorizedOnly,
                                     Description = o.Description,
                                     Id = o.Id
                                 },
                                 TenantDocumentCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                 Recordfilename = s2 == null || s2.filename == null ? "" : s2.filename.ToString()
                             });

                var tenantDocumentListDtos = await query.ToListAsync();

                return _tenantDocumentsExcelExporter.ExportToFile(tenantDocumentListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_TenantDocuments)]
        public async Task<PagedResultDto<TenantDocumentTenantDocumentCategoryLookupTableDto>> GetAllTenantDocumentCategoryForLookupTable(GetAllForLookupTableInput input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                var query = _tenantDocumentCategoryLookUpRepository.GetAll().WhereIf(
                       !string.IsNullOrWhiteSpace(input.Filter),
                      e => e.Name != null && e.Name.Contains(input.Filter)
                   );

                var totalCount = await query.CountAsync();

                var tenantDocumentCategoryList = await query
                    .PageBy(input)
                    .ToListAsync();

                var lookupTableDtoList = new List<TenantDocumentTenantDocumentCategoryLookupTableDto>();
                foreach (var tenantDocumentCategory in tenantDocumentCategoryList)
                {
                    lookupTableDtoList.Add(new TenantDocumentTenantDocumentCategoryLookupTableDto
                    {
                        Id = tenantDocumentCategory.Id.ToString(),
                        DisplayName = tenantDocumentCategory.Name?.ToString()
                    });
                }

                return new PagedResultDto<TenantDocumentTenantDocumentCategoryLookupTableDto>(
                    totalCount,
                    lookupTableDtoList
                );
        }

        [AbpAuthorize(AppPermissions.Pages_TenantDocuments)]
        public async Task<PagedResultDto<TenantDocumentRecordLookupTableDto>> GetAllRecordForLookupTable(GetAllForLookupTableInput input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                var query = _recordLookUpRepository.GetAll().WhereIf(
                       !string.IsNullOrWhiteSpace(input.Filter),
                      e => e.filename != null && e.filename.Contains(input.Filter)
                   );

                var totalCount = await query.CountAsync();

                var recordList = await query
                    .PageBy(input)
                    .ToListAsync();

                var lookupTableDtoList = new List<TenantDocumentRecordLookupTableDto>();
                foreach (var record in recordList)
                {
                    lookupTableDtoList.Add(new TenantDocumentRecordLookupTableDto
                    {
                        Id = record.Id.ToString(),
                        DisplayName = record.filename?.ToString()
                    });
                }

                return new PagedResultDto<TenantDocumentRecordLookupTableDto>(
                    totalCount,
                    lookupTableDtoList
                );
        }

    }
}