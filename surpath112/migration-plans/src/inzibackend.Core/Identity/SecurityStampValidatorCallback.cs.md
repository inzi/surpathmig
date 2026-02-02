# Modified
## Filename
SecurityStampValidatorCallback.cs
## Relative Path
inzibackend.Core\Identity\SecurityStampValidatorCallback.cs
## Language
C#
## Summary
Implements callback for SecurityStampValidator's OnRefreshingPrincipal event. Maintains the claims captured at login time that are not being created by ASP.NET Identity.
## Changes
Added using statement in namespace declaration to make it more specific (using inzibackend.Identity instead of using inzibackend).
## Purpose
Part of an ASP.NET Zero project, handling security stamp validation and principal refresh events.
