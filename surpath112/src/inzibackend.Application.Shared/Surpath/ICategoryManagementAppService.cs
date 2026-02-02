using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.Dtos;

namespace inzibackend.Surpath
{
    public interface ICategoryManagementAppService : IApplicationService
    {
        // Validation methods
        Task<CategoryMoveValidationDto> ValidateCategoryMove(Guid categoryId, Guid? targetRequirementId);
        
        // Move operations
        Task<MoveCategoryResultDto> MoveCategory(MoveCategoryDto input);
        Task<MoveCategoryResultDto> MoveCategoryToNewRequirement(MoveCategoryDto input);
        Task<MoveCategoryResultDto> MoveCategoryToExistingRequirement(MoveCategoryDto input);
        
        // Copy operations
        Task<CopyCategoryResultDto> CopyCategories(CopyCategoryDto input);
        
        // Lookup methods
        Task<List<RequirementCategoryLookupDto>> GetRequirementsWithCategories(int? tenantId = null);
        Task<List<RecordRequirementDto>> GetAvailableTargetRequirements(Guid excludeRequirementId);
        
        // Options methods
        Task<GetCategoryMoveOptionsOutput> GetCategoryMoveOptions(GetCategoryMoveOptionsInput input);
        Task<GetCategoryCopyOptionsOutput> GetCategoryCopyOptions(GetCategoryCopyOptionsInput input);
        
        // Validation operations
        Task<ValidateCategoryOperationOutput> ValidateCategoryOperation(ValidateCategoryOperationInput input);
        
        // Utility methods
        Task<bool> CanDeleteRequirement(Guid requirementId);
        Task<int> GetAffectedRecordStatesCount(Guid categoryId);
        Task<int> GetAffectedUsersCount(Guid categoryId);
    }
} 