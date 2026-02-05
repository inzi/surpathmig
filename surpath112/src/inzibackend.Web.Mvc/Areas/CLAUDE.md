# Areas Documentation

## Overview
The Areas folder contains ASP.NET Core MVC areas that organize related functionality into separate namespaces and folder structures. Currently, the application has one main area - the App area - which contains the authenticated application interface for the student compliance tracking system.

## Contents

### App/
The main application area containing all authenticated user functionality including:
- **Controllers**: 72+ controllers handling all business operations
- **Models**: View models organized by feature
- **Views**: Razor views for the user interface
- **Startup**: Area configuration and navigation

See [App/CLAUDE.md](App/CLAUDE.md) for comprehensive documentation of the App area.

## Architecture Notes

### Areas Pattern
ASP.NET Core Areas provide a way to partition large MVC applications into smaller functional groupings. Each area:
- Has its own folder structure (Controllers, Models, Views)
- Can have its own routing rules
- Maintains separation from other areas
- Shares common application services

### Benefits of Areas
- **Organization**: Logical grouping of related functionality
- **Scalability**: Easy to add new areas for different application sections
- **Team Development**: Different teams can work on different areas
- **Routing**: Area-specific route patterns
- **Authorization**: Area-level security policies

### Current Structure
```
Areas/
└── App/              # Main authenticated application area
    ├── Controllers/  # HTTP request handlers
    ├── Models/      # View models
    ├── Views/       # Razor views
    └── Startup/     # Area configuration
```

## Potential Future Areas

### Public Area (if added)
Could contain:
- Public-facing pages
- Registration flows
- Public documentation
- Marketing pages

### API Area (if added)
Could contain:
- RESTful API controllers
- API documentation
- API-specific models
- Versioning support

### Admin Area (if separated)
Could contain:
- Super-admin functionality
- System configuration
- Advanced monitoring
- Database management

## Area Registration

Areas are registered in the application startup and have:
- Custom route templates
- Area-specific conventions
- Middleware configuration
- Service registration

### Routing
Default area route pattern:
```
{area:exists}/{controller=Home}/{action=Index}/{id?}
```

Example routes:
- `/App/Home/Index` - App area home page
- `/App/Compliance/Index` - Compliance management
- `/App/Users/Index` - User management

## Shared Resources

While areas maintain separation, they share:
- Application services (via DI)
- Domain entities
- Database context
- Authentication/Authorization
- Shared layouts (optionally)
- Static resources in wwwroot

## Security Considerations

### Area-Level Security
- Areas can have area-wide authorization policies
- Controllers inherit area authorization
- Actions can override with specific requirements

### Current Security Model
- App area requires authentication
- Permission-based access within the area
- Multi-tenant data isolation
- Audit logging for all actions

## Best Practices

### When to Create New Areas
- Distinct user roles (e.g., Customer vs Admin)
- Separate applications within the same project
- Different authentication requirements
- Isolated feature sets

### Area Guidelines
- Keep areas focused on specific functionality
- Share services, not controllers
- Use area-specific view models
- Maintain consistent naming conventions
- Document area purposes and boundaries

## Related Components
- `/wwwroot/`: Static resources shared across areas
- `/Controllers/`: Root-level controllers (outside areas)
- `/Views/`: Root-level views and layouts
- `/Models/`: Root-level models
- `Startup.cs`: Area registration and configuration