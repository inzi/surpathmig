using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;

namespace inzibackend.Surpath
{
    public interface IWelcomemessagesAppService : IApplicationService
    {
        Task<PagedResultDto<GetWelcomemessageForViewDto>> GetAll(GetAllWelcomemessagesInput input);

        Task<GetWelcomemessageForEditOutput> GetWelcomemessageForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditWelcomemessageDto input);

        Task Delete(EntityDto input);
        Task<WelcomemessageDto> GetCurrent();

    }
}