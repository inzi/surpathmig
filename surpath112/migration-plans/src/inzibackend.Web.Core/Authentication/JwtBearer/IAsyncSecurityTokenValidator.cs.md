# Modified
## Filename
IAsyncSecurityTokenValidator.cs
## Relative Path
inzibackend.Web.Core\Authentication\JwtBearer\IAsyncSecurityTokenValidator.cs
## Language
C#
## Summary
The modified file introduces a new interface IAsyncSecurityTokenValidator with methods for token validation including CanReadToken, CanValidateToken (a property), and ValidateToken. The unmodified version adds an additional method, ValidateRefreshToken.
## Changes
Added the ValidateRefreshToken method with signature Task<(ClaimsPrincipal, SecurityToken)> ValidateRefreshToken(string securityToken, TokenValidationParameters validationParameters);
## Purpose
The interface is part of an authentication system handling security tokens, including validation for refresh tokens.
