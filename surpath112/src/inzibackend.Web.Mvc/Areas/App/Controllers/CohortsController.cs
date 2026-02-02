using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.Cohorts;
using inzibackend.Web.Areas.App.Models.Users;
using inzibackend.Web.Controllers;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using inzibackend.Web.Areas.App.Startup;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Cohorts)]
    public class CohortsController : inzibackendControllerBase
    {
        private readonly ICohortsAppService _cohortsAppService;
        private readonly ICohortMigrationAppService _cohortMigrationAppService;

        public CohortsController(ICohortsAppService cohortsAppService, ICohortMigrationAppService cohortMigrationAppService)
        {
            _cohortsAppService = cohortsAppService;
            _cohortMigrationAppService = cohortMigrationAppService;
        }

        public ActionResult Index()
        {
            var model = new CohortsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Cohorts_Create, AppPermissions.Pages_Cohorts_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetCohortForEditOutput getCohortForEditOutput;

            if (id.HasValue)
            {
                getCohortForEditOutput = await _cohortsAppService.GetCohortForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getCohortForEditOutput = new GetCohortForEditOutput
                {
                    Cohort = new CreateOrEditCohortDto()
                };
            }

            var viewModel = new CreateOrEditCohortModalViewModel()
            {
                Cohort = getCohortForEditOutput.Cohort,
                TenantDepartmentName = getCohortForEditOutput.TenantDepartmentName,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewCohortModal(Guid id)
        {
            var getCohortForViewDto = await _cohortsAppService.GetCohortForView(id);

            var model = new CohortViewModel()
            {
                Cohort = getCohortForViewDto.Cohort
                ,
                TenantDepartmentName = getCohortForViewDto.TenantDepartmentName

            };

            return PartialView("_ViewCohortModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Cohorts_Create, AppPermissions.Pages_Cohorts_Edit)]
        public PartialViewResult TenantDepartmentLookupTableModal(Guid? id, string displayName)
        {
            var viewModel = new CohortTenantDepartmentLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_CohortTenantDepartmentLookupTableModal", viewModel);
        }

        #region Cohort Migration Wizard

        [AbpMvcAuthorize(AppPermissions.Pages_Cohorts_Migrate)]
        public async Task<PartialViewResult> MigrationWizardModal(Guid cohortId)
        {
            // Get cohort information
            var cohortForView = await _cohortsAppService.GetCohortForView(cohortId);
            
            // Get available departments for selection
            var availableDepartments = await _cohortMigrationAppService.GetAvailableTargetDepartments(
                cohortForView.Cohort.TenantDepartmentId ?? Guid.Empty);

            var viewModel = new CohortMigrationWizardViewModel
            {
                CohortId = cohortId,
                CohortName = cohortForView.Cohort.Name,
                SourceDepartmentName = cohortForView.TenantDepartmentName,
                SourceDepartmentId = cohortForView.Cohort.TenantDepartmentId,
                AvailableDepartments = availableDepartments,
                CurrentStep = 1
            };

            return PartialView("_MigrationWizardModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Cohorts_Migrate)]
        public async Task<JsonResult> AnalyzeMigration(Guid cohortId, Guid? targetDepartmentId)
        {
            try
            {
                var analysis = await _cohortMigrationAppService.AnalyzeCohortMigration(cohortId, targetDepartmentId);
                return Json(new { success = true, data = analysis });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Cohorts_Migrate)]
        public async Task<JsonResult> GetAvailableDepartments(Guid excludeDepartmentId)
        {
            try
            {
                var departments = await _cohortMigrationAppService.GetAvailableTargetDepartments(excludeDepartmentId);
                return Json(new { success = true, data = departments });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Cohorts_Migrate)]
        public async Task<JsonResult> ValidateDepartmentSelection(ValidateDepartmentSelectionInput input)
        {
            try
            {
                var validation = await _cohortMigrationAppService.ValidateDepartmentSelection(input);
                return Json(new { success = true, data = validation });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Cohorts_Migrate)]
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
                
                var options = await _cohortMigrationAppService.GetTargetCategoryOptions(input);
                return Json(new { success = true, data = options });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Cohorts_Migrate)]
        public async Task<JsonResult> GetCohortRequirementCategories(Guid cohortId)
        {
            try
            {
                var categories = await _cohortMigrationAppService.GetCohortRequirementCategories(cohortId);
                return Json(new { success = true, data = categories });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }



        #endregion

        #region User Transfer Wizard

        [AbpMvcAuthorize(AppPermissions.Pages_CohortUsers_Transfer)]
        public async Task<PartialViewResult> UserTransferWizardModal(Guid cohortId)
        {
            // Get cohort information
            var cohortForView = await _cohortsAppService.GetCohortForView(cohortId);
            
            // Get all active departments (including current department for cohort transfers)
            var availableDepartments = await _cohortMigrationAppService.GetAvailableTargetDepartments(Guid.Empty);

            var viewModel = new UserTransferWizardViewModel
            {
                CohortId = cohortId,
                CohortName = cohortForView.Cohort.Name,
                SourceDepartmentName = cohortForView.TenantDepartmentName,
                SourceDepartmentId = cohortForView.Cohort.TenantDepartmentId,
                AvailableDepartments = availableDepartments,
                CurrentStep = 1
            };

            return PartialView("~/Areas/App/Views/Users/_TransferWizardModal.cshtml", viewModel);
        }

        #endregion

    }
}