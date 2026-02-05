# view-resources/Views/Error Documentation

## Overview
This folder contains CSS stylesheets for error pages in the Web Host application. These styles provide a consistent, user-friendly appearance for error messages and exception pages.

## Contents

### Files

#### Index.css
- **Purpose**: Stylesheet for the generic error page
- **Styles Applied To**:
  - `.m-error_container`: Main error page container
  - `.m-error_title`: Error title/heading styles
  - `.m-error_subtitle`: Error message subtitle
  - `.m-error_description`: Detailed error description and validation messages
- **Usage**: Loaded by `Views/Error/Error.cshtml`
- **Design**: Consistent with application branding and layout

### Key Components

#### Error Page Styling
- Professional error presentation
- Clear visual hierarchy (title → subtitle → description)
- Readable typography
- Responsive layout
- User-friendly appearance

### Dependencies
- Referenced by Razor views via `asp-append-version` tag helper
- May reference common CSS variables or Bootstrap

## Architecture Notes

### File Organization
- Matches view structure: `/Views/Error/` → `/view-resources/Views/Error/`
- One CSS file per view or shared across error views
- Version cache-busting via `asp-append-version="true"`

### Naming Convention
- Matches view names (Index.css for Error view)
- BEM or custom naming for CSS classes
- Prefixed with `m-error_` for error-specific styles

### Styling Approach
- Scoped styles for error pages
- Minimal dependencies on global styles
- Self-contained error page presentation

## Business Logic

### Error Display Strategy
1. **Title**: Main error type (e.g., "Error", "Forbidden", "Not Found")
2. **Subtitle**: Specific error message or details
3. **Description**: Validation errors or additional context

### User Experience
- Clear communication of what went wrong
- Actionable information when possible
- Professional appearance maintains trust
- Consistent with overall application design

### Error Types Supported
- Generic errors (500)
- Validation errors (400)
- Authorization errors (403)
- Not found errors (404)
- Custom application errors

## Usage Across Codebase

### Referenced By

#### Views/Error/Error.cshtml
```cshtml
<link href="~/view-resources/Views/Error/Index.css"
      rel="stylesheet"
      asp-append-version="true"/>
```

#### Error Controller
- Returns error views that reference these styles
- Passes error model with title, subtitle, description
- Handles validation error collections

### Style Application Flow
1. Exception occurs or error status code returned
2. Error middleware redirects to Error controller
3. Controller returns appropriate error view
4. View loads Index.css stylesheet
5. Error information displayed with proper styling

### Integration with Views
- CSS classes match view markup structure
- Container → Title → Subtitle → Description hierarchy
- Validation errors displayed in description section

## Development Notes

### Modifying Error Styles

#### Updating Appearance
1. Edit Index.css with new styles
2. Maintain class name structure
3. Test with different error scenarios
4. Ensure responsive design
5. Verify browser compatibility

#### CSS Structure
```css
.m-error_container {
    /* Main container styles */
}

.m-error_title {
    /* Heading styles */
}

.m-error_subtitle {
    /* Message styles */
}

.m-error_description {
    /* Detail and validation styles */
}
```

### Testing Error Pages
1. Trigger different error types
2. Verify styling consistency
3. Check responsive behavior
4. Test with long error messages
5. Validate validation error display

### Best Practices
- Keep error pages simple and clear
- Avoid overwhelming users with technical details
- Provide navigation options when appropriate
- Maintain consistent branding
- Ensure accessibility (color contrast, readable fonts)

### Common Customizations
- Brand colors and fonts
- Error icons or illustrations
- Button styles for actions (go home, try again)
- Animation or transitions
- Dark mode support