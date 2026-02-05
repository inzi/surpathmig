# wwwroot Documentation

## Overview
The wwwroot folder contains static files served directly by the Web Host application. These include images, CSS stylesheets, JavaScript files, and Swagger UI customizations. As the Web Host primarily serves as an API, the static content is minimal and focused on supporting the authentication UI and API documentation.

## Contents

### Folder Structure

#### Common/Images
**Purpose**: Shared image assets for the application

- **Logo Files**:
  - `app-logo-on-dark.svg`: Logo for dark backgrounds
  - `app-logo-on-light.svg`: Logo for light backgrounds
  - `app-logo-small.svg`: Compact version of logo
  - `logo.svg`: Primary logo file
  - `default-profile-picture.png`: Placeholder for user profiles

#### Common/Images/SampleProfilePics
**Purpose**: Sample profile pictures for demo/testing
- Contains 10 sample profile images (sample-profile-01.jpg through sample-profile-10.jpg)
- Used for demo accounts or testing
- JPEG format for photo-realistic samples

#### Plugins
**Purpose**: Placeholder for plugin static content
- Contains `.gitkeep` file
- Reserved for future plugin system
- Allows dynamic plugin loading

#### swagger/ui
**Purpose**: Swagger UI customizations and overrides

- **abp.js**: ABP framework integration for Swagger
  - Adds authentication headers
  - Configures tenant headers
  - Customizes Swagger behavior

- **index.html**: Custom Swagger UI page
  - Overrides default Swagger UI
  - Includes ABP-specific modifications
  - Custom branding and styling

#### view-resources/Views/Error
**Purpose**: CSS styles for error pages
- `Index.css`: Styles for generic error page
- Error page specific styling
- Consistent error UI appearance

#### view-resources/Views/Ui
**Purpose**: CSS styles for authentication UI
- `Index.css`: Styles for authenticated home page
- `Login.css`: Styles for login page
- Form styling and layout
- Responsive design support

### Key Components

#### Static File Serving
- Configured in Startup.cs via UseStaticFiles()
- Direct file access without processing
- Browser caching enabled

#### Swagger Customization
- Custom JavaScript for API authentication
- Tenant header injection
- Token management in Swagger UI

### Dependencies
- ASP.NET Core Static Files middleware
- Swagger UI distribution
- Bootstrap CSS (likely referenced)
- jQuery (if used in views)

## Architecture Notes

### File Organization
1. **Common**: Shared across all views/pages
2. **View-specific**: Organized by controller/view
3. **Third-party**: Swagger UI customizations
4. **Plugins**: Extensibility point

### Naming Conventions
- Kebab-case for file names
- Descriptive names for images
- View-matching names for CSS

### Security Considerations
- Static files publicly accessible
- No sensitive data in wwwroot
- Images optimized for web delivery

## Business Logic

### Profile Picture Management
1. Default picture for new users
2. Sample pictures for demo accounts
3. Actual user uploads stored elsewhere
4. Fallback to default when needed

### Logo Usage
- Dark/light variants for different themes
- Small version for compact displays
- SVG format for scalability
- Consistent branding across UI

### Swagger Integration
- ABP.js adds multi-tenancy support
- Authentication token management
- Custom UI for better UX
- API testing capabilities

## Usage Across Codebase

### Static File References

#### In Razor Views
```html
<link rel="stylesheet" href="~/view-resources/Views/Ui/Login.css" />
<img src="~/Common/Images/logo.svg" />
```

#### In Swagger
- swagger/ui/index.html served at /swagger
- abp.js automatically included
- Customizations applied to all API docs

#### Profile Pictures
- Default picture path referenced in code
- Sample pictures for seeding database
- Fallback logic in ProfileController

### URL Patterns
- ~/Common/* - Shared resources
- ~/swagger/* - API documentation
- ~/view-resources/* - View-specific resources
- ~/Plugins/* - Plugin resources

### Caching Strategy
- Static files cached by browser
- Version query strings for updates
- Long cache durations for images
- Shorter durations for CSS/JS

### File Upload Storage
- Uploaded files NOT stored in wwwroot
- Separate storage location for security
- Only static assets in wwwroot

## Development Notes

### Adding Static Files
1. Place in appropriate subfolder
2. Follow naming conventions
3. Optimize images for web
4. Update references in views

### Swagger Customization
1. Modify swagger/ui/abp.js for behavior
2. Update index.html for UI changes
3. Test with different authentication scenarios
4. Ensure multi-tenant support works