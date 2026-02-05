# Modified
## Filename
UserEmailer.cs
## Relative Path
inzibackend.Core\Authorization\Users\UserEmailer.cs
## Language
C#
## Summary
The bug in GetTenancyNameOrNull occurs when tenantId is non-null. The method incorrectly uses tenantId.Value which may be incorrect if the repository expects a different type.
## Changes
Modify the method to correctly handle non-null tenantId by using tenantId instead of tenantId.Value and ensure proper handling of null cases.
## Purpose
Ensure correct retrieval of tenancy names whether or not a specific tenant ID is provided.
