using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos
{
    // Input DTO for retrieving hierarchical pricing data
    public class GetHierarchicalPricingInput
    {
        public int TenantId { get; set; }
        public Guid? SurpathServiceId { get; set; }
    }

    // Root DTO for hierarchical pricing structure
    public class HierarchicalPricingDto
    {
        public TenantPricingDto Tenant { get; set; }
        public List<DepartmentPricingDto> Departments { get; set; }
        public List<CohortPricingDto> Cohorts { get; set; } // For cohorts that don't belong to departments

        public HierarchicalPricingDto()
        {
            Departments = new List<DepartmentPricingDto>();
            Cohorts = new List<CohortPricingDto>();
        }
    }

    // Tenant level pricing information
    public class TenantPricingDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ServicePriceDto> Services { get; set; }

        public TenantPricingDto()
        {
            Services = new List<ServicePriceDto>();
        }
    }

    // Department level pricing information
    public class DepartmentPricingDto : TenantDepartmentDto
    {
        public List<ServicePriceDto> Services { get; set; }
        public List<CohortPricingDto> Cohorts { get; set; }

        public DepartmentPricingDto()
        {
            Services = new List<ServicePriceDto>();
            Cohorts = new List<CohortPricingDto>();
        }
    }

    // Cohort level pricing information
    public class CohortPricingDto : CohortDto
    {
        public List<ServicePriceDto> Services { get; set; }
        public List<UserPricingDto> Users { get; set; }

        public CohortPricingDto()
        {
            Services = new List<ServicePriceDto>();
            Users = new List<UserPricingDto>();
        }
    }

    // User level pricing information
    public class UserPricingDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public List<ServicePriceDto> Services { get; set; }

        public UserPricingDto()
        {
            Services = new List<ServicePriceDto>();
        }

        public string FullName => $"{Name} {Surname}".Trim();
    }

    // Service pricing information at any level
    public class ServicePriceDto
    {
        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ServiceDescription { get; set; }
        public double BasePrice { get; set; }
        public double? OverridePrice { get; set; }
        public double EffectivePrice { get; set; }
        public bool IsInherited { get; set; }
        public Guid? TenantSurpathServiceId { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsInvoiced { get; set; }

        // Helper property to get display price
        public double DisplayPrice => OverridePrice ?? EffectivePrice;
    }

    // DTO for batch price updates
    public class BatchUpdatePriceDto
    {
        public List<UpdatePriceItemDto> Updates { get; set; }

        public BatchUpdatePriceDto()
        {
            Updates = new List<UpdatePriceItemDto>();
        }
    }

    // Individual price update item
    public class UpdatePriceItemDto
    {
        public Guid? Id { get; set; } // Existing TenantSurpathService Id
        public Guid SurpathServiceId { get; set; }
        public double? Price { get; set; } // Null means inherit from parent
        public Guid? TenantDepartmentId { get; set; }
        public Guid? CohortId { get; set; }
        public long? UserId { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsInvoiced { get; set; }
    }

    // DTO for setting price for all services at once
    public class SetAllServicesPriceDto
    {
        public double Price { get; set; }
        public string TargetType { get; set; } // "tenant", "department", "cohort", "user"
        public string TargetId { get; set; }
        public int TenantId { get; set; }
    }

}