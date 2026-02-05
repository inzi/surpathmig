using Abp.Application.Services.Dto;
using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetAllTenantDocumentCategoriesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public int? AuthorizedOnlyFilter { get; set; }

        public int? HostOnlyFilter { get; set; }

        public string UserNameFilter { get; set; }

    }
}