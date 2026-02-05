using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;

namespace inzibackend.Surpath
{
    public interface IDrugsAppService : IApplicationService
    {
        Task<PagedResultDto<GetDrugForViewDto>> GetAll(GetAllDrugsInput input);

        Task<GetDrugForViewDto> GetDrugForView(Guid id);

        Task<GetDrugForEditOutput> GetDrugForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditDrugDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetDrugsToExcel(GetAllDrugsForExcelInput input);

    }
}