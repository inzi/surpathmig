using inzibackend.Surpath.Dtos;

using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Models.TenantSurpathServices
{
    public class CreateOrEditTenantSurpathServiceModalViewModel
    {
        public CreateOrEditTenantSurpathServiceDto TenantSurpathService { get; set; }

        public string SurpathServiceName { get; set; }

        public string TenantDepartmentName { get; set; }

        public string CohortName { get; set; }

        public string UserName { get; set; }

        public string RecordCategoryRuleName { get; set; }

        public bool IsEditMode => TenantSurpathService.Id.HasValue;
    }
}