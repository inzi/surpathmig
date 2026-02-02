# inzibackend.Core Documentation

## Overview
Core domain layer of the inzibackend application implementing Domain-Driven Design principles. Contains all business entities, domain services, business rules, and core logic for a multi-tenant SaaS platform focused on student/user compliance tracking for educational institutions. The system manages drug testing, background checks, document compliance, certifications, and various institutional requirements.

## Contents

### Primary Domain - Surpath
The Surpath module is the core business domain handling all compliance-related functionality:
- **Entities**: Cohorts, Users, Requirements, Records, Services
- **Compliance Logic**: Hierarchical requirement resolution
- **Payment Processing**: Authorize.Net integration
- **Document Management**: Secure file handling
- See [Surpath/CLAUDE.md](Surpath/CLAUDE.md) for detailed documentation

### Core Infrastructure

#### Authorization
Complete authorization system with roles, permissions, and access control:
- User management with ASP.NET Core Identity
- Role-based and permission-based access
- Delegation and impersonation support
- LDAP/Active Directory integration
- See [Authorization/CLAUDE.md](Authorization/CLAUDE.md)

#### MultiTenancy
Full multi-tenant architecture with complete data isolation:
- Tenant management and lifecycle
- Subscription and billing
- Payment gateway integrations
- Automated maintenance workers
- See [MultiTenancy/CLAUDE.md](MultiTenancy/CLAUDE.md)

#### Authentication
Multi-factor authentication and security:
- Google Authenticator integration
- Two-factor authentication
- Secure token management
- See [Authentication/TwoFactor/Google/CLAUDE.md](Authentication/TwoFactor/Google/CLAUDE.md)

### Supporting Modules

#### Chat
Real-time messaging system:
- User-to-user messaging
- Message persistence
- Real-time notifications

#### Configuration
Application settings and configuration:
- Host and tenant settings
- Feature configuration
- System preferences

#### DashboardCustomization
Customizable dashboard system:
- Widget definitions
- User preferences
- Layout management

#### Editions
SaaS edition/plan management:
- Feature toggles
- Edition definitions
- Subscription tiers

#### EntityHistory
Audit and history tracking:
- Entity change tracking
- Audit logs
- History queries

#### Features
Feature flag system:
- Tenant-specific features
- Edition-based features
- Dynamic feature checking

#### Friendships
Social features:
- Friend requests
- Friend lists
- Caching for performance

#### Identity
Extended identity management:
- Security stamps
- Login tracking
- Identity extensions

#### Localization
Multi-language support:
- Resource files
- Language management
- Culture settings

#### Net
Communication services:
#### Emailing
- Email templates
- SMTP configuration
- Email queuing
#### SMS
- SMS providers
- Message templates

#### Notifications
Application notifications:
- Real-time notifications
- Notification persistence
- User preferences

#### Security
Security utilities:
- Password complexity
- Encryption helpers
- Security validations

#### Storage
File and binary storage:
- Binary object management
- File upload/download
- Storage providers

#### Timing
Time zone and timing utilities:
- Time zone conversion
- Business hours
- Scheduling helpers

#### Url
URL management:
- Web URL configuration
- App URL settings
- URL helpers

#### Webhooks
Webhook system:
- Event subscriptions
- Webhook definitions
- Delivery management

### Root Level Files

#### inzibackendConsts.cs
Application-wide constants

#### inzibackendCoreModule.cs
Module initialization and configuration

#### inzibackendDomainServiceBase.cs
Base class for all domain services

#### inzibackendServiceBase.cs
Base class for all services

#### AppFolders.cs
Application folder structure

#### AppVersionHelper.cs
Version management utilities

#### DebugHelper.cs
Debug utilities and helpers

### Key Architectural Patterns

1. **Domain-Driven Design**
   - Rich domain models
   - Domain services
   - Value objects
   - Aggregates

2. **Multi-Tenancy**
   - Complete data isolation
   - Tenant-specific configurations
   - Shared infrastructure

3. **Event-Driven**
   - Domain events
   - Event handlers
   - Asynchronous processing

4. **Repository Pattern**
   - Entity repositories
   - Custom repositories
   - Query specifications

### Dependencies

#### External Libraries
- ABP Framework (foundation)
- Entity Framework Core (ORM)
- ASP.NET Core Identity (authentication)
- Authorize.Net SDK (payments)
- Twilio (SMS)
- SignalR (real-time)

#### Internal Dependencies
- inzibackend.Core.Shared (shared DTOs and enums)
- Various NuGet packages for specific functionality

## Business Domain

### Core Business Concepts

1. **Compliance Tracking**
   - Requirements at multiple levels (tenant, department, cohort, user)
   - Document uploads and verification
   - Expiration tracking
   - Status management

2. **Service Management**
   - Drug testing services
   - Background check services
   - Custom institutional services
   - Hierarchical pricing

3. **User Hierarchy**
   - Tenants (institutions)
   - Departments
   - Cohorts (student groups)
   - Individual users

4. **Payment Models**
   - Institution pay
   - Donor pay (individual payment)
   - Subscription-based
   - Per-service pricing

## Usage Patterns

The Core project is referenced by:
- **Application Layer**: For business logic implementation
- **EntityFrameworkCore**: For persistence
- **Web Projects**: For domain models
- **Background Jobs**: For domain services

## Security Considerations

- Multi-tenant data isolation
- Permission-based access control
- Secure file handling
- Payment data security
- Audit logging
- Input sanitization
- HTTPS enforcement

## Extension Points

- Custom requirement types
- Additional payment gateways
- New notification channels
- Custom authentication providers
- Additional storage providers
- New compliance services

## Configuration

Key configuration areas:
- Connection strings
- Payment gateway settings
- Email/SMS providers
- Storage locations
- Feature flags
- Security settings