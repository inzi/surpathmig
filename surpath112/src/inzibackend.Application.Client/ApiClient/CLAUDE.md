# ApiClient Documentation

## Overview
Core API client infrastructure that provides HTTP communication between client applications and the server API. This folder contains the foundational classes for authentication, token management, and HTTP request handling using the Flurl HTTP client library.

## Contents

### Files

#### AbpApiClient.cs
- **Purpose**: Main HTTP client wrapper providing typed API calls with authentication
- **Key Features**:
  - Full CRUD operations (GET, POST, PUT, DELETE) with generic type support
  - Automatic token injection for authenticated requests
  - Anonymous endpoint support for public APIs
  - Multipart/file upload capabilities
  - ABP Ajax response wrapper handling
  - Tenant and culture header management
- **Methods**: PostAsync<T>, GetAsync<T>, PutAsync<T>, DeleteAsync<T>, PostMultipartAsync<T>
- **Dependencies**: Flurl.Http for HTTP operations, IAccessTokenManager for auth tokens

#### AccessTokenManager.cs
- **Purpose**: Manages authentication tokens, login/logout, and token refresh operations
- **Key Features**:
  - Login with username/password or email
  - Automatic refresh token management
  - Token expiration tracking
  - Multi-tenancy support during authentication
- **Properties**: 
  - `IsUserLoggedIn`: Indicates if user has valid access token
  - `IsRefreshTokenExpired`: Checks refresh token validity
  - `AuthenticateResult`: Stores current authentication state
- **Security**: Handles token lifecycle and secure storage

#### ApplicationContext.cs
- **Purpose**: Maintains application-wide context including tenant, user, and configuration
- **Key Features**:
  - Current tenant information management
  - Login information storage
  - User configuration caching
  - Language/localization context
- **Usage**: Singleton service providing global application state

#### AuthenticationHttpHandler.cs
- **Purpose**: HTTP message handler for automatic authentication header injection
- **Features**: Intercepts HTTP requests to add authentication tokens

#### ApiUrlConfig.cs
- **Purpose**: Configuration class for API base URLs
- **Usage**: Centralizes API endpoint configuration

#### TenantInformation.cs
- **Purpose**: Data model for tenant context
- **Properties**: TenancyName, TenantId

#### DebugServerIpAddresses.cs
- **Purpose**: Development/debug configuration for server endpoints
- **Usage**: Allows dynamic server URL configuration during development

#### ModernHttpClientFactory.cs
- **Purpose**: Factory for creating optimized HTTP clients
- **Features**: Platform-specific HTTP client optimizations

#### IAccessTokenManager.cs
- **Purpose**: Interface defining token management contract
- **Methods**: GetAccessToken, LoginAsync, RefreshTokenAsync, Logout

#### IApplicationContext.cs
- **Purpose**: Interface for application context operations
- **Methods**: SetAsTenant, SetAsHost, SetLoginInfo, ClearLoginInfo

### Key Components
- **HTTP Client Layer**: AbpApiClient with Flurl integration
- **Authentication Layer**: AccessTokenManager with JWT token handling
- **Context Management**: ApplicationContext for global state
- **Multi-tenancy Support**: Built-in tenant resolution and headers

### Dependencies
- `Flurl.Http`: Modern HTTP client library
- `Abp.Dependency`: Dependency injection framework
- `Abp.Web.Models`: ABP response models
- `inzibackend.Application.Shared`: Shared DTOs and interfaces

## Subfolders

### Models
Contains authentication request/response models including AbpAuthenticateModel and AbpAuthenticateResultModel for login workflows.

## Architecture Notes
- **Singleton Pattern**: Most services use ISingletonDependency for application-wide state
- **Fluent API**: Leverages Flurl's fluent interface for HTTP operations
- **ABP Integration**: Fully integrated with ASP.NET Boilerplate framework conventions
- **Token Management**: Implements OAuth 2.0 bearer token authentication
- **Response Wrapping**: Handles ABP's AjaxResponse wrapper transparently
- **Error Handling**: Converts API errors to UserFriendlyException

## Business Logic
- **Authentication Flow**: 
  1. User provides credentials
  2. AccessTokenManager sends login request
  3. Receives access and refresh tokens
  4. Stores tokens for subsequent API calls
  5. Automatically refreshes expired tokens
- **Multi-tenancy**: Tenant context is resolved and included in all API requests
- **Localization**: Current culture is passed via headers for server-side localization

## Usage Across Codebase
- Used by all proxy service classes for API communication
- Referenced by authentication workflows in client applications
- Provides foundation for mobile and desktop client apps
- Consumed by ProxyAppServiceBase and ProxyControllerBase

## Cross-References
- **Consumers**: All proxy services in Authorization, Common, MultiTenancy, Sessions folders
- **Dependencies**: Requires inzibackend.Application.Shared for DTOs
- **Configuration**: Relies on ApiUrlConfig for endpoint configuration