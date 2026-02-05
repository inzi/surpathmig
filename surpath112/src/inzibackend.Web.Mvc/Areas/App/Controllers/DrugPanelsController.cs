using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.DrugPanels;
using inzibackend.Web.Controllers;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_DrugPanels)]
    public class DrugPanelsController : inzibackendControllerBase
    {
        private readonly IDrugPanelsAppService _drugPanelsAppService;

        public DrugPanelsController(IDrugPanelsAppService drugPanelsAppService)
        {
            _drugPanelsAppService = drugPanelsAppService;

        }

        public ActionResult Index()
        {
            var model = new DrugPanelsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_DrugPanels_Create, AppPermissions.Pages_Administration_DrugPanels_Edit)]
        public async Task<ActionResult> CreateOrEdit(Guid? id)
        {
            GetDrugPanelForEditOutput getDrugPanelForEditOutput;

            if (id.HasValue)
            {
                getDrugPanelForEditOutput = await _drugPanelsAppService.GetDrugPanelForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getDrugPanelForEditOutput = new GetDrugPanelForEditOutput
                {
                    DrugPanel = new CreateOrEditDrugPanelDto()
                };
            }

            var viewModel = new CreateOrEditDrugPanelViewModel()
            {
                DrugPanel = getDrugPanelForEditOutput.DrugPanel,
                DrugName = getDrugPanelForEditOutput.DrugName,
                PanelName = getDrugPanelForEditOutput.PanelName,
            };

            return View(viewModel);
        }

        public async Task<ActionResult> ViewDrugPanel(Guid id)
        {
            var getDrugPanelForViewDto = await _drugPanelsAppService.GetDrugPanelForView(id);

            var model = new DrugPanelViewModel()
            {
                DrugPanel = getDrugPanelForViewDto.DrugPanel
                ,
                DrugName = getDrugPanelForViewDto.DrugName

                ,
                PanelName = getDrugPanelForViewDto.PanelName

            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_DrugPanels_Create, AppPermissions.Pages_Administration_DrugPanels_Edit)]
        public PartialViewResult DrugLookupTableModal(Guid? id, string displayName)
        {
            var viewModel = new DrugPanelDrugLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_DrugPanelDrugLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_Administration_DrugPanels_Create, AppPermissions.Pages_Administration_DrugPanels_Edit)]
        public PartialViewResult PanelLookupTableModal(Guid? id, string displayName)
        {
            var viewModel = new DrugPanelPanelLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_DrugPanelPanelLookupTableModal", viewModel);
        }

    }
}