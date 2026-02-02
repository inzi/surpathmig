# Views/Shared/Components Documentation

## Overview
This folder contains View Components for the root-level (public) pages of the application, primarily authentication and tenant management components. These are reusable UI elements that encapsulate both rendering logic and data retrieval.

## Subfolders

### AccountLanguages/
**Purpose**: Language selection component for authentication pages

**Functionality**:
- Displays available application languages
- Shows current language with visual indicator
- Allows switching between languages
- Persists language choice across sessions

**Usage**: Login, register, password reset pages

**See**: [AccountLanguages/CLAUDE.md](AccountLanguages/CLAUDE.md)

### AccountLogo/
**Purpose**: Logo display component for authentication pages

**Functionality**:
- Shows application or tenant-specific logo
- Supports multi-tenancy (different logos per tenant)
- Falls back to default logo if tenant has none
- Responsive image sizing

**Usage**: All authentication pages (login, register, etc.)

**See**: [AccountLogo/CLAUDE.md](AccountLogo/CLAUDE.md)

### TenantChange/
**Purpose**: Tenant switching component for multi-tenant users

**Functionality**:
- Displays current tenant context
- Provides modal for selecting different tenant
- Allows switching between tenant and host contexts
- Validates user has access to selected tenant

**Usage**: Header of authenticated pages for multi-tenant users

**See**: [TenantChange/CLAUDE.md](TenantChange/CLAUDE.md)

## Architecture Notes

### View Component Pattern
View Components are similar to partial views but include logic:
```csharp
public class MyViewComponent : ViewComponent {
    public async Task<IViewComponentResult> InvokeAsync(params) {
        // Data retrieval logic
        var model = await _service.GetDataAsync();

        // Return view with model
        return View(model);
    }
}
```

### Advantages Over Partial Views
- **Testable**: Logic can be unit tested
- **Dependency Injection**: Constructor injection supported
- **Async Support**: Full async/await support
- **Separation of Concerns**: Logic separated from view
- **Reusability**: Can be invoked from multiple locations

### Invocation Methods
**From Razor View**:
```razor
@await Component.InvokeAsync("AccountLanguages")
@await Component.InvokeAsync(typeof(AccountLanguagesViewComponent))
```

**From Controller**:
```csharp
return ViewComponent("AccountLanguages");
```

**As Tag Helper**:
```razor
<vc:account-languages />
```

### View Discovery
ASP.NET Core searches for component views in:
1. `/Views/{ControllerName}/Components/{ComponentName}/Default.cshtml`
2. `/Views/Shared/Components/{ComponentName}/Default.cshtml`
3. `/Pages/Shared/Components/{ComponentName}/Default.cshtml`

Custom view names supported:
```csharp
return View("CustomViewName", model);
```

## Usage Across Codebase

### Layout Pages
Components typically invoked from layout pages:
```razor
<!-- _Layout.cshtml -->
<div class="header-logo">
    @await Component.InvokeAsync("AccountLogo")
</div>

<div class="language-selector">
    @await Component.InvokeAsync("AccountLanguages")
</div>
```

### Shared Layouts
Account-related layouts:
- `_Layout.cshtml`: Main layout
- `_AccountLayout.cshtml`: Authentication-specific layout

Both can use these components for consistent branding.

## Naming Conventions

### Component Class
- Suffix with `ViewComponent`
- Example: `AccountLanguagesViewComponent`

### Component Name
- Omit `ViewComponent` suffix when invoking
- Example: `@await Component.InvokeAsync("AccountLanguages")`

### View Folder
- Matches component name (without suffix)
- Example: `/Components/AccountLanguages/`

### Default View
- Named `Default.cshtml`
- Custom views allowed

## Dependencies

### Component Classes
- `ViewComponent` base class (ASP.NET Core)
- ABP services (localization, session, etc.)
- Application services for data retrieval

### Component Views
- Razor view engine
- Tag helpers
- HTML helpers
- Partial views (can be used within components)

## Testing

### Unit Testing Components
```csharp
[Fact]
public async Task AccountLanguages_Returns_AllLanguages() {
    // Arrange
    var component = new AccountLanguagesViewComponent(_languageManager);

    // Act
    var result = await component.InvokeAsync();

    // Assert
    var viewResult = Assert.IsType<ViewViewComponentResult>(result);
    var model = Assert.IsType<LanguageSelectionViewModel>(viewResult.ViewData.Model);
    Assert.NotEmpty(model.Languages);
}
```

### Integration Testing
Test components in context of full page renders.

## Performance Considerations
- Components execute on every request
- Cache data when possible (per-request caching)
- Avoid heavy database queries
- Use async methods for I/O operations

## Related Documentation
- [Views/Shared/CLAUDE.md](../CLAUDE.md): Shared views
- [Views/Account/CLAUDE.md](../../Account/CLAUDE.md): Authentication views
- [Areas/App/Views/Shared/Components/CLAUDE.md](../../../Areas/App/Views/Shared/Components/CLAUDE.md): App area components