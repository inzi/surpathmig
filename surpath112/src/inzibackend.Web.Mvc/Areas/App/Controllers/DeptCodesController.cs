using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.DeptCodes;
using inzibackend.Web.Controllers;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_DeptCodes)]
    public class DeptCodesController : inzibackendControllerBase
    {
        private readonly IDeptCodesAppService _deptCodesAppService;

        public DeptCodesController(IDeptCodesAppService deptCodesAppService)
        {
            _deptCodesAppService = deptCodesAppService;

        }

        public ActionResult Index()
        {
            var model = new DeptCodesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_DeptCodes_Create, AppPermissions.Pages_DeptCodes_Edit)]
        public async Task<ActionResult> CreateOrEdit(Guid? id)
        {
            GetDeptCodeForEditOutput getDeptCodeForEditOutput;

            if (id.HasValue)
            {
                getDeptCodeForEditOutput = await _deptCodesAppService.GetDeptCodeForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getDeptCodeForEditOutput = new GetDeptCodeForEditOutput
                {
                    DeptCode = new CreateOrEditDeptCodeDto()
                };
            }

            var viewModel = new CreateOrEditDeptCodeViewModel()
            {
                DeptCode = getDeptCodeForEditOutput.DeptCode,
                CodeTypeName = getDeptCodeForEditOutput.CodeTypeName,
                TenantDepartmentName = getDeptCodeForEditOutput.TenantDepartmentName,
                DeptCodeCodeTypeList = await _deptCodesAppService.GetAllCodeTypeForTableDropdown(),
            };

            return View(viewModel);
        }

        public async Task<ActionResult> ViewDeptCode(Guid id)
        {
            var getDeptCodeForViewDto = await _deptCodesAppService.GetDeptCodeForView(id);

            var model = new DeptCodeViewModel()
            {
                DeptCode = getDeptCodeForViewDto.DeptCode
                ,
                CodeTypeName = getDeptCodeForViewDto.CodeTypeName

                ,
                TenantDepartmentName = getDeptCodeForViewDto.TenantDepartmentName

            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_DeptCodes_Create, AppPermissions.Pages_DeptCodes_Edit)]
        public PartialViewResult TenantDepartmentLookupTableModal(Guid? id, string displayName)
        {
            var viewModel = new DeptCodeTenantDepartmentLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_DeptCodeTenantDepartmentLookupTableModal", viewModel);
        }

    }
}