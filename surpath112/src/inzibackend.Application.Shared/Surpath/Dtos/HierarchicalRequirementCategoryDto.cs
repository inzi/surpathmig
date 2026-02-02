using System;

namespace inzibackend.Surpath.Dtos
{
    /// <summary>
    /// DTO for representing requirement categories with their hierarchical context
    /// </summary>
    public class HierarchicalRequirementCategoryDto
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryInstructions { get; set; }
        public Guid RequirementId { get; set; }
        public string RequirementName { get; set; }
        public string RequirementDescription { get; set; }
        public bool IsDepartmentSpecific { get; set; }
        public bool IsCohortSpecific { get; set; }
        public bool IsRequired { get; set; }
        public bool IsSurpathOnly { get; set; }
        /// <summary>
        /// Hierarchy level: Tenant, Department, Cohort, CohortAndDepartment
        /// </summary>
        public string HierarchyLevel { get; set; }
        
        public Guid? DepartmentId { get; set; }
        public Guid? CohortId { get; set; }
    }
}