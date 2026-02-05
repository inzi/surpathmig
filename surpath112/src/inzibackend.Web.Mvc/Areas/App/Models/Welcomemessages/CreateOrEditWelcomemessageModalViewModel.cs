using inzibackend.Surpath.Dtos;

using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Models.Welcomemessages
{
    public class CreateOrEditWelcomemessageModalViewModel
    {
        public CreateOrEditWelcomemessageDto Welcomemessage { get; set; }

        public bool IsEditMode => Welcomemessage.Id.HasValue;
    }
}