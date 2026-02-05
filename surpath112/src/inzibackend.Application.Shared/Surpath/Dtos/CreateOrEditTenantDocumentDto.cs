using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class CreateOrEditTenantDocumentDto : EntityDto<Guid?>
    {

        public string Name { get; set; }

        public bool AuthorizedOnly { get; set; }

        public string Description { get; set; }

        public Guid TenantDocumentCategoryId { get; set; }

        public Guid? RecordId { get; set; }

    }
}