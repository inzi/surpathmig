# Url Documentation

## Overview
The Url folder contains URL generation services that construct application URLs for various purposes including email links, client application routing, and server API endpoints. These services ensure consistent URL formatting across the multi-tenant application.

## Contents

### Files

#### WebUrlService.cs
- **Purpose**: Provides URL generation for web application endpoints
- **Inherits**: WebUrlServiceBase
- **Implements**: IWebUrlService, ITransientDependency
- **Key Configuration**:
  - `WebSiteRootAddressFormatKey`: "App:ClientRootAddress" - Points to the client application URL
  - `ServerRootAddressFormatKey`: "App:ServerRootAddress" - Points to the server API URL
- **Dependency Injection**: Registered as transient service
- **Functionality**:
  - Generates URLs for client-side application
  - Generates URLs for server-side API endpoints
  - Configuration-driven URL formatting

#### AngularAppUrlService.cs
- **Purpose**: Generates URLs specific to Angular application routes
- **Inherits**: AppUrlServiceBase
- **Key Routes**:
  - `EmailActivationRoute`: "account/confirm-email" - Email confirmation page
  - `PasswordResetRoute`: "account/reset-password" - Password reset page
- **Dependencies**:
  - IWebUrlService for base URL generation
  - ITenantCache for multi-tenant URL construction
- **Functionality**:
  - Creates tenant-aware URLs for email links
  - Generates activation and password reset URLs
  - Ensures proper routing to Angular app pages

### Key Components

#### URL Generation Pattern
1. Base URL from configuration (ClientRootAddress/ServerRootAddress)
2. Tenant subdomain injection for multi-tenant scenarios
3. Route path appending for specific pages
4. Query parameter handling for tokens and IDs

#### Configuration Keys
- `App:ClientRootAddress`: Base URL for client application
- `App:ServerRootAddress`: Base URL for API server
- Support for tenant-specific subdomains

### Dependencies

#### External Libraries
- Abp.Dependency (IoC container)
- Abp.MultiTenancy (tenant management)

#### Internal Dependencies
- inzibackend.Configuration (configuration access)
- inzibackend.Url (base URL service interfaces)
- Base classes from Web.Core or shared libraries

## Architecture Notes

### Service Registration
- WebUrlService: Transient lifetime (new instance per request)
- AngularAppUrlService: Likely transient or scoped

### Multi-Tenancy Support
- URLs automatically adjusted for tenant subdomains
- Tenant context retrieved from ITenantCache
- Format: `https://{tenant}.domain.com/route`

### Configuration-Driven Design
- All base URLs from configuration
- No hardcoded URLs in code
- Environment-specific URL configuration supported

## Business Logic

### Email Link Generation
1. **Account Activation**:
   - Generated when new users register
   - Contains activation token in query string
   - Routes to Angular confirmation page

2. **Password Reset**:
   - Generated for forgot password requests
   - Contains reset token in query string
   - Routes to Angular password reset page

### URL Construction Rules
- Client URLs for user-facing pages
- Server URLs for API calls
- Tenant-specific URLs when in tenant context
- Host URLs when in host context

## Usage Across Codebase

### Direct Consumers

#### Email Services
- Account activation emails use AngularAppUrlService
- Password reset emails use AngularAppUrlService
- Notification emails may use WebUrlService

#### Authentication Services
- OAuth redirect URLs constructed via WebUrlService
- External login callbacks use server URLs

#### API Controllers
- Return URLs in responses using these services
- Webhook URLs generated for external services

### Common Patterns
```csharp
// Generate activation link
var activationLink = angularAppUrlService.CreateEmailActivationUrlFormat(tenantId);

// Generate API endpoint
var apiUrl = webUrlService.GetServerRootAddress();

// Generate client app URL
var clientUrl = webUrlService.GetWebSiteRootAddress();
```

### Impact Areas
- All outbound emails with links
- OAuth/SSO configuration
- Multi-tenant routing
- API response URLs
- File download links