using Abp.Application.Services.Dto;
using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetAllTenantDocumentsForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public int? AuthorizedOnlyFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public string TenantDocumentCategoryNameFilter { get; set; }

        public string RecordfilenameFilter { get; set; }

    }
}