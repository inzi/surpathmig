# Compliance Background Jobs Documentation

## Overview
Background worker service that automatically monitors and processes compliance record expirations and warnings. Runs every 12 hours to identify expiring documents, update their status, and send notifications to affected users, ensuring proactive compliance management.

## Contents

### Files

#### ComplianceExpireBackgoundService.cs
- **Purpose**: Periodic background worker for compliance expiration monitoring and notification
- **Key Features**:
  - Runs every 12 hours automatically
  - Executes on application startup
  - Multi-tenant aware (processes all tenants)
  - Three-tier warning system (first, second, final)
  - Automatic status changes on expiration
  - Email and in-app notifications
  - Metadata tracking to prevent duplicate notifications
- **Configuration**:
  - Check Period: 12 hours (43,200,000 milliseconds)
  - Run on Start: Yes
  - Feature-gated: Can be enabled/disabled via `IsEnabled` property
- **Key Methods**:
  - `DoWork()`: Main execution method processing all warn rules
  - `Expired()`: Handles records that have passed expiration date
  - `Warn()`: Sends warning notifications before expiration
  - `GetRecordsFor Expired/Warning()`: Query methods for finding affected records
  - `StateGhangeNotify()`: Sends notifications to users
  - `UpdateRecordMetadata()`: Tracks notification history
- **Dependencies**: 30+ injected services including repositories, managers, notification systems

### Key Components

**Expiration Processing:**
1. Queries all record category rules with notifications enabled
2. Extracts warning day thresholds (first, second, final)
3. Processes warnings for each tenant/rule combination
4. Processes expirations for each tenant/rule combination
5. Updates record statuses based on configured rules
6. Sends notifications via email and app notifier

**Warning Tiers:**
- **First Warning**: Earliest warning (e.g., 30 days before expiration)
- **Second Warning**: Middle warning (e.g., 14 days before expiration)
- **Final Warning**: Last warning before expiration (e.g., 7 days before)
- Each tier has configurable status change and notification

**Metadata Tracking:**
Prevents duplicate notifications by tracking:
- `WarnedDaysBeforeFirst` / `WarnDaysBeforeFirst`
- `WarnedDaysBeforeSecond` / `WarnDaysBeforeSecond`
- `WarnedDaysBeforeFinal` / `WarnDaysBeforeFinal`
- `ExpiredNotificationSent` / `ExpiredNotification`

### Dependencies
- **External**:
  - ABP Framework (Background Workers, UnitOfWork, Logging)
  - Entity Framework Core (queries)
  - Microsoft.Extensions.Logging
  - Newtonsoft.Json (metadata serialization)
  - AutoMapper (DTO mapping)
- **Internal**:
  - 15+ domain entities (UserPid, Cohort, CohortUser, Record, RecordState, etc.)
  - `IAppNotifier`: In-app notification system
  - `IUserEmailer`: Email notification service
  - `IAppUrlService`: URL generation for email links
  - `IFeatureChecker`: Feature flag validation
  - Compliance manager components

## Architecture Notes
- **Pattern**: Periodic Background Worker with Unit of Work per tenant
- **Lifetime**: Singleton - one worker instance for the application
- **Multi-Tenancy**: Disables tenant filter to process all tenants
- **Thread Safety**: ABP framework handles concurrency
- **Transaction Management**: Nested Unit of Work for each record update
- **Error Handling**: Try-catch with logging, continues processing on error

## Business Logic

### Expiration Detection Flow
1. Query `RecordState` entities with:
   - Compliant status
   - Expiration date in the past
   - Rule has `Expires = true`
   - Not archived
