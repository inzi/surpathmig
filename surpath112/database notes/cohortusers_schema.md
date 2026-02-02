# cohortusers Table Schema

**Database:** surpathv2  
**Table:** cohortusers  
**Purpose:** Junction table linking users to cohorts

## Columns

| Column Name | Data Type | Nullable | Default | Key | Extra | Comment |
|-------------|-----------|----------|---------|-----|-------|---------|
| Id | char | NO | null | PRI |  |  |
| TenantId | int | YES | null | MUL |  |  |
| CohortId | char | YES | null | MUL |  |  |
| UserId | bigint | NO | null | MUL |  |  |
| CreationTime | datetime | NO | 0001-01-01 00:00:00.000000 |  |  |  |
| CreatorUserId | bigint | YES | null |  |  |  |
| DeleterUserId | bigint | YES | null |  |  |  |
| DeletionTime | datetime | YES | null |  |  |  |
| IsDeleted | tinyint | NO | 0 |  |  |  |
| LastModificationTime | datetime | YES | null |  |  |  |
| LastModifierUserId | bigint | YES | null |  |  |  |

## Key Fields
- **Primary Key:** Id (GUID/char)
- **Foreign Key:** TenantId (references abptenants.Id)
- **Foreign Key:** CohortId (references cohorts.Id)
- **Foreign Key:** UserId (references abpusers.Id)

## Important Notes
- Junction table for many-to-many relationship between users and cohorts
- Uses GUID (char) as primary key
- Follows ABP soft delete and audit patterns
- Users can belong to multiple cohorts

## Relationships
- **Links:** abpusers ↔ cohorts
- **Scoped by:** TenantId for multi-tenancy 