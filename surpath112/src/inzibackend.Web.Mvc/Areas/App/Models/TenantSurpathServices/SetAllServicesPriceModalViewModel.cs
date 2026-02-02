using System.Collections.Generic;
using inzibackend.Surpath.Dtos;

namespace inzibackend.Web.Areas.App.Models.TenantSurpathServices
{
    public class SetAllServicesPriceModalViewModel
    {
        public string TargetType { get; set; }
        public string TargetId { get; set; }
        public string TargetName { get; set; }
        public int TenantId { get; set; }
        public List<SurpathServiceDto> Services { get; set; }

        public SetAllServicesPriceModalViewModel()
        {
            Services = new List<SurpathServiceDto>();
        }
    }
}