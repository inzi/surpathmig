# view-resources/Views Documentation

## Overview
This folder contains view-specific static resources (CSS stylesheets) organized to mirror the Views folder structure. Each view or view group has its own subfolder containing the CSS files needed for that specific UI component.

## Contents

### Files
No files exist directly in this folder - all resources are organized in subfolders matching the view structure.

### Key Components

#### View-Specific Resources
- CSS files named to match their corresponding views
- Organized by controller/area structure
- Clean separation of concerns
- Easy to locate and maintain

### Dependencies
- Referenced by Razor views via tag helpers
- Served via ASP.NET Core Static Files middleware
- Version cache-busting via `asp-append-version="true"`

## Subfolders

### Error
[See Error/CLAUDE.md]
- **Index.css**: Stylesheet for the generic error page
- Styles for error message display (.m-error_container, .m-error_title, .m-error_subtitle, .m-error_description)
- Used by Error.cshtml, Error403.cshtml, Error404.cshtml

### Ui
[See Ui/CLAUDE.md]
- **Login.css**: Stylesheet for the login form (.login-form, .login-form-row, .login-form-button)
- **Index.css**: Stylesheet for the authenticated home page (.main-content, .user-name, .content-list, .logout)
- Also used by Consent views for consistent styling

## Architecture Notes

### Folder Structure Convention
```
view-resources/
└── Views/
    ├── Error/
    │   └── Index.css (for Views/Error/*.cshtml)
    └── Ui/
        ├── Login.css (for Views/Ui/Login.cshtml)
        └── Index.css (for Views/Ui/Index.cshtml)
```

### Mirroring Views Folder
- Structure matches /Views/ folder organization
- Easy to find resources for a specific view
- Consistent pattern across application
- Clear relationship between views and resources

### Naming Convention
- CSS files named after the view they style
- Subfolder names match controller names
- Lowercase with hyphens for consistency
- Descriptive file names

### Static File Serving
- Served from /view-resources/* URL path
- No processing or transformation
- Browser caching enabled
- Direct file access

## Business Logic

### Resource Organization Strategy
1. **Separation**: View-specific styles separate from global styles
2. **Modularity**: Each view has its own stylesheet
3. **Maintainability**: Easy to find and update view-specific styles
4. **Performance**: Load only styles needed for current view
5. **Caching**: Browser caches individual files

### Loading Pattern
1. View references specific CSS file
2. Browser requests from /view-resources/Views/
3. Static file middleware serves file
4. Browser caches with version hash
5. Styles applied to view elements

### Scope of Styles
- **View-Specific**: Only styles needed for that view
- **Not Global**: Don't affect other views
- **Self-Contained**: Minimal external dependencies
- **Focused**: Clear, single-purpose stylesheets

## Usage Across Codebase

### View References

#### Error Views
```cshtml
@* Views/Error/Error.cshtml *@
<head>
    <title>Surpath - Error</title>
    <link href="~/view-resources/Views/Error/Index.css"
          rel="stylesheet"
          asp-append-version="true"/>
</head>
```

#### Ui Views
```cshtml
@* Views/Ui/Login.cshtml *@
<head>
    <title>Surpath</title>
    <link href="~/view-resources/Views/Ui/Login.css"
          rel="stylesheet"
          asp-append-version="true"/>
</head>

@* Views/Ui/Index.cshtml *@
<head>
    <title>Surpath</title>
    <link href="~/view-resources/Views/Ui/Index.css"
          rel="stylesheet"
          asp-append-version="true"/>
</head>
```

#### Consent Views (Reusing)
```cshtml
@* Views/Consent/Index.cshtml *@
<head>
    <title>Surpath</title>
    <link href="~/view-resources/Views/Ui/Login.css"
          rel="stylesheet"
          asp-append-version="true"/>
</head>
```

### Tag Helper Usage
- **asp-append-version="true"**: Adds version hash for cache-busting
- Updates automatically when file changes
- Prevents stale cached stylesheets
- No manual version management needed

### URL Paths
- **Physical**: wwwroot/view-resources/Views/*/
- **Web**: /view-resources/Views/*/
- **Example**: /view-resources/Views/Ui/Login.css

## Development Notes

### Adding New View Resources

#### Step 1: Create Subfolder (if needed)
```bash
mkdir wwwroot/view-resources/Views/NewController
```

#### Step 2: Add CSS File
```bash
touch wwwroot/view-resources/Views/NewController/ViewName.css
```

#### Step 3: Reference in View
```cshtml
<head>
    <link href="~/view-resources/Views/NewController/ViewName.css"
          rel="stylesheet"
          asp-append-version="true"/>
</head>
```

### Organizing Styles

#### View-Specific CSS Structure
```css
/* Main container */
.view-container {
    /* Layout styles */
}

