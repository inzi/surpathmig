using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.CohortUsers;
using inzibackend.Web.Controllers;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_CohortUsers)]
    public class MasterDetailChild_Cohort_CohortUsersController : inzibackendControllerBase
    {
        private readonly ICohortUsersAppService _cohortUsersAppService;

        public MasterDetailChild_Cohort_CohortUsersController(ICohortUsersAppService cohortUsersAppService)
        {
            _cohortUsersAppService = cohortUsersAppService;
        }

        public ActionResult Index(Guid cohortId)
        {
            var model = new MasterDetailChild_Cohort_CohortUsersViewModel
            {
                FilterText = "",
                CohortId = cohortId
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_CohortUsers_Create, AppPermissions.Pages_CohortUsers_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetCohortUserForEditOutput getCohortUserForEditOutput;

            if (id.HasValue)
            {
                getCohortUserForEditOutput = await _cohortUsersAppService.GetCohortUserForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getCohortUserForEditOutput = new GetCohortUserForEditOutput
                {
                    CohortUser = new CreateOrEditCohortUserDto()
                };
            }

            var viewModel = new MasterDetailChild_Cohort_CreateOrEditCohortUserModalViewModel()
            {
                CohortUser = getCohortUserForEditOutput.CohortUser,
                UserName = getCohortUserForEditOutput.UserName,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewCohortUserModal(Guid id)
        {
            var getCohortUserForViewDto = await _cohortUsersAppService.GetCohortUserForView(id);

            var model = new MasterDetailChild_Cohort_CohortUserViewModel()
            {
                CohortUser = getCohortUserForViewDto.CohortUser
                ,
                UserName = getCohortUserForViewDto.UserName

            };

            return PartialView("_ViewCohortUserModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_CohortUsers_Create, AppPermissions.Pages_CohortUsers_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new MasterDetailChild_Cohort_CohortUserUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_CohortUserUserLookupTableModal", viewModel);
        }

    }
}