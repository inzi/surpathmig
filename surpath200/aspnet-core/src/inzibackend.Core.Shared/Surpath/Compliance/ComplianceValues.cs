using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inzibackend.Surpath.Compliance
{
    public class ComplianceValues
    {
        public bool Drug { get; set; } = false;
        public bool Background { get; set; } = false;
        public bool Immunization { get; set; } = false;
        public bool InCompliance { get; set; } = false;
        public long? UserId { get; set; }
        public int TenantId { get; set; }
    }
}
