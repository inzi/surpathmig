using Abp.Application.Services.Dto;
using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetAllSurpathServicesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public double? MaxPriceFilter { get; set; }
        public double? MinPriceFilter { get; set; }

        public decimal? MaxDiscountFilter { get; set; }
        public decimal? MinDiscountFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public int? IsEnabledByDefaultFilter { get; set; }

        public string TenantDepartmentNameFilter { get; set; }

        public string CohortNameFilter { get; set; }

        public string UserNameFilter { get; set; }

        public string RecordCategoryRuleNameFilter { get; set; }

        public long? TenantId { get; set; }

    }
}