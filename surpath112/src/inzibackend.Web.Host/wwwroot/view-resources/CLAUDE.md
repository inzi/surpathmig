# view-resources Documentation

## Overview
The view-resources folder contains static resources (primarily CSS files) that are specific to individual views or groups of related views. This folder organizes view-specific styles separately from common assets and follows the application's view structure for easy maintenance.

## Contents

### Files
No files exist directly in this folder - all resources are organized in the Views subfolder.

### Key Components

#### View-Specific Resource Management
- CSS stylesheets for individual views
- Organized to mirror the Views folder structure
- Easy location and maintenance of view styles
- Clear separation between global and view-specific styles

### Dependencies
- **ASP.NET Core Static Files**: Middleware for serving static files
- **Tag Helpers**: asp-append-version for cache-busting
- **Razor Views**: Reference these resources via link tags

## Subfolders

### Views
[See Views/CLAUDE.md]
Contains CSS files organized by view controller/area:

#### Error (Views/Error/)
- **Index.css**: Styles for error pages (Error.cshtml, Error403.cshtml, Error404.cshtml)
- Error message formatting and layout
- Classes: .m-error_container, .m-error_title, .m-error_subtitle, .m-error_description

#### Ui (Views/Ui/)
- **Login.css**: Styles for login form (Login.cshtml, also used by Consent/Index.cshtml)
- **Index.css**: Styles for authenticated home page (Index.cshtml)
- Form layouts, buttons, user display components

## Architecture Notes

### Folder Structure
```
view-resources/
└── Views/
    ├── Error/
    │   └── Index.css
    └── Ui/
        ├── Login.css
        └── Index.css
```

### Design Philosophy
1. **View-Specific**: Styles only used by specific views
2. **Organized by Context**: Mirrors Views folder structure
3. **Easy Discovery**: Clear path from view to its resources
4. **Maintainable**: Changes to view isolated to its resource folder
5. **Performant**: Load only what's needed per view

### Naming Conventions
- Folder names match controller names
- CSS file names match view names (or shared purpose)
- Lowercase with hyphens for consistency
- Descriptive and predictable paths

### Static File Serving
- URL path: /view-resources/*
- Physical path: wwwroot/view-resources/*
- No processing or compilation
- Browser caching enabled
- Version cache-busting via tag helpers

## Business Logic

### Resource Loading Strategy
1. **View Requests Resource**: Link tag in view head
2. **Browser Requests File**: GET /view-resources/Views/*/file.css
3. **Static Files Middleware**: Serves file from wwwroot
4. **Browser Caches**: With version hash for cache-busting
5. **Styles Applied**: To view elements

### Scope and Isolation
- **View-Specific Styles**: Don't pollute global namespace
- **Component Isolation**: Changes don't affect other views
- **Maintainability**: Easy to refactor individual views
- **Testing**: Can test view styles independently

### Performance Considerations
- **Minimal Loading**: Only load styles needed for current view
- **Caching**: Browser caches individual files
- **Version Hashing**: Automatic cache invalidation on changes
- **Compression**: Enable gzip/brotli compression

## Usage Across Codebase

### View References

#### Standard Pattern
```cshtml
@* In view head section *@
<head>
    <title>Page Title</title>
    <link href="~/view-resources/Views/ControllerName/ViewName.css"
          rel="stylesheet"
          asp-append-version="true"/>
</head>
```

#### Actual Examples
```cshtml
@* Views/Error/Error.cshtml *@
<link href="~/view-resources/Views/Error/Index.css"
      rel="stylesheet"
      asp-append-version="true"/>

@* Views/Ui/Login.cshtml *@
<link href="~/view-resources/Views/Ui/Login.css"
      rel="stylesheet"
      asp-append-version="true"/>

@* Views/Ui/Index.cshtml *@
<link href="~/view-resources/Views/Ui/Index.css"
      rel="stylesheet"
      asp-append-version="true"/>
```

### Configuration

#### Startup.cs - Static Files
```csharp
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        // Cache for 1 year (version hash handles updates)
        const int durationInSeconds = 60 * 60 * 24 * 365;
        ctx.Context.Response.Headers[HeaderNames.CacheControl] =
            "public,max-age=" + durationInSeconds;
    }
});
```

