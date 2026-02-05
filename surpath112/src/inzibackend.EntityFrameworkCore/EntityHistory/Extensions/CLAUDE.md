# Extensions Documentation

## Overview
Entity Framework Core extension methods for tracking property changes in entities during audit history operations. Provides utility methods to extract original and new values from entity property entries.

## Contents

### Files

#### EntityEntryExtensions.cs
- **Purpose**: Extension methods for Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry
- **Key Functionality**: Determines if an entity is marked for soft deletion
- **Status**: Commented out/inactive code

#### PropertyEntryExtensions.cs
- **Purpose**: Extension methods for Microsoft.EntityFrameworkCore.ChangeTracking.PropertyEntry
- **Key Functionality**:
  - `GetNewValue()`: Extracts the current/new value from a property entry
  - `GetOriginalValue()`: Retrieves the original value before changes
  - Handles both standard and soft-deleted entities

### Key Components

**PropertyEntryExtensions Class**
- Static extension methods for PropertyEntry
- Supports audit trail generation by capturing before/after values
- Special handling for soft-deleted entities using IsDeleted() extension

### Dependencies
- Microsoft.EntityFrameworkCore.ChangeTracking
- Abp.EntityHistory.Extensions (for soft deletion support)

## Architecture Notes

**Extension Methods Pattern**
- Extends EF Core's change tracking capabilities
- Provides cleaner API for audit history implementations
- Encapsulates complex logic for value extraction

**Soft Delete Support**
- Integrates with ABP's soft delete pattern
- Maintains original values for deleted entities
- Preserves audit trail integrity

## Business Logic
- When an entity is soft-deleted, original values are preserved
- New values are captured for standard updates
- Provides foundation for comprehensive audit logging

## Usage Across Codebase
These extensions are primarily used by:
- `SurpathEntityHistoryHelper` for creating entity property changes
- Audit trail generation during SaveChanges operations
- Entity history tracking mechanisms