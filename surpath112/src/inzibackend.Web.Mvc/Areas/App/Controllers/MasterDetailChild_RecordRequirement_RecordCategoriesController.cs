using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.RecordCategories;
using inzibackend.Web.Controllers;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_RecordCategories)]
    public class MasterDetailChild_RecordRequirement_RecordCategoriesController : inzibackendControllerBase
    {
        private readonly IRecordCategoriesAppService _recordCategoriesAppService;

        public MasterDetailChild_RecordRequirement_RecordCategoriesController(IRecordCategoriesAppService recordCategoriesAppService)
        {
            _recordCategoriesAppService = recordCategoriesAppService;
        }

        public ActionResult Index(Guid recordRequirementId)
        {
            var model = new MasterDetailChild_RecordRequirement_RecordCategoriesViewModel
            {
                FilterText = "",
                RecordRequirementId = recordRequirementId
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_RecordCategories_Create, AppPermissions.Pages_RecordCategories_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetRecordCategoryForEditOutput getRecordCategoryForEditOutput;

            if (id.HasValue)
            {
                getRecordCategoryForEditOutput = await _recordCategoriesAppService.GetRecordCategoryForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getRecordCategoryForEditOutput = new GetRecordCategoryForEditOutput
                {
                    RecordCategory = new CreateOrEditRecordCategoryDto()
                };
            }

            var viewModel = new MasterDetailChild_RecordRequirement_CreateOrEditRecordCategoryModalViewModel()
            {
                RecordCategory = getRecordCategoryForEditOutput.RecordCategory,
                RecordCategoryRuleName = getRecordCategoryForEditOutput.RecordCategoryRuleName,
                RecordCategoryRecordCategoryRuleList = await _recordCategoriesAppService.GetAllRecordCategoryRuleForTableDropdown(),
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewRecordCategoryModal(Guid id)
        {
            var getRecordCategoryForViewDto = await _recordCategoriesAppService.GetRecordCategoryForView(id);

            var model = new MasterDetailChild_RecordRequirement_RecordCategoryViewModel()
            {
                RecordCategory = getRecordCategoryForViewDto.RecordCategory
                ,
                RecordCategoryRuleName = getRecordCategoryForViewDto.RecordCategoryRuleName

            };

            return PartialView("_ViewRecordCategoryModal", model);
        }

    }
}