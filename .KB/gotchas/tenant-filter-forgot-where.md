# Gotcha: Disabling Tenant Filter Without Explicit WHERE

[2026-02-02|62b86a8]

## The Mistake

```csharp
// ❌ DANGER: Returns ALL tenants' data!
if (AbpSession.TenantId == null)
{
    CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
}
var users = await _userRepository.GetAll().ToListAsync();
// Now you have EVERYONE's data, not just the target tenant!
```

## Why It Happens

Disabling `MayHaveTenant` removes the **automatic** filter:
```sql
-- Before disable: WHERE TenantId = @CurrentTenantId
-- After disable: (no WHERE clause at all!)
```

You must add **manual filtering** after disabling the filter.

## The Fix

```csharp
// ✅ CORRECT: Explicitly filter to target tenant
if (AbpSession.TenantId == null)
{
    CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
}
var users = await _userRepository.GetAll()
    .Where(u => u.TenantId == targetTenantId)  // MUST HAVE THIS!
    .ToListAsync();
```

## How to Avoid

**Pattern**:
1. Disable filter
2. Immediately add `.Where(e => e.TenantId == targetTenantId)`
3. Never rely on automatic filtering after disabling

**Code Review Checklist**:
- [ ] `DisableFilter()` is followed by explicit `.Where()` on `TenantId`
- [ ] Target tenant ID is validated (not null, exists)
- [ ] No possibility of returning wrong tenant's data

## Real-World Impact

This bug can cause:
- ✘ PII data leakage between tenants
- ✘ Compliance violations (FERPA, HIPAA)
- ✘ Security breach
- ✘ Loss of customer trust
- ✘ Regulatory fines

## Related

- (see: architecture/multi-tenancy-filter) Full pattern explanation
- Source: `surpath112/conventions/service-layer.md:175-178`
