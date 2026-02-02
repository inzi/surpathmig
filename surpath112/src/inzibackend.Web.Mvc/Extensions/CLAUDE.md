# Extensions Documentation

## Overview
This folder contains extension methods for HTML helpers and other utility extensions used throughout the MVC application to enhance Razor view functionality.

## Files

### HtmlHelperExtensions.cs
**Purpose**: Extension methods for `IHtmlHelper` to simplify common view operations

**Key Extensions**:
- **IsActive()**: Determines if a menu item is active based on current controller/action
- **GetActiveClass()**: Returns CSS class for active navigation items
- **RenderScripts()**: Renders script tags for bundled JavaScript
- **RenderStyles()**: Renders link tags for bundled CSS
- **GetCurrentTheme()**: Retrieves current UI theme from session/settings
- **LocalizedString()**: Helper for localization in views
- **AbpButton()**: Renders ABP-styled buttons with permissions
- **Permission()**: Checks if user has permission for conditional rendering

**Usage Example**:
```razor
@Html.IsActive("Home", "Index") ? "active" : ""
@Html.AbpButton("EditUser", "Edit", permissions: "Pages.Users.Edit")
```

**Benefits**:
- Reduces code duplication in views
- Centralizes common rendering logic
- Simplifies permission checks
- Improves view readability

## Architecture Notes

### Extension Method Pattern
- Extends `IHtmlHelper` interface
- Allows fluent syntax in Razor views
- Integrates with ABP framework helpers
- Type-safe compared to ViewBag/ViewData

### Razor Integration
- Methods available in all Razor views automatically
- IntelliSense support in IDE
- Strongly-typed parameters
- Compile-time checking

## Dependencies
- ASP.NET Core MVC
- ABP Framework helpers
- Localization system
- Permission system

## Related Documentation
- [TagHelpers/CLAUDE.md](../TagHelpers/CLAUDE.md): Tag Helper extensions
- [Views/CLAUDE.md](../Views/CLAUDE.md): Razor views using these extensions