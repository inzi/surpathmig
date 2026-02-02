using System.Collections.Generic;
using MvvmHelpers;
using inzibackend.Models.NavigationMenu;

namespace inzibackend.Services.Navigation
{
    public interface IMenuProvider
    {
        ObservableRangeCollection<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}