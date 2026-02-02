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
    [AbpAuthorize(AppPermissions.Pages_TestCategories)]
    public class TestCategoriesAppService : inzibackendAppServiceBase, ITestCategoriesAppService
    {
        private readonly IRepository<TestCategory, Guid> _testCategoryRepository;
        private readonly ITestCategoriesExcelExporter _testCategoriesExcelExporter;

        public TestCategoriesAppService(IRepository<TestCategory, Guid> testCategoryRepository, ITestCategoriesExcelExporter testCategoriesExcelExporter)
        {
            _testCategoryRepository = testCategoryRepository;
            _testCategoriesExcelExporter = testCategoriesExcelExporter;

        }

        public async Task<PagedResultDto<GetTestCategoryForViewDto>> GetAll(GetAllTestCategoriesInput input)
        {

            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var filteredTestCategories = _testCategoryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.InternalName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InternalNameFilter), e => e.InternalName == input.InternalNameFilter);

            var pagedAndFilteredTestCategories = filteredTestCategories
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var testCategories = from o in pagedAndFilteredTestCategories
                                 select new
                                 {

                                     o.Name,
                                     o.InternalName,
                                     Id = o.Id
                                 };

            var totalCount = await filteredTestCategories.CountAsync();

            var dbList = await testCategories.ToListAsync();
            var results = new List<GetTestCategoryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetTestCategoryForViewDto()
                {
                    TestCategory = new TestCategoryDto
                    {

                        Name = o.Name,
                        InternalName = o.InternalName,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetTestCategoryForViewDto>(
                totalCount,
                results
            );
        }

        public async Task<GetTestCategoryForViewDto> GetTestCategoryForView(Guid id)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var testCategory = await _testCategoryRepository.GetAsync(id);

            var output = new GetTestCategoryForViewDto { TestCategory = ObjectMapper.Map<TestCategoryDto>(testCategory) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_TestCategories_Edit)]
        public async Task<GetTestCategoryForEditOutput> GetTestCategoryForEdit(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var testCategory = await _testCategoryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTestCategoryForEditOutput { TestCategory = ObjectMapper.Map<CreateOrEditTestCategoryDto>(testCategory) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTestCategoryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_TestCategories_Create)]
        protected virtual async Task Create(CreateOrEditTestCategoryDto input)
        {
            var testCategory = ObjectMapper.Map<TestCategory>(input);

            await _testCategoryRepository.InsertAsync(testCategory);

        }

        [AbpAuthorize(AppPermissions.Pages_TestCategories_Edit)]
        protected virtual async Task Update(CreateOrEditTestCategoryDto input)
        {
            var testCategory = await _testCategoryRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, testCategory);

        }

        [AbpAuthorize(AppPermissions.Pages_TestCategories_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            await _testCategoryRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetTestCategoriesToExcel(GetAllTestCategoriesForExcelInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var filteredTestCategories = _testCategoryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.InternalName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InternalNameFilter), e => e.InternalName == input.InternalNameFilter);

            var query = (from o in filteredTestCategories
                         select new GetTestCategoryForViewDto()
                         {
                             TestCategory = new TestCategoryDto
                             {
                                 Name = o.Name,
                                 InternalName = o.InternalName,
                                 Id = o.Id
                             }
                         });

            var testCategoryListDtos = await query.ToListAsync();

            return _testCategoriesExcelExporter.ExportToFile(testCategoryListDtos);
        }

    }
}