using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.DrugTestCategories;
using inzibackend.Web.Controllers;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_DrugTestCategories)]
    public class DrugTestCategoriesController : inzibackendControllerBase
    {
        private readonly IDrugTestCategoriesAppService _drugTestCategoriesAppService;

        public DrugTestCategoriesController(IDrugTestCategoriesAppService drugTestCategoriesAppService)
        {
            _drugTestCategoriesAppService = drugTestCategoriesAppService;

        }

        public ActionResult Index()
        {
            var model = new DrugTestCategoriesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_DrugTestCategories_Create, AppPermissions.Pages_Administration_DrugTestCategories_Edit)]
        public async Task<ActionResult> CreateOrEdit(Guid? id)
        {
            GetDrugTestCategoryForEditOutput getDrugTestCategoryForEditOutput;

            if (id.HasValue)
            {
                getDrugTestCategoryForEditOutput = await _drugTestCategoriesAppService.GetDrugTestCategoryForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getDrugTestCategoryForEditOutput = new GetDrugTestCategoryForEditOutput
                {
                    DrugTestCategory = new CreateOrEditDrugTestCategoryDto()
                };
            }

            var viewModel = new CreateOrEditDrugTestCategoryViewModel()
            {
                DrugTestCategory = getDrugTestCategoryForEditOutput.DrugTestCategory,
                DrugName = getDrugTestCategoryForEditOutput.DrugName,
                PanelName = getDrugTestCategoryForEditOutput.PanelName,
                TestCategoryName = getDrugTestCategoryForEditOutput.TestCategoryName,
                DrugTestCategoryTestCategoryList = await _drugTestCategoriesAppService.GetAllTestCategoryForTableDropdown(),
            };

            return View(viewModel);
        }

        public async Task<ActionResult> ViewDrugTestCategory(Guid id)
        {
            var getDrugTestCategoryForViewDto = await _drugTestCategoriesAppService.GetDrugTestCategoryForView(id);

            var model = new DrugTestCategoryViewModel()
            {
                DrugTestCategory = getDrugTestCategoryForViewDto.DrugTestCategory
                ,
                DrugName = getDrugTestCategoryForViewDto.DrugName

                ,
                PanelName = getDrugTestCategoryForViewDto.PanelName

                ,
                TestCategoryName = getDrugTestCategoryForViewDto.TestCategoryName

            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_DrugTestCategories_Create, AppPermissions.Pages_Administration_DrugTestCategories_Edit)]
        public PartialViewResult DrugLookupTableModal(Guid? id, string displayName)
        {
            var viewModel = new DrugTestCategoryDrugLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_DrugTestCategoryDrugLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_Administration_DrugTestCategories_Create, AppPermissions.Pages_Administration_DrugTestCategories_Edit)]
        public PartialViewResult PanelLookupTableModal(Guid? id, string displayName)
        {
            var viewModel = new DrugTestCategoryPanelLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_DrugTestCategoryPanelLookupTableModal", viewModel);
        }

    }
}