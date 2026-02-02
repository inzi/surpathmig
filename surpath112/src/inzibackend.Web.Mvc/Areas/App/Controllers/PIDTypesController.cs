using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.PidTypes;
using inzibackend.Web.Controllers;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_PidTypes)]
    public class PidTypesController : inzibackendControllerBase
    {
        private readonly IPidTypesAppService _pidTypesAppService;

        public PidTypesController(IPidTypesAppService pidTypesAppService)
        {
            _pidTypesAppService = pidTypesAppService;

        }

        public ActionResult Index()
        {
            var model = new PidTypesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_PidTypes_Create, AppPermissions.Pages_PidTypes_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetPidTypeForEditOutput getPidTypeForEditOutput;

            if (id.HasValue)
            {
                getPidTypeForEditOutput = await _pidTypesAppService.GetPidTypeForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getPidTypeForEditOutput = new GetPidTypeForEditOutput
                {
                    PidType = new CreateOrEditPidTypeDto()
                };
                getPidTypeForEditOutput.PidType.CreatedOn = DateTime.UtcNow;
                getPidTypeForEditOutput.PidType.ModifiedOn = DateTime.UtcNow;
            }

            var viewModel = new CreateOrEditPidTypeModalViewModel()
            {
                PidType = getPidTypeForEditOutput.PidType,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewPidTypeModal(Guid id)
        {
            var getPidTypeForViewDto = await _pidTypesAppService.GetPidTypeForView(id);

            var model = new PidTypeViewModel()
            {
                PidType = getPidTypeForViewDto.PidType
            };

            return PartialView("_ViewPidTypeModal", model);
        }

    }
}