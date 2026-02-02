using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;

namespace inzibackend.Surpath
{
    public interface IPidTypesAppService : IApplicationService
    {
        Task<PagedResultDto<GetPidTypeForViewDto>> GetAll(GetAllPidTypesInput input);

        Task<GetPidTypeForViewDto> GetPidTypeForView(Guid id);

        Task<GetPidTypeForEditOutput> GetPidTypeForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditPidTypeDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetPidTypesToExcel(GetAllPidTypesForExcelInput input);

    }
}