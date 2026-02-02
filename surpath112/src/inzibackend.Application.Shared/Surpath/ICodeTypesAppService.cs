using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;

namespace inzibackend.Surpath
{
    public interface ICodeTypesAppService : IApplicationService
    {
        Task<PagedResultDto<GetCodeTypeForViewDto>> GetAll(GetAllCodeTypesInput input);

        Task<GetCodeTypeForViewDto> GetCodeTypeForView(Guid id);

        Task<GetCodeTypeForEditOutput> GetCodeTypeForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditCodeTypeDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetCodeTypesToExcel(GetAllCodeTypesForExcelInput input);

    }
}