# Application Documentation

## Overview
The Application layer implements the core business logic and application services for the Surpath compliance tracking system. Built on the ABP (ASP.NET Boilerplate) framework, it provides comprehensive services for managing student compliance with school policies including drug testing, documentation requirements, background checks, and medical records. This layer orchestrates between the domain layer (Core) and the presentation layer (Web.Mvc), implementing DTOs, authorization, and business workflows.

## Contents

### Files

#### inzibackendApplicationModule.cs
- **Purpose**: ABP module definition for the application layer
- **Key Features**:
  - Configures dependency injection for application services
  - Registers authorization providers
  - Sets up AutoMapper configuration for DTO mappings
  - Declares dependencies on ApplicationShared and Core modules
- **Module Dependencies**:
  - `inzibackendApplicationSharedModule`: Shared DTOs and interfaces
  - `inzibackendCoreModule`: Domain entities and business rules

#### inzibackendAppServiceBase.cs
- **Purpose**: Base class for all application services providing common functionality
- **Key Features**:
  - User and tenant management helpers
  - Localization support
  - Identity error handling
  - Organizational unit security management
- **Injected Services**:
  - `TenantManager`: Multi-tenancy management
  - `UserManager`: User operations
  - `OUSecurityManager`: Organizational unit security
- **Helper Methods**:
  - `GetCurrentUserAsync()`: Retrieve authenticated user
  - `GetCurrentTenantAsync()`: Get current tenant context
  - `CheckErrors()`: Validate identity operations

#### CustomDtoMapper.cs
- **Purpose**: Custom AutoMapper configuration for entity-to-DTO mappings
- **Key Features**:
  - Configures complex mapping rules
  - Handles nested object mappings
  - Manages collection mappings
  - Custom value resolvers for special cases

### Key Components

**Core Infrastructure:**
- ABP application service base class
- Module initialization and configuration
- DTO mapping infrastructure
- Common service helpers

**Service Categories:**
- Authorization and user management
- Compliance tracking and evaluation
- Document and record management
- Organization and department services
- Financial and ledger services
- Reporting and export services

### Dependencies
- **External:**
  - ABP Framework (complete stack)
  - AutoMapper (object mapping)
  - Entity Framework Core
  - Microsoft.AspNetCore.Identity
  - Castle Windsor (IoC container)

- **Internal:**
  - inzibackend.Core (domain layer)
  - inzibackend.Application.Shared (DTOs)
  - inzibackend.EntityFrameworkCore (data access)

## Subfolders

### Surpath
Core business logic for compliance management including cohorts, requirements, records, drug testing, document management, and department organization. Contains 30+ application services implementing the main business workflows.

### Authorization
User and role management services including:
- **Users**: User CRUD operations, profile management, importing/exporting
- **Roles**: Role management and permissions
- **Accounts**: Account registration and management
- **Permissions**: Dynamic permission system

### MultiTenancy
Multi-tenant architecture support:
- **Tenants**: Tenant management services
- **HostDashboard**: Host-level dashboard data
- **Payments**: Tenant subscription and payment processing
- **Accounting**: Financial tracking per tenant

### Common
Shared utilities and services used across the application (already documented).

### Configuration
Application configuration services:
- **Host**: Host-level settings management
- **Tenants**: Tenant-specific settings

### Auditing
Audit log services and export functionality for compliance tracking and security.

### Chat
Real-time messaging services for user communication within the platform.

### DataExporting
Excel export infrastructure:
- **Excel/EpPlus**: EpPlus-based Excel generation
- **Excel/NPOI**: NPOI-based Excel operations

### DashboardCustomization
Customizable dashboard widgets and layouts per user/tenant.

### DynamicEntityProperties
Dynamic property system for extending entities without code changes.

### Editions
Edition management for feature packaging and tenant subscriptions.

### Friendships
Social features for user connections and relationships.

### Gdpr
GDPR compliance features including data export and deletion.

### HealthChecks
Application health monitoring services.

### Install
Installation and setup services for new deployments.

### Localization
Multi-language support and translation services.

### Logging
Application logging and web log services.

### Notifications
Push notification and alert services.

### Organizations
Organizational unit management and hierarchy.

### Security
Security services including reCAPTCHA integration.

### Sessions
Session management and user session services.

### Timing
Timezone and timing-related services.

### UiCustomization
UI theme and customization services.

### Url
URL generation and management services.

### WebHooks
Webhook management for external integrations.

## Architecture Notes
- **Pattern**: Application Service pattern with DTOs
- **Authorization**: Attribute-based using `[AbpAuthorize]`
- **Validation**: Automatic input validation via ABP
- **Transaction**: Unit of Work pattern for data consistency
- **Caching**: Integrated caching for performance
- **Mapping**: AutoMapper for entity-DTO conversions
- **Localization**: Multi-language support throughout

## Business Logic

### Service Layer Responsibilities
1. **Input Validation**: Validate DTOs before processing
2. **Authorization**: Check permissions for operations
3. **Business Rules**: Implement domain logic
4. **Data Access**: Coordinate repository operations
5. **DTO Mapping**: Convert between entities and DTOs
6. **Transaction Management**: Ensure data consistency
7. **Event Publishing**: Raise domain events
8. **Caching**: Manage cached data

### Key Workflows
- **User Registration**: Account creation with role assignment
- **Compliance Tracking**: Real-time compliance calculation
- **Document Upload**: File processing and storage
- **Report Generation**: Excel export with formatting
- **Notification**: Alert users of compliance changes
- **Audit Logging**: Track all system changes

## Usage Across Codebase

### Primary Consumers
- Web.Mvc controllers
- API endpoints
- Background jobs
- SignalR hubs
- Mobile applications

### Service Integration
- Services often call other services for complex operations
- Shared base class provides common functionality
- Dependency injection manages service lifecycles
- Unit of Work ensures transaction boundaries

## Security Considerations
- All services require authentication by default
- Permission-based authorization at method level
- Input validation prevents injection attacks
- Audit logging tracks all changes
- Tenant isolation enforced throughout
- Sensitive data encrypted in DTOs

## Performance Considerations
- Lazy loading disabled by default
- Explicit includes for related data
- Pagination on all list operations
- Caching for reference data
- Bulk operations for large datasets
- Background jobs for long-running tasks

## Best Practices
- Keep services focused on single responsibility
- Use DTOs for data transfer, never entities
- Implement authorization consistently
- Handle exceptions gracefully
- Log important operations
- Write unit tests for business logic
- Document complex business rules