using inzibackend.Surpath;

using inzibackend;

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
    [AbpAuthorize(AppPermissions.Pages_MedicalUnits)]
    public class MedicalUnitsAppService : inzibackendAppServiceBase, IMedicalUnitsAppService
    {
        private readonly IRepository<MedicalUnit> _medicalUnitRepository;
        private readonly IMedicalUnitsExcelExporter _medicalUnitsExcelExporter;
        private readonly IRepository<Hospital, int> _hospitalookUpRepository;

        public MedicalUnitsAppService(IRepository<MedicalUnit> medicalUnitRepository, IMedicalUnitsExcelExporter medicalUnitsExcelExporter, IRepository<Hospital, int> lookup_hospitalRepository)
        {
            _medicalUnitRepository = medicalUnitRepository;
            _medicalUnitsExcelExporter = medicalUnitsExcelExporter;
            _hospitalookUpRepository = lookup_hospitalRepository;

        }

        public async Task<PagedResultDto<GetMedicalUnitForViewDto>> GetAll(GetAllMedicalUnitsInput input)
        {
            if (AbpSession.TenantId == null)
            {
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            }

            var stateFilter = input.StateFilter.HasValue
                        ? (enumUSStates)input.StateFilter
                        : default;

            var filteredMedicalUnits = _medicalUnitRepository.GetAll()
                        .Include(e => e.HospitalFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.PrimaryContact.Contains(input.Filter) || e.PrimaryContactPhone.Contains(input.Filter) || e.PrimaryContactEmail.Contains(input.Filter) || e.Address1.Contains(input.Filter) || e.Address2.Contains(input.Filter) || e.City.Contains(input.Filter) || e.ZipCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PrimaryContactFilter), e => e.PrimaryContact.Contains(input.PrimaryContactFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PrimaryContactPhoneFilter), e => e.PrimaryContactPhone.Contains(input.PrimaryContactPhoneFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PrimaryContactEmailFilter), e => e.PrimaryContactEmail.Contains(input.PrimaryContactEmailFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Address1Filter), e => e.Address1.Contains(input.Address1Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Address2Filter), e => e.Address2.Contains(input.Address2Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityFilter), e => e.City.Contains(input.CityFilter))
                        .WhereIf(input.StateFilter.HasValue && input.StateFilter > -1, e => e.State == stateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ZipCodeFilter), e => e.ZipCode.Contains(input.ZipCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HospitalNameFilter), e => e.HospitalFk != null && e.HospitalFk.Name == input.HospitalNameFilter);

            var pagedAndFilteredMedicalUnits = filteredMedicalUnits
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var medicalUnits = from o in pagedAndFilteredMedicalUnits
                               join o1 in _hospitalookUpRepository.GetAll() on o.HospitalId equals o1.Id into j1
                               from s1 in j1.DefaultIfEmpty()

                               select new
                               {

                                   o.Name,
                                   o.PrimaryContact,
                                   o.PrimaryContactPhone,
                                   o.PrimaryContactEmail,
                                   o.Address1,
                                   o.Address2,
                                   o.City,
                                   o.State,
                                   o.ZipCode,
                                   Id = o.Id,
                                   HospitalName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                               };

            var totalCount = await filteredMedicalUnits.CountAsync();

            var dbList = await medicalUnits.ToListAsync();
            var results = new List<GetMedicalUnitForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetMedicalUnitForViewDto()
                {
                    MedicalUnit = new MedicalUnitDto
                    {

                        Name = o.Name,
                        PrimaryContact = o.PrimaryContact,
                        PrimaryContactPhone = o.PrimaryContactPhone,
                        PrimaryContactEmail = o.PrimaryContactEmail,
                        Address1 = o.Address1,
                        Address2 = o.Address2,
                        City = o.City,
                        State = o.State,
                        ZipCode = o.ZipCode,
                        Id = o.Id,
                    },
                    HospitalName = o.HospitalName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetMedicalUnitForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetMedicalUnitForViewDto> GetMedicalUnitForView(int id)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var medicalUnit = await _medicalUnitRepository.GetAsync(id);

            var output = new GetMedicalUnitForViewDto { MedicalUnit = ObjectMapper.Map<MedicalUnitDto>(medicalUnit) };

            if (output.MedicalUnit.HospitalId != null)
            {
                var _lookupHospital = await _hospitalookUpRepository.FirstOrDefaultAsync((int)output.MedicalUnit.HospitalId);
                output.HospitalName = _lookupHospital?.Name?.ToString();
            }

            return output;

        }

        [AbpAuthorize(AppPermissions.Pages_MedicalUnits_Edit)]
        public async Task<GetMedicalUnitForEditOutput> GetMedicalUnitForEdit(EntityDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var medicalUnit = await _medicalUnitRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetMedicalUnitForEditOutput { MedicalUnit = ObjectMapper.Map<CreateOrEditMedicalUnitDto>(medicalUnit) };

            if (output.MedicalUnit.HospitalId != null)
            {
                var _lookupHospital = await _hospitalookUpRepository.FirstOrDefaultAsync((int)output.MedicalUnit.HospitalId);
                output.HospitalName = _lookupHospital?.Name?.ToString();
            }

            return output;

        }

        public async Task CreateOrEdit(CreateOrEditMedicalUnitDto input)
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

        [AbpAuthorize(AppPermissions.Pages_MedicalUnits_Create)]
        protected virtual async Task Create(CreateOrEditMedicalUnitDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var medicalUnit = ObjectMapper.Map<MedicalUnit>(input);

            if (AbpSession.TenantId != null)
            {
                medicalUnit.TenantId = (int?)AbpSession.TenantId;
            }

            await _medicalUnitRepository.InsertAsync(medicalUnit);

        }

        [AbpAuthorize(AppPermissions.Pages_MedicalUnits_Edit)]
        protected virtual async Task Update(CreateOrEditMedicalUnitDto input)
        {

            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var medicalUnit = await _medicalUnitRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, medicalUnit);

        }

        [AbpAuthorize(AppPermissions.Pages_MedicalUnits_Delete)]
        public async Task Delete(EntityDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            await _medicalUnitRepository.DeleteAsync(input.Id);

        }

        public async Task<FileDto> GetMedicalUnitsToExcel(GetAllMedicalUnitsForExcelInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var stateFilter = input.StateFilter.HasValue
                        ? (enumUSStates)input.StateFilter
                        : default;

            var filteredMedicalUnits = _medicalUnitRepository.GetAll()
                        .Include(e => e.HospitalFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.PrimaryContact.Contains(input.Filter) || e.PrimaryContactPhone.Contains(input.Filter) || e.PrimaryContactEmail.Contains(input.Filter) || e.Address1.Contains(input.Filter) || e.Address2.Contains(input.Filter) || e.City.Contains(input.Filter) || e.ZipCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PrimaryContactFilter), e => e.PrimaryContact.Contains(input.PrimaryContactFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PrimaryContactPhoneFilter), e => e.PrimaryContactPhone.Contains(input.PrimaryContactPhoneFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PrimaryContactEmailFilter), e => e.PrimaryContactEmail.Contains(input.PrimaryContactEmailFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Address1Filter), e => e.Address1.Contains(input.Address1Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Address2Filter), e => e.Address2.Contains(input.Address2Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityFilter), e => e.City.Contains(input.CityFilter))
                        .WhereIf(input.StateFilter.HasValue && input.StateFilter > -1, e => e.State == stateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ZipCodeFilter), e => e.ZipCode.Contains(input.ZipCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HospitalNameFilter), e => e.HospitalFk != null && e.HospitalFk.Name == input.HospitalNameFilter);

            var query = (from o in filteredMedicalUnits
                         join o1 in _hospitalookUpRepository.GetAll() on o.HospitalId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetMedicalUnitForViewDto()
                         {
                             MedicalUnit = new MedicalUnitDto
                             {
                                 Name = o.Name,
                                 PrimaryContact = o.PrimaryContact,
                                 PrimaryContactPhone = o.PrimaryContactPhone,
                                 PrimaryContactEmail = o.PrimaryContactEmail,
                                 Address1 = o.Address1,
                                 Address2 = o.Address2,
                                 City = o.City,
                                 State = o.State,
                                 ZipCode = o.ZipCode,
                                 Id = o.Id
                             },
                             HospitalName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var medicalUnitListDtos = await query.ToListAsync();

            return _medicalUnitsExcelExporter.ExportToFile(medicalUnitListDtos);

        }

        [AbpAuthorize(AppPermissions.Pages_MedicalUnits)]
        public async Task<List<MedicalUnitHospitalLookupTableDto>> GetAllHospitalForTableDropdown()
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            return await _hospitalookUpRepository.GetAll()
                .Select(hospital => new MedicalUnitHospitalLookupTableDto
                {
                    Id = hospital.Id,
                    DisplayName = hospital == null || hospital.Name == null ? "" : hospital.Name.ToString()
                }).ToListAsync();

        }

    }
}