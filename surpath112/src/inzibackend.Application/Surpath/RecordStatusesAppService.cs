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
using inzibackend.MultiTenancy;


namespace inzibackend.Surpath
{
    [AbpAuthorize(AppPermissions.Pages_RecordStatuses)]
    public class RecordStatusesAppService : inzibackendAppServiceBase, IRecordStatusesAppService
    {
        public TenantManager TenantManager { get; set; }

        private readonly IRepository<RecordStatus, Guid> _recordStatusRepository;
        private readonly IRepository<RecordState, Guid> _recordStateRepository;

        private readonly IRecordStatusesExcelExporter _recordStatusesExcelExporter;
        private readonly IRepository<TenantDepartment, Guid> _tenantDepartmentLookUpRepository;

        public RecordStatusesAppService(IRepository<RecordStatus, Guid> recordStatusRepository,
            IRecordStatusesExcelExporter recordStatusesExcelExporter,
            IRepository<RecordState, Guid> recordStateRepository,
            IRepository<TenantDepartment, Guid> lookup_tenantDepartmentRepository)
        {
            _recordStatusRepository = recordStatusRepository;
            _recordStatusesExcelExporter = recordStatusesExcelExporter;
            _recordStateRepository = recordStateRepository;
            _tenantDepartmentLookUpRepository = lookup_tenantDepartmentRepository;

        }

        public async Task<PagedResultDto<GetRecordStatusForViewDto>> GetAll(GetAllRecordStatusesInput input)
        {
            if (AbpSession.TenantId == null)
            {
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            }

            if (AbpSession.ImpersonatorUserId != null)
            {
                input.IsSurpathServiceStatusFilter = 1;
            }

            var complianceImpactFilter = input.ComplianceImpactFilter.HasValue
                        ? (EnumStatusComplianceImpact)input.ComplianceImpactFilter
                        : default;

            var filteredRecordStatuses = _recordStatusRepository.GetAll()
                        .Include(e => e.TenantDepartmentFk)
                            //.WhereIf(AbpSession.TenantId != null, e => e.IsSurpathServiceStatus == false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.StatusName.Contains(input.Filter) || e.HtmlColor.Contains(input.Filter) || e.CSSCLass.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StatusNameFilter), e => e.StatusName.Contains(input.StatusNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HtmlColorFilter), e => e.HtmlColor.Contains(input.HtmlColorFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CSSCLassFilter), e => e.CSSCLass.Contains(input.CSSCLassFilter))
                        .WhereIf(input.IsDefaultFilter.HasValue && input.IsDefaultFilter > -1, e => (input.IsDefaultFilter == 1 && e.IsDefault) || (input.IsDefaultFilter == 0 && !e.IsDefault))
                        .WhereIf(input.RequireNoteOnSetFilter.HasValue && input.RequireNoteOnSetFilter > -1, e => (input.RequireNoteOnSetFilter == 1 && e.RequireNoteOnSet) || (input.RequireNoteOnSetFilter == 0 && !e.RequireNoteOnSet))
                        //.WhereIf(input.IsSurpathServiceStatusFilter.HasValue && input.IsSurpathServiceStatusFilter > -1, e => ((input.IsSurpathServiceStatusFilter == 1 && e.IsSurpathServiceStatus) || (input.IsSurpathServiceStatusFilter == 0 && !e.IsSurpathServiceStatus)))
                        .WhereIf(!input.IsSurpathServiceStatusFilter.HasValue, e => input.IsSurpathServiceStatusFilter == 0 && !e.IsSurpathServiceStatus)
                        .WhereIf(input.ComplianceImpactFilter.HasValue && input.ComplianceImpactFilter > -1, e => e.ComplianceImpact == complianceImpactFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TenantDepartmentNameFilter), e => e.TenantDepartmentFk != null && e.TenantDepartmentFk.Name == input.TenantDepartmentNameFilter);

            var pagedAndFilteredRecordStatuses = filteredRecordStatuses
                .OrderBy(input.Sorting ?? "statusname asc")
                .PageBy(input);

            var recordStatuses = from o in pagedAndFilteredRecordStatuses
                                 join o1 in _tenantDepartmentLookUpRepository.GetAll() on o.TenantDepartmentId equals o1.Id into j1
                                 from s1 in j1.DefaultIfEmpty()
                                 join o2 in TenantManager.Tenants on o.TenantId equals o2.Id into j2
                                 from tn in j2.DefaultIfEmpty()

