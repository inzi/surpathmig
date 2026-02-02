# Resources Documentation

## Overview
This folder contains resource management infrastructure for organizing and loading JavaScript and CSS files dynamically in Razor views, providing a structured approach to client-side resource dependencies.

## Files

### IWebResourceManager.cs
**Purpose**: Interface for web resource management service

**Methods**:
- `AddScript(string path)`: Register JavaScript file for inclusion
- `AddStyle(string path)`: Register CSS file for inclusion
- `GetScripts()`: Get all registered scripts for current request
- `GetStyles()`: Get all registered styles for current request

**Usage**: Inject into views or view components to declare dependencies

### WebResourceManager.cs
**Purpose**: Implementation of web resource manager

**Functionality**:
- Tracks scripts and styles per request
- Prevents duplicate inclusions
- Maintains load order
- Integrates with bundling/minification
- Scoped per HTTP request

**Example**:
```csharp
public class MyViewComponent : ViewComponent {
    private readonly IWebResourceManager _resourceManager;

    public IViewComponentResult Invoke() {
        _resourceManager.AddScript("~/view-resources/MyComponent.js");
        _resourceManager.AddStyle("~/view-resources/MyComponent.css");
        return View();
    }
}
```

### ScriptPaths.cs
**Purpose**: Constants for commonly-used script paths

**Constants**:
- jQuery and jQuery UI paths
- Bootstrap paths
- Application-specific library paths
- Third-party library paths
- View-specific script paths

**Benefits**:
- Centralized path management
- IntelliSense for script paths
- Refactoring support
- Prevents typos

**Example**:
```csharp
public static class ScriptPaths {
    public const string JQuery = "~/lib/jquery/dist/jquery.js";
    public const string Bootstrap = "~/lib/bootstrap/dist/js/bootstrap.bundle.js";
    public const string AbpWeb = "~/Abp/Framework/scripts/abp.js";
    public const string Datatables = "~/lib/datatables/js/jquery.dataTables.js";
}
```

## Architecture Notes

### Request-Scoped Resources
- Each HTTP request has its own resource manager instance
- Resources registered during request processing
- Rendered in layout before `</body>` or `</head>`
- Cleared after request completes

### Dependency Resolution
Views and components declare their dependencies:
```razor
@inject IWebResourceManager ResourceManager
@{
    ResourceManager.AddScript(ScriptPaths.Datatables);
}
```

Layout renders all dependencies:
```razor
@foreach (var script in ResourceManager.GetScripts()) {
    <script src="@script"></script>
}
```

### Loading Order
Scripts loaded in registration order:
1. Core libraries (jQuery, Bootstrap)
2. ABP framework scripts
3. Third-party plugins
4. Application common scripts
5. View-specific scripts

Styles loaded similarly:
1. Core styles (Bootstrap)
2. Theme styles
3. Plugin styles
4. View-specific styles

## Usage Across Codebase

### View Components
```csharp
public class DashboardWidgetViewComponent : ViewComponent {
    public IViewComponentResult Invoke() {
        ResourceManager.AddScript("~/view-resources/Dashboard/Widget.js");
        return View();
    }
}
```

### Razor Views
```razor
@section Scripts {
    @{
        ResourceManager.AddScript("~/view-resources/Users/Index.js");
    }
}
```

### Layout Pages
```razor
<!-- Render all registered styles -->
@foreach (var style in ResourceManager.GetStyles()) {
    <link href="@style" rel="stylesheet" />
}

<!-- Render all registered scripts -->
@foreach (var script in ResourceManager.GetScripts()) {
    <script src="@script"></script>
}
```

## Benefits

### Modular Resources
- Components declare their own dependencies
- No need to modify layout for new views
- Reusable components are self-contained

### Duplicate Prevention
- Same script registered multiple times = included once
- Reduces page weight
- Improves load times

### Centralized Management
- All resource paths in one place
- Easy to update library versions
- Support for CDN switching

## Dependencies
- ASP.NET Core DI system
- Scoped service registration
- Razor view engine

## Testing
- Unit test resource registration
- Verify no duplicates
- Test load order
- Integration tests for full page rendering

## Related Documentation
- [TagHelpers/CLAUDE.md](../TagHelpers/CLAUDE.md): Script and link tag helpers
- [Extensions/CLAUDE.md](../Extensions/CLAUDE.md): HTML helper extensions
- [Views/CLAUDE.md](../Views/CLAUDE.md): Razor views using resource manager