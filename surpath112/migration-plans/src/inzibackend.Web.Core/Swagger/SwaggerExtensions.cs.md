# Modified
## Filename
SwaggerExtensions.cs
## Relative Path
inzibackend.Web.Core\Swagger\SwaggerExtensions.cs
## Language
C#
## Summary
The modified file contains two static methods within the SwaggerExtensions class: InjectBaseUrl and CustomDefaultSchemaIdSelector. InjectBaseUrl configures the Swagger UI page by appending a script that sets the application's base path using ABP. CustomDefaultSchemaIdSelector customizes the default schema ID selector to generate IDs based on model types.
## Changes
Added a comment in the InjectBaseUrl method referencing GitHub issue #752, indicating it will be removed when Swashbuckle.AspNetCore 5.0 is released.
## Purpose
The file contains helper methods for configuring Swagger UI integration with ABP.
