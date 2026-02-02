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
    [AbpAuthorize(AppPermissions.Pages_ConfirmationValues)]
    public class ConfirmationValuesAppService : inzibackendAppServiceBase, IConfirmationValuesAppService
    {
        private readonly IRepository<ConfirmationValue, Guid> _confirmationValueRepository;
        private readonly IConfirmationValuesExcelExporter _confirmationValuesExcelExporter;
        private readonly IRepository<Drug, Guid> _drugLookUpRepository;
        private readonly IRepository<TestCategory, Guid> _testCategoryLookUpRepository;

        public ConfirmationValuesAppService(IRepository<ConfirmationValue, Guid> confirmationValueRepository, IConfirmationValuesExcelExporter confirmationValuesExcelExporter, IRepository<Drug, Guid> lookup_drugRepository, IRepository<TestCategory, Guid> lookup_testCategoryRepository)
        {
            _confirmationValueRepository = confirmationValueRepository;
            _confirmationValuesExcelExporter = confirmationValuesExcelExporter;
            _drugLookUpRepository = lookup_drugRepository;
            _testCategoryLookUpRepository = lookup_testCategoryRepository;

        }

        public async Task<PagedResultDto<GetConfirmationValueForViewDto>> GetAll(GetAllConfirmationValuesInput input)
        {

                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var unitOfMeasurementFilter = input.UnitOfMeasurementFilter.HasValue
                            ? (EnumUnitOfMeasurement)input.UnitOfMeasurementFilter
                            : default;

                var filteredConfirmationValues = _confirmationValueRepository.GetAll()
                            .Include(e => e.DrugFk)
                            .Include(e => e.TestCategoryFk)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                            .WhereIf(input.MinScreenValueFilter != null, e => e.ScreenValue >= input.MinScreenValueFilter)
                            .WhereIf(input.MaxScreenValueFilter != null, e => e.ScreenValue <= input.MaxScreenValueFilter)
                            .WhereIf(input.MinConfirmValueFilter != null, e => e.ConfirmValue >= input.MinConfirmValueFilter)
                            .WhereIf(input.MaxConfirmValueFilter != null, e => e.ConfirmValue <= input.MaxConfirmValueFilter)
                            .WhereIf(input.UnitOfMeasurementFilter.HasValue && input.UnitOfMeasurementFilter > -1, e => e.UnitOfMeasurement == unitOfMeasurementFilter)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.DrugNameFilter), e => e.DrugFk != null && e.DrugFk.Name == input.DrugNameFilter)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.TestCategoryNameFilter), e => e.TestCategoryFk != null && e.TestCategoryFk.Name == input.TestCategoryNameFilter);

                var pagedAndFilteredConfirmationValues = filteredConfirmationValues
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var confirmationValues = from o in pagedAndFilteredConfirmationValues
                                         join o1 in _drugLookUpRepository.GetAll() on o.DrugId equals o1.Id into j1
                                         from s1 in j1.DefaultIfEmpty()

                                         join o2 in _testCategoryLookUpRepository.GetAll() on o.TestCategoryId equals o2.Id into j2
                                         from s2 in j2.DefaultIfEmpty()

                                         select new
                                         {

                                             o.ScreenValue,
                                             o.ConfirmValue,
                                             o.UnitOfMeasurement,
                                             Id = o.Id,
                                             DrugName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                             TestCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                         };

                var totalCount = await filteredConfirmationValues.CountAsync();

                var dbList = await confirmationValues.ToListAsync();
                var results = new List<GetConfirmationValueForViewDto>();

                foreach (var o in dbList)
                {
                    var res = new GetConfirmationValueForViewDto()
                    {
                        ConfirmationValue = new ConfirmationValueDto
                        {

                            ScreenValue = o.ScreenValue,
                            ConfirmValue = o.ConfirmValue,
                            UnitOfMeasurement = o.UnitOfMeasurement,
                            Id = o.Id,
                        },
                        DrugName = o.DrugName,
                        TestCategoryName = o.TestCategoryName
                    };

                    results.Add(res);
                }

                return new PagedResultDto<GetConfirmationValueForViewDto>(
                    totalCount,
                    results
                );
        }

        public async Task<GetConfirmationValueForViewDto> GetConfirmationValueForView(Guid id)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var confirmationValue = await _confirmationValueRepository.GetAsync(id);

                var output = new GetConfirmationValueForViewDto { ConfirmationValue = ObjectMapper.Map<ConfirmationValueDto>(confirmationValue) };

                if (output.ConfirmationValue.DrugId != null)
                {
                    var _lookupDrug = await _drugLookUpRepository.FirstOrDefaultAsync((Guid)output.ConfirmationValue.DrugId);
                    output.DrugName = _lookupDrug?.Name?.ToString();
                }

                if (output.ConfirmationValue.TestCategoryId != null)
                {
                    var _lookupTestCategory = await _testCategoryLookUpRepository.FirstOrDefaultAsync((Guid)output.ConfirmationValue.TestCategoryId);
                    output.TestCategoryName = _lookupTestCategory?.Name?.ToString();
                }

                return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ConfirmationValues_Edit)]
        public async Task<GetConfirmationValueForEditOutput> GetConfirmationValueForEdit(EntityDto<Guid> input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                var confirmationValue = await _confirmationValueRepository.FirstOrDefaultAsync(input.Id);

                var output = new GetConfirmationValueForEditOutput { ConfirmationValue = ObjectMapper.Map<CreateOrEditConfirmationValueDto>(confirmationValue) };

                if (output.ConfirmationValue.DrugId != null)
                {
                    var _lookupDrug = await _drugLookUpRepository.FirstOrDefaultAsync((Guid)output.ConfirmationValue.DrugId);
                    output.DrugName = _lookupDrug?.Name?.ToString();
                }

                if (output.ConfirmationValue.TestCategoryId != null)
                {
                    var _lookupTestCategory = await _testCategoryLookUpRepository.FirstOrDefaultAsync((Guid)output.ConfirmationValue.TestCategoryId);
                    output.TestCategoryName = _lookupTestCategory?.Name?.ToString();
                }

                return output;
        }

        public async Task CreateOrEdit(CreateOrEditConfirmationValueDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ConfirmationValues_Create)]
        protected virtual async Task Create(CreateOrEditConfirmationValueDto input)
        {
            var confirmationValue = ObjectMapper.Map<ConfirmationValue>(input);

            await _confirmationValueRepository.InsertAsync(confirmationValue);

        }

        [AbpAuthorize(AppPermissions.Pages_ConfirmationValues_Edit)]
        protected virtual async Task Update(CreateOrEditConfirmationValueDto input)
        {
            var confirmationValue = await _confirmationValueRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, confirmationValue);

        }

        [AbpAuthorize(AppPermissions.Pages_ConfirmationValues_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                await _confirmationValueRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetConfirmationValuesToExcel(GetAllConfirmationValuesForExcelInput input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                var unitOfMeasurementFilter = input.UnitOfMeasurementFilter.HasValue
                            ? (EnumUnitOfMeasurement)input.UnitOfMeasurementFilter
                            : default;

                var filteredConfirmationValues = _confirmationValueRepository.GetAll()
                            .Include(e => e.DrugFk)
                            .Include(e => e.TestCategoryFk)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                            .WhereIf(input.MinScreenValueFilter != null, e => e.ScreenValue >= input.MinScreenValueFilter)
                            .WhereIf(input.MaxScreenValueFilter != null, e => e.ScreenValue <= input.MaxScreenValueFilter)
                            .WhereIf(input.MinConfirmValueFilter != null, e => e.ConfirmValue >= input.MinConfirmValueFilter)
                            .WhereIf(input.MaxConfirmValueFilter != null, e => e.ConfirmValue <= input.MaxConfirmValueFilter)
                            .WhereIf(input.UnitOfMeasurementFilter.HasValue && input.UnitOfMeasurementFilter > -1, e => e.UnitOfMeasurement == unitOfMeasurementFilter)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.DrugNameFilter), e => e.DrugFk != null && e.DrugFk.Name == input.DrugNameFilter)
                            .WhereIf(!string.IsNullOrWhiteSpace(input.TestCategoryNameFilter), e => e.TestCategoryFk != null && e.TestCategoryFk.Name == input.TestCategoryNameFilter);

                var query = (from o in filteredConfirmationValues
                             join o1 in _drugLookUpRepository.GetAll() on o.DrugId equals o1.Id into j1
                             from s1 in j1.DefaultIfEmpty()

                             join o2 in _testCategoryLookUpRepository.GetAll() on o.TestCategoryId equals o2.Id into j2
                             from s2 in j2.DefaultIfEmpty()

                             select new GetConfirmationValueForViewDto()
                             {
                                 ConfirmationValue = new ConfirmationValueDto
                                 {
                                     ScreenValue = o.ScreenValue,
                                     ConfirmValue = o.ConfirmValue,
                                     UnitOfMeasurement = o.UnitOfMeasurement,
                                     Id = o.Id
                                 },
                                 DrugName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                 TestCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                             });

                var confirmationValueListDtos = await query.ToListAsync();

                return _confirmationValuesExcelExporter.ExportToFile(confirmationValueListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ConfirmationValues)]
        public async Task<PagedResultDto<ConfirmationValueDrugLookupTableDto>> GetAllDrugForLookupTable(GetAllForLookupTableInput input)
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                var query = _drugLookUpRepository.GetAll().WhereIf(
                       !string.IsNullOrWhiteSpace(input.Filter),
                      e => e.Name != null && e.Name.Contains(input.Filter)
                   );

                var totalCount = await query.CountAsync();

                var drugList = await query
                    .PageBy(input)
                    .ToListAsync();

                var lookupTableDtoList = new List<ConfirmationValueDrugLookupTableDto>();
                foreach (var drug in drugList)
                {
                    lookupTableDtoList.Add(new ConfirmationValueDrugLookupTableDto
                    {
                        Id = drug.Id.ToString(),
                        DisplayName = drug.Name?.ToString()
                    });
                }

                return new PagedResultDto<ConfirmationValueDrugLookupTableDto>(
                    totalCount,
                    lookupTableDtoList
                );
        }
        [AbpAuthorize(AppPermissions.Pages_ConfirmationValues)]
        public async Task<List<ConfirmationValueTestCategoryLookupTableDto>> GetAllTestCategoryForTableDropdown()
        {
                if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                return await _testCategoryLookUpRepository.GetAll()
                    .Select(testCategory => new ConfirmationValueTestCategoryLookupTableDto
                    {
                        Id = testCategory.Id.ToString(),
                        DisplayName = testCategory == null || testCategory.Name == null ? "" : testCategory.Name.ToString()
                    }).ToListAsync();
        }

    }
}