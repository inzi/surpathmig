using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.RecordStatuses;
using inzibackend.Web.Controllers;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_RecordStatuses)]
    public class RecordStatusesController : inzibackendControllerBase
    {
        private readonly IRecordStatusesAppService _recordStatusesAppService;

        public RecordStatusesController(IRecordStatusesAppService recordStatusesAppService)
        {
            _recordStatusesAppService = recordStatusesAppService;

        }

        public ActionResult Index()
        {
            var model = new RecordStatusesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_RecordStatuses_Create, AppPermissions.Pages_RecordStatuses_Edit)]
        public async Task<ActionResult> CreateOrEdit(Guid? id)
        {
            try
            {
                GetRecordStatusForEditOutput getRecordStatusForEditOutput;

                if (id.HasValue)
                {
                    var _entId = new EntityDto<Guid> { Id = (Guid)id };
                    getRecordStatusForEditOutput = await _recordStatusesAppService.GetRecordStatusForEdit(_entId);
                }
                else
                {
                    getRecordStatusForEditOutput = new GetRecordStatusForEditOutput
                    {
                        RecordStatus = new CreateOrEditRecordStatusDto()
                    };
                }

                var viewModel = new CreateOrEditRecordStatusViewModel()
                {
                    RecordStatus = getRecordStatusForEditOutput.RecordStatus,
                    TenantDepartmentName = getRecordStatusForEditOutput.TenantDepartmentName,
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Logger.Error("An error occurred in CreateOrEdit method", ex);
                throw;
            }
        }

        public async Task<ActionResult> ViewRecordStatus(Guid id)
        {
            var getRecordStatusForViewDto = await _recordStatusesAppService.GetRecordStatusForView(id);

            var model = new RecordStatusViewModel()
            {
                RecordStatus = getRecordStatusForViewDto.RecordStatus
                ,
                TenantDepartmentName = getRecordStatusForViewDto.TenantDepartmentName

            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_RecordStatuses_Create, AppPermissions.Pages_RecordStatuses_Edit)]
        public PartialViewResult TenantDepartmentLookupTableModal(Guid? id, string displayName)
        {
            var viewModel = new RecordStatusTenantDepartmentLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_RecordStatusTenantDepartmentLookupTableModal", viewModel);
        }

    }
}