# Modified
## Filename
EmailConfirmationViewModel.cs
## Relative Path
inzibackend.Web.Mvc\Models\Account\EmailConfirmationViewModel.cs
## Language
C#
## Summary
The EmailConfirmationViewModel class handles activation emails by decrypting a parameter and setting the tenant ID from the query string.
## Changes
Added null check for 'c' variable in ResolveParameters method to prevent potential null reference exceptions.
## Purpose
Provides activation email functionality with tenant-specific data handling.
