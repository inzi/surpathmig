using inzibackend.Surpath.Dtos;

using Abp.Extensions;
using inzibackend.Editions.Dto;
using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace inzibackend.Web.Areas.App.Models.SurpathServices
{
    public class CreateOrEditSurpathServiceModalViewModel
    {
        public CreateOrEditSurpathServiceDto SurpathService { get; set; }

        public string TenantDepartmentName { get; set; }

        public string CohortName { get; set; }

        public string UserName { get; set; }

        public string RecordCategoryRuleName { get; set; }

        public bool IsEditMode => SurpathService.Id.HasValue;

        public List<ComboboxItemDto> FeatureList { get; set; } = new List<ComboboxItemDto>();
    }
}