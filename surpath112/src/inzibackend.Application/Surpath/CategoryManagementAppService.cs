using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.UI;
using inzibackend.Authorization;
using inzibackend.Authorization.Users;
using inzibackend.Surpath.Dtos;
using Microsoft.EntityFrameworkCore;

namespace inzibackend.Surpath
{
    [AbpAuthorize(AppPermissions.Pages_Administration_RecordRequirements_ManageCategories)]
    public class CategoryManagementAppService : inzibackendAppServiceBase, ICategoryManagementAppService
    {
        private readonly IRepository<RecordCategory, Guid> _recordCategoryRepository;
        private readonly IRepository<RecordRequirement, Guid> _recordRequirementRepository;
        private readonly IRepository<RecordState, Guid> _recordStateRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<TenantDepartment, Guid> _tenantDepartmentRepository;
        private readonly IRepository<Cohort, Guid> _cohortRepository;

        public CategoryManagementAppService(
            IRepository<RecordCategory, Guid> recordCategoryRepository,
            IRepository<RecordRequirement, Guid> recordRequirementRepository,
            IRepository<RecordState, Guid> recordStateRepository,
            IRepository<User, long> userRepository,
            IRepository<TenantDepartment, Guid> tenantDepartmentRepository,
            IRepository<Cohort, Guid> cohortRepository)
        {
            _recordCategoryRepository = recordCategoryRepository;
            _recordRequirementRepository = recordRequirementRepository;
            _recordStateRepository = recordStateRepository;
            _userRepository = userRepository;
            _tenantDepartmentRepository = tenantDepartmentRepository;
            _cohortRepository = cohortRepository;
        }

        #region Validation Methods

        public async Task<CategoryMoveValidationDto> ValidateCategoryMove(Guid categoryId, Guid? targetRequirementId)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var category = await _recordCategoryRepository.GetAsync(categoryId);
            var sourceRequirement = await _recordRequirementRepository.GetAsync(category.RecordRequirementId.Value);
            
            var validation = new CategoryMoveValidationDto
            {
                CanMove = true,
                Warnings = new List<string>(),
                Errors = new List<string>()
            };

            // Check if this is the only category in the source requirement
            var categoriesInSourceRequirement = await _recordCategoryRepository.GetAll()
                .Where(c => c.RecordRequirementId == sourceRequirement.Id)
                .CountAsync();

            validation.WillDeleteSourceRequirement = categoriesInSourceRequirement == 1;

            // Get affected record states count
            validation.AffectedRecordStatesCount = await GetAffectedRecordStatesCount(categoryId);
            validation.AffectedUsersCount = await GetAffectedUsersCount(categoryId);

            // Add warnings
            if (validation.WillDeleteSourceRequirement)
            {
                validation.Warnings.Add($"Moving this category will delete the requirement '{sourceRequirement.Name}' as it will have no categories left.");
            }

            if (validation.AffectedRecordStatesCount > 0)
            {
                validation.Warnings.Add($"This operation will affect {validation.AffectedRecordStatesCount} record states for {validation.AffectedUsersCount} users.");
            }

            // Validate target requirement if specified
            if (targetRequirementId.HasValue)
            {
                var targetRequirement = await _recordRequirementRepository.FirstOrDefaultAsync(targetRequirementId.Value);
                if (targetRequirement == null)
                {
                    validation.CanMove = false;
                    validation.Errors.Add("Target requirement not found.");
                }
                else if (targetRequirement.TenantId != category.TenantId)
                {
                    validation.CanMove = false;
                    validation.Errors.Add("Cannot move category to a requirement in a different tenant.");
                }
            }

            return validation;
        }

        public async Task<ValidateCategoryOperationOutput> ValidateCategoryOperation(ValidateCategoryOperationInput input)
        {
            var validationResult = await ValidateCategoryMove(input.CategoryId, input.TargetRequirementId);
            
            return new ValidateCategoryOperationOutput
            {
                ValidationResult = validationResult
            };
        }

