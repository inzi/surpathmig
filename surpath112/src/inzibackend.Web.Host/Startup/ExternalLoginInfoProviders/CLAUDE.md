# ExternalLoginInfoProviders Documentation

## Overview
This folder contains tenant-aware external authentication providers for the application's multi-tenant architecture. It implements a flexible system for managing OAuth/SSO login configurations that can be customized per tenant or use host-level defaults.

## Contents

### Files

#### Core Infrastructure
- **TenantBasedExternalLoginInfoProviderBase.cs**
  - Abstract base class for all tenant-based external login providers
  - Implements tenant-aware configuration retrieval with caching
  - Falls back to host settings when tenant settings are not available
  - Uses ABP session to determine current tenant context
  - Cache keys are formatted as `{ProviderName}-{TenantId}` for tenant-specific settings

- **ExternalLoginInfoProvidersCacheManagerExtensions.cs**
  - Extension methods for cache manager to get typed cache for external login providers
  - Cache name: "AppExternalLoginInfoProvidersCache"
  - Provides centralized cache access for all external login configurations

- **ExternalLoginOptionsCacheManager.cs**
  - Manages caching of external login options
  - Coordinates between different providers and their configurations

#### Provider Implementations
- **TenantBasedGoogleExternalLoginInfoProvider.cs**
  - Google OAuth provider implementation
  - Settings include ClientId, ClientSecret, and UserInfoEndpoint
  - Reads from AppSettings.ExternalLoginProvider.Tenant.Google (tenant) or Host.Google (host)
  - Implements ISingletonDependency for application lifetime

- **TenantBasedFacebookExternalLoginInfoProvider.cs**
  - Facebook OAuth provider implementation
  - Similar structure to Google provider
  - Tenant-specific Facebook authentication settings

- **TenantBasedMicrosoftExternalLoginInfoProvider.cs**
  - Microsoft OAuth/Azure AD provider implementation
  - Supports Microsoft account authentication
  - Tenant-aware configuration management

- **TenantBasedTwitterExternalLoginInfoProvider.cs**
  - Twitter OAuth provider implementation
  - Handles Twitter-specific authentication flow
  - Tenant-based configuration support

- **TenantBasedOpenIdConnectExternalLoginInfoProvider.cs**
  - Generic OpenID Connect provider implementation
  - Supports any OpenID Connect compliant identity provider
  - Flexible configuration for custom SSO solutions

- **TenantBasedWsFederationExternalLoginInfoProvider.cs**
  - WS-Federation protocol implementation
  - Supports SAML-based authentication
  - Enterprise SSO integration capabilities

### Key Components
- **IExternalLoginInfoProvider** interface implementation across all providers
- Tenant-based settings management with host fallback
- Caching layer for performance optimization
- JSON serialization for complex provider settings

### Dependencies
- **External Libraries**
  - Abp.AspNetZeroCore.Web.Authentication.External
  - Abp.Configuration
  - Abp.Runtime.Caching
  - Abp.Runtime.Session
  - Abp.Dependency
  - Abp.Json

- **Internal Dependencies**
  - inzibackend.Authentication
  - inzibackend.Configuration (AppSettings)

## Architecture Notes

### Multi-Tenant Authentication Pattern
1. Each provider checks if the current session has a tenant ID
2. If tenant-specific settings exist, they are used
3. Otherwise, falls back to host-level settings
4. All settings are cached to avoid repeated database queries

### Caching Strategy
- Provider configurations are cached using a typed cache
- Cache keys include tenant ID for isolation
- Cache invalidation handled by ABP framework
- Reduces database load for authentication flows

### Provider Registration
- All providers implement ISingletonDependency
- Registered once at application startup
- Available throughout application lifetime

## Business Logic
- **Tenant Isolation**: Each tenant can have their own OAuth/SSO configurations
- **Fallback Mechanism**: If a tenant hasn't configured a provider, host settings are used
- **Dynamic Provider Loading**: Providers are loaded based on configuration presence
- **Security**: Client secrets are stored in settings and retrieved securely

## Usage Across Codebase
These providers are consumed by:
- Authentication middleware during login flows
- Account controller for external login handling
- Startup configuration for authentication setup
- Token authentication for API access