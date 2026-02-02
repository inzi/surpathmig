# inzibackend.EntityFrameworkCore Documentation

## Overview
The complete Entity Framework Core data access layer implementation for the Surpath medical compliance application. This layer provides database connectivity, migrations, repository patterns, and comprehensive data seeding infrastructure, supporting multi-tenant architecture with MySQL as the backend database.

## Contents

### Core Files

#### Project Configuration
- **inzibackend.EntityFrameworkCore.csproj**: Project dependencies and EF Core packages

### Key Components

**Data Access Architecture**
- Entity Framework Core 6.0 with MySQL provider (Pomelo)
- Repository pattern implementation with ABP framework
- Unit of Work pattern for transaction management
- Multi-tenant data isolation

**Database Features**
- Code-first migrations
- Soft delete with archiving
- Entity history tracking
- Automatic audit fields

**Performance Optimizations**
- Strategic indexing on TenantId
- Query splitting for complex queries
- Bulk operations support
- Efficient change tracking

### Dependencies
- **Core Framework**: ABP (ASP.NET Boilerplate) Zero
- **ORM**: Entity Framework Core 6.0
- **Database**: MySQL/MariaDB via Pomelo provider
- **Extensions**: Z.EntityFramework.Plus for bulk operations
- **Identity**: ASP.NET Core Identity integration

## Subfolders

### EntityFrameworkCore
Core DbContext, configuration, repositories, and database utilities. The heart of the data access layer. [See EntityFrameworkCore documentation](EntityFrameworkCore/CLAUDE.md)

### Migrations
Complete migration history from initial schema to current version, tracking all database changes. [See Migrations documentation](Migrations/CLAUDE.md)

### Authorization
Custom authorization-related repositories, particularly for user management and password policies. [See Authorization documentation](Authorization/CLAUDE.md)

### MultiTenancy
Multi-tenant specific repositories for subscription payments and tenant isolation. [See MultiTenancy documentation](MultiTenancy/CLAUDE.md)

### EntityHistory
Custom entity history tracking with PII masking for audit compliance. [See EntityHistory documentation](EntityHistory/CLAUDE.md)

## Architecture Notes

**Layered Architecture**
```
Application Services
        ↓
Domain Layer (Core)
        ↓
EntityFrameworkCore (This Layer)
        ↓
MySQL Database
```

**Design Patterns**
- **Repository Pattern**: Abstraction over EF Core
- **Unit of Work**: Transaction management
- **Factory Pattern**: DbContext creation
- **Module Pattern**: ABP module system

**Multi-Tenancy Strategy**
- Shared database, separate data
- TenantId-based filtering
- Automatic tenant resolution
- Host/Tenant separation

**Migration Strategy**
- Code-first approach
- Incremental migrations
- Automated deployment support
- Rollback capabilities

## Business Logic

**Core Domain Support**
- Medical compliance tracking
- User and role management
- Document management
- Payment processing
- Rotation scheduling
- Department organization

**Data Integrity**
- Referential integrity via foreign keys
- Cascade delete rules
- Soft delete preservation
- Audit trail maintenance

**Security Features**
- PII data masking in history
- Tenant data isolation
- User permission checks
- Secure password storage

## Usage Across Codebase

**Primary Integration Points**
- **Application Layer**: Uses repositories for data access
- **Web Layer**: Connection string configuration
- **Core Layer**: Entity definitions
- **Test Projects**: In-memory database option

**Configuration**
- **Connection Strings**: appsettings.json
- **Migration**: Automatic or manual via CLI
- **Seeding**: Controlled by configuration flags
- **Module**: Registered in application startup

**Development Workflow**
```bash
# Add new migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update

# Remove last migration
dotnet ef migrations remove
```

**Key Services**
- `inzibackendDbContext`: Main database context
- `IRepository<T>`: Generic repository interface
- `IUnitOfWorkManager`: Transaction management
- `IDbContextProvider`: Context resolution

**Performance Considerations**
- Lazy loading disabled by default
- Explicit loading via Include
- Query splitting for complex joins
- Indexed lookups on TenantId

## Cross-Reference Analysis

**High-Impact Components**
- `inzibackendDbContext`: Central to all data operations
- Repository base classes: Foundation for all repositories
- Migration system: Database schema management
- Seeding infrastructure: Initial data setup

**Downstream Dependencies**
- All application services depend on repositories
- Web API controllers use application services
- Background jobs access repositories directly
- Unit tests mock repository interfaces

**Upstream Dependencies**
- Core domain entities
- ABP framework infrastructure
- MySQL database server
- Configuration system

## Testing Considerations

**Unit Testing**
- Repository interfaces enable mocking
- In-memory database for integration tests
- Transaction rollback for test isolation

**Migration Testing**
- Test migrations in development first
- Backup before production migrations
- Verify rollback procedures

**Performance Testing**
- Query performance monitoring
- Index effectiveness validation
- Connection pool optimization