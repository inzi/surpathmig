# Startup/AuthConfigurers Documentation

## Overview
This folder contains tenant-aware external authentication provider configuration classes. It implements a dynamic options pattern that allows different tenants in the multi-tenant system to have their own OAuth/SSO provider settings (Facebook, Google, Microsoft, Twitter, OpenID Connect, WS-Federation) without affecting other tenants.

## Files

### TenantBasedSocialLoginOptionsBase.cs
**Purpose**: Abstract base class for tenant-aware external authentication options

**Key Features**:
- Implements `OptionsMonitor<TOptions>` pattern
- Provides tenant-specific configuration caching
- Separates host settings from tenant settings
- Uses ABP session to determine current tenant context

**Core Methods**:
- `Get(string name)`: Retrieves options for current tenant or host
- `TenantHasSettings()`: Abstract method to check if tenant has custom settings
- `GetTenantSettings()` / `GetHostSettings()`: Abstract methods to retrieve settings
- `SetOptions()`: Abstract method to apply settings to options object
- `GetTenantCacheKey()`: Creates tenant-specific cache keys

**Pattern**: Template method pattern with caching

### Provider-Specific Implementations

#### TenantBasedFacebookOptions.cs
- Configures Facebook OAuth authentication per tenant
- Reads AppId and AppSecret from tenant settings
- Falls back to host settings if tenant doesn't configure

#### TenantBasedGoogleOptions.cs
- Configures Google OAuth authentication per tenant
- Reads ClientId and ClientSecret from tenant settings
- Supports tenant-specific redirect URIs

#### TenantBasedTwitterOptions.cs
- Configures Twitter OAuth authentication per tenant
- Reads ConsumerKey and ConsumerSecret from tenant settings
- Handles Twitter OAuth 1.0a specifics

#### TenantBasedMicrosoftAccountOptions.cs
- Configures Microsoft Account authentication per tenant
- Reads ClientId and ClientSecret from tenant settings
- Supports Azure AD integration

#### TenantBasedOpenIdConnectOptions.cs
- Configures OpenID Connect authentication per tenant
- Supports custom identity providers (Okta, Auth0, etc.)
- Reads Authority, ClientId, ClientSecret from tenant settings

#### TenantBasedWsFederationOptions.cs
- Configures WS-Federation authentication per tenant
- Supports ADFS and enterprise SSO
- Reads MetadataAddress, Wtrealm from tenant settings

### ExternalLoginOptionsCacheManager.cs
**Purpose**: Manages cache invalidation for all external login providers

**Key Features**:
- Implements `IExternalLoginOptionsCacheManager`
- Provides `ClearCache()` method to invalidate all provider caches
- Tenant-aware cache key generation
- Injected into all external login configurers

**Cache Keys**:
- Host: `{ProviderName}` (e.g., "Google", "Facebook")
- Tenant: `{ProviderName}-{TenantId}` (e.g., "Google-42")

**Usage**: Called when tenant settings are updated to ensure fresh configuration is loaded

## Architecture Notes

### Multi-Tenancy Support
- Each tenant can configure their own OAuth provider credentials
- Host configuration serves as default/fallback
- Settings stored in tenant-specific configuration (typically database)
- Cache prevents repeated database lookups

### Options Pattern
- Uses ASP.NET Core `IOptions` pattern
- Extends `OptionsMonitor<T>` for dynamic configuration
- Integrates with authentication middleware
- Cache invalidation on settings change

### Security Considerations
- Credentials stored encrypted in database
- Tenant isolation prevents cross-tenant access
- Cache keys include tenant ID to prevent leakage
- No credentials in code or configuration files

## Dependencies

### Internal
- `inzibackend.Configuration`: Settings management
- ABP Session: Tenant context determination

### External
- `Microsoft.AspNetCore.Authentication.*`: Provider-specific packages
- `Microsoft.Extensions.Options`: Options pattern infrastructure
- `Abp.Dependency`: Dependency injection
- `Abp.Runtime.Session`: Session management

## Usage Across Codebase

### Registration
- Registered in `Startup.cs` during authentication configuration
- One instance per provider type
- Scoped to authentication middleware

### Invocation
- Called automatically by authentication middleware
- Triggered on external login attempts
- Cache refreshed when tenant settings change

### Settings Source
- Host settings: `appsettings.json` or host database
- Tenant settings: Tenant-specific database records
- Retrieved via ABP Settings system

## Business Logic

### Configuration Flow
1. User initiates external login (e.g., "Login with Google")
2. Authentication middleware requests options for current tenant
3. `Get()` method checks if tenant has custom settings
4. If yes, retrieves tenant-specific options from cache or creates new
5. If no, retrieves host options from cache or creates new
6. Options applied to authentication challenge
7. User redirected to external provider with tenant-specific credentials

### Cache Strategy
- Options cached per tenant per provider
- Cache invalidated when settings updated
- Reduces database queries for high-traffic tenants
- Memory-efficient (only active tenants cached)

## Extension Points

### Adding New Provider
1. Create `TenantBased{Provider}Options.cs` inheriting from base class
2. Implement abstract methods for settings retrieval
3. Add cache entry to `ExternalLoginOptionsCacheManager`
4. Register in `Startup.cs` authentication configuration

### Custom Settings
- Override `GetTenantSettings()` to read additional properties
- Extend `SetOptions()` to apply custom configuration
- Add validation in derived classes

## Related Documentation
- [Startup/CLAUDE.md](../CLAUDE.md): Parent folder documentation
- [inzibackend.Web.Core/Authentication/External/CLAUDE.md](../../../../inzibackend.Web.Core/Authentication/External/CLAUDE.md): External authentication infrastructure
- [inzibackend.Core.Shared/Authentication/CLAUDE.md](../../../../inzibackend.Core.Shared/Authentication/CLAUDE.md): Authentication constants