# recordstates Table Schema

**Database:** surpathv2  
**Table:** recordstates  
**Purpose:** Tracks the compliance state of users for specific record requirements

## Columns

| Column Name | Data Type | Nullable | Default | Key | Extra | Comment |
|-------------|-----------|----------|---------|-----|-------|---------|
| Id | char | NO | null | PRI |  |  |
| TenantId | int | YES | null | MUL |  |  |
| State | int | NO | null |  |  |  |
| Notes | longtext | YES | null |  |  |  |
| RecordId | char | YES | null | MUL |  |  |
| RecordCategoryId | char | YES | null | MUL |  |  |
| UserId | bigint | YES | null | MUL |  |  |
| RecordStatusId | char | NO | null | MUL |  |  |
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
- **Foreign Key:** RecordId (references records.Id) - Optional, links to specific uploaded record
- **Foreign Key:** RecordCategoryId (references recordcategories.Id) - Links to requirement category
- **Foreign Key:** UserId (references abpusers.Id) - The user whose compliance is being tracked
- **Foreign Key:** RecordStatusId (references recordstatuses.Id) - Current status

## Important Notes
- **CRITICAL TABLE:** This tracks user compliance with requirements
- State field appears to be an enumeration for compliance state
- Can link to either a specific uploaded record (RecordId) or just track requirement category compliance (RecordCategoryId)
- RecordStatusId determines the visual status and compliance impact
- Follows ABP soft delete and audit patterns

## Key Relationships
- **User Compliance:** Links users to their requirement fulfillment status
- **Record Link:** Can optionally link to specific uploaded records
- **Status Tracking:** Uses RecordStatusId for current compliance status 