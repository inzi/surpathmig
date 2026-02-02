using System;
using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos
{
    public class TenantDocumentDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        public bool AuthorizedOnly { get; set; }

        public string Description { get; set; }

        public Guid TenantDocumentCategoryId { get; set; }

        public Guid? RecordId { get; set; }

        public Guid? BinaryObjId { get; set; }
    }
}