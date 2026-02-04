# Knowledge Base Index

[2026-02-02|62b86a8] - Knowledge base initialized

This KB documents the Surpath compliance tracking system migration from MVC/jQuery (surpath112) to React SPA (surpath200).

## Concepts

### Architecture
- [abp-dynamic-web-api](architecture/abp-dynamic-web-api.md) - How ABP auto-generates Web API endpoints from services
- [multi-tenancy-filter](architecture/multi-tenancy-filter.md) - Tenant data isolation and filter management
- [clean-architecture-layers](architecture/clean-architecture-layers.md) - Layer organization and responsibilities

### Business Logic
- [compliance/requirement-resolution](business-logic/compliance/requirement-resolution.md) - Hierarchical requirement resolution (User > Cohort > Department > Tenant)
- [user-types/cohortuser-user-distinction](business-logic/user-types/cohortuser-user-distinction.md) - CohortUser vs User: not all Users are CohortUsers

### Patterns
- [asset-bundling](patterns/asset-bundling.md) - JavaScript/CSS bundling via Gulp (surpath112 only)

### Gotchas
- [tenant-filter-forgot-where](gotchas/tenant-filter-forgot-where.md) - Must add explicit WHERE after disabling tenant filter
- [getalllist-vs-getall](gotchas/getalllist-vs-getall.md) - Use GetAll() for Include(), not GetAllListAsync()
- [mediatr-obsolete](gotchas/mediatr-obsolete.md) - MediatR going commercial, not best practice

## Quick Reference

### Critical Patterns
- **jQuery → Backend**: Use `abp.services.app.*` proxies, NOT direct controller calls
- **Host → Tenant Data**: Disable filter + explicit WHERE clause required
- **EF Eager Loading**: Use `.GetAll().Include()`, not `.GetAllListAsync()`

### Migration Context
- **Source**: surpath112 (MVC + jQuery multi-page app)
- **Target**: surpath200 (React SPA)
- **Backend**: Translates directly (Core, Application, EntityFrameworkCore)
- **Frontend**: Complete rewrite (jQuery → React)
- **Guides**: `migration/` folder with 8 comprehensive guides

### Test Credentials
- Username: `claude@inzi.com`
- Password: `25.Testing.25`

## Folder Structure

```
.kb/
├── kb.md                           # This file
├── mandatory.md                    # KB rules and requirements
├── knowledgebase-rules.md          # Content organization rules
├── corrections.md                  # Correction protocol
├── gwt-format.md                   # Business logic GWT template
├── architecture/                   # Technical architecture
│   ├── index.md
│   ├── abp-dynamic-web-api.md
│   ├── multi-tenancy-filter.md
│   └── clean-architecture-layers.md
├── business-logic/                 # GWT specs for business rules
│   ├── index.md
│   ├── compliance/
│   │   └── requirement-resolution.md
│   └── user-types/
│       └── cohortuser-user-distinction.md
├── patterns/                       # Development patterns
│   ├── index.md
│   └── asset-bundling.md
└── gotchas/                        # Common mistakes
    ├── index.md
    ├── tenant-filter-forgot-where.md
    ├── getalllist-vs-getall.md
    └── mediatr-obsolete.md
```

## Domain Summary

**Surpath** is a compliance tracking SaaS for educational institutions:
- **56+ domain entities** for compliance management
- **268+ DTOs** for data transfer
- **Multi-tenant** with complete data isolation
- **Core services**: Drug testing, background checks, document compliance
- **Payment models**: Institution-pay vs. donor-pay
- **User hierarchy**: Tenant → Department → Cohort → User

## Key Technologies

### Surpath112 (Source)
- ASP.NET Zero 11.4 MVC
- jQuery + Bootstrap 5.1.1 + DataTables
- Gulp + bundles.json for asset bundling
- Traditional multi-page application

### Surpath200 (Target)
- ASP.NET Zero Core + React 18.3.1
- TypeScript + Ant Design 5.26.7
- Redux Toolkit 2.8.2 + React Hook Form 7.62.0
- Vite 7.0.4 for bundling
- Single Page Application (SPA)

### Shared (Both Versions)
- .NET 8+ backend
- MySQL database with EF Core
- ABP Framework for architecture
- Castle Windsor for DI
- AutoMapper for DTO mapping
- SignalR for real-time features
