# Seed Documentation

## Overview
Database seeding infrastructure for initializing and migrating data in the Surpath application. Provides comprehensive data seeding capabilities for both greenfield deployments and migrations from legacy systems, supporting multi-tenant architecture, reference data, and complex data imports.

## Contents

### Files

#### SeedHelper.cs
- **Purpose**: Central orchestrator for all database seeding operations
- **Key Functionality**:
  - Coordinates host and tenant seeding
  - Manages legacy data imports
  - Controls seeding workflow
  - Handles configuration and environment detection
  - Provides transaction management for seeding operations
  - Configurable import based on appsettings

#### Log.cs
- **Purpose**: Logging utility for seed operations
- **Key Functionality**:
  - Tracks seeding progress
  - Records import statistics
  - Error logging
  - Debug information capture

### Key Components

**Seeding Workflow**
1. Host database initialization
2. Default tenant creation
3. Reference data seeding (drugs, code types)
4. Legacy data import (if configured)
5. Integrity validation

**Configuration Management**
- Environment-based configuration
- Connection string management
- Import flags and parameters
- File storage paths

**Transaction Handling**
- Suppressed transactions for bulk operations
- Unit of Work pattern implementation
- Rollback capabilities

### Dependencies
- Entity Framework Core
- ABP Framework
- MySQL connections (legacy)
- Configuration system

## Subfolders

### Host
Core system initialization including editions, languages, roles, and default settings. [See Host documentation](Host/CLAUDE.md)

### Tenants
Default tenant creation with roles, users, and initial configuration. [See Tenants documentation](Tenants/CLAUDE.md)

### Surpath
Domain-specific reference data for medical compliance including drugs, test panels, and code types. [See Surpath documentation](Surpath/CLAUDE.md)

### Importing
Comprehensive legacy data migration system for Surpath/Surscan Live data. [See Importing documentation](Importing/CLAUDE.md)

## Architecture Notes

**Modular Seeding Design**
- Separate seeders per domain
- Independent transaction scopes
- Configurable execution

**Environment Support**
- Development: Full seeding with test data
- Production: Minimal required data
- Migration: Legacy import support

**Error Recovery**
- Transaction rollback
- Partial seeding support
- Logging for troubleshooting

## Business Logic

**Initialization Strategy**
- Check database existence
- Apply migrations
- Seed in dependency order
- Validate data integrity

**Multi-Tenant Setup**
- Host data first
- Default tenant creation
- Tenant-specific data
- Isolation verification

**Legacy Migration**
- Configurable import
- Client-by-client migration
- Data transformation
- Relationship preservation

## Usage Across Codebase

**Application Startup**
- Called from EntityFrameworkCoreModule
- Part of database initialization
- First-run setup

**Development Workflow**
- Database reset and reseed
- Test data generation
- Demo environment setup

**Production Deployment**
- Initial deployment
- Migration from legacy
- Data updates

**Configuration Points**
- Surpath:DoImport - Enable legacy import
- Surpath:SurpathRecordsRootFolder - File storage
- Surpath:SurpathLiveClientIdsToImport - Import list
- ConnectionStrings:surpathlive - Legacy database

**Key Interfaces**
- IIocResolver - Dependency resolution
- IUnitOfWorkManager - Transaction management
- IDbContextProvider - Database access
- IBinaryObjectManager - File storage