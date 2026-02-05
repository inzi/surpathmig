# swagger/ui Documentation

## Overview
This folder contains customizations and extensions for the Swagger UI used to document and test the inzibackend Web API. The files integrate Swagger with ABP framework features, particularly multi-tenancy and authentication.

## Contents

### Files

#### index.html
- **Purpose**: Custom Swagger UI HTML template
- **Key Features**:
  - Overrides default Swagger UI distribution
  - Integrates ABP framework JavaScript (abp.js)
  - Configures OAuth2 redirect handling
  - Adds anti-forgery token support
  - Removes default authorize button (custom handling)
- **Configuration**:
  - Uses `%(DocumentTitle)` placeholder for title
  - Uses `%(ConfigObject)` for JSON configuration injection
  - Uses `%(HeadContent)` for additional head content
  - Placeholders replaced by ASP.NET Core middleware
- **ABP Integration**:
  - Includes `ui/abp.js` for framework support
  - Request interceptor adds XSRF tokens to API calls
  - Custom plugin removes default authorization UI
- **Styling**:
  - Google Fonts: Open Sans, Source Code Pro, Titillium Web
  - Custom CSS for layout and scrolling
  - Light background (#fafafa)
  - SVG icons for UI elements (lock, arrow, expand, etc.)

#### abp.js
- **Purpose**: Minimal ABP framework JavaScript for Swagger UI
- **Key Features**:
  - Cookie utilities for token management
  - Anti-forgery token support
  - XSRF token header injection
- **Utilities Provided**:
  - `abp.utils.setCookieValue(key, value, expireDate, path)`: Set browser cookies
  - `abp.utils.getCookieValue(key)`: Read browser cookies
  - `abp.utils.deleteCookie(key, path)`: Remove browser cookies
- **Security Constants**:
  - `abp.security.antiForgery.tokenCookieName = 'XSRF-TOKEN'`
  - `abp.security.antiForgery.tokenHeaderName = 'X-XSRF-TOKEN'`
  - `abp.security.antiForgery.getToken()`: Retrieve current XSRF token
- **Multi-Tenancy Support**: Ready for tenant header injection (if extended)

### Key Components

#### Token Management
- Automatic XSRF token injection in API requests
- Cookie-based token storage
- Synchronizes with ASP.NET Core anti-forgery system

#### Custom UI Integration
- Removes default Swagger authorize button
- Custom authentication flow via index.html
- Better integration with ABP authentication

#### Configuration Injection
- Server-side configuration via placeholders
- Dynamic title and content
- Flexible OAuth2 configuration

### Dependencies
- Swagger UI Bundle (swagger-ui-bundle.js)
- Swagger UI Standalone Preset (swagger-ui-standalone-preset.js)
- ABP Framework conventions

## Architecture Notes

### File Serving
1. Files served from `/swagger/ui/*` path
2. index.html served as custom Swagger UI
3. abp.js loaded automatically via script tag
4. Standard Swagger UI bundles loaded from parent directory

### Customization Approach
- **Override Strategy**: Replace index.html entirely
- **Extension Strategy**: Add abp.js for additional functionality
- **Configuration Strategy**: Use placeholders for server-side injection

### Security Integration
- XSRF protection on all API calls from Swagger
- Tenant isolation maintained
- Authentication tokens managed properly

## Business Logic

### API Testing Flow
1. User navigates to /swagger
2. Custom index.html loads
3. abp.js initializes ABP utilities
4. Swagger UI renders API documentation
5. Request interceptor adds security headers
6. API calls include XSRF and auth tokens

### Multi-Tenant API Testing
- Tenant identification via header or subdomain
- XSRF tokens scoped to tenant
- API responses filtered by tenant context

### Security Considerations
- All API calls protected by XSRF tokens
- Cookie-based token management
- Secure token transmission via headers
- Authorization required for protected endpoints

## Usage Across Codebase

### Configuration

#### Startup.cs
```csharp
app.UseSwaggerUI(options => {
    options.IndexStream = () => GetType().Assembly
        .GetManifestResourceStream("...");
    // Or uses custom index.html from wwwroot
});
```

#### SwaggerUI Middleware
- Replaces placeholders in index.html
- Injects configuration JSON
- Serves custom UI files

### Integration Points

#### Anti-Forgery System
- ASP.NET Core ValidateAntiForgeryToken
- Automatic token validation
- abp.js reads token from cookie
- Request interceptor adds to headers

#### Authentication
- JWT Bearer tokens via Authorization header
- IdentityServer integration (if enabled)
- Cookie-based authentication for UI

#### API Documentation
- All API endpoints documented via Swagger
- Interactive testing via custom UI
- Multi-tenant aware testing

## Development Notes

### Customizing Swagger UI

#### Modifying Appearance
1. Edit CSS in index.html `<style>` section
2. Update SVG icons as needed
3. Adjust layout and colors
4. Maintain responsive design

#### Adding Features to abp.js
1. Follow existing namespace pattern
2. Add utilities under `abp.utils` or `abp.security`
3. Maintain compatibility with ABP conventions
4. Test with multi-tenant scenarios

#### Configuration Changes
1. Update placeholder replacement logic in server
2. Modify ConfigObject structure
3. Test OAuth2 flows
4. Verify token handling

### Testing Swagger Changes
1. Navigate to /swagger endpoint
2. Verify custom UI loads
3. Test API calls with XSRF tokens
4. Confirm multi-tenant isolation
5. Check OAuth2 redirect flow

### Troubleshooting
- **XSRF token errors**: Check cookie and header names
- **Authorization failures**: Verify token in Authorization header
- **UI not loading**: Check file paths and middleware order
- **Tenant isolation issues**: Verify tenant header/subdomain logic