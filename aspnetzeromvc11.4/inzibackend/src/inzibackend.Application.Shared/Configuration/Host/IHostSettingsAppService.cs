using System.Threading.Tasks;
using Abp.Application.Services;
using inzibackend.Configuration.Host.Dto;

namespace inzibackend.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
