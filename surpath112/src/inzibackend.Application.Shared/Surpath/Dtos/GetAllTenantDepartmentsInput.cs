using Abp.Application.Services.Dto;
using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetAllTenantDepartmentsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public int? ActiveFilter { get; set; }

        public int? MROTypeFilter { get; set; }

        public string DescriptionFilter { get; set; }

    }
}