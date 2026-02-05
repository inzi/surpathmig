# Views/Shared/Components/AccountLogo Documentation

## Overview
This View Component renders the application or tenant-specific logo on authentication pages (login, register, forgot password).

## Files

### AccountLogoViewComponent.cs
**Purpose**: View Component class for displaying logo

**Key Features**:
- Retrieves current tenant information
- Determines whether to show tenant logo or default logo
- Handles multi-tenancy logo display

**Dependencies**:
- Session cache for tenant information
- Base class: `inzibackendViewComponent`

### AccountLogoViewModel.cs
**Purpose**: View model for logo display

**Properties**:
- `TenantId`: Current tenant ID (null for host)
- `TenantName`: Name of current tenant
- `LogoUrl`: URL to logo image
- `UseCustomLogo`: Whether tenant has custom logo

### Default.cshtml
**Purpose**: Razor view for logo rendering

**UI Elements**:
- Logo image with appropriate source
- Alt text for accessibility
- CSS classes for styling
- Responsive design support

**Behavior**:
- Shows tenant-specific logo if available
- Falls back to default application logo
- Supports both image files and inline SVG
- Links to home page or tenant home

## Usage

### Invocation
Called from authentication layout pages:
```razor
@await Component.InvokeAsync(typeof(AccountLogoViewComponent))
```

### Multi-Tenancy
- **Host Context**: Shows default application logo
- **Tenant Context**: Shows tenant's custom logo if configured
- Logo files stored per tenant in blob storage or file system

## Dependencies
- `IPerRequestSessionCache`: Session information
- Tenant settings for logo configuration
- File storage system for logo images

## Related Documentation
- [Views/Shared/Components/CLAUDE.md](../CLAUDE.md): Parent folder documentation
- [AccountLanguages/CLAUDE.md](../AccountLanguages/CLAUDE.md): Language switcher
- [TenantChange/CLAUDE.md](../TenantChange/CLAUDE.md): Tenant switcher component