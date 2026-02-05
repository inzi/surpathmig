using System;
using System.Collections.Generic;

namespace inzibackend.Surpath
{
    // Domain model for hierarchical pricing node
    public class HierarchicalPricingNode
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
        public List<TenantSurpathServiceInfo> Services { get; set; }
        
        // Child nodes
        public List<HierarchicalPricingNode> Children { get; set; }
        
        public HierarchicalPricingNode()
        {
            Services = new List<TenantSurpathServiceInfo>();
            Children = new List<HierarchicalPricingNode>();
        }
    }
    
    // Domain model for service information
    public class TenantSurpathServiceInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public bool IsEnabled { get; set; }
        public Guid? SurpathServiceId { get; set; }
        public Guid? TenantDepartmentId { get; set; }
        public Guid? CohortId { get; set; }
        public long? UserId { get; set; }
        public double BasePrice { get; set; } // Base price from SurpathService
        public bool IsInvoiced { get; set; }
    }
}