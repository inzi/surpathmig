using System.Collections.Generic;
using inzibackend.Caching.Dto;

namespace inzibackend.Web.Areas.App.Models.Maintenance
{
    public class MaintenanceViewModel
    {
        public IReadOnlyList<CacheDto> Caches { get; set; }
    }
}