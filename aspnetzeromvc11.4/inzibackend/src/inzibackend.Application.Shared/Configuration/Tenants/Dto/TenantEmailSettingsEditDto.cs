using Abp.Auditing;
using inzibackend.Configuration.Dto;

namespace inzibackend.Configuration.Tenants.Dto
{
    public class TenantEmailSettingsEditDto : EmailSettingsEditDto
    {
        public bool UseHostDefaultEmailSettings { get; set; }
    }
}