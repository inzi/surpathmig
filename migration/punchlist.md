# Surpath Migration Punchlist: surpath112/inzibackend.Web.sln vs surpath200 Structure

## Summary
**surpath112**: Full ABP .NET Framework (MVC) monolith solution with `inzibackend.Web.sln` (VS 2022 format).
- Contains: src/ (Web.Mvc, Core, Application, EF, etc.), test/, ui-tests/
- Heavy MVC focus with Metronic JS theme files (~1000+ JS/CSS files in wwwroot/metronic/)
- Custom Surpath entities (Cohorts, Compliance, Ledger, etc.) in view-resources/

**surpath200**: ABP .NET 8+ modern structure.
- Contains: aspnet-core/src/ (.NET Core/8 projects: Web.Host, Web.Public, GraphQL, Maui), react/ (React SPA with Vite), restore.log
- Frontend: Full React app with Metronic 5+ themes (modern JS bundles)
- Backend: Modern .sln with Client, Shared, EF Core, etc.
- Build artifacts: build.log, restore.log

## Key Differences (.NET Migration)
| Aspect | surpath112 (.NET Framework) | surpath200 (.NET 8+) |
|--------|-----------------------------|----------------------|
| **Solution** | `inzibackend.Web.sln` (monolith, MVC heavy) | `inzibackend.Web.sln` (modern, includes React/Maui) |
| **Web** | Web.Mvc (Razor Pages heavy) | Web.Host, Web.Public, Maui |
| **Frontend** | MVC view-resources/ JS | Full React SPA |
| **EF** | EntityFrameworkCore (likely EF6) | EntityFrameworkCore (EF Core) |
| **Tests** | Unit tests, GraphQL tests | Same + updated deps |
| **Build** | Gulp-based (old packages) | Modern npm/yarn + Vite |

## File Changes (High-Level)
- **surpath112**: ~2000+ files (heavy JS in MVC wwwroot/)
- **surpath200**: ~1500+ files (React src/, fewer MVC assets)
- **Added in 200**: React app (src/, hooks/, pages/), modern Metronic bundles, NSwag config
- **Removed/Changed**: Old Gulp assets, MVC view JS moved to React

## Punchlist for Migration (Use EF Core for DB only)
1. **Backend (.NET Upgrade)**
   - Update projects to .NET 8+ (TargetFramework=net8.0)
   - Migrate EF6 → EF Core (DbContext, migrations)
   - Port Core/Application logic (minimal changes expected)
   - Update Web.Host/Public for API-only (remove MVC views)
   - Test GraphQL, Migrator, SeedHelper

2. **Database (EF Core)**
   - Scaffold EF Core DbContext from existing DB
   - Generate migrations for custom entities (Cohorts, Ledger, etc.)
   - Update connection strings, ensure multi-tenancy works
   - Run migrations on existing DB

3. **Frontend (React Migration)**
   - surpath200 already has React - integrate Surpath-specific pages/components
   - Port dashboard widgets (CohortCompliance, DeptFilter, etc.) to React
   - Migrate ABP modules to React (Dynamic Properties, Settings, etc.)
   - Update navigation, auth flows

4. **Custom Surpath Features**
   - Compliance hierarchy, RecordRequirements → React tables/forms
   - LedgerEntries, TenantSurpathServices → API endpoints + React UI
   - PricingManagement, MigrationWizard → New React components

5. **Testing**
   - Update ui-tests for React (Playwright/Jest)
   - Backend unit tests (likely compatible)

6. **Deployment**
   - Docker: Update compose files for .NET 8 + React
   - Remove Gulp build, use npm run build

## Risks/Notes
- **Custom Entities**: Heavy Surpath logic in Compliance/Ledger - ensure data migration
- **Metronic**: 112 has theme6/7/8 JS, 200 has modern bundles - styles may need tweak
- **Build Logs**: 200 has recent build.log - use for deps
- **React Complete**: Frontend migration biggest lift, but structure ready

**Next**: Scaffold EF Core, port 1-2 key entities, test API endpoints.
