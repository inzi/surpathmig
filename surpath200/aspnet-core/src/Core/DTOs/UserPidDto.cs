using System;
namespace surpath200.Core.DTOs
{
    public class UserPidDto
    {
        public Guid Id { get; set; }
        public int? TenantId { get; set; }
        public long UserId { get; set; }
        public string PidValue { get; set; }
        public string PidType { get; set; }
    }
}