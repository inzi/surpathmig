# Common Documentation

## Overview
This folder contains web-specific constants and configuration values used throughout the web layer. It centralizes endpoint URLs, feature flags, and whitelists for various web infrastructure components.

## Contents

### Files

#### WebConsts.cs
- **Purpose**: Static constants and configuration for web infrastructure features
- **Key Constants**:
  - **SwaggerUiEndPoint**: `/swagger` - Swagger UI URL
  - **HangfireDashboardEndPoint**: `/hangfire` - Hangfire dashboard URL
  - **SwaggerUiEnabled**: true - Enable/disable Swagger UI
  - **HangfireDashboardEnabled**: false - Enable/disable Hangfire dashboard
- **GraphQL Configuration**:
  - **PlaygroundEndPoint**: `/ui/playground` - GraphQL playground URL
  - **EndPoint**: `/graphql` - GraphQL API endpoint
  - **PlaygroundEnabled**: false - GraphQL playground disabled by default
  - **Enabled**: false - GraphQL API disabled by default
- **Security Whitelists**:
  - **ReCaptchaIgnoreWhiteList**: User agents that bypass reCAPTCHA (includes API client)
- **Usage**: Referenced during startup configuration and middleware setup

### Key Components
- **Endpoint Configuration**: Centralized URL paths for web tools
- **Feature Flags**: Enable/disable features without code changes
- **Security Whitelist**: Bypass CAPTCHA for trusted clients

### Dependencies
- **inzibackendConsts**: Core constants (AbpApiClientUserAgent)

## Architecture Notes

### Design Pattern
- Static class with const and static fields
- Allows runtime modification of feature flags
- Const for URLs (compile-time constants)
- Static fields for boolean flags (runtime toggleable)

### Feature Flag Usage
Features can be toggled at runtime by setting static properties:
```csharp
WebConsts.SwaggerUiEnabled = false; // Disable Swagger
WebConsts.HangfireDashboardEnabled = true; // Enable Hangfire
```

### Security Considerations
- reCAPTCHA whitelist allows API clients to bypass CAPTCHA
- GraphQL disabled by default (security-first approach)
- Hangfire dashboard disabled by default (administrative tool)

## Business Logic

### Swagger UI
- Enabled by default for development convenience
- Should be disabled in production or secured with authentication
- Provides interactive API documentation and testing

### Hangfire Dashboard
- Disabled by default (administrative access required)
- Provides UI for background job monitoring
- Should be secured with authorization in production

### GraphQL
- Disabled by default
- Optional feature for GraphQL API clients
- Playground provides interactive query builder

### reCAPTCHA Whitelist
- Allows trusted user agents to bypass CAPTCHA challenges
- API client included by default
- Prevents CAPTCHA blocking automated API usage

## Usage Across Codebase

### Consumed By
- **Startup.cs**: Feature flag checks during application configuration
- **Middleware Configuration**: Conditional middleware registration
- **Controllers**: reCAPTCHA whitelist validation
- **Security Filters**: CAPTCHA bypass logic

### Configuration Flow
1. Application startup reads WebConsts
2. Conditionally registers middleware based on flags
3. Controllers/filters check whitelist during request processing
4. Endpoints mapped based on configuration

## Performance Considerations
- Static constants compiled directly into consuming code
- Static fields have negligible performance impact
- No database or configuration file reads required

## Deployment Notes
- Feature flags can be toggled via code or environment configuration
- Consider using appsettings.json for environment-specific overrides
- Ensure Swagger disabled in production builds
- Secure Hangfire dashboard with authentication if enabled