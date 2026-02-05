using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.EntityChanges.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace inzibackend.EntityChanges;

public interface IEntityChangeAppService : IApplicationService
{
    Task<ListResultDto<EntityAndPropertyChangeListDto>> GetEntityChangesByEntity(GetEntityChangesByEntityInput input);
}

