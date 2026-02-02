# SurpathV2 Database Schema Documentation

## Overview
The `surpathv2` database is a comprehensive multi-tenant application database built on the ABP (ASP.NET Boilerplate) framework. It contains 83 tables that manage various aspects of a medical/healthcare compliance system.

## Database Structure

### Core ABP Framework Tables (36 tables)
These tables provide the foundational multi-tenant architecture, user management, and framework features:

#### User Management
- `abpusers` - Core user information with extended fields for medical context
- `abpuserroles` - User-role assignments
- `abpuserclaims` - User claims for authorization
- `abpuserlogins` - External login providers
- `abpusertokens` - User authentication tokens
- `abpuseraccounts` - Cross-tenant user account linking
- `abpuserorganizationunits` - User assignments to organizational units
- `abpuserloginattempts` - Login attempt tracking

#### Role & Permission Management
- `abproles` - System roles
- `abproleclaims` - Role-based claims
- `abppermissions` - Granular permissions (role and user-based)

#### Tenant Management
- `abptenants` - Multi-tenant configuration with medical-specific fields
- `abpeditions` - Subscription editions with pricing
- `abpfeatures` - Feature management per tenant/edition

#### Organizational Structure
- `abporganizationunits` - Hierarchical organizational units

#### Audit & Logging
- `abpauditlogs` - Comprehensive audit trail
- `abpentitychanges` - Entity change tracking
- `abpentitychangesets` - Change set grouping
- `abpentitypropertychanges` - Property-level change tracking

#### Notifications & Communication
- `abpnotifications` - System notifications
- `abpnotificationsubscriptions` - User notification preferences
- `abptenantnotifications` - Tenant-specific notifications
- `abpusernotifications` - User notification instances

#### Background Processing
- `abpbackgroundjobs` - Background job queue
- `abpwebhookevents` - Webhook event management
- `abpwebhooksendattempts` - Webhook delivery tracking
- `abpwebhooksubscriptions` - Webhook subscriptions

#### Configuration & Localization
- `abpsettings` - Application settings
- `abplanguages` - Supported languages
- `abplanguagetexts` - Localized text resources
- `abpdynamicproperties` - Dynamic property definitions
- `abpdynamicentityproperties` - Entity-specific dynamic properties
- `abpdynamicpropertyvalues` - Dynamic property values
- `abpdynamicentitypropertyvalues` - Entity dynamic property values

#### Security & Identity
- `abppersistedgrants` - OAuth/OpenID Connect grants

### Application-Specific Tables (47 tables)

#### Medical/Healthcare Core
- `hospitals` - Hospital information with contact details
- `medicalunits` - Medical units within hospitals
- `drugs` - Drug/substance definitions
- `panels` - Test panels for drug screening
- `drugpanels` - Drug-panel relationships
- `testcategories` - Test category definitions
- `drugtestcategories` - Drug-test category relationships
- `confirmationvalues` - Confirmation test values and thresholds

#### Tenant & Department Management
- `tenantdepartments` - Tenant-specific departments
- `tenantdepartmentusers` - User-department assignments
- `tenantdepartmentorganizationunits` - Department-org unit mapping
- `departmentusers` - Department user relationships
- `codetypes` - Code type definitions
- `deptcodes` - Department-specific codes

#### User Groups & Cohorts
- `cohorts` - User groupings/cohorts
- `cohortusers` - User-cohort assignments
- `pidtypes` - Personal identifier types
- `userpids` - User personal identifiers

#### Document & Record Management
- `records` - Core document/record storage
- `recordcategories` - Record categorization
- `recordcategoryrules` - Category-specific rules and compliance
- `recordrequirements` - Record requirements per department/cohort
- `recordstates` - Record state tracking
- `recordstatuses` - Available record statuses
- `recordnotes` - Notes attached to records
- `tenantdocumentcategories` - Tenant-specific document categories
- `tenantdocuments` - Tenant document management

#### Services & Billing
- `surpathservices` - Available services
- `tenantsurpathservices` - Tenant-enabled services
- `userpurchases` - User service purchases
- `ledgerentries` - Financial ledger entries
- `ledgerentrydetails` - Detailed ledger line items