        #endregion

        #region Move Operations

        [AbpAuthorize(AppPermissions.Pages_Administration_RecordRequirements_MoveCategories)]
        public async Task<MoveCategoryResultDto> MoveCategory(MoveCategoryDto input)
        {
            if (input.TargetRequirementId.HasValue)
            {
                return await MoveCategoryToExistingRequirement(input);
            }
            else
            {
                return await MoveCategoryToNewRequirement(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_RecordRequirements_MoveCategories)]
        public async Task<MoveCategoryResultDto> MoveCategoryToNewRequirement(MoveCategoryDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            using (var uow = UnitOfWorkManager.Begin())
            {
                try
                {
                    // Validate the operation
                    var validation = await ValidateCategoryMove(input.CategoryId, null);
                    if (!validation.CanMove)
                    {
                        return new MoveCategoryResultDto
                        {
                            Success = false,
                            Message = string.Join("; ", validation.Errors)
                        };
                    }

                    // Get the category and source requirement
                    var category = await _recordCategoryRepository.GetAsync(input.CategoryId);
                    var sourceRequirement = await _recordRequirementRepository.GetAsync(category.RecordRequirementId.Value);

                    // Create new requirement
                    var newRequirement = new RecordRequirement
                    {
                        Name = input.NewRequirementName,
                        Description = input.NewRequirementDescription ?? "",
                        TenantId = sourceRequirement.TenantId,
                        TenantDepartmentId = sourceRequirement.TenantDepartmentId,
                        CohortId = sourceRequirement.CohortId,
                        IsSurpathOnly = sourceRequirement.IsSurpathOnly
                    };

                    var newRequirementId = await _recordRequirementRepository.InsertAndGetIdAsync(newRequirement);

                    // Move the category
                    category.RecordRequirementId = newRequirementId;
                    await _recordCategoryRepository.UpdateAsync(category);

                    // Delete source requirement if it becomes empty and confirmed
                    bool requirementDeleted = false;
                    if (validation.WillDeleteSourceRequirement && input.ConfirmRequirementDeletion)
                    {
                        await _recordRequirementRepository.DeleteAsync(sourceRequirement.Id);
                        requirementDeleted = true;
                    }

                    await uow.CompleteAsync();

                    return new MoveCategoryResultDto
                    {
                        Success = true,
                        Message = "Category moved to new requirement successfully.",
                        NewRequirementId = newRequirementId,
                        RequirementDeleted = requirementDeleted,
                        Warnings = validation.Warnings
                    };
                }
                catch (Exception ex)
                {
                    Logger.Error("Error moving category to new requirement", ex);
                    return new MoveCategoryResultDto
                    {
                        Success = false,
                        Message = "An error occurred while moving the category: " + ex.Message
                    };
                }
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_RecordRequirements_MoveCategories)]
        public async Task<MoveCategoryResultDto> MoveCategoryToExistingRequirement(MoveCategoryDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            using (var uow = UnitOfWorkManager.Begin())
            {
                try
                {
                    // Validate the operation
                    var validation = await ValidateCategoryMove(input.CategoryId, input.TargetRequirementId);
                    if (!validation.CanMove)
                    {
                        return new MoveCategoryResultDto
                        {
                            Success = false,
                            Message = string.Join("; ", validation.Errors)
                        };
                    }

                    // Get the category and requirements
                    var category = await _recordCategoryRepository.GetAsync(input.CategoryId);
                    var sourceRequirement = await _recordRequirementRepository.GetAsync(category.RecordRequirementId.Value);
                    var targetRequirement = await _recordRequirementRepository.GetAsync(input.TargetRequirementId.Value);

                    // Move the category
                    category.RecordRequirementId = input.TargetRequirementId.Value;
                    await _recordCategoryRepository.UpdateAsync(category);

                    // Delete source requirement if it becomes empty and confirmed
                    bool requirementDeleted = false;
                    if (validation.WillDeleteSourceRequirement && input.ConfirmRequirementDeletion)
                    {
                        await _recordRequirementRepository.DeleteAsync(sourceRequirement.Id);
                        requirementDeleted = true;
                    }

                    await uow.CompleteAsync();

                    return new MoveCategoryResultDto
                    {
                        Success = true,
                        Message = $"Category moved to '{targetRequirement.Name}' successfully.",
                        RequirementDeleted = requirementDeleted,
                        Warnings = validation.Warnings
                    };
                }
                catch (Exception ex)
                {
                    Logger.Error("Error moving category to existing requirement", ex);
                    return new MoveCategoryResultDto
                    {
                        Success = false,
                        Message = "An error occurred while moving the category: " + ex.Message
                    };
                }
            }
        }

        #endregion

        #region Copy Operations

        [AbpAuthorize(AppPermissions.Pages_Administration_RecordRequirements_CopyCategories)]
        public async Task<CopyCategoryResultDto> CopyCategories(CopyCategoryDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            using (var uow = UnitOfWorkManager.Begin())
            {
                try
                {
                    var targetRequirement = await _recordRequirementRepository.GetAsync(input.TargetRequirementId);
                    var newCategoryIds = new List<Guid>();
                    int recordStatesCopied = 0;

                    foreach (var categoryId in input.CategoryIds)
                    {
                        var sourceCategory = await _recordCategoryRepository.GetAsync(categoryId);
                        
                        // Create new category
                        var newCategory = new RecordCategory
                        {
                            Name = sourceCategory.Name,
                            Instructions = sourceCategory.Instructions,
                            RecordRequirementId = input.TargetRequirementId,
                            RecordCategoryRuleId = sourceCategory.RecordCategoryRuleId,
                            TenantId = targetRequirement.TenantId
                        };

                        var newCategoryId = await _recordCategoryRepository.InsertAndGetIdAsync(newCategory);
                        newCategoryIds.Add(newCategoryId);

                        // Copy record states if requested
                        if (input.CopyRecordStates)
                        {
                            var recordStates = await _recordStateRepository.GetAll()
                                .Where(rs => rs.RecordCategoryId == categoryId)
                                .ToListAsync();

                            foreach (var recordState in recordStates)
                            {
                                var newRecordState = new RecordState
                                {
                                    State = recordState.State,
                                    Notes = recordState.Notes + " (Copied)",
                                    RecordId = recordState.RecordId,
                                    RecordCategoryId = newCategoryId,
                                    UserId = recordState.UserId,
                                    RecordStatusId = recordState.RecordStatusId,
                                    TenantId = targetRequirement.TenantId
                                };

                                await _recordStateRepository.InsertAsync(newRecordState);
                                recordStatesCopied++;
                            }
                        }
                    }

                    await uow.CompleteAsync();

                    return new CopyCategoryResultDto
                    {
                        Success = true,
                        Message = $"Successfully copied {input.CategoryIds.Count} categories to '{targetRequirement.Name}'.",
                        NewCategoryIds = newCategoryIds,
                        RecordStatesCopied = recordStatesCopied
                    };
                }
                catch (Exception ex)
                {
                    Logger.Error("Error copying categories", ex);
                    return new CopyCategoryResultDto
                    {
                        Success = false,
                        Message = "An error occurred while copying categories: " + ex.Message
                    };
                }
            }
        }

        #endregion

        #region Lookup Methods

        public async Task<List<RequirementCategoryLookupDto>> GetRequirementsWithCategories(int? tenantId = null)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var effectiveTenantId = tenantId ?? AbpSession.TenantId;

            var requirements = await _recordRequirementRepository.GetAll()
                .Where(r => r.TenantId == effectiveTenantId)
                .ToListAsync();

            var categories = await _recordCategoryRepository.GetAll()
                .Where(c => c.TenantId == effectiveTenantId)
                .ToListAsync();

            var recordStateCounts = await _recordStateRepository.GetAll()
                .Where(rs => rs.TenantId == effectiveTenantId)
                .GroupBy(rs => rs.RecordCategoryId)
                .Select(g => new { CategoryId = g.Key, Count = g.Count() })
                .ToListAsync();

            var result = new List<RequirementCategoryLookupDto>();

            foreach (var requirement in requirements)
            {
                var requirementCategories = categories.Where(c => c.RecordRequirementId == requirement.Id).ToList();
                
                var categoryLookups = requirementCategories.Select(c => new CategoryLookupDto
                {
                    CategoryId = c.Id,
                    CategoryName = c.Name,
                    Instructions = c.Instructions,
                    RecordStatesCount = recordStateCounts.FirstOrDefault(rsc => rsc.CategoryId == c.Id)?.Count ?? 0,
                    IsOnlyCategory = requirementCategories.Count == 1
                }).ToList();

                result.Add(new RequirementCategoryLookupDto
                {
                    RequirementId = requirement.Id,
                    RequirementName = requirement.Name,
                    Categories = categoryLookups
                });
            }

            return result;
        }

