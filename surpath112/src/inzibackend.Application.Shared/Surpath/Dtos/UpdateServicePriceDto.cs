using System;

namespace inzibackend.Surpath.Dtos
{
    public class UpdateServicePriceDto
    {
        public Guid? Id { get; set; } // Will be determined by the service based on existing record
        public Guid SurpathServiceId { get; set; }
        public double? Price { get; set; }
        public bool IsInvoiced { get; set; }
        public string Level { get; set; } // "tenant", "department", "cohort", "user"
        public string TargetId { get; set; }
        public int TenantId { get; set; }
    }
}