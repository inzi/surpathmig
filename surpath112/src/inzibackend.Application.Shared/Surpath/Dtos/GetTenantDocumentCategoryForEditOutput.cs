using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class GetTenantDocumentCategoryForEditOutput
    {
        public CreateOrEditTenantDocumentCategoryDto TenantDocumentCategory { get; set; }

        public string UserName { get; set; }

    }
}