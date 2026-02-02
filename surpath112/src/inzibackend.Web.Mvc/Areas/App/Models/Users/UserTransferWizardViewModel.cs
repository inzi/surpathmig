using System;
using System.Collections.Generic;
using inzibackend.Surpath.Dtos;

namespace inzibackend.Web.Areas.App.Models.Users
{
    public class UserTransferWizardViewModel
    {
        public Guid CohortId { get; set; }
        public string CohortName { get; set; }
        public string SourceDepartmentName { get; set; }
        public Guid? SourceDepartmentId { get; set; }
        
        // Step 1: Department Selection
        public string DepartmentOption { get; set; } = "existing"; // "existing" or "new"
        public Guid? TargetDepartmentId { get; set; }
        public string NewDepartmentName { get; set; }
        public string NewDepartmentDescription { get; set; }
        
        // Transfer analysis data
        public UserTransferAnalysisDto TransferAnalysis { get; set; }
        
        // Available departments for selection
        public List<DepartmentLookupDto> AvailableDepartments { get; set; } = new List<DepartmentLookupDto>();
        
        // Category mappings for step 2
        public List<RequirementCategoryMappingDto> CategoryMappings { get; set; } = new List<RequirementCategoryMappingDto>();
        
        // Current wizard step
        public int CurrentStep { get; set; } = 1;
        
        // Validation flags
        public bool IsStep1Valid { get; set; }
        public bool IsStep2Valid { get; set; }
        
        // Transfer result
        public UserTransferResultDto TransferResult { get; set; }
    }
}