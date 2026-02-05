# Modified
## Filename
UserClaimsPrincipalFactory.cs
## Relative Path
inzibackend.Core\Authorization\Users\UserClaimsPrincipalFactory.cs
## Language
C#
## Summary
The modified code defines a UserClaimsPrincipalFactory class that inherits from AbpUserClaimsPrincipalFactory, which is part of an ASP.NET Core application setup. It includes dependency injection for IUserManager, IRoleManager, IOptions<IdentityOptions>, and IUnitOfWorkManager.
## Changes
The using statement 'Microsoft.Extensions.Options' was removed from the unmodified version.
## Purpose
This file configures the user claims principal factory in an ASP.NET Core application.
