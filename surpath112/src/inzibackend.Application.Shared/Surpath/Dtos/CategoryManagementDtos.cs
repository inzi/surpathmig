using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos
{
    // Move Category DTOs
    public class MoveCategoryDto
    {
        [Required]
        public Guid CategoryId { get; set; }
        
        public Guid? TargetRequirementId { get; set; } // null if creating new requirement
        
        public string NewRequirementName { get; set; } // required if TargetRequirementId is null
        
        public string NewRequirementDescription { get; set; }
        
        public bool ConfirmRequirementDeletion { get; set; } // for when source requirement becomes empty
    }

    public class MoveCategoryResultDto
    {
        public bool Success { get; set; }
        
        public string Message { get; set; }
        
        public bool RequirementDeleted { get; set; }
        
        public Guid? NewRequirementId { get; set; }
        
        public List<string> Warnings { get; set; } = new List<string>();
    }

    // Copy Category DTOs
    public class CopyCategoryDto
    {
        [Required]
        public List<Guid> CategoryIds { get; set; } = new List<Guid>();
        
        [Required]
        public Guid TargetRequirementId { get; set; }
        
        public bool CopyRecordStates { get; set; } // option to copy existing user record states
    }

    public class CopyCategoryResultDto
    {
        public bool Success { get; set; }
        
        public string Message { get; set; }
        
        public List<Guid> NewCategoryIds { get; set; } = new List<Guid>();
        
        public int RecordStatesCopied { get; set; }
    }

    // Validation DTOs
    public class CategoryMoveValidationDto
    {
        public bool CanMove { get; set; }
        
        public bool WillDeleteSourceRequirement { get; set; }
        
        public int AffectedRecordStatesCount { get; set; }
        
        public int AffectedUsersCount { get; set; }
        
        public List<string> Warnings { get; set; } = new List<string>();
        
        public List<string> Errors { get; set; } = new List<string>();
    }

    // Lookup DTOs
    public class RequirementCategoryLookupDto
    {
        public Guid RequirementId { get; set; }
        
        public string RequirementName { get; set; }
        
        public List<CategoryLookupDto> Categories { get; set; } = new List<CategoryLookupDto>();
    }

    public class CategoryLookupDto
    {
        public Guid CategoryId { get; set; }
        
        public string CategoryName { get; set; }
        
        public string Instructions { get; set; }
        
        public int RecordStatesCount { get; set; }
        
        public bool IsOnlyCategory { get; set; }
    }

    // Input DTOs for operations
    public class GetCategoryMoveOptionsInput
    {
        [Required]
        public Guid CategoryId { get; set; }
    }

    public class GetCategoryMoveOptionsOutput
    {
        public List<RecordRequirementDto> AvailableTargetRequirements { get; set; } = new List<RecordRequirementDto>();
        
        public CategoryMoveValidationDto ValidationInfo { get; set; }
    }

    public class GetCategoryCopyOptionsInput
    {
        public Guid? SourceRequirementId { get; set; }
    }

    public class GetCategoryCopyOptionsOutput
    {
        public List<RequirementCategoryLookupDto> SourceRequirements { get; set; } = new List<RequirementCategoryLookupDto>();
        
        public List<RecordRequirementDto> TargetRequirements { get; set; } = new List<RecordRequirementDto>();
    }

    public class ValidateCategoryOperationInput
    {
        [Required]
        public Guid CategoryId { get; set; }
        
        public Guid? TargetRequirementId { get; set; }
        
        [Required]
        public string OperationType { get; set; } // "move" or "copy"
    }

    public class ValidateCategoryOperationOutput
    {
        public CategoryMoveValidationDto ValidationResult { get; set; }
    }
} 