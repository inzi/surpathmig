# Url Documentation

## Overview
This folder contains base classes for URL generation services that support multi-tenant URL schemes. It provides infrastructure for generating tenant-specific URLs using subdomain or path-based tenancy.

## Contents

### Files

#### WebUrlServiceBase.cs
- **Purpose**: Abstract base class for web URL generation with multi-tenant support
- **Key Properties**:
  - **WebSiteRootAddressFormat**: Template for website URLs (from config)
  - **ServerRootAddressFormat**: Template for server/API URLs (from config)
  - **SupportsTenancyNameInUrl**: Checks if URLs support tenant placeholders
  - **TenancyNamePlaceHolder**: Constant `{TENANCY_NAME}` for URL templates
- **Key Methods**:
  - **GetSiteRootAddress(tenancyName)**: Returns website root URL for tenant
  - **GetServerRootAddress(tenancyName)**: Returns server/API root URL for tenant
  - **GetRedirectAllowedExternalWebSites()**: Returns whitelist of allowed redirect targets
- **Pattern**: Template method pattern with abstract properties

#### AppUrlServiceBase.cs
- **Purpose**: Concrete implementation for application-specific URLs (not shown but inferred)
- **Extends**: WebUrlServiceBase
- **Usage**: Implements abstract properties with app-specific configuration keys

### Key Components
- **URL Templates**: Configuration-based URL formats with tenant placeholders
- **Tenant Resolution**: Replaces placeholders with actual tenant names
- **Multi-Tenancy Support**: Subdomain and path-based tenancy
- **Redirect Whitelist**: Security feature for external redirects

### Dependencies
- **Abp.Extensions**: Extension methods (IsNullOrEmpty)
- **Microsoft.Extensions.Configuration**: Configuration access
- **inzibackend.Configuration**: IAppConfigurationAccessor

## Architecture Notes

### Multi-Tenant URL Schemes

#### Subdomain-Based Tenancy
```
WebSiteRootAddressFormat: "https://{TENANCY_NAME}.myapp.com/"

tenant1 → https://tenant1.myapp.com/
tenant2 → https://tenant2.myapp.com/
host    → https://host.myapp.com/
```

#### Path-Based Tenancy
```
WebSiteRootAddressFormat: "https://myapp.com/{TENANCY_NAME}/"

tenant1 → https://myapp.com/tenant1/
tenant2 → https://myapp.com/tenant2/
host    → https://myapp.com/host/
```

#### No Tenancy in URL
```
WebSiteRootAddressFormat: "https://myapp.com/"

All tenants: https://myapp.com/
(Tenant identified by login, not URL)
```

### URL Template Processing
```csharp
ReplaceTenancyNameInUrl(siteRootFormat, tenancyName)
  ↓
Check if template contains {TENANCY_NAME}
  ├─ No → Return template as-is
  └─ Yes → Replace placeholder
      ├─ tenancyName is null → Use "host."
      └─ tenancyName provided → Use "tenancyName."
```

### Configuration Format
```json
{
  "App": {
    "WebSiteRootAddress": "https://{TENANCY_NAME}.myapp.com/",
    "ServerRootAddress": "https://{TENANCY_NAME}.api.myapp.com/",
    "RedirectAllowedExternalWebSites": "https://external1.com,https://external2.com"
  }
}
```

## Business Logic

### GetSiteRootAddress(tenancyName)
**Purpose**: Get website URL for specific tenant
```csharp
var url = GetSiteRootAddress("tenant1");
// Returns: https://tenant1.myapp.com/
```

**Use Cases**:
- Generate email links (password reset, invitation)
- Redirect after login to correct tenant site
- Display tenant-specific URLs in admin panel
- Build breadcrumbs and navigation

### GetServerRootAddress(tenancyName)
**Purpose**: Get API server URL for specific tenant
```csharp
var apiUrl = GetServerRootAddress("tenant1");
// Returns: https://tenant1.api.myapp.com/
```

**Use Cases**:
- Configure API clients with correct base URL
- Generate API documentation links
- CORS configuration for tenant domains
- Webhook callback URLs

