using Microsoft.AspNetCore.Components;
using inzibackend.Maui.Core.Components;
using inzibackend.Maui.Core.Threading;
using inzibackend.Maui.Services.UI;


namespace inzibackend.Maui.Pages.MySettings;

public partial class ThemeSwitch : inzibackendComponentBase
{
    private string _selectedTheme = ThemeService.GetUserTheme();

    private string[] _themes = ThemeService.GetAllThemes();

    public string SelectedTheme
    {
        get => _selectedTheme;
        set
        {
            _selectedTheme = value;
            AsyncRunner.Run(ThemeService.SetUserTheme(JS, SelectedTheme));
        }
    }
}