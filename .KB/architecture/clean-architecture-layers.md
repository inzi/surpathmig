# Clean Architecture Layers

[2026-02-02|62b86a8]

## Layer Hierarchy (Dependency Flow)

```
Web.Mvc (UI Layer)
    ↓ depends on
Web.Core (Web Infrastructure)
    ↓ depends on
Application (Business Logic)
    ↓ depends on
Application.Shared (Service Contracts)
    ↓ depends on
Core (Domain Entities + Rules)
    ↓ depends on
Core.Shared (Cross-cutting)
    ↓ depends on
[External Framework: ABP]

EntityFrameworkCore (Data Access)
    ↓ depends on
Core (Domain Entities)
```

## Layer Responsibilities

### Core (Domain Layer)
- Domain entities (56+ Surpath entities)
- Domain services
- Business rules
- Rich domain models

Location: `src/inzibackend.Core/`

### Core.Shared (Shared Kernel)
- Enums
- Constants
- Helper classes
- No dependencies on other layers

Location: `src/inzibackend.Core.Shared/`

### Application.Shared (Service Contracts)
- Service interfaces (`I*AppService`)
- DTOs (268+ DTOs)
- Input/Output models
- Defines API surface

Location: `src/inzibackend.Application.Shared/`

### Application (Business Logic)
- Service implementations (`*AppService`)
- Business logic execution
- Authorization enforcement
- Object mapping
- Orchestration

Location: `src/inzibackend.Application/`

### EntityFrameworkCore (Data Access)
- DbContext
- Repositories
- Database migrations
- EF configurations

Location: `src/inzibackend.EntityFrameworkCore/`

### Web.Core (Web Infrastructure)
- Authentication (JWT, OAuth, SSO)
- SignalR chat
- Base controllers
- Shared web utilities

Location: `src/inzibackend.Web.Core/`

### Web.Host (API Host)
- Headless API endpoints
- Swagger/OpenAPI
- API-only UI (minimal Razor views)

Location: `src/inzibackend.Web.Host/`

### Web.Mvc (MVC Application)
- Traditional MVC controllers
- Razor views
- jQuery client code
- Asset bundling (Gulp)

Location: `src/inzibackend.Web.Mvc/`
Only in Surpath112 - not present in Surpath200 (replaced by React SPA)

## Key Patterns

### Repository Pattern
```csharp
// Application layer uses repository
private readonly IRepository<Tenant> _tenantRepository;

// No direct DbContext access in Application layer
```

### Unit of Work (via ABP)
```csharp
CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
```

### Dependency Injection (Castle Windsor)
```csharp
public TenantAppService(IRepository<Tenant> tenantRepository)
{
    _tenantRepository = tenantRepository;
}
```

## What Belongs Where

### ✅ Application Services (*.Application)
- Business logic
- Authorization (`[AbpAuthorize]`)
- Data queries
- Object mapping
- Calling other services/repositories

### ✅ Controllers (*.Web.Mvc)
- Page rendering
- HTTP request/response handling
- File upload/download coordination
- < 10 lines per action (thin wrappers)

### ❌ Anti-Patterns
- Business logic in controllers
- Data access in controllers
- Authorization in controllers (use service layer)
- Domain entities passed to views (use DTOs)

## Migration Context

**Surpath112 → Surpath200**:
- Core, Core.Shared, Application, Application.Shared, EntityFrameworkCore → translate directly
- Web.Mvc (jQuery) → **completely replaced** by React SPA
- Web.Core → mostly compatible (backend focus)

Focus migration effort on frontend (Web.Mvc → React).

## Related

- (see: abp-dynamic-web-api) How services are exposed as APIs
- (see: service-proxy-pattern) How jQuery calls services
- Source: `CLAUDE.md:89-110`
