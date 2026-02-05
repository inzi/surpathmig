# User Delegation DTOs Documentation

## Overview
This folder contains Data Transfer Objects (DTOs) for the user delegation system. Delegation allows users to temporarily grant their access rights to another user for a specific time period. This is useful for scenarios like vacation coverage, temporary role assignments, or workflow approvals when a user is unavailable.

## Contents

### Files

- **UserDelegationDto.cs** - Active delegation record:
  - Username - The delegate user who receives temporary access
  - StartTime - When delegation becomes active
  - EndTime - When delegation expires
  - Inherits Id from EntityDto<long> for tracking

- **DelegateNewUserInput.cs** - Create new delegation request:
  - Target user to receive delegation
  - Time range for delegation validity
  - Validation for user existence and time range

- **GetUserDelegationsInput.cs** - Query parameters for listing delegations:
  - Filter by active/expired/upcoming
  - Paging and sorting support

### Key Components

#### Delegation Workflow
1. User creates delegation to another user with time range
2. During delegation period, delegate can act on behalf of original user
3. Delegation automatically expires after EndTime
4. Audit trail tracks actions performed under delegation

### Dependencies
- **Abp.Application.Services.Dto** - Base DTO classes
- Date/Time - For temporal delegation windows

## Architecture Notes

### Temporal Design
- **Time-Bounded**: All delegations have explicit start and end times
- **Automatic Expiration**: System enforces time bounds without manual intervention
- **Flexible Scheduling**: Can create future delegations that activate later

### Security Considerations
- Delegates receive full access of original user during delegation period
- All actions under delegation are audited with impersonation tracking
- Original user retains their access (doesn't lose permissions)
- Delegation cannot be extended beyond original EndTime

### Multi-Tenancy
- Delegations are tenant-isolated
- Cannot delegate to users in different tenants
- Each tenant manages its own delegation rules

## Business Logic

### Delegation Use Cases
1. **Vacation Coverage**: Temporarily grant access while out of office
2. **Workload Distribution**: Share responsibilities during busy periods
3. **Training**: Allow trainer to act with trainee's account for demonstrations
4. **Emergency Access**: Quick temporary access without role changes

### Time Range Validation
- EndTime must be after StartTime
- Cannot create delegations entirely in the past
- Overlapping delegations to same user are allowed

### Delegation vs Impersonation
- **Delegation**: User-initiated, time-bounded, for peer collaboration
- **Impersonation**: Admin-initiated, for support and troubleshooting
- Both tracked separately in audit logs

## Usage Across Codebase
These DTOs are consumed by:
- **IUserDelegationAppService** - Delegation CRUD operations
- **User Profile** - Self-service delegation management
- **Authorization System** - Runtime delegation checking
- **Audit Services** - Tracking delegated actions
- **Notification System** - Alerting users about delegations

## Cross-Reference Impact
Changes to these DTOs affect:
- User delegation management interfaces
- Authorization middleware (delegation checking)
- Audit log interpretation
- Time-based access control systems
- User profile management screens