# UiCustomization Documentation

## Overview
This folder contains the UI theme customization infrastructure, including a factory pattern for selecting and instantiating theme-specific customizers based on user/tenant/application settings.

## Contents

### Files

#### UiThemeCustomizerFactory.cs
- **Purpose**: Factory for creating theme customizer instances based on selected theme
- **Key Methods**:
  - **GetCurrentUiCustomizer()**: Returns customizer for current user's selected theme
  - **GetUiCustomizer(string theme)**: Returns customizer for specific theme name
- **Supported Themes**: Default, Theme0, Theme2-13 (14 total theme variants)
- **Pattern**: Factory pattern with service locator (IServiceProvider)

### Key Components
- **Theme Selection**: Based on AppSettings.UiManagement.Theme setting
- **Service Resolution**: Uses DI container to resolve theme customizers
- **Fallback**: Defaults to ThemeDefaultUiCustomizer if theme not recognized

### Dependencies
- **Abp.Configuration**: Settings management (ISettingManager)
- **Microsoft.Extensions.DependencyInjection**: Service provider for DI
- **inzibackend.Configuration**: AppSettings constants
- **inzibackend.UiCustomization**: IUiThemeCustomizerFactory interface
- **inzibackend.Web.UiCustomization.Metronic**: Theme customizer implementations

## Subfolders

### Metronic
Contains 14 Metronic theme variant implementations with multi-level customization support.
- **Base Class**: UiThemeCustomizerBase - Settings management infrastructure
- **Default Theme**: ThemeDefaultUiCustomizer - Left sidebar layout
- **Theme Variants**: Theme2-13 - Alternative visual configurations
- **Settings Scope**: Application, tenant, and user-level customization
- **Dark Mode**: Automatic color scheme switching with aside skin adjustment

Key features:
- Hierarchical settings (app → tenant → user)
- Configurable layouts, menus, headers, footers
- Dark mode with dependent setting adjustments
- Reset operations to parent-level defaults

## Architecture Notes

### Factory Pattern
```
Client requests theme
  ↓
UiThemeCustomizerFactory.GetCurrentUiCustomizer()
  ↓
Read theme setting from database
  ↓
Match theme name to customizer
  ↓
Resolve customizer from DI container
  ↓
Return IUiCustomizer implementation
```

### Theme Selection Hierarchy
1. **User Setting**: Personal theme preference (highest priority)
2. **Tenant Setting**: Organization-wide default theme
3. **Application Setting**: System-wide default (fallback)

### Service Locator Pattern
```csharp
_serviceProvider.GetService<Theme8UiCustomizer>();
```
- Resolves theme customizers from DI container
- Allows late binding (theme selected at runtime)
- Each customizer registered as transient or scoped
- Factory acts as abstraction over service resolution

## Business Logic

### Theme Matching
```csharp
private IUiCustomizer GetUiCustomizerInternal(string theme)
{
    if (theme.Equals(AppConsts.Theme8, StringComparison.InvariantCultureIgnoreCase))
        return _serviceProvider.GetService<Theme8UiCustomizer>();

    // ... more theme checks ...

    return _serviceProvider.GetService<ThemeDefaultUiCustomizer>(); // Fallback
}
```

**Case-Insensitive Matching**: Handles "theme8", "THEME8", "Theme8" identically
**Explicit Checks**: Each theme has explicit if statement (no reflection/dynamic lookup)
**Fallback**: Unknown themes use ThemeDefaultUiCustomizer

### Current User Theme
```csharp
public async Task<IUiCustomizer> GetCurrentUiCustomizer()
{
    var theme = await _settingManager.GetSettingValueAsync(AppSettings.UiManagement.Theme);
    return GetUiCustomizerInternal(theme);
}
```

**Setting Resolution**: ABP SettingManager automatically handles:
- User-level setting (if exists)
- Falls back to tenant-level
- Falls back to application-level
- Returns default if none set

### Theme by Name
```csharp
public IUiCustomizer GetUiCustomizer(string theme)
{
    return GetUiCustomizerInternal(theme);
}
```
**Use Cases**:
- Preview theme before applying
- Load tenant default theme
- Administrative theme management

## Usage Across Codebase

### Consumed By
- **UiCustomizationAppService**: Manages theme settings
- **View Rendering**: Gets theme settings for layout
- **MVC Controllers**: Apply theme to views
- **Session Information**: Include theme in session data
- **Configuration Endpoints**: Theme selection API

### Typical Usage Pattern
```csharp
public class UiCustomizationController
{
    private readonly IUiThemeCustomizerFactory _themeFactory;

    public UiCustomizationController(IUiThemeCustomizerFactory themeFactory)
    {
        _themeFactory = themeFactory;
    }

    public async Task<IActionResult> GetThemeSettings()
    {
        var customizer = await _themeFactory.GetCurrentUiCustomizer();
        var settings = await customizer.GetUiSettings();
        return Json(settings);
    }

    public async Task<IActionResult> PreviewTheme(string themeName)
    {
        var customizer = _themeFactory.GetUiCustomizer(themeName);
        var settings = await customizer.GetUiSettings();
        return PartialView("_ThemePreview", settings);
    }
}
```

## Available Themes

