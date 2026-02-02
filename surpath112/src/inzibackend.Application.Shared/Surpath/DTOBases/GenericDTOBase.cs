using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inzibackend.Surpath.DTOBases
{
    public abstract class GenericDTOBase
    {
        public string TenantName { get; set; }
        public string TenantDepartmentName { get; set; }
        public Guid? TenantId { get; set; }
        public Guid? ExcludeIdGuid { get; set; }
        public long? ExcludeIdLong { get; set; }
    }
}
