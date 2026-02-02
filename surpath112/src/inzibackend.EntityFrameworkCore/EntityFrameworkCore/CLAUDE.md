# EntityFrameworkCore Documentation

## Overview
Core Entity Framework Core configuration and implementation for the Surpath application. Contains the main DbContext, database configuration, migration support, repository base classes, and helper utilities for database operations. This is the primary data access layer implementation.

## Contents

### Files

#### inzibackendDbContext.cs
- **Purpose**: Main Entity Framework Core DbContext for the application
- **Key Functionality**:
  - Inherits from `AbpZeroDbContext` for multi-tenancy and authorization support
  - Defines all DbSet properties for domain entities
  - Configures model relationships and indexes
  - Implements soft delete archiving pattern
  - Overrides SaveChanges to apply archiving concepts
  - Extensive tenant-based indexing for performance

#### inzibackendEntityFrameworkCoreModule.cs
- **Purpose**: ABP module for Entity Framework Core configuration
- **Key Functionality**:
  - Configures DbContext registration
  - Enables entity history tracking
  - Sets up database seeding
  - Integrates with dependency injection
  - Module dependencies: AbpZeroCoreEntityFrameworkCoreModule, inzibackendCoreModule

#### inzibackendDbContextConfigurer.cs
- **Purpose**: Static configuration for DbContext options
- **Key Functionality**:
  - Configures MySQL connection with auto-detection of server version
  - Enables query splitting behavior for performance
  - Provides overloads for connection string and DbConnection

#### inzibackendDbContextFactory.cs
- **Purpose**: Design-time factory for EF Core migrations
- **Key Functionality**:
  - Implements IDesignTimeDbContextFactory
  - Required for `dotnet ef` CLI commands
  - Loads configuration from appsettings.json
  - Creates DbContext instances for migration generation

#### AbpZeroDbMigrator.cs
- **Purpose**: Helper class for database migration execution
- **Key Functionality**:
  - Provides CreateOrMigrateForHost and CreateOrMigrateForTenant methods
  - Handles initial database creation
  - Applies pending migrations
  - Supports multi-tenant migration scenarios

#### DatabaseCheckHelper.cs
- **Purpose**: Utility for checking database existence
- **Key Functionality**:
  - Validates if database exists before operations
  - Used during application startup
  - Prevents seeding on non-existent databases

#### PredicateBuilder.cs
- **Purpose**: Dynamic LINQ expression builder
- **Key Functionality**:
  - Builds complex WHERE clauses dynamically
  - Supports AND/OR operations
  - Used for dynamic filtering in repositories

### Key Components

**Entity Configuration**
- 90+ DbSet properties for domain entities
- Comprehensive indexing strategy (TenantId-based)
- Cascade delete configuration for related entities
- Query filters for soft-deleted entities

**Archiving System**
- IHasArchiving interface support
- Automatic timestamp and user tracking for archives
- Applied during SaveChanges operations

**Multi-Tenancy Support**
- Tenant isolation through indexes
- SuppressAutoSetTenantId for host operations
- Tenant-specific migration support

### Dependencies
- Microsoft.EntityFrameworkCore
- Abp.Zero.EntityFrameworkCore
- Abp.IdentityServer4vNext
- MySQL database provider (Pomelo.EntityFrameworkCore.MySql)

## Subfolders

### MyExtensions
Custom extension methods for Entity Framework operations (currently inactive). [See MyExtensions documentation](MyExtensions/CLAUDE.md)

### Repositories
Base repository implementations and patterns for data access. [See Repositories documentation](Repositories/CLAUDE.md)

## Architecture Notes

**Database Provider**
- MySQL/MariaDB via Pomelo provider
- Auto-detection of server version
- Query splitting enabled for performance

**Model Building Strategy**
- Extensive use of indexes for query performance
- Focus on TenantId indexing for multi-tenant isolation
- Composite indexes for complex query scenarios

**Change Tracking**
- Custom SaveChanges implementation
- Archiving concept application
- Entity history integration

**Migration Strategy**
- Code-first approach with EF Core migrations
- Separate host and tenant migration support
- Design-time factory for CLI operations

## Business Logic

**Soft Delete & Archiving**
- Entities implementing IHasArchiving get automatic archive tracking
- Archives maintain timestamp and user who performed the action
- Supports both archiving and un-archiving operations

**Multi-Tenant Isolation**
- Every entity with TenantId gets indexed
- Automatic tenant filtering through ABP framework
- Support for host-level operations

**Performance Optimizations**
- Strategic indexing on all tenant-scoped entities
- Query splitting to avoid Cartesian products
- Cascade deletes for maintaining referential integrity

## Usage Across Codebase

**Primary Consumers**
- All repository implementations inherit from base classes here
- Application services use repositories for data access
- Migration system uses DbContext for schema updates
- Unit tests may use in-memory database provider

**Configuration Points**
- Connection string: appsettings.json
- Entity history: Enabled in module PreInitialize
- Database provider: MySQL with Pomelo

**Extension Points**
- Custom repositories extend inzibackendRepositoryBase
- Entity configurations in OnModelCreating
- Custom change tracking in SaveChanges overrides