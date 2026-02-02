using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Surpath.Dtos
{
    public class GetDrugTestCategoryForEditOutput
    {
        public CreateOrEditDrugTestCategoryDto DrugTestCategory { get; set; }

        public string DrugName { get; set; }

        public string PanelName { get; set; }

        public string TestCategoryName { get; set; }

    }
}