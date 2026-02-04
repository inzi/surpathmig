# Compliance: Requirement Resolution Hierarchy

[2026-02-02|62b86a8]

## Context

When determining what compliance requirements apply to a user, the system uses a hierarchical resolution model where more specific requirements override more general ones.

## Behaviors

### Happy path: User has no user-specific requirements
- **Given** a user in Cohort A, Department B, Tenant C
- **Given** no user-specific requirements exist
- **When** resolving requirements for the user
- **Then** system applies requirements from: Cohort A, then Department B, then Tenant C
- **Then** more specific scopes take precedence

### Happy path: User has user-specific requirements
- **Given** a user with user-specific requirements assigned
- **When** resolving requirements for the user
- **Then** user-specific requirements override all other scopes
- **Then** cohort/department/tenant requirements are ignored for that requirement type

### Edge case: User in multiple cohorts
- **Given** a user belongs to Cohort A and Cohort B
- **Given** both cohorts have different requirements
- **When** resolving requirements
- **Then** user must satisfy requirements from BOTH cohorts
- **Then** requirements are additive, not override

### Edge case: Requirement exists at multiple scopes
- **Given** Drug Test requirement at Cohort, Department, and Tenant levels
- **Given** Cohort requires 10-panel, Department requires 5-panel, Tenant requires 3-panel
- **When** resolving which test is required
- **Then** Cohort requirement (10-panel) takes precedence (most specific)

## Why

Schools need flexibility to set requirements at different organizational levels while allowing specific overrides for individual students or groups. More specific requirements (closer to the user) always win to ensure critical individual needs aren't missed.

## Exceptions

**Payment responsibility scope is independent**: Even if a user-specific requirement exists, payment can still be at institution level (institution-pay) vs. user level (donor-pay). Requirement scope and payment scope are separate concerns.

## Hierarchy (Most to Least Specific)

1. **User-specific** (userId set on RecordRequirement)
2. **Cohort-specific** (cohortId set)
3. **Department-specific** (departmentId set)
4. **Tenant-wide** (no specific scope set)

## Related

- (see: multi-tenancy/tenant-scoping) How tenants are isolated
- Source: `surpath112/src/inzibackend.Core/Surpath/CLAUDE.md:140-144`
- Source: `CLAUDE.md:167-183`
