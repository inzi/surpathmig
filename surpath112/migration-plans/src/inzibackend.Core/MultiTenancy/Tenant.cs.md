# Modified
## Filename
Tenant.cs
## Relative Path
inzibackend.Core\MultiTenancy\Tenant.cs
## Language
C#
## Summary
The modified file introduces a new constant MaxLogoMimeTypeLength set to 64, which likely enforces a maximum size limit for logo files. Additionally, it includes enhanced error checking in the UpdateSubscriptionDateForPayment method by validating that SubscriptionEndDateUtc is not null before attempting to add days.
## Changes
1. Added a new constant: public const int MaxLogoMimeTypeLength = 64;
## Purpose
The file implements changes related to tenant configuration, specifically enforcing size limits for logo files and improving subscription date handling with error checks.
