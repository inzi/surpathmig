using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetRecordStatusForViewDto
    {
        public RecordStatusDto RecordStatus { get; set; }

        public string TenantDepartmentName { get; set; }
        public int? TenantId { get; set; }
        public string TenantName { get; set; }

    }
}