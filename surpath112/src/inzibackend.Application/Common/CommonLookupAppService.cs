using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using inzibackend.Common.Dto;
using inzibackend.Editions;
using inzibackend.Editions.Dto;
using Abp.Domain.Uow;
using inzibackend.Authorization;
using inzibackend.Surpath.Dtos;
using System.Collections.Generic;
using System;
using Abp.Domain.Repositories;
using inzibackend.Surpath;
using Abp.Domain.Uow;
using inzibackend.Authorization;
using inzibackend.Surpath.Dtos;
using System.Collections.Generic;
using System;
using Abp.Domain.Repositories;
using inzibackend.Surpath;
using inzibackend.MultiTenancy.Dto;
using Abp.Organizations;
using inzibackend.Authorization.Users;

namespace inzibackend.Common
{
    [AbpAuthorize]
    public class CommonLookupAppService : inzibackendAppServiceBase, ICommonLookupAppService
    {
        private readonly EditionManager _editionManager;
        private readonly IRepository<TenantDepartment, Guid> _tenantDepartmentLookUpRepository;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly IRepository<Cohort, Guid> _cohortRepository;
        private readonly IRepository<TenantDepartment, Guid> _tenantDepartmentRepository;
        private readonly IRepository<User, long> _userRepository;

        public CommonLookupAppService(
            EditionManager editionManager,
            IRepository<TenantDepartment, Guid> lookup_tenantDepartmentRepository,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<Cohort, Guid> cohortRepository,
            IRepository<TenantDepartment, Guid> tenantDepartmentRepository,
            IRepository<User, long> userRepository
            )
        {
            _editionManager = editionManager;
            _tenantDepartmentLookUpRepository = lookup_tenantDepartmentRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _cohortRepository = cohortRepository;
            _tenantDepartmentRepository = tenantDepartmentRepository;
            _userRepository = userRepository;
        }

        public async Task<ListResultDto<SubscribableEditionComboboxItemDto>> GetEditionsForCombobox(bool onlyFreeItems = false)
        {
            var subscribableEditions = (await _editionManager.Editions.Cast<SubscribableEdition>().ToListAsync())
                .WhereIf(onlyFreeItems, e => e.IsFree)
                .OrderBy(e => e.MonthlyPrice);

            return new ListResultDto<SubscribableEditionComboboxItemDto>(
                subscribableEditions.Select(e => new SubscribableEditionComboboxItemDto(e.Id.ToString(), e.DisplayName, e.IsFree)).ToList()
            );
        }

        public async Task<PagedResultDto<NameValueDto>> FindUsers(FindUsersInput input)
        {
            if (AbpSession.TenantId != null)
            {
                //Prevent tenants to get other tenant's users.
                input.TenantId = AbpSession.TenantId;
            }

            using (CurrentUnitOfWork.SetTenantId(input.TenantId))
            {
                var query = UserManager.Users
                    .WhereIf(
                        !input.Filter.IsNullOrWhiteSpace(),
                        u =>
                            u.Name.Contains(input.Filter) ||
                            u.Surname.Contains(input.Filter) ||
                            u.UserName.Contains(input.Filter) ||
                            u.EmailAddress.Contains(input.Filter)
                    ).WhereIf(input.ExcludeCurrentUser, u => u.Id != AbpSession.GetUserId());

                var userCount = await query.CountAsync();
                var users = await query
                    .OrderBy(u => u.Name)
                    .ThenBy(u => u.Surname)
                    .PageBy(input)
                    .ToListAsync();

                return new PagedResultDto<NameValueDto>(
                    userCount,
                    users.Select(u =>
                        new NameValueDto(
                            u.FullName + " (" + u.EmailAddress + ")",
                            u.Id.ToString()
                            )
                        ).ToList()
                    );
            }
        }

