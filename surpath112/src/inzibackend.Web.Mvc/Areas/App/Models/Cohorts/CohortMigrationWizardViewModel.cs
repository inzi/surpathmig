using System;
using System.Collections.Generic;
using inzibackend.Surpath.Dtos;

namespace inzibackend.Web.Areas.App.Models.Cohorts
{
    public class CohortMigrationWizardViewModel
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
        
        // Migration analysis data
        public CohortMigrationAnalysisDto MigrationAnalysis { get; set; }
        
        // Available departments for selection
        public List<DepartmentLookupDto> AvailableDepartments { get; set; } = new List<DepartmentLookupDto>();
        
        // Category mappings for step 2
        public List<RequirementCategoryMappingDto> CategoryMappings { get; set; } = new List<RequirementCategoryMappingDto>();
        
        // Current wizard step
        public int CurrentStep { get; set; } = 1;
        
        // Validation flags
        public bool IsStep1Valid { get; set; }
        public bool IsStep2Valid { get; set; }
        
        // Migration result
        public CohortMigrationResultDto MigrationResult { get; set; }
    }
} 