# Repositories Documentation

## Overview
Base repository implementation for the Surpath application, providing a foundation for all custom repositories. Extends ABP's Entity Framework Core repository base class to establish consistent data access patterns across the application.

## Contents

### Files

#### inzibackendRepositoryBase.cs
- **Purpose**: Abstract base classes for all custom repositories in the application
- **Key Functionality**:
  - Generic repository base for any entity type and primary key
  - Specialized repository base for int primary keys
  - Placeholder for common repository methods shared across all repositories

### Key Components

**inzibackendRepositoryBase<TEntity, TPrimaryKey> Class**
- Abstract base class for custom repositories
- Inherits from `EfCoreRepositoryBase<inzibackendDbContext, TEntity, TPrimaryKey>`
- Generic parameters:
  - `TEntity`: The entity type (must implement IEntity<TPrimaryKey>)
  - `TPrimaryKey`: The primary key type
- Protected constructor accepting IDbContextProvider
- Designated location for common repository methods

**inzibackendRepositoryBase<TEntity> Class**
- Convenience base class for entities with int primary keys
- Inherits from `inzibackendRepositoryBase<TEntity, int>`
- Simplifies repository declarations for common int-key entities
- Warning comment: methods should be added to parent class only

### Dependencies
- Abp.Domain.Entities (for IEntity interface)
- Abp.EntityFrameworkCore (for EfCoreRepositoryBase)
- Abp.EntityFrameworkCore.Repositories (base repository functionality)
- inzibackendDbContext (application's EF Core context)

## Architecture Notes

**Repository Pattern Implementation**
- Follows ABP's repository pattern conventions
- Provides abstraction over Entity Framework Core
- Enables unit testing through repository interfaces

**Design Principles**
- DRY: Common functionality in single base class
- Type safety through generics
- Consistent data access patterns

**Extension Points**
- Common query methods can be added to base class
- Specialized repositories inherit and extend functionality
- Supports both generic and specific primary key types

## Business Logic

**Data Access Abstraction**
- Hides EF Core implementation details from application layer
- Provides testable data access interface
- Ensures consistent query patterns

**Common Operations** (placeholder for):
- Complex filtering logic
- Soft delete handling
- Multi-tenancy filters
- Common includes/eager loading

## Usage Across Codebase

**Direct Implementations**
- `UserRepository`: Custom user data access with password expiry logic
- `SubscriptionPaymentRepository`: Payment-specific queries
- `SubscriptionPaymentExtensionDataRepository`: Extension data management

**Inheritance Hierarchy**
```
EfCoreRepositoryBase (ABP Framework)
    ↓
inzibackendRepositoryBase<TEntity, TPrimaryKey>
    ↓
inzibackendRepositoryBase<TEntity> (int key shortcut)
    ↓
Specific Repository Implementations
```

**Integration Points**
- Registered via dependency injection in EntityFrameworkCore module
- Used by application services for data access
- Supports Unit of Work pattern through ABP framework