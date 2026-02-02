using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.RecordRequirements;
using inzibackend.Web.Controllers;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_RecordRequirements)]
    public class RecordRequirementsController : inzibackendControllerBase
    {
        private readonly IRecordRequirementsAppService _recordRequirementsAppService;
        private readonly ICategoryManagementAppService _categoryManagementAppService;

        public RecordRequirementsController(
            IRecordRequirementsAppService recordRequirementsAppService,
            ICategoryManagementAppService categoryManagementAppService)
        {
            _recordRequirementsAppService = recordRequirementsAppService;
            _categoryManagementAppService = categoryManagementAppService;
        }

        public ActionResult Index()
        {
            var model = new RecordRequirementsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_RecordRequirements_Create, AppPermissions.Pages_Administration_RecordRequirements_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetRecordRequirementForEditOutput getRecordRequirementForEditOutput;

            if (id.HasValue)
            {
                getRecordRequirementForEditOutput = await _recordRequirementsAppService.GetRecordRequirementForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getRecordRequirementForEditOutput = new GetRecordRequirementForEditOutput
                {
                    RecordRequirement = new CreateOrEditRecordRequirementDto()
                };
            }

            var viewModel = new CreateOrEditRecordRequirementModalViewModel()
            {
                RecordRequirement = getRecordRequirementForEditOutput.RecordRequirement,
                TenantDepartmentName = getRecordRequirementForEditOutput.TenantDepartmentName,
                CohortName = getRecordRequirementForEditOutput.CohortName,
                SurpathServiceName = getRecordRequirementForEditOutput.SurpathServiceName,
                //TenantSurpathServiceName = getRecordRequirementForEditOutput.TenantSurpathServiceName,
                //RecordRequirementSurpathServiceList = await _recordRequirementsAppService.GetAllSurpathServiceForTableDropdown(),
                //RecordRequirementTenantSurpathServiceList = await _recordRequirementsAppService.GetAllTenantSurpathServiceForTableDropdown(),

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewRecordRequirementModal(Guid id)
        {
            var getRecordRequirementForViewDto = await _recordRequirementsAppService.GetRecordRequirementForView(id);

            var model = new RecordRequirementViewModel()
            {
                RecordRequirement = getRecordRequirementForViewDto.RecordRequirement
                ,
                TenantDepartmentName = getRecordRequirementForViewDto.TenantDepartmentName

                ,
                CohortName = getRecordRequirementForViewDto.CohortName

                ,
                SurpathServiceName = getRecordRequirementForViewDto.SurpathServiceName

                ,
                TenantSurpathServiceName = getRecordRequirementForViewDto.TenantSurpathServiceName

            };

            return PartialView("_ViewRecordRequirementModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_RecordRequirements_Create, AppPermissions.Pages_Administration_RecordRequirements_Edit)]
        public PartialViewResult TenantDepartmentLookupTableModal(Guid? id, string displayName)
        {
            var viewModel = new RecordRequirementTenantDepartmentLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_RecordRequirementTenantDepartmentLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_Administration_RecordRequirements_Create, AppPermissions.Pages_Administration_RecordRequirements_Edit)]
        public PartialViewResult CohortLookupTableModal(Guid? id, string displayName)
        {
            var viewModel = new RecordRequirementCohortLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_RecordRequirementCohortLookupTableModal", viewModel);
        }

        #region Category Management

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_RecordRequirements_ManageCategories)]
        public async Task<PartialViewResult> CategoryManagementModal(Guid requirementId)
        {
            // Get requirement details
            var requirement = await _recordRequirementsAppService.GetRecordRequirementForView(requirementId);
            
            // Get categories for this requirement
            var requirementsWithCategories = await _categoryManagementAppService.GetRequirementsWithCategories();
            var targetRequirement = requirementsWithCategories.FirstOrDefault(r => r.RequirementId == requirementId);
            
            var viewModel = new CategoryManagementModalViewModel
            {
                RequirementId = requirementId,
                RequirementName = requirement.RecordRequirement.Name,
                Categories = targetRequirement?.Categories ?? new List<CategoryLookupDto>()
            };

            return PartialView("_CategoryManagementModal", viewModel);
        }



        [AbpMvcAuthorize(AppPermissions.Pages_Administration_RecordRequirements_MoveCategories)]
        public async Task<PartialViewResult> MoveCategoryModal(string categoryIds)
        {
            var categoryIdList = categoryIds.Split(',').Select(Guid.Parse).ToList();
            
            // Get selected categories info
            var requirementsWithCategories = await _categoryManagementAppService.GetRequirementsWithCategories();
            var selectedCategories = new List<CategoryLookupDto>();
            
            foreach (var requirement in requirementsWithCategories)
            {
                selectedCategories.AddRange(requirement.Categories.Where(c => categoryIdList.Contains(c.CategoryId)));
            }
            
            // Get available target requirements (excluding source requirements)
            var sourceRequirementIds = selectedCategories.Select(c => 
                requirementsWithCategories.First(r => r.Categories.Any(cat => cat.CategoryId == c.CategoryId)).RequirementId
            ).Distinct().ToList();
            
            var availableTargets = new List<RecordRequirementDto>();
            foreach (var sourceReqId in sourceRequirementIds)
            {
                var targets = await _categoryManagementAppService.GetAvailableTargetRequirements(sourceReqId);
                availableTargets.AddRange(targets);
            }
            
            // Remove duplicates
            availableTargets = availableTargets.GroupBy(r => r.Id).Select(g => g.First()).ToList();
            
            var viewModel = new MoveCategoryModalViewModel
            {
                CategoryIds = categoryIdList,
                SelectedCategories = selectedCategories,
                AvailableTargetRequirements = availableTargets
            };

            return PartialView("_MoveCategoryModal", viewModel);
        }

        #endregion

    }
}