using System;

namespace inzibackend.Web.Areas.App.Models.TenantDocuments
{
    public class TenantDocumentsViewModel
    {
        public string FilterText { get; set; }
        public Guid? TenantDocumentCategoryId { get; set; }
    }
}