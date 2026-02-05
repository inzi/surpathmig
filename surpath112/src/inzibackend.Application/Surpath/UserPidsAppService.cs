using inzibackend.Surpath;
using inzibackend.Authorization.Users;

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
    [AbpAuthorize(AppPermissions.Pages_UserPids)]
    public class UserPidsAppService : inzibackendAppServiceBase, IUserPidsAppService
    {
        private readonly IRepository<UserPid, Guid> _userPidRepository;
        private readonly IUserPidsExcelExporter _userPidsExcelExporter;
        private readonly IRepository<PidType, Guid> _pidTypeLookUpRepository;
        private readonly IRepository<User, long> _userLookUpRepository;

        public UserPidsAppService(IRepository<UserPid, Guid> userPidRepository, IUserPidsExcelExporter userPidsExcelExporter, IRepository<PidType, Guid> lookup_pidTypeRepository, IRepository<User, long> lookup_userRepository)
        {
            _userPidRepository = userPidRepository;
            _userPidsExcelExporter = userPidsExcelExporter;
            _pidTypeLookUpRepository = lookup_pidTypeRepository;
            _userLookUpRepository = lookup_userRepository;

        }

        public async Task<PagedResultDto<GetUserPidForViewDto>> GetAll(GetAllUserPidsInput input)
        {
                if (AbpSession.TenantId == null)
                {
                    CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                }

                var filteredUserPids = _userPidRepository.GetAll()
                            .Include(e => e.PidTypeFk)
                            .Include(e => e.UserFk)
                            .Where(e => e.UserFk.IsDeleted == false && e.UserId != null)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Pid.Contains(input.Filter))
                            .WhereIf(!string.IsNullOrWhiteSpace(input.PidFilter), e => e.Pid == input.PidFilter)
                            .WhereIf(input.ValidatedFilter.HasValue && input.ValidatedFilter > -1, e => (input.ValidatedFilter == 1 && e.Validated) || (input.ValidatedFilter == 0 && !e.Validated))
                            .WhereIf(!string.IsNullOrWhiteSpace(input.PidTypeNameFilter), e => e.PidTypeFk != null && e.PidTypeFk.Name == input.PidTypeNameFilter)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

                var pagedAndFilteredUserPids = filteredUserPids
                    .OrderBy(input.Sorting ?? "userfk.name asc")
                    .PageBy(input);

                var userPids = from o in pagedAndFilteredUserPids
                               join o1 in _pidTypeLookUpRepository.GetAll() on o.PidTypeId equals o1.Id into j1
                               from s1 in j1.DefaultIfEmpty()

                               join o2 in _userLookUpRepository.GetAll() on o.UserId equals o2.Id into j2
                               from s2 in j2.DefaultIfEmpty()

                               select new
                               {

                                   o.Pid,
                                   o.Validated,
                                   Id = o.Id,
                                   PidTypeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                   UserName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                               };

                var totalCount = await filteredUserPids.CountAsync();

                var dbList = await userPids.ToListAsync();
                var results = new List<GetUserPidForViewDto>();

                foreach (var o in dbList)
                {
                    var res = new GetUserPidForViewDto()
                    {
                        UserPid = new UserPidDto
                        {

                            Pid = o.Pid,
                            Validated = o.Validated,
                            Id = o.Id,
                        },
                        PidTypeName = o.PidTypeName,
                        UserName = o.UserName
                    };

                    results.Add(res);
                }

                return new PagedResultDto<GetUserPidForViewDto>(
                    totalCount,
                    results
                );
        }

        public async Task<GetUserPidForViewDto> GetUserPidForView(Guid id)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var userPid = await _userPidRepository.GetAsync(id);

                var output = new GetUserPidForViewDto { UserPid = ObjectMapper.Map<UserPidDto>(userPid) };

                if (output.UserPid.PidTypeId != null)
                {
                    var _lookupPidType = await _pidTypeLookUpRepository.FirstOrDefaultAsync((Guid)output.UserPid.PidTypeId);
                    output.PidTypeName = _lookupPidType?.Name?.ToString();
                }

                if (output.UserPid.UserId != null)
                {
                    var _lookupUser = await _userLookUpRepository.FirstOrDefaultAsync((long)output.UserPid.UserId);
                    output.UserName = _lookupUser?.Name?.ToString();
                }

                return output;
        }

        [AbpAuthorize(AppPermissions.Pages_UserPids_Edit)]
        public async Task<GetUserPidForEditOutput> GetUserPidForEdit(EntityDto<Guid> input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var userPid = await _userPidRepository.FirstOrDefaultAsync(input.Id);

                var output = new GetUserPidForEditOutput { UserPid = ObjectMapper.Map<CreateOrEditUserPidDto>(userPid) };

                if (output.UserPid.PidTypeId != null)
                {
                    var _lookupPidType = await _pidTypeLookUpRepository.FirstOrDefaultAsync((Guid)output.UserPid.PidTypeId);
                    output.PidTypeName = _lookupPidType?.Name?.ToString();
                }

                if (output.UserPid.UserId != null)
                {
                    var _lookupUser = await _userLookUpRepository.FirstOrDefaultAsync((long)output.UserPid.UserId);
                    output.UserName = _lookupUser?.Name?.ToString();
                }

                return output;
        }

        public async Task CreateOrEdit(CreateOrEditUserPidDto input)
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

        [AbpAuthorize(AppPermissions.Pages_UserPids_Create)]
        protected virtual async Task Create(CreateOrEditUserPidDto input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var userPid = ObjectMapper.Map<UserPid>(input);

                if (AbpSession.TenantId != null)
                {
                    userPid.TenantId = (int?)AbpSession.TenantId;
                }

                await _userPidRepository.InsertAsync(userPid);

        }

        [AbpAuthorize(AppPermissions.Pages_UserPids_Edit)]
        protected virtual async Task Update(CreateOrEditUserPidDto input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                var userPid = await _userPidRepository.FirstOrDefaultAsync((Guid)input.Id);
                ObjectMapper.Map(input, userPid);
        }

        [AbpAuthorize(AppPermissions.Pages_UserPids_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                await _userPidRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetUserPidsToExcel(GetAllUserPidsForExcelInput input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var filteredUserPids = _userPidRepository.GetAll()
                            .Include(e => e.PidTypeFk)
                            .Include(e => e.UserFk)
                            .Where(e => e.UserFk.IsDeleted == false)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Pid.Contains(input.Filter))
                            .WhereIf(!string.IsNullOrWhiteSpace(input.PidFilter), e => e.Pid == input.PidFilter)
                            .WhereIf(input.ValidatedFilter.HasValue && input.ValidatedFilter > -1, e => (input.ValidatedFilter == 1 && e.Validated) || (input.ValidatedFilter == 0 && !e.Validated))
                            .WhereIf(!string.IsNullOrWhiteSpace(input.PidTypeNameFilter), e => e.PidTypeFk != null && e.PidTypeFk.Name == input.PidTypeNameFilter)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

                var query = (from o in filteredUserPids
                             join o1 in _pidTypeLookUpRepository.GetAll() on o.PidTypeId equals o1.Id into j1
                             from s1 in j1.DefaultIfEmpty()

                             join o2 in _userLookUpRepository.GetAll() on o.UserId equals o2.Id into j2
                             from s2 in j2.DefaultIfEmpty()

                             select new GetUserPidForViewDto()
                             {
                                 UserPid = new UserPidDto
                                 {
                                     Pid = o.Pid,
                                     Validated = o.Validated,
                                     Id = o.Id
                                 },
                                 PidTypeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                 UserName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                             });

                var userPidListDtos = await query.ToListAsync();

                return _userPidsExcelExporter.ExportToFile(userPidListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_UserPids)]
        public async Task<List<UserPidPidTypeLookupTableDto>> GetAllPidTypeForTableDropdown()
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                return await _pidTypeLookUpRepository.GetAll()
                    .Select(pidType => new UserPidPidTypeLookupTableDto
                    {
                        Id = pidType.Id.ToString(),
                        DisplayName = pidType == null || pidType.Name == null ? "" : pidType.Name.ToString()
                    }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_UserPids)]
        public async Task<PagedResultDto<UserPidUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
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

                var lookupTableDtoList = new List<UserPidUserLookupTableDto>();
                foreach (var user in userList)
                {
                    lookupTableDtoList.Add(new UserPidUserLookupTableDto
                    {
                        Id = user.Id,
                        DisplayName = user.Name?.ToString()
                    });
                }

                return new PagedResultDto<UserPidUserLookupTableDto>(
                    totalCount,
                    lookupTableDtoList
                );
        }

        [AbpAllowAnonymous]

        public async Task<List<UserPidDto>> GetEmptyPidsList()
        {
            var _tenantId = AbpSession.TenantId;
            var _pidTypes = await _pidTypeLookUpRepository.GetAll().AsNoTracking().ToListAsync();
            var userPidList = new List<UserPidDto>();
            foreach (var pidType in _pidTypes)
            {
                userPidList.Add(new UserPidDto()
                {
                    Pid = String.Empty,
                    PidTypeId = pidType.Id,
                    Validated = false,
                    PidType = ObjectMapper.Map<PidTypeDto>(pidType)
                    //Pi
                    //MaskPid = pidType.MaskPid,
                    //PidTypeDescription = pidType.Description,
                    //PidTypeName = pidType.Name


                }); ;
            }
            return userPidList;
        }

    }
}

