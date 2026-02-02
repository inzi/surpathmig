using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using System.Collections.Generic;
using System.Collections.Generic;

namespace inzibackend.Surpath
{
    public interface IRotationSlotsAppService : IApplicationService
    {
        Task<PagedResultDto<GetRotationSlotForViewDto>> GetAll(GetAllRotationSlotsInput input);

        Task<GetRotationSlotForViewDto> GetRotationSlotForView(int id);

        Task<GetRotationSlotForEditOutput> GetRotationSlotForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditRotationSlotDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetRotationSlotsToExcel(GetAllRotationSlotsForExcelInput input);

        Task<List<RotationSlotHospitalLookupTableDto>> GetAllHospitalForTableDropdown();

        Task<List<RotationSlotMedicalUnitLookupTableDto>> GetAllMedicalUnitForTableDropdown();

        Task Clone(EntityDto[] input);
        Task MultiDelete(EntityDto[] input);
    }
}