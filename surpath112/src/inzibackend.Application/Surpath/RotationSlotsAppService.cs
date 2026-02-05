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
using Abp.EntityFrameworkCore.Repositories;
using inzibackend.EntityFrameworkCore;
using inzibackend.Surpath.Statics;
using NUglify.Helpers;

namespace inzibackend.Surpath
{
    [AbpAuthorize(AppPermissions.Pages_RotationSlots)]
    public class RotationSlotsAppService : inzibackendAppServiceBase, IRotationSlotsAppService
    {
        private readonly IRepository<RotationSlot> _rotationSlotRepository;
        private readonly IRotationSlotsExcelExporter _rotationSlotsExcelExporter;
        private readonly IRepository<Hospital, int> _hospitalLookUpRepository;
        private readonly IRepository<MedicalUnit, int> _medicalUnitLookUpRepository;

        public RotationSlotsAppService(IRepository<RotationSlot> rotationSlotRepository, IRotationSlotsExcelExporter rotationSlotsExcelExporter, IRepository<Hospital, int> lookup_hospitalRepository, IRepository<MedicalUnit, int> lookup_medicalUnitRepository)
        {
            _rotationSlotRepository = rotationSlotRepository;
            _rotationSlotsExcelExporter = rotationSlotsExcelExporter;
            _hospitalLookUpRepository = lookup_hospitalRepository;
            _medicalUnitLookUpRepository = lookup_medicalUnitRepository;

        }

