using Abp.Application.Services.Dto;
using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetAllDrugPanelsForExcelInput
    {
        public string Filter { get; set; }

        public string DrugNameFilter { get; set; }

        public string PanelNameFilter { get; set; }

    }
}