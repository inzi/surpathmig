# User Types: CohortUser vs User Distinction

[2026-02-02|62b86a8]

## Context

The system has two related but distinct concepts: `User` (ASP.NET Identity) and `CohortUser` (domain-specific student record). Understanding the difference prevents bugs when querying or updating data.

## Behaviors

### Rule: CohortUser always has a User
- **Given** a CohortUser record exists
- **Then** it MUST have an associated User record (UserId is required)
- **Then** CohortUser → User is a many-to-one relationship

### Rule: User may NOT have a CohortUser
- **Given** a User record exists
- **When** the user is staff, admin, or tenant admin
- **Then** no CohortUser record exists for them
- **Then** User → CohortUser is optional (zero or one)

### Happy path: Finding CohortUser from UserId
- **Given** a method accepts `userId` parameter
- **When** the user is a student with compliance requirements
- **Then** lookup CohortUser via `_cohortUserRepository.GetAll().Where(cu => cu.UserId == userId)`
- **Then** perform compliance operations on the CohortUser

### Edge case: Method receives UserId but user is not a student
- **Given** a compliance-checking method receives `userId`
- **Given** the user is staff (no CohortUser)
- **When** querying for CohortUser
- **Then** query returns no results
- **Then** method should handle gracefully (skip compliance checks or return empty)

### Pattern: Services accept both parameter types
- **Given** some methods accept `userId` (long)
- **Given** other methods accept `cohortUserId` (Guid)
- **When** the method operates on compliance data
- **Then** if given userId, find CohortUser internally
- **Then** if given cohortUserId, use directly

## Why

Not all users in the system are students tracking compliance. Staff, administrators, and tenant admins are `User` records but don't have associated `CohortUser` records because they don't track compliance themselves—they manage compliance for others.

## Exceptions

In rare cases, a staff member might ALSO be enrolled as a student (e.g., teaching assistant). In this case, they have:
- ONE User record (authentication/authorization)
- ONE CohortUser record (compliance tracking)
- Different permissions applied based on role

## Key Entities

**User** (inzibackend.Core/Authorization/Users/User.cs):
- ASP.NET Identity user
- Authentication & authorization
- Primary key: `long Id`

**CohortUser** (inzibackend.Core/Surpath/CohortUser.cs):
- Domain concept: student in a cohort
- Compliance tracking subject
- Primary key: `Guid Id`
- Foreign key: `long UserId` → User

## Related

- (see: compliance/requirement-resolution) How requirements apply to CohortUsers
- Source: `CLAUDE.md:174-177`
- Source: `surpath112/src/inzibackend.Core/Surpath/CLAUDE.md:18-21`
