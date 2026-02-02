# Modified
## Filename
UserStore.cs
## Relative Path
inzibackend.Core\Authorization\Users\UserStore.cs
## Language
C#
## Summary
The modified UserStore class extends AbpUserStore and includes additional repositories for organization unit roles. It performs database operations for UserManager.
## Changes
Added 'organizationUnitRoleRepository' as a parameter and included it in the base constructor call.
## Purpose
Dependency injection of user store services including various repository types.
