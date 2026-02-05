# TagHelpers Documentation

## Overview
This folder contains custom ASP.NET Core Tag Helpers that enhance HTML elements with application-specific functionality for script loading, CSS management, and UI components.

## Files

### inzibackendScriptSrcTagHelper.cs
**Purpose**: Custom tag helper for `<script>` tags to automatically handle minification and path resolution

**Attributes**:
- `abp-src`: Source path to JavaScript file (alternative to `src`)
- `abp-ignore-src-modification`: Skip automatic modifications

**Functionality**:
- **Development**: Uses non-minified files (e.g., `app.js`)
- **Production**: Automatically replaces with minified version (e.g., `app.min.js`)
- **Path Resolution**: Adds base path for sub-applications
- **Cache Busting**: Can append version query string

**Usage**:
```razor
<script abp-src="/Scripts/MyScript.js"></script>
<!-- Renders as: <script src="/Scripts/MyScript.min.js"></script> in production -->
```

### inzibackendLinkHrefTagHelper.cs
**Purpose**: Custom tag helper for `<link>` tags to manage CSS file references

**Attributes**:
- `abp-href`: Source path to CSS file (alternative to `href`)
- `abp-ignore-href-modification`: Skip automatic modifications

**Functionality**:
- **Development**: Uses non-minified CSS (e.g., `styles.css`)
- **Production**: Automatically uses minified version (e.g., `styles.min.css`)
- **Path Resolution**: Handles base path correctly
- **RTL Support**: Can swap to RTL stylesheet if needed

**Usage**:
```razor
<link abp-href="/Styles/MyStyles.css" rel="stylesheet" />
<!-- Renders as: <link href="/Styles/MyStyles.min.css" rel="stylesheet" /> in production -->
```

### inzibackendPageSubheaderTagHelper.cs
**Purpose**: Renders page subheader component for consistent page titles and breadcrumbs

**Attributes**:
- `title`: Page title
- `description`: Page description (optional)
- `breadcrumb`: Breadcrumb items (optional)
- `button`: Action button (optional)

**Functionality**:
- Renders consistent subheader across all pages
- Supports breadcrumb navigation
- Allows custom action buttons
- Responsive design
- Theme-aware styling

**Usage**:
```razor
<inzibackend-page-subheader
    title="@L("Users")"
    description="@L("ManageUsers")"
    breadcrumb="Home > Administration > Users">
    <button asp-action="Create" class="btn btn-primary">
        @L("CreateNewUser")
    </button>
</inzibackend-page-subheader>
```

## Architecture Notes

### Tag Helper Benefits
- **Clean Syntax**: More readable than HTML helpers
- **IntelliSense**: IDE support for attributes
- **Reusability**: Encapsulate complex HTML generation
- **Separation of Concerns**: Keeps views clean
- **Type Safety**: Strongly-typed attributes

### Processing Order
Tag helpers have an `Order` property to control processing sequence:
- Lower numbers process first
- Default is 0
- These helpers use -1001 to process before standard helpers

### Environment Awareness
Tag helpers check `IWebHostEnvironment`:
- **Development**: Use full, non-minified files for debugging
- **Staging**: Can use minified or non-minified
- **Production**: Always use minified for performance

### Integration with ABP
- Follows ABP naming conventions (`abp-*` attributes)
- Works with ABP resource management
- Supports ABP localization
- Compatible with ABP themes

## Usage Across Codebase

### Script Loading
Used in layout pages to load JavaScript:
```razor
<script abp-src="~/view-resources/Views/Users/Index.js"></script>
```

### CSS Loading
Used in layout pages to load stylesheets:
```razor
<link abp-href="~/Common/Styles/common.css" rel="stylesheet" />
```

### Page Headers
Used at the top of content pages:
```razor
<inzibackend-page-subheader title="@L("Dashboard")" />
```

## Dependencies
- ASP.NET Core Razor Tag Helpers
- `IWebHostEnvironment`: Environment detection
- `IHttpContextAccessor`: Request context
- ABP Framework conventions

## Performance Considerations
- **Minification**: Reduces bandwidth in production
- **Caching**: Browser can cache minified files efficiently
- **CDN Ready**: Paths can be absolute for CDN hosting
- **Lazy Loading**: Can be extended to support lazy loading

## Testing
- **Unit Tests**: Test tag helper processing logic
- **Integration Tests**: Verify correct HTML output
- **Environment Tests**: Test dev vs. production behavior

## Extension Points
To add new tag helpers:
1. Create class inheriting from `TagHelper`
2. Add `[HtmlTargetElement]` attribute
3. Implement `Process()` or `ProcessAsync()`
4. Register in `Startup.cs` (automatic via assembly scanning)

## Related Documentation
- [Extensions/CLAUDE.md](../Extensions/CLAUDE.md): HTML helper extensions
- [Views/CLAUDE.md](../Views/CLAUDE.md): Razor views
- [Resources/CLAUDE.md](../Resources/CLAUDE.md): Resource management