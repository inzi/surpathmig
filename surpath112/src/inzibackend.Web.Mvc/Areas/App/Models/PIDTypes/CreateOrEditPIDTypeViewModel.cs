using inzibackend.Surpath.Dtos;

using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Models.PidTypes
{
    public class CreateOrEditPidTypeModalViewModel
    {
        public CreateOrEditPidTypeDto PidType { get; set; }

        public bool IsEditMode => PidType.Id.HasValue;
    }
}