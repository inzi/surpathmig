# Common DTOs Documentation

## Overview
This folder contains shared Data Transfer Objects (DTOs) used across multiple features of the application. These DTOs provide common functionality like user lookup and edition retrieval that are needed by various parts of the system.

## Contents

### Files

- **FindUsersInput.cs** - User search/lookup parameters:
  - Filter - Text search term
  - TenantId - Optional tenant restriction (for multi-tenant searches)
  - ExcludeCurrentUser - Flag to exclude logged-in user from results
  - Extends PagedAndFilteredInputDto for paging support
  - Used in dropdowns, autocompletes, and user selection widgets

- **GetDefaultEditionNameOutput.cs** - Returns default SaaS edition:
  - Edition name for tenant registration defaults
  - Used during tenant creation process

### Key Components

#### User Lookup Pattern
FindUsersInput supports common scenarios:
- **Autocomplete Fields**: Type-ahead user selection
- **User Assignment**: Selecting users for roles, tasks, approvals
- **Chat Contacts**: Finding users to start conversations
- **Cross-Tenant**: Optional TenantId for host searching all tenants

#### Edition Defaults
GetDefaultEditionNameOutput provides:
- Default edition for new tenant signups
- Fallback edition when none specified
- Edition-based feature restrictions

### Dependencies
- **inzibackend.Dto** - PagedAndFilteredInputDto base class

## Architecture Notes

### Reusability Pattern
- **Common DTOs**: Shared across multiple service interfaces
- **Composition**: Used as nested objects in larger DTOs
- **Standardization**: Consistent user lookup behavior everywhere

### Multi-Tenant Considerations
- FindUsersInput TenantId parameter:
  - null = current tenant users
  - specified = specific tenant users
  - Enables host to search across all tenants

### Filtering Strategy
- Text filter searches across user fields (Name, Surname, Username, Email)
- ExcludeCurrentUser prevents self-assignment
- Paging prevents performance issues with large user lists

## Business Logic

### User Lookup Use Cases
1. **Task Assignment**: Finding users to assign work items
2. **Role Management**: Selecting users for role membership
3. **Chat**: Searching for users to message
4. **Impersonation**: Admin selecting user to impersonate
5. **Delegation**: Choosing delegate for temporary access
6. **Reports**: Filtering reports by user
7. **Auditing**: Looking up users in audit logs

### Edition Defaults
- New tenant registration uses default edition
- Provides baseline feature set
- Can be upgraded/downgraded after creation

### ExcludeCurrentUser Logic
Useful when:
- Assigning tasks (can't assign to self)
- Delegating access (can't delegate to self)
- Impersonation (can't impersonate self)
- Prevents illogical operations

## Usage Across Codebase
These DTOs are consumed by:
- **ICommonLookupAppService** - Common lookup operations
- **User Selection Components** - Dropdowns and autocompletes throughout UI
- **Assignment Workflows** - Task, role, and permission assignment
- **Chat Features** - User search for new conversations
- **Admin Tools** - User selection in various admin interfaces
- **Tenant Registration** - Edition default retrieval
- **Mobile Apps** - User search and selection

## Cross-Reference Impact
Changes to these DTOs affect:
- All user selection interfaces across the application
- Autocomplete components
- User assignment workflows
- Chat contact search
- Admin user selection tools
- Tenant registration process
- Any feature using common user lookup