using System;
namespace surpath200.Core.DTOs
{
    public class TenantDepartmentUserDto
    {
        public Guid Id { get; set; }
        public int? TenantId { get; set; }
        public long UserId { get; set; }
        public Guid TenantDepartmentId { get; set; }
    }
}