# MetaData Documentation

## Overview
Metadata models for tracking notification states and timing for compliance-related warnings and expiration notifications. Provides a structured approach to managing multi-stage notification workflows.

## Contents

### Files

#### MetaData.cs
- **Purpose**: Container class for metadata objects
- **Key Components**:
  - MetaDataObject: Wrapper class containing notification metadata
  - Initializes with default MetaDataNotifications instance

#### MetaDataNotifications.cs
- **Purpose**: Tracks notification timing and state for compliance warnings
- **Key Properties**:
  - WarnDaysBeforeFirst/WarnedDaysBeforeFirst: First warning tracking
  - WarnDaysBeforeSecond/WarnedDaysBeforeSecond: Second warning tracking
  - WarnDaysBeforeFinal/WarnedDaysBeforeFinal: Final warning tracking
  - ExpiredNotification/ExpiredNotificationSent: Expiration notification tracking
- **Default Values**:
  - All dates initialized to DateTime.MaxValue
  - All warning flags initialized to false

### Key Components

- **MetaDataObject**: Root metadata container
- **MetaDataNotifications**: Notification state tracking

### Dependencies

- **External Libraries**: None (uses standard .NET types)

- **Internal Dependencies**: None

## Architecture Notes

- **Pattern**: Data Transfer Object (DTO) pattern
- **State Management**: Tracks both timing and completion of notifications
- **Extensibility**: MetaDataObject can be extended with additional metadata types

## Business Logic

### Notification Workflow
1. **First Warning**: Initial notification before expiration
2. **Second Warning**: Follow-up reminder
3. **Final Warning**: Last chance notification
4. **Expiration**: Post-expiration notification

### State Tracking
- Each stage has both a datetime (when to warn) and a boolean (was warning sent)
- DateTime.MaxValue used as "not set" indicator
- Prevents duplicate notifications through boolean flags

## Usage Across Codebase

This metadata system is likely used for:
- Compliance document expiration warnings
- Certification renewal reminders
- Drug test scheduling notifications
- Background check expiration alerts
- Any time-sensitive compliance tracking

## Implementation Considerations

- Serializable for storage in JSON metadata fields
- Lightweight for inclusion in larger entities
- Clear separation between timing and state
- Supports multi-stage notification campaigns