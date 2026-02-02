# JWT Bearer Authentication Documentation

## Overview
This folder contains the JWT (JSON Web Token) bearer authentication implementation for the application. It provides custom JWT validation, security stamp handling, and asynchronous token processing capabilities for secure API authentication.

## Contents

### Files

#### TokenAuthConfiguration.cs
- **Purpose**: Configuration model for JWT authentication settings
- **Key Properties**:
  - `SecurityKey`: Symmetric security key for token signing
  - `Issuer`: Token issuer identifier
  - `Audience`: Token audience identifier
  - `SigningCredentials`: Credentials for token signing
  - `AccessTokenExpiration`: Access token lifetime
  - `RefreshTokenExpiration`: Refresh token lifetime

#### TokenTypes.cs
- **Purpose**: Defines different JWT token types
- **Token Types**:
  - Access tokens for API authorization
  - Refresh tokens for token renewal
  - Custom token types for specific scenarios

#### IAsyncSecurityTokenValidator.cs
- **Purpose**: Interface for asynchronous JWT token validation
- **Key Features**:
  - Async validation support for improved performance
  - Custom validation logic integration
  - Claims principal extraction

#### AsyncJwtBearerOptions.cs
- **Purpose**: Configuration options for async JWT bearer authentication
- **Features**:
  - Extends standard JWT bearer options
  - Supports async token validation
  - Custom security token validators collection

#### IJwtSecurityStampHandler.cs
- **Purpose**: Interface for managing JWT security stamps
- **Key Methods**:
  - `Validate`: Validates security stamp in claims principal
  - `SetSecurityStampCacheItem`: Caches security stamp for user
  - `RemoveSecurityStampCacheItem`: Removes cached security stamp
- **Security**: Prevents token reuse after password changes

#### JwtSecurityStampHandler.cs
- **Purpose**: Implementation of security stamp validation
- **Key Features**:
  - Cache-based security stamp storage
  - Multi-tenant support with tenant isolation
  - Automatic cache invalidation on security changes
- **Business Logic**: Ensures tokens become invalid when user security changes

#### inzibackendAsyncJwtSecurityTokenHandler.cs
- **Purpose**: Custom JWT token handler with async validation
- **Key Features**:
  - Extends Microsoft's JwtSecurityTokenHandler
  - Async token validation pipeline
  - Security stamp integration
  - Custom claims transformation

#### inzibackendAsyncJwtBearerHandler.cs
- **Purpose**: Custom JWT bearer authentication handler
- **Features**:
  - Handles JWT bearer authentication challenges
  - Integrates with ASP.NET Core authentication pipeline
  - Custom error handling for token validation failures
  - Support for token refresh flows

#### inzibackendJwtBearerExtensions.cs
- **Purpose**: Extension methods for JWT bearer configuration
- **Key Methods**:
  - Service registration extensions
  - Authentication scheme configuration
  - Middleware pipeline setup

#### LoggingExtensions.cs
- **Purpose**: Logging extensions for JWT authentication events
- **Features**:
  - Structured logging for authentication events
  - Debug information for token validation
  - Security event auditing

### Key Components
- **TokenAuthConfiguration**: Central JWT configuration model
- **JwtSecurityStampHandler**: Security stamp validation logic
- **AsyncJwtSecurityTokenHandler**: Custom async token handler
- **AsyncJwtBearerHandler**: Authentication handler implementation

### Dependencies
- Microsoft.IdentityModel.Tokens (JWT token handling)
- System.IdentityModel.Tokens.Jwt (JWT specific implementations)
- Microsoft.AspNetCore.Authentication.JwtBearer (ASP.NET Core JWT integration)
- Abp.Runtime.Caching (for security stamp caching)

## Architecture Notes
- Implements async/await pattern throughout for scalability
- Uses caching for performance optimization of security stamp checks
- Custom handlers allow for application-specific validation logic
- Modular design allows easy extension and customization

## Business Logic
- Security stamps prevent token reuse after password/security changes
- Multi-tenant isolation ensures tokens are tenant-specific
- Token expiration configurable per deployment environment
- Refresh tokens enable seamless user experience

## Usage Across Codebase
This JWT authentication system is used by:
- API controllers requiring [Authorize] attribute
- TokenAuthController for authentication endpoints
- Mobile and SPA applications for API access
- External service integrations
- Background job authentication