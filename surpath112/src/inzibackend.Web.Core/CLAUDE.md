# inzibackend.Web.Core Documentation

## Overview
The Web.Core project contains shared web infrastructure used by both the MVC (inzibackend.Web.Mvc) and API Host (inzibackend.Web.Host) projects. It provides authentication, authorization, controllers, real-time communication, and web-specific services that form the foundation of the web layer.

## Contents

### Files

#### inzibackendWebCoreModule.cs
- **Purpose**: Main module configuration for Web.Core
- **Key Features**:
  - Module dependencies configuration (Application, EntityFrameworkCore, ABP modules)
  - JWT token authentication setup
  - SignalR integration for real-time features
  - Redis cache configuration (optional)
  - Hangfire background job configuration
  - Language localization with database storage
- **Configuration**: Reads from appsettings.json for authentication, caching, and job settings
- **Initialization**: Registers services and configures application folders

#### inzibackend.Web.Core.csproj
- **Purpose**: Project file defining dependencies and build configuration
- **Target Framework**: .NET 6.0
- **Key Dependencies**: ABP framework, SignalR, JWT, Redis, Hangfire

### Key Components
- **Authentication System**: Complete JWT and external auth implementation
- **Base Controllers**: Reusable controller functionality
- **Real-time Communication**: SignalR chat and notifications
- **Configuration Management**: Runtime configuration access
- **Security Features**: reCAPTCHA, validation, protection

### Dependencies
- Abp.AspNetZeroCore.Web (ABP Zero web framework)
- Abp.AspNetCore.SignalR (real-time communication)
- Microsoft.AspNetCore.Authentication.JwtBearer (JWT auth)
- Abp.RedisCache (distributed caching)
- Abp.HangfireAspNetCore (background jobs)
- inzibackend.Application (business logic layer)
- inzibackend.EntityFrameworkCore (data access layer)

## Subfolders

### [Authentication](Authentication/CLAUDE.md)
Complete authentication infrastructure for JWT and external providers
- **[JWT Bearer](Authentication/JwtBearer/CLAUDE.md)**: Token-based API authentication with security stamps and refresh tokens
- **[External](Authentication/External/CLAUDE.md)**: OAuth/SSO provider integration (Google, Microsoft, Facebook, WS-Federation)
- **[Two-Factor](Authentication/TwoFactor/CLAUDE.md)**: 2FA code caching and validation infrastructure
- **Business Value**: Secure, flexible authentication supporting enterprise SSO and modern auth flows

### [Controllers](Controllers/CLAUDE.md)
Base and specialized controllers for core web functionality
- **inzibackendControllerBase**: Base class with localization, error handling, and common utilities
- **TokenAuthController**: Complete authentication endpoint implementation with external login support
- **FileController**: Binary file and document management with multi-tenant isolation
- **Profile/Chat/Users**: Specialized base controllers for feature-specific operations
- **Payment Controllers**: Stripe and Authorize.Net integration for payment processing
- **Business Value**: Consistent API surface, code reuse, and standardized error handling

### [Models](Models/CLAUDE.md)
Data transfer objects and view models for web layer
- **[TokenAuth](Models/TokenAuth/CLAUDE.md)**: Authentication request/response models for login, external auth, and token refresh
- **[Consent](Models/Consent/CLAUDE.md)**: OAuth consent flow models for IdentityServer integration
- **Business Value**: Type-safe API contracts and clear data structures

### [Chat](Chat/CLAUDE.md)
Real-time communication using SignalR
- **[SignalR](Chat/SignalR/CLAUDE.md)**: ChatHub implementation with WebSocket connection management
- **Message Routing**: Tenant-aware message delivery and user-to-user chat
- **Presence Tracking**: Online/offline status and connection management
- **Business Value**: Instant messaging and collaboration features

### [Configuration](Configuration/CLAUDE.md)
Runtime configuration access and modification
- **AppConfigurationAccessor**: Read configuration values from appsettings.json
- **AppConfigurationWriter**: Update configuration at runtime with persistence
- **Business Value**: Dynamic configuration without application restarts

