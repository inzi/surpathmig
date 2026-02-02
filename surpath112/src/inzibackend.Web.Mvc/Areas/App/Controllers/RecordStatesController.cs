using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.RecordStates;
using inzibackend.Web.Controllers;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using inzibackend.Web.Areas.App.Models.Records;
using System.Linq;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_RecordStates)]
    public class RecordStatesController : inzibackendControllerBase
    {
        private readonly IRecordStatesAppService _recordStatesAppService;
        public RecordStatesController(IRecordStatesAppService recordStatesAppService)
        {
            _recordStatesAppService = recordStatesAppService;

        }

        public ActionResult Index()
        {
            var model = new RecordStatesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_RecordStates_Create, AppPermissions.Pages_RecordStates_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetRecordStateForEditOutput getRecordStateForEditOutput;

            if (id.HasValue)
            {
                getRecordStateForEditOutput = await _recordStatesAppService.GetRecordStateForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getRecordStateForEditOutput = new GetRecordStateForEditOutput
                {
                    RecordState = new CreateOrEditRecordStateDto()
                };
            }
            getRecordStateForEditOutput.RecordState.Notes = String.Empty; // so a note can be added and sent for notification
            var viewModel = new CreateOrEditRecordStateModalViewModel()
            {
                RecordState = getRecordStateForEditOutput.RecordState,
                Recordfilename = getRecordStateForEditOutput.Recordfilename,
                RecordCategoryName = getRecordStateForEditOutput.RecordCategoryName,
                UserName = getRecordStateForEditOutput.UserName,
                RecordStatusStatusName = getRecordStateForEditOutput.RecordStatusStatusName,
                RecordStateRecordStatusList = await _recordStatesAppService.GetAllRecordStatusForTableDropdown(),
                CreateOrEditRecordModalViewModel = new CreateOrEditRecordModalViewModel()

            };
            viewModel.RecordState.RecordDto = new RecordDto();


            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewRecordStateModal(Guid id)
        {
            var getRecordStateForViewDto = await _recordStatesAppService.GetRecordStateForView(id);

            var model = new RecordStateViewModel()
            {
                RecordState = getRecordStateForViewDto.RecordState
                ,
                Recordfilename = getRecordStateForViewDto.Recordfilename

                ,
                RecordCategoryName = getRecordStateForViewDto.RecordCategoryName

                ,
                UserName = getRecordStateForViewDto.UserName

                ,
                RecordStatusStatusName = getRecordStateForViewDto.RecordStatusStatusName

            };

            return PartialView("_ViewRecordStateModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_RecordStates_Create, AppPermissions.Pages_RecordStates_Edit)]
        public PartialViewResult RecordLookupTableModal(Guid? id, string displayName)
        {
            var viewModel = new RecordStateRecordLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_RecordStateRecordLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_RecordStates_Create, AppPermissions.Pages_RecordStates_Edit)]
        public PartialViewResult RecordCategoryLookupTableModal(Guid? id, string displayName, bool confirm)
        {
            var viewModel = new RecordStateRecordCategoryLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = "",
                confirm = confirm,
            };

            return PartialView("_RecordStateRecordCategoryLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_RecordStates_Create, AppPermissions.Pages_RecordStates_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new RecordStateUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_RecordStateUserLookupTableModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Surpath_Administration_Surpath_Compliance_Review)]
        public async Task<ActionResult> ReviewRecordState(Guid id)
        {
            GetRecordStateForEditOutput getRecordStateForEditOutput;

            getRecordStateForEditOutput = await _recordStatesAppService.GetRecordStateForReview(new EntityDto<Guid> { Id = (Guid)id });

            getRecordStateForEditOutput.RecordState.Notes = String.Empty; // so a note can be added and sent for notification
            var _statuses = await _recordStatesAppService.GetAllRecordStatusForTableDropdown(getRecordStateForEditOutput.TenantId);
            if (getRecordStateForEditOutput.IsSurpathOnly) _statuses = await _recordStatesAppService.GetAllServiceRecordStatusForTableDropdown();//_statuses.Where(s => s.IsSurpathServiceStatus == true).ToList();

            var viewModel = new CreateOrEditRecordStateModalViewModel()
            {
                RecordState = getRecordStateForEditOutput.RecordState,
                Recordfilename = getRecordStateForEditOutput.Recordfilename,
                RecordCategoryName = getRecordStateForEditOutput.RecordCategoryName,
                RecordRequirementName = getRecordStateForEditOutput.RecordRequirementName,
                UserName = getRecordStateForEditOutput.UserName,
                RecordStatusStatusName = getRecordStateForEditOutput.RecordStatusStatusName,
                RecordStateRecordStatusList = _statuses,


            };

            // ensure Model.RecordState.RecordDto.EffectiveDate is not null
            if (viewModel.RecordState.RecordDto == null)
            {
                Logger.Error("RecordState.RecordDto is null, creating empty one");
                viewModel.RecordState.RecordDto = new RecordDto();
            }

            return View(viewModel);
        }

    }
}