                                 select new
                                 {

                                     o.StatusName,
                                     o.HtmlColor,
                                     o.CSSCLass,
                                     o.IsDefault,
                                     o.RequireNoteOnSet,
                                     o.IsSurpathServiceStatus,
                                     o.ComplianceImpact,
                                     Id = o.Id,
                                     TenantDepartmentName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                     o.TenantId,
                                     TenantName = String.IsNullOrWhiteSpace(tn.Name) ? "Surpath" : tn.Name,
                                 };

            var totalCount = await filteredRecordStatuses.CountAsync();

            var dbList = await recordStatuses.ToListAsync();
            var results = new List<GetRecordStatusForViewDto>();

            // order dbList so items where IsSurpathServiceStatus is true are at the end of the list.
            dbList = dbList.OrderBy(o => o.IsSurpathServiceStatus).ToList();


            foreach (var o in dbList)
            {
                var res = new GetRecordStatusForViewDto()
                {
                    RecordStatus = new RecordStatusDto
                    {

                        StatusName = o.StatusName,
                        HtmlColor = o.HtmlColor,
                        CSSCLass = o.CSSCLass,
                        IsDefault = o.IsDefault,
                        RequireNoteOnSet = o.RequireNoteOnSet,
                        IsSurpathServiceStatus = o.IsSurpathServiceStatus,
                        ComplianceImpact = o.ComplianceImpact,
                        Id = o.Id,
                    },
                    TenantDepartmentName = o.TenantDepartmentName,
                    TenantName = o.TenantName

                };

                results.Add(res);
            }

            return new PagedResultDto<GetRecordStatusForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetRecordStatusForViewDto> GetRecordStatusForView(Guid id)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var recordStatus = await _recordStatusRepository.GetAsync(id);

            var output = new GetRecordStatusForViewDto { RecordStatus = ObjectMapper.Map<RecordStatusDto>(recordStatus) };

            if (output.RecordStatus.TenantDepartmentId != null)
            {
                var _lookupTenantDepartment = await _tenantDepartmentLookUpRepository.FirstOrDefaultAsync((Guid)output.RecordStatus.TenantDepartmentId);
                output.TenantDepartmentName = _lookupTenantDepartment?.Name?.ToString();
            }

            return output;

        }

        [AbpAuthorize(AppPermissions.Pages_RecordStatuses_Edit)]
        public async Task<GetRecordStatusForEditOutput> GetRecordStatusForEdit(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var recordStatus = await _recordStatusRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetRecordStatusForEditOutput { RecordStatus = ObjectMapper.Map<CreateOrEditRecordStatusDto>(recordStatus) };

            if (output.RecordStatus.TenantDepartmentId != null)
            {
                var _lookupTenantDepartment = await _tenantDepartmentLookUpRepository.FirstOrDefaultAsync((Guid)output.RecordStatus.TenantDepartmentId);
                output.TenantDepartmentName = _lookupTenantDepartment?.Name?.ToString();
            }

            return output;

        }

        public async Task CreateOrEdit(CreateOrEditRecordStatusDto input)
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

