using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.TenantDepartments;
using inzibackend.Web.Controllers;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_TenantDepartments)]
    public class TenantDepartmentsController : inzibackendControllerBase
    {
        private readonly ITenantDepartmentsAppService _tenantDepartmentsAppService;

        public TenantDepartmentsController(ITenantDepartmentsAppService tenantDepartmentsAppService)
        {
            _tenantDepartmentsAppService = tenantDepartmentsAppService;

        }

        public ActionResult Index()
        {
            var model = new TenantDepartmentsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_TenantDepartments_Create, AppPermissions.Pages_TenantDepartments_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetTenantDepartmentForEditOutput getTenantDepartmentForEditOutput;

            if (id.HasValue)
            {
                getTenantDepartmentForEditOutput = await _tenantDepartmentsAppService.GetTenantDepartmentForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getTenantDepartmentForEditOutput = new GetTenantDepartmentForEditOutput
                {
                    TenantDepartment = new CreateOrEditTenantDepartmentDto()
                };
            }

            var viewModel = new CreateOrEditTenantDepartmentModalViewModel()
            {
                TenantDepartment = getTenantDepartmentForEditOutput.TenantDepartment,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewTenantDepartmentModal(Guid id)
        {
            var getTenantDepartmentForViewDto = await _tenantDepartmentsAppService.GetTenantDepartmentForView(id);

            var model = new TenantDepartmentViewModel()
            {
                TenantDepartment = getTenantDepartmentForViewDto.TenantDepartment
            };

            return PartialView("_ViewTenantDepartmentModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Cohorts_Create, AppPermissions.Pages_Cohorts_Edit)]
        public PartialViewResult TenantDepartmentLookupTableModal(Guid? id, string displayName)
        {
            var viewModel = new TenantDepartmentLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_TenantDepartmentLookupTableModal", viewModel);
        }
     
    }
}