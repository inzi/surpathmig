using inzibackend.Maui.Services.Navigation;
using inzibackend.Maui.Services.Permission;
using inzibackend.Maui.Services.UI;

namespace inzibackend.Maui.Core.Components;

public class inzibackendMainLayoutPageComponentBase : inzibackendComponentBase
{
    protected PageHeaderService PageHeaderService { get; set; }

    protected DomManipulatorService DomManipulatorService { get; set; }

    protected INavigationService NavigationService { get; set; }

    protected IPermissionService PermissionService { get; set; }

    public inzibackendMainLayoutPageComponentBase()
    {
        PageHeaderService = Resolve<PageHeaderService>();
        DomManipulatorService = Resolve<DomManipulatorService>();
        NavigationService = Resolve<INavigationService>();
        PermissionService = Resolve<IPermissionService>();
    }

    protected async Task SetPageHeader(string title)
    {
        PageHeaderService.Title = title;
        PageHeaderService.SubTitle = string.Empty;
        PageHeaderService.ClearButton();
        await DomManipulatorService.ClearModalBackdrop(JS);
    }

    protected async Task SetPageHeader(string title, string subTitle)
    {
        PageHeaderService.Title = title;
        PageHeaderService.SubTitle = subTitle;
        PageHeaderService.ClearButton();
        await DomManipulatorService.ClearModalBackdrop(JS);
    }

    protected async Task SetPageHeader(string title, string subTitle, List<PageHeaderButton> buttons)
    {
        PageHeaderService.Title = title;
        PageHeaderService.SubTitle = subTitle;
        PageHeaderService.SetButtons(buttons);
        await DomManipulatorService.ClearModalBackdrop(JS);
    }
}