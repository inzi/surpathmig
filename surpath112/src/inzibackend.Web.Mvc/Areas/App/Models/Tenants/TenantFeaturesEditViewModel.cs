using Abp.AutoMapper;
using inzibackend.MultiTenancy;
using inzibackend.MultiTenancy.Dto;
using inzibackend.Web.Areas.App.Models.Common;

namespace inzibackend.Web.Areas.App.Models.Tenants
{
    [AutoMapFrom(typeof (GetTenantFeaturesEditOutput))]
    public class TenantFeaturesEditViewModel : GetTenantFeaturesEditOutput, IFeatureEditViewModel
    {
        public Tenant Tenant { get; set; }
    }
}