        public GetDefaultEditionNameOutput GetDefaultEditionName()
        {
            return new GetDefaultEditionNameOutput
            {
                Name = EditionManager.DefaultEditionName
            };
        }
        [AbpAuthorize(
                   AppPermissions.Pages_Cohorts,
                   AppPermissions.Pages_DepartmentUsers,
                   AppPermissions.Pages_DeptCodes,
                   AppPermissions.Pages_TenantSurpathServices,
                   AppPermissions.Pages_TenantDepartmentUsers,
                   AppPermissions.Pages_SurpathServices,
                   AppPermissions.Pages_RecordStatuses,
                   AppPermissions.Pages_Administration_RecordRequirements,
                   AppPermissions.Pages_DeptCodes,
                   AppPermissions.Pages_DepartmentUsers,
                   AppPermissions.Pages_Cohorts
                   )]
        public async Task<PagedResultDto<CohortTenantDepartmentLookupTableDto>> GetAllTenantDepartmentForLookupTable(GetAllForLookupTableInput input)
        {
            var _isHost = AbpSession.TenantId == null;

            if (_isHost) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var query = _tenantDepartmentLookUpRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.Name != null && e.Name.Contains(input.Filter))
                .WhereIf(input.TenantId>0, e=>e.TenantId==input.TenantId);

            Dictionary<int, Tuple<int, string, string>> _tenantList = new Dictionary<int, Tuple<int, string, string>>();

            if (_isHost)
            {
                _tenantList = await TenantManager.GetTenancyInfoList();
            }

            var totalCount = await query.CountAsync();

            var tenantDepartmentList = await query
                .PageBy(input)
                .ToListAsync();

            if (input.Shuffle)
            {
                Random rand = new Random();
                tenantDepartmentList = tenantDepartmentList.OrderBy(c => rand.Next()).ToList();
            }

            var lookupTableDtoList = new List<CohortTenantDepartmentLookupTableDto>();
            foreach (var tenantDepartment in tenantDepartmentList)
            {
                lookupTableDtoList.Add(new CohortTenantDepartmentLookupTableDto
                {
                    Id = tenantDepartment.Id.ToString(),
                    DisplayName = tenantDepartment.Name?.ToString(),
                    TenantInfoDto = (_isHost && tenantDepartment.TenantId != null) ? new TenantInfoDto()
                    {
                        Id = _tenantList[(int)tenantDepartment.TenantId].Item1,
                        Name = _tenantList[(int)tenantDepartment.TenantId].Item2,
                        TenancyName = _tenantList[(int)tenantDepartment.TenantId].Item3
                    } : null
                });
            }

            return new PagedResultDto<CohortTenantDepartmentLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }


        //public async Task<PagedResultDto<NameValueDto>> CohortLookup(FindCohortsInput input)
        //{
        //    var query = _cohortRepository.GetAll()
        //        .WhereIf(!input.Filter.IsNullOrWhiteSpace(),
        //            x => x.Name.Contains(input.Filter));

        //    var totalCount = await query.CountAsync();
        //    var items = await query.OrderBy(x => x.Name)
        //        .PageBy(input)
        //        .ToListAsync();

        //    return new PagedResultDto<NameValueDto>(
        //        totalCount,
        //        items.Select(c => new NameValueDto(
        //            c.Id.ToString(),
        //            c.Name
        //        )).ToList()
        //    );
        //}

        //public async Task<PagedResultDto<NameValueDto>> FindDepartments(FindDepartmentsInput input)
        //{
        //    var query = _tenantDepartmentRepository.GetAll()
        //        .WhereIf(!input.Filter.IsNullOrWhiteSpace(), 
        //            x => x.Name.Contains(input.Filter));

        //    var totalCount = await query.CountAsync();
        //    var items = await query.OrderBy(x => x.Name)
        //        .PageBy(input)
        //        .ToListAsync();

        //    return new PagedResultDto<NameValueDto>(
        //        totalCount,
        //        items.Select(d => new NameValueDto(
        //            d.Id.ToString(),
        //            d.Name
        //        )).ToList()
        //    );
        //}

    }
}