        public async Task<List<RecordRequirementDto>> GetAvailableTargetRequirements(Guid excludeRequirementId)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var requirements = await _recordRequirementRepository.GetAll()
                .Where(r => r.Id != excludeRequirementId)
                .Where(r => r.TenantId == AbpSession.TenantId)
                .ToListAsync();

            return requirements.Select(r => ObjectMapper.Map<RecordRequirementDto>(r)).ToList();
        }

        #endregion

        #region Options Methods

        public async Task<GetCategoryMoveOptionsOutput> GetCategoryMoveOptions(GetCategoryMoveOptionsInput input)
        {
            var category = await _recordCategoryRepository.GetAsync(input.CategoryId);
            var sourceRequirement = await _recordRequirementRepository.GetAsync(category.RecordRequirementId.Value);
            
            var availableTargets = await GetAvailableTargetRequirements(sourceRequirement.Id);
            var validation = await ValidateCategoryMove(input.CategoryId, null);

            return new GetCategoryMoveOptionsOutput
            {
                AvailableTargetRequirements = availableTargets,
                ValidationInfo = validation
            };
        }

        public async Task<GetCategoryCopyOptionsOutput> GetCategoryCopyOptions(GetCategoryCopyOptionsInput input)
        {
            var sourceRequirements = await GetRequirementsWithCategories();
            var targetRequirements = new List<RecordRequirementDto>();

            if (input.SourceRequirementId.HasValue)
            {
                targetRequirements = await GetAvailableTargetRequirements(input.SourceRequirementId.Value);
            }
            else
            {
                // Get all requirements as potential targets
                targetRequirements = await _recordRequirementRepository.GetAll()
                    .Where(r => r.TenantId == AbpSession.TenantId)
                    .Select(r => ObjectMapper.Map<RecordRequirementDto>(r))
                    .ToListAsync();
            }

            return new GetCategoryCopyOptionsOutput
            {
                SourceRequirements = sourceRequirements,
                TargetRequirements = targetRequirements
            };
        }

        #endregion

        #region Utility Methods

        public async Task<bool> CanDeleteRequirement(Guid requirementId)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var categoryCount = await _recordCategoryRepository.GetAll()
                .Where(c => c.RecordRequirementId == requirementId)
                .CountAsync();

            return categoryCount <= 1; // Can delete if it has 1 or 0 categories
        }

        public async Task<int> GetAffectedRecordStatesCount(Guid categoryId)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            return await _recordStateRepository.GetAll()
                .Where(rs => rs.RecordCategoryId == categoryId)
                .CountAsync();
        }

        public async Task<int> GetAffectedUsersCount(Guid categoryId)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            return await _recordStateRepository.GetAll()
                .Where(rs => rs.RecordCategoryId == categoryId)
                .Select(rs => rs.UserId)
                .Distinct()
                .CountAsync();
        }

        #endregion
    }
} 