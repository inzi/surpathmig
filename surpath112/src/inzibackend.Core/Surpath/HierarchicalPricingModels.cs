using System;
using System.Collections.Generic;

namespace inzibackend.Surpath
{
    // Domain models for hierarchical pricing
    public class HierarchicalPricing
    {
        public TenantPricing Tenant { get; set; }
        public List<DepartmentPricing> Departments { get; set; }
        public List<CohortPricing> Cohorts { get; set; } // For standalone cohorts

        public HierarchicalPricing()
        {
            Departments = new List<DepartmentPricing>();
            Cohorts = new List<CohortPricing>();
        }
    }

    public class TenantPricing
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ServicePrice> Services { get; set; }

        public TenantPricing()
        {
            Services = new List<ServicePrice>();
        }
    }

    public class DepartmentPricing
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<ServicePrice> Services { get; set; }
        public List<CohortPricing> Cohorts { get; set; }

        public DepartmentPricing()
        {
            Services = new List<ServicePrice>();
            Cohorts = new List<CohortPricing>();
        }
    }

    public class CohortPricing
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<ServicePrice> Services { get; set; }
        public List<UserPricing> Users { get; set; }

        public CohortPricing()
        {
            Services = new List<ServicePrice>();
            Users = new List<UserPricing>();
        }
    }

    public class UserPricing
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public List<ServicePrice> Services { get; set; }

        public UserPricing()
        {
            Services = new List<ServicePrice>();
        }

        public string FullName => $"{Name} {Surname}".Trim();
    }

    public class ServicePrice
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
}