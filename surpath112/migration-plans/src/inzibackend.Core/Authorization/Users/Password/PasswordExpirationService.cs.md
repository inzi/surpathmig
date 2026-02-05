# Modified
## Filename
PasswordExpirationService.cs
## Relative Path
inzibackend.Core\Authorization\Users\Password\PasswordExpirationService.cs
## Language
C#
## Summary
Implementation of PasswordExpirationService class managing password expiration for users, including host and tenant-based processing with pagination.
## Changes
Corrected repository name from _usersRepository to _recentPasswordRepository in the internal method call.
## Purpose
Manages user password expiration across different tenants by checking expired passwords and updating user credentials.
