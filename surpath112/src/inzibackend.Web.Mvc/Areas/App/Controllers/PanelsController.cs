using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.Panels;
using inzibackend.Web.Controllers;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Panels)]
    public class PanelsController : inzibackendControllerBase
    {
        private readonly IPanelsAppService _panelsAppService;

        public PanelsController(IPanelsAppService panelsAppService)
        {
            _panelsAppService = panelsAppService;

        }

        public ActionResult Index()
        {
            var model = new PanelsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Panels_Create, AppPermissions.Pages_Administration_Panels_Edit)]
        public async Task<ActionResult> CreateOrEdit(Guid? id)
        {
            GetPanelForEditOutput getPanelForEditOutput;

            if (id.HasValue)
            {
                getPanelForEditOutput = await _panelsAppService.GetPanelForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getPanelForEditOutput = new GetPanelForEditOutput
                {
                    Panel = new CreateOrEditPanelDto()
                };
            }

            var viewModel = new CreateOrEditPanelViewModel()
            {
                Panel = getPanelForEditOutput.Panel,
                TestCategoryName = getPanelForEditOutput.TestCategoryName,
                PanelTestCategoryList = await _panelsAppService.GetAllTestCategoryForTableDropdown(),
            };

            return View(viewModel);
        }

        public async Task<ActionResult> ViewPanel(Guid id)
        {
            var getPanelForViewDto = await _panelsAppService.GetPanelForView(id);

            var model = new PanelViewModel()
            {
                Panel = getPanelForViewDto.Panel
                ,
                TestCategoryName = getPanelForViewDto.TestCategoryName

            };

            return View(model);
        }

    }
}