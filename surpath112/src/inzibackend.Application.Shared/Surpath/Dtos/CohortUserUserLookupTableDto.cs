using Abp.Application.Services.Dto;
using System;

namespace inzibackend.Surpath.Dtos
{
    public class CohortUserUserLookupTableDto
    {
        public long Id { get; set; }
        public Guid CohortUserId { get; set; }
        public string DisplayName { get; set; }
        public string Surname { get; set; }
        public string Tenant { get; set; }
        public int TenantId { get; set; }
        public string Cohort { get; set; }
        public Guid CohortId { get; set; }

    }
}