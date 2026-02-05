using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.BackgroundJobs;
using inzibackend.Authorization;
using inzibackend.Authorization.Users.Importing;
using inzibackend.DataImporting.Excel;
using inzibackend.Storage;
using inzibackend.Storage.FileValidator;

namespace inzibackend.Web.Controllers;

[AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
public class UsersController(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager, IFileValidatorManager fileValidatorManager)
    : ExcelImportControllerBase(binaryObjectManager, backgroundJobManager, fileValidatorManager)
{
    public override string ImportExcelPermission => AppPermissions.Pages_Administration_Users_Create;

    public override async Task EnqueueExcelImportJobAsync(ImportFromExcelJobArgs args)
    {
        await BackgroundJobManager.EnqueueAsync<ImportUsersToExcelJob, ImportFromExcelJobArgs>(args);
    }
}