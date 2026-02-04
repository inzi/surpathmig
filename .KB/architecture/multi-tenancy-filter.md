# Multi-Tenancy Filter Pattern

[2026-02-02|62b86a8]

## The Problem

When a **Host user** (no tenant) queries data belonging to a specific **Tenant**, EF Core's automatic tenant filter blocks the query.

## Why

ABP adds a global query filter:
```csharp
.Where(e => e.TenantId == CurrentTenantId)
```

- If `CurrentTenantId == null` (Host), tenant data is invisible
- If `CurrentTenantId == 123`, only tenant 123's data is visible

## The Solution

Disable the filter temporarily when Host needs to access tenant data:

```csharp
public async Task<FileDto> GetTenantUsersToExcel(EntityDto input)
{
    var tenantId = input.Id;

    // Critical: Disable filter if Host is querying
    if (AbpSession.TenantId == null)
    {
        CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
    }

    // Now can query tenant data
    var users = await _userRepository.GetAll()
        .Where(u => u.TenantId == tenantId)
        .ToListAsync();

    // ...
}
```

## When NOT Needed

If the current user IS a tenant, no filter disabling needed:

```csharp
// Tenant user querying their own data - automatic filtering works
var users = await _userRepository.GetAll().ToListAsync();
// Returns only current tenant's users automatically
```

## Gotcha: Must Still Filter Manually

Disabling `MayHaveTenant` removes **automatic** filtering, so you MUST add explicit WHERE:

```csharp
// ❌ WRONG - returns ALL tenants' data
CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
var users = await _userRepository.GetAll().ToListAsync();

// ✅ CORRECT - explicitly filter to target tenant
CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
var users = await _userRepository.GetAll()
    .Where(u => u.TenantId == tenantId)  // Explicit filter!
    .ToListAsync();
```

## Filter Types

- `AbpDataFilters.MayHaveTenant` - For entities with nullable `TenantId`
- `AbpDataFilters.MustHaveTenant` - For entities requiring `TenantId`
- `AbpDataFilters.SoftDelete` - For soft-delete filtering

## Common Use Cases

- Host exporting tenant users
- Host viewing tenant compliance data
- Host managing tenant settings
- Cross-tenant administrative operations

## Related

- (see: clean-architecture-layers) Where this logic lives (Application Services)
- Source: `surpath112/conventions/service-layer.md:111-128`
