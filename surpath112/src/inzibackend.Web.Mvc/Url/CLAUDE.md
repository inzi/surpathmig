# Url Documentation

## Overview
This folder contains URL generation services for creating absolute URLs to application resources, APIs, and pages, with support for multi-tenancy and different deployment scenarios.

## Files

### WebUrlService.cs
**Purpose**: Main URL service for MVC application

**Functionality**:
- Generates absolute URLs to application pages
- Reads root address from configuration
- Supports different environments (dev, staging, prod)
- Inherits from ABP's `WebUrlServiceBase`

**Configuration Keys**:
- `WebSiteRootAddressFormatKey`: `"App:WebSiteRootAddress"`
- `ServerRootAddressFormatKey`: `"App:WebSiteRootAddress"`

**Usage**:
```csharp
var resetPasswordUrl = _webUrlService.GetSiteRootAddress() +
    "Account/ResetPassword?userId=" + userId + "&code=" + code;
```

### MvcAppUrlService.cs
**Purpose**: Enhanced URL service with MVC-specific helpers

**Additional Functionality**:
- Route-based URL generation
- Controller/action URL building
- Query string handling
- Tenant-specific URLs (subdomains or paths)

**Methods**:
- `GetUrl(string controller, string action, object routeValues)`: Generate URL from route
- `GetAbsoluteUrl(string relativePath)`: Convert relative to absolute URL
- `GetTenantUrl(int tenantId, string path)`: Generate tenant-specific URL

**Multi-Tenancy Support**:
```csharp
// Subdomain approach: {tenancy}.example.com
var tenantUrl = _urlService.GetTenantUrl(tenantId, "/dashboard");
// Returns: https://acme.example.com/dashboard

// Path approach: example.com/t/{tenancy}
var tenantUrl = _urlService.GetTenantUrl(tenantId, "/dashboard");
// Returns: https://example.com/t/acme/dashboard
```

## Architecture Notes

### Configuration
URLs configured in `appsettings.json`:
```json
{
  "App": {
    "WebSiteRootAddress": "https://localhost:44302/",
    "ServerRootAddress": "https://localhost:44302/",
    "CorsOrigins": "https://localhost:44302"
  }
}
```

### Environment-Specific URLs
- **Development**: `https://localhost:44302/`
- **Staging**: `https://staging.example.com/`
- **Production**: `https://www.example.com/`

Use environment-specific `appsettings.{Environment}.json` files.

### Multi-Tenancy URL Strategies

#### Subdomain (Recommended)
- Each tenant gets subdomain: `{tenancy}.example.com`
- Requires wildcard DNS: `*.example.com`
- Requires wildcard SSL certificate
- Best user experience (short, branded URLs)

#### Path-Based
- Tenant in URL path: `example.com/t/{tenancy}`
- Single domain, single SSL certificate
- Easier deployment
- Slightly longer URLs

#### Hybrid
- Default domain for host: `admin.example.com`
- Tenant subdomains: `{tenancy}.example.com`
- Custom domains: `customer.com` (enterprise)

### URL Generation Use Cases

#### Email Links
```csharp
// Password reset email
var resetUrl = _urlService.GetUrl("Account", "ResetPassword", new {
    userId = user.Id,
    code = resetCode,
    tenancyName = tenant.TenancyName
});
```

#### Absolute URLs for APIs
```csharp
// API callback URL
var callbackUrl = _urlService.GetAbsoluteUrl($"/api/payments/webhook");
```

#### Tenant-Specific URLs
```csharp
// Dashboard link in email to specific tenant
var dashboardUrl = _urlService.GetTenantUrl(tenantId, "/app/dashboard");
```

## Usage Across Codebase

### Email Services
Generating links in emails:
- Password reset links
- Email confirmation links
- Invitation links
- Receipt/invoice links

### Payment Gateways
Callback URLs for payment confirmations:
- Stripe success/cancel URLs
- PayPal return URLs
- Webhook notification URLs

### External Integrations
API endpoints for webhooks:
- Payment gateway webhooks
- SSO callback URLs
- OAuth redirect URIs

### Controllers
```csharp
public class AccountController {
    private readonly IWebUrlService _webUrlService;

    public async Task<ActionResult> ForgotPassword(string email) {
        var resetLink = _webUrlService.GetUrl("Account", "ResetPassword",
            new { code = resetCode });
        await _emailSender.SendPasswordResetLink(email, resetLink);
        return View();
    }
}
```

## Dependencies

### Internal
- `inzibackend.Configuration`: Configuration access
- `inzibackend.Url`: Base URL services (in Core)
- ABP Session: Tenant context

### External
- ASP.NET Core configuration system
- `IAppConfigurationAccessor`: Configuration reading

## Security Considerations
- Always use HTTPS in production
- Validate URLs before redirecting (prevent open redirect)
- Include tenant context in URLs to prevent cross-tenant access
- Use signed tokens in URLs for sensitive operations

## Testing
- **Unit Tests**: Mock `IWebUrlService` for testing controllers
- **Integration Tests**: Verify correct URL generation in different environments
- **Multi-Tenancy Tests**: Test tenant-specific URL generation

## Configuration Best Practices
- Use environment variables for production URLs
- Never hardcode URLs in code
- Use URL service consistently (don't build URLs manually)
- Configure CORS to match URL service addresses

## Related Documentation
- [Startup/CLAUDE.md](../Startup/CLAUDE.md): Application configuration
- [Controllers/CLAUDE.md](../Controllers/CLAUDE.md): URL usage in controllers
- [inzibackend.Core.Shared/MultiTenancy/CLAUDE.md](../../../inzibackend.Core.Shared/MultiTenancy/CLAUDE.md): Multi-tenancy concepts