using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using inzibackend.Sessions.Dto;

namespace inzibackend.Models.Common
{
    [AutoMapFrom(typeof(TenantLoginInfoDto)),
     AutoMapTo(typeof(TenantLoginInfoDto))]
    public class TenantLoginInfoPersistanceModel : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }
    }
}