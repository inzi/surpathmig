# inzibackend.Application.Client Documentation

## Overview
The client-side application layer that provides proxy services for accessing server-side application services via HTTP/REST APIs. This project implements the client-side of the application layer, enabling remote procedure calls from client applications (mobile, desktop, or other web clients) to the server's application services. It uses Flurl HTTP client for modern, fluent API communication and implements comprehensive authentication, multi-tenancy, and error handling.

## Contents

### Files

#### inzibackendClientModule.cs
- **Purpose**: ABP module definition for the client application layer
- **Key Features**:
  - Module initialization and registration
  - Dependency injection setup via convention
  - Assembly scanning for automatic service registration
- **Dependencies**: Inherits from AbpModule
- **Usage**: Entry point for client module initialization

#### ProxyAppServiceBase.cs
- **Purpose**: Base class for all proxy application services
- **Key Features**:
  - Automatic endpoint URL construction by convention
  - Injected AbpApiClient for HTTP operations
  - Service name extraction from class naming
  - Standard API path construction (api/services/app/)
- **Methods**:
  - `GetEndpoint`: Constructs full endpoint URL for service methods
  - `GetServiceUrlSegmentByConvention`: Extracts service name from class
- **Interface**: Implements IApplicationService
- **Pattern**: Template method pattern for proxy services

#### ProxyControllerBase.cs
- **Purpose**: Base class for proxy services that call controller endpoints
- **Key Features**:
  - Alternative to ProxyAppServiceBase for controller-based APIs
  - Transient dependency lifecycle
  - Controller name extraction by convention
- **Dependency**: ITransientDependency (new instance per request)
- **Usage**: For direct controller endpoint access

### Key Components
- **Proxy Services**: Client-side implementations of server interfaces
- **HTTP Client Layer**: AbpApiClient with Flurl integration
- **Authentication**: Token-based authentication with refresh support
- **Multi-tenancy**: Full multi-tenant support with tenant context
- **Error Handling**: Comprehensive error handling with user-friendly messages

### Dependencies
- `Flurl.Http` (v3.2.3): Modern, fluent HTTP client library
- `Flurl` (v3.0.5): URL builder and HTTP client extensions
- `inzibackend.Application.Shared`: Shared interfaces and DTOs
- `ABP Framework`: Core framework for dependency injection and modules

## Subfolders

### ApiClient
Core HTTP client infrastructure including:
- `AbpApiClient`: Main HTTP client with CRUD operations
- `AccessTokenManager`: JWT token lifecycle management
- `ApplicationContext`: Global application state
- Authentication models and handlers
- Multi-tenant support

### Authorization
Complete authorization and user management:
- **Accounts**: Registration, password reset, impersonation
- **Users**: User CRUD and permission management
- **Users/Profile**: Profile management and 2FA setup

### Common
Shared lookup services for dropdowns and searches

### Configuration
User configuration and session management with automatic token refresh

### Extensions
Helper extension methods for error handling and validation

### MultiTenancy
Tenant management operations for SaaS multi-tenancy

### Sessions
Session information and sign-in token management

## Architecture Notes

### Design Patterns
- **Proxy Pattern**: Remote proxy implementation for server services
- **Template Method**: Base classes define common behavior
- **Singleton Pattern**: Shared services use singleton lifecycle
- **Repository Pattern**: Abstracted data access through services

### Communication Architecture
- **RESTful APIs**: All communication via REST endpoints
- **JWT Authentication**: Bearer token authentication
- **Automatic Retry**: Token refresh and request retry
- **Response Wrapping**: ABP Ajax response handling

### Naming Conventions
- Proxy services prefixed with "Proxy"
- Service names extracted by removing prefix/suffix
- Endpoint URLs follow ABP conventions

## Business Logic

### Authentication Flow
1. User provides credentials
2. AccessTokenManager obtains JWT tokens
3. Tokens injected into all API requests
4. Automatic refresh when tokens expire
5. Session timeout handling

### Multi-tenant Context
- Tenant resolved during login
- Tenant ID included in all requests
- Data isolation per tenant
- Host admin can impersonate tenants

### Error Handling
- API errors converted to UserFriendlyException
- Validation errors formatted for display
- Network errors handled gracefully
- Session timeouts trigger re-authentication

## Usage Across Codebase

### Client Applications
- **Mobile Apps**: Xamarin/MAUI applications use these proxies
- **Desktop Apps**: WPF/WinForms clients consume services
- **Web Clients**: Alternative web frontends can use this layer
- **Integration Tests**: Test harnesses use proxy services

### Service Discovery
- Services auto-registered by convention
- Dependency injection provides instances
- Naming convention enables automatic discovery

### Cross-Platform Support
- .NET Standard 2.0 target
- Compatible with .NET Framework and .NET Core
- Platform-agnostic HTTP communication

## Security Considerations

### Authentication Security
- JWT tokens with expiration
- Refresh token rotation
- Secure token storage responsibility of client
- HTTPS required for production

### Authorization
- Permission checks on server side
- Client caches permissions for UI
- Role-based and user-specific permissions

### Data Protection
- No sensitive data in client code
- API keys and secrets server-side only
- Input validation on both client and server

## Performance Optimizations

### HTTP Client
- Connection pooling via singleton client
- Configurable timeout settings
- Multipart upload for files
- Response caching where appropriate

### Token Management
- Proactive token refresh
- Minimal authentication overhead
- Session reuse across requests

## Development Guidelines

### Adding New Proxy Services
1. Create proxy class inheriting from ProxyAppServiceBase
2. Implement server interface
3. Use naming convention (ProxyXxxAppService)
4. Service auto-registered by module

### Error Handling
- Let base classes handle common errors
- Use GetConsolidatedMessage for error display
- Handle specific business errors as needed

### Testing
- Mock AbpApiClient for unit tests
- Use test server for integration tests
- Verify token refresh scenarios

## Cross-References

### Server Dependencies
- `inzibackend.Application`: Server-side application services
- `inzibackend.Application.Shared`: Shared contracts and DTOs
- `inzibackend.Core`: Domain entities and business rules

### Client Dependencies  
- UI frameworks consume proxy services
- Mobile/desktop apps use for backend communication
- Test projects reference for integration testing

### Related Projects
- `inzibackend.Web.Mvc`: Main web application
- `inzibackend.Mobile`: Mobile client applications
- `inzibackend.Web.Host`: API host project