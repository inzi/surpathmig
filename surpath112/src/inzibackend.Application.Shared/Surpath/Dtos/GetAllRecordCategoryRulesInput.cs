using Abp.Application.Services.Dto;
using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetAllRecordCategoryRulesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public int? NotifyFilter { get; set; }

        public int? MaxExpireInDaysFilter { get; set; }
        public int? MinExpireInDaysFilter { get; set; }

        public int? MaxWarnDaysBeforeFirstFilter { get; set; }
        public int? MinWarnDaysBeforeFirstFilter { get; set; }

        public int? ExpiresFilter { get; set; }

        public int? RequiredFilter { get; set; }

        public int? IsSurpathOnlyFilter { get; set; }

        public int? MaxWarnDaysBeforeSecondFilter { get; set; }
        public int? MinWarnDaysBeforeSecondFilter { get; set; }

        public int? MaxWarnDaysBeforeFinalFilter { get; set; }
        public int? MinWarnDaysBeforeFinalFilter { get; set; }

        public string MetaDataFilter { get; set; }

    }
}