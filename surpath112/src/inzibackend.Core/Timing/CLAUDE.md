# Timing Documentation

## Overview
Time zone management and conversion utilities for handling dates and times across different user time zones in a multi-tenant environment.

## Contents

### Files

#### TimeZoneService.cs
- **Purpose**: Service for time zone operations
- **Key Features**:
  - Get user's time zone
  - Convert UTC to user local time
  - Convert user local time to UTC
  - Time zone list
  - Default time zone handling

#### ITimeZoneService.cs
- **Purpose**: Interface for time zone operations

### Key Components

- **TimeZoneService**: Time zone conversion and management

### Dependencies

- **External Libraries**:
  - ABP Framework (timing module)
  - System.TimeZoneInfo

- **Internal Dependencies**:
  - User settings
  - Tenant settings
  - Configuration system

## Architecture Notes

- **Pattern**: Service pattern
- **Storage**: UTC in database, convert for display
- **User Preference**: Stored in user profile
- **Default**: System or tenant default time zone

## Business Logic

### Time Zone Handling

#### Storage
- All times stored as UTC in database
- No time zone information in database
- Ensures consistency across all operations

#### Display
- Convert UTC to user's time zone
- Format according to culture
- Show time zone abbreviation
- Relative times (e.g., "2 hours ago")

#### Input
- User inputs in their local time
- Convert to UTC before saving
- Validate date ranges
- Handle DST transitions

### Time Zone Selection Priority
1. User preference (profile setting)
2. Tenant default time zone
3. Application default (UTC or system)

### Common Time Zone Operations
- Get current time in user's zone
- Convert appointment times
- Schedule recurring events
- Calculate expiration times
- Display activity timestamps

## Usage Across Codebase

Used by:
- UI date/time display
- Appointment scheduling
- Report generation (timestamps)
- Notification scheduling
- Activity logs
- Document expiration
- Compliance deadlines
- Subscription renewals

## Best Practices

### Always Use UTC Internally
- Database: UTC only
- Business logic: UTC
- API contracts: UTC (ISO 8601)
- Display layer: Convert to user time zone

### Handle DST Transitions
- Recurring events across DST changes
- Ambiguous times (fall back)
- Missing times (spring forward)
- Time zone rule changes

### User Experience
- Show time zone in UI
- Allow user to change time zone
- Relative times for recent events
- Clear date/time formats

## Extension Points

- Business hours per time zone
- Holiday calendars
- Working day calculations
- Time zone abbreviations
- Custom time zone groups
- Scheduling constraints