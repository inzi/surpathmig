# URL Service Documentation

## Overview
URL generation and management for multi-tenant applications, providing tenant-specific URLs and server address resolution.

## Contents

### Files

#### IWebUrlService.cs
- **Purpose**: Interface for web URL generation
- **Key Methods**:
  - `GetServerRootAddress()`: Get base URL for current tenant
  - `GetServerRootAddress(string tenancyName)`: Get URL for specific tenant
  - Generate absolute URLs from relative paths
  - Support for custom domains per tenant

### Key Components

- **IWebUrlService**: URL generation interface

### Dependencies

- **External Libraries**:
  - ABP Framework
  - Microsoft.AspNetCore.Http

- **Internal Dependencies**:
  - Tenant management
  - Configuration system
  - Multi-tenancy

## Architecture Notes

- **Pattern**: Service pattern
- **Multi-Tenancy**: Tenant-specific URLs
- **Custom Domains**: Support for tenant custom domains
- **Configuration**: URL configuration per tenant

## Business Logic

### URL Patterns

#### Single Domain with Tenancy Name
- Format: `https://app.example.com/?tenancyName=tenant1`
- All tenants share domain
- Tenancy in query string or header

#### Subdomain per Tenant
- Format: `https://tenant1.example.com`
- Each tenant gets subdomain
- DNS wildcard required
- Cleaner URLs

#### Custom Domain per Tenant
- Format: `https://www.tenant1.com`
- Fully custom domains
- Requires DNS configuration
- Professional appearance

### URL Generation Use Cases
- Email links (must be absolute)
- Redirect after login
- Logo URLs in emails
- API callback URLs
- OAuth redirect URIs
- Webhook endpoints
- File download links

## Usage Across Codebase

Used by:
- Email service (template logo URLs)
- Account controller (redirect URLs)
- Payment gateways (success/error URLs)
- External authentication (redirect URIs)
- API documentation
- File download links
- Tenant customization

## Configuration

### Web URL Settings
```json
{
  "App": {
    "WebSiteRootAddress": "https://app.example.com",
    "TenantResolveMode": "Subdomain" // or "QueryString"
  }
}
```

### Tenant-Specific URLs
- Stored in tenant settings
- Override default app URL
- Support custom domains
- SSL certificate management

## Security Considerations

- Validate tenant exists before redirect
- Prevent open redirect vulnerabilities
- HTTPS enforcement
- CORS configuration per tenant
- CSP headers for custom domains

## Extension Points

- Multi-domain support
- Regional domains
- Load balancer integration
- CDN URL generation
- API versioning in URLs
- URL shortening