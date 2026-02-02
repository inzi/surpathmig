using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;

namespace inzibackend.Surpath
{
    public interface IHospitalsAppService : IApplicationService
    {
        Task<PagedResultDto<GetHospitalForViewDto>> GetAll(GetAllHospitalsInput input);

        Task<GetHospitalForViewDto> GetHospitalForView(int id);

        Task<GetHospitalForEditOutput> GetHospitalForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditHospitalDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetHospitalsToExcel(GetAllHospitalsForExcelInput input);

    }
}