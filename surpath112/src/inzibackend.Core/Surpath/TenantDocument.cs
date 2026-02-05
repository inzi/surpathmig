using inzibackend.Surpath;
using inzibackend.Surpath;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace inzibackend.Surpath
{
    [Table("TenantDocuments")]
    [Audited]
    public class TenantDocument : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Name { get; set; }

        public virtual bool AuthorizedOnly { get; set; }

        public virtual string Description { get; set; }

        public virtual Guid TenantDocumentCategoryId { get; set; }

        [ForeignKey("TenantDocumentCategoryId")]
        public TenantDocumentCategory TenantDocumentCategoryFk { get; set; }

        public virtual Guid? RecordId { get; set; }

        [ForeignKey("RecordId")]
        public Record RecordFk { get; set; }

    }
}