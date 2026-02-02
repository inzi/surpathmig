# Importing Documentation

## Overview
Comprehensive data import system for migrating legacy Surpath/Surscan Live data into the new system. Handles complex ETL operations including client data, user accounts, medical records, compliance information, and associated documents with full data integrity and relationship preservation.

## Contents

### Files

#### SPLImporter.cs
- **Purpose**: Main importer for Surpath Live data migration
- **Key Functionality**:
  - Imports entire client organizations with all related data
  - Creates tenants, departments, users, and records
  - Handles document and binary file migration
  - Maintains referential integrity across systems
  - Configurable import parameters (days_old, max_donors)

#### ClientImporter.cs
- **Purpose**: Client/Tenant creation and configuration
- **Key Functionality**:
  - Creates new tenants from legacy clients
  - Sets up organizational structure
  - Configures tenant settings
  - Establishes departments and hierarchies

#### UserImporter.cs
- **Purpose**: User account migration
- **Key Functionality**:
  - Imports donor and admin users
  - Handles password hashing
  - Sets up roles and permissions
  - Maps legacy user IDs to new system

#### ClientCreatorImporter.cs
- **Purpose**: Client admin and organization setup
- **Key Functionality**:
  - Creates administrative users
  - Sets up organization hierarchy
  - Configures client-specific settings

#### SPTenantIntegrityChecker.cs
- **Purpose**: Post-import validation and integrity checking
- **Key Functionality**:
  - Validates imported data
  - Checks referential integrity
  - Reports import issues
  - Ensures data consistency

#### ImportFromSurscanLiveJob.cs & ImportFromSurscanLiveJobArgs.cs
- **Purpose**: Background job for async imports
- **Key Functionality**:
  - Queues import operations
  - Tracks import progress
  - Handles large-scale migrations
  - Configurable import parameters

#### ClientRecordCategoryClass.cs
- **Purpose**: DTO for record category mapping
- **Key Functionality**:
  - Maps legacy categories to new system
  - Maintains category relationships
  - Handles category metadata

#### ClientUserOrAdmin.cs
- **Purpose**: DTO for user type differentiation
- **Key Functionality**:
  - Distinguishes admin vs regular users
  - Maps user roles
  - Handles user metadata

#### SafeGetString.cs
- **Purpose**: Safe data extraction utilities
- **Key Functionality**:
  - Null-safe database field reading
  - Type conversion helpers
  - Error prevention utilities

### Key Components

**Import Pipeline**
1. Client/Tenant creation
2. Department structure setup
3. User account migration
4. Medical record import
5. Document/file migration
6. Integrity validation

**Data Mapping Strategy**
- Legacy ID preservation
- New ID generation
- Cross-reference tables
- Relationship mapping

**File Migration**
- Binary object storage
- Document categorization
- Path mapping
- Storage optimization

### Dependencies
- Legacy database connection (MySQL)
- Binary object storage system
- File system access
- Identity management

## Architecture Notes

**ETL Architecture**
- Extract from legacy MySQL
- Transform data structures
- Load into new EF Core model
- Validate post-import

**Performance Optimizations**
- Batch processing
- Configurable limits
- Async operations
- Transaction management

**Error Handling**
- Comprehensive logging
- Rollback capabilities
- Partial import support
- Error recovery

## Business Logic

**Multi-Tenant Migration**
- Each legacy client becomes a tenant
- Preserves organizational structure
- Maintains user relationships
- Retains historical data

**Document Management**
- Migrates file attachments
- Preserves document categories
- Maintains audit trails
- Handles large files

**Compliance Data**
- Medical records preservation
- Testing history migration
- Requirement tracking
- Status preservation

## Usage Across Codebase

**Migration Scenarios**
- Initial system deployment
- Client onboarding
- Data consolidation
- System upgrades

**Configuration**
- appsettings.json controls
- Import parameters (client_id, days_old, max_donors)
- Connection strings
- File storage paths

**Integration Points**
- Called from SeedHelper
- Background job processing
- Admin import interfaces
- Data validation tools

**Related Entities**
- Tenants and departments
- Users and roles
- Records and requirements
- Documents and files