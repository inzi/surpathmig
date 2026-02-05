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
using inzibackend.Authorization.Users;
using inzibackend.Authorization.Organizations;

namespace inzibackend.Surpath
{
    [AbpAuthorize(AppPermissions.Pages_TenantDepartments)]
    public class TenantDepartmentsAppService : inzibackendAppServiceBase, ITenantDepartmentsAppService
    {
        private readonly IRepository<TenantDepartment, Guid> _tenantDepartmentRepository;

        private readonly IRepository<Cohort, Guid> _cohortRepository;
        private readonly IRepository<CohortUser, Guid> _cohortUserRepository;
        private readonly ITenantDepartmentsExcelExporter _tenantDepartmentsExcelExporter;
        private readonly IRepository<RecordState, Guid> _recordStateRepository;
        private readonly IRepository<Record, Guid> _recordLookUpRepository;
        private readonly IRepository<RecordCategory, Guid> _recordCategoryLookUpRepository;
        private readonly IRepository<User, long> _userLookUpRepository;
        private readonly IRepository<RecordStatus, Guid> _recordStatusLookUpRepository;

        private readonly ISurpathComplianceAppService _surpathComplianceAppService;

        private readonly IOUSecurityManager _ouSecurityManager;

        public TenantDepartmentsAppService(
            IOUSecurityManager ouSecurityManager,
            IRepository<TenantDepartment, Guid> tenantDepartmentRepository,
            ITenantDepartmentsExcelExporter tenantDepartmentsExcelExporter,
            IRepository<Cohort, Guid> cohortRepository,
            IRepository<RecordState, Guid> recordStateRepository,
            IRepository<Record, Guid> lookup_recordRepository,
            IRepository<RecordCategory, Guid> lookup_recordCategoryRepository,
            IRepository<User, long> lookup_userRepository,
            IRepository<RecordStatus, Guid> lookup_recordStatusRepository,
            IRepository<CohortUser, Guid> cohortUserRepository,
             ISurpathComplianceAppService surpathComplianceAppService)
        {
            _tenantDepartmentRepository = tenantDepartmentRepository;
            _tenantDepartmentsExcelExporter = tenantDepartmentsExcelExporter;
            _cohortRepository = cohortRepository;
            _recordStateRepository = recordStateRepository;
            _recordLookUpRepository = lookup_recordRepository;
            _recordCategoryLookUpRepository = lookup_recordCategoryRepository;
            _userLookUpRepository = lookup_userRepository;
            _recordStatusLookUpRepository = lookup_recordStatusRepository;
            _cohortUserRepository = cohortUserRepository;

            _surpathComplianceAppService = surpathComplianceAppService;
            _ouSecurityManager = ouSecurityManager;
        }

