using inzibackend.Surpath.Dtos;

using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Models.CodeTypes
{
    public class CreateOrEditCodeTypeViewModel
    {
        public CreateOrEditCodeTypeDto CodeType { get; set; }

        public bool IsEditMode => CodeType.Id.HasValue;
    }
}