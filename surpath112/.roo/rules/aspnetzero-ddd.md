---
description: 
globs: 
alwaysApply: true
---
# AspNetZero an AspBoilerPlate Guidelines


## Solution Structure
- *.Core: Contains the Domain Layer (entities, domain services, value objects, domain events, and interfaces).
- *.Application: Contains the Application Layer (application services, DTOs, and mappings).
- *.EntityFrameworkCore: Contains the Infrastructure Layer (database context, migrations, and repository implementations).
- *.Web.Core: Contains web-related core logic (authentication, authorization).
- *.Web.Mvc or *.Web.Host: Contains the Presentation Layer (MVC controllers, Razor views, or API endpoints).

## Guidance
- Place entities, value objects, domain services, and domain events in the *.Core project under folders like Domain/Entities, Domain/ValueObjects, Domain/Services, and Domain/Events.
- Place application services and DTOs in the *.Application project under folders like Services and Dtos.
- Place repository implementations and DbContext in the *.EntityFrameworkCore project under Data or Repositories.
- Place controllers and views in the *.Web.Mvc project under Controllers and Views.
- Place services in *.Application/inzibackend/Services
- Prevent cross-layer references that violate DDD principles (e.g., *.Web.Mvc should not directly reference *.EntityFrameworkCore).

## Best Practices
- Avoid business logic in repositories; keep them thin

- Return DTOs or view models to the presentation layer, not entities.