        public async Task<PagedResultDto<GetTenantDepartmentForViewDto>> GetAll(GetAllTenantDepartmentsInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var mroTypeFilter = input.MROTypeFilter.HasValue
                        ? (EnumClientMROTypes)input.MROTypeFilter
                        : default;

            var filteredTenantDepartments = _tenantDepartmentRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(input.ActiveFilter.HasValue && input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(input.MROTypeFilter.HasValue && input.MROTypeFilter > -1, e => e.MROType == mroTypeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);

            filteredTenantDepartments = _ouSecurityManager.ApplyTenantDepartmentVisibilityFilter(filteredTenantDepartments, AbpSession.UserId.Value);
            
            var pagedAndFilteredTenantDepartments = filteredTenantDepartments
                .OrderBy(input.Sorting ?? "name asc")
                .PageBy(input);

            var tenantDepartments = from o in pagedAndFilteredTenantDepartments
                                    select new
                                    {

                                        o.Name,
                                        o.Active,
                                        o.MROType,
                                        o.Description,
                                        Id = o.Id
                                    };

            var totalCount = await filteredTenantDepartments.CountAsync();

            var dbList = await tenantDepartments.ToListAsync();
            var results = new List<GetTenantDepartmentForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetTenantDepartmentForViewDto()
                {
                    TenantDepartment = new TenantDepartmentDto
                    {

                        Name = o.Name,
                        Active = o.Active,
                        MROType = o.MROType,
                        Description = o.Description,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetTenantDepartmentForViewDto>(
                totalCount,
                results
            );
        }



        public async Task<GetTenantDepartmentForViewDto> GetTenantDepartmentForView(Guid id)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var tenantDepartment = await _tenantDepartmentRepository.GetAsync(id);

            var output = new GetTenantDepartmentForViewDto { TenantDepartment = ObjectMapper.Map<TenantDepartmentDto>(tenantDepartment) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_TenantDepartments_Edit)]
        public async Task<GetTenantDepartmentForEditOutput> GetTenantDepartmentForEdit(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var tenantDepartment = await _tenantDepartmentRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTenantDepartmentForEditOutput { TenantDepartment = ObjectMapper.Map<CreateOrEditTenantDepartmentDto>(tenantDepartment) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTenantDepartmentDto input)
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

        [AbpAuthorize(AppPermissions.Pages_TenantDepartments_Create)]
        protected virtual async Task Create(CreateOrEditTenantDepartmentDto input)
        {
            var tenantDepartment = ObjectMapper.Map<TenantDepartment>(input);

            if (AbpSession.TenantId != null)
            {
                tenantDepartment.TenantId = (int?)AbpSession.TenantId;
            }

            await _tenantDepartmentRepository.InsertAsync(tenantDepartment);

        }

        [AbpAuthorize(AppPermissions.Pages_TenantDepartments_Edit)]
        protected virtual async Task Update(CreateOrEditTenantDepartmentDto input)
        {
            var tenantDepartment = await _tenantDepartmentRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, tenantDepartment);

        }

        [AbpAuthorize(AppPermissions.Pages_TenantDepartments_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            await _tenantDepartmentRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetTenantDepartmentsToExcel(GetAllTenantDepartmentsForExcelInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var mroTypeFilter = input.MROTypeFilter.HasValue
                        ? (EnumClientMROTypes)input.MROTypeFilter
                        : default;

            var filteredTenantDepartments = _tenantDepartmentRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(input.ActiveFilter.HasValue && input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(input.MROTypeFilter.HasValue && input.MROTypeFilter > -1, e => e.MROType == mroTypeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);

            var query = (from o in filteredTenantDepartments
                         select new GetTenantDepartmentForViewDto()
                         {
                             TenantDepartment = new TenantDepartmentDto
                             {
                                 Name = o.Name,
                                 Active = o.Active,
                                 MROType = o.MROType,
                                 Description = o.Description,
                                 Id = o.Id
                             }
                         });

            var tenantDepartmentListDtos = await query.ToListAsync();

            return _tenantDepartmentsExcelExporter.ExportToFile(tenantDepartmentListDtos);
        }


        public async Task<PagedResultDto<GetTenantDepartmentForComplianceViewDto>> GetCompliance(GetAllTenantDepartmentsInput input)
        {

            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);



            var mroTypeFilter = input.MROTypeFilter.HasValue
                        ? (EnumClientMROTypes)input.MROTypeFilter
                        : default;

            var filteredTenantDepartments = _tenantDepartmentRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(input.ActiveFilter.HasValue && input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(input.MROTypeFilter.HasValue && input.MROTypeFilter > -1, e => e.MROType == mroTypeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);


            var pagedAndFilteredTenantDepartments = filteredTenantDepartments
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var tenantDepartments = from o in pagedAndFilteredTenantDepartments
                                    select new
                                    {

                                        o.Name,
                                        o.Active,
                                        o.MROType,
                                        o.Description,
                                        Id = o.Id
                                    };
            List<ComplianceCohortTotalsForViewDto> compliancetotals = await _surpathComplianceAppService.GetTenantDeptCompliance(pagedAndFilteredTenantDepartments);

            var totalCount = await filteredTenantDepartments.CountAsync();

            var dbList = await tenantDepartments.ToListAsync();
            var results = new List<GetTenantDepartmentForComplianceViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetTenantDepartmentForComplianceViewDto()
                {
                    TenantDepartment = new TenantDepartmentDto
                    {

                        Name = o.Name,
                        Active = o.Active,
                        MROType = o.MROType,
                        Description = o.Description,
                        Id = o.Id,
                    }
                };

                res.ComplianceSummary = compliancetotals.Where(c => c.TenantDepartmentId == o.Id).ToList();

                results.Add(res);
            }

            return new PagedResultDto<GetTenantDepartmentForComplianceViewDto>(
                totalCount,
                results
            );
        }
    }
}