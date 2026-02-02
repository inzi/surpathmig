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

namespace inzibackend.Surpath
{
    [AbpAuthorize(AppPermissions.Pages_DepartmentUsers)]
    public class DepartmentUsersAppService : inzibackendAppServiceBase, IDepartmentUsersAppService
    {
        private readonly IRepository<DepartmentUser, Guid> _departmentUserRepository;
        private readonly IDepartmentUsersExcelExporter _departmentUsersExcelExporter;
        private readonly IRepository<User, long> _userLookUpRepository;
        private readonly IRepository<TenantDepartment, Guid> _tenantDepartmentLookUpRepository;

        public DepartmentUsersAppService(IRepository<DepartmentUser, Guid> departmentUserRepository, IDepartmentUsersExcelExporter departmentUsersExcelExporter, IRepository<User, long> lookup_userRepository, IRepository<TenantDepartment, Guid> lookup_tenantDepartmentRepository)
        {
            _departmentUserRepository = departmentUserRepository;
            _departmentUsersExcelExporter = departmentUsersExcelExporter;
            _userLookUpRepository = lookup_userRepository;
            _tenantDepartmentLookUpRepository = lookup_tenantDepartmentRepository;

        }

        public async Task<PagedResultDto<GetDepartmentUserForViewDto>> GetAll(GetAllDepartmentUsersInput input)
        {

            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var filteredDepartmentUsers = _departmentUserRepository.GetAll()
                        .Include(e => e.UserFk)
                        .Include(e => e.TenantDepartmentFk)
                        .Where(e => e.UserFk.IsDeleted == false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TenantDepartmentNameFilter), e => e.TenantDepartmentFk != null && e.TenantDepartmentFk.Name == input.TenantDepartmentNameFilter);

            var pagedAndFilteredDepartmentUsers = filteredDepartmentUsers
                .OrderBy(input.Sorting ?? "userFk.surname asc")
                .PageBy(input);

            var departmentUsers = from o in pagedAndFilteredDepartmentUsers
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

            var totalCount = await filteredDepartmentUsers.CountAsync();

            var dbList = await departmentUsers.ToListAsync();
            var results = new List<GetDepartmentUserForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetDepartmentUserForViewDto()
                {
                    DepartmentUser = new DepartmentUserDto
                    {

                        Id = o.Id,
                    },
                    UserName = o.UserName,
                    TenantDepartmentName = o.TenantDepartmentName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetDepartmentUserForViewDto>(
                totalCount,
                results
            );
        }

        public async Task<GetDepartmentUserForViewDto> GetDepartmentUserForView(Guid id)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var departmentUser = await _departmentUserRepository.GetAsync(id);

            var output = new GetDepartmentUserForViewDto { DepartmentUser = ObjectMapper.Map<DepartmentUserDto>(departmentUser) };

            if (output.DepartmentUser.UserId != null)
            {
                var _lookupUser = await _userLookUpRepository.FirstOrDefaultAsync((long)output.DepartmentUser.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.DepartmentUser.TenantDepartmentId != null)
            {
                var _lookupTenantDepartment = await _tenantDepartmentLookUpRepository.FirstOrDefaultAsync((Guid)output.DepartmentUser.TenantDepartmentId);
                output.TenantDepartmentName = _lookupTenantDepartment?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_DepartmentUsers_Edit)]
        public async Task<GetDepartmentUserForEditOutput> GetDepartmentUserForEdit(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var departmentUser = await _departmentUserRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDepartmentUserForEditOutput { DepartmentUser = ObjectMapper.Map<CreateOrEditDepartmentUserDto>(departmentUser) };

            if (output.DepartmentUser.UserId != null)
            {
                var _lookupUser = await _userLookUpRepository.FirstOrDefaultAsync((long)output.DepartmentUser.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.DepartmentUser.TenantDepartmentId != null)
            {
                var _lookupTenantDepartment = await _tenantDepartmentLookUpRepository.FirstOrDefaultAsync((Guid)output.DepartmentUser.TenantDepartmentId);
                output.TenantDepartmentName = _lookupTenantDepartment?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDepartmentUserDto input)
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

        [AbpAuthorize(AppPermissions.Pages_DepartmentUsers_Create)]
        protected virtual async Task Create(CreateOrEditDepartmentUserDto input)
        {
            var departmentUser = ObjectMapper.Map<DepartmentUser>(input);

            if (AbpSession.TenantId != null)
            {
                departmentUser.TenantId = (int?)AbpSession.TenantId;
            }

            await _departmentUserRepository.InsertAsync(departmentUser);

        }

        [AbpAuthorize(AppPermissions.Pages_DepartmentUsers_Edit)]
        protected virtual async Task Update(CreateOrEditDepartmentUserDto input)
        {
            var departmentUser = await _departmentUserRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, departmentUser);

        }

        [AbpAuthorize(AppPermissions.Pages_DepartmentUsers_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            await _departmentUserRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetDepartmentUsersToExcel(GetAllDepartmentUsersForExcelInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var filteredDepartmentUsers = _departmentUserRepository.GetAll()
                        .Include(e => e.UserFk)
                        .Include(e => e.TenantDepartmentFk)
                        .Where(e => e.UserFk.IsDeleted == false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TenantDepartmentNameFilter), e => e.TenantDepartmentFk != null && e.TenantDepartmentFk.Name == input.TenantDepartmentNameFilter);

            var query = (from o in filteredDepartmentUsers
                         join o1 in _userLookUpRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _tenantDepartmentLookUpRepository.GetAll() on o.TenantDepartmentId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetDepartmentUserForViewDto()
                         {
                             DepartmentUser = new DepartmentUserDto
                             {
                                 Id = o.Id
                             },
                             UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             TenantDepartmentName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var departmentUserListDtos = await query.ToListAsync();

            return _departmentUsersExcelExporter.ExportToFile(departmentUserListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_DepartmentUsers)]
        public async Task<PagedResultDto<DepartmentUserUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
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

            var lookupTableDtoList = new List<DepartmentUserUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new DepartmentUserUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.Name?.ToString()
                });
            }

            return new PagedResultDto<DepartmentUserUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}