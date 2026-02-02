# inzibackend.Web.Host Documentation

## Overview
The Web.Host project is the Web API host application that serves as the primary backend for the inzibackend system. It provides RESTful API endpoints, handles authentication/authorization, manages background jobs, and serves minimal UI for authentication scenarios. This project implements a multi-tenant SaaS architecture for school compliance tracking (drug testing, documentation, background checks).

## Contents

### Files

#### inzibackend.Web.Host.csproj
- **Framework**: .NET 6.0
- **Type**: Web application (Exe output)
- **Key Dependencies**:
  - inzibackend.Web.Core (project reference)
  - Hangfire.MySqlStorage (background jobs)
  - Abp.Castle.Log4Net (logging)
  - AspNetCore.HealthChecks.UI (health monitoring)
  - Microsoft.EntityFrameworkCore.Design (EF migrations)
- **Features**:
  - User secrets support
  - Docker support (Dockerfile included)
  - Embedded Swagger UI customization
  - Auto-generated binding redirects

#### appsettings.json
- **Connection Strings**:
  - Default: SQL Server for main database
  - Hangfire: MySQL for background job storage
  - surpathlive: MySQL for legacy/additional data
- **Application Settings**:
  - Server URL: https://localhost:44301/
  - Client URL: http://localhost:4200/
  - CORS origins configured for development
  - Swagger endpoint configuration
- **Authentication**:
  - JWT Bearer enabled with symmetric key
  - Social login providers configured (disabled by default)
  - IdentityServer configuration (disabled)
- **Services**:
  - Twilio for SMS (unconfigured)
  - reCAPTCHA v3 for bot protection
  - PayPal and Stripe payment integration
- **Features**:
  - Health checks (disabled by default)
  - Redis cache configuration
  - Audit log management

#### log4net.config / log4net.Production.config
- Logging configuration for development and production
- File-based and console logging
- Different log levels per environment

#### app.config
- .NET Framework compatibility settings
- Binding redirects for assemblies

### Key Components

#### API Architecture
- RESTful API design
- JWT Bearer authentication
- Multi-tenant support
- CORS enabled for SPA clients

#### Background Processing
- Hangfire for job scheduling
- MySQL storage for job persistence
- Workers for compliance expiration, subscriptions, etc.

#### Real-time Communication
- SignalR hubs for chat and notifications
- Encrypted token authentication for SignalR

#### Payment Processing
- Stripe webhook integration
- PayPal support
- Authorize.Net alternative processor

### Dependencies

#### Core Framework
- ASP.NET Core 6.0
- ABP Framework 7.3
- Entity Framework Core

#### Authentication & Security
- IdentityServer4 (optional)
- JWT Bearer tokens
- OAuth/OpenID Connect providers

#### Infrastructure
- Hangfire (background jobs)
- SignalR (real-time)
- Swagger/OpenAPI (documentation)
- Health Checks UI

## Subfolders

### Controllers
[See Controllers/CLAUDE.md]
- API endpoints for authentication, payments, profiles
- Webhook handlers for payment providers
- Minimal UI controllers for IdentityServer

### Models
[See Models/CLAUDE.md]
- View models for authentication UI
- DTOs for minimal web interface

### Properties
[See Properties/CLAUDE.md]
- Launch profiles for different environments
- Development configuration

### Startup
[See Startup/CLAUDE.md]
- Application initialization and configuration
- Service registration
- Middleware pipeline setup
- External authentication provider configuration

### Url
[See Url/CLAUDE.md]
- URL generation services
- Multi-tenant URL construction
- Email link generation

### Views
[See Views/CLAUDE.md]
- Razor views for authentication UI
- Error pages
- OAuth consent screens

### wwwroot
[See wwwroot/CLAUDE.md]
- Static files (images, CSS, JavaScript)
- Swagger UI customizations
- Sample profile pictures

## Architecture Notes

### Hosting Model
- Self-hosted Kestrel server
- IIS Integration support
- Docker containerization ready
- HTTPS enforced

### Multi-Tenancy Design
- Tenant identification via subdomain or header
- Per-tenant configuration
- Tenant-isolated data access
- Host and tenant separation

### API Design Patterns
- Controller-based routing
- Dependency injection throughout
- Base controller inheritance
- Service layer abstraction

### Security Architecture
- JWT tokens for API authentication
- Anti-forgery tokens for forms
- CORS policy enforcement
- HTTPS-only communication
- Webhook signature validation

## Business Logic

### Core Functionality
1. **Student Compliance Tracking**:
   - API endpoints for compliance management
   - Background jobs for expiration monitoring
   - Real-time notifications via SignalR

2. **Multi-School SaaS**:
   - Tenant isolation per school
   - School-specific configurations
   - Subscription management

3. **Authentication & Authorization**:
   - JWT-based API authentication
   - Role-based access control
   - External login provider support

4. **Payment Processing**:
   - Subscription billing via Stripe
   - Payment webhook handling
   - Multiple payment provider support

### Background Jobs
- Compliance expiration checks
- Subscription expiration monitoring
- Email notifications
- Audit log cleanup
- Password expiration checks

### Integration Points
- External payment providers (Stripe, PayPal)
- SMS services (Twilio)
- OAuth providers (Google, Microsoft, etc.)
- Client applications (Angular/React)

## Usage Across Codebase

### API Consumers
- Angular SPA (primary client)
- Mobile applications
- Third-party integrations
- Admin tools

### Cross-Reference Analysis

#### Critical Endpoints
- `/api/TokenAuth/Authenticate` - Authentication
- `/api/services/app/*` - Application services
- `/signalr` - Real-time communication
- `/swagger` - API documentation

#### Shared Dependencies
- Web.Core project for shared logic
- Application project for business services
- EntityFrameworkCore for data access
- Core for domain entities

#### Configuration Impact
- appsettings.json affects entire application
- Environment-specific settings override base
- Tenant-specific settings in database

## Deployment Considerations

### Environment Configuration
- Development: Detailed errors, Swagger enabled
- Production: Optimized logging, security headers
- Docker: Container-ready configuration

### Performance Optimization
- Response caching where appropriate
- Background job optimization
- Database connection pooling
- Static file caching

### Monitoring & Health
- Health check endpoints
- Structured logging
- Performance metrics
- Error tracking

## Security Checklist
- [ ] JWT secret key rotation
- [ ] HTTPS enforcement
- [ ] CORS policy review
- [ ] Input validation
- [ ] SQL injection prevention
- [ ] XSS protection
- [ ] CSRF token validation
- [ ] Rate limiting
- [ ] Audit logging