# Views/Shared/Components/TenantChange Documentation

## Overview
This View Component provides a tenant switcher interface for users with access to multiple tenants, allowing them to switch between tenant contexts without re-authenticating.

## Files

### TenantChangeViewComponent.cs
**Purpose**: View Component class for tenant switching

**Key Features**:
- Retrieves current login information including tenant
- Maps login info to view model
- Displays current tenant context

**Dependencies**:
- `IPerRequestSessionCache`: Current session information
- `ObjectMapper`: ABP auto-mapper
- Base class: `inzibackendViewComponent`

**Method**:
- `InvokeAsync()`: Returns view with current tenant information

### TenantChangeViewModel.cs
**Purpose**: View model for tenant switcher

**Properties**:
- `Tenant`: Current tenant information (null for host)
  - `TenancyName`: Unique tenant identifier
  - `Name`: Display name of tenant
- `HasTenantContext`: Whether user is in a tenant context

### Default.cshtml
**Purpose**: Razor view for tenant switcher UI

**UI Elements**:
- Current tenant display
  - Shows "Not Selected" if in host context
  - Shows tenant tenancy name if in tenant context
- "Change" link to open tenant selection modal
- CSS class: `tenant-change-component`

**Behavior**:
- Displays current tenant prominently
- Click "Change" opens modal (`_ChangeModal.cshtml`)
- Modal allows selecting different tenant or host context

### _ChangeModal.cshtml
**Purpose**: Modal dialog for tenant selection

**UI Elements**:
- Tenant selection dropdown or list
- Option to switch to host context (if permitted)
- Confirmation button
- Cancel button

**Behavior**:
1. User clicks "Change" link
2. Modal opens with available tenants
3. User selects new tenant or host
4. Application switches context
5. Page refreshes with new tenant context
6. All subsequent requests use new tenant

### Default.js
**Purpose**: Client-side JavaScript for tenant switching

**Functionality**:
- Opens/closes modal
- Handles tenant selection
- Makes AJAX call to switch tenant
- Refreshes page after successful switch
- Error handling for failed switches

## Usage

### Invocation
Called from layout pages (typically in header):
```razor
@await Component.InvokeAsync(typeof(TenantChangeViewComponent))
```

### User Flow
1. User sees current tenant in header
2. Clicks "Change" link
3. Selects new tenant from modal
4. Application context switches
5. Page refreshes showing new tenant's data

### Permissions
- Only shown if user has access to multiple tenants
- May require special permission to switch contexts
- Host users can switch to any tenant
- Tenant users only see tenants they have access to

## Business Logic

### Tenant Switching
- Uses ABP's tenant resolution system
- Sets tenant ID in session
- All subsequent database queries filtered by new tenant
- Multi-tenant data isolation maintained

### Security
- Validates user has permission for target tenant
- Prevents unauthorized tenant access
- Logs tenant switching for audit
- Session updated securely

## Dependencies

### Internal
- `inzibackend.Web.Session`: Session management
- ABP Multi-Tenancy: Tenant resolution
- AutoMapper: ViewModel mapping

### External
- jQuery: AJAX calls
- Bootstrap: Modal UI
- ABP JavaScript API: Client-side tenant switching

## Related Documentation
- [Views/Shared/Components/CLAUDE.md](../CLAUDE.md): Parent folder documentation
- [AccountLanguages/CLAUDE.md](../AccountLanguages/CLAUDE.md): Language switcher
- [AccountLogo/CLAUDE.md](../AccountLogo/CLAUDE.md): Logo component
- [inzibackend.Core.Shared/MultiTenancy/CLAUDE.md](../../../../../../inzibackend.Core.Shared/MultiTenancy/CLAUDE.md): Multi-tenancy infrastructure