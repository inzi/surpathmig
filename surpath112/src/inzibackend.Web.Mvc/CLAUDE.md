# inzibackend.Web.Mvc Documentation

## Overview
The inzibackend.Web.Mvc project is the main MVC web application that serves as the primary user interface for the student compliance tracking system. Built on ASP.NET Core 6.0 with ABP Framework and ASP.NET Zero, it provides a multi-tenant SaaS application for schools to track student compliance with policies including drug testing, documentation, background checks, and more. This project handles both public-facing pages (authentication, registration) and the authenticated application interface.

## Project Structure

### Core Folders

#### Areas/
- **Purpose**: Contains MVC areas for organizing related functionality
- **Main Area**: App - authenticated application interface
- **Features**: Controllers, Models, Views organized by area
- **See**: [Areas/CLAUDE.md](Areas/CLAUDE.md)

#### Controllers/
- **Purpose**: Root-level controllers for public and auth functionality
- **Key Controllers**:
  - AccountController: Authentication and account management
  - TenantRegistrationController: New tenant onboarding
  - PaymentController: Payment processing
  - HomeController: Application entry point
- **See**: [Controllers/CLAUDE.md](Controllers/CLAUDE.md)

#### Views/
- **Purpose**: Root-level Razor views
- **Contents**:
  - Account views (login, register, password reset)
  - TenantRegistration views
  - Shared layouts and components
  - Error pages
- **Organization**: Follows MVC convention (folder per controller)

#### Models/
- **Purpose**: Root-level view models
- **Contents**:
  - Account models
  - TenantRegistration models
  - Error models
  - Common models
- **Pattern**: View-specific data transfer objects

#### wwwroot/
- **Purpose**: Static files and client-side resources
- **Contents**:
  - JavaScript/TypeScript files
  - CSS stylesheets
  - Images and fonts
  - Third-party libraries
- **See**: [wwwroot/CLAUDE.md](wwwroot/CLAUDE.md)

#### Startup/
- **Purpose**: Application configuration and initialization
- **Key Files**:
  - `Program.cs`: Application entry point
  - `Startup.cs`: Service configuration and middleware
  - Navigation providers
  - Authentication configuration

#### Extensions/
- **Purpose**: Extension methods and helpers
- **Contents**:
  - HTML helpers
  - Tag helpers
  - Utility extensions
- **Usage**: Enhance Razor views and controllers

### Configuration Files

#### appsettings.json
- **Purpose**: Application configuration
- **Sections**:
  - Connection strings
  - Authentication settings
  - Logging configuration
  - API keys and secrets
- **Environment-specific**: appsettings.{Environment}.json

#### bundles.json
- **Purpose**: JavaScript and CSS bundling configuration
- **Features**:
  - Input file definitions
  - Output bundle specifications
  - Minification settings
- **Processing**: Used by gulp build tasks

#### gulpfile.js
- **Purpose**: Build automation for client-side assets
- **Tasks**:
  - JavaScript minification and bundling
  - CSS compilation and minification
  - File copying and optimization
- **Commands**: `gulp buildDev`, `gulp build`

#### package.json
- **Purpose**: Node.js package management
- **Contents**:
  - Client-side library dependencies
  - Development tool dependencies
  - Build scripts
- **Management**: npm or yarn

#### log4net.config
- **Purpose**: Logging configuration
- **Features**:
  - Log levels
  - Appenders (file, console)
  - Log formatting
- **Environment-specific**: log4net.{Environment}.config

### Project File

#### inzibackend.Web.Mvc.csproj
- **Target Framework**: .NET 6.0
- **Project Type**: ASP.NET Core Web Application
- **Key Dependencies**:
  - ABP Framework packages
  - ASP.NET Zero packages
  - Entity Framework Core
  - SignalR
  - IdentityServer4
  - Hangfire

## Architecture

### Technology Stack
- **Framework**: ASP.NET Core 6.0
- **Base Framework**: ABP (ASP.NET Boilerplate)
- **Template**: ASP.NET Zero
- **ORM**: Entity Framework Core
- **UI Theme**: Metronic
- **Real-time**: SignalR
- **Background Jobs**: Hangfire
- **Authentication**: IdentityServer4

