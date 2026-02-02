using inzibackend.Authorization.Users;
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
using inzibackend.Authorization.Organizations;

namespace inzibackend.Surpath
{
    [AbpAuthorize(AppPermissions.Pages_TenantDepartmentUsers)]
    public class TenantDepartmentUsersAppService : inzibackendAppServiceBase, ITenantDepartmentUsersAppService
    {
        private readonly IRepository<TenantDepartmentUser, Guid> _tenantDepartmentUserRepository;
        private readonly ITenantDepartmentUsersExcelExporter _tenantDepartmentUsersExcelExporter;
        private readonly IRepository<User, long> _userLookUpRepository;
        private readonly IRepository<TenantDepartment, Guid> _tenantDepartmentLookUpRepository;
        private readonly IOUSecurityManager _ouSecurityManager;


        public TenantDepartmentUsersAppService(IOUSecurityManager ouSecurityManager, IRepository<TenantDepartmentUser, Guid> tenantDepartmentUserRepository, ITenantDepartmentUsersExcelExporter tenantDepartmentUsersExcelExporter, IRepository<User, long> lookup_userRepository, IRepository<TenantDepartment, Guid> lookup_tenantDepartmentRepository)
        {
            _tenantDepartmentUserRepository = tenantDepartmentUserRepository;
            _tenantDepartmentUsersExcelExporter = tenantDepartmentUsersExcelExporter;
            _userLookUpRepository = lookup_userRepository;
            _tenantDepartmentLookUpRepository = lookup_tenantDepartmentRepository;
            _ouSecurityManager = ouSecurityManager;

        }

        public async Task<PagedResultDto<GetTenantDepartmentUserForViewDto>> GetAll(GetAllTenantDepartmentUsersInput input)
        {

            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var filteredTenantDepartmentUsers = _tenantDepartmentUserRepository.GetAll()
                        .Include(e => e.UserFk)
                        .Include(e => e.TenantDepartmentFk)
                        .Where(e => e.UserFk.IsDeleted == false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TenantDepartmentNameFilter), e => e.TenantDepartmentFk != null && e.TenantDepartmentFk.Name == input.TenantDepartmentNameFilter)
                        .WhereIf(input.TenantDepartmentIdFilter.HasValue, e => false || e.TenantDepartmentId == input.TenantDepartmentIdFilter.Value);

            filteredTenantDepartmentUsers = _ouSecurityManager.ApplyTenantDepartmentUserVisibilityFilter(filteredTenantDepartmentUsers, AbpSession.UserId.Value);

            var pagedAndFilteredTenantDepartmentUsers = filteredTenantDepartmentUsers
                .OrderBy(input.Sorting ?? "userFk.surname asc")
                .PageBy(input);

            var tenantDepartmentUsers = from o in pagedAndFilteredTenantDepartmentUsers
                                        join o1 in _userLookUpRepository.GetAll() on o.UserId equals o1.Id into j1
                                        from s1 in j1.DefaultIfEmpty()

                                        join o2 in _tenantDepartmentLookUpRepository.GetAll() on o.TenantDepartmentId equals o2.Id into j2
                                        from s2 in j2.DefaultIfEmpty()

                                        select new
                                        {

                                            Id = o.Id,
                                            UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                            TenantDepartmentName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                        };

            var totalCount = await filteredTenantDepartmentUsers.CountAsync();

            var dbList = await tenantDepartmentUsers.ToListAsync();
            var results = new List<GetTenantDepartmentUserForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetTenantDepartmentUserForViewDto()
                {
                    TenantDepartmentUser = new TenantDepartmentUserDto
                    {

                        Id = o.Id,
                    },
                    UserName = o.UserName,
                    TenantDepartmentName = o.TenantDepartmentName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetTenantDepartmentUserForViewDto>(
                totalCount,
                results
            );
        }

        public async Task<GetTenantDepartmentUserForViewDto> GetTenantDepartmentUserForView(Guid id)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var tenantDepartmentUser = await _tenantDepartmentUserRepository.GetAsync(id);

            var output = new GetTenantDepartmentUserForViewDto { TenantDepartmentUser = ObjectMapper.Map<TenantDepartmentUserDto>(tenantDepartmentUser) };

