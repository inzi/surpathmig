# Startup Documentation

## Overview
The Startup folder contains the core initialization and configuration code for the Web API host application. It orchestrates the application bootstrap process, configures services, middleware pipeline, authentication mechanisms, and integrates various third-party services and frameworks.

## Contents

### Files

#### Program.cs
- **Purpose**: Entry point for the Web API host application
- **Key Functionality**:
  - Creates and configures the web host builder
  - Sets up Kestrel server with custom limits (16KB max request line size)
  - Configures content root directory
  - Sets up logging with Entity Framework command filtering
  - Enables IIS and IIS Integration support
  - Initializes the Startup class for application configuration

#### Startup.cs
- **Purpose**: Main configuration hub for the ASP.NET Core application
- **Key Components**:
  - Service configuration (ConfigureServices method)
  - Middleware pipeline setup (Configure method)
  - CORS configuration for cross-origin requests
  - Authentication and authorization setup
  - Swagger/OpenAPI documentation configuration
  - SignalR hub configuration for real-time communication
  - Hangfire background job processing setup
  - Health checks configuration
  - GraphQL API setup
  - Stripe payment integration
  - Multiple environment configurations (Development, Production)
- **Notable Settings**:
  - Default CORS policy name: "localhost"
  - Supports wildcard subdomain CORS origins
  - Conditional compilation for debug/release modes
  - Custom Surpath settings initialization
  - File upload limits and allowed extensions configuration

#### AuthConfigurer.cs
- **Purpose**: Centralizes authentication configuration for JWT Bearer and IdentityServer
- **Key Features**:
  - JWT Bearer token authentication with symmetric key validation
  - Async JWT token validation support
  - IdentityServer authentication for OAuth/OpenID Connect
  - SignalR authentication via encrypted query string tokens
  - Special handling for file download endpoints with encrypted tokens
  - Token validation parameters (issuer, audience, lifetime)
  - Support for anonymous SignalR connections (configurable)
- **Protected Endpoints**:
  - `/Chat/GetUploadedObject`
  - `/Profile/GetProfilePictureByUser`

#### inzibackendWebHostModule.cs
- **Purpose**: ABP module that initializes the Web Host application
- **Key Responsibilities**:
  - Module dependency configuration
  - Multi-tenancy domain format setup
  - License code configuration
  - Background worker registration:
    - SubscriptionExpirationCheckWorker
    - SubscriptionExpireEmailNotifierWorker
    - ExpiredAuditLogDeleterWorker
    - PasswordExpirationBackgroundWorker
    - ComplianceExpireBackgoundService (registered twice - potential bug)
  - External authentication provider configuration
  - Support for tenant-specific or host-wide social login settings
- **Supported External Auth Providers**:
  - OpenID Connect
  - WS-Federation
  - Facebook
  - Twitter
  - Google
  - Microsoft

### Key Components

#### Middleware Pipeline
1. ABP Framework initialization
2. Exception handling (SurpathExceptionLogger)
3. Static files serving
4. CORS
5. Authentication (JWT/IdentityServer)
6. Authorization
7. Request localization
8. SignalR hubs
9. Health checks
10. Swagger UI

#### Service Registrations
- MVC with Razor runtime compilation (DEBUG only)
- SignalR for real-time communication
- CORS with dynamic origin configuration
- Kestrel HTTPS configuration
- Identity registration
- Swagger/OpenAPI documentation
- reCAPTCHA v3 integration
- Hangfire with MySQL storage
- GraphQL with Playground UI
- Health checks with UI dashboard

### Dependencies

#### External Libraries
- ABP Framework (core dependency)
- ASP.NET Core (web framework)
- IdentityServer4 (OAuth/OpenID)
- Hangfire (background jobs)
- Stripe.NET (payment processing)
- Swashbuckle (Swagger/OpenAPI)
- GraphQL.Server
- HealthChecks.UI
- Owl.reCAPTCHA
- Log4Net (logging)

#### Internal Dependencies
- inzibackend.EntityFrameworkCore
- inzibackend.Configuration
- inzibackend.Authorization
- inzibackend.Web.Core
- inzibackend.Web.Chat
- inzibackend.Web.Common
- inzibackend.Web.IdentityServer
- inzibackend.Web.Swagger
- inzibackend.Web.Surpath

## Subfolders

### ExternalLoginInfoProviders
[See detailed documentation in ExternalLoginInfoProviders/CLAUDE.md]

Implements tenant-aware external authentication providers with caching support for various OAuth/SSO providers including Google, Facebook, Microsoft, Twitter, OpenID Connect, and WS-Federation.

## Architecture Notes

### Configuration Hierarchy
1. appsettings.json (base configuration)
2. appsettings.{Environment}.json (environment-specific)
3. User secrets (development only)
4. Environment variables
5. Command-line arguments

### Security Patterns
- JWT tokens with symmetric key encryption
- Encrypted query string tokens for SignalR
- Anti-forgery token validation
- CORS policy enforcement
- HTTPS enforcement via Kestrel

### Performance Optimizations
- Response compression
- Static file caching
- Configuration caching for external providers
- Hangfire for background job processing
- Connection pooling for database

### Multi-Tenancy Support
- Tenant-based domain resolution
- Per-tenant authentication settings
- Tenant-specific background workers
- Isolated data access per tenant

## Business Logic

### Authentication Flow
1. Supports multiple authentication schemes simultaneously
2. JWT Bearer for API authentication
3. IdentityServer for SSO/OAuth flows
4. External providers for social login
5. Special token handling for SignalR and file downloads

### Background Jobs
- Subscription expiration monitoring
- Email notifications for expiring subscriptions
- Audit log cleanup
- Password expiration checks
- Compliance expiration processing

### File Upload Configuration
- Maximum file size limits (configurable)
- Allowed file extensions validation
- Profile picture size limits
- Temporary file storage configuration

## Usage Across Codebase

### Direct Consumers
- Controllers inherit authentication/authorization from this configuration
- SignalR hubs use the configured authentication
- Background services use the registered workers
- API endpoints leverage Swagger documentation
- Health check endpoints monitor application status

### Configuration Impact
- All API endpoints affected by CORS settings
- Authentication required for protected endpoints
- Background jobs run based on configuration
- External login availability controlled here
- Logging levels affect entire application