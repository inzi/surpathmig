using Abp.Application.Services.Dto;

namespace inzibackend.Auditing.Dto
{
    public class EntityPropertyChangeDto : EntityDto<long>
    {
        public long EntityChangeId { get; set; }

        public string NewValue { get; set; }

        public string OriginalValue { get; set; }

        public string PropertyName { get; set; }

        public string PropertyTypeFullName { get; set; }

        public int? TenantId { get; set; }

        /// <summary>
        /// User-friendly description of the original value (e.g., "Spring 2024 Chemistry Class" instead of cohort ID)
        /// </summary>
        public string OriginalValueDescription { get; set; }

        /// <summary>
        /// User-friendly description of the new value (e.g., "Fall 2024 Chemistry Class" instead of cohort ID)
        /// </summary>
        public string NewValueDescription { get; set; }
    }
}