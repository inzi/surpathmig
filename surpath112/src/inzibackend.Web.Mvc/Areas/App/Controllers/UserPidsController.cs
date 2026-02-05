using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.UserPids;
using inzibackend.Web.Controllers;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_UserPids)]
    public class UserPidsController : inzibackendControllerBase
    {
        private readonly IUserPidsAppService _userPidsAppService;

        public UserPidsController(IUserPidsAppService userPidsAppService)
        {
            _userPidsAppService = userPidsAppService;

        }

        public ActionResult Index()
        {
            var model = new UserPidsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_UserPids_Create, AppPermissions.Pages_UserPids_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetUserPidForEditOutput getUserPidForEditOutput;

            if (id.HasValue)
            {
                getUserPidForEditOutput = await _userPidsAppService.GetUserPidForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getUserPidForEditOutput = new GetUserPidForEditOutput
                {
                    UserPid = new CreateOrEditUserPidDto()
                };
            }

            var viewModel = new CreateOrEditUserPidModalViewModel()
            {
                UserPid = getUserPidForEditOutput.UserPid,
                PidTypeName = getUserPidForEditOutput.PidTypeName,
                UserName = getUserPidForEditOutput.UserName,
                UserPidPidTypeList = await _userPidsAppService.GetAllPidTypeForTableDropdown(),

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewUserPidModal(Guid id)
        {
            var getUserPidForViewDto = await _userPidsAppService.GetUserPidForView(id);

            var model = new UserPidViewModel()
            {
                UserPid = getUserPidForViewDto.UserPid
                ,
                PidTypeName = getUserPidForViewDto.PidTypeName

                ,
                UserName = getUserPidForViewDto.UserName

            };

            return PartialView("_ViewUserPidModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_UserPids_Create, AppPermissions.Pages_UserPids_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new UserPidUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_UserPidUserLookupTableModal", viewModel);
        }

    }
}