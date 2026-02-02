using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class GetRecordForEditOutput
    {
        public CreateOrEditRecordDto Record { get; set; }

        public string TenantDocumentCategoryName { get; set; }

        public string filedataFileName { get; set; }

        public Guid? TenantDocumentCategoryId { get; set; }
    }
}