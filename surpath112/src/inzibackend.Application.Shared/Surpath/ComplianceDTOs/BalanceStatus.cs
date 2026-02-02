using inzibackend.Surpath.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inzibackend.Surpath.Compliance
{
    public class BalanceStatus
    {
        public bool IsCurrent { get; set; }
        public List<TenantSurpathServiceDto> TenantSurpathServiceDtos { get; set; } = new List<TenantSurpathServiceDto>();
    }
}
