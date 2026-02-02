# EntityHistory Documentation

## Overview
Custom entity history tracking implementation for the Surpath application. Extends ABP's entity history framework to provide specialized audit logging with PII masking capabilities and comprehensive change tracking for all database operations.

## Contents

### Files

#### SurpathEntityHistoryHelper.cs
- **Purpose**: Core implementation of entity history tracking with custom PII masking
- **Key Functionality**:
  - Creates comprehensive change sets for all entity modifications
  - Masks sensitive PID (Personal Identification) data based on PidType configuration
  - Tracks foreign key relationships and navigation properties
  - Handles owned entities and complex type changes
  - Provides both synchronous and asynchronous save operations

### Key Components

**SurpathEntityHistoryHelper Class**
- Inherits from `EntityHistoryHelperBase`
- Implements `IEntityHistoryHelper` interface
- Key methods:
  - `CreateEntityChangeSet()`: Builds comprehensive change tracking
  - `DoMask()`: Applies PII masking to sensitive data
  - `UpdateChangeSet()`: Post-processes changes after SaveChanges
  - `GetPropertyChanges()`: Extracts all property modifications

**PII Masking Logic**
- Automatically detects UserPid entities
- Checks PidType.MaskPid configuration
- Applies regex-based masking: replaces all but last 4 characters with asterisks
- Preserves JSON formatting in masked values

### Dependencies
- Abp.EntityHistory framework
- Microsoft.EntityFrameworkCore
- Entity change tracking infrastructure
- PidType configuration system

## Subfolders

### Extensions
Extension methods for Entity Framework change tracking that support the entity history system. [See Extensions documentation](Extensions/CLAUDE.md)

## Architecture Notes

**Change Tracking Pattern**
- Hooks into EF Core's ChangeTracker
- Processes changes before and after SaveChanges
- Maintains separate transaction scope for history storage

**Audit Trail Design**
- Captures user context (including impersonation)
- Records browser info and IP addresses
- Tracks creation time based on entity type
- Preserves full property change history

**Performance Optimizations**
- Skips unchanged/detached entities
- Filters out non-audited properties
- Uses suppressed transaction scope for history saves

## Business Logic

**Security & Compliance**
- PII data masking for sensitive identifiers
- Maintains audit trail for regulatory compliance
- Tracks all data modifications with user attribution

**Data Integrity**
- Foreign key relationship tracking
- Shadow property support
- Owned entity change detection

**Masking Rules**
- Applied to UserPid.Pid field when PidType.MaskPid is true
- Preserves last 4 characters for partial identification
- Maintains JSON structure in stored values

## Usage Across Codebase

**Direct Integration Points**
- Registered in `inzibackendEntityFrameworkCoreModule` during initialization
- Called automatically by `inzibackendDbContext.SaveChanges()`
- Configured via EntityHistory.EntityHistoryHelper.TrackedTypes

**Tracked Entity Types**
- All entities implementing auditing interfaces
- Specifically configured entity types
- Owned types when parent is audited

**Configuration**
- Enabled/disabled via Configuration.EntityHistory.IsEnabled
- Entity selection via Configuration.EntityHistory.Selectors
- Custom configuration provider: EntityHistoryConfigProvider