### ReplaceTenancyNameInUrl() - URL Template Processing
```csharp
private string ReplaceTenancyNameInUrl(string siteRootFormat, string tenancyName)
{
    // No placeholder, return as-is
    if (!siteRootFormat.Contains(TenancyNamePlaceHolder))
        return siteRootFormat;

    // Remove trailing dot if present: {TENANCY_NAME}. → {TENANCY_NAME}
    if (siteRootFormat.Contains(TenancyNamePlaceHolder + "."))
        siteRootFormat = siteRootFormat.Replace(TenancyNamePlaceHolder + ".", TenancyNamePlaceHolder);

    // No tenant name provided, use "host"
    if (tenancyName.IsNullOrEmpty())
        return siteRootFormat.Replace(TenancyNamePlaceHolder, "host.");

    // Replace with actual tenant name
    return siteRootFormat.Replace(TenancyNamePlaceHolder, tenancyName + ".");
}
```

**Edge Cases Handled**:
1. **Template without placeholder**: `https://myapp.com/` → Return unchanged
2. **Placeholder with dot**: `{TENANCY_NAME}.myapp.com` → Normalize to `{TENANCY_NAME}`
3. **Null tenant name**: Use "host." as tenant name (for host administration)
4. **Valid tenant name**: Insert with trailing dot for subdomain

### GetRedirectAllowedExternalWebSites()
**Purpose**: Security whitelist for external redirects
```csharp
var allowedSites = GetRedirectAllowedExternalWebSites();
// Returns: ["https://external1.com", "https://external2.com"]
```

**Use Cases**:
- Validate redirect URLs after authentication
- Prevent open redirect vulnerabilities
- Allow redirects to trusted partner sites
- SSO integration with external systems

## Usage Across Codebase

### Consumed By
- **Email Templates**: Generate password reset links
- **Account Services**: Redirect after login/logout
- **Tenant Management**: Display tenant URLs
- **API Documentation**: Show tenant-specific API URLs
- **Mobile App Configuration**: Provide API base URLs
- **OAuth/OIDC**: Generate callback URLs

### Typical Usage Pattern
```csharp
public class AccountService
{
    private readonly IWebUrlService _webUrlService;

    public async Task SendPasswordResetEmail(string tenancyName, string email)
    {
        var siteUrl = _webUrlService.GetSiteRootAddress(tenancyName);
        var resetLink = $"{siteUrl}Account/ResetPassword?token={token}";

        await _emailSender.SendAsync(
            email,
            "Reset Password",
            $"Click here to reset: {resetLink}"
        );
    }
}
```

### Configuration Inheritance
Derived classes define configuration keys:
```csharp
public class WebUrlService : WebUrlServiceBase
{
    public override string WebSiteRootAddressFormatKey => "App:WebSiteRootAddress";
    public override string ServerRootAddressFormatKey => "App:ServerRootAddress";
}

public class MvcUrlService : WebUrlServiceBase
{
    public override string WebSiteRootAddressFormatKey => "App:MvcWebSiteRootAddress";
    public override string ServerRootAddressFormatKey => "App:MvcServerRootAddress";
}
```

## Multi-Tenant Considerations

### Tenant Identification Methods
1. **Subdomain**: Tenant identified by subdomain (tenant1.app.com)
2. **Path**: Tenant identified by path (app.com/tenant1)
3. **Header**: Tenant identified by HTTP header (X-Tenant-Name)
4. **Cookie**: Tenant identified by cookie
5. **Login**: Tenant identified during login (not in URL)

This service supports methods 1 and 2 (subdomain and path).

### Host vs. Tenant URLs
- **Host**: Administrative interface (host.myapp.com or /host)
- **Tenant**: Customer-facing interface (tenant1.myapp.com or /tenant1)
- **Null tenancy**: Treated as host in URL generation

### URL Consistency
- All emails use GetSiteRootAddress() for consistency
- API clients use GetServerRootAddress() for correct endpoint
- Redirects validated against whitelist
- URL format configured once, used everywhere

## Security Considerations

### Open Redirect Prevention
```csharp
public bool IsRedirectAllowed(string url)
{
    var allowedSites = GetRedirectAllowedExternalWebSites();
    return allowedSites.Any(site => url.StartsWith(site, StringComparison.OrdinalIgnoreCase));
}
```

**Vulnerability**: Accepting arbitrary redirect URLs can be exploited for phishing
**Protection**: Whitelist approach - only allow configured external sites
**Usage**: Validate all external redirects (OAuth callbacks, external login returns)

