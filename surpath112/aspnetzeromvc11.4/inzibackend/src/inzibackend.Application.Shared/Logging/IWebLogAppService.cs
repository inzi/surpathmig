using Abp.Application.Services;
using inzibackend.Dto;
using inzibackend.Logging.Dto;

namespace inzibackend.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