2. Check metadata to see if expiration notification already sent
3. Determine target status (rule's ExpiredStatusId or tenant default)
4. Update record status to expired status
5. Send notification to user
6. Update metadata to prevent duplicate notifications

### Warning Detection Flow
1. Query `RecordState` entities with:
   - Compliant status
   - Expiration date between now and warning threshold
   - Rule has `Expires = true`
   - Not archived
2. Calculate days until expiration
3. Check which warning tier applies (final > second > first)
4. Verify warning not already sent (check metadata)
5. Update record status to warning status (if configured)
6. Send warning notification
7. Update metadata with warning timestamps
8. Mark all previous warning tiers as sent

### Notification Types
Localized notification names based on warning level:
- `AppNotificationNames.RecordStateExpirationFirstWarning`
- `AppNotificationNames.RecordStateExpirationSecondWarning`
- `AppNotificationNames.RecordStateExpirationFinalWarning`
- `AppNotificationNames.RecordStateExpirationExpired`

### Email Integration
For rejected record states:
- Generates cohort user view URL
- Sends compliance-related email to user
- Includes direct link to view compliance status

## Usage Across Codebase

### Registered By
- ABP background worker manager
- Automatically started on application initialization
- Registered as singleton dependency

### Interacts With
- **Record Management**: Updates RecordState status
- **Notification System**: Sends in-app and email notifications
- **Metadata System**: Tracks notification history in Record.metadata
- **Compliance Evaluator**: May trigger recompilation of compliance status
- **User Management**: Retrieves user and cohort user information

### Related Components
- `RecordCategoryRule`: Defines expiration rules and warning thresholds
- `RecordStatus`: Target statuses for warnings and expiration
- `MetaDataObject`: Notification tracking structure
- `IAppNotifier`: Notification delivery
- `IUserEmailer`: Email delivery

## Configuration

### WarnRuleDays Helper Class
```csharp
public class WarnRuleDays
{
    public Guid RecordCategoryRuleId { get; set; }
    public int TenantId { get; set; }
    public int WarnDaysBeforeFirst { get; set; }
    public int WarnDaysBeforeSecond { get; set; }
    public int WarnDaysBeforeFinal { get; set; }
}
```

### Record Category Rule Settings
- `Notify`: Enable/disable notifications for this rule
- `Expires`: Enable/disable expiration tracking
- `WarnDaysBeforeFirst`: Days before expiration for first warning
- `WarnDaysBeforeSecond`: Days before expiration for second warning
- `WarnDaysBeforeFinal`: Days before expiration for final warning
- `FirstWarnStatusId`: Status to set on first warning
- `SecondWarnStatusId`: Status to set on second warning
- `FinalWarnStatusId`: Status to set on final warning
- `ExpiredStatusId`: Status to set when expired

## Performance Considerations
- **Execution Frequency**: 12 hours balances responsiveness with system load
- **Query Optimization**: AsNoTracking() for read-only queries
- **Batch Processing**: Processes all tenants in single execution
- **Nested Transactions**: Fresh Unit of Work for each update
- **Error Isolation**: Exceptions don't stop processing of other records
- **Logging**: Comprehensive logging for monitoring and debugging

## Security Considerations
- **Tenant Isolation**: Temporarily disables filter to access all tenants
- **Notification Privacy**: Only notifies the record owner
- **Audit Trail**: All status changes logged via ABP audit system
- **Permission-less Execution**: Runs with system privileges (background job)

## Monitoring and Debugging
### Log Messages
- Start of execution with timestamp
- Progress counters (X/Y processing)
- Warning/expiration events with record IDs
- Notification deliveries with user information
- Critical errors for missing default statuses

### Common Issues
- **Missing Default Status**: Logs CRITICAL error if tenant has no default record status
- **Already Notified**: Skips records with notification already sent (check metadata)
- **Archived Records**: Intentionally skipped (not processed)

## Best Practices
- **Monitor Logs**: Check for CRITICAL messages about missing statuses
- **Configure Warning Thresholds**: Set appropriate day values per requirement type
- **Test Notifications**: Verify emails and in-app notifications are delivered
- **Review Metadata**: Ensure notification tracking is working correctly
- **Performance**: Monitor execution time as record count grows

## Extension Points
- Add additional warning tiers
- Customize notification messages per tenant
- Implement escalation workflows
- Add dashboard for expiring documents
- Create reports on notification effectiveness