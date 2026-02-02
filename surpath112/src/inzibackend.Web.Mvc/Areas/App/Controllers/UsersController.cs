using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using inzibackend.Authorization;
using inzibackend.Authorization.Permissions;
using inzibackend.Authorization.Permissions.Dto;
using inzibackend.Authorization.Roles;
using inzibackend.Authorization.Roles.Dto;
using inzibackend.Authorization.Users;
using inzibackend.Authorization.Users.Dto;
using inzibackend.Security;
using inzibackend.Web.Areas.App.Models.Roles;
using inzibackend.Web.Areas.App.Models.Users;
using inzibackend.Web.Controllers;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize]
    public class UsersController : inzibackendControllerBase
    {
        private readonly IUserAppService _userAppService;
        private readonly UserManager _userManager;
        private readonly IUserLoginAppService _userLoginAppService;
        private readonly IRoleAppService _roleAppService;
        private readonly IPermissionAppService _permissionAppService;
        private readonly IPasswordComplexitySettingStore _passwordComplexitySettingStore;
        private readonly IOptions<UserOptions> _userOptions;
        private readonly IUserTransferAppService _userTransferAppService;
        private readonly ICohortsAppService _cohortsAppService;

        public UsersController(
            IUserAppService userAppService,
            UserManager userManager,
            IUserLoginAppService userLoginAppService,
            IRoleAppService roleAppService,
            IPermissionAppService permissionAppService,
            IPasswordComplexitySettingStore passwordComplexitySettingStore,
            IOptions<UserOptions> userOptions,
            IUserTransferAppService userTransferAppService,
            ICohortsAppService cohortsAppService)
        {
            _userAppService = userAppService;
            _userManager = userManager;
            _userLoginAppService = userLoginAppService;
            _roleAppService = roleAppService;
            _permissionAppService = permissionAppService;
            _passwordComplexitySettingStore = passwordComplexitySettingStore;
            _userOptions = userOptions;
            _userTransferAppService = userTransferAppService;
            _cohortsAppService = cohortsAppService;
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
        public async Task<ActionResult> Index()
        {
            var roles = new List<ComboboxItemDto>();

            if (await IsGrantedAsync(AppPermissions.Pages_Administration_Roles))
            {
                var getRolesOutput = await _roleAppService.GetRoles(new GetRolesInput());
                roles = getRolesOutput.Items.Select(r => new ComboboxItemDto(r.Id.ToString(), r.DisplayName)).ToList();
            }

            roles.Insert(0, new ComboboxItemDto("", L("FilterByRole")));

            var permissions = _permissionAppService.GetAllPermissions().Items.ToList();

            var model = new UsersViewModel
            {
                FilterText = Request.Query["filterText"],
                Roles = roles,
                Permissions = ObjectMapper.Map<List<FlatPermissionDto>>(permissions).OrderBy(p => p.DisplayName)
                    .ToList(),
                OnlyLockedUsers = false
            };

            return View(model);
        }

        [AbpMvcAuthorize(
            AppPermissions.Pages_Administration_Users,
            AppPermissions.Pages_Administration_Users_Create,
            AppPermissions.Pages_Administration_Users_Edit
        )]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
            var output = await _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
            var viewModel = ObjectMapper.Map<CreateOrEditUserModalViewModel>(output);
            viewModel.PasswordComplexitySetting = await _passwordComplexitySettingStore.GetSettingsAsync();
            viewModel.AllowedUserNameCharacters = _userOptions.Value.AllowedUserNameCharacters;

            return PartialView("_CreateOrEditModal", viewModel);
        }

        [AbpMvcAuthorize(
            AppPermissions.Pages_Administration_Users,
            AppPermissions.Pages_Administration_Users_ChangePermissions
        )]
        public async Task<PartialViewResult> PermissionsModal(long id)
        {
            var output = await _userAppService.GetUserPermissionsForEdit(new EntityDto<long>(id));
            var viewModel = ObjectMapper.Map<UserPermissionsEditViewModel>(output);
            viewModel.User = await _userManager.GetUserByIdAsync(id);
            ;
            return PartialView("_PermissionsModal", viewModel);
        }

        public ActionResult LoginAttempts()
        {
            var loginResultTypes = Enum.GetNames(typeof(AbpLoginResultType))
                .Select(e => new ComboboxItemDto(e, L("AbpLoginResultType_" + e)))
                .ToList();

            loginResultTypes.Insert(0, new ComboboxItemDto("", L("All")));

            return View("LoginAttempts", new UserLoginAttemptsViewModel()
            {
                LoginAttemptResults = loginResultTypes
            });
        }

        #region User Transfer Wizard

        [AbpMvcAuthorize(AppPermissions.Pages_CohortUsers_Transfer)]
        public async Task<PartialViewResult> TransferWizardModal(Guid cohortId)
        {
            // Get cohort information
            var cohortForView = await _cohortsAppService.GetCohortForView(cohortId);

            // Get available departments for selection
            var availableDepartments = await _userTransferAppService.GetAvailableTargetDepartments(
                cohortForView.Cohort.TenantDepartmentId ?? Guid.Empty);

            var viewModel = new UserTransferWizardViewModel
            {
                CohortId = cohortId,
                CohortName = cohortForView.Cohort.Name,
                SourceDepartmentName = cohortForView.TenantDepartmentName,
                SourceDepartmentId = cohortForView.Cohort.TenantDepartmentId,
                AvailableDepartments = availableDepartments,
                CurrentStep = 1
            };

            return PartialView("_TransferWizardModal", viewModel);
        }

        //[AbpMvcAuthorize(AppPermissions.Pages_CohortUsers_Transfer)]
        //public async Task<JsonResult> UserTransferAnalysis(Guid cohortId, Guid? targetDepartmentId)
        //{
        //    try
        //    {
        //        var analysis = await _userTransferAppService.UserTransferAnalysis(cohortId, targetDepartmentId);
        //        return Json(new { success = true, data = analysis });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = ex.Message });
        //    }
        //}

        [AbpMvcAuthorize(AppPermissions.Pages_CohortUsers_Transfer)]
        public async Task<JsonResult> GetAvailableDepartments(Guid excludeDepartmentId)
        {
            try
            {
                var departments = await _userTransferAppService.GetAvailableTargetDepartments(excludeDepartmentId);
                return Json(new { success = true, data = departments });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [AbpMvcAuthorize(AppPermissions.Pages_CohortUsers_Transfer)]
        public async Task<JsonResult> ValidateDepartmentSelection(ValidateDepartmentSelectionInput input)
        {
            try
            {
                var validation = await _userTransferAppService.ValidateDepartmentSelection(input);
                return Json(new { success = true, data = validation });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [AbpMvcAuthorize(AppPermissions.Pages_CohortUsers_Transfer)]
        public async Task<JsonResult> GetTargetCategoryOptions(Guid sourceCategoryId, Guid targetDepartmentId, int? minMatchScore = null, int? maxResults = null, string searchFilter = null)
        {
            try
            {
                var input = new GetTargetCategoryOptionsInput
                {
                    SourceCategoryId = sourceCategoryId,
                    TargetDepartmentId = targetDepartmentId,
                    MinMatchScore = minMatchScore,
                    MaxResults = maxResults,
                    SearchFilter = searchFilter,
                    PrioritizeHighMatches = true
                };

                var options = await _userTransferAppService.GetTargetCategoryOptions(input);
                return Json(new { success = true, data = options });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [AbpMvcAuthorize(AppPermissions.Pages_CohortUsers_Transfer)]
        public async Task<JsonResult> GetCohortRequirementCategories(Guid cohortId)
        {
            try
            {
                var categories = await _userTransferAppService.GetCohortRequirementCategories(cohortId);
                return Json(new { success = true, data = categories });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion User Transfer Wizard
    }
}