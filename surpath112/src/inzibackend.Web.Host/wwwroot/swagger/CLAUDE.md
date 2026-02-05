# swagger Documentation

## Overview
The swagger folder contains customizations and extensions for Swagger UI, the interactive API documentation interface. These files integrate Swagger with ABP Framework features, particularly multi-tenancy, authentication, and anti-forgery token management.

## Contents

### Files
No files exist directly in the swagger folder - all customizations are in the ui subfolder.

### Key Components

#### Swagger UI Customization
- Custom HTML template for Swagger interface
- ABP Framework integration JavaScript
- Anti-forgery token support
- Multi-tenancy header injection
- Custom authentication handling

### Dependencies
- **Swashbuckle.AspNetCore**: Swagger generation and UI
- **ABP Framework**: Multi-tenancy and security conventions
- **ASP.NET Core**: Static files and middleware

## Subfolders

### ui
[See ui/CLAUDE.md]
- **index.html**: Custom Swagger UI page with ABP integration
- **abp.js**: ABP framework utilities for Swagger (cookie management, XSRF tokens)

Contains the customized Swagger UI implementation that:
- Adds anti-forgery token to all API requests
- Supports multi-tenant API testing
- Removes default authorization UI (custom handling)
- Configures OAuth2 redirect handling
- Provides cookie utilities for token management

## Architecture Notes

### Integration Strategy
1. **Override Default UI**: Replace Swashbuckle's default index.html
2. **Extend with ABP**: Add ABP-specific JavaScript utilities
3. **Request Interception**: Inject security headers automatically
4. **Configuration Injection**: Use server-side placeholders for dynamic config

### File Structure
```
swagger/
└── ui/
    ├── index.html (custom Swagger UI template)
    └── abp.js (ABP utilities for token management)
```

### Security Integration
- **XSRF Protection**: Automatic token injection
- **Authentication**: JWT or Cookie-based
- **Multi-Tenancy**: Tenant header support
- **Authorization**: Permission-based endpoint visibility

## Business Logic

### API Documentation Flow
1. Developer accesses `/swagger` endpoint
2. Swashbuckle generates OpenAPI specification
3. Custom index.html loads Swagger UI
4. abp.js initializes ABP utilities
5. Request interceptor adds security headers
6. Developer can test API endpoints interactively

### Token Management
1. **XSRF Token**: Retrieved from XSRF-TOKEN cookie
2. **Header Injection**: Added to X-XSRF-TOKEN header
3. **Automatic**: Happens on every API call from Swagger
4. **Validation**: Server validates token on requests

### Multi-Tenant Testing
- Tenant can be specified via header or subdomain
- Each request includes tenant context
- API responses filtered by tenant
- Testing isolated per tenant

## Usage Across Codebase

### Configuration

#### Startup.cs - Swagger Configuration
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "inzibackend API",
            Version = "v1"
        });

        // Add JWT authentication
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });

        // Include XML comments
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        options.IncludeXmlComments(xmlPath);
    });
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "inzibackend API v1");
        options.IndexStream = () => GetType().Assembly
            .GetManifestResourceStream($"{GetType().Namespace}.swagger.ui.index.html");
        // Or use custom index.html from wwwroot
    });
}
```

#### appsettings.json - Swagger Settings
```json
{
  "SwaggerUI": {
    "Enabled": true,
    "Endpoint": "/swagger",
    "Title": "inzibackend API Documentation"
  }
}
```

### API Controller Documentation
```csharp
/// <summary>
/// Manages user accounts
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    /// <summary>
    /// Authenticates user and returns JWT token
    /// </summary>
    /// <param name="model">Login credentials</param>
    /// <returns>Authentication token and user information</returns>
    /// <response code="200">Login successful</response>
    /// <response code="401">Invalid credentials</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthenticateResultModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        // Implementation
    }
}
```

### Referenced By
- **Developers**: Primary tool for API testing and documentation
- **API Consumers**: Reference for endpoint definitions
- **Integration Teams**: Documentation for third-party integrations
- **QA Teams**: Manual API testing
- **Documentation**: Auto-generated API reference

## Development Notes

### Customizing Swagger UI

#### Modifying Appearance
Edit `ui/index.html`:
```html
<style>
    .swagger-ui .topbar {
        background-color: #your-brand-color;
    }
    /* Additional customizations */
</style>
```

#### Adding Custom Headers
Edit `ui/index.html` requestInterceptor:
```javascript
configObject.requestInterceptor = function (request) {
    var token = abp.security.antiForgery.getToken();
    request.headers[abp.security.antiForgery.tokenHeaderName] = token;

    // Add custom headers
    request.headers['X-Custom-Header'] = 'custom-value';

    // Add tenant header (if needed)
    var tenantId = getTenantId();
    if (tenantId) {
        request.headers['Abp.TenantId'] = tenantId;
    }

    return request;
};
```

#### Extending abp.js
Add new utilities to `ui/abp.js`:
```javascript
// Add tenant ID management
abp.multiTenancy = abp.multiTenancy || {};

abp.multiTenancy.getTenantId = function () {
    return abp.utils.getCookieValue('Abp.TenantId');
};

abp.multiTenancy.setTenantId = function (tenantId) {
    abp.utils.setCookieValue('Abp.TenantId', tenantId);
};
```

### Documenting APIs

#### XML Documentation Comments
```csharp
/// <summary>
/// Brief description of endpoint
/// </summary>
/// <remarks>
/// Detailed explanation, usage examples, notes
/// </remarks>
/// <param name="id">Parameter description</param>
/// <returns>Return value description</returns>
/// <response code="200">Success description</response>
/// <response code="400">Bad request description</response>
/// <response code="404">Not found description</response>
```

#### Response Type Attributes
```csharp
[ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
[ProducesResponseType(typeof(ErrorInfo), StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
```

### Testing with Swagger
1. Navigate to `/swagger` endpoint
2. Authenticate (if required):
   - Click "Authorize" button (if not removed)
   - Enter JWT token or use OAuth flow
3. Select endpoint to test
4. Enter parameters
5. Click "Execute"
6. Review response

### Best Practices
- Document all public API endpoints
- Include example requests and responses
- Specify all possible response codes
- Group related endpoints with tags
- Use descriptive parameter names
- Provide XML documentation comments
- Test authentication flows
- Keep Swagger UI version updated

### Security Considerations
- **Production Access**: Consider disabling in production or restricting access
- **Authentication**: Require authentication to view API docs
- **Sensitive Data**: Don't expose sensitive endpoints unnecessarily
- **Rate Limiting**: Apply rate limits to prevent abuse
- **CORS**: Configure properly for Swagger UI origin

### Common Issues and Solutions

#### XSRF Token Not Working
- Verify cookie is set correctly
- Check header name matches server configuration
- Ensure abp.js loads before Swagger UI initialization

#### Authorization Not Working
- Verify JWT token format
- Check token expiration
- Ensure Bearer prefix included
- Verify token in Authorization header

#### Tenant Isolation Issues
- Confirm tenant header included in requests
- Verify tenant ID format and validation
- Check tenant resolution logic

### Maintenance Tasks
- [ ] Update Swagger UI version periodically
- [ ] Review and update API documentation
- [ ] Test authentication flows after changes
- [ ] Verify all endpoints documented
- [ ] Check for security vulnerabilities
- [ ] Update XML comments for new endpoints
- [ ] Test with different tenant contexts