### [Security](Security/CLAUDE.md)
Web security components
- **[Recaptcha](Security/Recaptcha/CLAUDE.md)**: Google reCAPTCHA v2/v3 integration with validation
- **Validation**: Input sanitization and security checks
- **Business Value**: Bot protection, spam prevention, and security hardening

### [Surpath](Surpath/CLAUDE.md)
Domain-specific infrastructure
- **WatchDogLogger**: Enhanced exception logging with IP tracking and tenant context
- **Business Value**: Comprehensive error tracking and diagnostics for support

### [Common](Common/CLAUDE.md)
Shared constants and utilities
- **WebConsts**: Web-specific constants (Swagger URLs, Hangfire endpoints, feature flags)
- **GraphQL Configuration**: GraphQL endpoint and playground settings
- **reCAPTCHA Whitelist**: Bypass list for trusted user agents
- **Business Value**: Centralized configuration values and feature toggles

### [DashboardCustomization](DashboardCustomization/CLAUDE.md)
Dashboard widget and view customization infrastructure
- **Widget Definitions**: Configurable dashboard components with dynamic loading
- **View Definitions**: Customizable dashboard layouts with associated assets
- **Filter Definitions**: Reusable filter controls for widget data
- **Business Value**: Personalized user dashboards and extensible widget system

### [Extensions](Extensions/CLAUDE.md)
Extension methods for web components
- **ApplicationBuilderExtensions**: Middleware configuration helpers for forwarded headers
- **Proxy Support**: Handles X-Forwarded-For and X-Forwarded-Proto headers
- **Business Value**: Simplified startup configuration and proper proxy integration

### [HealthCheck](HealthCheck/CLAUDE.md)
Application health monitoring infrastructure
- **AbpZeroHealthCheck**: Health check registration for database, users, and cache
- **Kubernetes Integration**: Readiness and liveness probe support
- **Business Value**: Application monitoring, reliability, and automated health checks

### [Helpers](Helpers/CLAUDE.md)
Utility classes for web layer
- **CurrentDirectoryHelpers**: IIS in-process hosting directory resolution
- **ANCM Integration**: Windows/IIS specific path fixing for configuration files
- **Business Value**: Cross-platform path handling and IIS compatibility

### [IdentityServer](IdentityServer/CLAUDE.md)
OAuth/OpenID Connect provider configuration
- **IdentityServerConfig**: IdentityServer 4 setup with API resources, scopes, and clients
- **Client Configuration**: Dynamic client loading from appsettings.json
- **OAuth Flows**: Support for authorization code, client credentials, and refresh tokens
- **Business Value**: OAuth provider capabilities and third-party authentication

### [Navigation](Navigation/CLAUDE.md)
Menu and navigation infrastructure
- **UserMenuItemExtensions**: Active menu detection, URL calculation, and custom ordering
- **Recursive Menu Support**: Multi-level menu hierarchy with active state propagation
- **Business Value**: Dynamic navigation management and consistent menu rendering

### [Properties](Properties/CLAUDE.md)
Assembly metadata and development configuration
- **AssemblyInfo.cs**: Assembly-level attributes and COM interop settings
- **launchSettings.json**: Development launch profiles for Visual Studio/Rider
- **Business Value**: Project metadata and development environment configuration

### [Session](Session/CLAUDE.md)
User session management and caching
- **PerRequestSessionCache**: Per-request caching of user session data
- **Performance Optimization**: Reduces database queries within single HTTP request
- **Business Value**: Stateful user experience and improved performance

### [Swagger](Swagger/CLAUDE.md)
API documentation generation and customization
- **Swagger Extensions**: ABP base URL injection and custom schema naming
- **Enum Filters**: Enhanced enum documentation with available values
- **Operation Filters**: Custom operation IDs and metadata
- **Business Value**: Interactive API documentation and client SDK generation

### [UiCustomization](UiCustomization/CLAUDE.md)
User interface customization infrastructure
- **UiThemeCustomizerFactory**: Factory for theme customizer selection
- **[Metronic](UiCustomization/Metronic/CLAUDE.md)**: 14 Metronic theme variants with multi-level settings
  - Theme variants: Default, Theme0, Theme2-13
  - Settings hierarchy: Application → Tenant → User
  - Dark mode with automatic color scheme switching
  - Configurable layouts, menus, headers, and footers
