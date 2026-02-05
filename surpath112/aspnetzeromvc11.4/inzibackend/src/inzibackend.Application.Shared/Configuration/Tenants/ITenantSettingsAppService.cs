using System.Threading.Tasks;
using Abp.Application.Services;
using inzibackend.Configuration.Tenants.Dto;

namespace inzibackend.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearLogo();

        Task ClearCustomCss();
    }
}
