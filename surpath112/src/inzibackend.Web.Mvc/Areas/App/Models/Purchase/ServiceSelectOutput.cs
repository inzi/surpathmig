

using inzibackend.Surpath.Dtos;
using System.Collections.Generic;

namespace inzibackend.Web.Areas.App.Models.Purchase
{
    public class ServiceSelectOutput
    {
        public ServiceSelectOutput()
        {
            SurpathServices = new List<SurpathServiceDto>();
        }

        public List<SurpathServiceDto> SurpathServices { get; set; }
    }
}
