using inzibackend.Surpath.Dtos;

using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Models.DrugPanels
{
    public class CreateOrEditDrugPanelViewModel
    {
        public CreateOrEditDrugPanelDto DrugPanel { get; set; }

        public string DrugName { get; set; }

        public string PanelName { get; set; }

        public bool IsEditMode => DrugPanel.Id.HasValue;
    }
}