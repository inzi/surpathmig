using inzibackend.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using Abp.Organizations;

namespace inzibackend.Surpath
{
    [Table("TenantDocumentCategories")]
    [Audited]
    public class TenantDocumentCategory : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual bool AuthorizedOnly { get; set; }

        public virtual bool HostOnly { get; set; }

        public virtual long? UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserFk { get; set; }

    }
}