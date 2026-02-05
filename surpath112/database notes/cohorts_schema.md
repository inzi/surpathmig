# cohorts Table Schema

**Database:** surpathv2  
**Table:** cohorts  
**Purpose:** Student cohorts/groups within departments

## Columns

| Column Name | Data Type | Nullable | Default | Key | Extra | Comment |
|-------------|-----------|----------|---------|-----|-------|---------|
| Id | char | NO | null | PRI |  |  |
| TenantId | int | YES | null | MUL |  |  |
| Name | longtext | YES | null |  |  |  |
| Description | longtext | YES | null |  |  |  |
| DefaultCohort | tinyint | NO | null |  |  |  |
| TenantDepartmentId | char | YES | null | MUL |  |  |
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
- **Foreign Key:** TenantDepartmentId (references tenantdepartments.Id)

## Important Notes
- Uses GUID (char) as primary key
- Belongs to a specific department via TenantDepartmentId
- DefaultCohort flag indicates if this is the default cohort for the department
- Follows ABP soft delete and audit patterns

## Relationships
- **Parent:** tenantdepartments (via TenantDepartmentId)
- **Children:** Users can be assigned to cohorts 