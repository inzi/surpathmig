using Abp.Application.Services.Dto;
using inzibackend.MultiTenancy.Dto;

namespace inzibackend.Surpath.Dtos
{
    public class CohortTenantDepartmentLookupTableDto
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }
        public TenantInfoDto TenantInfoDto { get; set; }
    }
}