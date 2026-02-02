using inzibackend.Surpath.Dtos;

using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Models.TenantDocumentCategories
{
    public class CreateOrEditTenantDocumentCategoryViewModel
    {
        public CreateOrEditTenantDocumentCategoryDto TenantDocumentCategory { get; set; }

        public string UserName { get; set; }

        public bool IsEditMode => TenantDocumentCategory.Id.HasValue;
    }
}