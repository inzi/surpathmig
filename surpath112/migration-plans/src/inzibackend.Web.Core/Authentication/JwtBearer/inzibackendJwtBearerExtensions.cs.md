# Modified
## Filename
inzibackendJwtBearerExtensions.cs
## Relative Path
inzibackend.Web.Core\Authentication\JwtBearer\inzibackendJwtBearerExtensions.cs
## Language
C#
## Summary
The modified file introduces additional flexibility by adding a 'displayName' parameter in one of its method signatures. This allows users to specify a display name when configuring the JWT bearer handler.
## Changes
Added 'displayName: string?' as an optional parameter with default value null in the last method signature.
## Purpose
The file's role is to provide extension methods for configuring JWT bearer authentication, enabling async token validation and allowing customization through display names.
