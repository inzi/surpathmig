using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.TestCategories;
using inzibackend.Web.Controllers;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_TestCategories)]
    public class TestCategoriesController : inzibackendControllerBase
    {
        private readonly ITestCategoriesAppService _testCategoriesAppService;

        public TestCategoriesController(ITestCategoriesAppService testCategoriesAppService)
        {
            _testCategoriesAppService = testCategoriesAppService;

        }

        public ActionResult Index()
        {
            var model = new TestCategoriesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_TestCategories_Create, AppPermissions.Pages_TestCategories_Edit)]
        public async Task<ActionResult> CreateOrEdit(Guid? id)
        {
            GetTestCategoryForEditOutput getTestCategoryForEditOutput;

            if (id.HasValue)
            {
                getTestCategoryForEditOutput = await _testCategoriesAppService.GetTestCategoryForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getTestCategoryForEditOutput = new GetTestCategoryForEditOutput
                {
                    TestCategory = new CreateOrEditTestCategoryDto()
                };
            }

            var viewModel = new CreateOrEditTestCategoryViewModel()
            {
                TestCategory = getTestCategoryForEditOutput.TestCategory,
            };

            return View(viewModel);
        }

        public async Task<ActionResult> ViewTestCategory(Guid id)
        {
            var getTestCategoryForViewDto = await _testCategoriesAppService.GetTestCategoryForView(id);

            var model = new TestCategoryViewModel()
            {
                TestCategory = getTestCategoryForViewDto.TestCategory
            };

            return View(model);
        }

    }
}