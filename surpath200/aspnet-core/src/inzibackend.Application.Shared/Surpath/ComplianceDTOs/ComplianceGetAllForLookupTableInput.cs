using System;
using Abp.Application.Services.Dto;
using inzibackend.Surpath.DTOBases;

namespace inzibackend.Surpath.Dtos
{
    public class ComplianceGetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public Guid? TenantDepartmentId { get; set; }
        public bool shuffle { get; set; } = false;
        public Guid? CohortUserId { get; set; }
        public long? TenantId { get; set; }
        public Guid? ExcludeCohortId { get; set; }

    }
}