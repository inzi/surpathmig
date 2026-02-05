# Modified
## Filename
inzibackendAsyncJwtBearerHandler.cs
## Relative Path
inzibackend.Web.Core\Authentication\JwtBearer\inzibackendAsyncJwtBearerHandler.cs
## Language
C#
## Summary
Modify GetSafeDateTime to handle DateTime.MinValue by converting it into the current time.
## Changes
Replace null return with setting DateTime to DateTime.UtcNow when input is DateTime.MinValue
## Purpose
Prevent returning null from GetSafeDateTime which could cause issues in DateTime operations.
