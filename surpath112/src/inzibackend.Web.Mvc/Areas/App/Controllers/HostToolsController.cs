using Abp.AspNetCore.Mvc.Authorization;
using inzibackend.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using inzibackend.Storage;
using inzibackend.Surpath;
using inzibackend.Web.Controllers;
using Abp.UI;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration)]
    public class HostToolsController : inzibackendControllerBase
    {
        private readonly IRecordsAppService _recordsAppService;

        public HostToolsController(IRecordsAppService recordsAppService)
        {
            _recordsAppService = recordsAppService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ManualDocumentUpload()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> UploadDocument(IFormFile file, string documentType)
        {
            try
            {
                var recordId = await _recordsAppService.ManualDocumentUpload(file, documentType);
                return Json(new { success = true, message = L("FileUploaded"), recordId = recordId });
            }
            catch (UserFriendlyException ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}