            if (output.TenantDepartmentUser.UserId != null)
            {
                var _lookupUser = await _userLookUpRepository.FirstOrDefaultAsync((long)output.TenantDepartmentUser.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.TenantDepartmentUser.TenantDepartmentId != null)
            {
                var _lookupTenantDepartment = await _tenantDepartmentLookUpRepository.FirstOrDefaultAsync((Guid)output.TenantDepartmentUser.TenantDepartmentId);
                output.TenantDepartmentName = _lookupTenantDepartment?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_TenantDepartmentUsers_Edit)]
        public async Task<GetTenantDepartmentUserForEditOutput> GetTenantDepartmentUserForEdit(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var tenantDepartmentUser = await _tenantDepartmentUserRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTenantDepartmentUserForEditOutput { TenantDepartmentUser = ObjectMapper.Map<CreateOrEditTenantDepartmentUserDto>(tenantDepartmentUser) };

            if (output.TenantDepartmentUser.UserId != null)
            {
                var _lookupUser = await _userLookUpRepository.FirstOrDefaultAsync((long)output.TenantDepartmentUser.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.TenantDepartmentUser.TenantDepartmentId != null)
            {
                var _lookupTenantDepartment = await _tenantDepartmentLookUpRepository.FirstOrDefaultAsync((Guid)output.TenantDepartmentUser.TenantDepartmentId);
                output.TenantDepartmentName = _lookupTenantDepartment?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTenantDepartmentUserDto input)
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

        [AbpAuthorize(AppPermissions.Pages_TenantDepartmentUsers_Create)]
        protected virtual async Task Create(CreateOrEditTenantDepartmentUserDto input)
        {
            var tenantDepartmentUser = ObjectMapper.Map<TenantDepartmentUser>(input);

            if (AbpSession.TenantId != null)
            {
                tenantDepartmentUser.TenantId = (int?)AbpSession.TenantId;
            }

            await _tenantDepartmentUserRepository.InsertAsync(tenantDepartmentUser);

        }

        [AbpAuthorize(AppPermissions.Pages_TenantDepartmentUsers_Edit)]
        protected virtual async Task Update(CreateOrEditTenantDepartmentUserDto input)
        {
            var tenantDepartmentUser = await _tenantDepartmentUserRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, tenantDepartmentUser);

        }

        [AbpAuthorize(AppPermissions.Pages_TenantDepartmentUsers_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            await _tenantDepartmentUserRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetTenantDepartmentUsersToExcel(GetAllTenantDepartmentUsersForExcelInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var filteredTenantDepartmentUsers = _tenantDepartmentUserRepository.GetAll()
                        .Include(e => e.UserFk)
                        .Include(e => e.TenantDepartmentFk)
                        .Where(e => e.UserFk.IsDeleted == false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TenantDepartmentNameFilter), e => e.TenantDepartmentFk != null && e.TenantDepartmentFk.Name == input.TenantDepartmentNameFilter);

            var query = (from o in filteredTenantDepartmentUsers
                         join o1 in _userLookUpRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _tenantDepartmentLookUpRepository.GetAll() on o.TenantDepartmentId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetTenantDepartmentUserForViewDto()
                         {
                             TenantDepartmentUser = new TenantDepartmentUserDto
                             {
                                 Id = o.Id
                             },
                             UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             TenantDepartmentName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var tenantDepartmentUserListDtos = await query.ToListAsync();

            return _tenantDepartmentUsersExcelExporter.ExportToFile(tenantDepartmentUserListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_TenantDepartmentUsers)]
        public async Task<PagedResultDto<TenantDepartmentUserUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var query = _userLookUpRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => (e.Name != null && e.Name.Contains(input.Filter))
            || (e.MiddleName != null && e.MiddleName.Contains(input.Filter))
            || (e.Surname != null && e.Surname.Contains(input.Filter))
            || (e.EmailAddress != null && e.EmailAddress.Contains(input.Filter))
            || (e.PhoneNumber != null && e.PhoneNumber.Contains(input.Filter))
               );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<TenantDepartmentUserUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new TenantDepartmentUserUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.Name?.ToString()
                });
            }

            return new PagedResultDto<TenantDepartmentUserUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}