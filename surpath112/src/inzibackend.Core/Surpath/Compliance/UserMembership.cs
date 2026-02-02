using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inzibackend.Surpath.Compliance
{
    public class UserMembership
    {
        public Guid? TenantDepartmentId { get; set; }
        public string TenantDepartmentName { get; set; }
        public Guid? CohortId { get; set; }
        public string CohortName { get; set; }
        public Guid? CohortUserId { get; set; }
        public Guid? CohortDepartmentId { get; set; }
        public long UserId { get; set; }

    }
}
