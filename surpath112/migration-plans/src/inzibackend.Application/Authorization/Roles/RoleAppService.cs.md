# Modified
## Filename
RoleAppService.cs
## Relative Path
inzibackend.Application\Authorization\Roles\RoleAppService.cs
## Language
C#
## Summary
The modified file introduces an additional condition in the GetRoles method to handle cases where input.Permissions is null or empty by considering static roles.
## Changes
Added a check for null or empty input.Permissions and applies static roles when permissions are not provided.
## Purpose
The RoleAppService provides role management functionalities including retrieving roles with filters, getting edit details, creating/updating roles, and deleting them.
