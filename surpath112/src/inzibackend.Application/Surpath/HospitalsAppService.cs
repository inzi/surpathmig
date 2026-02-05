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
using inzibackend.Surpath;
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
    [AbpAuthorize(AppPermissions.Pages_Administration_Hospitals)]
    public class HospitalsAppService : inzibackendAppServiceBase, IHospitalsAppService
    {
        private readonly IRepository<Hospital> _hospitalRepository;
        private readonly IHospitalsExcelExporter _hospitalsExcelExporter;

        public HospitalsAppService(IRepository<Hospital> hospitalRepository, IHospitalsExcelExporter hospitalsExcelExporter)
        {
            _hospitalRepository = hospitalRepository;
            _hospitalsExcelExporter = hospitalsExcelExporter;

        }

        public async Task<PagedResultDto<GetHospitalForViewDto>> GetAll(GetAllHospitalsInput input)
        {
            if (AbpSession.TenantId == null)
            {
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            }

            var stateFilter = input.StateFilter.HasValue
                        // ? (enumUSStates)((int)input.StateFilter)
                        ? (enumUSStates)Enum.ToObject(typeof(enumUSStates), input.StateFilter)
                        : default;

            var filteredHospitals = _hospitalRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.PrimaryContact.Contains(input.Filter) || e.PrimaryContactPhone.Contains(input.Filter) || e.PrimaryContactEmail.Contains(input.Filter) || e.Address1.Contains(input.Filter) || e.Address2.Contains(input.Filter) || e.City.Contains(input.Filter) || e.ZipCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PrimaryContactFilter), e => e.PrimaryContact.Contains(input.PrimaryContactFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PrimaryContactPhoneFilter), e => e.PrimaryContactPhone.Contains(input.PrimaryContactPhoneFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PrimaryContactEmailFilter), e => e.PrimaryContactEmail.Contains(input.PrimaryContactEmailFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Address1Filter), e => e.Address1.Contains(input.Address1Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Address2Filter), e => e.Address2.Contains(input.Address2Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityFilter), e => e.City.Contains(input.CityFilter))
                        .WhereIf(input.StateFilter.HasValue && input.StateFilter > -1, e => e.State == stateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ZipCodeFilter), e => e.ZipCode.Contains(input.ZipCodeFilter));

            var pagedAndFilteredHospitals = filteredHospitals
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var hospitals = from o in pagedAndFilteredHospitals
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
                                Id = o.Id
                            };

            var totalCount = await filteredHospitals.CountAsync();

            var dbList = await hospitals.ToListAsync();
            var results = new List<GetHospitalForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetHospitalForViewDto()
                {
                    Hospital = new HospitalDto
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
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetHospitalForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetHospitalForViewDto> GetHospitalForView(int id)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var hospital = await _hospitalRepository.GetAsync(id);

            var output = new GetHospitalForViewDto { Hospital = ObjectMapper.Map<HospitalDto>(hospital) };

            return output;

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Hospitals_Edit)]
        public async Task<GetHospitalForEditOutput> GetHospitalForEdit(EntityDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var hospital = await _hospitalRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetHospitalForEditOutput { Hospital = ObjectMapper.Map<CreateOrEditHospitalDto>(hospital) };

            return output;

        }

        public async Task CreateOrEdit(CreateOrEditHospitalDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_Hospitals_Create)]
        protected virtual async Task Create(CreateOrEditHospitalDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var hospital = ObjectMapper.Map<Hospital>(input);

            if (AbpSession.TenantId != null)
            {
                hospital.TenantId = (int?)AbpSession.TenantId;
            }

            await _hospitalRepository.InsertAsync(hospital);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Hospitals_Edit)]
        protected virtual async Task Update(CreateOrEditHospitalDto input)
        {

            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var hospital = await _hospitalRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, hospital);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Hospitals_Delete)]
        public async Task Delete(EntityDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            await _hospitalRepository.DeleteAsync(input.Id);

        }

        public async Task<FileDto> GetHospitalsToExcel(GetAllHospitalsForExcelInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var stateFilter = input.StateFilter.HasValue
                        ? (enumUSStates)Enum.ToObject(typeof(enumUSStates), input.StateFilter)
                        : default;

            var filteredHospitals = _hospitalRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.PrimaryContact.Contains(input.Filter) || e.PrimaryContactPhone.Contains(input.Filter) || e.PrimaryContactEmail.Contains(input.Filter) || e.Address1.Contains(input.Filter) || e.Address2.Contains(input.Filter) || e.City.Contains(input.Filter) || e.ZipCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PrimaryContactFilter), e => e.PrimaryContact.Contains(input.PrimaryContactFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PrimaryContactPhoneFilter), e => e.PrimaryContactPhone.Contains(input.PrimaryContactPhoneFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PrimaryContactEmailFilter), e => e.PrimaryContactEmail.Contains(input.PrimaryContactEmailFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Address1Filter), e => e.Address1.Contains(input.Address1Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Address2Filter), e => e.Address2.Contains(input.Address2Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityFilter), e => e.City.Contains(input.CityFilter))
                        .WhereIf(input.StateFilter.HasValue && input.StateFilter > -1, e => e.State == stateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ZipCodeFilter), e => e.ZipCode.Contains(input.ZipCodeFilter));

            var query = (from o in filteredHospitals
                         select new GetHospitalForViewDto()
                         {
                             Hospital = new HospitalDto
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
                             }
                         });

            var hospitalListDtos = await query.ToListAsync();

            return _hospitalsExcelExporter.ExportToFile(hospitalListDtos);

        }

    }
}