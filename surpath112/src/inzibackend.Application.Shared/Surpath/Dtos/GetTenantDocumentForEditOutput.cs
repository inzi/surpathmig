using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class GetTenantDocumentForEditOutput
    {
        public CreateOrEditTenantDocumentDto TenantDocument { get; set; }

        public string TenantDocumentCategoryName { get; set; }

        public string Recordfilename { get; set; }

        public Guid TenantDocumentCategoryId { get; set; }

    }
}