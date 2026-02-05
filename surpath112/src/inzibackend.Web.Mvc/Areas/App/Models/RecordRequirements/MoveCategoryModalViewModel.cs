using System;
using System.Collections.Generic;
using inzibackend.Surpath.Dtos;

namespace inzibackend.Web.Areas.App.Models.RecordRequirements
{
    public class MoveCategoryModalViewModel
    {
        public List<Guid> CategoryIds { get; set; } = new List<Guid>();
        public List<CategoryLookupDto> SelectedCategories { get; set; } = new List<CategoryLookupDto>();
        public List<RecordRequirementDto> AvailableTargetRequirements { get; set; } = new List<RecordRequirementDto>();
        public CategoryMoveValidationDto ValidationInfo { get; set; }
    }
} 