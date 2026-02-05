# Surpath Documentation

## Overview
Core domain layer for the Surpath compliance management system. This module contains all business entities, domain services, and business logic for tracking student/user compliance with institutional requirements including drug testing, background checks, document management, and various certifications. It implements a hierarchical requirement system with multi-tenant support.

## Contents

### Core Entities

#### Cohort.cs
- **Purpose**: Groups of users (typically students) with shared compliance requirements
- **Key Features**: Default cohort support, department association, full auditing

#### CohortUser.cs
- **Purpose**: Junction entity linking users to cohorts
- **Key Relationships**: Many-to-many between Users and Cohorts

#### RecordRequirement.cs
- **Purpose**: Defines compliance requirements that users must satisfy
- **Key Features**:
  - Hierarchical scoping (Tenant → Department → Cohort)
  - Support for Surpath services (drug tests, background checks)
  - Metadata for additional configuration

#### Record.cs
- **Purpose**: Uploaded documents/files satisfying requirements
- **Key Features**: File storage, metadata, expiration tracking, document categories

#### RecordState.cs
- **Purpose**: Tracks the compliance status of a user for a specific requirement
- **Key Features**: Status tracking, expiration dates, approval workflow

#### RecordCategory.cs
- **Purpose**: Categories within requirements for organizing documents
- **Key Features**: Rules-based categorization, requirement association

#### RecordCategoryRule.cs
- **Purpose**: Business rules for document categories
- **Key Features**: Validation rules, auto-categorization logic

#### TenantDepartment.cs
- **Purpose**: Organizational units within a tenant
- **Key Features**: Hierarchical organization, requirement scoping

#### TenantDepartmentUser.cs
- **Purpose**: Associates users with departments
- **Key Features**: Many-to-many relationship, access control basis

#### SurpathService.cs
- **Purpose**: System-wide service definitions (drug testing, background checks)
- **Key Features**: Feature flags, pricing, service configuration

#### TenantSurpathService.cs
- **Purpose**: Tenant-specific service configurations and overrides
- **Key Features**: Pricing overrides, user-specific assignments, hierarchical pricing

### Payment Entities

#### LedgerEntry.cs & LedgerEntryDetail.cs
- **Purpose**: Financial transaction tracking for paid services

#### UserPurchase.cs
- **Purpose**: Tracks individual user purchases of services

#### ProcessedTransactions.cs
- **Purpose**: Payment gateway transaction records

### Supporting Entities

#### UserPid.cs & PidType.cs
- **Purpose**: Personal identifiers (SSN, employee ID, etc.) for users

#### Panel.cs, Drug.cs, DrugPanel.cs
- **Purpose**: Drug testing panel configurations

#### TenantDocument.cs & TenantDocumentCategory.cs
- **Purpose**: Tenant-specific document templates and categories

#### RecordStatus.cs
- **Purpose**: Status definitions for compliance tracking

#### RecordNote.cs
- **Purpose**: Administrative notes on compliance records

#### RotationSlot.cs, SlotAvailableDay.cs, SlotRotatationDay.cs
- **Purpose**: Scheduling system for rotations and appointments

#### Welcomemessage.cs
- **Purpose**: Customizable welcome messages for users

#### MigrationAuditLog.cs
- **Purpose**: Tracking data migrations and system updates

#### HierarchicalPricingModels.cs & HierarchicalPricingManager.cs
- **Purpose**: Complex pricing structures for services

### Subfolders

#### AuthNet/
- Authorize.Net payment gateway integration
- Credit card processing for services

#### Compliance/
- Core compliance checking logic
- Requirement resolution algorithms
- Access control for compliance documents

#### Helpers/
- HTML sanitization for security
- Utility functions

#### LegalDocuments/
- Privacy policies and terms of service management

#### MetaData/
- Notification tracking metadata
- Extensible metadata system

#### OUManager/
- Organization unit based security
- Row-level data filtering

#### OUs/
- Organization unit entity relationships

#### Statics/
- ID generation utilities
- Configuration constants

#### SurpathPayManager/
- Payment processing domain services
- Ledger management

#### Workers/
- Background jobs for system maintenance
- Periodic synchronization tasks

### Key Business Logic

#### Requirement Resolution Hierarchy
1. **User-specific** requirements (highest priority)
2. **Cohort-specific** requirements
3. **Department-specific** requirements
4. **Tenant-wide** requirements (lowest priority)

#### Service Types
- **Drug Testing**: Random and scheduled drug screens
- **Background Checks**: Criminal and other background verifications
- **Document Compliance**: Certifications, immunizations, training records

#### Payment Models
- **Institution Pay**: School/organization covers costs
- **Donor Pay**: Individual users pay for services

### Dependencies

- **External Libraries**:
  - ABP Framework (Domain entities, auditing, multi-tenancy)
  - Entity Framework Core
  - Authorize.Net SDK
  - System.Security.Cryptography

- **Internal Dependencies**:
  - inzibackend.Authorization (Users, Roles, Permissions)
  - inzibackend.MultiTenancy
  - inzibackend.Configuration

## Architecture Notes

- **Domain-Driven Design**: Rich domain models with business logic
- **Multi-tenancy**: Full tenant isolation with IMayHaveTenant
- **Auditing**: Comprehensive audit trail with FullAuditedEntity
- **Soft Deletes**: Most entities support soft deletion
- **GUID Primary Keys**: Consistent use of GUIDs for entities

## Usage Across Codebase

The Surpath domain is the core of:
- Compliance tracking and reporting
- Service enrollment and management
- Payment processing
- Document upload and verification
- User onboarding and registration
- Administrative compliance reviews
- Scheduling and rotation management

## Security Considerations

- Tenant isolation enforced at entity level
- Permission-based access control
- HTML sanitization for user content
- Secure payment tokenization
- Audit trails for compliance