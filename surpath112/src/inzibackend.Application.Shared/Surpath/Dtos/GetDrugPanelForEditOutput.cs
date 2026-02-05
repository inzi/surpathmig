using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class GetDrugPanelForEditOutput
    {
        public CreateOrEditDrugPanelDto DrugPanel { get; set; }

        public string DrugName { get; set; }

        public string PanelName { get; set; }

    }
}