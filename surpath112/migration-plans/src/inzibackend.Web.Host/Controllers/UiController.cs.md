# Modified
## Filename
UiController.cs
## Relative Path
inzibackend.Web.Host\Controllers\UiController.cs
## Language
C#
## Summary
The modified UiController implements additional authentication logic with tenant-specific login validation. It includes a new parameter `tenancyName` in the `GetLoginResultAsync` method to handle multi-tenant environments.
## Changes
Added 'tenancyName' parameter to GetLoginResultAsync method (string) and updated its signature accordingly.
## Purpose
Enhances authentication by validating users against their specific tenant context.
