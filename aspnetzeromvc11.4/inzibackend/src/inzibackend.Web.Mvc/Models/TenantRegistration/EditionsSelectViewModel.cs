using Abp.AutoMapper;
using inzibackend.MultiTenancy.Dto;

namespace inzibackend.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(EditionsSelectOutput))]
    public class EditionsSelectViewModel : EditionsSelectOutput
    {
    }
}