- **Business Value**: Branded user experience and tenant-specific theming

### [Url](Url/CLAUDE.md)
URL generation and management for multi-tenant scenarios
- **WebUrlServiceBase**: Base class for URL generation with tenant support
- **Multi-Tenant URLs**: Subdomain and path-based tenancy support
- **Redirect Whitelist**: Security feature for external redirects
- **Business Value**: Consistent URL generation and tenant isolation

## Architecture Notes
- **Shared Infrastructure**: Used by both MVC and API projects
- **Modular Design**: Features organized in focused modules
- **Dependency Injection**: All services registered via DI
- **Multi-tenant**: Complete tenant isolation throughout
- **Async/Await**: Async patterns for scalability
- **Caching**: Redis support for distributed scenarios

## Business Logic
- **Authentication Flow**: JWT tokens with refresh capability and external provider support
- **Real-time Features**: SignalR for instant updates and notifications
- **File Management**: Abstract storage with provider support (Azure Blob, local filesystem)
- **Configuration**: Hot-reload capable settings with runtime modification
- **Security**: Multiple layers of protection (JWT, reCAPTCHA, HTTPS, tenant isolation)

## Security Considerations
- JWT tokens with expiration and refresh
- Security stamps prevent token reuse after password change
- External auth reduces password exposure
- reCAPTCHA prevents automated attacks and bot traffic
- Tenant isolation enforced throughout all operations
- HTTPS enforcement in production
- Forwarded headers validation for proxy scenarios
- OAuth consent flows for third-party access

## Usage Across Codebase
- **inzibackend.Web.Mvc**: MVC project depends on Web.Core for shared infrastructure
- **inzibackend.Web.Host**: API host depends on Web.Core for authentication and controllers
- **Mobile Apps**: Consume APIs defined in Web.Core controllers
- **Background Jobs**: Use services from Web.Core for operations
- **Integration Tests**: Test Web.Core components and endpoints

## Configuration
Key configuration sections:
- `Authentication:JwtBearer`: JWT token settings (issuer, audience, security key)
- `Authentication:External`: OAuth provider configuration (Google, Microsoft, Facebook)
- `Abp:RedisCache`: Redis cache settings for distributed scenarios
- `Storage`: File storage provider settings (Azure, local)
- `Payment`: Payment processor configuration (Stripe, Authorize.Net)
- `App:WebSiteRootAddress`: Multi-tenant URL template
- `Recaptcha`: reCAPTCHA site key and secret key

## Cross-References and Impact Analysis
The Web.Core module is referenced by:
- **inzibackend.Web.Mvc**: Main web application (direct dependency)
- **inzibackend.Web.Host**: API host project (direct dependency)
- **Test Projects**: Integration testing of web layer

Changes to Web.Core impact:
- All web authentication flows (login, external auth, 2FA)
- API endpoint definitions and base controller functionality
- Real-time communication features (chat, notifications)
- File upload/download functionality across all projects
- Payment processing for Stripe and Authorize.Net
- User session management and caching
- Theme customization and UI branding
- Health check monitoring and reliability

## Performance Considerations
- Redis caching for scalability in multi-server scenarios
- Async operations throughout for better resource utilization
- SignalR for efficient real-time updates (WebSocket fallback)
- Hangfire for background processing (job queuing)
- Connection pooling for database operations
- Per-request session caching to reduce database load
- Swagger document generation cached at startup

## Deployment Notes
- Requires HTTPS in production for security
- Redis optional but recommended for scale (multi-server deployments)
- SignalR requires WebSocket support (configure load balancer)
- Configuration transforms per environment (appsettings.{env}.json)
- Secrets management via environment variables or Azure Key Vault
- IIS in-process hosting requires CurrentDirectoryHelpers
- Forwarded headers middleware required behind proxy/load balancer
- Health check endpoints for Kubernetes probes (/health)

## Documentation Status
**Complete Documentation**: All 28 folders fully documented with CLAUDE.md files

Last Updated: 2025-09-29