using System;
using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos
{
    public class TenantSurpathServiceDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

        public bool IsPricingOverrideEnabled { get; set; }

        public Guid? SurpathServiceId { get; set; }

        public Guid? TenantDepartmentId { get; set; }

        public Guid? CohortId { get; set; }

        public long? UserId { get; set; }

        public Guid? RecordCategoryRuleId { get; set; }

        public long? OrganizationUnitId { get; set; }

        public string SurpathServiceName { get; set; }

        public string OrganizationUnitName { get; set; }

        public double BasePrice { get; set; } // Base price from SurpathService

        public bool IsInvoiced { get; set; }

        public double AmountDue { get; set; }
    }
}
