# User Delegation Service Documentation

## Overview
Application service managing user delegation functionality, allowing users to temporarily delegate their permissions and responsibilities to other users. Commonly used for vacation coverage, temporary role transfers, and permission sharing scenarios.

## Contents

### Files

#### UserDelegationAppService.cs
- **Purpose**: Complete application service for user delegation management
- **Authorization**: `[AbpAuthorize]` - Requires authentication
- **Key Features**:
  - Create new delegations with date ranges
  - List delegated users (who current user has delegated to)
  - List active delegations (who has delegated to current user)
  - Remove delegations
  - Validates delegation configuration is enabled
  - Prevents delegation during impersonation
  - Prevents self-delegation
- **Key Methods**:
  - `GetDelegatedUsers()`: Paginated list of users the current user has delegated permissions to
  - `DelegateNewUser()`: Create new delegation to another user
  - `RemoveDelegation()`: Remove an existing delegation
  - `GetActiveUserDelegations()`: Get active delegations where current user is the delegate (someone delegated to you)
- **Dependencies**:
  - `IRepository<UserDelegation, long>`: Delegation data access
  - `IRepository<User, long>`: User data access
  - `IUserDelegationManager`: Domain logic for delegation management
  - `IUserDelegationConfiguration`: Delegation feature configuration
- **Security Checks**:
  - Prevents delegation when user is impersonating another user
  - Validates delegation feature is enabled in configuration
  - Prevents users from delegating to themselves

### Key Components
**Delegation Operations:**
- Create delegation with start/end times
- Query delegations by source or target user
- Filter active delegations by current time
- Join with user repository for display names

**Query Building:**
- `CreateDelegatedUsersQuery()`: Build query for delegations created by a user
- `CreateActiveUserDelegationsQuery()`: Build query for delegations targeting a user
- Supports sorting and filtering
- Returns DTOs with username and time ranges

### Dependencies
- **External**:
  - ABP Framework (Repository, Session, Authorization)
  - Entity Framework Core (queries)
  - System.Linq.Dynamic.Core (dynamic sorting)
- **Internal**:
  - `UserDelegation` entity (Core.Authorization.Delegation)
  - `User` entity (Core.Authorization.Users)
  - `IUserDelegationManager` (Core domain service)
  - `IUserDelegationConfiguration` (Core configuration)
  - DTOs from Authorization.Users.Delegation.Dto namespace

## Architecture Notes
- **Pattern**: Application Service with repository and domain service coordination
- **Authorization**: Method-level authorization via attributes
- **Pagination**: Supports paging for large delegation lists
- **Sorting**: Dynamic sorting via LINQ expressions
- **Time-based Filtering**: Filters by current time for active delegations

## Business Logic
### Delegation Creation
1. Validate user is not delegating to themselves
2. Check delegation feature is enabled
3. Verify user is not impersonating
4. Create delegation record with:
   - Source user: Current user
   - Target user: Specified user
   - Start/End time: Specified date range
   - Tenant ID: Current tenant

### Active Delegation Detection
- Query delegations where TargetUserId = CurrentUser
- Filter by EndTime >= Clock.Now
- Returns users who have delegated authority to you

### Delegation Removal
- Delegates to `IUserDelegationManager` for business rules
- Validates ownership and permissions
- Removes delegation record

### Impersonation Protection
Delegation operations are blocked during impersonation to prevent:
- Delegating as another user
- Creating fake delegation chains
- Security vulnerabilities in permission systems

## Usage Across Codebase
### Primary Consumers
- User management UI (delegation tab)
- Permission checking system
- Authorization handlers

### Related Systems
- **IUserDelegationManager**: Enforces delegation business rules
- **Authorization System**: Checks active delegations during permission evaluation
- **User Interface**: Delegation management screens

### Typical Workflow
1. User A navigates to delegation settings
2. User A creates delegation to User B with date range
3. During delegation period:
   - User B can act with User A's permissions
   - System tracks actions are performed via delegation
4. After end date, delegation automatically becomes inactive
5. Either user can manually remove delegation early

## Security Considerations
- **Self-Delegation Prevention**: Users cannot delegate to themselves
- **Impersonation Check**: Blocks delegation during admin impersonation
- **Configuration Gate**: Feature can be globally disabled
- **Tenant Isolation**: Delegations scoped to current tenant
- **Time-based Control**: Automatic expiration via end time
- **Audit Trail**: All delegation operations logged via ABP framework

## Configuration
### Delegation Enable/Disable
Controlled via `IUserDelegationConfiguration.IsEnabled`:
- Can be toggled per tenant or globally
- Prevents all delegation operations when disabled
- Existing delegations become inactive when feature disabled

## Best Practices
- **Set Reasonable Date Ranges**: Avoid overly long delegations
- **Review Active Delegations**: Periodically audit who has delegation authority
- **Remove When Complete**: Delete delegations when no longer needed
- **Monitor Usage**: Track delegation usage for security compliance
- **Test Permission Scope**: Ensure delegates only get intended permissions

## Extension Points
- Add notification when delegation is created/removed
- Implement approval workflow for sensitive delegations
- Add audit reporting for delegation usage
- Restrict delegation by role or permission level