# CLAUDE.md - Coding Guidelines and Commands

## Conventions Folder

**IMPORTANT**: The `conventions/` folder contains established patterns and best practices for this codebase.

- **Before planning any implementation**: Review relevant convention files in `conventions/`
- **When learning new patterns**: Use the `/capture-convention` skill to document them
- **Location**: `I:\surpath150\conventions\`
- **Purpose**: Single source of truth for architectural patterns, coding standards, and ASP.NET Zero MVC + jQuery best practices
- **Note**: These conventions are specific to ASP.NET Zero MVC + jQuery (multi-page application). ASP.NET Zero also supports Angular (SPA), which may have different patterns.

## Build Commands
- .NET: `dotnet restore && dotnet build`
- Web Frontend: `cd src/inzibackend.Web.Mvc && yarn && gulp buildDev`
- Build & Publish: `build/build-mvc.ps1`
- **Project to Build**: `inzibackend.Web.Mvc`

## Test Commands
- Run all tests: `dotnet test`
- Run specific test: `dotnet test --filter "FullyQualifiedName=inzibackend.Tests.YourTestName"`
- Test project: `test/inzibackend.Tests/inzibackend.Tests.csproj`

## Code Style Guidelines
- **Never modify** files in ABP7.3 or ANZ11.4 folders
- When editing an entity, update the corresponding DTO as needed
- JavaScript is bundled dynamically via `gulpfile.js` and `bundles.json`
- C# Style: Follow standard C# conventions with proper XML documentation
- Database: Refer to `sql/surpathv2schema.sql` for database structure

## EntityFramework
- Domain entities in `inzibackend.Core`
- DTOs in `inzibackend.Application.Shared`
- Services in `inzibackend.Application`
- Database context in `inzibackend.EntityFrameworkCore`

## Important notes about the environment
- You run in an environment where `ast-grep` is available; whenever a search requires syntax-aware or structural matching, default to `ast-grep --lang rust -p '<pattern>'` (or set `--lang` appropriately) and avoid falling back to text-only tools like `rg` or `grep` unless I explicitly request a plain-text search.
- The project to build is `inzibackend.Web.Mvc`

## Domain Concepts
- Remember that these are really targeting cohortuserids. A cohortuser is also a user, but a user is not necessarily a cohortuser
- When checking for compliance, it finds the cohortuser by the userid in some cases, so they are used both ways

## Documentation Structure

The following folders have been documented with CLAUDE.md files:

### Domain Layer (Core Business Logic)
- [src/inzibackend.Core](src/inzibackend.Core/CLAUDE.md) - Core domain layer with entities, domain services, and business rules
  - [src/inzibackend.Core/Surpath](src/inzibackend.Core/Surpath/CLAUDE.md) - Compliance management domain (56 entities)
    - [src/inzibackend.Core/Surpath/AuthNet](src/inzibackend.Core/Surpath/AuthNet/CLAUDE.md) - Authorize.Net payment integration
    - [src/inzibackend.Core/Surpath/Compliance](src/inzibackend.Core/Surpath/Compliance/CLAUDE.md) - Compliance evaluation and requirement resolution
    - [src/inzibackend.Core/Surpath/Helpers](src/inzibackend.Core/Surpath/Helpers/CLAUDE.md) - Security utilities (HTML sanitization)
    - [src/inzibackend.Core/Surpath/LegalDocuments](src/inzibackend.Core/Surpath/LegalDocuments/CLAUDE.md) - Legal document management
    - [src/inzibackend.Core/Surpath/MetaData](src/inzibackend.Core/Surpath/MetaData/CLAUDE.md) - Notification metadata tracking
    - [src/inzibackend.Core/Surpath/OUManager](src/inzibackend.Core/Surpath/OUManager/CLAUDE.md) - Organization unit security management
    - [src/inzibackend.Core/Surpath/OUs](src/inzibackend.Core/Surpath/OUs/CLAUDE.md) - Organization unit relationships
    - [src/inzibackend.Core/Surpath/Statics](src/inzibackend.Core/Surpath/Statics/CLAUDE.md) - ID generation and configuration
    - [src/inzibackend.Core/Surpath/SurpathPayManager](src/inzibackend.Core/Surpath/SurpathPayManager/CLAUDE.md) - Payment processing services
    - [src/inzibackend.Core/Surpath/Workers](src/inzibackend.Core/Surpath/Workers/CLAUDE.md) - Background maintenance workers
  - [src/inzibackend.Core/Authorization](src/inzibackend.Core/Authorization/CLAUDE.md) - Authorization system with roles and permissions
    - [src/inzibackend.Core/Authorization/Users](src/inzibackend.Core/Authorization/Users/CLAUDE.md) - User management domain
      - [src/inzibackend.Core/Authorization/Users/Password](src/inzibackend.Core/Authorization/Users/Password/CLAUDE.md) - Password expiration services
      - [src/inzibackend.Core/Authorization/Users/Profile](src/inzibackend.Core/Authorization/Users/Profile/CLAUDE.md) - Profile image services
    - [src/inzibackend.Core/Authorization/Delegation](src/inzibackend.Core/Authorization/Delegation/CLAUDE.md) - User delegation system
    - [src/inzibackend.Core/Authorization/Impersonation](src/inzibackend.Core/Authorization/Impersonation/CLAUDE.md) - User impersonation for support
  - [src/inzibackend.Core/Authentication/TwoFactor/Google](src/inzibackend.Core/Authentication/TwoFactor/Google/CLAUDE.md) - Google Authenticator 2FA
  - [src/inzibackend.Core/MultiTenancy](src/inzibackend.Core/MultiTenancy/CLAUDE.md) - Multi-tenant architecture and subscription management

### Application Shared Layer (Service Contracts & DTOs)
- [src/inzibackend.Application.Shared](src/inzibackend.Application.Shared/CLAUDE.md) - Service interfaces and DTOs defining the API surface
  - [src/inzibackend.Application.Shared/Surpath](src/inzibackend.Application.Shared/Surpath/CLAUDE.md) - Core business domain contracts for compliance tracking
    - [src/inzibackend.Application.Shared/Surpath/ComplianceDTOs](src/inzibackend.Application.Shared/Surpath/ComplianceDTOs/CLAUDE.md) - Compliance-specific DTOs
    - [src/inzibackend.Application.Shared/Surpath/Dtos](src/inzibackend.Application.Shared/Surpath/Dtos/CLAUDE.md) - Comprehensive business entity DTOs (268+ files)
  - [src/inzibackend.Application.Shared/Authorization](src/inzibackend.Application.Shared/Authorization/CLAUDE.md) - Authorization and account management contracts
    - [src/inzibackend.Application.Shared/Authorization/Accounts](src/inzibackend.Application.Shared/Authorization/Accounts/CLAUDE.md) - Account service interfaces
      - [src/inzibackend.Application.Shared/Authorization/Accounts/Dto](src/inzibackend.Application.Shared/Authorization/Accounts/Dto/CLAUDE.md) - Account management DTOs
  - [src/inzibackend.Application.Shared/MultiTenancy](src/inzibackend.Application.Shared/MultiTenancy/CLAUDE.md) - Multi-tenant SaaS platform contracts
  - [src/inzibackend.Application.Shared/Sessions](src/inzibackend.Application.Shared/Sessions/CLAUDE.md) - Session management contracts

### Application Layer
- [src/inzibackend.Application](src/inzibackend.Application/CLAUDE.md) - Core application services and business logic
  - [src/inzibackend.Application/Surpath](src/inzibackend.Application/Surpath/CLAUDE.md) - Compliance tracking and management services
    - [src/inzibackend.Application/Surpath/ComplianceManager](src/inzibackend.Application/Surpath/ComplianceManager/CLAUDE.md) - Core compliance evaluation engine
  - [src/inzibackend.Application/Authorization/Users/Profile](src/inzibackend.Application/Authorization/Users/Profile/CLAUDE.md) - User profile management
    - [src/inzibackend.Application/Authorization/Users/Profile/Cache](src/inzibackend.Application/Authorization/Users/Profile/Cache/CLAUDE.md) - SMS verification caching
  - [src/inzibackend.Application/Authorization/Users/Importing](src/inzibackend.Application/Authorization/Users/Importing/CLAUDE.md) - Bulk user import infrastructure
    - [src/inzibackend.Application/Authorization/Users/Importing/Dto](src/inzibackend.Application/Authorization/Users/Importing/Dto/CLAUDE.md) - Import DTOs
  - [src/inzibackend.Application/Common](src/inzibackend.Application/Common/CLAUDE.md) - Common services and utilities

### Client Application Layer
- [src/inzibackend.Application.Client](src/inzibackend.Application.Client/CLAUDE.md) - Client-side proxy services for API access
  - [src/inzibackend.Application.Client/ApiClient](src/inzibackend.Application.Client/ApiClient/CLAUDE.md) - Core HTTP client infrastructure
    - [src/inzibackend.Application.Client/ApiClient/Models](src/inzibackend.Application.Client/ApiClient/Models/CLAUDE.md) - Authentication models
  - [src/inzibackend.Application.Client/Authorization](src/inzibackend.Application.Client/Authorization/CLAUDE.md) - Authorization proxy services
    - [src/inzibackend.Application.Client/Authorization/Accounts](src/inzibackend.Application.Client/Authorization/Accounts/CLAUDE.md) - Account management proxies
    - [src/inzibackend.Application.Client/Authorization/Users](src/inzibackend.Application.Client/Authorization/Users/CLAUDE.md) - User management proxies
      - [src/inzibackend.Application.Client/Authorization/Users/Profile](src/inzibackend.Application.Client/Authorization/Users/Profile/CLAUDE.md) - Profile management proxies
  - [src/inzibackend.Application.Client/Common](src/inzibackend.Application.Client/Common/CLAUDE.md) - Common lookup proxies
  - [src/inzibackend.Application.Client/Configuration](src/inzibackend.Application.Client/Configuration/CLAUDE.md) - Configuration and session management
  - [src/inzibackend.Application.Client/Extensions](src/inzibackend.Application.Client/Extensions/CLAUDE.md) - Helper extensions
  - [src/inzibackend.Application.Client/MultiTenancy](src/inzibackend.Application.Client/MultiTenancy/CLAUDE.md) - Tenant management proxies
  - [src/inzibackend.Application.Client/Sessions](src/inzibackend.Application.Client/Sessions/CLAUDE.md) - Session management proxies

### Core Shared Layer
- [src/inzibackend.Core.Shared](src/inzibackend.Core.Shared/CLAUDE.md) - Shared constants, enums, and helper classes for cross-cutting concerns
  - [src/inzibackend.Core.Shared/Authentication](src/inzibackend.Core.Shared/Authentication/CLAUDE.md) - External authentication provider configurations
  - [src/inzibackend.Core.Shared/Authorization](src/inzibackend.Core.Shared/Authorization/CLAUDE.md) - Authorization constants and role definitions
    - [src/inzibackend.Core.Shared/Authorization/Roles](src/inzibackend.Core.Shared/Authorization/Roles/CLAUDE.md) - Static role names
    - [src/inzibackend.Core.Shared/Authorization/Users](src/inzibackend.Core.Shared/Authorization/Users/CLAUDE.md) - User validation constants
  - [src/inzibackend.Core.Shared/Chat](src/inzibackend.Core.Shared/Chat/CLAUDE.md) - Chat messaging enumerations
  - [src/inzibackend.Core.Shared/Editions](src/inzibackend.Core.Shared/Editions/CLAUDE.md) - Edition payment types
  - [src/inzibackend.Core.Shared/Friendships](src/inzibackend.Core.Shared/Friendships/CLAUDE.md) - User relationship states
  - [src/inzibackend.Core.Shared/MultiTenancy](src/inzibackend.Core.Shared/MultiTenancy/CLAUDE.md) - Multi-tenancy infrastructure
    - [src/inzibackend.Core.Shared/MultiTenancy/Payments](src/inzibackend.Core.Shared/MultiTenancy/Payments/CLAUDE.md) - Payment gateway models
  - [src/inzibackend.Core.Shared/Notifications](src/inzibackend.Core.Shared/Notifications/CLAUDE.md) - Notification name constants
  - [src/inzibackend.Core.Shared/Security](src/inzibackend.Core.Shared/Security/CLAUDE.md) - Security configuration models
  - [src/inzibackend.Core.Shared/Storage](src/inzibackend.Core.Shared/Storage/CLAUDE.md) - Binary storage constants
  - [src/inzibackend.Core.Shared/Surpath](src/inzibackend.Core.Shared/Surpath/CLAUDE.md) - Core business domain constants and models
    - [src/inzibackend.Core.Shared/Surpath/AuthNet](src/inzibackend.Core.Shared/Surpath/AuthNet/CLAUDE.md) - Authorize.Net payment models
    - [src/inzibackend.Core.Shared/Surpath/Compliance](src/inzibackend.Core.Shared/Surpath/Compliance/CLAUDE.md) - Compliance tracking models
    - [src/inzibackend.Core.Shared/Surpath/Extensions](src/inzibackend.Core.Shared/Surpath/Extensions/CLAUDE.md) - Utility extensions and settings
    - [src/inzibackend.Core.Shared/Surpath/LegalDocuments](src/inzibackend.Core.Shared/Surpath/LegalDocuments/CLAUDE.md) - Legal document DTOs
    - [src/inzibackend.Core.Shared/Surpath/ParserClasses](src/inzibackend.Core.Shared/Surpath/ParserClasses/CLAUDE.md) - Lab report parsing utilities
    - [src/inzibackend.Core.Shared/Surpath/Purchase](src/inzibackend.Core.Shared/Surpath/Purchase/CLAUDE.md) - Purchase service interfaces
  - [src/inzibackend.Core.Shared/Validation](src/inzibackend.Core.Shared/Validation/CLAUDE.md) - Common validation utilities
  - [src/inzibackend.Core.Shared/Webhooks](src/inzibackend.Core.Shared/Webhooks/CLAUDE.md) - Webhook event definitions

### Console Applications
- [src/ConsoleClient](src/ConsoleClient/CLAUDE.md) - Standalone console application for batch processing lab reports
  - [src/ConsoleClient/DependencyInjection](src/ConsoleClient/DependencyInjection/CLAUDE.md) - Service registration and dummy implementations

### Data Access Layer (Entity Framework Core)
- [src/inzibackend.EntityFrameworkCore](src/inzibackend.EntityFrameworkCore/CLAUDE.md) - Complete EF Core data access layer with MySQL support
  - [src/inzibackend.EntityFrameworkCore/EntityFrameworkCore](src/inzibackend.EntityFrameworkCore/EntityFrameworkCore/CLAUDE.md) - Core DbContext and configuration
    - [src/inzibackend.EntityFrameworkCore/EntityFrameworkCore/MyExtensions](src/inzibackend.EntityFrameworkCore/EntityFrameworkCore/MyExtensions/CLAUDE.md) - Custom EF extensions (inactive)
    - [src/inzibackend.EntityFrameworkCore/EntityFrameworkCore/Repositories](src/inzibackend.EntityFrameworkCore/EntityFrameworkCore/Repositories/CLAUDE.md) - Base repository implementations
  - [src/inzibackend.EntityFrameworkCore/Migrations](src/inzibackend.EntityFrameworkCore/Migrations/CLAUDE.md) - Database migrations and schema evolution
    - [src/inzibackend.EntityFrameworkCore/Migrations/Seed](src/inzibackend.EntityFrameworkCore/Migrations/Seed/CLAUDE.md) - Database seeding infrastructure
      - [src/inzibackend.EntityFrameworkCore/Migrations/Seed/Host](src/inzibackend.EntityFrameworkCore/Migrations/Seed/Host/CLAUDE.md) - Host database initialization
      - [src/inzibackend.EntityFrameworkCore/Migrations/Seed/Tenants](src/inzibackend.EntityFrameworkCore/Migrations/Seed/Tenants/CLAUDE.md) - Tenant setup and configuration
      - [src/inzibackend.EntityFrameworkCore/Migrations/Seed/Surpath](src/inzibackend.EntityFrameworkCore/Migrations/Seed/Surpath/CLAUDE.md) - Domain-specific reference data
      - [src/inzibackend.EntityFrameworkCore/Migrations/Seed/Importing](src/inzibackend.EntityFrameworkCore/Migrations/Seed/Importing/CLAUDE.md) - Legacy data migration system
  - [src/inzibackend.EntityFrameworkCore/Authorization](src/inzibackend.EntityFrameworkCore/Authorization/CLAUDE.md) - Authorization-related repositories
    - [src/inzibackend.EntityFrameworkCore/Authorization/Users](src/inzibackend.EntityFrameworkCore/Authorization/Users/CLAUDE.md) - User repository with password management
  - [src/inzibackend.EntityFrameworkCore/MultiTenancy](src/inzibackend.EntityFrameworkCore/MultiTenancy/CLAUDE.md) - Multi-tenant data access
    - [src/inzibackend.EntityFrameworkCore/MultiTenancy/Payments](src/inzibackend.EntityFrameworkCore/MultiTenancy/Payments/CLAUDE.md) - Payment repository implementations
  - [src/inzibackend.EntityFrameworkCore/EntityHistory](src/inzibackend.EntityFrameworkCore/EntityHistory/CLAUDE.md) - Entity audit history with PII masking
    - [src/inzibackend.EntityFrameworkCore/EntityHistory/Extensions](src/inzibackend.EntityFrameworkCore/EntityHistory/Extensions/CLAUDE.md) - Change tracking extensions

### Web Layer

#### Web Core (Shared Infrastructure)
- [src/inzibackend.Web.Core](src/inzibackend.Web.Core/CLAUDE.md) - Shared web infrastructure for MVC and API projects
  - [src/inzibackend.Web.Core/Authentication](src/inzibackend.Web.Core/Authentication/CLAUDE.md) - Complete authentication infrastructure
    - [src/inzibackend.Web.Core/Authentication/External](src/inzibackend.Web.Core/Authentication/External/CLAUDE.md) - External authentication providers (OAuth, WS-Federation)
    - [src/inzibackend.Web.Core/Authentication/JwtBearer](src/inzibackend.Web.Core/Authentication/JwtBearer/CLAUDE.md) - JWT token authentication implementation
    - [src/inzibackend.Web.Core/Authentication/TwoFactor](src/inzibackend.Web.Core/Authentication/TwoFactor/CLAUDE.md) - Two-factor authentication caching
  - [src/inzibackend.Web.Core/Controllers](src/inzibackend.Web.Core/Controllers/CLAUDE.md) - Base controllers and authentication endpoints
  - [src/inzibackend.Web.Core/Models](src/inzibackend.Web.Core/Models/CLAUDE.md) - Web layer DTOs and view models
    - [src/inzibackend.Web.Core/Models/TokenAuth](src/inzibackend.Web.Core/Models/TokenAuth/CLAUDE.md) - Authentication request/response models
    - [src/inzibackend.Web.Core/Models/Consent](src/inzibackend.Web.Core/Models/Consent/CLAUDE.md) - OAuth consent flow models
  - [src/inzibackend.Web.Core/Chat](src/inzibackend.Web.Core/Chat/CLAUDE.md) - Real-time chat infrastructure
    - [src/inzibackend.Web.Core/Chat/SignalR](src/inzibackend.Web.Core/Chat/SignalR/CLAUDE.md) - SignalR hub and messaging
  - [src/inzibackend.Web.Core/Configuration](src/inzibackend.Web.Core/Configuration/CLAUDE.md) - Runtime configuration management
  - [src/inzibackend.Web.Core/Security](src/inzibackend.Web.Core/Security/CLAUDE.md) - Security components
    - [src/inzibackend.Web.Core/Security/Recaptcha](src/inzibackend.Web.Core/Security/Recaptcha/CLAUDE.md) - Google reCAPTCHA integration
  - [src/inzibackend.Web.Core/Surpath](src/inzibackend.Web.Core/Surpath/CLAUDE.md) - Enhanced exception logging

#### Web API Host
- [src/inzibackend.Web.Host](src/inzibackend.Web.Host/CLAUDE.md) - Web API host application serving RESTful endpoints
  - [src/inzibackend.Web.Host/Controllers](src/inzibackend.Web.Host/Controllers/CLAUDE.md) - API controllers for authentication, payments, and webhooks
  - [src/inzibackend.Web.Host/Models](src/inzibackend.Web.Host/Models/CLAUDE.md) - View models for minimal authentication UI
  - [src/inzibackend.Web.Host/Properties](src/inzibackend.Web.Host/Properties/CLAUDE.md) - Development launch profiles and configuration
  - [src/inzibackend.Web.Host/Startup](src/inzibackend.Web.Host/Startup/CLAUDE.md) - Application initialization and service configuration
    - [src/inzibackend.Web.Host/Startup/ExternalLoginInfoProviders](src/inzibackend.Web.Host/Startup/ExternalLoginInfoProviders/CLAUDE.md) - Tenant-aware OAuth/SSO providers
  - [src/inzibackend.Web.Host/Url](src/inzibackend.Web.Host/Url/CLAUDE.md) - URL generation services for multi-tenant routing
  - [src/inzibackend.Web.Host/Views](src/inzibackend.Web.Host/Views/CLAUDE.md) - Razor views for authentication and error pages
  - [src/inzibackend.Web.Host/wwwroot](src/inzibackend.Web.Host/wwwroot/CLAUDE.md) - Static files, images, and Swagger customizations

#### Web MVC Application
- [src/inzibackend.Web.Mvc](src/inzibackend.Web.Mvc/CLAUDE.md) - Main MVC web application serving the primary user interface
  - [src/inzibackend.Web.Mvc/Areas](src/inzibackend.Web.Mvc/Areas/CLAUDE.md) - ASP.NET Core Areas for organizing functionality
    - [src/inzibackend.Web.Mvc/Areas/App](src/inzibackend.Web.Mvc/Areas/App/CLAUDE.md) - Authenticated application area
      - [src/inzibackend.Web.Mvc/Areas/App/Controllers](src/inzibackend.Web.Mvc/Areas/App/Controllers/CLAUDE.md) - 72+ controllers for all business operations
      - [src/inzibackend.Web.Mvc/Areas/App/Models](src/inzibackend.Web.Mvc/Areas/App/Models/CLAUDE.md) - View models organized by feature
        - [src/inzibackend.Web.Mvc/Areas/App/Models/Common](src/inzibackend.Web.Mvc/Areas/App/Models/Common/CLAUDE.md) - Shared models and interfaces
          - [src/inzibackend.Web.Mvc/Areas/App/Models/Common/Modals](src/inzibackend.Web.Mvc/Areas/App/Models/Common/Modals/CLAUDE.md) - Modal dialog view models
      - [src/inzibackend.Web.Mvc/Areas/App/Views](src/inzibackend.Web.Mvc/Areas/App/Views/CLAUDE.md) - Razor views for the authenticated UI
  - [src/inzibackend.Web.Mvc/Controllers](src/inzibackend.Web.Mvc/Controllers/CLAUDE.md) - Root-level controllers for public and auth functionality
  - [src/inzibackend.Web.Mvc/wwwroot](src/inzibackend.Web.Mvc/wwwroot/CLAUDE.md) - Static files, JavaScript/TypeScript, CSS, and client libraries

---
*Documentation structure updated on 2025-09-27*

# important-instruction-reminders
Do what has been asked; nothing more, nothing less.
NEVER create files unless they're absolutely necessary for achieving your goal.
ALWAYS prefer editing an existing file to creating a new one.
NEVER proactively create documentation files (*.md) or README files. Only create documentation files if explicitly requested by the User.
- Your credentials for testing are available. Login as "claude@inzi.com" with the password "25.Testing.25"

# CRITICAL IMPORTANT AspNetZero Details
In the ASP.NET Zero MVC + jQuery version (multi-page application):
App services follow the classic ABP pattern and serve as the business logic / use-case layer.
Key points (very concise):

App services live in the *.Application project (implementation) + *.Application.Shared (interfaces + DTOs).
They are not called directly from Razor views or controllers in the traditional sense for AJAX-heavy pages.
Instead: JavaScript (jQuery) calls them via AJAX → using dynamic Web API proxies automatically created by ABP/ASP.NET Zero.
Typical flow:
jQuery code uses abp.services.app.yourServiceName.methodName({ ... }).done(...)
ABP dynamically exposes every app service method as a Web API endpoint (no manual ApiController needed in most cases).
The proxy handles JSON, authorization, validation, localization, auditing, etc. transparently.

Classic MVC controllers (in *.Web.Mvc) are still used for:
Initial page rendering (server-side Razor views)
Non-AJAX actions
Sometimes as thin wrappers that delegate to app services when returning ViewModels


Bottom line
→ Most interactive UI operations (CRUD, modals, datatables, etc.) → jQuery AJAX → dynamic app service API → business logic executes → JSON result back to the page.
This keeps the backend clean, reusable (same services used by Angular versions too), and follows clean architecture separation.