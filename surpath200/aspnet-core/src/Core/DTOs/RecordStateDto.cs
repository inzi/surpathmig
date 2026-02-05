using System;
namespace surpath200.Core.DTOs
{
    public class RecordStateDto
    {
        public Guid Id { get; set; }
        public int? TenantId { get; set; }
        public string State { get; set; }
        public string Notes { get; set; }
        public Guid? RecordId { get; set; }
        public Guid? RecordCategoryId { get; set; }
        public long? UserId { get; set; }
        public Guid RecordStatusId { get; set; }
        public bool IsArchived { get; set; }
        public DateTime? ArchivedTime { get; set; }
    }
}