# inzibackend.Application.Shared Documentation

## Overview
This folder contains the shared application service interfaces and Data Transfer Objects (DTOs) that define the API surface for the Surpath compliance tracking system. It serves as the contract layer between the application services and their consumers (web controllers, mobile apps, integration endpoints). The folder implements a comprehensive multi-tenant SaaS platform for schools to track student compliance with policies including drug testing, documentation, background checks, immunizations, and clinical requirements.

## Contents

### Files

#### Core Configuration
- **AppConsts.cs** - Application-wide constants:
  - Paging configuration (DefaultPageSize: 500, MaxPageSize: 1000)
  - Security settings and pass phrases
  - Token expiration settings (Access: 1 day, Refresh: 365 days)
  - UI theme definitions (14 themes)
  - US States lookup data
  - Date/time format standards

- **inzibackendApplicationSharedModule.cs** - Module configuration for dependency injection and initialization

### Key Components
- **Service Interfaces**: Define contracts for all application services
- **DTOs**: Data transfer objects for all operations
- **Multi-tenancy**: Complete tenant isolation and management
- **Compliance Tracking**: Core business domain for student compliance
- **Payment Processing**: Multiple gateway integrations
- **Authentication**: Comprehensive auth with impersonation support

### Dependencies
- ABP Framework - Base application framework
- System.ComponentModel.DataAnnotations - Validation
- Microsoft.AspNetCore.Http - File uploads
- Payment gateways - Stripe, PayPal, Authorize.Net

## Subfolders

### Surpath
**Core Business Domain** - Complete compliance tracking system
- 36+ service interfaces for all business operations
- 268+ DTOs covering all entities and workflows
- Compliance requirements and multi-step workflows
- Drug testing management
- Document upload and verification
- Clinical rotation scheduling
- Financial tracking
[Full details in Surpath/CLAUDE.md](Surpath/CLAUDE.md)

### Authorization
**Authentication & Authorization** - User and account management
- Account registration and activation
- Password management
- Multi-tenant user impersonation
- Role and permission management
- User delegation
[Full details in Authorization/CLAUDE.md](Authorization/CLAUDE.md)

### MultiTenancy
**SaaS Platform Foundation** - Multi-tenant architecture
- Tenant creation and management
- Feature toggles per tenant
- Subscription and billing
- Payment gateway integrations
- Host dashboard for SaaS operations
[Full details in MultiTenancy/CLAUDE.md](MultiTenancy/CLAUDE.md)

### Sessions
**User Session Management** - Session and context handling
- Current user information
- Tenant context
- Application metadata
- Security token management
[Full details in Sessions/CLAUDE.md](Sessions/CLAUDE.md)

### Auditing
**Audit Trail System** - Comprehensive audit logging
- Entity change tracking
- User action logging
- Audit log queries and exports
- Expired log backup services

### Configuration
**System Configuration** - Application settings management
- Host-level configuration
- Tenant-specific settings
- Email settings
- Security configurations

### Notifications
**Notification System** - Real-time and async notifications
- Push notifications
- Email notifications
- In-app notifications
- Notification preferences

### Organizations
**Organizational Structure** - Organization unit management
- Hierarchical organization trees
- Department management
- Organization-based permissions

### Editions
**Feature Editions** - SaaS pricing tiers
- Edition management
- Feature sets per edition
- Pricing configuration

### Chat
**Real-time Communication** - Chat functionality
- User-to-user messaging
- Real-time updates
- Message history

### Localization
**Internationalization** - Multi-language support
- Language management
- Resource strings
- Culture settings

### DynamicEntityProperties
**Runtime Customization** - Dynamic property system
- Custom fields for entities
- Runtime property definitions
- Property value management

### Common
**Shared Components** - Common DTOs and utilities
- Lookup table inputs
- Paging and sorting DTOs
- Common validation

### Dto
**Base DTOs** - Fundamental data structures
- FileDto for file operations
- PagedAndSortedInputDto
- Common result DTOs

### Logging
**Application Logging** - Log management
- Web log viewing
- Log download
- Log filtering

### Caching
**Cache Management** - Application cache control
- Cache statistics
- Cache clearing
- Cache configuration

### Friendships
**Social Features** - User relationships
- Friend requests
- Friend lists
- Blocking functionality

### Tenants
**Tenant Features** - Tenant-specific functionality
- Tenant dashboard
- Tenant statistics
- Usage tracking

### UiCustomization
**UI Theming** - User interface customization
- Theme selection
- UI settings per tenant
- Branding options

### WebHooks
**Integration Hooks** - Webhook management
- Event subscriptions
- Webhook endpoints
- Delivery tracking

### Timing
**Time Management** - Timezone handling
- Timezone lists
- User timezone preferences
- Time conversion utilities

### DemoUiComponents
**Demo Features** - UI component demonstrations
- Sample components
- Demo data generators

## Architecture Notes
- **Clean Architecture**: Clear separation between contracts (interfaces) and DTOs
- **Service-Oriented**: All functionality exposed through service interfaces
- **Multi-Tenant Design**: Every service is tenant-aware
- **DTO Pattern**: Consistent use of DTOs for data transfer
- **Async/Await**: All services use async Task-based operations
- **Paged Results**: Consistent paging pattern with PagedResultDto
- **Validation**: DTOs include validation attributes and logic
- **Audit Trail**: Comprehensive auditing built into the framework

## Business Logic
The application implements a complete student compliance tracking system:

1. **Student Lifecycle**:
   - Registration with comprehensive profile
   - Assignment to cohorts and departments
   - Requirement tracking and compliance monitoring
   - Document upload and verification
   - Expiration tracking and notifications

2. **Compliance Workflow**:
   - Requirements defined at tenant/department/cohort level
   - Multi-step requirements with categories and rules
   - Document upload → Review → Approval/Rejection
   - Automatic expiration monitoring
   - Notification on status changes

3. **Drug Testing**:
   - Panel and drug configuration
   - Test scheduling and tracking
   - Result confirmation workflow
   - Chain of custody documentation

4. **Clinical Rotations**:
   - Hospital and medical unit management
   - Rotation slot scheduling
   - Student placement tracking

5. **Financial Management**:
   - Ledger entry tracking
   - Invoice generation
   - Payment processing
   - Multi-gateway support

## Usage Across Codebase
This shared layer is consumed by:
- **inzibackend.Application**: Service implementations
- **inzibackend.Web.Mvc**: MVC controllers and views
- **inzibackend.Web.Host**: API endpoints
- **Mobile Applications**: Via REST APIs
- **Background Jobs**: For async processing
- **Integration Services**: Third-party integrations
- **Report Generators**: For compliance reporting
- **JavaScript/TypeScript**: Client-side consumption

## Cross-Reference Impact
Changes to this layer have wide impact:
- **Service Interfaces**: Breaking changes affect all implementations
- **DTOs**: Changes impact serialization and client compatibility
- **Validation**: Changes affect data integrity across the system
- **Constants**: Changes to AppConsts affect system-wide behavior

## Security Considerations
- Sensitive fields marked with [DisableAuditing]
- Password fields use proper security attributes
- Multi-tenant isolation enforced at service level
- Impersonation requires special permissions
- Token expiration for session security