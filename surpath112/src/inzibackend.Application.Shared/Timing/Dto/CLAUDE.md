# Timing DTOs Documentation

## Overview
This folder contains Data Transfer Objects (DTOs) for timezone management and time-related functionality. These DTOs enable users to select their timezone preferences and ensure proper time display across different timezones in the multi-tenant application.

## Contents

### Files

- **GetTimezonesInput.cs** - Query available timezones:
  - DefaultTimezoneScope - User, tenant, or application level
  - Filter parameters for timezone search

- **GetTimezoneComboboxItemsInput.cs** - Timezone dropdown options:
  - DefaultTimezoneScope - Determines default selection
  - SelectedTimezoneId - Currently selected timezone
  - Returns timezone list formatted for combobox/dropdown

### Key Components

#### Timezone Scopes
- Application - System default timezone
- Tenant - Tenant's default timezone
- User - User's personal timezone preference

#### Timezone Management
- List all available IANA timezones
- Map Windows timezones to IANA
- Display timezone with UTC offset
- Handle daylight saving time transitions

### Dependencies
- **Abp.Timing** - ABP timezone infrastructure
- **System.TimeZoneInfo** - .NET timezone support

## Architecture Notes

### Timezone Handling
- Server stores all times in UTC
- Convert to user's timezone for display
- Convert from user's timezone on input
- Audit logs show time in UTC

### Timezone Inheritance
1. User timezone (highest priority)
2. Tenant default timezone
3. Application default timezone
4. UTC (fallback)

### Display Format
Timezones shown as:
- "(UTC-05:00) Eastern Time (US & Canada)"
- "(UTC+00:00) Dublin, Edinburgh, Lisbon, London"
- Includes UTC offset for clarity

## Business Logic

### Timezone Selection Workflow
1. User opens profile settings
2. System calls GetTimezoneComboboxItemsInput
3. Returns timezone list with current selection highlighted
4. User selects new timezone
5. All future times displayed in selected timezone

### Time Display
- Created dates in user's timezone
- Requirement deadlines adjusted for timezone
- Notification times timezone-aware
- Scheduled events shown in local time

### Timezone Use Cases
- User in California sees times in PT
- User in New York sees times in ET
- Expiration dates consistent regardless of viewer timezone
- Email notifications use recipient's timezone

## Usage Across Codebase
These DTOs are consumed by:
- **ITimingAppService** - Timezone operations
- **User Profile Settings** - Timezone selection
- **Date Display Components** - Convert UTC to user timezone
- **Scheduling Services** - Timezone-aware scheduling
- **Email Services** - Timezone in email timestamps

## Cross-Reference Impact
Changes affect:
- Timezone selection dropdowns
- Time display throughout application
- Scheduled notification timing
- Report timestamp displays
- Email timestamp formatting