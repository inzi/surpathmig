# Entity History Documentation

## Overview
Audit trail and change tracking system for entities. Records all modifications with before/after values, user context, and timestamps.

## Contents

### Files

#### EntityHistoryHelper.cs / IMyEntityHistoryHelper.cs / MyEntityHistoryHelper.cs
- **Purpose**: Utilities for entity history operations
- **Key Features**:
  - Configure tracked entities
  - PII/sensitive data masking
  - Change comparison logic
  - History query helpers

#### EntityHistoryConfigProvider.cs
- **Purpose**: Configuration of which entities to track
- **Features**:
  - Entity registration for tracking
  - Property-level tracking control
  - Exclusion of sensitive properties

#### EntityHistoryUiSetting.cs
- **Purpose**: UI settings for history display
- **Features**: Presentation configuration for audit UI

### Key Components

- **EntityHistoryHelper**: History operations
- **EntityHistoryConfigProvider**: Tracking configuration
- **EntityHistoryUiSetting**: UI presentation

### Dependencies

- **External Libraries**:
  - ABP Framework (entity history module)
  - Abp.EntityHistory

- **Internal Dependencies**:
  - All tracked entities
  - User system (for audit context)

## Architecture Notes

- **Pattern**: Interceptor pattern (ABP entity change tracking)
- **Storage**: Separate audit tables
- **Performance**: Async background processing
- **Flexibility**: Configurable per entity

## Business Logic

### Change Tracking
- Insert operations logged
- Update operations show before/after
- Delete operations recorded
- Property-level changes
- Related entity tracking

### Audit Context
- User who made change
- Timestamp
- Tenant context
- IP address (optional)
- Browser info (optional)

### Sensitive Data Handling
- PII masking in history
- Password exclusion
- Credit card redaction
- Configurable per property

## Usage Across Codebase

Used by:
- Audit log UI
- Compliance reporting
- Change history views
- Admin oversight
- Debugging
- Regulatory compliance

## Security Considerations

- Sensitive data masked
- Access control on history viewing
- Immutable history records
- Tenant isolation
- GDPR compliance support

## Extension Points

- Custom audit processors
- Additional context data
- Audit retention policies
- Export capabilities
- Advanced filtering