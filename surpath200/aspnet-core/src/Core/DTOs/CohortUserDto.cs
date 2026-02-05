using System;
namespace surpath200.Core.DTOs
{
    public class CohortUserDto
    {
        public Guid Id { get; set; }
        public int? TenantId { get; set; }
        public long UserId { get; set; }
        public Guid CohortId { get; set; }
    }
}