# view-resources/Views/Ui Documentation

## Overview
This folder contains CSS stylesheets for the minimal authentication UI pages in the Web Host application. These styles support the login flow and authenticated home page, providing a clean and professional appearance.

## Contents

### Files

#### Login.css
- **Purpose**: Stylesheet for the login page
- **Styles Applied To**:
  - `.login-form`: Main login form container
  - `.login-form-row`: Individual form field rows
  - `.login-form-button`: Submit button styling
  - Form inputs, labels, checkboxes
  - Error messages and validation
- **Usage**: Loaded by `Views/Ui/Login.cshtml`
- **Features**:
  - Professional login form design
  - Responsive layout
  - Multi-tenant field styling
  - Remember me checkbox styling

#### Index.css
- **Purpose**: Stylesheet for the authenticated home page
- **Styles Applied To**:
  - `.main-content`: Content container
  - `.user-name`: Logged-in user display
  - `.content-list`: Links to admin tools (Swagger, Hangfire, GraphQL)
  - `.logout`: Logout link styling
- **Usage**: Loaded by `Views/Ui/Index.cshtml`
- **Features**:
  - Simple dashboard layout
  - Link styling for admin tools
  - User information display
  - Logout button styling

### Key Components

#### Login Page Styling
- Form field layout and spacing
- Input field styling
- Button appearance
- Checkbox custom styling
- Tenant name field (multi-tenancy)
- Password field security styling
- Error message display

#### Home Page Styling
- User greeting display
- Admin tool links
- Navigation layout
- Logout option styling

### Dependencies
- May reference common CSS or Bootstrap
- Version cache-busting via `asp-append-version="true"`
- Standalone styles (minimal external dependencies)

## Architecture Notes

### File Organization
- Matches view structure: `/Views/Ui/` → `/view-resources/Views/Ui/`
- One CSS file per view
- View-specific styles (not shared)
- Clean separation of concerns

### Naming Convention
- Matches view names (Login.css for Login.cshtml)
- Class names describe purpose (`.login-form`, `.main-content`)
- Consistent naming pattern across views

### Styling Approach
- Scoped to specific views
- Minimal global style dependencies
- Self-contained UI components
- Responsive design

## Business Logic

### Login Flow Styling
1. **Form Presentation**: Clean, focused login form
2. **Multi-Tenancy**: Tenant name field styled appropriately
3. **Validation**: Error messages displayed clearly
4. **Remember Me**: Checkbox styled for clarity
5. **Submit**: Prominent login button

### Home Page Styling
1. **User Context**: Display logged-in user with tenant info
2. **Admin Links**: Quick access to dev tools
3. **Logout**: Clear exit option
4. **Conditional Display**: Links shown based on permissions

### User Experience Goals
- Simple, distraction-free login
- Clear error messaging
- Professional appearance
- Consistent with IdentityServer standards
- Accessible and responsive

## Usage Across Codebase

### Referenced By

#### Views/Ui/Login.cshtml
```cshtml
<link href="~/view-resources/Views/Ui/Login.css"
      rel="stylesheet"
      asp-append-version="true"/>
```

#### Views/Ui/Index.cshtml
```cshtml
<link href="~/view-resources/Views/Ui/Index.css"
      rel="stylesheet"
      asp-append-version="true"/>
```

#### Views/Consent/Index.cshtml
```cshtml
<!-- Also uses Login.css for consistent styling -->
<link href="~/view-resources/Views/Ui/Login.css"
      rel="stylesheet"
      asp-append-version="true"/>
```

### Style Application Flow

#### Login Flow
1. User navigates to /ui/login or redirected
2. UiController returns Login view
3. View loads Login.css
4. Form styled with .login-form classes
5. User submits credentials
6. Validation errors styled if needed

#### Home Page Flow
1. Authenticated user accesses /ui/
2. UiController returns Index view
3. View loads Index.css
4. User info displayed with tenant context
5. Admin links styled and rendered
6. Logout link available

### Integration with Views
- CSS classes match HTML structure in views
- Form elements styled consistently
- Responsive to different screen sizes
- Supports multi-tenancy display

## Development Notes

### Modifying Login Styles

#### Form Customization
```css
.login-form {
    /* Container styles */
}

.login-form-row {
    /* Field row layout */
}

.login-form-button {
    /* Submit button appearance */
}
```

#### Multi-Tenant Field
- Style tenant name input distinctively
- Optional field presentation
- Clear placeholder text

### Modifying Home Page Styles

#### Layout Customization
```css
.main-content {
    /* Page container */
}

.user-name {
    /* User display */
}

.content-list {
    /* Admin links */
}

.logout {
    /* Logout link */
}
```

### Testing UI Changes
1. Test login form on different screen sizes
2. Verify error message styling
3. Check multi-tenant field display
4. Test home page with/without permissions
5. Verify admin tool links
6. Test logout styling

### Best Practices
- Keep login page focused (minimal distractions)
- Use clear, readable fonts
- Provide visual feedback on form interaction
- Style validation errors prominently
- Ensure accessibility (labels, contrast, focus)
- Maintain consistency with main application

### Common Customizations
- Brand colors and logo
- Button styles and hover effects
- Input field borders and focus states
- Checkbox custom styling
- Error message colors
- Link hover effects
- Dark mode support

### Responsive Design
- Mobile-friendly form layout
- Touch-friendly button sizes
- Readable text on small screens
- Proper spacing and padding
- Flexible container widths