            // if this is a surpath status and the tenantid on the input is null, we need to update all the tenant's surpath services status

        }

        private async Task SurpathServiceStatusHandler(RecordStatus recordStatus)
        {
            if (AbpSession.TenantId == null && recordStatus.IsSurpathServiceStatus == true)
            {
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                var tenants = TenantManager.Tenants.ToList();
                foreach (var tenant in tenants)
                {
                    await EnsureSurpathServiceStatuses(tenant.Id);
                }
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Tenants)]

        public async Task EnsureSurpathServiceStatuses(int tenantId)
        {
            if (AbpSession.TenantId != null) { return; }
            //CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            //var _t = _recordStatusRepository.GetAll().AsNoTracking().ToList();
            //CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var _surpathRecordStatuses = _recordStatusRepository.GetAll().IgnoreQueryFilters().AsNoTracking().Where(r => r.TenantId == null && r.IsDeleted == false && r.IsSurpathServiceStatus == true).ToList();
            var _tenantRecordStatuses = _recordStatusRepository.GetAll().IgnoreQueryFilters().AsNoTracking().Where(r => r.TenantId == tenantId).ToList();

            foreach (var recordStatus in _surpathRecordStatuses)
            {
                var _surpathRecordStatusId = recordStatus.Id;
                if (_tenantRecordStatuses.Where(r => r.TemplateServiceId == _surpathRecordStatusId).Any()) continue;
                var _exists = _tenantRecordStatuses.Where(r =>
                    r.TenantId == tenantId &&
                    r.StatusName.Trim().Equals(recordStatus.StatusName.Trim()) &&
                    (r.HtmlColor + "").Trim().Equals(recordStatus.HtmlColor.Trim()) &&
                    (r.CSSCLass + "").Trim().Equals(recordStatus.CSSCLass.Trim()) &&
                    r.IsSurpathServiceStatus == recordStatus.IsSurpathServiceStatus &&
                    (int)r.ComplianceImpact == (int)recordStatus.ComplianceImpact).FirstOrDefault();

                if (_exists != null)
                {
                    // this status needs it's template id set
                    var _rs = _recordStatusRepository.GetAll().IgnoreQueryFilters().Where(r => r.Id == _exists.Id).First();
                    _rs.TemplateServiceId = _surpathRecordStatusId;
                    await _recordStatusRepository.UpdateAsync(_rs);
                }
                else
                {
                    recordStatus.Id = Guid.NewGuid();
                    recordStatus.TenantId = tenantId;
                    recordStatus.TemplateServiceId = _surpathRecordStatusId;
                    recordStatus.IsDefault = !_tenantRecordStatuses.Where(r => r.IsDefault == true).Any();

                    await _recordStatusRepository.InsertAsync(recordStatus);
                    var _recordStatesPointingToSurpathStatus = await _recordStateRepository.GetAll().Where(rs => rs.RecordStatusId == _surpathRecordStatusId).ToListAsync();
                    foreach (var _rs in _recordStatesPointingToSurpathStatus)
                    {
                        _rs.RecordStatusId = recordStatus.Id;
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                }

                await CurrentUnitOfWork.SaveChangesAsync();

            }
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_RecordStatuses_Create)]
        protected virtual async Task Create(CreateOrEditRecordStatusDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var recordStatus = ObjectMapper.Map<RecordStatus>(input);

            if (AbpSession.TenantId != null)
            {
                recordStatus.TenantId = (int?)AbpSession.TenantId;
            }

            await _recordStatusRepository.InsertAsync(recordStatus);
            if (recordStatus.IsDefault)
                await SetDefaultRecordStatus(recordStatus.Id, (int)recordStatus.TenantId, recordStatus.IsSurpathServiceStatus);
            await SurpathServiceStatusHandler(recordStatus);

        }

        [AbpAuthorize(AppPermissions.Pages_RecordStatuses_Edit)]
        protected virtual async Task Update(CreateOrEditRecordStatusDto input)
        {

            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var recordStatus = await _recordStatusRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, recordStatus);
            if (recordStatus.IsDefault)
                await SetDefaultRecordStatus(recordStatus.Id, recordStatus.TenantId, recordStatus.IsSurpathServiceStatus);
            await SurpathServiceStatusHandler(recordStatus);
        }

        [AbpAuthorize(AppPermissions.Pages_RecordStatuses_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var recordStatus = await _recordStatusRepository.FirstOrDefaultAsync((Guid)input.Id);
            if (recordStatus.IsDefault)
            {
                return; //throw?
            }
            await _recordStatusRepository.DeleteAsync(input.Id);

        }

        public async Task<FileDto> GetRecordStatusesToExcel(GetAllRecordStatusesForExcelInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var complianceImpactFilter = input.ComplianceImpactFilter.HasValue
                        ? (EnumStatusComplianceImpact)input.ComplianceImpactFilter
                        : default;

            var filteredRecordStatuses = _recordStatusRepository.GetAll()
                        .Include(e => e.TenantDepartmentFk)
                            .WhereIf(AbpSession.TenantId == null, e => e.IsSurpathServiceStatus == false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.StatusName.Contains(input.Filter) || e.HtmlColor.Contains(input.Filter) || e.CSSCLass.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StatusNameFilter), e => e.StatusName.Contains(input.StatusNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HtmlColorFilter), e => e.HtmlColor.Contains(input.HtmlColorFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CSSCLassFilter), e => e.CSSCLass.Contains(input.CSSCLassFilter))
                        .WhereIf(input.IsDefaultFilter.HasValue && input.IsDefaultFilter > -1, e => (input.IsDefaultFilter == 1 && e.IsDefault) || (input.IsDefaultFilter == 0 && !e.IsDefault))
                        .WhereIf(input.RequireNoteOnSetFilter.HasValue && input.RequireNoteOnSetFilter > -1, e => (input.RequireNoteOnSetFilter == 1 && e.RequireNoteOnSet) || (input.RequireNoteOnSetFilter == 0 && !e.RequireNoteOnSet))
                        .WhereIf(input.IsSurpathServiceStatusFilter.HasValue && input.IsSurpathServiceStatusFilter > -1, e => (input.IsSurpathServiceStatusFilter == 1 && e.IsSurpathServiceStatus) || (input.IsSurpathServiceStatusFilter == 0 && !e.IsSurpathServiceStatus))
                        .WhereIf(input.ComplianceImpactFilter.HasValue && input.ComplianceImpactFilter > -1, e => e.ComplianceImpact == complianceImpactFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TenantDepartmentNameFilter), e => e.TenantDepartmentFk != null && e.TenantDepartmentFk.Name == input.TenantDepartmentNameFilter);

            var query = (from o in filteredRecordStatuses
                         join o1 in _tenantDepartmentLookUpRepository.GetAll() on o.TenantDepartmentId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetRecordStatusForViewDto()
                         {
                             RecordStatus = new RecordStatusDto
                             {
                                 StatusName = o.StatusName,
                                 HtmlColor = o.HtmlColor,
                                 CSSCLass = o.CSSCLass,
                                 IsDefault = o.IsDefault,
                                 RequireNoteOnSet = o.RequireNoteOnSet,
                                 IsSurpathServiceStatus = o.IsSurpathServiceStatus,
                                 ComplianceImpact = o.ComplianceImpact,
                                 Id = o.Id
                             },
                             TenantDepartmentName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var recordStatusListDtos = await query.ToListAsync();

            return _recordStatusesExcelExporter.ExportToFile(recordStatusListDtos);

        }


        private async Task SetDefaultRecordStatus(Guid id, int? TenantId, bool IsSurpathServiceStatus)
        {
            var filteredRecordStatuses = _recordStatusRepository.GetAll();
            var Default = _recordStatusRepository.GetAll().Where(r => r.Id == id && r.TenantId == TenantId && r.IsSurpathServiceStatus == IsSurpathServiceStatus).FirstOrDefault();
            if (Default == null)
            {
                return; // throw?
            }
            // if (Default.Id == id) return; // already default

            var others = _recordStatusRepository.GetAll().Where(r => r.TenantId == TenantId && r.Id != id && r.IsSurpathServiceStatus == IsSurpathServiceStatus);
            if (others != null)
            {
                foreach (var other in others)
                {
                    other.IsDefault = false;
                    _recordStatusRepository.Update(other);
                }
            }

            //var currentdefault = _recordStatusRepository.GetAll().Where(r => r.Id == id && r.TenantId == TenantId).FirstOrDefault();
            //if (currentdefault==null)
            //{
            //    var first = _recordStatusRepository.GetAll().Where(r => r.TenantId == TenantId).FirstOrDefault();
            //    if (first == null) return;
            //    first.IsDefault = true;
            //    _recordStatusRepository.Update(first);
            //    currentdefault = first;
            //}

            //_recordStatusRepository.Update(currentdefault);



            return;
        }

        public async Task<List<RecordStatusDto>> GetAllForDropdown()
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var recordStatuses = await _recordStatusRepository.GetAll()
                .WhereIf(AbpSession.TenantId != null, e => e.IsSurpathServiceStatus == false)
                .Select(rs => new RecordStatusDto
                {
                    Id = rs.Id,
                    StatusName = rs.StatusName,
                    HtmlColor = rs.HtmlColor,
                    CSSCLass = rs.CSSCLass,
                    IsDefault = rs.IsDefault,
                    RequireNoteOnSet = rs.RequireNoteOnSet,
                    IsSurpathServiceStatus = rs.IsSurpathServiceStatus,
                    ComplianceImpact = rs.ComplianceImpact
                })
                .ToListAsync();

            return recordStatuses;
        }

    }
}
