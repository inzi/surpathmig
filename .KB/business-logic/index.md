# Business Logic

Business rules, workflows, and domain concepts captured as GWT specs.

## Domains

- [compliance](compliance/) - Compliance tracking and requirement resolution
- [multi-tenancy](multi-tenancy/) - Tenant isolation and scoping rules
- [user-types](user-types/) - CohortUser vs User distinction

## Quick Reference

- **Compliance Hierarchy**: User > Cohort > Department > Tenant (see: compliance/requirement-resolution)
- **Tenant Isolation**: Each school is a tenant with isolated data (see: multi-tenancy/tenant-scoping)
- **CohortUser vs User**: Not all Users are CohortUsers (see: user-types/cohortuser-user-distinction)
