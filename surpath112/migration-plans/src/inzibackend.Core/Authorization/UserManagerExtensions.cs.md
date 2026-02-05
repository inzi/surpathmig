# Modified
## Filename
UserManagerExtensions.cs
## Relative Path
inzibackend.Core\Authorization\UserManagerExtensions.cs
## Language
C#
## Summary
The modified code contains only the async GetAdminAsync method that retrieves an admin user by name using FindByNameAsync. The unmodified version includes both this async method and a synchronous GetAdmin method that uses FindByNameOrEmail.
## Changes
The unmodified file added a synchronous GetAdmin method using FindByNameOrEmail, which has been removed in the modified version. Only the async GetAdminAsync method remains.
## Purpose
These methods are part of an extension class for user management, specifically retrieving admin users by name or email address.
