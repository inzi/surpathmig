using inzibackend.Maui.Models.NavigationMenu;

namespace inzibackend.Maui.Services.Navigation;

public interface IMenuProvider
{
    List<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
}