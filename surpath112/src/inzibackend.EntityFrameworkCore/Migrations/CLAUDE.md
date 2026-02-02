# Migrations Documentation

## Overview
Entity Framework Core database migrations for the Surpath application, tracking all schema changes from initial deployment through current version. Contains both migration files and comprehensive seeding infrastructure for data initialization and legacy system imports.

## Contents

### Migration Files

**Initial Setup (2022)**
- `20220620203631_surpathv2-initial`: Initial database schema
- `20220622084230_extended-binaryobjects`: Binary storage enhancements
- `20220627163241_final-11-0`: ANZ 11.0 compatibility
- `20220627210742_update-to-anz-112`: ANZ 11.2 update

**User & Identity Management**
- `20220906164831_add-user-middle-name`: User profile extensions
- `20221011124632_extending-user-with-democraphic-data`: Demographic fields
- `20221011215343_added-datofbirth-to-user`: DOB tracking
- `20221014210751_userid-to-userpid`: PID system integration

**Document Management**
- `20220910*_TenantDocumentCategory`: Multiple document category updates
- `20220910082402_Regenerated_TenantDocument`: Document entity updates
- `20220911145446_add-originalfilename-to-binaryobject`: File tracking

**Compliance & Records**
- `20220915055149_dates_to_record_entity`: Record date tracking
- `20220915060231_cohorts_dept_torequirements`: Department requirements
- `20220916121738_add-user-instruction-review-to-record`: Review tracking
- `20230812010725_record-effective-expiration-dates`: Expiration management

**Payment & Billing**
- `20221119234935_add-ledger`: Ledger system
- `20240103051915_CardLastFourToLedgerEntry`: Payment card tracking
- `20230716011515_donor_pay_ledger_updates`: Donor payment system
- `20240824193009_addProcessedTransactions`: Transaction tracking

**Service Management**
- `20221120003207_added-tenant-surpath-service`: Service configuration
- `20250102040359_surpath-services-application-update`: Service updates
- `20250915185911_tenantsurpathservice-isenabled`: Pricing overrides

**Medical Rotation System (2024)**
- `20240304022439_hospitalentity`: Hospital management
- `20240304093222_rotationslots`: Rotation scheduling
- `20240305102544_medicalunits`: Medical unit tracking

**Archiving & Soft Delete (2025)**
- `20250627013236_iscurrentdocument-flag`: Document versioning
- `20250627031423_ihasarchiving`: Archiving interface
- `20250627100111_ConvertDeletedToArchived`: Soft delete migration

**Recent Updates**
- `20241027060515_deptOUs`: Department organization units
- `20250515103140_legal-document-entity`: Legal document tracking
- `20250526232735_AddMigrationAuditLog`: Migration auditing
- `20250903055201_is-invoiced`: Invoice tracking

### Key Components

**Migration Patterns**
- Incremental schema updates
- Data transformation migrations
- Index optimizations
- Relationship adjustments

**Naming Convention**
- Timestamp prefix (yyyyMMddHHmmss)
- Descriptive suffix
- Designer files for metadata

### Dependencies
- Entity Framework Core
- MySQL database provider
- ABP Framework entities

## Subfolders

### Seed
Comprehensive database seeding infrastructure for initialization and data migration. [See Seed documentation](Seed/CLAUDE.md)

## Architecture Notes

**Migration Strategy**
- Code-first approach
- Automatic migration on startup
- Separate host/tenant migrations
- Rollback capability

**Schema Evolution**
- Additive changes preferred
- Data migrations for breaking changes
- Index optimization migrations

**Performance Considerations**
- Strategic index creation
- Query optimization updates
- Batch processing for large migrations

## Business Logic

**Version Management**
- Sequential migration application
- Migration history tracking
- Rollback support

**Multi-Tenant Support**
- Host database migrations
- Tenant-specific migrations
- Shared schema updates

**Data Integrity**
- Foreign key constraints
- Default values
- Null handling

## Usage Across Codebase

**Migration Commands**
```bash
dotnet ef migrations add MigrationName
dotnet ef database update
dotnet ef migrations remove
```

**Automatic Migration**
- On application startup
- During deployment
- Development environment

**Manual Migration**
- Production deployments
- Specific version targeting
- Rollback operations

**Related Systems**
- DbContext configuration
- Entity definitions
- Seeding infrastructure
- Database initialization

**Critical Migrations**
- Initial schema creation
- User system extensions
- Payment system implementation
- Archiving system addition
- Medical rotation features