### Referenced By
- **Razor Views**: All views in Views/Error/ and Views/Ui/
- **Tag Helpers**: asp-append-version tag helper
- **Static File Middleware**: Serves files to browser
- **Browser Cache**: Caches files with version hash

## Development Notes

### Adding View-Specific Resources

#### For New View
```bash
# 1. Create view
touch Views/NewController/NewView.cshtml

# 2. Create matching CSS folder
mkdir wwwroot/view-resources/Views/NewController

# 3. Create CSS file
touch wwwroot/view-resources/Views/NewController/NewView.css

# 4. Reference in view
# <link href="~/view-resources/Views/NewController/NewView.css"
#       rel="stylesheet"
#       asp-append-version="true"/>
```

#### For Existing View
```bash
# Add CSS file to existing folder
touch wwwroot/view-resources/Views/ExistingController/Additional.css

# Reference in view
# <link href="~/view-resources/Views/ExistingController/Additional.css"
#       rel="stylesheet"
#       asp-append-version="true"/>
```

### CSS Organization Guidelines

#### File Structure
```css
/* ViewName.css */

/* Container/Layout */
.view-container {
    /* Main container styles */
}

/* Components */
.view-header { }
.view-content { }
.view-footer { }

/* Elements */
.view-specific-button { }
.view-specific-form { }

/* States */
.view-loading { }
.view-error { }
```

#### Naming Best Practices
- Prefix with view/controller name to avoid conflicts
- Use semantic, descriptive names
- Follow consistent methodology (BEM, SMACSS, etc.)
- Keep specificity low for flexibility

### Common vs View-Specific

#### When to Use view-resources
- Styles used by single view
- Styles specific to view's functionality
- Styles that may change independently
- Component-level styling

#### When to Use Common
- Styles shared across multiple views
- Global typography, colors, spacing
- Reusable components
- Application-wide patterns

### Build and Optimization

#### Development
- Unminified CSS for debugging
- Source maps if using preprocessor
- Hot reload during development
- Clear error messages

#### Production
- Minified CSS files
- Remove unused styles
- Combine if beneficial
- Enable compression
- Long cache duration

### Testing View Resources

#### Visual Testing
```bash
# Test in different browsers
- Chrome
- Firefox
- Safari
- Edge

# Test at different screen sizes
- Mobile (320px, 375px, 414px)
- Tablet (768px, 1024px)
- Desktop (1280px, 1920px)
```

#### Performance Testing
- Measure CSS file sizes
- Check load times
- Verify caching works
- Test cache invalidation

#### Accessibility Testing
- Color contrast ratios
- Readable font sizes
- Focus indicators
- Screen reader compatibility

### Best Practices
- **Keep Focused**: Only styles for specific view
- **Avoid Duplication**: Extract common patterns
- **Use Variables**: For colors, spacing, etc.
- **Document Complex Rules**: Add comments
- **Optimize Performance**: Minimize file sizes
- **Test Thoroughly**: Cross-browser and responsive
- **Version Control**: Track changes properly
- **Cache-Bust**: Use asp-append-version

### Alternative Approaches

#### CSS Preprocessors
```scss
// Use SASS/LESS for view resources
wwwroot/view-resources/Views/Ui/Login.scss

// Compile to CSS
wwwroot/view-resources/Views/Ui/Login.css
```

#### CSS Modules
```javascript
// For SPA components
import styles from './Login.module.css';
```

#### CSS-in-JS
```javascript
// For React/Vue components
const styles = styled.div`
  /* View-specific styles */
`;
```

#### Bundling
```csharp
// Bundle multiple CSS files
bundles.Add(new StyleBundle("~/bundles/ui")
    .Include("~/view-resources/Views/Ui/Login.css")
    .Include("~/view-resources/Views/Ui/Index.css"));
```

### Current Approach Benefits
- **Simple**: No build process required
- **Clear**: Direct mapping to views
- **Maintainable**: Easy to locate and update
- **Performant**: Leverages browser caching
- **Flexible**: Can evolve as needed

### Maintenance Checklist
- [ ] Review unused CSS files quarterly
- [ ] Optimize file sizes regularly
- [ ] Update for new CSS features
- [ ] Maintain naming consistency
- [ ] Document complex styles
- [ ] Test after updates
- [ ] Verify cache-busting
- [ ] Remove deprecated styles
- [ ] Update browser compatibility
- [ ] Monitor performance metrics