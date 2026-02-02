using Abp.Application.Services.Dto;
using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetAllRecordsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string filenameFilter { get; set; }

        public string physicalfilepathFilter { get; set; }

        public string metadataFilter { get; set; }

        public Guid? BinaryObjIdFilter { get; set; }

        public DateTime? MaxDateUploadedFilter { get; set; }
        public DateTime? MinDateUploadedFilter { get; set; }

        public DateTime? MaxDateLastUpdatedFilter { get; set; }
        public DateTime? MinDateLastUpdatedFilter { get; set; }

        public int? InstructionsConfirmedFilter { get; set; }

        public DateTime? MaxEffectiveDateFilter { get; set; }
        public DateTime? MinEffectiveDateFilter { get; set; }

        public DateTime? MaxExpirationDateFilter { get; set; }
        public DateTime? MinExpirationDateFilter { get; set; }

        public string TenantDocumentCategoryNameFilter { get; set; }
        
        public Guid? TenantDocumentCategoryIdFilter { get; set; }

    }
}