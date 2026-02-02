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
    [AbpAuthorize(AppPermissions.Pages_RecordCategoryRules)]
    public class RecordCategoryRulesAppService : inzibackendAppServiceBase, IRecordCategoryRulesAppService
    {
        private readonly IRepository<RecordCategoryRule, Guid> _recordCategoryRuleRepository;
        private readonly IRecordCategoryRulesExcelExporter _recordCategoryRulesExcelExporter;

        public RecordCategoryRulesAppService(IRepository<RecordCategoryRule, Guid> recordCategoryRuleRepository, IRecordCategoryRulesExcelExporter recordCategoryRulesExcelExporter)
        {
            _recordCategoryRuleRepository = recordCategoryRuleRepository;
            _recordCategoryRulesExcelExporter = recordCategoryRulesExcelExporter;

        }

        public async Task<PagedResultDto<GetRecordCategoryRuleForViewDto>> GetAll(GetAllRecordCategoryRulesInput input)
        {
            if (AbpSession.TenantId == null)
            {
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            }

            var filteredRecordCategoryRules = _recordCategoryRuleRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.MetaData.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(input.NotifyFilter.HasValue && input.NotifyFilter > -1, e => (input.NotifyFilter == 1 && e.Notify) || (input.NotifyFilter == 0 && !e.Notify))
                        .WhereIf(input.MinExpireInDaysFilter != null, e => e.ExpireInDays >= input.MinExpireInDaysFilter)
                        .WhereIf(input.MaxExpireInDaysFilter != null, e => e.ExpireInDays <= input.MaxExpireInDaysFilter)
                        .WhereIf(input.MinWarnDaysBeforeFirstFilter != null, e => e.WarnDaysBeforeFirst >= input.MinWarnDaysBeforeFirstFilter)
                        .WhereIf(input.MaxWarnDaysBeforeFirstFilter != null, e => e.WarnDaysBeforeFirst <= input.MaxWarnDaysBeforeFirstFilter)
                        .WhereIf(input.ExpiresFilter.HasValue && input.ExpiresFilter > -1, e => (input.ExpiresFilter == 1 && e.Expires) || (input.ExpiresFilter == 0 && !e.Expires))
                        .WhereIf(input.RequiredFilter.HasValue && input.RequiredFilter > -1, e => (input.RequiredFilter == 1 && e.Required) || (input.RequiredFilter == 0 && !e.Required))
                        .WhereIf(input.IsSurpathOnlyFilter.HasValue && input.IsSurpathOnlyFilter > -1, e => (input.IsSurpathOnlyFilter == 1 && e.IsSurpathOnly) || (input.IsSurpathOnlyFilter == 0 && !e.IsSurpathOnly))
                        .WhereIf(input.MinWarnDaysBeforeSecondFilter != null, e => e.WarnDaysBeforeSecond >= input.MinWarnDaysBeforeSecondFilter)
                        .WhereIf(input.MaxWarnDaysBeforeSecondFilter != null, e => e.WarnDaysBeforeSecond <= input.MaxWarnDaysBeforeSecondFilter)
                        .WhereIf(input.MinWarnDaysBeforeFinalFilter != null, e => e.WarnDaysBeforeFinal >= input.MinWarnDaysBeforeFinalFilter)
                        .WhereIf(input.MaxWarnDaysBeforeFinalFilter != null, e => e.WarnDaysBeforeFinal <= input.MaxWarnDaysBeforeFinalFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MetaDataFilter), e => e.MetaData.Contains(input.MetaDataFilter));

            var pagedAndFilteredRecordCategoryRules = filteredRecordCategoryRules
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var recordCategoryRules = from o in pagedAndFilteredRecordCategoryRules
                                      select new
                                      {

                                          o.Name,
                                          o.Description,
                                          o.Notify,
                                          o.ExpireInDays,
                                          o.WarnDaysBeforeFirst,
                                          o.Expires,
                                          o.Required,
                                          o.IsSurpathOnly,
                                          o.WarnDaysBeforeSecond,
                                          o.WarnDaysBeforeFinal,
                                          o.MetaData,
                                          Id = o.Id
                                      };

            var totalCount = await filteredRecordCategoryRules.CountAsync();

            var dbList = await recordCategoryRules.ToListAsync();
            var results = new List<GetRecordCategoryRuleForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetRecordCategoryRuleForViewDto()
                {
                    RecordCategoryRule = new RecordCategoryRuleDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        Notify = o.Notify,
                        ExpireInDays = o.ExpireInDays,
                        WarnDaysBeforeFirst = o.WarnDaysBeforeFirst,
                        Expires = o.Expires,
                        Required = o.Required,
                        IsSurpathOnly = o.IsSurpathOnly,
                        WarnDaysBeforeSecond = o.WarnDaysBeforeSecond,
                        WarnDaysBeforeFinal = o.WarnDaysBeforeFinal,
                        MetaData = o.MetaData,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetRecordCategoryRuleForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetRecordCategoryRuleForViewDto> GetRecordCategoryRuleForView(Guid id)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var recordCategoryRule = await _recordCategoryRuleRepository.GetAsync(id);

            var output = new GetRecordCategoryRuleForViewDto { RecordCategoryRule = ObjectMapper.Map<RecordCategoryRuleDto>(recordCategoryRule) };

            return output;

        }

        [AbpAuthorize(AppPermissions.Pages_RecordCategoryRules_Edit)]
        public async Task<GetRecordCategoryRuleForEditOutput> GetRecordCategoryRuleForEdit(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var recordCategoryRule = await _recordCategoryRuleRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetRecordCategoryRuleForEditOutput { RecordCategoryRule = ObjectMapper.Map<CreateOrEditRecordCategoryRuleDto>(recordCategoryRule) };

            return output;

        }

        public async Task CreateOrEdit(CreateOrEditRecordCategoryRuleDto input)
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

        [AbpAuthorize(AppPermissions.Pages_RecordCategoryRules_Create)]
        protected virtual async Task Create(CreateOrEditRecordCategoryRuleDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var recordCategoryRule = ObjectMapper.Map<RecordCategoryRule>(input);

            if (AbpSession.TenantId != null)
            {
                recordCategoryRule.TenantId = (int?)AbpSession.TenantId;
            }

            await _recordCategoryRuleRepository.InsertAsync(recordCategoryRule);

        }

        [AbpAuthorize(AppPermissions.Pages_RecordCategoryRules_Edit)]
        protected virtual async Task Update(CreateOrEditRecordCategoryRuleDto input)
        {

            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var recordCategoryRule = await _recordCategoryRuleRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, recordCategoryRule);

        }

        [AbpAuthorize(AppPermissions.Pages_RecordCategoryRules_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            await _recordCategoryRuleRepository.DeleteAsync(input.Id);

        }

        public async Task<FileDto> GetRecordCategoryRulesToExcel(GetAllRecordCategoryRulesForExcelInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var filteredRecordCategoryRules = _recordCategoryRuleRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.MetaData.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(input.NotifyFilter.HasValue && input.NotifyFilter > -1, e => (input.NotifyFilter == 1 && e.Notify) || (input.NotifyFilter == 0 && !e.Notify))
                        .WhereIf(input.MinExpireInDaysFilter != null, e => e.ExpireInDays >= input.MinExpireInDaysFilter)
                        .WhereIf(input.MaxExpireInDaysFilter != null, e => e.ExpireInDays <= input.MaxExpireInDaysFilter)
                        .WhereIf(input.MinWarnDaysBeforeFirstFilter != null, e => e.WarnDaysBeforeFirst >= input.MinWarnDaysBeforeFirstFilter)
                        .WhereIf(input.MaxWarnDaysBeforeFirstFilter != null, e => e.WarnDaysBeforeFirst <= input.MaxWarnDaysBeforeFirstFilter)
                        .WhereIf(input.ExpiresFilter.HasValue && input.ExpiresFilter > -1, e => (input.ExpiresFilter == 1 && e.Expires) || (input.ExpiresFilter == 0 && !e.Expires))
                        .WhereIf(input.RequiredFilter.HasValue && input.RequiredFilter > -1, e => (input.RequiredFilter == 1 && e.Required) || (input.RequiredFilter == 0 && !e.Required))
                        .WhereIf(input.IsSurpathOnlyFilter.HasValue && input.IsSurpathOnlyFilter > -1, e => (input.IsSurpathOnlyFilter == 1 && e.IsSurpathOnly) || (input.IsSurpathOnlyFilter == 0 && !e.IsSurpathOnly))
                        .WhereIf(input.MinWarnDaysBeforeSecondFilter != null, e => e.WarnDaysBeforeSecond >= input.MinWarnDaysBeforeSecondFilter)
                        .WhereIf(input.MaxWarnDaysBeforeSecondFilter != null, e => e.WarnDaysBeforeSecond <= input.MaxWarnDaysBeforeSecondFilter)
                        .WhereIf(input.MinWarnDaysBeforeFinalFilter != null, e => e.WarnDaysBeforeFinal >= input.MinWarnDaysBeforeFinalFilter)
                        .WhereIf(input.MaxWarnDaysBeforeFinalFilter != null, e => e.WarnDaysBeforeFinal <= input.MaxWarnDaysBeforeFinalFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MetaDataFilter), e => e.MetaData.Contains(input.MetaDataFilter));

            var query = (from o in filteredRecordCategoryRules
                         select new GetRecordCategoryRuleForViewDto()
                         {
                             RecordCategoryRule = new RecordCategoryRuleDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Notify = o.Notify,
                                 ExpireInDays = o.ExpireInDays,
                                 WarnDaysBeforeFirst = o.WarnDaysBeforeFirst,
                                 Expires = o.Expires,
                                 Required = o.Required,
                                 IsSurpathOnly = o.IsSurpathOnly,
                                 WarnDaysBeforeSecond = o.WarnDaysBeforeSecond,
                                 WarnDaysBeforeFinal = o.WarnDaysBeforeFinal,
                                 MetaData = o.MetaData,
                                 Id = o.Id
                             }
                         });

            var recordCategoryRuleListDtos = await query.ToListAsync();

            return _recordCategoryRulesExcelExporter.ExportToFile(recordCategoryRuleListDtos);

        }

    }
}