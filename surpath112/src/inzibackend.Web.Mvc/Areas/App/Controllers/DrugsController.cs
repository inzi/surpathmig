using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.Drugs;
using inzibackend.Web.Controllers;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Drugs)]
    public class DrugsController : inzibackendControllerBase
    {
        private readonly IDrugsAppService _drugsAppService;

        public DrugsController(IDrugsAppService drugsAppService)
        {
            _drugsAppService = drugsAppService;

        }

        public ActionResult Index()
        {
            var model = new DrugsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Drugs_Create, AppPermissions.Pages_Administration_Drugs_Edit)]
        public async Task<ActionResult> CreateOrEdit(Guid? id)
        {
            GetDrugForEditOutput getDrugForEditOutput;

            if (id.HasValue)
            {
                getDrugForEditOutput = await _drugsAppService.GetDrugForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getDrugForEditOutput = new GetDrugForEditOutput
                {
                    Drug = new CreateOrEditDrugDto()
                };
            }

            var viewModel = new CreateOrEditDrugViewModel()
            {
                Drug = getDrugForEditOutput.Drug,
            };

            return View(viewModel);
        }

        public async Task<ActionResult> ViewDrug(Guid id)
        {
            var getDrugForViewDto = await _drugsAppService.GetDrugForView(id);

            var model = new DrugViewModel()
            {
                Drug = getDrugForViewDto.Drug
            };

            return View(model);
        }

    }
}