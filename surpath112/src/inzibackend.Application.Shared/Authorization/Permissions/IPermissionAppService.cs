using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Authorization.Permissions.Dto;

namespace inzibackend.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