#### Scheduling & Rotations
- `rotationslots` - Available rotation slots
- `slotavailabledays` - Available days for slots
- `slotrotationdays` - Rotation day assignments

#### Communication & Social
- `appchatmessages` - Internal messaging system
- `appfriendships` - User friendship/connections

#### System & Utility
- `appbinaryobjects` - Binary file storage
- `apprecentpasswords` - Password history tracking
- `appuserdelegations` - User delegation management
- `approvedemaildomains` - Approved email domains
- `legaldocuments` - Legal document management
- `welcomemessages` - Welcome message management
- `processedtransactions` - Transaction processing tracking

#### Payment & Subscription
- `appsubscriptionpayments` - Subscription payment tracking
- `appsubscriptionpaymentsextensiondata` - Payment extension data
- `appinvoices` - Invoice management

#### Migration & Framework
- `__efmigrationshistory` - Entity Framework migration history

## Key Relationships

### User-Centric Relationships
- Users belong to tenants and can have multiple roles
- Users can be assigned to departments, cohorts, and organizational units
- Users can have multiple PIDs (personal identifiers)
- Users can purchase services and have associated ledger entries

### Document/Record Flow
- Records are categorized and have states/statuses
- Record categories have rules that define compliance requirements
- Records can have notes and are linked to tenant documents
- Record requirements are defined per department/cohort

### Service & Billing
- Services can be global (surpathservices) or tenant-specific (tenantsurpathservices)
- User purchases link to services and generate ledger entries
- Ledger entries have detailed line items

### Medical/Testing Workflow
- Drugs are organized into panels for testing
- Test categories define types of tests
- Confirmation values set thresholds for test results
- Hospitals contain medical units for rotation scheduling

## Multi-Tenancy
The database is designed for multi-tenancy with:
- Most tables include a `TenantId` column for data isolation
- Tenant-specific configurations for departments, services, and documents
- Cross-tenant user account linking capabilities
- Tenant-specific feature and edition management

## Audit & Compliance
Strong audit capabilities with:
- Comprehensive audit logging of all operations
- Entity change tracking at the property level
- Record state management for compliance tracking
- Password history and security tracking

## Data Types & Patterns
- Extensive use of GUIDs (char(36)) for primary keys
- Consistent audit fields (CreationTime, CreatorUserId, etc.)
- Soft delete pattern (IsDeleted, DeletionTime, DeleterUserId)
- Multi-tenant isolation via TenantId columns
- Rich metadata support through JSON/longtext fields

This schema supports a comprehensive medical compliance and record management system with robust multi-tenancy, audit trails, and flexible service delivery capabilities.

## Recent Migrations

### Navarro College Cohort Migration (2024)
- **Purpose:** Move LVN-RN Bridge cohorts from ADN department to LVN-RN Bridge department
- **Affected Cohorts:**
  - LVN-RN Bridge (Grads 2026)-COR
  - LVN-RN Bridge (Grads 2026)-Wax
- **Comprehensive Requirement Mappings:**
  - Cohort-specific "Hep B (Titer)" → "Hepatitis B (Titer)"
  - Department-wide "General Immunizations" → "Measles (Rubeola), Mumps & Rubella (MMR)"
  - Department-wide "Nursing License" → "LVN License (X)"
  - "Student Handbook Signature Form" → "Student Handbook Signature Form"
- **Migration Features:**
  - Maps ALL existing recordstates and uploaded records
  - Creates missing recordstates for new department requirements
  - Ensures complete compliance tracking for all users
  - Handles both current and future uploaded documents
- **Script:** `moveuserstocohort.sql`
- **Schema Files:** Individual table schemas saved as ragdocs for reference

## Schema Documentation Files
Individual table schemas have been documented in separate files for easy reference:
- `abptenants_schema.md` - Core tenant information
- `tenantdepartments_schema.md` - Department/organizational units
- `cohorts_schema.md` - Student cohorts/groups
- `cohortusers_schema.md` - User-cohort relationships
- `records_schema.md` - Document/file storage
- `recordrequirements_schema.md` - Requirements definition
- `recordstates_schema.md` - User compliance tracking 