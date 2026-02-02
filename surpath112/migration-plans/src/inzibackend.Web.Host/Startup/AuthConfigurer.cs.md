# Modified
## Filename
AuthConfigurer.cs
## Relative Path
inzibackend.Web.Host\Startup\AuthConfigurer.cs
## Language
C#
## Summary
The modified code includes additional configuration handling for JWT token validation parameters and IdentityServer authentication. It also modifies the token setting logic to check for 'enc_auth_token' in request queries.
## Changes
Added explicit clearing of SecurityTokenValidators before adding a new handler, moved token validation parameter setup outside an anonymous function, and updated SetToken method to check for 'enc_auth_token'.
## Purpose
Configuration file for ASP.NET Zero authentication system, handling JWT and IdentityServer tokens.
