# Gotchas

Common mistakes and non-obvious behaviors to watch out for.

## Critical Gotchas

- [n-plus-one-queries](n-plus-one-queries.md) - Performance trap with EF Core
- [tenant-filter-forgot-where](tenant-filter-forgot-where.md) - Disabling filter without explicit WHERE
- [getalllist-vs-getall](getalllist-vs-getall.md) - Repository method differences
- [jquery-controller-bypass](jquery-controller-bypass.md) - Calling controllers directly from jQuery
- [mediatr-obsolete](mediatr-obsolete.md) - MediatR going commercial

## Quick Warnings

⚠️ Disabling `MayHaveTenant` filter? Must add explicit `.Where(e => e.TenantId == targetTenantId)`

⚠️ Need `.Include()`? Use `.GetAll()`, not `.GetAllListAsync()`

⚠️ jQuery calling backend? Use `abp.services.app.*`, NOT `/App/Controller/Action`

⚠️ MediatR pattern? Avoid - going commercial, not best practice
