# User Password Management Documentation

## Overview
Background worker service that automatically enforces password expiration policies. Runs daily to identify users with expired passwords and force them to change their password on next login, enhancing security compliance.

## Contents

### Files

#### PasswordExpirationBackgroundWorker.cs
- **Purpose**: Periodic background task that enforces password expiration policies
- **Key Features**:
  - Runs automatically every 24 hours
  - Executes on application startup
  - Uses Unit of Work for transactional consistency
  - Singleton dependency (one instance per application)
- **Configuration**:
  - Check Period: 24 hours (86,400,000 milliseconds)
  - Run on Start: Yes (executes immediately when app starts)
  - Timer-based scheduling via ABP framework
- **Dependencies**:
  - `IUnitOfWorkManager`: Ensures database operations are transactional
  - `IPasswordExpirationService`: Domain service implementing expiration logic
  - `AbpTimer`: Framework timer for periodic execution
- **Inheritance**:
  - `PeriodicBackgroundWorkerBase`: ABP base class for scheduled tasks
  - `ISingletonDependency`: Ensures single instance lifetime

### Key Components
**Background Processing:**
- Scheduled task running every 24 hours
- Transactional password expiration enforcement
- Automatic retry on application restart

**Password Expiration Flow:**
1. Timer triggers every 24 hours
2. Worker creates Unit of Work transaction
3. Calls `IPasswordExpirationService.ForcePasswordExpiredUsersToChangeTheirPassword()`
4. Service identifies users with expired passwords based on policy
5. Sets flags forcing password change on next login
6. Transaction commits

### Dependencies
- **External**:
  - `Abp.Threading.BackgroundWorkers`: Background worker infrastructure
  - `Abp.Threading.Timers`: Timer management
  - `Abp.Domain.Uow`: Unit of Work pattern
  - `Abp.Dependency`: Dependency injection
- **Internal**:
  - `IPasswordExpirationService`: (Defined in Core layer)
  - `inzibackendConsts.LocalizationSourceName`: Localization configuration

## Architecture Notes
- **Pattern**: Background Worker pattern with periodic execution
- **Lifetime**: Singleton - one worker instance for the application
- **Thread Safety**: ABP framework handles thread safety for background workers
- **Startup Behavior**: Executes immediately on app start, then every 24 hours
- **Transaction Management**: Wraps work in Unit of Work for database consistency

## Business Logic
### Password Expiration Rules
The worker itself doesn't implement policy logic - it delegates to `IPasswordExpirationService`, which:
- Checks password age against tenant/host settings
- Identifies users whose passwords have exceeded max age
- Sets `ShouldChangePasswordOnNextLogin` flag for expired users
- May send notifications about password expiration

### Execution Frequency Rationale
- **24 Hour Interval**: Balances responsiveness with system load
- **Daily Checks**: Adequate for typical password policies (30-90 day expiration)
- **Startup Execution**: Ensures no gaps if application was offline

### Error Handling
- Framework handles exceptions in background workers
- Failed executions logged automatically
- Worker continues running on subsequent intervals even if one execution fails

## Usage Across Codebase
### Registered By
- ABP background worker manager
- Automatically started when application initializes
- Registered in application startup/module initialization

### Interacts With
- **IPasswordExpirationService**: Core domain service (src/inzibackend.Core/Authorization/Users/Password/)
- **User Repository**: Via password expiration service
- **Notification System**: May trigger password expiration notifications

### Related Components
- Password expiration domain service
- User login logic (checks ShouldChangePasswordOnNextLogin flag)
- Password change workflows
- Security settings management

## Security Considerations
- **Automated Enforcement**: Ensures password policies are consistently applied
- **No Manual Intervention**: Reduces risk of forgotten expirations
- **Login-time Enforcement**: Users forced to change password at next authentication
- **Audit Trail**: Password changes logged via standard audit system

## Performance Considerations
- **Low Impact**: Runs only once per day
- **Transactional**: Uses Unit of Work to avoid partial updates
- **Efficient Query**: Service should query only users with old passwords
- **Off-Peak Execution**: Can be configured to run during low-traffic periods if needed

## Configuration Options
### Potential Customization Points
- **Check Period**: Currently 24 hours, adjustable via constant
- **Run On Start**: Currently enabled, can be disabled
- **Expiration Policy**: Configured in IPasswordExpirationService implementation
- **Notification Settings**: Managed separately in notification system

## Best Practices
- **Don't Modify Timer Period Without Consideration**: 24 hours is standard for password policies
- **Monitor Execution**: Check logs to ensure worker runs successfully
- **Test Password Expiration**: Verify users are actually forced to change passwords
- **Coordinate with Login Logic**: Ensure login process checks and enforces password change flags