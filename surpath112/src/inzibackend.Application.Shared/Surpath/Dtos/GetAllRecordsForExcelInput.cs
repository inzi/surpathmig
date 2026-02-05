using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class GetAllRecordsForExcelInput
    {
        [MaxLength(AppConsts.MaxFilterLength)]
        public string Filter { get; set; }

        [MaxLength(AppConsts.MaxFilterLength)]
        public string filenameFilter { get; set; }

        [MaxLength(AppConsts.MaxFilterLength)]
        public string physicalfilepathFilter { get; set; }

        [MaxLength(AppConsts.MaxFilterLength)]
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

        [MaxLength(AppConsts.MaxFilterLength)]
        public string TenantDocumentCategoryNameFilter { get; set; }

        public Guid? TenantDocumentCategoryIdFilter { get; set; }

    }
}