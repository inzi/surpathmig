using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.Welcomemessages;
using inzibackend.Web.Controllers;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Welcomemessages)]
    public class WelcomemessagesController : inzibackendControllerBase
    {
        private readonly IWelcomemessagesAppService _welcomemessagesAppService;

        public WelcomemessagesController(IWelcomemessagesAppService welcomemessagesAppService)
        {
            _welcomemessagesAppService = welcomemessagesAppService;

        }

        public ActionResult Index()
        {
            var model = new WelcomemessagesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Welcomemessages_Create, AppPermissions.Pages_Administration_Welcomemessages_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetWelcomemessageForEditOutput getWelcomemessageForEditOutput;

            if (id.HasValue)
            {
                getWelcomemessageForEditOutput = await _welcomemessagesAppService.GetWelcomemessageForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getWelcomemessageForEditOutput = new GetWelcomemessageForEditOutput
                {
                    Welcomemessage = new CreateOrEditWelcomemessageDto()
                };
                getWelcomemessageForEditOutput.Welcomemessage.DisplayStart = DateTime.Now;
                getWelcomemessageForEditOutput.Welcomemessage.DisplayEnd = DateTime.Now;
            }

            var viewModel = new CreateOrEditWelcomemessageModalViewModel()
            {
                Welcomemessage = getWelcomemessageForEditOutput.Welcomemessage,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

    }
}