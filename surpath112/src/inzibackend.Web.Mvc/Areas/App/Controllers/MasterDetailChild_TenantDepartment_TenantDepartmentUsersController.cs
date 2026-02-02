using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.TenantDepartmentUsers;
using inzibackend.Web.Controllers;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_TenantDepartmentUsers)]
    public class MasterDetailChild_TenantDepartment_TenantDepartmentUsersController : inzibackendControllerBase
    {
        private readonly ITenantDepartmentUsersAppService _tenantDepartmentUsersAppService;

        public MasterDetailChild_TenantDepartment_TenantDepartmentUsersController(ITenantDepartmentUsersAppService tenantDepartmentUsersAppService)
        {
            _tenantDepartmentUsersAppService = tenantDepartmentUsersAppService;
        }

        public ActionResult Index(Guid tenantDepartmentId)
        {
            var model = new MasterDetailChild_TenantDepartment_TenantDepartmentUsersViewModel
            {
                FilterText = "",
                TenantDepartmentId = tenantDepartmentId
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_TenantDepartmentUsers_Create, AppPermissions.Pages_TenantDepartmentUsers_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetTenantDepartmentUserForEditOutput getTenantDepartmentUserForEditOutput;

            if (id.HasValue)
            {
                getTenantDepartmentUserForEditOutput = await _tenantDepartmentUsersAppService.GetTenantDepartmentUserForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getTenantDepartmentUserForEditOutput = new GetTenantDepartmentUserForEditOutput
                {
                    TenantDepartmentUser = new CreateOrEditTenantDepartmentUserDto()
                };
            }

            var viewModel = new MasterDetailChild_TenantDepartment_CreateOrEditTenantDepartmentUserModalViewModel()
            {
                TenantDepartmentUser = getTenantDepartmentUserForEditOutput.TenantDepartmentUser,
                UserName = getTenantDepartmentUserForEditOutput.UserName,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewTenantDepartmentUserModal(Guid id)
        {
            var getTenantDepartmentUserForViewDto = await _tenantDepartmentUsersAppService.GetTenantDepartmentUserForView(id);

            var model = new MasterDetailChild_TenantDepartment_TenantDepartmentUserViewModel()
            {
                TenantDepartmentUser = getTenantDepartmentUserForViewDto.TenantDepartmentUser
                ,
                UserName = getTenantDepartmentUserForViewDto.UserName

            };

            return PartialView("_ViewTenantDepartmentUserModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_TenantDepartmentUsers_Create, AppPermissions.Pages_TenantDepartmentUsers_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new MasterDetailChild_TenantDepartment_TenantDepartmentUserUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_TenantDepartmentUserUserLookupTableModal", viewModel);
        }

    }
}