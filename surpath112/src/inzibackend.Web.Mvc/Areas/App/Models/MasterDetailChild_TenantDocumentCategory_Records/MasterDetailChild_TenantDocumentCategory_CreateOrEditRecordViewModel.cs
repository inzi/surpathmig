using inzibackend.Surpath.Dtos;

using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Models.Records
{
    public class MasterDetailChild_TenantDocumentCategory_CreateOrEditRecordViewModel
    {
        public CreateOrEditRecordDto Record { get; set; }

        public string filedataFileName { get; set; }
        public string filedataFileAcceptedTypes { get; set; }

        public bool IsEditMode => Record.Id.HasValue;
    }
}