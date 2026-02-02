# tenantdepartments Table Schema

**Database:** surpathv2  
**Table:** tenantdepartments  
**Purpose:** Department/organizational units within tenants

## Columns

| Column Name | Data Type | Nullable | Default | Key | Extra | Comment |
|-------------|-----------|----------|---------|-----|-------|---------|
| Id | char | NO | null | PRI |  |  |
| TenantId | int | YES | null | MUL |  |  |
| Name | longtext | NO | null |  |  |  |
| Active | tinyint | NO | null |  |  |  |
| MROType | int | NO | null |  |  |  |
| Description | longtext | YES | null |  |  |  |
| CreationTime | datetime | NO | 0001-01-01 00:00:00.000000 |  |  |  |
| CreatorUserId | bigint | YES | null |  |  |  |
| DeleterUserId | bigint | YES | null |  |  |  |
| DeletionTime | datetime | YES | null |  |  |  |
| IsDeleted | tinyint | NO | 0 |  |  |  |
| LastModificationTime | datetime | YES | null |  |  |  |
| LastModifierUserId | bigint | YES | null |  |  |  |
| OrganizationUnitId | bigint | NO | 0 |  |  |  |

## Key Fields
- **Primary Key:** Id (GUID/char)
- **Foreign Key:** TenantId (references abptenants.Id)
- **Organization Link:** OrganizationUnitId

## Important Notes
- Uses GUID (char) as primary key instead of auto-increment int
- MROType appears to be an enumeration for department type
- Links to ABP's organization unit system via OrganizationUnitId
- Follows ABP soft delete and audit patterns 