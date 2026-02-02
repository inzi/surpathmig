# records Table Schema

**Database:** surpathv2  
**Table:** records  
**Purpose:** User uploaded documents/files and their metadata

## Columns

| Column Name | Data Type | Nullable | Default | Key | Extra | Comment |
|-------------|-----------|----------|---------|-----|-------|---------|
| Id | char | NO | null | PRI |  |  |
| TenantId | int | YES | null | MUL |  |  |
| filedata | char | YES | null |  |  |  |
| filename | longtext | YES | null |  |  |  |
| physicalfilepath | longtext | YES | null |  |  |  |
| metadata | longtext | YES | null |  |  |  |
| BinaryObjId | char | NO | null |  |  |  |
| TenantDocumentCategoryId | char | YES | null | MUL |  |  |
| DateLastUpdated | datetime | YES | null |  |  |  |
| DateUploaded | datetime | YES | null |  |  |  |
| InstructionsConfirmed | tinyint | NO | 0 |  |  |  |
| CreationTime | datetime | NO | 0001-01-01 00:00:00.000000 |  |  |  |
| CreatorUserId | bigint | YES | null |  |  |  |
| DeleterUserId | bigint | YES | null |  |  |  |
| DeletionTime | datetime | YES | null |  |  |  |
| IsDeleted | tinyint | NO | 0 |  |  |  |
| LastModificationTime | datetime | YES | null |  |  |  |
| LastModifierUserId | bigint | YES | null |  |  |  |
| EffectiveDate | datetime | YES | null |  |  |  |
| ExpirationDate | datetime | YES | null |  |  |  |

## Key Fields
- **Primary Key:** Id (GUID/char)
- **Foreign Key:** TenantId (references abptenants.Id)
- **Foreign Key:** TenantDocumentCategoryId (references document categories)
- **File Reference:** BinaryObjId (references binary storage)

## Important Notes
- Stores uploaded documents/files with metadata
- Links to document categories for classification
- Tracks effective and expiration dates for compliance
- Uses binary object storage system (BinaryObjId)
- Follows ABP soft delete and audit patterns

## File Storage
- **filedata:** Legacy file data field (char/GUID)
- **filename:** Original filename
- **physicalfilepath:** Physical storage path
- **BinaryObjId:** Reference to binary storage system 