### HTTPS Enforcement
- Configuration should always use HTTPS in production
- No code enforcement (trust configuration)
- Consider validation in startup for production environments

### Tenant Isolation
- Each tenant gets unique URL
- Prevents cross-tenant URL confusion
- Subdomain isolation provides additional security (separate cookies domain)

## Performance Considerations

### Configuration Caching
- Configuration read once at startup
- Cached by IConfiguration infrastructure
- No performance impact per request

### String Manipulation
- Simple string replacement (no regex)
- Minimal CPU overhead
- Could cache results per tenant if needed (likely unnecessary)

### URL Generation Frequency
- Generated during authentication (once per session)
- Email sending (infrequent)
- API client initialization (once)
- Not in hot path (no performance concerns)

## Deployment Configurations

### Development
```json
{
  "App": {
    "WebSiteRootAddress": "https://localhost:44302/",
    "ServerRootAddress": "https://localhost:44302/",
    "RedirectAllowedExternalWebSites": ""
  }
}
```

### Production - Subdomain
```json
{
  "App": {
    "WebSiteRootAddress": "https://{TENANCY_NAME}.myapp.com/",
    "ServerRootAddress": "https://{TENANCY_NAME}.api.myapp.com/",
    "RedirectAllowedExternalWebSites": "https://partner.com,https://external.com"
  }
}
```

### Production - Path-Based
```json
{
  "App": {
    "WebSiteRootAddress": "https://myapp.com/{TENANCY_NAME}/",
    "ServerRootAddress": "https://api.myapp.com/{TENANCY_NAME}/",
    "RedirectAllowedExternalWebSites": "https://partner.com"
  }
}
```

### Production - No Tenant in URL
```json
{
  "App": {
    "WebSiteRootAddress": "https://myapp.com/",
    "ServerRootAddress": "https://api.myapp.com/",
    "RedirectAllowedExternalWebSites": ""
  }
}
```

## DNS and Infrastructure Setup

### Subdomain-Based Tenancy
**DNS Configuration**:
```
*.myapp.com → CNAME → myapp.com
myapp.com   → A     → 1.2.3.4
```

**Advantages**:
- Clean URLs
- Separate cookie domains (better isolation)
- Professional appearance

**Disadvantages**:
- Requires wildcard DNS
- SSL certificate complexity (wildcard cert needed)

### Path-Based Tenancy
**DNS Configuration**:
```
myapp.com → A → 1.2.3.4
```

**Advantages**:
- Simple DNS setup
- Single SSL certificate
- Easier local development

**Disadvantages**:
- Less clean URLs
- Shared cookie domain (requires careful handling)

## Troubleshooting

### Wrong URLs Generated
**Symptom**: Generated URLs point to wrong domain
- **Cause**: Incorrect WebSiteRootAddressFormat in configuration
- **Solution**: Verify appsettings.json has correct format

### Host Shows as "host." in URL
**Symptom**: URLs like https://host..myapp.com (double dot)
- **Cause**: Template includes trailing dot: `{TENANCY_NAME}.myapp.com`
- **Solution**: Code handles this, but verify ReplaceTenancyNameInUrl() logic

### External Redirects Blocked
**Symptom**: Legitimate external redirects rejected
- **Cause**: URL not in RedirectAllowedExternalWebSites whitelist
- **Solution**: Add external site to configuration

### Subdomain Not Working
**Symptom**: Subdomain URLs don't resolve
- **Cause**: DNS wildcard not configured
- **Solution**: Add wildcard DNS record (*.myapp.com)

## Best Practices

### Configuration
- ✅ Always use HTTPS in production
- ✅ Include trailing slash in URL formats
- ✅ Test URL generation for each tenant
- ✅ Validate configuration at startup

### Security
- ✅ Always validate external redirects against whitelist
- ✅ Use separate domains for API and web (CORS security)
- ✅ Consider subdomain isolation for sensitive data
- ✅ Enforce HTTPS for all tenant URLs

### Testing
```csharp
[Fact]
public void Should_Generate_Correct_Tenant_URL()
{
    // Arrange
    var config = CreateConfigWithFormat("https://{TENANCY_NAME}.app.com/");
    var urlService = new WebUrlService(config);

    // Act
    var url = urlService.GetSiteRootAddress("tenant1");

    // Assert
    Assert.Equal("https://tenant1.app.com/", url);
}
```