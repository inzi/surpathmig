using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetTenantDocumentForViewDto
    {
        public TenantDocumentDto TenantDocument { get; set; }

        public string TenantDocumentCategoryName { get; set; }

        public string Recordfilename { get; set; }

    }
}