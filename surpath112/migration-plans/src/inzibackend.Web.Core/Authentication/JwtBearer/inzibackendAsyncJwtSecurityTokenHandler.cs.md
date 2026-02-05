# Modified
## Filename
inzibackendAsyncJwtSecurityTokenHandler.cs
## Relative Path
inzibackend.Web.Core\Authentication\JwtBearer\inzibackendAsyncJwtSecurityTokenHandler.cs
## Language
C#
## Summary
The modified file introduces additional security checks and functionality in the JWTBearer class. It includes new methods for validating access tokens, user delegation permissions, and token validity keys, as well as modifications to existing validation methods to incorporate these checks.
## Changes
Added HasAccessTokenType method, ValidateTokenValidityKey method, and ValidateSecurityStampAsync method. Modified ValidateTokenInternal method signature and added TokenAuthConfiguration resolver usage. Added cache expiration logic for token validity keys.
## Purpose
The file implements advanced security validation for JWT tokens, including access type checks, user delegation permissions verification, and token validity key caching.
