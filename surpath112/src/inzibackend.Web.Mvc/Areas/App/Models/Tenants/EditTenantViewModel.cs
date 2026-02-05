using System.Collections.Generic;
using inzibackend.Editions.Dto;
using inzibackend.MultiTenancy.Dto;
using inzibackend.Surpath.Dtos;

namespace inzibackend.Web.Areas.App.Models.Tenants
{
    public class EditTenantViewModel
    {
        public TenantEditDto Tenant { get; set; }

        public IReadOnlyList<SubscribableEditionComboboxItemDto> EditionItems { get; set; }

        public List<TenantSurpathServiceDto> ServiceItems { get; set; }

        public EditTenantViewModel(TenantEditDto tenant, IReadOnlyList<SubscribableEditionComboboxItemDto> editionItems, List<TenantSurpathServiceDto> serviceItems)
        {
            Tenant = tenant;
            EditionItems = editionItems;
            ServiceItems = serviceItems;
        }
    }
}