# abptenants Table Schema

**Database:** surpathv2  
**Table:** abptenants  
**Purpose:** Core tenant information for multi-tenant ABP framework

## Columns

| Column Name | Data Type | Nullable | Default | Key | Extra | Comment |
|-------------|-----------|----------|---------|-----|-------|---------|
| Id | int | NO | null | PRI | auto_increment |  |
| SubscriptionEndDateUtc | datetime | YES | null | MUL |  |  |
| IsInTrialPeriod | tinyint | NO | null |  |  |  |
| CustomCssId | char | YES | null |  |  |  |
| LogoId | char | YES | null |  |  |  |
| LogoFileType | varchar | YES | null |  |  |  |
| SubscriptionPaymentType | int | NO | null |  |  |  |
| ClientCode | longtext | YES | null |  |  |  |
| CreationTime | datetime | NO | null | MUL |  |  |
| CreatorUserId | bigint | YES | null | MUL |  |  |
| LastModificationTime | datetime | YES | null |  |  |  |
| LastModifierUserId | bigint | YES | null | MUL |  |  |
| IsDeleted | tinyint | NO | null |  |  |  |
| DeleterUserId | bigint | YES | null | MUL |  |  |
| DeletionTime | datetime | YES | null |  |  |  |
| TenancyName | varchar | NO | null | MUL |  |  |
| Name | varchar | NO | null |  |  |  |
| ConnectionString | varchar | YES | null |  |  |  |
| IsActive | tinyint | NO | null |  |  |  |
| EditionId | int | YES | null | MUL |  |  |
| IsDonorPay | tinyint | NO | 0 |  |  |  |
| ClientPaymentType | int | NO | 0 |  |  |  |
| IsDeferDonorPerpetualPay | tinyint | NO | 0 |  |  |  |

## Key Fields
- **Primary Key:** Id
- **Unique Identifier:** TenancyName (used for tenant identification)
- **Display Name:** Name (human-readable tenant name)

## Common Patterns
- Follows ABP soft delete pattern (IsDeleted, DeleterUserId, DeletionTime)
- Includes audit fields (CreationTime, CreatorUserId, LastModificationTime, LastModifierUserId)
- Subscription management fields for SaaS functionality 