using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class CreateOrEditTenantDocumentCategoryDto : EntityDto<Guid?>
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public bool AuthorizedOnly { get; set; }

        public bool HostOnly { get; set; }

        public long? UserId { get; set; }

    }
}