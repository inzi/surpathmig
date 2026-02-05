using System;
namespace surpath200.Core.DTOs
{
    public class RecordStatusDto
    {
        public Guid Id { get; set; }
        public int? TenantId { get; set; }
        public string Name { get; set; }
        public string CssClass { get; set; }
    }
}