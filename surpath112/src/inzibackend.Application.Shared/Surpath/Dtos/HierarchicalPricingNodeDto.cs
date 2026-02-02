using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos
{
    // Unified pricing node that can represent any level in the hierarchy
    public class HierarchicalPricingNodeDto
    {
        public string Id { get; set; } // Can be tenant ID, department ID, cohort ID, or user ID
        public string NodeType { get; set; } // "tenant", "department", "cohort", "user"
        public string Name { get; set; }
        public string Description { get; set; }
        
        // User-specific properties (null for other types)
        public string Surname { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string FullName => NodeType == "user" ? $"{Name} {Surname}".Trim() : Name;
        
        // Services at this level
        public List<TenantSurpathServiceDto> Services { get; set; }
        
        // Child nodes
        public List<HierarchicalPricingNodeDto> Children { get; set; }
        
        public HierarchicalPricingNodeDto()
        {
            Services = new List<TenantSurpathServiceDto>();
            Children = new List<HierarchicalPricingNodeDto>();
        }
    }
    
    // New input DTO for hierarchical pricing
    public class GetHierarchicalPricingInputV2
    {
        public int TenantId { get; set; }
        public Guid? SurpathServiceId { get; set; }
        public bool IncludeDisabled { get; set; } = false; // Include disabled services for UI management
    }
}