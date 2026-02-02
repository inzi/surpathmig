using System;
using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos
{
    public class SurpathServiceDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        public double Price { get; set; }

        public decimal Discount { get; set; }

        public string Description { get; set; }

        public bool IsEnabledByDefault { get; set; }

        public Guid? TenantDepartmentId { get; set; }

        public Guid? CohortId { get; set; }

        public long? UserId { get; set; }

        public Guid? RecordCategoryRuleId { get; set; }

        public string FeatureIdentifier { get; set; }


    }
}