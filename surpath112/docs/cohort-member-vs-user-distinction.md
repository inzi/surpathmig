# Important: User vs CohortUser Distinction

## Key Concepts

### User
- A **User** is a person in the system with login credentials
- Users exist independently of cohorts
- Users can have various roles and permissions
- Users can belong to multiple cohorts simultaneously
- Users are stored in the `AbpUsers` table

### CohortUser (Cohort Member)
- A **CohortUser** represents the membership of a User in a specific Cohort
- This is the relationship/association between a User and a Cohort
- Stored in the `CohortUsers` table with:
  - `Id` (unique identifier for the membership)
  - `UserId` (reference to the User)
  - `CohortId` (reference to the Cohort)
  - Audit fields (creation time, creator, etc.)

### Cohort
- A **Cohort** is a group within a department
- Cohorts define requirements and categories that their members must comply with
- Members of a cohort are subject to compliance tracking

## Compliance Data Structure

When a user is a member of a cohort (CohortUser), they:
1. Are subject to the requirements and categories defined for that cohort's department
2. Can upload records for review and approval
3. Have RecordStates that track their compliance status for each requirement category
4. Have RecordNotes associated with their RecordStates

## Important Implementation Details

### Member Transfer
When transferring members between cohorts:
- **DO NOT** create new CohortUser records
- **DO** update the existing CohortUser's `CohortId` field
- **DO** migrate all associated compliance data:
  - RecordStates
  - RecordCategories (map to equivalent in target cohort)
  - RecordNotes (automatically follow via RecordStateId)
  - Record files and statuses

### Key Relationships
```
User (1) --> (N) CohortUser
CohortUser (N) --> (1) Cohort
Cohort (N) --> (1) TenantDepartment
RecordState (N) --> (1) User
RecordState (N) --> (1) RecordCategory
RecordCategory (N) --> (1) RecordRequirement
RecordRequirement (N) --> (1) TenantDepartment
```

### Migration Considerations
1. When moving a CohortUser to a different cohort in the same department:
   - Categories and requirements should match exactly
   - RecordStates can be preserved as-is

2. When moving to a cohort in a different department:
   - Categories must be mapped to equivalents in the new department
   - Some categories may not have equivalents (requires handling)
   - RecordStates must be updated to reference the new categories

## Common Mistakes to Avoid
1. Creating duplicate CohortUser records instead of updating existing ones
2. Losing compliance history during transfers
3. Not updating category references when moving between departments
4. Forgetting that users can be in multiple cohorts simultaneously