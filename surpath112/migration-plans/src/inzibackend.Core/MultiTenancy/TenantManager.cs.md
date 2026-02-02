# Modified
## Filename
TenantManager.cs
## Relative Path
inzibackend.Core\MultiTenancy\TenantManager.cs
## Language
C#
## Summary
The bug occurs because the new tenant's IsActive property isn't explicitly set to true after creation.
## Changes
Added `tenant.IsActive = true;` after creating the admin user and before enqueuing the background job.
## Purpose
Ensure the newly created tenant is marked as active in the application.
