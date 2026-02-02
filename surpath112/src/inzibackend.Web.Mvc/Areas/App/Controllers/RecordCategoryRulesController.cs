using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.RecordCategoryRules;
using inzibackend.Web.Controllers;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_RecordCategoryRules)]
    public class RecordCategoryRulesController : inzibackendControllerBase
    {
        private readonly IRecordCategoryRulesAppService _recordCategoryRulesAppService;
        private readonly IRecordStatusesAppService _recordStatusesAppService;

        public RecordCategoryRulesController(IRecordCategoryRulesAppService recordCategoryRulesAppService, IRecordStatusesAppService recordStatusesAppService)
        {
            _recordCategoryRulesAppService = recordCategoryRulesAppService;
            _recordStatusesAppService = recordStatusesAppService;
        }

        public ActionResult Index()
        {
            var model = new RecordCategoryRulesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_RecordCategoryRules_Create, AppPermissions.Pages_RecordCategoryRules_Edit)]
        public async Task<ActionResult> CreateOrEdit(Guid? id)
        {
            GetRecordCategoryRuleForEditOutput getRecordCategoryRuleForEditOutput;

            if (id.HasValue)
            {
                getRecordCategoryRuleForEditOutput = await _recordCategoryRulesAppService.GetRecordCategoryRuleForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getRecordCategoryRuleForEditOutput = new GetRecordCategoryRuleForEditOutput
                {
                    RecordCategoryRule = new CreateOrEditRecordCategoryRuleDto()
                };
            }

            var viewModel = new CreateOrEditRecordCategoryRuleViewModel()
            {
                RecordCategoryRule = getRecordCategoryRuleForEditOutput.RecordCategoryRule,
                RecordStatusList = await _recordStatusesAppService.GetAllForDropdown()
            };

            return View(viewModel);
        }

        public async Task<ActionResult> ViewRecordCategoryRule(Guid id)
        {
            var getRecordCategoryRuleForViewDto = await _recordCategoryRulesAppService.GetRecordCategoryRuleForView(id);

            var model = new RecordCategoryRuleViewModel()
            {
                RecordCategoryRule = getRecordCategoryRuleForViewDto.RecordCategoryRule
            };

            return View(model);
        }

    }
}