using System.Collections.Generic;
using inzibackend.MultiTenancy.Dto;

namespace inzibackend.Web.Areas.App.Models.TenantSurpathServices
{
    public class PricingManagementViewModel
    {
        public List<TenantListDto> Tenants { get; set; }

        public PricingManagementViewModel()
        {
            Tenants = new List<TenantListDto>();
        }
    }
}