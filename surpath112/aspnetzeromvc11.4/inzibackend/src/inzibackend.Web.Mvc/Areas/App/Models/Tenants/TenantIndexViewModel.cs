using System.Collections.Generic;
using inzibackend.Editions.Dto;

namespace inzibackend.Web.Areas.App.Models.Tenants
{
    public class TenantIndexViewModel
    {
        public List<SubscribableEditionComboboxItemDto> EditionItems { get; set; }
    }
}