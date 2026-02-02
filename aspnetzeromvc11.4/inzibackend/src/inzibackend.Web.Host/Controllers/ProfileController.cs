using Abp.AspNetCore.Mvc.Authorization;
using inzibackend.Authorization.Users.Profile;
using inzibackend.Graphics;
using inzibackend.Storage;

namespace inzibackend.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ProfileController : ProfileControllerBase
    {
        public ProfileController(
            ITempFileCacheManager tempFileCacheManager,
            IProfileAppService profileAppService,
            IImageFormatValidator imageFormatValidator) :
            base(tempFileCacheManager, profileAppService, imageFormatValidator)
        {
        }
    }
}