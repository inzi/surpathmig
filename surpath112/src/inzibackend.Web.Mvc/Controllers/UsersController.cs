using Abp.AspNetCore.Mvc.Authorization;
using inzibackend.Authorization;
using inzibackend.Storage;
using Abp.BackgroundJobs;
using Abp.Authorization;

namespace inzibackend.Web.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UsersController : UsersControllerBase
    {
        public UsersController(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager)
            : base(binaryObjectManager, backgroundJobManager)
        {
        }
    }
}