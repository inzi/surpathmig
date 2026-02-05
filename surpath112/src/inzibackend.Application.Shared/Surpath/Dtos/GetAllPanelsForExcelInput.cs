using Abp.Application.Services.Dto;
using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetAllPanelsForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public double? MaxCostFilter { get; set; }
        public double? MinCostFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public string TestCategoryNameFilter { get; set; }

    }
}