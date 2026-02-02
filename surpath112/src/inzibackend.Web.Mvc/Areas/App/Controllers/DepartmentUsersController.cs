using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.DepartmentUsers;
using inzibackend.Web.Controllers;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_DepartmentUsers)]
    public class DepartmentUsersController : inzibackendControllerBase
    {
        private readonly IDepartmentUsersAppService _departmentUsersAppService;

        public DepartmentUsersController(IDepartmentUsersAppService departmentUsersAppService)
        {
            _departmentUsersAppService = departmentUsersAppService;

        }

        public ActionResult Index()
        {
            var model = new DepartmentUsersViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_DepartmentUsers_Create, AppPermissions.Pages_DepartmentUsers_Edit)]
        public async Task<ActionResult> CreateOrEdit(Guid? id)
        {
            GetDepartmentUserForEditOutput getDepartmentUserForEditOutput;

            if (id.HasValue)
            {
                getDepartmentUserForEditOutput = await _departmentUsersAppService.GetDepartmentUserForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getDepartmentUserForEditOutput = new GetDepartmentUserForEditOutput
                {
                    DepartmentUser = new CreateOrEditDepartmentUserDto()
                };
            }

            var viewModel = new CreateOrEditDepartmentUserViewModel()
            {
                DepartmentUser = getDepartmentUserForEditOutput.DepartmentUser,
                UserName = getDepartmentUserForEditOutput.UserName,
                TenantDepartmentName = getDepartmentUserForEditOutput.TenantDepartmentName,
            };

            return View(viewModel);
        }

        public async Task<ActionResult> ViewDepartmentUser(Guid id)
        {
            var getDepartmentUserForViewDto = await _departmentUsersAppService.GetDepartmentUserForView(id);

            var model = new DepartmentUserViewModel()
            {
                DepartmentUser = getDepartmentUserForViewDto.DepartmentUser
                ,
                UserName = getDepartmentUserForViewDto.UserName

                ,
                TenantDepartmentName = getDepartmentUserForViewDto.TenantDepartmentName

            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_DepartmentUsers_Create, AppPermissions.Pages_DepartmentUsers_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new DepartmentUserUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_DepartmentUserUserLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_DepartmentUsers_Create, AppPermissions.Pages_DepartmentUsers_Edit)]
        public PartialViewResult TenantDepartmentLookupTableModal(Guid? id, string displayName)
        {
            var viewModel = new DepartmentUserTenantDepartmentLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_DepartmentUserTenantDepartmentLookupTableModal", viewModel);
        }

    }
}