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
    public class RecordCategoriesController : inzibackendControllerBase
    {
        private readonly IRecordCategoriesAppService _recordCategoriesAppService;
        private readonly IRecordRequirementsAppService _recordRequirementsAppService;

        public RecordCategoriesController(IRecordCategoriesAppService recordCategoriesAppService, IRecordRequirementsAppService recordRequirementsAppService)
        {
            _recordCategoriesAppService = recordCategoriesAppService;
            _recordRequirementsAppService = recordRequirementsAppService;

        }

        public ActionResult Index()
        {
            var model = new RecordCategoriesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_RecordCategories_Create, AppPermissions.Pages_RecordCategories_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id, Guid? reqid, bool nosave = false)
        {
            GetRecordCategoryForEditOutput getRecordCategoryForEditOutput;
            GetRecordRequirementForEditOutput getRecordRequirementForEditOutput = new GetRecordRequirementForEditOutput();

            if (reqid.HasValue)
            {
                getRecordRequirementForEditOutput = await _recordRequirementsAppService.GetRecordRequirementForEdit(new EntityDto<Guid> { Id = (Guid)reqid });
            }

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

            if (getRecordRequirementForEditOutput.RecordRequirement.Id.HasValue)
            {
                getRecordCategoryForEditOutput.RecordRequirementName = getRecordRequirementForEditOutput.RecordRequirement.Name;
                getRecordCategoryForEditOutput.RecordCategory.RecordRequirementId = getRecordRequirementForEditOutput.RecordRequirement.Id.Value;
            }

            var viewModel = new CreateOrEditRecordCategoryModalViewModel()
            {
                RecordCategory = getRecordCategoryForEditOutput.RecordCategory,
                RecordRequirementName = getRecordCategoryForEditOutput.RecordRequirementName,
                RecordCategoryRuleName = getRecordCategoryForEditOutput.RecordCategoryRuleName,
                RecordCategoryRecordCategoryRuleList = await _recordCategoriesAppService.GetAllRecordCategoryRuleForTableDropdown(),
                HideRequirementLookup = getRecordRequirementForEditOutput.RecordRequirement.Id.HasValue

            };
            ViewBag.nosave = nosave;
            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewRecordCategoryModal(Guid id)
        {
            var getRecordCategoryForViewDto = await _recordCategoriesAppService.GetRecordCategoryForView(id);

            var model = new RecordCategoryViewModel()
            {
                RecordCategory = getRecordCategoryForViewDto.RecordCategory
                ,
                RecordRequirementName = getRecordCategoryForViewDto.RecordRequirementName

                ,
                RecordCategoryRuleName = getRecordCategoryForViewDto.RecordCategoryRuleName

            };

            return PartialView("_ViewRecordCategoryModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_RecordCategories_Create, AppPermissions.Pages_RecordCategories_Edit)]
        public PartialViewResult RecordRequirementLookupTableModal(Guid? id, string displayName)
        {
            var viewModel = new RecordCategoryRecordRequirementLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_RecordCategoryRecordRequirementLookupTableModal", viewModel);
        }

    }
}