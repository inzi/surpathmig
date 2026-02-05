using inzibackend.Surpath.Dtos;

using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Models.RecordStatuses
{
    public class CreateOrEditRecordStatusViewModel
    {
        public CreateOrEditRecordStatusDto RecordStatus { get; set; }

        public string TenantDepartmentName { get; set; }

        public bool IsEditMode => RecordStatus.Id.HasValue;
    }
}