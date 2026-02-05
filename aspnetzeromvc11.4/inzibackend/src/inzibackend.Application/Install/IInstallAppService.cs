using System.Threading.Tasks;
using Abp.Application.Services;
using inzibackend.Install.Dto;

namespace inzibackend.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}