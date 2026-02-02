using Abp.Application.Services.Dto;
using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetAllCohortUsersInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string CohortDescriptionFilter { get; set; }

        public string UserNameFilter { get; set; }

        public Guid? CohortIdFilter { get; set; }

        public long? TenantId { get; set; }
        public Guid? Id { get; set; }

    }
}