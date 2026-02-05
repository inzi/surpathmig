# inzibackend.Core.Shared Documentation

## Overview
Shared constants, enumerations, DTOs, and helper classes used across both mobile and web projects. This library provides the common data structures and cross-cutting concerns for the Surpath compliance tracking system, supporting multi-tenant SaaS operations for educational institutions.

## Contents

### Files

#### inzibackendConsts.cs
- **Purpose**: Application-wide constants and configuration
- **Key Constants**:
  - LocalizationSourceName: "inzibackend"
  - ConnectionStringName: "Default"
  - MultiTenancyEnabled: true
  - Currency: USD with "$" sign
  - MinimumUpgradePaymentAmount: $1.00
  - AllowTenantsToChangeEmailSettings: false
- **Usage**: Core application configuration values

#### inzibackendCoreSharedModule.cs
- **Purpose**: ABP module registration for Core.Shared assembly
- **Functionality**: Registers all services and components via IoC container
- **Pattern**: ABP module initialization pattern

#### inzibackendDashboardCustomizationConst.cs
- **Purpose**: Dashboard widget and filter identifiers
- **Widget Categories**:
  - **Tenant Widgets**: GeneralStats, DailySales, ProfitShare, MemberActivity, RegionalStats, SalesSummary, TopStats, SurpathDept, SurpathCohortCompliance
  - **Host Widgets**: TopStats, IncomeStatistics, EditionStatistics, SubscriptionExpiringTenants, RecentTenants
- **Filters**: DateRangePicker, SurpathDeptFilter
- **Dashboards**: TenantDashboard, HostDashboard
- **Applications**: Mvc, Angular

### Key Components
- Multi-tenant architecture support
- Localization infrastructure
- Payment processing configuration
- Dashboard customization framework
- Shared business domain models

### Dependencies
- Abp.Modules: ABP framework module system
- System libraries for base types
- Various third-party integrations (Authorize.Net, PdfPig)

## Subfolders

### Authentication
External authentication provider configurations:
- Facebook, Google, Microsoft OAuth
- OpenID Connect and WS-Federation
- Claim mapping for external identities

### Authorization
Role and user authorization constants:
- Static role definitions (Host Admin, Tenant Admin, User)
- User validation constraints (phone number limits)

### Chat
Real-time messaging enumerations:
- Message read states (Unread/Read)
- Chat participant sides (Sender/Receiver)

### Editions
SaaS edition and payment types:
- Payment scenarios (NewRegistration, BuyNow, Upgrade, Extend)
- Edition management for multi-tenant subscriptions

### Friendships
User relationship management:
- Friendship states (Accepted/Blocked)

### MultiTenancy
Multi-tenant infrastructure and payments:
- Tenant configuration constants
- Comprehensive payment system (PayPal, Stripe)
- Subscription management models

### Notifications
Application notification system:
- System notifications
- Record state notifications
- Progressive warning system

### Security
Security configurations:
- Password complexity settings
- Configurable security policies

### Storage
Binary storage configuration:
- Size limits for stored objects (10KB max)

### Surpath
Core business domain for compliance tracking:
- **Compliance**: Multi-category tracking (drug, background, immunization)
- **AuthNet**: Authorize.Net payment integration
- **Extensions**: Utility methods and settings
- **LegalDocuments**: Privacy policy and terms management
- **ParserClasses**: Lab report PDF parsing
- **Purchase**: Service purchase workflows
- Comprehensive enumerations for all business states
- Constants for validation and constraints

### Validation
Common validation utilities:
- Email validation helpers
- Regex patterns

### Webhooks
External integration support:
- Webhook event definitions

## Architecture Notes
- Clean separation between shared and implementation-specific code
- Domain-driven design with rich enumerations
- Multi-tenant isolation at the core level
- Extensible payment gateway architecture
- Comprehensive notification and webhook systems
- Support for multiple UI frameworks (MVC, Angular)

## Business Logic
- **Compliance System**: Tracks drug testing, background checks, and immunizations
- **Multi-Tenancy**: Complete tenant isolation with shared infrastructure
- **Payment Processing**: Multiple gateway support with pre-auth workflows
- **Document Management**: Legal documents, lab reports, and compliance records
- **Dashboard System**: Customizable widgets per tenant/host
- **Notification System**: Multi-stage warnings and alerts

## Usage Across Codebase
This shared library is referenced by:
- inzibackend.Core: Domain entities and services
- inzibackend.Application: Application services
- inzibackend.Web.Mvc: MVC web application
- inzibackend.Web.Host: API host project
- Mobile applications (if any)
- Test projects for shared functionality

## Cross-Reference Impact
Changes to Core.Shared affect:
- All entity definitions using these constants
- Validation logic across all layers
- UI dropdowns and displays
- Payment processing workflows
- Compliance calculations
- Dashboard widget implementations
- External authentication flows