/* Components within view */
.view-header { }
.view-content { }
.view-footer { }

/* Specific elements */
.view-specific-element { }
```

#### Naming Guidelines
- Prefix classes with view/controller name to avoid conflicts
- Use descriptive, semantic names
- Follow BEM or similar methodology
- Keep specificity low for maintainability

### Shared Styles
For styles used across multiple views:
1. **Option 1**: Create shared CSS in /Common/
2. **Option 2**: Use application-wide stylesheet
3. **Option 3**: Duplicate (if truly view-specific logic)
4. **Best Practice**: Prefer shared over duplication

### Performance Optimization

#### CSS Optimization
- Minify CSS files for production
- Remove unused styles
- Combine if appropriate
- Use CSS compression
- Leverage browser caching

#### Loading Strategy
```cshtml
@* Critical CSS inline *@
<style>
    /* Above-the-fold styles */
</style>

@* Full stylesheet deferred *@
<link href="~/view-resources/Views/Ui/Login.css"
      rel="stylesheet"
      asp-append-version="true"/>
```

### Best Practices
- Keep view-specific styles in view-resources
- Don't mix global and view-specific styles
- Use consistent naming conventions
- Optimize CSS file sizes
- Leverage browser caching
- Document complex styles
- Test responsive design
- Ensure cross-browser compatibility

### Testing View Styles
1. **Visual Testing**: Verify appearance in different browsers
2. **Responsive Testing**: Test at various screen sizes
3. **Cache Testing**: Verify version hash updates on change
4. **Performance Testing**: Check load times
5. **Accessibility Testing**: Ensure sufficient contrast, readable fonts

### Common Patterns

#### Loading Multiple Stylesheets
```cshtml
<head>
    <link href="~/view-resources/Views/Shared/Common.css"
          rel="stylesheet"
          asp-append-version="true"/>
    <link href="~/view-resources/Views/Ui/Login.css"
          rel="stylesheet"
          asp-append-version="true"/>
</head>
```

#### Conditional Loading
```cshtml
@if (Model.UseCustomStyles)
{
    <link href="~/view-resources/Views/Ui/Custom.css"
          rel="stylesheet"
          asp-append-version="true"/>
}
```

### Maintenance Tasks
- [ ] Review and remove unused CSS files
- [ ] Optimize file sizes periodically
- [ ] Update for new browser features
- [ ] Ensure naming consistency
- [ ] Document complex styles
- [ ] Test after framework updates
- [ ] Verify cache-busting works

### Alternative Organization
Other valid approaches:
1. **By Feature**: Group all resources by feature area
2. **Flat Structure**: All CSS files in one folder
3. **Asset Pipeline**: Use bundling and minification
4. **CSS-in-JS**: For SPA components
5. **CSS Modules**: Scoped styles for components

### Current Approach Benefits
- Clear view-to-resource mapping
- Easy to locate specific styles
- Mirrors view folder structure
- Simple to maintain
- No build process required
- Framework-agnostic