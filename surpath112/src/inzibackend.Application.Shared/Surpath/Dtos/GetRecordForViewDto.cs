using System;

namespace inzibackend.Surpath.Dtos
{
    public class GetRecordForViewDto
    {
        public RecordDto Record { get; set; }

        public string TenantDocumentCategoryName { get; set; }

    }
}