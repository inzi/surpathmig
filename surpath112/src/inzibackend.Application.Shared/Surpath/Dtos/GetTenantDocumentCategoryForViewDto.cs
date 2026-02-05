using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetTenantDocumentCategoryForViewDto
    {
        public TenantDocumentCategoryDto TenantDocumentCategory { get; set; }

        public string UserName { get; set; }

    }
}