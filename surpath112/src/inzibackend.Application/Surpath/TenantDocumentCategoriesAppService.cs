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
    [AbpAuthorize(AppPermissions.Pages_TenantDocumentCategories)]
    public class TenantDocumentCategoriesAppService : inzibackendAppServiceBase, ITenantDocumentCategoriesAppService
    {
        private readonly IRepository<TenantDocumentCategory, Guid> _tenantDocumentCategoryRepository;
        private readonly ITenantDocumentCategoriesExcelExporter _tenantDocumentCategoriesExcelExporter;
        private readonly IRepository<User, long> _userLookUpRepository;

        public TenantDocumentCategoriesAppService(IRepository<TenantDocumentCategory, Guid> tenantDocumentCategoryRepository, ITenantDocumentCategoriesExcelExporter tenantDocumentCategoriesExcelExporter, IRepository<User, long> lookup_userRepository)
        {
            _tenantDocumentCategoryRepository = tenantDocumentCategoryRepository;
            _tenantDocumentCategoriesExcelExporter = tenantDocumentCategoriesExcelExporter;
            _userLookUpRepository = lookup_userRepository;

        }

        public async Task<PagedResultDto<GetTenantDocumentCategoryForViewDto>> GetAll(GetAllTenantDocumentCategoriesInput input)
        {
                if (AbpSession.TenantId == null)
                {
                    CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                }

                var filteredTenantDocumentCategories = _tenantDocumentCategoryRepository.GetAll()
                            .Include(e => e.UserFk)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                            .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                            .WhereIf(input.AuthorizedOnlyFilter.HasValue && input.AuthorizedOnlyFilter > -1, e => (input.AuthorizedOnlyFilter == 1 && e.AuthorizedOnly) || (input.AuthorizedOnlyFilter == 0 && !e.AuthorizedOnly))
                            .WhereIf(input.HostOnlyFilter.HasValue && input.HostOnlyFilter > -1, e => (input.HostOnlyFilter == 1 && e.HostOnly) || (input.HostOnlyFilter == 0 && !e.HostOnly))
                            .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

                var pagedAndFilteredTenantDocumentCategories = filteredTenantDocumentCategories
                    .OrderBy(input.Sorting ?? "name asc")
                    .PageBy(input);

                var tenantDocumentCategories = from o in pagedAndFilteredTenantDocumentCategories
                                               join o1 in _userLookUpRepository.GetAll() on o.UserId equals o1.Id into j1
                                               from s1 in j1.DefaultIfEmpty()

                                               select new
                                               {

                                                   o.Name,
                                                   o.Description,
                                                   o.AuthorizedOnly,
                                                   o.HostOnly,
                                                   Id = o.Id,
                                                   UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                                               };

                var totalCount = await filteredTenantDocumentCategories.CountAsync();

                var dbList = await tenantDocumentCategories.ToListAsync();
                var results = new List<GetTenantDocumentCategoryForViewDto>();

                foreach (var o in dbList)
                {
                    var res = new GetTenantDocumentCategoryForViewDto()
                    {
                        TenantDocumentCategory = new TenantDocumentCategoryDto
                        {

                            Name = o.Name,
                            Description = o.Description,
                            AuthorizedOnly = o.AuthorizedOnly,
                            HostOnly = o.HostOnly,
                            Id = o.Id,
                        },
                        UserName = o.UserName
                    };

                    results.Add(res);
                }

                return new PagedResultDto<GetTenantDocumentCategoryForViewDto>(
                    totalCount,
                    results
                );
        }

        public async Task<GetTenantDocumentCategoryForViewDto> GetTenantDocumentCategoryForView(Guid id)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var tenantDocumentCategory = await _tenantDocumentCategoryRepository.GetAsync(id);

                var output = new GetTenantDocumentCategoryForViewDto { TenantDocumentCategory = ObjectMapper.Map<TenantDocumentCategoryDto>(tenantDocumentCategory) };

                if (output.TenantDocumentCategory.UserId != null)
                {
                    var _lookupUser = await _userLookUpRepository.FirstOrDefaultAsync((long)output.TenantDocumentCategory.UserId);
                    output.UserName = _lookupUser?.Name?.ToString();
                }

                return output;
        }

        [AbpAuthorize(AppPermissions.Pages_TenantDocumentCategories_Edit)]
        public async Task<GetTenantDocumentCategoryForEditOutput> GetTenantDocumentCategoryForEdit(EntityDto<Guid> input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var tenantDocumentCategory = await _tenantDocumentCategoryRepository.FirstOrDefaultAsync(input.Id);

                var output = new GetTenantDocumentCategoryForEditOutput { TenantDocumentCategory = ObjectMapper.Map<CreateOrEditTenantDocumentCategoryDto>(tenantDocumentCategory) };

                if (output.TenantDocumentCategory.UserId != null)
                {
                    var _lookupUser = await _userLookUpRepository.FirstOrDefaultAsync((long)output.TenantDocumentCategory.UserId);
                    output.UserName = _lookupUser?.Name?.ToString();
                }

                return output;
        }

        public async Task CreateOrEdit(CreateOrEditTenantDocumentCategoryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_TenantDocumentCategories_Create)]
        protected virtual async Task Create(CreateOrEditTenantDocumentCategoryDto input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var tenantDocumentCategory = ObjectMapper.Map<TenantDocumentCategory>(input);

                if (AbpSession.TenantId != null)
                {
                    tenantDocumentCategory.TenantId = (int?)AbpSession.TenantId;
                }

                await _tenantDocumentCategoryRepository.InsertAsync(tenantDocumentCategory);

        }

        [AbpAuthorize(AppPermissions.Pages_TenantDocumentCategories_Edit)]
        protected virtual async Task Update(CreateOrEditTenantDocumentCategoryDto input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                var tenantDocumentCategory = await _tenantDocumentCategoryRepository.FirstOrDefaultAsync((Guid)input.Id);
                ObjectMapper.Map(input, tenantDocumentCategory);

        }

        [AbpAuthorize(AppPermissions.Pages_TenantDocumentCategories_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                await _tenantDocumentCategoryRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetTenantDocumentCategoriesToExcel(GetAllTenantDocumentCategoriesForExcelInput input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var filteredTenantDocumentCategories = _tenantDocumentCategoryRepository.GetAll()
                            .Include(e => e.UserFk)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                            .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                            .WhereIf(input.AuthorizedOnlyFilter.HasValue && input.AuthorizedOnlyFilter > -1, e => (input.AuthorizedOnlyFilter == 1 && e.AuthorizedOnly) || (input.AuthorizedOnlyFilter == 0 && !e.AuthorizedOnly))
                            .WhereIf(input.HostOnlyFilter.HasValue && input.HostOnlyFilter > -1, e => (input.HostOnlyFilter == 1 && e.HostOnly) || (input.HostOnlyFilter == 0 && !e.HostOnly))
                            .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

                var query = (from o in filteredTenantDocumentCategories
                             join o1 in _userLookUpRepository.GetAll() on o.UserId equals o1.Id into j1
                             from s1 in j1.DefaultIfEmpty()

                             select new GetTenantDocumentCategoryForViewDto()
                             {
                                 TenantDocumentCategory = new TenantDocumentCategoryDto
                                 {
                                     Name = o.Name,
                                     Description = o.Description,
                                     AuthorizedOnly = o.AuthorizedOnly,
                                     HostOnly = o.HostOnly,
                                     Id = o.Id
                                 },
                                 UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                             });

                var tenantDocumentCategoryListDtos = await query.ToListAsync();

                return _tenantDocumentCategoriesExcelExporter.ExportToFile(tenantDocumentCategoryListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_TenantDocumentCategories)]
        public async Task<PagedResultDto<TenantDocumentCategoryUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
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

                var lookupTableDtoList = new List<TenantDocumentCategoryUserLookupTableDto>();
                foreach (var user in userList)
                {
                    lookupTableDtoList.Add(new TenantDocumentCategoryUserLookupTableDto
                    {
                        Id = user.Id,
                        DisplayName = user.Name?.ToString()
                    });
                }

                return new PagedResultDto<TenantDocumentCategoryUserLookupTableDto>(
                    totalCount,
                    lookupTableDtoList
                );
        }

    }
}