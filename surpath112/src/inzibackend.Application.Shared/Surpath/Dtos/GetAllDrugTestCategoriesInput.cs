using Abp.Application.Services.Dto;
using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetAllDrugTestCategoriesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string DrugNameFilter { get; set; }

        public string PanelNameFilter { get; set; }

        public string TestCategoryNameFilter { get; set; }

    }
}