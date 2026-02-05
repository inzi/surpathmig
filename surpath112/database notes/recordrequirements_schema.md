# recordrequirements Table Schema

**Database:** surpathv2  
**Table:** recordrequirements  
**Purpose:** Requirements/document types that users must fulfill, scoped by cohort or department

## Columns

| Column Name | Data Type | Nullable | Default | Key | Extra | Comment |
|-------------|-----------|----------|---------|-----|-------|---------|
| Id | char | NO | null | PRI |  |  |
| TenantId | int | YES | null | MUL |  |  |
| Name | longtext | NO | null |  |  |  |
| Description | longtext | YES | null |  |  |  |
| Metadata | longtext | YES | null |  |  |  |
| CohortId | char | YES | null | MUL |  |  |
| TenantDepartmentId | char | YES | null | MUL |  |  |
| CreationTime | datetime | NO | 0001-01-01 00:00:00.000000 |  |  |  |
| CreatorUserId | bigint | YES | null |  |  |  |
| DeleterUserId | bigint | YES | null |  |  |  |
| DeletionTime | datetime | YES | null |  |  |  |
| IsDeleted | tinyint | NO | 0 |  |  |  |
| LastModificationTime | datetime | YES | null |  |  |  |
| LastModifierUserId | bigint | YES | null |  |  |  |
| IsSurpathOnly | tinyint | NO | 0 |  |  |  |
| SurpathServiceId | char | YES | null | MUL |  |  |
| TenantSurpathServiceId | char | YES | null | MUL |  |  |

## Key Fields
- **Primary Key:** Id (GUID/char)
- **Foreign Key:** TenantId (references abptenants.Id)
- **Foreign Key:** CohortId (references cohorts.Id) - Optional, for cohort-specific requirements
- **Foreign Key:** TenantDepartmentId (references tenantdepartments.Id) - Optional, for department-wide requirements
- **Service Links:** SurpathServiceId, TenantSurpathServiceId

## Important Notes
- Defines requirements that users must fulfill (document uploads, etc.)
- Can be scoped to specific cohorts (CohortId) or departments (TenantDepartmentId)
- IsSurpathOnly flag indicates if requirement is system-managed
- Links to services for automated processing
- Follows ABP soft delete and audit patterns

## Scoping Logic
- **Cohort-specific:** CohortId is set, TenantDepartmentId may be null
- **Department-wide:** TenantDepartmentId is set, CohortId may be null
- **Global:** Both CohortId and TenantDepartmentId may be null 