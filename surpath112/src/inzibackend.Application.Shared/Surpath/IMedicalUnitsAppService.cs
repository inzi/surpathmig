using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using System.Collections.Generic;

namespace inzibackend.Surpath
{
    public interface IMedicalUnitsAppService : IApplicationService
    {
        Task<PagedResultDto<GetMedicalUnitForViewDto>> GetAll(GetAllMedicalUnitsInput input);

        Task<GetMedicalUnitForViewDto> GetMedicalUnitForView(int id);

        Task<GetMedicalUnitForEditOutput> GetMedicalUnitForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditMedicalUnitDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetMedicalUnitsToExcel(GetAllMedicalUnitsForExcelInput input);

        Task<List<MedicalUnitHospitalLookupTableDto>> GetAllHospitalForTableDropdown();

    }
}