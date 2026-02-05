using Abp.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;

namespace inzibackend.Surpath
{
    [Table("ProcessedTransactions")]
    [Audited]
    public class ProcessedTransactions : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public string TransactionId { get; set; }

    }
}
