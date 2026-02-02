using inzibackend.Surpath.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Models.Panels
{
    public class CreateOrEditPanelViewModel
    {
        public CreateOrEditPanelDto Panel { get; set; }

        public string TestCategoryName { get; set; }

        public List<PanelTestCategoryLookupTableDto> PanelTestCategoryList { get; set; }

        public bool IsEditMode => Panel.Id.HasValue;
    }
}