### Design Patterns
- **MVC Pattern**: Model-View-Controller architecture
- **Repository Pattern**: Data access abstraction
- **Unit of Work**: Transaction management
- **Dependency Injection**: IoC container (Castle Windsor)
- **Domain Driven Design**: Through ABP framework

### Multi-Tenancy
- **Tenant Isolation**: Data separation per tenant
- **Host Features**: System-wide administration
- **Shared Database**: With discriminator column
- **Feature Management**: Per-tenant feature toggles

## Key Features

### Authentication & Authorization
- Local authentication (username/password)
- External providers (Google, Facebook, etc.)
- Two-factor authentication
- Role-based permissions
- JWT token support
- Single Sign-On via IdentityServer4

### Compliance Management
- Document upload and tracking
- Requirement management
- Status monitoring
- Deadline tracking
- Multi-level approvals
- Audit trails

### User Management
- User CRUD operations
- Role assignment
- Department associations
- Cohort memberships
- Profile management
- Impersonation

### Tenant Management
- Tenant registration
- Edition/subscription management
- Feature configuration
- Billing and payments
- Usage tracking

### Dashboard & Reporting
- Customizable dashboards
- Real-time metrics
- Compliance statistics
- Widget framework
- Export capabilities

## Dependencies

### Project References
- `inzibackend.Core`: Domain layer
- `inzibackend.Core.Shared`: Shared constants and enums
- `inzibackend.Application`: Application services
- `inzibackend.Application.Shared`: DTOs and interfaces
- `inzibackend.EntityFrameworkCore`: Data access layer
- `inzibackend.Web.Core`: Shared web components

### NuGet Packages
- ABP Framework packages
- ASP.NET Core packages
- Entity Framework Core
- SignalR
- Hangfire
- Stripe.net
- PayPal SDK
- Various utility libraries

### Client-Side Libraries
- jQuery and plugins
- Bootstrap
- DataTables
- Select2
- Moment.js
- SweetAlert2
- Chart.js

## Build & Deployment

### Build Process
1. **Backend**: `dotnet build`
2. **Frontend**: `yarn && gulp buildDev`
3. **Combined**: `build/build-mvc.ps1`

### Configuration
- Environment variables
- User secrets (development)
- Azure Key Vault (production)
- Configuration transformation

### Deployment
- IIS hosting
- Docker support
- Azure App Service
- Health checks
- Application Insights

## Security

### Authentication
- Cookie authentication for MVC
- JWT for API endpoints
- Anti-forgery tokens
- Session management

### Authorization
- Permission-based access
- Feature-based restrictions
- Data filtering by tenant
- Row-level security

### Security Headers
- HTTPS enforcement
- HSTS
- Content Security Policy
- X-Frame-Options
- XSS protection

## Performance

### Caching
- Response caching
- Distributed caching (Redis)
- Query result caching
- Static file caching

### Optimization
- Bundle and minification
- Lazy loading
- Async operations
- Database query optimization

## Monitoring & Logging

### Logging
- Log4Net integration
- Structured logging
- Multiple appenders
- Log levels per category

### Monitoring
- Health checks
- Application Insights
- Performance counters
- Error tracking

## Testing Considerations

### Test Support
- Dependency injection for testability
- Service interfaces
- Repository pattern
- Separate test database

### Test Types
- Unit tests for services
- Integration tests for APIs
- UI tests for critical flows
- Performance tests

## Development Workflow

### Local Development
1. Configure connection strings
2. Run database migrations
3. Build frontend assets: `gulp buildDev`
4. Run application: `dotnet run`
5. Navigate to: https://localhost:44302

### Hot Reload
- Razor file changes
- CSS/JS with gulp watch
- Backend with dotnet watch

## Related Documentation
- [Areas/App/CLAUDE.md](Areas/App/CLAUDE.md): Main application area
- [Controllers/CLAUDE.md](Controllers/CLAUDE.md): Root controllers
- [wwwroot/CLAUDE.md](wwwroot/CLAUDE.md): Static resources
- Project root [CLAUDE.md](../../CLAUDE.md): Overall guidelines