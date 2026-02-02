# Modified
## Filename
DefaultExternalLoginInfoManager.cs
## Relative Path
inzibackend.Web.Core\Authentication\External\DefaultExternalLoginInfoManager.cs
## Language
C#
## Summary
The modified file implements a simplified method for retrieving user names from claims by using ClaimTypes.Email instead of checking both 'unique_name' and Email types.
## Changes
Replaced GetUserNameFromClaims method to use ClaimTypes.Email instead of checking both "unique_name" and Email types, simplifying the authentication process.
## Purpose
The class provides methods for external login information management in an ASP.NET Zero application.
