using System;
using System.Collections.Generic;
using inzibackend.Surpath.Dtos;

namespace inzibackend.Web.Areas.App.Models.RecordRequirements
{
    public class CategoryManagementModalViewModel
    {
        public Guid RequirementId { get; set; }
        public string RequirementName { get; set; }
        public List<CategoryLookupDto> Categories { get; set; } = new List<CategoryLookupDto>();
    }
} 