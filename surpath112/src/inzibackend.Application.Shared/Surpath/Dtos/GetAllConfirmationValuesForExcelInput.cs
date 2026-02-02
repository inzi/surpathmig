using Abp.Application.Services.Dto;
using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetAllConfirmationValuesForExcelInput
    {
        public string Filter { get; set; }

        public double? MaxScreenValueFilter { get; set; }
        public double? MinScreenValueFilter { get; set; }

        public double? MaxConfirmValueFilter { get; set; }
        public double? MinConfirmValueFilter { get; set; }

        public int? UnitOfMeasurementFilter { get; set; }

        public string DrugNameFilter { get; set; }

        public string TestCategoryNameFilter { get; set; }

    }
}