### Theme Catalog
| Theme | Description | Layout Type |
|-------|-------------|-------------|
| Default | Standard left sidebar | Left navigation |
| Theme0 | Alternative default | Varies |
| Theme2 | Variant 2 | Varies |
| Theme3 | Variant 3 | Varies |
| Theme4 | Variant 4 | Varies |
| Theme5 | Variant 5 | Varies |
| Theme6 | Variant 6 | Varies |
| Theme7 | Variant 7 | Varies |
| Theme8 | Variant 8 | Varies |
| Theme9 | Variant 9 | Varies |
| Theme10 | Variant 10 | Varies |
| Theme11 | Variant 11 | Varies |
| Theme12 | Variant 12 | Varies |
| Theme13 | Variant 13 | Varies |

**Note**: Each theme is a Metronic template variant with different visual styling, colors, and layout configurations.

## Multi-Tenant Considerations

### Tenant Branding
- Each tenant can select default theme
- Tenant users inherit tenant default
- Users can override with personal preference
- Host can set system-wide default

### Theme Isolation
- Theme settings stored per user/tenant
- No cross-tenant theme contamination
- Each tenant's theme choices independent
- Settings hierarchy respected

## Performance Considerations

### Factory Resolution
- Factory is lightweight (simple if/else chain)
- Theme customizers resolved from DI (cached by container)
- Settings cached by ABP SettingManager
- Minimal overhead per theme selection

### Theme Loading
- Theme selected once per page load
- Settings loaded from cache (not database every time)
- Customizer reused within request scope
- No expensive operations during theme selection

## Extensibility

### Adding a New Theme
1. Create theme customizer class:
```csharp
public class Theme14UiCustomizer : UiThemeCustomizerBase, IUiCustomizer
{
    public Theme14UiCustomizer(ISettingManager settingManager)
        : base(settingManager, AppConsts.Theme14)
    {
    }

    // Implement IUiCustomizer methods...
}
```

2. Add theme constant:
```csharp
// AppConsts.cs
public const string Theme14 = "Theme14";
```

3. Register in factory:
```csharp
if (theme.Equals(AppConsts.Theme14, StringComparison.InvariantCultureIgnoreCase))
{
    return _serviceProvider.GetService<Theme14UiCustomizer>();
}
```

4. Register in DI container:
```csharp
// Startup.cs or Module
services.AddTransient<Theme14UiCustomizer>();
```

5. Add frontend CSS/JavaScript for theme

### Custom Theme Selector
For more advanced theme selection:
```csharp
public class AdvancedUiThemeCustomizerFactory : IUiThemeCustomizerFactory
{
    public async Task<IUiCustomizer> GetCurrentUiCustomizer()
    {
        var theme = await DetermineThemeBasedOnMultipleFactors();
        return GetUiCustomizer(theme);
    }

    private async Task<string> DetermineThemeBasedOnMultipleFactors()
    {
        // Could consider:
        // - User preference
        // - Time of day (dark mode at night)
        // - Device type (mobile vs desktop themes)
        // - Tenant settings
        // - Feature flags
        return selectedTheme;
    }
}
```

## Security Considerations

### Theme Settings
- Theme selection is cosmetic (no security implications)
- Settings validated before storage
- No script injection risk (themes are server-side classes)
- Users cannot inject custom themes (only select from predefined list)

### Permission Checks
- Theme selection typically doesn't require special permissions
- Administrative theme management may require admin role
- Tenant theme defaults require tenant admin permission

## Testing Considerations

### Unit Testing
```csharp
[Fact]
public void Should_Return_Correct_Theme_Customizer()
{
    // Arrange
    var serviceProvider = CreateServiceProviderWithThemes();
    var settingManager = Substitute.For<ISettingManager>();
    var factory = new UiThemeCustomizerFactory(settingManager, serviceProvider);

    // Act
    var customizer = factory.GetUiCustomizer("Theme8");

    // Assert
    Assert.IsType<Theme8UiCustomizer>(customizer);
}

[Fact]
public void Should_Return_Default_For_Unknown_Theme()
{
    // Arrange
    var factory = CreateFactory();

    // Act
    var customizer = factory.GetUiCustomizer("UnknownTheme");

    // Assert
    Assert.IsType<ThemeDefaultUiCustomizer>(customizer);
}
```

### Integration Testing
- Test theme selection via API
- Verify settings persistence across requests
- Test theme inheritance (user → tenant → app)
- Validate theme preview functionality

## Troubleshooting

### Theme Not Applied
**Symptom**: User selects theme but it doesn't apply
- **Cause 1**: Theme customizer not registered in DI
  - **Solution**: Add `services.AddTransient<ThemeXUiCustomizer>()` to startup
- **Cause 2**: Theme constant mismatch
  - **Solution**: Verify AppConsts.ThemeX matches database value
- **Cause 3**: Frontend CSS not loaded
  - **Solution**: Ensure theme CSS files are deployed

### Wrong Theme Displayed
**Symptom**: Different theme shown than selected
- **Cause**: Settings cache not cleared after change
  - **Solution**: Clear ABP settings cache or restart application

### Default Theme Used Always
**Symptom**: Theme selection ignored, always uses default
- **Cause**: Factory fallback always triggered
  - **Solution**: Debug theme name matching (check case sensitivity)