        public async Task<PagedResultDto<GetRotationSlotForViewDto>> GetAll(GetAllRotationSlotsInput input)
        {
            if (AbpSession.TenantId == null)
            {
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            }

            var shiftTypeFilter = input.ShiftTypeFilter.HasValue
                        ? (EnumSlotShiftType)input.ShiftTypeFilter
                        : default;

            var filteredRotationSlots = _rotationSlotRepository.GetAll()
                        .Include(e => e.HospitalFk)
                        .Include(e => e.MedicalUnitFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.SlotId.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SlotIdFilter), e => e.SlotId.Contains(input.SlotIdFilter))
                        .WhereIf(input.MinAvailableSlotsFilter != null, e => e.AvailableSlots >= input.MinAvailableSlotsFilter)
                        .WhereIf(input.MaxAvailableSlotsFilter != null, e => e.AvailableSlots <= input.MaxAvailableSlotsFilter)
                        .WhereIf(input.MinShiftStartDateFilter != null, e => e.ShiftStartDate >= input.MinShiftStartDateFilter)
                        .WhereIf(input.MaxShiftStartDateFilter != null, e => e.ShiftStartDate <= input.MaxShiftStartDateFilter)
                        .WhereIf(input.MinShiftEndDateFilter != null, e => e.ShiftEndDate >= input.MinShiftEndDateFilter)
                        .WhereIf(input.MaxShiftEndDateFilter != null, e => e.ShiftEndDate <= input.MaxShiftEndDateFilter)
                        .WhereIf(input.MinShiftStartTimeFilter != null, e => e.ShiftStartTime >= input.MinShiftStartTimeFilter)
                        .WhereIf(input.MaxShiftStartTimeFilter != null, e => e.ShiftStartTime <= input.MaxShiftStartTimeFilter)
                        .WhereIf(input.MinShiftEndTimeFilter != null, e => e.ShiftEndTime >= input.MinShiftEndTimeFilter)
                        .WhereIf(input.MaxShiftEndTimeFilter != null, e => e.ShiftEndTime <= input.MaxShiftEndTimeFilter)
                        .WhereIf(input.MinShiftHoursFilter != null, e => e.ShiftHours >= input.MinShiftHoursFilter)
                        .WhereIf(input.MaxShiftHoursFilter != null, e => e.ShiftHours <= input.MaxShiftHoursFilter)
                        .WhereIf(input.NotifyHospitalFilter.HasValue && input.NotifyHospitalFilter > -1, e => (input.NotifyHospitalFilter == 1 && e.NotifyHospital) || (input.NotifyHospitalFilter == 0 && !e.NotifyHospital))
                        .WhereIf(input.MinHospitalNotifiedDateTimeFilter != null, e => e.HospitalNotifiedDateTime >= input.MinHospitalNotifiedDateTimeFilter)
                        .WhereIf(input.MaxHospitalNotifiedDateTimeFilter != null, e => e.HospitalNotifiedDateTime <= input.MaxHospitalNotifiedDateTimeFilter)
                        .WhereIf(input.ShiftTypeFilter.HasValue && input.ShiftTypeFilter > -1, e => e.ShiftType == shiftTypeFilter)
                        .WhereIf(input.MinBidStartDateTimeFilter != null, e => e.BidStartDateTime >= input.MinBidStartDateTimeFilter)
                        .WhereIf(input.MaxBidStartDateTimeFilter != null, e => e.BidStartDateTime <= input.MaxBidStartDateTimeFilter)
                        .WhereIf(input.MinBidEndDateTimeFilter != null, e => e.BidEndDateTime >= input.MinBidEndDateTimeFilter)
                        .WhereIf(input.MaxBidEndDateTimeFilter != null, e => e.BidEndDateTime <= input.MaxBidEndDateTimeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HospitalNameFilter), e => e.HospitalFk != null && e.HospitalFk.Name == input.HospitalNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MedicalUnitNameFilter), e => e.MedicalUnitFk != null && e.MedicalUnitFk.Name == input.MedicalUnitNameFilter);

            var pagedAndFilteredRotationSlots = filteredRotationSlots
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var rotationSlots = from o in pagedAndFilteredRotationSlots
                                join o1 in _hospitalLookUpRepository.GetAll() on o.HospitalId equals o1.Id into j1
                                from s1 in j1.DefaultIfEmpty()

                                join o2 in _medicalUnitLookUpRepository.GetAll() on o.MedicalUnitId equals o2.Id into j2
                                from s2 in j2.DefaultIfEmpty()

                                select new
                                {

                                    o.SlotId,
                                    o.AvailableSlots,
                                    o.ShiftStartDate,
                                    o.ShiftEndDate,
                                    o.ShiftStartTime,
                                    o.ShiftEndTime,
                                    o.ShiftHours,
                                    o.NotifyHospital,
                                    o.HospitalNotifiedDateTime,
                                    o.ShiftType,
                                    o.BidStartDateTime,
                                    o.BidEndDateTime,
                                    Id = o.Id,
                                    HospitalName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                    MedicalUnitName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                };

            var totalCount = await filteredRotationSlots.CountAsync();

            var dbList = await rotationSlots.ToListAsync();
            var results = new List<GetRotationSlotForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetRotationSlotForViewDto()
                {
                    RotationSlot = new RotationSlotDto
                    {

                        SlotId = o.SlotId,
                        AvailableSlots = o.AvailableSlots,
                        ShiftStartDate = o.ShiftStartDate,
                        ShiftEndDate = o.ShiftEndDate,
                        ShiftStartTime = o.ShiftStartTime,
                        ShiftEndTime = o.ShiftEndTime,
                        ShiftHours = o.ShiftHours,
                        NotifyHospital = o.NotifyHospital,
                        HospitalNotifiedDateTime = o.HospitalNotifiedDateTime,
                        ShiftType = o.ShiftType,
                        BidStartDateTime = o.BidStartDateTime,
                        BidEndDateTime = o.BidEndDateTime,
                        Id = o.Id,
                    },
                    HospitalName = o.HospitalName,
                    MedicalUnitName = o.MedicalUnitName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetRotationSlotForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetRotationSlotForViewDto> GetRotationSlotForView(int id)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            //var rotationSlot = await _rotationSlotRepository.GetAsync(id);
            var rotationSlot = await ((inzibackendDbContext)_rotationSlotRepository.GetDbContext()).RotationSlots
    .Include(rs => rs.HospitalFk)
    .Include(rs => rs.MedicalUnitFk)
    .Include(rs => rs.SlotAvailableDays)
    .Include(rs => rs.SlotRotationDays)
    .FirstOrDefaultAsync(e => e.Id == id);
            var output = new GetRotationSlotForViewDto { RotationSlot = ObjectMapper.Map<RotationSlotDto>(rotationSlot) };

            if (output.RotationSlot.HospitalId != null)
            {
                var _lookupHospital = await _hospitalLookUpRepository.FirstOrDefaultAsync((int)output.RotationSlot.HospitalId);
                output.HospitalName = _lookupHospital?.Name?.ToString();
            }

            if (output.RotationSlot.MedicalUnitId != null)
            {
                var _lookupMedicalUnit = await _medicalUnitLookUpRepository.FirstOrDefaultAsync((int)output.RotationSlot.MedicalUnitId);
                output.MedicalUnitName = _lookupMedicalUnit?.Name?.ToString();
            }

            return output;

        }

        [AbpAuthorize(AppPermissions.Pages_RotationSlots_Edit)]
        public async Task<GetRotationSlotForEditOutput> GetRotationSlotForEdit(EntityDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            //var rotationSlot = await _rotationSlotRepository.FirstOrDefaultAsync(input.Id);
            var rotationSlot = await ((inzibackendDbContext)_rotationSlotRepository.GetDbContext()).RotationSlots
    .Include(rs => rs.HospitalFk)
    .Include(rs => rs.MedicalUnitFk)
    .Include(rs => rs.SlotAvailableDays)
    .Include(rs => rs.SlotRotationDays)
    .FirstOrDefaultAsync(e => e.Id == input.Id);
            var output = new GetRotationSlotForEditOutput { RotationSlot = ObjectMapper.Map<CreateOrEditRotationSlotDto>(rotationSlot) };

            return output;

        }

        public async Task CreateOrEdit(CreateOrEditRotationSlotDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            if (string.IsNullOrWhiteSpace(input.SlotId) || input.SlotId == RotationSlotConsts.DefaultSlotId)
                input.SlotId = await GenerateUniqueSlotId();
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }

        }

        [AbpAuthorize(AppPermissions.Pages_RotationSlots_Create)]
        protected virtual async Task Create(CreateOrEditRotationSlotDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var rotationSlot = ObjectMapper.Map<RotationSlot>(input);

            if (AbpSession.TenantId != null)
            {
                rotationSlot.TenantId = (int?)AbpSession.TenantId;
            }

            await _rotationSlotRepository.InsertAsync(rotationSlot);

        }

        [AbpAuthorize(AppPermissions.Pages_RotationSlots_Create)]
        public async Task Clone(EntityDto[] input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            foreach (var entityDto in input)
            {
                //var rotationSlot = await _rotationSlotRepository.GetAllIncluding(id.Id);

                var rotationSlot = await _rotationSlotRepository.GetAll()
                      .Include(e => e.HospitalFk)
                      .Include(e => e.MedicalUnitFk)
                      .Include(e => e.SlotAvailableDays)
                      .Include(e => e.SlotRotationDays)
                      .FirstOrDefaultAsync(e => e.Id == entityDto.Id);
                var newRotationSlot = ObjectMapper.Map<RotationSlot>(rotationSlot);
                newRotationSlot.Id = 0;
                newRotationSlot.SlotId = await GenerateUniqueSlotId();
                await _rotationSlotRepository.InsertAsync(newRotationSlot);
            }

        }



        [AbpAuthorize(AppPermissions.Pages_RotationSlots_Edit)]
        protected virtual async Task Update(CreateOrEditRotationSlotDto input)
        {

            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            //var rotationSlot = await _rotationSlotRepository.FirstOrDefaultAsync((int)input.Id);
            //ObjectMapper.Map(input, rotationSlot);
            var rotationSlot = await _rotationSlotRepository.GetAll()
               .Include(e => e.HospitalFk)
               .Include(e => e.MedicalUnitFk)
               .Include(e => e.SlotAvailableDays)
               .Include(e => e.SlotRotationDays)
               .FirstOrDefaultAsync(e => e.Id == input.Id);
            ObjectMapper.Map<CreateOrEditRotationSlotDto, RotationSlot>(input, rotationSlot);
            _rotationSlotRepository.GetDbContext().SaveChanges();
        }

        [AbpAuthorize(AppPermissions.Pages_RotationSlots_Delete)]
        public async Task Delete(EntityDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            await _rotationSlotRepository.DeleteAsync(input.Id);

        }

        [AbpAuthorize(AppPermissions.Pages_RotationSlots_Delete)]
        public async Task MultiDelete(EntityDto[] input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            foreach (var entityDto in input)
            {
                await _rotationSlotRepository.DeleteAsync(entityDto.Id);
            }

        }


        public async Task<FileDto> GetRotationSlotsToExcel(GetAllRotationSlotsForExcelInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var shiftTypeFilter = input.ShiftTypeFilter.HasValue
                        ? (EnumSlotShiftType)input.ShiftTypeFilter
                        : default;

            var filteredRotationSlots = _rotationSlotRepository.GetAll()
                        .Include(e => e.HospitalFk)
                        .Include(e => e.MedicalUnitFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.SlotId.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SlotIdFilter), e => e.SlotId.Contains(input.SlotIdFilter))
                        .WhereIf(input.MinAvailableSlotsFilter != null, e => e.AvailableSlots >= input.MinAvailableSlotsFilter)
                        .WhereIf(input.MaxAvailableSlotsFilter != null, e => e.AvailableSlots <= input.MaxAvailableSlotsFilter)
                        .WhereIf(input.MinShiftStartDateFilter != null, e => e.ShiftStartDate >= input.MinShiftStartDateFilter)
                        .WhereIf(input.MaxShiftStartDateFilter != null, e => e.ShiftStartDate <= input.MaxShiftStartDateFilter)
                        .WhereIf(input.MinShiftEndDateFilter != null, e => e.ShiftEndDate >= input.MinShiftEndDateFilter)
                        .WhereIf(input.MaxShiftEndDateFilter != null, e => e.ShiftEndDate <= input.MaxShiftEndDateFilter)
                        .WhereIf(input.MinShiftStartTimeFilter != null, e => e.ShiftStartTime >= input.MinShiftStartTimeFilter)
                        .WhereIf(input.MaxShiftStartTimeFilter != null, e => e.ShiftStartTime <= input.MaxShiftStartTimeFilter)
                        .WhereIf(input.MinShiftEndTimeFilter != null, e => e.ShiftEndTime >= input.MinShiftEndTimeFilter)
                        .WhereIf(input.MaxShiftEndTimeFilter != null, e => e.ShiftEndTime <= input.MaxShiftEndTimeFilter)
                        .WhereIf(input.MinShiftHoursFilter != null, e => e.ShiftHours >= input.MinShiftHoursFilter)
                        .WhereIf(input.MaxShiftHoursFilter != null, e => e.ShiftHours <= input.MaxShiftHoursFilter)
                        .WhereIf(input.NotifyHospitalFilter.HasValue && input.NotifyHospitalFilter > -1, e => (input.NotifyHospitalFilter == 1 && e.NotifyHospital) || (input.NotifyHospitalFilter == 0 && !e.NotifyHospital))
                        .WhereIf(input.MinHospitalNotifiedDateTimeFilter != null, e => e.HospitalNotifiedDateTime >= input.MinHospitalNotifiedDateTimeFilter)
                        .WhereIf(input.MaxHospitalNotifiedDateTimeFilter != null, e => e.HospitalNotifiedDateTime <= input.MaxHospitalNotifiedDateTimeFilter)
                        .WhereIf(input.ShiftTypeFilter.HasValue && input.ShiftTypeFilter > -1, e => e.ShiftType == shiftTypeFilter)
                        .WhereIf(input.MinBidStartDateTimeFilter != null, e => e.BidStartDateTime >= input.MinBidStartDateTimeFilter)
                        .WhereIf(input.MaxBidStartDateTimeFilter != null, e => e.BidStartDateTime <= input.MaxBidStartDateTimeFilter)
                        .WhereIf(input.MinBidEndDateTimeFilter != null, e => e.BidEndDateTime >= input.MinBidEndDateTimeFilter)
                        .WhereIf(input.MaxBidEndDateTimeFilter != null, e => e.BidEndDateTime <= input.MaxBidEndDateTimeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HospitalNameFilter), e => e.HospitalFk != null && e.HospitalFk.Name == input.HospitalNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MedicalUnitNameFilter), e => e.MedicalUnitFk != null && e.MedicalUnitFk.Name == input.MedicalUnitNameFilter);

            var query = (from o in filteredRotationSlots
                         join o1 in _hospitalLookUpRepository.GetAll() on o.HospitalId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _medicalUnitLookUpRepository.GetAll() on o.MedicalUnitId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetRotationSlotForViewDto()
                         {
                             RotationSlot = new RotationSlotDto
                             {
                                 SlotId = o.SlotId,
                                 AvailableSlots = o.AvailableSlots,
                                 ShiftStartDate = o.ShiftStartDate,
                                 ShiftEndDate = o.ShiftEndDate,
                                 ShiftStartTime = o.ShiftStartTime,
                                 ShiftEndTime = o.ShiftEndTime,
                                 ShiftHours = o.ShiftHours,
                                 NotifyHospital = o.NotifyHospital,
                                 HospitalNotifiedDateTime = o.HospitalNotifiedDateTime,
                                 ShiftType = o.ShiftType,
                                 BidStartDateTime = o.BidStartDateTime,
                                 BidEndDateTime = o.BidEndDateTime,
                                 Id = o.Id
                             },
                             HospitalName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             MedicalUnitName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var rotationSlotListDtos = await query.ToListAsync();

            return _rotationSlotsExcelExporter.ExportToFile(rotationSlotListDtos);

        }

        [AbpAuthorize(AppPermissions.Pages_RotationSlots)]
        public async Task<List<RotationSlotHospitalLookupTableDto>> GetAllHospitalForTableDropdown()
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            return await _hospitalLookUpRepository.GetAll()
                .Select(hospital => new RotationSlotHospitalLookupTableDto
                {
                    Id = hospital.Id,
                    DisplayName = hospital == null || hospital.Name == null ? "" : hospital.Name.ToString()
                }).ToListAsync();

        }

        [AbpAuthorize(AppPermissions.Pages_RotationSlots)]
        public async Task<List<RotationSlotMedicalUnitLookupTableDto>> GetAllMedicalUnitForTableDropdown()
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            return await _medicalUnitLookUpRepository.GetAll()
                .Select(medicalUnit => new RotationSlotMedicalUnitLookupTableDto
                {
                    Id = medicalUnit.Id,
                    DisplayName = medicalUnit == null || medicalUnit.Name == null ? "" : medicalUnit.Name.ToString()
                }).ToListAsync();

        }
        private async Task<string> GenerateUniqueSlotId(string preface = "")
        {
            var slotId = SlotIdGenerator.Generate(RotationSlotConsts.DefaultSlotIdLength, preface);
            while (_rotationSlotRepository.GetAll().Any(e => e.SlotId == slotId))
            {
                slotId = SlotIdGenerator.Generate(RotationSlotConsts.DefaultSlotIdLength, preface);
            }

            return slotId;
        }
    }
}