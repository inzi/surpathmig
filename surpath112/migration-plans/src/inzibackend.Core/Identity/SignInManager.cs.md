# Modified
## Filename
SignInManager.cs
## Relative Path
inzibackend.Core\Identity\SignInManager.cs
## Language
C#
## Summary
The modified file introduces additional functionality by adding a method to filter external authentication schemes based on tenant-specific settings.
## Changes
Added the IsSchemeEnabledOnTenant method which checks if each authentication scheme is enabled for a specific tenant.
## Purpose
Manages and filters external authentication schemes per tenant configuration in an ASP.NET Zero application.
