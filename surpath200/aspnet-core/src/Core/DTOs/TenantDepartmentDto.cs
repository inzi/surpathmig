using System;
namespace surpath200.Core.DTOs
{
    public class TenantDepartmentDto
    {
        public Guid Id { get; set; }
        public int? TenantId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public string MROType { get; set; }
        public string Description { get; set; }
        public long OrganizationUnitId { get; set; }
    }
}