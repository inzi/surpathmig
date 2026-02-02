# Modified
## Filename
WsFederationExternalLoginInfoManager.cs
## Relative Path
inzibackend.Web.Core\Authentication\External\WsFederationExternalLoginInfoManager.cs
## Language
C#
## Summary
The modified class overrides GetUserNameFromClaims to extract Windows Account Name from claims. If not present, it falls back to the base implementation.
## Changes
Added an unnecessary block around a single line variable assignment in the GetUserNameFromClaims method.
## Purpose
Part of an ASP.NET Zero authentication solution for extracting user names from security claims.
