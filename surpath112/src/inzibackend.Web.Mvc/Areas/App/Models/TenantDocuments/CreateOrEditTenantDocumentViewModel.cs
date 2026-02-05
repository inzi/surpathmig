using inzibackend.Surpath.Dtos;

using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Models.TenantDocuments
{
    public class CreateOrEditTenantDocumentViewModel
    {
        public CreateOrEditTenantDocumentDto TenantDocument { get; set; }

        public string TenantDocumentCategoryName { get; set; }

        public string Recordfilename { get; set; }

        public bool IsEditMode => TenantDocument.Id.HasValue;
    }
}